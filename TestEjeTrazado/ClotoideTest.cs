using EjeDeTrazado.componentes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;
using EjeDeTrazado.puntosDelEje;

namespace TestEjeTrazado
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para ClotoideTest y se pretende que
    ///contenga todas las pruebas unitarias ClotoideTest.
    ///</summary>
    [TestClass()]
    public class ClotoideTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Obtiene o establece el contexto de la prueba que proporciona
        ///la información y funcionalidad para la ejecución de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Atributos de prueba adicionales
        // 
        //Puede utilizar los siguientes atributos adicionales mientras escribe sus pruebas:
        //
        //Use ClassInitialize para ejecutar código antes de ejecutar la primera prueba en la clase 
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup para ejecutar código después de haber ejecutado todas las pruebas en una clase
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize para ejecutar código antes de ejecutar cada prueba
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup para ejecutar código después de que se hayan ejecutado todas las pruebas
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Una prueba de getLongitud
        ///</summary>
        [TestMethod()]
        public void getLongitudTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;
                double a;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                double expected;
                if(isCurvaS)
                {
                    expected = miLe;
                }
                else
                {
                    a = target.getValorA();
                    expected = target.getValorA() * target.getValorA() / iRc;
                }
                double actual;
                
                
                actual = target.getLongitud();

                if (!((double.IsNaN(actual)) && (double.IsNaN(expected))) && actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);

        }

        /// <summary>
        ///Una prueba de getMargenDer
        ///</summary>
        [TestMethod()]
        public void getMargenDerTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);



                double iPk = miRandom.NextDouble() * 1000;
                double expected = (iPk - iPkIni) * target.getVariacionMD() + iPeralteAnt;


                if (isSigCurva)
                {
                    expected = (iPk - iPkIni) * target.getVariacionMD() + iPeralteAnt;
                }
                else
                {
                    if (target.isHorario())
                    {
                        expected = (iPk - iPkIni) * target.getVariacionMD() + iPeralteAnt;
                    }
                    else
                    {
                        expected = (iPk - iPkIni) * target.getVariacionMD() + iPeralteAnt * -1;
                    }
                }


                double actual;
                actual = target.getMargenDer(iPk);

                if (!((double.IsNaN(actual)) && (double.IsNaN(expected))) && actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
            
        }

        /// <summary>
        ///Una prueba de getMargenIzq
        ///</summary>
        [TestMethod()]
        public void getMargenIzqTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);



                double iPk = miRandom.NextDouble() * 1000;
                double expected = (iPk - iPkIni) * target.getVariacionMI() + iPeralteAnt;


                if (isSigCurva)
                {
                    expected = (iPk - iPkIni) * target.getVariacionMI() + iPeralteAnt * -1;
                }
                else
                {
                    if (target.isHorario())
                    {
                        expected = (iPk - iPkIni) * target.getVariacionMI() + iPeralteAnt;
                    }
                    else
                    {
                        expected = (iPk - iPkIni) * target.getVariacionMI() + iPeralteAnt * -1;
                    }
                }


                double actual;
                actual = target.getMargenIzq(iPk);

                if (!((double.IsNaN(actual)) && (double.IsNaN(expected))) && actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
            
        }

        /// <summary>
        ///Una prueba de getPeralte
        ///</summary>
        [TestMethod()]
        public void getPeralteTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;
                double a;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                double expected = 0;


                double actual;
                actual = target.getPeralte();

                if (!((double.IsNaN(actual)) && (double.IsNaN(expected))) && actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getPkFinal
        ///</summary>
        [TestMethod()]
        public void getPkFinalTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;
                double a;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                double expected = iPkIni + target.getLongitud();


                double actual;
                actual = target.getPkFinal();

                if (!((double.IsNaN(actual)) && (double.IsNaN(expected))) && actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);

        }



        /// <summary>
        ///Una prueba de getQe
        ///</summary>
        [TestMethod()]
        public void getQeTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                double expected = 0.5 * Math.Pow((target.getLongitud() / target.getValorA()), 2);


                double actual;
                actual = target.getQe();

                if (!((double.IsNaN(actual)) && (double.IsNaN(expected))) && actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getTipoComponente
        ///</summary>
        [TestMethod()]
        public void getTipoComponenteTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;
                double a;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                Componente.tipoComponente expected;

                if (isSigCurva)
                {
                    expected = Componente.tipoComponente.clotoideEntrada;
                }
                else
                {
                    expected = Componente.tipoComponente.clotoideSalida;
                }


                Componente.tipoComponente actual;
                actual = target.getTipoComponente();

                if (!actual.ToString().Equals(expected.ToString()))
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
        }


        /// <summary>
        ///Una prueba de getVariacionMD
        ///</summary>
        [TestMethod()]
        public void getVariacionMDTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                double expected;

                if (isSigCurva)
                {
                    if (target.isHorario())
                    {
                        expected = (iPeraltePos - iPeralteAnt) / target.getLongitud();
                    }
                    else
                    {
                        expected = ((iPeraltePos * -1) - iPeralteAnt) / target.getLongitud();
                    }
                }
                else
                {
                    if (target.isHorario())
                    {
                        expected = (iPeraltePos - iPeralteAnt) / target.getLongitud();
                    }
                    else
                    {
                        expected = (iPeraltePos - (iPeralteAnt * -1)) / target.getLongitud();
                    }
                }


                double actual;
                actual = target.getVariacionMD();

                if (!((double.IsNaN(actual)) && (double.IsNaN(expected))) && actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getVariacionMI
        ///</summary>
        [TestMethod()]
        public void getVariacionMITest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                double expected;

                if (isSigCurva)
                {
                    if (target.isHorario())
                    {
                        expected = (iPeraltePos - iPeralteAnt * (-1)) / target.getLongitud();
                    }
                    else
                    {
                        expected = ((iPeraltePos * -1) - iPeralteAnt * (-1)) / target.getLongitud();
                    }
                }
                else
                {
                    if (target.isHorario())
                    {
                        expected = (iPeraltePos * (-1) - iPeralteAnt) / target.getLongitud();
                    }
                    else
                    {
                        expected = (iPeraltePos * (-1) - (iPeralteAnt * -1)) / target.getLongitud();
                    }
                }


                double actual;
                actual = target.getVariacionMI();

                if (!((double.IsNaN(actual)) && (double.IsNaN(expected))) && actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
            
        }


        /// <summary>
        ///Una prueba de isHorario
        ///</summary>
        [TestMethod()]
        public void isHorarioTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;
                double a;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                bool expected = (iSentCurva == EjeTrazado.sentidoCurva.Horario);


                bool actual;
                actual = target.isHorario();

                if (actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);

        }

        /// <summary>
        ///Una prueba de curvaS
        ///</summary>
        [TestMethod()]
        public void curvaSTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;
                double a;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                bool expected = isCurvaS;


                bool actual;
                actual = target.curvaS;

                if (actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getRadio
        ///</summary>
        [TestMethod()]
        public void getRadioTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);

                double expected = iRc;


                double actual;
                actual = target.getRadio;

                if (actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool); 
        }

        /// <summary>
        ///Una prueba de getTipo
        ///</summary>
        [TestMethod()]
        public void getTipoTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX2 = miRandom.NextDouble() * 1000;
                double miPuntoY2 = miRandom.NextDouble() * 1000;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double miPuntoX3 = miRandom.NextDouble() * 1000;
                double miPuntoY3 = miRandom.NextDouble() * 1000;
                double miPuntoZ3 = miRandom.NextDouble() * 1000;


                Punto3d iCentroCurva = new Punto3d(miPuntoX3, miPuntoY3, miPuntoZ3);

                double iRc = miRandom.NextDouble() * 1000;

                double iPkIni = miRandom.NextDouble() * 1000;

                EjeTrazado.sentidoCurva iSentCurva;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iSentCurva = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {

                    iSentCurva = EjeTrazado.sentidoCurva.Horario;
                }


                bool isSigCurva;
                EjeTrazado.tipoClotoide iTipoClotoide;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isSigCurva = true;
                    iTipoClotoide = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {

                    isSigCurva = false;
                    iTipoClotoide = EjeTrazado.tipoClotoide.salida;
                }

                bool iReducido;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    iReducido = true;
                }
                else
                {
                    iReducido = false;
                }


                bool isCurvaS;

                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isCurvaS = true;
                }
                else
                {
                    isCurvaS = false;
                }



                double iPeralteAnt = miRandom.NextDouble() * 1000;
                double iPeraltePos = miRandom.NextDouble() * 1000;

                double iAzimut = miRandom.NextDouble() * 1000;
                double iDelta = miRandom.NextDouble() * 1000;

                double miLe = miRandom.NextDouble() * 1000;

                Clotoide target = new Clotoide(iPuntoEntrada, iPuntoSalida, iRc, iPkIni, iSentCurva, iPeralteAnt, iPeraltePos, isSigCurva, iTipoClotoide, iAzimut, iReducido, iDelta, isCurvaS, miLe);


                EjeTrazado.tipoClotoide expected;
                if (isSigCurva)
                {
                    expected = EjeTrazado.tipoClotoide.entrada;
                }
                else
                {
                    expected = EjeTrazado.tipoClotoide.salida;
                }


                EjeTrazado.tipoClotoide actual;
                actual = target.getTipo;

                if (!actual.ToString().Equals(expected.ToString()))
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool); 
        }
    }
}
