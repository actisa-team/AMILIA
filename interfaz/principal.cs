using System;
using System.Windows.Forms;

namespace interfaz
{
    using Datos;
    using MaterialSkin;
    using MaterialSkin.Controls;
    using Logica;
    public partial class principal : MaterialForm
    {
        public principal()
        {
            InitializeComponent();
            MaterialSkinManager m = MaterialSkinManager.Instance;
            m.AddFormToManage(this);
            m.Theme = MaterialSkinManager.Themes.LIGHT;
            //m.ColorScheme = new ColorScheme(Primary.Green900,Primary.Green700,Primary.Green500,Accent.LightGreen200,TextShade.WHITE);
            postcarga();
        }
        private void postcarga()
        {

            tabPage2.Text = "Filtrar puntos";
            tabPage1.Text = "Estudio previo";
            
            materialCheckBox1.Checked = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox1.Text = "5";
            textBox2.Text = "2";
            materialLabel1.Enabled = false;
            materialLabel2.Enabled = false;

            materialLabel4.Text = "";
            materialLabel5.Text = "";
            materialLabel7.Text = "";

            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;

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
            bool comprueba=true;
            if (numericUpDown1.Value != 0)
            {
                if (numericUpDown1.Value == numericUpDown2.Value || numericUpDown1.Value == numericUpDown3.Value)
                {
                    comprueba= false;
                }
            }
            if(numericUpDown2.Value != 0)
            {
                if (numericUpDown2.Value == numericUpDown3.Value || numericUpDown2.Value == numericUpDown1.Value)
                {
                    comprueba = false;
                }
            }

            if (numericUpDown3.Value != 0)
            {
                if (numericUpDown2.Value == numericUpDown3.Value || numericUpDown3.Value == numericUpDown1.Value)
                {
                    comprueba = false;
                }
            }
            return comprueba;
        }
        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            
            
            if (comprobar())
            {
                int counter = 1;
                string line;
                string miFileOut = string.Empty;
                int opcion = 1;
                double grados = 0;
                double metros = 0;
                double ratio = 0;
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
                    if (materialCheckBox1.Checked == true)
                    {
                        opcion = 1;
                        grados = 0;
                        metros = 0;
                        ratio = 0;
                    }
                    else if (materialCheckBox2.Checked == true)
                    {
                        opcion = 2;
                        grados = 0;
                        metros = 0;
                        ratio = 0;
                    }
                    else if (materialCheckBox3.Checked == true)
                    {
                        opcion = 3;
                        grados = double.Parse(textBox1.Text);
                        metros = double.Parse(textBox2.Text);
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
                    if (materialCheckBox5.Checked == false)
                    {
                        double t_med, t_max,p_cluster,gran_r;
                        int n_curvas;
                        if (!string.IsNullOrEmpty(textBox5.Text))
                        {
                            t_med = double.Parse(textBox5.Text);
                        }
                        else
                        {
                            t_med = 1;
                        }
                        if (!string.IsNullOrEmpty(textBox6.Text))
                        {
                            t_max = double.Parse(textBox6.Text);
                        }
                        else
                        {
                            t_max = 1;
                        }
                        if (!string.IsNullOrEmpty(textBox7.Text))
                        {
                            p_cluster = double.Parse(textBox7.Text);
                        }
                        else
                        {
                            p_cluster = 2;
                        }
                        if (!string.IsNullOrEmpty(textBox8.Text))
                        {
                            gran_r = double.Parse(textBox8.Text);
                        }
                        else
                        {
                            gran_r = 2500;
                        }
                        if (!string.IsNullOrEmpty(textBox9.Text))
                        {
                            n_curvas = int.Parse(textBox9.Text);
                        }
                        else
                        {
                            n_curvas = 2;
                        }
                        Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, it);

                        /*
                         * 
                         * Primero dividimos todos los cambios de giro y comprobamos que la tolerancia media *3
                         * a la recta es menor suavizamos 10 veces ese tramos 
                         * 
                         */
                        calculo.Cambios_Sentido(t_med);
                        calculo.nueva_relacion();
                        calculo.Set_minimos();
                        calculo.Set_grupo();
                        calculo.Set_recta_curva();
                        calculo.mostrardatos();
                        calculo.Entidades_Curvas(t_max, p_cluster);
                        calculo.Recorrido();
                        calculo.Combinacion(t_med,t_max, n_curvas);
                        
                        calculo.Dibujar_entidades(1);
                        calculo.Comprobacion();
                        calculo.Dibujar_entidades(2);

                        /*
                         * Primera parte acabada
                         */

                        //calculo.FiltroDeCurvas();
                        //calculo.RecortarCurvas();
                        //calculo.Dibujar_entidades();

                       // calculo.viabilidad();
                        //calculo.Comprobacion();
                        //calculo.Dibujar_entidades(3);
                        //calculo.ajuste_2();
                        //calculo.ajuste();
                        //calculo.Dibujar_entidades();
                        //calculo.Enlaces(gran_r);
                        //calculo.centro();
                       // calculo.Dibujar_Todo();






                    }
                    else
                    {
                        int dis = (int)numericUpDown1.Value;
                        int rad = (int)numericUpDown2.Value;
                        int gir = (int)numericUpDown3.Value;
                        int[] orden = new int[3];
                        orden[0] = dis;
                        orden[1] = rad;
                        orden[2] = gir;
                        grados = double.Parse(textBox1.Text);
                        metros = double.Parse(textBox2.Text);
                        ratio = grados / metros;


                        Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, orden, it);
                    }
                    
                }
                else
                {
                    
                }
            }
            else
            {
                MessageBox.Show("Error. El orden de los filtros esta repetido");
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
            materialCheckBox1.Checked = true;
            materialCheckBox2.Checked = false;
            materialCheckBox3.Checked = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            materialLabel1.Enabled = false;
            materialLabel2.Enabled = false;
        }
        private void materialCheckBox2_Click(object sender, EventArgs e)
        {
            materialCheckBox1.Checked = false;
            materialCheckBox2.Checked = true;
            materialCheckBox3.Checked = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            materialLabel1.Enabled = false;
            materialLabel2.Enabled = false;
        }
        private void materialCheckBox3_Click(object sender, EventArgs e)
        {
            materialCheckBox1.Checked = false;
            materialCheckBox2.Checked = false;
            materialCheckBox3.Checked = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            materialLabel1.Enabled = true;
            materialLabel2.Enabled = true;

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
                ratio = double.Parse(textBox4.Text)/ double.Parse(textBox3.Text);


                Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio,ref  giro,ref  sentido,ref  gl);
                materialLabel4.Text = string.Concat(giro);
                materialLabel5.Text = string.Concat(sentido);
                materialLabel7.Text = string.Concat(gl); 
            }
            else
            {

            }
        }

        private void materialCheckBox5_Click(object sender, EventArgs e)
        {
            if (materialCheckBox5.Checked==true)
            {
                materialCheckBox1.Enabled = false;
                materialCheckBox2.Enabled = false;
                materialCheckBox3.Enabled = false;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
                numericUpDown3.Enabled = true;
            }
            else
            {
                materialCheckBox1.Enabled = true;
                materialCheckBox2.Enabled = true;
                materialCheckBox3.Enabled = true;
                if (materialCheckBox3.Checked==true)
                {
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                }
                else
                {
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                }
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
            }

            
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
                    if (materialCheckBox1.Checked == true)
                    {
                        opcion = 1;
                        grados = 0;
                        metros = 0;
                        ratio = 0;
                    }
                    else if (materialCheckBox2.Checked == true)
                    {
                        opcion = 2;
                        grados = 0;
                        metros = 0;
                        ratio = 0;
                    }
                    else if (materialCheckBox3.Checked == true)
                    {
                        opcion = 3;
                        grados = double.Parse(textBox1.Text);
                        metros = double.Parse(textBox2.Text);
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
                    if (materialCheckBox5.Checked == false)
                    {
                        double t_med, t_max, p_cluster;
                        if (!string.IsNullOrEmpty(textBox5.Text))
                        {
                            t_med = double.Parse(textBox5.Text);
                        }
                        else
                        {
                            t_med = 0;
                        }
                        if (!string.IsNullOrEmpty(textBox6.Text))
                        {
                            t_max = double.Parse(textBox6.Text);
                        }
                        else
                        {
                            t_max = 1;
                        }
                        if (!string.IsNullOrEmpty(textBox7.Text))
                        {
                            p_cluster = double.Parse(textBox7.Text);
                        }
                        else
                        {
                            p_cluster = 10;
                        }
                    int n_curvas;
                    if (!string.IsNullOrEmpty(textBox9.Text))
                    {
                        n_curvas = int.Parse(textBox9.Text);
                    }
                    else
                    {
                        n_curvas = 2;
                    }
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, it);
                            if (a.Polilinea.Count>0)
                            {
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
                }
                    else
                    {
                        int dis = (int)numericUpDown1.Value;
                        int rad = (int)numericUpDown2.Value;
                        int gir = (int)numericUpDown3.Value;
                        int[] orden = new int[3];
                        orden[0] = dis;
                        orden[1] = rad;
                        orden[2] = gir;
                        grados = double.Parse(textBox1.Text);
                        metros = double.Parse(textBox2.Text);
                        ratio = grados / metros;
                    double t_med, t_max, p_cluster;
                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        t_med = double.Parse(textBox5.Text);
                    }
                    else
                    {
                        t_med = 0;
                    }
                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        t_max = double.Parse(textBox6.Text);
                    }
                    else
                    {
                        t_max = 1;
                    }
                    if (!string.IsNullOrEmpty(textBox7.Text))
                    {
                        p_cluster = double.Parse(textBox7.Text);
                    }
                    else
                    {
                        p_cluster = 10;
                    }
                    int n_curvas;
                    if (!string.IsNullOrEmpty(textBox9.Text))
                    {
                        n_curvas = int.Parse(textBox9.Text);
                    }
                    else
                    {
                        n_curvas = 2;
                    }
                    Logica.CalculoPolilinea calculo = new CalculoPolilinea(ref a, opcion, ratio, orden, it);
                    if (a.Polilinea.Count > 0)
                    {
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
                
            }
                else
                {

                }
            }

        private void materialFlatButton5_Click(object sender, EventArgs e)
        {
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
    }
}
