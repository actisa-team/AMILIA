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
        double ratio;
        int escala=1;
        List<Punto> polilinea = new List<Punto>();
        List<Punto> polilinea_inicial = new List<Punto>();
        List<PuntoPerfil> polilinea_perfil_inicial = new List<PuntoPerfil>();
        List<PuntoPerfil> polilinea_perfil = new List<PuntoPerfil>();
        List<Parabola> Lista_parabolas = new List<Parabola>();
        List<double> lista_puntos_rectas = new List<double>();
        dsApp datoApp = new dsApp();
        public List<Punto> Polilinea { get => polilinea; set => polilinea = value; }
        public List<PuntoPerfil> Polilinea_Perfil { get => polilinea_perfil; set => polilinea_perfil = value; }
        public CalculoPolilineaPerfil(ref dsApp a, int opcion, double ratio, int it,int escal)
        {
            try
            {
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
                    for (int i=0;i<25;i++)
                    {
                        polilinea = Suavizar(polilinea);
                        Vaciar_Puntos();
                        RellenarDatos();
                    }

                    Iguales();
                    Dibujar(1);
                    //Guardar();
                    /*  Vaciar_Puntos();
                      RellenarDatos();
                      if (dibujar)
                      {
                          Dibujar(0);
                      }

                      if (it == 1)
                      {
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
                              else if (opcion == 2)
                              {
                                  FiltradoCambioSentido(true, ref contador);
                                  if (contador == 0)
                                  {
                                      salida = false;
                                  }
                                  contador = 0;
                                  Vaciar_Puntos();
                                  RellenarDatos();
                              }
                              else if (opcion == 3)
                              {
                                  FiltradoGiroLongitud(ratio, true, ref contador);
                                  if (contador == 0)
                                  {
                                      salida = false;
                                  }
                                  contador = 0;
                                  Vaciar_Puntos();
                                  RellenarDatos();
                              }else if (opcion == 0)
                              {
                                  salida = false;
                              }
                          }

                      }
                      else
                      {
                          if (opcion == 1)
                          {
                              FiltradoDisrupcion(true, ref contador);
                          }
                          else if (opcion == 2)
                          {
                              FiltradoCambioSentido(true, ref contador);
                          }
                          else if (opcion == 3)
                          {
                              FiltradoGiroLongitud(ratio, true, ref contador);
                          }else if (opcion == 0)
                          {

                          }
                          Vaciar_Puntos();
                          RellenarDatos();
                      }


                      Dibujar(1);*/
                    //datoApp.WriteXml("prueba_modificada.aplitop");
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
            try
            {
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

                poly.Add(new Point3d(polilinea[i].p.X, polilinea[i].p.Y*escala, 0));
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
        public void RellenarPerfil(double v_ac)
        {
            //Rellenamos parametros equivalentes entre punto y punto perfil
            for (int i=0;i<polilinea.Count;i++)
            {
                polilinea_perfil.Add(new PuntoPerfil(polilinea[i]));
            }
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

                if (polilinea_perfil[i].tipo!=1)
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
                if (lista_puntos_rectas.Count-1>=x+1)
                {
                    if (polilinea_perfil_inicial[i].p.X > lista_puntos_rectas[x] && polilinea_perfil_inicial[i].p.X < lista_puntos_rectas[x + 1])
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
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
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
        public void CalcularEntreParabolas()
        {
            double a=0, b=0, c=0, d=0, e=0, f=0;
            double x1, x2;
            double ec_a, ec_b, ec_c,r1,r2;
            a = Lista_parabolas[1].parabola[0];
            b = Lista_parabolas[1].parabola[1];
            c = Lista_parabolas[1].parabola[2];

            d = Lista_parabolas[2].parabola[0];
            e = Lista_parabolas[2].parabola[1];
            f = Lista_parabolas[2].parabola[2];

            a = 1;
            b = 1;
            c = 1;

            d = 1;
            e = -2;
            f = 3;

            ec_a = d - ((d * d) / a) - (2 * d) + ((2 * (d * d)) / a);
            ec_b = e - (((e - b) / a) * d) - ((b * d) / a) - (e - b) - b + (((e - b) / a) * 2 * d) + ((b * d) / a);
            ec_c = f - (((e - b) * (e - b)) / (4 * a)) - ((b * e - (b * b)) / (2 * a)) - c + (((e - b) * (e - b)) /( 2 * a)) + ((b * e )/ (2 * a ))- ((b * b )/ (2 * a));

            r1 = (-ec_b + (Math.Sqrt(Math.Pow(ec_b, 2) - 4 * ec_a * ec_c))) / (2 * ec_a);
            r2 = (-ec_b - (Math.Sqrt(Math.Pow(ec_b, 2) - 4 * ec_a * ec_c))) / (2 * ec_a);

            x1 = (e - b + 2 * d * r1) / (2 * a);
            x2 = (e - b + 2 * d * r2) / (2 * a);
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
            for (int i = 1; i < l.Count - 1; i++)
            {
                Lista_suavizada_temp.Add(new Punto(new Point2d((l[i].p.X + l[i + 1].p.X) / 2, (l[i].p.Y + l[i + 1].p.Y) / 2)));
            }
            Lista_suavizada_temp.Add(new Punto(l[l.Count - 1].p));
            Lista_suavizada = new List<Punto>(Lista_suavizada_temp);
            for (int t = 0; t < 10; t++)
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
            //RellenarDatos();
            //Dibujar_Suavizado(Lista_suavizada);
            return Lista_suavizada;
        }
        public void QuitarSuavizado()
        {
            for (int i = 0; i < polilinea_inicial.Count; i++)
            {
                polilinea_perfil_inicial.Add(new PuntoPerfil(polilinea_inicial[i]));
            }
            RellenarDatos_inicial();
           
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
            double x0 = (-b) / (2 * a);

            if (op==1)
            {
                d = a - 0.01;
            }
            else
            {
                d = a + 0.01;
            }
            e = b * d / a;
            f = a * (x0 * x0) + b * x0 + c - d * (x0 * x0) - e * x0;

            Lista_parabolas[p].parabola[0] = d;
            Lista_parabolas[p].parabola[1] = e;
            Lista_parabolas[p].parabola[2] = f;

        }
        public void Cotas_Trazado(CalculoPolilinea poli)
        {
            List<Point3d> Polilinea3d = new List<Point3d>();
            List<Point3d> Polilinea3d_original = new List<Point3d>();
            Point3d p3d = new Point3d();
            foreach (var componente in poli.Mcomponenetes)
            {
                foreach (var componentPoint in componente.getComponentPoints())
                {
                    p3d = new Point3d(componentPoint[0], componentPoint[1],0);
                }
            }
            string miFileOut = string.Empty;
            string line;
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
                        MessageBox.Show("Este archivo no es correcto.\nNo es una polilinea 3D.","Información");
                        break;
                    }
                }
                file.Close();
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
            int r;
            double x, y,z;
            z = (p2.Z - p1.Z) / partes;
            for (int i=1; i<=partes;i++)
            {
                r = (partes - i) / i;
                x = (p1.X + r * p2.X) / (1 + r);
                y = (p1.Y + r * p2.Y) / (1 + r);
                segmento.Add(new Point3d(x,y,p1.Z+z));
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
                if (Distancia(p1,p2)>0.1)
                {
                    polilinea_temp.Add(polilinea[i]);
                    partes = (int)Math.Truncate(Distancia(p1, p2)*10);
                    if (partes>1)
                    {
                        segmento = Dividir_Segmento(polilinea[i], polilinea[i + 1], partes);
                    }
                    else
                    {
                        segmento = Dividir_Segmento(polilinea[i], polilinea[i + 1], 2);
                    }
                    for (int t=0;t< segmento.Count;t++)
                    {
                        polilinea_temp.Add(segmento[i]);
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
    }
}
