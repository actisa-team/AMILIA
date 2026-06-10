using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.BaseDatos
{
   
    using System.Data;
    using tadLayLan;
    using tadLayData;
    using tadLayShare;

    public class oSingletonDsBd : IDisposable
    {


        private static dsBd mDs = null;

        private static oSingletonDsBd mInstance = null;
        private dsBd.tbGeoRow mZonaMovimientoTierrasDefault = null;
        private dsBd.tbTunRow mZonaTunelesDefault = null;
        private dsBd.tbEstRow mZonaEstructurasDefault = null;
        private dsBd.tbCimRow mZonaCimentacionDefault = null;

        private string mSimboloMonetario = string.Empty;
        private string mNombreMonetario = string.Empty;

        private dsBd.tbValExcavabilidadTaludRow mValoracionExcavabilidadTalud = null;


        #region "Constructores"
        private oSingletonDsBd()
        {
                mDs = new dsBd();

                mDs.ReadXml(oTadil.data.Files.fileBbdd);

        }
        public static oSingletonDsBd getInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new oSingletonDsBd();

                   // oTadilCore.evReset += new EventHandler<engNet.eventos.oEventArgs<bool>>(oTadilCore_evReset);
                }

                return mInstance;
            }
        }

        static void oTadilCore_evReset(object sender, engNet.eventos.oEventArgs<bool> e)
        {
            if (e.Value)
            {

                oTadilCore.evReset -= new EventHandler<engNet.eventos.oEventArgs<bool>>(oTadilCore_evReset);   

                mInstance.Dispose();

               
            }
        }

        public static void deleteInstance()
        {
            if (mDs != null)
            {
                mDs.Clear();
                mDs.Dispose();
                mDs = null;
            }
            
            
            mInstance = null;
        }

        #endregion


        #region "Propiedades"

        public  string version
        {
            get
            {
               return oSingletonDsBd.getInstance.dataset.tbVersion[0].id;
            }
        }


        public string monedaSimbolo
        {

            get
            {
                if (string.IsNullOrEmpty(mSimboloMonetario))
                {
                    mSimboloMonetario = oSingletonDsBd.getInstance.dataset.tbMoneda[0].simbolo;
                }

                return mSimboloMonetario;
            }


        }

        public string monedaNombre
        {

            get
            {
                if (string.IsNullOrEmpty(mNombreMonetario))
                {
                    mNombreMonetario = oSingletonDsBd.getInstance.dataset.tbMoneda[0].descripcion;
                }

                return mNombreMonetario;
            }


        }



        public dsBd dataset
        {
            get
            {
                return mDs;
            }
        }

        #endregion


        #region "Metodos"

        public void save (bool iShowInfo)
        {
            oSingletonDsBd.getInstance.dataset.AcceptChanges();

            oSingletonDsBd.getInstance.dataset.WriteXml(oTadil.data.Files.fileBbdd);

           if (iShowInfo)
           {
               oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
           }
           this.Dispose();

       }
        public void saveDataTable(DataTable iTb, bool iShowInfo)
        {
            iTb.AcceptChanges();
            oSingletonDsBd.getInstance.dataset.WriteXml(oTadil.data.Files.fileBbdd);

            if (iShowInfo)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
            }
            this.Dispose();

        }



        public dsBd.tbGeoRow getZonaMovimientoTierras(Guid iId)
        {
            return oSingletonDsBd.getInstance.dataset.tbGeo.FindByid(iId);
        }
        public dsBd.tbZgItemsRow getZonaClasificacion(Guid iId)
        {
            return oSingletonDsBd.getInstance.dataset.tbZgItems.FindByid(iId);
        }




        #endregion


        #region "ValoracionTalud"


        #endregion

       

        /// <summary>
        /// Datos Valoracion Excavabilidad
        /// </summary>
        public dsBd.tbValExcavabilidadTaludRow valoracionExcavabilidadTalud
        {
            get
            {
                if (mValoracionExcavabilidadTalud == null)
                {
                    mValoracionExcavabilidadTalud =  this.dataset.tbValExcavabilidadTalud.FindByid("VAEXTA");
                }

                return mValoracionExcavabilidadTalud;
            }
        }


        #region "Zonas Dominio Publico Hidraulico "

        public dsBd.tbDoHiDataTable getTablaDominioPublicoHidraulico()
        {
            return oSingletonDsBd.getInstance.dataset.tbDoHi;
        }
        public dsBd.tbDoHiRow getZonaDominioPublicoHidraulico(Guid iId)
        {
            return oSingletonDsBd.getInstance.dataset.tbDoHi.FindByid(iId);
        }
        public void deleteById(Guid iIdZona)
        {
            oSingletonDsBd.getInstance.dataset.tbDoHi.FindByid(iIdZona).Delete();
            saveDataTable(oSingletonDsBd.getInstance.dataset.tbDoHi, true);
        }
        #endregion
        #region "CruceInfraEstructuras"


        public  dsBd.tbCruceInfraRow getZonaCruceInfraEstructuras (Guid iId)
        {
            return oSingletonDsBd.getInstance.dataset.tbCruceInfra.FindByid(iId);
        }



        #endregion


        public void Dispose()
        {
            if (mInstance != null)
            {
                mDs.Dispose();
                mDs = null;
                mInstance = null;
            }
        }
    }
    
}
