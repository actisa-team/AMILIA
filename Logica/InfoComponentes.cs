using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EjeDeTrazado.puntosDelEje;
using EjeDeTrazado.componentes;
using tadLayShare.puntos;


namespace Logica
{
    using engNet.ClassT;
    using engNet.Csv;
    using engNet.CustomAtributos;
    using System.ComponentModel;



    public class InfoComponentes
    {
        List<InfoComponente> mComponentes = new List<InfoComponente>();

        public InfoComponentes(List<Componente> misComponentes, List<Vertice> misVertices, bool mostrarLongZero = true)
        {
            int i = 1;
            foreach (Componente miComponente in misComponentes)
            {
                if (miComponente.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                {
                    if (!(miComponente.getLongitud() == 0 && !mostrarLongZero))
                    {
                        InfoComponente miNewComp = new InfoComponente(miComponente.draw() as double[],
                            miComponente.getPkIni, miComponente.getPkFinal(), miComponente.getLongitud(),
                            miComponente.getPuntoEntrada, miComponente.getPuntoSalida);
                        miNewComp.setSentGiro = misVertices[i].getSentCurva;
                        miNewComp.setTipoCurva = misVertices[i].getTipocurva;
                        miNewComp.setPeralte = miComponente.getPeralte();
                        miNewComp.setMargenD = miComponente.getMargenDer(0);
                        miNewComp.setMargenI = miComponente.getMargenIzq(0);
                        mComponentes.Add(miNewComp);
                    }

                }
                else if (miComponente.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                {
                    if (!(miComponente.getLongitud() == 0 && !mostrarLongZero))
                    {
                        InfoComponente miNewComp = new InfoComponente(miComponente.getTipoComponente(),
                            miComponente.draw() as List<Punto3d>, miComponente.getPkIni, miComponente.getPkFinal(),
                            miComponente.getLongitud(), miComponente.getPuntoEntrada, miComponente.getPuntoSalida);
                        miNewComp.setTipoRecta = misVertices[i - 1].getTipoSeg;
                        miNewComp.setPeralte = miComponente.getPeralte();
                        miNewComp.setAzimutFinal = misVertices[i - 1].getAzimut;
                        miNewComp.setMargenD = miComponente.getMargenDer(0);
                        miNewComp.setMargenI = miComponente.getMargenIzq(0);
                        mComponentes.Add(miNewComp);
                    }
                }
                else
                {
                    if (!(miComponente.getLongitud() == 0 && !mostrarLongZero))
                    {
                        Clotoide miClo = miComponente as Clotoide;


                        InfoComponente miNewComp = new InfoComponente(miComponente.getTipoComponente(),
                            miComponente.draw() as List<Punto3d>, miComponente.getPkIni, miComponente.getPkFinal(),
                            miClo.getValorA(), miComponente.getLongitud(), miComponente.getPuntoEntrada,
                            miComponente.getPuntoSalida);
                        miNewComp.setPeralte = miComponente.getPeralte();
                        miNewComp.setVariacionMI = miClo.getVariacionMI();
                        miNewComp.setVariacionMD = miClo.getVariacionMD();
                        mComponentes.Add(miNewComp);
                    }

                    if (miComponente.getTipoComponente() == Componente.tipoComponente.clotoideSalida)
                    {
                        i++;
                    }
                }
            }
        }

        public List<InfoComponente> getInfoComponentes
        {
            get
            {
                return mComponentes;
            }

        }

        public List<oInformeEje> escribirInforme()
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
                    if (miComponenteInfo.getTipoCurva == EjeTrazado.tipoCurva.c3500) tipoC = strEje.c3500;
                    if (miComponenteInfo.getTipoCurva == EjeTrazado.tipoCurva.c7500) tipoC = strEje.c7500;
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
            //oCsv.write<oValDesT<string, string>, oInforme, oValDesT<string, double?>>(miLstHeader, miInforme, miLstFooter, @"C:\Users\pruebaEje.csv");
            return miInforme;

        }

    }
}
