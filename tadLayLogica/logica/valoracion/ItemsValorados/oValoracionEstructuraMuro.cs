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
	
	
	
	public class oValoracionEstructuraMuro
	{

	   private dsApp.tbSolucionRow mSolRow = null;
	   private List<oMedItemModel> mLstMediciones;

	   private double? mMuroM3 = null;


		#region "Constructores

		public oValoracionEstructuraMuro (Guid iIdSol)
		{
			mSolRow = oSingletonDsApp.getInstance.getSolucion(iIdSol);
			mLstMediciones = tadLayLogica.datos.proyecto.oDalAppMedicones.getMedicionesCadByGrupo(mSolRow.id, eNodo.MURO);
		}

		public oValoracionEstructuraMuro(dsApp.tbSolucionRow iSolucionRow)
		{
			mSolRow = iSolucionRow;
			mLstMediciones = tadLayLogica.datos.proyecto.oDalAppMedicones.getMedicionesCadByGrupo(mSolRow.id, eNodo.MURO);    
		}

		#endregion






		#region "Propiedades"

		public double muroHormigonM3
		{
			get
			{
				if (mMuroM3 == null)
				{
					mMuroM3 = (from p in mLstMediciones
							  where p.code == eNodo.MURO
							  select p).Sum(row => row.medicion).roundOffOne();
				}

				return mMuroM3.Value;
			}

		}


		#endregion

		#region "Inferface"
		public IValoracion valoracion
		{
			get { return new oComponetValEstructurasMuro(muroHormigonM3);}
		}
		#endregion


		public void writeCSV (string iPath, string iName)
		{

			List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
			List<oValDesT<string, double?>> miLstFooter = new List<oValDesT<string, double?>>();

			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiinformeMedMuro, string.Empty));

			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
			miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiNombreSol, mSolRow.nombre));


			miLstFooter.Add(new oValDesT<string, double?>("", null));
			miLstFooter.Add(new oValDesT<string, double?>("", null));
			miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiMuroHormigon, muroHormigonM3));
			
			tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
														 oMedItemModel,
														 oValDesT<string, double?>>(miLstHeader, mLstMediciones, miLstFooter, iPath, iName);


		}





	}
}
