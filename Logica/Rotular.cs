using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using EjeDeTrazado.componentes;
using engCadNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Rotular
    {
        private double distancia_global=0;
        private double distancia_global2 = 100;
        private double rotulacion;
        public Rotular(double r)
        {
            this.rotulacion = r;
            if (r<1)
            {
                rotulacion = 100;
            }
        }
        
        public void Dibujar_Transversales(EjeDeTrazado.componentes.Componente c)
        {
            int contador = 0;
            double az=0;
            List<double[]> componentPoint_ant = new List<double[]>();
            componentPoint_ant.Add(new double[] { 0, 0 });
            double[] componentPoint = new double[2];
            double distancia = 0;
            double distancia_ac = 0;
            double i = 0;
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
            {
                Linea elemento = (Linea)c;
                i = distancia_global;
                while (i < elemento.getPkFin - 0.1)
                {
                    componentPoint[0] = c.getPointAtDist(i)[0];
                    componentPoint[1] = c.getPointAtDist(i)[1];
                    if (contador != 0)
                    {
                        az = elemento.AzimutFinal;
                        //az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                        az = 360 - az;

                        if (i == distancia_global2)
                        {
                            double x = componentPoint[0] + 4 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint[1] + 4 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 7, "Linea_Rotulacion_Recta-100");
                            oTexto.addText2D("Pk: " + getStringPK(i), x, y, 4*(rotulacion/100), az * Math.PI / 180, 7, "Rotulacion-Recta-100");
                            distancia_global2 += 100;
                        }
                        else
                        {
                            double x = componentPoint[0] + 2 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint[1] + 2 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 2, "Linea_Rotulacion_Recta");
                            oTexto.addText2D("Pk: " + getStringPK(i), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 2, "Rotulacion-Recta");
                        }
                        i = i + 20;
                    }

                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;
                    
                }
                distancia_global = i;
            }
            if (c.getTipoComponente()== EjeDeTrazado.componentes.Componente.tipoComponente.curva)
            {
                Curva elemento = (Curva)c;
                i = distancia_global;
                while (i < elemento.getPkFin - 0.1)
                {
                    if (i>0)
                    {
                        componentPoint_ant[0][0] = elemento.getPointAtDist(i - 0.01)[0];
                        componentPoint_ant[0][1] = elemento.getPointAtDist(i - 0.01)[1];
                        componentPoint[0] = c.getPointAtDist(i)[0];
                        componentPoint[1] = c.getPointAtDist(i)[1];
                    }
                    else
                    {
                        componentPoint_ant[0][0] = elemento.getPointAtDist(i)[0];
                        componentPoint_ant[0][1] = elemento.getPointAtDist(i)[1];
                        componentPoint[0] = c.getPointAtDist(i+0.0001)[0];
                        componentPoint[1] = c.getPointAtDist(i+0.0001)[1];
                    }
                    
                   /* if (contador != 0)
                    {*/
                        az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                        az = 360 - az;
                        
                        if (i== distancia_global2)
                        {
                            double x = componentPoint[0] + 4 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint[1] + 4 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 7, "Linea_Rotulacion_Curva-100");
                            oTexto.addText2D("Pk: " + getStringPK(i), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-Curva-100");
                            distancia_global2 += 100;
                        }
                        else
                        {
                            double x = componentPoint[0] + 2 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint[1] + 2 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 1, "Linea_Rotulacion_Curva");
                            oTexto.addText2D("Pk: " + getStringPK(i), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-Curva");
                        }
                        i = i + 20;
                    /*}
                    
                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;*/
                   
                }
                distancia_global = i;
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada)
            {
                Clotoide elemento = (Clotoide)c;
                i = distancia_global;
                while (i < elemento.getPkFin - 0.1)
                {
                    if (elemento.Get_Le_r() > 0)
                    {
                        componentPoint_ant[0][0] = elemento.getPointAtDist(elemento.Get_Le_r() + i - 0.01)[0];
                        componentPoint_ant[0][1] = elemento.getPointAtDist(elemento.Get_Le_r() + i - 0.01)[1];
                        componentPoint[0] = c.getPointAtDist(elemento.Get_Le_r() + i)[0];
                        componentPoint[1] = c.getPointAtDist(elemento.Get_Le_r() + i)[1];
                    }
                    else
                    {
                        componentPoint_ant[0][0] = elemento.getPointAtDist(i - 0.01)[0];
                        componentPoint_ant[0][1] = elemento.getPointAtDist(i - 0.01)[1];
                        componentPoint[0] = c.getPointAtDist(i)[0];
                        componentPoint[1] = c.getPointAtDist(i)[1];
                    }

                    //if (contador != 0)
                    //{
                    az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;

                        az = 360 - az;
                        if (i == distancia_global2)
                        {
                            double x = componentPoint[0] + 4 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint[1] + 4 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 7, "Linea_Rotulacion_Clo-100");
                            oTexto.addText2D("Pk: " + getStringPK(i), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-Clotoide-100");
                            distancia_global2 += 100;
                        }
                        else
                        {
                            double x = componentPoint[0] + 2 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint[1] + 2 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 3, "Linea_Rotulacion_Clo");
                            oTexto.addText2D("Pk: " + getStringPK(i), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 3, "Rotulacion-Clotoide");
                        }
                        i = i + 20;
                    /*}

                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;*/
                    
                }
                distancia_global = i;
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
            {
                Clotoide elemento = (Clotoide)c;
                i = distancia_global;
                while (i < elemento.getPkFin - 0.1)
                {
                    componentPoint_ant[0][0] = elemento.getPointAtDist(i-0.01)[0];
                    componentPoint_ant[0][1] = elemento.getPointAtDist(i-0.01)[1];
                    componentPoint[0] = c.getPointAtDist(i)[0];
                    componentPoint[1] = c.getPointAtDist(i)[1];
                    //if (contador != 0)
                    //{
                        az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                        az = 360 - az;
                        if (i == distancia_global2)
                        {
                            double x = componentPoint[0] + 4 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint[1] + 4 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 7, "Linea_Rotulacion_Clo-100");
                            oTexto.addText2D("Pk: " + getStringPK(i), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-Clotoide-100");
                            distancia_global2 += 100;
                        }
                        else
                        {
                            double x = componentPoint[0] + 2 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint[1] + 2 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 3, "Linea_Rotulacion_Clo");
                            oTexto.addText2D("Pk: " + getStringPK(i), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 3, "Rotulacion-Clotoide");
                        }
                        i = i + 20;
                    //}

                    //componentPoint_ant[0][0] = componentPoint[0];
                    //componentPoint_ant[0][1] = componentPoint[1];
                    //contador++;
                    

                }
                distancia_global = i;
            }
        }
        public void Dibujar_Singulares(EjeDeTrazado.componentes.Componente c)
        {
            int contador = 0;
            double az = 0;
            List<double[]> componentPoint_ant = new List<double[]>();
            componentPoint_ant.Add(new double[] { 0, 0 });
            double distancia = 0;
            double distancia_ac = 0;
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
            {
                Linea elemento = (Linea)c;
                foreach (var componentPoint in c.getComponentPoints())
                {
                    if (contador != 0)
                    {

                        az = elemento.AzimutFinal;
                        az = 360 - az;
                        double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                        Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 2, "Linea_Rotulacion_Recta_inicial");
                        x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 2, "Rotulacion-Recta_inicial");
                        double miDir;
                        if (az + 90 > 360)
                        {
                            miDir = az + 90 - 360;
                        }
                        else
                        {
                            miDir = az + 90;
                        }

                        x = x - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                        y = y - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                        oTexto.addText2D("Az: " + Math.Round(elemento.AzimutFinal, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 2, "Rotulacion-Recta_inicial");
                    }
                    if (contador == 1)
                    {
                        break;
                    }
                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;
                }
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
            {
                Curva elemento = (Curva)c;
                foreach (var componentPoint in c.getComponentPoints())
                {
                    if (contador != 0)
                    {
                        
                        az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                        az = 360 - az;
                        double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        
                        Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 1, "Linea_Rotulacion_Curva_inicial");
                        x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-Curva_inicial");
                        double miDir;
                        if (az + 90 > 360)
                        {
                            miDir = az + 90 - 360;
                        }
                        else
                        {
                            miDir = az + 90;
                        }

                        x = x - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                        y = y - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                        oTexto.addText2D(" R: " + Math.Round(elemento.getRadio, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-Curva_inicial");
                    }
                    if (contador==1)
                    {
                        break;
                    }
                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;
                }
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada)
            {
                Clotoide elemento = (Clotoide)c;
                foreach (var componentPoint in c.getComponentPoints())
                {
                    if (contador != 0)
                    {
                        
                        az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                        az = 360 - az;
                        double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 3, "Linea_Rotulacion_Clo_inicial");
                        x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 3, "Rotulacion-Clotoide_inicial");
                        double miDir;
                        if (az + 90 > 360)
                        {
                            miDir = az + 90 - 360;
                        }
                        else
                        {
                            miDir = az + 90;
                        }

                        x = x - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                        y = y - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                        oTexto.addText2D(" A: " + Math.Round(elemento.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 3, "Rotulacion-Clotoide_inicial");
                    }
                    if (contador == 1)
                    {
                        break;
                    }
                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;
                }
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
            {
                Clotoide elemento = (Clotoide)c;
                foreach (var componentPoint in c.getComponentPoints())
                {
                    if (contador != 0)
                    {
                        az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                        az = 360 - az;
                        double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 3, "Linea_Rotulacion_Clo_inicial");
                        x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 3, "Rotulacion-Clotoide_inicial");
                        double miDir;
                        if (az + 90 > 360)
                        {
                            miDir = az + 90 - 360;
                        }
                        else
                        {
                            miDir = az + 90;
                        }

                        x = x - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                        y = y - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                        oTexto.addText2D(" A: " + Math.Round(elemento.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 3, "Rotulacion-Clotoide_inicial");
                    }
                    if (contador == 1)
                    {
                        break;
                    }
                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;
                }
            }
        }
        public void Dibujar_Singulares(EjeDeTrazado.componentes.Componente c, EjeDeTrazado.componentes.Componente c_ant)
        {
            int contador = 0;
            double az = 0;
            List<double[]> componentPoint_ant = new List<double[]>();
            componentPoint_ant.Add(new double[] { 0, 0 });
            double distancia = 0;
            double distancia_ac = 0;
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
            {
                Linea elemento = (Linea)c;
                foreach (var componentPoint in c.getComponentPoints())
                {
                    if (contador != 0)
                    {

                        az = elemento.AzimutFinal;
                        az = 360 - az;
                        double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        
                        Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 2, "Linea_Rotulacion_Recta_inicial");
                        x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        double x_p = x;
                        double y_p = y;

                        double miDir;
                        if (az + 90 > 360)
                        {
                            miDir = az + 90 - 360;
                        }
                        else
                        {
                            miDir = az + 90;
                        }

                        x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                        y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                        oTexto.addText2D("RECTA", x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                        oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                        
                        x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                        y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                        if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                        {
                            Curva elemento_ant = (Curva)c_ant;
                            oTexto.addText2D("R: " + Math.Round(elemento_ant.getRadio, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                        }
                        if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada)
                        {
                            Clotoide elemento_ant = (Clotoide)c_ant;
                            oTexto.addText2D("A: " + Math.Round(elemento_ant.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                        }
                        if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
                        {
                            Clotoide elemento_ant = (Clotoide)c_ant;
                            oTexto.addText2D("A: " + Math.Round(elemento_ant.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                        }
                        //oTexto.addText2D("Az: " + Math.Round(elemento.AzimutFinal, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 2, "Rotulacion-Recta_inicial");
                    }
                    if (contador == 1)
                    {
                        break;
                    }
                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;
                }
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
            {
                Curva elemento = (Curva)c;
                foreach (var componentPoint in c.getComponentPoints())
                {
                    if (contador != 0)
                    {

                        az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                        az = 360 - az;
                        double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                        Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 1, "Linea_Rotulacion_Curva_inicial");
                        x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                        y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                        double x_p = x;
                        double y_p = y;
                        oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                        double miDir;
                        if (az + 90 > 360)
                        {
                            miDir = az + 90 - 360;
                        }
                        else
                        {
                            miDir = az + 90;
                        }

                        x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                        y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                        oTexto.addText2D(" R: " + Math.Round(elemento.getRadio, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                        x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                        y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                        if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                        {
                            
                            oTexto.addText2D("RECTA", x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                        }
                        if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada)
                        {
                            Clotoide elemento_ant = (Clotoide)c_ant;
                            oTexto.addText2D("A: " + Math.Round(elemento_ant.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                        }
                        if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
                        {
                            Clotoide elemento_ant = (Clotoide)c_ant;
                            oTexto.addText2D("A: " + Math.Round(elemento_ant.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                        }
                    }
                    if (contador == 1)
                    {
                        break;
                    }
                    componentPoint_ant[0][0] = componentPoint[0];
                    componentPoint_ant[0][1] = componentPoint[1];
                    contador++;
                }
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada)
            {
                Clotoide elemento = (Clotoide)c;
                if (elemento.Get_Le_r() > 0)
                {
                    foreach (var componentPoint in c.getComponentPoints(c.Get_Le_r()+c.getPkIni))
                    {
                        if (contador != 0)
                        {
                            az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                            az = 360 - az;
                            double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 3, "Linea_Rotulacion_Clo_inicial");
                            x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            double x_p = x;
                            double y_p = y;
                            oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            double miDir;
                            if (az + 90 > 360)
                            {
                                miDir = az + 90 - 360;
                            }
                            else
                            {
                                miDir = az + 90;
                            }

                            x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                            y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                            oTexto.addText2D(" A: " + Math.Round(elemento.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                            y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                            {

                                oTexto.addText2D("RECTA", x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                            }
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                            {
                                Curva elemento_ant = (Curva)c_ant;
                                oTexto.addText2D("R: " + Math.Round(elemento_ant.getRadio, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                            }
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
                            {
                                Clotoide elemento_ant = (Clotoide)c_ant;
                                oTexto.addText2D("A: " + Math.Round(elemento_ant.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            }
                        }
                        if (contador == 1)
                        {
                            break;
                        }
                        componentPoint_ant[0][0] = componentPoint[0];
                        componentPoint_ant[0][1] = componentPoint[1];
                        contador++;
                    }
                }
                else
                {
                    foreach (var componentPoint in c.getComponentPoints())
                    {
                        if (contador != 0)
                        {
                            az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                            az = 360 - az;
                            double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 3, "Linea_Rotulacion_Clo_inicial");
                            x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            double x_p = x;
                            double y_p = y;
                            oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            double miDir;
                            if (az + 90 > 360)
                            {
                                miDir = az + 90 - 360;
                            }
                            else
                            {
                                miDir = az + 90;
                            }

                            x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                            y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                            oTexto.addText2D(" A: " + Math.Round(elemento.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                            y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                            {

                                oTexto.addText2D("RECTA", x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                            }
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                            {
                                Curva elemento_ant = (Curva)c_ant;
                                oTexto.addText2D("R: " + Math.Round(elemento_ant.getRadio, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                            }
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
                            {
                                Clotoide elemento_ant = (Clotoide)c_ant;
                                oTexto.addText2D("A: " + Math.Round(elemento_ant.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            }
                        }
                        if (contador == 1)
                        {
                            break;
                        }
                        componentPoint_ant[0][0] = componentPoint[0];
                        componentPoint_ant[0][1] = componentPoint[1];
                        contador++;
                    }
                }
                
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
            {
                Clotoide elemento = (Clotoide)c;
                if (elemento.Get_Le_r() > 0)
                {
                    foreach (var componentPoint in c.getComponentPoints(c.Get_Le_r()))
                    {
                        if (contador != 0)
                        {
                            az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                            az = 360 - az;
                            double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 3, "Linea_Rotulacion_Clo_inicial");
                            x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            double x_p = x;
                            double y_p = y;
                            oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            double miDir;
                            if (az + 90 > 360)
                            {
                                miDir = az + 90 - 360;
                            }
                            else
                            {
                                miDir = az + 90;
                            }

                            x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                            y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                            oTexto.addText2D(" A: " + Math.Round(elemento.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                            y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                            {

                                oTexto.addText2D("RECTA", x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                            }
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                            {
                                Curva elemento_ant = (Curva)c_ant;
                                oTexto.addText2D("R: " + Math.Round(elemento_ant.getRadio, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                            }
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada)
                            {
                                Clotoide elemento_ant = (Clotoide)c_ant;
                                oTexto.addText2D("A: " + Math.Round(elemento_ant.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            }
                        }
                        if (contador == 1)
                        {
                            break;
                        }
                        componentPoint_ant[0][0] = componentPoint[0];
                        componentPoint_ant[0][1] = componentPoint[1];
                        contador++;
                    }
                }
                else
                {
                    foreach (var componentPoint in c.getComponentPoints())
                    {
                        if (contador != 0)
                        {
                            az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                            az = 360 - az;
                            double x = componentPoint_ant[0][0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            double y = componentPoint_ant[0][1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            Dibujar_r(new Point2d(componentPoint_ant[0][0], componentPoint_ant[0][1]), new Point2d(x, y), 3, "Linea_Rotulacion_Clo_inicial");
                            x = componentPoint_ant[0][0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                            y = componentPoint_ant[0][1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                            double x_p = x;
                            double y_p = y;
                            oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkIni, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            double miDir;
                            if (az + 90 > 360)
                            {
                                miDir = az + 90 - 360;
                            }
                            else
                            {
                                miDir = az + 90;
                            }

                            x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                            y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                            oTexto.addText2D(" A: " + Math.Round(elemento.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                            y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                            {

                                oTexto.addText2D("RECTA", x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                            }
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                            {
                                Curva elemento_ant = (Curva)c_ant;
                                oTexto.addText2D("R: " + Math.Round(elemento_ant.getRadio, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");

                            }
                            if (c_ant.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada)
                            {
                                Clotoide elemento_ant = (Clotoide)c_ant;
                                oTexto.addText2D("A: " + Math.Round(elemento_ant.getValorA(), 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-singular");
                            }
                        }
                        if (contador == 1)
                        {
                            break;
                        }
                        componentPoint_ant[0][0] = componentPoint[0];
                        componentPoint_ant[0][1] = componentPoint[1];
                        contador++;
                    }
                }
                
            }
        }
        public void Dibujar_Final(EjeDeTrazado.componentes.Componente c)
        {
            int contador = 0;
            double az = 0;
            List<double[]> componentPoint_ant = new List<double[]>();
            componentPoint_ant.Add(new double[] { 0, 0 });
            double[] componentPoint = new double[2];
            double distancia = 0;
            double distancia_ac = 0;
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
            {
                Linea elemento = (Linea)c;
                componentPoint_ant[0][0] = elemento.getPointAtDist(c.getPkFin - 0.01)[0];
                componentPoint_ant[0][1] = elemento.getPointAtDist(c.getPkFin - 0.01)[1];
                componentPoint[0] = elemento.getPointAtDist(c.getPkFin)[0];
                componentPoint[1] = elemento.getPointAtDist(c.getPkFin)[1];
                az = elemento.AzimutFinal;
                az = 360 - az;
                double x = componentPoint[0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                double y = componentPoint[1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 2, "Linea_Rotulacion_Recta_inicial");
                x = componentPoint[0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = componentPoint[1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkFin, 2)), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 2, "Rotulacion-Recta_inicial");
                double miDir;
                if (az + 90 > 360)
                {
                    miDir = az + 90 - 360;
                }
                else
                {
                    miDir = az + 90;
                }
                x = x - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                oTexto.addText2D("Az: " + Math.Round(elemento.AzimutFinal, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 2, "Rotulacion-Recta_inicial");
            }
            if (c.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
            {
                Curva elemento = (Curva)c;
                componentPoint_ant[0][0] = elemento.getPointAtDist(c.getPkFin - 0.01)[0];
                componentPoint_ant[0][1] = elemento.getPointAtDist(c.getPkFin - 0.01)[1];
                componentPoint[0] = elemento.getPointAtDist(c.getPkFin)[0];
                componentPoint[1] = elemento.getPointAtDist(c.getPkFin)[1];
                az = Rellenar_centro(componentPoint[0], componentPoint[1], componentPoint_ant[0][0], componentPoint_ant[0][1], 1).Az;
                az = 360 - az;
                double x = componentPoint[0] - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                double y = componentPoint[1] - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                Dibujar_r(new Point2d(componentPoint[0], componentPoint[1]), new Point2d(x, y), 1, "Linea_Rotulacion_Curva_final");
                x = componentPoint[0] - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = componentPoint[1] - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                oTexto.addText2D("Pk: " + getStringPK(Math.Round(c.getPkFin, 2)), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-Curva_final");
                double miDir;
                if (az + 90 > 360)
                {
                    miDir = az + 90 - 360;
                }
                else
                {
                    miDir = az + 90;
                }
                x = x - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                oTexto.addText2D("R: " + Math.Round(elemento.getRadio, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-Curva_final");
            }
        }
        private string getStringPK(double i)
        {

            int miles = (int)(i / 1000);
            double d = i - miles * 1000;
            int cent = (int)(d / 100);
            d = d - cent * 100;
            int dec = (int)(d / 10);
            d = d - dec * 10;
            int uni = (int)d;
            d = d - uni;

            d = Math.Truncate(d * 100);

            string miPk = miles + "+" + cent + dec + uni;
            if (d != 0)
            {
                if (d >= 10)
                    miPk += "." + d;
                else
                    miPk += ".0" + d;
            }
            return miPk;
        }
        private void Dibujar_r(Point2d p1, Point2d p2,short color,string iLayer)
        {
            Point3dCollection poly = new Point3dCollection();
            poly.Add(new Point3d(p1.X, p1.Y, 0));
            poly.Add(new Point3d(p2.X, p2.Y, 0));


            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {

                engCadNet.oLayer.addLayer(iLayer, color, false);
                using (Transaction acTrans = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(AcCurDb2.BlockTableId,
                        OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite) as BlockTableRecord;

                    Editor e = Application.DocumentManager.MdiActiveDocument.Editor;
                    Document d = Application.DocumentManager.MdiActiveDocument;
                    Polyline3d pol = new Polyline3d(new Poly3dType(), poly, false);
                    pol.Layer = iLayer;

                    acBlkTblRec.AppendEntity(pol);

                    acTrans.AddNewlyCreatedDBObject(pol, true);

                    acTrans.Commit();
                }
            }
        }
        private Punto Rellenar_centro(double xc1, double yc1, double xc, double yc, int direccion)
        {
            Punto p, p_a;
            if (direccion == 1)
            {
                p = new Punto(new Point2d(xc1, yc1));
                p_a = new Punto(new Point2d(xc, yc));
            }
            else
            {
                p_a = new Punto(new Point2d(xc1, yc1));
                p = new Punto(new Point2d(xc, yc));
            }

            p.Dx = p.p.X - p_a.p.X;
            p.Dy = p.p.Y - p_a.p.Y;
            if (p.Dx == 0)
            {
                p.Ad1 = 0;
            }
            else
            {
                if (p.Dy == 0)
                {
                    p.Ad1 = 0;
                }
                else
                {
                    p.Ad1 = Math.Atan(p.Dy / p.Dx);
                }
            }
            p.Ad2 = p.Ad1 * (180 / Math.PI);

            if (p.Ad1 == 0)
            {
                p.signod = 0;
            }
            else
            {
                if (p.Ad1 < 0)
                {
                    p.signod = 1;
                }
                else
                {
                    p.signod = 2;
                }
            }

            if (p.Dx == 0)
            {
                p.signodx = 0;
            }
            else
            {
                if (p.Dx < 0)
                {
                    p.signodx = 1;
                }
                else
                {
                    p.signodx = 2;
                }
            }

            if (p.Dy == 0)
            {
                p.signody = 0;
            }
            else
            {
                if (p.Dy < 0)
                {
                    p.signody = 1;
                }
                else
                {
                    p.signody = 2;
                }
            }

            if (p.signod == 0)
            {
                p.Dc = 2;
            }
            else
            {
                p.Dc = 1;
            }
            if (p.Dc == 2)
            {
                if (p.Dy == 0)
                {
                    p.Orientacion = "E-W";
                }
                else
                {
                    p.Orientacion = "N-S";
                }
            }

            if (p.Dc == 2)
            {
                if (p.Orientacion == "E-W")
                {
                    if (p.Dx < 0)
                    {
                        p.Azcardinal = 270;
                    }
                    else
                    {
                        p.Azcardinal = 90;
                    }
                }
                else
                {
                    if (p.Dy < 0)
                    {
                        p.Azcardinal = 180;
                    }
                    else
                    {
                        p.Azcardinal = 0;
                    }
                }
            }

            //cuadrante
            if (p.Dc == 2)
            {
                p.cuadrante = 0;
            }
            else
            {
                if (p.Dx > 0 && p.Dy > 0)
                {
                    p.cuadrante = 1;
                }
                else
                {
                    if (p.Dx > 0 && p.Dy < 0)
                    {
                        p.cuadrante = 2;
                    }
                    else
                    {
                        if (p.Dx < 0 && p.Dy < 0)
                        {
                            p.cuadrante = 3;
                        }
                        else
                        {
                            p.cuadrante = 4;
                        }
                    }
                }
            }

            //Azimut
            if (p.Dc == 2)
            {
                p.Az = p.Azcardinal;
            }
            else
            {
                if (p.signod == 1)
                {
                    if (p.signodx == 2)
                    {
                        p.Az = 90 - p.Ad2;

                    }
                    else
                    {
                        p.Az = 270 - p.Ad2;
                    }
                }
                else
                {
                    if (p.signodx == 2)
                    {
                        p.Az = 90 - p.Ad2;

                    }
                    else
                    {
                        p.Az = 270 - p.Ad2;
                    }
                }
            }
            return p;
        }
        /// <summary>
        /// Dibuja los puntos singulares entre parabola y pendiente
        /// </summary>
        /// <param name="p">parabola</param>
        /// <param name="p_ini">punto inicial</param>
        /// <param name="p_fin">punto final</param>
        /// <param name="escala">escala de aumento de y</param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="tipo">si el trazado empieza en acuerdo la primera parte ya esta dibujada, con el valor diferente de un 1 o 3 no se pintará y con el 3 solo se pintara el principio del acuerdo</param>
        public void Dibujar_Singulares_Perfil(Parabola p,Point2d p_ini,Point2d p_fin,double escala,double p1,double p2,int tipo)
        {
            engCadNet.oLayer.addLayer("Rotulacion-singular", 1, false);
            double x2;
            double x1;
            double x;
            double pk_ini, p_ini_x, p_ini_y;
            double pk;
            double y;//x^2+x+c
            double p_fin_x;
            double p_fin_y;
            double az;
            double x_p;
            double y_p;
            double miDir;
            double kv;
            if (tipo==1 || tipo==3)
            {
                //inicial del acuerdo
                x2 = p.parabola[0];
                x1 = p.parabola[1];
                x = p.parabola[2];
                pk_ini = p_ini.X;
                pk = p_ini.X + 0.1;
                y = (pk * pk) * x2 + pk * x1 + x;//x^2+x+c
                p_ini_x = p_ini.X;
                p_ini_y = p_ini.Y * escala;
                p_fin_x = p_fin.X;
                p_fin_y = p_fin.Y * escala;
                az = Rellenar_centro(p_ini_x, p_ini_y, pk, y * escala, 1).Az;
                az = 360 - az;
                x = p_ini_x - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_ini_y - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                Dibujar_r(new Point2d(p_ini_x, p_ini_y), new Point2d(x, y), 1, "Linea_Rotulacion_Acuerdo_inicial");
                x = p_ini_x - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_ini_y - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                x_p = x;
                y_p = y;
                oTexto.addText2D("Pk: " + getStringPK(Math.Round(p_ini_x, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular");
                
                if (az + 90 > 360)
                {
                    miDir = az + 90 - 360;
                }
                else
                {
                    miDir = az + 90;
                }

                x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                kv = 1 / (2 * x2);
                oTexto.addText2D(" Kv: " + Math.Round(kv, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular");
                x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                oTexto.addText2D(" Pdte: " + Math.Round(p1, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular");

            }

            if (tipo==3)
            {

            }
            else
            {

                //final del acuerdo
                x2 = p.parabola[0];
                x1 = p.parabola[1];
                x = p.parabola[2];
                pk_ini = p_fin.X - 0.1;
                pk = p_fin.X;
                y = (pk_ini * pk_ini) * x2 + pk_ini * x1 + x;//x^2+x+c
                p_fin_x = p_fin.X;
                p_fin_y = p_fin.Y * escala;
                az = Rellenar_centro(pk_ini, y * escala, p_fin_x, p_fin_y, 1).Az;
                az = 360 - az;
                x = p_fin_x - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_fin_y - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                Dibujar_r(new Point2d(p_fin_x, p_fin_y), new Point2d(x, y), 1, "Linea_Rotulacion_Acuerdo_inicial");
                x = p_fin_x - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_fin_y - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                x_p = x;
                y_p = y;
                oTexto.addText2D("Pk: " + getStringPK(Math.Round(p_fin_x, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular");
                if (az + 90 > 360)
                {
                    miDir = az + 90 - 360;
                }
                else
                {
                    miDir = az + 90;
                }

                x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                kv = 1 / (2 * x2);
                oTexto.addText2D(" Kv: " + Math.Round(kv, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular");
                x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                oTexto.addText2D(" Pdte: " + Math.Round(p2, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular");
            }

        }
        /// <summary>
        /// Dibuja el punto inicial y final rotulado
        /// </summary>
        /// <param name="p">Pendiente a rotular</param>
        /// <param name="tipo">1 inicio y 2 final</param>
        public void Dibujar_Ini_Fin_Pendiente(Pendiente p,int tipo,double escala,double pendiente)
        {
            engCadNet.oLayer.addLayer("Rotulacion-singular_inicial", 1, false);
            engCadNet.oLayer.addLayer("Rotulacion-singular_final", 1, false);
            if (tipo==1)
            {
                double p_ini_x = p.Puntos[0].X;
                double p_ini_y = p.Puntos[0].Y * escala;
                double p_fin_x = p.Puntos[1].X;
                double p_fin_y = p.Puntos[1].Y * escala;
                double az = Rellenar_centro(p_ini_x, p_ini_y, p_fin_x, p_fin_y, 1).Az;
                az = 360 - az;
                double x = p_ini_x - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                double y = p_ini_y - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                Dibujar_r(new Point2d(p_ini_x, p_ini_y), new Point2d(x, y), 1, "Linea_Rotulacion_Inicial");
                x = p_ini_x - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_ini_y - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                double x_p = x;
                double y_p = y;

                oTexto.addText2D("Pk: " + 0, x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular_inicial");
                double miDir;
                if (az + 90 > 360)
                {
                    miDir = az + 90 - 360;
                }
                else
                {
                    miDir = az + 90;
                }

                x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                oTexto.addText2D(" Pdte: " + Math.Round(pendiente, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular_inicial");
            }
            else
            {
                double p_ini_x = p.Puntos[0].X;
                double p_ini_y = p.Puntos[0].Y * escala;
                double p_fin_x = p.Puntos[1].X;
                double p_fin_y = p.Puntos[1].Y * escala;
                double az = Rellenar_centro(p_ini_x, p_ini_y, p_fin_x, p_fin_y, 1).Az;
                az = 360 - az;
                double x = p_fin_x - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                double y = p_fin_y - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                Dibujar_r(new Point2d(p_fin_x, p_fin_y), new Point2d(x, y), 1, "Linea_Rotulacion_Final");
                x = p_fin_x - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_fin_y - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                double x_p = x;
                double y_p = y;

                oTexto.addText2D("Pk: " + getStringPK(Math.Round(p_fin_x, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular_final");
                double miDir;
                if (az + 90 > 360)
                {
                    miDir = az + 90 - 360;
                }
                else
                {
                    miDir = az + 90;
                }

                x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

                oTexto.addText2D(" Pdte: " + Math.Round(pendiente, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular_final");
            }
        }/// <summary>
         /// Dibuja el punto inicial y final rotulado
         /// </summary>
         /// <param name="p">Parabola a rotular</param>
         /// <param name="tipo">1 inicio y 2 final</param>
        public void Dibujar_Ini_Fin_Acuerdo(Parabola p, int tipo, double escala, double pkfin)
        {
            engCadNet.oLayer.addLayer("Rotulacion-singular_inicial", 1, false);
            engCadNet.oLayer.addLayer("Rotulacion-singular_final", 1, false);
            if (tipo == 1)
            {
                double x2 = p.parabola[0];
                double x1 = p.parabola[1];
                double x = p.parabola[2];
                double pk_ini = 0;
                double y = (0 * 0) * x2 + 0 * x1 + x;//x^2+x+c
                double pk = pk_ini + 0.1;
                double y1 = (pk * pk) * x2 + pk * x1 + x;//x^2+x+c
                double p_ini_x = 0;
                double p_ini_y = y * escala;
                double p_fin_x = pk;
                double p_fin_y = y1 * escala;
                double az = Rellenar_centro(p_ini_x, p_ini_y, pk, y1 * escala, 1).Az;
                az = 360 - az;
                x = p_ini_x - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_ini_y - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                Dibujar_r(new Point2d(p_ini_x, p_ini_y), new Point2d(x, y), 1, "Linea_Rotulacion_Acuerdo_inicial");
                x = p_ini_x - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_ini_y - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                double x_p = x;
                double y_p = y;
                oTexto.addText2D("Pk: " + getStringPK(Math.Round(p_ini_x, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular_inicial");
                double miDir;
                if (az + 90 > 360)
                {
                    miDir = az + 90 - 360;
                }
                else
                {
                    miDir = az + 90;
                }

                x = x_p - 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y_p - 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                double kv = 1 / (2 * x2);
                oTexto.addText2D(" Kv: " + Math.Round(kv, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular_inicial");
            }
            else
            {
                //final del acuerdo
                double x2 = p.parabola[0];
                double x1 = p.parabola[1];
                double x = p.parabola[2];
                double pk_ini = pkfin - 0.1;
                double pk = pkfin;
                double y1 = (pk_ini * pk_ini) * x2 + pk_ini * x1 + x;//x^2+x+c
                double y = (pk * pk) * x2 + pk * x1 + x;//x^2+x+c
                double p_fin_x = pkfin;
                double p_fin_y = y * escala;
                double az = Rellenar_centro(pk_ini, y1* escala, p_fin_x, p_fin_y, 1).Az;
                az = 360 - az;
                x = p_fin_x - 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_fin_y - 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

                Dibujar_r(new Point2d(p_fin_x, p_fin_y), new Point2d(x, y), 1, "Linea_Rotulacion_Acuerdo_inicial");
                x = p_fin_x - 40 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                y = p_fin_y - 40 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                double x_p = x;
                double y_p = y;
                oTexto.addText2D("Pk: " + getStringPK(Math.Round(p_fin_x, 2)), x_p, y_p, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular_final");
                double miDir;
                if (az + 90 > 360)
                {
                    miDir = az + 90 - 360;
                }
                else
                {
                    miDir = az + 90;
                }

                x = x_p + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
                y = y_p + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);
                double kv = 1 / (2 * x2);
                oTexto.addText2D(" Kv: " + Math.Round(kv, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 1, "Rotulacion-singular_final");
            }
        }
        public int Dibujar_PK_Acuerdo(Parabola p, int pk, double escala,double pk_fin,double min)
        {
            double a = p.parabola[0];
            double b = p.parabola[1];
            double c = p.parabola[2];
            double y1=0,y2=0;
            double pk2 = 0;
            int minimo = (int)Math.Truncate(min) * (int)escala - 6 * (int)escala;
            while (pk < pk_fin)
            {
                pk2 = pk - 0.1;
                y1 = (pk * pk) * a + pk * b + c;
                y2 = (pk2 * pk2) * a + pk2 * b + c;
                y1 = y1 * escala;
                y2 = y2 * escala;
                double az = Rellenar_centro(pk-0.1, y2, pk, y1, 1).Az;
                az = 360 - az;
                //double x = pk + 4 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
                //double y = y1 + 4 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
                double x = pk + 4 * (rotulacion / 100) * Math.Cos(270 * Math.PI / 180);
                double y = minimo + 4 * (rotulacion / 100) * Math.Sin(270 * Math.PI / 180);
                //Dibujar_r(new Point2d(pk, y1), new Point2d(x, y), 7, "Linea_Rotulacion_Acuerdo");
                oTexto.addText2D("Cota: " + Math.Round(y1 /escala,2), x, minimo, 4 * (rotulacion / 100), 270 * Math.PI / 180, 7, "Rotulacion-pk");
                pk += 20;
            }
            return pk;
        }
        public void Dibujar_PK_Acuerdo_Final(Parabola p, double pk, double escala, double min)
        {
            double a = p.parabola[0];
            double b = p.parabola[1];
            double c = p.parabola[2];
            double y1 = 0, y2 = 0;
            double pk2 = 0;
            int minimo = (int)Math.Truncate(min) * (int)escala - 6 * (int)escala;
            pk2 = pk - 0.1;
            y1 = (pk * pk) * a + pk * b + c;
            y2 = (pk2 * pk2) * a + pk2 * b + c;
            y1 = y1 * escala;
            y2 = y2 * escala;
            double az = Rellenar_centro(pk - 0.1, y2, pk, y1, 1).Az;
            az = 360 - az;
            double x = pk + 4 * (rotulacion / 100) * Math.Cos(270 * Math.PI / 180);
            double y = minimo + 4 * (rotulacion / 100) * Math.Sin(270 * Math.PI / 180);

            oTexto.addText2D("Cota: " + Math.Round(y1 / escala, 2), x, minimo, 4 * (rotulacion / 100), 270 * Math.PI / 180, 7, "Rotulacion-pk");
        }
        public void Guitarra(double min,double max,double dist,double escala)
        {
            int minimo = (int)Math.Truncate(min)*(int)escala- 5 * (int)escala;
            int maximo = (int)Math.Truncate(max)*(int)escala+ 5 * (int)escala;
            double distancia= Math.Truncate(dist)+20;
            distancia = Math.Truncate(distancia / 10) + 20;
            distancia = distancia * 10;
            double x = 0;
            double y = minimo - 20;
            bool primero = true;
            while (minimo<maximo+25)
            {
                if (primero)
                {
                    Dibujar_r(new Point2d(x, minimo), new Point2d(x + distancia, minimo), 2, "Guitarra horizontal");
                    oTexto.addText2D(getStringPK(minimo/escala), x - 4, minimo, 1, 0 * Math.PI / 180, 7, "Rotulacion-Cota");
                    minimo += 20;
                    primero = false;
                }
                else
                {
                    Dibujar_r(new Point2d(x, minimo), new Point2d(x + distancia, minimo), 8, "Guitarra horizontal intermedia");
                    oTexto.addText2D(getStringPK(minimo/escala), x - 4, minimo, 1, 0 * Math.PI / 180, 7, "Rotulacion-Cota");
                    minimo += 20;
                }
                
            }
            Dibujar_r(new Point2d(x, minimo), new Point2d(x + distancia, minimo), 2, "Guitarra horizontal");
            oTexto.addText2D(getStringPK(minimo/escala), x - 4, minimo, 1, 0 * Math.PI / 180, 7, "Rotulacion-Cota");
            maximo = minimo;
            minimo = (int)Math.Truncate(min) * (int)escala-5 * (int)escala;
            
            primero = true;
            while (x < distancia )
            {
                if (primero)
                {
                    Dibujar_r(new Point2d(x, minimo), new Point2d(x, maximo), 3, "Guitarra vertical");
                    oTexto.addText2D(getStringPK(x), x-1, minimo-1, 1, 0 * Math.PI / 180, 7, "Rotulacion-Cota");
                    x += 20;
                    primero = false;
                }
                else
                {
                    Dibujar_r(new Point2d(x, minimo), new Point2d(x, maximo), 8, "Guitarra vertical intermedia");
                    oTexto.addText2D(getStringPK(x), x-1, minimo - 1, 1, 0 * Math.PI / 180, 7, "Rotulacion-Cota");
                    x += 20;
                }
                
            }
            Dibujar_r(new Point2d(distancia, minimo), new Point2d(distancia, maximo), 3, "Guitarra vertical");
            oTexto.addText2D(getStringPK(x), x-1, minimo - 1, 1, 0 * Math.PI / 180, 7, "Rotulacion-Cota");

        }
        public int Dibujar_PK_Pendiente(Pendiente p,int pk,double escala,double min)
        {
            
            double az = Rellenar_centro(p.Puntos[0].X, p.Puntos[0].Y*escala, p.Puntos[1].X, p.Puntos[1].Y*escala, 1).Az;
            az = 360 - az;
            double a_x0 = p.Puntos[0].X;
            double a_y0 = p.Puntos[0].Y*escala;
            double b_x1 = p.Puntos[1].X;
            double b_y1 = p.Puntos[1].Y*escala;

            double a_1 = (a_y0 - b_y1) / (a_x0 - b_x1);
            double b_1 = -b_x1 * (a_y0 - b_y1) / (a_x0 - b_x1) + b_y1;
            double pk_y = 0;
            int minimo = (int)Math.Truncate(min) * (int)escala - 6 * (int)escala;
            while (pk< p.Puntos[1].X)
            {
                pk_y = a_1 * pk + b_1;
                double x = pk + 4 * (rotulacion / 100) * Math.Cos(270 * Math.PI / 180);
                double y = minimo + 4 * (rotulacion / 100) * Math.Sin(270 * Math.PI / 180);
                //Dibujar_r(new Point2d(pk, minimo), new Point2d(x, y), 7, "Linea_Rotulacion_Pendiente");
                oTexto.addText2D("Cota: " + Math.Round(pk_y /escala,2), x, minimo, 4 * (rotulacion / 100), 270 * Math.PI / 180, 7, "Rotulacion-pk");
                pk += 20;
            }
            

            return pk;
        }
        public void Dibujar_PK_Pendiente_Final(Pendiente p, int pk, double escala, double min)
        {

            double az = Rellenar_centro(p.Puntos[0].X, p.Puntos[0].Y * escala, p.Puntos[1].X, p.Puntos[1].Y * escala, 1).Az;
            az = 360 - az;
            double a_x0 = p.Puntos[0].X;
            double a_y0 = p.Puntos[0].Y * escala;
            double b_x1 = p.Puntos[1].X;
            double b_y1 = p.Puntos[1].Y * escala;

            double a_1 = (a_y0 - b_y1) / (a_x0 - b_x1);
            double b_1 = -b_x1 * (a_y0 - b_y1) / (a_x0 - b_x1) + b_y1;
            double pk_y = 0;
            int minimo = (int)Math.Truncate(min) * (int)escala - 6 * (int)escala;
            pk_y = a_1 * b_x1 + b_1;
            double x = b_x1 + 4 * (rotulacion / 100) * Math.Cos(270 * Math.PI / 180);
            double y = minimo + 4 * (rotulacion / 100) * Math.Sin(270 * Math.PI / 180);
            //Dibujar_r(new Point2d(pk, minimo), new Point2d(x, y), 7, "Linea_Rotulacion_Pendiente");
            oTexto.addText2D("Cota: " + Math.Round(pk_y / escala, 2), x, minimo, 4 * (rotulacion / 100), 270 * Math.PI / 180, 7, "Rotulacion-pk");
            b_x1 += 20;
        }
        public void Cota(double x,double y,Parabola p,double escala)
        {
            double a = p.parabola[0];
            double b = p.parabola[1];
            double c = p.parabola[2];
            double yy = (x * x) * a + x * b + c;
            yy = yy*escala;
            double x2 = x - 0.1;
            double yy1 = (x2 * x2) * a + x2 * b + c;
            yy1= yy1* escala;
            double az = Rellenar_centro(x-0.1, yy1, x, yy, 1).Az;
            az = 360 - az;
            double x3 = x + 6 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
            double y3 = yy + 6 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);

            Dibujar_r(new Point2d(x, yy), new Point2d(x3, y3), 7, "Linea_Cota");
            x3 = x + 4 * (rotulacion / 100) * Math.Cos(az * Math.PI / 180);
            y3 = yy + 4 * (rotulacion / 100) * Math.Sin(az * Math.PI / 180);
            Dibujar_r(new Point2d(x, yy), new Point2d(x3, y3), 7, "Linea_Rotulacion_Pendiente");
            oTexto.addText2D("Pk: " + getStringPK(x), x3, y3, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-Cota");

            double miDir;
            if (az + 90 > 360)
            {
                miDir = az + 90 - 360;
            }
            else
            {
                miDir = az + 90;
            }
            x = x + 5 * (rotulacion / 100) * Math.Cos(miDir * Math.PI / 180);
            y = yy + 5 * (rotulacion / 100) * Math.Sin(miDir * Math.PI / 180);

            oTexto.addText2D("  Cota: " + Math.Round(yy/escala, 2), x, y, 4 * (rotulacion / 100), az * Math.PI / 180, 7, "Rotulacion-Cota");

        }
    }
}
