using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
   public class oHistoricoAccidentes
	{



	   private Dictionary<int, oCosteAccidenteAnual> mLstAccidentesHistory= new Dictionary<int,oCosteAccidenteAnual>();

	   public oHistoricoAccidentes(double iLon, 
								    double iIndiceMortalidad, 
								    double iIndicePeligrosidad,
								    double iKIndicePeligrosidad,
								    double iCosteMuerto,
								    double iCosteHerido,
								    int iYearsHistory,
								    Func<int,int> iTraficoHistorico)
	   {

		   for (int i = 1; i <= iYearsHistory; i++)
		   {
			   mLstAccidentesHistory.Add(i,new oCosteAccidenteAnual(i,
																	iLon,
																	iTraficoHistorico(i),
																	iIndiceMortalidad,
																	iIndicePeligrosidad,
																	iKIndicePeligrosidad,
																	iCosteMuerto,
																	iCosteHerido));

		   }

		  

	   }


	   public Dictionary<int, oCosteAccidenteAnual> lstAccidentesHistory
	   {
		   get 
		   {
			   return mLstAccidentesHistory;   
		   }
		  
	   }







	}
   public class oCosteAccidenteAnual
	{
	
			public int  year { get; private set; }

			public int numMuertosAnual { get; private set; }
			public int numHeridosAnual { get; private set; }


			public double costeMuertosAnual { get; private set; }
			public double costeHeridosAnual { get; private set; }
			public double costeTotalAnual { get {return costeHeridosAnual+costeMuertosAnual;}}
		
			public oCosteAccidenteAnual (int iYearBaseUno,
										 double iLonKm, 
										 int iTraficoTotal,
										 double iIndiceMortalidad, 
										 double iIndicePeligrosidad,
										 double iKindidePeligrosidad,
										 double iCosteMuerto,
										 double iCosteHerido)
			{
			   year = iYearBaseUno;
			   numMuertosAnual = getNumeroMuertosAnual(iLonKm, iTraficoTotal, iIndiceMortalidad);
			   numHeridosAnual = getNumeroHeridosAnual(iKindidePeligrosidad,iLonKm, iTraficoTotal, iIndicePeligrosidad);
			   costeMuertosAnual = numMuertosAnual * iCosteMuerto;
			   costeHeridosAnual = numHeridosAnual * iCosteHerido;
			}



			 private static int getNumeroMuertosAnual(double iLonKm, double iTraficoTotal, double iIndiceMortalidad)
			 {
				 return Convert.ToInt32((iTraficoTotal * iLonKm * iIndiceMortalidad * 1.00e-8));
			 }

			 private static int getNumeroHeridosAnual(double iK,  double iLonKm, double iTraficoTotal, double iIndicePeligrosidad)
			 {
				 return Convert.ToInt32(iK*iTraficoTotal * iLonKm * iIndicePeligrosidad * 1.00e-8);
			 }

	
	
	
	
	
	}
}
