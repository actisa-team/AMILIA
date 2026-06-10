using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad.EjeRasante
{
   public  class oZmaxMinAjuste
    {

       public double? zTerreno = null;
       public bool? isZK = null;
       public bool? isViable = null;




       public double? zAvancePosPendMax = null;
       public double? zAvancePosPendMin = null;
       public double? zAvancePosTerraplenMax = null;
       public double? zAvancePosDesmonteMin = null;

    
       public double? zAvanceNegPendMax = null;
       public double? zAvanceNegPendMin = null;
       public double? zAvanceNegTerraplenMax = null;
       public double? zAvanceNegDesmonteMin = null;


       public double? ZAvancePosMax = null;
       public double? ZAvancePosMin = null;

       public double? ZAvanceNegMax = null;
       public double? ZAvanceNegMin = null;





       public oZmaxMinAjuste(double iZterreno, bool iIsViable, bool iIsZK)
       {
           zTerreno = iZterreno;
           isZK = iIsZK;
           isViable = iIsViable;

       }


       /// <summary>
       /// Obtener del Zmax el Valor Minimo
       /// </summary>
       public double getAvancePosZmaxMin
       {
           get
           {
               if (isViable.Value) //ES Viable 
               {
                   return getMenor(zAvancePosTerraplenMax.Value, zAvancePosPendMax.Value);
               }
               else
               {
                   return zAvancePosPendMax.Value;
               }
                   
           }     
       }
       public double getAvancePosZminMax
       {
           get
           {
               if (isViable.Value) //ES Viable 
               {
                   return getMayor(zAvancePosDesmonteMin.Value, zAvancePosPendMin.Value);
               }
               else
               {
                   return zAvancePosPendMin.Value;
               }

           }
       }
       public double getAvanceNegZmaxMin
       {
           get
           {
               if (isViable.Value) //ES Viable 
               {
                   return getMenor(zAvanceNegTerraplenMax.Value, zAvanceNegPendMax.Value);
               }
               else
               {
                   return zAvanceNegPendMax.Value;
               }

           }
       }
       public double getAvanceNegZminMax
       {
           get
           {
               if (isViable.Value) //ES Viable 
               {
                   return getMayor(zAvanceNegDesmonteMin.Value, zAvanceNegPendMin.Value);
               }
               else
               {
                   return zAvanceNegPendMin.Value;
               }

           }
       }

       /// <summary>
       /// Obtener el Valor Máximo de la Cota Z ; Para Ajuste del Perfil Longitudinal
       /// </summary>
       public double getZmax
       {

           get
           {

               return getMenor(ZAvancePosMax.Value, ZAvanceNegMax.Value);
           
           
           }
       
       }

       public double getZmin
       {

           get
           {
               return getMenor(ZAvanceNegMax.Value, ZAvanceNegMin.Value);
           }
       }


       public override string ToString()
       {
           return "Zmax: " + getZmax.ToString() + " ; Zmin: " + getZmin.ToString();
       }


       private double getMayor(double iV1, double iV2)
      {

          if (iV1 > iV2)
          {
              return iV1;
          }
          else
          {
              return iV2;
          }
      
      }
       private double getMenor(double iV1, double iV2)
       {

           if (iV1 > iV2)
           {
               return iV2;
           }
           else
           {
               return iV1;
           }

       
       }


    }
}
