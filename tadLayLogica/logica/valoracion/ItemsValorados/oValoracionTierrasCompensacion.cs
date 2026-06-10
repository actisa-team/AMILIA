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
	
   
	public class oValoracionTierrasCompensacion:IValoracionPropiedad
	{



	   private dsApp.tbSolucionRow mSolRow = null;
	   private List<oMedItemModel> mLstMediciones;

	   private double? mVolumenExcavacionEmpleo = null;
	   private double? mVolumenExcavacionPrestamo = null;

	   private double? mNotaLocal = null;
       private double? mNotaLocalValoracionPC = null;


       public oValoracionTierrasCompensacion(Guid iIdSol, double iValoracionTierrasCompensacionPC)
       {
           mSolRow = oSingletonDsApp.getInstance.getSolucion(iIdSol);
           mNotaLocalValoracionPC = iValoracionTierrasCompensacionPC;
           mLstMediciones = tadLayLogica.datos.proyecto.oDalAppMedicones.getMedicionesCadByGrupo(mSolRow.id, eNodo.EXCAVACION);
       }

		public oValoracionTierrasCompensacion(dsApp.tbSolucionRow iSolucionRow, double iValoracionTierrasCompensacionPC)
		{
            mSolRow = iSolucionRow;
            mNotaLocalValoracionPC = iValoracionTierrasCompensacionPC;
			mLstMediciones = tadLayLogica.datos.proyecto.oDalAppMedicones.getMedicionesCadByGrupo(mSolRow.id, eNodo.EXCAVACION);    
		}




		#region "Propiedades"
		public double volumenExcavacionEmpleo
		{
			get
			{
				if (mVolumenExcavacionEmpleo == null)
				{

					mVolumenExcavacionEmpleo = (from p in mLstMediciones
												where p.code == eNodo.EXCAVACION && p.isPrecioPrincipal==true
												select p).Sum(row => row.medicion).roundOffOne();
				}

				return mVolumenExcavacionEmpleo.Value;
			}

		}
		public double volumenExcavacionPrestamo
		{
			get
			{
				if (mVolumenExcavacionPrestamo == null)
				{

					mVolumenExcavacionPrestamo = (from p in mLstMediciones
												  where p.code == eNodo.EXCAVACION && p.isPrecioPrincipal == false
												  select p).Sum(row => row.medicion).roundOffOne();

				}

				return mVolumenExcavacionPrestamo.Value;
			}

		}
		public double notaLocal
		{
			get
			{
				if (mNotaLocal == null)
				{
					mNotaLocal = 100 * (volumenExcavacionEmpleo) / (volumenExcavacionEmpleo + volumenExcavacionPrestamo);
				}

				return mNotaLocal.Value;
			}

		}
		#endregion

        #region "Inferface"
        public IValoracion valoracion
        {
            get { return new oComponentValTrazadoTierrasCompensacion(notaLocal,mNotaLocalValoracionPC.Value);}
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
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiVolExcEmpleo, volumenExcavacionEmpleo));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiVolExcPrestamo, volumenExcavacionPrestamo));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiValPropia, notaLocal));
			
			tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
														 oMedItemModel,
														 oValDesT<string, double?>>(miLstHeader, mLstMediciones, miLstFooter, iPath, iName);


		}



    }






}
