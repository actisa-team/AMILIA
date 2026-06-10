using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{

	using System.ComponentModel;
    using tadLayLan.Tdi;
    using engNet.CustomAtributos;


	public abstract class oRenCosteFunTie
	{


		protected double? mVelocidad = null;
		private double? mCombustibleConsumoLitroKm = null;
		private double? mCombustibleCosteEuroLitro = null;
		private double? mLubricanteConsumoLitroKm = null;
		private double? mLubricanteCosteEuroLitro = null;
		private double? mTiempoCosteEuroHora = null;
		private double? mCoeficientePonderacion = null;



		public oRenCosteFunTie() {}


		public oRenCosteFunTie(double iVelocidad,
							double iCombustibleConsumoCcKm,
							double iLubricanteConsumoLitrosKm,
							double iCombustibleCosteEuroLitro,
							double iLubricanteCosteEuroLitro,
							double iAmortizacionEuroKm,
							double iNeumaticoCosteEuroKm,
							double iMantenimientoEuroKm,
							double iTiempoCosteEuroHora,
							double iCoeficientePonderacion)

		{

			mVelocidad = iVelocidad;
			mCombustibleConsumoLitroKm = iCombustibleConsumoCcKm / 1000;
			mCombustibleCosteEuroLitro = iCombustibleCosteEuroLitro;
			mLubricanteConsumoLitroKm = iLubricanteConsumoLitrosKm;
			mLubricanteCosteEuroLitro = iLubricanteCosteEuroLitro;
			amortizacionEuroKm = iAmortizacionEuroKm;
			neumaticosEuroKm = iNeumaticoCosteEuroKm;
			mantenimientoEuroKm = iMantenimientoEuroKm;
			mTiempoCosteEuroHora = iTiempoCosteEuroHora;
			mCoeficientePonderacion = iCoeficientePonderacion;



		}


		#region "Propiedades"
        [LocalizedDisplayName("uiCombustible", typeof(strFrmInformes))]
		public virtual double combustibleEuroKm
		{
			get
			{
				return mCombustibleConsumoLitroKm.Value * mCombustibleCosteEuroLitro.Value;
			}
		}
        [LocalizedDisplayName("uiLublicante", typeof(strFrmInformes))]
		public virtual double lubricanteEuroKm
		{
			get
			{
				return mLubricanteConsumoLitroKm.Value * mLubricanteCosteEuroLitro.Value;
			}
		}
        [LocalizedDisplayName("uiAmotizacion", typeof(strFrmInformes))]
		public virtual double amortizacionEuroKm { get; private set; }
        [LocalizedDisplayName("uiNeumaticos", typeof(strFrmInformes))]
		public virtual double neumaticosEuroKm { get; private set; }
        [LocalizedDisplayName("uiMantenimiento", typeof(strFrmInformes))]
		public virtual double mantenimientoEuroKm { get; private set; }

        [LocalizedDisplayName("uiTotalCostFunc", typeof(strFrmInformes))]
		public virtual double costesFuncionamientoEuroKm
		{
			get
			{
				return combustibleEuroKm +
					   lubricanteEuroKm +
					   amortizacionEuroKm +
					   neumaticosEuroKm +
					   mantenimientoEuroKm;
			}
		}
        [LocalizedDisplayName("uiCostTiempo", typeof(strFrmInformes))]
		public virtual double costesTiempoEuroKm
		{
			get
			{
				return (mCoeficientePonderacion.Value * mTiempoCosteEuroHora.Value) / mVelocidad.Value;
			}
		}
        [LocalizedDisplayName("uiCostTiempoTotal", typeof(strFrmInformes))]
		public double costesTotalEuroKm
		{

			get
			{

				return costesFuncionamientoEuroKm+
					   costesTiempoEuroKm;

			}


		}
		#endregion



		public double costesFuncionamientoPorTrayecto (double iLonKm , int iTraficoTotal)
		{
			 return iLonKm * costesFuncionamientoEuroKm * iTraficoTotal;
		}

		public double costesTiempoPorTrayecto (double iLonKm, int iTraficoTotal)
		{
			 return iLonKm * costesTiempoEuroKm * iTraficoTotal;
		}

	
	}
 
	public class oRenCosteFunTieVehiculoLigero: oRenCosteFunTie
	{


	  


	  
	   public oRenCosteFunTieVehiculoLigero(double iVelocidad,
											double iCombustibleConsumoCcKm,
											double iLubricanteConsumoCcKm,
											double iCombustibleCosteEuroLitro,
											double iLubricanteCosteEuroLitro,
											double iAmortizacionEuroKm,
											double iNeumaticoCosteEuroKm,
											double iMantenimientoEuroKm,
											double iTiempoCosteEuroHora,
											double iCoeficientePonderacionCosteTiempo)
										   
		 
		   :base(iVelocidad,iCombustibleConsumoCcKm,iLubricanteConsumoCcKm,iCombustibleCosteEuroLitro,iLubricanteCosteEuroLitro,iAmortizacionEuroKm,iNeumaticoCosteEuroKm,iMantenimientoEuroKm,iTiempoCosteEuroHora,iCoeficientePonderacionCosteTiempo)
	  
	   {
		  

	   }







	}
	public class oRenCosteFunTieVehiculoPesado : oRenCosteFunTie
	{

		public oRenCosteFunTieVehiculoPesado(double iVelocidad,
											 double iCombustibleConsumoCcKm,
											 double iLubricanteConsumoCcKm,
											 double iCombustibleCosteEuroLitro,
											 double iLubricanteCosteEuroLitro,
											 double iAmortizacionEuroKm,
											 double iNeumaticoCosteEuroKm,
											 double iMantenimientoEuroKm,
											 double iTiempoCosteEuroHora,
											 double iCoeficientePonderacionCosteTiempo)
											 

			: base(iVelocidad, iCombustibleConsumoCcKm, iLubricanteConsumoCcKm, iCombustibleCosteEuroLitro, iLubricanteCosteEuroLitro, iAmortizacionEuroKm, iNeumaticoCosteEuroKm,iMantenimientoEuroKm,iTiempoCosteEuroHora, iCoeficientePonderacionCosteTiempo)
		{
		  

		}




	}
	public class oRenCosteFunTieVehiculoPonderado : oRenCosteFunTie
	{

		private oRenCosteFunTieVehiculoLigero mVehiculoLigero;
		private oRenCosteFunTieVehiculoPesado mVehiculoPesado;
		private double mPorcentajeVehiculoPesadoPU;
		private double mPorcentajeVehiculoLigeroPU;

		public oRenCosteFunTieVehiculoPonderado()
		{

		}


		public oRenCosteFunTieVehiculoPonderado(oRenCosteFunTieVehiculoLigero iVehiculoLigero, oRenCosteFunTieVehiculoPesado iVehiculoPesado, double iVehiculosPesadosPC)
		{
			mVehiculoLigero = iVehiculoLigero;
			mVehiculoPesado = iVehiculoPesado;
			mPorcentajeVehiculoPesadoPU = iVehiculosPesadosPC / 100;
			mPorcentajeVehiculoLigeroPU = 1-mPorcentajeVehiculoPesadoPU;
		}
	
		   
		public override double combustibleEuroKm
		{
			get
			{
				return (mPorcentajeVehiculoLigeroPU* mVehiculoLigero.combustibleEuroKm) +
					   (mPorcentajeVehiculoPesadoPU * mVehiculoPesado.combustibleEuroKm);
			}
		}
		public override double lubricanteEuroKm
		{
			get
			{
				return (mPorcentajeVehiculoLigeroPU* mVehiculoLigero.lubricanteEuroKm) +
					   (mPorcentajeVehiculoPesadoPU * mVehiculoPesado.lubricanteEuroKm);
			}
		}
		public override double amortizacionEuroKm
		{ 
			  get
			  {
				  return (mPorcentajeVehiculoLigeroPU* mVehiculoLigero.amortizacionEuroKm) +
						 (mPorcentajeVehiculoPesadoPU * mVehiculoPesado.amortizacionEuroKm);
			  }
		} 
		public override double neumaticosEuroKm 
		  { 
			  get
			  {
				  return (mPorcentajeVehiculoLigeroPU* mVehiculoLigero.neumaticosEuroKm) +
						 (mPorcentajeVehiculoPesadoPU * mVehiculoPesado.neumaticosEuroKm);
			  }
		}  
		public override double mantenimientoEuroKm
		{
			get 
			{
					return (mPorcentajeVehiculoLigeroPU * mVehiculoLigero.mantenimientoEuroKm) +
							(mPorcentajeVehiculoPesadoPU * mVehiculoPesado.mantenimientoEuroKm);
			}
		}


		public override double costesFuncionamientoEuroKm
		{
			get
			{
				return (mPorcentajeVehiculoLigeroPU * mVehiculoLigero.costesFuncionamientoEuroKm) +
						(mPorcentajeVehiculoPesadoPU * mVehiculoPesado.costesFuncionamientoEuroKm);
			}
		}
		public override double  costesTiempoEuroKm
		{
			get 
			{ 
					return (mPorcentajeVehiculoLigeroPU* mVehiculoLigero.costesTiempoEuroKm) +
						   (mPorcentajeVehiculoPesadoPU * mVehiculoPesado.costesTiempoEuroKm);
			}
		}
  
	}
	public class oRenCosteFunTieVehiculoPonderadoCosteCero : oRenCosteFunTieVehiculoPonderado
	{

  


		public oRenCosteFunTieVehiculoPonderadoCosteCero()
		{

		}


		public override double combustibleEuroKm
		{
			get
			{
				return 0;
			}
		}
		public override double lubricanteEuroKm
		{
			get
			{
				return 0;
			}
		}
		public override double amortizacionEuroKm
		{
			get
			{
				return 0;
			}
		}
		public override double neumaticosEuroKm
		{
			get
			{
				return 0;
			}
		}
		public override double mantenimientoEuroKm
		{
			get
			{
				return 0;
			}
		}


		public override double costesFuncionamientoEuroKm
		{
			get
			{
				return 0;
			}
		}
		public override double costesTiempoEuroKm
		{
			get
			{
				return 0;
			}
		}

	}
}
