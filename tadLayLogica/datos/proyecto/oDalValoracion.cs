using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{

    using tadLayLan;
    using tadLayData;
    using engNet.ClassT;
    
    
    public class oDalValoracion
    {


     /// <summary>
     ///Añadimos los Valores Por Defecto a la Matriz de Decisión
     /// </summary>
     public static void addMatrizValoracionDefault()
       {
                //Consulto el número de Registros
               if (oSingletonDsApp.getInstance.dataset.tbMatrizDecision.Rows.Count==0)
               {
                 engNet.oCsvLoad miCsvLoad = new engNet.oCsvLoad(oTadil.data.Files.fileDatMatrizDecisionDefault,";","/",true);

                 miCsvLoad.loadCsvIntoDataTableTyped(oSingletonDsApp.getInstance.dataset.tbMatrizDecision);
                   
                 oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbMatrizDecision,false);

               }

       }









    

     

       }
}
