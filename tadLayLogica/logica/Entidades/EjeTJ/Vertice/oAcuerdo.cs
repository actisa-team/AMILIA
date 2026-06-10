using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ.Vertice
{

    using tadLayShare;
    using tadLayShare.puntos;
    

    public class oAcuerdo : oVer
    {


        /// <summary>
        /// Coordenada Z es Constante // Casos ; Tramo Salida Vertice Final // Tramo LLegada Vertice Final -1
        /// </summary>
        public bool? IsZKon = false;


        private double? mZmaxAjuste = null;
        private double? mZminAjuste = null;


       
      
       
        public static Func<int, eAcuertoTipo> mFunGetAcuerdo;
        public static Func<int, double?> mFunGetIncPend;
        public static Func<int, double?> mFunGetKvVisibilidad;
        public static Func<int, double?> mFunGetKvSolape;
        public static Func<int, double?> mFunGetKvDesign;
        public static Func<int, oViabilidad> mFunGetViabilidad;
        public static Func<double, double> mFunGetZTerreno;

        public oAcuerdo()
        { 
        
        
        }




        

        public oAcuerdo(int iId, oP3d iPosition)
            : base(iId, iPosition)
        {
            

        }


   


         

        public override string ToString()
        {
            return "id: " + this.id.ToString() + " - " +
                   "Pk: " + this.position.X.ToString() + " - " +
                   "Zrasante: " + this.position.Z.ToString() + " - " +
                   "Zterreno: " + this.Zterreno.ToString() + " - " +
                   "Kv: " + this.KvDesign.ToString() + " - " +
                   "Viabilidad : " +this.viabilidad.ToString();
          
        }


        #region "Propiedades"

        /// <summary>
        /// Inc Z max por Ajuste
        /// </summary>
        public double incZmaxAjuste
        {

            get
            {
                return ZmaxAjuste - position.Z;
            }
        }
        /// <summary>
        /// Valor Máximo del Acuerdo para el Ajuste del Perfil Longitudinal
        /// </summary>
        public double ZmaxAjuste
        {

            get
            {
                if (mZmaxAjuste.HasValue)
                {
                    return mZmaxAjuste.Value;
                }
                else
                {
                    throw new Exception("El Valor del Z máximo por Ajuste Longitudinal es Nulo.");              
                }            
            }

            set
            {
                mZmaxAjuste = value;
            }
        
        }
        /// <summary>
        /// Valor Minimo del Acuerdo para el Ajuste del Perfil Longitudinal
        /// </summary>
        public double ZminAjuste
        {

            get
            {
                if (mZminAjuste.HasValue)
                {
                    return mZminAjuste.Value;
                }
                else
                {
                    throw new Exception("El Valor del Z mínimo por Ajuste Longitudinal es Nulo.");
                }
            }

            set
            {
                mZminAjuste = value;
            }

        }
        /// <summary>
        /// Obtener Z Terreno Función del Pk (X)
        /// </summary>
        public double? Zterreno
        {

            get
            {
                return mFunGetZTerreno(this.position.X);
            
            }
        
        }
        /// <summary>
        /// Tipo de Acuerdo Plano;Convexo;Concavo
        /// </summary>
        public eAcuertoTipo acuerdoTipo
        {
            get
            {
                return mFunGetAcuerdo(id);
            }
        }
        /// <summary>
        /// Incremento Pendiente Respecto Tramo Anterior y Posterior
        /// </summary>
        public double? incPend
        {
            get
            {
                return mFunGetIncPend(id);
            }
        }
        /// <summary>
        /// Kv Visibilidad
        /// </summary>
        public double? KvVisibilidad
        {
            get
            {
                return mFunGetKvVisibilidad(id);
            }
        }
        /// <summary>
        /// Kv Solape
        /// </summary>
        public double? KvSolape
        {
            get
            {
                return mFunGetKvSolape(id);
            }
        }
        /// <summary>
        /// Kv de Diseño
        /// </summary>
        public double? KvDesign
        {
            get
            {

                return mFunGetKvDesign(id);

            }

        }
        /// <summary>
        /// Viabilidad del Vertice
        /// </summary>
        public oViabilidad viabilidad
        {

            get
            {
                return mFunGetViabilidad(id);

            }

        }



        #endregion

    }




    public class oViabilidad
    {

        public double? B1 = null;
        public double? B2 = null;
        public double? C1 = null;
        public double? C2 = null;
        public double? D1 = null;
        public double? D2 = null;

        /// <summary>
        /// MAXIMO VALOR RASANTE
        /// </summary>
        public double E1MaxValorRasante
        {

            get
            {

                List<double> myList = new List<double>();

                myList.Add(B1.Value);
                myList.Add(C1.Value);
                myList.Add(D1.Value);

                return getMinValue(myList);

            }

        }

        /// <summary>
        /// MINIMO VALOR RASANTE
        /// </summary>
        public double E2MinValorRasante
        {

            get
            {
                List<double> myList = new List<double>();

                myList.Add(B2.Value);
                myList.Add(C2.Value);
                myList.Add(D2.Value);

                return getMaxValue(myList);


            }


        }

        private double getMinValue(List<double> iLstDouble)
        {
            return iLstDouble.Min();
        }


        private double getMaxValue(List<double> iLstDouble)
        {
            return iLstDouble.Max();
        }


        public bool isViable
        {
            get
            {

                if (E1MaxValorRasante > E2MinValorRasante)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public override string ToString()
        {
            return "B1: " + B1.ToString() + " ; " +
                   "B2: " + B2.ToString() + " ; " +
                   "C1: " + C1.ToString() + " ; " +
                   "C2: " + C2.ToString() + " ; " +
                   "D1: " + D1.ToString() + " ; " +
                   "D2: " + D2.ToString() + " ; " +
                   "E1 Max Ras.: " + E1MaxValorRasante.ToString() + " ; " +
                   "E2 Min Ras.: " + E2MinValorRasante.ToString() + " ; " +
                   "Viable: " + isViable.ToString();
        }
    }


}
