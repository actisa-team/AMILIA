using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayData;
using tadLayLogica.datos.BaseDatos;

namespace tadLayLogica.datos.bbdd.Secciones
{
   public class oDalSecciones
    {
        public static dsBd.tbRoadUniGenDataTable getRoadUni()
        {
            oSingletonDsBd.getInstance.Dispose();
            return oSingletonDsBd.getInstance.dataset.tbRoadUniGen;
        }
        public static dsBd.tbRoadUniGenRow getRoadUni(Guid iId)
        {
            return oSingletonDsBd.getInstance.dataset.tbRoadUniGen.FindByid(iId);
        }
        public static void deleteZona(Guid iId)
        {
            oSingletonDsBd.getInstance.dataset.tbRoadUniGen.FindByid(iId).Delete();
            oSingletonDsBd.getInstance.save(false);
        }


        public static dsBd.tbRoadDobleAutoviaDataTable getRoadDoble()
        {
            oSingletonDsBd.getInstance.Dispose();
            return oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia;
        }
        public static dsBd.tbRoadDobleAutoviaRow getRoadDoble(Guid iId)
        {
            return oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia.FindByid(iId);
        }
        public static void deleteZonaDoble(Guid iId)
        {
            oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia.FindByid(iId).Delete();
            oSingletonDsBd.getInstance.save(false);
        }

        public static dsBd.tbRoadDobleSinMedianaDataTable getRoadDobleSinMediana()
        {
            oSingletonDsBd.getInstance.Dispose();
            return oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana;
        }
        public static dsBd.tbRoadDobleSinMedianaRow getRoadDobleSinMediana(Guid iId)
        {
            return oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana.FindByid(iId);
        }
        public static void deleteZonaDobleSinMediana(Guid iId)
        {
            oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana.FindByid(iId).Delete();
            oSingletonDsBd.getInstance.save(false);
        }



    }
}
