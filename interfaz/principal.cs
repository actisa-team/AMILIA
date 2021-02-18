using System;
using System.Windows.Forms;

namespace interfaz {
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
        private List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int>> Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int>>();
        private List<Tuple<Polyline, double>> Lista_Resultados_Perfil = new List<Tuple<Polyline, double>>();
        private List<Point2d> lista_original_perfil = new List<Point2d>();
        private List<Tuple<List<Parabola>, List<Pendiente>>> Lista_Entidades_Perfil = new List<Tuple<List<Parabola>, List<Pendiente>>>();
        private List<CalculoPolilineaPerfil> Lista_Polilineas_Perfil = new List<CalculoPolilineaPerfil>();
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
        private int contador_tipo1 = 0;
        private int contador_tipo2 = 0;
        private double grados_union = 2;
        private int[] List_N_C = { 6, 4, 2, 8 };
        private double distancia_menor = 100;
        public bool salir_bucle = false;
        public bool repetir_pregunta = false;
        System.Threading.Thread th2;
        System.Threading.Thread th3;
        int contador_resultados_numero = 0;
        int conta_apartado = 0;
        int conta_apartado_perfil = 0;
        /*
         * Perfil
         */
        private CalculoPolilineaPerfil calculoPolilineaPerfil;
        private CalculoPolilineaPreferencias calculoPolilineaPreferenciasPerfil;
        private List<Point3d> Lista_original_3d = new List<Point3d>();
        private List<Point2d> Lista_original_2d = new List<Point2d>();
        List<Resultados> L_Resultados = new List<Resultados>();
        /// <summary>
        /// es donde inicializa la interfaz
        /// </summary>
        public principal()
        {
            InitializeComponent();
            MaterialSkinManager m = MaterialSkinManager.Instance;
            m.AddFormToManage(this);

            m.Theme = MaterialSkinManager.Themes.LIGHT;

            //m.ColorScheme = new ColorScheme(Primary.Green900,Primary.Green700,Primary.Green500,Accent.LightGreen200,TextShade.WHITE);
            m.ColorScheme = new ColorScheme(Primary.Indigo50, Primary.Indigo700, Primary.Blue900, Accent.Blue400, default);

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


            this.Text = "Aplitop";

            tabPage2.Text = "Filtrar puntos";
            tabPage1.Text = "Estudio previo";
            tabPage3.Text = "Iniciar cálculo en planta";
            tabPage4.Text = "Iniciar cálculo en perfil";
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
        }

        private void principal_ResizeEnd(object sender, EventArgs e)
        {
            materialTabSelector1.Width = this.Size.Width;
            materialTabControl1.Height = this.Size.Height;
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
            miDialogo.Title = "APLITOP" + " | " + "Selecciona un Archivo de Proyecto";
            miDialogo.Filter = "Ditel Project Files (*.txt)|*.txt";
            miDialogo.Multiselect = false;
            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
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
                dsApp.WriteXml("prueba.aplitop");
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
            miDialogo.Title = "APLITOP" + " | " + "Selecciona un Archivo de Proyecto";
            miDialogo.Filter = "Ditel Project Files (*.txt)|*.txt";
            miDialogo.Multiselect = false;
            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
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
                dsApp.WriteXml("prueba.aplitop");
                file.Close();
                return dsApp;
            }
            return null;
        }

        private dsApp CargarArchivoDeProyectoPerfil(CalculoPolilineaPerfil poli)
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

            ca.Eli_Clo = double.Parse(textBox10.Text);
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


                    calculoPolilinea.Cambios_Sentido(calculoPolilineaPreferencias.T_med);
                    //PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);

                    calculoPolilinea.nueva_relacion();
                    calculoPolilinea.Set_minimos();
                    calculoPolilinea.Set_grupo();

                    calculoPolilinea.Set_recta_curva(P_C);
                    int pol = calculoPolilinea.Dividir_Polilinea();
                    //PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);

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

                    //calculoPolilinea.Dibujar_entidades(3);

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
                    /* TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(trazaViabilidadComponentes, this.calculoPolilinea.Componentes, "Trazas viabilidad etapa 2");
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
                    calculoPolilinea.Crear_Trazado_Error(this.calculoPolilineaPreferencias.Gran_r);
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

                        //ComponentesInfoPanel componentesInfoPanel2 = new ComponentesInfoPanel(calculoPolilinea.Componentes);
                        //componentesInfoPanel2.Show();
                        //calculoPolilinea.Dibujar_entidades(4);
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
                            calculoPolilinea.DibujarTrazado(calculoPolilinea.Mcomponenetes);
                            calculoPolilinea.Rotulado_final(calculoPolilinea.Mcomponenetes, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu);
                            this.pasosEjecutados = 3;


                            MessageBox.Show("Revise autocad para ver la salida del algoritmo");
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
                Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int>>();
                if (comprobar())
                {
                    dsApp dsApp = this.abrirArchivoDeProyecto();

                    if (dsApp != null)
                    {
                        //obtener parametros de inicializacion de CalculoPolilineaController
                        this.calculoPolilineaPreferencias = this.obtenerParametrosCalculoPolilinea();

                        if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar);
                        }
                        else
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar);
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

                            calculoPolilinea.Eli_Clo = double.Parse(textBox10.Text);
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

        }

        private void materialFlatButton3_Click(object sender, EventArgs e)
        {
            string miFileOut = string.Empty;
            OpenFileDialog miDialogo = new OpenFileDialog();
            miDialogo.Title = "APLITOP" + " | " + "Selecciona un Archivo de Proyecto";
            miDialogo.Filter = "Ditel Project Files (*.aplitop)|*.aplitop";
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
                miDialogo.Title = "APLITOP" + " | " + "Selecciona un Archivo de Proyecto";
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
                    a.WriteXml("prueba.aplitop");
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
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, ref giro, ref sentido, ref gl, dis, ref p_dis);
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
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, it, 0, 0);

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
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, orden, it, 0, 0);
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
                Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, ref giro, ref sentido, ref gl, 0, ref p_dis);
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
                            this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.It, escala, n_suavizados, calculoPolilineaPreferenciasPerfil.Dis_eliminar);
                        }
                        else
                        {
                            this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.Orden, calculoPolilineaPreferenciasPerfil.It);
                        }

                        this.calculoPolilineaStatusTextView.Visible = true;
                        this.pasosEjecutados = 0;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 1;
                        MessageBox.Show("CalculoPolilinea Perfil inicializado");
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
                if (calculoPolilineaPerfil != null)
                {
                    if (calculoPolilineaPerfil.iniciado)
                    {
                        calculoPolilineaPerfil = null;
                    }
                }
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
                        
                        calculoPolilineaPerfil.CrearTrazado();
                        progressBar1.Value++;
                        calculoPolilineaPerfil.Rotular(rotu);

                        calculoPolilineaPerfil.Informe();
                        progressBar1.Value++;
                        //calculoPolilineaPerfil = null;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 1;
                        materialFlatButton34.Visible = true;
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
                MessageBox.Show("Cargando trazado");
                if (this.calculoPolilinea != null)
                {
                    if (this.calculoPolilinea.Mcomponenetes != null)
                    {
                        calculoPolilineaPerfil.Cotas_Trazado(this.calculoPolilinea.Mcomponenetes, Lista_original_3d);

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
                                        this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.It, escala, n_suavizados, calculoPolilineaPreferenciasPerfil.Dis_eliminar);
                                    }
                                    else
                                    {
                                        this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.Orden, calculoPolilineaPreferenciasPerfil.It);
                                    }

                                    this.calculoPolilineaStatusTextView.Visible = true;
                                    this.pasosEjecutados = 0;
                                    progressBar1.Style = ProgressBarStyle.Blocks;
                                    progressBar1.Maximum = 1;
                                    progressBar1.Value = 1;
                                    MessageBox.Show("CalculoPolilinea Perfil inicializado");
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
            if (contador_tipo1 > 4)
            {
                contador_tipo1 = 0;
                contador_tipo2++;
                if (tipo == 1 && contador_tipo2 == 4 && calculoPolilineaPreferencias.Gr_Rectas == 2)
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
                progressBar1.Maximum = 21;
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

                        if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar);
                        }
                        else
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar);
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

                            calculoPolilinea.Eli_Clo = double.Parse(textBox10.Text);
                            //MessageBox.Show("CalculoPolilinea inicializado");
                            int[] r = calculoPolilinea.Propiedades(dsApp.Polilinea.Count() - 1, dsApp);
                            tipoA = r[0];
                            tipoC = r[1];
                            Nuevos_Datos();
                            N_P = Get_N_P(tipoA, tipoC);
                            Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int>>();
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
                            while (contador_tipo2 < 4 && !salir_bucle)
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
                                            th.Abort();
                                            th = new System.Threading.Thread(Detener);
                                            if (!salir_bucle)
                                            {
                                                th.Start();
                                            }
                                            else
                                            {
                                                th.Abort();
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
                                        progressBar1.Maximum = 42;
                                    }
                                    materialLabel47.Text = "Soluciones: " + Lista_Resultados.Count();
                                    materialLabel47.Update();
                                    progressBar1.Value++;
                                    th2 = new System.Threading.Thread(App);
                                    th2.Start();
                                }
                            }
                            th2.Interrupt();
                            th2.Abort();
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
                                    calculoPolilinea.DibujarTrazado(Lista_Resultados[dibujar].Item1);
                                    calculoPolilinea.Rotulado_final(Lista_Resultados[dibujar].Item1, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu);
                                    calculoPolilinea.Mcomponenetes = Lista_Resultados[dibujar].Item1;

                                    progressBar1.Value = 20;
                                    progressBar1.Maximum = 21;
                                    progressBar1.Value++;
                                    th.Interrupt();
                                    th.Abort();
                                    MessageBox.Show("Se han encontrado " + resultados + " trazados resultantes. \n\nRevise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hágalo con el cálculo manual y las opciones avanzadas.");
                                    Crear_Informe_Automatico(L_Resultados, dibujar, minimo, medio, Lista_Resultados);
                                    L_Resultados = new List<Resultados>();
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
                                    Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int>>();
                                    lista_resultados_numero = new List<int>();
                                    contador_resultados_numero = 0;
                                    distancia_menor = 100;
                                    th3 = new System.Threading.Thread(App2);
                                    th3.Start();
                                    while (contador_tipo2 < 4 && !salir_bucle)
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
                                                    th.Abort();
                                                    th = new System.Threading.Thread(Detener);
                                                    if (!salir_bucle)
                                                    {
                                                        th.Start();
                                                    }
                                                    else
                                                    {
                                                        th.Abort();
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
                                    th3.Abort();
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
                                            calculoPolilinea.DibujarTrazado(Lista_Resultados[dibujar].Item1);
                                            calculoPolilinea.Rotulado_final(Lista_Resultados[dibujar].Item1, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu);
                                            calculoPolilinea.Mcomponenetes = Lista_Resultados[dibujar].Item1;
                                            progressBar1.Value = 20;
                                            progressBar1.Maximum = 21;
                                            progressBar1.Value++;
                                            th.Interrupt();
                                            th.Abort();
                                            MessageBox.Show("Se han encontrado " + resultados + " trazados resultantes.\n\nRevise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hágalo con el cálculo manual y las opciones avanzadas.");
                                            Crear_Informe_Automatico(L_Resultados, dibujar, minimo, medio, Lista_Resultados);
                                            L_Resultados = new List<Resultados>();
                                        }
                                        else
                                        {
                                            progressBar1.Style = ProgressBarStyle.Blocks;
                                            progressBar1.Value = 0;
                                            progressBar1.Maximum = 1;
                                            th.Interrupt();
                                            th.Abort();
                                            MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                        }

                                    }
                                    else
                                    {
                                        progressBar1.Style = ProgressBarStyle.Blocks;

                                        progressBar1.Value = 0;
                                        progressBar1.Maximum = 1;
                                        th.Interrupt();
                                        th.Abort();
                                        MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                    }
                                }
                                else
                                {
                                    progressBar1.Style = ProgressBarStyle.Blocks;
                                    progressBar1.Value = 0;
                                    progressBar1.Maximum = 1;
                                    th.Interrupt();
                                    th.Abort();
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
                                Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int>>();
                                lista_resultados_numero = new List<int>();
                                contador_resultados_numero = 0;
                                distancia_menor = 100;
                                th3 = new System.Threading.Thread(App2);
                                th3.Start();
                                while (contador_tipo2 < 4 && !salir_bucle)
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
                                                th.Abort();
                                                th = new System.Threading.Thread(Detener);
                                                if (!salir_bucle)
                                                {
                                                    th.Start();
                                                }
                                                else
                                                {
                                                    th.Abort();
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
                                th3.Abort();
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
                                        calculoPolilinea.DibujarTrazado(Lista_Resultados[dibujar].Item1);
                                        calculoPolilinea.Rotulado_final(Lista_Resultados[dibujar].Item1, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu);
                                        calculoPolilinea.Mcomponenetes = Lista_Resultados[dibujar].Item1;
                                        progressBar1.Value = 20;
                                        progressBar1.Maximum = 21;
                                        progressBar1.Value++;
                                        th.Interrupt();
                                        th.Abort();
                                        MessageBox.Show("Se han encontrado " + resultados + " trazados resultantes.\n\nRevise autocad para ver la salida del algoritmo. Si quiere mejorar el resultado hágalo con el cálculo manual y las opciones avanzadas.");
                                        Crear_Informe_Automatico(L_Resultados, dibujar, minimo, medio, Lista_Resultados);
                                        L_Resultados = new List<Resultados>();
                                    }
                                    else
                                    {
                                        progressBar1.Style = ProgressBarStyle.Blocks;
                                        progressBar1.Value = 0;
                                        progressBar1.Maximum = 1;
                                        th.Interrupt();
                                        th.Abort();
                                        MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                    }
                                }
                                else
                                {
                                    progressBar1.Style = ProgressBarStyle.Blocks;
                                    progressBar1.Value = 0;
                                    progressBar1.Maximum = 1;
                                    th.Interrupt();
                                    th.Abort();
                                    MessageBox.Show("No se ha encontrado una solución correcta. Si quiere encontrar un resultado hagalo con el calculo manual y las opciones avanzadas.");
                                }
                            }
                            else
                            {
                                progressBar1.Style = ProgressBarStyle.Blocks;
                                progressBar1.Value = 0;
                                progressBar1.Maximum = 1;
                                th.Interrupt();
                                th.Abort();
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
        }
        private void App()
        {
            ejecutar1ButtonClick_Automatico(calculoPolilineaPreferencias.Suavizado);
            calculoPolilinea.automatico = true;
            calculoPolilinea.error_automatico = false;
            ejecutar2ButtonClick_Automatico();
            ejecutar3ButtonClick_Automatico(contador_resultados_numero);
            contador_resultados_numero++;
        }
        private void App2()
        {
            ejecutar1ButtonClick_Automatico(calculoPolilineaPreferencias.Suavizado);
            calculoPolilinea.automatico = true;
            calculoPolilinea.error_automatico = false;
            ejecutar2ButtonClick_Automatico();
            ejecutar3ButtonClick_Automatico(contador_resultados_numero);
            contador_resultados_numero++;
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
                        th2.Abort();
                    }
                    if (th3 != null)
                    {
                        th3.Interrupt();
                        th3.Abort();
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
                Lista_Entidades_Perfil.Add(Tuple.Create(calculoPolilineaPerfil.Lista_Parabolas, calculoPolilineaPerfil.Lista_Rectas));
                Lista_Polilineas_Perfil.Add(calculoPolilineaPerfil);
                this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(lista_original_perfil, escala, x, y, n_suavizados);
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
                            th.Abort();
                            th = new System.Threading.Thread(Detener);
                            if (!salir_bucle)
                            {
                                th.Start();
                            }
                            else
                            {
                                th.Abort();
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
                calculoPolilineaPerfil.Rotular(rotu);
                progressBar1.Value = 7;
                calculoPolilineaPerfil.Informe();
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
            th.Abort();
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
                Lista_Entidades_Perfil.Add(Tuple.Create(calculoPolilineaPerfil.Lista_Parabolas, calculoPolilineaPerfil.Lista_Rectas));
                Lista_Polilineas_Perfil.Add(calculoPolilineaPerfil);
                this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(lista_original_perfil, escala, x, y, n_suavizados);
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
                            th.Abort();
                            th = new System.Threading.Thread(Detener);
                            if (!salir_bucle)
                            {
                                th.Start();
                            }
                            else
                            {
                                th.Abort();
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
                calculoPolilineaPerfil.Rotular_Circular(rotu);
                progressBar1.Value = 7;
                calculoPolilineaPerfil.Informe_Circular();
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
            th.Abort();
        }
        private void materialFlatButton19_Click(object sender, EventArgs e)
        {
            if (Comprobar_funcion_AUTOCAD())
            {
                if (calculoPolilineaPerfil != null)
                {
                    if (calculoPolilineaPerfil.iniciado)
                    {
                        calculoPolilineaPerfil = null;
                    }
                }
                materialFlatButton24.Visible = false;
                materialFlatButton28.Visible = false;
                conta_apartado_perfil = 1;
                progressBar1.Style = ProgressBarStyle.Blocks;
                if (calculoPolilineaPerfil != null)
                {
                    calculoPolilineaPerfil.iniciado = true;
                    Perfil_Automatico();
                    materialFlatButton34.Visible = true;
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
            double medio, List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int>> Lista_resultados)
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
                string nombre_informe = "";
                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog1.Title = "Guardar informe de los resultados";
                saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.DefaultExt = "csv";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    nombre_informe = saveFileDialog1.FileName;
                    System.IO.TextWriter output = new StreamWriter(nombre_informe, false, Encoding.BigEndianUnicode);
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
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar);
                        }
                        else
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar);
                        }
                        this.calculoPolilineaStatusTextView.Visible = true;
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
                        string mensaje = "El tipo de polilínea es A" + tipoA + " - C" + tipoC + "\n\nSe utilizarán los siguientes valores en el cálculo automático:\n\n" +
                            "Porcentaje de Clusterización:\n" + porcentaje + "\nCurvas máximas utilizadas: 6, 4, 2, 8.";
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
                Lista_Resultados = new List<Tuple<List<EjeDeTrazado.componentes.Componente>, double, double, int>>();
                if (comprobar())
                {
                    dsApp dsApp = this.abrirArchivoDeProyecto();

                    if (dsApp != null)
                    {
                        //obtener parametros de inicializacion de CalculoPolilineaController
                        this.calculoPolilineaPreferencias = this.obtenerParametrosCalculoPolilinea();

                        if (aplicarMultiplesFiltradosCheckBox.Checked == false)
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, true);
                        }
                        else
                        {
                            this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar, true);
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
                if (calculoPolilineaPerfil != null)
                {
                    if (calculoPolilineaPerfil.iniciado)
                    {
                        calculoPolilineaPerfil = null;
                    }
                }
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
                        //calculoPolilineaPerfil.Dibujar_Rectas(3);
                        calculoPolilineaPerfil.CrearTrazado_Circular();
                        progressBar1.Value++;
                        calculoPolilineaPerfil.Rotular_Circular(rotu);

                        calculoPolilineaPerfil.Informe_Circular();
                        progressBar1.Value++;
                        //calculoPolilineaPerfil = null;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 1;
                        materialFlatButton24.Visible = true;
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
                if (calculoPolilineaPerfil != null)
                {
                    if (calculoPolilineaPerfil.iniciado)
                    {
                        calculoPolilineaPerfil = null;
                    }
                }
                conta_apartado_perfil = 1;
                progressBar1.Style = ProgressBarStyle.Blocks;
                if (calculoPolilineaPerfil != null)
                {
                    calculoPolilineaPerfil.iniciado = true;
                    Perfil_Automatico_Circular();
                    materialFlatButton24.Visible = true;
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
            calculoPolilinea.Dibujar_entidades_finales_curvas();

            materialFlatButton25.Visible = false;
            materialFlatButton26.Visible = false;
            materialFlatButton29.Visible = true;
            materialFlatButton28.Visible = false;
            materialFlatButton30.Visible = false;
        }

        private void materialFlatButton26_Click(object sender, EventArgs e)
        {
            calculoPolilinea.Dibujar_entidades_finales_rectas();
            materialFlatButton25.Visible = false;
            materialFlatButton26.Visible = false;
            materialFlatButton29.Visible = false;
            materialFlatButton30.Visible = true;
        }

        private void materialFlatButton27_Click(object sender, EventArgs e)
        {
            try
            {
                calculoPolilinea.Eliminar_Capas();
                calculoPolilinea.Recalcular_componentes();
                calculoPolilinea.Dibujar_entidades(conta_apartado);
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

        }

        private void materialFlatButton24_Click_1(object sender, EventArgs e)
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

        private void materialFlatButton28_Click(object sender, EventArgs e)
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
                    calculoPolilineaPerfil.Eliminar_Componentes();
                    conta_apartado_perfil++;
                    materialFlatButton28.Visible = false;
                    materialFlatButton24.Visible = true;
                    materialFlatButton31.Visible = false;
                }
                catch
                {
                    MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                }
            }
        }

        private void materialFlatButton29_Click(object sender, EventArgs e)
        {
            try
            {
                calculoPolilinea.Dibujar_entidades_finales_curvas_paso2();
                calculoPolilinea.Mcomponenetes = new List<EjeDeTrazado.componentes.Componente>();
                calculoPolilinea.Crear_Trazado_Modificado(this.calculoPolilineaPreferencias.Gran_r);
                calculoPolilinea.DibujarTrazado(calculoPolilinea.Mcomponenetes, conta_apartado);
                calculoPolilinea.Rotulado_final(calculoPolilinea.Mcomponenetes, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, conta_apartado);
                calculoPolilinea.Desactivar_Capas_Componentes();
                materialFlatButton29.Visible = false;
                conta_apartado++;
            }
            catch
            {
                MessageBox.Show("No se puede modificar el trazado con este cambio.", "Error en la creación del trazado");
            }
        }

        private void materialFlatButton30_Click(object sender, EventArgs e)
        {
            try
            {
                calculoPolilinea.Dibujar_entidades_finales_rectas_paso2();
                calculoPolilinea.Mcomponenetes = new List<EjeDeTrazado.componentes.Componente>();
                calculoPolilinea.Crear_Trazado_Modificado(this.calculoPolilineaPreferencias.Gran_r);
                calculoPolilinea.DibujarTrazado(calculoPolilinea.Mcomponenetes);
                calculoPolilinea.Rotulado_final(calculoPolilinea.Mcomponenetes, this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu);
                calculoPolilinea.Desactivar_Capas_Componentes();
                materialFlatButton30.Visible = false;
                conta_apartado++;
            }
            catch
            {
                MessageBox.Show("No se puede modificar el trazado con este cambio.", "Error en la creación del trazado");
            }
        }

        private void materialFlatButton31_Click(object sender, EventArgs e)
        {
            if (calculoPolilineaPerfil != null)
            {
                try
                {
                    calculoPolilineaPerfil.Dibujar_entidades_finales_rectas();
                    materialFlatButton28.Visible = false;
                    materialFlatButton31.Visible = false;
                    materialFlatButton32.Visible = true;
                }
                catch
                {
                    MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                }
            }
        }

        private void materialFlatButton32_Click(object sender, EventArgs e)
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
                    calculoPolilineaPerfil.Eliminar_Componentes();
                    conta_apartado_perfil++;
                    materialFlatButton28.Visible = false;
                    materialFlatButton24.Visible = true;
                    materialFlatButton31.Visible = false;
                    materialFlatButton32.Visible = false;
                }
                catch
                {
                    MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
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
                    calculoPolilineaPerfil.Eliminar_Componentes();
                    conta_apartado_perfil++;
                    materialFlatButton28.Visible = false;
                    materialFlatButton24.Visible = true;
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

        private void materialFlatButton35_Click(object sender, EventArgs e)
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
                    calculoPolilineaPerfil.Eliminar_Componentes();
                    conta_apartado_perfil++;
                    materialFlatButton35.Visible = false;
                    materialFlatButton34.Visible = true;
                }
                catch
                {
                    MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                }
            }
        }

        private void materialFlatButton36_Click(object sender, EventArgs e)
        {
            
            if (calculoPolilineaPerfil != null)
            {
                try
                {
                    materialFlatButton35.Visible = false;
                    materialFlatButton34.Visible = false;
                    calculoPolilineaPerfil.Dibujar_entidades_finales_rectas();
                    materialFlatButton37.Visible = true;

                    materialFlatButton36.Visible = false;
                }
                catch
                {
                    MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                }
            }

        }

        private void materialFlatButton37_Click(object sender, EventArgs e)
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
                    calculoPolilineaPerfil.Eliminar_Componentes();
                    conta_apartado_perfil++;
                    materialFlatButton34.Visible = true;
                    materialFlatButton37.Visible = false;
                }
                catch
                {
                    MessageBox.Show("No se pueden dibujar las componentes del trazado", "Información");
                }

            }
        }
    }
}
