using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{
	
	//System
	using System.ComponentModel;

	using engNet.CustomAtributos;

	using tadLayLogica.logica.medicion;

    using tadLayLogica.datos;
	using tadLayLogica.datos.proyecto;

	using engNet.ClassT;

	using engNet.Extension.Double;

	using tadLayData;
    using tadLayLan.Tdi;
	
   
	public class oValoracionTierrasVolumenMovimiento:IValoracionPropiedad
	{



	   private dsApp.tbSolucionRow mSolRow = null;
	   private List<oMedItemModel> mLstMediciones;
	   private double? mVolumenExcavacion = null;
	   private double? mVolumenTerraplenSaneo = null;
	   private double? mVolumenCapasAsiento = null;

	   private double? mNotaLocal = null;
       private double? mNotaLocalValoracionPC = null;


       public oValoracionTierrasVolumenMovimiento(Guid iIdSolucion, double iValoracionTierrasMovimientoPC)
       {
           mSolRow = oSingletonDsApp.getInstance.getSolucion(iIdSolucion);
           mNotaLocalValoracionPC = iValoracionTierrasMovimientoPC;
           mLstMediciones = tadLayLogica.datos.proyecto.oDalAppMedicones.getMedicionesCad(mSolRow.id);
       }

		public oValoracionTierrasVolumenMovimiento(dsApp.tbSolucionRow iSolRow, double iValoracionTierrasMovimientoPC)
		{
            mSolRow = iSolRow;
            mNotaLocalValoracionPC= iValoracionTierrasMovimientoPC;
			mLstMediciones = tadLayLogica.datos.proyecto.oDalAppMedicones.getMedicionesCad(mSolRow.id);
		}


		#region "Propiedades"
		public double volumenExcavacion
		{
			get
			{
				if (mVolumenExcavacion == null)
				{

					mVolumenExcavacion = (from p in mLstMediciones
										  where p.code == eNodo.EXCAVACION
										  select p).Sum(row => row.medicion).roundOffOne();

				}

				return mVolumenExcavacion.Value;
			}

		}
		public double volumenTerraplenSaneo
		{
			get
			{
				if (mVolumenTerraplenSaneo == null)
				{

					mVolumenTerraplenSaneo = (from p in mLstMediciones
											  where p.code == eNodo.TERRAPLENandSANEOS
											  select p).Sum(row => row.medicion).roundOffOne();

				}

				return mVolumenTerraplenSaneo.Value;
			}

		}
		public double volumenCapasAsiento
		{
			get
			{
				if (mVolumenCapasAsiento == null)
				{

					mVolumenCapasAsiento = (from p in mLstMediciones
											  where p.code == eNodo.CAPASASIENTO
											  select p).Sum(row => row.medicion).roundOffOne();

				}

				return mVolumenCapasAsiento.Value;
			}

		}
		public double notaLocal
		{
			get
			{
				if (mNotaLocal == null)
				{
					mNotaLocal = volumenExcavacion + volumenTerraplenSaneo + volumenCapasAsiento;
				}

				return mNotaLocal.Value;
			}

		}
		#endregion


        #region "Interface"

        public IValoracion valoracion
        {
            get { return new oComponentValTrazadoTierrasVolumenMovimiento(notaLocal,mNotaLocalValoracionPC.Value);}
        }

        #endregion


        public void writeCSV (string iPath, string iName)
		{

			List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
			List<oValDesT<string, double?>> miLstFooter = new List<oValDesT<string, double?>>();

			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
			miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInformeValTierras, string.Empty));

			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
			miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiNombreSol, mSolRow.nombre));

			miLstFooter.Add(new oValDesT<string, double?>("", null));
			miLstFooter.Add(new oValDesT<string, double?>("", null));
			miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiVolExc, volumenExcavacion));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiVolTerrSaneo, volumenTerraplenSaneo));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiVolCapasAsiento, volumenCapasAsiento));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiValPropiaM3, notaLocal));
			
			tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
														 oMedItemModel,
														 oValDesT<string, double?>>(miLstHeader, mLstMediciones, miLstFooter, iPath, iName);

		}



    }






}
