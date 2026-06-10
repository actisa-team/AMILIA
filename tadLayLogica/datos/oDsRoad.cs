using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayLogica.datos
{

    using System.Data;
    using tadLayLan;
    using tadLayData;
    using tadLayShare;
    
    
    public class oDsRoad :IDisposable
    {
       private dsInfra mDs;
       private string mFile;


       #region "Constructores"
  

       public oDsRoad()
       {
           mDs = new dsInfra();
           mFile = oTadil.data.Files.fileNormasCarreteras;
           mDs.ReadXml(mFile);
       }


        public oDsRoad (string iFileXml)
        {
            mDs = new dsInfra();
            mFile = iFileXml;
            mDs.ReadXml(mFile);
        }






       #endregion
       #region "PROPIEDADES"

   
       /// <summary>
       /// DataSet
       /// </summary>
       public dsInfra dataset
       {
           get
           {
               return mDs;
           }

       }


       public List<dsInfra.tbCarreteraItemsRow> getRowsByGrupo (eRoadGrupo iGrupo)
       {

           var miQuery = from p in dataset.tbCarreteraGrupos
                         where p.nombre == iGrupo.ToString()
                         select p;

           if (miQuery == null || miQuery.Count() == 0)
           {
               throw new oExRowNotFound(iGrupo.ToString(), dataset.tbCarreteraGrupos.TableName);
           }
           else
           {
             dsInfra.tbCarreteraGruposRow miRowGr = miQuery.First();

             var miLstRows = miRowGr.GetChildRows("FK_tbCarreteraGrupos_tbCarreteraItems");

             if (miLstRows == null || miLstRows.Count() == 0)
             {
                 throw new oExRowNotFound(iGrupo.ToString(), dataset.tbCarreteraItems.TableName);
             }
             else
             {
                 return miLstRows.ToList().ConvertAll<dsInfra.tbCarreteraItemsRow>(t => (dsInfra.tbCarreteraItemsRow)t);
             }
           }
       }

       #endregion







       public void Dispose()
       {
           mDs.Dispose();
           mDs = null;
       }



    }


  
}
