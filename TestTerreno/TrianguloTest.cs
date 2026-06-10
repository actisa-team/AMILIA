using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;
using System.Collections.Generic;
using Terrenos.triangulos;

namespace TestTerreno
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para TrianguloTest y se pretende que
    ///contenga todas las pruebas unitarias TrianguloTest.
    ///</summary>
    [TestClass()]
    public class TrianguloTest
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
        ///Una prueba de Constructor Triangulo terreno normal
        ///</summary>
        [TestMethod()]
        public void TrianguloConstructorTest_000()
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

            Terrenos.Terreno target2 = new Terrenos.Terreno(miLstPuntos);
            Punto3d[] actual;
            actual = target2.limitesTerreno;


            Punto3d iLimiteInfIqz = actual[0];
            Punto3d iLimiteInfDer = actual[1];
            Punto3d iLimiteSupIzq = actual[2];
            Punto3d iLimiteSupDer = actual[3];


            Terrenos.triangulos.Triangulo target = new Terrenos.triangulos.Triangulo(iLimiteInfIqz, iLimiteInfDer, iLimiteSupIzq, iLimiteSupDer);

            bool expected = true;
            bool real = true;

            foreach (Punto3d miPunto in target2.getPuntosTerreno)
            {
                if (!target.isDentro(miPunto)) real = false;
            }


            Assert.AreEqual(expected, real);
        }


        /// <summary>
        ///Una prueba de Constructor Triangulo alineado horizontal
        ///</summary>
        [TestMethod()]
        public void TrianguloConstructorTest_001()
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

            Terrenos.Terreno target2 = new Terrenos.Terreno(miLstPuntos);
            Punto3d[] actual;
            actual = target2.limitesTerreno;


            Punto3d iLimiteInfIqz = actual[0];
            Punto3d iLimiteInfDer = actual[1];
            Punto3d iLimiteSupIzq = actual[2];
            Punto3d iLimiteSupDer = actual[3];


            Terrenos.triangulos.Triangulo target = new Terrenos.triangulos.Triangulo(iLimiteInfIqz, iLimiteInfDer, iLimiteSupIzq, iLimiteSupDer);

            bool expected = true;
            bool real = true;

            foreach (Punto3d miPunto in target2.getPuntosTerreno)
            {
                if (!target.isDentro(miPunto)) real = false;
            }


            Assert.AreEqual(expected, real);
        }



        #region "Constructor dado tres puntos"
        /// <summary>
        ///Una prueba de Constructor Triangulo 3 puntos no alineados
        ///</summary>
        [TestMethod()]
        public void TrianguloConstructorTest1_000()
        {
            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 2, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            bool expected = true;
            bool real = target.isTrianguloValido();
            Assert.AreEqual(expected, real);
            
        }

        /// <summary>
        ///Una prueba de Constructor Triangulo 2 puntos iguales y uno diferente
        ///</summary>
        [TestMethod()]
        public void TrianguloConstructorTest1_001()
        {
            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 2, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            bool expected = false;
            bool real = target.isTrianguloValido();
            Assert.AreEqual(expected, real);

        }


        /// <summary>
        ///Una prueba de Constructor Triangulo 3 puntos alineados
        ///</summary>
        [TestMethod()]
        public void TrianguloConstructorTest1_002()
        {
            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 3, 0);
            Punto3d iVerticeC = new Punto3d(1, 2, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            bool expected = false;
            bool real = target.isTrianguloValido();
            Assert.AreEqual(expected, real);

        }

        #endregion

        #region "Alineados"

        /// <summary>
        ///Una prueba de alineados 3 puntos no alineados
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void alineadosTest_000()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 2, 0);
            Triangulo target1 = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(target1); 
            Triangulo_Accessor target = new Triangulo_Accessor(param0);
            Punto3d iPunto1 = new Punto3d(1, 1, 0);
            Punto3d iPunto2 = new Punto3d(2, 4, 0);
            Punto3d iPunto3 = new Punto3d(4, 2, 0);

            bool expected = false;
            bool actual;
            actual = target.alineados(iPunto1, iPunto2, iPunto3);
            Assert.AreEqual(expected, actual);
        }

                /// <summary>
        ///Una prueba de alineados 2 puntos iguales y uno diferente
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void alineadosTest_001()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 2, 0);
            Triangulo target1 = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(target1);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);
            Punto3d iPunto1 = new Punto3d(1, 1, 0);
            Punto3d iPunto2 = new Punto3d(1, 1, 0);
            Punto3d iPunto3 = new Punto3d(4, 2, 0);

            bool expected = true;
            bool actual;
            actual = target.alineados(iPunto1, iPunto2, iPunto3);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de alineados 3 puntos alineados 
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void alineadosTest_002()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 2, 0);
            Triangulo target1 = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(target1);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);
            Punto3d iPunto1 = new Punto3d(-5, 3, 0);
            Punto3d iPunto2 = new Punto3d(-1, -3, 0);
            Punto3d iPunto3 = new Punto3d(1, -6, 0);

            bool expected = true;
            bool actual;
            actual = target.alineados(iPunto1, iPunto2, iPunto3);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de alineados 3 puntos iguales
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void alineadosTest_003()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(1, 1, 0);
            Triangulo target1 = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(target1);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);
            Punto3d iPunto1 = new Punto3d(-5, 3, 0);
            Punto3d iPunto2 = new Punto3d(-1, -3, 0);
            Punto3d iPunto3 = new Punto3d(1, -6, 0);

            bool expected = true;
            bool actual;
            actual = target.alineados(iPunto1, iPunto2, iPunto3);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region "getCota"
        /// <summary>
        ///Una prueba de getCota, cualquier punto, misma cota
        ///</summary>
        [TestMethod()]
        public void getCotaTest_000()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 3);
            Punto3d iVerticeB = new Punto3d(2, 4, 3);
            Punto3d iVerticeC = new Punto3d(4, 2, 3);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            double iX = 2; 
            double iY = 2.5;
            double expected = 3;
            double actual;
            actual = target.getCota(iX, iY);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getCota, vertice, misma cota
        ///</summary>
        [TestMethod()]
        public void getCotaTest_001()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 3);
            Punto3d iVerticeB = new Punto3d(2, 4, 3);
            Punto3d iVerticeC = new Punto3d(4, 2, 3);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            double iX = 1;
            double iY = 1;
            double expected = 3;
            double actual;
            actual = target.getCota(iX, iY);
            Assert.AreEqual(expected, actual);
        }



        /// <summary>
        ///Una prueba de getCota, cualquier punto
        ///</summary>
        [TestMethod()]
        public void getCotaTest_002()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 1);
            Punto3d iVerticeB = new Punto3d(2, 4, 2);
            Punto3d iVerticeC = new Punto3d(4, 2, 4);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            double iX = 2.7;
            double iY = 2.5;
            double expected = 2.7;
            double actual;
            actual = target.getCota(iX, iY);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getCota, vertice
        ///</summary>
        [TestMethod()]
        public void getCotaTest_003()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 1);
            Punto3d iVerticeB = new Punto3d(2, 4, 2);
            Punto3d iVerticeC = new Punto3d(4, 2, 4);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            double iX = 4;
            double iY = 2;
            double expected = 4;
            double actual;
            actual = target.getCota(iX, iY);
            Assert.AreEqual(expected, actual);
        }

        #endregion


        #region "getIndice"
        /// <summary>
        ///Una prueba de getIndice, menor en X (posicion 2) (diferentes X)
        ///</summary>
        [TestMethod()]
        public void getIndiceTest_000()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(1, 1, 0);
            int expected = 2;
            int actual;
            actual = target.getIndice(iPunto);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getIndice, punto intermedio (posicion 1)
        ///</summary>
        [TestMethod()]
        public void getIndiceTest_001()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(2, 4, 0);
            int expected = 1;
            int actual;
            actual = target.getIndice(iPunto);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de getIndice, punto mayor X (posicion 0)
        ///</summary>
        [TestMethod()]
        public void getIndiceTest_002()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(4, 5, 0);
            int expected = 0;
            int actual;
            actual = target.getIndice(iPunto);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de getIndice, mismo X menor Y (posicion 2)
        ///</summary>
        [TestMethod()]
        public void getIndiceTest_003()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 4, 0);
            Punto3d iVerticeC = new Punto3d(3, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(1, 1, 0);
            int expected = 2;
            int actual;
            actual = target.getIndice(iPunto);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///Una prueba de getIndice, mismo X mayor Y (posicion 1)
        ///</summary>
        [TestMethod()]
        public void getIndiceTest_004()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 4, 0);
            Punto3d iVerticeC = new Punto3d(3, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(1, 4, 0);
            int expected = 1;
            int actual;
            actual = target.getIndice(iPunto);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getIndice, mayor X (posicion 0)
        ///</summary>
        [TestMethod()]
        public void getIndiceTest_005()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 4, 0);
            Punto3d iVerticeC = new Punto3d(3, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(3, 5, 0);
            int expected = 0;
            int actual;
            actual = target.getIndice(iPunto);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getIndice, punto que no es un vertice
        ///</summary>
        [TestMethod()]
        public void getIndiceTest_006()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 4, 0);
            Punto3d iVerticeC = new Punto3d(3, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(2, 2, 0);
            int expected = -1;
            int actual;
            actual = target.getIndice(iPunto);
            Assert.AreEqual(expected, actual);
        } 
        
        /// <summary>
        ///Una prueba de getIndice, triangulo no valido
        ///</summary>
        [TestMethod()]
        public void getIndiceTest_007()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(1, 1, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(2, 2, 0);
            int expected = -1;
            int actual;
            actual = target.getIndice(iPunto);
            Assert.AreEqual(expected, actual);
        }

        #endregion


        #region "getLadoComun"
        /// <summary>
        ///Una prueba de getLadoComun, con un lado en comun TRUE
        ///</summary>
        [TestMethod()]
        public void getLadoComunTest_000()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(1, 1, 0);
            Punto3d iVerticeB1 = new Punto3d(2, 4, 0);
            Punto3d iVerticeC1 = new Punto3d(7, 5, 0);
            Triangulo iTriangulo = new Triangulo(iVerticeA1, iVerticeB1, iVerticeC1);


            Punto3d[] expected = new Punto3d[2];
            expected[0] = new Punto3d(1, 1, 0);
            expected[1] = new Punto3d(2, 4, 0);
            Punto3d[] actual;
            actual = target.getLadoComun(iTriangulo);

            bool expectedbool = true;
            bool actualbool = false;
            if (((expected[0].CompareTo(actual[0]) == 0) && (expected[1].CompareTo(actual[1]) == 0)) || ((expected[1].CompareTo(actual[0]) == 0) && (expected[0].CompareTo(actual[1]) == 0)))
            {
                actualbool = true;
            }
            Assert.AreEqual(expectedbool, actualbool);
        }



        /// <summary>
        ///Una prueba de getLadoComun, con un punto en comun FALSE
        ///</summary>
        [TestMethod()]
        public void getLadoComunTest_001()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(0, 0, 0);
            Punto3d iVerticeB1 = new Punto3d(2, 4, 0);
            Punto3d iVerticeC1 = new Punto3d(7, 5, 0);
            Triangulo iTriangulo = new Triangulo(iVerticeA1, iVerticeB1, iVerticeC1);


            Punto3d[] actual;
            actual = target.getLadoComun(iTriangulo);

            bool expectedbool = false;
            bool actualbool = true;
            if (actual[0] == null && actual[1]==null)
            {
                actualbool = false;
            }
            Assert.AreEqual(expectedbool, actualbool);
        }


        /// <summary>
        ///Una prueba de getLadoComun, con ningun punto en comun FALSE
        ///</summary>
        [TestMethod()]
        public void getLadoComunTest_002()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(0, 0, 0);
            Punto3d iVerticeB1 = new Punto3d(4, 4, 0);
            Punto3d iVerticeC1 = new Punto3d(7, 5, 0);
            Triangulo iTriangulo = new Triangulo(iVerticeA1, iVerticeB1, iVerticeC1);


            Punto3d[] actual;
            actual = target.getLadoComun(iTriangulo);

            bool expectedbool = false;
            bool actualbool = true;
            if (actual[0] == null && actual[1] == null)
            {
                actualbool = false;
            }
            Assert.AreEqual(expectedbool, actualbool);
        }

        /// <summary>
        ///Una prueba de getLadoComun, con ningun punto en comun FALSE
        ///</summary>
        [TestMethod()]
        public void getLadoComunTest_003()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(0, 0, 0);
            Punto3d iVerticeB1 = new Punto3d(0, 0, 0);
            Punto3d iVerticeC1 = new Punto3d(7, 5, 0);
            Triangulo iTriangulo = new Triangulo(iVerticeA1, iVerticeB1, iVerticeC1);


            Punto3d[] actual;
            actual = target.getLadoComun(iTriangulo);

            bool expectedbool = false;
            bool actualbool = true;
            if (actual[0] == null && actual[1] == null)
            {
                actualbool = false;
            }
            Assert.AreEqual(expectedbool, actualbool);
        }

        #endregion

        #region "getPendiente"

        /// <summary>
        ///Una prueba de getPendiente, dos puntos con la misma y
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPendienteTest_000()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            Punto3d iPuntoA = new Punto3d(3, 1, 0);
            Punto3d iPuntoB = new Punto3d(1, 1, 0);
            double expected = 0;
            double actual;

            actual = target.getPendiente(iPuntoA, iPuntoB);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de getPendiente, dos puntos con la misma x
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPendienteTest_001()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            Punto3d iPuntoA = new Punto3d(3, 1, 0);
            Punto3d iPuntoB = new Punto3d(3, 5, 0);
            double expected = double.NegativeInfinity;
            double actual;

            actual = target.getPendiente(iPuntoA, iPuntoB);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de getPendiente, dos puntos iguales
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPendienteTest_002()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            Punto3d iPuntoA = new Punto3d(3, 1, 0);
            Punto3d iPuntoB = new Punto3d(3, 5, 0);
            double expected = double.NegativeInfinity;
            double actual;

            actual = target.getPendiente(iPuntoA, iPuntoB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getPendiente, dos puntos 
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPendienteTest_003()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            Punto3d iPuntoA = new Punto3d(3, 1, 0);
            Punto3d iPuntoB = new Punto3d(2, 4, 0);
            double expected = -3;
            double actual;

            actual = target.getPendiente(iPuntoA, iPuntoB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getPendiente, dos puntos (al reves)
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPendienteTest_004()
        {


            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            Punto3d iPuntoA = new Punto3d(3, 1, 0);
            Punto3d iPuntoB = new Punto3d(2, 4, 0);
            double expected = -3;
            double actual;

            actual = target.getPendiente(iPuntoB, iPuntoA);
            Assert.AreEqual(expected, actual);
        }

        #endregion


        #region "getPunto"
        /// <summary>
        ///Una prueba de getPunto, dado su posicion (el mayor constuido el segundo)
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoTest_000()
        {

            Punto3d iVerticeA = new Punto3d(1, 3, 0);
            Punto3d iVerticeC = new Punto3d(2, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            int iIndice = 0;

            Punto3d expected = new Punto3d(4, 5, 0);
            Punto3d actual;
            actual = target.getPunto(iIndice);

            bool expectedB = true;
            bool actualB = false;

            if (expected.CompareTo(actual) == 0)
            {
                actualB = true;
            }
            Assert.AreEqual(expectedB, actualB);
        }

        /// <summary>
        ///Una prueba de getPunto, dado su posicion (el menor constuido el ultimo)
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoTest_001()
        {

            Punto3d iVerticeC = new Punto3d(1, 3, 0);
            Punto3d iVerticeA = new Punto3d(2, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            int iIndice = 2;

            Punto3d expected = new Punto3d(1, 3, 0);
            Punto3d actual;
            actual = target.getPunto(iIndice);

            bool expectedB = true;
            bool actualB = false;

            if (expected.CompareTo(actual) == 0)
            {
                actualB = true;
            }
            Assert.AreEqual(expectedB, actualB);
        }


        /// <summary>
        ///Una prueba de getPunto, dado su posicion (medio construido el primero)
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoTest_002()
        {

            Punto3d iVerticeC = new Punto3d(1, 3, 0);
            Punto3d iVerticeA = new Punto3d(2, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            int iIndice = 1;

            Punto3d expected = new Punto3d(2, 3, 0);
            Punto3d actual;
            actual = target.getPunto(iIndice);

            bool expectedB = true;
            bool actualB = false;

            if (expected.CompareTo(actual) == 0)
            {
                actualB = true;
            }
            Assert.AreEqual(expectedB, actualB);
        }


        /// <summary>
        ///Una prueba de getPunto, dado su posicion (medio construido el primero)
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoTest_003()
        {

            Punto3d iVerticeC = new Punto3d(1, 3, 0);
            Punto3d iVerticeA = new Punto3d(2, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            int iIndice = 10;

            Punto3d expected = null;
            Punto3d actual;
            actual = target.getPunto(iIndice);

            
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region "getPuntoMedio"

        /// <summary>
        ///Una prueba de getPuntoMedio, el mismo punto
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoMedioTest_000()
        {

            Punto3d iVerticeA = new Punto3d(1, 3, 0);
            Punto3d iVerticeC = new Punto3d(2, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0); 

            int iIndiceA = 0;
            int iIndiceB = 0;
            Punto3d expected = null; 
            Punto3d actual;
            actual = target.getPuntoMedio(iIndiceA, iIndiceB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getPuntoMedio, indice fuera de rango
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoMedioTest_001()
        {

            Punto3d iVerticeA = new Punto3d(1, 3, 0);
            Punto3d iVerticeC = new Punto3d(2, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            int iIndiceA = 0;
            int iIndiceB = 10;
            Punto3d expected = null;
            Punto3d actual;
            actual = target.getPuntoMedio(iIndiceA, iIndiceB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getPuntoMedio, indices correctos 
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoMedioTest_002()
        {

            Punto3d iVerticeA = new Punto3d(1, 3, 0);
            Punto3d iVerticeC = new Punto3d(2, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            int iIndiceA = 1;
            int iIndiceB = 2;
            Punto3d expected = new Punto3d(1.5, 3, 0);
            Punto3d actual;
            actual = target.getPuntoMedio(iIndiceA, iIndiceB);


            bool expectedB = true;
            bool actualB = false;

            if (expected.CompareTo(actual) == 0)
            {
                actualB = true;
            }
            Assert.AreEqual(expectedB, actualB);

        }


        /// <summary>
        ///Una prueba de getPuntoMedio, indices correctos, orden al reves
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoMedioTest_003()
        {

            Punto3d iVerticeA = new Punto3d(1, 3, 0);
            Punto3d iVerticeC = new Punto3d(2, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            int iIndiceA = 2;
            int iIndiceB = 1;
            Punto3d expected = new Punto3d(1.5, 3, 0);
            Punto3d actual;
            actual = target.getPuntoMedio(iIndiceA, iIndiceB);


            bool expectedB = true;
            bool actualB = false;

            if (expected.CompareTo(actual) == 0)
            {
                actualB = true;
            }
            Assert.AreEqual(expectedB, actualB);

        }


        /// <summary>
        ///Una prueba de getPuntoMedio, indices correctos, orden al reves
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getPuntoMedioTest_004()
        {

            Punto3d iVerticeA = new Punto3d(1, 3, 0);
            Punto3d iVerticeC = new Punto3d(1, 3, 0);
            Punto3d iVerticeB = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            PrivateObject param0 = new PrivateObject(triangulo);
            Triangulo_Accessor target = new Triangulo_Accessor(param0);

            int iIndiceA = 0;
            int iIndiceB = 1;
            Punto3d expected = null;
            Punto3d actual;
            actual = target.getPuntoMedio(iIndiceA, iIndiceB);
            Assert.AreEqual(expected, actual);

        }
#endregion

        #region "getVertice"

        /// <summary>
        ///Una prueba de getVertice, puntos que no son vertices
        ///</summary>
        [TestMethod()]
        public void getVerticeTest_000()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(2, 1, 0);
            Punto3d iVerticeB1 = new Punto3d(7, 1, 0);

            Punto3d expected = null; 
            Punto3d actual;
            actual = target.getVertice(iVerticeA1, iVerticeB1);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de getVertice, dos puntos iguales (pertenecientes a los vértices) 
        ///</summary>
        [TestMethod()]
        public void getVerticeTest_001()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(2, 4, 0);
            Punto3d iVerticeB1 = new Punto3d(2, 4, 0);

            Punto3d expected = null;
            Punto3d actual;
            actual = target.getVertice(iVerticeA1, iVerticeB1);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de getVertice, un punto pertenece y el segundo no
        ///</summary>
        [TestMethod()]
        public void getVerticeTest_002()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(2, 4, 0);
            Punto3d iVerticeB1 = new Punto3d(2, 5, 0);

            Punto3d expected = null;
            Punto3d actual;
            actual = target.getVertice(iVerticeA1, iVerticeB1);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de getVertice, los dos pertenecen
        ///</summary>
        [TestMethod()]
        public void getVerticeTest_003()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(2, 4, 0);
            Punto3d iVerticeB1 = new Punto3d(4, 5, 0);

            Punto3d expected = new Punto3d(1, 1, 0);
            Punto3d actual;
            actual = target.getVertice(iVerticeA1, iVerticeB1);



            bool expectedB = true;
            bool actualB = false;

            if (expected.CompareTo(actual) == 0)
            {
                actualB = true;
            }
            Assert.AreEqual(expectedB, actualB);
        }



        /// <summary>
        ///Una prueba de getVertice, los dos pertenecen, al reves
        ///</summary>
        [TestMethod()]
        public void getVerticeTest_004()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(2, 4, 0);
            Punto3d iVerticeB1 = new Punto3d(4, 5, 0);

            Punto3d expected = new Punto3d(1, 1, 0);
            Punto3d actual;
            actual = target.getVertice(iVerticeB1, iVerticeA1);



            bool expectedB = true;
            bool actualB = false;

            if (expected.CompareTo(actual) == 0)
            {
                actualB = true;
            }
            Assert.AreEqual(expectedB, actualB);
        }


        /// <summary>
        ///Una prueba de getVertice, triangulo no valido
        ///</summary>
        [TestMethod()]
        public void getVerticeTest_005()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iVerticeA1 = new Punto3d(2, 4, 0);
            Punto3d iVerticeB1 = new Punto3d(2, 5, 0);

            Punto3d expected = null;
            Punto3d actual;
            actual = target.getVertice(iVerticeA1, iVerticeB1);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region "isDentro"

        /// <summary>
        ///Una prueba de isDentro, uno de los vertices
        ///</summary>
        [TestMethod()]
        public void isDentroTest_000()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(4, 5, 0);
            bool expected = true;
            bool actual;
            actual = target.isDentro(iPunto);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de isDentro, punto fuera 
        ///</summary>
        [TestMethod()]
        public void isDentroTest_001()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(0, 0, 0);
            bool expected = false;
            bool actual;
            actual = target.isDentro(iPunto);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de isDentro, punto dentro 
        ///</summary>
        [TestMethod()]
        public void isDentroTest_002()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 4, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(2, 3, 0);
            bool expected = true;
            bool actual;
            actual = target.isDentro(iPunto);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de isDentro, punto en un lado 
        ///</summary>
        [TestMethod()]
        public void isDentroTest_003()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(2, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(1.5, 1, 0);
            bool expected = true;
            bool actual;
            actual = target.isDentro(iPunto);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Una prueba de isDentro, triangulo no valido
        ///</summary>
        [TestMethod()]
        public void isDentroTest_004()
        {

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d iPunto = new Punto3d(1.5, 1, 0);
            bool expected = false;
            bool actual;
            actual = target.isDentro(iPunto);
            Assert.AreEqual(expected, actual);
        }

        #endregion


        #region "getCentro"
        /// <summary>
        ///Una prueba de getCentro, triangulos aleatorios
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getCentroTest_000()
        {


            Random miRandom = new Random();

            bool expected = true;
            bool real = true;

            for (int i = 0; i <= 1000000; i++)
            {
                double miPuntoXA = miRandom.NextDouble() * 1000;
                double miPuntoYA = miRandom.NextDouble() * 1000;
                double miPuntoZA = miRandom.NextDouble() * 1000;
                Punto3d miPuntoA = new Punto3d(miPuntoXA, miPuntoYA, miPuntoZA);


                double miPuntoXB = miRandom.NextDouble() * 1000;
                double miPuntoYB = miRandom.NextDouble() * 1000;
                double miPuntoZB = miRandom.NextDouble() * 1000;
                Punto3d miPuntoB = new Punto3d(miPuntoYB, miPuntoYB, miPuntoZB);


                double miPuntoXC = miRandom.NextDouble() * 1000;
                double miPuntoYC = miRandom.NextDouble() * 1000;
                double miPuntoZC = miRandom.NextDouble() * 1000;
                Punto3d miPuntoC = new Punto3d(miPuntoXC, miPuntoYC, miPuntoZC);


                Punto3d iVerticeA = miPuntoA;
                Punto3d iVerticeB = miPuntoB;
                Punto3d iVerticeC = miPuntoC;

                Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

                PrivateObject param0 = new PrivateObject(triangulo);
                Triangulo_Accessor target = new Triangulo_Accessor(param0); // TODO: Inicializar en un valor adecuado
                Punto3d actual;
                actual = target.getCentro;

                double distA = actual.distancia2d(iVerticeA);
                double distB = actual.distancia2d(iVerticeB);
                double distC = actual.distancia2d(iVerticeC);

                bool igualAB = (distA < distB + 0.000001 && distA > distB - 0.000001);
                bool igualBC = (distB < distC + 0.000001 && distB > distC - 0.000001);

                if (!(igualAB && igualBC))
                {
                    real = false;
                }

            }

            Assert.AreEqual(expected, real);


        }

        /// <summary>
        ///Una prueba de getCentro, triangulo no valido
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void getCentroTest_001()
        {

            bool expected = true;
            bool real = false;
            

            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


                PrivateObject param0 = new PrivateObject(triangulo);
                Triangulo_Accessor target = new Triangulo_Accessor(param0); // TODO: Inicializar en un valor adecuado
                Punto3d actual;
                actual = target.getCentro;


                if (actual==null)
                {
                    real = true;
                }


            Assert.AreEqual(expected, real);


        }

        #endregion


        #region "isDentroCircunferencia"
        /// <summary>
        ///Una prueba de isDentroCircunferencia, triangulos aleatorios
        ///</summary>
        [TestMethod()]
        public void isDentroCircunferenciaTest_000()
        {

            

            Random miRandom = new Random();

            bool expected = true;
            bool real = true;

            for (int i = 0; i <= 1000000; i++)
            {
                double miPuntoXA = miRandom.NextDouble() * 10;
                double miPuntoYA = miRandom.NextDouble() * 10;
                double miPuntoZA = miRandom.NextDouble() * 10;
                Punto3d miPuntoA = new Punto3d(miPuntoXA, miPuntoYA, miPuntoZA);


                double miPuntoXB = miRandom.NextDouble() * 10;
                double miPuntoYB = miRandom.NextDouble() * 10;
                double miPuntoZB = miRandom.NextDouble() * 10;
                Punto3d miPuntoB = new Punto3d(miPuntoYB, miPuntoYB, miPuntoZB);


                double miPuntoXC = miRandom.NextDouble() * 10;
                double miPuntoYC = miRandom.NextDouble() * 10;
                double miPuntoZC = miRandom.NextDouble() * 10;
                Punto3d miPuntoC = new Punto3d(miPuntoXC, miPuntoYC, miPuntoZC);

                Triangulo target = new Triangulo(miPuntoA, miPuntoB, miPuntoC);

                
                PrivateObject param0 = new PrivateObject(target);
                Triangulo_Accessor privado = new Triangulo_Accessor(param0); // TODO: Inicializar en un valor adecuado
                Punto3d centro;
                centro = privado.getCentro;

                Punto3d iPunto = new Punto3d(5, 5, 0);
                expected = true;
                bool actual;
                actual = target.isDentroCircunferencia(iPunto);



                bool isDentro = (centro.distancia2d(miPuntoA) > centro.distancia2d(iPunto));


                if (!(isDentro == actual))
                {
                    real = false;
                }
            }


            Assert.AreEqual(expected, real);
        }

                /// <summary>
        ///Una prueba de isDentroCircunferencia, triangulo no valido
        ///</summary>
        [TestMethod()]
        public void isDentroCircunferenciaTest_001()
        {
            bool expected = false;
            
            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);
                
                PrivateObject param0 = new PrivateObject(triangulo);
                Triangulo_Accessor privado = new Triangulo_Accessor(param0); // TODO: Inicializar en un valor adecuado
                Punto3d centro;
                centro = privado.getCentro;

                Punto3d iPunto = new Punto3d(5, 5, 0);
                bool actual;
                actual = triangulo.isDentroCircunferencia(iPunto);
                Assert.AreEqual(expected, actual);
        }
        #endregion

        #region "isEnLado"
        /// <summary>
        ///Una prueba de isEnLado, punto si esta en un lado
        ///</summary>
        [TestMethod()]
        public void isEnLadoTest_000()
        {
            Punto3d iVerticeA = new Punto3d(2, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);
            Punto3d iPunto = new Punto3d(1.5, 1, 0);

            bool expected = true;
            bool real = false;
           
            Punto3d[] actual;
            actual = target.isEnLado(iPunto);

            if (((actual[0].CompareTo(iVerticeA) == 0) && (actual[1].CompareTo(iVerticeB) == 0)) || ((actual[1].CompareTo(iVerticeA) == 0) && (actual[0].CompareTo(iVerticeB) == 0)))
            {
                real = true;
            }
            Assert.AreEqual(expected, real);
        }

        /// <summary>
        ///Una prueba de isEnLado, punto dentro del triangulo
        ///</summary>
        [TestMethod()]
        public void isEnLadoTest_001()
        {
            Punto3d iVerticeA = new Punto3d(2, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);
            Punto3d iPunto = new Punto3d(1.5, 1.3, 0);

            bool expected = true;
            bool real = false;

            Punto3d[] actual;
            actual = target.isEnLado(iPunto);

            if (((actual[0]==null) && (actual[1]==null)))
            {
                real = true;
            }
            Assert.AreEqual(expected, real);
        }

        /// <summary>
        ///Una prueba de isEnLado, punto fuera del triangulo
        ///</summary>
        [TestMethod()]
        public void isEnLadoTest_002()
        {
            Punto3d iVerticeA = new Punto3d(2, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);
            Punto3d iPunto = new Punto3d(1.5, 6, 0);

            bool expected = true;
            bool real = false;

            Punto3d[] actual;
            actual = target.isEnLado(iPunto);

            if (((actual[0] == null) && (actual[1] == null)))
            {
                real = true;
            }
            Assert.AreEqual(expected, real);
        }


        /// <summary>
        ///Una prueba de isEnLado, punto fuera del triangulo
        ///</summary>
        [TestMethod()]
        public void isEnLadoTest_003()
        {
            Punto3d iVerticeA = new Punto3d(1, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);
            Punto3d iPunto = new Punto3d(1.5, 6, 0);

            bool expected = true;
            bool real = false;

            Punto3d[] actual;
            actual = target.isEnLado(iPunto);

            if (((actual[0] == null) && (actual[1] == null)))
            {
                real = true;
            }
            Assert.AreEqual(expected, real);
        }

        #endregion

        #region "getPendienteMaxima"

        /// <summary>
        ///Una prueba de getPendienteMaxima, misma cota
        ///</summary>
        [TestMethod()]
        public void getPendienteMaximaTest_000()
        {
            Punto3d iVerticeA = new Punto3d(2, 1, 0);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            double expected = 0;
            double actual;
            actual = target.getPendienteMaxima;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getPendienteMaxima, diferentes cotas
        ///</summary>
        [TestMethod()]
        public void getPendienteMaximaTest_001()
        {
            Punto3d iVerticeA = new Punto3d(2, 1, 100);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            double expected = 125;
            double actual;
            actual = target.getPendienteMaxima;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Una prueba de getPendienteMaxima, triangulo no valido
        ///</summary>
        [TestMethod()]
        public void getPendienteMaximaTest_002()
        {
            Punto3d iVerticeA = new Punto3d(1, 1, 100);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

            double expected = double.PositiveInfinity;
            double actual;
            actual = target.getPendienteMaxima;

            Assert.AreEqual(expected, actual);
        }

        #endregion


        #region "getVerticeA"


        /// <summary>
        ///Una prueba de getVerticeA, triangulos diferentes cotas
        ///</summary>
        [TestMethod()]
        public void getVerticeATest_000()
        {

            Random miRandom = new Random();

            bool expected = true;
            bool real = true;

            for (int i = 0; i <= 1000000; i++)
            {
                double miPuntoXA = miRandom.NextDouble() * 1000;
                double miPuntoYA = miRandom.NextDouble() * 1000;
                double miPuntoZA = miRandom.NextDouble() * 1000;
                Punto3d miPuntoA = new Punto3d(miPuntoXA, miPuntoYA, miPuntoZA);


                double miPuntoXB = miRandom.NextDouble() * 1000;
                double miPuntoYB = miRandom.NextDouble() * 1000;
                double miPuntoZB = miRandom.NextDouble() * 1000;
                Punto3d miPuntoB = new Punto3d(miPuntoYB, miPuntoYB, miPuntoZB);


                double miPuntoXC = miRandom.NextDouble() * 1000;
                double miPuntoYC = miRandom.NextDouble() * 1000;
                double miPuntoZC = miRandom.NextDouble() * 1000;
                Punto3d miPuntoC = new Punto3d(miPuntoXC, miPuntoYC, miPuntoZC);


                Punto3d iVerticeA = miPuntoA;
                Punto3d iVerticeB = miPuntoB;
                Punto3d iVerticeC = miPuntoC;

                Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

                PrivateObject param0 = new PrivateObject(target);
                Triangulo_Accessor privado = new Triangulo_Accessor(param0); // TODO: Inicializar en un valor adecuado

                Punto3d actual;
                actual = target.getVerticeA;

                Punto3d miPunto0 = privado.getPunto(0);

                if (!(actual.CompareTo(miPunto0) == 0))
                {
                    real = false;
                }

            }
            Assert.AreEqual(expected, real);
        }


        /// <summary>
        ///Una prueba de getVerticeA,- triangulo no valido
        ///</summary>
        [TestMethod()]
        public void getVerticeATest_001()
        {

            Random miRandom = new Random();

            bool expected = true;
            bool real = true;

            Punto3d iVerticeA = new Punto3d(1, 1, 100);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d actual;
            actual = target.getVerticeA;


            if (!(actual == null))
            {
                real = false;
            }


            Assert.AreEqual(expected, real);
        }

        #endregion

        #region "getVerticeB"

        /// <summary>
        ///Una prueba de getVerticeA, triangulos diferentes cotas
        ///</summary>
        [TestMethod()]
        public void getVerticeBTest_000()
        {

            Random miRandom = new Random();

            bool expected = true;
            bool real = true;

            for (int i = 0; i <= 1000000; i++)
            {
                double miPuntoXA = miRandom.NextDouble() * 1000;
                double miPuntoYA = miRandom.NextDouble() * 1000;
                double miPuntoZA = miRandom.NextDouble() * 1000;
                Punto3d miPuntoA = new Punto3d(miPuntoXA, miPuntoYA, miPuntoZA);


                double miPuntoXB = miRandom.NextDouble() * 1000;
                double miPuntoYB = miRandom.NextDouble() * 1000;
                double miPuntoZB = miRandom.NextDouble() * 1000;
                Punto3d miPuntoB = new Punto3d(miPuntoYB, miPuntoYB, miPuntoZB);


                double miPuntoXC = miRandom.NextDouble() * 1000;
                double miPuntoYC = miRandom.NextDouble() * 1000;
                double miPuntoZC = miRandom.NextDouble() * 1000;
                Punto3d miPuntoC = new Punto3d(miPuntoXC, miPuntoYC, miPuntoZC);


                Punto3d iVerticeA = miPuntoA;
                Punto3d iVerticeB = miPuntoB;
                Punto3d iVerticeC = miPuntoC;

                Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

                PrivateObject param0 = new PrivateObject(target);
                Triangulo_Accessor privado = new Triangulo_Accessor(param0); // TODO: Inicializar en un valor adecuado

                Punto3d actual;
                actual = target.getVerticeB;

                Punto3d miPunto1 = privado.getPunto(1);

                if (!(actual.CompareTo(miPunto1) == 0))
                {
                    real = false;
                }

            }
            Assert.AreEqual(expected, real);
        }


        /// <summary>
        ///Una prueba de getVerticeA,- triangulo no valido
        ///</summary>
        [TestMethod()]
        public void getVerticeBTest_001()
        {

            Random miRandom = new Random();

            bool expected = true;
            bool real = true;

            Punto3d iVerticeA = new Punto3d(1, 1, 100);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d actual;
            actual = target.getVerticeB;


            if (!(actual == null))
            {
                real = false;
            }


            Assert.AreEqual(expected, real);
        }
        #endregion

        #region "getVerticeC"


        /// <summary>
        ///Una prueba de getVerticeA, triangulos diferentes cotas
        ///</summary>
        [TestMethod()]
        public void getVerticeCTest_000()
        {

            Random miRandom = new Random();

            bool expected = true;
            bool real = true;

            for (int i = 0; i <= 1000000; i++)
            {
                double miPuntoXA = miRandom.NextDouble() * 1000;
                double miPuntoYA = miRandom.NextDouble() * 1000;
                double miPuntoZA = miRandom.NextDouble() * 1000;
                Punto3d miPuntoA = new Punto3d(miPuntoXA, miPuntoYA, miPuntoZA);


                double miPuntoXB = miRandom.NextDouble() * 1000;
                double miPuntoYB = miRandom.NextDouble() * 1000;
                double miPuntoZB = miRandom.NextDouble() * 1000;
                Punto3d miPuntoB = new Punto3d(miPuntoYB, miPuntoYB, miPuntoZB);


                double miPuntoXC = miRandom.NextDouble() * 1000;
                double miPuntoYC = miRandom.NextDouble() * 1000;
                double miPuntoZC = miRandom.NextDouble() * 1000;
                Punto3d miPuntoC = new Punto3d(miPuntoXC, miPuntoYC, miPuntoZC);


                Punto3d iVerticeA = miPuntoA;
                Punto3d iVerticeB = miPuntoB;
                Punto3d iVerticeC = miPuntoC;

                Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

                PrivateObject param0 = new PrivateObject(target);
                Triangulo_Accessor privado = new Triangulo_Accessor(param0); // TODO: Inicializar en un valor adecuado

                Punto3d actual;
                actual = target.getVerticeC;

                Punto3d miPunto2 = privado.getPunto(2);

                if (!(actual.CompareTo(miPunto2) == 0))
                {
                    real = false;
                }

            }
            Assert.AreEqual(expected, real);
        }


        /// <summary>
        ///Una prueba de getVerticeA,- triangulo no valido
        ///</summary>
        [TestMethod()]
        public void getVerticeCTest_001()
        {

            Random miRandom = new Random();

            bool expected = true;
            bool real = true;

            Punto3d iVerticeA = new Punto3d(1, 1, 100);
            Punto3d iVerticeB = new Punto3d(1, 1, 0);
            Punto3d iVerticeC = new Punto3d(4, 5, 0);
            Triangulo target = new Triangulo(iVerticeA, iVerticeB, iVerticeC);


            Punto3d actual;
            actual = target.getVerticeC;


            if (!(actual == null))
            {
                real = false;
            }


            Assert.AreEqual(expected, real);
        }


        #endregion
    }
}
