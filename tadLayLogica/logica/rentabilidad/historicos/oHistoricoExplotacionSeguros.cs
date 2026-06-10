using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
  public  class oHistoricoExplotacionSeguros
    {

      private bool? mIsInversionPrivada = null;
      private double? mCostesExplotacion = null;
      private double? mCostesExplotacionManoObraPU = null;
      private double? mCostesSeguros = null;
      private int? mYearsExplotacion = null;


      Dictionary<int, oRenExplotacionSeguros> mLstCostes = null;

      public oHistoricoExplotacionSeguros(bool iIsInversionPrivada,    
                                          double iCosteExplotacionAnual,
                                          double iCosteExplotacionManoObraPC,
                                          double iCostesSegurosAnual,
                                          int iYearsExplotacion)
						                   
      {
          mIsInversionPrivada = iIsInversionPrivada;
          mCostesExplotacion = iCosteExplotacionAnual;
          mCostesExplotacionManoObraPU= iCosteExplotacionManoObraPC/100;
          mCostesSeguros = iCostesSegurosAnual;
          mYearsExplotacion = iYearsExplotacion;        
      }


      public Dictionary<int,oRenExplotacionSeguros> lstCoste
      {
          get
          {
              if (mLstCostes == null)
              {
                  mLstCostes = getLstCostes();
              }

              return mLstCostes;

          }



      }

      private Dictionary<int,oRenExplotacionSeguros> getLstCostes()
      {

          Dictionary<int, oRenExplotacionSeguros> miLstCostes = new Dictionary<int, oRenExplotacionSeguros>();
 
        
          if (mIsInversionPrivada.Value)
          {

              for (int i = 1; i <= mYearsExplotacion.Value; i++)
              {
                  miLstCostes.Add(i, new oRenExplotacionSeguros(mCostesExplotacion.Value,mCostesExplotacionManoObraPU.Value, mCostesSeguros.Value));
              }
          }
          else
          {
              for (int i = 1; i <= mYearsExplotacion.Value; i++)
              {
                  miLstCostes.Add(i, new oRenExplotacionSeguros(0,0,0));
              }
          }

          return miLstCostes;
      }


    

    }

  public class oRenExplotacionSeguros
  {

      public double costesExplotacion { get; private set; }
      public double costesExplotacionManoObra { get; private set; }
      public double costesSeguros { get; private set; }

      public double costeExplotacionySeguros { get { return costesExplotacion + costesSeguros; } }

      public oRenExplotacionSeguros(double iCosteExplotacion, double iCosteManoObraPU,  double iCosteSeguros)
      {
          costesExplotacion = iCosteExplotacion;
          costesExplotacionManoObra = iCosteExplotacion*iCosteManoObraPU;
          costesSeguros = iCosteSeguros;
      }

  }

 

}
