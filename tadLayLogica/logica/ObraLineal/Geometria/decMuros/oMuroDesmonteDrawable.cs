using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{

    using tadLayLogica.logica.medicion;

    using System.ComponentModel;
    using engCadNet;

    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.Colors;
  
    using tadLayLogica.Secciones.Interfaz;

    /// <summary>
    /// JUAN 05.09.14
    /// </summary>
    public class oMuroDesmonteDrawable : oMuroDesmonteModel, ISecDrawPlus
    {

        private bool? mMuroDraw= null;
        private double mTaludH;
        private Point3dCollection mLstGeometria = null;
        private Point3dCollection mEnvolvente = null;

        #region "Constructores"

        public oMuroDesmonteDrawable(Guid iIdMaterial, double iEspesor, double iEmpotramiento, double iAltura, double iTaludH)
          :base(iIdMaterial, iEspesor,iEmpotramiento,iAltura)
        {
            mTaludH = iTaludH;
        }

        public oMuroDesmonteDrawable(oMuroDesmonteModel iMuro, double iTaludH)
            : base(iMuro.material,iMuro.espesor, iMuro.empotramiento, iMuro.altura)
        {
            mTaludH = iTaludH;
        }

        #endregion
 
        public  Point3dCollection envolvente
        {

            get
            {
                if (mEnvolvente == null )
                {

                    if (mMuroDraw.Value)
                    {
                        mEnvolvente = new Point3dCollection();
                        mEnvolvente.Add(cC1);
                        mEnvolvente.Add(cC2);
                        mEnvolvente.Add(cC3);
                    }
                    else
                    {
                        mEnvolvente = new Point3dCollection();
                    }
                    
                }

                return mEnvolvente;
            }
        }

        #region "INTERFACE MEDICION"

       public List<oMedItemModel> medicion
       {
           get
           { 
               
               List<oMedItemModel> miLst = new List<oMedItemModel>();

               if (mMuroDraw.Value)
               {
                   oMedItemModel miMed = new oMedMuro (material,area);
                   miLst.Add(miMed);
               }
              
               return miLst;
        
           }
       }

       #endregion

        #region "INTERFACE IDRAW"

        public Dictionary<int, ISecDrawPlus> dicIsecDraw { get; set; }
        public ISecDrawPlus parent { get { return dicIsecDraw[0]; } }
        public ISecDrawPlus previo { get { return dicIsecDraw[dicIsecDraw.Count - 2]; } }
        public Point3d ptoInsertChild { get { return cC3; } set { ; } }
        public bool taludDraw { get; set; }
        public double pk { get; set; }
        public eLado lado { get; set; }
     



        public Point3dCollection taludLstPol { get; set; }


        public void addDecorator(ISecDrawPlus iSecDecorator)
        {
            dicIsecDraw = new Dictionary<int, ISecDrawPlus>(iSecDecorator.dicIsecDraw);
            dicIsecDraw.Add(dicIsecDraw.Count, this);

            pk = parent.pk;
            lado = parent.lado;

            //Obtengo la Geometria del Muro
            mLstGeometria = geometria(previo.ptoInsertChild);

            taludLstPol = getTalud(); 
        }




        public void draw(string iLayer, Matrix3d? iMatrix)
        {

            #region "DECORATOR"
            previo.draw(iLayer, iMatrix);
            #endregion
            #region "DRAW"
            if (mMuroDraw.Value)
            {
                oLw.addLw2d(mLstGeometria, true, iLayer, iMatrix, oSeccionDecoradorParent.colorDecorator);
            }
            #endregion
        }

         private Point3dCollection getTalud()
        {


            Point3dCollection miColTalud = new Point3dCollection();


            //Debo de Establecer los Casos
            double miC3Ztnd = funGetZtnd(pk, cC3.X, cC3.Y, lado);


            // CASO 1 [TND > C3]
            if (miC3Ztnd > cC3.Y)
            {
                //Puntos Talud
                miColTalud.Add(cC3);
                miColTalud.Add(cC3.getFromTalud(true, true, mTaludH, 1));

                //Dibujo el Talud
                taludDraw = true;
                mMuroDraw = true;

            }
            else if (miC3Ztnd > cC0.Y)
            {
                //Configuro la altura del muro, para que se ajuste al terreno
                cC3 = new Point3d(cC3.X, miC3Ztnd, 0);
                cC4 = new Point3d(cC4.X, miC3Ztnd, 0);
               
                miColTalud.Add(cC2);
                miColTalud.Add(cC3);

                //He modificado los puntos debo de cargarlos
                mLstGeometria[4] = cC4;
                mLstGeometria[3] = cC3;

                //Dibujo el Talud
                taludDraw = false;
                mMuroDraw = true;

            }

            else //Por Debajo de la Cota del Z=
            {

                miColTalud.Add(cC4);
                miColTalud.Add(cC1);

                //Dibujo el Talud
                taludDraw = false;
                mMuroDraw = false;
            }


            return miColTalud;




        }

        #endregion

      

    }

}        

