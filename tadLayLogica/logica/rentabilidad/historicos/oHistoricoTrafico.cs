using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{

    using engNet.Extension.Double;
    using engNet;
    using engNet.ClassT;

    public class oHistoricoTrafico
    {
        #region "FieldsPrivados
        
        private int? mImd=null;
        private double? mTasaCrecimientoTraficoPU=null;
        private double? mAbsorcionTraficoYearIniPU=null;
        private double? mAbsorcionTraficoYearFinPU=null;
        private int? mYearsExplotacion=null;
        /// <summary>
        /// Key --> Year
        /// Value --> Trafico
        /// </summary>
        private Dictionary<int, int> mLstTrafico= null;
        #endregion
        #region "Constructor"

        public oHistoricoTrafico(int iImd,
                          double iCrecimientoAnualPC,
                          double iAbsorcionTraficoYearIniConSignoPC,
                          double iAbsorcionTraficoYearFinConSignoPC,
                          int iYearExplotacion)
        {


            mImd = iImd;
            mTasaCrecimientoTraficoPU = iCrecimientoAnualPC / 100;
            mAbsorcionTraficoYearIniPU = iAbsorcionTraficoYearIniConSignoPC / 100;
            mAbsorcionTraficoYearFinPU = iAbsorcionTraficoYearFinConSignoPC / 100;
            mYearsExplotacion = iYearExplotacion;
        }

        #endregion
        #region "Propiedades"

        /// <summary>
        /// Trafico Total Anual
        /// </summary>
        public Dictionary<int, int> lstTrafico
        {
            get
            {
                if (mLstTrafico == null)
                {
                    mLstTrafico = getLstTrafico();
                }

                return mLstTrafico;
            }
        }




        public double absorcionTraficoAnualPU
        {
            get
            {
                return (mAbsorcionTraficoYearFinPU.Value - mAbsorcionTraficoYearIniPU.Value) / (mYearsExplotacion.Value - 1);
            }
        }

        #endregion
        #region "Metodos"

        public int traficoByYear(int iYearBaseUno)
        {
            return lstTrafico[iYearBaseUno];
        }


        public void print(string iFilePath, string iFileName)
        {

            List<oValDesT<int, int>> miLst = new List<oValDesT<int, int>>();

            foreach (var item in lstTrafico)
            {
                miLst.Add(new oValDesT<int, int>(item.Key, item.Value));
            }


            engNet.oCsvWrite.write<oValDesT<int, int>>(miLst, iFilePath, iFileName);

        }


        #endregion
        #region "Metodos Privados"
        private Dictionary<int, int> getLstTrafico()
        {

            Dictionary<int, int> miTrafico = new Dictionary<int, int>();



            int miNumVehiculoTotalYearN;
            double miCoefAbsorcionYearN;
            int miVehiculosNum;


            miTrafico = new Dictionary<int, int>();

            for (int i = 1; i <= mYearsExplotacion; i++)
            {
                miNumVehiculoTotalYearN = this.getTraficoAnualByYear(i);
                miCoefAbsorcionYearN = mAbsorcionTraficoYearIniPU.Value + ((i - 1) * absorcionTraficoAnualPU);
                miVehiculosNum = Convert.ToInt32(miNumVehiculoTotalYearN * miCoefAbsorcionYearN);
                miTrafico.Add(i, miVehiculosNum);
            }

            return miTrafico;

        }
        private int getTraficoAnualByYear(double iYear)
        {


            double miTasaCrecimiento = this.mImd.Value * this.mTasaCrecimientoTraficoPU.Value * (iYear - 1);

            double miImdByYear = this.mImd.Value + miTasaCrecimiento;

            double miImdByYearRedondeoUp = Math.Ceiling(miImdByYear);


            return Convert.ToInt32(365 * miImdByYearRedondeoUp);

        }
        #endregion

    }
        
  
}
