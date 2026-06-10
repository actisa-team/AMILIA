using PerfilLongitudinal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;

namespace TestPerfilLongitudinal
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para GuitarraTest y se pretende que
    ///contenga todas las pruebas unitarias GuitarraTest.
    ///</summary>
    [TestClass()]
    public class GuitarraTest
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
        ///Una prueba de getEscalaAlto
        ///</summary>
        [TestMethod()]
        public void getEscalaAltoTest()
        {
            
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoOrigen = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                double minX = miRandom.NextDouble() * 1000;
                double maxX = miRandom.NextDouble() * 1000;
                double minZ = miRandom.NextDouble() * 1000;
                double maxZ = miRandom.NextDouble() * 1000;

                int escalaAncho = (int)miRandom.NextDouble() * 1000;
                int escalaAlto = (int)miRandom.NextDouble() * 1000;

                Guitarra target = new Guitarra(minX, maxX, minZ, maxZ, iPuntoOrigen, escalaAncho, escalaAlto);

                int actual;
                actual = target.getEscalaAlto;

                int expected = escalaAlto;

                if (actual != expected)
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getEscalaAncho
        ///</summary>
        [TestMethod()]
        public void getEscalaAnchoTest()
        {

            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoOrigen = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                double minX = miRandom.NextDouble() * 1000;
                double maxX = miRandom.NextDouble() * 1000;
                double minZ = miRandom.NextDouble() * 1000;
                double maxZ = miRandom.NextDouble() * 1000;

                int escalaAncho = (int)miRandom.NextDouble() * 1000;
                int escalaAlto = (int)miRandom.NextDouble() * 1000;

                Guitarra target = new Guitarra(minX, maxX, minZ, maxZ, iPuntoOrigen, escalaAncho, escalaAlto);

                int actual;
                actual = target.getEscalaAncho;

                int expected = escalaAncho;

                if (actual != expected)
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getMaxX
        ///</summary>
        [TestMethod()]
        public void getMaxXTest()
        {

            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoOrigen = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                double minX = miRandom.NextDouble() * 1000;
                double maxX = miRandom.NextDouble() * 1000;
                double minZ = miRandom.NextDouble() * 1000;
                double maxZ = miRandom.NextDouble() * 1000;

                int escalaAncho = (int)miRandom.NextDouble() * 1000;
                int escalaAlto = (int)miRandom.NextDouble() * 1000;

                Guitarra target = new Guitarra(minX, maxX, minZ, maxZ, iPuntoOrigen, escalaAncho, escalaAlto);

                double actual;
                actual = target.getMaxX;


                double difX = maxX - minX;
                int mCuadradosAncho = (int)(difX / escalaAncho) + 1;
                double expected = iPuntoOrigen.coordenadaX + mCuadradosAncho * 100;

                if (actual != expected)
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getMaxY
        ///</summary>
        [TestMethod()]
        public void getMaxYTest()
        {

            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoOrigen = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                double minX = miRandom.NextDouble() * 1000;
                double maxX = miRandom.NextDouble() * 1000;
                double minZ = miRandom.NextDouble() * 1000;
                double maxZ = miRandom.NextDouble() * 1000;

                int escalaAncho = (int)miRandom.NextDouble() * 1000;
                int escalaAlto = (int)miRandom.NextDouble() * 1000;

                Guitarra target = new Guitarra(minX, maxX, minZ, maxZ, iPuntoOrigen, escalaAncho, escalaAlto);

                double actual;
                actual = target.getMaxY;


                double difZ = maxZ - minZ;
                int mCuadradosAlto = (int)(difZ / escalaAlto) + 6;
                double expected = iPuntoOrigen.coordenadaY + mCuadradosAlto * 100;

                if (actual != expected)
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getMinX
        ///</summary>
        [TestMethod()]
        public void getMinXTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoOrigen = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                double minX = miRandom.NextDouble() * 1000;
                double maxX = miRandom.NextDouble() * 1000;
                double minZ = miRandom.NextDouble() * 1000;
                double maxZ = miRandom.NextDouble() * 1000;

                int escalaAncho = (int)miRandom.NextDouble() * 1000;
                int escalaAlto = (int)miRandom.NextDouble() * 1000;

                Guitarra target = new Guitarra(minX, maxX, minZ, maxZ, iPuntoOrigen, escalaAncho, escalaAlto);

                double actual;
                actual = target.getMinX;

                double expected = minX;

                if (actual != expected)
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getMinY
        ///</summary>
        [TestMethod()]
        public void getMinYTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoOrigen = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                double minX = miRandom.NextDouble() * 1000;
                double maxX = miRandom.NextDouble() * 1000;
                double minZ = miRandom.NextDouble() * 1000;
                double maxZ = miRandom.NextDouble() * 1000;

                int escalaAncho = (int)miRandom.NextDouble() * 1000;
                int escalaAlto = (int)miRandom.NextDouble() * 1000;

                Guitarra target = new Guitarra(minX, maxX, minZ, maxZ, iPuntoOrigen, escalaAncho, escalaAlto);

                double actual;
                actual = target.getMinY;

                double expected = minZ;

                if (actual != expected)
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getPuntoOrigX
        ///</summary>
        [TestMethod()]
        public void getPuntoOrigXTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoOrigen = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                double minX = miRandom.NextDouble() * 1000;
                double maxX = miRandom.NextDouble() * 1000;
                double minZ = miRandom.NextDouble() * 1000;
                double maxZ = miRandom.NextDouble() * 1000;

                int escalaAncho = (int)miRandom.NextDouble() * 1000;
                int escalaAlto = (int)miRandom.NextDouble() * 1000;

                Guitarra target = new Guitarra(minX, maxX, minZ, maxZ, iPuntoOrigen, escalaAncho, escalaAlto);

                double actual;
                actual = target.getPuntoOrigX;

                double expected = iPuntoOrigen.coordenadaX;

                if (actual != expected)
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);

        }

        /// <summary>
        ///Una prueba de getPuntoOrigY
        ///</summary>
        [TestMethod()]
        public void getPuntoOrigYTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            for (int i = 0; i <= 10000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d iPuntoOrigen = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                double minX = miRandom.NextDouble() * 1000;
                double maxX = miRandom.NextDouble() * 1000;
                double minZ = miRandom.NextDouble() * 1000;
                double maxZ = miRandom.NextDouble() * 1000;

                int escalaAncho = (int)miRandom.NextDouble() * 1000;
                int escalaAlto = (int)miRandom.NextDouble() * 1000;

                Guitarra target = new Guitarra(minX, maxX, minZ, maxZ, iPuntoOrigen, escalaAncho, escalaAlto);

                double actual;
                actual = target.getPuntoOrigY;

                double expected = iPuntoOrigen.coordenadaY;

                if (actual != expected)
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);
        }
    }
}
