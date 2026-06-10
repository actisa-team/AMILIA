using PerfilLongitudinal.componentes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;

namespace TestPerfilLongitudinal
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para RectaTest y se pretende que
    ///contenga todas las pruebas unitarias RectaTest.
    ///</summary>
    [TestClass()]
    public class RectaTest
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
        ///Una prueba de getPendiente
        ///</summary>
        [TestMethod()]
        public void getPendienteTest()
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


                double iPkIni = miRandom.NextDouble() * 1000;


                Recta target = new Recta(iPuntoEntrada, puntoSalida, iPkIni);

                double expected = (puntoSalida.coordenadaY - iPuntoEntrada.coordenadaY) / (puntoSalida.coordenadaX - iPuntoEntrada.coordenadaX);
                if ((puntoSalida.coordenadaX - iPuntoEntrada.coordenadaX) == 0) expected = 0;


                double actual;
                actual = target.getPendiente();


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
                Punto3d puntoSalida = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);


                double iPkIni = miRandom.NextDouble() * 1000;


                Recta target = new Recta(iPuntoEntrada, puntoSalida, iPkIni);

                ComponenteLong.tipoComponente expected = ComponenteLong.tipoComponente.recta;


                ComponenteLong.tipoComponente actual;
                actual = target.getTipoComponente();

                if (!actual.ToString().Equals(expected.ToString()))
                {
                    actualBool = false;
                }
            }
            Assert.AreEqual(expectedBool, actualBool);

        }
    }
}
