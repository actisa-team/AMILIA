using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using engCadNet;
using tadLayLogica.Comandos;
using tadLayLogica.datos;
using tadLayShare;

namespace tadLayUI
{

    using tadLayLan.Tdm;
    using tadLayLogica;
    using tadLayLan;
    using tadLayShare.puntos;
    using Terrenos;
    using System.Globalization;

    public partial class frmManagerTerreno : Form
    {

        private int mNumPuntos = 0;
        private List<List<Punto3d>> mLineasRotura = new List<List<Punto3d>>();
        private int mNumCurvas = 0;

        private List<Triangulo> _znpTriangulos;
        private Triangulacion _triangulacion;
        private bool triangulationProcessCanceled = false;

        private static string _triangulationFileName;
        private string iCapaPuntos;
        public List<Punto3d> Lista_puntos = new List<Punto3d>();
        private string path;
        private static string TriangulationFileName
        {
            get
            {
                if (_triangulationFileName == null)
                {
                    var acDoc = oCadManager.thisMdi;
                    var nombreFichero = "TADILtriangulation.dwg";
                    var directoryName = Path.GetTempPath();
                    var filePath = Path.Combine(directoryName, nombreFichero);
                    _triangulationFileName = filePath;
                }
                return _triangulationFileName;
            }
            set { _triangulationFileName = value; }
        }

        public frmManagerTerreno()
        {
            InitializeComponent();
            postConstrutor();
            ucLblTxtDistCurvNivel.valorMinimo = 0;
            ucLblTxtDistCurvNivel.valorMaximo = double.MaxValue;
            ucLblTxtRangoBusqueda.valorMinimo = 0;
            ucLblTxtRangoBusqueda.valorMaximo = double.MaxValue;
        }

        private void postConstrutor()
        {

            var name = oTadil.KAppHeaderName;
            this.Text = name;
            toolStripStatusLabel1.Text = "";

            groupBox1.Text = strFormTdm.gbOrigDatos;
            ucCmbLayersTodas1.uiLbl = strFormTdm.lbCapaPuntos;
            ucCmbLayersTodas2.uiLbl = strFormTdm.lbCapaLineasRot;


            groupBox2.Text = strFormTdm.gbConfiguracion;
            ucLblTxt1.uiLbl = strFormTdm.lbNombre;
            ucLblTxt2.uiLbl = strFormTdm.lbLongMax;
            ucLblTxt3.uiLbl = strFormTdm.lbIntervalo;
            ucLblTxt4.uiLbl = strFormTdm.lbPendienteMax;
            ucNodosPorHoja.uiLbl = strFormTdm.uiNodosPorHoja;
            ucLblTxtDistCurvNivel.uiLbl = strFormTdm.lbDistCurvaNivel;
            chbCurvasRotura.Text = strFormTdm.uiTratarCurvasRotura;
            ucLblTxtRangoBusqueda.uiLbl = strFormTdm.uiRangoBusqueda;

            buttonCrearTerreno.Text = strFormTdm.btCreaMalla;
            buttonInfo.Text = strFormTdm.uiInfoBoton;


            groupBox4.Text = strFormTdm.gbEditarMalla;
            ucTerrain1.uiLbl = strFormTdm.lbMallaAEditar;
            button2.Text = strFormTdm.btEliminarTriangulos;

            ucIntervaloLineasR.uiLbl = strFormTdm.uiIntervaloLineasR;


            ucTerrain1.populate();
            ucCmbLayersTodas1.populate();
            ucCmbLayersTodas2.populate();
            ucCmbLayersTodas1.uiCombo.SelectedIndexChanged += new EventHandler(uiCombo_SelectedIndexChanged1);
            ucCmbLayersTodas2.uiCombo.SelectedIndexChanged += new EventHandler(uiCombo_SelectedIndexChanged2);


        }


        void uiCombo_SelectedIndexChanged1(object sender, EventArgs e)
        {

            double miIntervalo = ucLblTxt3.valorDouble;
            mNumPuntos = oComandoTerreno.getOnlyPoints(ucCmbLayersTodas1.uiCombo.SelectedValue.ToString());
            mNumCurvas = oComandoTerreno.getOnlyCurves(ucCmbLayersTodas1.uiCombo.SelectedValue.ToString());
            toolStripStatusLabel1.Text = strFormTdm.toolStatus1 + mNumPuntos + strFormTdm.toolStatus2 + mNumCurvas + strFormTdm.toolStatus3 + mLineasRotura.Count + strFormTdm.toolStatus4;
        }


        void uiCombo_SelectedIndexChanged2(object sender, EventArgs e)
        {

            mLineasRotura = oComandoTerreno.getLineasRotura(ucCmbLayersTodas2.uiCombo.SelectedValue.ToString());
            toolStripStatusLabel1.Text = strFormTdm.toolStatus1 + mNumPuntos + strFormTdm.toolStatus2 + mNumCurvas + strFormTdm.toolStatus3 + mLineasRotura.Count + strFormTdm.toolStatus4;
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            oTadil.data.UserInfo.showInfo(strFormTdm.uiInfoNodosPorHoja);
        }

        private void buttonCrearTerreno_Click(object sender, EventArgs e)
        {
            /*double miMax = ucLblTxt2.valorDouble;
            double miPendienteMax2 = ucLblTxt4.valorDouble;
            path = oTadil.data.Files.getFileTxt();
            CargarPuntosASC(miMax, miPendienteMax2,true,2);*/
            


            if ((ucLblTxtDistCurvNivel.Enabled && ucLblTxtDistCurvNivel.valorDouble < ucLblTxtRangoBusqueda.valorDouble) || (!ucLblTxtDistCurvNivel.Enabled))
            {
                iCapaPuntos=ucCmbLayersTodas1.uiCombo.SelectedValue.ToString();
                backgroundWorker1 = new BackgroundWorker();
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.DoWork += DoTdmAsync;
                backgroundWorker1.ProgressChanged += OnReportProgress;
                backgroundWorker1.RunWorkerCompleted += OnBackgroundWorkerCompleted;
                progressBar1.MarqueeAnimationSpeed = 30;
                progressBar1.Style = ProgressBarStyle.Marquee;
                backgroundWorker1.RunWorkerAsync();
                buttonCrearTerreno.Enabled = false;
                button2.Enabled = false;
                ucNodosPorHoja.Enabled = false;
                ucCmbLayersTodas1.Enabled = false;
                ucCmbLayersTodas2.Enabled = false;
                ucIntervaloLineasR.Enabled = false;
                ucLblTxt1.Enabled = false;
                ucLblTxt2.Enabled = false;
                ucLblTxt3.Enabled = false;
                ucLblTxt4.Enabled = false;
                ucLblTxtDistCurvNivel.Enabled = false;
                ucLblTxtRangoBusqueda.Enabled = false;
                ucTerrain1.Enabled = false;
                chbCurvasRotura.Enabled = false;
                
            }
            else
            {
                oTadil.data.UserInfo.showInfo(strFormTdm.uiRangoMayorDist);
            }
        }

        private void OnBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.Style = ProgressBarStyle.Continuous;
            buttonCrearTerreno.Enabled = true;
            button2.Enabled = true;
            ucNodosPorHoja.Enabled = true;
            ucCmbLayersTodas1.Enabled = true;
            ucCmbLayersTodas2.Enabled = true;
            ucIntervaloLineasR.Enabled = true;
            ucLblTxt1.Enabled = true;
            ucLblTxt2.Enabled = true;
            ucLblTxt3.Enabled = true;
            ucLblTxt4.Enabled = true;
            ucLblTxtDistCurvNivel.Enabled = chbCurvasRotura.Checked;
            ucLblTxtRangoBusqueda.Enabled = chbCurvasRotura.Checked;
            ucTerrain1.Enabled = true;
            chbCurvasRotura.Enabled = true;
            if (!triangulationProcessCanceled)
            {
                string miNombre = ucLblTxt1.textbox.Text;
                double miMax = ucLblTxt2.valorDouble;
                double miIntervalo = ucLblTxt3.valorDouble;
                double miPendienteMax = ucLblTxt4.valorDouble;
                double distCurvaNivel = ucLblTxtDistCurvNivel.valorDouble;
                int rangoBusqueda = (int)(ucLblTxtRangoBusqueda.valorDouble / distCurvaNivel);
                bool isCurvaAsRotura = chbCurvasRotura.Checked;

                 //var handle = oSubDMesh.createMesh(_triangulacion, TriangulationFileName);
                 //oSubDMesh.CopiarTriangulacionToOriginal(_znpTriangulos);
                 //ucTerrain1.AddElement(handle, ucLblTxt1.textbox.Text);
                string filtro = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                oTadil.data.Files.SaveAsFileFromDialog_Puntos(Lista_puntos, filtro, miMax, miPendienteMax, ucNodosPorHoja.valorInt);
               
            }
        }

        private void OnReportProgress(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            toolStripStatusLabel1.Text = progressChangedEventArgs.UserState.ToString();
        }

        private void DoTdmAsync(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            try
            {
                if (oValidar.isValidoGrupoByFrm(this))
                {
                    if (!oSubDMesh.existName(ucLblTxt1.textbox.Text, oTadil.IsDemo))
                    {
                        string miNombre = ucLblTxt1.textbox.Text;
                        double miMax = ucLblTxt2.valorDouble;
                        double miIntervalo = ucLblTxt3.valorDouble;
                        double? miPendienteMax = ucLblTxt4.valorDoubleNull;
                        double distCurvaNivel = ucLblTxtDistCurvNivel.valorDouble;
                        int rangoBusqueda = (int)(ucLblTxtRangoBusqueda.valorDouble/ distCurvaNivel);
                        bool  isCurvaAsRotura = chbCurvasRotura.Checked;

                        var time = new Stopwatch();
                        time.Start();

                        List<List<Punto3d>> misLineasR = new List<List<Punto3d>>();
                        backgroundWorker1.ReportProgress(0,"Calculando linea de rotura ...");
                        /*if (ucCmbLayersTodas2.uiCombo.SelectedValue != null)
                        {
                            misLineasR = mLineasRotura;
                        }*/
                        backgroundWorker1.ReportProgress(0, "Calculando nube de puntos ...");
                        //long before = GC.GetTotalMemory(true);
                        var nubePuntos =
                                oComandoTerreno.GetNubeDePuntos(iCapaPuntos,
                                    miIntervalo, misLineasR, ucIntervaloLineasR.valorDouble, distCurvaNivel, isCurvaAsRotura, rangoBusqueda, backgroundWorker1);
                        //long after = GC.GetTotalMemory(true);  // Memoria después de crearlo
                        var userInfo = new oUserInfo();
                      


                        /*
                         * Aqui da error en debugger al mostrar la pregunta por lo que hay que quitarla cuando se hay debug
                         * Se pone debug en true o false para que entre en un sitio u otro
                         */
                        this.Lista_puntos = nubePuntos;



                        double miPendienteMax2 = ucLblTxt4.valorDouble;
//                        CargarPuntosASC(miMax, miPendienteMax2, false, miIntervalo);

                        nubePuntos = this.Lista_puntos;

                        /*string filePath = ObtenerRutaArchivo();

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            GuardarPuntosEnArchivo(nubePuntos, filePath);
                            //Console.WriteLine($"Puntos guardados en {filePath}");
                        }
                        else
                        {
                            //Console.WriteLine("Operación cancelada.");
                        }*/

                        bool debuger = true;
                        if (debuger)
                        {
/*                            triangulationProcessCanceled = false;
                            backgroundWorker1.ReportProgress(0, "Creando triangulación ...");
                            _znpTriangulos = new List<Triangulo>();
                            //long after2 = GC.GetTotalMemory(true);  // Memoria después de crearlo
                            _triangulacion = oComandoTerreno.crearMallaNoUI(nubePuntos, miNombre, miMax, miPendienteMax,
                                TriangulationFileName, ref _znpTriangulos, ucNodosPorHoja.valorInt, this);
                            //long after3 = GC.GetTotalMemory(true);  // Memoria después de crearlo
*/                          time.Stop();
                            var min = (time.ElapsedMilliseconds / 1000.0) / 60.0;
                            backgroundWorker1.ReportProgress(0, String.Format("Tiempo en Calcular: {0} minutos", min));

                        }
                        else
                        {
                            var result = userInfo.showSiNo(this, String.Format("La malla contiene {0} puntos, ¿Desea continuar?", nubePuntos.Count));
                            if (result == DialogResult.Yes)
                            {
                                triangulationProcessCanceled = false;
                                backgroundWorker1.ReportProgress(0, "Creando triangulación ...");
                                _znpTriangulos = new List<Triangulo>();

                                _triangulacion = oComandoTerreno.crearMallaNoUI(nubePuntos, miNombre, miMax, miPendienteMax,
                                    TriangulationFileName, ref _znpTriangulos, ucNodosPorHoja.valorInt, this);

                                time.Stop();
                                var min = (time.ElapsedMilliseconds / 1000.0) / 60.0;
                                backgroundWorker1.ReportProgress(0, String.Format("Tiempo en triangular: {0} minutos", min));
                            }
                            else
                            {
                                triangulationProcessCanceled = true;
                            }
                        }
                        
                        
                        
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strFormTdm.eExTerrain);
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }
            }
            catch (InvalidCastException ex)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiPolilinieas3dNoValidas);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                //this.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                oTadil.data.UserInfo.showInfo(strFormTdm.cargandoTerreno);
                var triangulacion =
                    oComandoTerreno.getTriangulacionByName(
                        ucTerrain1.uiCombo.GetItemText(ucTerrain1.uiCombo.SelectedItem));
                    this.Hide();
                    oComandoTerreno.modificarMalla(triangulacion,
                        TriangulationFileName);
                    oSubDMesh.CopiarTriangulacionToOriginal(new List<Triangulo>());

                this.Show();
            }
            catch(Exception)
            {
                oTadil.data.UserInfo.showInfo(strFormTdm.eExMalla);
            }
            finally
            {
                this.Show();
            }

        }
        
        private void ChbCurvasRotura_CheckedChanged(object sender, EventArgs e)
        {
            ucLblTxtDistCurvNivel.Enabled = chbCurvasRotura.Checked;
            ucLblTxtRangoBusqueda.Enabled = chbCurvasRotura.Checked;
        }

        static string[] SplitLine(string line)
        {
            return line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private void CargarPuntosASC( double miMax,double miPendienteMax,bool guardar,double intervalo)
        {
            //string path = "C:\\Users\\Juanma\\Desktop\\prueba tadil\\1024-1.asc";
            
            string[] lines = File.ReadAllLines(path);

            int ncols = int.Parse(SplitLine(lines[0])[1]);
            int nrows = int.Parse(SplitLine(lines[1])[1]);
            double xllcorner = double.Parse(SplitLine(lines[2])[1], CultureInfo.InvariantCulture);
            double yllcorner = double.Parse(SplitLine(lines[3])[1], CultureInfo.InvariantCulture);
            double cellsize = double.Parse(SplitLine(lines[4])[1], CultureInfo.InvariantCulture);
            double nodata = double.Parse(SplitLine(lines[5])[1], CultureInfo.InvariantCulture);

            Lista_puntos = new List<Punto3d>();
            int aumento =(int)intervalo / (int)cellsize;
            int rowOffset = 6; // Donde empieza la matriz
            for (int row = 0; row < nrows; row+= aumento)
            {
                string[] zValues = lines[row + rowOffset].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < ncols; col+= aumento)
                {
                    double z = double.Parse(zValues[col], CultureInfo.InvariantCulture);
                    if (z == nodata) continue;

                    double x = xllcorner + col * cellsize;
                    double y = yllcorner + (nrows - 1 - row) * cellsize;

                    Lista_puntos.Add(new Punto3d
                    {
                        coordenadaX = x,
                        coordenadaY = y,
                        coordenadaZ = z
                    });
                }
            }
            if (guardar)
            {
                string filtro = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                oTadil.data.Files.SaveAsFileFromDialog_Puntos(Lista_puntos, filtro, miMax, miPendienteMax, ucNodosPorHoja.valorInt);
            }
            

        }
    }

}
