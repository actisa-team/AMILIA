using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad
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
        

        public static void addXdataSolucionNombre(object iEntidad,string iXdataData)
        {

            if (!string.IsNullOrEmpty(iXdataData))
            {
                Entity myEntidad = (Entity)iEntidad;
                engCadNet.oXdata.setXdata(myEntidad.ObjectId,eXdataKey.solucionName.ToString(), iXdataData);
            }


        }
        
        
        /// <summary>
        /// Obtener el Nombre de la Solución del Eje Básico
        /// </summary>
        public static string getXdataSolucionNombre(object iLwEjeBasico)
        {
          return  engCadNet.oXdata.getXData(iLwEjeBasico, eXdataKey.solucionName.ToString(), ";")[0];
        }

        #endregion

       #region "Datos Diseño"
       

        public static void addXdataRoadDesign(object iLwEjeBasico, oRoadDes iRoadDesign)
        {

            Entity myEntidad = (Entity)iLwEjeBasico;

            StringBuilder myXdata = new StringBuilder();

            myXdata.Append(iRoadDesign.Grupo.ToString()); //0
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.Vp.ToString()); // 1
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.Rp.ToString()); // 2
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.Tipo.ToString()); //3
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.IsAijK.ToString()); //4
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.kv.KvConcavo.ToString()); //5
            myXdata.Append(";");
            myXdata.Append(iRoadDesign.kv.kvConvexo.ToString()); //6


            engCadNet.oXdata.setXdata(myEntidad.ObjectId,eXdataKey.roadDis.ToString(), myXdata.ToString());
       
        }


        /// <summary>
        /// Obtener los Datos de Diseño del eje Básico
        /// </summary>
       public static oRoadDes getXdataRoadDesign (object iLwEjeBasico)
       {

           List<string> myLstRoad = engCadNet.oXdata.getXData(iLwEjeBasico,eXdataKey.roadDis.ToString(), ";");

           oRoadDes myRoadData = new oRoadDes();

           myRoadData.Grupo = (eRoadGrupo)Enum.Parse(typeof(eRoadGrupo), myLstRoad[0], true);

           myRoadData.Vp = Math.Round(Convert.ToDouble(myLstRoad[1]), 0);

           myRoadData.Rp = Math.Round(Convert.ToDouble(myLstRoad[2]), 0);

           myRoadData.Tipo = (eRoadTipo)Enum.Parse(typeof(eRoadTipo), myLstRoad[3], true);

           myRoadData.IsAijK = Convert.ToBoolean(myLstRoad[4]);

           myRoadData.kv.KvConcavo = Math.Round(Convert.ToDouble(myLstRoad[5]), 0);

           myRoadData.kv.kvConvexo = Math.Round(Convert.ToDouble(myLstRoad[6]), 0);

           return myRoadData;
      
       }

        #endregion

       #region "Eje Trazado ; Tipo Curvas"

       public static Dictionary<int, eRoadCurva> getListCurvasTipo(object iEjeTrazado)
       {

           Dictionary<int, eRoadCurva> myDicCurvas = new Dictionary<int, eRoadCurva>();
           eRoadCurva myCurva ;

           List<string> myLstCurves =  engCadNet.oXdata.getXData(iEjeTrazado, eXdataKey.ejeTrazadoCurvas.ToString(), ";");


           for (int i = 0; i < myLstCurves.Count; i++)
           {
               
               myCurva = (eRoadCurva) Enum.Parse(typeof(eRoadCurva), myLstCurves[i], true);

               myDicCurvas.Add(i, myCurva);

           }

           return myDicCurvas;
       
       }

       public static void addXdataCurvasTipo(object iEntidad, string iXdata)
       {
           if (!string.IsNullOrEmpty(iXdata))
           {
               Entity myEntidad = (Entity)iEntidad;
               engCadNet.oXdata.setXdata(myEntidad.ObjectId, eXdataKey.ejeTrazadoCurvas.ToString(), iXdata);
           }

       }

       #endregion

       #region "Datos Geometria"
       
       public static void addXdataRoadGeo(object iLwEjeBasico, oRoadGeo iRoadGeo)
       {

           Entity myEntidad = (Entity)iLwEjeBasico;

           StringBuilder myXdata = new StringBuilder();

           myXdata.Append(iRoadGeo.pEstructuraGenerar.ToString()); //0
           myXdata.Append(";");
           myXdata.Append(iRoadGeo.pTramoLonDiscre.ToString()); //1
           myXdata.Append(";");
           myXdata.Append(iRoadGeo.terraplenDesmonteMaxProyecto.ToString()); // 2
           myXdata.Append(";");
           myXdata.Append(iRoadGeo.pendMaxPorCientoProyecto.ToString()); // 3
           myXdata.Append(";");
           myXdata.Append(iRoadGeo.pendMaxPorCientoProyectoEstructura.ToString()); // 4
           myXdata.Append(";");
           myXdata.Append(iRoadGeo.pilaAlturaMaximaProyecto.ToString()); // 5
           myXdata.Append(";");
           myXdata.Append(iRoadGeo.pendMinProyectoPorCiento.ToString()); // 6
           myXdata.Append(";");
           myXdata.Append(iRoadGeo.pendMinEstructurasPorCiento.ToString()); // 7
 
           engCadNet.oXdata.setXdata(myEntidad.ObjectId, eXdataKey.roadGeo.ToString(), myXdata.ToString());

       }


       public static oRoadGeo getXdataRoadGeo(object iLwEjeBasico)
       {

           List<string> myLstRoad = engCadNet.oXdata.getXData(iLwEjeBasico, eXdataKey.roadGeo.ToString(), ";");

           oRoadGeo myRoadGeo = new oRoadGeo();

           myRoadGeo.pEstructuraGenerar = Convert.ToBoolean(myLstRoad[0]); //0

           myRoadGeo.pTramoLonDiscre = Math.Round(Convert.ToDouble(myLstRoad[1]), 2); //1

           myRoadGeo.terraplenDesmonteMaxProyecto = Math.Round(Convert.ToDouble(myLstRoad[2]), 0); //2

           myRoadGeo.pendMaxPorCientoProyecto = Math.Round(Convert.ToDouble(myLstRoad[3]), 2); //3

           myRoadGeo.pendMaxPorCientoProyectoEstructura = Math.Round(Convert.ToDouble(myLstRoad[4]), 2); // 4

           myRoadGeo.pilaAlturaMaximaProyecto= Math.Round(Convert.ToDouble(myLstRoad[5]), 2); // 5

           myRoadGeo.pendMinProyectoPorCiento = Math.Round(Convert.ToDouble(myLstRoad[6]), 2); // 6

           myRoadGeo.pendMinEstructurasPorCiento = Math.Round(Convert.ToDouble(myLstRoad[7]), 2); // 7

           return myRoadGeo;
       
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
       public static Dictionary<int, eEstructura> getListEstructuras(object iEjeBasico)
       {

           Dictionary<int, eEstructura> myDicEstructuras = new Dictionary<int, eEstructura>();
           eEstructura myEstructura;

           List<string> myLstEstructuras = engCadNet.oXdata.getXData(iEjeBasico, eXdataKey.ejeBasicoEstructuras.ToString(), ";");


           for (int i = 0; i < myLstEstructuras.Count; i++)
           {

               myEstructura = (eEstructura)Enum.Parse(typeof(eEstructura), myLstEstructuras[i], true);

               myDicEstructuras.Add(i, myEstructura);

           }

           return myDicEstructuras;

       }

        #endregion

    }
}