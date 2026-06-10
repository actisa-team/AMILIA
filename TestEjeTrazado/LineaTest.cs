using EjeDeTrazado.componentes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;
using EjeDeTrazado.puntosDelEje;

namespace TestEjeTrazado
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para LineaTest y se pretende que
    ///contenga todas las pruebas unitarias LineaTest.
    ///</summary>
    [TestClass()]
    public class LineaTest
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
        ///Una prueba de Constructor Linea
        ///</summary>
        [TestMethod()]
        public void LineaConstructorTest()
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
                Punto3d puntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = 2;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, puntoSalida, puntoES, iPkIni, iPeralte, iAzimut);

                if (!((target.getPkIni == iPkIni) && (target.getPuntoEntrada.coordenadaX == puntoES.coordenadaX) && (target.getPuntoEntrada.coordenadaY == puntoES.coordenadaY) && (target.getPuntoSalida.coordenadaX == puntoES.coordenadaX) && (target.getPuntoSalida.coordenadaY == puntoES.coordenadaY) && (target.azimut != 0)))
                {
                    actualBool = false;
                }

            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de Constructor Linea
        ///</summary>
        [TestMethod()]
        public void LineaConstructorTest1()
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = 2;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);

                if (!((target.getPkIni == iPkIni) && (target.getPuntoEntrada.coordenadaX == iPuntoEntrada.coordenadaX) && (target.getPuntoEntrada.coordenadaY == iPuntoEntrada.coordenadaY) && (target.getPuntoSalida.coordenadaX == iPuntoSalida.coordenadaX) && (target.getPuntoSalida.coordenadaY == puntoES.coordenadaY) && (target.azimut != 0)))
                {
                    actualBool = false;
                }

            }
            Assert.AreEqual(expectedBool, actualBool);
        }

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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = 2;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);

                double expected = iPuntoEntrada.distancia2d(iPuntoSalida);
                double actual;

                actual = target.getLongitud();

                if (!((expected -0.00001< actual)&&(expected +0.00001> actual)))
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = miRandom.NextDouble() * 1000;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);

                double expected = iPeralte;
                double actual;

                actual = target.getMargenDer(miRandom.NextDouble() * 1000);

                if (!(expected == actual))
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = miRandom.NextDouble() * 1000;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);

                double expected = iPeralte * -1;
                double actual;

                actual = target.getMargenIzq(miRandom.NextDouble() * 1000);

                if (!(expected == actual))
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = miRandom.NextDouble() * 1000;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);

                double expected = iPeralte;
                double actual;

                actual = target.getPeralte();

                if (!(expected == actual))
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = miRandom.NextDouble() * 1000;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);

                double expected = iPkIni + target.getLongitud();
                double actual;

                actual = target.getPkFinal();

                if (!(expected == actual))
                {
                    actualBool = false;
                }

            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getPuntoEntradaInterseccion
        ///</summary>
        [TestMethod()]
        public void getPuntoEntradaInterseccionTest()
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = miRandom.NextDouble() * 1000;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);



                double[] actual;
                actual = target.getPuntoEntradaInterseccion();

                actual = target.getPuntoEntradaInterseccion();

                if (!((actual[0] == miPuntoX) && (actual[1] == miPuntoY)))
                {
                    actualBool = false;
                }

            }
            Assert.AreEqual(expectedBool, actualBool);

        }

        /// <summary>
        ///Una prueba de getPuntoSalidaInterseccion
        ///</summary>
        [TestMethod()]
        public void getPuntoSalidaInterseccionTest()
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = miRandom.NextDouble() * 1000;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);



                double[] actual;

                actual = target.getPuntoSalidaInterseccion();

                if (!((actual[0] == miPuntoX2) && (actual[1] == miPuntoY2)))
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = miRandom.NextDouble() * 1000;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);

                double expected = 0;
                double actual;
                actual = target.getVariacionMD();

                if (!(expected == actual))
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

                Punto3d puntoES = new Punto3d(miPuntoX2, miPuntoY2, 0);

                double iPkIni = miRandom.NextDouble() * 10000;


                double iPeralte = miRandom.NextDouble() * 1000;

                double iAzimut = 0;

                Linea target = new Linea(iPuntoEntrada, iPuntoSalida, iPkIni, iPeralte, iAzimut);

                double expected = 0;
                double actual;
                actual = target.getVariacionMI();

                if (!(expected == actual))
                {
                    actualBool = false;
                }

            }
            Assert.AreEqual(expectedBool, actualBool);


        }

    }
}
