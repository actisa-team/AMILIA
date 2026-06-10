using EjeDeTrazado.puntosDelEje;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tadLayShare.puntos;
using System.Collections.Generic;
using EjeDeTrazado;
using EjeDeTrazado.componentes;
using System.IO;
using engNet;
using System.Data;

namespace TestEjeTrazado
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para EjeTrazadoTest y se pretende que
    ///contenga todas las pruebas unitarias EjeTrazadoTest.
    ///</summary>
    [TestClass()]
    public class EjeTrazadoTest
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
        ///Una prueba de addCurvaGranRadio
        ///</summary>
        [TestMethod()]
        public void addCurvaGranRadioTest()
        {
             Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;


            
            List<Punto3d> iPolilinea = new List<Punto3d>();

                double miPuntoX = 907.01;
                double miPuntoY = 730.36;
                double miPuntoZ = 130.17;
                Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


                double miPuntoX1 = 367.52;
                double miPuntoY1 = 991.57;
                double miPuntoZ1 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada1 = new Punto3d(miPuntoX1, miPuntoY1, miPuntoZ1);

                double miPuntoX2 = 829.18;
                double miPuntoY2 = 339.5;
                double miPuntoZ2 = miRandom.NextDouble() * 1000;
                Punto3d iPuntoEntrada2 = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);

                iPolilinea.Add(iPuntoEntrada);
                iPolilinea.Add(iPuntoEntrada1);
                iPolilinea.Add(iPuntoEntrada2);
            
            int iGrupo = 2;

            double prefCur = miRandom.NextDouble() * 1000;
            bool iPrefCurvas = false;
            if (prefCur > 500) iPrefCurvas = true;

            double iRadio = miRandom.NextDouble() * 1000;
            double iVelocidad = miRandom.NextDouble() * 120;
            double iPeralteCurva = miRandom.NextDouble() * 1000;
            double iPeralteRecta = miRandom.NextDouble() * 1000;


            double constante = miRandom.NextDouble() * 1000;
            bool iConstante = false;
            if (constante > 500) iConstante = true;
            EjeTrazado target = new EjeTrazado(iPolilinea, iGrupo, iPrefCurvas, iRadio, iVelocidad, iPeralteCurva, iPeralteRecta, iConstante); 


            oCsvLoad misResultadosCSV = new oCsvLoad("C:\\Users\\Angeles\\Documents\\excel\\Excel\\Resultados\\Res2500y5000.csv", ";", "what?", true);
            DataTable misDatosTabla = new DataTable();

            misDatosTabla.Columns.Add("Radio", typeof(double));
            misDatosTabla.Columns.Add("A1", typeof(double));
            misDatosTabla.Columns.Add("A2", typeof(double));
            misDatosTabla.Columns.Add("P1x", typeof(double));
            misDatosTabla.Columns.Add("P1y", typeof(double));
            misDatosTabla.Columns.Add("SentGiro", typeof(string));
            misDatosTabla.Columns.Add("T1x", typeof(double));
            misDatosTabla.Columns.Add("T1y", typeof(double));
            misDatosTabla.Columns.Add("T2x", typeof(double));
            misDatosTabla.Columns.Add("T2y", typeof(double));
            misDatosTabla.Columns.Add("Pcx", typeof(double));
            misDatosTabla.Columns.Add("Pcy", typeof(double));

            misResultadosCSV.loadCsvIntoDataTableTyped(misDatosTabla);

            for (int i = 0; i < misDatosTabla.Rows.Count; i++)
            {
                double iRc = (double)misDatosTabla.Rows[i][0];
                double iAzimut1 = (double)misDatosTabla.Rows[i][1];
                double iAzimut2 = (double)misDatosTabla.Rows[i][2];
                Punto3d iVertice = new Punto3d((double)misDatosTabla.Rows[i][3], (double)misDatosTabla.Rows[i][4], 0);
                EjeTrazado.sentidoCurva sentG;
                string sentidoCurva = (string)misDatosTabla.Rows[i][5];
                if (sentidoCurva.Equals("Antihorario"))
                {
                    sentG = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    sentG = EjeTrazado.sentidoCurva.Horario;
                }

                double T1x = (double)misDatosTabla.Rows[i][6];
                double T1y = (double)misDatosTabla.Rows[i][7];
                double T2x = (double)misDatosTabla.Rows[i][8];
                double T2y = (double)misDatosTabla.Rows[i][9];
                double Pcx = (double)misDatosTabla.Rows[i][10];
                double Pcy = (double)misDatosTabla.Rows[i][11];


                Punto3d[] actual;
                actual = target.addCurvaGranRadio(iRc, iAzimut1, iAzimut2, iVertice, sentG);

                if (!((actual[0].coordenadaX > T1x - 0.001) && (actual[0].coordenadaX < T1x + 0.001)
                    && (actual[0].coordenadaY > T1y - 0.001) && (actual[0].coordenadaY < T1y + 0.001)
                    && (actual[2].coordenadaX > T2x - 0.001) && (actual[2].coordenadaX < T2x + 0.001)
                    && (actual[2].coordenadaY > T2y - 0.001) && (actual[2].coordenadaY < T2y + 0.001)
                    && (actual[4].coordenadaX > Pcx - 0.001) && (actual[4].coordenadaX < Pcx + 0.001)
                    && (actual[4].coordenadaY > Pcy - 0.001) && (actual[4].coordenadaY < Pcy + 0.001)))
                {
                    actualBool = false;
                }


            }

            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de addCurvaNoPaso
        ///</summary>
        [TestMethod()]
        public void addCurvaNoPasoTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            List<Punto3d> iPolilinea = new List<Punto3d>();

            double miPuntoX = 907.01;
            double miPuntoY = 730.36;
            double miPuntoZ = 130.17;
            Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


            double miPuntoX1 = 367.52;
            double miPuntoY1 = 991.57;
            double miPuntoZ1 = miRandom.NextDouble() * 1000;
            Punto3d iPuntoEntrada1 = new Punto3d(miPuntoX1, miPuntoY1, miPuntoZ1);

            double miPuntoX2 = 829.18;
            double miPuntoY2 = 339.5;
            double miPuntoZ2 = miRandom.NextDouble() * 1000;
            Punto3d iPuntoEntrada2 = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);

            iPolilinea.Add(iPuntoEntrada);
            iPolilinea.Add(iPuntoEntrada1);
            iPolilinea.Add(iPuntoEntrada2);

            int iGrupo = 2;

            double prefCur = miRandom.NextDouble() * 1000;
            bool iPrefCurvas = false;
            if (prefCur > 500) iPrefCurvas = true;

            double iRadio = miRandom.NextDouble() * 1000;
            double iVelocidad = miRandom.NextDouble() * 120;
            double iPeralteCurva = miRandom.NextDouble() * 1000;
            double iPeralteRecta = miRandom.NextDouble() * 1000;


            double constante = miRandom.NextDouble() * 1000;
            bool iConstante = false;
            if (constante > 500) iConstante = true;
            EjeTrazado target = new EjeTrazado(iPolilinea, iGrupo, iPrefCurvas, iRadio, iVelocidad, iPeralteCurva, iPeralteRecta, iConstante);


            oCsvLoad misResultadosCSV = new oCsvLoad("C:\\Users\\Angeles\\Documents\\excel\\Excel\\Resultados\\ResCnp.csv", ";", "what?", true);
            DataTable misDatosTabla = new DataTable();

            misDatosTabla.Columns.Add("Radio", typeof(double));
            misDatosTabla.Columns.Add("A1", typeof(double));
            misDatosTabla.Columns.Add("A2", typeof(double));
            misDatosTabla.Columns.Add("P2x", typeof(double));
            misDatosTabla.Columns.Add("P2y", typeof(double));
            misDatosTabla.Columns.Add("Pcx", typeof(double));
            misDatosTabla.Columns.Add("Pcy", typeof(double));
            misDatosTabla.Columns.Add("SentGiro", typeof(string));
            misDatosTabla.Columns.Add("Px1x", typeof(double));
            misDatosTabla.Columns.Add("Px1y", typeof(double));
            misDatosTabla.Columns.Add("Pcl1x", typeof(double));
            misDatosTabla.Columns.Add("Pcl1y", typeof(double));
            misDatosTabla.Columns.Add("Pc1x", typeof(double));
            misDatosTabla.Columns.Add("Pc1y", typeof(double));
            misDatosTabla.Columns.Add("Px2x", typeof(double));
            misDatosTabla.Columns.Add("Px2y", typeof(double));
            misDatosTabla.Columns.Add("Pc2x", typeof(double));
            misDatosTabla.Columns.Add("Pc2y", typeof(double));
            misDatosTabla.Columns.Add("Pcl2x", typeof(double));
            misDatosTabla.Columns.Add("Pcl2y", typeof(double));

            misResultadosCSV.loadCsvIntoDataTableTyped(misDatosTabla);

            for (int i = 0; i < misDatosTabla.Rows.Count; i++)
            {
                double iRc = (double)misDatosTabla.Rows[i][0];
                double iAzimut1 = (double)misDatosTabla.Rows[i][1];
                double iAzimut2 = (double)misDatosTabla.Rows[i][2];
                Punto3d iVertice = new Punto3d((double)misDatosTabla.Rows[i][3], (double)misDatosTabla.Rows[i][4], 0);
                Punto3d iVerticeAnt = iVertice;
                EjeTrazado.sentidoCurva sentG;
                string sentidoCurva = (string)misDatosTabla.Rows[i][7];
                if (sentidoCurva.Equals("Antihorario"))
                {
                    sentG = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    sentG = EjeTrazado.sentidoCurva.Horario;
                }


                double Px1x = (double)misDatosTabla.Rows[i][8];
                double Px1y = (double)misDatosTabla.Rows[i][9];
                double Pcl1x = (double)misDatosTabla.Rows[i][10];
                double  Pcl1y = (double)misDatosTabla.Rows[i][11];
                double Pc1x = (double)misDatosTabla.Rows[i][12];
                double Pc1y = (double)misDatosTabla.Rows[i][13];
                double Px2x = (double)misDatosTabla.Rows[i][14];
                double Px2y = (double)misDatosTabla.Rows[i][15];
                double Pc2x = (double)misDatosTabla.Rows[i][16];
                double Pc2y = (double)misDatosTabla.Rows[i][17];
                double Pcl2x = (double)misDatosTabla.Rows[i][18];
                double Pcl2y = (double)misDatosTabla.Rows[i][19];

                bool iReducido = false;

                Punto3d[] actual;
                double miA = 0;
                actual = target.addCurvaNoPaso(ref iRc, iAzimut1, iAzimut2, iVerticeAnt, iVertice, sentG, iReducido, out miA);

                if (!((actual[0].coordenadaX > Pcl1x - 0.001) && (actual[0].coordenadaX < Pcl1x + 0.001)
                    && (actual[0].coordenadaY > Pcl1y - 0.001) && (actual[0].coordenadaY < Pcl1y + 0.001)
                    && (actual[1].coordenadaX > Pc1x - 0.001) && (actual[1].coordenadaX < Pc1x + 0.001)
                    && (actual[1].coordenadaY > Pc1y - 0.001) && (actual[1].coordenadaY < Pc1y + 0.001)
                    && (actual[2].coordenadaX > Pc2x - 0.001) && (actual[2].coordenadaX < Pc2x + 0.001)
                    && (actual[2].coordenadaY > Pc2y - 0.001) && (actual[2].coordenadaY < Pc2y + 0.001)
                    && (actual[3].coordenadaX > Pcl2x - 0.001) && (actual[3].coordenadaX < Pcl2x + 0.001)
                    && (actual[3].coordenadaY > Pcl2y - 0.001) && (actual[3].coordenadaY < Pcl2y + 0.001)))
                {
                    actualBool = false;
                }


            }
            Assert.AreEqual(expectedBool, actualBool);


        }

        /// <summary>
        ///Una prueba de addCurvaPaso
        ///</summary>
        [TestMethod()]
        public void addCurvaPasoTest()
        {

            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            List<Punto3d> iPolilinea = new List<Punto3d>();

            double miPuntoX = 907.01;
            double miPuntoY = 730.36;
            double miPuntoZ = 130.17;
            Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


            double miPuntoX1 = 367.52;
            double miPuntoY1 = 991.57;
            double miPuntoZ1 = miRandom.NextDouble() * 1000;
            Punto3d iPuntoEntrada1 = new Punto3d(miPuntoX1, miPuntoY1, miPuntoZ1);

            double miPuntoX2 = 829.18;
            double miPuntoY2 = 339.5;
            double miPuntoZ2 = miRandom.NextDouble() * 1000;
            Punto3d iPuntoEntrada2 = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);

            iPolilinea.Add(iPuntoEntrada);
            iPolilinea.Add(iPuntoEntrada1);
            iPolilinea.Add(iPuntoEntrada2);

            int iGrupo = 2;

            double prefCur = miRandom.NextDouble() * 1000;
            bool iPrefCurvas = false;
            if (prefCur > 500) iPrefCurvas = true;

            double iRadio = miRandom.NextDouble() * 1000;
            double iVelocidad = miRandom.NextDouble() * 120;
            double iPeralteCurva = miRandom.NextDouble() * 1000;
            double iPeralteRecta = miRandom.NextDouble() * 1000;


            double constante = miRandom.NextDouble() * 1000;
            bool iConstante = false;
            if (constante > 500) iConstante = true;
            EjeTrazado target = new EjeTrazado(iPolilinea, iGrupo, iPrefCurvas, iRadio, iVelocidad, iPeralteCurva, iPeralteRecta, iConstante);


            oCsvLoad misResultadosCSV = new oCsvLoad("C:\\Users\\Angeles\\Documents\\excel\\Excel\\Resultados\\ResCp.csv", ";", "what?", true);
            DataTable misDatosTabla = new DataTable();

            misDatosTabla.Columns.Add("Radio", typeof(double));
            misDatosTabla.Columns.Add("A1", typeof(double));
            misDatosTabla.Columns.Add("A2", typeof(double));
            misDatosTabla.Columns.Add("SentGiro", typeof(string));
            misDatosTabla.Columns.Add("Pcx", typeof(double));
            misDatosTabla.Columns.Add("Pcy", typeof(double));
            misDatosTabla.Columns.Add("Px1x", typeof(double));
            misDatosTabla.Columns.Add("Px1y", typeof(double));
            misDatosTabla.Columns.Add("Pcl1x", typeof(double));
            misDatosTabla.Columns.Add("Pcl1y", typeof(double));
            misDatosTabla.Columns.Add("Pc1x", typeof(double));
            misDatosTabla.Columns.Add("Pc1y", typeof(double));
            misDatosTabla.Columns.Add("Pc2x", typeof(double));
            misDatosTabla.Columns.Add("Pc2y", typeof(double));
            misDatosTabla.Columns.Add("Px2x", typeof(double));
            misDatosTabla.Columns.Add("Px2y", typeof(double));
            misDatosTabla.Columns.Add("Pcl2x", typeof(double));
            misDatosTabla.Columns.Add("Pcl2y", typeof(double));


            misResultadosCSV.loadCsvIntoDataTableTyped(misDatosTabla);


            for (int i = 0; i < misDatosTabla.Rows.Count; i++)
            {
                double iRc = (double)misDatosTabla.Rows[i][0];
                double iAzimut1 = (double)misDatosTabla.Rows[i][1];
                double iAzimut2 = (double)misDatosTabla.Rows[i][2];
                Punto3d iPuntoC = new Punto3d((double)misDatosTabla.Rows[i][4], (double)misDatosTabla.Rows[i][5], 0);
                Punto3d iVerticeAnt = iPuntoC;
                EjeTrazado.sentidoCurva sentG;
                string sentidoCurva = (string)misDatosTabla.Rows[i][3];
                if (sentidoCurva.Equals("Antihorario"))
                {
                    sentG = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    sentG = EjeTrazado.sentidoCurva.Horario;
                }



                double Pcl1x = (double)misDatosTabla.Rows[i][8];
                double Pcl1y = (double)misDatosTabla.Rows[i][9];
                double Pc1x = (double)misDatosTabla.Rows[i][10];
                double Pc1y = (double)misDatosTabla.Rows[i][11];
                double Pc2x = (double)misDatosTabla.Rows[i][12];
                double Pc2y = (double)misDatosTabla.Rows[i][13];
                double Px2x = (double)misDatosTabla.Rows[i][14];
                double Px2y = (double)misDatosTabla.Rows[i][15];
                double Pcl2x = (double)misDatosTabla.Rows[i][16];
                double Pcl2y = (double)misDatosTabla.Rows[i][17];


                Punto3d[] actual;
                actual = target.addCurvaPaso(iRc, iAzimut1, iAzimut2, iVerticeAnt, iPuntoC, sentG);

                if (!((actual[0].coordenadaX > Pcl1x - 0.001) && (actual[0].coordenadaX < Pcl1x + 0.001)
                    && (actual[0].coordenadaY > Pcl1y - 0.001) && (actual[0].coordenadaY < Pcl1y + 0.001)
                    && (actual[1].coordenadaX > Pc1x - 0.001) && (actual[1].coordenadaX < Pc1x + 0.001)
                    && (actual[1].coordenadaY > Pc1y - 0.001) && (actual[1].coordenadaY < Pc1y + 0.001)
                    && (actual[2].coordenadaX > Pc2x - 0.001) && (actual[2].coordenadaX < Pc2x + 0.001)
                    && (actual[2].coordenadaY > Pc2y - 0.001) && (actual[2].coordenadaY < Pc2y + 0.001)
                    && (actual[3].coordenadaX > Pcl2x - 0.001) && (actual[3].coordenadaX < Pcl2x + 0.001)
                    && (actual[3].coordenadaY > Pcl2y - 0.001) && (actual[3].coordenadaY < Pcl2y + 0.001)))
                {
                    actualBool = false;
                }


            }
            Assert.AreEqual(expectedBool, actualBool);

        }


        /// <summary>
        ///Una prueba de addCurvaenSRecta
        ///</summary>
        [TestMethod()]
        public void addCurvaenSRectaTest()
        {
            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            List<Punto3d> iPolilinea = new List<Punto3d>();

            double miPuntoX = 907.01;
            double miPuntoY = 730.36;
            double miPuntoZ = 130.17;
            Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


            double miPuntoX1 = 367.52;
            double miPuntoY1 = 991.57;
            double miPuntoZ1 = miRandom.NextDouble() * 1000;
            Punto3d iPuntoEntrada1 = new Punto3d(miPuntoX1, miPuntoY1, miPuntoZ1);

            double miPuntoX2 = 829.18;
            double miPuntoY2 = 339.5;
            double miPuntoZ2 = miRandom.NextDouble() * 1000;
            Punto3d iPuntoEntrada2 = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);

            iPolilinea.Add(iPuntoEntrada);
            iPolilinea.Add(iPuntoEntrada1);
            iPolilinea.Add(iPuntoEntrada2);

            int iGrupo = 2;

            double prefCur = miRandom.NextDouble() * 1000;
            bool iPrefCurvas = false;
            if (prefCur > 500) iPrefCurvas = true;

            double iRadio = miRandom.NextDouble() * 1000;
            double iVelocidad = miRandom.NextDouble() * 120;
            double iPeralteCurva = miRandom.NextDouble() * 1000;
            double iPeralteRecta = miRandom.NextDouble() * 1000;


            double constante = miRandom.NextDouble() * 1000;
            bool iConstante = false;
            if (constante > 500) iConstante = true;
            EjeTrazado target = new EjeTrazado(iPolilinea, iGrupo, iPrefCurvas, iRadio, iVelocidad, iPeralteCurva, iPeralteRecta, iConstante);

            oCsvLoad misResultadosCSV = new oCsvLoad("C:\\Users\\Angeles\\Documents\\excel\\Excel\\Resultados\\ResCuervaSconRecta.csv", ";", "what?", true);
            DataTable misDatosTabla = new DataTable();

            misDatosTabla.Columns.Add("Radio1", typeof(double));
            misDatosTabla.Columns.Add("Radio2", typeof(double));
            misDatosTabla.Columns.Add("SentGiro1", typeof(string));
            misDatosTabla.Columns.Add("SentGiro2", typeof(string));
            misDatosTabla.Columns.Add("Pc1x", typeof(double));
            misDatosTabla.Columns.Add("Pc1y", typeof(double));
            misDatosTabla.Columns.Add("Pc2x", typeof(double));
            misDatosTabla.Columns.Add("Pc2y", typeof(double));
            misDatosTabla.Columns.Add("Pcl1x", typeof(double));
            misDatosTabla.Columns.Add("Pcl1y", typeof(double));
            misDatosTabla.Columns.Add("Pcl2x", typeof(double));
            misDatosTabla.Columns.Add("Pcl2y", typeof(double));
            misDatosTabla.Columns.Add("PC1x", typeof(double));
            misDatosTabla.Columns.Add("PC1y", typeof(double));
            misDatosTabla.Columns.Add("PC2x", typeof(double));
            misDatosTabla.Columns.Add("PC2y", typeof(double));



            misResultadosCSV.loadCsvIntoDataTableTyped(misDatosTabla);


            for (int i = 0; i < misDatosTabla.Rows.Count; i++)
            {
                double iRc1 = (double)misDatosTabla.Rows[i][0];
                double iRc2 = (double)misDatosTabla.Rows[i][1];
                Punto3d iPuntoC1 = new Punto3d((double)misDatosTabla.Rows[i][4], (double)misDatosTabla.Rows[i][5], 0);
                Punto3d iPuntoC2 = new Punto3d((double)misDatosTabla.Rows[i][6], (double)misDatosTabla.Rows[i][7], 0);


                EjeTrazado.sentidoCurva sentG1;
                EjeTrazado.sentidoCurva sentG2;
                string sentidoCurva1 = (string)misDatosTabla.Rows[i][2];
                string sentidoCurva2 = (string)misDatosTabla.Rows[i][3];

                if (sentidoCurva1.Equals("Antihorario"))
                {
                    sentG1 = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    sentG1 = EjeTrazado.sentidoCurva.Horario;
                }
                if (sentidoCurva2.Equals("Antihorario"))
                {
                    sentG2 = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    sentG2 = EjeTrazado.sentidoCurva.Horario;
                }




                double Pc1x = (double)misDatosTabla.Rows[i][4];
                double Pc1y = (double)misDatosTabla.Rows[i][5];
                double Pc2x = (double)misDatosTabla.Rows[i][6];
                double Pc2y = (double)misDatosTabla.Rows[i][7];
                double Pcl1x = (double)misDatosTabla.Rows[i][8];
                double Pcl1y = (double)misDatosTabla.Rows[i][9];
                double Pcl2x = (double)misDatosTabla.Rows[i][10];
                double Pcl2y = (double)misDatosTabla.Rows[i][11];
                double PC1x = (double)misDatosTabla.Rows[i][12];
                double PC1y = (double)misDatosTabla.Rows[i][13];
                double PC2x = (double)misDatosTabla.Rows[i][14];
                double PC2y = (double)misDatosTabla.Rows[i][15];


                Punto3d[] actual;
                actual = target.addCurvaenSRecta(iRc1, iRc2, iPuntoC1, iPuntoC2, sentG1, sentG2);

                if (!((actual[0].coordenadaX > PC1x - 0.001) && (actual[0].coordenadaX < PC1x + 0.001)
                    && (actual[0].coordenadaY > PC1y - 0.001) && (actual[0].coordenadaY < PC1y + 0.001)
                    && (actual[1].coordenadaX > Pcl1x - 0.001) && (actual[1].coordenadaX < Pcl1x + 0.001)
                    && (actual[1].coordenadaY > Pcl1y - 0.001) && (actual[1].coordenadaY < Pcl1y + 0.001)
                    && (actual[2].coordenadaX > Pcl2x - 0.001) && (actual[2].coordenadaX < Pcl2x + 0.001)
                    && (actual[2].coordenadaY > Pcl2y - 0.001) && (actual[2].coordenadaY < Pcl2y + 0.001)
                    && (actual[3].coordenadaX > PC2x - 0.001) && (actual[3].coordenadaX < PC2x + 0.001)
                    && (actual[3].coordenadaY > PC2y - 0.001) && (actual[3].coordenadaY < PC2y + 0.001)))
                {
                    actualBool = false;
                }


            }
            Assert.AreEqual(expectedBool, actualBool);
        }

        /// <summary>
        ///Una prueba de addPararelismo
        ///</summary>
        [TestMethod()]
        public void addPararelismoTest()
        {

            Random miRandom = new Random();

            bool expectedBool = true;
            bool actualBool = true;



            List<Punto3d> iPolilinea = new List<Punto3d>();

            double miPuntoX = 907.01;
            double miPuntoY = 730.36;
            double miPuntoZ = 130.17;
            Punto3d iPuntoEntrada = new Punto3d(miPuntoX, miPuntoY, miPuntoZ);


            double miPuntoX1 = 367.52;
            double miPuntoY1 = 991.57;
            double miPuntoZ1 = miRandom.NextDouble() * 1000;
            Punto3d iPuntoEntrada1 = new Punto3d(miPuntoX1, miPuntoY1, miPuntoZ1);

            double miPuntoX2 = 829.18;
            double miPuntoY2 = 339.5;
            double miPuntoZ2 = miRandom.NextDouble() * 1000;
            Punto3d iPuntoEntrada2 = new Punto3d(miPuntoX2, miPuntoY2, miPuntoZ2);

            iPolilinea.Add(iPuntoEntrada);
            iPolilinea.Add(iPuntoEntrada1);
            iPolilinea.Add(iPuntoEntrada2);

            int iGrupo = 2;

            double prefCur = miRandom.NextDouble() * 1000;
            bool iPrefCurvas = false;
            if (prefCur > 500) iPrefCurvas = true;

            double iRadio = miRandom.NextDouble() * 1000;
            double iVelocidad = miRandom.NextDouble() * 120;
            double iPeralteCurva = miRandom.NextDouble() * 1000;
            double iPeralteRecta = miRandom.NextDouble() * 1000;


            double constante = miRandom.NextDouble() * 1000;
            bool iConstante = false;
            if (constante > 500) iConstante = true;
            EjeTrazado target = new EjeTrazado(iPolilinea, iGrupo, iPrefCurvas, iRadio, iVelocidad, iPeralteCurva, iPeralteRecta, iConstante);


            oCsvLoad misResultadosCSV = new oCsvLoad("C:\\Users\\Angeles\\Documents\\excel\\Excel\\Resultados\\ResParalelismo.csv", ";", "what?", true);
            DataTable misDatosTabla = new DataTable();


            misDatosTabla.Columns.Add("Radio1", typeof(double));
            misDatosTabla.Columns.Add("Radio2", typeof(double));
            misDatosTabla.Columns.Add("SentGiro1", typeof(string));
            misDatosTabla.Columns.Add("SentGiro2", typeof(string));
            misDatosTabla.Columns.Add("Pc1x", typeof(double));
            misDatosTabla.Columns.Add("Pc1y", typeof(double));
            misDatosTabla.Columns.Add("Pc2x1", typeof(double));
            misDatosTabla.Columns.Add("Pc2y1", typeof(double));
            misDatosTabla.Columns.Add("Pc2x", typeof(double));
            misDatosTabla.Columns.Add("Pc2y", typeof(double));
            misDatosTabla.Columns.Add("Px2x", typeof(double));
            misDatosTabla.Columns.Add("Px2y", typeof(double));
            misDatosTabla.Columns.Add("Pcl2x", typeof(double));
            misDatosTabla.Columns.Add("Pcl2y", typeof(double));
            misDatosTabla.Columns.Add("Px3x", typeof(double));
            misDatosTabla.Columns.Add("Px3y", typeof(double));
            misDatosTabla.Columns.Add("Pcl3x", typeof(double));
            misDatosTabla.Columns.Add("Pcl3y", typeof(double));
            misDatosTabla.Columns.Add("Pc3x", typeof(double));
            misDatosTabla.Columns.Add("Pc3y", typeof(double));


            misResultadosCSV.loadCsvIntoDataTableTyped(misDatosTabla);


            for (int i = 0; i < misDatosTabla.Rows.Count; i++)
            {
                double iRc1 = (double)misDatosTabla.Rows[i][0];
                double iRc2 = (double)misDatosTabla.Rows[i][1];
                Punto3d iPuntoC1 = new Punto3d((double)misDatosTabla.Rows[i][4], (double)misDatosTabla.Rows[i][5], 0);
                Punto3d iPuntoC2 = new Punto3d((double)misDatosTabla.Rows[i][6], (double)misDatosTabla.Rows[i][7], 0);


                EjeTrazado.sentidoCurva sentG1;
                EjeTrazado.sentidoCurva sentG2;
                string sentidoCurva1 = (string)misDatosTabla.Rows[i][2];
                string sentidoCurva2 = (string)misDatosTabla.Rows[i][3];

                if (sentidoCurva1.Equals("Antihorario"))
                {
                    sentG1 = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    sentG1 = EjeTrazado.sentidoCurva.Horario;
                }
                if (sentidoCurva2.Equals("Antihorario"))
                {
                    sentG2 = EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    sentG2 = EjeTrazado.sentidoCurva.Horario;
                }

                double Pc2x = (double)misDatosTabla.Rows[i][8];
                double Pc2y = (double)misDatosTabla.Rows[i][9];
                double Px2x = (double)misDatosTabla.Rows[i][10];
                double Px2y = (double)misDatosTabla.Rows[i][11];
                double Pcl2x = (double)misDatosTabla.Rows[i][12];
                double Pcl2y = (double)misDatosTabla.Rows[i][13];
                double Px3x = (double)misDatosTabla.Rows[i][14];
                double Px3y = (double)misDatosTabla.Rows[i][15];
                double Pcl3x = (double)misDatosTabla.Rows[i][16];
                double Pcl3y = (double)misDatosTabla.Rows[i][17];
                double Pc3x = (double)misDatosTabla.Rows[i][18];
                double Pc3y = (double)misDatosTabla.Rows[i][19];




                Punto3d[] actual;
                actual = target.addPararelismo(iRc1, iRc2, iPuntoC1, iPuntoC2, sentG1, sentG2);

                if (!((actual[0].coordenadaX > Pc2x - 0.001) && (actual[0].coordenadaX < Pc2x + 0.001)
                    && (actual[0].coordenadaY > Pc2y - 0.001) && (actual[0].coordenadaY < Pc2y + 0.001)
                    && (actual[1].coordenadaX > Pcl2x - 0.001) && (actual[1].coordenadaX < Pcl2x + 0.001)
                    && (actual[1].coordenadaY > Pcl2y - 0.001) && (actual[1].coordenadaY < Pcl2y + 0.001)
                    && (actual[2].coordenadaX > Pcl3x - 0.001) && (actual[2].coordenadaX < Pcl3x + 0.001)
                    && (actual[2].coordenadaY > Pcl3y - 0.001) && (actual[2].coordenadaY < Pcl3y + 0.001)
                    && (actual[3].coordenadaX > Pc3x - 0.001) && (actual[3].coordenadaX < Pc3x + 0.001)
                    && (actual[3].coordenadaY > Pc3y - 0.001) && (actual[3].coordenadaY < Pc3y + 0.001)))
                {
                    actualBool = false;
                }


            }
            Assert.AreEqual(expectedBool, actualBool);
        }
    }
}
