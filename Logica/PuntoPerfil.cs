using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class PuntoPerfil
    {
        public Point2d p { get; set; }
        public List<double> parabola = new List<double>();
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }
        public double s { get; set; }
        public double R { get; set; }
        public double dx { get; set; }
        public double dy { get; set; }
        public double pendiente { get; set; }
        public double pendiente_p { get; set; }
        public double v_m { get; set; }
        public double primero { get; set; }
        public double ultimo { get; set; }
        public double varianza { get; set; }
        public double varianza_a { get; set; }
        public int vertice { get; set; }//1->superior//2->inferior
        public int tipo { get; set; }//1->pendiente uniforme//2->depuracion
        public double valor { get; set; }
        public PuntoPerfil()
        {

        }
        public PuntoPerfil(Punto punto)
        {
            p = new Point2d(punto.p.X, punto.p.Y);
            a = punto.a;
            b = punto.b;
            c = punto.c;
            s = punto.s;
            R = punto.R;
            dx = punto.Dx;
            dy = punto.Dy;
        }
        public PuntoPerfil(PuntoPerfil punto)
        {
            p = new Point2d(punto.p.X, punto.p.Y);
            a = punto.a;
            b = punto.b;
            c = punto.c;
            s = punto.s;
            R = punto.R;
            dx = punto.dx;
            dy = punto.dy;
            pendiente = punto.pendiente;
            pendiente_p = punto.pendiente;
            v_m = punto.v_m;
            primero = punto.primero;
            ultimo = punto.ultimo;
            varianza = punto.varianza;
            varianza_a = punto.varianza_a;
            vertice = punto.vertice;
            tipo = punto.tipo;
        }
        /// <summary>
        /// Rellena un punto perfil con los datos de un punto
        /// </summary>
        /// <param name="punto"></param>
        public void Get_inicial(Punto punto)
        {
            p = new Point2d(punto.p.X, punto.p.Y);
            a = punto.a;
            b = punto.b;
            c = punto.c;
            s = punto.s;
            R = punto.R;
            dx = punto.Dx;
            dy = punto.Dy;
        }
        public void Set_Parabola(double p)
        {
            parabola.Add(p);
        }
    }
}
