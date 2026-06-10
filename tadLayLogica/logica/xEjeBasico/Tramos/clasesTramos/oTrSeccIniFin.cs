//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace tadLayLogica
//{

//    using tadLayShare;
//    using tadLayShare.puntoOld;
    
//    public  class oTrSecIniFin : oTramoSec
//    {


  
  


    
//       public oTrSecIniFin(int iTrId,oP3d iPtoIni, oP3d iPtoFin)
//       {
//           pOrden = iTrId;
//           P1 = iPtoIni;
//           P2 = iPtoFin;     
//           pEstGenerar = false;
//           pIsAbanicoAvance = false;
//           pName = "Entronque";
           
          
         
           
//       }


 
//       public override void getXY()
//       {


   

//       }


//       public override void getSeccion(double iTramoLonDiscre, double iProPendMaxPorCiento, double iProTerDesMax, double iEstPendMaxPorCiento, double iEstPuenteMax)
//       {

//           if (pTramoValido.HasValue && pTramoValido.Value)
//           {
           
//               //Obtengo la Sección Sin Estructura
//               pLstTramoSeccion = getLstSeccion(iTramoLonDiscre);

//               //Confirmo Si el Movimiento de Tierras es Valido
//               bool myMovTierras = getMovTierrasSinEstructuras(iProTerDesMax);

//               //Movimiento Tierras OK
//               if (myMovTierras)
//               {
//                   pTramoValido = true;
//                   pEstHas = false;
//                   return;
//               }
//               else
//               {
//                   pTramoValido = false;
//                   return;
//               }     
//           }

//       }

//    }
//}
