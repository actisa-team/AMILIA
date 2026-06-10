using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Interop.Common;
using Autodesk.AutoCAD.Windows;

namespace Logica
{
    public class Punto
    {
        public Point2d p { get; set; }
        
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }
        public double s { get; set; }
        public double R { get; set; }
        public double ap { get; set; }
        public double bp { get; set; }
        public double cp { get; set; }
        public double sp { get; set; }
        public double Rp { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
        public double Ad1 { get; set; }
        public double Ad2 { get; set; }
        public double signod { get; set; }//1-->negativo//2-->positivo
        public double signodx { get; set; }//1-->negativo//2-->positivo
        public double signody { get; set; }//-->negativo//2-->positivo
        public double Dc { get; set; }//1-->No//2-->Si
        public string Orientacion { get; set; }
        public double Azcardinal { get; set; }
        public double cuadrante { get; set; }
        public double Az { get; set; }
        public double CambioAz { get; set; }
        public double Tipogiro { get; set; }//1-->horario//2-->antihorario//3-->recta
        public double secuenciagiro { get; set; }//1-->cambio de sentido//2-->mismo sentido
        public int grupo { get; set; }
        public int Recta { get; set; }
        public int Curva { get; set; }
        public bool minimo { get; set; }
        public bool minimop { get; set; }
        
        public double xc { get; set; }
        public double yc { get; set; }
        public double xc_p { get; set; }
        public double yc_p { get; set; }
        public double dis_raux { get; set; }
        public List<double> distancia_c_p = new List<double>();
        public List<double> distancia_c_p_p = new List<double>();

        public List<double> diferencia_r = new List<double>();
        public List<double> diferencia_r_p = new List<double>();

        public List<int> Min_diferencia = new List<int>();
        public List<int> Min_diferencia_p = new List<int>();
        public int seccion_giro { get; set; }
        public bool cambio_giro_modificado { get; set; }
        public bool p_perfil {get;set;}//solo lo utilizamos para guardar la polilinea
        public double pp_perfil { get; set; }
        public double z { get; set; }
    public Punto()
        {

        }
        public Punto(Point2d p)
        {
            this.p = p;
        }
        
        public Punto(Punto p)
        {
            this.p = p.p;
            this.a = p.a;
            this.b = p.b;
            this.c = p.c;
            this.s = p.s;
            this.R = p.R;
            this.ap = p.ap;
            this.bp = p.bp;
            this.cp = p.cp;
            this.sp = p.sp;
            this.Rp = p.Rp;
            this.Dx = p.Dx;
            this.Dy = p.Dy;
            this.Ad1 = p.Ad1;
            this.Ad2 = p.Ad2;
            this.signod = p.signod;
            this.signodx = p.signodx;
            this.signody = p.signody;
            this.Dc = p.Dc;
            this.Orientacion = p.Orientacion;
            this.Azcardinal = p.Azcardinal;
            this.cuadrante = p.cuadrante;
            this.Az = p.Az;
            this.CambioAz = p.CambioAz;
            this.Tipogiro = p.Tipogiro;
            this.secuenciagiro = p.secuenciagiro;
            this.grupo = p.grupo;
            this.Recta = p.Recta;
            this.Curva = p.Curva;
            this.minimo = p.minimo;
            this.minimop = p.minimop;
    }
        public void Cambiar_X_Y(double x,double y)
        {
            this.p = new Point2d(x,y);

        }
        public void add_distancia_c_p(double d)
        {
            distancia_c_p.Add(d);
        }
        public void add_distancia_c_p_p(double d)
        {
            distancia_c_p_p.Add(d);
        }
        public void add_diferencia_r(double d)
        {
            diferencia_r.Add(d);
        }
        public void add_diferencia_r_p(double d)
        {
            diferencia_r_p.Add(d);
        }
        public void add_Min_diferencia(int d)
        {
            Min_diferencia.Add(d);
        }
        public void add_Min_diferencia_p(int d)
        {
            Min_diferencia_p.Add(d);
        }
        public void Vaciar()
        {
            this.a = 0;
            this.b = 0;
            this.c = 0;
            this.s = 0;
            this.R = 0;
            this.ap =0;
            this.bp = 0;
            this.cp = 0;
            this.sp = 0;
            this.Rp = 0;
            this.Dx = 0;
            this.Dy = 0;
            this.Ad1 = 0;
            this.Ad2 = 0;
            this.signod = 0;
            this.signodx = 0;
            this.signody = 0;
            this.Dc = 0;
            this.Orientacion = "";
            this.Azcardinal = 0;
            this.cuadrante = 0;
            this.Az = 0;
            this.CambioAz = 0;
            this.Tipogiro = 0;
            this.secuenciagiro = 0;
            this.grupo = 0;
            this.Recta = 0;
            this.Curva = 0;
            this.minimo = false;
            this.minimop = false;
        }
    }
    

}
