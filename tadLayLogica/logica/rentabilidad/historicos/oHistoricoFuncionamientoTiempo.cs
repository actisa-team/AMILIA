using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
  public class oHistoricoFuncionamientoTiempo
    {

    
           oRenCosteFunTieVehiculoLigero mVehiculoLigero = null;
           oRenCosteFunTieVehiculoPesado mVehiculoPesado = null;

           private double? mVehiculoPesadoPC = null;
           protected int? mYearsExplotacion = null;


           protected Dictionary<int, oRenCosteFunTieVehiculoPonderado> mLstCostes = null;


           public oHistoricoFuncionamientoTiempo()
           {

           }

           public oHistoricoFuncionamientoTiempo(oRenCosteFunTieVehiculoLigero iVehiculoLigero,
                                                 oRenCosteFunTieVehiculoPesado iVehiculoPesado,
                                                 double iVehiculoPesadoPC,
                                                 int iYearsExplotacion)

           {


               mVehiculoLigero = iVehiculoLigero;
               mVehiculoPesado = iVehiculoPesado;
               mVehiculoPesadoPC = iVehiculoPesadoPC;
               mYearsExplotacion = iYearsExplotacion;

           }


           public virtual Dictionary<int, oRenCosteFunTieVehiculoPonderado> lstCostes
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



           private Dictionary<int, oRenCosteFunTieVehiculoPonderado> getLstCostes()
           {
               Dictionary<int, oRenCosteFunTieVehiculoPonderado> miLstCostes = new Dictionary<int, oRenCosteFunTieVehiculoPonderado>();


               for (int i = 1; i <= mYearsExplotacion; i++)
               {
                   miLstCostes.Add(i, new oRenCosteFunTieVehiculoPonderado(mVehiculoLigero, mVehiculoPesado, mVehiculoPesadoPC.Value));
               }

               return miLstCostes;
           }
       


    }
  public class oHistoricoFuncionamientoTiempoCosteCero : oHistoricoFuncionamientoTiempo
  {

      public oHistoricoFuncionamientoTiempoCosteCero(int iYearsExplotacion)
      {
          mYearsExplotacion = iYearsExplotacion;
      }


      public override Dictionary<int, oRenCosteFunTieVehiculoPonderado> lstCostes
      {
          get
          {
              Dictionary<int, oRenCosteFunTieVehiculoPonderado> miLstCostes = new Dictionary<int, oRenCosteFunTieVehiculoPonderado>();
              
              
              for (int i = 1; i <= mYearsExplotacion; i++)
              {
                  miLstCostes.Add(i, new oRenCosteFunTieVehiculoPonderadoCosteCero());
              }

              return miLstCostes;
          }
      }



  }
}
