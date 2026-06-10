using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Xml.Linq;
using EjeDeTrazado.componentes;
using tadLayShare.puntos;

namespace tadLayLogica.logica.LandXml
{
    public class TrazadoLandXml_old
    {
        public List<Componente> Trazado { get; } = new List<Componente>();
        public double Longitud { get; private set; }
        public double pk_ini_eje { get; private set; }
        public double pk_fin_eje { get; private set; }

        private static readonly XNamespace lxml = "http://www.landxml.org/schema/LandXML-1.2";
        private static readonly CultureInfo CI = CultureInfo.InvariantCulture;

        public TrazadoLandXml_old() { }

        public TrazadoLandXml_old(string landxmlPath)
        {
            CargarXml(landxmlPath);
        }

        public void CargarXml(string file)
        {
            // Cargar documento
            var doc = XDocument.Load(file);
            var root = doc.Root;
            if (root == null) throw new InvalidOperationException("LandXML no tiene elemento raíz.");

            // Leer unidades angulares declaradas (por defecto: grados decimales)
            var tuple = ReadAngularUnit(root);
            var angFactor = tuple.Item1;
            var angUnit = tuple.Item2;

            // Reiniciar estado
            Trazado.Clear();
            Longitud = 0;
            pk_ini_eje = 0;
            pk_fin_eje = 0;

            // Procesar alineaciones
            foreach (var alignment in root.Element(lxml + "Alignments")?.Elements(lxml + "Alignment") ?? Enumerable.Empty<XElement>())
            {
                double staStart = ReadDoubleAttr(alignment, "staStart") ?? 0.0;
                double pkAcum = 0.0;
                double dist_clotoide_ant = 0;
                foreach (var coordGeom in alignment.Elements(lxml + "CoordGeom"))
                {
                    foreach (var n in coordGeom.Elements())
                    {
                        switch (n.Name.LocalName)
                        {
                            case "Line":
                                {
                                    double dir = ReadDoubleAttr(n, "dir") ?? 0.0;
                                    double length = ReadDoubleAttr(n, "length") ?? CalcLenFallback(n);
                                    var startXY = ReadXY(n.Element(lxml + "Start"));
                                    var endXY = ReadXY(n.Element(lxml + "End"));
                                    double az = dir * angFactor;

                                    var pStart = new Punto3d(startXY.Item1, startXY.Item2, 0);
                                    var pEnd = new Punto3d(endXY.Item1, endXY.Item2, 0);
                                    var recta = new EjeDeTrazado.componentes.Linea(pStart, pEnd, staStart + pkAcum, staStart + pkAcum + length, az);
                                    Trazado.Add(recta);
                                    pkAcum += length;
                                    break;
                                }

                            case "Curve":
                                {
                                    var rot = n.Attribute("rot")?.Value; // "cw" | "ccw"
                                    double? radius = ReadDoubleAttr(n, "radius");
                                    double length = ReadDoubleAttr(n, "length") ?? 0.0;
                                    double dirStart = ReadDoubleAttr(n, "dirStart") ?? 0.0;
                                    double dirEnd = ReadDoubleAttr(n, "dirEnd") ?? 0.0;

                                    var sXY = ReadXY(n.Element(lxml + "Start"));
                                    var cXY = ReadXY(n.Element(lxml + "Center"));
                                    var eXY = ReadXY(n.Element(lxml + "End"));

                                    // Convertir ángulos según Units
                                    double azIni = dirStart * angFactor;
                                    double azFin = dirEnd * angFactor;

                                    var pS = new Punto3d(sXY.Item1, sXY.Item2, 0);
                                    var pC = new Punto3d(cXY.Item1, cXY.Item2, 0);
                                    var pE = new Punto3d(eXY.Item1, eXY.Item2, 0);
                                    var sentido = rot ?? "cw"; // default conservador
                                    var curva = new Curva(pS, pC, pE, radius ?? 0.0, staStart + pkAcum, 8, 2, 
                                        EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario, dist_clotoide_ant);

                                    if (sentido=="cw")
                                    {
                                        curva = new Curva(pS, pC, pE, radius ?? 0.0, staStart + pkAcum, 8, 2, 
                                            EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario, dist_clotoide_ant);
                                    }
                                    Trazado.Add(curva);
                                    pkAcum += length;
                                    break;
                                }

                            case "Spiral":
                                {
                                    double length = ReadDoubleAttr(n, "length") ?? 0.0;
                                    var rot = n.Attribute("rot")?.Value;              // cw/ccw
                                    var spiType = n.Attribute("spiType")?.Value;      // clothoid, etc.

                                    double? rStart = ReadDoubleInfAttr(n, "radiusStart");
                                    double? rEnd = ReadDoubleInfAttr(n, "radiusEnd");

                                    var sXY = ReadXY(n.Element(lxml + "Start"));
                                    var eXY = ReadXY(n.Element(lxml + "End"));
                                    var pS = new Punto3d(sXY.Item1, sXY.Item2, 0);
                                    var pE = new Punto3d(eXY.Item1, eXY.Item2, 0);

                                    // Tipo Entrada/Salida/S (ambos finitos)
                                    bool esEntrada = IsInf(rStart) && !IsInf(rEnd);
                                    bool esSalida = !IsInf(rStart) && IsInf(rEnd);
                                    bool esS = !IsInf(rStart) && !IsInf(rEnd);

                                    // Radio a usar para la transición
                                    double rc = esEntrada ? (rEnd ?? double.PositiveInfinity)
                                                          : (esSalida ? (rStart ?? double.PositiveInfinity)
                                                                      : (rEnd ?? rStart ?? double.PositiveInfinity));

                                    // Constantes opcionales
                                    // LandXML puede guardar la constante de la clotoide como "constant" (A) o en Features/Property("A"), aquí usamos atributo si viene
                                    double A = 0.0;
                                    if (ReadDoubleAttr(n, "constant") is double k) A = k;

                                    // Si falta A y hay R finito, usar A = sqrt(L*R)
                                    if (A <= 0 && !double.IsInfinity(rc) && rc > 0 && length > 0)
                                        A = Math.Sqrt(length * rc);

                                    // Deflexión aproximada de la transición: theta = L/(2R) [rad] => grados
                                    double deltaDeg = (!double.IsInfinity(rc) && rc > 0)
                                        ? (length / (2.0 * rc)) * 180.0 / Math.PI
                                        : 0.0;

                                    // Azimut base: dirStart preferente si existe; en su defecto, azimut de la cuerda Start->End
                                    double? dirStart = ReadDoubleAttr(n, "dirStart");
                                    double azBaseDeg = dirStart.HasValue ? dirStart.Value * angFactor : AzimuthDeg(pS, pE);

                                    var sentido = (rot == "ccw")
                                        ? EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario
                                        : EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;

                                    var tipoEnum = esEntrada
                                        ? EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.entrada
                                        : EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.salida;

                                    bool isSigCurva = esEntrada;     // Entrada transiciona desde recta a curva
                                    bool reducido = false;           // Sin reducción por defecto
                                    bool isCurvaS = esS;             // “S” cuando ambos radios son finitos
                                    double miLe = length;

                                    var clotoide = new Clotoide(
                                        pS, pE,
                                        double.IsInfinity(rc) ? 0.0 : rc, // mRc debe ser finito si se usa fórmula L=A^2/R
                                        staStart + pkAcum,
                                        sentido,
                                        0.0,     // peralte anterior
                                        0.0,     // peralte posterior
                                        isSigCurva,
                                        tipoEnum,
                                        azBaseDeg,
                                        reducido,
                                        deltaDeg,
                                        isCurvaS,
                                        miLe,
                                        A        // iA si está disponible/calculado
                                    );
                                    if (IsInf(rStart) && !IsInf(rEnd))
                                        dist_clotoide_ant = length;
                                    Trazado.Add(clotoide);
                                    pkAcum += length;
                                    break;
                                }
                        }
                    }
                }

                // PKs por alineación
                pk_ini_eje = staStart;
                pk_fin_eje = staStart + pkAcum;
                Longitud = pkAcum;
            }
        }


        // Helpers

        // Retorna factor para convertir desde Units.angularUnit a GRADOS
        private static Tuple<double, string> ReadAngularUnit(XElement root)
        {
            var units = root.Element(lxml + "Units");
            var metric = units?.Element(lxml + "Metric");
            var imperial = units?.Element(lxml + "Imperial");

            string ang = metric?.Attribute("angularUnit")?.Value
                      ?? imperial?.Attribute("angularUnit")?.Value
                      ?? "radians"; // por especificación, por defecto radianes

            double factorToDegrees;
            if (ang.IndexOf("radian", StringComparison.OrdinalIgnoreCase) >= 0)
                factorToDegrees = 180.0 / Math.PI;
            else if (ang.IndexOf("grad", StringComparison.OrdinalIgnoreCase) >= 0
                  || ang.IndexOf("gon", StringComparison.OrdinalIgnoreCase) >= 0)
                factorToDegrees = 0.9;   // 100 grads = 90 deg
            else
                factorToDegrees = 1.0;   // decimal degrees y variantes

            return Tuple.Create(factorToDegrees, ang);
        }

        private static Tuple<double, double> ReadXY(XElement node)
        {
            if (node == null) throw new InvalidOperationException("Nodo de punto no encontrado.");
            var parts = node.Value.Trim()
                .Split((char[])null, StringSplitOptions.RemoveEmptyEntries); // null → separa por blancos
            if (parts.Length < 2) throw new FormatException("Coordenadas insuficientes en punto LandXML.");
            double x = double.Parse(parts[0], CI);
            double y = double.Parse(parts[1], CI);
            return Tuple.Create(x, y);
        }

        private static double? ReadDoubleAttr(XElement e, string name)
        {
            var a = e.Attribute(name);
            if (a == null) return null;

            double v;
            if (double.TryParse(a.Value, NumberStyles.Float, CI, out v))
                return v;                // implícito a double? fuera del ternario en C# 7.3
            return null;
        }

        private static double? ReadDoubleInfAttr(XElement e, string name)
        {
            var a = e.Attribute(name);
            if (a == null) return null;

            if (string.Equals(a.Value, "INF", StringComparison.OrdinalIgnoreCase))
                return double.PositiveInfinity;

            double v;
            if (double.TryParse(a.Value, NumberStyles.Float, CI, out v))
                return v;                // implícito a double? en C# 7.3
            return null;
        }

        private static bool IsInf(double? v) => v.HasValue && double.IsPositiveInfinity(v.Value);

        private static double CalcLenFallback(XElement line)
        {
            var s = ReadXY(line.Element(lxml + "Start"));
            var e = ReadXY(line.Element(lxml + "End"));
            double dx = e.Item1 - s.Item1;
            double dy = e.Item2 - s.Item2;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static double AzimuthDeg(Punto3d a, Punto3d b)
        {
            double dx = b.coordenadaX - a.coordenadaX;
            double dy = b.coordenadaY - a.coordenadaY;
            double ang = Math.Atan2(dy, dx) * 180.0 / Math.PI;
            return (ang < 0) ? ang + 360.0 : ang;
        }
    }
}
