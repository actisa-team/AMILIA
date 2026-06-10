using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadCom
{

    using System.IO;
    
   
    using Autodesk.AutoCAD.Interop.Common;
    using Autodesk.AECC.Interop.Land;

    
    
    public class oTool
    {



        public static object getTinSurfaceObjByName(string iTinSurface)
        {
            return (object)getTinSurfaceByName(iTinSurface);
        }

        
        public static AeccTinSurface  getTinSurfaceByName (string iTinSurfaceName)
        {
        
            foreach (AeccTinSurface surface in oEngCadCom.get.thisCiv3dDoc.Surfaces)
            {
                if (surface.Name == iTinSurfaceName)
                {
                    return surface;
                }
            }

            throw new Exception(string.Format("La superficie {0} No se ha encontrado", iTinSurfaceName));

        }


        public static double? getFindZFromXY(object iTinSurface, double iPx, double iPy)
        {

            AeccTinSurface mySurface = (AeccTinSurface)iTinSurface;

            try
            {

                double? myZ = mySurface.FindElevationAtXY(iPx, iPy);

                if (myZ.HasValue)
                {

                    return myZ.Value;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception)
            {
                return null;
            }
        
        
        
        
        }




        public static void drawLine(double iP1x, double iP1y,double iP1z, double iP2x, double iP2y, double iP2z, string iLayerName)
        { 
        
            double[] myP1 = new double[3];
            double[] myP2 = new double[3];

            myP1[0] = iP1x;
            myP1[1] = iP1y;
            myP1[2] = iP1z;

            myP2[0] = iP2x;
            myP2[1] = iP2y;
            myP2[2] = iP2z;

             
           AcadLine myLine =  oEngCadCom.get.thisCadApp.ActiveDocument.ModelSpace.AddLine(myP1, myP2);

           myLine.Layer = iLayerName;
        
        
        }



        public static void drawText(string iTexto, double iP1x, double iP1y, double iP1z, double iTxtAlto, string iLayerName)
        { 
        
           double[] myP1 = new double[3];
         

            myP1[0] = iP1x;
            myP1[1] = iP1y;
            myP1[2] = iP1z;

            AcadText myacadText= engCadCom.oEngCadCom.get.thisCadApp.ActiveDocument.ModelSpace.AddText(iTexto, myP1, iTxtAlto);

            myacadText.Layer = iLayerName;

        
        
        }


        public static void layerCreateAndErase(string iLayerName, int iLayerColorIndex )
        {


            if (!layerExiste(iLayerName))
            {

                AcadLayer myLayer = oEngCadCom.get.thisCadApp.ActiveDocument.Layers.Add(iLayerName);

                AcadAcCmColor myLayColor = (AcadAcCmColor)  oEngCadCom.get.thisCadApp.GetInterfaceObject("AutoCAD.AcCmColor.18");

                myLayColor.ColorIndex = (AcColor)iLayerColorIndex;

                myLayer.TrueColor = myLayColor;

            }
            else
            { 
            
            
            }


            //     Dim col As AcadAcCmColor
            //col = thisAcadApp.GetInterfaceObject(oCadShare.appColor)
            //col.ColorIndex = pLayerColor
            //pLayer.TrueColor = col
        
        
        
        }


        public static bool layerExiste(string iLayerName)
        {


            try
            {

                AcadLayer myLayer = oEngCadCom.get.thisCadApp.ActiveDocument.Layers.Item(iLayerName);
                return true;

            }
            catch (Exception)
            {

                return false;
            }

        
        
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static void selectPoint()
        {

            //object myPtoOld = null;

            //object myPtoObj = oEngCadCom.get.thisCadApp.ActiveDocument..Utility.GetPoint(myPtoOld, "Selecciona Punto");

            //double[] myPto = (double[])myPtoObj;


        }


        public static void drawBlock(string iBlockDefName, double iP1x, double iP1y, double iP1z, string iAttValor)
        {

            AcadBlockReference myBlock;
            
            double[] myP1 = new double[3];

            myP1[0] = iP1x;
            myP1[1] = iP1y;
            myP1[2] = iP1z;


           myBlock= oEngCadCom.get.thisCadApp.ActiveDocument.ModelSpace.InsertBlock(myP1, iBlockDefName, 1, 1, 1, 0);


        }


        public static string getPathDwg()
        { 
        
          string myFolderPath = oEngCadCom.get.thisCadApp.ActiveDocument.FullName;



          myFolderPath=  Path.GetDirectoryName(myFolderPath);


          return myFolderPath;
        
        }


        public static void gCadUpdate()
        {


            engCadCom.oEngCadCom.get.thisCadApp.Update();

        
        
        
        }
   

               

            


    }
}
