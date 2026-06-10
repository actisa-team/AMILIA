using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class  Parabola
    {
        public List<double> parabola = new List<double>();
        public List<PuntoPerfil> polilinea_perfil = new List<PuntoPerfil>();
        public Point2d puntoEntrada = new Point2d();
        public Point2d puntoSalida = new Point2d();
        public List<double> lista_parabola { get => parabola; set => parabola = value; }
        public List<PuntoPerfil> Polilinea_Perfil { get => polilinea_perfil; set => polilinea_perfil = value; }
        public double max_min { get; set; }
        public Parabola()
        {

        }
        public Parabola(List<double> p)
        {
            parabola = p;
        }
        public Parabola(List<double> p , List<PuntoPerfil> poli)
        {
            parabola = p;
            polilinea_perfil = poli;
        }
        public void Add_PuntoPerfil(PuntoPerfil p)
        {
            polilinea_perfil.Add(p);
        }
        public void Add_parabola(List<double> p)
        {
            parabola = p;
        }
        /// <summary>
        /// Calcula el parámetro Kv de la curva vertical parabólica
        /// Kv = L / θ, donde θ = |i2 - i1| (diferencia de pendientes)
        /// </summary>
        /// <param name="pendienteEntrada">Pendiente de entrada en % (puede ser positiva o negativa)</param>
        /// <param name="pendienteSalida">Pendiente de salida en % (puede ser positiva o negativa)</param>
        /// <returns>Valor de Kv en metros</returns>
        public double CalcularKv(double pendienteEntrada, double pendienteSalida,bool usarCorreccionAngular)
        {
            // Calcular longitud de la curva
            double longitud = CalcularLongitudCurva();

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
                theta = Math.Abs(angulo2_rad - angulo1_rad);

                if (theta == 0)
                    return 0;

                // Kv = L / θ (donde θ está en radianes)
                return longitud / theta;
            }
            else
            {
                // Método estándar: diferencia algebraica de pendientes (en tanto por uno)
                theta = Math.Abs(pendienteSalida - pendienteEntrada) / 100.0;

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
        public double CalcularKvDesdePuntos()
        {
            if (polilinea_perfil == null || polilinea_perfil.Count < 2)
                return 0;

            PuntoPerfil puntoAnterior = polilinea_perfil.First(); 
            PuntoPerfil puntoSiguiente=polilinea_perfil.Last();
            PuntoPerfil primerPunto = polilinea_perfil[1];
            PuntoPerfil ultimoPunto = polilinea_perfil[polilinea_perfil.Count()-2];

            // Calcular pendiente de entrada (desde punto anterior hasta inicio de parábola)
            double deltaX_entrada = primerPunto.p.X - puntoAnterior.p.X;
            double deltaY_entrada = primerPunto.p.Y - puntoAnterior.p.Y;
            double pendienteEntrada = 0;

            if (deltaX_entrada != 0)
                pendienteEntrada = (deltaY_entrada / deltaX_entrada) * 100; // En porcentaje

            // Calcular pendiente de salida (desde fin de parábola hasta punto siguiente)
            double deltaX_salida = puntoSiguiente.p.X - ultimoPunto.p.X;
            double deltaY_salida = puntoSiguiente.p.Y - ultimoPunto.p.Y;
            double pendienteSalida = 0;

            if (deltaX_salida != 0)
                pendienteSalida = (deltaY_salida / deltaX_salida) * 100; // En porcentaje

            double kv1= CalcularKv(pendienteEntrada, pendienteSalida,true);
            double kv2 = CalcularKv(pendienteEntrada, pendienteSalida,false);
            return kv1;
        }

        /// <summary>
        /// Calcula el Kv usando las propiedades 'pendiente' de los PuntoPerfil
        /// </summary>
        /// <returns>Valor de Kv en metros</returns>
        public double CalcularKvDesdePropiedadesPuntos()
        {
            if (polilinea_perfil == null || polilinea_perfil.Count < 2)
                return 0;

            PuntoPerfil primerPunto = polilinea_perfil.First();
            PuntoPerfil ultimoPunto = polilinea_perfil.Last();

            // Usar las propiedades 'pendiente' de los puntos
            double pendienteEntrada = primerPunto.pendiente * 100; // Convertir a %
            double pendienteSalida = ultimoPunto.pendiente * 100;   // Convertir a %

            double kv1 = CalcularKv(pendienteEntrada, pendienteSalida, true);
            double kv2 = CalcularKv(pendienteEntrada, pendienteSalida, false);
            return kv1;
        }

        /// <summary>
        /// Calcula la longitud horizontal de la curva vertical
        /// </summary>
        /// <returns>Longitud en metros</returns>
        public double CalcularLongitudCurva()
        {
            // OPCIÓN 1: Usar puntoEntrada y puntoSalida
            /*if (puntoEntrada != null && puntoSalida != null)
            {
                double dx = puntoSalida.X - puntoEntrada.X;
                return Math.Abs(dx);
            }
            */
            // OPCIÓN 2: Usar Polilinea_Perfil
            if (polilinea_perfil != null && polilinea_perfil.Count >= 2)
            {
                PuntoPerfil primero = polilinea_perfil.First();
                PuntoPerfil ultimo = polilinea_perfil.Last();
                double dx = ultimo.p.X - primero.p.X;
                return Math.Abs(dx);
            }

            return 0;
        }

        /// <summary>
        /// Obtiene el tipo de curva vertical (convexa o cóncava)
        /// </summary>
        /// <param name="pendienteEntrada">Pendiente de entrada en %</param>
        /// <param name="pendienteSalida">Pendiente de salida en %</param>
        /// <returns>"CONVEXA" si es en cresta, "CONCAVA" si es en valle</returns>
        public string ObtenerTipoCurva(double pendienteEntrada, double pendienteSalida)
        {
            // Si la pendiente disminuye (i2 < i1), es convexa (cresta)
            // Si la pendiente aumenta (i2 > i1), es cóncava (valle)
            if (pendienteSalida < pendienteEntrada)
                return "CONVEXA";
            else
                return "CONCAVA";
        }

        /// <summary>
        /// Calcula el Kv y retorna su signo según convención:
        /// Positivo para curvas convexas (cresta)
        /// Negativo para curvas cóncavas (valle)
        /// </summary>
        public double CalcularKvConSigno(double pendienteEntrada, double pendienteSalida)
        {
            double kv = CalcularKv(pendienteEntrada, pendienteSalida,true);

            // Aplicar signo según tipo de curva
            string tipo = ObtenerTipoCurva(pendienteEntrada, pendienteSalida);

            if (tipo == "CONCAVA")
                return -kv; // Negativo para cóncavas
            else
                return kv;  // Positivo para convexas
        }
    }
}
