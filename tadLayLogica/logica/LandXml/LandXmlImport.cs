// LandXmlImporter.cs - C# 7.3
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

// Referencias a tus namespaces
using EjeDeTrazado.componentes;
using EjeDeTrazado.puntosDelEje;
using PerfilLongitudinal.componentes;
using tadLayShare.puntos;

namespace tadLayLogica.logica.LandXml
{
    public sealed class LandXmlImporter
    {
        public ImportResult Import(string xmlPath, double peralteRecta = 0.0, double peralteCurva = 0.0)
        {
            var doc = XDocument.Load(xmlPath);
            XNamespace ns = "http://www.landxml.org/schema/LandXML-1.2";

            var alignment = doc.Descendants(ns + "Alignment").FirstOrDefault();
            if (alignment == null)
                throw new InvalidOperationException("No se encontró <Alignment> en el LandXML.");

            var coordGeom = alignment.Element(ns + "CoordGeom");
            if (coordGeom == null)
                throw new InvalidOperationException("No se encontró <CoordGeom> en el LandXML.");

            var componentesPlanta = new List<Componente>();
            var vertices = new List<Vertice>();

            // Llevar PK acumulado por componente
            double pk = 0.0;
            Punto3d ultimoFin = null;

            // Utilidad: parsear "X Y" de elementos tipo <Start>...</Start>
            Func<XElement, Punto3d> parseXY = el =>
            {
                // Soporta contenido tipo "x y" o atributos x/y si aparecen
                if (!string.IsNullOrWhiteSpace((string)el))
                {
                    var parts = el.Value.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        var x = ParseD(parts[0]); // LandXML ejemplo usa "northing easting"
                        var y = ParseD(parts[1]);
                        return new Punto3d(x, y, 0);
                    }
                }
                var xs = (string)el.Attribute("x");
                var ys = (string)el.Attribute("y");
                if (xs != null && ys != null)
                {
                    var x = ParseD(xs);
                    var y = ParseD(ys);
                    return new Punto3d(x, y, 0);
                }
                throw new InvalidOperationException("No se pudo leer coordenas X/Y de " + el.Name.LocalName);
            };

            // Recorre secuencialmente los elementos de CoordGeom
            foreach (var seg in coordGeom.Elements())
            {
                var name = seg.Name.LocalName;

                if (name == "Line")
                {
                    var start = parseXY(seg.Element(ns + "Start"));
                    var end = parseXY(seg.Element(ns + "End"));

                    // Azimut según tu implementación: 0=norte, sentido horario, usando dx/dy [Linea.cs]
                    var az = ComputeAzimut(end.coordenadaX - start.coordenadaX, end.coordenadaY - start.coordenadaY);

                    var linea = new Linea(start, end, pk, peralteRecta, az);
                    componentesPlanta.Add(linea);

                    // Vertice en el inicio de la línea
                    var vert = new Vertice(
                        start,
                        az,
                        EjeTrazado.sentidoCurva.noValorado,
                        0,
                        EjeTrazado.tipoCurva.noValorado,
                        EjeTrazado.tipoSegmento.noValorado,
                        0,
                        new Punto3d(0, 0, 0)
                    );
                    vertices.Add(vert);

                    pk = linea.getPkFinal();
                    ultimoFin = end;
                }
                else if (name == "Curve")
                {
                    // Curva circular con Start/Center/End y radio
                    var start = parseXY(seg.Element(ns + "Start"));
                    var end = parseXY(seg.Element(ns + "End"));
                    var center = parseXY(seg.Element(ns + "Center"));
                    var radiusAttr = (string)seg.Attribute("radius");
                    var radius = radiusAttr != null ? ParseD(radiusAttr) : Dist2D(center, start);
                    var rot = ((string)seg.Attribute("rot")) ?? "cw";
                    var sentido = rot == "ccw" ? EjeTrazado.sentidoCurva.Antihorario : EjeTrazado.sentidoCurva.Horario;

                    // Evitar marcar como “gran radio” en tu Curva si no hay clotoide anterior: longClotoideAnterior > 0 [Curva.cs]
                    var curva = new Curva(
                        start,
                        end,
                        center,
                        radius,
                        pk,
                        peralteCurva,
                        0.0,      // bombeo
                        sentido,
                        1.0       // longClotoideAnterior != 0
                    );

                    componentesPlanta.Add(curva);

                    // Azimut tangente en inicio = radial +/− 90º según sentido
                    var az = TangentAzimutAtStartForArc(start, center, sentido);

                    // Vértice con datos de curva (radio reducido no aplica aquí, se usa radio)
                    var vert = new Vertice(
                        start,
                        az,
                        sentido,
                        radius,
                        EjeTrazado.tipoCurva.cp,
                        EjeTrazado.tipoSegmento.noValorado,
                        0,
                        center
                    );
                    vertices.Add(vert);

                    pk = curva.getPkFinal();
                    ultimoFin = end;
                }
                else if (name == "Spiral")
                {
                    // Spiral tipo clothoid
                    var start = parseXY(seg.Element(ns + "Start"));
                    var end = parseXY(seg.Element(ns + "End"));
                    var rot = ((string)seg.Attribute("rot")) ?? "cw";
                    var sentido = rot == "ccw" ? EjeTrazado.sentidoCurva.Antihorario : EjeTrazado.sentidoCurva.Horario;
                    var spiType = ((string)seg.Attribute("spiType")) ?? "clothoid";
                    var lengthAttr = (string)seg.Attribute("length");
                    var length = lengthAttr != null ? ParseD(lengthAttr) : Dist2D(start, end);

                    // Determina si es entrada o salida: INF en radiusStart o radiusEnd [LandXML]
                    var radiusStartStr = (string)seg.Attribute("radiusStart");
                    var radiusEndStr = (string)seg.Attribute("radiusEnd");
                    var isEntrada = (radiusStartStr ?? "").ToUpperInvariant().Contains("INF");
                    var isSalida = (radiusEndStr ?? "").ToUpperInvariant().Contains("INF");
                    var tipoClo = isEntrada ? EjeTrazado.tipoClotoide.entrada : EjeTrazado.tipoClotoide.salida;

                    // Radio efectivo en la clotoide
                    double rc;
                    if (isEntrada)
                        rc = radiusEndStr != null && !radiusEndStr.ToUpperInvariant().Contains("INF") ? ParseD(radiusEndStr) : 0.0;
                    else
                        rc = radiusStartStr != null && !radiusStartStr.ToUpperInvariant().Contains("INF") ? ParseD(radiusStartStr) : 0.0;

                    // Azimut de la dirección de la clotoide
                    var az = ComputeAzimut(end.coordenadaX - start.coordenadaX, end.coordenadaY - start.coordenadaY);

                    // Delta óptico si existe
                    var thetaAttr = (string)seg.Attribute("theta");
                    var delta = thetaAttr != null ? ParseD(thetaAttr) : 0.0;

                    var clotoide = new Clotoide(
                        start,
                        end,
                        rc,
                        pk,
                        sentido,
                        0.0, // peralteAnt
                        0.0, // peraltePos
                        true, // isSigCurva: transicion hacia curva
                        tipoClo,
                        az,
                        false, // reducido
                        delta,
                        false, // isCurvaS
                        length,
                        0.0    // iA opcional
                    );

                    componentesPlanta.Add(clotoide);

                    // Vértice representativo en el arranque de la transición
                    var vert = new Vertice(
                        start,
                        az,
                        sentido,
                        rc,
                        EjeTrazado.tipoCurva.cnp, // transición
                        EjeTrazado.tipoSegmento.noValorado,
                        delta,
                        new Punto3d(0, 0, 0)
                    );
                    vertices.Add(vert);

                    pk = clotoide.getPkFinal();
                    ultimoFin = end;
                }
                // Otros tipos (IrregularLine, etc.) pueden añadirse si fuesen necesarios
            }

            // Asegurar un último vértice al final
            if (ultimoFin != null)
            {
                vertices.Add(new Vertice(
                    ultimoFin,
                    0.0,
                    EjeTrazado.sentidoCurva.noValorado,
                    0.0,
                    EjeTrazado.tipoCurva.noValorado,
                    EjeTrazado.tipoSegmento.noValorado,
                    0.0,
                    new Punto3d(0, 0, 0)
                ));
            }

            // Construir el EjeTrazado con tus tipos
            var eje = new EjeTrazado(vertices, componentesPlanta, peralteCurva, peralteRecta);

            // PERFIL LONGITUDINAL: PVIs y ParaCurve
            var pendientes = new List<double>();
            var parabolas = new List<Parabola>();
            var alzadoComponentes = new List<ComponenteLong>();
            var kvs = new List<double>();
            var profile = alignment.Element(ns + "Profile");

           

            if (profile != null)
            {
                // Civil3D suele anidar ProfAlign
                var profAlign = profile.Element(ns + "ProfAlign") ?? profile;


                var children = profAlign.Elements().ToList();

                var pviList = new List<Tuple<double, double>>();
                for (int idx = 0; idx < children.Count; idx++)
                {
                    var el = children[idx];
                    var parts = el.Value.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    pviList.Add(Tuple.Create(ParseD(parts[0]), ParseD(parts[1])));
                }

                for (int idx = 0; idx < children.Count-1; idx++)
                {
                    var el = children[idx];
                    var el_sig = children[idx+1];

                    var parts = el.Value.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    var parts_sig = el_sig.Value.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (el.Name == ns + "ParaCurve")
                    {
                        var lenAttr = (string)el.Attribute("length");
                        var kAttr = (string)el.Attribute("desc");

                        double len = lenAttr != null ? ParseD(lenAttr) : 0.0;
                        double kv = kAttr != null ? ParseD(kAttr) : 0.0;

                        bool concavo = true;
                        if (kv<0)
                        {
                            concavo = false;
                        }
                        var r_pendientes=ComputeSlopesFromEndPoints(ParseD(parts[0]), ParseD(parts[1]),ParseD(parts_sig[0]), ParseD(parts_sig[1]),kv, true, true, false);


                        var parabola = new Parabola(
                                new Punto3d(ParseD(parts[0]), ParseD(parts[1]), 0),
                                new Punto3d(ParseD(parts_sig[0]), ParseD(parts_sig[1]), 0),
                               ParseD(parts[0]),
                               r_pendientes.GIn,
                               kv,
                               r_pendientes.DeltaG,
                               concavo
                           );
                        alzadoComponentes.Add(parabola);
                        kvs.Add(kv);
                    }
                    if (el.Name == ns + "PVI")
                    {

                        var recta = new Recta(
                        new Punto3d(ParseD(parts[0]), ParseD(parts[1]), 0),
                        new Punto3d(ParseD(parts_sig[0]), ParseD(parts_sig[1]), 0),
                        ParseD(parts[0])
                    );
                        alzadoComponentes.Add(recta);
                    }
                }

            }

            return new ImportResult
            {
                ComponentesPlanta = componentesPlanta,
                Eje = eje,
                AlzadoComponentes = alzadoComponentes,
                Pendientes = pendientes,
                Parabolas = parabolas,
                KVs=kvs
            };
        }

        // ----------------- Utilidades geométricas coherentes con tus clases -----------------

        public struct VCurveResult
        {
            public double GIn;     // pendiente de entrada (fracción o % según flags)
            public double GOut;    // pendiente de salida
            public double DeltaG;  // salto de pendiente
            public double L;       // longitud del acuerdo
        }
        /// <summary>
        /// Calcula gIn, gOut y dG a partir de dos puntos tangentes (entrada/salida) y Kv.
        /// sEnt/sSal: estaciones de entrada y salida; zEnt/zSal: cotas; 
        /// kv: parámetro Kv (puede venir con signo si así lo manejas en desc);
        /// kvHasSign: true si kv ya porta el signo de sag/crest; 
        /// isSagIfUnsigned: si kv no trae signo (kvHasSign=false), true=sag (+), false=crest (-);
        /// slopesInPercent: true para devolver pendientes en %.
        /// </summary>
        public static VCurveResult ComputeSlopesFromEndPoints(
            double sEnt, double zEnt,
            double sSal, double zSal,
            double kv,
            bool kvHasSign = true,
            bool isSagIfUnsigned = true,
            bool slopesInPercent = false)
        {
            const double EPS = 1e-12;

            double L = sSal - sEnt;
            if (L <= EPS)
                throw new ArgumentException("Estaciones inválidas: L <= 0 (sSal debe ser mayor que sEnt).");

            double deltaZ = zSal - zEnt;

            double absKv = Math.Abs(kv);
            if (absKv <= EPS)
                throw new ArgumentException("Kv inválido o cero.");

            // ΔG = L / Kv (con el signo según convención)
            double dG = L / absKv;
            if (kvHasSign)
            {
                int sgn = Math.Sign(kv);
                dG *= (sgn == 0 ? 1.0 : sgn); // kv<0 => crest (ΔG negativo), kv>0 => sag (ΔG positivo)
            }
            else
            {
                dG *= (isSagIfUnsigned ? 1.0 : -1.0);
            }

            double gMean = deltaZ / L;   // media de pendientes entre tangentes
            double gIn = gMean - 0.5 * dG;
            double gOut = gMean + 0.5 * dG;

            if (slopesInPercent)
            {
                gIn *= 100.0;
                gOut *= 100.0;
                dG *= 100.0;
            }

            return new VCurveResult
            {
                GIn = gIn,
                GOut = gOut,
                DeltaG = dG,
                L = L
            };
        }
        private static double ParseD(string s)
        {
            return double.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        private static double Dist2D(Punto3d a, Punto3d b)
        {
            var dx = a.coordenadaX - b.coordenadaX;
            var dy = a.coordenadaY - b.coordenadaY;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // Reproduce la lógica de azimut de Linea.getAzimut (0=norte, en grados) [Linea.cs]
        private static double ComputeAzimut(double dx, double dy)
        {
            if (dx == 0 || dy == 0)
            {
                if (dy == 0)
                    return dx < 0 ? 180.0 : 0.0;
                else
                    return dx < 0 ? 270.0 : 90.0;
            }
            var delta = Math.Atan(dx / dy) * 180.0 / Math.PI;
            double az;
            if (delta == 0)
            {
                if (dy == 0)
                    az = dx < 0 ? 180.0 : 0.0;
                else
                    az = dx < 0 ? 270.0 : 90.0;
            }
            else if (delta < 0)
            {
                az = dx >= 0 ? 90.0 - delta : 270.0 - delta;
            }
            else
            {
                az = dy >= 0 ? 90.0 - delta : 270.0 - delta;
            }
            return az;
        }

        private static double TangentAzimutAtStartForArc(Punto3d start, Punto3d center, EjeTrazado.sentidoCurva sentido)
        {
            var dx = start.coordenadaX - center.coordenadaX;
            var dy = start.coordenadaY - center.coordenadaY;

            // Azimut radial de centro a inicio
            double azRadial;
            if (dx == 0 || dy == 0)
            {
                if (dy == 0)
                    azRadial = dx < 0 ? 180.0 : 0.0;
                else
                    azRadial = dx < 0 ? 270.0 : 90.0;
            }
            else
            {
                var delta = Math.Atan(dx / dy) * 180.0 / Math.PI;
                if (delta == 0)
                {
                    azRadial = dy == 0 ? (dx < 0 ? 180.0 : 0.0) : (dx < 0 ? 270.0 : 90.0);
                }
                else if (delta < 0)
                {
                    azRadial = dx >= 0 ? 90.0 - delta : 270.0 - delta;
                }
                else
                {
                    azRadial = dy >= 0 ? 90.0 - delta : 270.0 - delta;
                }
            }

            // Tangente = radial ± 90 según sentido (Horario suma, Antihorario resta)
            var azTan = sentido == EjeTrazado.sentidoCurva.Horario ? azRadial + 90.0 : azRadial - 90.0;
            if (azTan >= 360.0) azTan -= 360.0;
            if (azTan < 0.0) azTan += 360.0;
            return azTan;
        }

        private static double Slope(Tuple<double, double> p1, Tuple<double, double> p2)
        {
            var dx = p2.Item1 - p1.Item1;
            var dy = p2.Item2 - p1.Item2;
            if (Math.Abs(dx) < 1e-12) return 0.0;
            return dy / dx;
        }
    }

    public sealed class ImportResult
    {
        public List<Componente> ComponentesPlanta { get; set; }
        public EjeTrazado Eje { get; set; }
        public List<ComponenteLong> AlzadoComponentes { get; set; }
        public List<double> Pendientes { get; set; }
        public List<Parabola> Parabolas { get; set; }
        public List<double> KVs { get; set; }
    }
}
