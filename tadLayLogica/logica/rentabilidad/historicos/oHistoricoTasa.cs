using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
    using engNet.ClassT;
    
    
    public class oHistoricoTasa
    {


        private double? mTasaYearUnoPU= null;
        private double? mTasaUpdatePU = null;
        private int? mYearsBaseUno= null;

        /// <summary>
        /// Key --> Year
        /// Value -->Ipc
        /// </summary>
        private Dictionary<int, double> mLstIpc = null;


        public oHistoricoTasa (bool iUpdateTasa,  double iTasaUpdatePC, int iYearsExplotacion)
        {
            if (iUpdateTasa)
            {
                mTasaYearUnoPU = 1;
                mTasaUpdatePU = 1 + (iTasaUpdatePC / 100);
                mYearsBaseUno = iYearsExplotacion;
            }
            else
            {
                mTasaYearUnoPU = 1;
                mTasaUpdatePU = 1;
                mYearsBaseUno = iYearsExplotacion;
            }


        }


        public oHistoricoTasa(double iTasaUpdatePC, int iYears)
        {
            mTasaYearUnoPU = 1;
            mTasaUpdatePU = 1 + (iTasaUpdatePC / 100);
            mYearsBaseUno = iYears;
        }

        public oHistoricoTasa(double iTasaYearUnoPC, double iTasaUpdatePC , int iYears)
        {
            mTasaYearUnoPU = 1+(iTasaYearUnoPC/100);
            mTasaUpdatePU = 1 +(iTasaUpdatePC / 100);
            mYearsBaseUno = iYears;
        }

        #region "Propiedades

        public Dictionary<int, double> lstTasa
        {
            get
            {
                if (mLstIpc == null)
                {
                    mLstIpc = getLstIpc();
                }

                return mLstIpc;
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

            foreach (var item in lstTasa)
            {
                miLst.Add(new oValDesT<int, double>(item.Key, item.Value));
            }


            engNet.oCsvWrite.write<oValDesT<int, double>>(miLst, iFilePath, iFileName);

        }



        #endregion





        private Dictionary<int,double> getLstIpc ()
        {

            Dictionary<int, double> miLst = new Dictionary<int, double>();
            miLst.Add(1, mTasaYearUnoPU.Value);


            double? miTasa = null;

            for (int i = 2; i <= mYearsBaseUno; i++)
            {
                miTasa =  mTasaUpdatePU  * miLst[i - 1];
                miLst.Add(i, miTasa.Value);
            }

            return miLst;



        }




    }
}
