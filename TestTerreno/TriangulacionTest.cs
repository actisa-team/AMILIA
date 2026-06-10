using Terrenos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;
using System.Collections.Generic;
using Terrenos.triangulos;
using System.Collections;
using System.IO;

namespace TestTerreno
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para TriangulacionTest y se pretende que
    ///contenga todas las pruebas unitarias TriangulacionTest.
    ///</summary>
    [TestClass()]
    public class TriangulacionTest
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



        #region "addAdyacente"
        /// <summary>
        ///Una prueba de addAdyacente, triangulo no existentes en la triangulacion
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void addAdyacenteTest_000()
        {
            Random miRandom = new Random();


            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 100; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            double longMax = 1000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion triangulacion = new Triangulacion(iTerreno, longMax, name, ref isEliminados);

            Hashtable misAds = triangulacion.getAdyacentes;


            PrivateObject param0 = new PrivateObject(triangulacion);
            Triangulacion_Accessor target = new Triangulacion_Accessor(param0);


            string iTrianguloA = "prueba1"; 
            string iTrianguloB = "prueba2"; 
            target.addAdyacente(iTrianguloA, iTrianguloB);

            bool expected = true;
            bool real = false;

            Hashtable miAds2 = triangulacion.getAdyacentes;

            List<string> miAdsPrueba1 = miAds2[iTrianguloA] as List<string>;
            List<string> miAdsPrueba2 = miAds2[iTrianguloB] as List<string>;

            if ((miAdsPrueba1.Count == 1) && (miAdsPrueba1[0].Equals(iTrianguloB)) && (miAdsPrueba2.Count == 1) && (miAdsPrueba2[0].Equals(iTrianguloA)))
            {
                real = true;
            }

            Assert.AreEqual(expected, real);

        }


        /// <summary>
        ///Una prueba de addAdyacente, triangulos existentes en la triangulacion
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void addAdyacenteTest_001()
        {
            Random miRandom = new Random();


            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 100; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            double longMax = 1000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion triangulacion = new Triangulacion(iTerreno, longMax, name, ref isEliminados);

            Hashtable misAds = triangulacion.getAdyacentes;


            PrivateObject param0 = new PrivateObject(triangulacion);
            Triangulacion_Accessor target = new Triangulacion_Accessor(param0);


            List<Triangulo> misTriangulos = triangulacion.getTriangulos;


            string iTrianguloA = misTriangulos[17].getHashCode;
            string iTrianguloB = misTriangulos[30].getHashCode;
            target.addAdyacente(iTrianguloA, iTrianguloB);

            bool expected = true;
            bool real = false;

            Hashtable miAds2 = triangulacion.getAdyacentes;

            List<string> miAdsPrueba1 = miAds2[iTrianguloA] as List<string>;
            List<string> miAdsPrueba2 = miAds2[iTrianguloB] as List<string>;

            if (miAdsPrueba1.Contains(iTrianguloB) && miAdsPrueba2.Contains(iTrianguloA))
            {
                real = true;
            }

            Assert.AreEqual(expected, real);

        }

        #endregion

        [TestMethod()]
        public void deserializaTriangulacion()
        {
            var serializada = oSubDMesh.getStreamCartografia2();
            serializada.Position = 0;

            InfoTriangulacion miInfo = InfoTriangulacion.recuperaInformacion(serializada);

            Assert.IsTrue(true);

        }


        #region "clasificaTriangulos"
        /// <summary>
        ///Una prueba de clasificaTriangulos
        ///</summary>
        [TestMethod()]
        public void clasificaTriangulosTest()
        {

            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            double longMax = 1000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion triangulacion = new Triangulacion(iTerreno, longMax, name, ref isEliminados);


            PrivateObject param0 = new PrivateObject(triangulacion);
            Triangulacion_Accessor target = new Triangulacion_Accessor(param0);


            double iPendienteMax = 100; // TODO: Inicializar en un valor adecuado
            List<Terrenos.triangulos.Triangulo> misGrupos = new List<Triangulo>(target.clasificaTriangulos(iPendienteMax));

            foreach (Triangulo grupo in misGrupos)
            {

                //foreach (var miVar in grupo.Values)
                //{
                //    Triangulo miTriangulo = (Triangulo)miVar;
                double miPendiente = grupo.getPendienteMaxima;

                    if (miPendiente < iPendienteMax)
                    {
                        actualBool = false;
                    }

                //}
            }



            Assert.AreEqual(expectedBool, actualBool);
        }

        #endregion

        #region "eliminarTriangulos"

        /// <summary>
        ///Una prueba de eliminarTriangulosLong
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void eliminarTriangulosLongTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            double longMax = 1000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion triangulacion = new Triangulacion(iTerreno, longMax, name, ref isEliminados);


            PrivateObject param0 = new PrivateObject(triangulacion);
            Triangulacion_Accessor target = new Triangulacion_Accessor(param0);

            double longMaxElimina = 80; // TODO: Inicializar en un valor adecuado
            bool eliminados = target.eliminarTriangulosLong(longMaxElimina);

            List<Triangulo> misTriangulos = triangulacion.getTriangulos;

            foreach (Triangulo miTriangulo in misTriangulos)
            {
                double minDist = 0;
                double dist1 = miTriangulo.getVerticeA.distancia2d(miTriangulo.getVerticeB);
                double dist2 = miTriangulo.getVerticeA.distancia2d(miTriangulo.getVerticeC);
                double dist3 = miTriangulo.getVerticeB.distancia2d(miTriangulo.getVerticeC);

                minDist = dist1;

                if (dist2 < minDist) minDist = dist2;
                if (dist3 < minDist) minDist = dist3;

                if (minDist >= longMaxElimina && eliminados)
                {
                    actualBool = false;
                }
            }

            Assert.AreEqual(expectedBool, actualBool);


        }

        #endregion


        #region "getPendiente"

        /// <summary>
        ///Una prueba de getPendiente
        ///</summary>
        [TestMethod()]
        public void getPendienteTest()
        {

            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 10; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            double longMax = 1000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion triangulacion = new Triangulacion(iTerreno, longMax, name, ref isEliminados);


            PrivateObject param0 = new PrivateObject(triangulacion);
            Triangulacion_Accessor target = new Triangulacion_Accessor(param0);


            double actual;
            double expected;


            for (int i = 0; i <= 100; i++)
            {
                Punto3d iVerticeA = new Punto3d(miRandom.NextDouble() * 1000, miRandom.NextDouble() * 1000, 0);
                Punto3d iVerticeB = new Punto3d(miRandom.NextDouble() * 1000, miRandom.NextDouble() * 1000, 0);
                Punto3d iVerticeC = new Punto3d(miRandom.NextDouble() * 1000, miRandom.NextDouble() * 1000, 0);
                Triangulo triangulo = new Triangulo(iVerticeA, iVerticeB, iVerticeC);

                if (triangulo.isTrianguloValido())
                {

                    actual = target.getPendiente(triangulo);


                    double x1 = triangulo.getVerticeA.coordenadaX;
                    double y1 = triangulo.getVerticeA.coordenadaY;
                    double z1 = triangulo.getVerticeA.coordenadaZ;

                    double x2 = triangulo.getVerticeB.coordenadaX;
                    double y2 = triangulo.getVerticeB.coordenadaY;
                    double z2 = triangulo.getVerticeB.coordenadaZ;

                    double x3 = triangulo.getVerticeC.coordenadaX;
                    double y3 = triangulo.getVerticeC.coordenadaY;
                    double z3 = triangulo.getVerticeC.coordenadaZ;



                    double normX = (y2 - y1) * (z3 - z1) - (y3 - y1) * (z2 - z1);
                    double normY = (z2 - z1) * (x3 - x1) - (z3 - z1) * (x2 - x1);
                    double normZ = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);

                    expected = Math.Abs(Math.Pow((normX * normX + normY * normY), 0.5) / normZ);

                    if (expected != actual)
                    {
                        actualBool = false;
                    }
                    
                }
            }

            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de getPendiente
        ///</summary>
        [TestMethod()]
        public void getPendienteTest1()
        {

            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            double longMax = 1000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion triangulacion = new Triangulacion(iTerreno, longMax, name, ref isEliminados);


            PrivateObject param0 = new PrivateObject(triangulacion);
            Triangulacion_Accessor target = new Triangulacion_Accessor(param0);


            double actual;
            double expected;


            for (int i = 0; i <= 100; i++)
            {
                Punto3d iPunto = new Punto3d(miRandom.NextDouble() * 1000, miRandom.NextDouble() * 1000, 0);
                Triangulo triangulo = target.getTrianguloReg(iPunto);

                if (triangulo!=null)
                {

                    actual = target.getPendiente(iPunto.coordenadaX, iPunto.coordenadaY);


                    double x1 = triangulo.getVerticeA.coordenadaX;
                    double y1 = triangulo.getVerticeA.coordenadaY;
                    double z1 = triangulo.getVerticeA.coordenadaZ;

                    double x2 = triangulo.getVerticeB.coordenadaX;
                    double y2 = triangulo.getVerticeB.coordenadaY;
                    double z2 = triangulo.getVerticeB.coordenadaZ;

                    double x3 = triangulo.getVerticeC.coordenadaX;
                    double y3 = triangulo.getVerticeC.coordenadaY;
                    double z3 = triangulo.getVerticeC.coordenadaZ;



                    double normX = (y2 - y1) * (z3 - z1) - (y3 - y1) * (z2 - z1);
                    double normY = (z2 - z1) * (x3 - x1) - (z3 - z1) * (x2 - x1);
                    double normZ = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);

                    expected = Math.Abs(Math.Pow((normX * normX + normY * normY), 0.5) / normZ);

                    if (expected != actual)
                    {
                        actualBool = false;
                    }

                }
            }

            Assert.AreEqual(expectedBool, actualBool);
        }

        #endregion


        #region "getTriangulo"
        /// <summary>
        ///Una prueba de getTriangulo
        ///</summary>
        [TestMethod()]
        public void getTrianguloTest()
        {
            Random miRandom = new Random();


            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 100;
                double miPuntoY = miRandom.NextDouble() * 100;
                double miPuntoZ = miRandom.NextDouble() * 100;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            bool expected = true;
            bool real = true;
            double longMax = 1000000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion target = new Triangulacion(iTerreno, longMax, name, ref isEliminados);

            for (int i = 0; i <= 100000; i++)
            {

                double miPuntoXi = miRandom.NextDouble() * 10;
                double miPuntoYi = miRandom.NextDouble() * 10;
                double miPuntoZi = miRandom.NextDouble() * 10;
                Punto3d iPunto = new Punto3d(miPuntoXi, miPuntoYi, miPuntoZi);

                Triangulo actual;
                actual = target.getTriangulo(iPunto);


                if (!(((actual == null)) || (actual.isTrianguloValido() && actual.isDentro(iPunto))))
                {
                    real = false;
                }
            }
            Assert.AreEqual(expected, real);
        }

        /// <summary>
        ///Una prueba de getTrianguloReg
        ///</summary>
        [TestMethod()]
        public void getTrianguloRegTest()
        {
            Random miRandom = new Random();


            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 100;
                double miPuntoY = miRandom.NextDouble() * 100;
                double miPuntoZ = miRandom.NextDouble() * 100;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            bool expected = true;
            bool real = true;
            double longMax = 1000000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion target = new Triangulacion(iTerreno, longMax, name, ref isEliminados);


            for (int i = 0; i <= 10000; i++)
            {

                double miPuntoXi = miRandom.NextDouble() * 10;
                double miPuntoYi = miRandom.NextDouble() * 10;
                double miPuntoZi = miRandom.NextDouble() * 10;
                Punto3d iPunto = new Punto3d(miPuntoXi, miPuntoYi, miPuntoZi);

                Triangulo actual1, actual2;
                actual1 = target.getTriangulo(iPunto);
                actual2 = target.getTrianguloReg(iPunto);


                if (!(((actual1 == null) && (actual2 == null)) || ((actual1.getVerticeA.Equals(actual2.getVerticeA) && (actual1.getVerticeB.Equals(actual2.getVerticeB)) && (actual1.getVerticeC.Equals(actual2.getVerticeC))))))
                {
                    real = false;
                }
            }
            Assert.AreEqual(expected, real);
        }


        #endregion


        #region "limpiarTriangulacion"
        /// <summary>
        ///Una prueba de limpiarTriangulacion
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Terrenos.dll")]
        public void limpiarTriangulacionTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;

            List<Punto3d> miLstPuntos = new List<Punto3d>();

            for (int i = 0; i <= 1000; i++)
            {
                double miPuntoX = miRandom.NextDouble() * 1000;
                double miPuntoY = miRandom.NextDouble() * 1000;
                double miPuntoZ = miRandom.NextDouble() * 1000;
                Punto3d miPunto = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                miLstPuntos.Add(miPunto);
            }

            Terreno iTerreno = new Terreno(miLstPuntos);

            List<List<Punto3d>> iLineasRotura = new List<List<Punto3d>>();



            double longMax = 1000;
            string name = "MiTerreno";
            bool isEliminados = true;
            Triangulacion triangulacion = new Triangulacion(iTerreno, longMax, name, ref isEliminados);


            PrivateObject param0 = new PrivateObject(triangulacion);
            Triangulacion_Accessor target = new Triangulacion_Accessor(param0);

            target.limpiarTriangulacion();

            Triangulo miTrianguloMax = target.getTrianguloMax();

            foreach (Triangulo miTriangulo in triangulacion.getTriangulos)
            {
                if (miTriangulo.getVerticeA.Equals(miTrianguloMax.getVerticeA) || miTriangulo.getVerticeA.Equals(miTrianguloMax.getVerticeB) || miTriangulo.getVerticeA.Equals(miTrianguloMax.getVerticeC))
                {
                    actualBool = false;
                }

                if (miTriangulo.getVerticeB.Equals(miTrianguloMax.getVerticeA) || miTriangulo.getVerticeB.Equals(miTrianguloMax.getVerticeB) || miTriangulo.getVerticeB.Equals(miTrianguloMax.getVerticeC))
                {
                    actualBool = false;
                }

                if (miTriangulo.getVerticeC.Equals(miTrianguloMax.getVerticeA) || miTriangulo.getVerticeC.Equals(miTrianguloMax.getVerticeB) || miTriangulo.getVerticeC.Equals(miTrianguloMax.getVerticeC))
                {
                    actualBool = false;
                }
            }

            Assert.AreEqual(expectedBool, actualBool);

        }

    }
        #endregion
}
