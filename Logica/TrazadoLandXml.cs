using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Autodesk.AutoCAD.Geometry;
using DTOs;
using Logica.Componentes;

namespace Terraplan.Logica
{
    public class TrazadoLandXml
    {
        public List<Componente> Trazado = new List<Componente>();
        public double Longitud { get; set; }
        public double pk_ini_eje { get; set; }
        public double pk_fin_eje { get; set; }
        public TrazadoLandXml()
        {

        }
        public TrazadoLandXml(string landxml)
        {
            CargarXml(landxml);
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
            XmlNodeList alineaciones= xmlDoc.GetElementsByTagName("Alignment");
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("lxml", "http://www.landxml.org/schema/LandXML-1.2");

            for (int i=0; i<alineaciones.Count;i++)
            {
                introducida = false;
                XmlNodeList nodo = alineaciones[i].ChildNodes;
                for (int t=0;t<nodo.Count;t++)
                {
                    if (nodo[t].LocalName == "CoordGeom")
                    {
                       
                        XmlNodeList Componentes = nodo[t].ChildNodes;
                        for (int j=0;j<Componentes.Count;j++)
                        {
                            XmlNode entidad = Componentes[j];
                            if (entidad.LocalName=="Line")
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
                                if (az == 0)
                                {
                                    az = CalcularAzimut(start.X, start.Y, end.X, end.Y);
                                }
                                Recta recta = new Recta(start,end,pk,pk+longuitud,az);
                                pk = pk + longuitud;
                                Trazado.Add(recta);
                            }
                            if (entidad.LocalName =="Curve")
                            {
                                introducida = true;
                                sentido = entidad.Attributes["rot"].Value;
                                double az_ini = Double.Parse(entidad.Attributes["dirStart"].Value, CultureInfo.InvariantCulture);
                                az_ini= 90 * az_ini / 100;
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
                                double radio= Double.Parse(entidad.Attributes["radius"].Value, CultureInfo.InvariantCulture);
                                XmlNodeList parametros=entidad.ChildNodes;
                                text = parametros[0].InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d start = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                text = parametros[1].InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d centro = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                text = parametros[2].InnerText;
                                coords = ArreglarArray(text.Split(separator));
                                Punto3d end = new Punto3d(Double.Parse(coords[1], CultureInfo.InvariantCulture), Double.Parse(coords[0], CultureInfo.InvariantCulture), 0);
                                Curva curva = new Curva(centro,start,end,radio,az_ini,az_fin,sentido,pk,pk+longuitud);
                                pk = pk + longuitud;
                                Trazado.Add(curva);

                            }
                            if (entidad.LocalName == "Spiral")
                            {
                                introducida = true;
                                longuitud = Double.Parse(entidad.Attributes["length"].Value, CultureInfo.InvariantCulture);
                                sentido = entidad.Attributes["rot"].Value;
                                double A=0;
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
                                if (parametros.Count == 3)
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
                                
                                Clotoide clotoide;
                                if (entidad.Attributes["radiusEnd"].Value=="INF")
                                {
                                    double radio = Double.Parse(entidad.Attributes["radiusStart"].Value, CultureInfo.InvariantCulture);
                                    if (A==0){
                                        A = longuitud * radio;
                                    }
                                    clotoide = new Clotoide(start, end, sentido, pk, pk + longuitud, A, longuitud,"Salida",radio);
                                }
                                else
                                {
                                    double radio = Double.Parse(entidad.Attributes["radiusEnd"].Value, CultureInfo.InvariantCulture);
                                    if (A == 0)
                                    {
                                        A = longuitud * radio;
                                    }
                                    clotoide = new Clotoide(start, end, sentido, pk, pk + longuitud, A, longuitud,"Entrada",radio);
                                }
                                double az_entrada= Double.Parse(entidad.Attributes["dirStart"].Value, CultureInfo.InvariantCulture);
                                double az_salida = Double.Parse(entidad.Attributes["dirEnd"].Value, CultureInfo.InvariantCulture);
                                if (clotoide.get_tipoclotoide() == "Entrada")
                                {
                                    az = 90 * az_entrada / 100;
                                   /* az = 90 - az;
                                    if (az < 0)
                                    {
                                        az += 360;
                                    }*/
                                    clotoide.set_azimut(az);
                                }
                                else
                                {
                                    az = 90 * az_salida / 100;
                                    /*az = 90 - az;
                                    if (az < 0)
                                    {
                                        az += 360;
                                    }*/
                                    clotoide.set_azimut(az);
                                }

                                pk = pk + longuitud;
                                Trazado.Add(clotoide);
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
        private string[] ArreglarArray(string[] coord)
        {
            List<string> lista = new List<string>(coord);

            // Eliminar todos los elementos vacíos
            lista.RemoveAll(string.IsNullOrEmpty);

            return lista.ToArray().Where(x => x != "\r\n").ToArray();
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
    }
}
