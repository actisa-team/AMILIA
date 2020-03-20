using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using System.Windows.Forms;

namespace Logica
{
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using tadLayShare.puntos;
    using EjeDeTrazado.puntosDelEje;
    using EjeDeTrazado.componentes;
    using Logica.verificacion;
    public class CalculoPolilineaPerfil
    {
        double ratio,x_ins,y_ins;
        int escala=1;
        List<Punto> polilinea = new List<Punto>();
        List<Punto> polilinea_inicial = new List<Punto>();
        List<PuntoPerfil> polilinea_perfil_inicial = new List<PuntoPerfil>();
        List<PuntoPerfil> polilinea_perfil = new List<PuntoPerfil>();
        List<Parabola> Lista_parabolas = new List<Parabola>();
        List<Pendiente> Lista_rectas = new List<Pendiente>();
        List<double> lista_puntos_rectas = new List<double>();
        List<Point3d> Polilinea3d_original = new List<Point3d>();
        List<List<PuntoPerfil>> lista_sentidos = new List<List<PuntoPerfil>>();
        dsApp datoApp = new dsApp();
        public List<Parabola> Lista_Parabolas { get => Lista_parabolas; set => Lista_parabolas = value; }
        public List<Point3d> Polilinea3d_Original { get => Polilinea3d_original; set => Polilinea3d_original = value; }
        public List<Punto> Polilinea { get => polilinea; set => polilinea = value; }
        public List<PuntoPerfil> Polilinea_Perfil { get => polilinea_perfil; set => polilinea_perfil = value; }
        public CalculoPolilineaPerfil() { 
        }
        public CalculoPolilineaPerfil(ref dsApp a, int opcion, double ratio, int it,int escal,int n_suavizados)
        {
            MessageBox.Show("Eliga en autocad el punto de inserción.");
            Point3d fstCnr = new Point3d();
            GuardarPunto gp = new GuardarPunto(ref fstCnr);
            try
            {
                Insercion(fstCnr.X, fstCnr.Y);
                escala = escal;
                bool dibujar = true;
                if (a.Polilinea.Rows.Count == 0)
                {
                    dibujar = false;
                    //Set_Polilinea3d(ref a);
                }
                //a.Polilinea.Clear();
                int contador = 0;
                this.ratio = ratio;
                if (a.Polilinea.Rows.Count != 0)
                {
                    foreach (DataRow r in a.Polilinea.Rows)
                    {
                        string x = (string)r["X"];
                        string y = (string)r["Y"];
                        //string z = (string)r["Z"];

                        Punto p = new Punto(new Point2d(double.Parse(x), double.Parse(y)));
                        //p.p_perfil = true;
                        //p.pp_perfil = double.Parse(z);
             /*           string x = (string)r["X"];
                        string y = (string)r["Y"];

                        Punto p = new Punto(new Point2d(double.Parse(x), double.Parse(y)));*/
                        polilinea.Add(p);
                        
                    }
                    double y_alt = polilinea[0].p.Y;
                    for (int i=1;i<polilinea.Count;i++)
                    {
                        if (polilinea[0].p.Y< y_alt)
                        {
                            y_alt = polilinea[0].p.Y;
                        }
                    }
                    y_ins -= (y_alt* escala);
                    RellenarDatos();
                    Dibujar(0);
                    bool salida = true;
                    while (salida)
                    {
                        if (opcion == 1)
                        {
                            FiltradoDisrupcion(true, ref contador);
                            if (contador == 0)
                            {
                                salida = false;
                            }
                            contador = 0;
                            Vaciar_Puntos();
                            RellenarDatos();
                        }
                    }
                    Dibujar(2);
                    foreach (Punto p in polilinea)
                    {
                        polilinea_inicial.Add(p);
                    }
                    /*                   List<Punto> List_temp = new List<Punto>();
                                       List<Punto> List_aux = new List<Punto>();
                                       //Detectamos los vertices
                                       List_temp.Add(polilinea[0]);
                                       List_temp.Add(polilinea[1]);
                                       List_temp.Add(polilinea[2]);
                                       for (int i = 3; i < polilinea.Count - 2; i++)
                                       {
                                           //superior
                                           if (polilinea[i].p.Y > polilinea[i - 2].p.Y &&
                                               polilinea[i].p.Y > polilinea[i - 1].p.Y &&
                                               polilinea[i].p.Y > polilinea[i + 1].p.Y &&
                                               polilinea[i].p.Y > polilinea[i + 2].p.Y)
                                           {
                                               List_aux.Add(polilinea[i]);
                                               for (int t = 0; t < n_suavizados; t++)
                                               {
                                                   List_aux = Suavizar(List_aux);
                                               }

                                               for (int t = 0; t < List_aux.Count; t++)
                                               {
                                                   List_temp.Add(List_aux[t]);
                                               }
                                               List_aux.Clear();
                                               List_aux.Add(polilinea[i]);
                                           }
                                           else if (polilinea[i].p.Y < polilinea[i - 2].p.Y &&
                                               polilinea[i].p.Y < polilinea[i - 1].p.Y &&
                                               polilinea[i].p.Y < polilinea[i + 1].p.Y &&
                                               polilinea[i].p.Y < polilinea[i + 2].p.Y)
                                           {
                                               List_aux.Add(polilinea[i]);
                                               for (int t = 0; t < n_suavizados; t++)
                                               {
                                                   List_aux = Suavizar(List_aux);
                                               }
                                               for (int t = 0; t < List_aux.Count; t++)
                                               {
                                                   List_temp.Add(List_aux[t]);
                                               }
                                               List_aux.Clear();
                                               List_aux.Add(polilinea[i]);
                                           }
                                           else
                                           {
                                               List_aux.Add(polilinea[i]);
                                           }
                                       }
                                       for (int t = 0; t < n_suavizados; t++)
                                       {
                                           List_aux = Suavizar(List_aux);
                                       }
                                       for (int t = 0; t < List_aux.Count; t++)
                                       {
                                           List_temp.Add(List_aux[t]);
                                       }
                                       List_aux.Clear();
                                       List_temp.Add(polilinea[polilinea.Count - 2]);
                                       List_temp.Add(polilinea[polilinea.Count - 1]);
                                       polilinea = List_temp;

                                         /*for (int i=0;i< n_suavizados; i++)
                                         {
                                             polilinea = Suavizar(polilinea);
                                         }*/

                    polilinea= Duplicar_puntos(polilinea);
                    Vaciar_Puntos();
                    RellenarDatos();
                    polilinea = Duplicar_puntos(polilinea);
                    Vaciar_Puntos();
                    RellenarDatos();
                    polilinea = Duplicar_puntos(polilinea);
                    Vaciar_Puntos();
                    RellenarDatos();

                    for (int i = 0; i < n_suavizados; i++)
                    {
                        polilinea = Suavizar(polilinea);
                    }

                    Vaciar_Puntos();
                    RellenarDatos();

                    Iguales();
                    Dibujar(1);
                }
                else
                {
                    MessageBox.Show("No hay ninguna polilinea cargada");
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Debe realizar primero la busqueda para realizar la exportacion", ex);

            }
        }
        public CalculoPolilineaPerfil(ref dsApp a, int opcion, double ratio, int[] orden, int it)
        {
            MessageBox.Show("Eliga en autocad el punto de inserción.");
            Point3d fstCnr = new Point3d();
            GuardarPunto gp = new GuardarPunto(ref fstCnr);
            try
            {
                Insercion(fstCnr.X, fstCnr.Y);
                bool dibujar = true;
                if (a.Polilinea3d.Rows.Count == 0)
                {
                    dibujar = false;
                    Set_Polilinea3d(ref a);
                }
                a.Polilinea.Clear();
                int contador = 0;
                int contador_total = 0;
                this.ratio = ratio;
                foreach (DataRow r in a.Polilinea3d.Rows)
                {

                    string x = (string)r["X"];
                    string y = (string)r["Y"];
                    string z = (string)r["Z"];

                    Punto p = new Punto(new Point2d(Math.Sqrt(Math.Pow(double.Parse(x), 2) + Math.Pow(double.Parse(y), 2)), double.Parse(z)));
                    polilinea.Add(p);
                }
                double y_alt = polilinea[0].p.Y;
                for (int i = 1; i < polilinea.Count; i++)
                {
                    if (polilinea[0].p.Y < y_alt)
                    {
                        y_alt = polilinea[0].p.Y;
                    }
                }
                y_ins -= (y_alt * escala);
                RellenarDatos();
                if (dibujar)
                {
                    Dibujar(0);
                }

                if (it == 1)//1 si 2 no
                {
                    while (salir())
                    {
                        int filtrado = 1;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int t = 0; t < 3; t++)
                            {
                                if (orden[t] == filtrado)
                                {
                                    if (t == 0)
                                    {
                                        FiltradoDisrupcion2(ref contador);
                                        contador_total += contador;
                                        contador = 0;
                                        RellenarDatos();
                                    }
                                    else if (t == 1)
                                    {
                                        FiltradoCambioSentido2(ref contador);
                                        contador_total += contador;
                                        contador = 0;
                                        RellenarDatos();
                                    }
                                    else if (t == 2)
                                    {
                                        FiltradoGiroLongitud2(ratio, ref contador);
                                        contador_total += contador;
                                        contador = 0;
                                        RellenarDatos();
                                    }
                                    filtrado++;
                                }
                            }
                        }

                    }
                }
                else
                {
                    int filtrado = 1;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int t = 0; t < 3; t++)
                        {
                            if (orden[t] == filtrado)
                            {
                                if (t == 0)
                                {
                                    FiltradoDisrupcion2(ref contador);
                                    contador_total += contador;
                                    contador = 0;
                                    RellenarDatos();
                                }
                                else if (t == 1)
                                {
                                    FiltradoCambioSentido2(ref contador);
                                    contador_total += contador;
                                    contador = 0;
                                    RellenarDatos();
                                }
                                else if (t == 2)
                                {
                                    FiltradoGiroLongitud2(ratio, ref contador);
                                    contador_total += contador;
                                    contador = 0;
                                    RellenarDatos();
                                }
                                filtrado++;
                            }
                        }
                    }
                }
                MessageBox.Show("Se han eliminado " + contador_total + " puntos");

                DialogResult result = MessageBox.Show("¿Desea guardar el resultado?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {

                    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

                    saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.DefaultExt = "txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter sw2 = new StreamWriter(saveFileDialog1.FileName);

                        for (int i = 0; i < polilinea.Count; i++)
                        {
                            sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                        }
                        sw2.Close();

                    }




                }
                else if (result == DialogResult.No)
                {
                }
                Dibujar(1);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Debe realizar primero la busqueda para realizar la exportacion", ex);
            }
        }
        private bool salir()
        {
            int contador = 0;
            int contador2 = 0;
            FiltradoDisrupcion(false, ref contador);
            contador2 += contador;
            contador = 0;
            FiltradoCambioSentido(false, ref contador);
            contador2 += contador;
            contador = 0;
            FiltradoGiroLongitud(ratio, false, ref contador);
            contador2 += contador;
            contador = 0;
            if (contador2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void Set_Polilinea3d(ref dsApp a)
        {
            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {
                //Para cambiar a puntos 3d hay que cambiar Polyline por Polyline3d


                //Selecciono la Entidad Generica
                Polyline3d miEjeVisibilidadUsuario = oSs.seleccionUsuario<Polyline3d>("Selecciona una polilinea", "No has seleccionado una polilinea");
                /*int c_p = miEjeVisibilidadUsuario.NumberOfVertices;
                
                for (int i=0;i<c_p;i++)
                {
                    a.Polilinea.Rows.Add(miEjeVisibilidadUsuario.GetPoint3dAt(i).X, miEjeVisibilidadUsuario.GetPoint3dAt(i).Y, i+1);
                }*/

                if (miEjeVisibilidadUsuario != null)
                {
                    Polyline3d pol = new Polyline3d();
                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline3d> miLw = new oEntidad<Polyline3d>(miEjeVisibilidadUsuario))
                    {
                        miLw.open();
                        engCadNet.oLayer.addLayer("Polilinea seleccionada", 4, false);
                        pol = miLw.entidad;
                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, 4);
                        miLw.entidad.Layer = "Polilinea seleccionada";
                        /*Para 3d*/
                        double x, y, z;
                        for (int i = 0; i < miLw.entidad.EndParam; i++)
                        {
                            a.Polilinea3d.Rows.Add(miLw.entidad.GetPointAtParameter(i).X, miLw.entidad.GetPointAtParameter(i).Y, miLw.entidad.GetPointAtParameter(i).Z, i + 1);
                        }
                       /* StringBuilder sb = new StringBuilder();

                        IEnumerable<string> columnNames = a.Polilinea3d.Columns.Cast<System.Data.DataColumn>().
                                                          Select(column => column.ColumnName);
                        sb.AppendLine(string.Join(";", columnNames));
                        foreach (DataRow row in a.Polilinea3d.Rows)
                        {
                            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                            sb.AppendLine(string.Join(";", fields));
                        }
                        */
                        //File.WriteAllText("test.csv", sb.ToString());
                        miLw.save();

                    }
                }
                else
                {

                }

            }
        }
        public void Guardar()
        {
            DialogResult result = MessageBox.Show("¿Desea guardar el resultado?", "Salir", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {

                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.DefaultExt = "txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw2 = new StreamWriter(saveFileDialog1.FileName);

                    for (int i = 0; i < polilinea.Count; i++)
                    {
                        sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                    }
                    sw2.Close();

                }
            }
            else if (result == DialogResult.No)
            {
            }
        }
        public void Vaciar_Puntos()
        {
            for (int i = 0; i < polilinea.Count; i++)
            {
                polilinea[i].Vaciar();
            }
        }
        public List<Punto> Vaciar_Puntos_aux(List<Punto> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                lista[i].Vaciar();
            }
            return lista;
        }
        public void Vaciar_Puntos_inicial()
        {
            for (int i = 0; i < polilinea_inicial.Count; i++)
            {
                polilinea[i].Vaciar();
            }
        }
        private void RellenarDatos()
        {
            for (int i = 1; i < polilinea.Count; i++)
            {
                Punto p = polilinea[i];
                Punto p_a = polilinea[i - 1];
                Punto p_s = new Punto();
                if (i < polilinea.Count - 1)
                {
                    p_s = polilinea[i + 1];
                }
                p.a = Math.Pow(Math.Pow((p.p.X - p_a.p.X), 2) + Math.Pow((p.p.Y - p_a.p.Y), 2), 0.5);
                if (i < polilinea.Count - 1)
                {
                    p.b = Math.Pow(Math.Pow((p_s.p.X - p.p.X), 2) + Math.Pow((p_s.p.Y - p.p.Y), 2), 0.5);
                    p.c = Math.Pow(Math.Pow((p_s.p.X - p_a.p.X), 2) + Math.Pow((p_s.p.Y - p_a.p.Y), 2), 0.5);
                    p.s = (p.a + p.b + p.c) / 2;
                    p.R = (p.a * p.b * p.c) / (4 * Math.Pow(p.s * (p.s - p.a) * (p.s - p.b) * (p.s - p.c), 0.5));
                    if (double.IsNaN(p.R) || double.IsInfinity(p.R))
                    {
                        p.R = 0;
                    }
                }
                p.Dx = p.p.X - p_a.p.X;
                p.Dy = p.p.Y - p_a.p.Y;
                if (p.Dx == 0)
                {
                    p.Ad1 = 0;
                }
                else
                {
                    if (p.Dy == 0)
                    {
                        p.Ad1 = 0;
                    }
                    else
                    {
                        p.Ad1 = Math.Atan(p.Dy / p.Dx);
                    }
                }
                p.Ad2 = p.Ad1 * (180 / Math.PI);

                if (p.Ad1 == 0)
                {
                    p.signod = 0;
                }
                else
                {
                    if (p.Ad1 < 0)
                    {
                        p.signod = 1;
                    }
                    else
                    {
                        p.signod = 2;
                    }
                }

                if (p.Dx == 0)
                {
                    p.signodx = 0;
                }
                else
                {
                    if (p.Dx < 0)
                    {
                        p.signodx = 1;
                    }
                    else
                    {
                        p.signodx = 2;
                    }
                }

                if (p.Dy == 0)
                {
                    p.signody = 0;
                }
                else
                {
                    if (p.Dy < 0)
                    {
                        p.signody = 1;
                    }
                    else
                    {
                        p.signody = 2;
                    }
                }

                if (p.signod == 0)
                {
                    p.Dc = 2;
                }
                else
                {
                    p.Dc = 1;
                }
                if (p.Dc == 2)
                {
                    if (p.Dy == 0)
                    {
                        p.Orientacion = "E-W";
                    }
                    else
                    {
                        p.Orientacion = "N-S";
                    }
                }

                if (p.Dc == 2)
                {
                    if (p.Orientacion == "E-W")
                    {
                        if (p.Dx < 0)
                        {
                            p.Azcardinal = 270;
                        }
                        else
                        {
                            p.Azcardinal = 90;
                        }
                    }
                    else
                    {
                        if (p.Dy < 0)
                        {
                            p.Azcardinal = 180;
                        }
                        else
                        {
                            p.Azcardinal = 0;
                        }
                    }
                }

                //cuadrante
                if (p.Dc == 2)
                {
                    p.cuadrante = 0;
                }
                else
                {
                    if (p.Dx > 0 && p.Dy > 0)
                    {
                        p.cuadrante = 1;
                    }
                    else
                    {
                        if (p.Dx > 0 && p.Dy < 0)
                        {
                            p.cuadrante = 2;
                        }
                        else
                        {
                            if (p.Dx < 0 && p.Dy < 0)
                            {
                                p.cuadrante = 3;
                            }
                            else
                            {
                                p.cuadrante = 4;
                            }
                        }
                    }
                }

                //Azimut
                if (p.Dc == 2)
                {
                    p.Az = p.Azcardinal;
                }
                else
                {
                    if (p.signod == 1)
                    {
                        if (p.signodx == 2)
                        {
                            p.Az = 90 - p.Ad2;

                        }
                        else
                        {
                            p.Az = 270 - p.Ad2;
                        }
                    }
                    else
                    {
                        if (p.signodx == 2)//esto esta mal
                        {
                            p.Az = 90 - p.Ad2;

                        }
                        else
                        {
                            p.Az = 270 - p.Ad2;
                        }
                    }
                }

                //calculo azimutal
                if (i > 1)
                {
                    if (p.Az < p_a.Az)
                    {
                        if (Math.Abs(p.Az - p_a.Az) > 180)
                        {
                            p.CambioAz = p.Az + 360 - p_a.Az;
                        }
                        else
                        {
                            p.CambioAz = Math.Truncate(p.Az * 100000) - Math.Truncate(p_a.Az * 100000);
                        }
                    }
                    else
                    {
                        p.CambioAz = Math.Truncate(p.Az * 100000) - Math.Truncate(p_a.Az * 100000);
                    }


                    if (i < polilinea.Count)
                    {
                        Punto p_aa = polilinea[i - 2];
                        if (p.CambioAz == 0)
                        {
                            p_a.Tipogiro = p_aa.Tipogiro;
                        }
                        else
                        {
                            if (getSentidoCurva(p_a.Az, p.Az) == EjeTrazado.sentidoCurva.Horario)
                            {
                                p_a.Tipogiro = 1;
                            }
                            else
                            {
                                p_a.Tipogiro = 2;
                            }
                            /*if (p_a.Az >= 0 && p_a.Az < 180)
                            {
                                if (Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000) <= 0 && Math.Abs(Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000)) <= 180)
                                {
                                    p_a.Tipogiro = 1;
                                }
                                else
                                {
                                    p_a.Tipogiro = 2;
                                }
                            }
                            else
                            {
                                if (Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000) >= 0 && Math.Abs(Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000)) <= 180)
                                {
                                    p_a.Tipogiro = 2;
                                }
                                else
                                {
                                    p_a.Tipogiro = 1;
                                }
                            }*/
                        }


                        //cambiado codigo al anterior
                        /*if (p.CambioAz == 0)
                        {
                            p_a.Tipogiro = 3;
                        }
                        else if (p.CambioAz < 0)
                        {
                            p_a.Tipogiro = 2;
                        }
                        else
                        {
                            p_a.Tipogiro = 1;
                        }*/
                    }
                }
                if (i < polilinea.Count)
                {
                    if (i > 2)
                    {
                        Punto p_aa = polilinea[i - 2];
                        if (p_aa.Tipogiro == 3)
                        {
                            p_a.secuenciagiro = 2;
                        }
                        else
                        {
                            if (p_aa.Tipogiro == p_a.Tipogiro)
                            {
                                p_a.secuenciagiro = 2;
                            }
                            else
                            {
                                if (p_a.Tipogiro == 3)
                                {
                                    p_a.secuenciagiro = 2;
                                }
                                else
                                {
                                    p_a.secuenciagiro = 1;
                                }

                            }
                        }



                    }
                    if (i == polilinea.Count - 2)
                    {
                        Punto p_u = polilinea[i - 1];
                        //p_u.secuenciagiro = 2;
                        Punto p_u2 = polilinea[i];
                        p_u2.secuenciagiro = 2;
                    }
                }

            }
        }
        private void RellenarDatos_inicial()
        {
            for (int i = 1; i < polilinea_inicial.Count; i++)
            {
                Punto p = polilinea_inicial[i];
                Punto p_a = polilinea_inicial[i - 1];
                Punto p_s = new Punto();
                if (i < polilinea_inicial.Count - 1)
                {
                    p_s = polilinea_inicial[i + 1];
                }
                p.a = Math.Pow(Math.Pow((p.p.X - p_a.p.X), 2) + Math.Pow((p.p.Y - p_a.p.Y), 2), 0.5);
                if (i < polilinea_inicial.Count - 1)
                {
                    p.b = Math.Pow(Math.Pow((p_s.p.X - p.p.X), 2) + Math.Pow((p_s.p.Y - p.p.Y), 2), 0.5);
                    p.c = Math.Pow(Math.Pow((p_s.p.X - p_a.p.X), 2) + Math.Pow((p_s.p.Y - p_a.p.Y), 2), 0.5);
                    p.s = (p.a + p.b + p.c) / 2;
                    p.R = (p.a * p.b * p.c) / (4 * Math.Pow(p.s * (p.s - p.a) * (p.s - p.b) * (p.s - p.c), 0.5));
                    if (double.IsNaN(p.R) || double.IsInfinity(p.R))
                    {
                        p.R = 0;
                    }
                }
                p.Dx = p.p.X - p_a.p.X;
                p.Dy = p.p.Y - p_a.p.Y;
                if (p.Dx == 0)
                {
                    p.Ad1 = 0;
                }
                else
                {
                    if (p.Dy == 0)
                    {
                        p.Ad1 = 0;
                    }
                    else
                    {
                        p.Ad1 = Math.Atan(p.Dy / p.Dx);
                    }
                }
                p.Ad2 = p.Ad1 * (180 / Math.PI);

                if (p.Ad1 == 0)
                {
                    p.signod = 0;
                }
                else
                {
                    if (p.Ad1 < 0)
                    {
                        p.signod = 1;
                    }
                    else
                    {
                        p.signod = 2;
                    }
                }

                if (p.Dx == 0)
                {
                    p.signodx = 0;
                }
                else
                {
                    if (p.Dx < 0)
                    {
                        p.signodx = 1;
                    }
                    else
                    {
                        p.signodx = 2;
                    }
                }

                if (p.Dy == 0)
                {
                    p.signody = 0;
                }
                else
                {
                    if (p.Dy < 0)
                    {
                        p.signody = 1;
                    }
                    else
                    {
                        p.signody = 2;
                    }
                }

                if (p.signod == 0)
                {
                    p.Dc = 2;
                }
                else
                {
                    p.Dc = 1;
                }
                if (p.Dc == 2)
                {
                    if (p.Dy == 0)
                    {
                        p.Orientacion = "E-W";
                    }
                    else
                    {
                        p.Orientacion = "N-S";
                    }
                }

                if (p.Dc == 2)
                {
                    if (p.Orientacion == "E-W")
                    {
                        if (p.Dx < 0)
                        {
                            p.Azcardinal = 270;
                        }
                        else
                        {
                            p.Azcardinal = 90;
                        }
                    }
                    else
                    {
                        if (p.Dy < 0)
                        {
                            p.Azcardinal = 180;
                        }
                        else
                        {
                            p.Azcardinal = 0;
                        }
                    }
                }

                //cuadrante
                if (p.Dc == 2)
                {
                    p.cuadrante = 0;
                }
                else
                {
                    if (p.Dx > 0 && p.Dy > 0)
                    {
                        p.cuadrante = 1;
                    }
                    else
                    {
                        if (p.Dx > 0 && p.Dy < 0)
                        {
                            p.cuadrante = 2;
                        }
                        else
                        {
                            if (p.Dx < 0 && p.Dy < 0)
                            {
                                p.cuadrante = 3;
                            }
                            else
                            {
                                p.cuadrante = 4;
                            }
                        }
                    }
                }

                //Azimut
                if (p.Dc == 2)
                {
                    p.Az = p.Azcardinal;
                }
                else
                {
                    if (p.signod == 1)
                    {
                        if (p.signodx == 2)
                        {
                            p.Az = 90 - p.Ad2;

                        }
                        else
                        {
                            p.Az = 270 - p.Ad2;
                        }
                    }
                    else
                    {
                        if (p.signodx == 2)//esto esta mal
                        {
                            p.Az = 90 - p.Ad2;

                        }
                        else
                        {
                            p.Az = 270 - p.Ad2;
                        }
                    }
                }

                //calculo azimutal
                if (i > 1)
                {
                    if (p.Az < p_a.Az)
                    {
                        if (Math.Abs(p.Az - p_a.Az) > 180)
                        {
                            p.CambioAz = p.Az + 360 - p_a.Az;
                        }
                        else
                        {
                            p.CambioAz = Math.Truncate(p.Az * 100000) - Math.Truncate(p_a.Az * 100000);
                        }
                    }
                    else
                    {
                        p.CambioAz = Math.Truncate(p.Az * 100000) - Math.Truncate(p_a.Az * 100000);
                    }


                    if (i < polilinea_inicial.Count)
                    {
                        Punto p_aa = polilinea_inicial[i - 2];
                        if (p.CambioAz == 0)
                        {
                            p_a.Tipogiro = p_aa.Tipogiro;
                        }
                        else
                        {
                            if (getSentidoCurva(p_a.Az, p.Az) == EjeTrazado.sentidoCurva.Horario)
                            {
                                p_a.Tipogiro = 1;
                            }
                            else
                            {
                                p_a.Tipogiro = 2;
                            }
                            /*if (p_a.Az >= 0 && p_a.Az < 180)
                            {
                                if (Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000) <= 0 && Math.Abs(Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000)) <= 180)
                                {
                                    p_a.Tipogiro = 1;
                                }
                                else
                                {
                                    p_a.Tipogiro = 2;
                                }
                            }
                            else
                            {
                                if (Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000) >= 0 && Math.Abs(Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000)) <= 180)
                                {
                                    p_a.Tipogiro = 2;
                                }
                                else
                                {
                                    p_a.Tipogiro = 1;
                                }
                            }*/
                        }


                        //cambiado codigo al anterior
                        /*if (p.CambioAz == 0)
                        {
                            p_a.Tipogiro = 3;
                        }
                        else if (p.CambioAz < 0)
                        {
                            p_a.Tipogiro = 2;
                        }
                        else
                        {
                            p_a.Tipogiro = 1;
                        }*/
                    }
                }
                if (i < polilinea_inicial.Count)
                {
                    if (i > 2)
                    {
                        Punto p_aa = polilinea_inicial[i - 2];
                        if (p_aa.Tipogiro == 3)
                        {
                            p_a.secuenciagiro = 2;
                        }
                        else
                        {
                            if (p_aa.Tipogiro == p_a.Tipogiro)
                            {
                                p_a.secuenciagiro = 2;
                            }
                            else
                            {
                                if (p_a.Tipogiro == 3)
                                {
                                    p_a.secuenciagiro = 2;
                                }
                                else
                                {
                                    p_a.secuenciagiro = 1;
                                }

                            }
                        }



                    }
                    if (i == polilinea_inicial.Count - 2)
                    {
                        Punto p_u = polilinea_inicial[i - 1];
                        //p_u.secuenciagiro = 2;
                        Punto p_u2 = polilinea_inicial[i];
                        p_u2.secuenciagiro = 2;
                    }
                }

            }
        }
        private void Dibujar(int a)
        {
            Point3dCollection poly = new Point3dCollection();
            for (int i = 0; i < polilinea.Count; i++)
            {

                poly.Add(new Point3d(polilinea[i].p.X + x_ins, y_ins + polilinea[i].p.Y*escala, 0));
            }
            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {
                if (a == 0)
                {
                    engCadNet.oLayer.addLayer("polilinia original", 4, false);
                }
                else
                {
                    if (a == 2)
                    {
                        engCadNet.oLayer.addLayer("Polilinea depurada", 6, false);
                    }
                    else
                    {
                        engCadNet.oLayer.addLayer("Nueva suavizada", 3, false);
                    }

                }

                using (Transaction acTrans = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(AcCurDb2.BlockTableId,
                        OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite) as BlockTableRecord;

                    Editor e = Application.DocumentManager.MdiActiveDocument.Editor;
                    Document d = Application.DocumentManager.MdiActiveDocument;
                    Polyline3d pol = new Polyline3d(new Poly3dType(), poly, false);

                    if (a == 0)
                    {
                        pol.Layer = "polilinia original";
                    }
                    else
                    {
                        if (a == 2)
                        {
                            pol.Layer = "Polilinea depurada";
                        }
                        else
                        {
                            pol.Layer = "Nueva suavizada";
                        }

                    }
                    acBlkTblRec.AppendEntity(pol);

                    acTrans.AddNewlyCreatedDBObject(pol, true);

                    acTrans.Commit();
                }
            }
        }
        private void Dibujar(int a,List<PuntoPerfil> Lista )
        {
            Point3dCollection poly = new Point3dCollection();
            for (int i = 0; i < Lista.Count; i++)
            {

                poly.Add(new Point3d(Lista[i].p.X + x_ins, y_ins + Lista[i].p.Y * escala, 0));
            }
            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {
                if (a == 0)
                {
                    engCadNet.oLayer.addLayer("polilinia original", 4, false);
                }
                else
                {
                    if (a == 2)
                    {
                        engCadNet.oLayer.addLayer("Polilinea depurada", 6, false);
                    }else if (a==3)
                    {
                        engCadNet.oLayer.addLayer("Polilinea Cota", 5, false);
                    }
                    else
                    {
                        engCadNet.oLayer.addLayer("Nueva suavizada", 3, false);
                    }

                }

                using (Transaction acTrans = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(AcCurDb2.BlockTableId,
                        OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite) as BlockTableRecord;

                    Editor e = Application.DocumentManager.MdiActiveDocument.Editor;
                    Document d = Application.DocumentManager.MdiActiveDocument;
                    Polyline3d pol = new Polyline3d(new Poly3dType(), poly, false);

                    if (a == 0)
                    {
                        pol.Layer = "polilinia original";
                    }
                    else
                    {
                        if (a == 2)
                        {
                            pol.Layer = "Polilinea depurada";
                        }
                        else if (a == 3)
                        {
                            pol.Layer = "Polilinea Cota";
                        }
                        else
                        {
                            pol.Layer = "Nueva suavizada";
                        }

                    }
                    acBlkTblRec.AppendEntity(pol);

                    acTrans.AddNewlyCreatedDBObject(pol, true);

                    acTrans.Commit();
                }
            }
        }
        private void FiltradoDisrupcion(bool acc, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;

            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(new Punto(new Point2d(p.p.X, p.p.Y)));
                }
                else
                {
                    if (polilinea[i].secuenciagiro == 1 && polilinea[i + 1].secuenciagiro == 1)
                    {
                        contador++;


                    }
                    else
                    {
                        if (acc)
                        {

                            polilinea_temp.Add(new Punto(new Point2d(p.p.X, p.p.Y)));
                            datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                            contador2++;
                        }

                    }
                }
            }
            if (acc)
            {
                polilinea_temp.Add((new Punto(new Point2d(polilinea[polilinea.Count - 1].p.X, polilinea[polilinea.Count - 1].p.Y))));

                polilinea = polilinea_temp;
                RellenarDatos();
                /*MessageBox.Show("Se han eliminado " + contador + " puntos");
                if (contador > 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea guardar el resultado?", "Salir", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {

                        System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

                        saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.DefaultExt = "txt";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            StreamWriter sw2 = new StreamWriter(saveFileDialog1.FileName);

                            for (int i = 0; i < polilinea.Count; i++)
                            {
                                sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                            }
                            sw2.Close();

                        }




                    }
                    else if (result == DialogResult.No)
                    {
                    }
                }*/

            }
        }
        private void FiltradoDisrupcion_inicial(bool acc, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;

            for (int i = 0; i < polilinea_inicial.Count - 1; i++)
            {
                Punto p = polilinea_inicial[i];
                if (i == 0)
                {
                    polilinea_temp.Add(new Punto(new Point2d(p.p.X, p.p.Y)));
                }
                else
                {
                    if (polilinea[i].secuenciagiro == 1 && polilinea_inicial[i + 1].secuenciagiro == 1)
                    {
                        contador++;


                    }
                    else
                    {
                        if (acc)
                        {

                            polilinea_temp.Add(new Punto(new Point2d(p.p.X, p.p.Y)));
                            datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                            contador2++;
                        }

                    }
                }
            }
            if (acc)
            {
                polilinea_temp.Add((new Punto(new Point2d(polilinea_inicial[polilinea_inicial.Count - 1].p.X, polilinea_inicial[polilinea_inicial.Count - 1].p.Y))));

                polilinea_inicial = polilinea_temp;
                RellenarDatos_inicial();
                /*MessageBox.Show("Se han eliminado " + contador + " puntos");
                if (contador > 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea guardar el resultado?", "Salir", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {

                        System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

                        saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.DefaultExt = "txt";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            StreamWriter sw2 = new StreamWriter(saveFileDialog1.FileName);

                            for (int i = 0; i < polilinea.Count; i++)
                            {
                                sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                            }
                            sw2.Close();

                        }




                    }
                    else if (result == DialogResult.No)
                    {
                    }
                }*/

            }
        }
        private void FiltradoDisrupcion2(ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (polilinea[i].secuenciagiro == 1 && polilinea[i + 1].secuenciagiro == 1)
                    {
                        contador++;
                    }
                    else
                    {
                        polilinea_temp.Add(p);
                        datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                        contador2++;
                    }
                }
            }
            polilinea_temp.Add(polilinea[polilinea.Count - 1]);
            polilinea = polilinea_temp;
        }
        private void FiltradoCambioSentido(bool acc, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();

            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (polilinea[i + 1].secuenciagiro == 1 && (polilinea[i + 1].R < p.R && polilinea[i + 1].R < polilinea[i + 2].R))
                    {
                        contador++;


                    }
                    else
                    {
                        if (acc)
                        {
                            polilinea_temp.Add(p);
                            datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                            contador2++;
                        }

                    }
                }
            }
            if (acc)
            {
                polilinea_temp.Add(polilinea[polilinea.Count - 1]);
                polilinea = polilinea_temp;
                MessageBox.Show("Se han eliminado " + contador + " puntos");

                DialogResult result = MessageBox.Show("¿Desea guardar el resultado?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {

                    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

                    saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.DefaultExt = "txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter sw2 = new StreamWriter(saveFileDialog1.FileName);

                        for (int i = 0; i < polilinea.Count; i++)
                        {
                            sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                        }
                        sw2.Close();

                    }




                }
                else if (result == DialogResult.No)
                {
                }
            }
        }
        private void FiltradoCambioSentido2(ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();

            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (polilinea[i + 1].secuenciagiro == 1 && (polilinea[i + 1].R < p.R && polilinea[i + 1].R < polilinea[i + 2].R))
                    {
                        contador++;


                    }
                    else
                    {
                        polilinea_temp.Add(p);
                        datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                        contador2++;

                    }
                }
            }

            polilinea_temp.Add(polilinea[polilinea.Count - 1]);
            polilinea = polilinea_temp;

        }
        private void FiltradoGiroLongitud(double ratio, bool acc, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (Math.Abs(polilinea[i + 1].CambioAz / Distancia(polilinea[i].p, polilinea[i + 1].p)) > ratio)
                    {
                        contador++;


                    }
                    else
                    {
                        if (acc)
                        {
                            polilinea_temp.Add(p);
                            datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                            contador2++;
                        }

                    }
                }
            }
            if (acc)
            {
                polilinea_temp.Add(polilinea[polilinea.Count - 1]);
                polilinea = polilinea_temp;
                MessageBox.Show("Se han eliminado " + contador + " puntos");

                DialogResult result = MessageBox.Show("¿Desea guardar el resultado?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {

                    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

                    saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.DefaultExt = "txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter sw2 = new StreamWriter(saveFileDialog1.FileName);

                        for (int i = 0; i < polilinea.Count; i++)
                        {
                            sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                        }
                        sw2.Close();

                    }




                }
                else if (result == DialogResult.No)
                {
                }
            }
        }
        private void FiltradoGiroLongitud2(double ratio, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (Math.Abs(polilinea[i + 1].CambioAz / Distancia(polilinea[i].p, polilinea[i + 1].p)) > ratio)
                    {
                        contador++;


                    }
                    else
                    {
                        polilinea_temp.Add(p);
                        datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                        contador2++;
                    }
                }
            }
            polilinea_temp.Add(polilinea[polilinea.Count - 1]);
            polilinea = polilinea_temp;
        }
        private double Distancia(Point2d a, Point2d b)
        {
            double d;
            d = Math.Pow(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2), 0.5);
            return d;
        }
        public EjeTrazado.sentidoCurva getSentidoCurva(double iAzSegAnt, double iAz) {
            EjeTrazado.sentidoCurva miSent;
            if ((iAzSegAnt >= 0) && (iAzSegAnt <= 180)) {
                if (((iAzSegAnt - iAz) < 0) && (Math.Abs(iAzSegAnt - iAz) < 180)) {
                    miSent = EjeTrazado.sentidoCurva.Horario;
                } else {
                    miSent = EjeTrazado.sentidoCurva.Antihorario;
                }
            } else {
                if (((iAzSegAnt - iAz) > 0) && (Math.Abs(iAzSegAnt - iAz) < 180)) {
                    miSent = EjeTrazado.sentidoCurva.Antihorario;
                } else {
                    miSent = EjeTrazado.sentidoCurva.Horario;
                }
            }


            return miSent;
        }
        public void RellenarPerfil(double v_ac,int n_suavizados)
        {
            //Rellenamos parametros equivalentes entre punto y punto perfil
            for (int i=0;i<polilinea.Count;i++)
            {
                polilinea_perfil.Add(new PuntoPerfil(polilinea[i]));
            }
            //Detectamos los vertices
            for (int i = 3; i < polilinea_perfil.Count - 2; i++)
            {
                //superior
                if (polilinea_perfil[i].p.Y > polilinea_perfil[i - 2].p.Y &&
                    polilinea_perfil[i].p.Y > polilinea_perfil[i - 1].p.Y &&
                    polilinea_perfil[i].p.Y > polilinea_perfil[i + 1].p.Y &&
                    polilinea_perfil[i].p.Y > polilinea_perfil[i + 2].p.Y)
                {
                    polilinea_perfil[i].vertice = 1;
                }
                //inferior
                if (polilinea_perfil[i].p.Y < polilinea_perfil[i - 2].p.Y &&
                    polilinea_perfil[i].p.Y < polilinea_perfil[i - 1].p.Y &&
                    polilinea_perfil[i].p.Y < polilinea_perfil[i + 1].p.Y &&
                    polilinea_perfil[i].p.Y < polilinea_perfil[i + 2].p.Y)
                {
                    polilinea_perfil[i].vertice = 2;
                }
            }
            Reajustar_Cota();
            Dibujar(3, polilinea_perfil);
            //Calculamos la pendiente y la pendiente_p
            for (int i=1;i<polilinea_perfil.Count;i++)
            {
                polilinea_perfil[i].pendiente = polilinea_perfil[i].dy / polilinea_perfil[i].dx;
                polilinea_perfil[i].pendiente_p = polilinea_perfil[i].pendiente * 100;
            }

            //Calculamos Valor medio
            for (int i=3;i<polilinea_perfil.Count-2;i++)
            {
                polilinea_perfil[i].v_m=(polilinea_perfil[i - 2].pendiente_p + polilinea_perfil[i - 1].pendiente_p + polilinea_perfil[i].pendiente_p + polilinea_perfil[i + 1].pendiente_p + polilinea_perfil[i + 2].pendiente_p)/ 5;
            }
            //calculamos Valor medio para los 2 primeros y los 2 últimos
            polilinea_perfil[1].v_m = polilinea_perfil[3].v_m;
            polilinea_perfil[2].v_m = polilinea_perfil[3].v_m;
            polilinea_perfil[polilinea_perfil.Count - 1].v_m = polilinea_perfil[polilinea_perfil.Count - 3].v_m;
            polilinea_perfil[polilinea_perfil.Count - 2].v_m = polilinea_perfil[polilinea_perfil.Count - 3].v_m;

            //Calculamos primero y ultimo
            for (int i = 3; i < polilinea_perfil.Count - 2; i++)
            {
                polilinea_perfil[i].primero = polilinea_perfil[i - 2].pendiente_p;
                polilinea_perfil[i].ultimo = polilinea_perfil[i + 2].pendiente_p;
            }
            
            //Calculamos la Varianza
            for (int i = 3; i < polilinea_perfil.Count - 2; i++)
            {
                polilinea_perfil[i].varianza = (
                    Math.Pow(polilinea_perfil[i - 2].v_m - polilinea_perfil[i].primero, 2) +
                    Math.Pow(polilinea_perfil[i - 1].v_m - polilinea_perfil[i].primero, 2) +
                    Math.Pow(polilinea_perfil[  i  ].v_m - polilinea_perfil[i].primero, 2) +
                    Math.Pow(polilinea_perfil[i + 1].v_m - polilinea_perfil[i].primero, 2) +
                    Math.Pow(polilinea_perfil[i + 2].v_m - polilinea_perfil[i].primero, 2) ) / 5;
            }

            //Calculamos Varianza acumulada
            for (int i = 5; i < polilinea_perfil.Count - 4; i++)
            {
                polilinea_perfil[i].varianza_a = 
                    polilinea_perfil[i - 2].varianza + 
                    polilinea_perfil[i - 1].varianza + 
                    polilinea_perfil[  i  ].varianza + 
                    polilinea_perfil[i + 1].varianza + 
                    polilinea_perfil[i + 2].varianza;
            }

            //Detectamos los vertices
            for (int i = 3; i < polilinea_perfil.Count - 2; i++)
            {
                //superior
                if (polilinea_perfil[i].p.Y > polilinea_perfil[i - 2].p.Y &&
                    polilinea_perfil[i].p.Y > polilinea_perfil[i - 1].p.Y &&
                    polilinea_perfil[i].p.Y > polilinea_perfil[i + 1].p.Y &&
                    polilinea_perfil[i].p.Y > polilinea_perfil[i + 2].p.Y)
                {
                    polilinea_perfil[i].vertice = 1;
                }
                //inferior
                if (polilinea_perfil[i].p.Y < polilinea_perfil[i - 2].p.Y &&
                    polilinea_perfil[i].p.Y < polilinea_perfil[i - 1].p.Y &&
                    polilinea_perfil[i].p.Y < polilinea_perfil[i + 1].p.Y &&
                    polilinea_perfil[i].p.Y < polilinea_perfil[i + 2].p.Y)
                {
                    polilinea_perfil[i].vertice = 2;
                }
            }

            //Calculamos media de la varianza
            double v_acumulada = 0;
            int c_cantidad = 0;
            for (int i=0;i<polilinea_perfil.Count;i++)
            {
                if (polilinea_perfil[i].varianza_a>0)
                {
                    v_acumulada += polilinea_perfil[i].varianza_a;
                    c_cantidad++;
                }
            }
            v_acumulada = v_acumulada / c_cantidad;
           // v_acumulada = v_acumulada * 0.3;
            //Calculamos el resultado
            List<int> Lista_aux = new List<int>();
            Lista_aux.Add(1);
            Lista_aux.Add(2);
            Lista_aux.Add(3);
            Lista_aux.Add(4);
            for (int i = 5; i < polilinea_perfil.Count - 4; i++)
            {
                if (polilinea_perfil[i].varianza_a< v_ac)///////varianza variable
                {
                    Lista_aux.Add(i);
                }
                else
                {
                    if (Lista_aux.Count>=7)
                    {
                        for (int t= 0;t< Lista_aux.Count;t++)
                        {
                            polilinea_perfil[Lista_aux[t]].tipo = 1;
                            
                        }
                        List<PuntoPerfil> lista_aux_valor = new List<PuntoPerfil>();
                        for (int t = 0; t < Lista_aux.Count; t++)
                        {
                            lista_aux_valor.Add(polilinea_perfil[Lista_aux[t]]);
                        }
                        double valor=ajuste_recta(lista_aux_valor);
                        for (int t = 0; t < Lista_aux.Count; t++)
                        {
                            polilinea_perfil[Lista_aux[t]].valor = valor;

                        }
                        Lista_aux.Clear();
                    }
                    else
                    {
                        if (Lista_aux.Count>0)
                        {
                            for (int t = 0; t < Lista_aux.Count; t++)
                            {
                                polilinea_perfil[Lista_aux[t]].tipo = 2;
                            }
                            Lista_aux.Clear();
                        }
                    }
                }
            }
            if (Lista_aux.Count >= 7)
            {
                for (int t = 0; t < Lista_aux.Count; t++)
                {
                    polilinea_perfil[Lista_aux[t]].tipo = 1;

                }
                List<PuntoPerfil> lista_aux_valor = new List<PuntoPerfil>();
                for (int t = 0; t < Lista_aux.Count; t++)
                {
                    lista_aux_valor.Add(polilinea_perfil[Lista_aux[t]]);
                }
                double valor = ajuste_recta(lista_aux_valor);
                for (int t = 0; t < Lista_aux.Count; t++)
                {
                    polilinea_perfil[Lista_aux[t]].valor = valor;

                }
                Lista_aux.Clear();
            }
            else
            {
                if (Lista_aux.Count > 0)
                {
                    for (int t = 0; t < Lista_aux.Count; t++)
                    {
                        polilinea_perfil[Lista_aux[t]].tipo = 2;
                    }
                    Lista_aux.Clear();
                }
            }
        }
        public double ajuste_recta(List<PuntoPerfil> listaa)
        {
            List<PuntoPerfil> ajustada = new List<PuntoPerfil>();
            double[] recta = new double[2];
            List<double> xi_xm = new List<double>();
            List<double> xi_xm2 = new List<double>();
            List<double> yi_ym = new List<double>();
            List<double> yi_ym2 = new List<double>();
            List<double> xi_yi = new List<double>();
            double xm = 0, ym = 0, sx2 = 0, sy2 = 0, sxy = 0;
            if (listaa.Count > 2)
            {
                for (int i = 0; i < listaa.Count; i++)
                {
                    xm += listaa[i].p.X;
                    ym += listaa[i].p.Y;
                }
                xm = xm / listaa.Count;
                ym = ym / listaa.Count;
                for (int i = 0; i < listaa.Count; i++)
                {
                    xi_xm.Add(listaa[i].p.X - xm);
                    xi_xm2.Add(Math.Pow(listaa[i].p.X - xm, 2));
                    yi_ym.Add(listaa[i].p.Y - ym);
                    yi_ym2.Add(Math.Pow(listaa[i].p.Y - ym, 2));
                    xi_yi.Add((listaa[i].p.X - xm) * (listaa[i].p.Y - ym));
                }
                for (int i = 0; i < listaa.Count; i++)
                {
                    sx2 += xi_xm2[i];
                    sy2 += yi_ym2[i];
                    sxy += xi_yi[i];
                }
                recta[0] = sxy / sx2;
                recta[1] = -(sxy / sx2) * xm + ym;
            }
            return recta[0];
        }
        public void MatrizAcuerdo()
        {
            List<double> l_x = new List<double>();
            List<double> l_y = new List<double>();
            List<int> Lista_aux = new List<int>();
            double suma_x,suma_x2,suma_x3,suma_x4;
            double suma_y, suma_xy, suma_xy2;
            for (int i=0;i<polilinea_perfil.Count;i++)
            {

                if (polilinea_perfil[i].tipo!=1 && polilinea_perfil[i].tipo != 2)
                {
                    Lista_aux.Add(i);
                    l_x.Add(polilinea_perfil[i].p.X);
                    l_y.Add(polilinea_perfil[i].p.Y);
                }
                else
                {
                    if (l_x.Count>0)
                    {
                        suma_x = 0;
                        suma_x2 = 0;
                        suma_x3 = 0;
                        suma_x4 = 0;
                        suma_y = 0;
                        suma_xy = 0;
                        suma_xy2 = 0;
                        for (int t = 0; t < l_x.Count; t++)
                        {
                            suma_x += l_x[t];
                            suma_x2 += Math.Pow(l_x[t], 2);
                            suma_x3 += Math.Pow(l_x[t], 3);
                            suma_x4 += Math.Pow(l_x[t], 4);
                            suma_y += l_y[t];
                            suma_xy += l_x[t] * l_y[t];
                            suma_xy2 += l_y[t] * Math.Pow(l_x[t], 2);
                        }
                        suma_x /= l_x.Count;
                        suma_x2 /= l_x.Count;
                        suma_x3 /= l_x.Count;
                        suma_x4 /= l_x.Count;
                        suma_y /= l_x.Count;
                        suma_xy /= l_x.Count;
                        suma_xy2 /= l_x.Count;
                        double[,] matrix=new double[3, 4];
                        matrix[0, 0] = 1;
                        matrix[0, 1] = suma_x;
                        matrix[0, 2] = suma_x2;
                        matrix[0, 3] = suma_y;

                        matrix[1, 0] = suma_x;
                        matrix[1, 1] = suma_x2;
                        matrix[1, 2] = suma_x3;
                        matrix[1, 3] = suma_xy;

                        matrix[2, 0] = suma_x2;
                        matrix[2, 1] = suma_x3;
                        matrix[2, 2] = suma_x4;
                        matrix[2, 3] = suma_xy2;
                        GJ(ref matrix,3,4);
                        for (int t = 0; t < Lista_aux.Count; t++)
                        {
                            polilinea_perfil[Lista_aux[t]].parabola.Add(matrix[2 , 3]);
                            polilinea_perfil[Lista_aux[t]].parabola.Add(matrix[1 , 3]);
                            polilinea_perfil[Lista_aux[t]].parabola.Add(matrix[0 , 3]);
                        }
                        if (Lista_aux.Count > 1)
                        {
                            Dibujar_Acuerdo(polilinea_perfil, Lista_aux[0], Lista_aux[Lista_aux.Count-1]);
                            Parabola p = new Parabola();
                            for (int r= Lista_aux[0];r<= Lista_aux[Lista_aux.Count - 1];r++)
                            {
                                p.Add_PuntoPerfil(polilinea_perfil[r]);
                            }
                            List<double> parabola = new List<double>();
                            parabola.Add(matrix[2, 3]);
                            parabola.Add(matrix[1, 3]);
                            parabola.Add(matrix[0, 3]);
                            p.Add_parabola(parabola);
                            Lista_parabolas.Add(p);
                        }
                        Lista_aux.Clear();
                        l_x.Clear();
                        l_y.Clear();
                    }
                }
            }
            if (l_x.Count > 0)
            {
                suma_x = 0;
                suma_x2 = 0;
                suma_x3 = 0;
                suma_x4 = 0;
                suma_y = 0;
                suma_xy = 0;
                suma_xy2 = 0;
                for (int t = 0; t < l_x.Count; t++)
                {
                    suma_x += l_x[t];
                    suma_x2 += Math.Pow(l_x[t], 2);
                    suma_x3 += Math.Pow(l_x[t], 3);
                    suma_x4 += Math.Pow(l_x[t], 4);
                    suma_y += l_y[t];
                    suma_xy += l_x[t] * l_y[t];
                    suma_xy2 += l_y[t] * Math.Pow(l_x[t], 2);
                }
                suma_x /= l_x.Count;
                suma_x2 /= l_x.Count;
                suma_x3 /= l_x.Count;
                suma_x4 /= l_x.Count;
                suma_y /= l_x.Count;
                suma_xy /= l_x.Count;
                suma_xy2 /= l_x.Count;
                double[,] matrix = new double[3, 4];
                matrix[0, 0] = 1;
                matrix[0, 1] = suma_x;
                matrix[0, 2] = suma_x2;
                matrix[0, 3] = suma_y;

                matrix[1, 0] = suma_x;
                matrix[1, 1] = suma_x2;
                matrix[1, 2] = suma_x3;
                matrix[1, 3] = suma_xy;

                matrix[2, 0] = suma_x2;
                matrix[2, 1] = suma_x3;
                matrix[2, 2] = suma_x4;
                matrix[2, 3] = suma_xy2;
                GJ(ref matrix, 3, 4);
                for (int t = 0; t < Lista_aux.Count; t++)
                {
                    polilinea_perfil[Lista_aux[t]].parabola.Add(matrix[2, 3]);
                    polilinea_perfil[Lista_aux[t]].parabola.Add(matrix[1, 3]);
                    polilinea_perfil[Lista_aux[t]].parabola.Add(matrix[0, 3]);
                }
                if (Lista_aux.Count > 1)
                {
                    Dibujar_Acuerdo(polilinea_perfil, Lista_aux[0], Lista_aux[Lista_aux.Count - 1]);
                    Parabola p = new Parabola();
                    for (int r = Lista_aux[0]; r <= Lista_aux[Lista_aux.Count - 1]; r++)
                    {
                        p.Add_PuntoPerfil(polilinea_perfil[r]);
                    }
                    List<double> parabola = new List<double>();
                    parabola.Add(matrix[2, 3]);
                    parabola.Add(matrix[1, 3]);
                    parabola.Add(matrix[0, 3]);
                    p.Add_parabola(parabola);
                    Lista_parabolas.Add(p);
                }
                Lista_aux.Clear();
                l_x.Clear();
                l_y.Clear();
            }
        }
        /// <summary>
        /// Se calculan los acuerdos con la polilinea sin suavizar inicial
        /// </summary>
        public void MatrizAcuerdo2()
        {
            List<double> l_x = new List<double>();
            List<double> l_y = new List<double>();
            List<int> Lista_aux = new List<int>();
            double suma_x, suma_x2, suma_x3, suma_x4;
            double suma_y, suma_xy, suma_xy2;
            int x = 0;
            for (int i = 0; i < polilinea_perfil_inicial.Count; i++)
            {
                if (lista_puntos_rectas.Count-1>=x)
                {
                    if (lista_puntos_rectas.Count==x+1)
                    {
                        Lista_aux.Add(i);
                        l_x.Add(polilinea_perfil_inicial[i].p.X);
                        l_y.Add(polilinea_perfil_inicial[i].p.Y);
                    }
                    else if (polilinea_perfil_inicial[i].p.X >= lista_puntos_rectas[x] && polilinea_perfil_inicial[i].p.X <= lista_puntos_rectas[x + 1])
                    {
                        Lista_aux.Add(i);
                        l_x.Add(polilinea_perfil_inicial[i].p.X);
                        l_y.Add(polilinea_perfil_inicial[i].p.Y);
                    }
                    else if (polilinea_perfil_inicial[i].p.X < lista_puntos_rectas[x] && polilinea_perfil_inicial[i].p.X > lista_puntos_rectas[x + 1])
                    {

                    }
                    else
                    {

                        if (l_x.Count > 0)
                        {
                            x += 2;
                            suma_x = 0;
                            suma_x2 = 0;
                            suma_x3 = 0;
                            suma_x4 = 0;
                            suma_y = 0;
                            suma_xy = 0;
                            suma_xy2 = 0;
                            for (int t = 0; t < l_x.Count; t++)
                            {
                                suma_x += l_x[t];
                                suma_x2 += Math.Pow(l_x[t], 2);
                                suma_x3 += Math.Pow(l_x[t], 3);
                                suma_x4 += Math.Pow(l_x[t], 4);
                                suma_y += l_y[t];
                                suma_xy += l_x[t] * l_y[t];
                                suma_xy2 += l_y[t] * Math.Pow(l_x[t], 2);
                            }
                            suma_x /= l_x.Count;
                            suma_x2 /= l_x.Count;
                            suma_x3 /= l_x.Count;
                            suma_x4 /= l_x.Count;
                            suma_y /= l_x.Count;
                            suma_xy /= l_x.Count;
                            suma_xy2 /= l_x.Count;
                            double[,] matrix = new double[3, 4];
                            matrix[0, 0] = 1;
                            matrix[0, 1] = suma_x;
                            matrix[0, 2] = suma_x2;
                            matrix[0, 3] = suma_y;

                            matrix[1, 0] = suma_x;
                            matrix[1, 1] = suma_x2;
                            matrix[1, 2] = suma_x3;
                            matrix[1, 3] = suma_xy;

                            matrix[2, 0] = suma_x2;
                            matrix[2, 1] = suma_x3;
                            matrix[2, 2] = suma_x4;
                            matrix[2, 3] = suma_xy2;
                            GJ(ref matrix, 3, 4);
                            for (int t = 0; t < Lista_aux.Count; t++)
                            {
                                polilinea_perfil_inicial[Lista_aux[t]].parabola.Add(matrix[2, 3]);
                                polilinea_perfil_inicial[Lista_aux[t]].parabola.Add(matrix[1, 3]);
                                polilinea_perfil_inicial[Lista_aux[t]].parabola.Add(matrix[0, 3]);
                            }
                            if (Lista_aux.Count > 1)
                            {
                                Dibujar_Acuerdo(polilinea_perfil_inicial, Lista_aux[0], Lista_aux[Lista_aux.Count - 1]);
                                Parabola p = new Parabola();
                                for (int r = Lista_aux[0]; r <= Lista_aux[Lista_aux.Count - 1]; r++)
                                {
                                    p.Add_PuntoPerfil(polilinea_perfil_inicial[r]);
                                }
                                List<double> parabola = new List<double>();
                                parabola.Add(matrix[2, 3]);
                                parabola.Add(matrix[1, 3]);
                                parabola.Add(matrix[0, 3]);
                                p.Add_parabola(parabola);
                                Lista_parabolas.Add(p);
                            }
                            Lista_aux.Clear();
                            l_x.Clear();
                            l_y.Clear();
                        }
                    }
                }
                
            }
            if (l_x.Count > 0)
            {
                suma_x = 0;
                suma_x2 = 0;
                suma_x3 = 0;
                suma_x4 = 0;
                suma_y = 0;
                suma_xy = 0;
                suma_xy2 = 0;
                for (int t = 0; t < l_x.Count; t++)
                {
                    suma_x += l_x[t];
                    suma_x2 += Math.Pow(l_x[t], 2);
                    suma_x3 += Math.Pow(l_x[t], 3);
                    suma_x4 += Math.Pow(l_x[t], 4);
                    suma_y += l_y[t];
                    suma_xy += l_x[t] * l_y[t];
                    suma_xy2 += l_y[t] * Math.Pow(l_x[t], 2);
                }
                suma_x /= l_x.Count;
                suma_x2 /= l_x.Count;
                suma_x3 /= l_x.Count;
                suma_x4 /= l_x.Count;
                suma_y /= l_x.Count;
                suma_xy /= l_x.Count;
                suma_xy2 /= l_x.Count;
                double[,] matrix = new double[3, 4];
                matrix[0, 0] = 1;
                matrix[0, 1] = suma_x;
                matrix[0, 2] = suma_x2;
                matrix[0, 3] = suma_y;

                matrix[1, 0] = suma_x;
                matrix[1, 1] = suma_x2;
                matrix[1, 2] = suma_x3;
                matrix[1, 3] = suma_xy;

                matrix[2, 0] = suma_x2;
                matrix[2, 1] = suma_x3;
                matrix[2, 2] = suma_x4;
                matrix[2, 3] = suma_xy2;
                GJ(ref matrix, 3, 4);
                for (int t = 0; t < Lista_aux.Count; t++)
                {
                    polilinea_perfil_inicial[Lista_aux[t]].parabola.Add(matrix[2, 3]);
                    polilinea_perfil_inicial[Lista_aux[t]].parabola.Add(matrix[1, 3]);
                    polilinea_perfil_inicial[Lista_aux[t]].parabola.Add(matrix[0, 3]);
                }
                if (Lista_aux.Count > 1)
                {
                    Dibujar_Acuerdo(polilinea_perfil_inicial, Lista_aux[0], Lista_aux[Lista_aux.Count - 1]);
                    Parabola p = new Parabola();
                    for (int r = Lista_aux[0]; r <= Lista_aux[Lista_aux.Count - 1]; r++)
                    {
                        p.Add_PuntoPerfil(polilinea_perfil_inicial[r]);
                    }
                    List<double> parabola = new List<double>();
                    parabola.Add(matrix[2, 3]);
                    parabola.Add(matrix[1, 3]);
                    parabola.Add(matrix[0, 3]);
                    p.Add_parabola(parabola);
                    Lista_parabolas.Add(p);
                }
                Lista_aux.Clear();
                l_x.Clear();
                l_y.Clear();
            }
        }
        /// <summary>
        /// Identifica los acuerdos que hay por cada tramo de giro
        /// </summary>
        public void MatrizAcuerdo3()
        {
            List<double> l_x = new List<double>();
            List<double> l_y = new List<double>();
            List<int> Lista_aux = new List<int>();
            double suma_x, suma_x2, suma_x3, suma_x4;
            double suma_y, suma_xy, suma_xy2;
            bool pintado = false;
            List<PuntoPerfil> list_aux = new List<PuntoPerfil>();
            double var_ac;
            int p_acuerdo=0;
            for (int s=0;s< lista_sentidos.Count;s++)
            {
                list_aux = lista_sentidos[s];
                pintado = false;
                for (int i = 0; i < list_aux.Count; i++)
                {

                    if (list_aux[i].tipo != 1 && list_aux[i].tipo != 2)
                    {
                        Lista_aux.Add(i);
                        l_x.Add(list_aux[i].p.X);
                        l_y.Add(list_aux[i].p.Y);
                    }
                    else
                    {
                        if (l_x.Count > 0)
                        {
                            suma_x = 0;
                            suma_x2 = 0;
                            suma_x3 = 0;
                            suma_x4 = 0;
                            suma_y = 0;
                            suma_xy = 0;
                            suma_xy2 = 0;
                            for (int t = 0; t < l_x.Count; t++)
                            {
                                suma_x += l_x[t];
                                suma_x2 += Math.Pow(l_x[t], 2);
                                suma_x3 += Math.Pow(l_x[t], 3);
                                suma_x4 += Math.Pow(l_x[t], 4);
                                suma_y += l_y[t];
                                suma_xy += l_x[t] * l_y[t];
                                suma_xy2 += l_y[t] * Math.Pow(l_x[t], 2);
                            }
                            suma_x /= l_x.Count;
                            suma_x2 /= l_x.Count;
                            suma_x3 /= l_x.Count;
                            suma_x4 /= l_x.Count;
                            suma_y /= l_x.Count;
                            suma_xy /= l_x.Count;
                            suma_xy2 /= l_x.Count;
                            double[,] matrix = new double[3, 4];
                            matrix[0, 0] = 1;
                            matrix[0, 1] = suma_x;
                            matrix[0, 2] = suma_x2;
                            matrix[0, 3] = suma_y;

                            matrix[1, 0] = suma_x;
                            matrix[1, 1] = suma_x2;
                            matrix[1, 2] = suma_x3;
                            matrix[1, 3] = suma_xy;

                            matrix[2, 0] = suma_x2;
                            matrix[2, 1] = suma_x3;
                            matrix[2, 2] = suma_x4;
                            matrix[2, 3] = suma_xy2;
                            GJ(ref matrix, 3, 4);
                            for (int t = 0; t < Lista_aux.Count; t++)
                            {
                                list_aux[Lista_aux[t]].parabola.Add(matrix[2, 3]);
                                list_aux[Lista_aux[t]].parabola.Add(matrix[1, 3]);
                                list_aux[Lista_aux[t]].parabola.Add(matrix[0, 3]);
                            }
                            if (Lista_aux.Count > 1)
                            {
                                Dibujar_Acuerdo(list_aux, Lista_aux[0], Lista_aux[Lista_aux.Count - 1]);
                                pintado = true;
                                Parabola p = new Parabola();
                                for (int r = Lista_aux[0]; r <= Lista_aux[Lista_aux.Count - 1]; r++)
                                {
                                    p.Add_PuntoPerfil(list_aux[r]);
                                }
                                List<double> parabola = new List<double>();
                                parabola.Add(matrix[2, 3]);
                                parabola.Add(matrix[1, 3]);
                                parabola.Add(matrix[0, 3]);
                                p.Add_parabola(parabola);
                                Lista_parabolas.Add(p);
                            }
                            Lista_aux.Clear();
                            l_x.Clear();
                            l_y.Clear();
                        }
                    }
                }
                if (l_x.Count > 0)
                {
                    suma_x = 0;
                    suma_x2 = 0;
                    suma_x3 = 0;
                    suma_x4 = 0;
                    suma_y = 0;
                    suma_xy = 0;
                    suma_xy2 = 0;
                    for (int t = 0; t < l_x.Count; t++)
                    {
                        suma_x += l_x[t];
                        suma_x2 += Math.Pow(l_x[t], 2);
                        suma_x3 += Math.Pow(l_x[t], 3);
                        suma_x4 += Math.Pow(l_x[t], 4);
                        suma_y += l_y[t];
                        suma_xy += l_x[t] * l_y[t];
                        suma_xy2 += l_y[t] * Math.Pow(l_x[t], 2);
                    }
                    suma_x /= l_x.Count;
                    suma_x2 /= l_x.Count;
                    suma_x3 /= l_x.Count;
                    suma_x4 /= l_x.Count;
                    suma_y /= l_x.Count;
                    suma_xy /= l_x.Count;
                    suma_xy2 /= l_x.Count;
                    double[,] matrix = new double[3, 4];
                    matrix[0, 0] = 1;
                    matrix[0, 1] = suma_x;
                    matrix[0, 2] = suma_x2;
                    matrix[0, 3] = suma_y;

                    matrix[1, 0] = suma_x;
                    matrix[1, 1] = suma_x2;
                    matrix[1, 2] = suma_x3;
                    matrix[1, 3] = suma_xy;

                    matrix[2, 0] = suma_x2;
                    matrix[2, 1] = suma_x3;
                    matrix[2, 2] = suma_x4;
                    matrix[2, 3] = suma_xy2;
                    GJ(ref matrix, 3, 4);
                    for (int t = 0; t < Lista_aux.Count; t++)
                    {
                        list_aux[Lista_aux[t]].parabola.Add(matrix[2, 3]);
                        list_aux[Lista_aux[t]].parabola.Add(matrix[1, 3]);
                        list_aux[Lista_aux[t]].parabola.Add(matrix[0, 3]);
                    }
                    if (Lista_aux.Count > 1)
                    {
                        Dibujar_Acuerdo(list_aux, Lista_aux[0], Lista_aux[Lista_aux.Count - 1]);
                        pintado = true;
                        Parabola p = new Parabola();
                        for (int r = Lista_aux[0]; r <= Lista_aux[Lista_aux.Count - 1]; r++)
                        {
                            p.Add_PuntoPerfil(list_aux[r]);
                        }
                        List<double> parabola = new List<double>();
                        parabola.Add(matrix[2, 3]);
                        parabola.Add(matrix[1, 3]);
                        parabola.Add(matrix[0, 3]);
                        p.Add_parabola(parabola);
                        Lista_parabolas.Add(p);
                    }
                    Lista_aux.Clear();
                    l_x.Clear();
                    l_y.Clear();
                }
                if (!pintado)
                {
                    if (list_aux.Count > 2)
                    {
                        var_ac = 0;
                        p_acuerdo = 0;
                        for (int i = 0; i < list_aux.Count; i++)
                        {
                            if (list_aux[i].varianza_a > var_ac)
                            {
                                p_acuerdo = i;
                            }

                            if (list_aux[i].varianza_a > 0)
                            {
                                var_ac = list_aux[i].varianza_a;
                            }
                        }

                        if (p_acuerdo>0 && p_acuerdo<list_aux.Count-1)
                        {
                            Lista_aux.Add(p_acuerdo - 1);
                            l_x.Add(list_aux[p_acuerdo - 1].p.X);
                            l_y.Add(list_aux[p_acuerdo - 1].p.Y);

                            Lista_aux.Add(p_acuerdo);
                            l_x.Add(list_aux[p_acuerdo].p.X);
                            l_y.Add(list_aux[p_acuerdo].p.Y);

                            Lista_aux.Add(p_acuerdo + 1);
                            l_x.Add(list_aux[p_acuerdo + 1].p.X);
                            l_y.Add(list_aux[p_acuerdo + 1].p.Y);
                        }else if (p_acuerdo > 1)
                        {
                            p_acuerdo--;
                            Lista_aux.Add(p_acuerdo - 1);
                            l_x.Add(list_aux[p_acuerdo - 1].p.X);
                            l_y.Add(list_aux[p_acuerdo - 1].p.Y);

                            Lista_aux.Add(p_acuerdo);
                            l_x.Add(list_aux[p_acuerdo].p.X);
                            l_y.Add(list_aux[p_acuerdo].p.Y);

                            Lista_aux.Add(p_acuerdo + 1);
                            l_x.Add(list_aux[p_acuerdo + 1].p.X);
                            l_y.Add(list_aux[p_acuerdo + 1].p.Y);
                        }
                        else if (p_acuerdo < list_aux.Count - 2)
                        {
                            p_acuerdo++;
                            Lista_aux.Add(p_acuerdo - 1);
                            l_x.Add(list_aux[p_acuerdo - 1].p.X);
                            l_y.Add(list_aux[p_acuerdo - 1].p.Y);

                            Lista_aux.Add(p_acuerdo);
                            l_x.Add(list_aux[p_acuerdo].p.X);
                            l_y.Add(list_aux[p_acuerdo].p.Y);

                            Lista_aux.Add(p_acuerdo + 1);
                            l_x.Add(list_aux[p_acuerdo + 1].p.X);
                            l_y.Add(list_aux[p_acuerdo + 1].p.Y);
                        }
                        if (l_x.Count > 0)
                        {
                            suma_x = 0;
                            suma_x2 = 0;
                            suma_x3 = 0;
                            suma_x4 = 0;
                            suma_y = 0;
                            suma_xy = 0;
                            suma_xy2 = 0;
                            for (int t = 0; t < l_x.Count; t++)
                            {
                                suma_x += l_x[t];
                                suma_x2 += Math.Pow(l_x[t], 2);
                                suma_x3 += Math.Pow(l_x[t], 3);
                                suma_x4 += Math.Pow(l_x[t], 4);
                                suma_y += l_y[t];
                                suma_xy += l_x[t] * l_y[t];
                                suma_xy2 += l_y[t] * Math.Pow(l_x[t], 2);
                            }
                            suma_x /= l_x.Count;
                            suma_x2 /= l_x.Count;
                            suma_x3 /= l_x.Count;
                            suma_x4 /= l_x.Count;
                            suma_y /= l_x.Count;
                            suma_xy /= l_x.Count;
                            suma_xy2 /= l_x.Count;
                            double[,] matrix = new double[3, 4];
                            matrix[0, 0] = 1;
                            matrix[0, 1] = suma_x;
                            matrix[0, 2] = suma_x2;
                            matrix[0, 3] = suma_y;

                            matrix[1, 0] = suma_x;
                            matrix[1, 1] = suma_x2;
                            matrix[1, 2] = suma_x3;
                            matrix[1, 3] = suma_xy;

                            matrix[2, 0] = suma_x2;
                            matrix[2, 1] = suma_x3;
                            matrix[2, 2] = suma_x4;
                            matrix[2, 3] = suma_xy2;
                            GJ(ref matrix, 3, 4);
                            for (int t = 0; t < Lista_aux.Count; t++)
                            {
                                list_aux[Lista_aux[t]].parabola.Add(matrix[2, 3]);
                                list_aux[Lista_aux[t]].parabola.Add(matrix[1, 3]);
                                list_aux[Lista_aux[t]].parabola.Add(matrix[0, 3]);
                            }
                            if (Lista_aux.Count > 1)
                            {
                                Dibujar_Acuerdo(list_aux, Lista_aux[0], Lista_aux[Lista_aux.Count - 1]);
                                pintado = true;
                                Parabola p = new Parabola();
                                for (int r = Lista_aux[0]; r <= Lista_aux[Lista_aux.Count - 1]; r++)
                                {
                                    p.Add_PuntoPerfil(list_aux[r]);
                                }
                                List<double> parabola = new List<double>();
                                parabola.Add(matrix[2, 3]);
                                parabola.Add(matrix[1, 3]);
                                parabola.Add(matrix[0, 3]);
                                p.Add_parabola(parabola);
                                Lista_parabolas.Add(p);
                            }
                            Lista_aux.Clear();
                            l_x.Clear();
                            l_y.Clear();
                        }
                    }
                   
                }
            }

            
        }

        /// <summary>
        /// Resuelve una matriz de nfilas por ncolumnas
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="filas"></param>
        /// <param name="columnas"></param>
        private void GJ(ref double[,] matrix, int filas, int columnas)
        {
            for (int fpivot = 0; fpivot < filas; fpivot++)
            {

                double nor = matrix[fpivot, fpivot];

                for (int i = 0; i < columnas; i++)
                {
                    matrix[fpivot, i] = matrix[fpivot, i] / nor;
                }

                int f = fpivot + 1;
                if (f == filas) f = 0;

                for (int fila = 0; fila < filas - 1; fila++)
                {
                    double k = matrix[f, fpivot];

                    for (int c = fpivot; c < columnas; c++)
                    {
                        matrix[f, c] = matrix[f, c] - (k * matrix[fpivot, c]);
                    }

                    if (f == filas - 1) f = 0;
                    else f++;
                }
            }
        }
        private void Dibujar_Acuerdo(List<PuntoPerfil> Parabola,int ini,int fin)
        {
        /*    using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    Polyline miEje = new Polyline();
                    int index = 0;

                    for (int i=ini;i<=fin;i++)
                    {
                        double x2 = Parabola[i].parabola[0];
                        double x1 = Parabola[i].parabola[1];
                        double x = Parabola[i].parabola[2];

                        double y = (Parabola[i].p.X * Parabola[i].p.X) * x2 + Parabola[i].p.X * x1 + x;//x^2+x+c

                        miEje.AddVertexAt(index, new Point2d(Parabola[i].p.X, y*escala), 0, 0, 0);
                        index++;
                    }
                    engCadNet.oLayer.addLayer("Acuerdo", 1, false);
                    miEje.Layer = "Acuerdo";

                    btr.AppendEntity(miEje);
                    tr.AddNewlyCreatedDBObject(miEje, true);

                    oCadManager.thisEditor.UpdateScreen();

                    tr.Commit();
                }
            }*/
        }
        private void Dibujar_Acuerdo(Parabola parabola,int paso)
        {
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    Polyline miEje = new Polyline();
                    int index = 0;
                    double x2 = parabola.parabola[0];
                    double x1 = parabola.parabola[1];
                    double x = parabola.parabola[2];
                    double pk_ini = parabola.polilinea_perfil[0].p.X;
                    double pk_fin = parabola.polilinea_perfil[parabola.polilinea_perfil.Count-1].p.X;
                    double pk=pk_ini;
                    double y;
                    for (int i = 0;pk<pk_fin ; i++)
                    {
                        y = (pk * pk) * x2 + pk * x1 + x;//x^2+x+c

                        miEje.AddVertexAt(i, new Point2d(pk+x_ins, y * escala+y_ins), 0, 0, 0);
                        pk = pk + 0.1;
                        index = i+1;
                    }
                    y = (pk_fin * pk_fin) * x2 + pk_fin * x1 + x;//x^2+x+c

                    miEje.AddVertexAt(index, new Point2d(pk_fin + x_ins, y * escala + y_ins), 0, 0, 0);

                    /* for (int i = 0; i < parabola.polilinea_perfil.Count; i++)
                     {
                         double y = (parabola.polilinea_perfil[i].p.X * parabola.polilinea_perfil[i].p.X) * x2 + parabola.polilinea_perfil[i].p.X * x1 + x;//x^2+x+c

                         miEje.AddVertexAt(index, new Point2d(parabola.polilinea_perfil[i].p.X, y * escala), 0, 0, 0);
                         index++;
                     }*/
                     
                    engCadNet.oLayer.addLayer("Acuerdo-"+paso, 1, false);
                    miEje.Layer = "Acuerdo-" + paso;

                    btr.AppendEntity(miEje);
                    tr.AddNewlyCreatedDBObject(miEje, true);

                    oCadManager.thisEditor.UpdateScreen();

                    tr.Commit();
                }
            }
        }
        public void PuntoInflexion()
        {
            /*
             * Se resuelve la ecuacion de segundo grado para el acuerdo
             */
            double a, b, c;
            var t=Lista_parabolas;
            for (int i=0;i< Lista_parabolas.Count;i++)
            {
                a = Lista_parabolas[i].parabola[0];
                b = Lista_parabolas[i].parabola[1];
                c = Lista_parabolas[i].parabola[2];
                Lista_parabolas[i].max_min = (-b) / (2 * a);
            }


        }
        public void CalculoEntreParabolas_Dibujar()
        {
            List<Point2d> lista_rectas_aux;
            for (int i = 0; i < Lista_parabolas.Count - 1; i++)
            {
                double a = 0, b = 0, c = 0, d = 0, e = 0, f = 0;
                double x1, x2;
                double ec_a, ec_b, ec_c, r1, r2;
                a = Lista_parabolas[i].parabola[0];
                b = Lista_parabolas[i].parabola[1];
                c = Lista_parabolas[i].parabola[2];

                d = Lista_parabolas[i + 1].parabola[0];
                e = Lista_parabolas[i + 1].parabola[1];
                f = Lista_parabolas[i + 1].parabola[2];

                //prueba
                /*  a = 0.00101023076138361;
                  b = -0.917444142349561;
                  c = 1038.12809913492;

                  d = 0.00308817329909175;
                  e = -3.02232478823384;
                  f = 1571.01983139839;*/

                ec_a = d - ((d * d) / a) - (2 * d) + ((2 * (d * d)) / a);
                ec_b = e - (((e - b) / a) * d) - ((b * d) / a) - (e - b) - b + (((e - b) / a) * 2 * d) + ((b * d) / a);
                ec_c = f - (((e - b) * (e - b)) / (4 * a)) - ((b * e - (b * b)) / (2 * a)) - c + (((e - b) * (e - b)) / (2 * a)) + ((b * e) / (2 * a)) - ((b * b) / (2 * a));

                r1 = (-ec_b + (Math.Sqrt(Math.Pow(ec_b, 2) - 4 * ec_a * ec_c))) / (2 * ec_a);
                r2 = (-ec_b - (Math.Sqrt(Math.Pow(ec_b, 2) - 4 * ec_a * ec_c))) / (2 * ec_a);

                x1 = (e - b + 2 * d * r1) / (2 * a);
                x2 = (e - b + 2 * d * r2) / (2 * a);

                double y = (x2 * x2) * a + x2 * b + c;//x^2+x+c
                double y2 = (r2 * r2) * d + r2 * e + f;//x^2+x+c
                Point2d p1 = new Point2d(x2, y);
                Point2d p2 = new Point2d(r2, y2);
                if (x2 < r2)
                {
                    lista_rectas_aux = new List<Point2d>();
                    lista_rectas_aux.Add(p1);
                    lista_rectas_aux.Add(p2);
                    Pendiente pendiente = new Pendiente(lista_rectas_aux);
                    Lista_rectas.Add(pendiente);
                    //Dibujar_r(p1, p2);
                }
                else
                {
                    y = (x1 * x1) * a + x1 * b + c;//x^2+x+c
                    y2 = (r1 * r1) * d + r1 * e + f;//x^2+x+c
                    Point2d p3 = new Point2d(x1, y );
                    Point2d p4 = new Point2d(r1, y2 );

                    if (x1 < r1)
                    {
                        lista_rectas_aux = new List<Point2d>();
                        lista_rectas_aux.Add(p3);
                        lista_rectas_aux.Add(p4);
                        Pendiente pendiente = new Pendiente(lista_rectas_aux);
                        Lista_rectas.Add(pendiente);
                        //Dibujar_r(p3, p4);
                    }
                    else
                    {
                       
                    }
                }
            }
        }
        
        public void CalcularEntreParabolas()
        {
            int contador = 0;
            for (int i=0;i< Lista_parabolas.Count-1;i++)
            {
                double a = 0, b = 0, c = 0, d = 0, e = 0, f = 0;
                double x1, x2;
                double ec_a, ec_b, ec_c, r1, r2;
                a = Lista_parabolas[i].parabola[0];
                b = Lista_parabolas[i].parabola[1];
                c = Lista_parabolas[i].parabola[2];

                d = Lista_parabolas[i+1].parabola[0];
                e = Lista_parabolas[i+1].parabola[1];
                f = Lista_parabolas[i+1].parabola[2];

                ec_a = d - ((d * d) / a) - (2 * d) + ((2 * (d * d)) / a);
                ec_b = e - (((e - b) / a) * d) - ((b * d) / a) - (e - b) - b + (((e - b) / a) * 2 * d) + ((b * d) / a);
                ec_c = f - (((e - b) * (e - b)) / (4 * a)) - ((b * e - (b * b)) / (2 * a)) - c + (((e - b) * (e - b)) / (2 * a)) + ((b * e) / (2 * a)) - ((b * b) / (2 * a));

                r1 = (-ec_b + (Math.Sqrt(Math.Pow(ec_b, 2) - 4 * ec_a * ec_c))) / (2 * ec_a);
                r2 = (-ec_b - (Math.Sqrt(Math.Pow(ec_b, 2) - 4 * ec_a * ec_c))) / (2 * ec_a);

                x1 = (e - b + 2 * d * r1) / (2 * a);
                x2 = (e - b + 2 * d * r2) / (2 * a);

                double y = (x2 * x2) * a + x2 * b + c;//x^2+x+c
                double y2 = (r2 * r2) * d + r2 * e + f;//x^2+x+c
                Point2d p1 = new Point2d(x2, y );
                Point2d p2 = new Point2d(r2, y2 );

               
                if (x2<r2)
                {
                }
                else
                {
                    y = (x1 * x1) * a + x1 * b + c;//x^2+x+c
                    y2 = (r1 * r1) * d + r1 * e + f;//x^2+x+c
                    Point2d p3 = new Point2d(x1, y );
                    Point2d p4 = new Point2d(r1, y2 );
                    if (Lista_parabolas[i].polilinea_perfil[0].p.X > 1140 && Lista_parabolas[i].polilinea_perfil[0].p.X < 1170)
                    {
                        //Dibujar_r(p3, p4, 5);
                    }
                    
                    if (x1 < r1)
                    {
                    }
                    else
                    {
                        if ((a>0 && d<0) || (a < 0 && d > 0))
                        {
                            if (a>0)
                            {
                                ReducirParabola(i, 2);
                                ReducirParabola(i + 1, 1);
                            }
                            else
                            {
                                ReducirParabola(i, 1);
                                ReducirParabola(i + 1, 2);
                            }
                            i = -1;



                        }
                        else
                        {
                            if (a>0)
                            {
                                if (a>d)
                                {
                                    ReducirParabola(i + 1, 2);
                                }
                                else
                                {
                                    ReducirParabola(i, 2);
                                }
                            }
                            else
                            {
                                if (a < d)
                                {
                                    ReducirParabola(i + 1, 1);
                                }
                                else
                                {
                                    ReducirParabola(i, 1);
                                }
                            }
                            i = -1;
                        }
                        //Dibujar_r(p1, p2);
                        //Dibujar_r(p3, p4);
                    }
                }
            }
            

        }
        private Tuple<double, double, double> Clusterizacion(List<Punto> lista)
        {
            double centrox = -1, centroy = -1, radio = -1;
            double sumax = 0;
            double sumay = 0;
            double sumaz = 0;
            double sumax2 = 0;
            double sumay2 = 0;
            double sumaxz = 0;
            double sumayz = 0;
            double sumaxy = 0;
            if (lista.Count > 0)
            {
                double x0 = lista[0].p.X;
                double y0 = lista[0].p.Y;
                double x, y, z, error;
                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    x = lista[i].p.X - x0;
                    y = lista[i].p.Y - y0;
                    sumax += x;
                    sumay += y;
                    z = -(x * x + y * y);
                    sumaz += z;
                    sumax2 += x * x;
                    sumay2 += y * y;
                    sumaxz += x * z;
                    sumayz += y * z;
                    sumaxy += x * y;
                }
                double[,] matrix = new double[3, 4] { { sumax2, sumaxy, sumax, sumaxz }, { sumaxy, sumay2, sumay, sumayz }, { sumax, sumay, lista.Count - 1 - 0 + 1, sumaz } };
                GJ(ref matrix, 3, 4);
                double xc_centro = matrix[0, 3] / (-2);
                double yc_centro = matrix[1, 3] / (-2);
                double cuadrado1 = Math.Pow(xc_centro, 2);
                double cuadrado2 = Math.Pow(yc_centro, 2);
                double r_centro1 = matrix[2, 3] - cuadrado1 - cuadrado2;
                double r_centro = Math.Pow(Math.Abs(r_centro1), 0.5);
                error = 0;
                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    error += Math.Pow(Math.Pow(Math.Pow(lista[i].p.X - (xc_centro + x0), 2) + Math.Pow(lista[i].p.Y - (yc_centro + y0), 2), 0.5) - r_centro, 2);
                }
                error = error / (lista.Count - 1 - 0 + 1);
                if (double.IsNaN(error) || double.IsInfinity(error))
                {

                }
                else
                {
                    centrox = xc_centro + x0;
                    centroy = yc_centro + y0;
                    radio = r_centro;
                }
                return Tuple.Create(centrox, centroy, radio);
            }
            else
            {
                return Tuple.Create(centrox, centroy, radio);
            }

        }
        public List<Punto> Suavizar(List<Punto> l)
        {
            List<Punto> Lista_suavizada = new List<Punto>();
            List<Punto> Lista_suavizada_temp = new List<Punto>();

            Lista_suavizada_temp.Add(new Punto(l[0].p));
            for (int i = 0; i < l.Count - 1; i++)
            {
                Lista_suavizada_temp.Add(new Punto(new Point2d((l[i].p.X + l[i + 1].p.X) / 2, (l[i].p.Y + l[i + 1].p.Y) / 2)));
            }
            Lista_suavizada_temp.Add(new Punto(l[l.Count - 1].p));
            Lista_suavizada = new List<Punto>(Lista_suavizada_temp);
            /*for (int t = 0; t < 10; t++)
            {
                Lista_suavizada_temp = new List<Punto>();
                Lista_suavizada_temp.Add(new Punto(Lista_suavizada[0].p));
                for (int i = 0; i < Lista_suavizada.Count - 1; i++)
                {
                    Lista_suavizada_temp.Add(new Punto(new Point2d((Lista_suavizada[i].p.X + Lista_suavizada[i + 1].p.X) / 2, (Lista_suavizada[i].p.Y + Lista_suavizada[i + 1].p.Y) / 2)));
                }
                Lista_suavizada_temp.Add(new Punto(Lista_suavizada[Lista_suavizada.Count - 1].p));
                Lista_suavizada = new List<Punto>(Lista_suavizada_temp);
            }
            Lista_suavizada_temp = new List<Punto>();
            for (int i = 0; i < Lista_suavizada.Count - 1; i++)
            {
                Lista_suavizada_temp.Add(new Punto(new Point2d((Lista_suavizada[i].p.X + Lista_suavizada[i + 1].p.X) / 2, (Lista_suavizada[i].p.Y + Lista_suavizada[i + 1].p.Y) / 2)));
            }
            Lista_suavizada = new List<Punto>(Lista_suavizada_temp);
            Lista_suavizada.Insert(0, l[0]);
            Lista_suavizada.Add(l[l.Count-1]);*/
            //RellenarDatos();
            //Dibujar_Suavizado(Lista_suavizada);
            return Lista_suavizada;
        }
        public void Suavizar_Max_Min(int n_suavizados)
        {
            
            for (int t=0;t< polilinea_perfil.Count; t++)
            {

                if (polilinea_perfil[t].vertice==1 || polilinea_perfil[t].vertice == 1)
                {
                    for (int i = 0; i < n_suavizados; i++)
                    {
                        polilinea = Suavizar(polilinea);
                        Vaciar_Puntos();
                        RellenarDatos();
                    }

                }
                
            }
            

        }
        public void QuitarSuavizado()
        {
            for (int i = 0; i < polilinea_inicial.Count; i++)
            {
                polilinea_perfil_inicial.Add(new PuntoPerfil(polilinea_inicial[i]));
            }
            RellenarDatos_inicial();
            for (int i = 2; i < Polilinea_Perfil.Count - 2; i++)
            {
                if(Polilinea_Perfil[i-1].tipo== Polilinea_Perfil[i + 1].tipo)
                {
                    Polilinea_Perfil[i].tipo = Polilinea_Perfil[i - 1].tipo;
                }
                if (Polilinea_Perfil[i - 1].tipo == 1 && Polilinea_Perfil[i + 1].tipo==2)
                {
                    Polilinea_Perfil[i].tipo = Polilinea_Perfil[i - 1].tipo;
                }
                if (Polilinea_Perfil[i - 1].tipo == 2 && Polilinea_Perfil[i + 1].tipo==1)
                {
                    Polilinea_Perfil[i].tipo = Polilinea_Perfil[i - 1].tipo;
                }

            }
            Point2d p = new Point2d();
            for (int i=2;i<Polilinea_Perfil.Count-2;i++)
            {
                if (!double.IsNaN(Polilinea_Perfil[i].varianza_a))
                {
                    if ((Polilinea_Perfil[i].tipo == 1 || Polilinea_Perfil[i].tipo == 2) && Polilinea_Perfil[i - 1].tipo == 0 && !double.IsNaN(Polilinea_Perfil[i-1].varianza_a))
                    {
                        lista_puntos_rectas.Add(Polilinea_Perfil[i - 1].p.X);
                    }
                    if ((Polilinea_Perfil[i - 1].tipo == 1 || Polilinea_Perfil[i - 1].tipo == 2) && Polilinea_Perfil[i].tipo == 0)
                    {
                        lista_puntos_rectas.Add(Polilinea_Perfil[i].p.X);
                    }
                }

                
            }
        }
        private void Iguales()
        {
            double x = polilinea[0].p.X;
            for (int i=1;i<polilinea.Count-1;i++)
            {
                if (Math.Truncate(polilinea[i].p.X*10000)== Math.Truncate(x * 10000))
                {
                    polilinea.RemoveAt(i);
                    i--;
                }
                else
                {
                    x = polilinea[i].p.X;
                }
            }
        }
        /// <summary>
        /// Reduce o aumenta una parábola
        /// </summary>
        /// <param name="p1">parabola a reducir</param>
        /// <param name="op">1 para reducir y 2 para aumentar</param>
        public void ReducirParabola(int p,int op)
        {
            double a = Lista_parabolas[p].parabola[0];
            double b = Lista_parabolas[p].parabola[1];
            double c = Lista_parabolas[p].parabola[2];
            double d, e, f;
            //double x0 = (-b) / (2 * a);
            double xm = (Lista_parabolas[p].polilinea_perfil[0].p.X + Lista_parabolas[p].polilinea_perfil[Lista_parabolas[p].polilinea_perfil.Count-1].p.X)/2;
            if (p==0)
            {
                xm = Lista_parabolas[p].polilinea_perfil[0].p.X;
            }
            if (p == Lista_parabolas.Count-1)
            {
                xm = Lista_parabolas[p].polilinea_perfil[Lista_parabolas[p].polilinea_perfil.Count - 1].p.X;
            }
            double ym = Lista_parabolas[p].polilinea_perfil[Lista_parabolas[p].polilinea_perfil.Count / 2].p.Y;
            //Se quitan estos 2 apartados para que la parabola reduzca sobre el punto medio y no sobre cotas maximas o minimas
            /*if ((Lista_parabolas[p].polilinea_perfil[0].pendiente > 0 && Lista_parabolas[p].polilinea_perfil[Lista_parabolas[p].polilinea_perfil.Count - 1].pendiente < 0))
            {
                for (int i=0;i< Lista_parabolas[p].polilinea_perfil.Count-1;i++)
                {
                    if (ym< Lista_parabolas[p].polilinea_perfil[i].p.Y)
                    {
                        xm = Lista_parabolas[p].polilinea_perfil[i].p.X;
                        ym = Lista_parabolas[p].polilinea_perfil[i].p.Y;
                    }
                }
            }
            if ((Lista_parabolas[p].polilinea_perfil[0].pendiente < 0 && Lista_parabolas[p].polilinea_perfil[Lista_parabolas[p].polilinea_perfil.Count - 1].pendiente > 0))
            {
                for (int i = 0; i < Lista_parabolas[p].polilinea_perfil.Count - 1; i++)
                {
                    if (ym > Lista_parabolas[p].polilinea_perfil[i].p.Y)
                    {
                        xm = Lista_parabolas[p].polilinea_perfil[i].p.X;
                        ym = Lista_parabolas[p].polilinea_perfil[i].p.Y;
                    }
                }
            }*/

            if (op==1)
            {
                d = a - 0.0000001;
            }
            else
            {
                d = a + 0.0000001;
            }

            /*e = b * d / a;
            f = a * (x0 * x0) + b * x0 + c - d * (x0 * x0) - e * x0;
            */
            e = 2 * a * xm + b - 2 * d * xm;
            f = a * (xm * xm) + b * xm + c - d * (xm * xm) - e * xm;
            Lista_parabolas[p].parabola[0] = d;
            Lista_parabolas[p].parabola[1] = e;
            Lista_parabolas[p].parabola[2] = f;

        }
        public void Distancia_Puntos_Resultado(List<EjeDeTrazado.componentes.Componente> Mcomponenetes, List<Point3d> lista_original_3d)
        {
            List<Point3d> Polilinea3d = new List<Point3d>();

            Point3d p3d = new Point3d();
            foreach (var componente in Mcomponenetes)
            {
                foreach (var componentPoint in componente.getComponentPoints())
                {
                    p3d = new Point3d(componentPoint[0], componentPoint[1], 0);
                    Polilinea3d.Add(p3d);
                }
            }
            Polilinea3d = Erroneos(Polilinea3d);
            Polilinea3d = Dividir_Segmentos_Largos(Polilinea3d);
            foreach (Point3d p in lista_original_3d)
            {
                Polilinea3d_original.Add(p);
            }
            List<double> distancias = new List<double>();
            List<Point3d> Polilinea3d_aux = new List<Point3d>();
            List<Point3d> polilinea_aux = new List<Point3d>();
            int punto_p_o = 0;//punto de la polilinea original seleccionado
            int punto_ini = 0, punto_fin = Polilinea3d.Count - 1;
            double distancia = 0;
            double distancia_g = 0;
            Point3d p_aux = new Point3d(Polilinea3d[0].X, Polilinea3d[0].Y, Polilinea3d_original[0].Z);
            Polilinea3d_aux.Add(p_aux);
            for (int i = 1; i < Polilinea3d_original.Count; i++)
            {
                distancia_g = Distancia(new Point2d(Polilinea3d[0].X, Polilinea3d[0].Y), new Point2d(Polilinea3d_original[i].X, Polilinea3d_original[i].Y));
                for (int t = 0; t < Polilinea3d.Count; t++)
                {
                    distancia = Distancia(new Point2d(Polilinea3d[t].X, Polilinea3d[t].Y), new Point2d(Polilinea3d_original[i].X, Polilinea3d_original[i].Y));
                    if (distancia_g > distancia)
                    {
                        punto_p_o = t;
                        distancia_g = distancia;
                    }

                }
                distancias.Add(distancia_g);
                p_aux = new Point3d(Polilinea3d[punto_p_o].X, Polilinea3d[punto_p_o].Y, Polilinea3d_original[i].Z);
                Polilinea3d_aux.Add(p_aux);
            }
            p_aux = new Point3d(Polilinea3d[Polilinea3d.Count - 1].X, Polilinea3d[Polilinea3d.Count - 1].Y, Polilinea3d_original[Polilinea3d_original.Count - 1].Z);
            Polilinea3d_aux.Add(p_aux);
            Polilinea3d_original = Polilinea3d_aux;
        }
        public void Cotas_Trazado(List<EjeDeTrazado.componentes.Componente> Mcomponenetes,List<Point3d> lista_original_3d)
        {
            List<Point3d> Polilinea3d = new List<Point3d>();
            
            Point3d p3d = new Point3d();
            foreach (var componente in Mcomponenetes)
            {
                foreach (var componentPoint in componente.getComponentPoints())
                {
                    p3d = new Point3d(componentPoint[0], componentPoint[1],0);
                    Polilinea3d.Add(p3d);
                }
            }
            Polilinea3d=Erroneos(Polilinea3d);
            Polilinea3d= Dividir_Segmentos_Largos(Polilinea3d);
            string miFileOut = string.Empty;
            string line;
            if (lista_original_3d.Count==0)
            {
                OpenFileDialog miDialogo = new OpenFileDialog();
                miDialogo.Title = "APLITOP" + " | " + "Cargar polilinea 3d original";
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
                        if (separadas.Count() > 2)
                        {
                            p3d = new Point3d(double.Parse(separadas[0]), double.Parse(separadas[1]), double.Parse(separadas[2]));
                            Polilinea3d_original.Add(p3d);
                        }
                        else
                        {
                            MessageBox.Show("Este archivo no es correcto.\nNo es una polilinea 3D.", "Información");
                            break;
                        }
                    }
                    file.Close();
                }
            }
            else
            {
                foreach (Point3d p in lista_original_3d)
                {
                    Polilinea3d_original.Add(p);
                }
            }
            
            List<Point3d> Polilinea3d_aux = new List<Point3d>();
            if (Polilinea3d_original.Count>1)
            {
                /*double x2 = poli.Polilinea3d_Original[i].X;
                double y2 = poli.Polilinea3d_Original[i].Y;
                double distancia = Math.Sqrt(Math.Pow(x2 - x, 2) + Math.Pow(y2 - y, 2));
                double d_acumulada += distancia;*/
                List<Point3d> polilinea_aux = new List<Point3d>();
                int punto_p_o=0;//punto de la polilinea original seleccionado
                int punto_ini = 0, punto_fin = Polilinea3d.Count - 1;
                double distancia=0;
                double distancia_g = 0;
                Point3d p_aux = new Point3d(Polilinea3d[0].X, Polilinea3d[0].Y, Polilinea3d_original[0].Z);
                Polilinea3d_aux.Add(p_aux);
                for (int i = 1; i < Polilinea3d_original.Count; i++)
                {
                    distancia_g = Distancia(new Point2d(Polilinea3d[0].X, Polilinea3d[0].Y), new Point2d(Polilinea3d_original[i].X, Polilinea3d_original[i].Y));
                    for (int t = 0; t < Polilinea3d.Count; t++)
                    {
                        distancia = Distancia(new Point2d(Polilinea3d[t].X, Polilinea3d[t].Y), new Point2d(Polilinea3d_original[i].X, Polilinea3d_original[i].Y));
                        if (distancia_g > distancia)
                        {
                            punto_p_o = t;
                            distancia_g = distancia;
                        }
                    }
                    p_aux = new Point3d(Polilinea3d[punto_p_o].X, Polilinea3d[punto_p_o].Y, Polilinea3d_original[i].Z);
                    Polilinea3d_aux.Add(p_aux);
                }
                p_aux = new Point3d(Polilinea3d[Polilinea3d.Count-1].X, Polilinea3d[Polilinea3d.Count - 1].Y, Polilinea3d_original[Polilinea3d_original.Count-1].Z);
                Polilinea3d_aux.Add(p_aux);
                /*
                for (int t=0;t<Polilinea3d.Count;t++)
                {
                    punto_ini = 0;
                    punto_fin = Polilinea3d_original.Count - 1;
                    distancia = Distancia(new Point2d(Polilinea3d[t].X, Polilinea3d[t].Y), new Point2d(Polilinea3d_original[0].X, Polilinea3d_original[0].Y));
                    for (int i = 1; i < Polilinea3d_original.Count; i++)
                    {
                        if (Distancia(new Point2d(Polilinea3d[t].X, Polilinea3d[t].Y), new Point2d(Polilinea3d_original[i].X, Polilinea3d_original[i].Y)) < distancia)
                        {
                            punto_p_o = i;
                        }
                        distancia = Distancia(new Point2d(Polilinea3d[t].X, Polilinea3d[t].Y), new Point2d(Polilinea3d_original[i].X, Polilinea3d_original[i].Y));
                    }
                    if (punto_p_o-50> punto_ini)
                    {
                        punto_ini = punto_p_o - 50;
                    }
                    if (punto_p_o + 50 < punto_fin)
                    {
                        punto_fin = punto_p_o + 50;
                    }
                    for (int i = punto_ini; i < punto_fin-1; i++)
                    {
                        polilinea_aux.Add(Polilinea3d_original[i]);
                    }
                    /*for (int i = 0; i < Polilinea3d_original.Count; i++)
                    {
                        if (Distancia(new Point2d(Polilinea3d_original[punto_p_o].X, Polilinea3d_original[punto_p_o].Y), new Point2d(Polilinea3d_original[i].X, Polilinea3d_original[i].Y)) < 200)
                        {
                            polilinea_aux.Add(Polilinea3d_original[i]);
                        }
                    }*/
               /*     polilinea_aux =Dividir_Segmentos_Largos(polilinea_aux);
                    Point3d p_aux = new Point3d(Polilinea3d[punto_p_o].X, Polilinea3d[punto_p_o].Y, CotaPunto(Polilinea3d[punto_p_o], Polilinea3d_original));
                    Polilinea3d_aux.Add(p_aux);
                    polilinea_aux.Clear();
                }*/
                Polilinea3d_original = Polilinea3d_aux;

                //Polilinea3d_original = Dividir_Segmentos_Largos(Polilinea3d_original);
            }
            
                
        }
        /// <summary>
        /// Divide un segmento
        /// </summary>
        /// <param name="p1">punto 1 del segmento</param>
        /// <param name="p2">punto 2 del segmento</param>
        /// <param name="partes">partes en las que dividimos el segmento</param>
        /// <returns>puntos intermedios del segmento ya dividido</returns>
        private List<Point3d> Dividir_Segmento(Point3d p1, Point3d p2,int partes)
        {
            List<Point3d> segmento = new List<Point3d>();
            double r;
            double x, y,z,z_ac=0;
            z = (p1.Z - p2.Z) / partes;
            
            for (int i=1; i<=partes;i++)
            {
                r = ((double)partes - (double)i) / (double)i;
                x = (p1.X + r * p2.X) / (1 + r);
                y = (p1.Y + r * p2.Y) / (1 + r);
                if (segmento.Count==0)
                {
                    segmento.Add(new Point3d(x, y, p2.Z + z));
                    z_ac = z;
                }
                else
                {
                    z_ac += z;
                    segmento.Add(new Point3d(x, y, p2.Z + z_ac));
                }
               
            }
            return segmento;
        }
        /// <summary>
        /// Crea una polilinea cuyos tramos no tienen mas de 0.1 metros
        /// </summary>
        /// <param name="polilinea">polilinea original a dividir</param>
        /// <returns>polilinea ya dividida</returns>
        private List<Point3d> Dividir_Segmentos_Largos(List<Point3d> polilinea)
        {
            List<Point3d> polilinea_temp = new List<Point3d>();
            List<Point3d> segmento = new List<Point3d>();
            Point2d p1 = new Point2d();
            Point2d p2 = new Point2d();
            int partes = 1;
            for (int i=0;i< polilinea.Count-1;i++)
            {
                p1 = new Point2d(polilinea[i].X, polilinea[i].Y);
                p2 = new Point2d(polilinea[i+1].X, polilinea[i+1].Y);
                if (Distancia(p1,p2)>0.01)
                {
                    polilinea_temp.Add(polilinea[i]);
                    partes = (int)Math.Truncate(Distancia(p1, p2)*100);
                    if (partes>1)
                    {
                        segmento = Dividir_Segmento(polilinea[i+1], polilinea[i], partes);
                    }
                    else
                    {
                        segmento = Dividir_Segmento(polilinea[i+1], polilinea[i], 2);
                    }
                    for (int t=0;t< segmento.Count;t++)
                    {
                        polilinea_temp.Add(segmento[t]);
                    }
                }
                else
                {
                    polilinea_temp.Add(polilinea[i]);
                }
            }
            polilinea_temp.Add(polilinea[polilinea.Count-1]);
            return polilinea_temp;
        }
        private double CotaPunto(Point3d p, List<Point3d> polilinea_aux)
        {
            double cota = polilinea_aux[0].Z;
            double distancia = Distancia(new Point2d(p.X,p.Y), new Point2d(polilinea_aux[0].X, polilinea_aux[0].Y));
            for (int i=1;i< polilinea_aux.Count;i++)
            {
                if (Distancia(new Point2d(p.X, p.Y), new Point2d(polilinea_aux[i].X, polilinea_aux[i].Y))<distancia)
                {
                    cota = polilinea_aux[i].Z;
                }
                distancia = Distancia(new Point2d(p.X, p.Y), new Point2d(polilinea_aux[i].X, polilinea_aux[i].Y));
            }

            return cota;
        }
        /// <summary>
        /// Se elimina el error de un punto erroneo que se detecta a una distancia demasiado grande
        /// </summary>
        /// <param name="poli">polilinea a corregir</param>
        /// <returns>polilinea corregida</returns>
        private List<Point3d> Erroneos(List<Point3d> poli)
        {
            for (int i=0;i<poli.Count-2;i++)
            {
                if (Distancia(new Point2d(poli[i].X, poli[i].Y), new Point2d(poli[i + 1].X, poli[i + 1].Y)) >100000 && 
                    Distancia(new Point2d(poli[i+1].X, poli[i+1].Y), new Point2d(poli[i + 2].X, poli[i + 2].Y)) > 100000)
                {
                    poli.RemoveAt(i + 1);
                }
            }
            return poli;
        }
        private List<Punto> Duplicar_puntos_C(List<Punto> l)
        {
            List<Punto> aux = new List<Punto>();


            Punto p_aux;
            double az_final;
            aux.Add(l[0]);
            double xx = 0, yy = 0;
            double xc;
            double yc;
            double r;
            double az1;
            double az2;
            int i;
            for (i = 1; i < l.Count - 1; i++)
            {
                xc = ((l[i + 1].p.Y + l[i].p.Y) * 0.5 + ((l[i + 1].p.X - l[i].p.X) * (l[i + 1].p.X + l[i].p.X) * 0.5 / (l[i + 1].p.Y - l[i].p.Y)) - (l[i].p.Y + l[i - 1].p.Y) * 0.5 - ((l[i].p.X - l[i - 1].p.X) * (l[i].p.X + l[i - 1].p.X) * 0.5 / (l[i].p.Y - l[i - 1].p.Y))) / (((l[i + 1].p.X - l[i].p.X) / (l[i + 1].p.Y - l[i].p.Y)) - ((l[i].p.X - l[i - 1].p.X) / (l[i].p.Y - l[i - 1].p.Y)));
                yc = (l[i].p.Y + l[i - 1].p.Y) * 0.5 + (l[i].p.X - l[i - 1].p.X) * 0.5 * (l[i].p.X + l[i - 1].p.X) * Math.Pow(l[i].p.Y - l[i - 1].p.Y, -1) - xc * (l[i].p.X - l[i - 1].p.X) * Math.Pow(l[i].p.Y - l[i - 1].p.Y, -1);
                r = l[i].R;
                if (r > 0 && r < 100000)
                {
                    az1 = Rellenar_centro(l[i - 1].p.X, l[i - 1].p.Y, xc, yc, 1).Az;
                    az2 = Rellenar_centro(l[i].p.X, l[i].p.Y, xc, yc, 1).Az;

                    if (l[i].Tipogiro == 1)
                    {
                        if (az1 > az2)
                        {
                            az1 -= 360;
                        }
                        az_final = (az2 + az1) / 2;
                        if (az_final > 360)
                        {
                            az_final -= 360;
                        }
                        if (az_final < 0)
                        {
                            az_final += 360;
                        }
                        xx = xc + (r) * Math.Sin((az_final) * Math.PI / 180);
                        yy = yc + (r) * Math.Cos((az_final) * Math.PI / 180);
                    }
                    else
                    {
                        if (az1 < az2)
                        {
                            az2 -= 360;
                        }
                        az_final = (az2 + az1) / 2;
                        if (az_final > 360)
                        {
                            az_final -= 360;
                        }
                        if (az_final < 0)
                        {
                            az_final += 360;
                        }
                        xx = xc + (r) * Math.Sin((az_final) * Math.PI / 180);
                        yy = yc + (r) * Math.Cos((az_final) * Math.PI / 180);
                    }
                    aux.Add(new Punto(new Point2d(xx, yy)));
                    aux.Add(l[i]);
                }

            }
            i = l.Count - 2;
            xc = ((l[i + 1].p.Y + l[i].p.Y) * 0.5 + ((l[i + 1].p.X - l[i].p.X) * (l[i + 1].p.X + l[i].p.X) * 0.5 / (l[i + 1].p.Y - l[i].p.Y)) - (l[i].p.Y + l[i - 1].p.Y) * 0.5 - ((l[i].p.X - l[i - 1].p.X) * (l[i].p.X + l[i - 1].p.X) * 0.5 / (l[i].p.Y - l[i - 1].p.Y))) / (((l[i + 1].p.X - l[i].p.X) / (l[i + 1].p.Y - l[i].p.Y)) - ((l[i].p.X - l[i - 1].p.X) / (l[i].p.Y - l[i - 1].p.Y)));
            yc = (l[i].p.Y + l[i - 1].p.Y) * 0.5 + (l[i].p.X - l[i - 1].p.X) * 0.5 * (l[i].p.X + l[i - 1].p.X) * Math.Pow(l[i].p.Y - l[i - 1].p.Y, -1) - xc * (l[i].p.X - l[i - 1].p.X) * Math.Pow(l[i].p.Y - l[i - 1].p.Y, -1);
            r = l[i].R;
            if (r > 0 && r < 100000)
            {
                az2 = Rellenar_centro(l[i + 1].p.X, l[i + 1].p.Y, xc, yc, 1).Az;
                az1 = Rellenar_centro(l[i].p.X, l[i].p.Y, xc, yc, 1).Az;

                if (l[i].Tipogiro == 1)
                {
                    if (az1 > az2)
                    {
                        az1 -= 360;
                    }
                    az_final = (az2 + az1) / 2;
                    if (az_final > 360)
                    {
                        az_final -= 360;
                    }
                    if (az_final < 0)
                    {
                        az_final += 360;
                    }
                    xx = xc + (r) * Math.Sin((az_final) * Math.PI / 180);
                    yy = yc + (r) * Math.Cos((az_final) * Math.PI / 180);
                }
                else
                {
                    if (az1 < az2)
                    {
                        az2 -= 360;
                    }
                    az_final = (az2 + az1) / 2;
                    if (az_final > 360)
                    {
                        az_final -= 360;
                    }
                    if (az_final < 0)
                    {
                        az_final += 360;
                    }
                    xx = xc + (r) * Math.Sin((az_final) * Math.PI / 180);
                    yy = yc + (r) * Math.Cos((az_final) * Math.PI / 180);
                }
                aux.Add(new Punto(new Point2d(xx, yy)));
            }
            aux.Add(l[l.Count - 1]);
            return aux;
        }
        private List<Punto> Duplicar_puntos(List<Punto> l)
        {
            List<Punto> aux = new List<Punto>();

            Punto p_aux;
            for (int i = 0; i < l.Count - 1; i++)
            {
                aux.Add(l[i]);
                p_aux = new Punto(new Point2d((l[i].p.X + l[i + 1].p.X) / 2, (l[i].p.Y + l[i + 1].p.Y) / 2));
                aux.Add(p_aux);
            }
            aux.Add(l[l.Count - 1]);
            return aux;
        }
        private Punto Rellenar_centro(double xc1, double yc1, double xc, double yc, int direccion)
        {
            Punto p, p_a;
            if (direccion == 1)
            {
                p = new Punto(new Point2d(xc1, yc1));
                p_a = new Punto(new Point2d(xc, yc));
            }
            else
            {
                p_a = new Punto(new Point2d(xc1, yc1));
                p = new Punto(new Point2d(xc, yc));
            }

            p.Dx = p.p.X - p_a.p.X;
            p.Dy = p.p.Y - p_a.p.Y;
            if (p.Dx == 0)
            {
                p.Ad1 = 0;
            }
            else
            {
                if (p.Dy == 0)
                {
                    p.Ad1 = 0;
                }
                else
                {
                    p.Ad1 = Math.Atan(p.Dy / p.Dx);
                }
            }
            p.Ad2 = p.Ad1 * (180 / Math.PI);

            if (p.Ad1 == 0)
            {
                p.signod = 0;
            }
            else
            {
                if (p.Ad1 < 0)
                {
                    p.signod = 1;
                }
                else
                {
                    p.signod = 2;
                }
            }

            if (p.Dx == 0)
            {
                p.signodx = 0;
            }
            else
            {
                if (p.Dx < 0)
                {
                    p.signodx = 1;
                }
                else
                {
                    p.signodx = 2;
                }
            }

            if (p.Dy == 0)
            {
                p.signody = 0;
            }
            else
            {
                if (p.Dy < 0)
                {
                    p.signody = 1;
                }
                else
                {
                    p.signody = 2;
                }
            }

            if (p.signod == 0)
            {
                p.Dc = 2;
            }
            else
            {
                p.Dc = 1;
            }
            if (p.Dc == 2)
            {
                if (p.Dy == 0)
                {
                    p.Orientacion = "E-W";
                }
                else
                {
                    p.Orientacion = "N-S";
                }
            }

            if (p.Dc == 2)
            {
                if (p.Orientacion == "E-W")
                {
                    if (p.Dx < 0)
                    {
                        p.Azcardinal = 270;
                    }
                    else
                    {
                        p.Azcardinal = 90;
                    }
                }
                else
                {
                    if (p.Dy < 0)
                    {
                        p.Azcardinal = 180;
                    }
                    else
                    {
                        p.Azcardinal = 0;
                    }
                }
            }

            //cuadrante
            if (p.Dc == 2)
            {
                p.cuadrante = 0;
            }
            else
            {
                if (p.Dx > 0 && p.Dy > 0)
                {
                    p.cuadrante = 1;
                }
                else
                {
                    if (p.Dx > 0 && p.Dy < 0)
                    {
                        p.cuadrante = 2;
                    }
                    else
                    {
                        if (p.Dx < 0 && p.Dy < 0)
                        {
                            p.cuadrante = 3;
                        }
                        else
                        {
                            p.cuadrante = 4;
                        }
                    }
                }
            }

            //Azimut
            if (p.Dc == 2)
            {
                p.Az = p.Azcardinal;
            }
            else
            {
                if (p.signod == 1)
                {
                    if (p.signodx == 2)
                    {
                        p.Az = 90 - p.Ad2;

                    }
                    else
                    {
                        p.Az = 270 - p.Ad2;
                    }
                }
                else
                {
                    if (p.signodx == 2)
                    {
                        p.Az = 90 - p.Ad2;

                    }
                    else
                    {
                        p.Az = 270 - p.Ad2;
                    }
                }
            }
            return p;
        }
        private void Reajustar_Cota()
        {
            int contador=0;
            double cota_inf,cota_sup;
            double cota;
            double dif_cota = 0;
            double dif_cota_inf=0, dif_cota_sup=0;
            double t_cota,c_cota;
            bool cont;
            int contador2;
            for (int i=0;i< polilinea_perfil.Count;i++)
            {
                if (i==0)
                {
                    contador = 0;
                    cota = Buscar_Cota(polilinea_perfil[i], 0);
                    dif_cota = cota - polilinea_perfil[i].p.Y;
                    cont = true;
                    for (int t = i; t < polilinea_perfil.Count && cont; t++)
                    {
                        if (polilinea_perfil[t].vertice == 2)
                        {
                            cota_sup = cota;
                            dif_cota_sup = dif_cota;
                            cota_inf = Buscar_Cota(polilinea_perfil[t], 2);
                            dif_cota_inf = cota_inf - polilinea_perfil[t].p.Y;
                            cont = false;
                            contador++;

                        }
                        if (polilinea_perfil[t].vertice == 1)
                        {
                            cota_inf = cota;
                            dif_cota_inf = dif_cota;
                            cota_sup = Buscar_Cota(polilinea_perfil[t], 1);
                            dif_cota_sup = cota_sup - polilinea_perfil[t].p.Y;
                            cont = false;
                            contador++;

                        }
                        if (cont)
                        {
                            contador++;
                        }
                    }
                    t_cota = dif_cota_sup - dif_cota_inf;
                    c_cota = t_cota / contador;
                    Point2d p;
                    contador2 = 1;
                    for (int t = i+1; t < contador + i-1; t++)
                    {
                        p = new Point2d(polilinea_perfil[t].p.X, polilinea_perfil[t].p.Y + 0 + (c_cota * contador2));
                        polilinea_perfil[t].p = p;
                        contador2++;
                    }
                }
                if (polilinea_perfil[i].vertice==1)
                {
                    contador = 0;
                    cota_sup = Buscar_Cota(polilinea_perfil[i],1);
                    dif_cota_sup = cota_sup - polilinea_perfil[i].p.Y;
                    cont = true;
                    for (int t=i;t< polilinea_perfil.Count && cont; t++)
                    {
                        if (polilinea_perfil[t].vertice ==2)
                        {
                            cota_inf = Buscar_Cota(polilinea_perfil[t],2);
                            dif_cota_inf= cota_inf- polilinea_perfil[t].p.Y;
                            cont = false;
                            contador++;
                            
                        }
                        if (cont)
                        {
                            contador++;
                        }
                    }
                    t_cota = dif_cota_sup - dif_cota_inf;
                    c_cota = t_cota / contador;
                    Point2d p;
                    contador2 = 1;
                    for (int t = i; t < contador+i-1; t++)
                    {
                        p = new Point2d(polilinea_perfil[t].p.X, polilinea_perfil[t].p.Y + dif_cota_sup- (c_cota*contador2));
                        polilinea_perfil[t].p = p;
                        contador2++;
                    }
                }
                if (polilinea_perfil[i].vertice == 2)
                {
                    contador = 0;
                    cota_inf = Buscar_Cota(polilinea_perfil[i], 2);
                    dif_cota_inf = cota_inf - polilinea_perfil[i].p.Y;
                    cont = true;
                    for (int t = i; t < polilinea_perfil.Count && cont; t++)
                    {
                        if (polilinea_perfil[t].vertice == 1)
                        {
                            cota_sup = Buscar_Cota(polilinea_perfil[t], 1);
                            dif_cota_sup = cota_sup - polilinea_perfil[t].p.Y;
                            cont = false;
                            contador++;

                        }
                        if (cont)
                        {
                            contador++;
                        }
                    }
                    t_cota = dif_cota_sup - dif_cota_inf;
                    c_cota = t_cota / contador;
                    Point2d p;
                    contador2 = 1;
                    for (int t = i; t < contador + i-1; t++)
                    {
                        p = new Point2d(polilinea_perfil[t].p.X, polilinea_perfil[t].p.Y + dif_cota_inf + (c_cota * contador2));
                        polilinea_perfil[t].p = p;
                        contador2++;
                    }
                }
            }
        }
        /// <summary>
        /// Busca La cota para el punto superior o inferior segun si el tipo es 1(superior) o 2(inferior)
        /// </summary>
        /// <param name="p">punto a buscar</param>
        /// <param name="tipo">tipo de busqueda 1(superior) o 2(inferior)(</param>
        /// <returns>Cota encontrada</returns>
        private double Buscar_Cota(PuntoPerfil p,int tipo)
        {
            double distancia = 100;
            double cota=0;
            int n_cota=0;
            for (int i=0;i< polilinea_inicial.Count;i++)
            {
                if (Distancia(polilinea_inicial[i].p,p.p)<distancia)
                {
                    cota = polilinea_inicial[i].p.Y;
                    n_cota = i;
                }
                distancia = Distancia(polilinea_inicial[i].p, p.p);
            }
            if (tipo==1)
            {
                if (polilinea_inicial[n_cota-1].p.Y>cota)
                {
                    cota = polilinea_inicial[n_cota - 1].p.Y;
                }
                else if (polilinea_inicial[n_cota + 1].p.Y > cota)
                {
                    cota = polilinea_inicial[n_cota + 1].p.Y;
                }
            }
            if (tipo==2)
            {
                if (polilinea_inicial[n_cota - 1].p.Y < cota)
                {
                    cota = polilinea_inicial[n_cota - 1].p.Y;
                }
                else if (polilinea_inicial[n_cota + 1].p.Y < cota)
                {
                    cota = polilinea_inicial[n_cota + 1].p.Y;
                }
            }
            return cota;
        }
        public void DividirSentidos()
        {
            
            List<PuntoPerfil> Lista_aux = new List<PuntoPerfil>();
            
            for (int i=0; i< polilinea_perfil.Count;i++)
            {
                if (polilinea_perfil[i].secuenciagiro==1)
                {
                    lista_sentidos.Add(Lista_aux);
                    Lista_aux = new List<PuntoPerfil>();
                    Lista_aux.Add(polilinea_perfil[i]);
                }
                else
                {
                    Lista_aux.Add(polilinea_perfil[i]);
                }
            }
            if (Lista_aux.Count>0)
            {
                lista_sentidos.Add(Lista_aux);
            }
            
        }
        public void Dibujar_Acuerdos(int paso)
        {
            for (int i=0;i<Lista_parabolas.Count;i++)
            {
                Dibujar_Acuerdo(Lista_parabolas[i],paso);
            }
        }
        public void Fusion_Acuerdos()
        {
            for (int i = 0; i < Lista_parabolas.Count-1; i++)
            {
                List<PuntoPerfil> lista_aux = new List<PuntoPerfil>();
                double a = Lista_parabolas[i].polilinea_perfil[Lista_parabolas[i].polilinea_perfil.Count-1].pendiente;
                double b = Lista_parabolas[i+1].polilinea_perfil[0].pendiente;
                if (Math.Truncate(a*1000)== Math.Truncate(b*1000) && 
                    ((Lista_parabolas[i].parabola[0] > 0 && Lista_parabolas[i + 1].parabola[0] > 0)||
                     (Lista_parabolas[i].parabola[0] < 0 && Lista_parabolas[i + 1].parabola[0] < 0)))
                {
                    lista_aux = new List<PuntoPerfil>();
                    for (int t = 0; t < Lista_parabolas[i].polilinea_perfil.Count - 1; t++)
                    {
                        //Lista_parabolas[i].polilinea_perfil.Add(Lista_parabolas[i + 1].polilinea_perfil[t]);
                        lista_aux.Add(Lista_parabolas[i].polilinea_perfil[t]);
                    }
                    for (int t = 0; t < Lista_parabolas[i+1].polilinea_perfil.Count - 1; t++)
                    {
                        //Lista_parabolas[i].polilinea_perfil.Add(Lista_parabolas[i + 1].polilinea_perfil[t]);
                        lista_aux.Add(Lista_parabolas[i+1].polilinea_perfil[t]);
                    }
                    Lista_parabolas.Insert(i,Crear_Acuerdo(lista_aux));
                    Lista_parabolas.RemoveAt(i + 1);
                    Lista_parabolas.RemoveAt(i + 1);
                }
            }
        }
        /// <summary>
        /// Quita los acuerdos que son producidos por errores en el trazado como pueden ser baches u otros motivos
        /// </summary>
        /// <param name="distancia">distancia a cumplir para eliminar</param>
        /// <param name="separacion">separacion entre el acuerdo y la flecha</param>
        public void Quitar_Acuerdos(double distancia,double separacion, double pendiente)
        {
            int lista;
            double x1, y1, x2, y2;
            double ypk;
            double ih;
            int maximo;
            //por distancia
            for (int i = 0; i < lista_sentidos.Count; i++)
            {
                if (lista_sentidos[i][lista_sentidos[i].Count - 1].p.X - lista_sentidos[i][0].p.X < distancia)
                {
                    for (int t = 0; t < Lista_parabolas.Count; t++)
                    {
                        if (Lista_parabolas[t].polilinea_perfil[0].p.X >= lista_sentidos[i][0].p.X &&
                            Lista_parabolas[t].polilinea_perfil[0].p.X < lista_sentidos[i][lista_sentidos[i].Count - 1].p.X)
                        {
                            Lista_parabolas.RemoveAt(t);
                            if (t > 0)
                            {
                                t--;
                            }
                            break;
                        }
                    }
                }
            }
            //por delta-h
            for (int i = 0; i < lista_sentidos.Count; i++)
            {
                x1 = lista_sentidos[i][0].p.X;
                y1 = lista_sentidos[i][0].p.Y;

                x2 = lista_sentidos[i][lista_sentidos[i].Count - 1].p.X;
                y2 = lista_sentidos[i][lista_sentidos[i].Count - 1].p.Y;
                ih = 0;
                maximo = 0;
                for (int t = 0; t < lista_sentidos[i].Count; t++)
                {
                    ypk = y1 + ((y2 - y1) / (x2 - x1)) * (lista_sentidos[i][t].p.X - x1);
                    if (Math.Abs(ypk - lista_sentidos[i][t].p.Y) > ih)
                    {
                        ih = Math.Abs(ypk - lista_sentidos[i][t].p.Y);
                        maximo = t;
                    }
                }
                ypk = y1 + ((y2 - y1) / (x2 - x1)) * (lista_sentidos[i][maximo].p.X - x1);
                if (Math.Abs(lista_sentidos[i][maximo].p.Y - ypk) < separacion)
                {
                    for (int t = 0; t < Lista_parabolas.Count; t++)
                    {
                        if (Lista_parabolas[t].polilinea_perfil[0].p.X >= lista_sentidos[i][0].p.X &&
                            Lista_parabolas[t].polilinea_perfil[0].p.X < lista_sentidos[i][lista_sentidos[i].Count - 1].p.X)
                        {
                            Lista_parabolas.RemoveAt(t);
                            if (t > 0)
                            {
                                t--;
                            }
                            break;
                        }
                    }
                }
            }
            //por diferencia de pendiente
            for (int i=0;i<Lista_parabolas.Count;i++)
            {
                double p1 = Lista_parabolas[i].polilinea_perfil[0].pendiente;
                double p3 = Lista_parabolas[i].polilinea_perfil[Lista_parabolas[i].polilinea_perfil.Count-1].pendiente;
                if (double.IsNaN(p3))
                {
                    p3 = Lista_parabolas[i].polilinea_perfil[Lista_parabolas[i].polilinea_perfil.Count - 2].pendiente;
                }
                if (Math.Abs(p1-p3)<pendiente/100)
                {
                    Lista_parabolas.RemoveAt(i);
                    if (i > 0)
                    {
                        i--;
                    }
                    
                }
            }



/*            for (int i = 0; i < lista_sentidos.Count; i++)
            {
                if (lista_sentidos[i][lista_sentidos[i].Count-1].p.X - lista_sentidos[i][0].p.X< distancia)
                {
                    x1 = lista_sentidos[i][0].p.X;
                    y1 = lista_sentidos[i][0].p.Y;

                    x2 = lista_sentidos[i][lista_sentidos[i].Count - 1].p.X;
                    y2 = lista_sentidos[i][lista_sentidos[i].Count - 1].p.Y;
                    ih = 0;
                    maximo = 0;
                    for (int t=0;t< lista_sentidos[i].Count;t++)
                    {
                        ypk = y1 + ((y2 - y1) / (x2 - x1)) * (lista_sentidos[i][t].p.X - x1);
                        if (Math.Abs(ypk- lista_sentidos[i][t].p.Y) > ih)
                        {
                            ih = Math.Abs(ypk - lista_sentidos[i][t].p.Y);
                            maximo = t;
                        }
                    }
                    ypk = y1 + ((y2 - y1) / (x2 - x1)) * (lista_sentidos[i][maximo].p.X - x1);
                    if (Math.Abs(lista_sentidos[i][maximo].p.Y- ypk)< separacion)
                    {
                        for (int t=0;t<Lista_parabolas.Count;t++)
                        {
                            if (Lista_parabolas[t].polilinea_perfil[0].p.X >= lista_sentidos[i][0].p.X && 
                                Lista_parabolas[t].polilinea_perfil[0].p.X < lista_sentidos[i][lista_sentidos[i].Count-1].p.X)
                            {
                                Lista_parabolas.RemoveAt(t);
                                if (t > 0)
                                {
                                    t--;
                                }
                            }
                        }
                    }
                   
                }
            }
*/

/*            for (int i = 0; i < Lista_parabolas.Count; i++)
            {
                if (Lista_parabolas[i].polilinea_perfil[Lista_parabolas[i].polilinea_perfil.Count-1].p.X - Lista_parabolas[i].polilinea_perfil[0].p.X<distancia)
                {
                    lista = Buscar_Lista_Sentido(Lista_parabolas[i].polilinea_perfil[0].p.X);

                    x1 = lista_sentidos[lista][0].p.X;
                    y1 = lista_sentidos[lista][0].p.Y;

                    x2 = lista_sentidos[lista][lista_sentidos[lista].Count - 1].p.X;
                    y2 = lista_sentidos[lista][lista_sentidos[lista].Count - 1].p.Y;
                    for (int t = 0; t < Lista_parabolas[i].polilinea_perfil.Count; t++)
                    {
                        ypk = y1 + ((y2 - y1) / (x2 - x1)) * (Lista_parabolas[i].polilinea_perfil[t].p.X - x1);
                        if (Math.Abs(Punto_y_Parabola(Lista_parabolas[i], Lista_parabolas[i].polilinea_perfil[t].p.X) - ypk) < separacion)
                        {
                            //eliminar
                            Lista_parabolas.RemoveAt(i);
                            if (i>0)
                            {
                                i--;
                            }
                            break;
                        }
                    }
                }
            }*/
        }
        /// <summary>
        /// Busca la lista donde se encuentra el pk que buscamos
        /// </summary>
        /// <param name="x">pk que buscamos</param>
        /// <returns>entero de la lista donde pertenece el acuerdo</returns>
        private int Buscar_Lista_Sentido(double x)
        {
            int sentido=0;
            for (int i=0;i<lista_sentidos.Count;i++)
            {
                if (lista_sentidos[i][0].p.X<=x && x< lista_sentidos[i][lista_sentidos[i].Count-1].p.X)
                {
                    sentido = i;
                    break;
                }
            }
            return sentido;
        }
        /// <summary>
        /// Devuelve la y respento de la x en una parabola
        /// </summary>
        /// <param name="parabola">parabola a buscar el punto</param>
        /// <param name="px">x para poder buscar la y</param>
        /// <returns>punto y de la parabola</returns>
        private double Punto_y_Parabola(Parabola parabola,double px)
        {

            double x2 = parabola.parabola[0];
            double x1 = parabola.parabola[1];
            double x = parabola.parabola[2];
            double y = (px * px) * x2 + px * x1 + x;

            return y;
        }

        public void Dibujar_Rectas(int paso)
        {
            for (int i=0;i<Lista_rectas.Count;i++)
            {
                Dibujar_r(Lista_rectas[i].Puntos[0], Lista_rectas[i].Puntos[1], paso);
            }
        }
        private void Dibujar_r(Point2d p1, Point2d p2,int paso)
        {
            Point3dCollection poly = new Point3dCollection();
            poly.Add(new Point3d(p1.X + x_ins,y_ins+ p1.Y*escala, 0));
            poly.Add(new Point3d(p2.X + x_ins, y_ins + p2.Y*escala, 0));


            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {

                engCadNet.oLayer.addLayer("Recta-"+paso, 7, false);
                using (Transaction acTrans = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(AcCurDb2.BlockTableId,
                        OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite) as BlockTableRecord;

                    Editor e = Application.DocumentManager.MdiActiveDocument.Editor;
                    Document d = Application.DocumentManager.MdiActiveDocument;
                    Polyline3d pol = new Polyline3d(new Poly3dType(), poly, false);
                    pol.Layer = "Recta-" + paso;

                    acBlkTblRec.AppendEntity(pol);

                    acTrans.AddNewlyCreatedDBObject(pol, true);

                    acTrans.Commit();
                }
            }
        }
        private Parabola Crear_Acuerdo(List<PuntoPerfil> list)
        {
            Parabola p = new Parabola();
            List<double> l_x = new List<double>();
            List<double> l_y = new List<double>();
            List<int> Lista_aux = new List<int>();
            List<PuntoPerfil> list_aux = list;
            bool pintado = false;
            for (int i=0;i< list_aux.Count;i++)
            {
                Lista_aux.Add(i);
                l_x.Add(list_aux[i].p.X);
                l_y.Add(list_aux[i].p.Y);
            }
            
            double suma_x = 0;
            double suma_x2 = 0;
            double suma_x3 = 0;
            double suma_x4 = 0;
            double suma_y = 0;
            double suma_xy = 0;
            double suma_xy2 = 0;
            for (int t = 0; t < l_x.Count; t++)
            {
                suma_x += l_x[t];
                suma_x2 += Math.Pow(l_x[t], 2);
                suma_x3 += Math.Pow(l_x[t], 3);
                suma_x4 += Math.Pow(l_x[t], 4);
                suma_y += l_y[t];
                suma_xy += l_x[t] * l_y[t];
                suma_xy2 += l_y[t] * Math.Pow(l_x[t], 2);
            }
            suma_x /= l_x.Count;
            suma_x2 /= l_x.Count;
            suma_x3 /= l_x.Count;
            suma_x4 /= l_x.Count;
            suma_y /= l_x.Count;
            suma_xy /= l_x.Count;
            suma_xy2 /= l_x.Count;
            double[,] matrix = new double[3, 4];
            matrix[0, 0] = 1;
            matrix[0, 1] = suma_x;
            matrix[0, 2] = suma_x2;
            matrix[0, 3] = suma_y;

            matrix[1, 0] = suma_x;
            matrix[1, 1] = suma_x2;
            matrix[1, 2] = suma_x3;
            matrix[1, 3] = suma_xy;

            matrix[2, 0] = suma_x2;
            matrix[2, 1] = suma_x3;
            matrix[2, 2] = suma_x4;
            matrix[2, 3] = suma_xy2;
            GJ(ref matrix, 3, 4);
            for (int t = 0; t < Lista_aux.Count; t++)
            {
                list_aux[Lista_aux[t]].parabola.Add(matrix[2, 3]);
                list_aux[Lista_aux[t]].parabola.Add(matrix[1, 3]);
                list_aux[Lista_aux[t]].parabola.Add(matrix[0, 3]);
            }
            if (Lista_aux.Count > 1)
            {
                Dibujar_Acuerdo(list_aux, Lista_aux[0], Lista_aux[Lista_aux.Count - 1]);
                pintado = true;
                
                for (int r = Lista_aux[0]; r <= Lista_aux[Lista_aux.Count - 1]; r++)
                {
                    p.Add_PuntoPerfil(list_aux[r]);
                }
                List<double> parabola = new List<double>();
                parabola.Add(matrix[2, 3]);
                parabola.Add(matrix[1, 3]);
                parabola.Add(matrix[0, 3]);
                p.Add_parabola(parabola);
                
            }
            Lista_aux.Clear();
            l_x.Clear();
            l_y.Clear();
            return p;
        }

        public void Componente_Inicial()
        {
            double a = Lista_parabolas[0].parabola[0];
            double b = Lista_parabolas[0].parabola[1];
            double c = Lista_parabolas[0].parabola[2];
            double x0 = polilinea_perfil[0].p.X;
            double y0 = polilinea_perfil[0].p.Y;
            //double xt = Math.Sqrt(((-y0 + c) / a));
            double xt = Math.Sqrt(((-y0 + c) / a));
            double yt = (xt * xt) * a + xt * b + c;
            if (double.IsNaN(xt))
            {
                Lista_parabolas[0].polilinea_perfil.Insert(0, polilinea_perfil[0]);
            }
            else
            {
                List<Point2d> l_p = new List<Point2d>();
                Point2d p1 = new Point2d(x0, y0);
                Point2d p2 = new Point2d(xt, yt);
                l_p.Add(p1);
                l_p.Add(p2);
                Pendiente p = new Pendiente(l_p);
                Lista_rectas.Insert(0, p);
            }
            
        }
        public void Componente_Final()
        {
            double a = Lista_parabolas[Lista_parabolas.Count-1].parabola[0];
            double b = Lista_parabolas[Lista_parabolas.Count - 1].parabola[1];
            double c = Lista_parabolas[Lista_parabolas.Count - 1].parabola[2];
            double x0 = polilinea_perfil[polilinea_perfil.Count-1].p.X;
            double y0 = polilinea_perfil[polilinea_perfil.Count - 1].p.Y;

            double ac = a;
            double bc = 2 * a * x0;
            double cc = y0 - x0 * b - c;
            double r1 = (bc + (Math.Sqrt(Math.Pow(bc, 2) - 4 * ac * cc))) / (2 * ac);
            double r2 = (bc - (Math.Sqrt(Math.Pow(bc, 2) - 4 * ac * cc))) / (2 * ac);

            double yt1 = (r1 * r1) * a + r1 * b + c;
            double yt2 = (r2 * r2) * a + r2 * b + c;
            double xt = 0, yt = 0;
            if (r1<x0)
            {
                xt = r1;
                yt = yt1;
            }
            else
            {
                xt = r2;
                yt = yt2;
            }
            if (double.IsNaN(xt))
            {
                Lista_parabolas[Lista_parabolas.Count-1].polilinea_perfil.Add(polilinea_perfil[polilinea_perfil.Count-1]);
            }
            else
            {
                List<Point2d> l_p = new List<Point2d>();
                Point2d p1 = new Point2d(x0, y0);
                Point2d p2 = new Point2d(xt, yt);
                l_p.Add(p2);
                l_p.Add(p1);
                Pendiente p = new Pendiente(l_p);
                Lista_rectas.Add(p);
            }
        }
        public void Acuerdo_Entre_Pendientes()
        {
            bool salir = false;
            int conta_parabolas = 0;
            if (Lista_parabolas[0].polilinea_perfil[0].p.X==0)
            {
                conta_parabolas++;
            }
            while (!salir)
            {
                for (int i = 0; i < Lista_rectas.Count - 1; i++)
                {
                    if (Lista_rectas[i].Puntos[1].X > Lista_rectas[i + 1].Puntos[0].X)
                    {
                        if (Lista_rectas[i].Puntos[0].X > Lista_rectas[i + 1].Puntos[0].X)
                        {
                            Lista_parabolas.RemoveAt(conta_parabolas);
                            Lista_rectas.Clear();
                            salir = true;
                            if (Lista_parabolas[0].polilinea_perfil[0].p.X == 0)
                            {
                                conta_parabolas=1;
                            }
                            else
                            {
                                conta_parabolas = 0;
                            }
                            break;
                        }
                        else
                        {
                            //ec de la primera recta
                            double a_x0 = Lista_rectas[i].Puntos[0].X;
                            double a_y0 = Lista_rectas[i].Puntos[0].Y;
                            double b_x1 = Lista_rectas[i].Puntos[1].X;
                            double b_y1 = Lista_rectas[i].Puntos[1].Y;

                            double a_1 = (a_y0 - b_y1) / (a_x0 - b_x1);
                            double b_1 = -b_x1 * (a_y0 - b_y1) / (a_x0 - b_x1) + b_y1;

                            double c_x0 = Lista_rectas[i + 1].Puntos[0].X;
                            double c_y0 = Lista_rectas[i + 1].Puntos[0].Y;
                            double d_x1 = Lista_rectas[i + 1].Puntos[1].X;
                            double d_y1 = Lista_rectas[i + 1].Puntos[1].Y;

                            double c_1 = (c_y0 - d_y1) / (c_x0 - d_x1);
                            double d_1 = -d_x1 * ((c_y0 - d_y1) / (c_x0 - d_x1)) + d_y1;

                            double i_x = (d_1 - b_1) / (a_1 - c_1);
                            double i_y = a_1 * ((d_1 - b_1) / (a_1 - c_1)) + b_1;
                            Punto3d p_vertice = new Punto3d(i_x, i_y, 0);

                            double d1 = Distancia(new Point2d(a_x0, a_y0), new Point2d(i_x, i_y));
                            double d2 = Distancia(new Point2d(i_x, i_y), new Point2d(d_x1, d_y1));
                            double df = 0;
                            if (d1 > d2)
                            {
                                df = d2 / 3;
                            }
                            else
                            {
                                df = d1 / 3;
                            }
                            double x1 = i_x - df;
                            double y1 = a_1 * x1 + b_1;

                            double x2 = i_x + df;
                            double y2 = c_1 * x2 + d_1;

                            double A = 2 * x1;
                            double B = 1;
                            double C = a_1;

                            double A1 = 2 * x2;
                            double B1 = 1;
                            double C1 = c_1;

                            double X = (-B * C1 + B1 * C) / (-B * A1 + A * B1);
                            double Y = (-C * A1 + A * C1) / (-B * A1 + A * B1);

                            double CF = (-X * (x1 * x1) - (Y * x1) + y1);
                            Parabola p = new Parabola();
                            List<double> para = new List<double>();
                            para.Add(X);
                            para.Add(Y);
                            para.Add(CF);

                            List<PuntoPerfil> poli = new List<PuntoPerfil>();
                            Punto punto = new Punto(new Point2d(x1, y1));
                            PuntoPerfil punto_perfil = new PuntoPerfil(punto);
                            poli.Add(punto_perfil);
                            punto = new Punto(new Point2d(x2, y2));
                            punto_perfil = new PuntoPerfil(punto);
                            poli.Add(punto_perfil);
                            p = new Parabola(para, poli);
                            Lista_Parabolas.Insert(conta_parabolas, p);
                            Lista_parabolas.RemoveAt(conta_parabolas + 1);
                            Lista_rectas[i].Puntos.RemoveAt(1);
                            Lista_rectas[i].Puntos.Add(new Point2d(x1, y1));
                            Lista_rectas[i + 1].Puntos.Insert(0, new Point2d(x2, y2));
                            Lista_rectas[i + 1].Puntos.RemoveAt(1);
                        }
                    }
                    conta_parabolas++;
                }
                if (salir)
                {
                    CalcularEntreParabolas();
                    CalculoEntreParabolas_Dibujar();
                    Componente_Inicial();
                    Componente_Final();
                    salir = false;
                }
                else
                {
                    salir = true;
                }
            }
            

            
        }
        public void CrearTrazado()
        {
            
            if (Lista_rectas[0].Puntos[0].X<Lista_parabolas[0].polilinea_perfil[0].p.X)
            {
                Dibujar_Trazado(1);
            }
            else
            {
                Dibujar_Trazado(2);
            }

        }
        /// <summary>
        /// Dibuja el trazado completo
        /// </summary>
        /// <param name="tipo">1 para empezar por pendiente y 2 para empezar por acuerdo</param>
        public void Dibujar_Trazado(int tipo)
        {
            if (tipo == 2)
            {
                using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                        Polyline miEje = new Polyline();
                        int index = 0;
                        double p2_x = 0;
                        for (int t = 0; t < Lista_rectas.Count && t < Lista_parabolas.Count; t++)
                        {
                            double x2 = Lista_parabolas[t].parabola[0];
                            double x1 = Lista_parabolas[t].parabola[1];
                            double x = Lista_parabolas[t].parabola[2];
                            double pk_ini = 0;
                            if (t>0)
                            {
                                pk_ini = p2_x;
                            }
                            
                            double pk_fin = Lista_parabolas[t].polilinea_perfil[Lista_parabolas[t].polilinea_perfil.Count - 1].p.X;
                            if (t <= Lista_rectas.Count - 1)
                            {
                                pk_fin = Lista_rectas[t].Puntos[0].X;
                            }
                            double pk = pk_ini;
                            double y;
                            for (int i = 0; pk < pk_fin; i++)
                            {
                                y = (pk * pk) * x2 + pk * x1 + x;//x^2+x+c

                                miEje.AddVertexAt(index, new Point2d(pk + x_ins, y * escala + y_ins), 0, 0, 0);
                                pk = pk + 1;
                                index++;
                            }
                            y = (pk_fin * pk_fin) * x2 + pk_fin * x1 + x;//x^2+x+c

                            miEje.AddVertexAt(index, new Point2d(pk_fin + x_ins, y * escala + y_ins), 0, 0, 0);
                            index++;

                            double p1_x = Lista_rectas[t].Puntos[0].X;
                            double p1_y = Lista_rectas[t].Puntos[0].Y;
                            miEje.AddVertexAt(index, new Point2d(p1_x + x_ins, p1_y * escala + y_ins), 0, 0, 0);
                            index++;
                            p2_x = Lista_rectas[t].Puntos[1].X;
                            double p2_y = Lista_rectas[t].Puntos[1].Y;
                            miEje.AddVertexAt(index, new Point2d(p2_x + x_ins, p2_y * escala + y_ins), 0, 0, 0);
                            index++;

                        }
                        if (Lista_rectas.Count > Lista_parabolas.Count)
                        {
                            double p1_x = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].X;
                            double p1_y = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].Y;
                            miEje.AddVertexAt(index, new Point2d(p1_x + x_ins, p1_y * escala + y_ins), 0, 0, 0);
                            index++;
                            p2_x = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X;
                            double p2_y = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].Y;
                            miEje.AddVertexAt(index, new Point2d(p2_x + x_ins, p2_y * escala + y_ins), 0, 0, 0);
                            index++;
                        }
                        else
                        {
                            if (Lista_rectas.Count < Lista_parabolas.Count)
                            {
                                double x2 = Lista_parabolas[Lista_parabolas.Count - 1].parabola[0];
                                double x1 = Lista_parabolas[Lista_parabolas.Count - 1].parabola[1];
                                double x = Lista_parabolas[Lista_parabolas.Count - 1].parabola[2];
                                double pk_ini = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X;
                                double pk_fin = Lista_parabolas[Lista_parabolas.Count - 1].polilinea_perfil[Lista_parabolas[Lista_parabolas.Count - 1].polilinea_perfil.Count - 1].p.X;
                                double pk = pk_ini;
                                double y;
                                for (int i = 0; pk < pk_fin; i++)
                                {
                                    y = (pk * pk) * x2 + pk * x1 + x;//x^2+x+c

                                    miEje.AddVertexAt(index, new Point2d(pk + x_ins, y * escala + y_ins), 0, 0, 0);
                                    pk = pk + 0.1;
                                    index++;
                                }
                                y = (pk_fin * pk_fin) * x2 + pk_fin * x1 + x;//x^2+x+c

                                miEje.AddVertexAt(index, new Point2d(pk_fin + x_ins, y * escala + y_ins), 0, 0, 0);
                                index++;
                            }

                        }

                        engCadNet.oLayer.addLayer("Trazado", 2, false);
                        miEje.Layer = "Trazado";

                        btr.AppendEntity(miEje);
                        tr.AddNewlyCreatedDBObject(miEje, true);

                        oCadManager.thisEditor.UpdateScreen();

                        tr.Commit();
                    }
                }
            }
            else
            {
                using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                        Polyline miEje = new Polyline();
                        int index = 0;
                        for (int t = 0; t < Lista_rectas.Count && t < Lista_parabolas.Count; t++)
                        {
                            double p1_x = Lista_rectas[t].Puntos[0].X;
                            double p1_y = Lista_rectas[t].Puntos[0].Y;
                            miEje.AddVertexAt(index, new Point2d(p1_x + x_ins, p1_y * escala + y_ins), 0, 0, 0);
                            index++;
                            double p2_x = Lista_rectas[t].Puntos[1].X;
                            double p2_y = Lista_rectas[t].Puntos[1].Y;
                            miEje.AddVertexAt(index, new Point2d(p2_x + x_ins, p2_y * escala + y_ins), 0, 0, 0);
                            index++;
                            double x2 = Lista_parabolas[t].parabola[0];
                            double x1 = Lista_parabolas[t].parabola[1];
                            double x = Lista_parabolas[t].parabola[2];
                            double pk_ini = p2_x;
                            double pk_fin = Lista_parabolas[t].polilinea_perfil[Lista_parabolas[t].polilinea_perfil.Count - 1].p.X;
                            if (t + 1 <= Lista_rectas.Count - 1)
                            {
                                pk_fin = Lista_rectas[t + 1].Puntos[0].X;
                            }
                            double pk = pk_ini;
                            double y;
                            for (int i = 0; pk < pk_fin; i++)
                            {
                                y = (pk * pk) * x2 + pk * x1 + x;//x^2+x+c

                                miEje.AddVertexAt(index, new Point2d(pk + x_ins, y * escala + y_ins), 0, 0, 0);
                                pk = pk + 1;
                                index++;
                            }
                            y = (pk_fin * pk_fin) * x2 + pk_fin * x1 + x;//x^2+x+c

                            miEje.AddVertexAt(index, new Point2d(pk_fin + x_ins, y * escala + y_ins), 0, 0, 0);
                            index++;
                        }
                        if (Lista_rectas.Count > Lista_parabolas.Count)
                        {
                            double p1_x = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].X;
                            double p1_y = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].Y;
                            miEje.AddVertexAt(index, new Point2d(p1_x + x_ins, p1_y * escala + y_ins), 0, 0, 0);
                            index++;
                            double p2_x = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X;
                            double p2_y = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].Y;
                            miEje.AddVertexAt(index, new Point2d(p2_x + x_ins, p2_y * escala + y_ins), 0, 0, 0);
                            index++;
                        }
                        else
                        {
                            if (Lista_rectas.Count < Lista_parabolas.Count)
                            {
                                double x2 = Lista_parabolas[Lista_parabolas.Count - 1].parabola[0];
                                double x1 = Lista_parabolas[Lista_parabolas.Count - 1].parabola[1];
                                double x = Lista_parabolas[Lista_parabolas.Count - 1].parabola[2];
                                double pk_ini = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X;
                                double pk_fin = Lista_parabolas[Lista_parabolas.Count - 1].polilinea_perfil[Lista_parabolas[Lista_parabolas.Count - 1].polilinea_perfil.Count - 1].p.X;
                                double pk = pk_ini;
                                double y;
                                for (int i = 0; pk < pk_fin; i++)
                                {
                                    y = (pk * pk) * x2 + pk * x1 + x;//x^2+x+c

                                    miEje.AddVertexAt(index, new Point2d(pk + x_ins, y * escala + y_ins), 0, 0, 0);
                                    pk = pk + 0.1;
                                    index++;
                                }
                                y = (pk_fin * pk_fin) * x2 + pk_fin * x1 + x;//x^2+x+c

                                miEje.AddVertexAt(index, new Point2d(pk_fin + x_ins, y * escala + y_ins), 0, 0, 0);
                                index++;
                            }

                        }

                        engCadNet.oLayer.addLayer("Trazado", 2, false);
                        miEje.Layer = "Trazado";

                        btr.AppendEntity(miEje);
                        tr.AddNewlyCreatedDBObject(miEje, true);

                        oCadManager.thisEditor.UpdateScreen();

                        tr.Commit();
                    }
                }
            }
            
        }
        public double Buscar_minimo()
        {
            double minimo= polilinea_perfil[0].p.Y;
            for (int i=1;i < polilinea_perfil.Count;i++)
            {
                if (polilinea_perfil[i].p.Y< minimo)
                {
                    minimo = polilinea_perfil[i].p.Y;
                }
            }
            return minimo;
        }
        public double Buscar_maximo()
        {
            double maximo = polilinea_perfil[0].p.Y;
            for (int i = 1; i < polilinea_perfil.Count; i++)
            {
                if (polilinea_perfil[i].p.Y > maximo)
                {
                    maximo = polilinea_perfil[i].p.Y;
                }
            }
            return maximo;
        }
        public void Rotular(double rotu)
        {
            Rotular rotular = new Rotular(rotu);
            double min = Buscar_minimo();
            double maximo = Buscar_maximo();
            engCadNet.oLayer.addLayer("Rotulacion-Cota", 1, false);
            rotular.Guitarra(min,maximo, polilinea_perfil[polilinea_perfil.Count-1].p.X,escala,x_ins, y_ins);
            double a_x0, a_y0, b_x1, b_y1, p1, p2;
            a_x0 = Lista_rectas[0].Puntos[0].X;
            a_y0 = Lista_rectas[0].Puntos[0].Y;
            b_x1 = Lista_rectas[0].Puntos[1].X;
            b_y1 = Lista_rectas[0].Puntos[1].Y;

            p1 = (a_y0 - b_y1) / (a_x0 - b_x1);
            if (Lista_rectas[0].Puntos[0].X<Lista_Parabolas[0].polilinea_perfil[0].p.X)
            {
                rotular.Dibujar_Ini_Fin_Pendiente(Lista_rectas[0], 1, escala, p1,x_ins, y_ins);
                for (int i = 0; i < Lista_parabolas.Count && i < Lista_rectas.Count-1; i++)
                {
                    a_x0 = Lista_rectas[i].Puntos[0].X;
                    a_y0 = Lista_rectas[i].Puntos[0].Y;
                    b_x1 = Lista_rectas[i].Puntos[1].X;
                    b_y1 = Lista_rectas[i].Puntos[1].Y;

                    p1 = (a_y0 - b_y1) / (a_x0 - b_x1);

                    a_x0 = Lista_rectas[i + 1].Puntos[0].X;
                    a_y0 = Lista_rectas[i + 1].Puntos[0].Y;
                    b_x1 = Lista_rectas[i + 1].Puntos[1].X;
                    b_y1 = Lista_rectas[i + 1].Puntos[1].Y;

                    p2 = (a_y0 - b_y1) / (a_x0 - b_x1);
                    rotular.Dibujar_Singulares_Perfil(Lista_parabolas[i], Lista_rectas[i].Puntos[1], Lista_rectas[i + 1].Puntos[0], escala, p1, p2,1, x_ins, y_ins);
                }
                if (Lista_parabolas.Count==Lista_rectas.Count)
                {
                    a_x0 = Lista_rectas[Lista_rectas.Count-1].Puntos[0].X;
                    a_y0 = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].Y;
                    b_x1 = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X;
                    b_y1 = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].Y;

                    p1 = (a_y0 - b_y1) / (a_x0 - b_x1);
                    rotular.Dibujar_Singulares_Perfil(Lista_parabolas[Lista_parabolas.Count-1], Lista_rectas[Lista_rectas.Count-1].Puntos[1], polilinea_perfil[polilinea_perfil.Count-1].p, escala, p1, 0, 3, x_ins, y_ins);
                }

            }
            else
            {
                rotular.Dibujar_Ini_Fin_Acuerdo(Lista_Parabolas[0], 1, escala, polilinea_perfil[polilinea_perfil.Count-1].p.X,x_ins, y_ins);
                for (int i = 0; i < Lista_parabolas.Count && i < Lista_rectas.Count-1; i++)
                {
                    if (i==0)
                    {
                        a_x0 = Lista_rectas[i].Puntos[0].X;
                        a_y0 = Lista_rectas[i].Puntos[0].Y;
                        b_x1 = Lista_rectas[i].Puntos[1].X;
                        b_y1 = Lista_rectas[i].Puntos[1].Y;

                        p1 = (a_y0 - b_y1) / (a_x0 - b_x1);
                        rotular.Dibujar_Singulares_Perfil(Lista_parabolas[i], Lista_rectas[i].Puntos[0], Lista_rectas[i].Puntos[0], escala, p1, 0, 2,x_ins, y_ins );

                        a_x0 = Lista_rectas[i].Puntos[0].X;
                        a_y0 = Lista_rectas[i].Puntos[0].Y;
                        b_x1 = Lista_rectas[i].Puntos[1].X;
                        b_y1 = Lista_rectas[i].Puntos[1].Y;

                        p1 = (a_y0 - b_y1) / (a_x0 - b_x1);

                        a_x0 = Lista_rectas[i + 1].Puntos[0].X;
                        a_y0 = Lista_rectas[i + 1].Puntos[0].Y;
                        b_x1 = Lista_rectas[i + 1].Puntos[1].X;
                        b_y1 = Lista_rectas[i + 1].Puntos[1].Y;

                        p2 = (a_y0 - b_y1) / (a_x0 - b_x1);
                        
                        rotular.Dibujar_Singulares_Perfil(Lista_parabolas[i+1], Lista_rectas[i].Puntos[1], Lista_rectas[i + 1].Puntos[0], escala, p1, p2, 1,x_ins, y_ins);

                    }
                    else
                    {
                        a_x0 = Lista_rectas[i].Puntos[0].X;
                        a_y0 = Lista_rectas[i].Puntos[0].Y;
                        b_x1 = Lista_rectas[i].Puntos[1].X;
                        b_y1 = Lista_rectas[i].Puntos[1].Y;

                        p1 = (a_y0 - b_y1) / (a_x0 - b_x1);

                        a_x0 = Lista_rectas[i + 1].Puntos[0].X;
                        a_y0 = Lista_rectas[i + 1].Puntos[0].Y;
                        b_x1 = Lista_rectas[i + 1].Puntos[1].X;
                        b_y1 = Lista_rectas[i + 1].Puntos[1].Y;

                        p2 = (a_y0 - b_y1) / (a_x0 - b_x1);
                        rotular.Dibujar_Singulares_Perfil(Lista_parabolas[i+1], Lista_rectas[i].Puntos[1], Lista_rectas[i + 1].Puntos[0], escala, p1, p2, 1, x_ins, y_ins );
                    }
                    
                }

            }

            if (Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X> Lista_parabolas[Lista_parabolas.Count-1].polilinea_perfil[Lista_parabolas[Lista_parabolas.Count - 1].polilinea_perfil.Count-1].p.X)
            {
                a_x0 = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].X;
                a_y0 = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].Y;
                b_x1 = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X;
                b_y1 = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].Y;

                p1 = (a_y0 - b_y1) / (a_x0 - b_x1);
                rotular.Dibujar_Ini_Fin_Pendiente(Lista_rectas[Lista_rectas.Count - 1], 2, escala, p1, x_ins, y_ins);
            }
            else
            {
                rotular.Dibujar_Ini_Fin_Acuerdo(Lista_Parabolas[Lista_Parabolas.Count-1], 2, escala, polilinea_perfil[polilinea_perfil.Count - 1].p.X, x_ins, y_ins );
            }
            
            engCadNet.oLayer.addLayer("Rotulacion-pk", 1, false);
            min = Buscar_minimo();
            if (Lista_rectas[0].Puntos[0].X< Lista_parabolas[0].polilinea_perfil[0].p.X)
             {
                 int pk = 0;
                 double pk_fin;
                 for (int i=0;i<Lista_rectas.Count-1 && i < Lista_parabolas.Count;i++)
                 {
                     pk = rotular.Dibujar_PK_Pendiente(Lista_rectas[i],pk,escala,min, x_ins, y_ins);
                     pk_fin = Lista_rectas[i + 1].Puntos[0].X;
                     pk = rotular.Dibujar_PK_Acuerdo(Lista_parabolas[i], pk, escala,pk_fin,min, x_ins, y_ins);
                 }
                if (Lista_rectas.Count> Lista_parabolas.Count)
                {
                    pk = rotular.Dibujar_PK_Pendiente(Lista_rectas[Lista_rectas.Count-1], pk, escala, min, x_ins, y_ins);
                    rotular.Dibujar_PK_Pendiente_Final(Lista_rectas[Lista_rectas.Count - 1], pk, escala, min, x_ins, y_ins);
                }
                else
                {
                    pk = rotular.Dibujar_PK_Acuerdo(Lista_parabolas[Lista_parabolas.Count - 1], pk, escala, polilinea_perfil[polilinea_perfil.Count - 1].p.X, min, x_ins, y_ins );
                    rotular.Dibujar_PK_Acuerdo_Final(Lista_parabolas[Lista_parabolas.Count - 1], polilinea_perfil[polilinea_perfil.Count - 1].p.X, escala, min,x_ins, y_ins);
                }
            }
            else
            {
                int pk = 0;
                double pk_fin;
                for (int i = 0; i < Lista_rectas.Count && i < Lista_parabolas.Count; i++)
                {
                    pk_fin = Lista_rectas[i].Puntos[0].X;
                    pk = rotular.Dibujar_PK_Acuerdo(Lista_parabolas[i], pk, escala, pk_fin, min, x_ins, y_ins);
                    pk = rotular.Dibujar_PK_Pendiente(Lista_rectas[i], pk, escala, min, x_ins, y_ins);
                }
                if (Lista_rectas.Count < Lista_parabolas.Count)
                {
                    pk = rotular.Dibujar_PK_Acuerdo(Lista_parabolas[Lista_parabolas.Count-1], pk, escala, polilinea_perfil[polilinea_perfil.Count - 1].p.X, min, x_ins, y_ins);
                    rotular.Dibujar_PK_Acuerdo_Final(Lista_parabolas[Lista_parabolas.Count - 1], polilinea_perfil[polilinea_perfil.Count - 1].p.X, escala, min, x_ins, y_ins);
                }
                else
                {
                    rotular.Dibujar_PK_Pendiente_Final(Lista_rectas[Lista_rectas.Count - 1], pk, escala, min, x_ins, y_ins);
                }
            }


            //Cota máxima y cota mínima
            for (int i=0;i<Lista_parabolas.Count;i++)
            {
                double a = Lista_parabolas[i].parabola[0];
                double b = Lista_parabolas[i].parabola[1];
                double c = Lista_parabolas[i].parabola[2];

                double x = -b / (2 * a);
                double y = (x * x) * a + b * x + c;
                min = Buscar_minimo();
                
                if (Lista_parabolas[i].polilinea_perfil[0].p.X<x && x< Lista_parabolas[i].polilinea_perfil[Lista_parabolas[i].polilinea_perfil.Count-1].p.X)
                {
                    for (int t=0;t<Lista_rectas.Count-1;t++)
                    {
                        if (Lista_rectas[t].Puntos[1].X<x && x<Lista_rectas[t+1].Puntos[0].X)
                        {
                            rotular.Cota(x, min, Lista_parabolas[i], escala,x_ins,y_ins);
                        }
                    }
                    
                }
            }
        }
        public void Informe()
        {
            string nombre_informe = "";
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.DefaultExt = "csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                nombre_informe = saveFileDialog1.FileName;
                System.IO.TextWriter output = new StreamWriter(nombre_informe, false, Encoding.BigEndianUnicode);
                string miVal;
                double a_x0, a_y0, b_x1, b_y1, p1, p2;
                double a;
                double b;
                double c;
                double x,x1;
                double y,y2;
                int componentes = 1;
                output.WriteLine();
                output.WriteLine();
                output.WriteLine();
                output.WriteLine();
                output.Write(";");
                output.Write(";");
                output.Write("Nº Componentes");
                output.Write(";");
                output.Write("Tipo");
                output.Write(";");
                output.Write("Pendiente");
                output.Write(";");
                output.Write("Kv");
                output.Write(";");
                output.Write("Entrada");
                output.Write(";");
                output.Write("Salida");
                output.Write(";");
                output.Write("Cota Inicio");
                output.Write(";");
                output.Write("Cota Fin");
                output.Write(";");
                output.WriteLine();

                if (Lista_parabolas[0].polilinea_perfil[0].p.X>Lista_rectas[0].Puntos[0].X)
                {
                    for (int i=0;i< Lista_rectas.Count && i<Lista_parabolas.Count;i++)
                    {
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(componentes));
                        output.Write(";");
                        output.Write("Pendiente");
                        output.Write(";");
                        a_x0 = Lista_rectas[i].Puntos[0].X;
                        a_y0 = Lista_rectas[i].Puntos[0].Y;
                        b_x1 = Lista_rectas[i].Puntos[1].X;
                        b_y1 = Lista_rectas[i].Puntos[1].Y;
                        p1 = (a_y0 - b_y1) / (a_x0 - b_x1);
                        output.Write(Convert.ToString(p1));
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(a_x0));
                        output.Write(";");
                        output.Write(Convert.ToString(b_x1));
                        output.Write(";");
                        output.Write(Convert.ToString(a_y0));
                        output.Write(";");
                        output.Write(Convert.ToString(b_y1));
                        output.Write(";");
                        output.WriteLine();
                        componentes++;
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(componentes));
                        output.Write(";");
                        output.Write("Acuerdo");
                        output.Write(";");
                        a = Lista_parabolas[i].parabola[0];
                        b = Lista_parabolas[i].parabola[1];
                        c = Lista_parabolas[i].parabola[2];
                        x = Lista_rectas[i].Puntos[1].X;
                        y = (x * x) * a + b * x + c;
                        x1 = polilinea_perfil[polilinea_perfil.Count-1].p.X;
                        if (i + 1 < Lista_rectas.Count)
                        {
                            x1 = Lista_rectas[i + 1].Puntos[0].X;
                        }
                        
                        y2 = (x1 * x1) * a + b * x1 + c;
                        output.Write(";");
                        output.Write(Convert.ToString(1/(2*a)));
                        output.Write(";");
                        output.Write(Convert.ToString(x));
                        output.Write(";");
                        output.Write(Convert.ToString(x1));
                        output.Write(";");
                        output.Write(Convert.ToString(y));
                        output.Write(";");
                        output.Write(Convert.ToString(y2));
                        output.Write(";");
                        componentes++;
                        output.WriteLine();
                    }
                    if (Lista_parabolas.Count < Lista_rectas.Count) 
                    {
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(componentes));
                        output.Write(";");
                        output.Write("Pendiente");
                        output.Write(";");
                        a_x0 = Lista_rectas[Lista_rectas.Count-1].Puntos[0].X;
                        a_y0 = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].Y;
                        b_x1 = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X;
                        b_y1 = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].Y;
                        p1 = (a_y0 - b_y1) / (a_x0 - b_x1);
                        output.Write(Convert.ToString(p1));
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(a_x0));
                        output.Write(";");
                        output.Write(Convert.ToString(b_x1));
                        output.Write(";");
                        output.Write(Convert.ToString(a_y0));
                        output.Write(";");
                        output.Write(Convert.ToString(b_y1));
                        output.Write(";");
                        output.WriteLine();
                    }
                }
                else
                {
                    output.Write(";");
                    output.Write(";");
                    output.Write(Convert.ToString(componentes));
                    output.Write(";");
                    output.Write("Acuerdo");
                    output.Write(";");
                    a = Lista_parabolas[0].parabola[0];
                    b = Lista_parabolas[0].parabola[1];
                    c = Lista_parabolas[0].parabola[2];
                    x = 0;
                    y = (x * x) * a + b * x + c;
                    x1 = Lista_rectas[0].Puntos[0].X;
                    y2 = (x1 * x1) * a + b * x1 + c;
                    output.Write(";");
                    output.Write(Convert.ToString(1 / (2 * a)));
                    output.Write(";");
                    output.Write(Convert.ToString(x));
                    output.Write(";");
                    output.Write(Convert.ToString(x1));
                    output.Write(";");
                    output.Write(Convert.ToString(y));
                    output.Write(";");
                    output.Write(Convert.ToString(y2));
                    output.Write(";");
                    componentes++;
                    output.WriteLine();
                    for (int i = 0; i < Lista_rectas.Count && i < Lista_parabolas.Count-1; i++)
                    {
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(componentes));
                        output.Write(";");
                        output.Write("Pendiente");
                        output.Write(";");
                        a_x0 = Lista_rectas[i].Puntos[0].X;
                        a_y0 = Lista_rectas[i].Puntos[0].Y;
                        b_x1 = Lista_rectas[i].Puntos[1].X;
                        b_y1 = Lista_rectas[i].Puntos[1].Y;
                        p1 = (a_y0 - b_y1) / (a_x0 - b_x1);
                        output.Write(Convert.ToString(p1));
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(a_x0));
                        output.Write(";");
                        output.Write(Convert.ToString(b_x1));
                        output.Write(";");
                        output.Write(Convert.ToString(a_y0));
                        output.Write(";");
                        output.Write(Convert.ToString(b_y1));
                        output.Write(";");
                        output.WriteLine();
                        componentes++;


                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(componentes));
                        output.Write(";");
                        output.Write("Acuerdo");
                        output.Write(";");
                        a = Lista_parabolas[i+1].parabola[0];
                        b = Lista_parabolas[i+1].parabola[1];
                        c = Lista_parabolas[i+1].parabola[2];
                        x = Lista_rectas[i].Puntos[1].X;
                        y = (x * x) * a + b * x + c;
                        x1 = polilinea_perfil[polilinea_perfil.Count - 1].p.X;
                        if (i+1< Lista_rectas.Count)
                        {
                            x1 = Lista_rectas[i + 1].Puntos[0].X;
                        }
                        
                        y2 = (x1 * x1) * a + b * x1 + c;
                        output.Write(";");
                        output.Write(Convert.ToString(1 / (2 * a)));
                        output.Write(";");
                        output.Write(Convert.ToString(x));
                        output.Write(";");
                        output.Write(Convert.ToString(x1));
                        output.Write(";");
                        output.Write(Convert.ToString(y));
                        output.Write(";");
                        output.Write(Convert.ToString(y2));
                        output.Write(";");
                        componentes++;
                        output.WriteLine();

                    }
                    if (Lista_parabolas.Count == Lista_rectas.Count)
                    {
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(componentes));
                        output.Write(";");
                        output.Write("Pendiente");
                        output.Write(";");
                        a_x0 = Lista_rectas[Lista_rectas.Count-1].Puntos[0].X;
                        a_y0 = Lista_rectas[Lista_rectas.Count - 1].Puntos[0].Y;
                        b_x1 = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].X;
                        b_y1 = Lista_rectas[Lista_rectas.Count - 1].Puntos[1].Y;
                        p1 = (a_y0 - b_y1) / (a_x0 - b_x1);
                        output.Write(Convert.ToString(p1));
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(a_x0));
                        output.Write(";");
                        output.Write(Convert.ToString(b_x1));
                        output.Write(";");
                        output.Write(Convert.ToString(a_y0));
                        output.Write(";");
                        output.Write(Convert.ToString(b_y1));
                        output.Write(";");
                        output.WriteLine();
                        componentes++;
                    }
                    if (Lista_parabolas.Count > Lista_rectas.Count)
                    {
                        output.Write(";");
                        output.Write(";");
                        output.Write(Convert.ToString(componentes));
                        output.Write(";");
                        output.Write("Acuerdo");
                        output.Write(";");
                        a = Lista_parabolas[Lista_parabolas.Count-1].parabola[0];
                        b = Lista_parabolas[Lista_parabolas.Count - 1].parabola[1];
                        c = Lista_parabolas[Lista_parabolas.Count - 1].parabola[2];
                        x = Lista_rectas[Lista_rectas.Count].Puntos[1].X;
                        y = (x * x) * a + b * x + c;
                        x1 = Lista_parabolas[Lista_parabolas.Count - 1].polilinea_perfil[Lista_parabolas[Lista_parabolas.Count - 1].polilinea_perfil.Count - 1].p.X;
                        y2 = (x1 * x1) * a + b * x1 + c;
                        output.Write(";");
                        output.Write(Convert.ToString(1 / (2 * a)));
                        output.Write(";");
                        output.Write(Convert.ToString(x));
                        output.Write(";");
                        output.Write(Convert.ToString(x1));
                        output.Write(";");
                        output.Write(Convert.ToString(y));
                        output.Write(";");
                        output.Write(Convert.ToString(y2));
                        output.Write(";");
                        componentes++;
                        output.WriteLine();
                    }
                }
                output.Flush();
                output.Close();
            }
            else
            {
                MessageBox.Show("No se ha creado el informe.");
            }
            

        }
        /// <summary>
        /// Se encarga de poner el punto de inserción
        /// </summary>
        public void Insercion(double x,double y)
        {
            x_ins = x;
            y_ins = y;
            
        }
    }
}
