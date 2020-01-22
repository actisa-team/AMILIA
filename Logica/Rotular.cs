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
    }
}
