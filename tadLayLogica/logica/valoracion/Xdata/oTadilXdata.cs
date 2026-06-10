using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{
   
     using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using tadLayShare;
    
    public  class oTadilXdata
    {

       #region "Solucion Nombre"


        public static string getXdataSolucionNombreGuid(Entity iEntidad)
        {
            return engCadNet.oXdata.getXData(iEntidad, eXdataKey.solucionGuid.ToString(), string.Empty)[0];
        }


        public static void addXdataSolucionNombreGuid(Entity iEntidad, string iXdataData)
        {
            if (!string.IsNullOrEmpty(iXdataData))
            {
                engCadNet.oXdata.setXdata(iEntidad.ObjectId, eXdataKey.solucionGuid.ToString(), iXdataData);
            }
        
        }

        public static void addXdataSolucionNombre(object iEntidad,string iXdataData)
        {

            if (!string.IsNullOrEmpty(iXdataData))
            {
                Entity myEntidad = (Entity)iEntidad;
                engCadNet.oXdata.setXdata(myEntidad.ObjectId,eXdataKey.solucionName.ToString(), iXdataData);
            }


        }
        
        
        /// <summary>
        /// Obtener el Nombre de la Solución del Eje
        /// </summary>
        public static string getXdataSolucionNombre(Entity iLwEjeBasico)
        {
          return  engCadNet.oXdata.getXData(iLwEjeBasico, eXdataKey.solucionName.ToString(), ";")[0];
        }

        #endregion

       #region "Datos Diseño"
       

        public static void addXdataRoadDesign(object iLwEjeBasico, oRoadDes iRoadDesign)
        {

            Entity myEntidad = (Entity)iLwEjeBasico;

            StringBuilder myXdata = new StringBuilder();

            myXdata.Append(iRoadDesign.grupo.ToString()); //0
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.Vp.ToString()); // 1
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.Rp.ToString()); // 2
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.preferencias.ToString()); //3
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.IsAijK.ToString()); //4
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.allowPermitirReduccionesVelocidad.ToString()); //5
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.preferenciasKv.ToString()); //6
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.kvConcavo.ToString()); //7
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.kvConvexo.ToString()); //8
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.peralte); //9
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.AijMin); //10
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.AijMinSalidaLlegada); //11


            engCadNet.oXdata.setXdata(myEntidad.ObjectId,eXdataKey.roadDis.ToString(), myXdata.ToString());
       
        }


        /// <summary>
        /// Obtener los Datos de Diseño del eje Básico
        /// </summary>
       public static oRoadDes getXdataRoadDesign (Entity iLwEjeBasico)
       {

           List<string> myLstRoad = engCadNet.oXdata.getXData(iLwEjeBasico,eXdataKey.roadDis.ToString(), ";");


           eRoadGrupo miGrupo = (eRoadGrupo)Enum.Parse(typeof(eRoadGrupo), myLstRoad[0], true);

           double miVp = Math.Round(Convert.ToDouble(myLstRoad[1]), 0);

           double miRp  = Math.Round(Convert.ToDouble(myLstRoad[2]), 0);

           eRoadPreferencias miPreferencias = (eRoadPreferencias)Enum.Parse(typeof(eRoadPreferencias), myLstRoad[3], true);

           bool miIsAijConstante = Convert.ToBoolean(myLstRoad[4]);

           bool miAllowReduccionVelocidad = Convert.ToBoolean(myLstRoad[5]);

           eKvPrefer miKvPreferencias = (eKvPrefer)Enum.Parse(typeof(eKvPrefer), myLstRoad[6], true);

           double miKvConcavo  = Math.Round(Convert.ToDouble(myLstRoad[7]), 0);

           double miKvConvexo = Math.Round(Convert.ToDouble(myLstRoad[8]), 0);

           double miPeralte = Math.Round(Convert.ToDouble(myLstRoad[9]), 2);

            double miAjmin = Math.Round(Convert.ToDouble(myLstRoad[10]), 2);

            double miAjminSalidaLLegada = Math.Round(Convert.ToDouble(myLstRoad[11]), 2);

            oRoadDes miRoad = new oRoadDes(miGrupo, miVp, miRp, miPreferencias, miIsAijConstante,miAllowReduccionVelocidad,miKvPreferencias, miKvConvexo, miKvConcavo,miPeralte, miAjmin, miAjminSalidaLLegada);

           return miRoad;

      
       }

        #endregion


       #region "Datos Estructuras"

       public static void addXdataEstructurasTramo(object iEntidad, string iXdata)
       {
           if (!string.IsNullOrEmpty(iXdata))
           {
               Entity myEntidad = (Entity)iEntidad;
               engCadNet.oXdata.setXdata(myEntidad.ObjectId, eXdataKey.ejeBasicoEstructuras.ToString(), iXdata);
           }

       }

        /// <summary>
        /// Obtengo el listado de Tramos Estructuras.
        /// </summary>
       public static Dictionary<int, eEstructuraOld> getListEstructuras(Entity iEjeBasico)
       {

           Dictionary<int, eEstructuraOld> myDicEstructuras = new Dictionary<int, eEstructuraOld>();
           eEstructuraOld myEstructura;

           List<string> myLstEstructuras = engCadNet.oXdata.getXData(iEjeBasico, eXdataKey.ejeBasicoEstructuras.ToString(), ";");


           for (int i = 0; i < myLstEstructuras.Count; i++)
           {

               myEstructura = (eEstructuraOld)Enum.Parse(typeof(eEstructuraOld), myLstEstructuras[i], true);

               myDicEstructuras.Add(i, myEstructura);

           }

           return myDicEstructuras;

       }

        #endregion

    }
}