using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
    using engNet.ClassT;


    using tadLayLogica.logica.presupuesto;
    
    public class oHistoricoConstruccion
    {

        private int? mYearsExplotacion = null;
        private double? mCosteConstruccionAnual = null;

   

        /// <summary>
        /// Key --> Year
        /// Value -->Coste Construccion
        /// </summary>
        private Dictionary<int, double> mLstCostesConstruccion = null;


        public oHistoricoConstruccion(double iCostesConstruccionTotal , int iYearsExplotacion)
        {
            mYearsExplotacion = iYearsExplotacion;
            mCosteConstruccionAnual = iCostesConstruccionTotal / mYearsExplotacion.Value;
        }

        #region "Propiedades

        public Dictionary<int, double> lstCostes
        {
            get
            {
                if (mLstCostesConstruccion == null)
                {
                    mLstCostesConstruccion = getLstCostesConstruccion();
                }

                return mLstCostesConstruccion;
            }
        }

        #endregion


        #region "Metodos"

        /// <summary>
        /// Imprimir el Historico de IPC
        /// </summary>
        public void print(string iFilePath, string iFileName)
        {

            List<oValDesT<int, double>> miLst = new List<oValDesT<int, double>>();

            foreach (var item in lstCostes)
            {
                miLst.Add(new oValDesT<int, double>(item.Key, item.Value));
            }


            engNet.oCsvWrite.write<oValDesT<int, double>>(miLst, iFilePath, iFileName);

        }



        #endregion





        private Dictionary<int,double> getLstCostesConstruccion ()
        {

            Dictionary<int, double> miLstCostesByYear = new Dictionary<int, double>();

            
            for (int i = 1; i <= mYearsExplotacion; i++)
            {
                miLstCostesByYear.Add(i, mCosteConstruccionAnual.Value);
            }

            return miLstCostesByYear;



        }




    }
}
