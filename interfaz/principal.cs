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

    public partial class principal : MaterialForm, IViabilidadListener, IViabilidadStatusInfoPanelListener {
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
        /*
         * Perfil
         */
        private CalculoPolilineaPerfil calculoPolilineaPerfil;
        private CalculoPolilineaPreferencias calculoPolilineaPreferenciasPerfil;
        private List<Point3d> Lista_original_3d = new List<Point3d>();
        private List<Point2d> Lista_original_2d = new List<Point2d>();
        
        /// <summary>
        /// es donde inicializa la interfaz
        /// </summary>
        public principal() {
            InitializeComponent();
            MaterialSkinManager m = MaterialSkinManager.Instance;
            m.AddFormToManage(this);

            m.Theme = MaterialSkinManager.Themes.LIGHT;
            
            //m.ColorScheme = new ColorScheme(Primary.Green900,Primary.Green700,Primary.Green500,Accent.LightGreen200,TextShade.WHITE);
            m.ColorScheme = new ColorScheme(Primary.Indigo50, Primary.Indigo700, Primary.Blue900, Accent.Blue400, default);
           
            postcarga();

            if (AplitopProperties.isDevelopment()) {
                this.debugButtonsContainer.Visible = true;
            } else {
                this.debugButtonsContainer.Visible = false;
            }


            //this.ejecutar1Button.Click += new EventHandler(this.ejecutar1ButtonClick);
            this.ejecutar2Button.Click += new EventHandler(this.ejecutar2ButtonClick);
            this.ejecutar3Button.Click += new EventHandler(this.ejecutar3ButtonClick);
        }
        private void postcarga() {

            
            this.Text = "Aplitop";
            
            tabPage2.Text = "Filtrar puntos";
            tabPage1.Text = "Estudio previo";
            tabPage3.Text = "Iniciar cálculo en planta";
            tabPage4.Text = "Iniciar cálculo en perfil";
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
                    if (separadas.Count()>2)
                    {
                        if (counter==1)
                        {
                            Lista_original_3d = new List<Point3d>();
                        }
                        Lista_original_3d.Add(new Point3d(double.Parse(separadas[0]), double.Parse(separadas[1]), double.Parse(separadas[2])));
                    }
                    if (separadas.Count() >1)
                    {
                        if (counter == 1)
                        {
                            Lista_original_2d = new List<Point2d>();
                        }
                        Lista_original_2d.Add(new Point2d(double.Parse(separadas[0]), double.Parse(separadas[1])));
                    }
                    counter++;

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
            double x=0, y=0,x2=0,y2=0;
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
                    if (separadas.Count()>2)
                    {
                        if (counter==1)
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

            dsApp dsApp = new dsApp();
            dsApp.Polilinea.Rows.Add(0, poli.Polilinea3d_Original[0].Z, 1);
            x = poli.Polilinea3d_Original[0].X;
            y = poli.Polilinea3d_Original[0].Y;
            for (int i=1; i< poli.Polilinea3d_Original.Count;i++)
            {
                x2 = poli.Polilinea3d_Original[i].X;
                y2 = poli.Polilinea3d_Original[i].Y;
                distancia = Math.Sqrt(Math.Pow(x2 - x, 2) + Math.Pow(y2 - y, 2));
                d_acumulada += distancia;
                dsApp.Polilinea.Rows.Add(d_acumulada, poli.Polilinea3d_Original[i].Z, i+1);
                x = x2;
                y = y2;
            }
            return dsApp;
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
            int puntos_cluster = 50;
            int it = 2;
            int[] orden = new int[3];
            int solapes = 2000;
            double rotulacion=100;
            bool rotu = false;
            int suavizado = 0;
            bool dividir = false;
            double rectas = 20;
            double curvas = 40;
            bool dividir_curvas=false;
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
                    if (double.Parse(textBox2.Text)>=rectas)
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
            }
            double dis_eliminar = 0;
            if (checkeliminar.Checked)
            {
                dis_eliminar = double.Parse(textBox_eliminar.Text);
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
            caPerfil.Dis_eliminar=dis;
            return caPerfil;
        }
        /// <summary>
        /// Se ejecuta el primer paso para la obtención del trazado
        /// </summary>
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
                    int pol=calculoPolilinea.Dividir_Polilinea();
                    PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilinea.Polilinea);
                    List_polilinea.Add(calculoPolilinea.Polilinea);
                    if (materialCheckBox8.Checked == true)
                    {
                        polilineaInfoPanel2.Show();
                    }
                    for (int i=0;i<=pol;i++)
                    {
                        calculoPolilinea.Seleccionar_Polilinea(i);
                        calculoPolilinea.Entidades_Curvas(calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.P_cluster);
                        calculoPolilinea.Recorrido(calculoPolilineaPreferencias.Dividir_curvas,calculoPolilineaPreferencias.Curvas);
                        calculoPolilinea.Combinacion(calculoPolilineaPreferencias.T_med, calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.N_curvas,
                            calculoPolilineaPreferencias.Puntos_cluster, calculoPolilineaPreferencias.Gran_r,calculoPolilineaPreferencias.Dividir_curvas);
                        calculoPolilinea.Limpiar(i);

                    }
                    
                    calculoPolilinea.Unir_Componentes();
                    calculoPolilinea.Dibujar_entidades(1);

                    calculoPolilinea.Aniadir_Rectar_Radio_Grande();

                    calculoPolilinea.Comprobacion(calculoPolilineaPreferencias.Dividir, calculoPolilineaPreferencias.T_max, calculoPolilineaPreferencias.T_med,calculoPolilineaPreferencias.Rectas);
                    if (calculoPolilineaPreferencias.Dividir_curvas)
                    {
                        calculoPolilinea.Recta_curva_dividida();
                    }
                    calculoPolilinea.Dibujar_entidades(2);

                    VerificacionComponentesStatus verificacionComponentesStatus = calculoPolilinea.obtenerEstadoVerificacionDeComponentes();
                    List_verificacion.Add(verificacionComponentesStatus);
                    aniadir_a_list(calculoPolilinea.Componentes);
                    materialFlatButton7.Visible = true;
                    ComponentesInfoPanel componentesInfoPanel = new ComponentesInfoPanel(calculoPolilinea.Componentes, verificacionComponentesStatus);
                    if (materialCheckBox8.Checked==true)
                    {
                        componentesInfoPanel.Show();
                    }
                    

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

        /// <summary>
        /// Se ejecuta el segundo paso para la obtención del trazado
        /// </summary>
        private void ejecutar2ButtonClick(object sender, EventArgs eventArgs) {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
            if (pasosEjecutados > 0) {
                if (this.calculoPolilinea != null) {
                    this.calculoPolilinea.removeAllViabilidadListener();
                    this.calculoPolilinea.addViabilidadListener(this);

                    List<ViabilidadComponentesStatus> trazaViabilidadComponentes = calculoPolilinea.viabilidad(this.calculoPolilineaPreferencias.Gran_r);

                    calculoPolilinea.Dibujar_entidades(3);

                    this.pasosEjecutados = 2;
                    this.paso2EjecutadoTextView.Visible = true;

                    //mostrar panel de la traza de viabilidad de los componentes
                    TrazaViabilidadInfo tvi = new TrazaViabilidadInfo(trazaViabilidadComponentes, this.calculoPolilinea.Componentes, "Trazas viabilidad etapa 2");
                    if (materialCheckBox9.Checked == true)
                    {
                        tvi.Show();
                    }
                    List_traza.Add(trazaViabilidadComponentes);

                    if (!trazaViabilidadComponentes.Any()) {
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
                   
                } else {
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            } else {
                MessageBox.Show("El paso 1 no se ha ejecutado todavia");
            }
        }
        public void Ejecutar_enlaces()
        {
            DateTime tiempo1 = DateTime.Now;
            Process process = new Process();
            TimeSpan id = Process.GetCurrentProcess().TotalProcessorTime;
            List<ViabilidadComponentesStatus> viabilidadEnlaces=null;
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
                if (viabilidadEnlaces!=null)
                {
                    calculoPolilinea.Dibujar_entidades(4);
                    calculoPolilinea.Crear_Trazado(this.calculoPolilineaPreferencias.Gran_r);
                    calculoPolilinea.Dibujar_Todo(this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, Lista_original_2d);
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
                if (total.TotalSeconds >1800 && total.TotalSeconds < 1900)
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
        private void ejecutar3ButtonClick(object sender, EventArgs eventArgs) {
            this.ejecutarViabilidadSinParar = false;
            this.detenerEnIteracion = false;
            this.iteracion = -1;

            if (this.pasosEjecutados > 1) {
                if (this.calculoPolilinea != null) {
                    List<ViabilidadComponentesStatus> viabilidadEnlaces=null;
                    try { 
                    viabilidadEnlaces = calculoPolilinea.Enlaces(this.calculoPolilineaPreferencias.Gran_r, calculoPolilineaPreferencias.Solapes);
                    //th2.Abort();
                    

                        calculoPolilinea.Dibujar_entidades(4);
                        calculoPolilinea.Crear_Trazado(this.calculoPolilineaPreferencias.Gran_r);
                        calculoPolilinea.Dibujar_Todo(this.calculoPolilineaPreferencias.Rotulacion, this.calculoPolilineaPreferencias.Rotu, Lista_original_2d);
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
                    catch {
                        MessageBox.Show("No ha dado una solución correcta. Pruebe con otros parámetros");
                    }
                    finally
                    {
                        //calculoPolilinea.Crear_Trazado_Error(this.calculoPolilineaPreferencias.Gran_r);
                    }

                    
                    
                    
               } else {
                    MessageBox.Show("Calculo polilinea no inicializado");
                }
            } else {
                MessageBox.Show("El paso 2 no se ha ejecutado todavia");
            }
        }

        private void materialFlatButton2_Click(object sender, EventArgs e) {
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

            if (comprobar()) {
                dsApp dsApp = this.abrirArchivoDeProyecto();

                if (dsApp != null) {
                    //obtener parametros de inicializacion de CalculoPolilineaController
                    this.calculoPolilineaPreferencias = this.obtenerParametrosCalculoPolilinea();

                    if (aplicarMultiplesFiltradosCheckBox.Checked == false) {
                        this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado,calculoPolilineaPreferencias.Dis_eliminar);
                    } else {
                        this.calculoPolilinea = new CalculoPolilinea(ref dsApp, calculoPolilineaPreferencias.Opcion, calculoPolilineaPreferencias.Ratio, calculoPolilineaPreferencias.Orden, calculoPolilineaPreferencias.It, calculoPolilineaPreferencias.Suavizado, calculoPolilineaPreferencias.Dis_eliminar);
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
        }
        private void materialCheckBox2_Click(object sender, EventArgs e) {
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
        }
        private void materialCheckBox3_Click(object sender, EventArgs e) {
            filtrado1CheckBox.Checked = false;
            filtrado2CheckBox.Checked = false;
            if (filtrado3CheckBox.Checked == true)
            {
                //filtrado3CheckBox.Checked = false;
                filtrado3GradosTextField.Enabled = true;
                filtrado3MetrosTextField.Enabled = true;
                materialLabel1.Enabled = true;
                materialLabel2.Enabled = true;
                materialCheckBox4.Checked = false;
                materialCheckBox4.Enabled = false;
            }
            else
            {
                //filtrado3CheckBox.Checked = true;
                
                filtrado3GradosTextField.Enabled = false;
                filtrado3MetrosTextField.Enabled = false;
                materialLabel1.Enabled = false;
                materialLabel2.Enabled = false;
                materialCheckBox4.Enabled = true;
            }
                

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
                Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, ref giro, ref sentido, ref gl,dis,ref p_dis);
                materialLabel4.Text = string.Concat(giro);
                materialLabel5.Text = string.Concat(sentido);
                materialLabel7.Text = string.Concat(gl);
                materialLabel41.Text= string.Concat(p_dis);
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
        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hay que elegir el orden en el que se desea hacer la iteracion. Si es 0 no se hará esa iteración", "Información");
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
                    if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text)) {
                        n_curvas = int.Parse(nCurvasMaxTextField.Text);
                    } else {
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
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, it,0,0);
                    
                    if (a.Polilinea.Count > 0) {
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
                } else {
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
                    if (!string.IsNullOrEmpty(nCurvasMaxTextField.Text)) {
                        n_curvas = int.Parse(nCurvasMaxTextField.Text);
                    } else {
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
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, orden, it,0,0);
                    if (a.Polilinea.Count > 0) {
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

            }    else {

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
            Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, ref giro, ref sentido, ref gl,0,ref p_dis);
            materialLabel4.Text = string.Concat(giro);
            materialLabel5.Text = string.Concat(sentido);
            materialLabel7.Text = string.Concat(gl);

        }

        private void tabPage2_Click(object sender, EventArgs e) {

        }

        private void materialFlatButton6_Click(object sender, EventArgs e) {

        }

        public void onNewViabilidadStatus(String etapa, ViabilidadComponentesStatus viabilidadComponentesStatus, List<Componente> componentes, int whileItIndex) {
            //si no se ha activado ejecutarViabilidadSinParar o si se ha activado detenerEnIteracion y la iteracion del while coincideC
            if (!this.ejecutarViabilidadSinParar || (this.detenerEnIteracion && whileItIndex == this.iteracion)) {
                ViabilidadComponentesStatusInfoPanel viabilidadComponentesInfoPanel = new ViabilidadComponentesStatusInfoPanel(viabilidadComponentesStatus, componentes, "Depuración " + etapa + " > iteracion: " + whileItIndex, whileItIndex);
                viabilidadComponentesInfoPanel.addListener(this);
                viabilidadComponentesInfoPanel.ShowDialog(this);
            }
        }

        public void continuarHastaElFinal() {
            this.ejecutarViabilidadSinParar = true;
            this.detenerEnIteracion = false;
            this.iteracion = -1;
        }

        public void continuarHastaLaIteracion(int iteracion) {
            this.detenerEnIteracion = true;
            this.iteracion = iteracion;
            this.ejecutarViabilidadSinParar = true;
        }

        public void continuarPasoAPaso() {
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
                        this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.It, escala, n_suavizados,calculoPolilineaPreferenciasPerfil.Dis_eliminar);
                    }
                    else
                    {
                        this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.Orden, calculoPolilineaPreferenciasPerfil.It);
                    }

                    this.calculoPolilineaStatusTextView.Visible = true;
                    this.pasosEjecutados = 0;

                    MessageBox.Show("CalculoPolilinea Perfil inicializado");
                }
                else
                {
                    MessageBox.Show("Error al abrir el archivo del proyecto");
                }
            }
            else
            {
                MessageBox.Show("Error. El orden de los filtros esta repetido");
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
                        calculoPolilinea.Recorrido(false,40);
                        calculoPolilinea.Combinacion(0.000001, 0.000001, 0, 50, calculoPolilineaPreferencias.Gran_r,false);
                        calculoPolilinea.Limpiar(i);

                    }
                    calculoPolilinea.Unir_Componentes();
                    calculoPolilinea.Dibujar_entidades(1);
                    calculoPolilinea.Comprobacion(false,0.01,0.01,20);
                    calculoPolilinea.Dibujar_entidades(2);

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
        /// <summary>
        /// Ejecución del primer paso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void materialFlatButton12_Click(object sender, EventArgs e)
        {
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
                if (pendiente==0)
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
            calculoPolilineaPerfil.RellenarPerfil(v_ac,n_suavizados);
            calculoPolilineaPerfil.DividirSentidos();
            calculoPolilineaPerfil.QuitarSuavizado();
            //calculoPolilineaPerfil.MatrizAcuerdo();
            //calculoPolilineaPerfil.MatrizAcuerdo2();
            calculoPolilineaPerfil.MatrizAcuerdo3();


            /*
             * para visualizar tablas de puntos y parabolas
             */
            /*PolilineaInfoPanel polilineaInfoPanel = new PolilineaInfoPanel(calculoPolilineaPerfil.Polilinea_Perfil);
            polilineaInfoPanel.Show();
            PolilineaInfoPanel polilineaInfoPanel2 = new PolilineaInfoPanel(calculoPolilineaPerfil.Lista_Parabolas);
            polilineaInfoPanel2.Show();*/
            
                calculoPolilineaPerfil.Quitar_Acuerdos(distancia, separacion, pendiente);
            try
            {
                calculoPolilineaPerfil.PuntoInflexion();
                //calculoPolilineaPerfil.Fusion_Acuerdos();


                calculoPolilineaPerfil.Dibujar_Acuerdos(1);
                calculoPolilineaPerfil.CalcularEntreParabolas();
                calculoPolilineaPerfil.CalculoEntreParabolas_Dibujar();

                calculoPolilineaPerfil.Componente_Inicial();
                calculoPolilineaPerfil.Componente_Final();
                calculoPolilineaPerfil.Dibujar_Rectas(2);
                calculoPolilineaPerfil.Dibujar_Acuerdos(2);
                calculoPolilineaPerfil.Acuerdo_Entre_Pendientes();
                calculoPolilineaPerfil.Dibujar_Rectas(3);
                calculoPolilineaPerfil.Dibujar_Acuerdos(3);
                calculoPolilineaPerfil.CrearTrazado();

                calculoPolilineaPerfil.Rotular(rotu);

                calculoPolilineaPerfil.Informe();
            }
            catch
            {
                MessageBox.Show("No se crea ningun acuerdo. Cambie los parametros y comience de nuevo.");
                calculoPolilineaPerfil = null;
            }
            
        }

        private void materialLabel23_Click(object sender, EventArgs e)
        {

        }

        private void Guardarpolilinea2d(object sender, EventArgs e)
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
            if (this.calculoPolilinea!=null)
            {
                if (this.calculoPolilinea.Mcomponenetes!=null)
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
                                    this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.It, escala, n_suavizados,calculoPolilineaPreferenciasPerfil.Dis_eliminar);
                                }
                                else
                                {
                                    this.calculoPolilineaPerfil = new CalculoPolilineaPerfil(ref dsApp, calculoPolilineaPreferenciasPerfil.Opcion, calculoPolilineaPreferenciasPerfil.Ratio, calculoPolilineaPreferenciasPerfil.Orden, calculoPolilineaPreferenciasPerfil.It);
                                }

                                this.calculoPolilineaStatusTextView.Visible = true;
                                this.pasosEjecutados = 0;

                                MessageBox.Show("CalculoPolilinea Perfil inicializado");
                            }
                            else
                            {
                                MessageBox.Show("Error al abrir el archivo del proyecto");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error. El orden de los filtros esta repetido");
                    }
                }
                else
                {
                    MessageBox.Show("Error. No hay trazado disponible");
                }
                
            }
            else
            {
                MessageBox.Show("Error. No hay trazado disponible");
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
            if (materialCheckBox1.Checked==true)
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
                nCurvasMaxTextField.Text = "1";
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
                nCurvasMaxTextField.Text = "1";
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
        }
    }
}
