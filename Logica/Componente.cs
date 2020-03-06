using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Componente
    {
        public List<Punto> lista_puntos = new List<Punto>();
        public List<Punto> lista_puntos_i = new List<Punto>();
        public int index { get; set; }//identificador
        public int Tipo { get; set; }//recta==1  // curva==2  //clotoide==3
        public double xc { get; set; }
        public double yc { get; set; }
        public double xc_i { get; set; }
        public double yc_i { get; set; }
        //para clotide
        public double azte { get; set; }
        public double azts { get; set; }
        public double azcurva { get; set; }
        public double azmax { get; set; }
        public double azr { get; set; }
        public double radio { get; set; }
        public double radio_i { get; set; }
        public int ini { get; set; }
        public int fin { get; set; }
        public bool cluster { get; set; }
        public bool bloqueo { get; set; }
        public double dif_az {get;set;}
        public double dif_az_i { get; set; }
        public bool curva_creada { get; set; }
        public bool curva_creada2 { get; set; }
        public bool Clusterizada { get; set; }
        /*
         * Viabilidad
         */
        public bool V_pr { get; set; }
        public bool V_ur { get; set; }
        /*
         * Viabilidad rectas intermedias
         */
        public bool pm_ca { get; set; }//El punto medio del segmento de recta queda dentro de la curva anterior
        public bool pm_cp { get; set; }//El punto medio del segmento de recta queda dentro de la curva posterior
        public bool ca_cp { get; set; }//Las curvas anterior y posterior a la recta se cortan
        public bool c_a { get; set; }//Se corta con la anterior
        public bool c_p { get; set; }//Se corta con la posterior
        public bool s_s { get; set; }//supersolape
        public int c_g_a_p { get; set; }//Cambio de giro en curvas anterior y posterior a la recta////1-->cambio de sentido//2-->mismo sentido
        /*
         * Casos
         */
        public bool caso1 { get; set; }//caso1
        public bool caso2 { get; set; }//caso2
        public bool caso3 { get; set; }//caso3
        public bool caso4 { get; set; }//caso4
        public bool caso5 { get; set; }//caso5
        public bool caso6 { get; set; }//caso6
        public int prioridad { get; set; }//prioridad
        public int giros { get; set; }//para ver que tipo de giro hizo anteriormente 1-->giro positivo 2-->giro negativo 0--> no ha girado
        /*
         * 
         * esto nos sirve para saver si ha sido creado posteriormente para segun que casos
         * caso1: Se ha eliminado una recta que no se podia girar a otra que es perpendicular 
         * al centro de la recta dada entre entre 2 circunferencias
         * 
         * caso2: se crea la recta tangente exterior a 2 circunferencias
         * 
         */
        public int creacion { get; set; }
        public double L_Arco { get; set; }
        public double L_Arco_i { get; set; }
        public int solape { get; set; }
        /*
         * Casos de enlaces
         */
        public bool v_p_c { get; set; }//La clotoide se solapa con la siguiente clotoide o bien la clotoide se inicia antes del punto inicial de la polilínea
        public bool v_u_c { get; set; }//La clotoide se solapa con la anterior clotoide o bien la clotoide finaliza después del punto final de la polilínea
        public bool v_p_r { get; set; }//La clotoide se solapa con la siguiente clotoide o bien la clotoide se inicia antes del punto inicial de la polilínea
        public bool v_u_r { get; set; }//La clotoide se solapa con la anterior clotoide o bien la clotoide finaliza después del punto final de la polilínea// se utiliza para el caso 7
        public bool v_s_c { get; set; }//Solape de clotoides en la curva intermedia
        public bool v_a_g { get; set; }//Ángulo de giro en la curva menor de 2º (a1-a2)<2º
        public bool v_m_g { get; set; }//No existe margen de giro de las rectas para evitar solape
        public bool v_c_gr { get; set; }//La curva de gran radio solapa con clotoides anterior o posterior
        public bool v_a_p { get; set; }//La clotoide de enlace de dos curvas solapa con la clotoide anterior o posterior
        public double reduccion_r { get; set; }//Reducción de radio
        public bool caso0_e { get; set; }
        public bool caso1_e { get; set; }
        public bool caso2_e { get; set; }
        public bool caso3_e { get; set; }
        public bool caso4_e { get; set; }
        public bool caso5_e { get; set; }
        public bool caso6_e { get; set; }
        public bool caso7_e { get; set; }
        
        
        public EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva direccion { get; set; }
        public Componente()
        {

        }
        public Componente(Punto p,int t)
        {
            this.lista_puntos.Add(p);
            Tipo = t;
        }
        public void add(Punto p)
        {
            lista_puntos.Add(p);
        }
        public void add_i(Punto p)
        {
            lista_puntos_i.Add(p);
        }
        public void Reiniciar_casos_solapes()
        {
            v_p_c = false;
            v_u_c = false;
            v_p_r = false;
            v_u_r = false;
            v_s_c = false;
            v_a_g = false;
            v_m_g = false;
            v_c_gr = false;
            v_a_p = false;
            reduccion_r = 0;
            caso0_e = false;
            caso1_e = false;
            caso2_e = false;
            caso3_e = false;
            caso4_e = false;
            caso5_e = false;
            caso6_e = false;
            caso7_e = false;
        }

    }
}
