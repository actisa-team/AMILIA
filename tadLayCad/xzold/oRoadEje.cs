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

    using cv = Autodesk.Civil.Land.DatabaseServices;
    using Autodesk.Civil;




    using engCadNet;
    using tadLayShare;


    public class oRoadEje
    {


        private string mStObjEje = "stObjEje";
        private string mStLabEje = "stLabEje";
        private string mLayEje = "0";
       
        private cv.Alignment mEje = null;


        public oRoadEje()
        {



        }


        public cv.Alignment pEje
        {

            get
            {
                if (mEje == null)
                {
                    throw new oExPropertieNullValue("Eje Alineación");
                }

                

                return mEje;

            }


        }



        public void getEntidades()
        {
            string msg;
            int i=0;


            foreach (cv.AlignmentEntity myAe in pEje.Entities)
            {
                i++;

                switch (myAe.EntityType)
                {
                    
                    case cv.AlignmentEntityType.Line:

                        cv.AlignmentLine myLine = myAe as cv.AlignmentLine;

                        msg = "Linea";

                        break;

                    
                    case cv.AlignmentEntityType.Arc:


                        cv.AlignmentArc myArc = myAe as cv.AlignmentArc;
                        msg = String.Format("Entity{0} is an Arc, length: {1}\n", i, myArc.Length);
                        break;

                    case cv.AlignmentEntityType.Spiral:
                        cv.AlignmentSpiral mySpiral = myAe as cv.AlignmentSpiral;
                        msg = String.Format("Entity{0} is a Spiral, length: {1}\n", i, mySpiral.Length);
                        break;

                    case cv.AlignmentEntityType.SpiralCurveSpiral :

                        cv.AlignmentSCS myScS = myAe as cv.AlignmentSCS;

                        msg = "EspiralCurvaEspiral";
                        break;

                    case cv.AlignmentEntityType.SpiralSpiral:

                        cv.AlignmentSCS myEspiralEspiral = myAe as cv.AlignmentSCS;

                        cv.AlignmentSTS mySs = myAe as cv.AlignmentSTS;

                        break;

                    default:
                        msg = String.Format("Entity{0} is not a spiral or arc.\n", i);
                        break;
                }

            }


        }


        public void CreateEje(string iEjeName)
        {


            using (Transaction tr = oCadManager.StartTransaction())
            {

                string ejeName = iEjeName;


                try
                {
                    ObjectId myEjeId = oCadManager.thisDoc.GetSitelessAlignmentId(iEjeName);
                    mEje = (cv.Alignment) tr.GetObject(myEjeId,OpenMode.ForWrite);
                   

                }
                catch (ArgumentException)
                {
                    ObjectId myEje = cv.Alignment.Create(oCadManager.thisDoc, ejeName, "",mLayEje, mStObjEje, mStLabEje);
                    mEje = (cv.Alignment)tr.GetObject(myEje, OpenMode.ForWrite);
                    mEje.Description = "Valor Descripción";
                    mEje.UseDesignCheckSet = true;
                    tr.Commit();
                }

            }
        }


        public void addCurvaPaso(double iRadio)
        {


            double myCloA = getAFromRadio(iRadio);

            Point3d v1 = new Point3d(-4383.731, 3183.638, 0);
            Point3d v2 = new Point3d(-3633.928, 3200.819, 0);
            Point3d v3 = new Point3d(-2863.577, 3898.566, 0);
            Point3d v4 = new Point3d(-2333.845, 3228.223, 0);

            
            Point3d v3center = new Point3d(-2884.343,3649.024,0);
            Point3d v4center = new Point3d(-2312.741, 3478.526, 0);
            
  

            //Creo la Linea Inicial
            cv.AlignmentLine myV12Line = pEje.Entities.AddFixedLine(v1, v2);

            //Creo el Arco
            cv.AlignmentArc myV23Arco = pEje.Entities.AddFixedCurve(v3center, v3, true);

            //Creo la  LineaFlotanteDesdeCurvaPuntoPaso
            cv.AlignmentSTS myLineFlotanteFin = pEje.Entities.AddFloatingLineWithSpiral(myV23Arco.EntityId, cv.EntityAttachType.Prepend, myCloA, cv.SpiralParamType.AValue, v2, SpiralType.Clothoid);

            //Creo la Clotoide
            cv.AlignmentSCS myScS = pEje.Entities.AddFreeSCS(myV12Line.EntityId, myLineFlotanteFin.EntityId, myCloA, myCloA, cv.SpiralParamType.AValue, iRadio,false, SpiralType.Clothoid);
    
            //Creo el Arco
            //cv.AlignmentArc myV4Arco = pEje.Entities.AddFixedCurve(v4center, v4, false);

            //FreeSts
            //cv.AlignmentSTS mySsFreeCurves = pEje.Entities.AddFreeSSBetweenCurves(myV23Arco.EntityId, myV4Arco.EntityId, 1, cv.SpiralParamType.AValue, SpiralType.Clothoid);
                   
        }





        //CASO CURVAS DE NO PASO
        public void addEntidades(double iRadio)
        {




            double myCloA = getAFromRadio(iRadio);
            Point3d startPoint = new Point3d(433, 2030, 0.0000);
            Point3d middlePoint = new Point3d(496, 1704, 0.0000);
            Point3d endPoint = new Point3d(403, 1385, 0.0000);


            //Creo una Linea Inicial
            cv.AlignmentLine myLineIni = pEje.Entities.AddFixedLine(startPoint, middlePoint);

             //Creo la Linea Final
           cv.AlignmentLine myLineFin = pEje.Entities.AddFixedLine(middlePoint, endPoint);

            //Creo la Clotoide
           cv.AlignmentSCS myScS = pEje.Entities.AddFreeSCS(myLineIni.EntityId, myLineFin.EntityId,myCloA, myCloA,cv.SpiralParamType.AValue,iRadio, false, SpiralType.Clothoid);

        }


        public static double getAFromRadio(double iRadio)
        { 
        
            double value1 = (12* Math.Pow(iRadio,3));
        
            return Math.Pow(value1,0.25);
 
        }

    }
    }

