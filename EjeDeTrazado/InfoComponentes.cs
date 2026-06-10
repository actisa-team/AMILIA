using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EjeDeTrazado.puntosDelEje;
using EjeDeTrazado.componentes;
using tadLayShare.puntos;


namespace EjeDeTrazado
{
    using engNet.ClassT;
    using engNet.Csv;
    using engNet.CustomAtributos;
    using System.ComponentModel;
    using System.IO;

    public class InfoComponentes
    {
        List<InfoComponente> mComponentes = new List<InfoComponente>();

        public InfoComponentes(List<Componente> misComponentes, List<Vertice> misVertices)
        {
            int i = 0;
            foreach (Componente miComponente in misComponentes)
            {
                if (miComponente.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                {
                    InfoComponente miNewComp = new InfoComponente(miComponente.draw() as double[], miComponente.getPkIni, miComponente.getPkFinal(), miComponente.getLongitud(), miComponente.getPuntoEntrada, miComponente.getPuntoSalida);
                    Curva miCurva = (Curva)misComponentes.ElementAt(i);
                    miNewComp.setSentGiro= miCurva.getSentCurva;
                   // miNewComp.setTipoCurva = EjeDeTrazado.puntosDelEje.EjeTrazado.tipoCurva.c;
                   // miNewComp.setSentGiro = misVertices[i].getSentCurva;
                   // miNewComp.setTipoCurva = misVertices[i].getTipocurva;
                    miNewComp.setPeralte = miComponente.getPeralte();
                    miNewComp.setMargenD = miComponente.getMargenDer(0);
                    miNewComp.setMargenI = miComponente.getMargenIzq(0);
                    mComponentes.Add(miNewComp);

                }
                else if (miComponente.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                {
                    InfoComponente miNewComp = new InfoComponente(miComponente.getTipoComponente(), miComponente.draw() as List<Punto3d>, miComponente.getPkIni, miComponente.getPkFinal(), miComponente.getLongitud(), miComponente.getPuntoEntrada, miComponente.getPuntoSalida);
                   // miNewComp.setTipoRecta = misVertices[i - 1].getTipoSeg;
                    miNewComp.setPeralte = miComponente.getPeralte();
                    //miNewComp.setAzimutFinal = misVertices[i - 1].getAzimut;
                    Linea miRecta = (Linea)misComponentes.ElementAt(i);
                    miNewComp.setAzimutFinal = miRecta.AzimutFinal;
                    miNewComp.setMargenD = miComponente.getMargenDer(0);
                    miNewComp.setMargenI = miComponente.getMargenIzq(0);
                    mComponentes.Add(miNewComp);
                }
                else
                {
                    Clotoide miClo = miComponente as Clotoide;
                    Punto3d Punto_Entrada;
                    Punto3d Punto_Salida;
                    if (miClo.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
                    {
                        if (miClo.Get_Le_r() > 0)
                        {
                            Punto_Entrada = new Punto3d(miComponente.getPointAtDist(miClo.getPkIni)[0], miComponente.getPointAtDist(miClo.getPkIni)[1], 0);
                            Punto_Salida = new Punto3d(miComponente.getPointAtDist(miComponente.getPkFin)[0], miComponente.getPointAtDist(miComponente.getPkFin)[1], 0);
                        }
                        else
                        {
                            Punto_Entrada = new Punto3d(miClo.getPointAtDist(miClo.getPkIni)[0], miClo.getPointAtDist(miClo.getPkIni)[1], 0);
                            Punto_Salida = new Punto3d(miClo.getPointAtDist(miClo.getPkFin)[0], miClo.getPointAtDist(miClo.getPkFin)[1], 0);
                        }

                    }
                    else if (miClo.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada)
                    {
                        if (miClo.Get_Le_r() > 0)
                        {
                            Punto_Entrada = new Punto3d(miComponente.getPointAtDist(miClo.getPkIni + miClo.Get_Le_r())[0], miComponente.getPointAtDist(miClo.getPkIni + miClo.Get_Le_r())[1], 0);
                            Punto_Salida = new Punto3d(miComponente.getPointAtDist(miClo.getPkIni+ miClo.Get_Le_m())[0], miComponente.getPointAtDist(miClo.getPkIni + miClo.Get_Le_m())[1], 0);
                        }
                        else
                        {
                            Punto_Entrada = new Punto3d(miClo.getPointAtDist(miClo.getPkIni)[0], miClo.getPointAtDist(miClo.getPkIni)[1], 0);
                            Punto_Salida = new Punto3d(miClo.getPointAtDist(miClo.getPkFin)[0], miClo.getPointAtDist(miClo.getPkFin)[1], 0);
                        }

                    }
                    else
                    {
                        Punto_Entrada = new Punto3d(miClo.getPointAtDist(miClo.getPkIni)[0], miClo.getPointAtDist(miClo.getPkIni)[1], 0);
                        Punto_Salida = new Punto3d(miClo.getPointAtDist(miClo.getPkFin)[0], miClo.getPointAtDist(miClo.getPkFin)[1], 0);
                    }

                    /*Punto_Entrada = new Punto3d(miClo.getPointAtDist(miClo.getPkIni)[0], miClo.getPointAtDist(miClo.getPkIni)[1], 0);
                    Punto_Salida = new Punto3d(miClo.getPointAtDist(miClo.getPkFin)[0], miClo.getPointAtDist(miClo.getPkFin)[1], 0);*/
                    List<double[]> Lista = miComponente.getComponentPoints();
                    
                    InfoComponente miNewComp = new InfoComponente(miComponente.getTipoComponente(), miComponente.draw() as List<Punto3d>, miClo.getPkIni, miClo.getPkIni+miClo.getLongitud(), miClo.getValorA(), miClo.getLongitud(), Punto_Entrada, Punto_Salida);
                    miNewComp.setPeralte = miComponente.getPeralte();
                    miNewComp.setVariacionMI = miClo.getVariacionMI();
                    miNewComp.setVariacionMD = miClo.getVariacionMD();
                    mComponentes.Add(miNewComp);

                    if (miComponente.getTipoComponente() == Componente.tipoComponente.clotoideSalida)
                    {
                        //i++;
                    }
                }
                i++;
            }
        }

        public List<InfoComponente> getInfoComponentes
        {
            get
            {
                return mComponentes;
            }

        }

        public List<oInformeEje> escribirInforme(string nombre_informe)
        {
            int vertice = 1;
            int seg = 1;

            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oInformeEje> miInforme = new List<oInformeEje>();
            List<oValDesT<string, double?>> miLstFooter = new List<oValDesT<string, double?>>();
            foreach (InfoComponente miComponenteInfo in mComponentes)
            {
                if (miComponenteInfo.getTipoComponente == Componente.tipoComponente.curva)
                {
                    string tipoC = "";
                    if (miComponenteInfo.getTipoCurva == EjeTrazado.tipoCurva.c2500) tipoC = strEje.c2500;
                    if (miComponenteInfo.getTipoCurva == EjeTrazado.tipoCurva.c5000) tipoC = strEje.c5000;
                    if (miComponenteInfo.getTipoCurva == EjeTrazado.tipoCurva.cnp) tipoC = strEje.CNP;
                    if (miComponenteInfo.getTipoCurva == EjeTrazado.tipoCurva.cnpAnguloReducido) tipoC = strEje.CNPAnguloReducido;
                    if (miComponenteInfo.getTipoCurva == EjeTrazado.tipoCurva.cp) tipoC = strEje.CP;
                    if (miComponenteInfo.getTipoCurva == EjeTrazado.tipoCurva.noValorado) tipoC = strEje.NoValorado;


                    vertice++;
                    miInforme.Add(new oInformeEje(null, vertice, strEje.curva, tipoC, null, miComponenteInfo.getPuntoIniX, miComponenteInfo.getPuntoIniY, miComponenteInfo.getPuntoFinX, miComponenteInfo.getPuntoFinY,
            miComponenteInfo.getPuntoCentroX, miComponenteInfo.getPuntoCentroY, miComponenteInfo.getRadio, miComponenteInfo.getSentGiro.ToString(), null, miComponenteInfo.getLongitud, miComponenteInfo.getPkInicial, miComponenteInfo.getPkFinal,
                miComponenteInfo.getPeralte, miComponenteInfo.getMargenI, miComponenteInfo.getMargenD, null, null));
                }
                else if (miComponenteInfo.getTipoComponente == Componente.tipoComponente.linea)
                {
                    string tipoS = "";
                    if (miComponenteInfo.getTipoRecta == EjeTrazado.tipoSegmento.giroEntradaCP) tipoS = strEje.GiroEntrada;
                    if (miComponenteInfo.getTipoRecta == EjeTrazado.tipoSegmento.giroSalidaCP) tipoS = strEje.GiroSalida;
                    if (miComponenteInfo.getTipoRecta == EjeTrazado.tipoSegmento.quedaFijo) tipoS = strEje.QuedaFijo;
                    if (miComponenteInfo.getTipoRecta == EjeTrazado.tipoSegmento.noValorado) tipoS = strEje.NoValorado;
                    if (miComponenteInfo.getTipoRecta == EjeTrazado.tipoSegmento.rectaInt) tipoS = strEje.ResctaIntermedia;
                    if (miComponenteInfo.getTipoRecta == EjeTrazado.tipoSegmento.RectaIntCurvaS) tipoS = strEje.RectaIntermediaCurvaS;
                    if (miComponenteInfo.getTipoRecta == EjeTrazado.tipoSegmento.RectaIntParalelismo) tipoS = strEje.RectaIntermediaParalelismo;

                    miInforme.Add(new oInformeEje(seg, null, strEje.Recta, tipoS, miComponenteInfo.getAzimutFinal, miComponenteInfo.getPuntoIniX, miComponenteInfo.getPuntoIniY, miComponenteInfo.getPuntoFinX, miComponenteInfo.getPuntoFinY,
           null, null, null, "", null, miComponenteInfo.getLongitud, miComponenteInfo.getPkInicial, miComponenteInfo.getPkFinal,
                miComponenteInfo.getPeralte, miComponenteInfo.getMargenI, miComponenteInfo.getMargenD, null, null));

                }
                else if (miComponenteInfo.getTipoComponente == Componente.tipoComponente.clotoideEntrada)
                {
                    seg++;
                    miInforme.Add(new oInformeEje(null, null, strEje.espiral, "", null, miComponenteInfo.getPuntoIniX, miComponenteInfo.getPuntoIniY, miComponenteInfo.getPuntoFinX, miComponenteInfo.getPuntoFinY,
           null, null, null, "", miComponenteInfo.getValorA, miComponenteInfo.getLongitud, miComponenteInfo.getPkInicial, miComponenteInfo.getPkFinal,
                null, null, null, miComponenteInfo.getVariacionMI, miComponenteInfo.getVariacionMD));
                }
                else
                {
                    miInforme.Add(new oInformeEje(null, null, strEje.espiral, "", null, miComponenteInfo.getPuntoIniX, miComponenteInfo.getPuntoIniY, miComponenteInfo.getPuntoFinX, miComponenteInfo.getPuntoFinY,
           null, null, null, "", miComponenteInfo.getValorA, miComponenteInfo.getLongitud, miComponenteInfo.getPkInicial, miComponenteInfo.getPkFinal,
                null, null, null, miComponenteInfo.getVariacionMI, miComponenteInfo.getVariacionMD));
                }
            }
                oCsv.write<oValDesT<string, string>, oInformeEje, oValDesT<string, double?>>(miLstHeader, miInforme, miLstFooter, nombre_informe);
            return miInforme;

        }

    }
}
