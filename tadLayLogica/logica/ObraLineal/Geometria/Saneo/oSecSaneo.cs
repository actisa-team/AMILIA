using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria.Saneo
{
   
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    

    using tadLayLogica.logica.medicion;
    using tadLayLogica.Secciones.Interfaz;
    using tadLayShare;
    
    public  class oSecSaneo
    {





        private List<ISecDrawSaneo> mLstIsaneo = null;

        private oSaneoDesmonteModel mSaneoDes;
        private oSaneoTerraplenModel mSaneoTer;

        private Polyline mLwExplanada;
        private Polyline mLwTerrenoOriginal;
        private Polyline mLwTerrenoSeccion;
        private double mPk = -1;


        
        public oSecSaneo(Polyline iLwExplanada, Polyline iLwTndOriginal, Polyline iLwTndSeccion, oSaneoDesmonteModel iSaneoDesData, oSaneoTerraplenModel iSaneoTerData, double iPk = -1)
        {
            mLwExplanada = iLwExplanada;
            mLwTerrenoOriginal = iLwTndOriginal;
            mLwTerrenoSeccion = iLwTndSeccion;
            mSaneoDes = iSaneoDesData;
            mSaneoTer = iSaneoTerData;
            mPk = iPk;
        }



        public bool createSaneo
        {
            get
            {
                if (mSaneoDes == null && mSaneoTer ==null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

        public bool createSaneoDesmonte ()
        {
            if (mSaneoDes != null)
            {
                return true;
            }
            else
            {
                return false;
            }
         
        }

        public bool createSaneoTerraplen()
        {

            if (mSaneoTer != null)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
   
  

        public void draw(string iLayer)
        {

            if (createSaneo)
            {
                mLstIsaneo = getSaneo();

                foreach (ISecDrawSaneo fSaneoDraw in mLstIsaneo)
                {
                    fSaneoDraw.drawSaneo(iLayer);
                }
            }
           
        }



        public List<oMedItemModel> medicion
        {


            get
            {

                List<oMedItemModel> miMedicion = new List<oMedItemModel>();

                if (mLstIsaneo != null && mLstIsaneo.Count > 0)
                {
                    foreach (ISecMedicion fSaneoMed in mLstIsaneo)
                    {
                        miMedicion.AddRange(fSaneoMed.medicion);
                    }
                }


                return miMedicion;
            }
        }



        private List<ISecDrawSaneo> getSaneo()
        {

          List<ISecDrawSaneo>  miLstSaneo = new List<ISecDrawSaneo>();


           //Creo el Eje de Saneo 
           oEjeSaneo miEjeSaneo = new oEjeSaneo("EjeSaneo",mLwExplanada, mLwTerrenoOriginal,mLwTerrenoSeccion);


           //Obtengo el Listado de Polilineas Saneo Base
           List<oLwSaneo> miList = miEjeSaneo.getSaneoExplanda(createSaneoDesmonte(),createSaneoTerraplen());

            //Genero los Saneos
            int i = 0;
            int k = miList.Count - 1;

            foreach (oLwSaneo fLwSaneo in miList)
            {

                //Primero o Ultimo
                if (i == 0 | i == k)
                {

                    switch (fLwSaneo.saneo)
                    {
                        case eSaneo.terraplen:
                            miLstSaneo.Add(saneoTerraplenEscalon(fLwSaneo));
                            break;
                        case eSaneo.desmonte:
                            miLstSaneo.Add(saneoDesmonte(fLwSaneo));
                            break;
                        default:
                            throw new oExEnumNotImplemented(fLwSaneo.saneo.ToString());                          
                    }


                }

                //Intermedios
                else
                {
                    switch (fLwSaneo.saneo)
                    {
                        case eSaneo.terraplen:
                            miLstSaneo.Add(saneoTerraplenSimple(fLwSaneo));
                            break;
                        case eSaneo.desmonte:
                            miLstSaneo.Add(saneoDesmonte(fLwSaneo));
                            break;
                        default:
                            throw new oExEnumNotImplemented(fLwSaneo.saneo.ToString());
                    }
                }

                i++;
            }


            return miLstSaneo;
        
        
        }

        private ISecDrawSaneo saneoDesmonte(oLwSaneo iLwSaneo)
        {
            oSaneoSimple miSaneo = new oSaneoSimple(
                                                    mSaneoDes.materialExcavacion,
                                                    mSaneoDes.materialRelleno,
                                                    mSaneoDes.espesor,
                                                    iLwSaneo.lwSaneo,
                                                    iLwSaneo.saneo);
            miSaneo.Pk = mPk;
            return miSaneo;  
        }
        private ISecDrawSaneo saneoTerraplenSimple(oLwSaneo iLwSaneo)
        {
            oSaneoSimple miSaneo = new oSaneoSimple(
                                                    mSaneoTer.materialExcavacion,
                                                    mSaneoTer.materialRelleno,
                                                    mSaneoTer.espesor,
                                                    iLwSaneo.lwSaneo,
                                                    iLwSaneo.saneo);
            miSaneo.Pk = mPk;
            return miSaneo;   
        }



       
        private ISecDrawSaneo saneoTerraplenEscalon(oLwSaneo iLwSaneo)
        {


            double miLwPendiente = oSaneo.getPendienteSaneoTerraplen_AbsPorCiento(iLwSaneo.lwSaneo);


            ISecDrawSaneo miSaneoOUT;

            if (miLwPendiente < mSaneoTer.pendienteMaxSinEscalon)
            {
                return saneoTerraplenSimple(iLwSaneo);
            }
            else
            {

                eSaneoTerraplenEscalon miSaneoEscalonTipo = this.getTipoSaneoTerraplenEscalon(iLwSaneo.lwSaneo);


                switch (miSaneoEscalonTipo)
                {
                    case eSaneoTerraplenEscalon.simple:

                      miSaneoOUT = new oSaneoEscalon(mSaneoTer.materialExcavacion, mSaneoTer.materialRelleno, mSaneoTer.espesor, mSaneoTer.escalonHmax, mSaneoTer.pendienteMaxSinEscalon, iLwSaneo.lwSaneo);
                      ((oSaneo)miSaneoOUT).Pk = mPk;
                      return miSaneoOUT;

                    case eSaneoTerraplenEscalon.dobleInferior:

                       miSaneoOUT = new oSaneoEscalonDobleInferior(mSaneoTer.materialExcavacion, mSaneoTer.materialRelleno, mSaneoTer.espesor, mSaneoTer.escalonHmax, mSaneoTer.pendienteMaxSinEscalon, iLwSaneo.lwSaneo);                                                                                      
                       ((oSaneo)miSaneoOUT).Pk = mPk;
                       return miSaneoOUT;
                       
                    case eSaneoTerraplenEscalon.dobleSuperior:

                       miSaneoOUT = new oSaneoEscalonDobleSuperior(mSaneoTer.materialExcavacion, mSaneoTer.materialRelleno, mSaneoTer.espesor, mSaneoTer.escalonHmax, mSaneoTer.pendienteMaxSinEscalon, iLwSaneo.lwSaneo);
                       ((oSaneo)miSaneoOUT).Pk = mPk;
                       return miSaneoOUT;

                    default:

                       throw new oExEnumNotImplemented(miSaneoEscalonTipo.ToString());      
                }


             
            }
       }





        private eSaneoTerraplenEscalon getTipoSaneoTerraplenEscalon(Polyline iLwSaneo)
        {

            if (iLwSaneo.NumberOfVertices == 2)
            {
                return eSaneoTerraplenEscalon.simple;
            }

            else
            {

                Point3d miPmin = oSaneo.getPtoIntermedioMinY(iLwSaneo);
                double miPtoIntermedioInferior = miPmin.Y;



                Point3d miPmax = oSaneo.getPtoIntermedioMaxY(iLwSaneo);
                double miPtoIntermedioSuperior = miPmax.Y;


                double miPtoIniCotaY = iLwSaneo.StartPoint.Y;
                double miPtoFinCotaY = iLwSaneo.EndPoint.Y;
              


                if (miPtoIniCotaY > miPtoIntermedioInferior && miPtoFinCotaY > miPtoIntermedioInferior)
                {
                    return eSaneoTerraplenEscalon.dobleInferior;
                }
                else if (miPtoIntermedioSuperior > miPtoIniCotaY && miPtoIntermedioSuperior > miPtoFinCotaY)
                {
                    return eSaneoTerraplenEscalon.dobleSuperior;
                }
                else
                {
                    return eSaneoTerraplenEscalon.simple;
                }

            }

        }
        
    }




}
