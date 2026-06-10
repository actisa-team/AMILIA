// ExportadorLandXml.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using EjeDeTrazado.componentes;
using tadLayShare.puntos;

public static class ExportadorLandXml
{
    private static readonly XNamespace ns = "http://www.landxml.org/schema/LandXML-1.2";
    // Clase auxiliar para representar elementos del perfil
    private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
    private static double distancia = 0;
    private static Punto3d ultimo_punto = null;

    public class ElementoPerfil
    {
        public enum TipoElemento { PVI, Parabola, Circular }
        public TipoElemento Tipo { get; set; }
        public double PK { get; set; }
        public double Cota { get; set; }
        public double Longitud { get; set; }  // Para parábolas y circulares
        public double Kv { get; set; }        // Parámetro Kv para parábolas
        public double Radio { get; set; }     // Radio para circulares
    }
    public static void Exportar(string rutaSalida,
                                string nombreAlineacion,
                                List<Componente> componentes,
                               List<Logica.Parabola> Lista_parabolas,
                               List<Curva> Lista_curvas,
                               List<Logica.Pendiente> Lista_rectas,
                                string nombreProyecto = null,bool tipo=true)
    {
        distancia = 0;
        List<ElementoPerfil> elementosPerfil = null;
        if (Lista_rectas != null)
        {
            /*if (Lista_parabolas != null && Lista_parabolas.Count > 0)
            {
                elementosPerfil = ConvertirPerfilAElementos(Lista_rectas, Lista_parabolas);
            }
            else if (Lista_curvas != null && Lista_curvas.Count > 0)
            {
                elementosPerfil = ConvertirPerfilAElementosCurva(Lista_rectas, Lista_curvas);
            }*/
            if (tipo)
            {
                elementosPerfil = ConvertirPerfilAElementos(Lista_rectas, Lista_parabolas);
            }
            else 
            {
                elementosPerfil = ConvertirPerfilAElementosCurva(Lista_rectas, Lista_curvas);
            }
        }
       

        if (componentes is null) throw new ArgumentNullException(nameof(componentes));

        // Cabecera LandXML con Units métricas (meter, squareMeter, cubicMeter, angular/direction en grados decimales).
        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement(ns + "LandXML",
                new XAttribute("version", "1.2"),
                new XAttribute("date", DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                new XAttribute("time", DateTime.UtcNow.ToString("HH:mm:ss", CultureInfo.InvariantCulture)),
                new XElement(ns + "Units",
                    new XElement(ns + "Metric",
                        new XAttribute("linearUnit", "meter"),
                        new XAttribute("areaUnit", "squareMeter"),
                        new XAttribute("volumeUnit", "cubicMeter"),
                        new XAttribute("angularUnit", "decimal degrees"),
                        new XAttribute("directionUnit", "decimal degrees")
                    )
                ),
                new XElement(ns + "Project",
                    new XAttribute("name", string.IsNullOrWhiteSpace(nombreProyecto) ? "Proyecto" : nombreProyecto)
                ),
                /*new XElement(ns + "Alignments",
                    new XAttribute("name", "Rutas"),
                    new XElement(ns + "Alignment",
                        new XAttribute("name", string.IsNullOrWhiteSpace(nombreAlineacion) ? "Eje" : nombreAlineacion),
                        // CoordGeom: secuencia de Line / Curve / Spiral
                        ConstruirCoordGeom(componentes)
                    )
                )*/
                new XElement(ns + "Alignments",
                        CrearAlignment(ns, "EJE_PRINCIPAL", componentes,
                                      elementosPerfil, 0)
                    )
            )
        );

        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(rutaSalida)) ?? ".");
        doc.Save(rutaSalida);
    }

    // Crear el elemento Alignment completo
    private static XElement CrearAlignment(
        XNamespace ns,
        string nombreEje,
        List<Componente> componentesPlanta,
        List<ElementoPerfil> elementosPerfil,
        double pkInicial)
    {
        var alignment = new XElement(ns + "Alignment",
            new XAttribute("name", nombreEje),
            new XAttribute("length", CalcularLongitudTotal(componentesPlanta).ToString(_culture)),
            new XAttribute("staStart", pkInicial.ToString(_culture))
        );

        // Geometría horizontal (planta)
        var coordGeom = CrearCoordGeom(ns, componentesPlanta);
        alignment.Add(coordGeom);

        if (elementosPerfil!=null)
        {
            // Perfil vertical
            if (elementosPerfil != null && elementosPerfil.Count > 0)
            {
                var profile = CrearProfile(ns, nombreEje + "_PROFILE",
                                          elementosPerfil, pkInicial);
                alignment.Add(profile);
            }
        }
        

        return alignment;
    }
    // Crear geometría horizontal (CoordGeom)
    private static XElement CrearCoordGeom(XNamespace ns, List<Componente> componentes)
    {
        var coordGeom = new XElement(ns + "CoordGeom");

        foreach (var componente in componentes)
        {
            XElement elemento = null;

            switch (componente.getTipoComponente())
            {
                case Componente.tipoComponente.linea:
                    elemento = CrearLine(ns, componente as Linea);
                    break;

                case Componente.tipoComponente.curva:
                    elemento = CrearCurveDesdeCurva(ns, componente as Curva);
                    break;

                case Componente.tipoComponente.clotoideEntrada:
                case Componente.tipoComponente.clotoideSalida:
                    elemento = CrearSpiralDesdeClotoide(ns, componente as Clotoide);
                    break;
            }

            if (elemento != null)
            {
                coordGeom.Add(elemento);
            }
        }

        return coordGeom;
    }
    private static XElement ConstruirCoordGeom(IEnumerable<Componente> componentes)
    {
        var coordGeom = new XElement(ns + "CoordGeom");
        foreach (var c in componentes)
        {
            switch (c.getTipoComponente())
            {
                case Componente.tipoComponente.linea:
                    coordGeom.Add(CrearLine(ns,c));
                    break;

                case Componente.tipoComponente.curva:
                    coordGeom.Add(CrearCurveDesdeCurva(ns,c));
                    break;

                case Componente.tipoComponente.clotoideEntrada:
                case Componente.tipoComponente.clotoideSalida:
                    coordGeom.Add(CrearSpiralDesdeClotoide(ns,c));
                    break;

                default:
                    throw new NotSupportedException($"Tipo no soportado: {c.getTipoComponente()}");
            }
        }
        return coordGeom;
    }

    private static XElement CrearLine(XNamespace ns, Componente c)
    {
        var p1 = c.getPuntoEntrada;
        var p2 = c.getPuntoSalida;
        var length = Dist2D(p1, p2);
        double pk = distancia;
        ultimo_punto = c.getPuntoSalida;
        distancia += length;

        double azimut = 0;
        if (c is Linea l)
        {
            azimut = l.AzimutFinal;
        }

        return new XElement(ns + "Line",
            // Atributos informativos recomendados
            new XAttribute("length", Formato(length)),
            new XAttribute("staStart", Formato(pk)),
            new XAttribute("dir", Formato(Az360To400(azimut))),
            new XElement(ns + "Start", FormatoPunto2D(p1)),
            new XElement(ns + "End", FormatoPunto2D(p2))
        );
    }

    private static XElement CrearCurveDesdeCurva(XNamespace ns, Componente c)
    {
        // Se asume que la clase derivada expone Centro, Radio y Sentido.
        // Adaptar nombres de métodos si difieren en la implementación real.
        dynamic curva = c;
        Punto3d p1 = c.getPuntoEntrada;
        Punto3d p2 = c.getPuntoSalida;
        ultimo_punto = c.getPuntoSalida;
        Punto3d centro = c.get_Centro();     // Punto3d esperado
        double radio = c.get_Radio();        // radio constante
        var sentido = c.getSentido();  // EjeTrazado.sentidoCurva (Horario/Antihorario)
        //string rot = (sentido.ToString().Contains("Horario", StringComparison.OrdinalIgnoreCase)) ? "cw" : "ccw";
        string rot = "";
        if (sentido == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
        {
            rot = "cw";
        }
        else if (sentido == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario)
        {
            rot = "ccw";
        }
        Curva cc = (Curva)c;
        var az_entrada=cc.getAzimutAtDist(0);
        var az_salida = cc.getAzimutAtDist(cc.getLongitud());
        double pk = distancia;
        distancia += cc.getLongitud();
        return new XElement(ns + "Curve",
            new XAttribute("radius", Formato(radio)),
            new XAttribute("rot", rot),
            new XAttribute("dirStart", Formato(Az360To400(az_entrada))),
            new XAttribute("dirEnd", Formato(Az360To400(az_salida))),
            new XAttribute("length", Formato(cc.getLongitud())),
            new XAttribute("staStart", Formato(pk)),
            new XAttribute("crvType", "arc"),
            new XElement(ns + "Start", FormatoPunto2D(p1)),
            new XElement(ns + "Center", FormatoPunto2D(centro)),
            new XElement(ns + "End", FormatoPunto2D(p2))
        );
    }
    public static double LongitudPolylinea(IList<double[]> puntos, bool cerrar = false)
    {
        if (puntos == null) throw new ArgumentNullException(nameof(puntos));
        int n = puntos.Count;
        if (n < 2) return 0d;

        // Validación básica de cada punto
        for (int i = 0; i < n; i++)
        {
            var p = puntos[i];
            if (p == null || p.Length < 2)
                throw new ArgumentException($"El punto en índice {i} no tiene 2 coordenadas [x,y].");
        }

        double total = 0d;

        // Suma de distancias euclídeas entre puntos consecutivos
        for (int i = 1; i < n; i++)
        {
            double dx = puntos[i][0] - puntos[i - 1][0];
            double dy = puntos[i][1] - puntos[i - 1][1];
            total += Math.Sqrt(dx * dx + dy * dy);
        }

        // Cierre opcional de la polilínea
        if (cerrar && n > 2)
        {
            double dx = puntos[0][0] - puntos[n - 1][0];
            double dy = puntos[0][1] - puntos[n - 1][1];
            total += Math.Sqrt(dx * dx + dy * dy);
        }

        return total;
    }
    private static XElement CrearSpiralDesdeClotoide(XNamespace ns, Componente c)
    {
        // Se asume que la clase derivada expone Radio (de la curva conectada) y Sentido.
        dynamic clo = c;
        //Punto3d p1 = c.getPuntoEntrada;
        Punto3d p1 = ultimo_punto;
        Punto3d p2 = c.getPuntoSalida;
        
        double L = c.getLongitud();
        double radio = c.get_Radio();          // radio de la curva circular adyacente (no infinito)
        var sentido = c.getSentido();    // EjeTrazado.sentidoCurva (Horario/Antihorario)
        //string rot = (sentido.ToString().Contains("Horario", StringComparison.OrdinalIgnoreCase)) ? "cw" : "ccw";
        string rot = "";
        if (sentido== EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
        {
            rot = "cw";
        }else if (sentido == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario)
        {
            rot = "ccw";
        }

        bool esEntrada = c.getTipoComponente() == Componente.tipoComponente.clotoideEntrada;
        string radiusStart = esEntrada ? "INF" : Formato(radio);
        string radiusEnd = esEntrada ? Formato(radio) : "INF";
        Clotoide cl = (Clotoide)c;
        double pk = distancia;
        double dis1 = cl.Get_Le_m();
        double dis2 = cl.getLe_r();
        var puntos = cl.getComponentPoints();
        double dist3 = LongitudPolylinea(puntos);
        L = dist3;
        if (dis1==0)
        {
            p2 = new Punto3d(puntos[puntos.Count() - 1][0], puntos[puntos.Count() - 1][1], 0);
            L = dis2;
        }


       
        ultimo_punto =p2;

        distancia += L;
       
        var spiral = new XElement(ns + "Spiral",
            new XAttribute("length", Formato(L)),
            new XAttribute("radiusStart", radiusStart),
            new XAttribute("radiusEnd", radiusEnd),
            new XAttribute("rot", rot),
            new XAttribute("spiType", "clothoid"),
            new XAttribute("staStart", Formato(pk)),
            new XAttribute("constant", Formato(cl.getValorA())),
            new XAttribute("dirStart", Formato(Az360To400(cl.Azimut))),
            new XAttribute("dirEnd", Formato(Az360To400(cl.Azimut))),
            new XElement(ns + "Start", FormatoPunto2D(p1)),
            // El nodo PI es opcional según consumidores; omitir si no se dispone de él con precisión.
            new XElement(ns + "End", FormatoPunto2D(p2))
        );

        // Si se dispone del parámetro A de la clotoide, se puede incluir como extensión Feature/Property.
        // double A = clo.getA(); // descomentar si existe
        // spiral.Add(new XElement(ns + "Feature",
        //     new XElement(ns + "Property",
        //         new XAttribute("label", "A"),
        //         new XAttribute("value", Formato(A)))));

        return spiral;
    }
    public static double Az360To400(double azDeg)
    {
        // Convierte grados (base 360) a gons (base 400)
        double azGon = azDeg * (400.0 / 360.0); // equivalente a azDeg * (10.0 / 9.0)

        // Normaliza al rango [0, 400)
        azGon %= 400.0;
        if (azGon < 0) azGon += 400.0;

        return azGon;
    }
    private static string Formato(double v) =>
        v.ToString("0.############", CultureInfo.InvariantCulture);

    private static string FormatoPunto2D(Punto3d p) =>
        $"{Formato(p.coordenadaY)} {Formato(p.coordenadaX)}";

    private static double Dist2D(Punto3d a, Punto3d b)
    {
        double dx = a.coordenadaX - b.coordenadaX;
        double dy = a.coordenadaY - b.coordenadaY;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    // NUEVO: Crear perfil vertical según formato del ejemplo
    private static XElement CrearProfile(
        XNamespace ns,
        string nombrePerfil,
        List<ElementoPerfil> elementosPerfil,
        double pkInicial)
    {
        var profile = new XElement(ns + "Profile",
            new XAttribute("name", nombrePerfil),
            new XAttribute("staStart", pkInicial.ToString(_culture))
        );

        var profAlign = new XElement(ns + "ProfAlign",
            new XAttribute("name", nombrePerfil)
        );

        // Generar elementos PVI y ParaCurve en secuencia
        foreach (var elemento in elementosPerfil)
        {
            if (elemento.Tipo == ElementoPerfil.TipoElemento.PVI)
            {
                // Formato: <PVI>PK Cota</PVI>
                profAlign.Add(new XElement(ns + "PVI",
                    $"{elemento.PK.ToString(_culture)} {elemento.Cota.ToString(_culture)}"
                ));
            }
            else if (elemento.Tipo == ElementoPerfil.TipoElemento.Parabola)
            {
                // Formato: <ParaCurve length="xxx" desc="Kv">PK Cota</ParaCurve>
                profAlign.Add(new XElement(ns + "ParaCurve",
                    new XAttribute("length", elemento.Longitud.ToString(_culture)),
                    new XAttribute("desc", elemento.Kv.ToString(_culture)),
                    $"{elemento.PK.ToString(_culture)} {elemento.Cota.ToString(_culture)}"
                ));
            }
            else if (elemento.Tipo == ElementoPerfil.TipoElemento.Circular)
            {
                // Formato: <CircCurve length="xxx" radius="R">PK Cota</CircCurve>
                profAlign.Add(new XElement(ns + "CircCurve",
                    new XAttribute("length", elemento.Longitud.ToString(_culture)),
                    new XAttribute("radius", elemento.Radio.ToString(_culture)),
                    $"{elemento.PK.ToString(_culture)} {elemento.Cota.ToString(_culture)}"
                ));
            }
        }

        profile.Add(profAlign);
        return profile;
    }
    // NUEVO: Método para convertir tus clases Pendiente y Parabola a ElementosPerfil
    public static List<ElementoPerfil> ConvertirPerfilAElementos(
        List<Logica.Pendiente> pendientes,
        List<Logica.Parabola> parabolas)
    {
        var elementos = new List<ElementoPerfil>();


        if (pendientes[0].Puntos[0].X<parabolas[0].Polilinea_Perfil[0].p.X)
        {
            elementos.Add(new ElementoPerfil
            {
                Tipo = ElementoPerfil.TipoElemento.PVI,
                PK = pendientes[0].Puntos[0].X,
                Cota = pendientes[0].Puntos[0].Y
            });
        }

        foreach (var parabola in parabolas)
        {
            if (parabola.lista_parabola != null && parabola.lista_parabola.Count > 0)
            {
                var primerPunto = parabola.polilinea_perfil.First();
                var ultimoPunto = parabola.polilinea_perfil.Last();

                double longitud = ultimoPunto.p.X - primerPunto.p.X;

                // El parámetro Kv debe ser calculado o proporcionado
                // Kv = L / |i1 - i2| donde i1 e i2 son las pendientes en %
                // Si tienes el valor max_min, úsalo, sino calcula según tus datos
                double kv = parabola.max_min; // O calcula según tus necesidades
                
                if (kv == 0)
                {
                    double kv1 = parabola.CalcularKvDesdePropiedadesPuntos();
                    double kv2 = parabola.CalcularKvDesdePuntos();
                    kv = kv2;
                }
                
                elementos.Add(new ElementoPerfil
                {
                    Tipo = ElementoPerfil.TipoElemento.Parabola,
                    PK = primerPunto.p.X,
                    Cota = primerPunto.p.Y,
                    Longitud = longitud,
                    Kv = kv
                });

                elementos.Add(new ElementoPerfil
                {
                    Tipo = ElementoPerfil.TipoElemento.PVI,
                    PK = ultimoPunto.p.X,
                    Cota = ultimoPunto.p.Y
                });
            }
        }

        if (pendientes[pendientes.Count()-1].Puntos[0].X >= parabolas[parabolas.Count()-1].Polilinea_Perfil[parabolas[parabolas.Count() - 1].polilinea_perfil.Count()-1].p.X)
        {
            elementos.Add(new ElementoPerfil
            {
                Tipo = ElementoPerfil.TipoElemento.PVI,
                PK = pendientes[pendientes.Count() - 1].Puntos[pendientes[pendientes.Count() - 1].Puntos.Count()-1].X,
                Cota = pendientes[pendientes.Count() - 1].Puntos[pendientes[pendientes.Count() - 1].Puntos.Count() - 1].Y
            });
        }

        // Procesar pendientes como PVIs
/*        if (pendientes != null)
        {
            foreach (var pendiente in pendientes)
            {
                foreach (var punto in pendiente.Puntos)
                {
                    elementos.Add(new ElementoPerfil
                    {
                        Tipo = ElementoPerfil.TipoElemento.PVI,
                        PK = punto.X,
                        Cota = punto.Y
                    });
                }
            }
        }

        // Procesar parábolas
        if (parabolas != null)
        {
            foreach (var parabola in parabolas)
            {
                if (parabola.lista_parabola != null && parabola.lista_parabola.Count > 0)
                {
                    var primerPunto = parabola.polilinea_perfil.First();
                    var ultimoPunto = parabola.polilinea_perfil.Last();

                    double longitud = ultimoPunto.p.X - primerPunto.p.X;

                    // El parámetro Kv debe ser calculado o proporcionado
                    // Kv = L / |i1 - i2| donde i1 e i2 son las pendientes en %
                    // Si tienes el valor max_min, úsalo, sino calcula según tus datos
                    double kv = parabola.max_min; // O calcula según tus necesidades

                    elementos.Add(new ElementoPerfil
                    {
                        Tipo = ElementoPerfil.TipoElemento.Parabola,
                        PK = primerPunto.p.X,
                        Cota = primerPunto.p.Y,
                        Longitud = longitud,
                        Kv = kv
                    });
                }
            }
        }
*/
        // Ordenar por PK
       // elementos = elementos.OrderBy(e => e.PK).ToList();

        return elementos;
    }
    public static List<ElementoPerfil> ConvertirPerfilAElementosCurva(
       List<Logica.Pendiente> pendientes,
       List<Curva> curvas)
    {
        var elementos = new List<ElementoPerfil>();

        double medio_pendiente = (pendientes[0].Puntos[0].X + pendientes[0].Puntos[1].X) / 2;
        double medio_curva = (curvas[0].getPuntoEntrada.coordenadaX + curvas[0].getPuntoSalida.coordenadaX) / 2;

        if (medio_pendiente < medio_curva)
        {
            elementos.Add(new ElementoPerfil
            {
                Tipo = ElementoPerfil.TipoElemento.PVI,
                PK = pendientes[0].Puntos[0].X,
                Cota = pendientes[0].Puntos[0].Y
            });
        }

        foreach (var curva in curvas)
        {
            Punto3d punto_ini = curva.getPuntoEntrada;
            Punto3d punto_fin = curva.getPuntoSalida;

            double dx = punto_fin.coordenadaX - punto_ini.coordenadaX;
            double longitud = Math.Abs(dx);

            double kv = CalcularKvDesdePuntos(curva);
            elementos.Add(new ElementoPerfil
            {
                Tipo = ElementoPerfil.TipoElemento.Circular, // Cambiado a Circular
                PK = punto_ini.coordenadaX,
                Cota = punto_ini.coordenadaY,
                Longitud = longitud,
                Kv = kv,
                Radio = curva.getRadio // Asumiendo que curva tiene getRadio
            });

            // Pendiente inmediatamente DESPUÉS de la curva
            double pkInicioPend = punto_fin.coordenadaX;
            double cotaInicioPend = punto_fin.coordenadaY;

            elementos.Add(new ElementoPerfil
            {
                Tipo = ElementoPerfil.TipoElemento.PVI,
                PK = pkInicioPend,
                Cota = cotaInicioPend
            });

        }

        medio_pendiente = (pendientes[pendientes.Count() - 1].Puntos[0].X + pendientes[pendientes.Count() - 1].Puntos[1].X) / 2;
        medio_curva = (curvas[curvas.Count() - 1].getPuntoEntrada.coordenadaX + curvas[curvas.Count() - 1].getPuntoSalida.coordenadaX) / 2;
        if (medio_pendiente > medio_curva)
        {
            elementos.Add(new ElementoPerfil
            {
                Tipo = ElementoPerfil.TipoElemento.PVI,
                PK = pendientes[pendientes.Count() - 1].Puntos[pendientes[pendientes.Count() - 1].Puntos.Count() - 1].X,
                Cota = pendientes[pendientes.Count() - 1].Puntos[pendientes[pendientes.Count() - 1].Puntos.Count() - 1].Y
            });
        }
        else
        {
            elementos.RemoveAt(elementos.Count()-1);
        }
        return elementos;
    }
    // Métodos auxiliares
    private static string FormatearCoordenada(double x, double y)
    {
        return $"{x.ToString(_culture)} {y.ToString(_culture)}";
    }

    private static double CalcularLongitudTotal(List<Componente> componentes)
    {
        return componentes.Sum(c => c.getLongitud());
    }
    /// <summary>
    /// Calcula el parámetro Kv de la curva vertical parabólica
    /// Kv = L / θ, donde θ = |i2 - i1| (diferencia de pendientes)
    /// </summary>
    /// <param name="pendienteEntrada">Pendiente de entrada en % (puede ser positiva o negativa)</param>
    /// <param name="pendienteSalida">Pendiente de salida en % (puede ser positiva o negativa)</param>
    /// <returns>Valor de Kv en metros</returns>
    public static double CalcularKv(double pendienteEntrada, double pendienteSalida, bool usarCorreccionAngular,double longitud)
    {
        // Calcular longitud de la curva
       

        if (longitud == 0)
            return 0;
        double theta;

        if (usarCorreccionAngular)
        {
            // Convertir pendientes de porcentaje a decimal (tanto por uno)
            double p1_decimal = pendienteEntrada / 100.0;
            double p2_decimal = pendienteSalida / 100.0;

            // Calcular ángulos en radianes usando arcotangente
            double angulo1_rad = Math.Atan(p1_decimal);
            double angulo2_rad = Math.Atan(p2_decimal);

            // Diferencia angular en radianes
            theta = angulo2_rad - angulo1_rad;

            if (theta == 0)
                return 0;

            // Kv = L / θ (donde θ está en radianes)
            return longitud / theta;
        }
        else
        {
            // Método estándar: diferencia algebraica de pendientes (en tanto por uno)
            theta = pendienteSalida - pendienteEntrada / 100.0;

            if (theta == 0)
                return 0;

            // Kv = L / θ
            return longitud / theta;
        }
    }

    /// <summary>
    /// Calcula el parámetro Kv usando los puntos del perfil (Polilinea_Perfil)
    /// Determina automáticamente las pendientes desde los puntos anterior y posterior
    /// </summary>
    /// <param name="puntoAnterior">Punto anterior a la parábola para calcular pendiente de entrada</param>
    /// <param name="puntoSiguiente">Punto siguiente a la parábola para calcular pendiente de salida</param>
    /// <returns>Valor de Kv en metros</returns>
    public static double CalcularKvDesdePuntos(Curva curva)
    {
        Punto3d punto_ini = curva.getPuntoEntrada;
        Punto3d punto_fin = curva.getPuntoSalida;

        var ini_sig= curva.getPointAtDist(0.1);
        Punto3d punto_ini_sig = new Punto3d(ini_sig[0],ini_sig[1],0);

        var fin_ant = curva.getPointAtDist(curva.getLongitud()-0.1);
        Punto3d punto_fin_ant = new Punto3d(fin_ant[0], fin_ant[1], 0);

        // Calcular pendiente de entrada (desde punto anterior hasta inicio de parábola)
        double deltaX_entrada = punto_ini_sig.coordenadaX - punto_ini.coordenadaX;
        double deltaY_entrada = punto_ini_sig.coordenadaY - punto_ini.coordenadaY;
        double pendienteEntrada = 0;

        if (deltaX_entrada != 0)
            pendienteEntrada = (deltaY_entrada / deltaX_entrada) * 100; // En porcentaje

        // Calcular pendiente de salida (desde fin de parábola hasta punto siguiente)
        /*double deltaX_salida = punto_fin_ant.coordenadaX - punto_fin.coordenadaX;
        double deltaY_salida = punto_fin_ant.coordenadaY - punto_fin.coordenadaY;*/
        double deltaX_salida = punto_fin.coordenadaX - punto_fin_ant.coordenadaX;
        double deltaY_salida = punto_fin.coordenadaY - punto_fin_ant.coordenadaY;
        double pendienteSalida = 0;

        if (deltaX_salida != 0)
            pendienteSalida = (deltaY_salida / deltaX_salida) * 100; // En porcentaje
        double dx = punto_fin.coordenadaX - punto_ini.coordenadaX;
        double longitud = Math.Abs(dx);
        double kv1 = CalcularKv(pendienteEntrada, pendienteSalida, true,longitud);
        double kv2 = CalcularKv(pendienteEntrada, pendienteSalida, false, longitud);
        return kv1;
    }
}
