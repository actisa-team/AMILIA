using System;
using System.Windows.Forms;

namespace interfaz
{
    using Datos;
    using MaterialSkin;
    using MaterialSkin.Controls;
    using Logica;
    using System.Collections.Generic;
    using System.Linq;
    using Logica.verificacion;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Diagnostics;
    using Autodesk.AutoCAD.Geometry;
    using System.Data;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using System.Threading.Tasks;
    using System.Text;
    using engCadNet;
    using Newtonsoft.Json;
    using Autodesk.AutoCAD.EditorInput;
    using tadLayShare.puntos;
    using EjeDeTrazado.puntosDelEje;
    using Terraplan.Logica;

    public partial class principal : MaterialForm, IViabilidadListener, IViabilidadStatusInfoPanelListener
    {
        private CalculoPolilinea calculoPolilinea;
        private CalculoPolilinea calculoPolilinea_prueba;
        private CalculoPolilineaPreferencias calculoPolilineaPreferencias;
        private int pasosEjecutados = -1;
        private Boolean ejecutarViabilidadSinParar = false;
        private Boolean terminar = false;
        private bool detenerEnIteracion = false;
        private int iteracion = -1;
        private List<VerificacionComponentesStatus> List_verificacion = new List<VerificacionComponentesStatus>();
        private List<List<Componente>> List_componentes = new List<List<Componente>>();
        private List<List<Punto>> List_polilinea = new List<List<Punto>>();
        private List<List<ViabilidadComponentesStatus>> List_traza = new List<List<ViabilidadComponentesStatus>>();
        private List<ViabilidadComponentesStatus> viabilidadEnlaces_hebra = new List<ViabilidadComponentesStatus>();
        private List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int, List<Componente>>> Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int, List<Componente>>>();
        private List<Tuple<Polyline, double>> Lista_Resultados_Perfil = new List<Tuple<Polyline, double>>();
        private List<Point2d> lista_original_perfil = new List<Point2d>();
        private List<Tuple<List<Parabola>, List<Pendiente>>> Lista_Entidades_Perfil = new List<Tuple<List<Parabola>, List<Pendiente>>>();
        private List<CalculoPolilineaPerfil> Lista_Polilineas_Perfil = new List<CalculoPolilineaPerfil>();
        private List<Componente> componentes_automaticas = new List<Componente>();
        private int tipoA;
        private int tipoB;
        private int tipoC;
        private int N_P;
        private double P_C;
        private int N_C;
        private int tipo;
        private double[] A1_C1 = { 5, 2, 4, 3, 1 };
        private double[] A1_C2 = { 10, 8, 12, 6, 14 };
        private double[] A1_C3 = { 15, 20, 25, 30, 35 };
        private double[] A2_C1 = { 10, 6, 2, 8, 4 };
        private double[] A2_C2 = { 10, 11, 12, 13, 14 };
        private double[] A2_C3 = { 15, 20, 25, 30, 35 };

        private double curv_ing;
        private double rect_ing;
        private double[] rectas_ingenieril = { 1, 2, 4, 6, 8 };
        private double[] curvas_ingenieril = { 10, 20, 30, 40, 50 };


        private int contador_tipo1 = 0;
        private int contador_tipo2 = 0;
        private double grados_union = 2;
        private int[] List_N_C = { 6, 4, 2, 8, 1 };
        private double distancia_menor = 100;
        public bool salir_bucle = false;
        public bool repetir_pregunta = false;
        System.Threading.Thread th2;
        System.Threading.Thread th3;
        int contador_resultados_numero = 0;
        int conta_apartado = 0;
        int conta_apartado_perfil = 0;
        string documento = "";
        /*
         * Perfil
         */
        private CalculoPolilineaPerfil calculoPolilineaPerfil;
        private CalculoPolilineaPreferencias calculoPolilineaPreferenciasPerfil;
        private List<Point3d> Lista_original_3d = new List<Point3d>();
        private List<Point2d> Lista_original_2d = new List<Point2d>();
        private List<Point2d> trazadoDensificado = null; // trazado con rectas densificadas para cálculo de Z con MDT
        List<Resultados> L_Resultados = new List<Resultados>();

        // Ruta del archivo .amilia cargado — se comparte con TrazadoUsuarioPerfil
        private string _rutaArchivoAmilia = null;

        /*
         * Nuevas variables para el terreno MDT
         */
        private nubepuntos nube = new nubepuntos { };
        private List<Punto3d> puntos = null;
        private TerrainService terrainService = null;
        private KDTree2D KDTree2D = null;
        /*
         * 
         */
        public static string ruta_principal = "";

        private List<SalidaAmilia> listaSalidas = null;
        /// <summary>
        /// es donde inicializa la interfaz
        /// </summary>
        /// 
        public principal()
        {
            InitializeComponent();
            MaterialSkinManager m = MaterialSkinManager.Instance;
            m.AddFormToManage(this);

            m.Theme = MaterialSkinManager.Themes.LIGHT;

            //m.ColorScheme = new ColorScheme(Primary.Green900,Primary.Green700,Primary.Green500,Accent.LightGreen200,TextShade.WHITE);
            //m.ColorScheme = new ColorScheme(Primary.Grey100, Primary.Grey200, Primary.Lime600, Accent.LightGreen700, TextShade.BLACK);
            m.ColorScheme = new ColorScheme(Primary.Grey300, Primary.Grey400, Primary.Grey200, Accent.LightBlue200, TextShade.BLACK);
            
            // Intentar forzar letras negras manteniendo el fondo (Primary = true)
            ConfigurarBotonesTextoNegro(this);

            postcarga();

            if (AplitopProperties.isDevelopment())
            {
                this.debugButtonsContainer.Visible = true;
            }
            else
            {
                this.debugButtonsContainer.Visible = false;
            }


            //this.ejecutar1Button.Click += new EventHandler(this.ejecutar1ButtonClick);
            this.ejecutar2Button.Click += new EventHandler(this.ejecutar2ButtonClick);
            this.ejecutar3Button.Click += new EventHandler(this.ejecutar3ButtonClick);
        }
        private void postcarga()
        {


            this.Text = "Amilia";

            tabPage2.Text = "Filtrar puntos";
            tabPage1.Text = "Estudio previo";
            tabPage3.Text = "Cálculo en planta";
            tabPage4.Text = "Cálculo en perfil";
            this.materialTabControl1.SelectedTab = tabPage3;
            tabPage2.AutoScroll = true;
            filtrado1CheckBox.Checked = true;
            filtrado3GradosTextField.Enabled = false;
            filtrado3MetrosTextField.Enabled = false;
            filtrado3GradosTextField.Text = "5";
            filtrado3MetrosTextField.Text = "2";
            materialLabel1.Enabled = false;
            materialLabel2.Enabled = false;

            materialLabel4.Text = "";
            materialLabel5.Text = "";
            materialLabel7.Text = "";

            filtrado1ExecuteOrderNumericField.Enabled = false;
            filtrado2ExecuteOrderNumericField.Enabled = false;
            filtrado3ExecuteOrderNumericField.Enabled = false;

            conta_apartado = 1;
            conta_apartado_perfil = 1;
            materialLabel26.Visible = false;
            suavizar.Visible = false;
            materialLabel30.Visible = false;
            textpendiente.Visible = false;
            materialLabel25.Visible = false;
            Varianza_acumulada.Visible = false;
            materialLabel27.Visible = false;
            text_distancia.Visible = false;
            materialLabel28.Visible = false;
            text_separacion.Visible = false;
            panel1.Visible = false;
            materialLabel23.Visible = false;
            materialLabel31.Visible = false;
            materialTabControl1.Visible = true;
            materialLabel36.Visible = false;
            materialLabel37.Visible = false;
            textBox_rot.Visible = false;
            materialCheckBox1.Visible = false;
            materialCheckBox3.Visible = false;


            /*     materialFlatButton27.Visible = true;
                 materialFlatButton25.Visible = true;
                 materialFlatButton26.Visible = true;
                 materialFlatButton29.Visible = true;
                 materialFlatButton30.Visible = true;*/

        }

        private void principal_ResizeEnd(object sender, EventArgs e)
        {
            materialTabSelector1.Width = this.Size.Width;
            materialTabControl1.Height = this.Size.Height;
        }

        /// <summary>
        /// Recorre recursivamente los controles para asegurar que los botones Material no fuercen letras blancas
        /// </summary>
        private void ConfigurarBotonesTextoNegro(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is BotonPersonalizado btn)
                {
                    // Forzar texto en mayúsculas
                    if (!string.IsNullOrEmpty(btn.Text))
                    {
                        btn.Text = btn.Text.ToUpper();
                    }

                    if (btn.Name == "button1" || btn.Name == "button2" || btn.Name == "button3" || btn.Name == "button4" ||
                    btn.Name == "button5" || btn.Name == "button6" || btn.Name == "button7" || btn.Name == "button8" ||
                     btn.Name == "button9" || btn.Name == "button10" || btn.Name == "button11" || btn.Name == "button12" ||
                     btn.Name == "button13" ||  btn.Name == "button14" || btn.Name == "button15" || btn.Name == "button16" ||
                      btn.Name == "button17" || btn.Name == "button18" || btn.Name == "button19" || btn.Name == "button20" ||
                       btn.Name == "button21" || btn.Name == "button22" || btn.Name == "button23" || btn.Name == "button24" ||
                       btn.Name == "button25" || btn.Name == "button26" || btn.Name == "botonPersonalizado1")
                    {
                        btn.ColorSolidoBase = Color.Transparent;
                        btn.ColorSolidoHover = Color.FromArgb(30, Color.Black);
                        btn.ColorBorde = Color.Transparent;
                        btn.Sombrear = false;
                        btn.GrosorBorde = 0;
                    }else if (btn.Name== "materialFlatButton17")
                    {
                        btn.BorderRadius = 10;
                        btn.ColorBorde = Color.DarkGray;
                        btn.ColorSolidoBase = Color.LightCoral;
                        btn.ColorSolidoHover = Color.IndianRed;
                        btn.GrosorBorde = 1;
                        btn.Sombrear = true;
                        btn.UsaGradiente = false;
                        btn.ForeColor = Color.Black;
                        btn.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
                    }
                    else if (btn.Name == "materialFlatButton18" ||
                        btn.Name == "materialFlatButton19" ||
                        btn.Name == "materialFlatButton23")
                    {
                        btn.BorderRadius = 10;
                        btn.ColorBorde = Color.DarkGray;
                        btn.ColorSolidoBase = Color.MediumTurquoise;
                        btn.ColorSolidoHover = Color.LightSeaGreen;
                        btn.GrosorBorde = 1;
                        btn.Sombrear = true;
                        btn.UsaGradiente = false;
                        btn.ForeColor = Color.Black;
                        btn.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
                    }
                    else if (btn.Name == "materialFlatButton2" ||
                        btn.Name == "materialFlatButton12" ||
                        btn.Name == "materialFlatButton22")
                    {
                        btn.BorderRadius = 10;
                        btn.ColorBorde = Color.DarkGray;
                        btn.ColorSolidoBase = Color.LightSkyBlue;
                        btn.ColorSolidoHover = Color.SteelBlue;
                        btn.GrosorBorde = 1;
                        btn.Sombrear = true;
                        btn.UsaGradiente = false;
                        btn.ForeColor = Color.Black;
                        btn.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        btn.BorderRadius = 10;
                        btn.ColorBorde = Color.DarkGray;
                        btn.ColorSolidoBase = Color.LightGray;
                        btn.ColorSolidoHover = Color.Silver;
                        btn.GrosorBorde = 1;
                        btn.Sombrear = true;
                        btn.UsaGradiente = false;
                        btn.ForeColor = Color.Black;
                        btn.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
                    }
                }
                else if (c is Button classicBtn && !(c is BotonPersonalizado))
                {
                    if (!string.IsNullOrEmpty(classicBtn.Text))
                    {
                        classicBtn.Text = classicBtn.Text.ToUpper();
                    }
                    classicBtn.TextAlign = ContentAlignment.MiddleCenter;
                    classicBtn.FlatStyle = FlatStyle.Standard;
                    classicBtn.BackColor = Color.FromArgb(224, 224, 224);
                    classicBtn.ForeColor = Color.Black;
                }

                if (c.HasChildren)
                {
                    ConfigurarBotonesTextoNegro(c);
                }
            }
        }

        private void principal_Load(object sender, EventArgs e)
        {
            materialTabSelector1.Width = this.Size.Width;
            materialTabControl1.Height = this.Size.Height;

        }
        private bool comprobar()
        {
            bool comprueba = true;
            if (filtrado1ExecuteOrderNumericField.Value != 0)
            {
                if (filtrado1ExecuteOrderNumericField.Value == filtrado2ExecuteOrderNumericField.Value || filtrado1ExecuteOrderNumericField.Value == filtrado3ExecuteOrderNumericField.Value)
                {
                    comprueba = false;
                }
            }
            if (filtrado2ExecuteOrderNumericField.Value != 0)
            {
                if (filtrado2ExecuteOrderNumericField.Value == filtrado3ExecuteOrderNumericField.Value || filtrado2ExecuteOrderNumericField.Value == filtrado1ExecuteOrderNumericField.Value)
                {
                    comprueba = false;
                }
            }

            if (filtrado3ExecuteOrderNumericField.Value != 0)
            {
                if (filtrado2ExecuteOrderNumericField.Value == filtrado3ExecuteOrderNumericField.Value || filtrado3ExecuteOrderNumericField.Value == filtrado1ExecuteOrderNumericField.Value)
                {
                    comprueba = false;
                }
            }
            return comprueba;
        }

        private dsApp abrirArchivoDeProyecto()
        {
            int counter = 1;
            string miFileOut = string.Empty;
            string line;
            OpenFileDialog miDialogo = new OpenFileDialog();
            miDialogo.Title = "AMILIA" + " | " + "Selecciona un Archivo de Proyecto";
            miDialogo.Filter = "Ditel Project Files (*.txt)|*.txt";
            miDialogo.Multiselect = false;
            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                ruta_principal = System.IO.Path.GetDirectoryName(miFileOut);
                documento = miDialogo.SafeFileName;
                Ocultar_Capas(documento.Split('.')[0]);
                string tiempo = DateTime.Now.ToString("HH.mm");
                documento = documento.Split('.')[0] + "-" + tiempo;
                Lista_original_3d = new List<Point3d>();
                Lista_original_2d = new List<Point2d>();

                System.IO.StreamReader file = new System.IO.StreamReader(@miFileOut);
                dsApp dsApp = new dsApp();

                while ((line = file.ReadLine()) != null)
                {
                    string[] separadas;
                    separadas = line.Split(',');
                    if (separadas.Count() > 1)
                    {
                        dsApp.Polilinea.Rows.Add(separadas[0], separadas[1], counter);
                        if (separadas.Count() > 2)
                        {
                            if (counter == 1)
                            {
                                Lista_original_3d = new List<Point3d>();
                            }
                            Lista_original_3d.Add(new Point3d(double.Parse(separadas[0]), double.Parse(separadas[1]), double.Parse(separadas[2])));
                            dsApp.Polilinea3d.Rows.Add(separadas[0], separadas[1], separadas[2], counter);
                        }
                        if (separadas.Count() > 1)
                        {
                            if (counter == 1)
                            {
                                Lista_original_2d = new List<Point2d>();
                            }
                            Lista_original_2d.Add(new Point2d(double.Parse(separadas[0]), double.Parse(separadas[1])));
                        }
                        counter++;
                    }
                    else
                    {
                        return null;
                    }


                }
                dsApp.WriteXml("prueba.amilia");
                file.Close();
                return dsApp;
            }
            return null;
        }
        private dsApp abrirArchivoDeProyectoPerfil()
        {
            int counter = 1;
            string miFileOut = string.Empty;
            string line;
            double x = 0, y = 0, x2 = 0, y2 = 0;
            double distancia = 0;
            double d_acumulada = 0;
            OpenFileDialog miDialogo = new OpenFileDialog();
            miDialogo.Title = "AMILIA" + " | " + "Selecciona un Archivo de Proyecto";
            miDialogo.Filter = "Ditel Project Files (*.txt)|*.txt";
            miDialogo.Multiselect = false;
            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                documento = miDialogo.SafeFileName;
                Ocultar_Capas(documento.Split('.')[0]);
                string tiempo = DateTime.Now.ToString("HH.mm");
                documento = documento.Split('.')[0] + "-" + tiempo;
                System.IO.StreamReader file = new System.IO.StreamReader(@miFileOut);
                dsApp dsApp = new dsApp();
                while ((line = file.ReadLine()) != null)
                {
                    string[] separadas;
                    separadas = line.Split(',');
                    if (separadas.Count() > 1)
                    {
                        if (separadas.Count() > 2)
                        {
                            if (counter == 1)
                            {
                                x = double.Parse(separadas[0]);
                                y = double.Parse(separadas[1]);
                                dsApp.Polilinea.Rows.Add(0, double.Parse(separadas[2]), counter);

                            }
                            else
                            {
                                x2 = double.Parse(separadas[0]);
                                y2 = double.Parse(separadas[1]);
                                distancia = Math.Sqrt(Math.Pow(x2 - x, 2) + Math.Pow(y2 - y, 2));
                                d_acumulada += distancia;
                                dsApp.Polilinea.Rows.Add(d_acumulada, double.Parse(separadas[2]), counter);
                                x = x2;
                                y = y2;
                                //dsApp.Polilinea.Rows.Add(Math.Sqrt(Math.Pow(double.Parse(separadas[0]), 2) + Math.Pow(double.Parse(separadas[1]), 2)), double.Parse(separadas[2]), counter);
                            }

                        }
                        else
                        {
                            dsApp.Polilinea.Rows.Add(double.Parse(separadas[0]), double.Parse(separadas[1]), counter);
                        }
                        counter++;
                    }
                    else
                    {
                        return null;
                    }


                }
                dsApp.WriteXml("prueba.amilia");
                file.Close();
                return dsApp;
            }
            return null;
        }

        /* private dsApp CargarArchivoDeProyectoPerfil(CalculoPolilineaPerfil poli)
         {
             double x = 0, y = 0, x2 = 0, y2 = 0;
             double distancia = 0;
             double d_acumulada = 0;
             double acumulada = 0;
             dsApp dsApp = new dsApp();
             dsApp.Polilinea.Rows.Add(0, poli.Polilinea3d_Original[0].Z, 1);
             x = poli.Polilinea3d_Original[0].X;
             y = poli.Polilinea3d_Original[0].Y;
             for (int i = 1; i < poli.Polilinea3d_Original.Count; i++)
             {
                 x2 = poli.Polilinea3d_Original[i].X;
                 y2 = poli.Polilinea3d_Original[i].Y;
                 distancia = Math.Sqrt(Math.Pow(x2 - x, 2) + Math.Pow(y2 - y, 2));
                 d_acumulada += poli.Get_Acumulada(i - 1, i);
                 //d_acumulada += distancia;
                 dsApp.Polilinea.Rows.Add(d_acumulada, poli.Polilinea3d_Original[i].Z, i + 1);
                 x = x2;
                 y = y2;
             }
             return dsApp;
         }*/
        private dsApp CargarArchivoDeProyectoPerfil(CalculoPolilineaPerfil poli)
        {
            if (poli?.Polilinea3d_Original == null || poli.Polilinea3d_Original.Count == 0)
                throw new ArgumentException("La polilínea original está vacía o es nula");


            double dis1 = CalcularLongitud2D(poli.Polilinea3d_Original);
            double dis2 = CalcularLongitud2D(poli.Polilinea3d_acumulada);
            double dis3 = CalcularLongitud3D(poli.Polilinea3d_Original);
            double dis4 = CalcularLongitud3D(poli.Polilinea3d_acumulada);
            dsApp dsApp = new dsApp();

            // Agregar primer punto (distancia acumulada = 0)
            dsApp.Polilinea.Rows.Add(0, poli.Polilinea3d_Original[0].Z, 1);

            double distanciaAcumulada = 0;

            // Calcular distancia acumulada para el resto de puntos
            for (int i = 1; i < poli.Polilinea3d_Original.Count; i++)
            {
                // Obtener distancia real recorriendo la polilínea acumulada
                //double acu2 = poli.Get_Acumulada2(i - 1, i);
                double acu1 = poli.Get_Acumulada(i - 1, i);

                distanciaAcumulada += acu1;
                dsApp.Polilinea.Rows.Add(
                    distanciaAcumulada,
                    poli.Polilinea3d_Original[i].Z,
                    i + 1);
            }

            return dsApp;
        }
        /// <summary>
        /// Calcula la longitud total de una polilínea 3D (proyección horizontal X-Y)
        /// Esta es la medida más común para perfiles y trazados viales
        /// </summary>
        /// <param name="polilinea">Lista de puntos Point3d</param>
        /// <returns>Longitud total en unidades del dibujo (metros normalmente)</returns>
        public static double CalcularLongitud2D(List<Point3d> polilinea)
        {
            if (polilinea == null || polilinea.Count < 2)
                return 0;

            double longitudTotal = 0;

            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                double dx = polilinea[i + 1].X - polilinea[i].X;
                double dy = polilinea[i + 1].Y - polilinea[i].Y;

                double distanciaSegmento = Math.Sqrt(dx * dx + dy * dy);
                longitudTotal += distanciaSegmento;
            }

            return longitudTotal;
        }

        /// <summary>
        /// Calcula la longitud total de una polilínea 3D (incluyendo desnivel Z)
        /// Útil para medir longitudes reales en terreno inclinado
        /// </summary>
        /// <param name="polilinea">Lista de puntos Point3d</param>
        /// <returns>Longitud total 3D en unidades del dibujo</returns>
        public static double CalcularLongitud3D(List<Point3d> polilinea)
        {
            if (polilinea == null || polilinea.Count < 2)
                return 0;

            double longitudTotal = 0;

            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                double dx = polilinea[i + 1].X - polilinea[i].X;
                double dy = polilinea[i + 1].Y - polilinea[i].Y;
                double dz = polilinea[i + 1].Z - polilinea[i].Z;

                double distanciaSegmento = Math.Sqrt(dx * dx + dy * dy + dz * dz);
                longitudTotal += distanciaSegmento;
            }

            return longitudTotal;
        }
        private CalculoPolilineaPreferencias obtenerParametrosCalculoPolilinea()
        {
            int opcion = 1;
            double grados = 0;
            double metros = 0;
            double ratio = 0;
            double t_med = 1;
            double t_max = 1;
            double p_cluster = 2;
            double gran_r = 2500;
            int n_curvas = 2;
            int puntos_cluster = 50;
            int it = 2;
            int[] orden = new int[3];
            int solapes = 4000;
            double rotulacion = 100;
            bool rotu = false;
            int suavizado = 0;
            bool dividir = false;
            double rectas = 1;
            double curvas = 40;
            bool dividir_curvas = false;
            if (filtrado1CheckBox.Checked == true)
            {
                opcion = 1;
                grados = 0;
                metros = 0;
                ratio = 0;
            }
            else if (filtrado2CheckBox.Checked == true)
            {
                opcion = 2;
                grados = 0;
                metros = 0;
                ratio = 0;
            }
            else if (filtrado3CheckBox.Checked == true)
            {
                opcion = 3;

                if (!string.IsNullOrEmpty(filtrado3GradosTextField.Text))
                {
                    grados = double.Parse(filtrado3GradosTextField.Text);
                }
                else
                {
                    grados = 2;
                }
                if (!string.IsNullOrEmpty(filtrado3MetrosTextField.Text))
                {
                    metros = double.Parse(filtrado3MetrosTextField.Text);
                }
                else
                {
                    metros = 5;
                }
                ratio = grados / metros;
            }

            if (materialCheckBox4.Checked == true)
            {
                it = 1;
            }
            else
            {
                it = 2;
            }
            ////////
            ///
            if (!string.IsNullOrEmpty(toleranciaMediaTextField.Text))
            {
                t_med = double.Parse(toleranciaMediaTextField.Text);
            }
            else
            {
                t_med = 1;
            }
            if (!string.IsNullOrEmpty(toleranciaMaximaTextField.Text))
            {
                t_max = double.Parse(toleranciaMaximaTextField.Text);
            }
            else
            {
                t_max = 1;
            }
            if (!string.IsNullOrEmpty(clusterizacionTextField.Text))
            {
                p_cluster = double.Parse(clusterizacionTextField.Text);
            }
            else
            {
                p_cluster = 2;
            }
            if (!string.IsNullOrEmpty(curvaGranRadioTextField.Text))
            {
                gran_r = double.Parse(curvaGranRadioTextField.Text);
            }
            else
            {
                gran_r = 2500;
            }
            if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text))
            {
                n_curvas = int.Parse(nCurvasMaxTextField.Text);
            }
            else
            {
                n_curvas = 2;
            }
            if (!string.IsNullOrEmpty(pclusterizacionTextField.Text))
            {
                puntos_cluster = int.Parse(pclusterizacionTextField.Text);
            }
            else
            {
                puntos_cluster = 50;
            }
            if (!string.IsNullOrEmpty(SolapesTextField.Text))
            {
                solapes = int.Parse(SolapesTextField.Text);
            }
            else
            {
                solapes = 4000;
            }
            if (!string.IsNullOrEmpty(RotulacionTextField.Text))
            {
                rotulacion = double.Parse(RotulacionTextField.Text);
            }
            else
            {
                rotulacion = 100;
            }
            if (RotularCheckBox.Checked == true)
            {
                rotu = true;
            }
            else
            {
                rotu = false;
            }
            int filtrado1Order = (int)filtrado1ExecuteOrderNumericField.Value;
            int filtrado2Order = (int)filtrado2ExecuteOrderNumericField.Value;
            int filtrado3Order = (int)filtrado3ExecuteOrderNumericField.Value;

            orden[0] = filtrado1Order;
            orden[1] = filtrado2Order;
            orden[2] = filtrado3Order;


            if (!string.IsNullOrEmpty(filtrado3GradosTextField.Text))
            {
                grados = double.Parse(filtrado3GradosTextField.Text);
            }
            else
            {
                grados = 2;
            }
            if (!string.IsNullOrEmpty(filtrado3MetrosTextField.Text))
            {
                metros = double.Parse(filtrado3MetrosTextField.Text);
            }
            else
            {
                metros = 5;
            }
            ratio = grados / metros;
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                suavizado = int.Parse(textBox1.Text);
            }
            else
            {
                suavizado = 5;
            }

            if (materialCheckBox5.Checked)
            {
                dividir = true;
            }
            else
            {
                dividir = false;
            }
            if (materialCheckBox6.Checked)
            {
                dividir_curvas = true;
            }
            else
            {
                dividir_curvas = false;
            }
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (double.Parse(textBox2.Text) >= rectas)
                {
                    rectas = double.Parse(textBox2.Text);
                }
                else
                {
                    if (dividir)
                    {
                        MessageBox.Show("El valor de la division de la recta no puede ser inferior a: " + rectas + "m, por lo que se utilizara este valor");
                    }

                }
            }
            if (!string.IsNullOrEmpty(textBox5.Text))
            {
                if (double.Parse(textBox5.Text) >= curvas)
                {
                    curvas = double.Parse(textBox5.Text);
                }
                else
                {
                    if (dividir_curvas)
                    {
                        MessageBox.Show("El valor de la division de la curva no puede ser inferior a: " + curvas + "m, por lo que se utilizara este valor");
                    }

                }
            }





            /////////
            /*if (aplicarMultiplesFiltradosCheckBox.Checked == false)
            {
                
            }*/
            double veces = 1;
            if (!string.IsNullOrEmpty(textBox8.Text))
            {
                veces = double.Parse(textBox8.Text);
            }
            double k = 0.1;
            if (!string.IsNullOrEmpty(textBox9.Text))
            {
                k = double.Parse(textBox9.Text);
            }
            double grado_rectas = 2;
            if (!string.IsNullOrEmpty(textBox7.Text))
            {
                grado_rectas = double.Parse(textBox7.Text);
            }
            double dis_eliminar = 0;
            if (checkeliminar.Checked)
            {
                dis_eliminar = double.Parse(textBox_eliminar.Text);
            }

            CalculoPolilineaPreferencias ca = new CalculoPolilineaPreferencias();
            if (materialCheckBox12.Checked)
            {
                ca.EliClo = true;
            }
            else
            {
                ca.EliClo = false;
            }
            if (materialCheckBox15.Checked)
            {
                ca.EliRec = true;
            }
            else
            {
                ca.EliRec = false;
            }

            ca.Eli_Clo = double.Parse(textBox10.Text);
            ca.Eli_Rec = double.Parse(textBox13.Text);
            ca.Opcion = opcion;
            ca.Grados = grados;
            ca.Metros = metros;
            ca.Ratio = ratio;
            ca.It = it;
            ca.T_med = t_med;
            ca.T_max = t_max;
            ca.P_cluster = p_cluster;
            ca.Gran_r = gran_r;
            ca.N_curvas = n_curvas;
            ca.Puntos_cluster = puntos_cluster;
            ca.Solapes = solapes;
            ca.Rotulacion = rotulacion;
            ca.Orden = orden;
            ca.Rotu = rotu;
            ca.Suavizado = suavizado;
            ca.Dividir = dividir;
            ca.Rectas = rectas;
            ca.Dividir_curvas = dividir_curvas;
            ca.Curvas = curvas;
            ca.Dis_eliminar = dis_eliminar;
            ca.Gr_Rectas = grado_rectas;
            ca.Veces = veces;
            ca.K = k;
            if (materialCheckBox13.Checked)
            {
                ca.Ingenieril = true;
                if (materialCheckBox14.Checked)
                {
                    ca.Duplicado = true;
                }
                else
                {
                    ca.Duplicado = false;
                }
            }
            else
            {
                ca.Duplicado = false;
                ca.Ingenieril = false;
            }
            ca.Variacion = double.Parse(textBox11.Text);
            ca.Dis_max_puntos = double.Parse(textBox12.Text);

            return ca;
        }
        private CalculoPolilineaPreferencias obtenerParametrosCalculoPolilineaPerfil()
        {
            int opcion = 0;
            double grados = 0;
            double metros = 0;
            double ratio = 0;
            double t_med = 1;
            double t_max = 1;
            double p_cluster = 2;
            double gran_r = 2500;
            int n_curvas = 2;
            int puntos_cluster = 50;
            int it = 2;
            int[] orden = new int[3];
            int solapes = 2000;
            double rotulacion = 100;
            bool rotu = false;
            int suavizados = 0;
            if (filtrado1CheckBox.Checked == true)
            {
                opcion = 1;
                grados = 0;
                metros = 0;
                ratio = 0;
            }
            else if (filtrado2CheckBox.Checked == true)
            {
                opcion = 2;
                grados = 0;
                metros = 0;
                ratio = 0;
            }
            else if (filtrado3CheckBox.Checked == true)
            {
                opcion = 3;

                if (!string.IsNullOrEmpty(filtrado3GradosTextField.Text))
                {
                    grados = double.Parse(filtrado3GradosTextField.Text);
                }
                else
                {
                    grados = 2;
                }
                if (!string.IsNullOrEmpty(filtrado3MetrosTextField.Text))
                {
                    metros = double.Parse(filtrado3MetrosTextField.Text);
                }
                else
                {
                    metros = 5;
                }
                ratio = grados / metros;
            }

            if (materialCheckBox4.Checked == true)
            {
                it = 1;
            }
            else
            {
                it = 2;
            }

            if (aplicarMultiplesFiltradosCheckBox.Checked == false)
            {
                if (!string.IsNullOrEmpty(toleranciaMediaTextField.Text))
                {
                    t_med = double.Parse(toleranciaMediaTextField.Text);
                }
                else
                {
                    t_med = 1;
                }
                if (!string.IsNullOrEmpty(toleranciaMaximaTextField.Text))
                {
                    t_max = double.Parse(toleranciaMaximaTextField.Text);
                }
                else
                {
                    t_max = 1;
                }
                if (!string.IsNullOrEmpty(clusterizacionTextField.Text))
                {
                    p_cluster = double.Parse(clusterizacionTextField.Text);
                }
                else
                {
                    p_cluster = 2;
                }
                if (!string.IsNullOrEmpty(curvaGranRadioTextField.Text))
                {
                    gran_r = double.Parse(curvaGranRadioTextField.Text);
                }
                else
                {
                    gran_r = 2500;
                }
                if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text))
                {
                    n_curvas = int.Parse(nCurvasMaxTextField.Text);
                }
                else
                {
                    n_curvas = 2;
                }
                if (!string.IsNullOrEmpty(pclusterizacionTextField.Text))
                {
                    puntos_cluster = int.Parse(pclusterizacionTextField.Text);
                }
                else
                {
                    puntos_cluster = 50;
                }
                if (!string.IsNullOrEmpty(SolapesTextField.Text))
                {
                    solapes = int.Parse(SolapesTextField.Text);
                }
                else
                {
                    solapes = 2000;
                }
                if (!string.IsNullOrEmpty(RotulacionTextField.Text))
                {
                    rotulacion = double.Parse(RotulacionTextField.Text);
                }
                else
                {
                    rotulacion = 100;
                }
                if (RotularCheckBox.Checked == true)
                {
                    rotu = true;
                }
                else
                {
                    rotu = false;
                }
                int filtrado1Order = (int)filtrado1ExecuteOrderNumericField.Value;
                int filtrado2Order = (int)filtrado2ExecuteOrderNumericField.Value;
                int filtrado3Order = (int)filtrado3ExecuteOrderNumericField.Value;

                orden[0] = filtrado1Order;
                orden[1] = filtrado2Order;
                orden[2] = filtrado3Order;

                if (!string.IsNullOrEmpty(filtrado3GradosTextField.Text))
                {
                    grados = double.Parse(filtrado3GradosTextField.Text);
                }
                else
                {
                    grados = 5;
                }
                if (!string.IsNullOrEmpty(filtrado3MetrosTextField.Text))
                {
                    metros = double.Parse(filtrado3MetrosTextField.Text);
                }
                else
                {
                    metros = 5;
                }
                ratio = grados / metros;
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    suavizados = int.Parse(textBox1.Text);
                }
                else
                {
                    suavizados = 0;
                }

            }
            double dis = 0;
            if (!string.IsNullOrEmpty(textBox_eliminar.Text))
            {
                dis = int.Parse(textBox_eliminar.Text);
            }
            else
            {
                dis = 0;
            }
            CalculoPolilineaPreferencias caPerfil = new CalculoPolilineaPreferencias();
            caPerfil.Opcion = opcion;
            caPerfil.Grados = grados;
            caPerfil.Metros = metros;
            caPerfil.Ratio = ratio;
            caPerfil.It = it;
            caPerfil.T_med = t_med;
            caPerfil.T_max = t_max;
            caPerfil.P_cluster = p_cluster;
            caPerfil.Gran_r = gran_r;
            caPerfil.N_curvas = n_curvas;
            caPerfil.Puntos_cluster = puntos_cluster;
            caPerfil.Solapes = solapes;
            caPerfil.Rotulacion = rotulacion;
            caPerfil.Orden = orden;
            caPerfil.Rotu = rotu;
            caPerfil.Suavizado = suavizados;
            caPerfil.Dis_eliminar = dis;
            return caPerfil;
        }
        /// <summary>
        /// Se ejecuta el primer paso para la obtención del trazado
        /// </summary>
        private void ejecutar1ButtonClick(object sender, EventArgs eventArgs)
        {
            if (this.pasosEjecutados > -1)
            {
                if (this.calculoPolilinea != null)
                {
                    /*
                    * 
                    * Primero dividimos todos los cambios de giro y comprobamos que la tolerancia media *3
                    * a la recta es menor suavizamos 10 veces ese tramos 
                    * 
                    */
                    int puntos_iniciales = 0;
                    try
                    {
                        if (Lista_original_2d != null)
                        {
                            if (Lista_original_2d.Count() > 0)
                            {
                                puntos_iniciales = Lista_original_2d.Count() - 1;
                            }
                            else if (Lista_original_3d != null)
                            {

                                puntos_iniciales = Lista_original_3d.Count() - 1;
                            }
                        }
                        else if (Lista_original_3d != null)
                        {
                            if (Lista_original_3d.Count() > 0)
                            {
                                puntos_iniciales = Lista_original_3d.Count() - 1;
                            }
                            else if (Lista_original_2d != null)
                            {

                                puntos_iniciales = Lista_original_2d.Count() - 1;
                            }
                        }
                    }
                    catch
                    {

                    }

                    int[] r = calculoPolilinea.Propiedades(puntos_iniciales, null);
                    tipoA = r[0];
                    tipoC = r[1];
                    calculoPolilinea.Cambios_Sentido(calculoPolilineaPreferencias.T_med);
                    PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);
                    if (materialCheckBox8.Checked == true)
                    {
                        polilineaInfoPanel.Show();
                    }
                    calculoPolilinea.nueva_relacion();
                    calculoPolilinea.Set_minimos();
                    calculoPolilinea.Set_grupo();
                    calculoPolilinea.Set_recta_curva(calculoPolilineaPreferencias.P_cluster);
                    int pol = calculoPolilinea.Dividir_Polilinea();
                    PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);
                    if (materialCheckBox8.Checked == true)
                    {
                        polilineaInfoPanel2.Show();
                    }
                    for (int i = 0; i <= pol; i++)
                    {
                        calculoPolilinea.Seleccionar_Polilinea(i);
                        calculoPolilinea.Entidades_Curvas(calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.P_cluster);
                        calculoPolilinea.Recorrido(calculoPolilineaPreferencias.Dividir_curvas, calculoPolilineaPreferencias.Curvas);
                        calculoPolilinea.Combinacion(calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.N_curvas,
                            calculoPolilineaPreferencias.Puntos_cluster, calculoPolilineaPreferencias.Gran_r, calculoPolilineaPreferencias.Dividir_curvas);
                        calculoPolilinea.Limpiar(i);

                    }

                    calculoPolilinea.Unir_Componentes();
                    //calculoPolilinea.Dibujar_entidades(1);

                    calculoPolilinea.Aniadir_Rectar_Radio_Grande();
                    if (materialCheckBox7.Checked)
                    {
                        calculoPolilinea.Comprobacion(calculoPolilineaPreferencias.Dividir, calculoPolilineaPreferencias.T_max,
                        calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.Rectas, calculoPolilineaPreferencias.Gr_Rectas, calculoPolilineaPreferencias.Veces, calculoPolilineaPreferencias.K, true);
                    }
                    else
                    {
                        calculoPolilinea.Comprobacion(calculoPolilineaPreferencias.Dividir, calculoPolilineaPreferencias.T_max,
                        calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.Rectas, calculoPolilineaPreferencias.Gr_Rectas, calculoPolilineaPreferencias.Veces, calculoPolilineaPreferencias.K, false);
                    }

                    if (calculoPolilineaPreferencias.Dividir_curvas)
                    {
                        calculoPolilinea.Recta_curva_dividida();
                    }
                    //calculoPolilinea.Dibujar_entidades(2);

                    VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                    List_verificacion.Add(verificacionComponentesStatus);
                    aniadir_a_list(calculoPolilinea.Componentes);
                    materialFlatButton7.Visible = true;
                    ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);
                    if (materialCheckBox8.Checked == true)
                    {
                        componentesInfoPanel.Show();
                    }


                    this.pasosEjecutados = 1;
                    this.paso1EjecutadoTextView.Visible = true;
                    MessageBox.Show("Revise autocad para ver la salida de la etapa 1 del algoritmo");
                }
                else
                {
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            }
            else
            {
                MessageBox.Show("Calculo polilinea no preparado");
            }
        }

        private void ejecutar1ButtonClick_Automatico(int suavizado)
        {
            if (this.pasosEjecutados > -1)
            {
                calculoPolilinea = new CalculoPolilinea(Lista_original_2d);
                calculoPolilinea.EliClo = calculoPolilineaPreferencias.EliClo;
                calculoPolilinea.Eli_Clo = calculoPolilineaPreferencias.Eli_Clo;
                calculoPolilinea.EliRec = calculoPolilineaPreferencias.EliRec;
                calculoPolilinea.Eli_Rec = calculoPolilineaPreferencias.Eli_Rec;
                if (suavizado > 0)
                {
                    calculoPolilinea.Suavizar_automatico(suavizado);
                }
                if (this.calculoPolilinea != null)
                {
                    /*
                    * 
                    * Primero dividimos todos los cambios de giro y comprobamos que la tolerancia media *3
                    * a la recta es menor suavizamos 10 veces ese tramos 
                    * 
                    */
                    if (calculoPolilineaPreferencias.Ingenieril)
                    {
                        calculoPolilinea.Crear_polilinea_ingenieril(calculoPolilinea.Polilinea, rect_ing, curv_ing,
                            calculoPolilineaPreferencias.Dis_max_puntos);
                        P_C = curv_ing;
                    }

                    calculoPolilinea.Cambios_Sentido(calculoPolilineaPreferencias.T_med);
                    //PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);

                    calculoPolilinea.nueva_relacion();
                    calculoPolilinea.Set_minimos();
                    calculoPolilinea.Set_grupo();

                    calculoPolilinea.Set_recta_curva(P_C);
                    calculoPolilinea.Propiedades_B();
                    int pol = calculoPolilinea.Dividir_Polilinea();
                    //PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);

                    if (calculoPolilineaPreferencias.Ingenieril)
                    {
                        calculoPolilinea.ComponentesIngenieril();
                        calculoPolilinea.Add_azimutes();
                    }
                    else
                    {
                        for (int i = 0; i <= pol; i++)
                        {
                            calculoPolilinea.Seleccionar_Polilinea(i);
                            calculoPolilinea.Entidades_Curvas(calculoPolilineaPreferencias.T_max, P_C);
                            calculoPolilinea.Recorrido(false, calculoPolilineaPreferencias.Curvas);
                            calculoPolilinea.Combinacion(calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.T_max, N_C,
                                N_P, calculoPolilineaPreferencias.Gran_r, false);
                            calculoPolilinea.Limpiar(i);

                        }

                        calculoPolilinea.Unir_Componentes();
                        //calculoPolilinea.Dibujar_entidades(1);

                        calculoPolilinea.Aniadir_Rectar_Radio_Grande();
                        calculoPolilinea.Comprobacion(calculoPolilineaPreferencias.Dividir, calculoPolilineaPreferencias.T_max,
                            calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.Rectas, calculoPolilineaPreferencias.Gr_Rectas, calculoPolilineaPreferencias.Veces, calculoPolilineaPreferencias.K, false);
                    }
                    //calculoPolilinea.Dibujar_entidades(2);

                    // VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                    // List_verificacion.Add(verificacionComponentesStatus);
                    aniadir_a_list(calculoPolilinea.Componentes);
                    //materialFlatButton7.Visible = true;
                    //ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);

                    this.pasosEjecutados = 1;
                    //this.paso1EjecutadoTextView.Visible = true;
                    //MessageBox.Show("Revise autocad para ver la salida de la etapa 1 del algoritmo");
                    //ejecutar2ButtonClick_Automatico();
                }
                else
                {
                    //MessageBox.Show("Calculo polilinea no inicializado");

                }
            }
            else
            {
                //MessageBox.Show("Calculo polilinea no preparado");
            }
        }
        private void ejecutar1ButtonClick()
        {
            if (this.pasosEjecutados > -1)
            {
                if (this.calculoPolilinea != null)
                {
                    /*
                    * 
                    * Primero dividimos todos los cambios de giro y comprobamos que la tolerancia media *3
                    * a la recta es menor suavizamos 10 veces ese tramos 
                    * 
                    */
                    //calculoPolilinea.Crear_polilinea_ingenieril(calculoPolilinea.Polilinea);

                    //var result_poli = calculoPolilinea.Crear_funcion_polilinea();
                    if (calculoPolilineaPreferencias.Ingenieril)
                    {
                        calculoPolilinea.Crear_polilinea_ingenieril(calculoPolilinea.Polilinea, calculoPolilineaPreferencias.Variacion, calculoPolilineaPreferencias.P_cluster,
                            calculoPolilineaPreferencias.Dis_max_puntos);
                    }


                    //calculoPolilinea.Componentes_Eje_Ingenieril(result_poli);

                    calculoPolilinea.Cambios_Sentido(calculoPolilineaPreferencias.T_med);
                    //PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);

                    calculoPolilinea.nueva_relacion();
                    calculoPolilinea.Set_minimos();
                    calculoPolilinea.Set_grupo();

                    calculoPolilinea.Set_recta_curva(calculoPolilineaPreferencias.P_cluster);
                    calculoPolilinea.Propiedades_B();
                    int pol = calculoPolilinea.Dividir_Polilinea();
                    //PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);



                    if (calculoPolilineaPreferencias.Ingenieril)
                    {
                        calculoPolilinea.ComponentesIngenieril();
                        calculoPolilinea.Add_azimutes();
                        //calculoPolilinea.Dibujar_entidades(1, "entidades");
                    }
                    else
                    {
                        for (int i = 0; i <= pol; i++)
                        {
                            calculoPolilinea.Seleccionar_Polilinea(i);
                            calculoPolilinea.Entidades_Curvas(calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.P_cluster);
                            calculoPolilinea.Recorrido(calculoPolilineaPreferencias.Dividir_curvas, calculoPolilineaPreferencias.Curvas);
                            calculoPolilinea.Combinacion(calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.N_curvas,
                                calculoPolilineaPreferencias.Puntos_cluster, calculoPolilineaPreferencias.Gran_r, calculoPolilineaPreferencias.Dividir_curvas);
                            calculoPolilinea.Limpiar(i);

                        }
                        calculoPolilinea.Unir_Componentes();
                        //calculoPolilinea.Dibujar_entidades(1,"entidades");

                        calculoPolilinea.Aniadir_Rectar_Radio_Grande();
                        if (materialCheckBox7.Checked)
                        {
                            calculoPolilinea.Comprobacion(calculoPolilineaPreferencias.Dividir, calculoPolilineaPreferencias.T_max,
                            calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.Rectas, calculoPolilineaPreferencias.Gr_Rectas, calculoPolilineaPreferencias.Veces, calculoPolilineaPreferencias.K, true);
                        }
                        else
                        {
                            calculoPolilinea.Comprobacion(calculoPolilineaPreferencias.Dividir, calculoPolilineaPreferencias.T_max,
                            calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.Rectas, calculoPolilineaPreferencias.Gr_Rectas, calculoPolilineaPreferencias.Veces, calculoPolilineaPreferencias.K, false);
                        }

                        if (calculoPolilineaPreferencias.Dividir_curvas)
                        {
                            calculoPolilinea.Recta_curva_dividida();
                        }
                    }


                    //calculoPolilinea.Dibujar_entidades(2);

                    //VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                    // List_verificacion.Add(verificacionComponentesStatus);
                    aniadir_a_list(calculoPolilinea.Componentes);
                    //materialFlatButton7.Visible = true;
                    //ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);


                    this.pasosEjecutados = 1;
                    //this.paso1EjecutadoTextView.Visible = true;
                    //MessageBox.Show("Revise autocad para ver la salida de la etapa 1 del algoritmo");
                    ejecutar2ButtonClick();
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Maximum = 1;
                progressBar1.Value = 0;
                MessageBox.Show("Calculo polilinea no preparado");
            }
        }
        /// <summary>
        /// Se ejecuta el segundo paso para la obtención del trazado
        /// </summary>
        private void ejecutar2ButtonClick(object sender, EventArgs eventArgs)
        {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
            if (pasosEjecutados > 0)
            {
                if (this.calculoPolilinea != null)
                {
                    this.calculoPolilinea.removeAllViabilidadListener();
                    this.calculoPolilinea.addViabilidadListener(this);

                    List<ViabilidadComponentesStatus> trazaViabilidadComponentes = calculoPolilinea.viabilidad(this.calculoPolilineaPreferencias.Gran_r);

                    //calculoPolilinea.Dibujar_entidades(3);

                    this.pasosEjecutados = 2;
                    this.paso2EjecutadoTextView.Visible = true;

                    //mostrar panel de la traza de viabilidad de los componentes
                    TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(trazaViabilidadComponentes, this.calculoPolilinea.Componentes, "Trazas viabilidad etapa 2");
                    if (materialCheckBox9.Checked == true)
                    {
                        tvi.Show();
                    }
                    List_traza.Add(trazaViabilidadComponentes);

                    if (!trazaViabilidadComponentes.Any())
                    {
                        MessageBox.Show("No se detectan problemas de viabilidad en los componentes o se han resuelto anteriormente");
                    }
                    MessageBox.Show("Revise autocad para ver la salida de la etapa 2 del algoritmo");
                    VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                    List_verificacion.Add(verificacionComponentesStatus);
                    aniadir_a_list(calculoPolilinea.Componentes);
                    materialFlatButton8.Visible = true;
                    ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);
                    if (materialCheckBox9.Checked == true)
                    {
                        componentesInfoPanel.Show();
                    }

                }
                else
                {
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            }
            else
            {
                MessageBox.Show("El paso 1 no se ha ejecutado todavia");
            }
        }
        private void ejecutar2ButtonClick()
        {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
            if (pasosEjecutados > 0)
            {
                if (this.calculoPolilinea != null)
                {
                    this.calculoPolilinea.removeAllViabilidadListener();
                    this.calculoPolilinea.addViabilidadListener(this);
                    List<ViabilidadComponentesStatus> trazaViabilidadComponentes = calculoPolilinea.viabilidad(this.calculoPolilineaPreferencias.Gran_r);

                    //calculoPolilinea.Dibujar_entidades(3,"Prueba");

                    this.pasosEjecutados = 2;
                    //this.paso2EjecutadoTextView.Visible = true;

                    //mostrar panel de la traza de viabilidad de los componentes
                    /*TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(trazaViabilidadComponentes, this.calculoPolilinea.Componentes, "Trazas viabilidad etapa 2");
                    if (materialCheckBox9.Checked == true)
                    {
                        tvi.Show();
                    }*/
                    List_traza.Add(trazaViabilidadComponentes);

                    if (!trazaViabilidadComponentes.Any())
                    {
                        //MessageBox.Show("No se detectan problemas de viabilidad en los componentes o se han resuelto anteriormente");
                    }
                    //MessageBox.Show("Revise autocad para ver la salida de la etapa 2 del algoritmo");
                    //VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                    //List_verificacion.Add(verificacionComponentesStatus);
                    aniadir_a_list(calculoPolilinea.Componentes);
                    //materialFlatButton8.Visible = true;
                    //ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);
                    ejecutar3ButtonClick();

                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Maximum = 1;
                progressBar1.Value = 0;
                MessageBox.Show("El paso 1 no se ha ejecutado todavia");
            }
        }
        private void ejecutar2ButtonClick_Automatico()
        {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
            if (pasosEjecutados > 0)
            {
                if (this.calculoPolilinea != null)
                {
                    this.calculoPolilinea.removeAllViabilidadListener();
                    this.calculoPolilinea.addViabilidadListener(this);

                    List<ViabilidadComponentesStatus> trazaViabilidadComponentes = calculoPolilinea.viabilidad(this.calculoPolilineaPreferencias.Gran_r);

                    //calculoPolilinea.Dibujar_entidades(3);

                    this.pasosEjecutados = 2;
                    //this.paso2EjecutadoTextView.Visible = true;

                    //mostrar panel de la traza de viabilidad de los componentes
                    /* TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(trazaViabilidadComponentes, this.calculoPolilin    ea.Componentes, "Trazas viabilidad etapa 2");
                     if (materialCheckBox9.Checked == true)
                     {
                         tvi.Show();
                     }*/
                    List_traza.Add(trazaViabilidadComponentes);

                    if (!trazaViabilidadComponentes.Any())
                    {
                        //MessageBox.Show("No se detectan problemas de viabilidad en los componentes o se han resuelto anteriormente");
                    }
                    //MessageBox.Show("Revise autocad para ver la salida de la etapa 2 del algoritmo");
                    //VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                    //List_verificacion.Add(verificacionComponentesStatus);
                    aniadir_a_list(calculoPolilinea.Componentes);
                    //materialFlatButton8.Visible = true;
                    //ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);
                    //ejecutar3ButtonClick_Automatico();

                }
                else
                {
                    //MessageBox.Show("Calculo polilinea no inicializado");
                }
            }
            else
            {
                // MessageBox.Show("El paso 1 no se ha ejecutado todavia");
            }
        }
        public void Ejecutar_enlaces()
        {
            DateTime tiempo1 = DateTime.Now;
            Process process = new Process();
            TimeSpan id = Process.GetCurrentProcess().TotalProcessorTime;
            List<ViabilidadComponentesStatus> viabilidadEnlaces = null;
            try
            {

                viabilidadEnlaces = calculoPolilinea.Enlaces(this.calculoPolilineaPreferencias.Gran_r, calculoPolilineaPreferencias.Solapes);

            }
            catch
            {
                MessageBox.Show("No se ha encontrado una solución óptima");
            }

            try
            {
                if (viabilidadEnlaces != null)
                {
                    //calculoPolilinea.Dibujar_entidades(4);
                    calculoPolilinea.Crear_Trazado(this.calculoPolilineaPreferencias.Gran_r);
                    calculoPolilinea.Dibujar_Todo(this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, Lista_original_2d, distancia_menor, false, 0);
                }

            }
            catch
            {
                MessageBox.Show("Se ha detectado un error al crear la entidad. Se dibujará lo creado");
            }
            finally
            {
                if (viabilidadEnlaces != null)
                {
                    calculoPolilinea.Crear_Trazado_Error(this.calculoPolilineaPreferencias.Gran_r, documento);
                }
            }

            this.pasosEjecutados = 3;
            this.paso3EjecutadoTextView.Visible = true;

            //mostrar panel de la traza de viabilidad de los componentes
            TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(viabilidadEnlaces, this.calculoPolilinea.Componentes, "Trazas viabilidad enlaces etapa 3");
            if (materialCheckBox10.Checked == true)
            {
                tvi.Show();
            }
            List_traza.Add(viabilidadEnlaces);
            VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
            ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);
            if (materialCheckBox10.Checked == true)
            {
                componentesInfoPanel.Show();
            }
            List_verificacion.Add(verificacionComponentesStatus);
            aniadir_a_list(calculoPolilinea.Componentes);
            materialFlatButton9.Visible = true;
            ComponentesInfoPanel componentesInfoPanel2 = new ComponentesInfoPanel(calculoPolilinea.Componentes_iniciales);
            if (materialCheckBox10.Checked == true)
            {
                componentesInfoPanel2.Show();
            }

            MessageBox.Show("Revise autocad para ver la salida de la etapa 3 del algoritmo");
        }
        public void time()
        {
            DateTime tiempo1 = DateTime.Now;
            while (true)
            {
                DateTime tiempo2 = DateTime.Now;
                TimeSpan total = new TimeSpan(tiempo2.Ticks - tiempo1.Ticks);
                if (total.TotalSeconds > 600 && total.TotalSeconds < 700)
                {
                    DialogResult result = MessageBox.Show("Se ha ejecutado durante 10 minutos. ¿Quiere continuar? Si no desea continuar el programa se cerrará.", "Información", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        foreach (Process proc in Process.GetProcessesByName("acad"))
                        {
                            proc.Kill();
                        }
                    }
                }
                if (total.TotalSeconds > 1200 && total.TotalSeconds < 1300)
                {
                    DialogResult result = MessageBox.Show("Se ha ejecutado durante 20 minutos. ¿Quiere continuar? Si no desea continuar el programa se cerrará.", "Información", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        foreach (Process proc in Process.GetProcessesByName("acad"))
                        {
                            proc.Kill();
                        }
                    }
                }
                if (total.TotalSeconds > 1800 && total.TotalSeconds < 1900)
                {
                    DialogResult result = MessageBox.Show("Se ha ejecutado durante 30 minutos. ¿Quiere continuar? Si no desea continuar el programa se cerrará.", "Información", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        foreach (Process proc in Process.GetProcessesByName("acad"))
                        {
                            proc.Kill();
                        }
                    }
                }
                if (total.TotalSeconds > 2400 && total.TotalSeconds < 2500)
                {
                    DialogResult result = MessageBox.Show("Se ha ejecutado durante 40 minutos. ¿Quiere continuar? Si no desea continuar el programa se cerrará.", "Información", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        foreach (Process proc in Process.GetProcessesByName("acad"))
                        {
                            proc.Kill();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Se ejecuta el tercer paso para la obtención del trazado
        /// </summary>
        private void ejecutar3ButtonClick(object sender, EventArgs eventArgs)
        {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;

            if (this.pasosEjecutados > 1)
            {
                if (this.calculoPolilinea != null)
                {
                    List<ViabilidadComponentesStatus> viabilidadEnlaces = null;
                    try
                    {

                        viabilidadEnlaces = calculoPolilinea.Enlaces(this.calculoPolilineaPreferencias.Gran_r, calculoPolilineaPreferencias.Solapes);

                        //th2.Abort();


                        //calculoPolilinea.Dibujar_entidades(4);
                        calculoPolilinea.Crear_Trazado(this.calculoPolilineaPreferencias.Gran_r);
                        calculoPolilinea.Dibujar_Todo(this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, Lista_original_2d, distancia_menor, false, 0);
                        this.pasosEjecutados = 3;
                        this.paso3EjecutadoTextView.Visible = true;

                        //mostrar panel de la traza de viabilidad de los componentes
                        TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(viabilidadEnlaces, this.calculoPolilinea.Componentes, "Trazas viabilidad enlaces etapa 3");
                        if (materialCheckBox10.Checked == true)
                        {
                            tvi.Show();
                        }
                        List_traza.Add(viabilidadEnlaces);
                        VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                        ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);
                        if (materialCheckBox10.Checked == true)
                        {
                            componentesInfoPanel.Show();
                        }
                        List_verificacion.Add(verificacionComponentesStatus);
                        aniadir_a_list(calculoPolilinea.Componentes);
                        materialFlatButton9.Visible = true;
                        ComponentesInfoPanel componentesInfoPanel2 = new ComponentesInfoPanel(calculoPolilinea.Componentes_iniciales);
                        if (materialCheckBox10.Checked == true)
                        {
                            componentesInfoPanel2.Show();
                        }
                        MessageBox.Show("Revise autocad para ver la salida de la etapa 3 del algoritmo");
                    }
                    catch
                    {
                        MessageBox.Show("No ha dado una solución correcta. Pruebe con otros parámetros");
                    }
                    finally
                    {
                        //calculoPolilinea.Crear_Trazado_Error(this.calculoPolilineaPreferencias.Gran_r);
                    }




                }
                else
                {
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            }
            else
            {
                MessageBox.Show("El paso 2 no se ha ejecutado todavia");
            }
        }
        private void ejecutar3ButtonClick()
        {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
            bool terminar;
            if (this.pasosEjecutados > 1)
            {
                if (this.calculoPolilinea != null)
                {
                    List<ViabilidadComponentesStatus> viabilidadEnlaces = null;
                    try
                    {
                        viabilidadEnlaces = calculoPolilinea.Enlaces(this.calculoPolilineaPreferencias.Gran_r, calculoPolilineaPreferencias.Solapes);
                        //th2.Abort();

                        /*ComponentesInfoPanel componentesInfoPanel3 = new ComponentesInfoPanel(calculoPolilinea.Componentes);
                        componentesInfoPanel3.Show();*/
                        //calculoPolilinea.Dibujar_entidades(4,documento);
                        calculoPolilinea.Crear_Trazado(this.calculoPolilineaPreferencias.Gran_r);
                        Lista_Resultados.Add(calculoPolilinea.Dibujar_Todo(this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, Lista_original_2d, distancia_menor, true, 0));
                        if (Lista_Resultados[Lista_Resultados.Count - 1].Item2 == 1000.0)
                        {
                            MessageBox.Show("No ha dado una solución correcta. Pruebe con otros parámetros");
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            progressBar1.Maximum = 1;
                            progressBar1.Value = 0;
                        }
                        else
                        {

                            calculoPolilinea.DibujarTrazado(calculoPolilinea.Mcomponenetes, documento);

                            calculoPolilinea.Rotulado_final(calculoPolilinea.Mcomponenetes, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, documento);
                            this.pasosEjecutados = 3;


                            MessageBox.Show("Revise autocad para ver la salida del algoritmo");
                            //materialFlatButton27.Visible = true;
                            Guardar_Json(calculoPolilinea.Componentes, Lista_original_3d, calculoPolilinea.Mcomponenetes, calculoPolilinea.Trazado_prueba);
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            progressBar1.Maximum = 1;
                            progressBar1.Value = 1;
                        }

                        //this.paso3EjecutadoTextView.Visible = true;

                        //mostrar panel de la traza de viabilidad de los componentes
                        //TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(viabilidadEnlaces, this.calculoPolilinea.Componentes, "Trazas viabilidad enlaces etapa 3");

                        /*List_traza.Add(viabilidadEnlaces);
                        VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                        ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);

                        List_verificacion.Add(verificacionComponentesStatus);*/
                        //aniadir_a_list(calculoPolilinea.Componentes);
                        //materialFlatButton9.Visible = true;
                        //ComponentesInfoPanel componentesInfoPanel2 = new ComponentesInfoPanel(calculoPolilinea.Componentes_iniciales);



                    }
                    catch
                    {
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 0;
                        MessageBox.Show("No ha dado una solución correcta. Pruebe con otros parámetros");
                    }
                    finally
                    {
                        //calculoPolilinea.Crear_Trazado_Error(this.calculoPolilineaPreferencias.Gran_r);
                    }




                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Maximum = 1;
                progressBar1.Value = 0;
                MessageBox.Show("El paso 2 no se ha ejecutado todavia");
            }
        }
        private void ejecutar3ButtonClick_Automatico(int contador_resultados_numero)
        {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
            bool terminar;
            if (this.pasosEjecutados > 1)
            {
                if (this.calculoPolilinea != null)
                {
                    List<ViabilidadComponentesStatus> viabilidadEnlaces = null;
                    try
                    {

                        viabilidadEnlaces = calculoPolilinea.Enlaces(this.calculoPolilineaPreferencias.Gran_r, calculoPolilineaPreferencias.Solapes);
                        //th2.Abort();

                        int contaRes = Lista_Resultados.Count();
                        //calculoPolilinea.Dibujar_entidades(4);
                        calculoPolilinea.Crear_Trazado(this.calculoPolilineaPreferencias.Gran_r);
                        Lista_Resultados.Add(calculoPolilinea.Dibujar_Todo(this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, Lista_original_2d, distancia_menor, false, contador_resultados_numero));
                        //materialFlatButton17_Click();
                        Resultados r = new Resultados();

                        r.N_C = N_C;
                        r.Por_C = P_C;
                        if (this.calculoPolilineaPreferencias.Ingenieril)
                        {
                            r.N_C = (int)rect_ing;
                            r.Por_C = curv_ing;
                        }
                        r.Pun_C = N_P;
                        r.G_U = calculoPolilineaPreferencias.Gr_Rectas;
                        r.D_R = calculoPolilineaPreferencias.Dividir;
                        r.Num_C = calculoPolilinea.Mcomponenetes.Count;
                        r.Resultado = contador_resultados_numero;
                        L_Resultados.Add(r);

                        this.pasosEjecutados = 3;
                        //this.paso3EjecutadoTextView.Visible = true;

                        //mostrar panel de la traza de viabilidad de los componentes
                        /*TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(viabilidadEnlaces, this.calculoPolilinea.Componentes, "Trazas viabilidad enlaces etapa 3");

                        List_traza.Add(viabilidadEnlaces);
                        VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                        ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);

                        List_verificacion.Add(verificacionComponentesStatus);*/
                        //aniadir_a_list(calculoPolilinea.Componentes);
                        //materialFlatButton9.Visible = true;
                        //ComponentesInfoPanel componentesInfoPanel2 = new ComponentesInfoPanel(calculoPolilinea.Componentes_iniciales);



                    }
                    catch
                    {

                        //MessageBox.Show("No ha dado una solución correcta. Pruebe con otros parámetros");
                    }
                    finally
                    {
                        //calculoPolilinea.Crear_Trazado_Error(this.calculoPolilineaPreferencias.Gran_r);
                    }




                }
                else
                {
                    //MessageBox.Show("Calculo polilinea no inicializado");
                }
            }
            else
            {
                //MessageBox.Show("El paso 2 no se ha ejecutado todavia");
            }
        }
        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                materialLabel47.Text = "";
                componentes_automaticas = null;
                conta_apartado = 1;
                materialLabel48.Text = "";
                //Resetear estado actual
                Desabilitar_opciones();
                this.calculoPolilinea = null;
                this.pasosEjecutados = -1;
                this.paso1EjecutadoTextView.Visible = false;
                this.paso2EjecutadoTextView.Visible = false;
                this.paso3EjecutadoTextView.Visible = false;
                this.calculoPolilineaStatusTextView.Visible = false;
                this.ejecutarViabilidadSinParar = false;
                this.detenerEnIteracion = false;
                this.iteracion = -1;
                materialFlatButton7.Visible = false;
                materialFlatButton8.Visible = false;
                materialFlatButton9.Visible = false;
                List_verificacion.Clear();
                List_componentes.Clear();
                List_polilinea.Clear();
                List_traza.Clear();
                progressBar1.Maximum = 5;
                progressBar1.Style = ProgressBarStyle.Marquee;
                distancia_menor = 100;
                Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int, List<Componente>>>();
                if (comprobar())
                {
                    dsApp dsApp = this.abrirArchivoDeProyecto();

                    if (dsApp != null)
                    {
                        //obtener parametros de inicializacion de CalculoPolilineaController
                        this.calculoPolilineaPreferencias = this.obtenerParametrosCalculoPolilinea();

                        if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, documento);
                        }
                        else
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, documento);
                        }
                        Lista_original_2d.Clear();
                        bool continuar = true;
                        if (Poli_Larga(this.calculoPolilinea.Polilinea))
                        {
                            DialogResult result = MessageBox.Show("La polilinea es demasiado extensa y con muchos puntos, esto provocará que el resultado tarde mucho en calcularse.\n\nSe aconseja reducir el número de puntos de la polilínea utilizando 'FILTRAR Y GUARDAR' de la  pestaña 'Filtrar puntos' o reducir la distancia que se desea calcular.\n\nSi desea volver a filtrar la polilínea o reducir la distancia pulse en aceptar, de lo contrario pulse en cancelar y el proceso seguirá. ", "Información", MessageBoxButtons.OKCancel);

                            if (result == DialogResult.OK)
                            {
                                continuar = false;
                            }
                        }
                        if (continuar)
                        {
                            foreach (Punto p in this.calculoPolilinea.Polilinea)
                            {
                                Lista_original_2d.Add(new Point2d(p.p.X, p.p.Y));
                            }
                            this.calculoPolilineaStatusTextView.Visible = true;
                            this.pasosEjecutados = 0;
                            if (materialCheckBox12.Checked)
                            {
                                calculoPolilinea.EliClo = true;
                            }
                            else
                            {
                                calculoPolilinea.EliClo = false;
                            }
                            if (materialCheckBox15.Checked)
                            {
                                calculoPolilinea.EliRec = true;
                            }
                            else
                            {
                                calculoPolilinea.EliRec = false;
                            }

                            calculoPolilinea.Eli_Clo = double.Parse(textBox10.Text);
                            calculoPolilinea.Eli_Rec = double.Parse(textBox13.Text);
                            //MessageBox.Show("CalculoPolilinea inicializado");
                            ejecutar1ButtonClick();

                        }
                        else
                        {
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            progressBar1.Maximum = 1;
                            progressBar1.Value = 0;
                            materialFlatButton17_Click();
                        }


                    }
                    else
                    {
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 0;
                        MessageBox.Show("Error de formato o al abrir el archivo del proyecto");
                    }
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    MessageBox.Show("Error. El orden de los filtros esta repetido");
                }
            }
            this.calculoPolilineaStatusTextView.Visible = false;

        }

        private void materialFlatButton3_Click(object sender, EventArgs e)
        {
            string miFileOut = string.Empty;
            OpenFileDialog miDialogo = new OpenFileDialog();
            miDialogo.Title = "AMILIA" + " | " + "Selecciona un Archivo de Proyecto";
            miDialogo.Filter = "Ditel Project Files (*.amilia)|*.amilia";
            miDialogo.Multiselect = false;
            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                System.IO.StreamReader file = new System.IO.StreamReader(@miFileOut);
                dsApp a = new dsApp();
                a.ReadXml(file);
            }
            else
            {

            }
        }

        private void materialCheckBox1_Click(object sender, EventArgs e)
        {
            /* if (filtrado1CheckBox.Checked==true)
             {
                 filtrado1CheckBox.Checked = false;
             }
             else
             {
                 filtrado1CheckBox.Checked = true;
             }
             */
            filtrado2CheckBox.Checked = false;
            filtrado3CheckBox.Checked = false;
            filtrado3GradosTextField.Enabled = false;
            filtrado3MetrosTextField.Enabled = false;
            materialLabel1.Enabled = false;
            materialLabel2.Enabled = false;
            materialCheckBox4.Enabled = true;
        }
        private void materialCheckBox2_Click(object sender, EventArgs e)
        {
            filtrado1CheckBox.Checked = false;
            /* if (filtrado2CheckBox.Checked == true)
             {
                 filtrado2CheckBox.Checked = false;
             }
             else
             {
                 filtrado2CheckBox.Checked = true;
             }*/

            filtrado3CheckBox.Checked = false;
            filtrado3GradosTextField.Enabled = false;
            filtrado3MetrosTextField.Enabled = false;
            materialLabel1.Enabled = false;
            materialLabel2.Enabled = false;
            materialCheckBox4.Enabled = true;
        }
        private void materialCheckBox3_Click(object sender, EventArgs e)
        {
            filtrado1CheckBox.Checked = false;
            filtrado2CheckBox.Checked = false;
            if (filtrado3CheckBox.Checked == true)
            {
                //filtrado3CheckBox.Checked = false;
                filtrado3GradosTextField.Enabled = true;
                filtrado3MetrosTextField.Enabled = true;
                materialLabel1.Enabled = true;
                materialLabel2.Enabled = true;
                //materialCheckBox4.Checked = false;
                //materialCheckBox4.Enabled = false;
            }
            else
            {
                //filtrado3CheckBox.Checked = true;

                filtrado3GradosTextField.Enabled = false;
                filtrado3MetrosTextField.Enabled = false;
                materialLabel1.Enabled = false;
                materialLabel2.Enabled = false;
                //materialCheckBox4.Enabled = true;
            }


        }
        private void TxtPruebaNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                int counter = 1;
                string line;
                string miFileOut = string.Empty;
                int opcion = 1;
                double grados = 0;
                double metros = 0;
                double ratio = 0;
                int giro = 0;
                int sentido = 0;
                int gl = 0;
                OpenFileDialog miDialogo = new OpenFileDialog();
                miDialogo.Title = "AMILIA" + " | " + "Selecciona un Archivo de Proyecto";
                miDialogo.Filter = "Ditel Project Files (*.txt)|*.txt";
                miDialogo.Multiselect = false;
                if (miDialogo.ShowDialog() == DialogResult.OK)
                {
                    miFileOut = miDialogo.FileName;
                    System.IO.StreamReader file = new System.IO.StreamReader(@miFileOut);
                    dsApp a = new dsApp();
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] separadas;
                        separadas = line.Split(',');
                        a.Polilinea.Rows.Add(separadas[0], separadas[1], counter);
                        counter++;

                    }
                    a.WriteXml("prueba.amilia");
                    file.Close();
                    if (!string.IsNullOrEmpty(textBox4.Text))
                    {
                        grados = double.Parse(textBox4.Text);
                    }
                    else
                    {
                        grados = 2;
                    }
                    if (!string.IsNullOrEmpty(textBox3.Text))
                    {
                        metros = double.Parse(textBox3.Text);
                    }
                    else
                    {
                        metros = 5;
                    }
                    ratio = grados / metros;
                    double dis = 0;
                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        dis = double.Parse(textBox6.Text);
                    }
                    int p_dis = 0;
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, ref giro, ref sentido, ref gl, dis, ref p_dis, documento);
                    materialLabel4.Text = string.Concat(giro);
                    materialLabel5.Text = string.Concat(sentido);
                    materialLabel7.Text = string.Concat(gl);
                    materialLabel41.Text = string.Concat(p_dis);
                }
                else
                {

                }
            }
        }

        private void materialCheckBox5_Click(object sender, EventArgs e)
        {
            if (aplicarMultiplesFiltradosCheckBox.Checked == true)
            {
                filtrado1CheckBox.Enabled = false;
                filtrado2CheckBox.Enabled = false;
                filtrado3CheckBox.Enabled = false;
                filtrado3GradosTextField.Enabled = true;
                filtrado3MetrosTextField.Enabled = true;
                filtrado1ExecuteOrderNumericField.Enabled = true;
                filtrado2ExecuteOrderNumericField.Enabled = true;
                filtrado3ExecuteOrderNumericField.Enabled = true;
            }
            else
            {
                filtrado1CheckBox.Enabled = true;
                filtrado2CheckBox.Enabled = true;
                filtrado3CheckBox.Enabled = true;
                if (filtrado3CheckBox.Checked == true)
                {
                    filtrado3GradosTextField.Enabled = true;
                    filtrado3MetrosTextField.Enabled = true;
                }
                else
                {
                    filtrado3GradosTextField.Enabled = false;
                    filtrado3MetrosTextField.Enabled = false;
                }
                filtrado1ExecuteOrderNumericField.Enabled = false;
                filtrado2ExecuteOrderNumericField.Enabled = false;
                filtrado3ExecuteOrderNumericField.Enabled = false;
            }


        }
        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hay que elegir el orden en el que se desea hacer la iteracion. Si es 0 no se hará esa iteración", "Información");
        }
        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hay que elegir el orden en el que se desea hacer la iteracion. Si es 0 no se hará esa iteración", "Información");
        }

        private void materialFlatButton4_Click(object sender, EventArgs e)
        {
            if (comprobar())
            {
                int opcion = 1;
                double grados = 0;
                double metros = 0;
                double ratio = 0;
                dsApp a = new dsApp();
                if (filtrado1CheckBox.Checked == true)
                {
                    opcion = 1;
                    grados = 0;
                    metros = 0;
                    ratio = 0;
                }
                else if (filtrado2CheckBox.Checked == true)
                {
                    opcion = 2;
                    grados = 0;
                    metros = 0;
                    ratio = 0;
                }
                else if (filtrado3CheckBox.Checked == true)
                {
                    opcion = 3;
                    if (!string.IsNullOrEmpty(filtrado3GradosTextField.Text))
                    {
                        grados = double.Parse(filtrado3GradosTextField.Text);
                    }
                    else
                    {
                        grados = 2;
                    }
                    if (!string.IsNullOrEmpty(filtrado3MetrosTextField.Text))
                    {
                        metros = double.Parse(filtrado3MetrosTextField.Text);
                    }
                    else
                    {
                        metros = 5;
                    }
                    ratio = grados / metros;
                }
                int it = 0;
                if (materialCheckBox4.Checked == true)
                {
                    it = 1;
                }
                else
                {
                    it = 2;
                }
                if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                {
                    double t_med, t_max, p_cluster;
                    if (!string.IsNullOrEmpty(toleranciaMediaTextField.Text))
                    {
                        t_med = double.Parse(toleranciaMediaTextField.Text);
                    }
                    else
                    {
                        t_med = 0;
                    }
                    if (!string.IsNullOrEmpty(toleranciaMaximaTextField.Text))
                    {
                        t_max = double.Parse(toleranciaMaximaTextField.Text);
                    }
                    else
                    {
                        t_max = 1;
                    }
                    if (!string.IsNullOrEmpty(clusterizacionTextField.Text))
                    {
                        p_cluster = double.Parse(clusterizacionTextField.Text);
                    }
                    else
                    {
                        p_cluster = 10;
                    }
                    double curva_g;
                    if (!string.IsNullOrEmpty(curvaGranRadioTextField.Text))
                    {
                        curva_g = int.Parse(curvaGranRadioTextField.Text);
                    }
                    else
                    {
                        curva_g = 2500;
                    }
                    int n_curvas;
                    if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text))
                    {
                        n_curvas = int.Parse(nCurvasMaxTextField.Text);
                    }
                    else
                    {
                        n_curvas = 2;
                    }
                    int puntos_cluster;
                    if (!string.IsNullOrEmpty(pclusterizacionTextField.Text))
                    {
                        puntos_cluster = int.Parse(pclusterizacionTextField.Text);
                    }
                    else
                    {
                        puntos_cluster = 50;
                    }
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, it, 0, 0, documento);

                    if (a.Polilinea.Count > 0)
                    {
                        /*   calculo.Cambios_Sentido(t_med);
                           calculo.nueva_relacion();
                           calculo.Set_minimos();
                           calculo.Set_grupo();
                           calculo.Set_recta_curva();
                           calculo.Entidades_Curvas(t_max, p_cluster);
                           calculo.Recorrido();
                           calculo.Combinacion(t_med, t_max, n_curvas,puntos_cluster,curva_g);
                           calculo.Dibujar_entidades(1);*/
                    }

                    /*
                                        calculo.nueva_relacion();
                                        calculo.Set_minimos();
                                        calculo.Set_grupo();
                                        calculo.Set_recta_curva();
                                        calculo.ajuste();
                                        calculo.centro();*/
                }
                else
                {
                    int dis = (int)filtrado1ExecuteOrderNumericField.Value;
                    int rad = (int)filtrado2ExecuteOrderNumericField.Value;
                    int gir = (int)filtrado3ExecuteOrderNumericField.Value;
                    int[] orden = new int[3];
                    orden[0] = dis;
                    orden[1] = rad;
                    orden[2] = gir;
                    if (!string.IsNullOrEmpty(filtrado3GradosTextField.Text))
                    {
                        grados = double.Parse(filtrado3GradosTextField.Text);
                    }
                    else
                    {
                        grados = 2;
                    }
                    if (!string.IsNullOrEmpty(filtrado3MetrosTextField.Text))
                    {
                        metros = double.Parse(filtrado3MetrosTextField.Text);
                    }
                    else
                    {
                        metros = 5;
                    }

                    ratio = grados / metros;
                    double t_med, t_max, p_cluster;
                    if (!string.IsNullOrEmpty(toleranciaMediaTextField.Text))
                    {
                        t_med = double.Parse(toleranciaMediaTextField.Text);
                    }
                    else
                    {
                        t_med = 0;
                    }
                    if (!string.IsNullOrEmpty(toleranciaMaximaTextField.Text))
                    {
                        t_max = double.Parse(toleranciaMaximaTextField.Text);
                    }
                    else
                    {
                        t_max = 1;
                    }
                    if (!string.IsNullOrEmpty(clusterizacionTextField.Text))
                    {
                        p_cluster = double.Parse(clusterizacionTextField.Text);
                    }
                    else
                    {
                        p_cluster = 10;
                    }
                    double curva_g;
                    if (!string.IsNullOrEmpty(curvaGranRadioTextField.Text))
                    {
                        curva_g = int.Parse(curvaGranRadioTextField.Text);
                    }
                    else
                    {
                        curva_g = 2500;
                    }
                    int n_curvas;
                    if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text))
                    {
                        n_curvas = int.Parse(nCurvasMaxTextField.Text);
                    }
                    else
                    {
                        n_curvas = 2;
                    }
                    int puntos_cluster;
                    if (!string.IsNullOrEmpty(pclusterizacionTextField.Text))
                    {
                        puntos_cluster = int.Parse(pclusterizacionTextField.Text);
                    }
                    else
                    {
                        puntos_cluster = 50;
                    }
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, orden, it, 0, 0, documento);
                    if (a.Polilinea.Count > 0)
                    {
                        /*  calculo.Cambios_Sentido(t_med);
                          calculo.nueva_relacion();
                          calculo.Set_minimos();
                          calculo.Set_grupo();
                          calculo.Set_recta_curva();
                          calculo.Entidades_Curvas(t_max, p_cluster);
                          calculo.Recorrido();
                          calculo.Combinacion(t_med, t_max, n_curvas, puntos_cluster,curva_g);*/
                    }

                    /*
                    calculo.nueva_relacion();
                    calculo.Set_minimos();
                    calculo.Set_grupo();
                    calculo.Set_recta_curva();
                    calculo.ajuste();
                    calculo.centro();*/
                }

            }
            else
            {

            }
        }

        private void materialFlatButton5_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                int opcion = 1;
                double grados = 0;
                double metros = 0;
                double ratio = 0;
                int giro = 0;
                int sentido = 0;
                int gl = 0;
                dsApp a = new dsApp();
                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    grados = double.Parse(textBox4.Text);
                }
                else
                {
                    grados = 2;
                }
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    metros = double.Parse(textBox3.Text);
                }
                else
                {
                    metros = 5;
                }
                ratio = grados / metros;

                int p_dis = 0;
                Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, ref giro, ref sentido, ref gl, 0, ref p_dis, documento);
                materialLabel4.Text = string.Concat(giro);
                materialLabel5.Text = string.Concat(sentido);
                materialLabel7.Text = string.Concat(gl);
            }

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void materialFlatButton6_Click(object sender, EventArgs e)
        {

        }

        public void onNewViabilidadStatus(String etapa, ViabilidadComponentesStatus viabilidadComponentesStatus, List<Componente> componentes, int whileItIndex)
        {
            //si no se ha activado ejecutarViabilidadSinParar o si se ha activado detenerEnIteracion y la iteracion del while coincideC
            if (!this.ejecutarViabilidadSinParar || (this.detenerEnIteracion && whileItIndex == this.iteracion))
            {
                ViabilidadComponentesStatusInfoPanel viabilidadComponentesInfoPanel = new ViabilidadComponentesStatusInfoPanel(viabilidadComponentesStatus, componentes, "Depuración " + etapa + " > iteracion: " + whileItIndex, whileItIndex);
                viabilidadComponentesInfoPanel.addListener(this);
                //viabilidadComponentesInfoPanel.ShowDialog(this);
                viabilidadComponentesInfoPanel.ejecutarHastaFinalizarButtonClick();
            }
        }

        public void continuarHastaElFinal()
        {
            this.ejecutarViabilidadSinParar = true;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
        }

        public void continuarHastaLaIteracion(int iteracion)
        {
            this.detenerEnIteracion = true;
            this.iteracion = iteracion;
            this.ejecutarViabilidadSinParar = true;
        }

        public void continuarPasoAPaso()
        {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
        }
        public void Terminar()
        {
            this.terminar = true;

        }
        /*
         * Cargar polilinea del perfil
         */
        private void CargarPoliPerfil_Click(object sender, EventArgs e)
        {
            //Resetear estado actual
            Ocultar_Modificadores();
            materialLabel47.Text = "Soluciones: 0";
            materialLabel47.Update();
            progressBar1.Value = 0;
            progressBar1.Style = ProgressBarStyle.Marquee;
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            if (acDoc.CommandInProgress.Count() == 0)
            {
                materialFlatButton28.Visible = false;
                materialFlatButton24.Visible = false;
                this.calculoPolilineaPerfil = null;
                this.pasosEjecutados = -1;
                this.paso1EjecutadoTextView.Visible = false;
                this.paso2EjecutadoTextView.Visible = false;
                this.paso3EjecutadoTextView.Visible = false;
                this.calculoPolilineaStatusTextView.Visible = false;
                this.ejecutarViabilidadSinParar = false;
                this.detenerEnIteracion = false;
                this.iteracion = -1;
                int escala;
                int n_suavizados;
                if (!string.IsNullOrEmpty(FactorEscala.Text))
                {
                    escala = int.Parse(FactorEscala.Text);
                }
                else
                {
                    escala = 1;
                }
                if (!string.IsNullOrEmpty(suavizar.Text))
                {
                    n_suavizados = int.Parse(suavizar.Text);
                }
                else
                {
                    n_suavizados = 1;
                }
                if (comprobar())
                {
                    dsApp dsApp = this.abrirArchivoDeProyectoPerfil();

                    if (dsApp != null)
                    {
                        //obtener parametros de inicializacion de CalculoPolilineaController
                        this.calculoPolilineaPreferenciasPerfil = this.obtenerParametrosCalculoPolilineaPerfil();

                        if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                        {
                            this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.It, escala, n_suavizados, calculoPolilineaPreferenciasPerfil.Dis_eliminar, documento);
                        }
                        else
                        {
                            this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.Orden, calculoPolilineaPreferenciasPerfil.It, documento);
                        }

                        this.calculoPolilineaStatusTextView.Visible = true;
                        this.pasosEjecutados = 0;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 1;
                        MessageBox.Show("CalculoPolilinea Perfil inicializado");
                        groupBox2.Visible = true;
                        groupBox3.Visible = true;
                        b_CrearComponentes.Visible = true;

                    }
                    else
                    {
                        calculoPolilineaPerfil = null;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 0;
                        MessageBox.Show("Error de formato o al abrir el archivo del proyecto");
                    }
                }
                else
                {
                    calculoPolilineaPerfil = null;
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    MessageBox.Show("Error. El orden de los filtros esta repetido");
                }
            }
            else
            {
                calculoPolilineaPerfil = null;
                MessageBox.Show("Cancele el comando " + acDoc.CommandInProgress + " para continuar");
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Maximum = 1;
                progressBar1.Value = 0;
            }
        }
        private void ejecutar1ButtonClickPerfil(object sender, EventArgs eventArgs)
        {
            calculoPolilinea = new CalculoPolilinea();
            calculoPolilinea.Polilinea = calculoPolilineaPerfil.Polilinea;
            calculoPolilineaPreferencias = new CalculoPolilineaPreferencias();
            calculoPolilineaPreferencias = calculoPolilineaPreferenciasPerfil;
            if (this.calculoPolilinea != null)
            {
                /*
                * 
                * Primero dividimos todos los cambios de giro y comprobamos que la tolerancia media *3
                * a la recta es menor suavizamos 10 veces ese tramos 
                * 
                */

                calculoPolilinea.Cambios_Sentido(0.000001);
                PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                polilineaInfoPanel.Show();
                calculoPolilinea.nueva_relacion();
                calculoPolilinea.Set_minimos();
                calculoPolilinea.Set_grupo();
                calculoPolilinea.Set_recta_curva(25);
                int pol = calculoPolilinea.Dividir_Polilinea();
                PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                polilineaInfoPanel2.Show();
                for (int i = 0; i <= pol; i++)
                {
                    calculoPolilinea.Seleccionar_Polilinea(i);
                    calculoPolilinea.Entidades_Curvas(0.000001, 40);
                    calculoPolilinea.Recorrido(false, 40);
                    calculoPolilinea.Combinacion(0.000001, 0.000001, 0, 50, calculoPolilineaPreferencias.Gran_r, false);
                    calculoPolilinea.Limpiar(i);

                }
                calculoPolilinea.Unir_Componentes();
                //calculoPolilinea.Dibujar_entidades(1);
                calculoPolilinea.Comprobacion(false, 0.01, 0.01, 20, 2, 1, 0.3, true);
                //calculoPolilinea.Dibujar_entidades(2);

                VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();

                ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);
                componentesInfoPanel.Show();


                this.pasosEjecutados = 1;
                this.paso1EjecutadoTextView.Visible = true;
                MessageBox.Show("Revise autocad para ver la salida de la etapa 1 del algoritmo");
            }
            else
            {
                MessageBox.Show("Calculo polilinea no inicializado");
            }
        }
        private void ejecutar2Button_Click(object sender, EventArgs e)
        {

        }

        private void ejecutar3Button_Click(object sender, EventArgs e)
        {

        }

        private void materialCheckBox1_opciones_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox1_opciones.Checked == true)
            {
                materialLabel20.Visible = true;
                pclusterizacionTextField.Visible = true;
                materialLabel17.Visible = true;
                clusterizacionTextField.Visible = true;
                materialLabel21.Visible = true;
                SolapesTextField.Visible = true;
                materialLabel16.Visible = true;
                toleranciaMaximaTextField.Visible = true;
                materialLabel15.Visible = true;
                toleranciaMediaTextField.Visible = true;
                materialLabel19.Visible = true;
                nCurvasMaxTextField.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
                button7.Visible = true;
                button20.Visible = true;
                materialCheckBox1.Visible = true;
                materialCheckBox3.Visible = true;
                textBox1.Visible = true;
                materialLabel33.Visible = true;
                button8.Visible = true;
                button18.Visible = true;
                materialCheckBox5.Visible = true;
                textBox2.Visible = true;
                materialCheckBox6.Visible = true;
                textBox5.Visible = true;
                materialLabel34.Visible = true;
                materialLabel35.Visible = true;
                materialLabel38.Visible = true;
                materialLabel42.Visible = true;
                materialLabel43.Visible = true;
                textBox7.Visible = true;
                button19.Visible = true;
                textBox8.Visible = true;
                textBox9.Visible = true;
                materialLabel44.Visible = true;
                materialLabel45.Visible = true;
                materialLabel46.Visible = true;
                materialCheckBox7.Visible = true;
                if (materialCheckBox7.Checked)
                {
                    materialLabel44.Enabled = true;
                    materialLabel45.Enabled = true;
                    materialLabel46.Enabled = true;
                    textBox8.Enabled = true;
                    textBox9.Enabled = true;
                }
                else
                {
                    materialLabel44.Enabled = false;
                    materialLabel45.Enabled = false;
                    materialLabel46.Enabled = false;
                    textBox8.Enabled = false;
                    textBox9.Enabled = false;
                }

                textBox11.Visible = true;
                textBox12.Visible = true;
                materialLabel49.Visible = true;
                materialLabel50.Visible = true;
                materialLabel51.Visible = true;
                materialCheckBox14.Visible = true;
                button22.Visible = true;
                button23.Visible = true;
                button24.Visible = true;

            }
            if (materialCheckBox1_opciones.Checked == false)
            {
                materialLabel20.Visible = false;
                pclusterizacionTextField.Visible = false;
                materialLabel17.Visible = false;
                clusterizacionTextField.Visible = false;
                materialLabel21.Visible = false;
                SolapesTextField.Visible = false;
                materialLabel16.Visible = false;
                toleranciaMaximaTextField.Visible = false;
                materialLabel15.Visible = false;
                toleranciaMediaTextField.Visible = false;
                materialLabel19.Visible = false;
                nCurvasMaxTextField.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button20.Visible = false;
                materialCheckBox1.Visible = false;
                materialCheckBox3.Visible = false;
                textBox1.Visible = false;
                materialLabel33.Visible = false;
                button8.Visible = false;
                button18.Visible = false;
                materialCheckBox5.Visible = false;
                textBox2.Visible = false;
                materialCheckBox6.Visible = false;
                textBox5.Visible = false;
                materialLabel34.Visible = false;
                materialLabel35.Visible = false;
                materialLabel38.Visible = false;
                materialLabel42.Visible = false;
                materialLabel43.Visible = false;
                textBox7.Visible = false;
                button19.Visible = false;
                textBox8.Visible = false;
                textBox9.Visible = false;
                materialLabel44.Visible = false;
                materialLabel45.Visible = false;
                materialLabel46.Visible = false;
                materialCheckBox7.Visible = false;
                if (materialCheckBox7.Checked)
                {
                    materialLabel44.Enabled = true;
                    materialLabel45.Enabled = true;
                    materialLabel46.Enabled = true;
                    textBox8.Enabled = true;
                    textBox9.Enabled = true;
                }
                else
                {
                    materialLabel44.Enabled = false;
                    materialLabel45.Enabled = false;
                    materialLabel46.Enabled = false;
                    textBox8.Enabled = false;
                    textBox9.Enabled = false;
                }
                textBox11.Visible = false;
                textBox12.Visible = false;
                materialLabel49.Visible = false;
                materialLabel50.Visible = false;
                materialLabel51.Visible = false;
                materialCheckBox14.Enabled = false;
                button22.Visible = false;
                button23.Visible = false;
                button24.Visible = false;
            }
        }

        private void materialFlatButton7_Click(object sender, EventArgs e)
        {
            ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(List_componentes[0], List_verificacion[0]);
            componentesInfoPanel.Show();
            PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(List_polilinea[0]);
            polilineaInfoPanel.Show();
            PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(List_polilinea[1]);
            // polilineaInfoPanel2.Show();

        }

        private void materialFlatButton8_Click(object sender, EventArgs e)
        {
            ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(List_componentes[1], List_verificacion[1]);
            componentesInfoPanel.Show();
            TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(List_traza[0], List_componentes[1], "Trazas viabilidad etapa 2");
            tvi.Show();
        }

        private void materialFlatButton9_Click(object sender, EventArgs e)
        {
            ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(List_componentes[2], List_verificacion[2]);
            componentesInfoPanel.Show();
            TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(List_traza[1], List_componentes[2], "Trazas viabilidad etapa 3");
            tvi.Show();
        }
        private void aniadir_a_list(List<Componente> l)
        {
            List<Componente> comp = new List<Componente>();
            foreach (Componente c in l)
            {

                comp.Add(c);
            }
            List_componentes.Add(comp);
        }

        private void materialFlatButton11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Seleccione una polilínea en 3d en AUTOCAD");
            if (Comprobar_funcion_AUTOCAD())
            {
                dsApp a = new dsApp();
                Logica.GuardarPolilinea3d Gp3d = new GuardarPolilinea3d(ref a);
                Lista_original_3d = new List<Point3d>();
                foreach (DataRow r in a.Polilinea3d.Rows)
                {
                    string x = (string)r["X"];
                    string y = (string)r["Y"];
                    string z = (string)r["Z"];
                    Lista_original_3d.Add(new Point3d(double.Parse(x), double.Parse(y), double.Parse(z)));
                }
            }


        }
        /// <summary>
        /// Ejecución del primer paso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void materialFlatButton12_Click(object sender, EventArgs e)
        {

            if (Comprobar_funcion_AUTOCAD())
            {
                /*if (calculoPolilineaPerfil != null)
                {
                    if (calculoPolilineaPerfil.iniciado)
                    {
                        calculoPolilineaPerfil = null;
                    }
                }*/
                calculoPolilineaPerfil.VaciarCalculopolilineaPerfil();
                Ocultar_Modificadores();
                materialFlatButton24.Visible = false;
                materialFlatButton28.Visible = false;
                conta_apartado_perfil = 1;
                progressBar1.Maximum = 6;
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Value = 0;
                progressBar1.Value++;
                if (calculoPolilineaPerfil != null)
                {
                    calculoPolilineaPerfil.iniciado = true;
                    int escala;
                    if (!string.IsNullOrEmpty(FactorEscala.Text))
                    {
                        escala = int.Parse(FactorEscala.Text);
                    }
                    else
                    {
                        escala = 1;
                    }
                    double v_ac;
                    if (!string.IsNullOrEmpty(Varianza_acumulada.Text))
                    {
                        v_ac = double.Parse(Varianza_acumulada.Text);
                    }
                    else
                    {
                        v_ac = 1;
                    }
                    int n_suavizados;
                    if (!string.IsNullOrEmpty(suavizar.Text))
                    {
                        n_suavizados = int.Parse(suavizar.Text);
                    }
                    else
                    {
                        n_suavizados = 1;
                    }
                    double distancia;
                    if (!string.IsNullOrEmpty(text_distancia.Text))
                    {
                        distancia = double.Parse(text_distancia.Text);
                    }
                    else
                    {
                        distancia = 20;
                    }
                    double separacion;
                    if (!string.IsNullOrEmpty(text_separacion.Text))
                    {
                        separacion = double.Parse(text_separacion.Text);
                    }
                    else
                    {
                        separacion = 40;
                    }
                    double pendiente;
                    if (!string.IsNullOrEmpty(textpendiente.Text))
                    {
                        pendiente = double.Parse(textpendiente.Text);
                        if (pendiente == 0)
                        {
                            pendiente = 0.001;
                        }
                    }
                    else
                    {
                        pendiente = 1;
                    }
                    double rotu;
                    if (!string.IsNullOrEmpty(textBox_rot.Text))
                    {
                        rotu = double.Parse(textBox_rot.Text);
                    }
                    else
                    {
                        rotu = 100;
                    }

                    calculoPolilineaPerfil.RellenarPerfil(v_ac, n_suavizados);
                    calculoPolilineaPerfil.DividirSentidos();
                    calculoPolilineaPerfil.QuitarSuavizado();
                    progressBar1.Value++;
                    //calculoPolilineaPerfil.MatrizAcuerdo();
                    //calculoPolilineaPerfil.MatrizAcuerdo2();
                    calculoPolilineaPerfil.MatrizAcuerdo3();


                    /*
                     * para visualizar tablas de puntos y parabolas
                     */
                    /*PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(calculoPolilineaPerfil.Polilinea_Perfil);
                    polilineaInfoPanel.Show();*/
                    /*PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilineaPerfil.Lista_Parabolas);
                    polilineaInfoPanel2.Show();*/

                    calculoPolilineaPerfil.Quitar_Acuerdos(distancia, separacion, pendiente);
                    try
                    {
                        calculoPolilineaPerfil.PuntoInflexion();
                        //calculoPolilineaPerfil.Fusion_Acuerdos();

                        progressBar1.Value++;
                        //               calculoPolilineaPerfil.Dibujar_Acuerdos(1);
                        calculoPolilineaPerfil.CalcularEntreParabolas();
                        calculoPolilineaPerfil.CalculoEntreParabolas_Dibujar();

                        calculoPolilineaPerfil.Componente_Inicial();
                        calculoPolilineaPerfil.Componente_Final();

                        calculoPolilineaPerfil.Acuerdo_Entre_Pendientes();
                        progressBar1.Value++;
                        //calculoPolilineaPerfil.Dibujar_Rectas(3);
                        //calculoPolilineaPerfil.Dibujar_Acuerdos(3);
                        calculoPolilineaPerfil.ExportarLandXML(calculoPolilinea.Mcomponenetes, true);

                        calculoPolilineaPerfil.CrearTrazado();
                        progressBar1.Value++;
                        calculoPolilineaPerfil.Rotular(rotu);

                        calculoPolilineaPerfil.Informe();
                        GuardarPerfilAutomatico(calculoPolilineaPerfil);

                        progressBar1.Value++;
                        //calculoPolilineaPerfil = null;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 1;
                        materialFlatButton34.Visible = false;
                        materialFlatButton24.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("No se crea ningun acuerdo. Cambie los parametros y comience de nuevo.");
                        calculoPolilineaPerfil = null;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 0;
                    }
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    MessageBox.Show("No ha cargado la polilínea 3d en el programa.");

                }
            }

        }

        private void materialLabel23_Click(object sender, EventArgs e)
        {

        }

        private void Guardarpolilinea2d(object sender, EventArgs e)
        {
            MessageBox.Show("Seleccione una polilínea en 2d en AUTOCAD");
            if (Comprobar_funcion_AUTOCAD())
            {
                dsApp a = new dsApp();
                Logica.GuardarPolilinea2d Gp2d = new GuardarPolilinea2d(ref a);
                Lista_original_2d = new List<Point2d>();
                foreach (DataRow r in a.Polilinea.Rows)
                {
                    string x = (string)r["X"];
                    string y = (string)r["Y"];
                    Lista_original_2d.Add(new Point2d(double.Parse(x), double.Parse(y)));
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Factor de escala de la rotulacion.\nEl tamaño por defecto de la letra es de 4 metros al 100%, cambiando este valor se adecuará al porcentaje establecido.", "Información");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Puntos mínimos con los que se puede crear un cluster.", "Información");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Se seleccionan los radios cuya diferencia no supere el porcentaje aquí dado para clusterizar.", "Información");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Veces que una entidad puede ser modificada antes de ser eliminada.", "Información");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Curvas maximas por tramo de giro.", "Información");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            info i = new info();
            i.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            info_m2 i = new info_m2();
            i.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            info_m3 i = new info_m3();
            i.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tolerancia máxima que utilizaremos para detectar las entidades.", "Información");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tolerancia media que utilizaremos para detectar las entidades.", "Información");
        }

        private void materialFlatButton15_Click(object sender, EventArgs e)
        {


        }

        private void materialFlatButton16_Click(object sender, EventArgs e)
        {
            materialLabel47.Text = "Soluciones: 0";
            Ocultar_Modificadores();
            materialLabel47.Update();
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            if (acDoc.CommandInProgress.Count() == 0)
            {
                materialFlatButton28.Visible = false;
                materialFlatButton24.Visible = false;
                progressBar1.Maximum = 3;
                progressBar1.Style = ProgressBarStyle.Marquee;
                this.calculoPolilineaPerfil = null;
                this.pasosEjecutados = -1;
                this.paso1EjecutadoTextView.Visible = false;
                this.paso2EjecutadoTextView.Visible = false;
                this.paso3EjecutadoTextView.Visible = false;
                this.calculoPolilineaStatusTextView.Visible = false;
                this.ejecutarViabilidadSinParar = false;
                this.detenerEnIteracion = false;
                this.iteracion = -1;
                int escala;
                int n_suavizados;
                calculoPolilineaPerfil = new CalculoPolilineaPerfil();
                if (MessageBox.Show("¿Desea cargar un trazado desde un archivo?", "Cargar Trazado", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Archivos de texto (*.amilia)|*.amilia|Todos los archivos (*.*)|*.*";
                    openFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string json = File.ReadAllText(openFileDialog.FileName);
                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        };
                        settings.Converters.Add(new AutoCADPointConverter());
                        // El archivo se guarda siempre como List<SalidaAmilia>
                        // pero por compatibilidad también intentamos leer el formato antiguo (objeto único).
                        listaSalidas = null;
                        try
                        {
                            listaSalidas = JsonConvert.DeserializeObject<List<SalidaAmilia>>(json, settings);
                        }
                        catch
                        {
                            // Formato antiguo: objeto único
                            try
                            {
                                var unico = JsonConvert.DeserializeObject<SalidaAmilia>(json, settings);
                                listaSalidas = new List<SalidaAmilia> { unico };
                            }
                            catch { }
                        }

                        if (listaSalidas == null || listaSalidas.Count == 0)
                        {
                            MessageBox.Show("El archivo no contiene ningún trazado o su formato no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        SalidaAmilia salida;
                        if (listaSalidas.Count == 1)
                        {
                            salida = listaSalidas[0];
                        }
                        else
                        {
                            // Mostrar un diálogo con ListBox para que el usuario seleccione el eje
                            string[] nombres = listaSalidas.Where(s => s.Tipo==TipoSalida.Longitudinal).Select(s => s.Nombre ?? "(sin nombre)").ToArray();

                            Form selForm = new Form();
                            selForm.Text = "Seleccionar eje";
                            selForm.Width = 360;
                            selForm.Height = 240;
                            selForm.StartPosition = FormStartPosition.CenterScreen;
                            selForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                            selForm.MinimizeBox = false;
                            selForm.MaximizeBox = false;

                            Label lbl = new Label { Text = "El archivo contiene varios ejes.\nSelecciona el que quieres cargar:", Left = 10, Top = 10, Width = 320, Height = 36 };
                            ListBox lb = new ListBox { Left = 10, Top = 52, Width = 320, Height = 110, SelectionMode = System.Windows.Forms.SelectionMode.One };
                            lb.Items.AddRange(nombres);
                            lb.SelectedIndex = 0;
                            Button btnOk = new Button { Text = "Aceptar", DialogResult = DialogResult.OK, Left = 165, Top = 170, Width = 80 };
                            Button btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Left = 255, Top = 170, Width = 75 };
                            selForm.Controls.AddRange(new Control[] { lbl, lb, btnOk, btnCancel });
                            selForm.AcceptButton = btnOk;
                            selForm.CancelButton = btnCancel;

                            if (selForm.ShowDialog() != DialogResult.OK || lb.SelectedIndex < 0)
                            {
                                return;
                            }
                            salida = listaSalidas[lb.SelectedIndex];
                        }

                        if (this.calculoPolilinea == null)
                        {
                            this.calculoPolilinea = new CalculoPolilinea();
                        }
                        this.calculoPolilinea.Mcomponenetes = salida.MComponentes;
                        this.calculoPolilinea.Trazado_prueba = salida.Trazado;
                        Lista_original_3d = salida.Lista_original;
                        // Guardar ruta para compartirla con TrazadoUsuarioPerfil
                        _rutaArchivoAmilia = openFileDialog.FileName;
                    }
                }
                else
                {
                    MessageBox.Show("Cargando trazado de memoria");
                }
                if (this.calculoPolilinea != null)
                {
                    if (this.calculoPolilinea.Mcomponenetes != null)
                    {
                        // -- Elegir fuente de cotas para el perfil --------------------------------
                        var opcionPerfil = MessageBox.Show(
                            "¿Calcular el perfil con el MDT del terreno?\n\n" +
                            "  Sí  → usar MDT (nube de puntos)\n" +
                            "  No  → usar el eje original en 3D (Lista_original)",
                            "Fuente del perfil",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (opcionPerfil == DialogResult.Yes)
                        {
                            // Si el terrainService no está cargado aún, cargar el MDT
                            if (terrainService == null)
                            {
                                using (OpenFileDialog ofdMdt = new OpenFileDialog())
                                {
                                    ofdMdt.Filter = "Archivos MDT (*.amilia_mdt)|*.amilia_mdt|Todos los archivos (*.*)|*.*";
                                    ofdMdt.Title  = "Seleccione el archivo MDT (.amilia_mdt)";
                                    if (ofdMdt.ShowDialog() == DialogResult.OK)
                                    {
                                        try
                                        {
                                            string jsonMdt = System.IO.File.ReadAllText(ofdMdt.FileName);
                                            try
                                            {
                                                nube   = JsonConvert.DeserializeObject<nubepuntos>(jsonMdt);
                                                puntos = nube.puntos;
                                            }
                                            catch
                                            {
                                                puntos = JsonConvert.DeserializeObject<List<Punto3d>>(jsonMdt);
                                                nube   = new nubepuntos { miMax = 1000, miPendienteMax = 200, ucNodosPorHoja = 150, puntos = puntos };
                                            }
                                            KDTree2D       = new KDTree2D(puntos);
                                            terrainService = new TerrainService(puntos, null);
                                        }
                                        catch (Exception exMdt)
                                        {
                                            MessageBox.Show("Error al cargar el MDT: " + exMdt.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            terrainService = null;
                                        }
                                    }
                                }
                            }

                            // Construir Lista_original_3d interpolando Z desde el TIN para cada punto 2D del trazado.
                            // Los tramos de recta se densifican añadiendo puntos intermedios cada paso_max metros
                            // para conseguir la misma precisión de Z que tienen los tramos de curva.
                            if (terrainService != null && this.calculoPolilinea.Trazado_prueba != null)
                            {
                                const double paso_max = 5.0; // metros entre muestras de recta
                                var trazado = this.calculoPolilinea.Trazado_prueba;
                                trazadoDensificado = new List<Autodesk.AutoCAD.Geometry.Point2d>();
                                Lista_original_3d    = new List<Autodesk.AutoCAD.Geometry.Point3d>();

                                for (int _idx = 0; _idx < trazado.Count; _idx++)
                                {
                                    var pt2d = trazado[_idx];

                                    // --- punto original ---
                                    trazadoDensificado.Add(pt2d);
                                    double z = terrainService.GetPreciseZ(pt2d.X, pt2d.Y);
                                    if (double.IsNaN(z))
                                        z = Interpolator.EstimateZ(KDTree2D, pt2d.X, pt2d.Y);
                                    Lista_original_3d.Add(new Autodesk.AutoCAD.Geometry.Point3d(pt2d.X, pt2d.Y, double.IsNaN(z) ? 0 : z));

                                    // --- puntos intermedios si el segmento es largo ---
                                    if (_idx < trazado.Count - 1)
                                    {
                                        var ptSig = trazado[_idx + 1];
                                        double dist = Math.Sqrt(Math.Pow(ptSig.X - pt2d.X, 2) + Math.Pow(ptSig.Y - pt2d.Y, 2));
                                        if (dist > paso_max)
                                        {
                                            int nInt = (int)(dist / paso_max); // nº de puntos intermedios
                                            for (int k = 1; k <= nInt; k++)
                                            {
                                                double t  = (double)k / (nInt + 1);
                                                double xi = pt2d.X + t * (ptSig.X - pt2d.X);
                                                double yi = pt2d.Y + t * (ptSig.Y - pt2d.Y);
                                                trazadoDensificado.Add(new Autodesk.AutoCAD.Geometry.Point2d(xi, yi));
                                                double zi = terrainService.GetPreciseZ(xi, yi);
                                                if (double.IsNaN(zi))
                                                    zi = Interpolator.EstimateZ(KDTree2D, xi, yi);
                                                Lista_original_3d.Add(new Autodesk.AutoCAD.Geometry.Point3d(xi, yi, double.IsNaN(zi) ? 0 : zi));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // -----------------------------------------------------------------

                        // Usar lista densificada (si se usó MDT) o la original (si no)
                        var trazado2dPerfil = (trazadoDensificado != null && trazadoDensificado.Count > 0)
                            ? trazadoDensificado
                            : this.calculoPolilinea.Trazado_prueba;

                        if (trazado2dPerfil == null || trazado2dPerfil.Count == 0)
                        {
                            MessageBox.Show("Error: No se ha podido obtener el trazado para generar el perfil.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            progressBar1.Value = 0;
                            return;
                        }

                        calculoPolilineaPerfil.Cotas_Trazado(trazado2dPerfil, Lista_original_3d);

                        if (!string.IsNullOrEmpty(FactorEscala.Text))
                        {
                            escala = int.Parse(FactorEscala.Text);
                        }
                        else
                        {
                            escala = 1;
                        }
                        if (!string.IsNullOrEmpty(suavizar.Text))
                        {
                            n_suavizados = int.Parse(suavizar.Text);
                        }
                        else
                        {
                            n_suavizados = 1;
                        }

                        if (comprobar())
                        {
                            if (calculoPolilineaPerfil.Polilinea3d_Original.Count > 1)
                            {
                                dsApp dsApp = this.CargarArchivoDeProyectoPerfil(calculoPolilineaPerfil);

                                if (dsApp != null)
                                {
                                    //obtener parametros de inicializacion de CalculoPolilineaController
                                    this.calculoPolilineaPreferenciasPerfil = this.obtenerParametrosCalculoPolilineaPerfil();

                                    if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                                    {
                                        this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.It, escala, n_suavizados, calculoPolilineaPreferenciasPerfil.Dis_eliminar, documento);
                                    }
                                    else
                                    {
                                        this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.Orden, calculoPolilineaPreferenciasPerfil.It, documento);
                                    }

                                    this.calculoPolilineaStatusTextView.Visible = true;
                                    this.pasosEjecutados = 0;
                                    progressBar1.Style = ProgressBarStyle.Blocks;
                                    progressBar1.Maximum = 1;
                                    progressBar1.Value = 1;
                                    MessageBox.Show("CalculoPolilinea Perfil inicializado");
                                    groupBox2.Visible = true;
                                    groupBox3.Visible = true;
                                    b_CrearComponentes.Visible = true;
                                }
                                else
                                {
                                    progressBar1.Style = ProgressBarStyle.Blocks;
                                    progressBar1.Maximum = 1;
                                    progressBar1.Value = 0;
                                    calculoPolilineaPerfil = null;
                                    MessageBox.Show("Error al abrir el archivo del proyecto");
                                }
                            }
                        }
                        else
                        {
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            progressBar1.Maximum = 1;
                            progressBar1.Value = 0;
                            calculoPolilineaPerfil = null;
                            MessageBox.Show("Error. El orden de los filtros esta repetido");
                        }
                    }
                    else
                    {
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 0;
                        calculoPolilineaPerfil = null;
                        MessageBox.Show("Error. No hay trazado disponible");
                    }

                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    calculoPolilineaPerfil = null;
                    MessageBox.Show("Error. No hay trazado disponible");
                }
            }
            else
            {
                calculoPolilineaPerfil = null;
                MessageBox.Show("Cancele el comando " + acDoc.CommandInProgress + " para continuar");
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Maximum = 1;
                progressBar1.Value = 0;
            }

        }

        private void materialCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox2.Checked)
            {
                materialLabel26.Visible = true;
                suavizar.Visible = true;
                materialLabel30.Visible = true;
                textpendiente.Visible = true;
                materialLabel25.Visible = true;
                Varianza_acumulada.Visible = true;
                materialLabel27.Visible = true;
                text_distancia.Visible = true;
                materialLabel28.Visible = true;
                text_separacion.Visible = true;
                panel1.Visible = true;
                materialLabel23.Visible = true;
                materialLabel31.Visible = true;
                materialLabel36.Visible = true;
                materialLabel37.Visible = true;
                textBox_rot.Visible = true;
                button12.Visible = true;
                button13.Visible = true;
                button14.Visible = true;
                button15.Visible = true;
                button16.Visible = true;
                button17.Visible = true;


            }
            else
            {
                materialLabel26.Visible = false;
                suavizar.Visible = false;
                materialLabel30.Visible = false;
                textpendiente.Visible = false;
                materialLabel25.Visible = false;
                Varianza_acumulada.Visible = false;
                materialLabel27.Visible = false;
                text_distancia.Visible = false;
                materialLabel28.Visible = false;
                text_separacion.Visible = false;
                panel1.Visible = false;
                materialLabel23.Visible = false;
                materialLabel31.Visible = false;
                materialLabel36.Visible = false;
                materialLabel37.Visible = false;
                textBox_rot.Visible = false;
                button12.Visible = false;
                button13.Visible = false;
                button14.Visible = false;
                button15.Visible = false;
                button16.Visible = false;
                button17.Visible = false;

            }
        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox1.Checked == true)
            {
                materialCheckBox3.Checked = false;
                pclusterizacionTextField.Text = "3";
                clusterizacionTextField.Text = "2";
                toleranciaMaximaTextField.Text = "0.01";
                toleranciaMediaTextField.Text = "0.01";
                nCurvasMaxTextField.Text = "6";
            }
            else
            {
                materialCheckBox3.Checked = true;
                pclusterizacionTextField.Text = "5";
                clusterizacionTextField.Text = "15";
                toleranciaMaximaTextField.Text = "0.01";
                toleranciaMediaTextField.Text = "0.01";
                nCurvasMaxTextField.Text = "2";
            }
        }

        private void materialCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox3.Checked == true)
            {
                materialCheckBox1.Checked = false;
                pclusterizacionTextField.Text = "5";
                clusterizacionTextField.Text = "15";
                toleranciaMaximaTextField.Text = "0.01";
                toleranciaMediaTextField.Text = "0.01";
                nCurvasMaxTextField.Text = "2";
            }
            else
            {
                materialCheckBox1.Checked = true;
                pclusterizacionTextField.Text = "3";
                clusterizacionTextField.Text = "2";
                toleranciaMaximaTextField.Text = "0.01";
                toleranciaMediaTextField.Text = "0.01";
                nCurvasMaxTextField.Text = "6";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tipos de trazado:\n-Trazados con prámetros variables:\n    +Para carreteras urbanas y montaña.\n-Trazados con prámetros uniformes:\n   +Para ferrocarriles y autovías.");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Desprecia si es necesario una deformación en el trazado que ocurra en longitudes inferiores a la introducida.");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Elimina si hay algun bache en el trazado.");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Diferencia de pendiente necesaria para obtener diferentes acuerdos.");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Suma de las varianzas respecto de la pendiente.");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nº de suavizados que le hacemos a la polilinea para minimizar errores.");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nº de suavizados que le hacemos a la polilinea para minimizar errores.");
        }

        private void materialCheckBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void materialCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox6.Checked)
            {
                clusterizacionTextField.Text = "5";
                nCurvasMaxTextField.Text = "0";
            }
            else
            {

            }
        }
        private void Desabilitar_opciones()
        {
            materialLabel20.Enabled = false;
            pclusterizacionTextField.Enabled = false;
            materialLabel17.Enabled = false;
            clusterizacionTextField.Enabled = false;
            materialLabel21.Enabled = false;
            SolapesTextField.Enabled = false;
            materialLabel16.Enabled = false;
            toleranciaMaximaTextField.Enabled = false;
            materialLabel15.Enabled = false;
            toleranciaMediaTextField.Enabled = false;
            materialLabel19.Enabled = false;
            nCurvasMaxTextField.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button20.Enabled = false;
            materialCheckBox1.Enabled = false;
            materialCheckBox3.Enabled = false;
            textBox1.Enabled = false;
            materialLabel33.Enabled = false;
            button8.Enabled = false;
            button18.Enabled = false;
            materialCheckBox5.Enabled = false;
            textBox2.Enabled = false;
            materialCheckBox6.Enabled = false;
            textBox5.Enabled = false;
            materialLabel34.Enabled = false;
            materialLabel35.Enabled = false;
            materialLabel38.Enabled = false;
            RotulacionTextField.Enabled = false;
            materialLabel22.Enabled = false;
            materialLabel29.Enabled = false;
            materialLabel18.Enabled = false;
            curvaGranRadioTextField.Enabled = false;
            button1.Enabled = false;
            materialCheckBox1_opciones.Enabled = false;
            RotularCheckBox.Enabled = false;
            materialLabel42.Enabled = false;
            materialLabel43.Enabled = false;
            button19.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            materialLabel44.Enabled = false;
            materialLabel45.Enabled = false;
            materialLabel46.Enabled = false;
            materialCheckBox7.Enabled = false;
        }

        private void materialFlatButton17_Click(object sender, EventArgs e)
        {
            componentes_automaticas = null;
            this.calculoPolilinea = null;
            this.pasosEjecutados = -1;
            this.paso1EjecutadoTextView.Visible = false;
            this.paso2EjecutadoTextView.Visible = false;
            this.paso3EjecutadoTextView.Visible = false;
            this.calculoPolilineaStatusTextView.Visible = false;
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
            materialFlatButton7.Visible = false;
            materialFlatButton8.Visible = false;
            materialFlatButton9.Visible = false;
            List_verificacion.Clear();
            List_componentes.Clear();
            List_polilinea.Clear();
            List_traza.Clear();
            materialLabel20.Enabled = true;
            pclusterizacionTextField.Enabled = true;
            materialLabel17.Enabled = true;
            clusterizacionTextField.Enabled = true;
            materialLabel21.Enabled = true;
            SolapesTextField.Enabled = true;
            materialLabel16.Enabled = true;
            toleranciaMaximaTextField.Enabled = true;
            materialLabel15.Enabled = true;
            toleranciaMediaTextField.Enabled = true;
            materialLabel19.Enabled = true;
            nCurvasMaxTextField.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button20.Enabled = true;
            materialCheckBox1.Enabled = true;
            materialCheckBox3.Enabled = true;
            textBox1.Enabled = true;
            materialLabel33.Enabled = true;
            button8.Enabled = true;
            button18.Enabled = true;
            materialCheckBox5.Enabled = true;
            textBox2.Enabled = true;
            materialCheckBox6.Enabled = true;
            textBox5.Enabled = true;
            materialLabel34.Enabled = true;
            materialLabel35.Enabled = true;
            materialLabel38.Enabled = true;
            RotulacionTextField.Enabled = true;
            materialLabel22.Enabled = true;
            materialLabel29.Enabled = true;
            materialLabel18.Enabled = true;
            curvaGranRadioTextField.Enabled = true;
            button1.Enabled = true;
            materialCheckBox1_opciones.Enabled = true;
            RotularCheckBox.Enabled = true;
            materialLabel42.Enabled = true;
            materialLabel43.Enabled = true;
            button19.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox9.Enabled = true;
            materialLabel44.Enabled = true;
            materialLabel45.Enabled = true;
            materialLabel46.Enabled = true;
            materialCheckBox7.Enabled = true;
            if (materialCheckBox7.Checked)
            {
                materialLabel44.Enabled = true;
                materialLabel45.Enabled = true;
                materialLabel46.Enabled = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
            }
            else
            {
                materialLabel44.Enabled = false;
                materialLabel45.Enabled = false;
                materialLabel46.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
            }
            materialLabel48.Text = "";
            materialCheckBox13.Checked = false;
            textBox11.Enabled = false;
            textBox12.Enabled = false;
            Ocultar_Capas(documento);
        }
        private void materialFlatButton17_Click()
        {
            this.calculoPolilinea = null;
            this.pasosEjecutados = 0;
            this.paso1EjecutadoTextView.Visible = false;
            this.paso2EjecutadoTextView.Visible = false;
            this.paso3EjecutadoTextView.Visible = false;
            this.calculoPolilineaStatusTextView.Visible = false;
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
            materialFlatButton7.Visible = false;
            materialFlatButton8.Visible = false;
            materialFlatButton9.Visible = false;
            List_verificacion.Clear();
            List_componentes.Clear();
            List_polilinea.Clear();
            List_traza.Clear();
            List_verificacion = null;
            List_componentes = null;
            List_polilinea = null;
            List_traza = null;
            List_verificacion = new List<VerificacionComponentesStatus>();
            List_componentes = new List<List<Componente>>();
            List_polilinea = new List<List<Punto>>();
            List_traza = new List<List<ViabilidadComponentesStatus>>();
            materialLabel48.Text = "";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Se hará un ajuste de recta a todas las rectas consecutivas con una diferencia azimutal menor al valor dado.", "Información");

        }

        private void materialCheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox7.Checked)
            {
                materialLabel44.Enabled = true;
                materialLabel45.Enabled = true;
                materialLabel46.Enabled = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
            }
            else
            {
                materialLabel44.Enabled = false;
                materialLabel45.Enabled = false;
                materialLabel46.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
            }
        }
        private void Nuevos_Datos()
        {
            rect_ing = rectas_ingenieril[contador_tipo1];
            if (tipoA == 1)
            {
                if (tipoC == 1)
                {
                    tipo = 1;
                    Set_A1_C1();
                    materialLabel48.Text = "Tipo:A1-C1";
                    materialLabel48.Update();
                }
                else if (tipoC == 2)
                {
                    tipo = 2;
                    Set_A1_C2();
                    materialLabel48.Text = "Tipo:A1-C2";
                    materialLabel48.Update();
                }
                else if (tipoC == 3)
                {
                    tipo = 3;
                    Set_A1_C3();
                    materialLabel48.Text = "Tipo:A1-C3";
                    materialLabel48.Update();
                }
            }
            else if (tipoA == 2)
            {
                if (tipoC == 1)
                {
                    tipo = 4;
                    Set_A2_C1();
                    materialLabel48.Text = "Tipo:A2-C1";
                    materialLabel48.Update();
                }
                else if (tipoC == 2)
                {
                    tipo = 5;
                    Set_A2_C2();
                    materialLabel48.Text = "Tipo:A2-C2";
                    materialLabel48.Update();
                }
                else if (tipoC == 3)
                {
                    tipo = 6;
                    Set_A2_C3();
                    materialLabel48.Text = "Tipo:A2-C3";
                    materialLabel48.Update();
                }
            }
            N_C = List_N_C[contador_tipo2];


            curv_ing = curvas_ingenieril[contador_tipo2];
            if (contador_tipo1 > 4)
            {
                contador_tipo1 = 0;
                contador_tipo2++;
                if (tipo == 1 && contador_tipo2 == 5 && calculoPolilineaPreferencias.Gr_Rectas == 2)
                {
                    calculoPolilineaPreferencias.Gr_Rectas = 0.2;
                    calculoPolilineaPreferencias.Dividir = true;
                    calculoPolilineaPreferencias.Rectas = 20;
                    contador_tipo2 = 0;
                }
            }
        }
        private void Set_A1_C1()
        {
            //10
            //N_P = 10;
            P_C = A1_C1[contador_tipo1];
            contador_tipo1++;

            /*if (contador_tipo1 > 4)
            {
                contador_tipo1 = 0;
                grados_union = 1;
            }*/
        }
        private void Set_A1_C2()
        {
            //5
            /*N_P = calculoPolilinea.Contar_Puntos_Cluster();
            if (N_P > 10)
            {
                N_P = 10;
            }
            if (N_P==0)
            {
                N_P = 5;
            }*/
            P_C = A1_C2[contador_tipo1];
            contador_tipo1++;
        }
        private void Set_A1_C3()
        {
            //N_P = 3;
            P_C = A1_C3[contador_tipo1];
            contador_tipo1++;
        }
        private void Set_A2_C1()
        {
            //5
            /*N_P = calculoPolilinea.Contar_Puntos_Cluster();
            if (N_P > 10)
            {
                N_P = 10;
            }
            if (N_P == 0)
            {
                N_P = 5;
            }*/
            P_C = A2_C1[contador_tipo1];
            contador_tipo1++;
        }
        private void Set_A2_C2()
        {
            //tenia 3
            /* N_P = calculoPolilinea.Contar_Puntos_Cluster();
             if (N_P > 10)
             {
                 N_P = 10;
             }
             if (N_P == 0)
             {
                 N_P = 3;
             }*/
            P_C = A2_C2[contador_tipo1];
            contador_tipo1++;
        }
        private void Set_A2_C3()
        {
            //N_P = 3;
            P_C = A2_C3[contador_tipo1];
            contador_tipo1++;
        }

        private void Duplicar_Click(object sender, EventArgs e)
        {
            dsApp dsApp = this.abrirArchivoDeProyecto();
            /*CalculoPolilinea cp = new CalculoPolilinea(ref dsApp);
            cp.Dibujar(0);
            cp.Polilinea = cp.Duplicar_puntos_C(cp.Polilinea);
            cp.Dibujar(3);*/
        }
        private void materialFlatButton18_Click(object sender, EventArgs e)
        {

            if (Comprobar_funcion_AUTOCAD())
            {
                componentes_automaticas = null;
                materialLabel48.Text = "";
                //Resetear estado actual
                materialLabel47.Text = "";
                salir_bucle = false;
                Desabilitar_opciones();
                this.calculoPolilinea = null;
                this.pasosEjecutados = -1;
                this.paso1EjecutadoTextView.Visible = false;
                this.paso2EjecutadoTextView.Visible = false;
                this.paso3EjecutadoTextView.Visible = false;
                this.calculoPolilineaStatusTextView.Visible = false;
                this.ejecutarViabilidadSinParar = false;
                this.detenerEnIteracion = false;
                this.iteracion = -1;
                materialFlatButton7.Visible = false;
                materialFlatButton8.Visible = false;
                materialFlatButton9.Visible = false;
                List_verificacion.Clear();
                List_componentes.Clear();
                List_polilinea.Clear();
                List_traza.Clear();
                contador_tipo1 = 0;
                contador_tipo2 = 0;
                int resultados;
                progressBar1.Maximum = 26;
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Value = 1;
                L_Resultados = new List<Resultados>();
                List<int> lista_resultados_numero = new List<int>();
                conta_apartado = 1;
                if (comprobar())
                {
                    dsApp dsApp = this.abrirArchivoDeProyecto();

                    if (dsApp != null)
                    {
                        //obtener parametros de inicializacion de CalculoPolilineaController
                        this.calculoPolilineaPreferencias = this.obtenerParametrosCalculoPolilinea();
                        this.calculoPolilineaPreferencias.Duplicado = false;
                        if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, documento);
                        }
                        else
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, documento);
                        }
                        Lista_original_2d.Clear();
                        bool continuar = true;
                        if (Poli_Larga(this.calculoPolilinea.Polilinea))
                        {
                            DialogResult result = MessageBox.Show("La polilinea es demasiado extensa y con muchos puntos, esto provocará que el resultado tarde mucho en calcularse.\n\nSe aconseja reducir el número de puntos de la polilínea utilizando 'FILTRAR Y GUARDAR' de la  pestaña 'Filtrar puntos' o reducir la distancia que se desea calcular.\n\nSi desea volver a filtrar la polilínea o reducir la distancia pulse en aceptar, de lo contrario pulse en cancelar y el proceso seguirá. ", "Información", MessageBoxButtons.OKCancel);

                            if (result == DialogResult.OK)
                            {
                                continuar = false;
                            }
                        }

                        if (continuar)
                        {
                            foreach (Punto p in this.calculoPolilinea.Polilinea)
                            {
                                Lista_original_2d.Add(new Point2d(p.p.X, p.p.Y));
                            }
                            this.calculoPolilineaStatusTextView.Visible = true;
                            this.pasosEjecutados = 0;

                            if (materialCheckBox12.Checked)
                            {
                                calculoPolilinea.EliClo = true;
                            }
                            else
                            {
                                calculoPolilinea.EliClo = false;
                            }
                            if (materialCheckBox15.Checked)
                            {
                                calculoPolilinea.EliRec = true;
                            }
                            else
                            {
                                calculoPolilinea.EliRec = false;
                            }

                            calculoPolilinea.Eli_Clo = double.Parse(textBox10.Text);
                            calculoPolilinea.Eli_Rec = double.Parse(textBox13.Text);
                            //MessageBox.Show("CalculoPolilinea inicializado");
                            int[] r = calculoPolilinea.Propiedades(dsApp.Polilinea.Count() - 1, dsApp);
                            tipoA = r[0];
                            tipoC = r[1];
                            Nuevos_Datos();
                            N_P = Get_N_P(tipoA, tipoC);
                            Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int, List<Componente>>>();
                            contador_resultados_numero = 0;
                            distancia_menor = 100;
                            /*(new System.Threading.Thread(() =>
                            {
                                Detener();
                            })).Start();*/
                            bool tiempo_activo = false;
                            repetir_pregunta = false;
                            System.Threading.Thread th = new System.Threading.Thread(Detener);
                            th.Start();
                            progressBar1.Value++;
                            DateTime tiempo1 = DateTime.Now;
                            th2 = new System.Threading.Thread(App);
                            th2.Start();
                            int n_contador_tipo2 = 5;
                            while (contador_tipo2 < n_contador_tipo2 && !salir_bucle)
                            {
                                //progressBar1.Value++;
                                if (repetir_pregunta)
                                {
                                    if (tiempo_activo)
                                    {
                                        DateTime tiempo2 = DateTime.Now;
                                        TimeSpan total = new TimeSpan(tiempo2.Ticks - tiempo1.Ticks);
                                        if (total.TotalMinutes > 5)
                                        {
                                            tiempo_activo = false;
                                            repetir_pregunta = false;
                                            th.Interrupt();// th.Abort();
                                            th = new System.Threading.Thread(Detener);
                                            if (!salir_bucle)
                                            {
                                                th.Start();
                                            }
                                            else
                                            {
                                                th.Interrupt();//th.Abort();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tiempo1 = DateTime.Now;
                                        tiempo_activo = true;
                                    }

                                }
                                try
                                {
                                    //System.Threading.Thread th2 = new System.Threading.Thread(App);
                                    //th2.Start();
                                    /*ejecutar1ButtonClick_Automatico(0);
                                    calculoPolilinea.automatico = true;
                                    ejecutar2ButtonClick_Automatico();
                                    ejecutar3ButtonClick_Automatico();*/

                                    //th2.Interrupt();
                                    if (Lista_Resultados.Count > 0)
                                    {
                                        if (Lista_Resultados[Lista_Resultados.Count - 1].Item2 < distancia_menor)
                                        {
                                            distancia_menor = Lista_Resultados[Lista_Resultados.Count - 1].Item2;
                                        }
                                        if (Lista_Resultados[Lista_Resultados.Count - 1].Item2 == 1000)
                                        {
                                            Lista_Resultados.RemoveAt(Lista_Resultados.Count - 1);
                                        }
                                    }
                                }
                                catch
                                {

                                }
                                if (th2.IsAlive == false)
                                {
                                    materialFlatButton17_Click();
                                    Nuevos_Datos();
                                    if (contador_tipo1 == 0 && contador_tipo2 == 0 && tipo == 1)
                                    {
                                        progressBar1.Maximum = 52;
                                    }
                                    materialLabel47.Text = "Soluciones: " + Lista_Resultados.Count();
                                    materialLabel47.Update();
                                    progressBar1.Value++;
                                    th2 = new System.Threading.Thread(App);
                                    th2.Start();
                                }
                            }
                            th2.Interrupt();
                            th2.Interrupt();//th2.Abort();
                            int dibujar = 0;
                            double minimo = 100;
                            double medio = 0;
                            if (Lista_Resultados.Count > 0)
                            {
                                resultados = Lista_Resultados.Count;
                                for (int i = 0; i < Lista_Resultados.Count; i++)
                                {
                                    if (Lista_Resultados[i].Item2 < minimo)
                                    {
                                        dibujar = i;
                                        minimo = Lista_Resultados[i].Item2;
                                        if (Lista_Resultados[i].Item3 > 0)
                                        {
                                            medio = Lista_Resultados[i].Item3;
                                        }
                                    }
                                }
                                if (minimo < 100)
                                {
                                    calculoPolilinea = new CalculoPolilinea();
                                    calculoPolilinea.DibujarTrazado(Lista_Resultados[dibujar].Item1, documento);
                                    string json = JsonConvert.SerializeObject(Lista_Resultados[dibujar].Item5, Formatting.Indented);

                                    calculoPolilinea.Rotulado_final(Lista_Resultados[dibujar].Item1, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, documento);
                                    calculoPolilinea.Mcomponenetes = Lista_Resultados[dibujar].Item1;

                                    progressBar1.Value = 25;
                                    progressBar1.Maximum = 26;
                                    progressBar1.Value++;
                                    th.Interrupt();
                                    th.Interrupt();//th.Abort();
                                    componentes_automaticas = Lista_Resultados[dibujar].Item5;
                                    MessageBox.Show("Se han encontrado " + resultados + " trazados resultantes. \n\nRevise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hágalo con el cálculo manual y las opciones avanzadas.");
                                    Crear_Informe_Automatico(L_Resultados, dibujar, minimo, medio, Lista_Resultados, calculoPolilineaPreferencias.Ingenieril);
                                    //materialFlatButton27.Visible = true;
                                    L_Resultados = new List<Resultados>();

                                    Guardar_Json(Lista_Resultados[dibujar].Item5, Lista_original_3d, calculoPolilinea.Mcomponenetes, calculoPolilinea.Trazado_prueba);


                                }
                                else if (!salir_bucle)
                                {
                                    MessageBox.Show("No se ha obtenido un resultado correcto. Se procederá a suavizar la polílinea para buscar otro resultado");
                                    progressBar1.Value = 0;
                                    calculoPolilineaPreferencias.Suavizado = 2;
                                    contador_tipo1 = 0;
                                    contador_tipo2 = 0;
                                    tipoA = r[0];
                                    tipoC = r[1];
                                    Nuevos_Datos();
                                    Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int, List<Componente>>>();
                                    lista_resultados_numero = new List<int>();
                                    contador_resultados_numero = 0;
                                    distancia_menor = 100;
                                    th3 = new System.Threading.Thread(App2);
                                    th3.Start();
                                    while (contador_tipo2 < n_contador_tipo2 && !salir_bucle)
                                    {
                                        progressBar1.Value++;
                                        if (repetir_pregunta)
                                        {
                                            if (tiempo_activo)
                                            {
                                                DateTime tiempo2 = DateTime.Now;
                                                TimeSpan total = new TimeSpan(tiempo2.Ticks - tiempo1.Ticks);
                                                if (total.TotalMinutes > 5)
                                                {
                                                    tiempo_activo = false;
                                                    repetir_pregunta = false;
                                                    th.Interrupt();//th.Abort();
                                                    th = new System.Threading.Thread(Detener);
                                                    if (!salir_bucle)
                                                    {
                                                        th.Start();
                                                    }
                                                    else
                                                    {
                                                        th.Interrupt();//th.Abort();
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                tiempo1 = DateTime.Now;
                                                tiempo_activo = true;
                                            }

                                        }
                                        /*if (contador_tipo2 == 1 && Lista_Resultados.Count == 0)
                                        {
                                            calculoPolilineaPreferencias.Suavizado = 4;
                                        }
                                        if (contador_tipo2 == 2 && Lista_Resultados.Count == 0)
                                        {
                                            calculoPolilineaPreferencias.Suavizado = 6;
                                        }
                                        if (contador_tipo2 == 3 && Lista_Resultados.Count == 0)
                                        {
                                            calculoPolilineaPreferencias.Suavizado = 8;
                                        }*/
                                        try
                                        {
                                            if (th3.IsAlive == false)
                                            {
                                                materialFlatButton17_Click();
                                                Nuevos_Datos();
                                                progressBar1.Value++;
                                                if (contador_tipo2 == 1 && Lista_Resultados.Count == 0)
                                                {
                                                    calculoPolilineaPreferencias.Suavizado = 4;
                                                }
                                                if (contador_tipo2 == 2 && Lista_Resultados.Count == 0)
                                                {
                                                    calculoPolilineaPreferencias.Suavizado = 6;
                                                }
                                                if (contador_tipo2 == 3 && Lista_Resultados.Count == 0)
                                                {
                                                    calculoPolilineaPreferencias.Suavizado = 8;
                                                }
                                                th3 = new System.Threading.Thread(App2);
                                                th3.Start();
                                                materialLabel47.Text = "Soluciones: " + Lista_Resultados.Count();
                                                materialLabel47.Update();

                                            }
                                            /*ejecutar1ButtonClick_Automatico(calculoPolilineaPreferencias.Suavizado);
                                            calculoPolilinea.automatico = true;
                                            ejecutar2ButtonClick_Automatico();

                                            ejecutar3ButtonClick_Automatico();*/
                                            if (Lista_Resultados.Count > 0)
                                            {
                                                if (Lista_Resultados[Lista_Resultados.Count - 1].Item2 < distancia_menor)
                                                {
                                                    distancia_menor = Lista_Resultados[Lista_Resultados.Count - 1].Item2;
                                                }
                                                if (Lista_Resultados[Lista_Resultados.Count - 1].Item2 == 1000)
                                                {
                                                    Lista_Resultados.RemoveAt(Lista_Resultados.Count - 1);
                                                }
                                            }
                                            /*materialLabel47.Text = "Soluciones: " + Lista_Resultados.Count();
                                            materialLabel47.Update();*/
                                        }
                                        catch
                                        {

                                        }
                                        /*materialFlatButton17_Click();
                                        Nuevos_Datos();*/
                                    }
                                    th3.Interrupt();
                                    th3.Interrupt();//th3.Abort();
                                    dibujar = 0;
                                    minimo = 100;
                                    medio = 0;
                                    if (Lista_Resultados.Count > 0)
                                    {
                                        resultados = Lista_Resultados.Count;
                                        for (int i = 0; i < Lista_Resultados.Count; i++)
                                        {
                                            if (Lista_Resultados[i].Item2 < minimo)
                                            {
                                                dibujar = i;
                                                minimo = Lista_Resultados[i].Item2;
                                                if (Lista_Resultados[i].Item3 > 0)
                                                {
                                                    medio = Lista_Resultados[i].Item3;
                                                }
                                            }
                                        }

                                        calculoPolilinea = new CalculoPolilinea();
                                        if (minimo < 100)
                                        {

                                            calculoPolilinea.DibujarTrazado(Lista_Resultados[dibujar].Item1, documento);

                                            calculoPolilinea.Rotulado_final(Lista_Resultados[dibujar].Item1, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, documento);

                                            calculoPolilinea.Mcomponenetes = Lista_Resultados[dibujar].Item1;
                                            progressBar1.Value = 25;
                                            progressBar1.Maximum = 26;
                                            progressBar1.Value++;
                                            th.Interrupt();
                                            th.Interrupt();//th.Abort();
                                            componentes_automaticas = Lista_Resultados[dibujar].Item5;
                                            MessageBox.Show("Se han encontrado " + resultados + " trazados resultantes.\n\nRevise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hágalo con el cálculo manual y las opciones avanzadas.");
                                            Crear_Informe_Automatico(L_Resultados, dibujar, minimo, medio, Lista_Resultados, calculoPolilineaPreferencias.Ingenieril);
                                            L_Resultados = new List<Resultados>();
                                            //materialFlatButton27.Visible = true;
                                            Guardar_Json(Lista_Resultados[dibujar].Item5, Lista_original_3d, calculoPolilinea.Mcomponenetes, calculoPolilinea.Trazado_prueba);
                                        }
                                        else
                                        {
                                            progressBar1.Style = ProgressBarStyle.Blocks;
                                            progressBar1.Value = 0;
                                            progressBar1.Maximum = 1;
                                            th.Interrupt();
                                            th.Interrupt();//th.Abort();
                                            MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                        }

                                    }
                                    else
                                    {
                                        progressBar1.Style = ProgressBarStyle.Blocks;

                                        progressBar1.Value = 0;
                                        progressBar1.Maximum = 1;
                                        th.Interrupt();
                                        th.Interrupt();//th.Abort();
                                        MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                    }
                                }
                                else
                                {
                                    progressBar1.Style = ProgressBarStyle.Blocks;
                                    progressBar1.Value = 0;
                                    progressBar1.Maximum = 1;
                                    th.Interrupt();
                                    th.Interrupt();//th.Abort();
                                    MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                }
                            }
                            else if (!salir_bucle)
                            {

                                MessageBox.Show("No se ha obtenido un resultado correcto. Se procederá a suavizar la polílinea para buscar otro resultado");
                                progressBar1.Value = 0;
                                calculoPolilineaPreferencias.Suavizado = 2;
                                contador_tipo1 = 0;
                                contador_tipo2 = 0;
                                tipoA = r[0];
                                tipoC = r[1];
                                Nuevos_Datos();
                                Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int, List<Componente>>>();
                                lista_resultados_numero = new List<int>();
                                contador_resultados_numero = 0;
                                distancia_menor = 100;
                                th3 = new System.Threading.Thread(App2);
                                th3.Start();
                                while (contador_tipo2 < n_contador_tipo2 && !salir_bucle)
                                {
                                    //progressBar1.Value++;
                                    if (repetir_pregunta)
                                    {
                                        if (tiempo_activo)
                                        {
                                            DateTime tiempo2 = DateTime.Now;
                                            TimeSpan total = new TimeSpan(tiempo2.Ticks - tiempo1.Ticks);
                                            if (total.TotalMinutes > 5)
                                            {
                                                tiempo_activo = false;
                                                repetir_pregunta = false;
                                                th.Interrupt();//th.Abort();
                                                th = new System.Threading.Thread(Detener);
                                                if (!salir_bucle)
                                                {
                                                    th.Start();
                                                }
                                                else
                                                {
                                                    th.Interrupt();//th.Abort();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            tiempo1 = DateTime.Now;
                                            tiempo_activo = true;
                                        }

                                    }


                                    try
                                    {
                                        if (th3.IsAlive == false)
                                        {
                                            materialFlatButton17_Click();
                                            Nuevos_Datos();
                                            progressBar1.Value++;
                                            if (contador_tipo2 == 1 && Lista_Resultados.Count == 0)
                                            {
                                                calculoPolilineaPreferencias.Suavizado = 4;
                                            }
                                            if (contador_tipo2 == 2 && Lista_Resultados.Count == 0)
                                            {
                                                calculoPolilineaPreferencias.Suavizado = 6;
                                            }
                                            if (contador_tipo2 == 3 && Lista_Resultados.Count == 0)
                                            {
                                                calculoPolilineaPreferencias.Suavizado = 8;
                                            }
                                            th3 = new System.Threading.Thread(App2);
                                            th3.Start();
                                            materialLabel47.Text = "Soluciones: " + Lista_Resultados.Count();
                                            materialLabel47.Update();

                                        }
                                        /*ejecutar1ButtonClick_Automatico(calculoPolilineaPreferencias.Suavizado);
                                        calculoPolilinea.automatico = true;
                                        ejecutar2ButtonClick_Automatico();
                                        ejecutar3ButtonClick_Automatico();*/
                                        if (Lista_Resultados.Count > 0)
                                        {
                                            if (Lista_Resultados[Lista_Resultados.Count - 1].Item2 < distancia_menor)
                                            {
                                                distancia_menor = Lista_Resultados[Lista_Resultados.Count - 1].Item2;
                                            }
                                            if (Lista_Resultados[Lista_Resultados.Count - 1].Item2 == 1000)
                                            {
                                                Lista_Resultados.RemoveAt(Lista_Resultados.Count - 1);
                                            }
                                        }
                                        /*materialLabel47.Text = "Soluciones: " + Lista_Resultados.Count();
                                        materialLabel47.Update();*/
                                    }
                                    catch
                                    {

                                    }
                                    /*materialFlatButton17_Click();
                                    Nuevos_Datos();*/
                                }
                                th3.Interrupt();
                                th3.Interrupt();//th3.Abort();
                                dibujar = 0;
                                minimo = 100;
                                medio = 0;
                                if (Lista_Resultados.Count > 0)
                                {
                                    resultados = Lista_Resultados.Count;
                                    for (int i = 0; i < Lista_Resultados.Count; i++)
                                    {
                                        if (Lista_Resultados[i].Item2 < minimo)
                                        {
                                            dibujar = i;
                                            minimo = Lista_Resultados[i].Item2;
                                            if (Lista_Resultados[i].Item3 > 0)
                                            {
                                                medio = Lista_Resultados[i].Item3;
                                            }
                                        }
                                    }
                                    calculoPolilinea = new CalculoPolilinea();
                                    if (minimo < 100)
                                    {

                                        calculoPolilinea.DibujarTrazado(Lista_Resultados[dibujar].Item1, documento);

                                        calculoPolilinea.Rotulado_final(Lista_Resultados[dibujar].Item1, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, documento);

                                        calculoPolilinea.Mcomponenetes = Lista_Resultados[dibujar].Item1;
                                        progressBar1.Value = 25;
                                        progressBar1.Maximum = 26;
                                        progressBar1.Value++;
                                        th.Interrupt();
                                        th.Interrupt();//th.Abort();
                                        componentes_automaticas = Lista_Resultados[dibujar].Item5;
                                        MessageBox.Show("Se han encontrado " + resultados + " trazados resultantes.\n\nRevise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hágalo con el cálculo manual y las opciones avanzadas.");
                                        Crear_Informe_Automatico(L_Resultados, dibujar, minimo, medio, Lista_Resultados, calculoPolilineaPreferencias.Ingenieril);
                                        L_Resultados = new List<Resultados>();
                                        //materialFlatButton27.Visible = true;
                                        Guardar_Json(Lista_Resultados[dibujar].Item5, Lista_original_3d, calculoPolilinea.Mcomponenetes, calculoPolilinea.Trazado_prueba);
                                    }
                                    else
                                    {
                                        progressBar1.Style = ProgressBarStyle.Blocks;
                                        progressBar1.Value = 0;
                                        progressBar1.Maximum = 1;
                                        th.Interrupt();
                                        th.Interrupt();//th.Abort();
                                        MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                    }
                                }
                                else
                                {
                                    progressBar1.Style = ProgressBarStyle.Blocks;
                                    progressBar1.Value = 0;
                                    progressBar1.Maximum = 1;
                                    th.Interrupt();
                                    th.Interrupt();//th.Abort();
                                    MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                }
                            }
                            else
                            {
                                progressBar1.Style = ProgressBarStyle.Blocks;
                                progressBar1.Value = 0;
                                progressBar1.Maximum = 1;
                                th.Interrupt();
                                th.Interrupt();//th.Abort();
                                MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                            }

                            Lista_Resultados = null;
                            //th.Abort();

                            //Dispose(true);
                            GC.SuppressFinalize(this);
                            //GC.Collect();

                            //ejecutar1ButtonClick_Automatico();
                        }
                        else
                        {
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            progressBar1.Maximum = 1;
                            progressBar1.Value = 0;
                            materialFlatButton17_Click();
                        }

                    }
                    else
                    {
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 0;
                        MessageBox.Show("Error de formato o al abrir el archivo del proyecto");
                    }
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    MessageBox.Show("Error. El orden de los filtros esta repetido");
                }
            }
            this.calculoPolilineaStatusTextView.Visible = false;
        }
        private void App()
        {
            try
            {


                ejecutar1ButtonClick_Automatico(calculoPolilineaPreferencias.Suavizado);
                calculoPolilinea.automatico = true;
                calculoPolilinea.error_automatico = false;
                ejecutar2ButtonClick_Automatico();
                ejecutar3ButtonClick_Automatico(contador_resultados_numero);
                contador_resultados_numero++;
            }
            catch (Exception e)
            {

            }
        }
        private void App2()
        {
            try
            {


                ejecutar1ButtonClick_Automatico(calculoPolilineaPreferencias.Suavizado);
                calculoPolilinea.automatico = true;
                calculoPolilinea.error_automatico = false;
                ejecutar2ButtonClick_Automatico();
                ejecutar3ButtonClick_Automatico(contador_resultados_numero);
                contador_resultados_numero++;
            }
            catch (Exception e)
            {

            }
        }
        private void Detener()
        {
            try
            {
                DialogResult result = MessageBox.Show("EL CÁLCULO DEL EJE SE ENCUENTRA EN PROCESO. \n\nSi desea cancelar pulse cancelar y si no desea ver esta ventana pulse en aceptar.", "Cálculo en proceso", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                {
                    salir_bucle = true;
                    repetir_pregunta = false;
                    if (th2 != null)
                    {
                        th2.Interrupt();
                        th2.Interrupt();//th2.Abort();
                    }
                    if (th3 != null)
                    {
                        th3.Interrupt();
                        th3.Interrupt();//th3.Abort();
                    }

                    MessageBox.Show("El cálculo de más soluciones se ha cancelado.\n\nEl programa se detendrá despues de calcular la solución que está en progreso.");

                }
                if (result == DialogResult.OK)
                {
                    repetir_pregunta = true;
                }
            }
            catch (Exception e)
            {
            }

        }
        private void Perfil_Automatico()
        {
            materialLabel47.Text = "";
            materialLabel47.Invalidate();
            materialLabel47.Update();
            materialLabel47.Refresh();
            salir_bucle = false;
            progressBar1.Maximum = 8;
            progressBar1.Value = 0;
            progressBar1.Value++;
            progressBar1.Update();
            MessageBox.Show("Cálculo automático comenzado.");
            int escala;
            int n_suavizados;
            if (!string.IsNullOrEmpty(FactorEscala.Text))
            {
                escala = int.Parse(FactorEscala.Text);
            }
            else
            {
                escala = 1;
            }
            if (!string.IsNullOrEmpty(suavizar.Text))
            {
                n_suavizados = int.Parse(suavizar.Text);
            }
            else
            {
                n_suavizados = 1;
            }
            double v_ac;
            if (!string.IsNullOrEmpty(Varianza_acumulada.Text))
            {
                v_ac = double.Parse(Varianza_acumulada.Text);
            }
            else
            {
                v_ac = 1;
            }
            double distancia;
            if (!string.IsNullOrEmpty(text_distancia.Text))
            {
                distancia = double.Parse(text_distancia.Text);
            }
            else
            {
                distancia = 20;
            }
            double separacion;
            if (!string.IsNullOrEmpty(text_separacion.Text))
            {
                separacion = double.Parse(text_separacion.Text);
            }
            else
            {
                separacion = 40;
            }
            double pendiente;
            if (!string.IsNullOrEmpty(textpendiente.Text))
            {
                pendiente = double.Parse(textpendiente.Text);
                if (pendiente == 0)
                {
                    pendiente = 0.001;
                }
            }
            else
            {
                pendiente = 1;
            }
            double rotu;
            if (!string.IsNullOrEmpty(textBox_rot.Text))
            {
                rotu = double.Parse(textBox_rot.Text);
            }
            else
            {
                rotu = 100;
            }
            lista_original_perfil = new List<Point2d>();
            for (int i = 0; i < calculoPolilineaPerfil.Polilinea_Inicial.Count; i++)
            {
                lista_original_perfil.Add(new Point2d(calculoPolilineaPerfil.Polilinea_Inicial[i].p.X, calculoPolilineaPerfil.Polilinea_Inicial[i].p.Y));
            }
            double x = calculoPolilineaPerfil.x_inicial;
            double y = calculoPolilineaPerfil.y_inicial;
            double[] List_v_ac = { 0.001, 0.00001, 0.0000001 };
            double[] List_pendiente = { 1, 0.5 };


            int cont1 = 0;
            int cont2 = 0;
            bool tiempo_activo = false;
            System.Threading.Thread th = new System.Threading.Thread(Detener);
            th.Start();
            materialLabel47.Text = "Soluciones: 0";
            materialLabel47.Update();
            for (int i = 0; i < 6 && !salir_bucle; i++)
            {
                progressBar1.Value++;
                Calculoautomatico_Perfil(escala, List_v_ac[cont1], n_suavizados, distancia, separacion, List_pendiente[cont2], rotu);
                cont1++;
                if (cont1 == 3)
                {
                    cont1 = 0;
                    cont2++;
                }
                materialLabel47.Text = "Soluciones: " + Lista_Resultados_Perfil.Count();
                materialLabel47.Update();
                if (calculoPolilineaPerfil != null)
                {
                    Lista_Entidades_Perfil.Add(Tuple.Create(calculoPolilineaPerfil.Lista_Parabolas, calculoPolilineaPerfil.Lista_Rectas));
                    Lista_Polilineas_Perfil.Add(calculoPolilineaPerfil);
                }
                this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(lista_original_perfil, escala, x, y, n_suavizados, documento);
            }
            int poly_correcta = 0;
            double minimo = 100;
            DateTime tiempo1 = DateTime.Now;
            if (Lista_Resultados_Perfil.Count > 0)
            {
                if (repetir_pregunta)
                {
                    if (tiempo_activo)
                    {
                        DateTime tiempo2 = DateTime.Now;
                        TimeSpan total = new TimeSpan(tiempo2.Ticks - tiempo1.Ticks);
                        if (total.TotalMinutes > 5)
                        {
                            tiempo_activo = false;
                            repetir_pregunta = false;
                            th.Interrupt();//th.Abort();
                            th = new System.Threading.Thread(Detener);
                            if (!salir_bucle)
                            {
                                th.Start();
                            }
                            else
                            {
                                th.Interrupt();//th.Abort();
                            }
                        }
                    }
                    else
                    {
                        tiempo1 = DateTime.Now;
                        tiempo_activo = true;
                    }

                }
                int resultados = Lista_Resultados_Perfil.Count;
                for (int i = 0; i < Lista_Resultados_Perfil.Count; i++)
                {
                    if (Lista_Resultados_Perfil[poly_correcta].Item2 > Lista_Resultados_Perfil[i].Item2)
                    {
                        poly_correcta = i;
                        minimo = Lista_Resultados_Perfil[i].Item2;
                    }
                }
                calculoPolilineaPerfil.Dibujar_Trazado_Automatico_final(Lista_Resultados_Perfil[poly_correcta].Item1);
                calculoPolilineaPerfil.Lista_Parabolas = Lista_Entidades_Perfil[poly_correcta].Item1;
                calculoPolilineaPerfil.Lista_Rectas = Lista_Entidades_Perfil[poly_correcta].Item2;
                calculoPolilineaPerfil = Lista_Polilineas_Perfil[poly_correcta];
                if (calculoPolilinea != null)
                {
                    calculoPolilineaPerfil.ExportarLandXML(calculoPolilinea.Mcomponenetes, true);
                }
                else
                {
                    calculoPolilineaPerfil.ExportarLandXML(new List<EjeDeTrazado.componentes.Componente>(), true);
                }
                calculoPolilineaPerfil.Rotular(rotu);
                progressBar1.Value = 7;
                calculoPolilineaPerfil.Informe();
                GuardarPerfilAutomatico(calculoPolilineaPerfil);
                progressBar1.Value = 8;
                //calculoPolilineaPerfil = new CalculoPolilineaPerfil();
                Lista_Entidades_Perfil = new List<Tuple<List<Parabola>, List<Pendiente>>>();
                Lista_Polilineas_Perfil = new List<CalculoPolilineaPerfil>();
                Lista_Resultados_Perfil = new List<Tuple<Polyline, double>>();
                MessageBox.Show("Se han encontrado " + resultados + " trazados resultantes.\n\nRevise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hágalo con el cálculo manual y las opciones avanzadas.");
            }
            else
            {
                progressBar1.Value = 0;
                MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hágalo con el cálculo manual y las opciones avanzadas.");
            }
            th.Interrupt();//th.Abort();
        }
        private void Perfil_Automatico_Circular()
        {
            materialLabel47.Text = "";
            materialLabel47.Invalidate();
            materialLabel47.Update();
            materialLabel47.Refresh();
            salir_bucle = false;
            progressBar1.Maximum = 8;
            progressBar1.Value = 0;
            progressBar1.Value++;
            progressBar1.Update();
            MessageBox.Show("Cálculo automático comenzado.");
            int escala;
            int n_suavizados;
            if (!string.IsNullOrEmpty(FactorEscala.Text))
            {
                escala = int.Parse(FactorEscala.Text);
            }
            else
            {
                escala = 1;
            }
            if (!string.IsNullOrEmpty(suavizar.Text))
            {
                n_suavizados = int.Parse(suavizar.Text);
            }
            else
            {
                n_suavizados = 1;
            }
            double v_ac;
            if (!string.IsNullOrEmpty(Varianza_acumulada.Text))
            {
                v_ac = double.Parse(Varianza_acumulada.Text);
            }
            else
            {
                v_ac = 1;
            }
            double distancia;
            if (!string.IsNullOrEmpty(text_distancia.Text))
            {
                distancia = double.Parse(text_distancia.Text);
            }
            else
            {
                distancia = 20;
            }
            double separacion;
            if (!string.IsNullOrEmpty(text_separacion.Text))
            {
                separacion = double.Parse(text_separacion.Text);
            }
            else
            {
                separacion = 40;
            }
            double pendiente;
            if (!string.IsNullOrEmpty(textpendiente.Text))
            {
                pendiente = double.Parse(textpendiente.Text);
                if (pendiente == 0)
                {
                    pendiente = 0.001;
                }
            }
            else
            {
                pendiente = 1;
            }
            double rotu;
            if (!string.IsNullOrEmpty(textBox_rot.Text))
            {
                rotu = double.Parse(textBox_rot.Text);
            }
            else
            {
                rotu = 100;
            }
            lista_original_perfil = new List<Point2d>();
            for (int i = 0; i < calculoPolilineaPerfil.Polilinea_Inicial.Count; i++)
            {
                lista_original_perfil.Add(new Point2d(calculoPolilineaPerfil.Polilinea_Inicial[i].p.X, calculoPolilineaPerfil.Polilinea_Inicial[i].p.Y));
            }
            double x = calculoPolilineaPerfil.x_inicial;
            double y = calculoPolilineaPerfil.y_inicial;
            double[] List_v_ac = { 0.001, 0.00001, 0.0000001 };
            double[] List_pendiente = { 1, 0.5 };


            int cont1 = 0;
            int cont2 = 0;
            bool tiempo_activo = false;
            System.Threading.Thread th = new System.Threading.Thread(Detener);
            th.Start();
            materialLabel47.Text = "Soluciones: 0";
            materialLabel47.Update();
            for (int i = 0; i < 6 && !salir_bucle; i++)
            {
                progressBar1.Value++;
                Calculoautomatico_Perfil_Circular(escala, List_v_ac[cont1], n_suavizados, distancia, separacion, List_pendiente[cont2], rotu);
                cont1++;
                if (cont1 == 3)
                {
                    cont1 = 0;
                    cont2++;
                }
                materialLabel47.Text = "Soluciones: " + Lista_Resultados_Perfil.Count();
                materialLabel47.Update();
                if (calculoPolilineaPerfil != null)
                {
                    Lista_Entidades_Perfil.Add(Tuple.Create(calculoPolilineaPerfil.Lista_Parabolas, calculoPolilineaPerfil.Lista_Rectas));
                    Lista_Polilineas_Perfil.Add(calculoPolilineaPerfil);
                }
                this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(lista_original_perfil, escala, x, y, n_suavizados, documento);
            }
            int poly_correcta = 0;
            double minimo = 100;
            DateTime tiempo1 = DateTime.Now;
            if (Lista_Resultados_Perfil.Count > 0)
            {
                if (repetir_pregunta)
                {
                    if (tiempo_activo)
                    {
                        DateTime tiempo2 = DateTime.Now;
                        TimeSpan total = new TimeSpan(tiempo2.Ticks - tiempo1.Ticks);
                        if (total.TotalMinutes > 5)
                        {
                            tiempo_activo = false;
                            repetir_pregunta = false;
                            th.Interrupt();//th.Abort();
                            th = new System.Threading.Thread(Detener);
                            if (!salir_bucle)
                            {
                                th.Start();
                            }
                            else
                            {
                                th.Interrupt();//th.Abort();
                            }
                        }
                    }
                    else
                    {
                        tiempo1 = DateTime.Now;
                        tiempo_activo = true;
                    }

                }
                int resultados = Lista_Resultados_Perfil.Count;
                for (int i = 0; i < Lista_Resultados_Perfil.Count; i++)
                {
                    if (Lista_Resultados_Perfil[poly_correcta].Item2 > Lista_Resultados_Perfil[i].Item2)
                    {
                        poly_correcta = i;
                        minimo = Lista_Resultados_Perfil[i].Item2;
                    }
                }
                calculoPolilineaPerfil.Dibujar_Trazado_Automatico_final(Lista_Resultados_Perfil[poly_correcta].Item1);
                calculoPolilineaPerfil.Lista_Parabolas = Lista_Entidades_Perfil[poly_correcta].Item1;
                calculoPolilineaPerfil.Lista_Rectas = Lista_Entidades_Perfil[poly_correcta].Item2;
                calculoPolilineaPerfil = Lista_Polilineas_Perfil[poly_correcta];
                if (calculoPolilinea != null)
                {
                    calculoPolilineaPerfil.ExportarLandXML(calculoPolilinea.Mcomponenetes, false);
                }
                else
                {
                    calculoPolilineaPerfil.ExportarLandXML(new List<EjeDeTrazado.componentes.Componente>(), false);
                }
                calculoPolilineaPerfil.Rotular_Circular(rotu);
                progressBar1.Value = 7;
                calculoPolilineaPerfil.Informe_Circular();
                GuardarPerfilAutomatico(calculoPolilineaPerfil);
                progressBar1.Value = 8;
                //calculoPolilineaPerfil = new CalculoPolilineaPerfil();
                Lista_Entidades_Perfil = new List<Tuple<List<Parabola>, List<Pendiente>>>();
                Lista_Polilineas_Perfil = new List<CalculoPolilineaPerfil>();
                Lista_Resultados_Perfil = new List<Tuple<Polyline, double>>();
                MessageBox.Show("Se han encontrado " + resultados + " trazados resultantes.\n\nRevise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hágalo con el cálculo manual y las opciones avanzadas.");
            }
            else
            {
                progressBar1.Value = 0;
                MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hágalo con el cálculo manual y las opciones avanzadas.");
            }
            th.Interrupt();//th.Abort();
        }
        private void materialFlatButton19_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                /*if (calculoPolilineaPerfil != null)
                {
                    if (calculoPolilineaPerfil.iniciado)
                    {
                        calculoPolilineaPerfil = null;
                    }
                }*/
                calculoPolilineaPerfil.VaciarCalculopolilineaPerfil();
                Ocultar_Modificadores();
                materialFlatButton24.Visible = false;
                materialFlatButton28.Visible = false;
                conta_apartado_perfil = 1;
                progressBar1.Style = ProgressBarStyle.Blocks;
                if (calculoPolilineaPerfil != null)
                {
                    calculoPolilineaPerfil.iniciado = true;
                    Perfil_Automatico();
                    materialFlatButton34.Visible = false;
                    materialFlatButton24.Visible = false;
                    //calculoPolilineaPerfil = null;
                }
                else
                {
                    progressBar1.Value = 0;
                    MessageBox.Show("No ha cargado la polilínea 3d en el programa.");
                }
            }
            /* this.calculoPolilineaPerfil = null;
             this.pasosEjecutados = -1;
             this.paso1EjecutadoTextView.Visible = false;
             this.paso2EjecutadoTextView.Visible = false;
             this.paso3EjecutadoTextView.Visible = false;
             this.calculoPolilineaStatusTextView.Visible = false;
             this.ejecutarViabilidadSinParar = false;
             this.detenerEnIteracion = false;
             this.iteracion = -1;
             int escala;
             int n_suavizados;
             if (!string.IsNullOrEmpty(FactorEscala.Text))
             {
                 escala = int.Parse(FactorEscala.Text);
             }
             else
             {
                 escala = 1;
             }
             if (!string.IsNullOrEmpty(suavizar.Text))
             {
                 n_suavizados = int.Parse(suavizar.Text);
             }
             else
             {
                 n_suavizados = 1;
             }
             double v_ac;
             if (!string.IsNullOrEmpty(Varianza_acumulada.Text))
             {
                 v_ac = double.Parse(Varianza_acumulada.Text);
             }
             else
             {
                 v_ac = 1;
             }
             double distancia;
             if (!string.IsNullOrEmpty(text_distancia.Text))
             {
                 distancia = double.Parse(text_distancia.Text);
             }
             else
             {
                 distancia = 20;
             }
             double separacion;
             if (!string.IsNullOrEmpty(text_separacion.Text))
             {
                 separacion = double.Parse(text_separacion.Text);
             }
             else
             {
                 separacion = 40;
             }
             double pendiente;
             if (!string.IsNullOrEmpty(textpendiente.Text))
             {
                 pendiente = double.Parse(textpendiente.Text);
                 if (pendiente == 0)
                 {
                     pendiente = 0.001;
                 }
             }
             else
             {
                 pendiente = 1;
             }
             double rotu;
             if (!string.IsNullOrEmpty(textBox_rot.Text))
             {
                 rotu = double.Parse(textBox_rot.Text);
             }
             else
             {
                 rotu = 100;
             }
             if (comprobar())
             {
                 dsApp dsApp = this.abrirArchivoDeProyectoPerfil();

                 if (dsApp != null)
                 {
                     //obtener parametros de inicializacion de CalculoPolilineaController
                     this.calculoPolilineaPreferenciasPerfil = this.obtenerParametrosCalculoPolilineaPerfil();
                     this.calculoPolilineaPerfil = null;
                     if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                     {
                         this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.It, escala, n_suavizados, calculoPolilineaPreferenciasPerfil.Dis_eliminar);
                     }
                     else
                     {
                         this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.Orden, calculoPolilineaPreferenciasPerfil.It);
                     }

                     this.calculoPolilineaStatusTextView.Visible = true;
                     this.pasosEjecutados = 0;

                     lista_original_perfil = new List<Point2d>();
                     for (int i=0;i<calculoPolilineaPerfil.Polilinea_Inicial.Count;i++)
                     {
                         lista_original_perfil.Add(new Point2d(calculoPolilineaPerfil.Polilinea_Inicial[i].p.X, calculoPolilineaPerfil.Polilinea_Inicial[i].p.Y));
                     }
                     double x = calculoPolilineaPerfil.x_inicial;
                     double y = calculoPolilineaPerfil.y_inicial;
                     double[] List_v_ac = {0.001, 0.00001, 0.0000001 };
                     double[] List_pendiente = { 1, 0.5 };
                     int cont1 = 0;
                     int cont2 = 0;
                     for (int i=0;i<6;i++)
                     {
                         Calculoautomatico_Perfil(escala, List_v_ac[cont1], n_suavizados, distancia, separacion, List_pendiente[cont2], rotu);
                         cont1++;
                         if (cont1==3)
                         {
                             cont1 = 0;
                             cont2++;
                         }
                         Lista_Entidades_Perfil.Add(Tuple.Create(calculoPolilineaPerfil.Lista_Parabolas,calculoPolilineaPerfil.Lista_Rectas));
                         Lista_Polilineas_Perfil.Add(calculoPolilineaPerfil);
                         this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(lista_original_perfil,escala,x,y,n_suavizados);
                     }
                     int poly_correcta=0;
                     if (Lista_Resultados_Perfil.Count>0)
                     {
                         for (int i = 0; i < Lista_Resultados_Perfil.Count; i++)
                         {
                             if (Lista_Resultados_Perfil[poly_correcta].Item2 > Lista_Resultados_Perfil[i].Item2)
                             {
                                 poly_correcta = i;
                             }
                         }
                         calculoPolilineaPerfil.Dibujar_Trazado_Automatico_final(Lista_Resultados_Perfil[poly_correcta].Item1);
                         calculoPolilineaPerfil.Lista_Parabolas = Lista_Entidades_Perfil[poly_correcta].Item1;
                         calculoPolilineaPerfil.Lista_Rectas = Lista_Entidades_Perfil[poly_correcta].Item2;
                         calculoPolilineaPerfil = Lista_Polilineas_Perfil[poly_correcta];
                         calculoPolilineaPerfil.Rotular(rotu);

                         calculoPolilineaPerfil.Informe();
                         calculoPolilineaPerfil = new CalculoPolilineaPerfil();
                         Lista_Entidades_Perfil = new List<Tuple<List<Parabola>, List<Pendiente>>>();
                         Lista_Polilineas_Perfil = new List<CalculoPolilineaPerfil>();
                         Lista_Resultados_Perfil = new List<Tuple<Polyline, double>>();
                         MessageBox.Show("Revise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hagalo con el calculo manual y las opciones avanzadas.");
                     }
                     else
                     {
                         MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                     }


                 }
                 else
                 {
                     MessageBox.Show("Error al abrir el archivo del proyecto");
                 }
             }
             else
             {
                 MessageBox.Show("Error. El orden de los filtros esta repetido");
             }*/

        }

        public void Calculoautomatico_Perfil(int escala, double v_ac, int n_suavizados, double distancia, double separacion, double pendiente, double rotu)
        {
            calculoPolilineaPerfil.RellenarPerfil(v_ac, n_suavizados);
            calculoPolilineaPerfil.DividirSentidos();
            calculoPolilineaPerfil.QuitarSuavizado();
            calculoPolilineaPerfil.MatrizAcuerdo3();
            calculoPolilineaPerfil.Quitar_Acuerdos(distancia, separacion, pendiente);

            try
            {
                calculoPolilineaPerfil.PuntoInflexion();
                //calculoPolilineaPerfil.Dibujar_Acuerdos(1);
                calculoPolilineaPerfil.CalcularEntreParabolas();
                calculoPolilineaPerfil.CalculoEntreParabolas_Dibujar();
                calculoPolilineaPerfil.Componente_Inicial();
                calculoPolilineaPerfil.Componente_Final();
                //calculoPolilineaPerfil.Dibujar_Rectas(2);
                //calculoPolilineaPerfil.Dibujar_Acuerdos(2);
                calculoPolilineaPerfil.Acuerdo_Entre_Pendientes();
                //calculoPolilineaPerfil.Dibujar_Rectas(3);
                //calculoPolilineaPerfil.Dibujar_Acuerdos(3);
                if (calculoPolilineaPerfil.Validar_pendientes())
                {

                    Polyline poly = calculoPolilineaPerfil.CrearTrazado_Automatico();
                    List<double> distancias = calculoPolilineaPerfil.Distancia_Puntos_Resultado(poly, lista_original_perfil);
                    double dis = 0;
                    for (int i = 0; i < distancias.Count; i++)
                    {
                        if (dis < distancias[i])
                        {
                            dis = distancias[i];
                        }
                    }
                    Lista_Resultados_Perfil.Add(Tuple.Create(poly, dis));
                }
                else
                {
                    
                    calculoPolilineaPerfil = null;
                }

            }
            catch
            {
                MessageBox.Show("No se crea ningun acuerdo. Cambie los parametros y comience de nuevo.");
                calculoPolilineaPerfil = null;
            }
        }
        public void Calculoautomatico_Perfil_Circular(int escala, double v_ac, int n_suavizados, double distancia, double separacion, double pendiente, double rotu)
        {
            calculoPolilineaPerfil.RellenarPerfil(v_ac, n_suavizados);
            calculoPolilineaPerfil.DividirSentidos();
            calculoPolilineaPerfil.QuitarSuavizado();
            calculoPolilineaPerfil.MatrizAcuerdo3();
            calculoPolilineaPerfil.Quitar_Acuerdos(distancia, separacion, pendiente);

            try
            {
                calculoPolilineaPerfil.PuntoInflexion();
                //calculoPolilineaPerfil.Dibujar_Acuerdos(1);
                calculoPolilineaPerfil.CalcularEntreParabolas();
                calculoPolilineaPerfil.CalculoEntreParabolas_Dibujar();
                calculoPolilineaPerfil.Componente_Inicial();
                calculoPolilineaPerfil.Componente_Final();
                //calculoPolilineaPerfil.Dibujar_Rectas(2);
                //calculoPolilineaPerfil.Dibujar_Acuerdos(2);
                calculoPolilineaPerfil.Acuerdo_Entre_Pendientes();
                calculoPolilineaPerfil.Acerdo_Curva();
                //calculoPolilineaPerfil.Dibujar_Rectas(3);
                //calculoPolilineaPerfil.Dibujar_Acuerdos(3);
                Polyline poly = calculoPolilineaPerfil.CrearTrazado_Circular_Automatico();
                List<double> distancias = calculoPolilineaPerfil.Distancia_Puntos_Resultado(poly, lista_original_perfil);
                double dis = 0;
                for (int i = 0; i < distancias.Count; i++)
                {
                    if (dis < distancias[i])
                    {
                        dis = distancias[i];
                    }
                }
                Lista_Resultados_Perfil.Add(Tuple.Create(poly, dis));

            }
            catch
            {
                MessageBox.Show("No se crea ningun acuerdo. Cambie los parametros y comience de nuevo.");
                calculoPolilineaPerfil = null;
            }
        }
        private bool Comprobar_funcion_AUTOCAD()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            if (acDoc.CommandInProgress.Count() == 0)
            {
                return true;
            }
            else
            {
                calculoPolilineaPerfil = null;
                MessageBox.Show("Cancele el comando " + acDoc.CommandInProgress + " para continuar");
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Maximum = 1;
                progressBar1.Value = 0;
                return false;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Si entre 2 curvas, hay mas de un punto sin asignación a elementos geométricos, se comprueba que la distancia entre esos puntos es 'n' veces más grande que la media de distancias entre esas 2 curvas y que es 'K' mayor respecto de la distancia total entre esas 2 curvas.", "Información");
        }
        private void Crear_Informe_Automatico(List<Resultados> lista, int n, double error,
            double medio, List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int, List<Componente>>> Lista_resultados, bool ingenieril)
        {
            int cont = 0;
            List<Resultados> lista_aux = new List<Resultados>();
            for (int i = 0; i < lista.Count; i++)
            {
                if (Lista_resultados.Count > cont)
                {
                    if (lista[i].Resultado == Lista_resultados[cont].Item4)
                    {
                        lista_aux.Add(lista[i]);
                        cont++;
                    }
                }

            }
            lista = lista_aux;
            DialogResult resultinforme = MessageBox.Show("¿Desea crear un informe de los resultados?", "Resultados", MessageBoxButtons.YesNo);
            if (resultinforme == DialogResult.Yes)
            {
                string nombre_informe = "Informe resultados";
                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog1.Title = "Guardar informe de los resultados";
                saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.DefaultExt = "csv";
                saveFileDialog1.InitialDirectory = !string.IsNullOrEmpty(ruta_principal) ? ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    nombre_informe = saveFileDialog1.FileName;
                    System.IO.TextWriter output = new StreamWriter(nombre_informe, false, Encoding.BigEndianUnicode);
                    if (ingenieril)
                    {
                        output.WriteLine();
                        output.WriteLine();
                        output.WriteLine();
                        output.Write("Nº");
                        output.Write(";");
                        output.Write("Varianza rectas");
                        output.Write(";");
                        output.Write("Porcentaje de clusterización");
                        output.Write(";");
                        output.Write("Nº Componentes");
                        output.Write(";");
                        output.WriteLine();
                        for (int i = 0; i < lista.Count(); i++)
                        {
                            output.Write(i + 1);
                            output.Write(";");
                            output.Write(lista[i].N_C);
                            output.Write(";");
                            output.Write(lista[i].Por_C);
                            output.Write(";");
                            output.Write(lista[i].Num_C);
                            output.Write(";");
                            output.WriteLine();
                        }
                    }
                    else
                    {
                        output.WriteLine();
                        output.WriteLine();
                        output.WriteLine();
                        output.Write("Nº");
                        output.Write(";");
                        output.Write("Nº de curvas máximas");
                        output.Write(";");
                        output.Write("Porcentaje de clusterización");
                        output.Write(";");
                        output.Write("Puntos de clusterización");
                        output.Write(";");
                        output.Write("Grados de unión");
                        output.Write(";");
                        output.Write("División en rectas");
                        output.Write(";");
                        output.Write("Nº Componentes");
                        output.Write(";");
                        output.WriteLine();
                        for (int i = 0; i < lista.Count(); i++)
                        {
                            output.Write(i + 1);
                            output.Write(";");
                            output.Write(lista[i].N_C);
                            output.Write(";");
                            output.Write(lista[i].Por_C);
                            output.Write(";");
                            output.Write(lista[i].Pun_C);
                            output.Write(";");
                            output.Write(lista[i].G_U);
                            output.Write(";");
                            if (lista[i].D_R)
                            {
                                output.Write("Si");
                            }
                            else
                            {
                                output.Write("No");
                            }
                            output.Write(";");
                            output.Write(lista[i].Num_C);
                            output.Write(";");
                            output.WriteLine();
                        }
                    }

                    output.WriteLine();
                    output.WriteLine();
                    output.Write(";");
                    output.Write(";");
                    output.Write("Mejor resultado:");
                    output.Write(";");
                    output.Write(n + 1);
                    output.Write(";");
                    output.WriteLine();
                    output.Write(";");
                    output.Write(";");
                    output.Write("Error Máximo");
                    output.Write(";");
                    output.Write(error);
                    output.Write(";");
                    output.Write("Error Medio");
                    output.Write(";");
                    output.Write(medio);
                    output.Write(";");
                    output.Flush();
                    output.Close();
                }
                else
                {

                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Con este algoritmo reducimos el número de clotoides inferiores al parámetro dado siempre que sea posible.", "Información");
        }

        private void materialCheckBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox12.Checked == true)
            {
                textBox10.Enabled = true;
            }
            else
            {
                textBox10.Enabled = false;
            }
        }

        private void materialFlatButton20_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                materialLabel48.Text = "";
                //Resetear estado actual
                materialLabel47.Text = "";
                salir_bucle = false;
                this.calculoPolilinea = null;
                this.pasosEjecutados = -1;
                this.paso1EjecutadoTextView.Visible = false;
                this.paso2EjecutadoTextView.Visible = false;
                this.paso3EjecutadoTextView.Visible = false;
                this.calculoPolilineaStatusTextView.Visible = false;
                this.ejecutarViabilidadSinParar = false;
                this.detenerEnIteracion = false;
                this.iteracion = -1;
                materialFlatButton7.Visible = false;
                materialFlatButton8.Visible = false;
                materialFlatButton9.Visible = false;
                List_verificacion.Clear();
                List_componentes.Clear();
                List_polilinea.Clear();
                List_traza.Clear();
                contador_tipo1 = 0;
                contador_tipo2 = 0;
                int resultados;

                L_Resultados = new List<Resultados>();
                if (comprobar())
                {
                    dsApp dsApp = this.abrirArchivoDeProyecto();

                    if (dsApp != null)
                    {
                        //obtener parametros de inicializacion de CalculoPolilineaController
                        this.calculoPolilineaPreferencias = this.obtenerParametrosCalculoPolilinea();

                        if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, documento);
                        }
                        else
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, documento);
                        }
                        this.calculoPolilineaStatusTextView.Visible = false;
                        this.pasosEjecutados = 0;
                        //MessageBox.Show("CalculoPolilinea inicializado");
                        int[] r = calculoPolilinea.Propiedades(dsApp.Polilinea.Count() - 1, dsApp);
                        tipoA = r[0];
                        tipoC = r[1];
                        string porcentaje = "";
                        int puntos_cluster = calculoPolilinea.Contar_Puntos_Cluster();
                        if (puntos_cluster > 10)
                        {
                            puntos_cluster = 10;
                        }
                        if (tipoA == 1 && tipoC == 1)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                porcentaje += "- " + A1_C1[i] + "\n";
                            }
                            if (puntos_cluster == 0)
                            {
                                porcentaje += "\nEl número de puntos de clusterización es de  10\n";
                            }
                            else
                            {
                                porcentaje += "\nEl número de puntos de clusterización es de  " + puntos_cluster + "\n";
                            }
                        }
                        else if (tipoA == 1 && tipoC == 2)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                porcentaje += "- " + A1_C2[i] + "\n";
                            }
                            if (puntos_cluster == 0)
                            {
                                porcentaje += "\nEl número de puntos de clusterización es de  5\n";
                            }
                            else
                            {
                                porcentaje += "\nEl número de puntos de clusterización es de  " + puntos_cluster + "\n";
                            }
                        }
                        else if (tipoA == 1 && tipoC == 3)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                porcentaje += "- " + A1_C3[i] + "\n";
                            }
                            porcentaje += "\nEl número de puntos de clusterización es de 3\n";
                        }
                        else if (tipoA == 2 && tipoC == 1)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                porcentaje += "- " + A2_C1[i] + "\n";
                            }
                            if (puntos_cluster == 0)
                            {
                                porcentaje += "\nEl número de puntos de clusterización es de  5\n";
                            }
                            else
                            {
                                porcentaje += "\nEl número de puntos de clusterización es de  " + puntos_cluster + "\n";
                            }
                        }
                        else if (tipoA == 2 && tipoC == 2)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                porcentaje += "- " + A2_C2[i] + "\n";
                            }
                            if (puntos_cluster == 0)
                            {
                                porcentaje += "\nEl número de puntos de clusterización es de  3\n";
                            }
                            else
                            {
                                porcentaje += "\nEl número de puntos de clusterización es de  " + puntos_cluster + "\n";
                            }

                        }
                        else if (tipoA == 2 && tipoC == 3)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                porcentaje += "- " + A2_C3[i] + "\n";
                            }
                            porcentaje += "\nEl número de puntos de clusterización es de 3\n";
                        }
                        //Para ingenieril
                        porcentaje += "\nPorcentaje de Clusterización ingenieril:\n";
                        for (int i = 0; i < 5; i++)
                        {
                            porcentaje += "- " + curvas_ingenieril[i] + "\n";
                        }
                        porcentaje += "\nVariación de recta ingenieril:\n";
                        for (int i = 0; i < 5; i++)
                        {
                            porcentaje += "- " + rectas_ingenieril[i] + "\n";
                        }
                        string mensaje = "El tipo de polilínea es A" + tipoA + " - C" + tipoC + "\n\nSe utilizarán los siguientes valores en el cálculo automático:\n\n" +
                            "Porcentaje de Clusterización:\n" + porcentaje + "\nCurvas máximas utilizadas: 6, 4, 2, 8, 1";
                        if (tipoA == 1 && tipoC == 1)
                        {
                            mensaje += "\nEn este caso se aconseja dividir las rectas en 20 metros y con un grado de unión en rectas de 0.2.";
                        }
                        MessageBox.Show(mensaje, "Tipo de polilinea");
                    }
                }
            }
        }

        private void materialFlatButton21_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                materialLabel48.Text = "";
                //Resetear estado actual
                this.calculoPolilinea = null;
                this.pasosEjecutados = -1;
                this.paso1EjecutadoTextView.Visible = false;
                this.paso2EjecutadoTextView.Visible = false;
                this.paso3EjecutadoTextView.Visible = false;
                this.calculoPolilineaStatusTextView.Visible = false;
                this.ejecutarViabilidadSinParar = false;
                this.detenerEnIteracion = false;
                this.iteracion = -1;
                materialFlatButton7.Visible = false;
                materialFlatButton8.Visible = false;
                materialFlatButton9.Visible = false;
                List_verificacion.Clear();
                List_componentes.Clear();
                List_polilinea.Clear();
                List_traza.Clear();
                distancia_menor = 100;
                Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int, List<Componente>>>();
                if (comprobar())
                {
                    dsApp dsApp = this.abrirArchivoDeProyecto();

                    if (dsApp != null)
                    {
                        //obtener parametros de inicializacion de CalculoPolilineaController
                        this.calculoPolilineaPreferencias = this.obtenerParametrosCalculoPolilinea();

                        if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion,
                                calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado,
                                calculoPolilineaPreferencias.Dis_eliminar, true, documento, Lista_original_3d);
                        }
                        else
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion,
                                calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It,
                                calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, true, documento, Lista_original_3d);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error de formato o al abrir el archivo del proyecto");
                    }
                }
                else
                {
                    MessageBox.Show("Error. El orden de los filtros esta repetido");
                }
            }
        }
        private int Get_N_P(int tipoA, int tipoC)
        {
            int puntos_cluster = calculoPolilinea.Contar_Puntos_Cluster();
            if (puntos_cluster > 10)
            {
                puntos_cluster = 10;
            }
            if (tipoA == 1 && tipoC == 1)
            {
                if (puntos_cluster == 0)
                {
                    return 10;
                }
                else
                {
                    return puntos_cluster;
                }
            }
            else if (tipoA == 1 && tipoC == 2)
            {
                if (puntos_cluster == 0)
                {
                    return 5;
                }
                else
                {
                    return puntos_cluster;
                }
            }
            else if (tipoA == 1 && tipoC == 3)
            {
                return 3;
            }
            else if (tipoA == 2 && tipoC == 1)
            {
                if (puntos_cluster == 0)
                {
                    return 3;
                }
                else
                {
                    return puntos_cluster;
                }
            }
            else if (tipoA == 2 && tipoC == 2)
            {
                if (puntos_cluster == 0)
                {
                    return 3;
                }
                else
                {
                    return puntos_cluster;
                }

            }
            else if (tipoA == 2 && tipoC == 3)
            {
                return 3;
            }
            return 3;
        }
        private bool Poli_Larga(List<Punto> lista)
        {
            double distancia = 0;
            for (int i = 0; i < lista.Count() - 1; i++)
            {
                distancia += Distancia(lista[i].p, lista[i + 1].p);
            }
            if (distancia > 10000 && lista.Count > 800)
            {
                return true;
            }
            return false;
        }
        private double Distancia(Point2d a, Point2d b)
        {
            double d;
            d = Math.Pow(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2), 0.5);
            return d;
        }

        private void materialFlatButton22_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                /*if (calculoPolilineaPerfil != null)
                {
                    if (calculoPolilineaPerfil.iniciado)
                    {
                        calculoPolilineaPerfil = null;
                    }
                }*/
                calculoPolilineaPerfil.VaciarCalculopolilineaPerfil();
                Ocultar_Modificadores();
                conta_apartado_perfil = 1;
                progressBar1.Maximum = 6;
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Value = 0;
                progressBar1.Value++;
                if (calculoPolilineaPerfil != null)
                {
                    calculoPolilineaPerfil.iniciado = true;
                    int escala;
                    if (!string.IsNullOrEmpty(FactorEscala.Text))
                    {
                        escala = int.Parse(FactorEscala.Text);
                    }
                    else
                    {
                        escala = 1;
                    }
                    double v_ac;
                    if (!string.IsNullOrEmpty(Varianza_acumulada.Text))
                    {
                        v_ac = double.Parse(Varianza_acumulada.Text);
                    }
                    else
                    {
                        v_ac = 1;
                    }
                    int n_suavizados;
                    if (!string.IsNullOrEmpty(suavizar.Text))
                    {
                        n_suavizados = int.Parse(suavizar.Text);
                    }
                    else
                    {
                        n_suavizados = 1;
                    }
                    double distancia;
                    if (!string.IsNullOrEmpty(text_distancia.Text))
                    {
                        distancia = double.Parse(text_distancia.Text);
                    }
                    else
                    {
                        distancia = 20;
                    }
                    double separacion;
                    if (!string.IsNullOrEmpty(text_separacion.Text))
                    {
                        separacion = double.Parse(text_separacion.Text);
                    }
                    else
                    {
                        separacion = 40;
                    }
                    double pendiente;
                    if (!string.IsNullOrEmpty(textpendiente.Text))
                    {
                        pendiente = double.Parse(textpendiente.Text);
                        if (pendiente == 0)
                        {
                            pendiente = 0.001;
                        }
                    }
                    else
                    {
                        pendiente = 1;
                    }
                    double rotu;
                    if (!string.IsNullOrEmpty(textBox_rot.Text))
                    {
                        rotu = double.Parse(textBox_rot.Text);
                    }
                    else
                    {
                        rotu = 100;
                    }

                    calculoPolilineaPerfil.RellenarPerfil(v_ac, n_suavizados);
                    calculoPolilineaPerfil.DividirSentidos();
                    calculoPolilineaPerfil.QuitarSuavizado();
                    progressBar1.Value++;
                    //calculoPolilineaPerfil.MatrizAcuerdo();
                    //calculoPolilineaPerfil.MatrizAcuerdo2();
                    calculoPolilineaPerfil.MatrizAcuerdo3();


                    /*
                     * para visualizar tablas de puntos y parabolas
                     */
                    /*PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(calculoPolilineaPerfil.Polilinea_Perfil);
                    polilineaInfoPanel.Show();*/
                    /*PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilineaPerfil.Lista_Parabolas);
                    polilineaInfoPanel2.Show();*/

                    calculoPolilineaPerfil.Quitar_Acuerdos(distancia, separacion, pendiente);
                    try
                    {
                        calculoPolilineaPerfil.PuntoInflexion();
                        //calculoPolilineaPerfil.Fusion_Acuerdos();

                        progressBar1.Value++;
                        //               calculoPolilineaPerfil.Dibujar_Acuerdos(1);
                        calculoPolilineaPerfil.CalcularEntreParabolas();
                        calculoPolilineaPerfil.CalculoEntreParabolas_Dibujar();

                        calculoPolilineaPerfil.Componente_Inicial();
                        calculoPolilineaPerfil.Componente_Final();
                        //calculoPolilineaPerfil.Dibujar_Rectas(2);
                        //calculoPolilineaPerfil.Dibujar_Acuerdos(2);
                        calculoPolilineaPerfil.Acuerdo_Entre_Pendientes();
                        //calculoPolilineaPerfil.Dibujar_Rectas(3);
                        //calculoPolilineaPerfil.Dibujar_Acuerdos(3);
                        calculoPolilineaPerfil.Acerdo_Curva();
                        progressBar1.Value++;
                        // calculoPolilineaPerfil.Dibujar_Rectas(3);
                        // calculoPolilineaPerfil.Dibujar_Acuerdos(3);

                        /*
                         * Exportador LANDXML
                         */
                        //calculoPolilineaPerfil.ExportarLandXML(calculoPolilinea.Mcomponenetes);
                        calculoPolilineaPerfil.ExportarLandXML(calculoPolilinea.Mcomponenetes,false);
                        /*
                         * 
                         */

                        calculoPolilineaPerfil.CrearTrazado_Circular();
                        progressBar1.Value++;
                        calculoPolilineaPerfil.Rotular_Circular(rotu);

                        calculoPolilineaPerfil.Informe_Circular();
                        GuardarPerfilAutomatico(calculoPolilineaPerfil);
                        progressBar1.Value++;
                        //calculoPolilineaPerfil = null;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 1;
                        materialFlatButton24.Visible = false;
                        materialFlatButton34.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("No se crea ningun acuerdo. Cambie los parametros y comience de nuevo.");
                        calculoPolilineaPerfil = null;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 0;
                    }
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                    MessageBox.Show("No ha cargado la polilínea 3d en el programa.");

                }
            }
        }

        private void materialFlatButton23_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                /*if (calculoPolilineaPerfil != null)
                {
                    if (calculoPolilineaPerfil.iniciado)
                    {
                        calculoPolilineaPerfil = null;
                    }
                }*/
                calculoPolilineaPerfil.VaciarCalculopolilineaPerfil();
                Ocultar_Modificadores();
                conta_apartado_perfil = 1;
                progressBar1.Style = ProgressBarStyle.Blocks;
                if (calculoPolilineaPerfil != null)
                {
                    calculoPolilineaPerfil.iniciado = true;
                    Perfil_Automatico_Circular();
                    materialFlatButton24.Visible = false;
                    materialFlatButton34.Visible = false;
                    //calculoPolilineaPerfil = null;
                }
                else
                {
                    progressBar1.Value = 0;
                    MessageBox.Show("No ha cargado la polilínea 3d en el programa.");
                }
            }
        }

        private void materialFlatButton24_Click(object sender, EventArgs e)
        {

        }

        private void materialFlatButton25_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                calculoPolilinea.Dibujar_entidades_finales_curvas();
                if (calculoPolilinea.curva_antigua != null)
                {
                    materialFlatButton25.Visible = false;
                    materialFlatButton26.Visible = false;
                    materialFlatButton29.Visible = true;
                    materialFlatButton28.Visible = false;
                    materialFlatButton30.Visible = false;
                }
            }


        }

        private void materialFlatButton26_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                calculoPolilinea.Dibujar_entidades_finales_rectas();
                if (calculoPolilinea.recta_antigua != null)
                {
                    materialFlatButton25.Visible = false;
                    materialFlatButton26.Visible = false;
                    materialFlatButton29.Visible = false;
                    materialFlatButton30.Visible = true;
                }
            }


        }

        private void materialFlatButton27_Click(object sender, EventArgs e)
        {
            try
            {
                calculoPolilinea.Eliminar_Capas(documento);
                calculoPolilinea.Recalcular_componentes(componentes_automaticas);
                calculoPolilinea.Dibujar_entidades(conta_apartado, documento);
                calculoPolilinea.Comprobar_componentes_modificacion();
                materialFlatButton25.Visible = true;
                materialFlatButton26.Visible = true;
                materialFlatButton29.Visible = false;
                materialFlatButton30.Visible = false;

            }
            catch
            {
                MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
            }
            materialFlatButton27.Visible = false;

        }

        private void materialFlatButton24_Click_1(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                if (calculoPolilineaPerfil != null)
                {
                    try
                    {
                        calculoPolilineaPerfil.Eliminar_Capas();
                        calculoPolilineaPerfil.Dibujar_Componentes_Circulares(conta_apartado_perfil);
                        materialFlatButton24.Visible = false;
                        materialFlatButton28.Visible = true;
                        materialFlatButton31.Visible = true;
                    }
                    catch
                    {
                        MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                    }
                }
            }

        }

        private void materialFlatButton28_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                double rotu;
                if (!string.IsNullOrEmpty(textBox_rot.Text))
                {
                    rotu = double.Parse(textBox_rot.Text);
                }
                else
                {
                    rotu = 100;
                }
                if (calculoPolilineaPerfil != null)
                {
                    try
                    {
                        calculoPolilineaPerfil.Dibujar_entidades_finales_curvas();
                        calculoPolilineaPerfil.Acerdo_Curva_modificada();

                        calculoPolilineaPerfil.CrearTrazado_Circular(conta_apartado_perfil);

                        calculoPolilineaPerfil.Rotular_Circular(rotu, conta_apartado_perfil);

                        calculoPolilineaPerfil.Informe_Circular();
                        GuardarPerfilAutomatico(calculoPolilineaPerfil);
                        calculoPolilineaPerfil.Eliminar_Componentes();
                        conta_apartado_perfil++;
                        materialFlatButton28.Visible = false;
                        materialFlatButton24.Visible = false;
                        materialFlatButton31.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                    }
                }
            }

        }

        private void materialFlatButton29_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                try
                {
                    calculoPolilinea.Dibujar_entidades_finales_curvas_paso2();
                    calculoPolilinea.Comprobar_componentes_modificacion();
                    calculoPolilinea.Mcomponenetes = new List<EjeDeTrazado.componentes.Componente>();
                    //string jsonContent = File.ReadAllText("C:\\Users\\Juanma\\Desktop\\creacion de componentes\\json 1.txt");
                    calculoPolilinea.Crear_Trazado_Modificado(this.calculoPolilineaPreferencias.Gran_r);
                    calculoPolilinea.DibujarTrazado(calculoPolilinea.Mcomponenetes, conta_apartado, documento);
                    calculoPolilinea.Rotulado_final(calculoPolilinea.Mcomponenetes, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, conta_apartado, documento);
                    calculoPolilinea.Desactivar_Capas_Componentes(documento);
                    materialFlatButton29.Visible = false;
                    conta_apartado++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se puede modificar el trazado con este cambio. Se dibujará el anterior.", "Error en la creación del trazado");
                    calculoPolilinea.Componentes_Correctas();
                    calculoPolilinea.Mcomponenetes = new List<EjeDeTrazado.componentes.Componente>();
                    calculoPolilinea.Crear_Trazado_Modificado(this.calculoPolilineaPreferencias.Gran_r);
                    calculoPolilinea.DibujarTrazado(calculoPolilinea.Mcomponenetes, conta_apartado, documento);
                    calculoPolilinea.Rotulado_final(calculoPolilinea.Mcomponenetes, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, conta_apartado, documento);
                    calculoPolilinea.Desactivar_Capas_Componentes(documento);
                    conta_apartado++;
                }
                materialFlatButton29.Visible = false;
                materialFlatButton27.Visible = true;
            }

        }

        private void materialFlatButton30_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {

                try
                {
                    calculoPolilinea.Dibujar_entidades_finales_rectas_paso2();
                    calculoPolilinea.Mcomponenetes = new List<EjeDeTrazado.componentes.Componente>();
                    calculoPolilinea.Crear_Trazado_Modificado(this.calculoPolilineaPreferencias.Gran_r);
                    calculoPolilinea.DibujarTrazado(calculoPolilinea.Mcomponenetes, conta_apartado, documento);
                    calculoPolilinea.Rotulado_final(calculoPolilinea.Mcomponenetes, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, conta_apartado, documento);
                    calculoPolilinea.Desactivar_Capas_Componentes(documento);
                    conta_apartado++;
                }
                catch
                {

                    MessageBox.Show("No se puede modificar el trazado con este cambio. Se dibujará el anterior.", "Error en la creación del trazado");
                    calculoPolilinea.Componentes_Correctas();
                    calculoPolilinea.Mcomponenetes = new List<EjeDeTrazado.componentes.Componente>();
                    calculoPolilinea.Crear_Trazado_Modificado(this.calculoPolilineaPreferencias.Gran_r);
                    calculoPolilinea.DibujarTrazado(calculoPolilinea.Mcomponenetes, conta_apartado, documento);
                    calculoPolilinea.Rotulado_final(calculoPolilinea.Mcomponenetes, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, conta_apartado, documento);
                    calculoPolilinea.Desactivar_Capas_Componentes(documento);
                    conta_apartado++;
                }
                materialFlatButton30.Visible = false;
                materialFlatButton27.Visible = true;
            }

        }

        private void materialFlatButton31_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                if (calculoPolilineaPerfil != null)
                {
                    try
                    {
                        calculoPolilineaPerfil.Dibujar_entidades_finales_rectas();
                        if (calculoPolilineaPerfil.pendiente_antigua != null)
                        {
                            materialFlatButton28.Visible = false;
                            materialFlatButton31.Visible = false;
                            materialFlatButton32.Visible = true;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                    }
                }
            }

        }

        private void materialFlatButton32_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                double rotu;
                if (!string.IsNullOrEmpty(textBox_rot.Text))
                {
                    rotu = double.Parse(textBox_rot.Text);
                }
                else
                {
                    rotu = 100;
                }
                if (calculoPolilineaPerfil != null)
                {
                    try
                    {
                        calculoPolilineaPerfil.Dibujar_entidades_finales_rectas_paso2_circular();
                        calculoPolilineaPerfil.Acerdo_Curva_modificada();

                        calculoPolilineaPerfil.CrearTrazado_Circular(conta_apartado_perfil);

                        calculoPolilineaPerfil.Rotular_Circular(rotu, conta_apartado_perfil);

                        calculoPolilineaPerfil.Informe_Circular();
                        GuardarPerfilAutomatico(calculoPolilineaPerfil);
                        calculoPolilineaPerfil.Eliminar_Componentes();
                        conta_apartado_perfil++;
                        materialFlatButton28.Visible = false;
                        materialFlatButton24.Visible = false;
                        materialFlatButton31.Visible = false;
                        materialFlatButton32.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                    }

                }
            }

        }

        private void materialFlatButton33_Click(object sender, EventArgs e)
        {
            double rotu;
            if (!string.IsNullOrEmpty(textBox_rot.Text))
            {
                rotu = double.Parse(textBox_rot.Text);
            }
            else
            {
                rotu = 100;
            }
            if (calculoPolilineaPerfil != null)
            {
                try
                {
                    calculoPolilineaPerfil.Dibujar_entidades_finales_curvas();
                    calculoPolilineaPerfil.Acerdo_Curva_modificada();

                    calculoPolilineaPerfil.CrearTrazado_Circular(conta_apartado_perfil);

                    calculoPolilineaPerfil.Rotular_Circular(rotu, conta_apartado_perfil);

                    calculoPolilineaPerfil.Informe_Circular();
                    GuardarPerfilAutomatico(calculoPolilineaPerfil);
                    calculoPolilineaPerfil.Eliminar_Componentes();
                    conta_apartado_perfil++;
                    materialFlatButton28.Visible = false;
                    materialFlatButton24.Visible = false;
                    materialFlatButton31.Visible = false;
                }
                catch
                {
                    MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                }
            }
        }

        private void materialFlatButton34_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                if (calculoPolilineaPerfil != null)
                {
                    try
                    {
                        calculoPolilineaPerfil.Eliminar_Capas();
                        calculoPolilineaPerfil.CrearTrazado_Componentes(conta_apartado_perfil);
                        materialFlatButton35.Visible = true;
                        materialFlatButton36.Visible = true;
                        materialFlatButton34.Visible = false;
                        materialFlatButton37.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                    }
                }
            }


        }

        private void materialFlatButton35_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                materialFlatButton36.Visible = false;
                double rotu;
                if (!string.IsNullOrEmpty(textBox_rot.Text))
                {
                    rotu = double.Parse(textBox_rot.Text);
                }
                else
                {
                    rotu = 100;
                }
                if (calculoPolilineaPerfil != null)
                {
                    try
                    {
                        calculoPolilineaPerfil.Dibujar_entidades_finales_parabola();
                        calculoPolilineaPerfil.CalculoEntreParabolas_Dibujar();

                        calculoPolilineaPerfil.Componente_Inicial();
                        calculoPolilineaPerfil.Componente_Final();

                        //calculoPolilineaPerfil.Acuerdo_Entre_Pendientes();
                        //progressBar1.Value++;
                        //calculoPolilineaPerfil.Dibujar_Rectas(3);
                        //calculoPolilineaPerfil.Dibujar_Acuerdos(3);

                        calculoPolilineaPerfil.CrearTrazado(conta_apartado_perfil);
                        //progressBar1.Value++;
                        calculoPolilineaPerfil.Rotular(rotu, conta_apartado_perfil);

                        calculoPolilineaPerfil.Informe();
                        GuardarPerfilAutomatico(calculoPolilineaPerfil);
                        calculoPolilineaPerfil.Eliminar_Componentes();
                        conta_apartado_perfil++;
                        materialFlatButton35.Visible = false;
                        materialFlatButton34.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                    }
                }
            }

        }

        private void materialFlatButton36_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                if (calculoPolilineaPerfil != null)
                {
                    try
                    {
                        calculoPolilineaPerfil.Dibujar_entidades_finales_rectas();
                        if (calculoPolilineaPerfil.pendiente_antigua != null)
                        {
                            materialFlatButton35.Visible = false;
                            materialFlatButton37.Visible = true;
                            materialFlatButton36.Visible = false;
                        }

                    }
                    catch
                    {
                        MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                    }
                }
            }


        }

        private void materialFlatButton37_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                materialFlatButton36.Visible = false;
                double rotu;
                if (!string.IsNullOrEmpty(textBox_rot.Text))
                {
                    rotu = double.Parse(textBox_rot.Text);
                }
                else
                {
                    rotu = 100;
                }
                if (calculoPolilineaPerfil != null)
                {
                    try
                    {
                        calculoPolilineaPerfil.Dibujar_entidades_finales_rectas_paso2();
                        calculoPolilineaPerfil.CalculoEntreParabolas_Dibujar();

                        calculoPolilineaPerfil.Componente_Inicial();
                        calculoPolilineaPerfil.Componente_Final();
                        calculoPolilineaPerfil.CrearTrazado(conta_apartado_perfil);
                        //progressBar1.Value++;
                        calculoPolilineaPerfil.Rotular(rotu, conta_apartado_perfil);

                        calculoPolilineaPerfil.Informe();
                        GuardarPerfilAutomatico(calculoPolilineaPerfil);
                        calculoPolilineaPerfil.Eliminar_Componentes();
                        conta_apartado_perfil++;
                        materialFlatButton34.Visible = false;
                        materialFlatButton37.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                    }

                }
            }

        }
        private void Ocultar_Capas(string docu)
        {
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                    List<string> lista = new List<string>();
                    lista.Add(docu);
                    List<string> capas = engCadNet.oLayer.getLayerListContains(lista, true);
                    engCadNet.oLayer.vLayerListActDes(capas, true, false);
                    oCadManager.thisEditor.UpdateScreen();
                    tr.Commit();
                }
            }
        }
        private void Ocultar_Modificadores()
        {
            materialFlatButton24.Visible = false;
            materialFlatButton34.Visible = false;
            materialFlatButton35.Visible = false;
            materialFlatButton28.Visible = false;
            materialFlatButton36.Visible = false;
            materialFlatButton31.Visible = false;
            materialFlatButton37.Visible = false;
            materialFlatButton32.Visible = false;
        }

        private void materialCheckBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox13.Checked)
            {
                textBox11.Enabled = true;
                textBox12.Enabled = true;
                pclusterizacionTextField.Enabled = false;
                nCurvasMaxTextField.Enabled = false;
                textBox7.Enabled = false;
                materialCheckBox7.Enabled = false;
                textBox2.Enabled = false;
                textBox5.Enabled = false;
                materialCheckBox5.Enabled = false;
                materialCheckBox6.Enabled = false;
                materialCheckBox1.Enabled = false;
                materialCheckBox3.Enabled = false;
                materialCheckBox14.Enabled = true;
            }
            else
            {
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                pclusterizacionTextField.Enabled = true;
                nCurvasMaxTextField.Enabled = true;
                textBox7.Enabled = true;
                materialCheckBox7.Enabled = true;
                textBox2.Enabled = true;
                textBox5.Enabled = true;
                materialCheckBox5.Enabled = true;
                materialCheckBox6.Enabled = true;
                materialCheckBox1.Enabled = true;
                materialCheckBox3.Enabled = true;
                materialCheckBox14.Enabled = false;

            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Valor máximo a la que la varianza azimutal se le considera como recta en un trazado ingenieril ", "Información");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Distancia máxima de los puntos en la polilínea. Si es mayor el programa creará puntos hasta que no ocurra. ", "Información");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Duplicará puntos si detecta solo 2 puntos como curva en un tramo de giro y así poder crear la entidad.  NOTA: No se utilizará en la automática  ", "Información");
        }

        private void CrearMateriales_Click(object sender, EventArgs e)
        {
            TrazadoUsuario trazadoUsuario = new TrazadoUsuario();
            
            // Ocultar este formulario (principal)
            this.Hide();

            // Al cerrar TrazadoUsuario, volver a mostrar este formulario
            trazadoUsuario.FormClosed += (s, args) => this.Show();

            trazadoUsuario.Show();

            /*VistaComponentes vistaComponentes = new VistaComponentes();
            vistaComponentes.Show();*/

            /*List<Componente> componentes = new List<Componente>();
            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {
                Editor ed = acDoc2.Editor;

                while (true)
                {
                    PromptEntityOptions opts = new PromptEntityOptions("\nSeleccione una línea o un arco:");
                    opts.SetRejectMessage("\nDebe seleccionar una línea o un arco.");
                    opts.AddAllowedClass(typeof(Line), false);
                    opts.AddAllowedClass(typeof(Arc), false);

                    PromptEntityResult res = ed.GetEntity(opts);
                    if (res.Status == PromptStatus.Cancel)  // Si el usuario presiona ESC, salir del bucle
                        break;
                    Componente c = new Componente();
                    if (res.Status == PromptStatus.OK)
                    {
                        using (Transaction tr = acDoc2.Database.TransactionManager.StartTransaction())
                        {
                            Entity ent = tr.GetObject(res.ObjectId, OpenMode.ForRead) as Entity;

                            if (ent is Line)
                            {
                                Line recta = (Line)ent;
                                c.lista_puntos = null;
                                c.lista_puntos = new List<Punto>();
                                Punto p1 = new Punto(new Point2d(recta.StartPoint.X, recta.StartPoint.Y));
                                Punto p2 = new Punto(new Point2d(recta.EndPoint.X, recta.EndPoint.Y));
                                c.lista_puntos.Add(p1);
                                c.lista_puntos.Add(p2);
                                calculoPolilinea.Rellenar_Recta(c);
                                c.Tipo = 1;
                                componentes.Add(c);
                                ed.WriteMessage("\nSeleccionó una línea.");
                            }
                            else if (ent is Arc)
                            {
                                Arc curva = (Arc)ent;
                                c.radio = curva.Radius;
                                c.xc = curva.Center.X;
                                c.yc = curva.Center.Y;
                                c.lista_puntos = null;
                                c.lista_puntos = new List<Punto>();
                                Punto p1 = new Punto(new Point2d(curva.StartPoint.X, curva.StartPoint.Y));
                                Punto p3 = new Punto(new Point2d(curva.EndPoint.X, curva.EndPoint.Y));
                                Punto p2 = calculoPolilinea.Crear_Curva_2Puntos_Modificada(p1, p3, c.xc, c.yc,c.radio, c);
                                c.lista_puntos.Add(p1);
                                c.lista_puntos.Add(p2);
                                c.lista_puntos.Add(p3);
                                c.Tipo = 2;
                                componentes.Add(c);
                                ed.WriteMessage("\nSeleccionó un arco.");
                            }
                            else
                            {
                                ed.WriteMessage("\nNo es una línea ni un arco.");
                            }

                            tr.Commit();
                        }
                    }
                }
                ed.WriteMessage("\nFinalizado.");
            }
            Crear_Trazado(2500,componentes);*/

        }
        private void materialFlatButton38_Click(object sender, EventArgs e)
        {
            // Crear el cuadro de diálogo para seleccionar el archivo
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecciona un archivo LandXML", // Título de la ventana
                Filter = "Archivos LandXML (*.xml)|*.xml|Todos los archivos (*.*)|*.*", // Filtro para solo mostrar archivos .landxml
                DefaultExt = "xml" // Extensión por defecto
            };

            // Mostrar el cuadro de diálogo y comprobar si se ha seleccionado un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string archivoLandXML = openFileDialog.FileName;

                TrazadoLandXml trazado = new TrazadoLandXml(archivoLandXML);
                this.DibujarTrazado(trazado.Trazado, "Prueba");
                List<EjeDeTrazado.componentes.Componente> trazado_componentes = new List<EjeDeTrazado.componentes.Componente>();
                double pk = 0;
                CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
                foreach (var item in trazado.Trazado)
                {
                    if (item.getTipoComponente() == Logica.Componentes.Componente.tipoComponente.recta)
                    {
                        tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(item.getPointAtDist(item.getPkIni())[0], item.getPointAtDist(item.getPkIni())[1], 0);
                        tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(item.getPointAtDist(item.getPkFin())[0], item.getPointAtDist(item.getPkFin())[1], 0);
                        EjeDeTrazado.componentes.Linea linea = new EjeDeTrazado.componentes.Linea(p1, p2, pk, 0, item.get_azimut());
                        trazado_componentes.Add(linea);
                    }
                    if (item.getTipoComponente() == Logica.Componentes.Componente.tipoComponente.curva)
                    {

                        tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(item.getPointAtDist(item.getPkIni())[0], item.getPointAtDist(item.getPkIni())[1], 0);
                        tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(item.getPointAtDist(item.getPkIni() + item.getLongitud())[0], item.getPointAtDist(item.getPkIni() + item.getLongitud())[1], 0);
                        tadLayShare.puntos.Punto3d centro = new tadLayShare.puntos.Punto3d(item.getCentro().X, item.getCentro().Y, 0);

                        double radio = item.getRadio();
                        string sentido = item.getSentido();
                        if (sentido == "cw")
                        {
                            EjeDeTrazado.componentes.Curva curva = new EjeDeTrazado.componentes.Curva(p1, p2, centro, radio, 0, 0, EjeTrazado.sentidoCurva.Horario);
                            trazado_componentes.Add(curva);
                        }
                        else
                        {
                            EjeDeTrazado.componentes.Curva curva = new EjeDeTrazado.componentes.Curva(p1, p2, centro, radio, 0, 0, EjeTrazado.sentidoCurva.Antihorario);
                            trazado_componentes.Add(curva);
                        }

                    }
                    if (item.getTipoComponente() == Logica.Componentes.Componente.tipoComponente.clotoide)
                    {

                        double radio = item.getRadio();
                        string sentido = item.getSentido();
                        string tipoclotoide = item.getTipoClotoide();
                        if (tipoclotoide == "Entrada")
                        {
                            tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(item.getPointAtDist(item.getPkIni())[0], item.getPointAtDist(item.getPkIni())[1], 0);
                            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(item.getPointAtDist(item.getPkFin())[0], item.getPointAtDist(item.getPkFin())[1], 0);
                            if (sentido == "cw")
                            {
                                EjeDeTrazado.componentes.Clotoide clotoide = new EjeDeTrazado.componentes.Clotoide(p1, p2, radio, 0, EjeTrazado.sentidoCurva.Horario
                                , 0, 0, true, EjeTrazado.tipoClotoide.entrada, item.get_azimut(), false, 0, false, item.getLe(), item.getA());
                                trazado_componentes.Add(clotoide);
                            }
                            else
                            {
                                EjeDeTrazado.componentes.Clotoide clotoide = new EjeDeTrazado.componentes.Clotoide(p1, p2, radio, 0, EjeTrazado.sentidoCurva.Antihorario
                                , 0, 0, true, EjeTrazado.tipoClotoide.entrada, item.get_azimut(), false, 0, false, item.getLe(), item.getA());
                                trazado_componentes.Add(clotoide);
                            }

                        }
                        else
                        {
                            tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(item.getPointAtDist(0)[0], item.getPointAtDist(0)[1], 0);
                            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(item.getPointAtDist(item.getLongitud())[0], item.getPointAtDist(item.getLongitud())[1], 0);

                            if (sentido == "cw")
                            {
                                EjeDeTrazado.componentes.Clotoide clotoide = new EjeDeTrazado.componentes.Clotoide(p1, p2, radio, 0, EjeTrazado.sentidoCurva.Horario
                                , 0, 0, true, EjeTrazado.tipoClotoide.salida, item.get_azimut(), false, 0, false, item.getLe(), item.getA());
                                trazado_componentes.Add(clotoide);
                            }
                            else
                            {
                                EjeDeTrazado.componentes.Clotoide clotoide = new EjeDeTrazado.componentes.Clotoide(p1, p2, radio, 0, EjeTrazado.sentidoCurva.Antihorario
                                                                , 0, 0, true, EjeTrazado.tipoClotoide.salida, item.get_azimut(), false, 0, false, item.getLe(), item.getA());
                                trazado_componentes.Add(clotoide);
                            }

                        }
                    }
                    pk += item.getLongitud();
                }

                calculoPolilinea.Rotulado_final(trazado_componentes, 200, true, "rotulacion");

            }
        }
        public void DibujarTrazado(List<Logica.Componentes.Componente> trazado, string documento)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    double miBombeo = 2;

                    Polyline miEje = new Polyline();
                    int index = 0;
                    foreach (var componente in trazado)
                    {
                        foreach (var componentPoint in componente.getComponentPoints())
                        {
                            miEje.AddVertexAt(index, new Point2d(componentPoint[0], componentPoint[1]), 0, 0, 0);
                            //Escribir_fichero(componentPoint[0]+";"+ componentPoint[1]+";0");
                            index++;
                        }
                        //miEje.AddVertexAt(index, new Point2d(0, 0), 0, 0, 0);//comprobacion
                        //index++;
                    }
                    engCadNet.oLayer.addLayer(documento + "-Trazado", 4, false);
                    miEje.Layer = documento + "-Trazado";

                    btr.AppendEntity(miEje);
                    tr.AddNewlyCreatedDBObject(miEje, true);

                    //oXdata.setXdata(miEje.ObjectId, "tadilEje", miSolucion.idSolucion.ToString());
                    //ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEje, tr, miEjeMem, miEjeTrazadoTadil.GetType().FullName);



                    oCadManager.thisEditor.UpdateScreen();

                    //Info UI
                    //oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);

                    tr.Commit();
                }
            }
        }
        public enum TipoSalida { Longitudinal, Perfil }

        public class SalidaAmilia
        {
            public TipoSalida Tipo { get; set; }
            public string Nombre { get; set; }

            // Campos para eje longitudinal (planta)
            public List<Logica.Componente> Componentes { get; set; }
            public List<EjeDeTrazado.componentes.Componente> MComponentes { get; set; }
            public List<Point2d> Trazado { get; set; }
            public List<Point3d> Lista_original { get; set; }

            // Campos para perfil longitudinal
            public List<ComponentePerfil> ComponentesPerfil { get; set; }
        }
        public static void Guardar_Json(List<Logica.Componente> comp, List<Point3d> lista_original_3d = null, List<EjeDeTrazado.componentes.Componente> mcomponentes = null, List<Point2d> trazado_prueba = null)
        {

        // Pedir nombre del eje
            string nombreEje = InputBox("Introduce el nombre del eje:", "Guardar Eje", "Eje 1");
            if (string.IsNullOrWhiteSpace(nombreEje))
            {
                MessageBox.Show("Debe introducir un nombre para el eje.");
                return;
            }

            MessageBox.Show("Se va a guardar el resultado del eje de trazado");

            // Generar lista Componentes (Logica) a partir de Mcomponenetes (EjeDeTrazado)
            // Solo Tipo 1 (Recta/Linea) y Tipo 2 (Curva)
            List<Logica.Componente> componentesLogica = new List<Logica.Componente>();

            componentesLogica = comp;
           

            // Crear el objeto actual
            SalidaAmilia solucion = new SalidaAmilia()
            {
                Tipo = TipoSalida.Longitudinal,
                Nombre = nombreEje,
                Componentes = componentesLogica,
                MComponentes = mcomponentes,
                Lista_original = lista_original_3d,
                Trazado = trazado_prueba
            };

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Guardar eje de trazado";
                sfd.Filter = "AMILIA Project Files (*.amilia)|*.amilia"; // Cambio de extensión
                sfd.DefaultExt = "amilia";
                sfd.AddExtension = true;
                sfd.OverwritePrompt = false; // Gestionamos nosotros la sobrescritura/añadido
                sfd.RestoreDirectory = true;
                sfd.FilterIndex = 1;
                sfd.FileName = "Resultado.amilia"; // Nombre por defectoº
                sfd.InitialDirectory = !string.IsNullOrEmpty(ruta_principal) ? ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string ruta = sfd.FileName;
                    List<SalidaAmilia> listaSoluciones = new List<SalidaAmilia>();

                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    settings.Converters.Add(new AutoCADPointConverter());

                    // Si el archivo existe, leemos su contenido
                    if (File.Exists(ruta))
                    {
                        try
                        {
                            string jsonExistente = File.ReadAllText(ruta);
                            listaSoluciones = JsonConvert.DeserializeObject<List<SalidaAmilia>>(jsonExistente, settings) ?? new List<SalidaAmilia>();
                        }
                        catch (Exception ex)
                        {
                            // Si falla la lectura (formato incorrecto, etc.), preguntamos si sobrescribir
                            if (MessageBox.Show("El archivo existente no tiene el formato esperado o está dañado. ¿Desea sobrescribirlo?", "Error de lectura", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }
                            listaSoluciones = new List<SalidaAmilia>();
                        }
                    }

                    // Buscar si ya existe un eje con ese nombre y actualizarlo, o añadirlo
                    int index = listaSoluciones.FindIndex(x => x.Nombre == nombreEje);
                    if (index != -1)
                    {
                        if (MessageBox.Show($"Ya existe un eje con el nombre '{nombreEje}'. ¿Desea actualizarlo?", "Eje existente", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            listaSoluciones[index] = solucion;
                        }
                        else
                        {
                            return; // Cancelar guardado
                        }
                    }
                    else
                    {
                        listaSoluciones.Add(solucion);
                    }

                    // Guardar la lista completa
                    var salida = JsonConvert.SerializeObject(listaSoluciones, Formatting.Indented, settings);
                    File.WriteAllText(ruta, salida);
                    MessageBox.Show("Archivo guardado correctamente en: " + ruta, "Éxito");
                }
            }
        }
        
        public static string InputBox(string promptText, string title, string defaultText)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = defaultText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            return dialogResult == DialogResult.OK ? textBox.Text : "";
        }

        private void b_CrearComponentes_Click(object sender, EventArgs e)
        {
            if (calculoPolilineaPerfil == null || calculoPolilineaPerfil.Polilinea==null || calculoPolilineaPerfil.Polilinea.Count()<1)
            {
                MessageBox.Show("-Debe tener una polilinea del perfil cargada");
                return;
            }
            
            // Pass the rotation/text height value to CalculoPolilineaPerfil if possible
            if (!string.IsNullOrEmpty(textBox_rot.Text))
            {
                double rotu = 2.5; 
                if (double.TryParse(textBox_rot.Text, out rotu))
                {
                    calculoPolilineaPerfil.AlturaTexto = rotu;
                }
            }
            else
            {
                calculoPolilineaPerfil.AlturaTexto = 100; // Default from other logic
            }

            TrazadoUsuarioPerfil trazadoUsuarioPerfil = new TrazadoUsuarioPerfil(calculoPolilineaPerfil, _rutaArchivoAmilia);

            // Ocultar este formulario (principal)
            this.Hide();

            // Al cerrar TrazadoUsuario, volver a mostrar este formulario
            trazadoUsuarioPerfil.FormClosed += (s, args) => this.Show();

            trazadoUsuarioPerfil.Show();
        }
        // Estructuras para guardar perfil (copiadas de TrazadoUsuarioPerfil para compatibilidad)
        public class ComponentePerfil
        {
            [Newtonsoft.Json.JsonProperty("Tipo", Order = 1)]
            public string Tipo { get; set; }
            
            [Newtonsoft.Json.JsonProperty("punto entrada", Order = 2)]
            public string PuntoInicial { get; set; }
            
            [Newtonsoft.Json.JsonProperty("punto salida", Order = 3)]
            public string PuntoFinal { get; set; }
            
            [Newtonsoft.Json.JsonProperty("kv", Order = 4, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public double? Kv { get; set; }
            
            [Newtonsoft.Json.JsonProperty("pendiente", Order = 5, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public double? Pendiente { get; set; } // En porcentaje

            [Newtonsoft.Json.JsonProperty("radio", Order = 6, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public double? Radio { get; set; }

            // No serializamos ObjetoOriginal, ya que Trazado formará la curva a partir de los campos básicos
            [Newtonsoft.Json.JsonIgnore] 
            public object ObjetoOriginal { get; set; } 
            
            [Newtonsoft.Json.JsonIgnore]
            public ObjectId EntityId { get; set; }
        }

        // salida_perfil eliminada — usar SalidaAmilia con Tipo = TipoSalida.Perfil

        private void GuardarPerfilAutomatico(CalculoPolilineaPerfil perfil)
        {
            try
            {
                string nombreEje = InputBox("Introduce el nombre del eje:", "Guardar Eje", "Eje 1");
                if (string.IsNullOrWhiteSpace(nombreEje))
                {
                    MessageBox.Show("Debe introducir un nombre para el eje.");
                    return;
                }
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Archivos de Perfil (*.amilia)|*.amilia|Todos los archivos (*.*)|*.*";
                sfd.DefaultExt = "amilia_perfil";
                sfd.FileName = "PerfilAutomatico.amilia";
                sfd.Title = "Guardar Perfil Automático para Edición Manual";
                sfd.InitialDirectory = !string.IsNullOrEmpty(ruta_principal) ? ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    List<ComponentePerfil> listaComponentes = new List<ComponentePerfil>();

                    var todos = new List<dynamic>();

                    // Recopilar todos los elementos (rectas, parábolas y curvas)
                    if (perfil.Lista_Rectas != null)
                    {
                        foreach (var r in perfil.Lista_Rectas)
                        {
                            if (r.Puntos != null && r.Puntos.Count > 0)
                                todos.Add(new { Tipo = "RECTA", Obj = (object)r, X = r.Puntos[0].X });
                        }
                    }
                    if (perfil.Lista_Parabolas != null)
                    {
                        foreach (var p in perfil.Lista_Parabolas)
                        {
                            if (p.polilinea_perfil != null && p.polilinea_perfil.Count > 0)
                                todos.Add(new { Tipo = "PARABOLA", Obj = (object)p, X = p.Polilinea_Perfil[0].p.X });
                        }
                    }
                    if (perfil.Lista_Curvas != null)
                    {
                        foreach (var c in perfil.Lista_Curvas)
                        {
                            if (c.getPuntoEntrada != null)
                                todos.Add(new { Tipo = "CIRCULAR", Obj = (object)c, X = c.getPuntoEntrada.coordenadaX });
                        }
                    }

                    // Ordenar por Punto Inicial (X) ya que es siempre ascendente
                    todos = todos.OrderBy(x => x.X).ToList();
                    
                    // 1. Filtrar duplicados y fusionar elementos consecutivos del mismo tipo
                    var filtrados = new List<dynamic>();
                    for (int i = 0; i < todos.Count; i++)
                    {
                        var item = todos[i];
                        
                        if (filtrados.Count > 0 && item.Tipo == filtrados.Last().Tipo)
                        {
                            var prevItem = filtrados.Last();
                            
                            // FUSIONAR elementos consecutivos del mismo tipo
                            if (item.Tipo == "RECTA")
                            {
                                Logica.Pendiente currRecta = item.Obj as Logica.Pendiente;
                                Logica.Pendiente prevRecta = prevItem.Obj as Logica.Pendiente;
                                if (currRecta != null && prevRecta != null && currRecta.Puntos != null && prevRecta.Puntos != null && currRecta.Puntos.Count > 0 && prevRecta.Puntos.Count > 0)
                                {
                                    // Extendemos la recta previa hasta el final de la actual
                                    var newPuntos = new List<Autodesk.AutoCAD.Geometry.Point2d> { prevRecta.Puntos.First(), currRecta.Puntos.Last() };
                                    prevRecta.Puntos = newPuntos;
                                }
                            }
                            else if (item.Tipo == "PARABOLA")
                            {
                                Logica.Parabola currPar = item.Obj as Logica.Parabola;
                                Logica.Parabola prevPar = prevItem.Obj as Logica.Parabola;
                                if (currPar != null && prevPar != null && currPar.polilinea_perfil != null && prevPar.polilinea_perfil != null &&
                                    currPar.polilinea_perfil.Count > 0 && prevPar.polilinea_perfil.Count > 0)
                                {
                                    // Combinamos los puntos de la parábola actual a la anterior
                                    prevPar.polilinea_perfil.AddRange(currPar.polilinea_perfil);
                                    // Actualizamos el punto de salida
                                    prevPar.puntoSalida = currPar.puntoSalida;
                                }
                            }
                            else if (item.Tipo == "CIRCULAR")
                            {
                                EjeDeTrazado.componentes.Curva currCur = item.Obj as EjeDeTrazado.componentes.Curva;
                                EjeDeTrazado.componentes.Curva prevCur = prevItem.Obj as EjeDeTrazado.componentes.Curva;
                                if (currCur != null && prevCur != null)
                                {
                                    // Actualizar el punto de salida de la curva previa
                                    prevCur.setPuntoSalida = currCur.getPuntoSalida;
                                }
                            }
                            
                            // Al fusionar, omitimos añadir el elemento actual a la lista
                            continue;
                        }
                        
                        filtrados.Add(item);
                    }

                    // 2. Limpiar secuencias extrañas
                    // La secuencia resultante ya está filtrada y fusionada en 'filtrados'.
                    var enSecuencia = filtrados;

                    // 3. Ajustar los puntos de las parábolas y rellenar listaComponentes
                    for (int i = 0; i < enSecuencia.Count; i++)
                    {
                        var item = enSecuencia[i];
                        
                        if (item.Tipo == "PARABOLA" || item.Tipo == "CIRCULAR")
                        {
                            Logica.Pendiente rectaAnterior = null;
                            Logica.Pendiente rectaSiguiente = null;
                            
                            if (i > 0) rectaAnterior = enSecuencia[i - 1].Obj as Logica.Pendiente;
                            if (i < enSecuencia.Count - 1) rectaSiguiente = enSecuencia[i + 1].Obj as Logica.Pendiente;
                            
                            if (item.Tipo == "PARABOLA")
                            {
                                Logica.Parabola par = item.Obj as Logica.Parabola;
                                if (rectaAnterior != null && rectaSiguiente != null && par != null && par.polilinea_perfil != null && rectaAnterior.Puntos.Count > 0 && rectaSiguiente.Puntos.Count > 0)
                                {
                                    var pFinRectaAnt = rectaAnterior.Puntos.Last();
                                    var pIniRectaSig = rectaSiguiente.Puntos.First();
                                    
                                    // Filtrar puntos estrictamente internos
                                    var nuevaPolilinea = par.polilinea_perfil
                                        .Where(pt => pt.p.X > pFinRectaAnt.X && pt.p.X < pIniRectaSig.X)
                                        .ToList();
                                        
                                    // Insertar extremos
                                    nuevaPolilinea.Insert(0, new Logica.PuntoPerfil { p = new Autodesk.AutoCAD.Geometry.Point2d(pFinRectaAnt.X, pFinRectaAnt.Y) });
                                    nuevaPolilinea.Add(new Logica.PuntoPerfil { p = new Autodesk.AutoCAD.Geometry.Point2d(pIniRectaSig.X, pIniRectaSig.Y) });
                                    
                                    par.polilinea_perfil = nuevaPolilinea;
                                }
                            }
                        }

                        ComponentePerfil cp = new ComponentePerfil();
                        cp.Tipo = item.Tipo;

                        if (item.Tipo == "RECTA")
                        {
                            Logica.Pendiente p = item.Obj as Logica.Pendiente;
                            if (p != null)
                            {
                                cp.ObjetoOriginal = item.Obj;
                                
                                // Rellenamos datos planos por si acaso
                                cp.PuntoInicial = p.Puntos[0].ToString(); 
                                cp.PuntoFinal = p.Puntos[p.Puntos.Count-1].ToString();
                                // Pendiente
                                double dx = p.Puntos[1].X - p.Puntos[0].X;
                                double dy = p.Puntos[1].Y - p.Puntos[0].Y;
                                if (dx != 0) cp.Pendiente = (dy / dx) * 100;
                            }
                        }
                        else if (item.Tipo == "PARABOLA")
                        {
                            Logica.Parabola par = item.Obj as Logica.Parabola;
                            if (par != null)
                            {
                                cp.ObjetoOriginal = item.Obj;
                                // Kv based on 'a' coefficient (parabola[0])
                                if (par.parabola != null && par.parabola.Count > 0)
                                {
                                    double a = par.parabola[0];
                                    if (a != 0) cp.Kv = Math.Abs(1.0 / (2.0 * a));
                                }
                                
                                if (par.polilinea_perfil != null && par.polilinea_perfil.Count > 0)
                                {
                                    cp.PuntoInicial = $"{par.polilinea_perfil.First().p.X:F3}, {par.polilinea_perfil.First().p.Y:F3}";
                                    cp.PuntoFinal = $"{par.polilinea_perfil.Last().p.X:F3}, {par.polilinea_perfil.Last().p.Y:F3}";
                                }
                            }
                        }
                        else if (item.Tipo == "CIRCULAR")
                        {
                            EjeDeTrazado.componentes.Curva curva = item.Obj as EjeDeTrazado.componentes.Curva;
                            if (curva != null)
                            {
                                cp.ObjetoOriginal = item.Obj;
                                cp.Radio = curva.getRadio;
                                cp.PuntoInicial = $"{curva.getPuntoEntrada.coordenadaX:F3}, {curva.getPuntoEntrada.coordenadaY:F3}";
                                cp.PuntoFinal = $"{curva.getPuntoSalida.coordenadaX:F3}, {curva.getPuntoSalida.coordenadaY:F3}";
                            }
                        }
                        listaComponentes.Add(cp);
                    }



                    string ruta = sfd.FileName;
                    List<SalidaAmilia> listaSoluciones = new List<SalidaAmilia>();

                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    settings.Converters.Add(new AutoCADPointConverter());

                    // Si el archivo existe, leemos su contenido
                    if (File.Exists(ruta))
                    {
                        try
                        {
                            string jsonExistente = File.ReadAllText(ruta);
                            listaSoluciones = JsonConvert.DeserializeObject<List<SalidaAmilia>>(jsonExistente, settings) ?? new List<SalidaAmilia>();
                        }
                        catch (Exception ex)
                        {
                            // Si falla la lectura (formato incorrecto, etc.), preguntamos si sobrescribir
                            if (MessageBox.Show("El archivo existente no tiene el formato esperado o está dañado. ¿Desea sobrescribirlo?", "Error de lectura", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }
                            listaSoluciones = new List<SalidaAmilia>();
                        }
                    }

                    var solucion=new SalidaAmilia { Tipo = TipoSalida.Perfil, Nombre = nombreEje, ComponentesPerfil = listaComponentes };
                    // Buscar si ya existe un eje con ese nombre y actualizarlo, o añadirlo
                    int index = listaSoluciones.FindIndex(x => x.Nombre == nombreEje);
                    if (index != -1)
                    {
                        if (MessageBox.Show($"Ya existe un eje con el nombre '{nombreEje}'. ¿Desea actualizarlo?", "Eje existente", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            listaSoluciones[index] = solucion;
                        }
                        else
                        {
                            return; // Cancelar guardado
                        }
                    }
                    else
                    {
                        listaSoluciones.Add(solucion);
                    }

                    // Guardar la lista completa
                    var salida = JsonConvert.SerializeObject(listaSoluciones, Formatting.Indented, settings);
                    File.WriteAllText(ruta, salida);
                    MessageBox.Show("Archivo guardado correctamente en: " + ruta, "Éxito");






                    /*

                    if (listaSalidas == null)
                    {
                        List<SalidaAmilia> listaSalida = new List<SalidaAmilia>();
                    }

                    listaSalidas.Add(new SalidaAmilia { Tipo = TipoSalida.Perfil, Nombre = "Perfil Automático", ComponentesPerfil = listaComponentes });

                    JsonSerializerSettings settings = new JsonSerializerSettings 
                    { 
                        Formatting = Formatting.Indented,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    try { settings.Converters.Add(new AutoCADPointConverter()); } catch {}

                    string json = JsonConvert.SerializeObject(listaSalidas, settings);
                    System.IO.File.WriteAllText(sfd.FileName, json);
                    
                    MessageBox.Show("Perfil guardado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el perfil: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Si ya tienes un resultado en planta con esto cargaras la solución de ese trazado para asignarle un perfil a dicha solución", "Información");

        }

        private void button26_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Si no posees una solución en planta o solo quieres trazar un perfil nuevo con este botón podrás cargar un eje nuevo en 3d para calcularlo.", "Información");

        }
        struct nubepuntos
        {
            public List<Punto3d> puntos { get; set; }
            public double miMax { get; set; }
            public double miPendienteMax { get; set; }
            public int ucNodosPorHoja { get; set; }
        }
        private void materialFlatButton39_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de texto (*.amilia_mdt)|*.amilia_mdt|Todos los archivos (*.*)|*.*";
                ofd.Title = "Seleccione el archivo amilia_mdt para cargar nube de puntos";
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string jsonContent = System.IO.File.ReadAllText(ofd.FileName);
                        
                        try
                        {
                            //CargarPuntosASC(2);
                            nube = JsonConvert.DeserializeObject<nubepuntos>(jsonContent);
                            puntos = nube.puntos;
                        }
                        catch (Exception)
                        {
                            puntos = JsonConvert.DeserializeObject<List<Punto3d>>(jsonContent);
                            nube = new nubepuntos
                            {
                                miMax = 1000,
                                miPendienteMax = 200,
                                ucNodosPorHoja = 150,
                                puntos = puntos
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            try
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                
                sw.Start();
                KDTree2D = new KDTree2D(puntos);
                sw.Stop();
                long timeKDTree = sw.ElapsedMilliseconds;

                List<Punto3d> contornoEnvolvente = null;
                if (MessageBox.Show("¿Desea seleccionar una polilínea envolvente cerrada para limitar la triangulación?", "Contorno Envolvente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Document acDoc = Application.DocumentManager.MdiActiveDocument;
                    if (acDoc != null)
                    {
                        Editor ed = acDoc.Editor;
                        PromptEntityOptions peo = new PromptEntityOptions("\nSeleccione la polilínea cerrada que servirá de contorno: ");
                        peo.SetRejectMessage("\nEl objeto seleccionado no es una polilínea.");
                        peo.AddAllowedClass(typeof(Polyline), true);
                        peo.AddAllowedClass(typeof(Polyline2d), true);
                        peo.AddAllowedClass(typeof(Polyline3d), true);

                        // Ocultamos la ventana principal temporalmente para poder seleccionar en CAD
                        this.Hide();
                        PromptEntityResult per = ed.GetEntity(peo);
                        this.Show();

                        if (per.Status == PromptStatus.OK)
                        {
                            using (DocumentLock docLock = acDoc.LockDocument())
                            {
                                using (Transaction tr = acDoc.Database.TransactionManager.StartTransaction())
                                {
                                    Entity ent = tr.GetObject(per.ObjectId, OpenMode.ForRead) as Entity;
                                    contornoEnvolvente = new List<Punto3d>();
                                    
                                    if (ent is Polyline acPoly)
                                    {
                                        double elv = acPoly.Elevation;
                                        for (int i = 0; i < acPoly.NumberOfVertices; i++)
                                        {
                                            Point2d pt2d = acPoly.GetPoint2dAt(i);
                                            contornoEnvolvente.Add(new Punto3d(pt2d.X, pt2d.Y, elv));
                                        }
                                        if (acPoly.Closed && contornoEnvolvente.Count > 0)
                                        {
                                            contornoEnvolvente.Add(contornoEnvolvente.First()); // Ensure physically closed
                                        }
                                    }
                                    else if (ent is Polyline3d acPoly3d)
                                    {
                                        foreach (ObjectId vId in acPoly3d)
                                        {
                                            PolylineVertex3d v3d = tr.GetObject(vId, OpenMode.ForRead) as PolylineVertex3d;
                                            if (v3d != null)
                                            {
                                                contornoEnvolvente.Add(new Punto3d(v3d.Position.X, v3d.Position.Y, v3d.Position.Z));
                                            }
                                        }
                                        if (acPoly3d.Closed && contornoEnvolvente.Count > 0)
                                        {
                                            contornoEnvolvente.Add(contornoEnvolvente.First());
                                        }
                                    }

                                    if (contornoEnvolvente.Count < 3)
                                    {
                                        MessageBox.Show("La polilínea seleccionada no es válida como contorno (tiene menos de 3 vértices). Se ignorará.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        contornoEnvolvente = null;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Contorno capturado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    
                                    tr.Commit();
                                }
                            }
                        }
                    }
                }

                sw.Restart();
                terrainService = new TerrainService(puntos, contornoEnvolvente);
                sw.Stop();
                long timeTerrain = sw.ElapsedMilliseconds;

                //GenerarInformeRendimiento(puntos, KDTree2D, terrainService, timeKDTree, timeTerrain);

                if (MessageBox.Show("Terreno Cargado.\n\n¿Desea pintar la triangulación (TIN) en AutoCAD?", "Pintar Triangulación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Document acDoc = Application.DocumentManager.MdiActiveDocument;
                    if (acDoc != null && terrainService.Triangles != null)
                    {
                        using (DocumentLock docLock = acDoc.LockDocument())
                        {
                            using (Transaction tr = acDoc.Database.TransactionManager.StartTransaction())
                            {
                                BlockTable bt = (BlockTable)tr.GetObject(acDoc.Database.BlockTableId, OpenMode.ForRead);
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                                // Check and create layer "triangulacion_mdt"
                                LayerTable lt = (LayerTable)tr.GetObject(acDoc.Database.LayerTableId, OpenMode.ForRead);
                                if (!lt.Has("triangulacion_mdt"))
                                {
                                    lt.UpgradeOpen();
                                    LayerTableRecord ltrNew = new LayerTableRecord();
                                    ltrNew.Name = "triangulacion_mdt";
                                    // Asignar un color naranja suave (RGB: 255, 178, 102)
                                    ltrNew.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 178, 102);
                                    lt.Add(ltrNew);
                                    tr.AddNewlyCreatedDBObject(ltrNew, true);
                                }

                                for (int i = 0; i < terrainService.Triangles.NumGeometries; i++)
                                {
                                    var coords = terrainService.Triangles.GetGeometryN(i).Coordinates;
                                    if (coords.Length >= 4) // Polígonos triangulares se cierran con 4 coordenadas
                                    {
                                        Point3d p1 = new Point3d(coords[0].X, coords[0].Y, coords[0].Z);
                                        Point3d p2 = new Point3d(coords[1].X, coords[1].Y, coords[1].Z);
                                        Point3d p3 = new Point3d(coords[2].X, coords[2].Y, coords[2].Z);

                                        Face face3d = new Face(p1, p2, p3, p3, true, true, true, true);
                                        face3d.SetDatabaseDefaults();
                                        face3d.Layer = "triangulacion_mdt";
                                        btr.AppendEntity(face3d);
                                        tr.AddNewlyCreatedDBObject(face3d, true);
                                    }
                                }
                                tr.Commit();
                            }
                        }
                        MessageBox.Show("Triangulación generada y pintada correctamente en el espacio modelo.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Terreno Cargado e informe de rendimiento generado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los puntos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerarInformeRendimiento(List<Punto3d> puntos_entrada, KDTree2D kdTree, TerrainService terrain, long timeKDCreation, long timeTerrainCreation)
        {
            if (puntos_entrada == null || puntos_entrada.Count == 0) return;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("=== INFORME DE RENDIMIENTO Y PRECISIÓN ===");
            sb.AppendLine($"Total de puntos en el terreno: {puntos_entrada.Count}\n");

            sb.AppendLine("--- 1. TIEMPO DE CREACIÓN ---");
            sb.AppendLine($"KDTree2D (Construcción árbol): {timeKDCreation} ms");
            sb.AppendLine($"TerrainService (Triangulación Delaunay TIN): {timeTerrainCreation} ms\n");

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            // 2. Tiempo de búsqueda de 1 solución
            double minX = puntos_entrada.Min(p => p.coordenadaX);
            double maxX = puntos_entrada.Max(p => p.coordenadaX);
            double minY = puntos_entrada.Min(p => p.coordenadaY);
            double maxY = puntos_entrada.Max(p => p.coordenadaY);

            double testX = minX + (maxX - minX) / 2.0;
            double testY = minY + (maxY - minY) / 2.0;

            sw.Start();
            double zKd1 = Interpolator.EstimateZ(kdTree, testX, testY);
            sw.Stop();
            long tKd1 = sw.ElapsedTicks;

            sw.Restart();
            double zTin1 = terrain.GetPreciseZ(testX, testY);
            sw.Stop();
            long tTin1 = sw.ElapsedTicks;

            sb.AppendLine("--- 2. TIEMPO DE BÚSQUEDA (1 PUNTO) ---");
            sb.AppendLine($"Punto de prueba (centro): X={testX:F3}, Y={testY:F3}");
            sb.AppendLine($"KDTree2D (Z={zKd1:F3}) -> {tKd1} ticks de procesador");
            sb.AppendLine($"TerrainService (Z={zTin1:F3}) -> {tTin1} ticks de procesador\n");

            // 3. Tiempo de búsqueda de múltiples soluciones
            int numPruebas = 10000;
            Random rnd = new Random(12345);
            List<Autodesk.AutoCAD.Geometry.Point2d> pruebas = new List<Autodesk.AutoCAD.Geometry.Point2d>(numPruebas);
            for(int i = 0; i < numPruebas; i++) {
                pruebas.Add(new Autodesk.AutoCAD.Geometry.Point2d(minX + rnd.NextDouble() * (maxX - minX), minY + rnd.NextDouble() * (maxY - minY)));
            }
            
            int hitKd = 0, hitTin = 0;
            
            sw.Restart();
            for(int i = 0; i < numPruebas; i++) {
                double z = Interpolator.EstimateZ(kdTree, pruebas[i].X, pruebas[i].Y);
                if(!double.IsNaN(z)) hitKd++;
            }
            sw.Stop();
            long tKDMuchos = sw.ElapsedMilliseconds;

            sw.Restart();
            for(int i = 0; i < numPruebas; i++) {
                double z = terrain.GetPreciseZ(pruebas[i].X, pruebas[i].Y);
                if(!double.IsNaN(z)) hitTin++;
            }
            sw.Stop();
            long tTinMuchos = sw.ElapsedMilliseconds;

            sb.AppendLine($"--- 3. TIEMPO DE BÚSQUEDA ({numPruebas} PUNTOS ALEATORIOS) ---");
            sb.AppendLine($"KDTree2D: {tKDMuchos} ms (Puntos con Z válida: {hitKd})");
            sb.AppendLine($"TerrainService: {tTinMuchos} ms (Puntos con Z válida: {hitTin})\n");

            // 4. Precisión en diferentes situaciones
            // a) Punto exacto
            var pExacto = puntos_entrada[puntos_entrada.Count / 2];
            double zKdExact = Interpolator.EstimateZ(kdTree, pExacto.coordenadaX, pExacto.coordenadaY);
            double zTinExact = terrain.GetPreciseZ(pExacto.coordenadaX, pExacto.coordenadaY);
            if (double.IsNaN(zTinExact)) 
            {
                // Un punto exacto a veces escapa intersecciones por precisión decimal, reintento
                zTinExact = terrain.GetPreciseZ(pExacto.coordenadaX + 0.001, pExacto.coordenadaY + 0.001);
            }
            
            sb.AppendLine("--- 4. PRECISIÓN: PUNTO EXACTO EXISTENTE ---");
            sb.AppendLine($"Z Real en MDE: {pExacto.coordenadaZ:F3}");
            sb.AppendLine($"KDTree2D Estimada: {zKdExact:F3} (Desvío de: {Math.Abs(pExacto.coordenadaZ - (double.IsNaN(zKdExact)? 0 : zKdExact)):F4} m)");
            sb.AppendLine($"TerrainService Estimada: {zTinExact:F3} (Desvío de: {Math.Abs(pExacto.coordenadaZ - (double.IsNaN(zTinExact)? 0 : zTinExact)):F4} m)");
            sb.AppendLine("*Nota: IDW (KDTree) suele atenuar puntos exactos, TIN encaja perfecto salvo fallos al estar justo en el borde del triángulo.\n");

            // b) Punto extrapolado (fuera de límites)
            double bordeX = minX - 100.0;
            double bordeY = minY - 100.0;
            double zKdBorde = Interpolator.EstimateZ(kdTree, bordeX, bordeY);
            double zTinBorde = terrain.GetPreciseZ(bordeX, bordeY);

            sb.AppendLine("--- 5. PRECISIÓN: EXTRAPOLACIÓN (FUERA DEL TERRENO) ---");
            sb.AppendLine($"Punto de prueba lejos: X={bordeX:F3}, Y={bordeY:F3}");
            sb.AppendLine($"KDTree2D: {(double.IsNaN(zKdBorde) ? "NaN (Controló distancia máxima)" : $"{zKdBorde:F3} m (Toma vecinos más de borde)")}");
            sb.AppendLine($"TerrainService: {(double.IsNaN(zTinBorde) ? "NaN (Estructuralmente fuera de los triángulos de Delaunay, comportamiento ideal)" : zTinBorde.ToString("F3"))}\n");

            // c) Punto mediano interpolado
            if (puntos_entrada.Count > 1)
            {
                var p1 = puntos_entrada[0];
                var p2 = puntos_entrada[1];
                double medioX = (p1.coordenadaX + p2.coordenadaX) / 2.0;
                double medioY = (p1.coordenadaY + p2.coordenadaY) / 2.0;
                double zRealEstimada = (p1.coordenadaZ + p2.coordenadaZ) / 2.0;
                
                double zKdMedio = Interpolator.EstimateZ(kdTree, medioX, medioY);
                double zTinMedio = terrain.GetPreciseZ(medioX, medioY);

                sb.AppendLine("--- 6. PRECISIÓN: INTERPOLACIÓN ENTRE 2 PUNTOS CONOCIDOS ---");
                sb.AppendLine($"Puntos de referencia combinados Z1: {p1.coordenadaZ:F3}, Z2: {p2.coordenadaZ:F3}");
                sb.AppendLine($"Media aritmética (Z teórica en vacío): {zRealEstimada:F3} m");
                sb.AppendLine($"KDTree2D estimación ponderada: {zKdMedio:F3}");
                sb.AppendLine($"TerrainService cálculo planar: {zTinMedio:F3}");
            }

            // Guardar reporte
            using(SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text files (*.txt)|*.txt";
                sfd.Title = "Guardar Reporte Comparativo Terrenos";
                sfd.FileName = "Informe_Precision_Tiempo.txt";
                sfd.InitialDirectory = !string.IsNullOrEmpty(ruta_principal) ? ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, sb.ToString());
                }
            }
        }

        private void b_CargarPuntosTerreno_Click(object sender, EventArgs e)
        {
            try
            {
                // Asegurar que tenemos un documento activo
                Document acDoc = Application.DocumentManager.MdiActiveDocument;
                Database acCurDb = acDoc.Database;
                Editor ed = acDoc.Editor;

                List<Punto3d> puntosCargados = new List<Punto3d>();

                // Bloqueo del documento y transacción
                using (DocumentLock docLock = acDoc.LockDocument())
                {
                    using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
                    {
                        // Obtener todas las capas
                        LayerTable lt = (LayerTable)tr.GetObject(acCurDb.LayerTableId, OpenMode.ForRead);
                        List<string> layerNames = new List<string>();
                        foreach (ObjectId layerId in lt)
                        {
                            LayerTableRecord ltr = (LayerTableRecord)tr.GetObject(layerId, OpenMode.ForRead);
                            layerNames.Add(ltr.Name);
                        }

                        // Ordenar alfabéticamente
                        layerNames.Sort();

                        // Mostrar el formulario
                        using (FormSeleccionCapas formCapas = new FormSeleccionCapas(layerNames))
                        {
                            if (formCapas.ShowDialog() == DialogResult.OK)
                            {
                                List<string> selectedLayers = formCapas.SelectedLayers;

                                // Abrir la tabla de bloques y el espacio modelo
                                BlockTable bt = (BlockTable)tr.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);

                                // Función local para evitar duplicados si queremos
                                Action<double, double, double> AddPointIfZ = (px, py, pz) =>
                                {
                                    // Consideramos que si tiene una Z (aunque sea 0) la extraemos porque el usuario dijo "si tienen Z"
                                    // Para evitar 0.0 perfectos si solo interesan con Z: (Omitimos filtro de z!=0 para llevar todo el MDT)
                                    puntosCargados.Add(new Punto3d(px, py, pz));
                                };

                                // Iterar por todas las entidades del espacio modelo
                                foreach (ObjectId objId in btr)
                                {
                                    Entity ent = tr.GetObject(objId, OpenMode.ForRead) as Entity;
                                    if (ent != null && selectedLayers.Contains(ent.Layer))
                                    {
                                        if (ent is DBPoint acPoint)
                                        {
                                            AddPointIfZ(acPoint.Position.X, acPoint.Position.Y, acPoint.Position.Z);
                                        }
                                        else if (ent is Line acLine)
                                        {
                                            AddPointIfZ(acLine.StartPoint.X, acLine.StartPoint.Y, acLine.StartPoint.Z);
                                            AddPointIfZ(acLine.EndPoint.X, acLine.EndPoint.Y, acLine.EndPoint.Z);
                                        }
                                        else if (ent is Polyline acPoly)
                                        {
                                            double elv = acPoly.Elevation;
                                            for (int i = 0; i < acPoly.NumberOfVertices; i++)
                                            {
                                                Point2d pt2d = acPoly.GetPoint2dAt(i);
                                                AddPointIfZ(pt2d.X, pt2d.Y, elv);
                                            }
                                        }
                                        else if (ent is Polyline3d acPoly3d)
                                        {
                                            foreach (ObjectId vId in acPoly3d)
                                            {
                                                PolylineVertex3d v3d = tr.GetObject(vId, OpenMode.ForRead) as PolylineVertex3d;
                                                if (v3d != null)
                                                {
                                                    AddPointIfZ(v3d.Position.X, v3d.Position.Y, v3d.Position.Z);
                                                }
                                            }
                                        }
                                        else if (ent is BlockReference acBlockRef)
                                        {
                                            AddPointIfZ(acBlockRef.Position.X, acBlockRef.Position.Y, acBlockRef.Position.Z);
                                        }
                                    }
                                }
                            }
                        }
                        tr.Commit();
                    }
                }

                // Asignar los puntos a la variable global y notificar
                if (puntosCargados.Count > 0)
                {
                    this.puntos = puntosCargados;
                    
                    // Llenar el struct nubepuntos
                    this.nube = new nubepuntos
                    {
                        miMax = 1000,
                        miPendienteMax = 200,
                        ucNodosPorHoja = 150,
                        puntos = puntosCargados
                    };

                    MessageBox.Show($"Se han extraído {puntosCargados.Count} puntos desde AutoCAD. A continuación, selecciona dónde guardar el archivo de la nube de puntos (.json).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Archivos amilia_mdt (*.amilia_mdt)|*.amilia_mdt|Todos los archivos (*.*)|*.*";
                        sfd.Title = "Guardar Nube de Puntos extraída";
                        sfd.FileName = "NubeDePuntosAutoCAD.amilia_mdt";
                        sfd.InitialDirectory = !string.IsNullOrEmpty(ruta_principal) ? ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            string jsonSerialize = JsonConvert.SerializeObject(this.nube, Formatting.Indented);
                            System.IO.File.WriteAllText(sfd.FileName, jsonSerialize);
                            MessageBox.Show("Archivo amilia_mdt guardado y Terreno cargado en memoria correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron puntos (DBPoint) en el dibujo actual de AutoCAD.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar puntos desde AutoCAD: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialFlatButton41_Click(object sender, EventArgs e)
        {
            // Solicitud interactiva de un punto tras la carga
            if (Application.DocumentManager.MdiActiveDocument != null)
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                PromptPointOptions ppo = new PromptPointOptions("\nSeleccione un punto en el terreno para comprobar la cota Z: ");
                PromptPointResult ppr = ed.GetPoint(ppo);

                if (ppr.Status == PromptStatus.OK)
                {
                    double x = ppr.Value.X;
                    double y = ppr.Value.Y;

                    double zKd = Interpolator.EstimateZ(KDTree2D, x, y);
                    double zKd2= Interpolator.EstimateZQuadrants(KDTree2D, x, y);
                    double zTin = terrainService.GetPreciseZ(x, y);

                    string msgKD = double.IsNaN(zKd) ? "Fuera de rango (o no suficientes vecinos)" : zKd.ToString("F3");
                    string msgKD2 = double.IsNaN(zKd2) ? "Fuera de rango (o no suficientes vecinos)" : zKd2.ToString("F3");
                    string msgTIN = double.IsNaN(zTin) ? "Fuera de área triangular (borde)" : zTin.ToString("F3");

                    MessageBox.Show($"Coordenadas picadas:\nX = {x:F3}\nY = {y:F3}\n\n" +
                                    $"Calculadora KDTree2D (IDW):\nZ = {msgKD}\n\n" +
                                    $"Calculadora KDTree2D2 (IDW):\nZ = {msgKD2}\n\n" +
                                    $"Calculadora TerrainService (TIN):\nZ = {msgTIN}",
                                    "Resultados de Interpolación",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void materialCheckBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox15.Checked == true)
            {
                textBox13.Enabled = true;
            }
            else
            {
                textBox13.Enabled = false;
            }
        }

        private void botonPersonalizado1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Con este algoritmo reducimos el número de rectas inferiores al parámetro dado siempre que sea posible.", "Información");
        }
    }
}
