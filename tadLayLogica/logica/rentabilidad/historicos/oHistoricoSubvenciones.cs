using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
  public  class oHistoricoSubvenciones
    {

      private bool? mIsInversionPrivada = null;
      private double? mSubvencionPeajeVehiculo = null;
      private double? mSubvencionAnual = null;
      private int? mYearsExplotacion = null;
      private Func<int, int> mTraficoHistorico = null;

      Dictionary<int, oRenSubvencionAnual> mLstCostes = null;

      public oHistoricoSubvenciones(bool iIsInversionPrivada,    
                                    double iSubvencionPorVehiculo,
                                    double iSubvencionAnual,
                                    int iYearsExplotacion,
						            Func<int,int> iTraficoHistorico)
      {

          mIsInversionPrivada = iIsInversionPrivada;
          mSubvencionPeajeVehiculo = iSubvencionPorVehiculo;
          mSubvencionAnual = iSubvencionAnual;
          mYearsExplotacion = iYearsExplotacion;
          mTraficoHistorico = iTraficoHistorico;
      }


      public Dictionary<int,oRenSubvencionAnual> lstCoste
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



      private Dictionary<int,oRenSubvencionAnual> getLstCostes()
      {

          Dictionary<int, oRenSubvencionAnual> miLstCostes = new Dictionary<int, oRenSubvencionAnual>();
          oRenSubvencionAnual miSubAnual;
          int? miTraficoAnual = null;

          if (mIsInversionPrivada.Value)
          {

              for (int i = 1; i <= mYearsExplotacion.Value; i++)
              {
                  miTraficoAnual = mTraficoHistorico(i);

                  miSubAnual = new oRenSubvencionAnual(miTraficoAnual.Value, mSubvencionPeajeVehiculo.Value, mSubvencionAnual.Value);

                  miLstCostes.Add(i, miSubAnual);

              }


          }
          else
          {

              for (int i = 1; i <= mYearsExplotacion.Value; i++)
              {
                  miSubAnual = new oRenSubvencionAnual(0, 0);
                  miLstCostes.Add(i, miSubAnual);
              }
          }

          return miLstCostes;
      }

    }
  public class oRenSubvencionAnual
  {

      public double subvencionPorVehiculos { get; private set; }
      public double subvencionAnual { get; private set; }


      public oRenSubvencionAnual(int iTraficoAnual, double iSubvencionPorVehiculo, double iSubAnual)
      {
          subvencionPorVehiculos = iTraficoAnual * iSubvencionPorVehiculo;
          subvencionAnual = iSubAnual;
      }

      public oRenSubvencionAnual(double iSubVehiculoAnual, double iSubAnual)
      {
          subvencionPorVehiculos = iSubAnual;
          subvencionAnual = iSubVehiculoAnual;
      }



  }

}
