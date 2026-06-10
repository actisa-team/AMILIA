using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Autodesk.AutoCAD.Geometry;

using System.Globalization;
using System.Xml.Linq;
using EjeDeTrazado.componentes;
using tadLayShare.puntos;
using EjeDeTrazado.puntosDelEje;

namespace tadLayLogica.logica.LandXml
{
    public class TrazadoLandXml
    {
        public List<Componente> Trazado = new List<Componente>();
        public List<Vertice> vertices = new List<Vertice>();
        public double Longitud { get; set; }
        public double pk_ini_eje { get; set; }
        public double pk_fin_eje { get; set; }
        public double peraltecurva { get; set; }
        public TrazadoLandXml()
        {

        }
        public TrazadoLandXml(string landxml,double peraltecurva)
        {
            this.peraltecurva = peraltecurva;
            CargarXml(landxml);
        }
        public double CalcularAzimut(double x1, double y1, double x2, double y2)
        {
            // Calcula la diferencia en coordenadas
            double deltaX = x2 - x1;
            double deltaY = y2 - y1;

            // Calcula el ángulo en radianes respecto al eje Y (Norte)
            double anguloRad = Math.Atan2(deltaX, deltaY);

            // Convierte a grados
            double anguloDeg = anguloRad * (180.0 / Math.PI);

            // Asegura que el azimut esté en el rango [0, 360)
            if (anguloDeg < 0)
                anguloDeg += 360.0;

            return anguloDeg;
        }
        private void CargarXml(string file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            char[] separator = new char[] { ' ' };
            string text;
            string[] coords;
            double az;
            double longuitud;
            double pk = 0;
            string sentido;
            bool introducida = false;
            XmlNodeList alineaciones = xmlDoc.GetElementsByTagName("Alignment");
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("lxml", "http://www.landxml.org/schema/LandXML-1.2");
            double dist_clotoide_ant = 0;
            for (int i = 0; i < alineaciones.Count; i++)
            {
                introducida = false;
                XmlNodeList nodo = alineaciones[i].ChildNodes;
                for (int t = 0; t < nodo.Count; t++)
                {
                    if (nodo[t].LocalName == "CoordGeom")
                    {

                        XmlNodeList Componentes = nodo[t].ChildNodes;
                        for (int j = 0; j < Componentes.Count; j++)
                        {
                            XmlNode entidad = Componentes[j];
                            if (entidad.LocalName == "Line")
                            {
                                introducida = true;
                                try
                                {
                                    az = Double.Parse(entidad.Attributes["dir"].Value, CultureInfo.InvariantCulture);

                                    az = 90 * az / 100;
                                }
                                catch
                                {
                                    az = 0;
                                }
                              

                                /*az = 90 - az;
                                if (az<0)
                                {
                                    az += 360;
                                }*/
                                longuitud = Double.Parse(entidad.Attributes["length"].Value, CultureInfo.InvariantCulture);
                                text = entidad.FirstChild.InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d start = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                XmlNodeList parametros = entidad.ChildNodes;
                                text = parametros[1].InnerText;
                                //text = entidad.LastChild.InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d end = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                if (az==0)
                                {
                                    az=CalcularAzimut(start.coordenadaX,start.coordenadaY,end.coordenadaX,end.coordenadaY);
                                }
                                Linea recta = new Linea(start, end, pk, 2, az);
                                pk = pk + longuitud;
                                Trazado.Add(recta);
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

                            }
                            if (entidad.LocalName == "Curve")
                            {
                                introducida = true;
                                sentido = entidad.Attributes["rot"].Value;
                                double az_ini = Double.Parse(entidad.Attributes["dirStart"].Value, CultureInfo.InvariantCulture);
                                az_ini = 90 * az_ini / 100;
                                //az_ini = (az_ini - 90);
                                if (az_ini >= 360)
                                {
                                    az_ini -= 360;
                                }
                                if (az_ini < 0)
                                {
                                    az_ini += 360;
                                }
                                az_ini = 360 - az_ini;

                                double az_fin = Double.Parse(entidad.Attributes["dirEnd"].Value, CultureInfo.InvariantCulture);
                                az_fin = 90 * az_fin / 100;
                                //az_fin =(az_fin - 90);
                                if (az_fin >= 360)
                                {
                                    az_fin -= 360;
                                }
                                if (az_fin < 0)
                                {
                                    az_fin += 360;
                                }
                                az_fin = 360 - az_fin;


                                longuitud = Double.Parse(entidad.Attributes["length"].Value, CultureInfo.InvariantCulture);
                                double radio = Double.Parse(entidad.Attributes["radius"].Value, CultureInfo.InvariantCulture);
                                XmlNodeList parametros = entidad.ChildNodes;
                                text = parametros[0].InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d start = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                text = parametros[1].InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d centro = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                text = parametros[2].InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d end = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);

                                EjeTrazado.sentidoCurva sentido1 = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario;
                                if (pk==0)
                                {
                                    dist_clotoide_ant = 0.1;
                                }
                                var curva = new Curva(start, end, centro, radio, pk, this.peraltecurva, 2,
                                    EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario, dist_clotoide_ant);

                                if (sentido == "cw")
                                {
                                    curva = new Curva(start, end, centro, radio, pk, this.peraltecurva, 2,
                                        EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario, dist_clotoide_ant);
                                    sentido1 = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;
                                }


                                //Curva curva = new Curva(centro,start,end,radio,az_ini,az_fin,sentido,pk,pk+longuitud);
                                pk = pk + longuitud;
                                Trazado.Add(curva);



                                // Azimut tangente en inicio = radial +/− 90º según sentido
                                var azi = TangentAzimutAtStartForArc(start, centro, sentido1);
                                // Vértice con datos de curva (radio reducido no aplica aquí, se usa radio)
                                var vert = new Vertice(
                                    start,
                                    azi,
                                    sentido1,
                                    radio,
                                    EjeTrazado.tipoCurva.cp,
                                    EjeTrazado.tipoSegmento.noValorado,
                                    0,
                                    centro
                                );
                                vertices.Add(vert);
                            }
                            if (entidad.LocalName == "Spiral")
                            {
                                introducida = true;
                                longuitud = Double.Parse(entidad.Attributes["length"].Value, CultureInfo.InvariantCulture);
                                sentido = entidad.Attributes["rot"].Value;
                                double A = 0;
                                try
                                {
                                    if (entidad.Attributes["theta"] != null)
                                    {
                                        A = Double.Parse(entidad.Attributes["theta"].Value, CultureInfo.InvariantCulture);
                                    }
                                    else if (entidad.Attributes["constant"] != null)
                                    {
                                        A = Double.Parse(entidad.Attributes["constant"].Value, CultureInfo.InvariantCulture);
                                    }

                                }
                                catch
                                {
                                    A = Double.Parse(entidad.Attributes["constant"].Value, CultureInfo.InvariantCulture);
                                }

                                text = entidad.FirstChild.InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d start = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                XmlNodeList parametros = entidad.ChildNodes;
                                if (parametros.Count==3)
                                {
                                    text = parametros[2].InnerText;
                                }
                                else
                                {
                                    text = parametros[1].InnerText;
                                }
                                
                                //text = entidad.LastChild.InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d end = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                var sentido_ = (sentido == "ccw")
                                        ? EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario
                                        : EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;


                                Clotoide clotoide;
                                if (entidad.Attributes["radiusEnd"].Value == "INF")
                                {
                                   
                                    double az_salida = Double.Parse(entidad.Attributes["dirEnd"].Value, CultureInfo.InvariantCulture);
                                    double radio = Double.Parse(entidad.Attributes["radiusStart"].Value, CultureInfo.InvariantCulture);
                                    if (A == 0)
                                    {
                                        A = longuitud * radio;
                                    }
                                    // Deflexión aproximada de la transición: theta = L/(2R) [rad] => grados
                                    double deltaDeg = (!double.IsInfinity(radio) && radio > 0)
                                        ? (longuitud / (2.0 * radio)) * 180.0 / Math.PI
                                        : 0.0;
                                    double azBaseDeg = AzimuthDeg(start, end);
                                    //clotoide = new Clotoide(start, end, sentido, pk, pk + longuitud, A, longuitud,"Salida",radio);
                                    az = 90 * az_salida / 100;

                                    clotoide = new Clotoide(
                                        start, end,
                                        radio, // mRc debe ser finito si se usa fórmula L=A^2/R
                                        pk,
                                        sentido_,
                                        0.0,     // peralte anterior
                                        0.0,     // peralte posterior
                                        false,
                                        EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.salida,
                                        az,
                                        false,
                                        deltaDeg,
                                        true,
                                        longuitud,
                                        A        // iA si está disponible/calculado
                                    );
                                }
                                else
                                {
                                    double az_entrada = Double.Parse(entidad.Attributes["dirStart"].Value, CultureInfo.InvariantCulture);
                                    double radio = Double.Parse(entidad.Attributes["radiusEnd"].Value, CultureInfo.InvariantCulture);
                                    if (A == 0)
                                    {
                                        A = longuitud * radio;

                                    }
                                    // Deflexión aproximada de la transición: theta = L/(2R) [rad] => grados
                                    double deltaDeg = (!double.IsInfinity(radio) && radio > 0)
                                        ? (longuitud / (2.0 * radio)) * 180.0 / Math.PI
                                        : 0.0;
                                    double azBaseDeg = AzimuthDeg(start, end);
                                    //clotoide = new Clotoide(start, end, sentido, pk, pk + longuitud, A, longuitud,"Entrada",radio);
                                    az = 90* az_entrada/100;
                                    clotoide = new Clotoide(
                                        start, end,
                                        radio, // mRc debe ser finito si se usa fórmula L=A^2/R
                                        pk,
                                        sentido_,
                                        0.0,     // peralte anterior
                                        0.0,     // peralte posterior
                                        false,
                                        EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.entrada,
                                        az,
                                        false,
                                        deltaDeg,
                                        true,
                                        longuitud,
                                        A        // iA si está disponible/calculado
                                    );
                                }




                                //double az_entrada = Double.Parse(entidad.Attributes["dirStart"].Value, CultureInfo.InvariantCulture);
                                //double az_salida = Double.Parse(entidad.Attributes["dirEnd"].Value, CultureInfo.InvariantCulture);
                                //if (clotoide.get_tipoclotoide() == "Entrada")
                                //{
                                //    az = 90 * az_entrada / 100;
                                //    /* az = 90 - az;
                                //     if (az < 0)
                                //     {
                                //         az += 360;
                                //     }*/
                                //    clotoide.set_azimut(az);
                                //}
                                //else
                                //{
                                //    az = 90 * az_salida / 100;
                                //    /*az = 90 - az;
                                //    if (az < 0)
                                //    {
                                //        az += 360;
                                //    }*/
                                //    clotoide.set_azimut(az);
                                //}
                                dist_clotoide_ant = longuitud;
                                pk = pk + longuitud;
                                Trazado.Add(clotoide);
            
                                // Vértice representativo en el arranque de la transición
                                var vert = new Vertice(
                                    start,
                                    az,
                                    sentido_,
                                    clotoide.getRadio,
                                    EjeTrazado.tipoCurva.cnp, // transición
                                    EjeTrazado.tipoSegmento.noValorado,
                                    A,
                                    new Punto3d(0, 0, 0)
                                );
                                vertices.Add(vert);


                            }
                            if (introducida)
                            {
                                pk_ini_eje = Double.Parse(alineaciones[i].Attributes["staStart"].Value, CultureInfo.InvariantCulture);
                            }
                        }
                    }
                }

            }
            Longitud = pk;
        }
        private static double AzimuthDeg(Punto3d a, Punto3d b)
        {
            double dx = b.coordenadaX - a.coordenadaX;
            double dy = b.coordenadaY - a.coordenadaY;
            double ang = Math.Atan2(dy, dx) * 180.0 / Math.PI;
            return (ang < 0) ? ang + 360.0 : ang;
        }
        private static bool IsInf(double? v) => v.HasValue && double.IsPositiveInfinity(v.Value);
        private string[] ArreglarArray(string[] coord)
        {
            List<string> lista = new List<string>(coord);

            // Eliminar todos los elementos vacíos
            lista.RemoveAll(string.IsNullOrEmpty);

            return lista.ToArray().Where(x => x != "\r\n").ToArray();
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
    }
}
