using System;
using System.Windows.Forms;

namespace interfaz {
    using Datos;
    using MaterialSkin;
    using MaterialSkin.Controls;
    using Logica;
    using System.Collections.Generic;
    using System.Linq;

    public partial class principal : MaterialForm {
        private CalculoPolilinea calculoPolilinea;
        private CalculoPolilineaPreferencias calculoPolilineaPreferencias;
        private int pasosEjecutados = -1;

        public principal() {
            InitializeComponent();
            MaterialSkinManager m = MaterialSkinManager.Instance;
            m.AddFormToManage(this);
            m.Theme = MaterialSkinManager.Themes.LIGHT;
            //m.ColorScheme = new ColorScheme(Primary.Green900,Primary.Green700,Primary.Green500,Accent.LightGreen200,TextShade.WHITE);
            postcarga();

            if (AplitopProperties.isDevelopment()) {
                this.debugButtonsContainer.Visible = true;
            } else {
                this.debugButtonsContainer.Visible = false;
            }


            this.ejecutar1Button.Click += new EventHandler(this.ejecutar1ButtonClick);
            this.ejecutar2Button.Click += new EventHandler(this.ejecutar2ButtonClick);
            this.ejecutar3Button.Click += new EventHandler(this.ejecutar3ButtonClick);
        }
        private void postcarga() {

            tabPage2.Text = "Filtrar puntos";
            tabPage1.Text = "Estudio previo";

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

        }

        private void principal_ResizeEnd(object sender, EventArgs e) {
            materialTabSelector1.Width = this.Size.Width;
            materialTabControl1.Height = this.Size.Height;
        }

        private void principal_Load(object sender, EventArgs e) {
            materialTabSelector1.Width = this.Size.Width;
            materialTabControl1.Height = this.Size.Height;
        }
        private bool comprobar() {
            bool comprueba = true;
            if (filtrado1ExecuteOrderNumericField.Value != 0) {
                if (filtrado1ExecuteOrderNumericField.Value == filtrado2ExecuteOrderNumericField.Value || filtrado1ExecuteOrderNumericField.Value == filtrado3ExecuteOrderNumericField.Value) {
                    comprueba = false;
                }
            }
            if (filtrado2ExecuteOrderNumericField.Value != 0) {
                if (filtrado2ExecuteOrderNumericField.Value == filtrado3ExecuteOrderNumericField.Value || filtrado2ExecuteOrderNumericField.Value == filtrado1ExecuteOrderNumericField.Value) {
                    comprueba = false;
                }
            }

            if (filtrado3ExecuteOrderNumericField.Value != 0) {
                if (filtrado2ExecuteOrderNumericField.Value == filtrado3ExecuteOrderNumericField.Value || filtrado3ExecuteOrderNumericField.Value == filtrado1ExecuteOrderNumericField.Value) {
                    comprueba = false;
                }
            }
            return comprueba;
        }

        private dsApp abrirArchivoDeProyecto() {
            int counter = 1;
            string miFileOut = string.Empty;
            string line;
            OpenFileDialog miDialogo = new OpenFileDialog();
            miDialogo.Title = "APLITOP" + " | " + "Selecciona un Archivo de Proyecto";
            miDialogo.Filter = "Ditel Project Files (*.txt)|*.txt";
            miDialogo.Multiselect = false;
            if (miDialogo.ShowDialog() == DialogResult.OK) {
                miFileOut = miDialogo.FileName;
                System.IO.StreamReader file = new System.IO.StreamReader(@miFileOut);
                dsApp dsApp = new dsApp();
                while ((line = file.ReadLine()) != null) {
                    string[] separadas;
                    separadas = line.Split(',');
                    dsApp.Polilinea.Rows.Add(separadas[0], separadas[1], counter);
                    counter++;

                }
                dsApp.WriteXml("prueba.aplitop");
                file.Close();
                return dsApp;
            }
            return null;
        }

        private CalculoPolilineaPreferencias obtenerParametrosCalculoPolilinea() {
            int opcion = 1;
            double grados = 0;
            double metros = 0;
            double ratio = 0;
            double t_med = 1;
            double t_max = 1;
            double p_cluster = 2;
            double gran_r = 2500;
            int n_curvas = 2;
            int it = 2;
            int[] orden = new int[3];


            if (filtrado1CheckBox.Checked == true) {
                opcion = 1;
                grados = 0;
                metros = 0;
                ratio = 0;
            } else if (filtrado2CheckBox.Checked == true) {
                opcion = 2;
                grados = 0;
                metros = 0;
                ratio = 0;
            } else if (filtrado3CheckBox.Checked == true) {
                opcion = 3;
                grados = double.Parse(filtrado3GradosTextField.Text);
                metros = double.Parse(filtrado3MetrosTextField.Text);
                ratio = grados / metros;
            }

            if (materialCheckBox4.Checked == true) {
                it = 1;
            } else {
                it = 2;
            }

            if (aplicarMultiplesFiltradosCheckBox.Checked == false) {
                if (!string.IsNullOrEmpty(toleranciaMediaTextField.Text)) {
                    t_med = double.Parse(toleranciaMediaTextField.Text);
                } else {
                    t_med = 1;
                }
                if (!string.IsNullOrEmpty(toleranciaMaximaTextField.Text)) {
                    t_max = double.Parse(toleranciaMaximaTextField.Text);
                } else {
                    t_max = 1;
                }
                if (!string.IsNullOrEmpty(clusterizacionTextField.Text)) {
                    p_cluster = double.Parse(clusterizacionTextField.Text);
                } else {
                    p_cluster = 2;
                }
                if (!string.IsNullOrEmpty(curvaGranRadioTextField.Text)) {
                    gran_r = double.Parse(curvaGranRadioTextField.Text);
                } else {
                    gran_r = 2500;
                }
                if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text)) {
                    n_curvas = int.Parse(nCurvasMaxTextField.Text);
                } else {
                    n_curvas = 2;
                }

                int filtrado1Order = (int)filtrado1ExecuteOrderNumericField.Value;
                int filtrado2Order = (int)filtrado2ExecuteOrderNumericField.Value;
                int filtrado3Order = (int)filtrado3ExecuteOrderNumericField.Value;

                orden[0] = filtrado1Order;
                orden[1] = filtrado2Order;
                orden[2] = filtrado3Order;

                grados = double.Parse(filtrado3GradosTextField.Text);
                metros = double.Parse(filtrado3MetrosTextField.Text);
                ratio = grados / metros;

            }

            CalculoPolilineaPreferencias ca = new CalculoPolilineaPreferencias();
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

            ca.Orden = orden;

            return ca;
        }

        private void ejecutar1ButtonClick(object sender, EventArgs eventArgs) {
            if (this.pasosEjecutados > -1) {
                if (this.calculoPolilinea != null) {
                    /*
                    * 
                    * Primero dividimos todos los cambios de giro y comprobamos que la tolerancia media *3
                    * a la recta es menor suavizamos 10 veces ese tramos 
                    * 
                    */
                    calculoPolilinea.Cambios_Sentido(calculoPolilineaPreferencias.T_med);
                    calculoPolilinea.nueva_relacion();
                    calculoPolilinea.Set_minimos();
                    calculoPolilinea.Set_grupo();
                    calculoPolilinea.Set_recta_curva();

                    calculoPolilinea.mostrardatos();
                    calculoPolilinea.Entidades_Curvas(calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.P_cluster);
                    calculoPolilinea.Recorrido();
                    calculoPolilinea.Combinacion(calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.N_curvas);

                    calculoPolilinea.Dibujar_entidades(1);
                    calculoPolilinea.Comprobacion();
                    calculoPolilinea.Dibujar_entidades(2);

                    this.pasosEjecutados = 1;
                    this.paso1EjecutadoTextView.Visible = true;
                    MessageBox.Show("Revise autocad para ver la salida de la etapa 1 del algoritmo");
                } else {
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            } else {
                MessageBox.Show("Calculo polilinea no preparado");
            }
        }

        private void ejecutar2ButtonClick(object sender, EventArgs eventArgs) {
            if (pasosEjecutados > 0) {
                if (this.calculoPolilinea != null) {
                    List<ViabilidadComponentesStatus> trazaViabilidadComponentes = calculoPolilinea.viabilidad();
                    calculoPolilinea.Dibujar_entidades(3);

                    //mostrar panel de la traza de viabilidad de los componentes
                    TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(trazaViabilidadComponentes, this.calculoPolilinea.Componentes);
                    tvi.Show();

                    this.pasosEjecutados = 2;
                    this.paso2EjecutadoTextView.Visible = true;

                    if (!trazaViabilidadComponentes.Any()) {
                        MessageBox.Show("No se detectan problemas de viabilidad en los componentes o se han resuelto anteriormente");
                    }
                    MessageBox.Show("Revise autocad para ver la salida de la etapa 2 del algoritmo");
                } else {
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            } else {
                MessageBox.Show("El paso 1 no se ha ejecutado todavia");
            }
        }

        private void ejecutar3ButtonClick(object sender, EventArgs eventArgs) {
            if (this.pasosEjecutados > 1) {
                if (this.calculoPolilinea != null) {
                    calculoPolilinea.Enlaces(this.calculoPolilineaPreferencias.Gran_r);
                    try {
                        calculoPolilinea.Dibujar_entidades(4);
                        calculoPolilinea.Crear_Trazado(this.calculoPolilineaPreferencias.Gran_r);
                        calculoPolilinea.Dibujar_Todo();
                    }
                    catch {
                        MessageBox.Show("Se ha detectado un error al crear la entidad. Se dibujará lo creado");
                    }
                    MessageBox.Show("Revise autocad para ver la salida de la etapa 3 del algoritmo");
                } else {
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            } else {
                MessageBox.Show("El paso 2 no se ha ejecutado todavia");
            }
        }

        private void materialFlatButton2_Click(object sender, EventArgs e) {
            //Resetear estado actual
            this.calculoPolilinea = null;
            this.pasosEjecutados = -1;
            this.paso1EjecutadoTextView.Visible = false;
            this.paso2EjecutadoTextView.Visible = false;
            this.paso3EjecutadoTextView.Visible = false;
            this.calculoPolilineaStatusTextView.Visible = false;

            if (comprobar()) {
                dsApp dsApp = this.abrirArchivoDeProyecto();

                if (dsApp != null) {
                    //obtener parametros de inicializacion de CalculoPolilineaController
                    this.calculoPolilineaPreferencias = this.obtenerParametrosCalculoPolilinea();

                    if (aplicarMultiplesFiltradosCheckBox.Checked == false) {
                        this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It);
                    } else {
                        this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It);
                    }

                    this.calculoPolilineaStatusTextView.Visible = true;
                    this.pasosEjecutados = 0;

                    MessageBox.Show("CalculoPolilinea inicializado");
                } else {
                    MessageBox.Show("Error al abrir el archivo del proyecto");
                }
            } else {
                MessageBox.Show("Error. El orden de los filtros esta repetido");
            }
        }

        private void materialFlatButton3_Click(object sender, EventArgs e) {
            string miFileOut = string.Empty;
            OpenFileDialog miDialogo = new OpenFileDialog();
            miDialogo.Title = "APLITOP" + " | " + "Selecciona un Archivo de Proyecto";
            miDialogo.Filter = "Ditel Project Files (*.aplitop)|*.aplitop";
            miDialogo.Multiselect = false;
            if (miDialogo.ShowDialog() == DialogResult.OK) {
                miFileOut = miDialogo.FileName;
                System.IO.StreamReader file = new System.IO.StreamReader(@miFileOut);
                dsApp a = new dsApp();
                a.ReadXml(file);
            } else {

            }
        }

        private void materialCheckBox1_Click(object sender, EventArgs e) {
            filtrado1CheckBox.Checked = true;
            filtrado2CheckBox.Checked = false;
            filtrado3CheckBox.Checked = false;
            filtrado3GradosTextField.Enabled = false;
            filtrado3MetrosTextField.Enabled = false;
            materialLabel1.Enabled = false;
            materialLabel2.Enabled = false;
        }
        private void materialCheckBox2_Click(object sender, EventArgs e) {
            filtrado1CheckBox.Checked = false;
            filtrado2CheckBox.Checked = true;
            filtrado3CheckBox.Checked = false;
            filtrado3GradosTextField.Enabled = false;
            filtrado3MetrosTextField.Enabled = false;
            materialLabel1.Enabled = false;
            materialLabel2.Enabled = false;
        }
        private void materialCheckBox3_Click(object sender, EventArgs e) {
            filtrado1CheckBox.Checked = false;
            filtrado2CheckBox.Checked = false;
            filtrado3CheckBox.Checked = true;
            filtrado3GradosTextField.Enabled = true;
            filtrado3MetrosTextField.Enabled = true;
            materialLabel1.Enabled = true;
            materialLabel2.Enabled = true;

        }
        private void TxtPruebaNumero_KeyPress(object sender, KeyPressEventArgs e) {
            if (Char.IsDigit(e.KeyChar)) {
                e.Handled = false;
            } else if (Char.IsControl(e.KeyChar)) {
                e.Handled = false;
            } else if (Char.IsSeparator(e.KeyChar)) {
                e.Handled = false;
            } else {
                e.Handled = true;
            }
        }

        private void materialFlatButton1_Click(object sender, EventArgs e) {
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
            if (miDialogo.ShowDialog() == DialogResult.OK) {
                miFileOut = miDialogo.FileName;
                System.IO.StreamReader file = new System.IO.StreamReader(@miFileOut);
                dsApp a = new dsApp();
                while ((line = file.ReadLine()) != null) {
                    string[] separadas;
                    separadas = line.Split(',');
                    a.Polilinea.Rows.Add(separadas[0], separadas[1], counter);
                    counter++;

                }
                a.WriteXml("prueba.aplitop");
                file.Close();
                ratio = double.Parse(textBox4.Text) / double.Parse(textBox3.Text);


                Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, ref giro, ref sentido, ref gl);
                materialLabel4.Text = string.Concat(giro);
                materialLabel5.Text = string.Concat(sentido);
                materialLabel7.Text = string.Concat(gl);
            } else {

            }
        }

        private void materialCheckBox5_Click(object sender, EventArgs e) {
            if (aplicarMultiplesFiltradosCheckBox.Checked == true) {
                filtrado1CheckBox.Enabled = false;
                filtrado2CheckBox.Enabled = false;
                filtrado3CheckBox.Enabled = false;
                filtrado3GradosTextField.Enabled = true;
                filtrado3MetrosTextField.Enabled = true;
                filtrado1ExecuteOrderNumericField.Enabled = true;
                filtrado2ExecuteOrderNumericField.Enabled = true;
                filtrado3ExecuteOrderNumericField.Enabled = true;
            } else {
                filtrado1CheckBox.Enabled = true;
                filtrado2CheckBox.Enabled = true;
                filtrado3CheckBox.Enabled = true;
                if (filtrado3CheckBox.Checked == true) {
                    filtrado3GradosTextField.Enabled = true;
                    filtrado3MetrosTextField.Enabled = true;
                } else {
                    filtrado3GradosTextField.Enabled = false;
                    filtrado3MetrosTextField.Enabled = false;
                }
                filtrado1ExecuteOrderNumericField.Enabled = false;
                filtrado2ExecuteOrderNumericField.Enabled = false;
                filtrado3ExecuteOrderNumericField.Enabled = false;
            }


        }

        private void materialRaisedButton1_Click(object sender, EventArgs e) {
            MessageBox.Show("Hay que elegir el orden en el que se desea hacer la iteracion. Si es 0 no se hará esa iteración", "Información");
        }

        private void materialFlatButton4_Click(object sender, EventArgs e) {
            if (comprobar()) {
                int opcion = 1;
                double grados = 0;
                double metros = 0;
                double ratio = 0;
                dsApp a = new dsApp();
                if (filtrado1CheckBox.Checked == true) {
                    opcion = 1;
                    grados = 0;
                    metros = 0;
                    ratio = 0;
                } else if (filtrado2CheckBox.Checked == true) {
                    opcion = 2;
                    grados = 0;
                    metros = 0;
                    ratio = 0;
                } else if (filtrado3CheckBox.Checked == true) {
                    opcion = 3;
                    grados = double.Parse(filtrado3GradosTextField.Text);
                    metros = double.Parse(filtrado3MetrosTextField.Text);
                    ratio = grados / metros;
                }
                int it = 0;
                if (materialCheckBox4.Checked == true) {
                    it = 1;
                } else {
                    it = 2;
                }
                if (aplicarMultiplesFiltradosCheckBox.Checked == false) {
                    double t_med, t_max, p_cluster;
                    if (!string.IsNullOrEmpty(toleranciaMediaTextField.Text)) {
                        t_med = double.Parse(toleranciaMediaTextField.Text);
                    } else {
                        t_med = 0;
                    }
                    if (!string.IsNullOrEmpty(toleranciaMaximaTextField.Text)) {
                        t_max = double.Parse(toleranciaMaximaTextField.Text);
                    } else {
                        t_max = 1;
                    }
                    if (!string.IsNullOrEmpty(clusterizacionTextField.Text)) {
                        p_cluster = double.Parse(clusterizacionTextField.Text);
                    } else {
                        p_cluster = 10;
                    }
                    int n_curvas;
                    if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text)) {
                        n_curvas = int.Parse(nCurvasMaxTextField.Text);
                    } else {
                        n_curvas = 2;
                    }
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, it);
                    if (a.Polilinea.Count > 0) {
                        calculo.Cambios_Sentido(t_med);
                        calculo.nueva_relacion();
                        calculo.Set_minimos();
                        calculo.Set_grupo();
                        calculo.Set_recta_curva();
                        calculo.Entidades_Curvas(t_max, p_cluster);
                        calculo.Recorrido();
                        calculo.Combinacion(t_med, t_max, n_curvas);
                        calculo.Dibujar_entidades(1);
                    }

                    /*
                                        calculo.nueva_relacion();
                                        calculo.Set_minimos();
                                        calculo.Set_grupo();
                                        calculo.Set_recta_curva();
                                        calculo.ajuste();
                                        calculo.centro();*/
                } else {
                    int dis = (int)filtrado1ExecuteOrderNumericField.Value;
                    int rad = (int)filtrado2ExecuteOrderNumericField.Value;
                    int gir = (int)filtrado3ExecuteOrderNumericField.Value;
                    int[] orden = new int[3];
                    orden[0] = dis;
                    orden[1] = rad;
                    orden[2] = gir;
                    grados = double.Parse(filtrado3GradosTextField.Text);
                    metros = double.Parse(filtrado3MetrosTextField.Text);
                    ratio = grados / metros;
                    double t_med, t_max, p_cluster;
                    if (!string.IsNullOrEmpty(toleranciaMediaTextField.Text)) {
                        t_med = double.Parse(toleranciaMediaTextField.Text);
                    } else {
                        t_med = 0;
                    }
                    if (!string.IsNullOrEmpty(toleranciaMaximaTextField.Text)) {
                        t_max = double.Parse(toleranciaMaximaTextField.Text);
                    } else {
                        t_max = 1;
                    }
                    if (!string.IsNullOrEmpty(clusterizacionTextField.Text)) {
                        p_cluster = double.Parse(clusterizacionTextField.Text);
                    } else {
                        p_cluster = 10;
                    }
                    int n_curvas;
                    if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text)) {
                        n_curvas = int.Parse(nCurvasMaxTextField.Text);
                    } else {
                        n_curvas = 2;
                    }
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, orden, it);
                    if (a.Polilinea.Count > 0) {
                        calculo.Cambios_Sentido(t_med);
                        calculo.nueva_relacion();
                        calculo.Set_minimos();
                        calculo.Set_grupo();
                        calculo.Set_recta_curva();
                        calculo.Entidades_Curvas(t_max, p_cluster);
                        calculo.Recorrido();
                        calculo.Combinacion(t_med, t_max, n_curvas);
                    }

                    /*
                    calculo.nueva_relacion();
                    calculo.Set_minimos();
                    calculo.Set_grupo();
                    calculo.Set_recta_curva();
                    calculo.ajuste();
                    calculo.centro();*/
                }

            } else {

            }
        }

        private void materialFlatButton5_Click(object sender, EventArgs e) {
            int opcion = 1;
            double grados = 0;
            double metros = 0;
            double ratio = 0;
            int giro = 0;
            int sentido = 0;
            int gl = 0;
            dsApp a = new dsApp();
            ratio = double.Parse(textBox4.Text) / double.Parse(textBox3.Text);


            Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, ref giro, ref sentido, ref gl);
            materialLabel4.Text = string.Concat(giro);
            materialLabel5.Text = string.Concat(sentido);
            materialLabel7.Text = string.Concat(gl);

        }

        private void tabPage2_Click(object sender, EventArgs e) {

        }

        private void materialFlatButton6_Click(object sender, EventArgs e) {

        }
    }
}
