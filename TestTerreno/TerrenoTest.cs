using Terrenos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;
using System.Collections.Generic;

namespace TestTerreno
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para TerrenoTest y se pretende que
    ///contenga todas las pruebas unitarias TerrenoTest.
    ///</summary>
    [TestClass()]
    public class TerrenoTest
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



        #region "isInside"
        /// <summary>
        ///Una prueba de isInside... Y dentro de los limites, pero X dentro TRUE
        ///</summary>
        [TestMethod()]
        public void isInsideTest_000()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos); 
            double iX = 100;
            double iY = 66; 
            bool expected = true; 
            bool actual;
            actual = target.isInside(iX, iY);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }



        /// <summary>
        ///Una prueba de isInside... Y dentro de los limites, pero X fuera FALSE
        ///</summary>
        [TestMethod()]
        public void isInsideTest_001()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            double iX = 1500;
            double iY = 66;
            bool expected = false;
            bool actual;
            actual = target.isInside(iX, iY);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }



        /// <summary>
        ///Una prueba de isInside... Y fuera de los limites, pero X dentro FALSE
        ///</summary>
        [TestMethod()]
        public void isInsideTest_002()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            double iX = 200;
            double iY = 1500;
            bool expected = false;
            bool actual;
            actual = target.isInside(iX, iY);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }



        /// <summary>
        ///Una prueba de isInside... Y fuera de los limites, pero X fuera FALSE
        ///</summary>
        [TestMethod()]
        public void isInsideTest_003()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            double iX = -200;
            double iY = 1500;
            bool expected = false;
            bool actual;
            actual = target.isInside(iX, iY);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }


        /// <summary>
        ///Una prueba de isInside... Y dentro de los limites, pero X limite TRUE (terreno vertical)
        ///</summary>
        [TestMethod()]
        public void isInsideTest_004()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = 500;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            double iX = 500;
            double iY = 500;
            bool expected = true;
            bool actual;
            actual = target.isInside(iX, iY);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }


        /// <summary>
        ///Una prueba de isInside... Y dentro de los limites, pero X fuera FALSE
        ///</summary>
        [TestMethod()]
        public void isInsideTest_005()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = 500;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            double iX = 501;
            double iY = 500;
            bool expected = false;
            bool actual;
            actual = target.isInside(iX, iY);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }

        #endregion



        #region "getListPuntos"

        /// <summary>
        /// getPuntosTerreno puntos repetidos
        ///</summary>
        [TestMethod()]
        public void getPuntosTerrenoTest_000()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();
            List<Punto3d> miLstPuntos2 = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
                miLstPuntos.Add(miPunto);
                if (!miLstPuntos2.Contains(miPunto)) miLstPuntos2.Add(miPunto);
                
            }
            Terreno target = new Terreno(miLstPuntos); 
            List<Punto3d> actual;
            actual = target.getPuntosTerreno;
            
            bool expected = true;
            bool real = true;

            if (miLstPuntos2.Count == actual.Count)
            {
                foreach (Punto3d miPunto in miLstPuntos2)
                {
                    if (!actual.Contains(miPunto)) real = false;
                }
            }
            else
            {
                real = false;
            }


            Assert.AreEqual(expected, real);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }


        /// <summary>
        ///Una prueba de getPuntosTerreno
        ///</summary>
        [TestMethod()]
        public void getPuntosTerrenoTest_001()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();
            List<Punto3d> miLstPuntos2 = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
                if (!miLstPuntos2.Contains(miPunto)) miLstPuntos2.Add(miPunto);

            }
            Terreno target = new Terreno(miLstPuntos);
            List<Punto3d> actual;
            actual = target.getPuntosTerreno;

            bool expected = true;
            bool real = true;

            if (miLstPuntos2.Count == actual.Count)
            {
                foreach (Punto3d miPunto in miLstPuntos2)
                {
                    if (!actual.Contains(miPunto)) real = false;
                }
            }
            else
            {
                real = false;
            }


            Assert.AreEqual(expected, real);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }


        #endregion


        #region "Limites terreno"
        /// <summary>
        ///Terreno puntos repetidos
        ///</summary>
        [TestMethod()]
        public void limitesTerrenoTest_000()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            bool expected = true;
            bool real = false;

            Punto3d[] actual;
            actual = target.limitesTerreno;

            if ((actual[0].coordenadaX == miMinX) && (actual[0].coordenadaY == miMinY) &&
                (actual[1].coordenadaX == miMaxX) && (actual[1].coordenadaY == miMinY) &&
                (actual[2].coordenadaX == miMinX) && (actual[2].coordenadaY == miMaxY) &&
                (actual[3].coordenadaX == miMaxX) && (actual[3].coordenadaY == miMaxY)) real = true;


            Assert.AreEqual(expected, real);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }



        /// <summary>
        ///Terreno normal
        ///</summary>
        [TestMethod()]
        public void limitesTerrenoTest_001()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            bool expected = true;
            bool real = false;

            Punto3d[] actual;
            actual = target.limitesTerreno;

            if ((actual[0].coordenadaX == miMinX) && (actual[0].coordenadaY == miMinY) &&
                (actual[1].coordenadaX == miMaxX) && (actual[1].coordenadaY == miMinY) &&
                (actual[2].coordenadaX == miMinX) && (actual[2].coordenadaY == miMaxY) &&
                (actual[3].coordenadaX == miMaxX) && (actual[3].coordenadaY == miMaxY)) real = true;


            Assert.AreEqual(expected, real);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }


        /// <summary>
        ///Terreno alineado horizontal
        ///</summary>
        [TestMethod()]
        public void limitesTerrenoTest_002()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = 500;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            bool expected = true;
            bool real = false;

            Punto3d[] actual;
            actual = target.limitesTerreno;

            if ((actual[0].coordenadaX == miMinX) && (actual[0].coordenadaY == miMinY) &&
                (actual[1].coordenadaX == miMaxX) && (actual[1].coordenadaY == miMinY) &&
                (actual[2].coordenadaX == miMinX) && (actual[2].coordenadaY == miMaxY) &&
                (actual[3].coordenadaX == miMaxX) && (actual[3].coordenadaY == miMaxY)) real = true;


            Assert.AreEqual(expected, real);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }


        /// <summary>
        ///Terreno alineado vertical
        ///</summary>
        [TestMethod()]
        public void limitesTerrenoTest_003()
        {
            Random miRandom = new Random();

            double miMaxX = 0;
            double miMinX = 100000;
            double miMaxY = 0;
            double miMinY = 100000;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = 500;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);

                if (miPuntoX < miMinX) miMinX = miPuntoX;
                if (miPuntoX > miMaxX) miMaxX = miPuntoX;
                if (miPuntoY < miMinY) miMinY = miPuntoY;
                if (miPuntoY > miMaxY) miMaxY = miPuntoY;

                miLstPuntos.Add(miPunto);
            }

            Terreno target = new Terreno(miLstPuntos);
            bool expected = true;
            bool real = false;

            Punto3d[] actual;
            actual = target.limitesTerreno;

            if ((actual[0].coordenadaX == miMinX) && (actual[0].coordenadaY == miMinY) &&
                (actual[1].coordenadaX == miMaxX) && (actual[1].coordenadaY == miMinY) &&
                (actual[2].coordenadaX == miMinX) && (actual[2].coordenadaY == miMaxY) &&
                (actual[3].coordenadaX == miMaxX) && (actual[3].coordenadaY == miMaxY)) real = true;


            Assert.AreEqual(expected, real);
            //Assert.Inconclusive("Compruebe la exactitud de este método de prueba.");
        }

    #endregion
 


    }
}
