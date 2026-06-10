using EjeDeTrazado.componentes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;
using EjeDeTrazado.puntosDelEje;

namespace TestEjeTrazado
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para CurvaTest y se pretende que
    ///contenga todas las pruebas unitarias CurvaTest.
    ///</summary>
    [TestClass()]
    public class CurvaTest
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                //double expected = 0F; // TODO: Inicializar en un valor adecuado
                double actual;

                actual = target.getLongitud();

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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);


                double iPk = miRandom.NextDouble() * 1000;


                double expected;
                if (iSentCurva == EjeTrazado.sentidoCurva.Antihorario)
                {
                    expected = iPeralte * -1;
                }
                else
                {
                    expected = iPeralte ;
                }

                double actual;

                actual = target.getMargenDer(iPk);

                if (actual != expected)
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);


                double iPk = miRandom.NextDouble() * 1000;


                double expected;
                if (iSentCurva == EjeTrazado.sentidoCurva.Antihorario)
                {
                    expected = iPeralte * -1;
                }
                else
                {
                    expected = iPeralte ;
                }

                double actual;

                actual = target.getMargenDer(iPk);

                if (actual != expected)
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                //double expected = 0F; // TODO: Inicializar en un valor adecuado
                double actual;

                actual = target.getPeralte();

                double expected = iPeralte;

                if (actual != expected)
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                //double expected = 0F; // TODO: Inicializar en un valor adecuado
                double actual;

                actual = target.getPkFinal();

                double expected = iPkIni + target.getLongitud();

                if (actual != expected)
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                //double expected = 0F; // TODO: Inicializar en un valor adecuado
                Componente.tipoComponente actual;

                actual = target.getTipoComponente();

                Componente.tipoComponente expected = Componente.tipoComponente.curva;

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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                double actual;
                actual = target.getVariacionMD();

                double expected = 0;

                if (actual != expected)
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                double actual;
                actual = target.getVariacionMI();

                double expected = 0;

                if (actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);

        }

        /// <summary>
        ///Una prueba de getCentroCurva
        ///</summary>
        [TestMethod()]
        public void getCentroCurvaTest()
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                Punto3d actual;
                actual = target.getCentroCurva;

                Punto3d expected = new Punto3d(iCentroCurva.coordenadaX, iCentroCurva.coordenadaY, 0);

                if (!actual.Equals(expected))
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                double actual;
                actual = target.getRadio;

                double expected = iRc;

                if (actual != expected)
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
           
        }

        /// <summary>
        ///Una prueba de getSentCurva
        ///</summary>
        [TestMethod()]
        public void getSentCurvaTest()
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
                double iPeralte = miRandom.NextDouble() * 1000;
                Curva target = new Curva(iPuntoEntrada, iPuntoSalida, iCentroCurva, iRc, iPkIni, iPeralte, iPeralte, iSentCurva, 0);

                EjeTrazado.sentidoCurva actual;
                actual = target.getSentCurva;

                EjeTrazado.sentidoCurva expected = iSentCurva;

                if (!actual.ToString().Equals(expected.ToString()))
                {
                    actualBool = false;
                }

            }

            Assert.AreEqual(expectedBool, actualBool);
        }

    }
}
