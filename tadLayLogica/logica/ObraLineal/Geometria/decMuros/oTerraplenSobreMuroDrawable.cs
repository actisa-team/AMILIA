using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;

    using engCadNet;


    using tadLayLogica.logica.medicion;
    using tadLayLogica.Secciones.Interfaz;
    using tadLayShare;
  
    
 
    public  class oTerraplenSobreMuroDrawable : oTerraplenSobreMuroModel, ISecDrawPlus
    {

       private bool? mMuroDraw = null;
       private List<Point3dCollection> mColGeometria;
       private Point3dCollection mEnvolvente = null;


       public oTerraplenSobreMuroDrawable(Guid iIdMaterial,double iTerraplenAltura, double iTerraplenTalud, double iMuroEspesor,double iMuroEmpotramiento)
           : base(iIdMaterial, iTerraplenAltura, iTerraplenTalud, iMuroEspesor, iMuroEmpotramiento)
       {


       }


       public oTerraplenSobreMuroDrawable(oTerraplenSobreMuroModel iTerraplenSobreMuro)
             :base(iTerraplenSobreMuro.material,iTerraplenSobreMuro.taludTerraplenAltura,iTerraplenSobreMuro.taludH,iTerraplenSobreMuro.espesor,iTerraplenSobreMuro.empotramiento)
        
       { 
       
       
       }


       public Dictionary<int, ISecDrawPlus> dicIsecDraw { get; set; }
       public ISecDrawPlus parent{ get {return dicIsecDraw[0] ;} }
       public ISecDrawPlus previo { get { return dicIsecDraw[dicIsecDraw.Count - 2]; } }
       public Point3d ptoInsertChild { get { return cC2; } set { ;} }
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
            setUp();
        }


        private void setUp()
        {

          
            mEnvolvente = new Point3dCollection();


            cC0 = previo.ptoInsertChild;
            cC1 = cC0.getFromTalud(true, false, taludH, taludTerraplenAltura);
            cC2 = cC1.getFromIncXIncY(espesor, 0, 0);


            bool miTaludCortaTerreno = this.islineaTaludInterseccionTerreno(cC0, cC1);

            eExcavacion miCasoEstudio;

            if (miTaludCortaTerreno)
            {
                miCasoEstudio = eExcavacion.desmonte;
            }
            else
            {
                miCasoEstudio = funGetTerrenoCorreccionTnd(pk, lado, cC1);
            }



          
            switch (miCasoEstudio)
            {
                case eExcavacion.desmonte:

                    taludLstPol = new Point3dCollection();
                    taludLstPol.Add(cC0);
                    taludLstPol.Add(cC1);

                    mMuroDraw = false;
                    taludDraw = true;

                    break;

                case eExcavacion.terraplen:

                    double miC1tnd = funGetZtnd(pk, cC1.X, cC1.Y, lado);
                    cC1tnd = new Point3d(cC1.X, miC1tnd, 0);

                    double miC2tnd = funGetZtnd(pk, cC2.X, cC2.Y, lado);
                    cC2tnd = new Point3d(cC2.X, miC2tnd, 0);

                    cC1emp = cC1tnd.getFromIncXIncY(0, -empotramiento, 0);

                    cC2emp = cC1emp.getFromIncXIncY(espesor, 0,0);


                    //Genero la Geometria
                    mColGeometria = new List<Point3dCollection>();

                    Point3dCollection miColTalud = new Point3dCollection();
                    miColTalud.Add(cC0);
                    miColTalud.Add(cC1);

                    Point3dCollection miColMuro = new Point3dCollection();
                    miColMuro.Add(cC1);
                    miColMuro.Add(cC1tnd);
                    miColMuro.Add(cC1emp);
                    miColMuro.Add(cC2emp);
                    miColMuro.Add(cC2tnd);
                    miColMuro.Add(cC2);
                    miColMuro.Add(cC1);

                    mColGeometria.Add(miColTalud);
                    mColGeometria.Add(miColMuro);

                    //Genero el Talud
                    taludLstPol = new Point3dCollection();
                    taludLstPol.Add(cC0);
                    taludLstPol.Add(cC1);
                    taludLstPol.Add(cC1emp);

                    //Genero los Puntos de la Envolvente                                                                     
                    mEnvolvente.Add(cC0);
                    mEnvolvente.Add(cC1);
                    mEnvolvente.Add(cC1tnd);
                 

                    mMuroDraw = true;
                    taludDraw = true;

                    break;

                case eExcavacion.acota:

                    //Genero el Talud
                    taludLstPol = new Point3dCollection();
                    taludLstPol.Add(cC0);
                    taludLstPol.Add(cC1);

                    mMuroDraw = false;
                    taludDraw = true;
                    break;
                   
                default:

                    throw new oExEnumNotImplemented(miCasoEstudio.ToString());
            }


        
        
        }


        private bool islineaTaludInterseccionTerreno(Point3d icC0,Point3d icC1)
        {

            Line miLineaTalud = new Line(icC0, icC1);


            double miDistanciaToOrigen = 0.25;

            double miIntervaloSeparacion = 0.25;

            
            Point3d miPtoConsulta;

            eExcavacion miPtoTipo; 

            while (miDistanciaToOrigen < miLineaTalud.Length)
            {
                miPtoConsulta = miLineaTalud.GetPointAtDist(miDistanciaToOrigen);

                miPtoTipo = funGetTerrenoCorreccionTnd(pk, lado, miPtoConsulta);

                if (miPtoTipo == eExcavacion.desmonte)
                {
                    return true;
                }

                miDistanciaToOrigen = miDistanciaToOrigen+miIntervaloSeparacion;
            }


            return false;

        }



        public override double area
        {
            get { return (cC1.Y - cC1emp.Y) * espesor; }
        }




        public void draw(string iLayer, Matrix3d? iMatrix)
        {

            #region "DECORATOR"
            previo.draw(iLayer, iMatrix);
            #endregion

            #region "DRAW"
            if (mMuroDraw.Value) 
            {
                foreach (Point3dCollection miColLw in mColGeometria)
                {
                    oLw.addLw2d(miColLw, false, iLayer, iMatrix, oSeccionDecoradorParent.colorDecorator);
                }
            }
            #endregion



        }


        #region "INTERFACE ENVOLVENTE"

        public Point3dCollection envolvente
        {
            get
            {

                if (mEnvolvente == null)
                {
                    throw new oExPropertieNullValue("Envolvente");
                }
                else
                {

                    return mEnvolvente;
                }
            
            }
        }



        #endregion

        public List<oMedItemModel> medicion
        {
            get
            {
                List<oMedItemModel> miLstMedicion = new List<oMedItemModel>();

                if (mMuroDraw.Value)
                {
                    miLstMedicion.Add(new oMedMuro(material,area));
                }

                return miLstMedicion;
            
            }
        }
    }
}
