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
    using tadLayLogica.datos.BaseDatos;
    
    
    public class oDsBd :IDisposable
    {

       private static oDsBd mInstance = null;
       private dsBd mDs;
      


       #region "Constructores"

       public oDsBd()
       {
                mDs = new dsBd();
                mDs = oSingletonDsBd.getInstance.dataset;
                //mDs.ReadXml(oTadil.data.Files.fileBbdd);
       }

        public static oDsBd getInstance()
        {
            if (mInstance == null)
            {
                mInstance = new oDsBd();
            }
            return mInstance;
        }






       #endregion
       #region "PROPIEDADES"

   
       /// <summary>
       /// DataSet
       /// </summary>
       public dsBd dataset
       {
           get
           {
               if (mDs == null)
               {
                   mDs = oSingletonDsBd.getInstance.dataset;
                   //mDs.ReadXml(oTadil.data.Files.fileBbdd);
               }
               return mDs;
           }

       }

       /// <summary>
       /// Version Base Datos
       /// </summary>


       #endregion


       public void saveDataTable(DataTable iTb, bool iShowInfo)
       {
           dataset.AcceptChanges();
           dataset.WriteXml(oTadil.data.Files.fileBbdd);
           this.Dispose();
           if (iShowInfo)
           {
               oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
           }
           this.Dispose();

       }


        /// <summary>
        /// USUARIO REALIZA EL ACCEPT CHANGES EN SU TABLA
        /// </summary>
       public void saveSinAccepChanges()
       {
           dataset.WriteXml(oTadil.data.Files.fileBbdd);
           oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
           this.Dispose();
       }



       public void save()
       {
           dataset.AcceptChanges();
           dataset.WriteXml(oTadil.data.Files.fileBbdd);
           oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
           this.Dispose();
       }



       public void Dispose()
       {
           if (mDs != null)
           {
               mDs.Dispose();
               mDs = null;
               mInstance = null;
           }
       }



    }


   
 
}
