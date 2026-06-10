using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad.EjeRasante
{

    using engCadNet;
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using cv = Autodesk.Civil.Land.DatabaseServices;
    using Autodesk.Civil;


    using tadLayShare.EjeTJ;
    using tadLayShare.EjeTJ.puntos;
    using tadLayShare;
    
    
    public class oEjeRasanteFactory
    {

        public oEjeRasanteFactory()
        { 
        
        }

        public oEjeTadilRasante getEjeRasante(string iTerrenoNombre, double? iPenIni, double? iPenFin)
        {
           
            
            
            
            //Obtengo el Eje Trazado
            cv.Alignment myEjeTrazado = oEjeTrazado.getEjeTrazadoUser();

            //Seleccion del Nombre de la Solucion
            string mySolName = tadLayCad.oTadilXdata.getXdataSolucionNombre(myEjeTrazado as object);

            //Seleccion el Eje Basico
            Polyline3d myEjeBasico = oEjeTrazado.getEjeBasicoFromTrazado(mySolName);

 
            //Obtengo los Datos de la Polilinea Basica
            oRoadGeo  myRoadGeo = tadLayCad.oTadilXdata.getXdataRoadGeo(myEjeBasico as object);
            oRoadDes  myRoadDes = tadLayCad.oTadilXdata.getXdataRoadDesign(myEjeBasico as object);

            //Obtengo los Listados
            Dictionary<int, oAcuerdo> myDicAcuerdos = getAcuerdos(myEjeBasico,myEjeTrazado);
            Dictionary<int, eEstructura> myDicEstructuras = getEstructuras(myEjeBasico);
            

            //Creo el Eje Tadil Rasante
            oEjeTadilRasante myEjeTadilRasante = new oEjeTadilRasante(iTerrenoNombre,
                                                                      mySolName,
                                                                      myRoadDes,
                                                                      myRoadGeo,
                                                                      myDicAcuerdos,
                                                                      myDicEstructuras,
                                                                      myEjeTrazado,
                                                                      iPenIni,
                                                                      iPenFin);
                                                                     




           //Devuelvo el Objeto

            return myEjeTadilRasante;


  
        }


       

       #region "Metodos Privados"


        private Dictionary<int, eEstructura> getEstructuras (Polyline3d iEjeBasico)
        {


            //Obtengo el listado de Estructuras por Tramos.
            Dictionary<int, eEstructura> myDicEst = tadLayCad.oTadilXdata.getListEstructuras(iEjeBasico as object);


            return myDicEst;
        }



       private  Dictionary<int,oAcuerdo> getAcuerdos(Polyline3d iEjeBasico, cv.Alignment iEjeTrazado)
        {

                  
            //Obtengo las Curvas del Eje de Trazado Incluyo Punto Inicial y Final
            Dictionary<int, eRoadCurva> myDicCurvas = tadLayCad.oTadilXdata.getListCurvasTipo(iEjeTrazado as object);


            //Obtengo las Entidades Curvas del Eje Trazado
            Dictionary<int, cv.AlignmentEntity> myDicCurvaEntidad = getCurvasEntidades(iEjeTrazado);

            //Creo el listado de Acuerdos
            Dictionary<int, oAcuerdo> myDicAcuerdos = new Dictionary<int, oAcuerdo>();

            //Creo el Acuerdo 1
            double myZ0 = iEjeBasico.GetPointAtParameter(0).Z;
            oAcuerdo myAcuerdoIni = new oAcuerdo(0, new oP3d(0, 0, myZ0));
            myDicAcuerdos.Add(0, myAcuerdoIni);


            //Recorro las curvas intermedia
            oAcuerdo myAcuerdo;
            double myVerticeId;
            double myPk;
            double myZ;

            for (int i = 1; i < myDicCurvas.Count - 1; i++)
            {
                myVerticeId = Convert.ToDouble(i);


                if (myDicCurvas[i] == eRoadCurva.Paso)
                {

                    if (myDicCurvaEntidad[i].EntityType == cv.AlignmentEntityType.Arc)
                    {
                        cv.AlignmentArc myArco = (cv.AlignmentArc)myDicCurvaEntidad[i];
                        myPk = iEjeTrazado.GetDistAtPoint(iEjeBasico.GetPointAtParameter(myVerticeId));
                        myZ = iEjeBasico.GetPointAtParameter(myVerticeId).Z;

                        myAcuerdo = new oAcuerdo(i, new oP3d(myPk, 0, myZ));

                        myDicAcuerdos.Add(i, myAcuerdo);

                    }
                    else
                    {

                        throw new NotImplementedException("Curva Paso Solo Ser Arco");

                    }



                }
                else if (myDicCurvas[i] == eRoadCurva.NoPaso)
                {

                    if (myDicCurvaEntidad[i].EntityType == cv.AlignmentEntityType.Arc)
                    {
                        cv.AlignmentArc myArco = (cv.AlignmentArc)myDicCurvaEntidad[i];
                        myPk = getPkCurveMid(myArco.StartStation, myArco.EndStation);
                        myZ = iEjeBasico.GetPointAtParameter(myVerticeId).Z;

                        myAcuerdo = new oAcuerdo(i, new oP3d(myPk, 0, myZ));

                        myDicAcuerdos.Add(i, myAcuerdo);

                    }

                    else if (myDicCurvaEntidad[i].EntityType == cv.AlignmentEntityType.SpiralCurveSpiral)
                    {
                        cv.AlignmentSCS mySCS = (cv.AlignmentSCS)myDicCurvaEntidad[i];
                        myPk = getPkCurveMid(mySCS.Arc.StartStation, mySCS.Arc.EndStation);
                        myZ = iEjeBasico.GetPointAtParameter(myVerticeId).Z;

                        myAcuerdo = new oAcuerdo(i, new oP3d(myPk, 0, myZ));

                        myDicAcuerdos.Add(i, myAcuerdo);

                    }

                    else
                    {
                        throw new NotImplementedException("Error Curva No Definidad");
                    }

                }

            }


            //Acuerdo Final Base Cero
            int myVerticeFin = myDicCurvas.Count - 1;

            double myZFin = iEjeBasico.GetPointAtParameter(myVerticeFin).Z;

            oAcuerdo myAcuerdoFin = new oAcuerdo(myVerticeFin, new oP3d(iEjeTrazado.Length, 0, myZFin));

            myDicAcuerdos.Add(myVerticeFin, myAcuerdoFin);



            return myDicAcuerdos;

            





        }



       private double getPkCurveMid(double iStartStation, double iEndStation)
       {

           double myPkini = iStartStation;
           double myPkFin = iEndStation;

           return myPkini + (myPkFin - myPkini) / 2;
       }
       private Dictionary<int, cv.AlignmentEntity> getCurvasEntidades(cv.Alignment iEjeTrazado)
       {

           Dictionary<int, cv.AlignmentEntity> myDicEntidadCurva = new Dictionary<int, cv.AlignmentEntity>();

           int myCurveNum = 1;

           cv.AlignmentEntityCollection entities = iEjeTrazado.Entities;
           int currentEntityId = entities.FirstEntity;
           int lastEntityId = entities.LastEntity;
           bool done = false;
           do
           {
               cv.AlignmentEntity entity = entities.EntityAtId(currentEntityId);



               if (entity.EntityType == cv.AlignmentEntityType.Arc | entity.EntityType == cv.AlignmentEntityType.SpiralCurveSpiral)
               {

                   myDicEntidadCurva.Add(myCurveNum, entity);

                   myCurveNum++;
               }



               if (currentEntityId == lastEntityId)
               {
                   done = true;
               }
               else
               {
                   currentEntityId = entity.EntityAfter;
               }

           } while (!done);


           return myDicEntidadCurva;
       }
       #endregion




      

    }
}
