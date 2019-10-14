using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logica.componentes;
using Logica.puntosDelEje;
using Logica;
using tadLayShare.puntos;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Logica.puntosDelEje
{
    
    /// <summary>
    /// EJE TRAZADO TADIL VERSION 5.0
    /// </summary>
  
    [Serializable]
    public class EjeTrazado
    {
        public enum TipoCurva { cnp, cnpAnguloReducido, cp, c7500, c3500, noValorado };
        public enum sentidoCurva { Horario, Antihorario, noValorado };
        public enum tipoSegmento { rectaInt, RectaIntParalelismo, RectaIntCurvaS, giroSalidaCP, giroEntradaCP, quedaFijo, noValorado };
        public enum tipoClotoide { entrada, salida };
        public enum ladoCalzada { Derecha, Izquierda };

        private List<componentes.Componente> mComponentes;
        private List<Vertice> mVertices;
        private bool mPrefCurvas;
        private double mRadio;
        private int mGrupo;
        private double mVelocidad;
        private double mPeralteCurva;
        private double mPeralteRecta;
        private bool mConstante;

        private double mAzimutTransTemp;
        private double mMaxRadio;


        public EjeTrazado(List<Vertice> iLstVertice, List<componentes.Componente> iLstComponente, double iPeralteCurva, double iPeralteRecta)
        {
            mComponentes = iLstComponente;
            mVertices = iLstVertice;

            mPeralteCurva = iPeralteCurva;
            mPeralteRecta = iPeralteRecta;
            mMaxRadio = 0;
            foreach (componentes.Componente miComp in mComponentes)
            {
                if (miComp.getTipoComponente() == componentes.Componente.tipoComponente.curva)
                {
                    Curva miCurva = (Curva)miComp;
                    if (mMaxRadio < miCurva.getRadio) mMaxRadio = miCurva.getRadio;
                }
            }
        }

        public EjeTrazado(List<Punto3d> iPolilinea, int iGrupo, bool iPrefCurvas, double iRadio, double iVelocidad, double iPeralteCurva, double iPeralteRecta, bool iConstante)
        {
            mComponentes = new List<componentes.Componente>();
            mVertices = new List<Vertice>();
            mPrefCurvas = iPrefCurvas;
            mRadio = iRadio;
            mGrupo = iGrupo;
            mVelocidad = iVelocidad;
            mPeralteCurva = iPeralteCurva;
            mPeralteRecta = iPeralteRecta;
            mConstante = iConstante;
            double miA = 0;

            double longitudClotoide = 0;
        
            crearVertices(iPolilinea);

            

            Punto3d miVerticeAnt = mVertices.ElementAt(0).getVertice;
            bool miReducido=false;
            bool isCurvaGranRadio = false;

            Punto3d[] puntosSing = new Punto3d[4];
            Punto3d[] puntosSingCurvaS = new Punto3d[5];
            Punto3d[] puntosSingRectaS = new Punto3d[5];
            Punto3d puntoAuxiliar= new Punto3d(0,0,0);

            bool curvaS = false;
            bool curvaS1 = false, curvaS2 = false;
            double miPk = 0;

            int j=1;
            while(j<mVertices.Count-1)
            {

                Vertice miVertice = mVertices.ElementAt(j);
                var radio = miVertice.getRadioR;


                switch (miVertice.getTipocurva)
                {
                    case TipoCurva.noValorado:
                        break;
                    case TipoCurva.cp:
                        Punto3d miCentro = miVertice.getCentro;
                        puntosSing = addCurvaPaso(miVertice.getRadioR, mVertices.ElementAt(j - 1).getAzimut, miVertice.getAzimut, miVerticeAnt, miCentro, miVertice.getSentCurva);
                        if (curvaS)
                        {
                            puntosSing[0] = puntosSingCurvaS[2];
                            puntosSing[1] = puntosSingCurvaS[3];
                            if (curvaS2)
                            {
                                puntoAuxiliar = puntosSingRectaS[0];
                            }
                        }
                        curvaS = false;
                        break;
                    case TipoCurva.cnp:
                        miReducido = false;
                        radio = miVertice.getRadioR;
                        puntosSing = addCurvaNoPaso(radio, mVertices.ElementAt(j - 1).getAzimut, miVertice.getAzimut, miVerticeAnt, mVertices.ElementAt(j).getVertice, miVertice.getSentCurva, miReducido, miA);
                        miVertice.setCentro = puntosSing[4];
                        if (curvaS)
                        {
                            puntosSing[0] = puntosSingCurvaS[2];
                            puntosSing[1] = puntosSingCurvaS[3];
                            if (curvaS2)
                            {
                                puntoAuxiliar = puntosSingRectaS[0];
                            }
                        }
                        curvaS = false;
                        break;
                    case TipoCurva.cnpAnguloReducido:
                        miReducido = true; //cambiado
                        double AzmAnt = mVertices.ElementAt(j - 1).getAzimut;
                        radio = miVertice.getRadioR;
                        puntosSing = addCurvaNoPaso(radio, mVertices.ElementAt(j - 1).getAzimut, miVertice.getAzimut, miVerticeAnt, mVertices.ElementAt(j).getVertice, miVertice.getSentCurva, miReducido, miA);
                        miVertice.setCentro = puntosSing[4];
                        miVertice.setRadio = radio;
                        if (curvaS)
                        {
                            puntosSing[0] = puntosSingCurvaS[2];
                            puntosSing[1] = puntosSingCurvaS[3];
                            if (curvaS2)
                            {
                                puntoAuxiliar = puntosSingRectaS[0];
                            }
                        }
                        curvaS = false;
                        break;
                    case TipoCurva.c7500:
                        var granRadio7500 = GetGranRadio(TipoCurva.c7500, mVertices.ElementAt(j - 1).getAzimut, miVertice.getAzimut);
                        isCurvaGranRadio = true;
                        puntosSing = addCurvaGranRadio(granRadio7500, mVertices.ElementAt(j - 1).getAzimut, miVertice.getAzimut, miVertice.getVertice, miVertice.getSentCurva);
                        miVertice.setCentro = puntosSing[4];
                        miVertice.setRadio = granRadio7500;
                        if (curvaS)
                        {
                            puntosSing[0] = puntosSingCurvaS[2];
                            puntosSing[1] = puntosSingCurvaS[3];
                            if (curvaS2)
                            {
                                puntoAuxiliar = puntosSingRectaS[0];
                            }
                        }
                        curvaS = false;
                        break;
                    case TipoCurva.c3500:
                        var granRadio3500 = GetGranRadio(TipoCurva.c3500, mVertices.ElementAt(j - 1).getAzimut, miVertice.getAzimut);
                        isCurvaGranRadio = true;
                        puntosSing = addCurvaGranRadio(granRadio3500, mVertices.ElementAt(j - 1).getAzimut, miVertice.getAzimut, miVertice.getVertice, miVertice.getSentCurva);
                        miVertice.setCentro = puntosSing[4];
                        miVertice.setRadio = granRadio3500;
                        if (curvaS)
                        {
                            puntosSing[0] = puntosSingCurvaS[2];
                            puntosSing[1] = puntosSingCurvaS[3];
                            if (curvaS2)
                            {
                                puntoAuxiliar = puntosSingRectaS[1];
                            }
                        }
                        curvaS = false;
                        break;
                }
                if (miVertice.getTipocurva != TipoCurva.noValorado)
                {
                    if (miVertice.getTipoSeg == tipoSegmento.RectaIntCurvaS)
                    {
                        if (mPrefCurvas)
                        {
                            puntosSingRectaS = addCurvaenSRecta(miVertice.getRadioR, mVertices.ElementAt(j + 1).getRadioR, miVertice.getCentro, mVertices.ElementAt(j + 1).getCentro, miVertice.getSentCurva, mVertices.ElementAt(j + 1).getSentCurva);
                            puntosSingCurvaS = addCurvaenS(miVertice.getRadioR, mVertices.ElementAt(j + 1).getRadioR, miVertice.getCentro, mVertices.ElementAt(j + 1).getCentro, miVertice.getSentCurva, mVertices.ElementAt(j + 1).getSentCurva);
                            curvaS1 = true;
                        }
                        else
                        {
                            puntosSingCurvaS = addCurvaenSRecta(miVertice.getRadioR, mVertices.ElementAt(j + 1).getRadioR, miVertice.getCentro, mVertices.ElementAt(j + 1).getCentro, miVertice.getSentCurva, mVertices.ElementAt(j + 1).getSentCurva);
                            curvaS1 = false;
                        }
                        puntosSing[2] = puntosSingCurvaS[0];
                        puntosSing[3] = puntosSingCurvaS[1];

                        mVertices.ElementAt(j).setAzimut = puntosSingCurvaS[4].coordenadaX;
                        curvaS = true;



                    }
                    else if (miVertice.getTipoSeg == tipoSegmento.RectaIntParalelismo)
                    {
                        puntosSingCurvaS = addPararelismo(miVertice.getRadioR, mVertices.ElementAt(j + 1).getRadioR, miVertice.getCentro, mVertices.ElementAt(j + 1).getCentro, miVertice.getSentCurva, mVertices.ElementAt(j + 1).getSentCurva);
                        puntosSing[2] = puntosSingCurvaS[0];
                        puntosSing[3] = puntosSingCurvaS[1];
                        mVertices.ElementAt(j).setAzimut = puntosSingCurvaS[4].coordenadaX;


                        curvaS = true;
                    }
                    else curvaS = false;


                   
                    if (curvaS2)
                    {
                        mComponentes.Add(new Linea(miVerticeAnt, puntoAuxiliar, puntosSing[0], miPk, mPeralteRecta, mVertices.ElementAt(j - 1).getAzimut));
                            miPk += mComponentes.ElementAt(mComponentes.Count - 1).getLongitud();
                            mComponentes.Add(new componentes.Clotoide(puntosSing[0], puntosSing[1], miVertice.getRadioR, miPk, miVertice.getSentCurva, mPeralteRecta, mPeralteCurva, true, tipoClotoide.entrada, mVertices.ElementAt(j - 1).getAzimut, miReducido, miVertice.getDelta, curvaS2, longitudClotoide, miA));

                            miPk += mComponentes.ElementAt(mComponentes.Count - 1).getLongitud();
                        curvaS2 = false;
                    }
                    else
                    {
                        mComponentes.Add(new Linea(miVerticeAnt, puntosSing[0], miPk, mPeralteRecta, mVertices.ElementAt(j - 1).getAzimut));
                            miPk += mComponentes.ElementAt(mComponentes.Count - 1).getLongitud();
                            mComponentes.Add(new componentes.Clotoide(puntosSing[0], puntosSing[1], miVertice.getRadioR, miPk, miVertice.getSentCurva, mPeralteRecta, mPeralteCurva, true, Logica.puntosDelEje.EjeTrazado.tipoClotoide.entrada, mVertices.ElementAt(j - 1).getAzimut, miReducido, miVertice.getDelta, curvaS2, 0, miA));//numero por miA
                        

                            miPk += mComponentes.ElementAt(mComponentes.Count - 1).getLongitud();
                            curvaS2 = false;
                    }

                    if (isCurvaGranRadio)
                    {
                        mComponentes.Add(new Curva(puntosSing[1], puntosSing[2], puntosSing[4], miVertice.getRadioR, miPk, mPeralteCurva, mPeralteRecta, miVertice.getSentCurva, mComponentes.Last().getLongitud()));
                        isCurvaGranRadio = false;
                    }
                    else
                    {
                        mComponentes.Add(new Curva(puntosSing[1], puntosSing[2], puntosSing[4], miVertice.getRadioR, miPk, mPeralteCurva, mPeralteRecta, miVertice.getSentCurva, mComponentes.Last().getLongitud()));
                    }
                    miPk += mComponentes.ElementAt(mComponentes.Count - 1).getLongitud();

                    double[] miPuntoMedio = this.getPointAtDist(mComponentes.ElementAt(mComponentes.Count - 1).getPkIni + (mComponentes.ElementAt(mComponentes.Count - 1).getPkFinal() - mComponentes.ElementAt(mComponentes.Count - 1).getPkIni) / 2);

                    if (curvaS1)
                    {
                        mComponentes.Add(new Clotoide(puntosSing[2], puntosSing[3], miVertice.getRadioR, miPk, miVertice.getSentCurva, mPeralteCurva, mPeralteRecta, false, tipoClotoide.salida, mVertices.ElementAt(j).getAzimut, miReducido, miVertice.getDelta, curvaS1, puntosSingCurvaS[4].coordenadaY, miA));
                        longitudClotoide = puntosSingCurvaS[4].coordenadaZ;
                        miPk += mComponentes.ElementAt(mComponentes.Count - 1).getLongitud();
                        curvaS2 = true;
                        curvaS = true;
                        curvaS1 = false;
                    }
                    else
                    {
                        mComponentes.Add(new Clotoide(puntosSing[2], puntosSing[3], miVertice.getRadioR, miPk, miVertice.getSentCurva, mPeralteCurva, mPeralteRecta, false, tipoClotoide.salida, mVertices.ElementAt(j).getAzimut, miReducido, miVertice.getDelta, curvaS1, 0, miA));

                        miPk += mComponentes.ElementAt(mComponentes.Count - 1).getLongitud();
                        curvaS1 = false;
                        curvaS2 = false;
                    }


                    miVerticeAnt = puntosSing[3];

                    //MODIFICACION

                    Clotoide miClo1 = (Clotoide) mComponentes.ElementAt(mComponentes.Count-1);
                    double miQe1 = miClo1.getQe();

                    
                    Clotoide miClo2 = (Clotoide) mComponentes.ElementAt(mComponentes.Count-3);
                    double miQe2 = miClo2.getQe();

                    if ((miVertice.getTipocurva == TipoCurva.cp) && (isSolapeS(puntosSing[4], puntosSing[1], puntosSing[2], new Punto3d(miPuntoMedio[0], miPuntoMedio[1], 0), miQe1, miQe2)))
                    {
                        miVertice.setRadio = miVertice.getRadioR * 110 / 100;
                        miVerticeAnt = mVertices.ElementAt(0).getVertice;
                        mComponentes = new List<Logica.componentes.Componente>();
                        j = 0;

                        curvaS = false;
                        curvaS1 = false;
                        curvaS2 = false;
                        miPk = 0;


                    }
                    //MODIFICACION



                }
                miA = 0;
                j++;
                miReducido = false;

            }
            mComponentes.Add(new Linea(miVerticeAnt, mVertices.ElementAt(mVertices.Count - 1).getVertice, miPk, mPeralteRecta, mVertices.ElementAt(mVertices.Count - 2).getAzimut));
            calculaMaxRadio();
        }

        private double GetGranRadio(TipoCurva tipoCurva, double azimutAnterior, double azimutAct)
        {
            var resultRadio = 0.0;
            double miAngulo = Math.Abs(azimutAnterior - azimutAct);

            if (miAngulo > 180)
            {
                miAngulo = 360 - miAngulo;
            }

            var param = (325 - (27.7777*miAngulo))*(57.2957/miAngulo);
            if (tipoCurva == EjeTrazado.TipoCurva.c3500)
            {
                resultRadio = 3500;
                if (param > resultRadio) resultRadio = param;
            }
            if (tipoCurva == EjeTrazado.TipoCurva.c7500)
            {
                resultRadio = 7500;
                if (param > resultRadio) resultRadio = param;
            }
            return resultRadio;
        }

        #region "Métodos privados"

        public static bool isSolapeS(Punto3d iCentroCurva, Punto3d iPuntoEntrada, Punto3d iPuntoSalida, Punto3d iVertice, double iQe1, double iQe2)
        {
            bool resultado = false;
            double miAzPE = getAzimutCardinal(iPuntoEntrada.coordenadaX - iCentroCurva.coordenadaX, iPuntoEntrada.coordenadaY - iCentroCurva.coordenadaY);
            double miAzM = getAzimutCardinal(iVertice.coordenadaX - iCentroCurva.coordenadaX, iVertice.coordenadaY - iCentroCurva.coordenadaY);
            double miAzPS = getAzimutCardinal(iPuntoSalida.coordenadaX - iCentroCurva.coordenadaX, iPuntoSalida.coordenadaY - iCentroCurva.coordenadaY);

            double giro1, giro2;

            if (Math.Abs(miAzPE - miAzM) > 180)
            {
                giro1 = 360 - Math.Abs(miAzPE - miAzM);
            }
            else
            {
                giro1 = Math.Abs(miAzPE - miAzM);
            }

            if (Math.Abs(miAzM - miAzPS) > 180)
            {
                giro2 = 360 - Math.Abs(miAzM - miAzPS);
            }
            else
            {
                giro2 = Math.Abs(miAzM - miAzPS);
            }

            double giroClo1 = iQe1 * 180 / Math.PI;
            double giroClo2 = iQe2 * 180 / Math.PI;

            double total = giroClo1 + giroClo2 + giro1 + giro2;
          

            resultado = (total>360);

            return resultado;
        }

        private void crearVertices(List<Punto3d> iPolilinea)
        {
            Punto3d miVertice = null;
            double miAzAnt = 0;
            TipoCurva miTipoCurvaAnt = TipoCurva.noValorado;
            sentidoCurva miSentCAnt = sentidoCurva.noValorado;
            int i = 0;
            foreach (Punto3d miPuntoPos in iPolilinea)
            {
                if (i == 1)
                {
                    miAzAnt = getAzimut(miVertice, miPuntoPos);
                    Punto3d miCentro = new Punto3d(0, 0, 0);
                    mVertices.Add(new Vertice(miVertice, miAzAnt, sentidoCurva.noValorado, 0, TipoCurva.noValorado, getTipoSegmento(TipoCurva.noValorado, TipoCurva.noValorado, false), 0, miCentro));

                }
                else if (i > 1)
                {
                    double miAz = getAzimut(miVertice, miPuntoPos);
                    double miDelta = getDelta(miAzAnt, miAz);
                    TipoCurva miTipoCurva = getTipoCurva(miDelta, mPrefCurvas, mGrupo, mRadio);
                    sentidoCurva miSentC = getSentidoCurva(miAzAnt, getAzimut(miVertice, miPuntoPos));
                    double miRadioR = getRadioReducido(miTipoCurva, miDelta);
                    Punto3d miCentro = new Punto3d(0,0,0);
                    if (miTipoCurva == TipoCurva.cp)
                    {
                        miCentro = getCentroCP(miSentC, miVertice, miAz, miDelta, miRadioR);
                    }
                    mVertices.Add(new Vertice(miVertice, miAz, miSentC, miRadioR, miTipoCurva, tipoSegmento.noValorado, miDelta, miCentro));
                    miAzAnt = getAzimut(miVertice, miPuntoPos);
                    miTipoCurvaAnt = miTipoCurva;
                    miSentCAnt = miSentC;

                }
                miVertice = miPuntoPos;

                i++;

            }

            Punto3d miCentro1 = new Punto3d(0, 0, 0);
            mVertices.Add(new Vertice(iPolilinea.ElementAt(iPolilinea.Count - 1), 0, sentidoCurva.noValorado, 0, TipoCurva.noValorado, tipoSegmento.noValorado, 0, miCentro1));

            for (int j = 0; j < mVertices.Count - 1; j++)
            {
                    tipoSegmento miTipoS = getTipoSegmento(mVertices.ElementAt(j).getTipocurva, mVertices.ElementAt(j + 1).getTipocurva, isParalelismo(mVertices.ElementAt(j).getSentCurva, mVertices.ElementAt(j + 1).getSentCurva));
                    mVertices.ElementAt(j).setTipoSeg = miTipoS;
                    double miAzfinal = getAzimutFinal(miTipoS, mVertices.ElementAt(j).getAzimut, mVertices.ElementAt(j).getVertice, mVertices.ElementAt(j).getSentCurva,
                       mVertices.ElementAt(j).getDelta, mVertices.ElementAt(j + 1).getVertice, mVertices.ElementAt(j + 1).getSentCurva, mVertices.ElementAt(j + 1).getDelta, mVertices.ElementAt(j).getRadioR, mVertices.ElementAt(j + 1).getRadioR);
                    mVertices.ElementAt(j).setAzimut = miAzfinal;
                
            }

            for (int j = 1; j < mVertices.Count - 1; j++)
            {
                TipoCurva miTipoCurva = mVertices.ElementAt(j).getTipocurva;
                miAzAnt = mVertices.ElementAt(j - 1).getAzimut;
                double miAz = mVertices.ElementAt(j).getAzimut;

                double miDelta = getDelta(miAzAnt, miAz);
                mVertices.ElementAt(j).setDelta = miDelta;
                if (getTipoCurva(miDelta, mPrefCurvas, mGrupo, mRadio)!= TipoCurva.cp)
                {
                    miTipoCurva = getTipoCurva(miDelta, mPrefCurvas, mGrupo, mRadio);
                    sentidoCurva miSentC = getSentidoCurva(miAzAnt, miAz);
                    double miRadioR = getRadioReducido(miTipoCurva, miDelta);
                    mVertices.ElementAt(j).setSentidoCurva = miSentC;
                    mVertices.ElementAt(j).setTipoCurva = miTipoCurva;
                    mVertices.ElementAt(j).setRadio = miRadioR;
                }
            }
            //mVertices.ElementAt(23).setRadio = mVertices.ElementAt(23).getRadioR * 110 / 100;
            
        }

        private bool isParalelismo(sentidoCurva iSentCAnt, sentidoCurva iSentC)
        {
            bool miPara = false;
            if ((iSentC == sentidoCurva.Antihorario) && (iSentCAnt == sentidoCurva.Antihorario)) miPara = true;
            if ((iSentC == sentidoCurva.Horario) && (iSentCAnt == sentidoCurva.Horario)) miPara = true;
            return miPara;

        }

        private double getAzimut(Punto3d iPunto1, Punto3d iPunto2)
        {
            double miAzimut;
            
            if (iPunto2.coordenadaX > iPunto1.coordenadaX)
            {
                if (iPunto2.coordenadaY > iPunto1.coordenadaY)
                {
                    miAzimut = 90 - 180 / Math.PI * Math.Atan((iPunto2.coordenadaY - iPunto1.coordenadaY) / (iPunto2.coordenadaX - iPunto1.coordenadaX));
                }
                else
                {
                    miAzimut = 90 + 180 / Math.PI * Math.Atan((iPunto1.coordenadaY - iPunto2.coordenadaY) / (iPunto2.coordenadaX - iPunto1.coordenadaX));
                }
            }
            else
            {
                if (iPunto2.coordenadaY > iPunto1.coordenadaY)
                {
                    miAzimut = 270 + 180 / Math.PI * Math.Atan((iPunto2.coordenadaY - iPunto1.coordenadaY) / (iPunto1.coordenadaX - iPunto2.coordenadaX));
                }
                else
                {
                    miAzimut = 270 - 180 / Math.PI * Math.Atan((iPunto1.coordenadaY - iPunto2.coordenadaY) / (iPunto1.coordenadaX - iPunto2.coordenadaX));
                }
            }
            return miAzimut;
        }

        private double getAzimutFinal(tipoSegmento iTipoS, double iAzimut, Punto3d iPunto1, sentidoCurva iSentC1, double iDelta1, Punto3d iPunto2, sentidoCurva iSentC2, double iDelta2, double iRadio1, double iRadio2)
        {
            double miAzFinal=iAzimut;
            if ((iTipoS == tipoSegmento.giroEntradaCP) || (iTipoS == tipoSegmento.giroSalidaCP))
            {
                bool entrada;
                sentidoCurva miSentC;
                double miDelta;
                double miRadio;
                if (iTipoS == tipoSegmento.giroEntradaCP)
                {
                    entrada = true;
                    miSentC = iSentC2;
                    miDelta = iDelta2;
                    miRadio = iRadio2;
                }
                else
                {
                    entrada = false;
                    miSentC = iSentC1;
                    miDelta = iDelta1;
                    miRadio = iRadio1;
                }
                miAzFinal = sentidoGiro(miRadio, iPunto1, iPunto2, miSentC, iAzimut, 180 - miDelta, entrada);
            }
            if (miAzFinal > 360)
            {
                miAzFinal = miAzFinal - 360;
            }
            return miAzFinal;
        }

        private tipoSegmento getTipoSegmento(TipoCurva iTipoCAnt, TipoCurva iTipoC, bool iParalelismo)
        {
            tipoSegmento miTipoS;
            if (iTipoCAnt == TipoCurva.cp)
            {
                if (iTipoC == TipoCurva.cp)
                {
                    miTipoS = tipoSegmento.rectaInt;
                }
                else
                {
                    miTipoS = tipoSegmento.giroSalidaCP;
                }
            }
            else
            {
                if (iTipoC == TipoCurva.cp)
                {
                    miTipoS = tipoSegmento.giroEntradaCP;
                }
                else
                {
                    miTipoS = tipoSegmento.quedaFijo;
                }
            }
            if (miTipoS == tipoSegmento.rectaInt)
            {
                if (iParalelismo)
                {
                    miTipoS = tipoSegmento.RectaIntParalelismo;
                }
                else miTipoS = tipoSegmento.RectaIntCurvaS;
            }

            return miTipoS;
        }

        public static double getDelta(double iAzimut1, double iAzimut2)
        {
            double miDelta;
            if (Math.Abs(iAzimut2 - iAzimut1) > 180)
            {
                miDelta = 360 - Math.Abs(iAzimut2 - iAzimut1);
            }
            else
            {
                miDelta = Math.Abs(iAzimut2 - iAzimut1);
            }
            return miDelta;
        }

        public static TipoCurva getTipoCurva(double iDelta, bool iPrefCruvas, int iGrupo, double iRadio)
        {
            TipoCurva mitipo;
            double miPhi = 180 - iDelta;
            if (!iPrefCruvas)
            {
                if (miPhi > 100)
                {
                    if (miPhi > 174.6)
                    {
                        if (iGrupo == 1)
                        {
                            mitipo = TipoCurva.c7500;
                        }
                        else
                        {
                            mitipo = TipoCurva.c3500;
                        }
                    }
                    else
                    {
                        double param = (Math.Pow(12, 0.5)*Math.Pow(iRadio, -0.5))*180/Math.PI;
                        if (iDelta < param)
                        {
                            mitipo = TipoCurva.cnpAnguloReducido;
                        }
                        else
                        {
                            mitipo = TipoCurva.cnp;
                        }
                    }
                }
                else
                {
                    mitipo = TipoCurva.cp;
                }
            }
            else
            {
                if (miPhi > 120)
                {
                    if (miPhi > 174.6)
                    {
                        if (iGrupo == 1)
                        {
                            mitipo = TipoCurva.c7500;

                        }
                        else
                        {
                            mitipo = TipoCurva.c3500;
                        }

                    }
                    else
                    {
                        double param = (Math.Pow(12, 0.5) * Math.Pow(iRadio, -0.5)) * 180 / Math.PI;
                        if (iDelta < param)
                        {
                            mitipo = TipoCurva.cnpAnguloReducido;
                        }
                        else
                        {
                            mitipo = TipoCurva.cnp;
                        }
                    }
                }
                else
                {
                    mitipo = TipoCurva.cp;
                }
            }
            return mitipo;
        }

        private double getRadioReducido(TipoCurva iTipo, double iDelta)
        {
            double miRadio = 0;
            double miPhi = 180 - iDelta;
            
            //Modificacion quitar mConstante
            //if ((mConstante) && (iTipo == tipoCurva.cp))
            if ((iTipo == TipoCurva.cp))
            {
                switch (iTipo)
                {
                    case TipoCurva.c3500:
                        miRadio = mRadio;
                        break;
                    case TipoCurva.c7500:
                        miRadio = mRadio;
                        break;
                    case TipoCurva.cnp:
                        miRadio = mRadio;
                        break;
                    case TipoCurva.cp:
                        if (miPhi > 83)
                        {
                            miRadio = mRadio;
                        }
                        else
                        {
                            if (miPhi > 40.7)
                            {
                                miRadio = 0.83 * mRadio;
                            }
                            else
                            {
                                miRadio = 0.72 * mRadio;
                            }
                        }
                        break;
                }
            }
            else
            {
                miRadio = mRadio;
            }
                
            return miRadio;
        }

        public static sentidoCurva getSentidoCurva(double iAzSegAnt, double iAz)
        {
            if (iAzSegAnt < 0) iAzSegAnt = iAzSegAnt + 360;
            if (iAz < 0) iAz = iAz + 360;
            sentidoCurva miSent;
            if ((iAzSegAnt >= 0) && (iAzSegAnt <= 180))
            {
                if (((iAzSegAnt - iAz) < 0) && (Math.Abs(iAzSegAnt - iAz) < 180))
                {
                    miSent = sentidoCurva.Horario;
                }
                else 
                {
                    miSent = sentidoCurva.Antihorario;
                }
            }
            else
            {
                if (((iAzSegAnt - iAz) > 0) && (Math.Abs(iAzSegAnt - iAz) < 180))
                {
                    miSent = sentidoCurva.Antihorario;
                }
                else
                {
                    miSent = sentidoCurva.Horario;
                }
            }


            return miSent;
        }

        private Punto3d getCentroCP(sentidoCurva iSentidoC, Punto3d iPuntoInicio,double iAzimut, double iDelta, double iRadioReducido)
        {
            double miPuntox, miPuntoy;
            double miPhi = 180 - iDelta;
            if(iSentidoC==sentidoCurva.Horario)
            {
                miPuntox = iPuntoInicio.coordenadaX + iRadioReducido * Math.Sin((iAzimut + miPhi / 2) * Math.PI / 180);
                miPuntoy = iPuntoInicio.coordenadaY + iRadioReducido * Math.Cos((iAzimut + miPhi / 2) * Math.PI / 180);
            }
            else
            {
                miPuntox = iPuntoInicio.coordenadaX + iRadioReducido * Math.Sin((iAzimut - miPhi / 2) * Math.PI / 180);
                miPuntoy = iPuntoInicio.coordenadaY + iRadioReducido * Math.Cos((iAzimut - miPhi / 2) * Math.PI / 180);
            }
            return new Punto3d(miPuntox, miPuntoy, 0);
        }

        public static double getAzimutCardinal(double iDx, double iDy)
        {

            double miDelta;
            if ((iDx == 0) || (iDy == 0))
            {
                miDelta = 0;
            }
            else
            {
                miDelta = Math.Atan(iDx / iDy);
            }
            double miDeltaGra = miDelta * 180 / Math.PI;
            double miAzimut;

            if (miDeltaGra == 0)
            {
                if (iDy == 0)
                {
                    if (iDx < 0)
                    {
                        miAzimut = 180;
                    }
                    else
                    {
                        miAzimut = 0;
                    }
                }
                else
                {
                    if (iDy < 0)
                    {
                        miAzimut = 270;
                    }
                    else
                    {
                        miAzimut = 90;
                    }
                }
            }
            else
            {
                if (miDeltaGra < 0)
                {
                    if (iDx >= 0)
                    {
                        miAzimut = 90 - miDeltaGra;
                    }
                    else
                    {
                        miAzimut = 270 - miDeltaGra;
                    }
                }
                else
                {
                    if (iDx >= 0)
                    {
                        miAzimut = 270 - miDeltaGra;
                    }
                    else
                    {
                        miAzimut = 90 - miDeltaGra;
                    }
                }
            }
            return miAzimut;
        }

        public double sentidoGiro(double iRc1, Punto3d iPuntoC1, Punto3d iPuntoC2, sentidoCurva sentG1, double iAz1, double iPhi, bool entrada)
        {
            double miA = Math.Pow((12 * Math.Pow(iRc1, 3)), 0.25);
            double miLe = Math.Pow(miA, 2) / iRc1;
            double miQe = miLe / (2 * iRc1);
            double miYe = ((miQe / 3) - (Math.Pow(miQe, 3) / 42) + (Math.Pow(miQe, 5) / 1320) - (Math.Pow(miQe, 7) / 75600)) * miLe;
            double miDR = miYe - iRc1 * (1 - Math.Cos(miQe));
            double miXe = (1 - Math.Pow(miQe, 2) / 10 + Math.Pow(miQe, 4) / 216 - Math.Pow(miQe, 6) / 9360 + Math.Pow(miQe, 8) / 685440) * miLe;
            double miXM = miXe - iRc1 * Math.Sin(miQe);

            double miL1 = Math.Sqrt(Math.Pow((iPuntoC2.coordenadaX - iPuntoC1.coordenadaX), 2) + Math.Pow((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY), 2));
            double miD1 = Math.Sqrt(Math.Pow((iRc1 * Math.Sin(iPhi / 2 * Math.PI / 180)), 2) + Math.Pow((miL1 - iRc1 * Math.Cos(iPhi / 2 * Math.PI / 180)), 2));
            double miTheta1 = Math.Asin((iRc1 + miDR) / miD1);
            double miPsi1 = Math.Atan((iRc1 * Math.Sin(iPhi / 2 * Math.PI / 180)) / (miL1 - iRc1 * Math.Cos(iPhi / 2 * Math.PI / 180)));
            double miGiro = miTheta1 - miPsi1;

            double miAz;
            if (sentG1 == sentidoCurva.Horario)
            {
                if (entrada)
                {
                    miAz = ((iAz1 * Math.PI / 180) - miGiro) * 180 / Math.PI;
                }
                else
                {
                    miAz = ((iAz1 * Math.PI / 180) + miGiro) * 180 / Math.PI;
                }
            }
            else
            {
                if (entrada)
                {
                    miAz = ((iAz1 * Math.PI / 180) + miGiro) * 180 / Math.PI;
                }
                else
                {
                    miAz = ((iAz1 * Math.PI / 180) - miGiro) * 180 / Math.PI;
                }
            }
            return miAz;

        }



        #endregion


        #region "add elementos"
        public Punto3d[] addCurvaNoPaso( double miRc, double iAzimut1, double iAzimut2, Punto3d iVerticeAnt, Punto3d iVertice, sentidoCurva sentG, bool iReducido,  double miA)
        {
            double miDelta;
            miDelta = getDelta(iAzimut1, iAzimut2);
            double miDeltaRad = miDelta * Math.PI / 180;
            var miLe = CalculoParametrosCurva(ref miRc, miDeltaRad, iReducido, out miA, mGrupo, mVelocidad);


            double miQe = miLe / (2 * miRc);
            double miYe = ((miQe / 3) - (Math.Pow(miQe, 3) / 42) + (Math.Pow(miQe, 5) / 1320) - (Math.Pow(miQe,7) / 75600)) * miLe;
            double miDR = miYe - miRc * (1 - Math.Cos(miQe));
            double miXe = (1 - Math.Pow(miQe, 2) / 10 + Math.Pow(miQe, 4) / 216 - Math.Pow(miQe, 6) / 9360 + Math.Pow(miQe, 8) / 685440) * miLe;
            double miXM = miXe - miRc * Math.Sin(miQe);

            double miPhi = 180 - miDelta;
            double miTe = miXM + (miRc + miDR) * Math.Tan(miDelta * Math.PI / 180 / 2);
            double miEe = ((miRc + miDR) / Math.Abs(Math.Cos(miDelta / 2 * Math.PI / 180))) - miRc;
            double miPcx, miPcy, miPx1x, miPx1y, miPx2x, miPx2y;
            double miPcl1x, miPcl1y, miPc1x, miPc1y, miPc2x, miPc2y, miPcl2x, miPcl2y;
            if (sentG == sentidoCurva.Horario)
            {
                miPcx = iVertice.coordenadaX + (miEe + miRc) * Math.Sin((iAzimut2 + miPhi / 2) * Math.PI / 180);
                miPcy = iVertice.coordenadaY + (miEe + miRc) * Math.Cos((iAzimut2 + miPhi / 2) * Math.PI / 180);
                miPx1x = miPcx + (miRc + miDR) * Math.Sin((iAzimut1 - 90) * Math.PI / 180);
                miPx1y = miPcy + (miRc + miDR) * Math.Cos((iAzimut1 - 90) * Math.PI / 180);
                miPx2x = miPcx + (miRc + miDR) * Math.Sin((iAzimut2 - 90) * Math.PI / 180);
                miPx2y = miPcy + (miRc + miDR) * Math.Cos((iAzimut2 - 90) * Math.PI / 180);

                miPcl1x = miPx1x + miXM * Math.Sin((iAzimut1 - 180) * Math.PI / 180);
                miPcl1y = miPx1y + miXM * Math.Cos((iAzimut1 - 180) * Math.PI / 180);

                miPc1x = miPcx + miRc * Math.Sin((iAzimut1 - 90 + miQe * 180 / Math.PI) * Math.PI / 180);
                miPc1y = miPcy + miRc * Math.Cos((iAzimut1 - 90 + miQe * 180 / Math.PI) * Math.PI / 180);

                miPc2x = miPcx + miRc * Math.Sin((iAzimut2 - 90 - miQe * 180 / Math.PI) * Math.PI / 180);
                miPc2y = miPcy + miRc * Math.Cos((iAzimut2 - 90 - miQe * 180 / Math.PI) * Math.PI / 180);

                miPcl2x = miPx2x + miXM * Math.Sin(iAzimut2 * Math.PI / 180);
                miPcl2y = miPx2y + miXM * Math.Cos(iAzimut2 * Math.PI / 180);

            }
            else
            {
                miPcx = iVertice.coordenadaX + (miEe + miRc) * Math.Sin((iAzimut2 - miPhi / 2) * Math.PI / 180);
                miPcy = iVertice.coordenadaY + (miEe + miRc) * Math.Cos((iAzimut2 - miPhi / 2) * Math.PI / 180);
                miPx1x = miPcx + (miRc + miDR) * Math.Sin((iAzimut1 + 90) * Math.PI / 180);
                miPx1y = miPcy + (miRc + miDR) * Math.Cos((iAzimut1 + 90) * Math.PI / 180);
                miPx2x = miPcx + (miRc + miDR) * Math.Sin((iAzimut2 + 90) * Math.PI / 180);
                miPx2y = miPcy + (miRc + miDR) * Math.Cos((iAzimut2 + 90) * Math.PI / 180);


                miPcl1x = miPx1x + miXM * Math.Sin((iAzimut1 - 180) * Math.PI / 180);
                miPcl1y = miPx1y + miXM * Math.Cos((iAzimut1 - 180) * Math.PI / 180);

                miPc1x = miPcx + miRc * Math.Sin((iAzimut1 + 90 - miQe * 180 / Math.PI) * Math.PI / 180);
                miPc1y = miPcy + miRc * Math.Cos((iAzimut1 + 90 - miQe * 180 / Math.PI) * Math.PI / 180);

                miPc2x = miPcx + miRc * Math.Sin((iAzimut2 + 90 + miQe * 180 / Math.PI) * Math.PI / 180);
                miPc2y = miPcy + miRc * Math.Cos((iAzimut2 + 90 + miQe * 180 / Math.PI) * Math.PI / 180);

                miPcl2x = miPx2x + miXM * Math.Sin(iAzimut2 * Math.PI / 180);
                miPcl2y = miPx2y + miXM * Math.Cos(iAzimut2 * Math.PI / 180);
            }

            Punto3d miPuntoC = new Punto3d(miPcx, miPcy, 0);
            Punto3d miPunto1 = new Punto3d(miPcl1x, miPcl1y, 0);
            Punto3d miPunto2 = new Punto3d(miPc1x, miPc1y, 0);
            Punto3d miPunto3 = new Punto3d(miPc2x, miPc2y, 0);  
            Punto3d miPunto4 = new Punto3d(miPcl2x, miPcl2y, 0);

            Punto3d[] puntosSing = new Punto3d[5];
            puntosSing[0] = miPunto1;
            puntosSing[1] = miPunto2;
            puntosSing[2] = miPunto3;
            puntosSing[3] = miPunto4;
            puntosSing[4] = miPuntoC;


            return puntosSing;
        }

        public static double CalculoParametrosCurva(ref double miRc, double miDeltaRad, bool iReducido, out double miA,
             int iGrupo, double iVp)
        {
            double miLe;
            if (iReducido)
            {
                miRc = 1.5*miRc;
                double miLmin;
                miA = Math.Pow((12*Math.Pow(miRc, 3)), 0.25);
                miLe = Math.Pow(miA, 2)/miRc;
                var param = Math.Pow(12, 0.5)*Math.Pow(miRc, -0.5);
                if (iGrupo == 1)
                {
                    miLmin = (10*3.5)/(1.8 - 0.01* iVp);
                }
                else
                {
                    miLmin = (9*3.5)/(1.8 - 0.01* iVp);
                }
                if (param < miDeltaRad)
                {
                    if (!(miLe > miLmin && miLe > 20))
                    {
                        miRc = 20/(miDeltaRad - 0.060999257);
                        miA = Math.Sqrt(Math.Pow(20, 2)/(miDeltaRad - 0.060999257));
                    }
                }
                else
                {
                    if (miLmin > 20)
                    {
                        miRc = miLmin/(miDeltaRad - 0.060999257);
                        miA = Math.Sqrt(Math.Pow(miLmin, 2)/(miDeltaRad - 0.060999257));
                    }
                    else
                    {
                        miRc = 20/(miDeltaRad - 0.060999257);
                        miA = Math.Sqrt(Math.Pow(20, 2)/(miDeltaRad - 0.060999257));
                    }
                }
                miLe = Math.Pow(miA, 2)/miRc;
            }
            else
            {
                miA = Math.Pow((12*Math.Pow(miRc, 3)), 0.25);
                miLe = Math.Pow(miA, 2)/miRc;


            }

            return miLe;
        }

        public Punto3d[] addCurvaPaso(double iRc, double iAzimut1, double iAzimut2, Punto3d iVerticeAnt, Punto3d iPuntoC, sentidoCurva sentG)
        {
            double miA = Math.Pow((12 * Math.Pow(iRc, 3)), 0.25);
            double miLe = Math.Pow(miA, 2) / iRc;
            double miQe = miLe / (2 * iRc);
            double miYe = ((miQe / 3) - (Math.Pow(miQe, 3) / 42) + (Math.Pow(miQe, 5) / 1320) - (Math.Pow(miQe, 7) / 75600)) * miLe;
            double miDR = miYe - iRc * (1 - Math.Cos(miQe));
            double miXe = (1 - Math.Pow(miQe, 2) / 10 + Math.Pow(miQe, 4) / 216 - Math.Pow(miQe, 6) / 9360 + Math.Pow(miQe, 8) / 685440) * miLe;
            double miXM = miXe - iRc * Math.Sin(miQe);

            double miPcx, miPcy, miPx1x, miPx1y, miPx2x, miPx2y;
            double miPcl1x, miPcl1y, miPc1x, miPc1y, miPc2x, miPc2y, miPcl2x, miPcl2y;
            miPcx = iPuntoC.coordenadaX;
            miPcy = iPuntoC.coordenadaY;
            if (sentG == sentidoCurva.Horario)
            {

                miPx1x = miPcx + (iRc + miDR) * Math.Sin((iAzimut1 - 90) * Math.PI / 180);
                miPx1y = miPcy + (iRc + miDR) * Math.Cos((iAzimut1 - 90) * Math.PI / 180);
                miPx2x = miPcx + (iRc + miDR) * Math.Sin((iAzimut2 - 90) * Math.PI / 180);
                miPx2y = miPcy + (iRc + miDR) * Math.Cos((iAzimut2 - 90) * Math.PI / 180);

                miPcl1x = miPx1x + miXM * Math.Sin((iAzimut1 - 180) * Math.PI / 180);
                miPcl1y = miPx1y + miXM * Math.Cos((iAzimut1 - 180) * Math.PI / 180);

                miPc1x = miPcx + iRc * Math.Sin((iAzimut1 - 90 + miQe * 180 / Math.PI) * Math.PI / 180);
                miPc1y = miPcy + iRc * Math.Cos((iAzimut1 - 90 + miQe * 180 / Math.PI) * Math.PI / 180);

                miPc2x = miPcx + iRc * Math.Sin((iAzimut2 - 90 - miQe * 180 / Math.PI) * Math.PI / 180);
                miPc2y = miPcy + iRc * Math.Cos((iAzimut2 - 90 - miQe * 180 / Math.PI) * Math.PI / 180);

                miPcl2x = miPx2x + miXM * Math.Sin(iAzimut2 * Math.PI / 180);
                miPcl2y = miPx2y + miXM * Math.Cos(iAzimut2 * Math.PI / 180);

            }
            else
            {

                miPx1x = miPcx + (iRc + miDR) * Math.Sin((iAzimut1 + 90) * Math.PI / 180);
                miPx1y = miPcy + (iRc + miDR) * Math.Cos((iAzimut1 + 90) * Math.PI / 180);
                miPx2x = miPcx + (iRc + miDR) * Math.Sin((iAzimut2 + 90) * Math.PI / 180);
                miPx2y = miPcy + (iRc + miDR) * Math.Cos((iAzimut2 + 90) * Math.PI / 180);


                miPcl1x = miPx1x + miXM * Math.Sin((iAzimut1 - 180) * Math.PI / 180);
                miPcl1y = miPx1y + miXM * Math.Cos((iAzimut1 - 180) * Math.PI / 180);

                miPc1x = miPcx + iRc * Math.Sin((iAzimut1 + 90 - miQe * 180 / Math.PI) * Math.PI / 180);
                miPc1y = miPcy + iRc * Math.Cos((iAzimut1 + 90 - miQe * 180 / Math.PI) * Math.PI / 180);

                miPc2x = miPcx + iRc * Math.Sin((iAzimut2 + 90 + miQe * 180 / Math.PI) * Math.PI / 180);
                miPc2y = miPcy + iRc * Math.Cos((iAzimut2 + 90 + miQe * 180 / Math.PI) * Math.PI / 180);

                miPcl2x = miPx2x + miXM * Math.Sin(iAzimut2 * Math.PI / 180);
                miPcl2y = miPx2y + miXM * Math.Cos(iAzimut2 * Math.PI / 180);
            }

            Punto3d miPunto1 = new Punto3d(miPcl1x, miPcl1y, 0);
            Punto3d miPunto2 = new Punto3d(miPc1x, miPc1y, 0);
            Punto3d miPunto3 = new Punto3d(miPc2x, miPc2y, 0);
            Punto3d miPunto4 = new Punto3d(miPcl2x, miPcl2y, 0);


            Punto3d[] puntosSing = new Punto3d[5];
            puntosSing[0] = miPunto1;
            puntosSing[1] = miPunto2;
            puntosSing[2] = miPunto3;
            puntosSing[3] = miPunto4;
            puntosSing[4] = iPuntoC;


            return puntosSing;

        }

        public Punto3d[] addCurvaenSRecta(double iRc1, double iRc2, Punto3d iPuntoC1, Punto3d iPuntoC2, sentidoCurva sentG1, sentidoCurva sentG2)
        {
            double miA1 = Math.Pow((12 * Math.Pow(iRc1, 3)), 0.25);
            double miLe1 = Math.Pow(miA1, 2) / iRc1;
            double miQe1 = miLe1 / (2 * iRc1);
            double miYe1 = ((miQe1 / 3) - (Math.Pow(miQe1, 3) / 42) + (Math.Pow(miQe1, 5) / 1320) - (Math.Pow(miQe1, 7) / 75600)) * miLe1;
            double miDR1 = miYe1 - iRc1 * (1 - Math.Cos(miQe1));
            double miXe1 = (1 - Math.Pow(miQe1, 2) / 10 + Math.Pow(miQe1, 4) / 216 - Math.Pow(miQe1, 6) / 9360 + Math.Pow(miQe1, 8) / 685440) * miLe1;
            double miXM1 = miXe1 - iRc1 * Math.Sin(miQe1);

            double miA2 = Math.Pow((12 * Math.Pow(iRc2, 3)), 0.25);
            double miLe2 = Math.Pow(miA2, 2) / iRc2;
            double miQe2 = miLe2 / (2 * iRc2);
            double miYe2 = ((miQe2 / 3) - (Math.Pow(miQe2, 3) / 42) + (Math.Pow(miQe2, 5) / 1320) - (Math.Pow(miQe2, 7) / 75600)) * miLe2;
            double miDR2 = miYe2 - iRc2 * (1 - Math.Cos(miQe2));
            double miXe2 = (1 - Math.Pow(miQe2, 2) / 10 + Math.Pow(miQe2, 4) / 216 - Math.Pow(miQe2, 6) / 9360 + Math.Pow(miQe2, 8) / 685440) * miLe2;
            double miXM2 = miXe2 - iRc2 * Math.Sin(miQe2);

            double miAc12;
            if (iPuntoC2.coordenadaX > iPuntoC1.coordenadaX)
            {
                if (iPuntoC2.coordenadaY > iPuntoC1.coordenadaY)
                {
                    miAc12 = 90 - Math.Atan((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY) / (iPuntoC2.coordenadaX - iPuntoC1.coordenadaX)) * 180 / Math.PI;
                }
                else
                {
                    miAc12 = 90+Math.Atan((iPuntoC1.coordenadaY-iPuntoC2.coordenadaY)/(iPuntoC2.coordenadaX-iPuntoC1.coordenadaX))*180/Math.PI;
                }
            }
            else
            {
                if (iPuntoC2.coordenadaY > iPuntoC1.coordenadaY)
                {
                     miAc12 =270+Math.Atan((iPuntoC2.coordenadaY-iPuntoC1.coordenadaY)/(iPuntoC1.coordenadaX-iPuntoC2.coordenadaX))*180/Math.PI;
                }
                else
                {
                     miAc12 =270-Math.Atan((iPuntoC1.coordenadaY-iPuntoC2.coordenadaY)/(iPuntoC1.coordenadaX-iPuntoC2.coordenadaX))*180/Math.PI;
                }
            }

            double miD = Math.Sqrt(Math.Pow((iPuntoC2.coordenadaX - iPuntoC1.coordenadaX),2) + Math.Pow((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY), 2));
            double miM1 = miD / (1 + (iRc2 + miDR2) / (iRc1 + miDR1));
            double miM2 = miD - miM1;

            //las formulas fallan.. 
            double miL1, miL2;
            miL1 = Math.Sqrt(Math.Pow(miM1, 2) - Math.Pow((iRc1 + miDR1), 2));
                 miL2 = Math.Sqrt(Math.Pow(miM2, 2) - Math.Pow((iRc2 + miDR2), 2));

            double miPIx = iPuntoC1.coordenadaX + miM1 * Math.Sin(miAc12 * Math.PI / 180);
            double miPIy = iPuntoC1.coordenadaY + miM1 * Math.Cos(miAc12 * Math.PI / 180);

            double miOmega1 = Math.Atan((iRc1 + miDR1) / miL1) * 180 / Math.PI;
            double miOmega2 = Math.Atan((iRc2 + miDR2) / miL2) * 180 / Math.PI;

            double miAzInt, miAx1, miAx2;
            if (sentG1 == sentidoCurva.Horario)
            {
                miAzInt = miAc12 + miOmega1;
                miAx1 = miAzInt - 90;
                miAx2 = miAzInt + 90;
            }
            else
            {
                miAzInt = miAc12 - miOmega1;
                miAx1 = miAzInt + 90;
                miAx2 = miAzInt - 90;
            }

            double miPcl1x, miPcl1y, miPcl2x, miPcl2y, miPc1x, miPc1y, miPc2x, miPc2y;

            miPcl1x = miPIx + (miL1 - miXM1) * Math.Sin((miAzInt + 180) * Math.PI / 180);
            miPcl1y = miPIy + (miL1 - miXM1) * Math.Cos((miAzInt + 180) * Math.PI / 180);

            miPcl2x = miPIx + (miL2 - miXM2) * Math.Sin(miAzInt * Math.PI / 180);
            miPcl2y = miPIy + (miL2 - miXM2) * Math.Cos(miAzInt * Math.PI / 180);

            if (sentG1 == sentidoCurva.Horario)
            {
                miPc1x = iPuntoC1.coordenadaX + iRc1 * Math.Sin((miAx1 - miQe1 * 180 / Math.PI) * Math.PI / 180);
                miPc1y = iPuntoC1.coordenadaY + iRc1 * Math.Cos((miAx1 - miQe1 * 180 / Math.PI) * Math.PI / 180);

            }
            else
            {
                miPc1x = iPuntoC1.coordenadaX + iRc1 * Math.Sin((miAx1 + miQe1 * 180 / Math.PI) * Math.PI / 180);
                miPc1y = iPuntoC1.coordenadaY + iRc1 * Math.Cos((miAx1 + miQe1 * 180 / Math.PI) * Math.PI / 180);
 
            }
            if (sentG2 == sentidoCurva.Horario)
            {
                miPc2x = iPuntoC2.coordenadaX + iRc2 * Math.Sin((miAx2 + miQe2 * 180 / Math.PI) * Math.PI / 180);
                miPc2y = iPuntoC2.coordenadaY + iRc2 * Math.Cos((miAx2 + miQe2 * 180 / Math.PI) * Math.PI / 180);

            }
            else
            {
                miPc2x = iPuntoC2.coordenadaX + iRc2 * Math.Sin((miAx2 - miQe2 * 180 / Math.PI) * Math.PI / 180);
                miPc2y = iPuntoC2.coordenadaY + iRc2 * Math.Cos((miAx2 - miQe2 * 180 / Math.PI) * Math.PI / 180);

            }

            Punto3d miPC1 = new Punto3d(miPc1x, miPc1y, 0);
            Punto3d miPCL1 = new Punto3d(miPcl1x, miPcl1y, 0);
            Punto3d miPC2 = new Punto3d(miPcl2x, miPcl2y, 0);
            Punto3d miPCL2 = new Punto3d(miPc2x, miPc2y, 0);


            Punto3d[] puntosSing = new Punto3d[5];
            puntosSing[0] = miPC1;
            puntosSing[1] = miPCL1;
            puntosSing[2] = miPC2;
            puntosSing[3] = miPCL2;


            Punto3d azFINAL = new Punto3d(miAzInt, 0, 0);
            puntosSing[4] = azFINAL;


            return puntosSing;

            
        }

        public Punto3d[] addCurvaenS(double iRc1, double iRc2, Punto3d iPuntoC1, Punto3d iPuntoC2, sentidoCurva sentG1, sentidoCurva sentG2)
        {
            double miA1 = Math.Pow((12 * Math.Pow(iRc1, 3)), 0.25);
            double miLe1 = Math.Pow(miA1, 2) / iRc1;
            double miQe1 = miLe1 / (2 * iRc1);
            double miYe1 = ((miQe1 / 3) - (Math.Pow(miQe1, 3) / 42) + (Math.Pow(miQe1, 5) / 1320) - (Math.Pow(miQe1, 7) / 75600)) * miLe1;
            double miDR1 = miYe1 - iRc1 * (1 - Math.Cos(miQe1));
            double miXe1 = (1 - Math.Pow(miQe1, 2) / 10 + Math.Pow(miQe1, 4) / 216 - Math.Pow(miQe1, 6) / 9360 + Math.Pow(miQe1, 8) / 685440) * miLe1;
            double miXM1 = miXe1 - iRc1 * Math.Sin(miQe1);

            double miA2 = Math.Pow((12 * Math.Pow(iRc2, 3)), 0.25);
            double miLe2 = Math.Pow(miA2, 2) / iRc2;
            double miQe2 = miLe2 / (2 * iRc2);
            double miYe2 = ((miQe2 / 3) - (Math.Pow(miQe2, 3) / 42) + (Math.Pow(miQe2, 5) / 1320) - (Math.Pow(miQe2, 7) / 75600)) * miLe2;
            double miDR2 = miYe2 - iRc2 * (1 - Math.Cos(miQe2));
            double miXe2 = (1 - Math.Pow(miQe2, 2) / 10 + Math.Pow(miQe2, 4) / 216 - Math.Pow(miQe2, 6) / 9360 + Math.Pow(miQe2, 8) / 685440) * miLe2;
            double miXM2 = miXe2 - iRc2 * Math.Sin(miQe2);

            double miAc12;
            if (iPuntoC2.coordenadaX > iPuntoC1.coordenadaX)
            {
                if (iPuntoC2.coordenadaY > iPuntoC1.coordenadaY)
                {
                    miAc12 = 90 - (Math.Atan((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY) / (iPuntoC2.coordenadaX - iPuntoC1.coordenadaX)) * 180 / Math.PI);
                }
                else
                {
                    miAc12 = (90 + (Math.Atan((iPuntoC1.coordenadaY - iPuntoC2.coordenadaY) / (iPuntoC2.coordenadaX - iPuntoC1.coordenadaX))) * 180 / Math.PI);
                }
            }
            else
            {
                if (iPuntoC2.coordenadaY > iPuntoC1.coordenadaY)
                {
                    miAc12 = 270 + (Math.Atan((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY) / (iPuntoC1.coordenadaX - iPuntoC2.coordenadaX)) * 180 / Math.PI);
                }
                else
                {
                    miAc12 = 270 - (Math.Atan((iPuntoC1.coordenadaY - iPuntoC2.coordenadaY) / (iPuntoC1.coordenadaX - iPuntoC2.coordenadaX)) * 180 / Math.PI);
                }
            }
            double miD = Math.Sqrt(Math.Pow((iPuntoC2.coordenadaX - iPuntoC1.coordenadaX), 2) + Math.Pow((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY), 2));
            double miM1 = miD / (1 + (iRc2 + miDR2) / (iRc1 + miDR1));
            double miM2 = miD - miM1;
            double miL1 = Math.Sqrt(Math.Pow(miM1, 2) - Math.Pow((iRc1 + miDR1), 2));
            double miL2 = Math.Sqrt(Math.Pow(miM2, 2) - Math.Pow((iRc2 + miDR2), 2));

            double miL = miL1 + miL2 - miXM1 - miXM2;
            double miDifL1XM1 = miL1 - miXM1;
            double miDifL1XM2 = miL2 - miXM2;

            double miDLe1 = miDifL1XM1;
            double miDLe2 = miDifL1XM2;
            int i = 0;
            while ((Math.Round(miDifL1XM1, 12) != 0) && (Math.Round(miDifL1XM2, 12) != 0))
            {
                i++;
                miLe1 = miLe1 + miDLe1;
                miLe2 = miLe2 + miDLe2;
                miQe1 = miLe1 / (2 * iRc1);
                miQe2 = miLe2 / (2 * iRc2);
                miXe1 = (1 - Math.Pow(miQe1, 2) / 10 + Math.Pow(miQe1, 4) / 216 - Math.Pow(miQe1, 6) / 9360 + Math.Pow(miQe1, 8) / 685440) * miLe1;
                miXe2 = (1 - Math.Pow(miQe2, 2) / 10 + Math.Pow(miQe2, 4) / 216 - Math.Pow(miQe2, 6) / 9360 + Math.Pow(miQe2, 8) / 685440) * miLe2;
                miYe1 = ((miQe1 / 3) - (Math.Pow(miQe1, 3) / 42) + (Math.Pow(miQe1, 5) / 1320) - (Math.Pow(miQe1, 7) / 75600)) * miLe1;
                miYe2 = ((miQe2 / 3) - (Math.Pow(miQe2, 3) / 42) + (Math.Pow(miQe2, 5) / 1320) - (Math.Pow(miQe2, 7) / 75600)) * miLe2;
                miDR1 = miYe1 - iRc1 * (1 - Math.Cos(miQe1));
                miDR2 = miYe2 - iRc2 * (1 - Math.Cos(miQe2));
                miXM1 = miXe1 - iRc1 * Math.Sin(miQe1);
                miXM2 = miXe2 - iRc2 * Math.Sin(miQe2);

                miM1 = miD / (1 + (iRc2 + miDR2) / (iRc1 + miDR1));
                miM2 = miD - miM1; 
                miL1 = Math.Sqrt(Math.Pow(miM1, 2) - Math.Pow((iRc1 + miDR1), 2));
                miL2 = Math.Sqrt(Math.Pow(miM2, 2) - Math.Pow((iRc2 + miDR2), 2));

                miL = miL1 + miL2 - miXM1 - miXM2;

                miDifL1XM1 = miL1 - miXM1;
                miDifL1XM2 = miL2 - miXM2;

                miDLe1 = miDifL1XM1;
                miDLe2 = miDifL1XM2;
                
            }

            double miOmega1 = (Math.Asin((iRc1 + miDR1) / miM1)) * 180 / Math.PI;
            double miOmega2 = (Math.Asin((iRc2 + miDR2) / miM2)) * 180 / Math.PI;

            double miAzInt, miAx1, miAx2;
            if (sentG1 == sentidoCurva.Horario)
            {
                miAzInt = miAc12 + miOmega1;
                miAx1 = miAzInt - 90;
                miAx2 = miAzInt + 90;
            }
            else
            {
                miAzInt = miAc12 - miOmega1;
                miAx1 = miAzInt + 90;
                miAx2 = miAzInt - 90;
            }

            double miPclx, miPcly,  miPc1x, miPc1y, miPc2x, miPc2y;

            miPclx = iPuntoC1.coordenadaX + miM1 * Math.Sin(miAc12 * Math.PI / 180);
            miPcly = iPuntoC1.coordenadaY + miM1 * Math.Cos(miAc12 * Math.PI / 180);

            if (sentG1 == sentidoCurva.Horario)
            {
                miPc1x = iPuntoC1.coordenadaX + iRc1 * Math.Sin((miAx1 - miQe1 * 180 / Math.PI) * Math.PI / 180);
                miPc1y = iPuntoC1.coordenadaY + iRc1 * Math.Cos((miAx1 - miQe1 * 180 / Math.PI) * Math.PI / 180);

            }
            else
            {
                miPc1x = iPuntoC1.coordenadaX + iRc1 * Math.Sin((miAx1 + miQe1 * 180 / Math.PI) * Math.PI / 180);
                miPc1y = iPuntoC1.coordenadaY + iRc1 * Math.Cos((miAx1 + miQe1 * 180 / Math.PI) * Math.PI / 180);

            }
            if (sentG2 == sentidoCurva.Horario)
            {
                miPc2x = iPuntoC2.coordenadaX + iRc2 * Math.Sin((miAx2 + miQe2 * 180 / Math.PI) * Math.PI / 180);
                miPc2y = iPuntoC2.coordenadaY + iRc2 * Math.Cos((miAx2 + miQe2 * 180 / Math.PI) * Math.PI / 180);

            }
            else
            {
                miPc2x = iPuntoC2.coordenadaX + iRc2 * Math.Sin((miAx2 - miQe2 * 180 / Math.PI) * Math.PI / 180);
                miPc2y = iPuntoC2.coordenadaY + iRc2 * Math.Cos((miAx2 - miQe2 * 180 / Math.PI) * Math.PI / 180);

            }
            Punto3d miPunto1 = new Punto3d(miPc1x, miPc1y, 0);
            Punto3d miPunto2 = new Punto3d(miPclx, miPcly, 0);
            Punto3d miPunto3 = new Punto3d(miPclx, miPcly, 0);
            Punto3d miPunto4 = new Punto3d(miPc2x, miPc2y, 0);

            
            Punto3d[] puntosSing = new Punto3d[5];
            puntosSing[0] = miPunto1;
            puntosSing[1] = miPunto2;
            puntosSing[2] = miPunto3;
            puntosSing[3] = miPunto4;

            Punto3d azFINAL = new Punto3d(miAzInt, miLe1, miLe2);
            puntosSing[4] = azFINAL;



            return puntosSing;


        }

        public Punto3d[] addPararelismo(double iRc1, double iRc2, Punto3d iPuntoC1, Punto3d iPuntoC2, sentidoCurva sentG1, sentidoCurva sentG2)
        {
            double miA1 = Math.Pow((12 * Math.Pow(iRc1, 3)), 0.25);
            double miLe1 = Math.Pow(miA1, 2) / iRc1;
            double miQe1 = miLe1 / (2 * iRc1);
            double miYe1 = ((miQe1 / 3) - (Math.Pow(miQe1, 3) / 42) + (Math.Pow(miQe1, 5) / 1320) - (Math.Pow(miQe1, 7) / 75600)) * miLe1;
            double miDR1 = miYe1 - iRc1 * (1 - Math.Cos(miQe1));
            double miXe1 = (1 - Math.Pow(miQe1, 2) / 10 + Math.Pow(miQe1, 4) / 216 - Math.Pow(miQe1, 6) / 9360 + Math.Pow(miQe1, 8) / 685440) * miLe1;
            double miXM1 = miXe1 - iRc1 * Math.Sin(miQe1);

            double miA2 = Math.Pow((12 * Math.Pow(iRc2, 3)), 0.25);
            double miLe2 = Math.Pow(miA2, 2) / iRc2;
            double miQe2 = miLe2 / (2 * iRc2);
            double miYe2 = ((miQe2 / 3) - (Math.Pow(miQe2, 3) / 42) + (Math.Pow(miQe2, 5) / 1320) - (Math.Pow(miQe2, 7) / 75600)) * miLe2;
            double miDR2 = miYe2 - iRc2 * (1 - Math.Cos(miQe2));
            double miXe2 = (1 - Math.Pow(miQe2, 2) / 10 + Math.Pow(miQe2, 4) / 216 - Math.Pow(miQe2, 6) / 9360 + Math.Pow(miQe2, 8) / 685440) * miLe2;
            double miXM2 = miXe2 - iRc2 * Math.Sin(miQe2);

            double miAc12;
            if (iPuntoC2.coordenadaX > iPuntoC1.coordenadaX)
            {
                if (iPuntoC2.coordenadaY > iPuntoC1.coordenadaY)
                {
                    miAc12 = 90 - Math.Atan((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY) / (iPuntoC2.coordenadaX - iPuntoC1.coordenadaX)) * 180 / Math.PI;
                }
                else
                {
                    miAc12 = 90 + Math.Atan((iPuntoC1.coordenadaY - iPuntoC2.coordenadaY) / (iPuntoC2.coordenadaX - iPuntoC1.coordenadaX)) * 180 / Math.PI;
                }
            }
            else
            {
                if (iPuntoC2.coordenadaY > iPuntoC1.coordenadaY)
                {
                    miAc12 = 270 + Math.Atan((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY) / (iPuntoC1.coordenadaX - iPuntoC2.coordenadaX)) * 180 / Math.PI;
                }
                else
                {
                    miAc12 = 270 - Math.Atan((iPuntoC1.coordenadaY - iPuntoC2.coordenadaY) / (iPuntoC1.coordenadaX - iPuntoC2.coordenadaX)) * 180 / Math.PI;
                }
            }
            double miDx2 = Math.Abs((iRc1+miDR1)-(iRc2+miDR2));
            double miD12 = Math.Sqrt(Math.Pow((iPuntoC2.coordenadaX - iPuntoC1.coordenadaX), 2) + Math.Pow((iPuntoC2.coordenadaY - iPuntoC1.coordenadaY), 2));
            double miAlpha = Math.Acos(miDx2 / miD12) * 180 / Math.PI;
            double miA23;
            if (iRc1 > iRc2)
            {
                if (sentG1 == sentidoCurva.Horario)
                {
                    miA23 = miAc12 - miAlpha + 90;
                }
                else
                {
                    miA23 = miAc12 + miAlpha - 90;
                }
            }
            else
            {
                if (sentG1 == sentidoCurva.Horario)
                {
                    miA23 = miAc12 - 180 + miAlpha + 90;
                }
                else
                {
                    miA23 = miAc12 + 180 - miAlpha - 90;
                }
            }

            double miPcl2x, miPcl2y, miPc2x, miPc2y, miPcl3x, miPcl3y, miPc3x, miPc3y, miPaux1x, miPaux1y, miPaux2x, miPaux2y;


            if (sentG1 == sentidoCurva.Horario)
            {
                miPaux1x = iPuntoC1.coordenadaX + (iRc1 + miDR1) * Math.Sin((miA23 - 90) * Math.PI / 180);
                miPaux1y = iPuntoC1.coordenadaY + (iRc1 + miDR1) * Math.Cos((miA23 - 90) * Math.PI / 180);
                miPc2x = iPuntoC1.coordenadaX + iRc1 * Math.Sin((miA23 + 180 + 90 - miQe1 * 180 / Math.PI) * Math.PI / 180);
                miPc2y = iPuntoC1.coordenadaY + iRc1 * Math.Cos((miA23 + 180 + 90 - miQe1 * 180 / Math.PI) * Math.PI / 180);
            }
            else
            {
                miPaux1x = iPuntoC1.coordenadaX + (iRc1 + miDR1) * Math.Sin((miA23 + 90) * Math.PI / 180);
                miPaux1y = iPuntoC1.coordenadaY + (iRc1 + miDR1) * Math.Cos((miA23 + 90) * Math.PI / 180);
                miPc2x = iPuntoC1.coordenadaX + iRc1 * Math.Sin((miA23 + 180 - 90 + miQe1 * 180 / Math.PI) * Math.PI / 180);
                miPc2y = iPuntoC1.coordenadaY + iRc1 * Math.Cos((miA23 + 180 - 90 + miQe1 * 180 / Math.PI) * Math.PI / 180);
            }

            miPcl2x = miPaux1x + miXM1 * Math.Sin(miA23 * Math.PI / 180);
            miPcl2y = miPaux1y + miXM1 * Math.Cos(miA23 * Math.PI / 180);

            if (sentG2 == sentidoCurva.Horario)
            {
                miPaux2x = iPuntoC2.coordenadaX + (iRc2 + miDR2) * Math.Sin((miA23 - 90) * Math.PI / 180);
                miPaux2y = iPuntoC2.coordenadaY + (iRc2 + miDR2) * Math.Cos((miA23 - 90) * Math.PI / 180);
                miPc3x = iPuntoC2.coordenadaX + iRc2 * Math.Sin((miA23 - 90 + miQe2 * 180 / Math.PI) * Math.PI / 180);
                miPc3y = iPuntoC2.coordenadaY + iRc2 * Math.Cos((miA23 - 90 + miQe2 * 180 / Math.PI) * Math.PI / 180);
            }
            else
            {
                miPaux2x = iPuntoC2.coordenadaX + (iRc2 + miDR2) * Math.Sin((miA23 + 90) * Math.PI / 180);
                miPaux2y = iPuntoC2.coordenadaY + (iRc2 + miDR2) * Math.Cos((miA23 + 90) * Math.PI / 180);
                miPc3x = iPuntoC2.coordenadaX + iRc2 * Math.Sin((miA23 + 90 - miQe2 * 180 / Math.PI) * Math.PI / 180);
                miPc3y = iPuntoC2.coordenadaY + iRc2 * Math.Cos((miA23 + 90 - miQe2 * 180 / Math.PI) * Math.PI / 180);
            }
            miPcl3x = miPaux2x + miXM2 * Math.Sin((miA23 + 180) * Math.PI / 180);
            miPcl3y = miPaux2y + miXM2 * Math.Cos((miA23 + 180) * Math.PI / 180);

            Punto3d miPunto1 = new Punto3d(miPc2x, miPc2y, 0);
            Punto3d miPunto2 = new Punto3d(miPcl2x, miPcl2y, 0);
            Punto3d miPunto3 = new Punto3d(miPcl3x, miPcl3y, 0);
            Punto3d miPunto4 = new Punto3d(miPc3x, miPc3y, 0);


            Punto3d[] puntosSing = new Punto3d[5];
            puntosSing[0] = miPunto1;
            puntosSing[1] = miPunto2;
            puntosSing[2] = miPunto3;
            puntosSing[3] = miPunto4;


            Punto3d azFINAL = new Punto3d(miA23, 0, 0);
            puntosSing[4] = azFINAL;

            return puntosSing;

        }

        public Punto3d[] addCurvaGranRadio(double iRc, double iAzimut1, double iAzimut2, Punto3d iVertice, sentidoCurva sentG)
        {
            double miDelta = getDelta(iAzimut1, iAzimut2);
            double miPhi = 180 - miDelta;
            double miT1x = iVertice.coordenadaX + (iRc * Math.Tan(miDelta / 2 * Math.PI / 180)) * Math.Sin((iAzimut1 - 180) * Math.PI / 180);
            double miT1y = iVertice.coordenadaY + (iRc * Math.Tan(miDelta / 2 * Math.PI / 180)) * Math.Cos((iAzimut1 - 180) * Math.PI / 180);
            double miT2x = iVertice.coordenadaX + (iRc * Math.Tan(miDelta / 2 * Math.PI / 180)) * Math.Sin(iAzimut2 * Math.PI / 180);
            double miT2y = iVertice.coordenadaY + (iRc * Math.Tan(miDelta / 2 * Math.PI / 180)) * Math.Cos(iAzimut2 * Math.PI / 180);
            double miPcx;
            double miPcy;

            if (sentG == sentidoCurva.Horario)
            {
                miPcx = miT1x + iRc * Math.Sin((iAzimut1 + 90) * Math.PI / 180);
                miPcy = miT1y + iRc * Math.Cos((iAzimut1 + 90) * Math.PI / 180);
            }
            else
            {
                miPcx = miT1x + iRc * Math.Sin((iAzimut1 - 90) * Math.PI / 180);
                miPcy = miT1y + iRc * Math.Cos((iAzimut1 - 90) * Math.PI / 180);
            }

            Punto3d miPunto1 = new Punto3d(miT1x, miT1y, 0);
            Punto3d miPunto2 = new Punto3d(miT1x, miT1y, 0);
            Punto3d miPunto3 = new Punto3d(miT2x, miT2y, 0);
            Punto3d miPunto4 = new Punto3d(miT2x, miT2y, 0);
            Punto3d miPunto5 = new Punto3d(miPcx, miPcy, 0);


            Punto3d[] puntosSing = new Punto3d[5];
            puntosSing[0] = miPunto1;
            puntosSing[1] = miPunto2;
            puntosSing[2] = miPunto3;
            puntosSing[3] = miPunto4;
            puntosSing[4] = miPunto5;


            return puntosSing;
        }
        

        #endregion

        #region "metodos publicos"

        public double Length
        {
            get
            {
                double miLong = 0;
                miLong = mComponentes.ElementAt(mComponentes.Count - 1).getPkFinal();
                return miLong;
            }
        }

        public void getPeralteAlDist(double iDistancia, ref double iPeralteLadoIzquierdo, ref double iPeralteLadoDerecho)
        {
            double miComponetePk = mComponentes.ElementAt(0).getPkFinal();
            int i = 0;
            while (miComponetePk < (iDistancia - 0.01))
            {
                i++;
                miComponetePk = mComponentes.ElementAt(i).getPkFinal();
            }
            iPeralteLadoIzquierdo = mComponentes.ElementAt(i).getMargenIzq(iDistancia);
            iPeralteLadoDerecho = mComponentes.ElementAt(i).getMargenDer(iDistancia);
        }

        public double[] getPointAtDist(double iDistancia)
        {
            if (iDistancia > mComponentes.ElementAt(mComponentes.Count - 1).getPkFinal())
            {
                iDistancia = mComponentes.ElementAt(mComponentes.Count - 1).getPkFinal();
            }
            
            double miComponetePk = mComponentes.ElementAt(0).getPkFinal();
            int i = 0;
            while (miComponetePk < iDistancia)
            {
                i++;
                miComponetePk = mComponentes.ElementAt(i).getPkFinal();
            }
            double[] miPunto = mComponentes.ElementAt(i).getPointAtDist(iDistancia);
            return miPunto;
        }

        public double[] getPointLocation(double iDistancia, double iOffset, ladoCalzada iLadoCalzada)
        {
            if (iDistancia > mComponentes.ElementAt(mComponentes.Count - 1).getPkFinal())
            {
                iDistancia = mComponentes.ElementAt(mComponentes.Count - 1).getPkFinal();
            }
            double miComponetePk = mComponentes.ElementAt(0).getPkFinal();
            int i = 0;
            while (miComponetePk < iDistancia || miComponetePk == 0)
            {
                i++;
                miComponetePk = mComponentes.ElementAt(i).getPkFinal();
            }

            double[] miPunto = new double[3];
            if ((mComponentes.ElementAt(i).getTipoComponente() == componentes.Componente.tipoComponente.curva)||(mComponentes.ElementAt(i).getTipoComponente() == componentes.Componente.tipoComponente.linea))
            {
                miPunto = mComponentes.ElementAt(i).getPointAtLocation(iDistancia, iOffset, iLadoCalzada);
            }
            else if (mComponentes.ElementAt(i).getTipoComponente() == componentes.Componente.tipoComponente.clotoideEntrada)
            {
                Linea miLineaAnt = (Linea)mComponentes.ElementAt(i - 1);
                double miAzimutAnt = miLineaAnt.azimut;
                Clotoide miClotiode = (Clotoide)mComponentes.ElementAt(i);
                miPunto = miClotiode.getPointAtLocationClotiode(iDistancia, iOffset, iLadoCalzada, miAzimutAnt);
            }
            else
            {
                Linea miLineaPost = (Linea)mComponentes.ElementAt(i + 1);
                Clotoide miClotiode = (Clotoide)mComponentes.ElementAt(i);
                
                double miAzimutAnt = miLineaPost.azimut;
                miPunto = miClotiode.getPointAtLocationClotiode(iDistancia, iOffset, iLadoCalzada, miAzimutAnt);


            }
            mAzimutTransTemp = miPunto[2];
            double[] miPunto2 = new double[2];
            miPunto2[0] = miPunto[0];
            miPunto2[1] = miPunto[1];

            return miPunto2;
        }

        public double getAzimutTrans()
        {
            return mAzimutTransTemp;
        }

        public List<componentes.Componente> getComponentes
        {
            get
            {
                return mComponentes;
            }
        }

        public Logica.InfoComponentes draw()
        {
            InfoComponentes miInfo = new InfoComponentes(mComponentes, mVertices);
            return miInfo;
        }

        public List<Vertice> getVertices
        {
            get
            {
                return mVertices;
            }
        }

        public MemoryStream guardarEjeTrazado()
        {
                BinaryFormatter serializer = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                serializer.Serialize(stream, this);
                return stream;

       }

        public void exportarEjeTrazado(MemoryStream iSerializado)
        {

            string fileName = "EjeTrazadoExp.txt";
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Write);

            byte[] miBuffer = iSerializado.GetBuffer();

            for (int i = 0; i < miBuffer.Length; i++)
            {
                stream.WriteByte(miBuffer[i]);
            }
            stream.Close();
        }


        public static EjeTrazado recuperaEjeTrazado(MemoryStream iStream)
        {
            BinaryFormatter formatterR = new BinaryFormatter();
            EjeTrazado deserializada = (EjeTrazado)formatterR.Deserialize(iStream);
            return deserializada;
        }

        public List<oInformeEje> escribirInforme()
        {
            InfoComponentes misComponentes = new InfoComponentes(mComponentes, mVertices, false);
            return misComponentes.escribirInforme();
        }
        
        private void calculaMaxRadio()
        {
            double miRadioMax = 0;
            foreach (Componente miComp in mComponentes)
            {
                if (miComp.getTipoComponente() == Componente.tipoComponente.curva)
                {
                    Curva miCurva = (Curva)miComp;
                    if (miRadioMax < miCurva.getRadio)
                    {
                        miRadioMax = miCurva.getRadio;
                    }
                }
            }
            mMaxRadio = miRadioMax;
        }

        public double getRadioMax
        {
            get
            {
                return mMaxRadio;
            }
        }

        public double getPeralteCurva
        {
            get
            {
                return mPeralteCurva;
            }
        }

        public double getBombeo
        {
            get
            {
                return mPeralteRecta;
            }
        }
        
        public Componente getComponente(double iPk)
        {

            double miComponetePk = mComponentes.ElementAt(0).getPkFinal();
            int i = 0;
            while (miComponetePk <= iPk && i < mComponentes.Count - 1)
            {
                i++;
                miComponetePk = mComponentes.ElementAt(i).getPkFinal();
            }

            return mComponentes.ElementAt(i);
        }



        #endregion

    }
}
