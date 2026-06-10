using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

    using tadLayCad;
    using tadLayShare;
    
    public class oRoadDesTrazado:oRoadDes
    {

        private string mLayer = string.Empty;
        private string mSolName = string.Empty;
        private Object mLw3d = null;


        public oRoadDesTrazado(Object iLw)
        {

            //Lw3d.
            mLw3d = iLw;

            //Solucion Nombre.
            mSolName = tadLayCad.oTadilXdata.getXdataSolucionNombre(iLw);

            //Capa Nombre.
            mLayer = string.Format(oTadil.KSolEjeTrazado, mSolName);

            //Datos de Diseño de la Carretera.
            oRoadDes myLwData = tadLayCad.oTadilXdata.getXdataRoadDesign(iLw);

            base.Grupo = myLwData.Grupo;

            base.Vp = myLwData.Vp;

            base.Rp = myLwData.Rp;

            base.Tipo = myLwData.Tipo;

            base.IsAijK = myLwData.IsAijK;

        
        }

           #region "Metodos Públicos"


        public  void addTrazado()
        {

           
            //Creo el Eje
            oEjeTrazado myEjeCivil = new oEjeTrazado(mSolName, Tipo, Rp, getTipoCurva,getRadioNormaAspiral);

            myEjeCivil.addVertices(mLw3d);

            myEjeCivil.check(Amin, AminSalidaLlegada, Amax,0.5);

            myEjeCivil.addRoad(mLayer,oTadil.KRoadEstiloEje,oTadil.KRoadEstiloLabel,this.ToString());
              
        }

        /// <summary>
        /// Obtener el Tipo de Curva del Eje
        /// </summary>
        /// <param name="iAngGr">Ángulo en el vertice</param>
        /// <returns>Tipo de Curva</returns>
        public eRoadCurva getTipoCurva(double iAngGr)
        {

            switch (Tipo)
            {
                case eRoadTipo.preferCurvas:

                    if (iAngGr <= 120)
                    {
                        return eRoadCurva.Paso;
                    }
                    else if (iAngGr > 120 && iAngGr <= 180)
                    {
                        return eRoadCurva.NoPaso;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Ángulo Tramos");
                    }


                case eRoadTipo.preferRectas:

                    if (iAngGr <= 100)
                    {
                        return eRoadCurva.Paso;
                    }
                    else if (iAngGr > 100 && iAngGr <= 180)
                    {
                        return eRoadCurva.NoPaso;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Ángulo Tramos");
                    }

                default:

                    throw new Exception(string.Format("La Preferencia {0} No esta Definidad", Tipo.ToString()));

            }
        }
        /// <summary>
        /// Obtener el Radio según la Norma y El Valor de la Espiral
        /// </summary>
        public oRadioNormaAvalue getRadioNormaAspiral(double iAngGr, eRoadCurva iCurvaTipo)
        {

            if (iAngGr > 180)
            {
                throw new ArgumentOutOfRangeException(iAngGr.ToString());
            }
            if (Grupo == eRoadGrupo.Grupo1 && iAngGr > 176.5 && iAngGr <= 180)
            {
                oRadioNormaAvalueTipo myRdAs = new oRadioNormaAvalueTipo(5000);

                return myRdAs;
            }
            else if (Grupo == eRoadGrupo.Grupo2 && iAngGr > 176.5 && iAngGr <= 180)
            {
                oRadioNormaAvalueTipo myRdAs = new oRadioNormaAvalueTipo(2500);

                return myRdAs;
            }
            else if (IsAijK && iCurvaTipo == eRoadCurva.Paso && iAngGr >= 0 && iAngGr <= 40.7)
            {
                oRadioNormaAvalueTipo myRdAs = new oRadioNormaAvalueTipo(Rp * 0.72);

                return myRdAs;
            }
            else if (IsAijK && iCurvaTipo == eRoadCurva.Paso && iAngGr > 40.7 && iAngGr <= 83)
            {
                oRadioNormaAvalueTipo myRdAs = new oRadioNormaAvalueTipo(Rp * 0.83);

                return myRdAs;
            }
            else if (iCurvaTipo == eRoadCurva.NoPaso)
            {
                //Caso Curvas de No Paso
                double myAngAgudoRadianes = oToolTrigo.getRadianesFromGrados(180 - iAngGr);
                double myValor = (Math.Pow(12, 0.5)) * Math.Pow(Rp, -0.5); // 12^0.5 * Rp^-0.5

                //Caso Tipo
                if (myAngAgudoRadianes >= myValor)
                {
                    oRadioNormaAvalueTipo myRdAs = new oRadioNormaAvalueTipo(Rp);

                    return myRdAs;
                }
                else
                {

                    double myRadioPrima = 1.5 * Rp;
                    double myLe = myRadioPrima * myAngAgudoRadianes;

                    //CNP CALIDAD
                    if (myLe > 20)
                    {

                        oRadioNormaAvalueCNPCalidad myRdAs = new oRadioNormaAvalueCNPCalidad(Rp, myAngAgudoRadianes);
                        return myRdAs;

                    }
                    //CNP SIN CALIDAD
                    else
                    {
                        
                        oRadioNormaAvalueCNPSinCalidad myRdAs = new oRadioNormaAvalueCNPSinCalidad(myAngAgudoRadianes);
                        return myRdAs;
                    }

                }

            }

            else
            {
                oRadioNormaAvalueTipo myRdAs = new oRadioNormaAvalueTipo(Rp);

                return myRdAs;

            }
        }


        public override string ToString()
        {
            return  Grupo.ToString() + " | " + 
                   "Vp: " + Vp.ToString() + "km/h | " + 
                   "Rp: " + Rp.ToString() + "m | " + 
                   "Preferencias: " + Tipo.ToString() + " | " +
                   "Aij Constante: " + IsAijK.ToString()+ " | " ;
        }


        #endregion



    }
}
