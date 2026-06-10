using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Calzada
{
    using engCadNet;
    using  Autodesk.AutoCAD.Geometry;

    using tadLayLogica.Secciones.Geometria;

    using tadLayData;
    
    public abstract  class oSecRoadAbstract
    {


        //Variables Constantes de la Sección NO Dependen del PK
        public double carrilAncho { get; set; }
        public int carrilNum { get; set; }

        public double firmeIntoArcen { get; set; }
        public double arcenExtAncho { get; set; }
        public double bermaExtAncho { get; set; }
        public double bermaExtPendiente { get; set; }
        public double taludFirme { get; set; }
        public double taludFirmeAsientoByCuneta { get; set; }


        public eCunetaPosicion cunetaPosicion { get; set; }

        public oCunetaAbstract cunetaExterior { get; set; }
       
       
       

        //Variables por Pk
        public Point3d ptoOrigen { get; set; }
        public double peralte { get; set; }
        public double firmeEspesor { get; set; }
        public double asientoEspesor { get; set; }
        public double arcenEspesor { get; set; }
        public double taludTerraplen { get; set; }


        //Areas
        protected double areaFirme{ get; set; }
        protected double areaAsiento { get; set; }
        protected double areaArcenExt { get; set; }
        protected double areaBermaExt { get; set; }
        protected double areaAsientoRecrecido { get; set; }



        #region "Constructores"     
        public oSecRoadAbstract(){}
        public oSecRoadAbstract(double iCarrilAncho,
                                int iCarrilNum,
                                double iFirmeIntoArcen,
                                double iArcenExtAncho,
                                double iBermaExtAncho,
                                double iBermaExtPendiente,
                                double iTaludFirme,
                                double iTaludFirmeAsientoByCuneta,
                                eCunetaPosicion iCunetaPosicion,
                                oCunetaAbstract iCunetaExterior)
                                
                                                 
        {

            carrilAncho = iCarrilAncho;
            carrilNum = iCarrilNum;
            firmeIntoArcen = iFirmeIntoArcen;
            arcenExtAncho = iArcenExtAncho;
            bermaExtAncho = iBermaExtAncho;
            bermaExtPendiente = iBermaExtPendiente;
            taludFirme = iTaludFirme;
            taludFirmeAsientoByCuneta = iTaludFirmeAsientoByCuneta;
            cunetaPosicion = iCunetaPosicion;
            cunetaExterior = iCunetaExterior;
        }
        #endregion


        #region "Propiedades"    
    protected double peraltePendienteUno
    {
        get
        {
            return peralte / 100;
        }
    }
    protected double bermaPendienteUno
    {
        get
        {
            return bermaExtPendiente / 100;
        }
    }
        #endregion


       #region "MetodosAbstractos" 

       public abstract double anchoCalzada {get;}
       public abstract eExcavacion taludTipo { get; }
       public abstract Point3dCollection lstPtoExplanada { get; }
       public abstract Point3d ptoExplanada { get;}
       public abstract Point3d ptoCalzadaExterior { get; }
       protected abstract void geometria();

       /// <summary>
       /// VARIABLES DEPENDEN DEL PK
       /// </summary>
       public abstract void setUpByPk(double iPk, eLado iLado,Point3d iPtoOrigen,double iPeraltePorCien,double iFirmeEspesor,double iArcenEspesor, double iAsientoEspesor, double iTaludTerraplen);


       public abstract void setUpMaterialesById(Dictionary<int, dsBd.tbCapasRow> iLstCapaFirme,
                                                Dictionary<int, dsBd.tbCapasRow> iLstCapasArcen,
                                                Dictionary<int, dsBd.tbCapasRow> iLstCapasFirme,
                                                Guid iBermaMat);

      
       #endregion




   

       public List<Point3dCollection> lstGeometria { get; set; }

  

       public void drawSection(string iLayer)
       {
           foreach (Point3dCollection miColLw in lstGeometria)
           {
               oLw.addLw2d(miColLw, false, iLayer);
           }
       }







    }
}
