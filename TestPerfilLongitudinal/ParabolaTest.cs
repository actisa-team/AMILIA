using PerfilLongitudinal.componentes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;

namespace TestPerfilLongitudinal
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para ParabolaTest y se pretende que
    ///contenga todas las pruebas unitarias ParabolaTest.
    ///</summary>
    [TestClass()]
    public class ParabolaTest
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
        ///Una prueba de getCotaAcuerdo
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PerfilLongitudinal.dll")]
        public void getCotaAcuerdoTest()
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


                double iPkIni = miRandom.NextDouble() * 1000;
                double iPendienteEntr = miRandom.NextDouble() * 1000;
                double iKv = miRandom.NextDouble() * 1000;
                double iCambioPendi = miRandom.NextDouble() * 1000;

                bool isConcavo;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isConcavo = false;
                }
                else
                {
                    isConcavo = true;
                }

                PrivateObject param0 = new PrivateObject(new Parabola(iPuntoEntrada, iPuntoSalida, iPkIni, iPendienteEntr, iKv, iCambioPendi, isConcavo));

                Parabola_Accessor target = new Parabola_Accessor(param0);
                double iX = miRandom.NextDouble() * 1000;



                double expected;
                if (isConcavo)
                {
                    expected = (iPuntoEntrada.coordenadaY + (iPendienteEntr * (iX - iPuntoEntrada.coordenadaX)) + (Math.Pow(iX - iPuntoEntrada.coordenadaX, 2) / (2 * iKv)));
                }
                else
                {
                    expected = (iPuntoEntrada.coordenadaY + (iPendienteEntr * (iX - iPuntoEntrada.coordenadaX)) + (Math.Pow(iX - iPuntoEntrada.coordenadaX, 2) / (2 * iKv * (-1))));
                }

                double actual;
                actual = target.getCotaAcuerdo(iX);


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


                double iPkIni = miRandom.NextDouble() * 1000;
                double iPendienteEntr = miRandom.NextDouble() * 1000;
                double iKv = miRandom.NextDouble() * 1000;
                double iCambioPendi = miRandom.NextDouble() * 1000;

                bool isConcavo;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isConcavo = false;
                }
                else
                {
                    isConcavo = true;
                }

                Parabola target = new Parabola(iPuntoEntrada, iPuntoSalida, iPkIni, iPendienteEntr, iKv, iCambioPendi, isConcavo);



                ComponenteLong.tipoComponente expected = ComponenteLong.tipoComponente.parabola;
                ComponenteLong.tipoComponente actual;
                actual = target.getTipoComponente();

                if (!actual.ToString().Equals(expected.ToString()))
                {
                    actualBool = false;
                }

            }
            Assert.AreEqual(expectedBool, actualBool);

        }

        /// <summary>
        ///Una prueba de getTipoParabola
        ///</summary>
        [TestMethod()]
        public void getTipoParabolaTest()
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


                double iPkIni = miRandom.NextDouble() * 1000;
                double iPendienteEntr = miRandom.NextDouble() * 1000;
                double iKv = miRandom.NextDouble() * 1000;
                double iCambioPendi = miRandom.NextDouble() * 1000;

                bool isConcavo;
                if (miRandom.NextDouble() * 1000 > 500)
                {
                    isConcavo = false;
                }
                else
                {
                    isConcavo = true;
                }

                Parabola target = new Parabola(iPuntoEntrada, iPuntoSalida, iPkIni, iPendienteEntr, iKv, iCambioPendi, isConcavo);



                Parabola.tipoParabola expected;
                if (isConcavo)
                {
                    expected = Parabola.tipoParabola.concavo;
                }
                else
                {
                    expected = Parabola.tipoParabola.convexo;
                }

                Parabola.tipoParabola actual;
                actual = target.getTipoParabola();

                if (!actual.ToString().Equals(expected.ToString()))
                {
                    actualBool = false;
                }

            }
            Assert.AreEqual(expectedBool, actualBool);


        }
    }
}
