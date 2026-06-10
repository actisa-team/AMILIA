using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.normas
{


    using tadLayLogica;
    using tadLayData;
    using tadLayShare;
    using tadLayLan;
    
    
    public class oDalTbNormasKv
   {




       public static dsKv.tbKvDataTable getTabla ()
        {

            //Creo el DATASET
            dsKv miDsKv = new dsKv();

            //Cargo con datos el DataSet
            miDsKv.ReadXml(oTadil.data.Files.fileNormasCarreterasKv);


            return miDsKv.tbKv;
        }
       public static oKv getKvFromXmlProyecto (double iVp, eKvPrefer iKvPrefer)
       {

           //Creo el DATASET
           dsKv miDsKv = new dsKv();

           //Cargo con datos el DataSet
           miDsKv.ReadXml(oTadil.data.Files.fileNormasCarreterasKv);


           var myQuery = from p in miDsKv.tbKv
                         where p.vp >= iVp
                         orderby p.vp ascending
                         select p;


           if (myQuery.Count() == 0)
           {
               throw new oExRowNotFound(iVp.ToString(), strError.eTablaKV);
           }
           else
           {
               dsKv.tbKvRow miRow = myQuery.First();

               if (iKvPrefer == eKvPrefer.minimo)
               {
                   return new oKv(miRow.KvMinConvexo, miRow.KvMinConcavo);
               }
               else if (iKvPrefer == eKvPrefer.deseable)
               {
                   return new oKv(miRow.KvOptConvexo, miRow.KvOptConcavo);
               }
               else
               {
                   throw new oExEnumNotImplemented(iKvPrefer.ToString());
               }

           }

       }



   }

}
