using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ.Vertice
{

    using Autodesk.AutoCAD.Geometry;

    using engCadNet;

    using engNet.Extension.Double;
    using engNet.Extension.Integer;
    using tadLayLogica.EjeTJ.Tramos;
    using tadLayLogica.EjeTJ.Vertice;
    using tadLayLogica.EjeTJ.Secciones;
    using tadLayShare.puntos;
    using tadLayShare;
    
    
    public class oVerticeExplanada:oVer
    {

        private double?  mDistanciaOrigen=null;
        private oSeccionSimple mSeccion;

        public oVerticeExplanada()
        {
        
        
        }


        public oVerticeExplanada(int iIdBaseCero, Point3d iPto, double iDistanciaOrigen, oSeccionSimple iSeccion)
            : base(iIdBaseCero, new oP3d(iPto.X, iPto.Y, iPto.Z))
        {
            mDistanciaOrigen = iDistanciaOrigen;
            mSeccion = iSeccion;
        }


       public oVerticeExplanada(int iIdBaseCero, double iX, double iY, double iDistanciaOrigen)
                :base(iIdBaseCero,new oP3d(iX,iY,0))
       
       {
           mDistanciaOrigen = iDistanciaOrigen;
        
       }

       public oSeccionSimple seccion
       {

           get
           {
               return mSeccion;
           }

           set
           {
               mSeccion = value;           
           }
       
       }
       public double distanciaOrigen
       {
           get
           {
               if (mDistanciaOrigen.HasValue)
               {
                   return mDistanciaOrigen.Value;
               }
               else
               {
                   throw new oExPropertieNullValue("distancia Origen");
               }
           
           }

           set
           {

               mDistanciaOrigen = value;
           
           }
       
       }


       public void draw(string iLayer)
       {

           oText.addText2D(id.ToString() + " ; " + distanciaOrigen.roundOff(2).ToString() + " ; " + seccion.ToString(), new Point2d(position.toArray2d()), 0.1, 1.57, iLayer);
       }


    }
    public class oTramoExplanada : oTramoMaster<oVerticeExplanada>
    {

        private List<oVerticeExplanada> mLstSeccion;



        #region "Constructores"


        

        public oTramoExplanada()
        { 
        
        
        }

        public oTramoExplanada(oVerticeExplanada iVi, oVerticeExplanada iVj)
            :base(iVi,iVj)
        
        { 
        
        
        
        
        
        }

        #endregion


        #region "Propiedades"


        public List<oVerticeExplanada> lstVerticesSeccion
        {

            get
            {

                if (mLstSeccion == null)
                {
                    throw new Exception("No se ha generado la Sección del Tramo.");
                }
                else
                {
                    return mLstSeccion;
                
                }
 
            }
              
        }




        #endregion

  



        public void addVerticeInterseccion(Point3d iPto)
        {

            IP3d miPto = new oP3d(iPto.X, iPto.Y, iPto.Z);

            //Distancia al Origen
            double miDistanciaOrigen = Vi.distanciaOrigen + Vi.position.distTo3d(miPto);

            //Seccion 
            oSeccionSimple miSeccion = new oSeccionSimple(eExcavacion.acota,0);

            //Creo el VerticeExplanda
            oVerticeExplanada miVertice = new oVerticeExplanada(int.MaxValue, iPto, miDistanciaOrigen, miSeccion);


            //Ahora Añado el Vertice
            mLstSeccion.Add(miVertice);


            //Reiniciamos los Valores del Id
             var myQuery = from p in mLstSeccion  
                           orderby p.distanciaOrigen ascending
                           select p;


            mLstSeccion = myQuery.ToList();

            int i = 0;

            foreach (oVerticeExplanada fvertice in mLstSeccion)
	        {
		 
                fvertice.id = i;

            i++;
                   
	        }


        }




        public void getSeccíon(double iIntervalo, Func<IP3d,oSeccionSimple> iFunSeccion)
        {

            mLstSeccion = new List<oVerticeExplanada>();
            oVerticeExplanada miVerExplanada;
            oP2d miPto;
            int j = 0;
            
           
            double miIntervaloBruto = this.lon2d.Value/ iIntervalo;

            int miIntervaloEntero = miIntervaloBruto.roundEnteroSuperior();

            double miIntervaloDist = this.lon2d.Value/miIntervaloEntero;

            double miLonOrigen = miIntervaloDist;



            if (this.isTramoInicial)
            {
                miVerExplanada = new oVerticeExplanada(0, Vi.position.X, Vi.position.Y, 0);
                miVerExplanada.seccion = iFunSeccion( (oP3d) Vi.position);
                mLstSeccion.Add(miVerExplanada);
                j=1;

            }
            else
            {
               j=0;
            }


            //Caso que la longitud del segmento sea inferior al Intervalo de Discretizacion
            if (this.lon2d.Value > iIntervalo)
            {

                miPto = (oP2d)oTrigo.getP2FromAzimutLon(Vi.position, this.azimutGrados, miLonOrigen);


                for (int i = 0; i <= miIntervaloEntero - 1; i++)
                {

                    miVerExplanada = new oVerticeExplanada(i + j, miPto.X, miPto.Y, this.Vi.distanciaOrigen + miLonOrigen);

                    miVerExplanada.seccion = iFunSeccion(miPto.convertTo3d(0));

                    mLstSeccion.Add(miVerExplanada);

                    miLonOrigen = miLonOrigen + miIntervaloDist;

                    miPto = (oP2d)oTrigo.getP2FromAzimutLon(this.Vi.position, this.azimutGrados, miLonOrigen);

                }
            }

        }


        public override object createDerivedFromBase(object myBase)
        {
            oTramoBase mySegmento = (oTramoBase)myBase;

            this.id = mySegmento.id;
            this.Vi = mySegmento.Vi as oVerticeExplanada;
            this.Vj = mySegmento.Vj as oVerticeExplanada;

            return this;


        }
    
    }
}
