using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica.Componentes;
using Logica;
using Autodesk.AutoCAD.Geometry;
using System.Reflection;
using System.Diagnostics;
using Datos;
using Terraplan.Logica;
using engCadNet;

namespace interfaz
{
    public partial class VistaComponentes : Form
    {
        private List<Logica.Componente> Componentes = new List<Logica.Componente>();
        private List<ComponenteTabla> ComponentesTabla = new List<ComponenteTabla>();
        private int id_componente = 0;

        // Expone los resultados del último Crear_Trazado para que el llamador pueda guardarlos
        public List<EjeDeTrazado.componentes.Componente> LastMComponentes { get; private set; }
        public List<Autodesk.AutoCAD.Geometry.Point2d> LastTrazado { get; private set; }
        public VistaComponentes()
        {
            InitializeComponent();
            IniciarComponentes();
        }
        private void IniciarComponentes()
        {

            //dataGridView1.DataSource = ComponentesTabla;
            dataGridView1.DataSource = dsApp1.Tables["Componentes"];
            OcultarColumnasExcepto("tipo", "azimut", "punto_inicial", "punto_final", "centro", "radio");
            //dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            //dataGridView1.AllowUserToAddRows = false;
        }

        private void AniadirEntidad_Click(object sender, EventArgs e)
        {

            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            Document acDoc2 = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {
                Editor ed = acDoc2.Editor;
                PromptEntityOptions opts = new PromptEntityOptions("\nSeleccione una línea o un arco:");
                opts.SetRejectMessage("\nDebe seleccionar una línea o un arco.");
                opts.AddAllowedClass(typeof(Line), false);
                opts.AddAllowedClass(typeof(Arc), false);
                PromptEntityResult res = ed.GetEntity(opts);

                Logica.Componente c = new Logica.Componente();
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
                            // Mostrar formulario de edición antes de agregar
                            EditarParametros form = new EditarParametros();
                            form.CargarDatos(c);
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                // Si el usuario confirma, actualiza los valores editados
                                c = form.c;
                                calculoPolilinea.Rellenar_Recta(c);
                                Componentes.Add(c);
                                ed.WriteMessage("\nSeleccionó una línea.");
                                Add_fila_componentes("recta", "", c.lista_puntos[0].p.ToString(), c.lista_puntos[c.lista_puntos.Count() - 1].p.ToString(),
                                0, c.azte, c.azts, "", 0, 0, c.azr, 0, 0, "", id_componente);
                                id_componente++;
                                //ed.WriteMessage("\nSeleccionó una línea.");
                            }

                            /*Componentes.Add(c);
                            ComponentesTabla.Add(new ComponenteTabla()
                            {
                                azr = c.azr,
                                lista_puntos = c.lista_puntos,
                                radio = c.radio,
                                Tipo = c.Tipo,
                                xc = c.xc,
                                yc = c.yc
                            });*/

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
                            Punto p2 = calculoPolilinea.Crear_Curva_2Puntos_Modificada(p1, p3, c.xc, c.yc, c.radio, c);
                            c.lista_puntos.Add(p1);
                            c.lista_puntos.Add(p2);
                            c.lista_puntos.Add(p3);
                            c.Tipo = 2;
                            EditarParametros form = new EditarParametros();
                            form.CargarDatos(c);
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                // Si el usuario confirma, actualiza los valores editados
                                c = form.c;
                                calculoPolilinea.Rellenar_Curva(c);
                                Componentes.Add(c);
                                ed.WriteMessage("\nSeleccionó una curva.");
                                Add_fila_componentes("curva", new Point3d(c.xc, c.yc, 0).ToString(),
                                    new Point3d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y, 0).ToString(),
                                    new Point3d(c.lista_puntos[c.lista_puntos.Count() - 1].p.X, c.lista_puntos[c.lista_puntos.Count() - 1].p.Y, 0).ToString(),
                               c.radio, c.azte, c.azts, "", 0, 0, c.azr, 0, 0, "", id_componente);
                                id_componente++;
                            }
                            /*Componentes.Add(c);
                            ComponentesTabla.Add(new ComponenteTabla() { 
                                azr=c.azr,
                                lista_puntos=c.lista_puntos,
                                radio=c.radio,
                                Tipo=c.Tipo,
                                xc=c.xc,
                                yc=c.yc
                            });
                           


                            ed.WriteMessage("\nSeleccionó un arco.");*/
                        }
                        else
                        {
                            ed.WriteMessage("\nNo es una línea ni un arco.");
                        }

                        tr.Commit();
                    }
                }
            }
            this.Show();

        }
        private void Add_fila_componentes(string tipo, string centro, string punto_inicial, string punto_final, double radio,
            double az_inicial, double az_final, string sentido, double pk_inicial, double pk_final, double azimut, double a,
            double le, string tipo_componente, int id_componente, bool insertar = false)
        {
            System.Data.DataTable dt = dsApp1.Tables["Componentes"];
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["tipo"] = tipo;
            nuevaFila["centro"] = centro;
            nuevaFila["punto_inicial"] = punto_inicial;
            nuevaFila["punto_final"] = punto_final;
            nuevaFila["radio"] = radio;
            nuevaFila["az_inicial"] = az_inicial;
            nuevaFila["az_final"] = az_final;
            nuevaFila["sentido"] = sentido;
            nuevaFila["pk_inicial"] = pk_inicial;
            nuevaFila["pk_final"] = pk_final;
            nuevaFila["azimut"] = azimut;
            nuevaFila["a"] = a;
            nuevaFila["le"] = le;
            nuevaFila["tipo_clotoide"] = tipo_componente;
            nuevaFila["id_componente"] = id_componente;
            if (insertar)
            {
                InsertarComponenteConDesplazamiento(nuevaFila);
            }
            else
            {
                dt.Rows.Add(nuevaFila);
            }

        }
        public void InsertarComponenteConDesplazamiento(DataRow nuevaFila)
        {
            System.Data.DataTable dt = dsApp1.Tables["Componentes"];
            int nuevoId = Convert.ToInt32(nuevaFila["id_componente"]);

            // Primero actualizamos los id_componente que sean mayores o iguales al nuevo id
            foreach (DataRow fila in dt.Rows)
            {
                int idActual = Convert.ToInt32(fila["id_componente"]);
                if (idActual >= nuevoId)
                {
                    fila["id_componente"] = idActual + 1;
                }
            }

            // Ahora agregamos la nueva fila
            dt.Rows.Add(nuevaFila);

            // Opcional: ordenar las filas por id_componente si necesitas mantener orden
            dt.DefaultView.Sort = "id_componente ASC";
            dt = dt.DefaultView.ToTable();
        }
        private void Modificar_fila_componentes(int id_componente, string tipo, string centro, string punto_inicial, string punto_final, double radio,
    double az_inicial, double az_final, string sentido, double pk_inicial, double pk_final, double azimut, double a,
    double le, string tipo_componente)
        {
            System.Data.DataTable dt = dsApp1.Tables["Componentes"];

            // Buscar la fila por id_componente
            DataRow[] filas = dt.Select($"id_componente = {id_componente}");

            if (filas.Length > 0)
            {
                DataRow fila = filas[0];
                fila["tipo"] = tipo;
                fila["centro"] = centro;
                fila["punto_inicial"] = punto_inicial;
                fila["punto_final"] = punto_final;
                fila["radio"] = radio;
                fila["az_inicial"] = az_inicial;
                fila["az_final"] = az_final;
                fila["sentido"] = sentido;
                fila["pk_inicial"] = pk_inicial;
                fila["pk_final"] = pk_final;
                fila["azimut"] = azimut;
                fila["a"] = a;
                fila["le"] = le;
                fila["tipo_clotoide"] = tipo_componente;
            }
            else
            {
                // Opcional: mensaje de error o manejo si no se encuentra el id
                Console.WriteLine($"No se encontró una fila con id_componente = {id_componente}");
            }
        }
        private void OcultarColumnasExcepto(params string[] nombresVisibles)
        {
            foreach (DataGridViewColumn columna in dataGridView1.Columns)
            {
                if (nombresVisibles.Contains(columna.Name))
                {
                    columna.Visible = true;  // Mostrar columnas deseadas
                }
                else
                {
                    columna.Visible = false; // Ocultar el resto
                }
            }
        }

       

        private void Crear_trazado_Click(object sender, EventArgs e)
        {
            Crear_Trazado(2500, Componentes);
        }
        public bool SeCortan(Point2d c1, Point2d c2, double r1, double r2)
        {
            double distancia = Distancia(c1, c2);

            // Comprobar si se cortan según las condiciones geométricas
            if (distancia > Math.Abs(r1 - r2) && distancia < (r1 + r2))
            {
                return true;  // Se intersecan
            }
            else if (distancia == Math.Abs(r1 - r2) || distancia == (r1 + r2))
            {
                return true;  // Se tocan en un solo punto (internamente o externamente)
            }

            return false;  // No se cortan
        }



        public void CrearCurva(List<Logica.Componente> componentes, int i)
        {
            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            double[] vert = calculoPolilinea.Vertice_rectas(componentes[i], componentes[i + 1]);
            double d1 = Distancia(componentes[i].lista_puntos[0].p, new Point2d(vert[0], vert[1]));
            double d2 = Distancia(componentes[i + 1].lista_puntos[componentes[i + 1].lista_puntos.Count - 1].p, new Point2d(vert[0], vert[1]));
            double radio = 0;
            double dif_az = Math.Abs(componentes[i].azr - componentes[i + 1].azr);
            if (d1 < d2)
            {
                radio = d1 / (dif_az * Math.PI / 180);
            }
            else
            {
                radio = d2 / (dif_az * Math.PI / 180);
            }
            double A = Math.Pow((12 * Math.Pow(radio, 3)), 0.25);
            EntidadFlotante entidadFlotante = new EntidadFlotante();
            entidadFlotante.CargarDatos("CURVA", radio, i + 1, A);

            bool clotoides = false;
            if (entidadFlotante.ShowDialog() == DialogResult.OK)  // Solo ShowDialog() para evitar doble ventana
            {
                radio = entidadFlotante.dato;
                if (entidadFlotante.clotoide)
                {
                    clotoides = true;
                    A = entidadFlotante.A;
                }
            }
            if (clotoides)
            {

                EjeDeTrazado.componentes.Curva curva = Curva_No_Paso(componentes[i], radio, componentes[i + 1], 1, A);
                /* componentes.Insert(i + 1, new Logica.Componente(new Punto(new Point2d(curva.getPuntoEntrada.coordenadaX, curva.getPuntoEntrada.coordenadaY)), 2));
                 componentes[i + 1].add(new Punto(new Point2d(curva.puntoMedioCurva.coordenadaX, curva.puntoMedioCurva.coordenadaY)));
                 componentes[i + 1].add(new Punto(new Point2d(curva.getPuntoSalida.coordenadaX, curva.getPuntoSalida.coordenadaY)));*/
                componentes.Insert(i + 1, new Logica.Componente(new Punto(new Point2d(curva.getPuntoEntrada.coordenadaX, curva.getPuntoEntrada.coordenadaY)), 2));
                componentes[i + 1].add(new Punto(new Point2d(curva.puntoMedioCurva.coordenadaX, curva.puntoMedioCurva.coordenadaY)));
                componentes[i + 1].add(new Punto(new Point2d(curva.getPuntoSalida.coordenadaX, curva.getPuntoSalida.coordenadaY)));
                componentes[i + 1].radio = curva.getRadio;
                componentes[i + 1].curva_creada = true;
                componentes[i + 1].curva_con_clotoide = true;
            }
            else
            {
                EjeDeTrazado.componentes.Curva curva = Curva_Gran_Radio(componentes[i], radio / 2, componentes[i + 1]);
                componentes.Insert(i + 1, new Logica.Componente(new Punto(new Point2d(curva.getPuntoEntrada.coordenadaX, curva.getPuntoEntrada.coordenadaY)), 2));
                componentes[i + 1].add(new Punto(new Point2d(curva.puntoMedioCurva.coordenadaX, curva.puntoMedioCurva.coordenadaY)));
                componentes[i + 1].add(new Punto(new Point2d(curva.getPuntoSalida.coordenadaX, curva.getPuntoSalida.coordenadaY)));
                componentes[i + 1].radio = curva.getRadio;
                componentes[i + 1].curva_creada = true;
                componentes[i + 1].curva_sin_clotoide = true;
            }

            Rellenar_Curva(componentes[i + 1]);
            componentes[i+1].direccion = SentidoArco(componentes[i+1].xc, componentes[i+1].yc, componentes[i+1].lista_puntos[0].p.X, componentes[i+1].lista_puntos[0].p.Y,
                            componentes[i+1].lista_puntos[componentes[i+1].lista_puntos.Count() - 1].p.X,
                            componentes[i+1].lista_puntos[componentes[i+1].lista_puntos.Count() - 1].p.Y);
        }
        private EjeDeTrazado.componentes.Curva Curva_Gran_Radio(Logica.Componente c1, double radio, Logica.Componente c3)
        {
            //ec de la primera recta
            double a_x0 = c1.lista_puntos[0].p.X;
            double a_y0 = c1.lista_puntos[0].p.Y;
            double b_x1 = c1.lista_puntos[1].p.X;
            double b_y1 = c1.lista_puntos[1].p.Y;

            double a_1 = (a_y0 - b_y1) / (a_x0 - b_x1);
            double b_1 = -b_x1 * (a_y0 - b_y1) / (a_x0 - b_x1) + b_y1;

            double c_x0 = c3.lista_puntos[0].p.X;
            double c_y0 = c3.lista_puntos[0].p.Y;
            double d_x1 = c3.lista_puntos[1].p.X;
            double d_y1 = c3.lista_puntos[1].p.Y;

            double c_1 = (c_y0 - d_y1) / (c_x0 - d_x1);
            double d_1 = -d_x1 * ((c_y0 - d_y1) / (c_x0 - d_x1)) + d_y1;

            double i_x = (d_1 - b_1) / (a_1 - c_1);
            double i_y = a_1 * ((d_1 - b_1) / (a_1 - c_1)) + b_1;
            tadLayShare.puntos.Punto3d p_vertice = new tadLayShare.puntos.Punto3d(i_x, i_y, 0);
            /*Punto3d[] puntosSing = addCurvaGranRadio(radio, c1.azr, c3.azr, p_vertice, getSentidoCurva(c1.azr, c3.azr));

            return new Curva(puntosSing[1], puntosSing[2], puntosSing[4], radio, 0, 0, getSentidoCurva(c1.azr, c3.azr));*/
            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            tadLayShare.puntos.Punto3d[] puntosSing = calculoPolilinea.addCurvaGranRadio(radio * 2, c1.azr, c3.azr, p_vertice, calculoPolilinea.getSentidoCurva(c1.azr, c3.azr));

            return new EjeDeTrazado.componentes.Curva(puntosSing[1], puntosSing[2], puntosSing[4], radio * 2, 0, 0, calculoPolilinea.getSentidoCurva(c1.azr, c3.azr));

        }
        /// <summary>
        /// Comprobar si hay alguna secuencia de curva - curva de distinto sentido y que no se corten
        /// </summary>
        public void ComprobarCurvasDifSentido(List<Logica.Componente> componentes)
        {
            for (int i = 0; i < componentes.Count() - 1; i++)
            {
                if (componentes[i].Tipo == 2 && componentes[i + 1].Tipo == 2)
                {

                    double distancia = Distancia(new Point2d(componentes[i].xc, componentes[i].yc), new Point2d(componentes[i + 1].xc, componentes[i + 1].yc));
                    if (distancia > componentes[i + 1].radio + componentes[i].radio &&
                        distancia + Math.Min(componentes[i + 1].radio, componentes[i].radio) > Math.Max(componentes[i + 1].radio, componentes[i].radio))
                    {
                        //Crear_RECD(componentes,i);
                        Crear_RECT(componentes, i);
                        bool estanOrdenados = false;
                        int contador = 0;
                        double minoracion = 0.5;
                        while (!estanOrdenados)
                        {
                            Giro_Tangente(componentes, i + 1, minoracion);

                            var res = Curva_Recta_Curva(componentes[i], componentes[i + 1], componentes[i + 2]);
                            tadLayShare.puntos.Punto3d p1 = res[0].getPuntoSalida;

                            tadLayShare.puntos.Punto3d p2 = res[1].getPuntoEntrada;
                            double distancia_p = p1.distancia(p2);
                            if (distancia_p < 200)
                            {
                                minoracion = 0.2;
                            }
                            if (distancia_p < 100)
                            {
                                minoracion = 0.5;
                            }
                            if (distancia_p < 50)
                            {
                                minoracion = 1;
                            }
                            if (distancia_p <10)
                            {
                                minoracion = 10;
                            }
                            if (distancia_p < 1)
                            {
                                minoracion = 100;
                            }
                            if (distancia_p < 0.1)
                            {
                                minoracion = 1000;
                            }
                            if (distancia_p < 0.01)
                            {
                                minoracion = 10000;
                            }
                            Point2d pl1 = componentes[i + 1].lista_puntos[0].p;

                            Point2d pl2 = componentes[i + 1].lista_puntos[1].p;

                            estanOrdenados = EstanOrdenados(pl1.X, pl1.Y, pl2.X, pl2.Y, p1.coordenadaX, p1.coordenadaY, p2.coordenadaX, p2.coordenadaY);
                            tadLayShare.puntos.Punto3d p22 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[0].p.X, componentes[i + 1].lista_puntos[0].p.Y, 0);
                            tadLayShare.puntos.Punto3d p33 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[1].p.X, componentes[i + 1].lista_puntos[1].p.Y, 0);
                            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();

                            //calculoPolilinea.Dibujar_r(p22, p33, "pruebaaa");
                            contador++;
                            if (estanOrdenados)
                            {
                                double dis=p22.distancia(p33);
                                Rellenar_Recta(componentes[i + 1]);

                            }
                        }
                        // La entidad se añadirá al DataGrid al final en ActualizarDataGridDesdeComponentes()
                    }
                    else if (SeCortan(new Point2d(componentes[i].xc, componentes[i].yc), new Point2d(componentes[i + 1].xc, componentes[i + 1].yc), componentes[i].radio, componentes[i + 1].radio))
                    {
                        Crear_RECT(componentes, i);
                        Rellenar_Recta(componentes[i + 1]);
                        tadLayShare.puntos.Punto3d p22 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[0].p.X, componentes[i + 1].lista_puntos[0].p.Y, 0);
                        tadLayShare.puntos.Punto3d p33 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[1].p.X, componentes[i + 1].lista_puntos[1].p.Y, 0);
                        CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
                        // La entidad se añadirá al DataGrid al final en ActualizarDataGridDesdeComponentes()
                        //calculoPolilinea.Dibujar_r(p22, p33, "pruebaaa");
                    }
                }
                if (componentes[i].Tipo == 1 && componentes[i + 1].Tipo == 1)
                {
                    CrearCurva(componentes, i);
                    // La curva se añadirá al DataGrid al final en ActualizarDataGridDesdeComponentes()
                    //CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
                    //calculoPolilinea.Dibujar_c(componentes[i+1].xc, componentes[i + 1].yc, componentes[i + 1].radio,componentes[i + 1].azte, componentes[i + 1].azts,"curva_creada");
                }
            }
        }
        public List<Logica.Componente> Crear_Trazado(double gran_r, List<Logica.Componente> componentes, string nombreEje="Eje 1")
        {

            List<EjeDeTrazado.componentes.Componente> mcomponenetes = new List<EjeDeTrazado.componentes.Componente>();
            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            EjeDeTrazado.componentes.Clotoide Clo_Entre_Curvas = null;
            bool clo_salida = false;
            List<Tuple<int, int>> solapes = new List<Tuple<int, int>>();
            for (int i = 0; i < componentes.Count(); i++)
            {
                if (componentes[i].Tipo == 1)
                {
                    Rellenar_Recta(componentes[i]);
                }
                if (componentes[i].Tipo == 2)
                {
                    Rellenar_Curva(componentes[i]);
                    componentes[i].direccion = SentidoArco(componentes[i].xc, componentes[i].yc, componentes[i].lista_puntos[0].p.X, componentes[i].lista_puntos[0].p.Y,
                        componentes[i].lista_puntos[componentes[i].lista_puntos.Count() - 1].p.X,
                        componentes[i].lista_puntos[componentes[i].lista_puntos.Count() - 1].p.Y);
                }
            }
            ComprobarCurvasDifSentido(componentes);
            for (int i = 0; i < componentes.Count(); i++)
            {
                if (componentes[i].Tipo == 1)
                {
                    Rellenar_Recta(componentes[i]);
                }
                if (componentes[i].Tipo == 2)
                {
                    Rellenar_Curva(componentes[i]);
                    componentes[i].direccion = SentidoArco(componentes[i].xc, componentes[i].yc, componentes[i].lista_puntos[0].p.X, componentes[i].lista_puntos[0].p.Y,
                        componentes[i].lista_puntos[componentes[i].lista_puntos.Count() - 1].p.X,
                        componentes[i].lista_puntos[componentes[i].lista_puntos.Count() - 1].p.Y);
                }
            }
            mcomponenetes = calculoPolilinea.Crear_Trazado_Modificado(gran_r, componentes);
            if (mcomponenetes == null || mcomponenetes.Count()==0)
            {
                MessageBox.Show("No ha dado solución");
                return componentes;
            }
            calculoPolilinea.DibujarTrazado(mcomponenetes, nombreEje);
            calculoPolilinea.Rotulado_final(mcomponenetes, 100, true, nombreEje);


            //if (componentes.Count >= 2){
            //    for (int i = 0; i < componentes.Count(); i++)
            //    {
            //        if (componentes[i].Tipo == 1)
            //        {
            //            Rellenar_Recta(componentes[i]);
            //        }
            //        if (componentes[i].Tipo == 2)
            //        {
            //            Rellenar_Curva(componentes[i]);
            //            componentes[i].direccion = SentidoArco(componentes[i].xc, componentes[i].yc, componentes[i].lista_puntos[0].p.X, componentes[i].lista_puntos[0].p.Y,
            //                componentes[i].lista_puntos[componentes[i].lista_puntos.Count() - 1].p.X,
            //                componentes[i].lista_puntos[componentes[i].lista_puntos.Count() - 1].p.Y);
            //        }
            //    }
            //    /*
            //     * Comprobar si hay alguna secuencia de curva - curva de distinto sentido y que no se corten
            //     */
            //    ComprobarCurvasDifSentido(componentes);
            //    for (int i = 0; i < componentes.Count(); i++)
            //    {
            //        if (componentes[i].Tipo == 1)
            //        {
            //            Rellenar_Recta(componentes[i]);
            //        }
            //        if (componentes[i].Tipo == 2)
            //        {
            //            Rellenar_Curva(componentes[i]);
            //            componentes[i].direccion = SentidoArco(componentes[i].xc, componentes[i].yc, componentes[i].lista_puntos[0].p.X, componentes[i].lista_puntos[0].p.Y,
            //                componentes[i].lista_puntos[componentes[i].lista_puntos.Count() - 1].p.X,
            //                componentes[i].lista_puntos[componentes[i].lista_puntos.Count() - 1].p.Y);
            //        }
            //    }
            //}
            //if (componentes.Count > 1)
            //{
            //    for (int i = 0; i < componentes.Count() - 1; i++)
            //    {
            //        if (componentes[i].Tipo == 1 && componentes[i + 1].Tipo == 2)
            //        {
            //            EjeDeTrazado.componentes.Clotoide Clo = Recta_Curva(componentes[i], componentes[i + 1], 1);
            //            if (Clo == null)
            //            {
            //                MessageBox.Show("No ha dado resueltado entre las componentes " + (i + 1) + " y " + (i + 2));
            //                break;
            //            }
            //            tadLayShare.puntos.Punto3d pl1 = new tadLayShare.puntos.Punto3d(componentes[i].lista_puntos[0].p.X, componentes[i].lista_puntos[0].p.Y, 0);
            //            if (i > 0)
            //            {
            //                var clo_ant = mcomponenetes[mcomponenetes.Count() - 1];
            //                pl1 = new tadLayShare.puntos.Punto3d(clo_ant.getPuntoSalida.coordenadaX, clo_ant.getPuntoSalida.coordenadaY, 0);
            //            }

            //            tadLayShare.puntos.Punto3d pl2 = new tadLayShare.puntos.Punto3d(Clo.getPuntoEntrada.coordenadaX, Clo.getPuntoEntrada.coordenadaY, 0);

            //            EjeDeTrazado.componentes.Linea linea = new EjeDeTrazado.componentes.Linea(pl1, pl2, 0, 0, componentes[i].azr);
            //            double longitud_recta = linea.getLongitud();
            //            if (longitud_recta > 0.001)
            //            {
            //                mcomponenetes.Add(linea);
            //            }
            //            if (Clo.getLongitud() > 1)
            //            {
            //                mcomponenetes.Add(Clo);

            //            }
            //        }
            //        if (componentes[i].Tipo == 2 && componentes[i + 1].Tipo == 1)
            //        {
            //            EjeDeTrazado.componentes.Clotoide Clo = Curva_Recta(componentes[i], componentes[i + 1], 1);
            //            if (Clo == null)
            //            {
            //                MessageBox.Show("No ha dado resueltado entre las componentes " + (i + 1) + " y " + (i + 2));
            //                break;
            //            }
            //            tadLayShare.puntos.Punto3d p3d = new tadLayShare.puntos.Punto3d(componentes[i].lista_puntos[0].p.X, componentes[i].lista_puntos[0].p.Y, 0);
            //            tadLayShare.puntos.Punto3d p3d2 = new tadLayShare.puntos.Punto3d(Clo.getPuntoEntrada.coordenadaX, Clo.getPuntoEntrada.coordenadaY, 0);
            //            tadLayShare.puntos.Punto3d p3dc = new tadLayShare.puntos.Punto3d(componentes[i].xc, componentes[i].yc, 0);
            //            if (i > 0)
            //            {
            //                var clo_ant = mcomponenetes[mcomponenetes.Count() - 1];
            //                p3d = new tadLayShare.puntos.Punto3d(clo_ant.getPuntoSalida.coordenadaX, clo_ant.getPuntoSalida.coordenadaY, 0);
            //            }

            //            EjeDeTrazado.componentes.Curva Curvaa = new EjeDeTrazado.componentes.Curva(p3d, p3d2, p3dc, componentes[i].radio, 0, 0, componentes[i].direccion);
            //            mcomponenetes.Add(Curvaa);
            //            if (Clo.getLongitud() > 1)
            //            {
            //                mcomponenetes.Add(Clo);

            //            }
            //        }
            //        if (componentes[i].Tipo == 2 && componentes[i + 1].Tipo == 2)
            //        {
            //            Rellenar_Curva(componentes[i]);
            //            Rellenar_Curva(componentes[i + 1]);
            //            EjeDeTrazado.componentes.Clotoide Clo = Curva_Curva_M(componentes[i], componentes[i + 1]);

            //            if (Clo == null)
            //            {
            //                MessageBox.Show("No ha dado resueltado entre las componentes " + (i + 1) + " y " + (i + 2));
            //                break;
            //            }
            //            if (componentes[i].radio < componentes[i + 1].radio)
            //            {
            //                double xx = 0;
            //                double yy = 0;
            //                if (mcomponenetes[mcomponenetes.Count - 1].getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
            //                {
            //                    xx = mcomponenetes[mcomponenetes.Count - 1].getPointAtDist(mcomponenetes[mcomponenetes.Count - 1].Get_Le_r())[0];
            //                    yy = mcomponenetes[mcomponenetes.Count - 1].getPointAtDist(mcomponenetes[mcomponenetes.Count - 1].Get_Le_r())[1];
            //                }
            //                else
            //                {
            //                    xx = mcomponenetes[mcomponenetes.Count - 1].getPuntoSalida.coordenadaX;
            //                    yy = mcomponenetes[mcomponenetes.Count - 1].getPuntoSalida.coordenadaY;
            //                }

            //                tadLayShare.puntos.Punto3d p3d = new tadLayShare.puntos.Punto3d(xx, yy, 0);
            //                tadLayShare.puntos.Punto3d p3d2 = new tadLayShare.puntos.Punto3d(Clo.getPointAtDist(0)[0], Clo.getPointAtDist(0)[1], 0);
            //                tadLayShare.puntos.Punto3d p3dc = new tadLayShare.puntos.Punto3d(componentes[i].xc, componentes[i].yc, 0);
            //                EjeDeTrazado.componentes.Curva Curvaa = new EjeDeTrazado.componentes.Curva(p3d, p3d2, p3dc, componentes[i].radio, 0, 0, componentes[i].direccion);
            //                mcomponenetes.Add(Curvaa);
            //                mcomponenetes.Add(Clo);
            //                Clo_Entre_Curvas = Clo;
            //                clo_salida = true;

            //            }
            //            else
            //            {
            //                double xx = 0;
            //                double yy = 0;
            //                double xx2 = 0;
            //                double yy2 = 0;
            //                if (mcomponenetes[mcomponenetes.Count - 1].getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
            //                {
            //                    xx = mcomponenetes[mcomponenetes.Count - 1].getPointAtDist(mcomponenetes[mcomponenetes.Count - 1].Get_Le_r())[0];
            //                    yy = mcomponenetes[mcomponenetes.Count - 1].getPointAtDist(mcomponenetes[mcomponenetes.Count - 1].Get_Le_r())[1];
            //                }
            //                else
            //                {
            //                    xx = mcomponenetes[mcomponenetes.Count - 1].getPuntoSalida.coordenadaX;
            //                    yy = mcomponenetes[mcomponenetes.Count - 1].getPuntoSalida.coordenadaY;
            //                }
            //                tadLayShare.puntos.Punto3d p3d = new tadLayShare.puntos.Punto3d(xx, yy, 0);
            //                if (Clo.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida)
            //                {
            //                    xx2 = Clo.getPointAtDist(0)[0];
            //                    yy2 = Clo.getPointAtDist(0)[1];
            //                }
            //                else
            //                {
            //                    xx2 = Clo.getPointAtDist(Clo.getLe_r())[0];
            //                    yy2 = Clo.getPointAtDist(Clo.getLe_r())[1];
            //                }
            //                tadLayShare.puntos.Punto3d p3d2 = new tadLayShare.puntos.Punto3d(xx2, yy2, 0);
            //                tadLayShare.puntos.Punto3d p3dc = new tadLayShare.puntos.Punto3d(componentes[i].xc, componentes[i].yc, 0);
            //                EjeDeTrazado.componentes.Curva Curvaa = new EjeDeTrazado.componentes.Curva(p3d, p3d2, p3dc, componentes[i].radio, 0, 0, componentes[i].direccion);
            //                mcomponenetes.Add(Curvaa);
            //                //Escribir_fichero(Clo.getLongitud() + " 4");
            //                mcomponenetes.Add(Clo);
            //                Clo_Entre_Curvas = Clo;
            //                clo_salida = true;
            //            }

            //        }
            //    }
            //    if (componentes[componentes.Count() - 1].Tipo == 1)
            //    {
            //        var clo_ant_ultima = mcomponenetes[mcomponenetes.Count() - 1];
            //        tadLayShare.puntos.Punto3d pl1 = new tadLayShare.puntos.Punto3d(clo_ant_ultima.getPuntoSalida.coordenadaX, clo_ant_ultima.getPuntoSalida.coordenadaY, 0);
            //        tadLayShare.puntos.Punto3d pl2 = new tadLayShare.puntos.Punto3d(componentes[componentes.Count() - 1].lista_puntos[1].p.X, componentes[componentes.Count() - 1].lista_puntos[1].p.Y, 0);

            //        tadLayShare.puntos.Punto3d pl2_ant = new tadLayShare.puntos.Punto3d(componentes[componentes.Count() - 1].lista_puntos[0].p.X, componentes[componentes.Count() - 1].lista_puntos[0].p.Y, 0);
            //        var compo_ant = mcomponenetes[mcomponenetes.Count() - 1];
            //        var punto = compo_ant.getPointAtDist(compo_ant.getLongitud());
            //        tadLayShare.puntos.Punto3d p = new tadLayShare.puntos.Punto3d(punto[0], punto[1], 0);

            //        if (!EsAntesDelSegundoPunto(pl2_ant, pl2, p))
            //        {
            //            solapes.Add(Tuple.Create(componentes.Count() - 2, componentes.Count() - 1));
            //        }
            //        EjeDeTrazado.componentes.Linea linea = new EjeDeTrazado.componentes.Linea(pl1, pl2, 0, 0, componentes[componentes.Count() - 1].azr);
            //        mcomponenetes.Add(linea);
            //    }
            //    if (componentes[componentes.Count() - 1].Tipo == 2)
            //    {
            //        var clo_ant_ultima = mcomponenetes[mcomponenetes.Count() - 1];
            //        tadLayShare.puntos.Punto3d p3d = new tadLayShare.puntos.Punto3d(clo_ant_ultima.getPuntoSalida.coordenadaX, clo_ant_ultima.getPuntoSalida.coordenadaY, 0);
            //        tadLayShare.puntos.Punto3d p3d2 = new tadLayShare.puntos.Punto3d(componentes[componentes.Count() - 1].lista_puntos[componentes[componentes.Count() - 1].lista_puntos.Count()-1].p.X, componentes[componentes.Count() - 1].lista_puntos[componentes[componentes.Count() - 1].lista_puntos.Count() - 1].p.Y, 0);
            //        tadLayShare.puntos.Punto3d p3dc = new tadLayShare.puntos.Punto3d(componentes[componentes.Count() - 1].xc, componentes[componentes.Count() - 1].yc, 0);

            //        EjeDeTrazado.componentes.Curva Curvaa = new EjeDeTrazado.componentes.Curva(p3d, p3d2, p3dc, componentes[componentes.Count() - 1].radio, 0, 0, componentes[componentes.Count() - 1].direccion);
            //        mcomponenetes.Add(Curvaa);


            //    }

            //    calculoPolilinea.DibujarTrazado(mcomponenetes, nombreEje);
            //    calculoPolilinea.Rotulado_final(mcomponenetes, 200, true, nombreEje);
            //    for (int i = componentes.Count() - 1; i > 0; i--)
            //    {
            //        if (componentes[i].creacion == 1 || componentes[i].creacion == 2)
            //        {
            //            componentes.RemoveAt(i);
            //            mcomponenetes.RemoveAt(i);
            //        }
            //    }
                
                
            //}else{


            //}
            if (solapes.Count() > 0)
            {
                string salida = "Hay solapes entre las entidades: \n";
                foreach (var item in solapes)
                {
                    salida += "- " + item.Item1 + " y " + item.Item2 + "\n";
                }
                MessageBox.Show(salida);
            }

            // Guardar todos los ejes en archivo .amilia tras calcular
            LastMComponentes = mcomponenetes;
            LastTrazado = calculoPolilinea.Trazado_prueba;
            //principal.Guardar_Json(Componentes, null, mcomponenetes, calculoPolilinea.Trazado_prueba);
            return componentes;
        }
        private EjeDeTrazado.componentes.Clotoide[] Curva_Recta_Curva(Logica.Componente c1, Logica.Componente c2, Logica.Componente c3)
        {
            EjeDeTrazado.componentes.Clotoide[] clotoides = new EjeDeTrazado.componentes.Clotoide[2];
            Rellenar_Curva(c1);
            Rellenar_Recta(c2);
            Rellenar_Curva(c3);
            EjeDeTrazado.componentes.Clotoide clo1 = Curva_Recta(c1, c2, 2);
            // Dibujar_Clotoide(clo1);
            EjeDeTrazado.componentes.Clotoide clo2 = Recta_Curva(c2, c3, 1);
            // Dibujar_Clotoide(clo2);

            if (clo1 != null)
            {
                clotoides[0] = clo1;
            }

            if (clo2 != null)
            {
                clotoides[1] = clo2;
            }


            return clotoides;
        }
        public static bool EstanOrdenados(double x0, double y0, double x1, double y1, double xA, double yA, double xB, double yB)
        {
            // Determinar la dirección de la línea
            double dx = x1 - x0;
            double dy = y1 - y0;

            // Proyectamos los puntos en la dirección de la línea usando el producto escalar
            double proyeccionA = (xA - x0) * dx + (yA - y0) * dy;
            double proyeccionB = (xB - x0) * dx + (yB - y0) * dy;

            // Verificamos si están en el orden correcto
            return proyeccionA <= proyeccionB;
        }
        public static bool EsAntesDelSegundoPunto(tadLayShare.puntos.Punto3d A, tadLayShare.puntos.Punto3d B, tadLayShare.puntos.Punto3d P)
        {
            // Verificar si A y B son el mismo punto
            if (A.coordenadaX == B.coordenadaX &&
                A.coordenadaY == B.coordenadaY &&
                A.coordenadaZ == B.coordenadaZ)
            {
                return false;
            }

            // Calcular t en cada dimensión
            double tX = (B.coordenadaX - A.coordenadaX) != 0 ? (P.coordenadaX - A.coordenadaX) / (B.coordenadaX - A.coordenadaX) : double.NaN;
            double tY = (B.coordenadaY - A.coordenadaY) != 0 ? (P.coordenadaY - A.coordenadaY) / (B.coordenadaY - A.coordenadaY) : double.NaN;
            double tZ = (B.coordenadaZ - A.coordenadaZ) != 0 ? (P.coordenadaZ - A.coordenadaZ) / (B.coordenadaZ - A.coordenadaZ) : double.NaN;

            // Determinar el valor válido de t (descartamos NaN)
            double t = !double.IsNaN(tX) ? tX : (!double.IsNaN(tY) ? tY : tZ);

            // Si el punto está alineado en todas las dimensiones y t < 1, P está antes de B
            bool alineado = (double.IsNaN(tX) || Math.Abs(tX - t) < 1e-6) &&
                            (double.IsNaN(tY) || Math.Abs(tY - t) < 1e-6) &&
                            (double.IsNaN(tZ) || Math.Abs(tZ - t) < 1e-6);

            return alineado && t >= 0 && t < 1;
        }
        private double xe1_re(double q1, int n)
        {
            double fac = factorial(n);
            return Math.Pow(q1, n) / ((n + n + 1) * fac);
        }
        private double ye1_re(double q1, int n)
        {
            return Math.Pow(q1, n) / ((n + n + 1) * factorial(n));
        }
        private double factorial(double n)
        {
            if (n == 1)
                return 1;
            else
                return n * factorial(n - 1);
        }
        private double Get_R(Logica.Componente c)
        {

            Punto p = c.lista_puntos[1];
            Punto p_a = c.lista_puntos[1 - 1];
            Punto p_s = c.lista_puntos[1 + 1];
            p.a = Math.Pow(Math.Pow((p.p.X - p_a.p.X), 2) + Math.Pow((p.p.Y - p_a.p.Y), 2), 0.5);
            p.b = Math.Pow(Math.Pow((p_s.p.X - p.p.X), 2) + Math.Pow((p_s.p.Y - p.p.Y), 2), 0.5);
            p.c = Math.Pow(Math.Pow((p_s.p.X - p_a.p.X), 2) + Math.Pow((p_s.p.Y - p_a.p.Y), 2), 0.5);
            p.s = (p.a + p.b + p.c) / 2;
            p.R = (p.a * p.b * p.c) / (4 * Math.Pow(p.s * (p.s - p.a) * (p.s - p.b) * (p.s - p.c), 0.5));

            return p.R;
        }
        private Punto Rellenar_centro(Punto punto, double xc, double yc, int direccion)
        {
            Punto p, p_a;
            if (direccion == 1)
            {
                p = new Punto(new Point2d(punto.p.X, punto.p.Y));
                p_a = new Punto(new Point2d(xc, yc));
            }
            else
            {
                p_a = new Punto(new Point2d(punto.p.X, punto.p.Y));
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
        private Punto R_aux(Logica.Componente c1, Logica.Componente c2, int p)
        {
            Punto p_raux = new Punto();
            int direccion;
            double dis_raux;
            double xc = c1.lista_puntos[c1.lista_puntos.Count - 1].p.X;
            double yc = c1.lista_puntos[c1.lista_puntos.Count - 1].p.Y;
            if (p == 1)
            {
                if (c1.lista_puntos[c1.lista_puntos.Count - 2].p.X < c1.lista_puntos[c1.lista_puntos.Count - 1].p.X)
                {
                    direccion = 1;//izq a der
                }
                else
                {
                    direccion = 2;//der a izq
                }
                p_raux = Rellenar_centro(c2.lista_puntos[c2.lista_puntos.Count - 2], xc, yc, direccion);
                p_raux.dis_raux = Math.Pow(Math.Pow(xc - c2.lista_puntos[0].p.X, 2) + Math.Pow(yc - c2.lista_puntos[0].p.Y, 2), 0.5);
            }
            else
            {
                if (c1.lista_puntos[c1.lista_puntos.Count - 2].p.X < c1.lista_puntos[c1.lista_puntos.Count - 1].p.X)
                {
                    direccion = 1;//izq a der
                }
                else
                {
                    direccion = 2;//der a izq
                }
                p_raux = Rellenar_centro(c2.lista_puntos[0], xc, yc, direccion);
                p_raux.dis_raux = Math.Pow(Math.Pow(xc - c2.lista_puntos[0].p.X, 2) + Math.Pow(yc - c2.lista_puntos[0].p.Y, 2), 0.5);

            }

            double x0 = c1.lista_puntos[c1.lista_puntos.Count - 1].p.X;
            double y0 = c1.lista_puntos[c1.lista_puntos.Count - 1].p.Y;
            double x1 = c1.lista_puntos[c1.lista_puntos.Count - 2].p.X;
            double y1 = c1.lista_puntos[c1.lista_puntos.Count - 2].p.Y;
            double az_raux = p_raux.Az;
            return p_raux;
        }
        public EjeDeTrazado.componentes.Clotoide Recta_Curva(Logica.Componente c1, Logica.Componente c2, int p)//p es si es recta curva o es una curva recta --1 primero recta 2 primero curva
        {
            bool clot = true;
            double azte = c2.azte;
            double azts = c2.azts;
            double xc, yc, az_r = 0, az_raux = 0, angulo_d, dis_raux = 0, xe_i, ye_i;
            List<double> qe, qe_p, rc, le, ye, a, ir, xe, xm, te, ee;
            double r = c2.radio;
            bool accion;
            bool salir = false;
            int contar = 1;
            double az_tf;
            double A;

            az_r = c1.azr;
            Punto p_aux = R_aux(c1, c2, p);
            az_raux = p_aux.Az;
            dis_raux = p_aux.dis_raux;

            angulo_d = Math.Abs(az_r - az_raux);
            angulo_d = angulo_d * Math.PI / 180;
            xe_i = Math.Abs(dis_raux * Math.Cos(angulo_d));
            ye_i = Math.Abs(dis_raux * Math.Sin(angulo_d));
            qe = new List<double>();

            double aztemp_e, aztemp_s, aztemp_r;
            if (c2.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                aztemp_e = azte - 90;
                aztemp_s = azts - 90;
                aztemp_r = az_r - 90;
                if (aztemp_e < 0)
                {
                    aztemp_e += 360;
                }
                if (aztemp_s < 0)
                {
                    aztemp_s += 360;
                }
                if (aztemp_r < 0)
                {
                    aztemp_r += 360;
                }
                if (aztemp_r < aztemp_e && aztemp_r < 90 && aztemp_e > 270)
                {
                    aztemp_r += 360;
                }
                if (aztemp_r > 270 && aztemp_e < 90)
                {
                    qe.Add(Math.Abs(360 - aztemp_r + aztemp_e));
                }
                else if (aztemp_e > 270 && aztemp_r < 90)
                {
                    qe.Add(Math.Abs(aztemp_r + (360 - aztemp_e)));
                }
                else
                {
                    qe.Add(Math.Abs(aztemp_e - aztemp_r));
                }

            }
            else
            {
                aztemp_e = azte + 90;
                aztemp_s = azts + 90;
                aztemp_r = az_r + 90;
                if (aztemp_e > 360)
                {
                    aztemp_e -= 360;
                }
                if (aztemp_s > 360)
                {
                    aztemp_s -= 360;
                }
                if (aztemp_r > 360)
                {
                    aztemp_r -= 360;
                }

                if (aztemp_r > 270 && aztemp_e < 90)
                {
                    qe.Add(Math.Abs(aztemp_e + (360 - aztemp_r)));
                }
                else if (aztemp_e > 270 && aztemp_r < 90)
                {
                    qe.Add(Math.Abs(aztemp_r + (360 - aztemp_e)));
                }
                else
                {
                    qe.Add(Math.Abs(aztemp_e - aztemp_r));
                }

            }
            qe_p = new List<double>();
            qe_p.Add(qe[0] * Math.PI / 180);
            le = new List<double>();
            le.Add(2 * r * qe_p[0]);
            ye = new List<double>();
            ye.Add(((qe_p[0] / 3) - (Math.Pow(qe_p[0], 3) / 42) + (Math.Pow(qe_p[0], 5) / 1320) - (Math.Pow(qe_p[0], 7) / 75600)
                + ye1_re(qe_p[0], 9) - ye1_re(qe_p[0], 11) + ye1_re(qe_p[0], 13) - ye1_re(qe_p[0], 15) + ye1_re(qe_p[0], 17) - ye1_re(qe_p[0], 19) + ye1_re(qe_p[0], 21) - ye1_re(qe_p[0], 23)) * le[0]);
            a = new List<double>();
            a.Add(Math.Pow(le[0] * r, 0.5));
            ir = new List<double>();
            ir.Add(ye[0] - r * (1 - Math.Cos(qe_p[0])));
            xe = new List<double>();
            xe.Add((1 - Math.Pow(qe_p[0], 2) / 10 + Math.Pow(qe_p[0], 4) / 216 - Math.Pow(qe_p[0], 6) / 9360 + Math.Pow(qe_p[0], 8) / 685440
                 - xe1_re(qe_p[0], 10) + xe1_re(qe_p[0], 12) - xe1_re(qe_p[0], 14) + xe1_re(qe_p[0], 16) - xe1_re(qe_p[0], 18) + xe1_re(qe_p[0], 20) - xe1_re(qe_p[0], 22)) * le[0]);
            xm = new List<double>();
            xm.Add(xe[0] - r * Math.Sin(qe_p[0]));
            te = new List<double>();
            ee = new List<double>();
            if (ye[0] < ye_i)
            {
                accion = false;//aumentar
            }
            else
            {
                accion = true;//reducir
            }
            int contador = 0;
            while (!salir)
            {
                contador++;
                if (contador > 10001)
                {
                    break;
                }
                if (!accion)
                {
                    if (ye[contar - 1] < ye_i)
                    {
                        qe.Add(qe[contar - 1] + 0.01);
                    }
                    else
                    {
                        qe.Add(qe[contar - 1]);
                        break;
                    }

                }
                else
                {
                    if (ye[contar - 1] > ye_i)
                    {
                        qe.Add(qe[contar - 1] - 0.01);
                    }
                    else
                    {
                        qe.Add(qe[contar - 1]);
                        break;
                    }
                }
                qe_p.Add(qe[contar] * Math.PI / 180);
                le.Add(2 * r * qe_p[contar]);
                ye.Add(((qe_p[contar] / 3) - (Math.Pow(qe_p[contar], 3) / 42) + (Math.Pow(qe_p[contar], 5) / 1320) - (Math.Pow(qe_p[contar], 7) / 75600)
                    + ye1_re(qe_p[contar], 9) - ye1_re(qe_p[contar], 11) + ye1_re(qe_p[contar], 13) - ye1_re(qe_p[contar], 15) + ye1_re(qe_p[contar], 17) - ye1_re(qe_p[contar], 19) + ye1_re(qe_p[contar], 21) - ye1_re(qe_p[contar], 23)) * le[contar]);
                a.Add(Math.Pow(le[contar] * r, 0.5));
                ir.Add(ye[contar] - r * (1 - Math.Cos(qe_p[contar])));
                xe.Add((1 - Math.Pow(qe_p[contar], 2) / 10 + Math.Pow(qe_p[contar], 4) / 216 - Math.Pow(qe_p[contar], 6) / 9360 + Math.Pow(qe_p[contar], 8) / 685440
                    - xe1_re(qe_p[contar], 10) + xe1_re(qe_p[contar], 12) - xe1_re(qe_p[contar], 14) + xe1_re(qe_p[contar], 16) - xe1_re(qe_p[contar], 18) + xe1_re(qe_p[contar], 20) - xe1_re(qe_p[contar], 22)) * le[contar]);
                xm.Add(xe[contar] - r * Math.Sin(qe_p[contar]));
                te.Add(xm[contar - 1] + (r + ir[contar - 1]) * Math.Tan(qe_p[contar] / 2));
                ee.Add((r + ir[contar - 1]) / (Math.Cos(qe_p[contar] / 2)) - r);
                contar++;
            }
            if (c2.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario)
            {
                if (az_r > azts)
                {
                    az_tf = az_r - qe[qe.Count - 1];
                }
                else
                {
                    az_tf = az_r + qe[qe.Count - 1];
                }
            }
            else
            {
                if (azts > az_r)
                {
                    az_tf = az_r - qe[qe.Count - 1];
                }
                else
                {
                    az_tf = az_r + qe[qe.Count - 1];
                }
            }
            A = a[a.Count - 1];
            Punto3d[] puntosSing = new Punto3d[4];

            //puntosSing = addCurvaNoPaso(r, az_r,77.17,pp, p2, sentidoCurva.Antihorario, salir,A, le[le.Count - 1]);
            //double teta = az_tf + 90 - qe[qe.Count - 1];


            double x0 = c1.lista_puntos[c1.lista_puntos.Count - 1].p.X;
            double y0 = c1.lista_puntos[c1.lista_puntos.Count - 1].p.Y;
            double x1 = c1.lista_puntos[c1.lista_puntos.Count - 2].p.X;
            double y1 = c1.lista_puntos[c1.lista_puntos.Count - 2].p.Y;
            double xcentro = c2.xc;
            double ycentro = c2.yc;


            double rx, ry;
            ry = (y0 - y1) / (x0 - x1);
            rx = -x1 * ((y0 - y1) / (x0 - x1)) + y1;
            double distancia = Math.Abs(ry * xcentro - ycentro + rx) / Math.Pow(Math.Pow(ry, 2) + 1, 0.5);
            double valor = 1;
            bool ajustada = false;
            double qe_def = 0, qe_p_def = 0, le_def = 0, ye_def = 0, a_def = 0, ir_def = 0, xe_def = 0, xm_def = 0, te_def = 0, ee_def = 0;
            double qe_temp = 0, qe_p_temp = 0, le_temp = 0, ye_temp = 0, a_temp = 0, ir_temp = 0, xe_temp = 0, xm_temp = 0, te_temp = 0, ee_temp = 0;
            qe_def = qe[qe.Count - 1];
            bool subir = false;
            bool bajar = false;
            if ((ir[ir.Count - 1] + r) < distancia)
            {
                valor = valor * -1;
                if (Math.Truncate((ir[ir.Count - 1] + r) * 1000) < Math.Truncate(distancia) * 1000)
                {
                    valor = valor / 10;
                }
                subir = true;
            }
            else
            {
                if (Math.Truncate((ir[ir.Count - 1] + r) * 1000) > Math.Truncate(distancia) * 1000)
                {
                    valor = valor / 10;
                }
                bajar = true;
            }
            if (subir && qe_def > 358)
            {
                valor = valor / 10;
            }
            if (bajar && qe_def < 2)
            {
                valor = valor / 10;
            }
            contador = 0;
            while (!ajustada && contador < 10000)
            {
                contador++;
                qe_temp = qe_def;
                qe_p_temp = qe_p_def;
                le_temp = le_def;
                ye_temp = ye_def;
                a_temp = a_def;
                ir_temp = ir_def;
                xe_temp = xe_def;
                xm_temp = xm_def;
                te_temp = te_def;
                ee_temp = ee_def;

                qe_def = qe_def - valor;
                qe_p_def = (qe_def * Math.PI / 180);
                le_def = (2 * r * qe_p_def);
                ye_def = (((qe_p_def / 3) - (Math.Pow(qe_p_def, 3) / 42) + (Math.Pow(qe_p_def, 5) / 1320) - (Math.Pow(qe_p_def, 7) / 75600) + (Math.Pow(qe_p_def, 9) / 6894720)) * le_def);
                a_def = (Math.Pow(le_def * r, 0.5));
                ir_def = (ye_def - r * (1 - Math.Cos(qe_p_def)));
                xe_def = ((1 - Math.Pow(qe_p_def, 2) / 10 + Math.Pow(qe_p_def, 4) / 216 - Math.Pow(qe_p_def, 6) / 9360 + Math.Pow(qe_p_def, 8) / 685440 - Math.Pow(qe_p_def, 10) / 76204800) * le_def);
                xm_def = (xe_def - r * Math.Sin(qe_p_def));
                te_def = (xm_def + (r + ir_def) * Math.Tan(qe_p_def / 2));
                ee_def = ((r + ir_def) / (Math.Cos(qe_p_def / 2)) - r);

                if (Math.Truncate((ir_def + r) * 1000000) == Math.Truncate(distancia * 1000000))
                {
                    ajustada = true;
                }
                else
                {

                    if (bajar)
                    {
                        if ((ir_def + r) < distancia)
                        {
                            qe_def = qe_temp;
                            qe_p_def = qe_p_temp;
                            le_def = le_temp;
                            ye_def = ye_temp;
                            a_def = a_temp;
                            ir_def = ir_temp;
                            xe_def = xe_temp;
                            xm_def = xm_temp;
                            te_def = te_temp;
                            ee_def = ee_temp;
                            valor /= 10;
                        }
                    }
                    if (subir)
                    {
                        if ((ir_def + r) > distancia)
                        {
                            qe_def = qe_temp;
                            qe_p_def = qe_p_temp;
                            le_def = le_temp;
                            ye_def = ye_temp;
                            a_def = a_temp;
                            ir_def = ir_temp;
                            xe_def = xe_temp;
                            xm_def = xm_temp;
                            te_def = te_temp;
                            ee_def = ee_temp;
                            valor /= 10;
                        }
                    }
                    if (qe_def - valor < 0 && qe_def > 0)
                    {
                        valor /= 10;
                    }
                }

            }
            if (c2.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario)
            {
                az_tf = az_r - qe_def;
            }
            else
            {
                az_tf = az_r + qe_def;
            }


            double xm0 = 0, xm1 = 0, ym0 = 0, ym1 = 0, xx = 0, yy = 0;
            if (c2.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                double teta = az_tf - 90;
                if (teta < 0)
                {
                    teta = teta + 360;
                }
                if (c2.lista_puntos[c2.lista_puntos.Count - 2].p.X < c2.lista_puntos[c2.lista_puntos.Count - 1].p.X)
                {
                    double seno = Math.Sin(teta * Math.PI / 180 - qe_p_def);
                    xm1 = xcentro + (r + ir_def) * seno;

                    double coseno = Math.Cos(teta * Math.PI / 180 - qe_p_def);
                    ym1 = ycentro + (r + ir_def) * coseno;
                    xm0 = xm1 + xm_def * Math.Sin((teta - 90 - qe_def) * Math.PI / 180);
                    ym0 = ym1 + xm_def * Math.Cos((teta - 90 - qe_def) * Math.PI / 180);
                    //az_tf = az_tf + qe_def * 2;
                }
                else
                {
                    double seno = Math.Sin(teta * Math.PI / 180 - qe_p_def);
                    xm1 = xcentro + (r + ir_def) * seno;

                    double coseno = Math.Cos(teta * Math.PI / 180 - qe_p_def);
                    ym1 = ycentro + (r + ir_def) * coseno;
                    xm0 = xm1 + xm_def * Math.Sin((teta - 90 - qe_def) * Math.PI / 180);
                    ym0 = ym1 + xm_def * Math.Cos((teta - 90 - qe_def) * Math.PI / 180);
                }


                xx = xcentro - (r) * Math.Sin((az_tf + 90) * Math.PI / 180);
                yy = ycentro - (r) * Math.Cos((az_tf + 90) * Math.PI / 180);


            }
            else
            {
                double teta = az_tf + 90;
                double seno = Math.Sin(teta * Math.PI / 180 + qe_p_def);
                xm1 = xcentro + (r + ir_def) * seno;

                double coseno = Math.Cos(teta * Math.PI / 180 + qe_p_def);
                ym1 = ycentro + (r + ir_def) * coseno;

                xm0 = xm1 + xm_def * Math.Sin((teta + 90 + qe_def) * Math.PI / 180);
                ym0 = ym1 + xm_def * Math.Cos((teta + 90 + qe_def) * Math.PI / 180);


                xx = xcentro + (r) * Math.Sin((az_tf + 90) * Math.PI / 180);
                yy = ycentro + (r) * Math.Cos((az_tf + 90) * Math.PI / 180);
            }



            tadLayShare.puntos.Punto3d p3 = new tadLayShare.puntos.Punto3d(xm0, ym0, 0);
            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(xx, yy, 0);

            A = a_def;
            if (!double.IsNaN(A))
            {

            }
            EjeDeTrazado.componentes.Clotoide Clo = new EjeDeTrazado.componentes.Clotoide(p3, p2, r, 0, c2.direccion, 2, 2, true, EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.entrada, az_r, true, 0, false, le_def, a_def);


            return Clo;
        }
        public EjeDeTrazado.componentes.Clotoide Curva_Recta(Logica.Componente c1, Logica.Componente c2, int p)//p es si es recta curva o es una curva recta --1 primero recta 2 primero curva
        {
            bool clot = true;
            double xc, yc, az_r = 0, az_raux = 0, angulo_d, dis_raux = 0, xe_i, ye_i;
            List<double> qe, qe_p, rc, le, ye, a, ir, xe, xm, te, ee;
            bool accion;
            bool salir = false;
            int contar = 1;
            double az_tf;
            double A;

            double azts = c1.azts;
            double azte = c1.azte;
            double r = Get_R(c1);
            double xcentro = c1.xc;
            double ycentro = c1.yc;


            az_r = c2.azr;
            Punto p_aux = R_aux(c1, c2, p);
            az_raux = p_aux.Az;
            dis_raux = p_aux.dis_raux;

            angulo_d = Math.Abs(az_r - az_raux);
            angulo_d = angulo_d * Math.PI / 180;
            xe_i = Math.Abs(dis_raux * Math.Cos(angulo_d));
            ye_i = Math.Abs(dis_raux * Math.Sin(angulo_d));
            qe = new List<double>();

            double aztemp_e, aztemp_s, aztemp_r;
            if (c1.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                aztemp_e = azte - 90;
                aztemp_s = azts - 90;
                aztemp_r = az_r - 90;
                if (aztemp_e < 0)
                {
                    aztemp_e += 360;
                }
                if (aztemp_s < 0)
                {
                    aztemp_s += 360;
                }
                if (aztemp_r < 0)
                {
                    aztemp_r += 360;
                }
                if (aztemp_r < aztemp_s && aztemp_r < 90 && aztemp_s > 270)
                {
                    aztemp_r += 360;
                }
                if (aztemp_r > 270 && aztemp_s < 90)
                {
                    qe.Add(Math.Abs(360 - aztemp_r + aztemp_s));
                }
                else if (aztemp_s > 270 && aztemp_r < 90)
                {
                    qe.Add(Math.Abs(aztemp_r + (360 - aztemp_s)));
                }
                else
                {
                    qe.Add(Math.Abs(aztemp_r - aztemp_s));
                }
            }
            else
            {
                aztemp_e = azte + 90;
                aztemp_s = azts + 90;
                aztemp_r = az_r + 90;
                if (aztemp_e > 360)
                {
                    aztemp_e -= 360;
                }
                if (aztemp_s > 360)
                {
                    aztemp_s -= 360;
                }
                if (aztemp_r > 360)
                {
                    aztemp_r -= 360;
                }
                if (aztemp_r > aztemp_s && aztemp_r > 270 && aztemp_s < 90)
                {
                    aztemp_r -= 360;
                }
                if (aztemp_r > 270 && aztemp_s < 90)
                {
                    qe.Add(Math.Abs(aztemp_s + (360 - aztemp_r)));
                }
                else if (aztemp_s > 270 && aztemp_r < 90)
                {
                    qe.Add(Math.Abs(aztemp_r + (360 - aztemp_s)));
                }
                else
                {
                    qe.Add(Math.Abs(aztemp_s - aztemp_r));
                }

            }
            qe_p = new List<double>();
            qe_p.Add(qe[0] * Math.PI / 180);
            le = new List<double>();
            le.Add(2 * r * qe_p[0]);
            ye = new List<double>();
            ye.Add(((qe_p[0] / 3) - (Math.Pow(qe_p[0], 3) / 42) + (Math.Pow(qe_p[0], 5) / 1320) - (Math.Pow(qe_p[0], 7) / 75600)
                + ye1_re(qe_p[0], 9) - ye1_re(qe_p[0], 11) + ye1_re(qe_p[0], 13) - ye1_re(qe_p[0], 15) + ye1_re(qe_p[0], 17) - ye1_re(qe_p[0], 19) + ye1_re(qe_p[0], 21) - ye1_re(qe_p[0], 23)) * le[0]);
            a = new List<double>();
            a.Add(Math.Pow(le[0] * r, 0.5));
            ir = new List<double>();
            ir.Add(ye[0] - r * (1 - Math.Cos(qe_p[0])));
            xe = new List<double>();
            xe.Add((1 - Math.Pow(qe_p[0], 2) / 10 + Math.Pow(qe_p[0], 4) / 216 - Math.Pow(qe_p[0], 6) / 9360 + Math.Pow(qe_p[0], 8) / 685440
                 - xe1_re(qe_p[0], 10) + xe1_re(qe_p[0], 12) - xe1_re(qe_p[0], 14) + xe1_re(qe_p[0], 16) - xe1_re(qe_p[0], 18) + xe1_re(qe_p[0], 20) - xe1_re(qe_p[0], 22)) * le[0]);
            xm = new List<double>();
            xm.Add(xe[0] - r * Math.Sin(qe_p[0]));
            te = new List<double>();
            ee = new List<double>();
            if (ye[0] < ye_i)
            {
                accion = false;//aumentar
            }
            else
            {
                accion = true;//reducir
            }

            /////////////////
            while (!salir && contar < 1000)
            {
                if (!accion)
                {
                    if (ye[contar - 1] < ye_i)
                    {
                        qe.Add(qe[contar - 1] + 0.1);
                    }
                    else
                    {
                        qe.Add(qe[contar - 1]);
                        break;
                    }

                }
                else
                {
                    if (ye[contar - 1] > ye_i)
                    {
                        qe.Add(qe[contar - 1] - 0.1);
                    }
                    else
                    {
                        qe.Add(qe[contar - 1]);
                        break;
                    }
                }
                qe_p.Add(qe[contar] * Math.PI / 180);
                le.Add(2 * r * qe_p[contar]);
                ye.Add(((qe_p[contar] / 3) - (Math.Pow(qe_p[contar], 3) / 42) + (Math.Pow(qe_p[contar], 5) / 1320) - (Math.Pow(qe_p[contar], 7) / 75600)
                     + ye1_re(qe_p[contar], 9) - ye1_re(qe_p[contar], 11) + ye1_re(qe_p[contar], 13) - ye1_re(qe_p[contar], 15) + ye1_re(qe_p[contar], 17) - ye1_re(qe_p[contar], 19) + ye1_re(qe_p[contar], 21) - ye1_re(qe_p[contar], 23)) * le[contar]);
                a.Add(Math.Pow(le[contar] * r, 0.5));
                ir.Add(ye[contar] - r * (1 - Math.Cos(qe_p[contar])));
                xe.Add((1 - Math.Pow(qe_p[contar], 2) / 10 + Math.Pow(qe_p[contar], 4) / 216 - Math.Pow(qe_p[contar], 6) / 9360 + Math.Pow(qe_p[contar], 8) / 685440
                     - xe1_re(qe_p[contar], 10) + xe1_re(qe_p[contar], 12) - xe1_re(qe_p[contar], 14) + xe1_re(qe_p[contar], 16) - xe1_re(qe_p[contar], 18) + xe1_re(qe_p[contar], 20) - xe1_re(qe_p[contar], 22)) * le[contar]);
                xm.Add(xe[contar] - r * Math.Sin(qe_p[contar]));
                te.Add(xm[contar - 1] + (r + ir[contar - 1]) * Math.Tan(qe_p[contar] / 2));
                ee.Add((r + ir[contar - 1]) / (Math.Cos(qe_p[contar] / 2)) - r);
                contar++;
            }
            if (c1.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario)
            {
                if (azts > az_r)
                {
                    az_tf = az_r + qe[qe.Count - 1];
                }
                else
                {
                    az_tf = az_r - qe[qe.Count - 1];
                }
            }
            else
            {
                if (az_r > azts)
                {
                    az_tf = az_r - qe[qe.Count - 1];
                }
                else
                {
                    az_tf = az_r + qe[qe.Count - 1];
                }
            }
            A = a[a.Count - 1];
            Punto3d[] puntosSing = new Punto3d[4];

            double x0 = c2.lista_puntos[c2.lista_puntos.Count - 1].p.X;
            double y0 = c2.lista_puntos[c2.lista_puntos.Count - 1].p.Y;
            double x1 = c2.lista_puntos[c2.lista_puntos.Count - 2].p.X;
            double y1 = c2.lista_puntos[c2.lista_puntos.Count - 2].p.Y;

            double rx, ry;
            ry = (y0 - y1) / (x0 - x1);
            rx = -x1 * ((y0 - y1) / (x0 - x1)) + y1;
            double distancia = Math.Abs(ry * xcentro - ycentro + rx) / Math.Pow(Math.Pow(ry, 2) + 1, 0.5);
            double valor = 1;
            bool ajustada = false;
            double qe_def = 0, qe_p_def = 0, le_def = 0, ye_def = 0, a_def = 0, ir_def = 0, xe_def = 0, xm_def = 0, te_def = 0, ee_def = 0;
            double qe_temp = 0, qe_p_temp = 0, le_temp = 0, ye_temp = 0, a_temp = 0, ir_temp = 0, xe_temp = 0, xm_temp = 0, te_temp = 0, ee_temp = 0;
            qe_def = qe[qe.Count - 1];
            bool subir = false;
            bool bajar = false;
            if ((ir[ir.Count - 1] + r) < distancia)
            {
                valor = valor * -1;
                subir = true;
            }
            else
            {
                bajar = true;
            }
            int contador = 0;
            if (subir && qe_def > 358)
            {
                valor = valor / 10;
            }
            if (bajar && qe_def < 2)
            {
                valor = valor / 10;
            }
            while (!ajustada && contador < 10000)
            {
                contador++;
                qe_temp = qe_def;
                qe_p_temp = qe_p_def;
                le_temp = le_def;
                ye_temp = ye_def;
                a_temp = a_def;
                ir_temp = ir_def;
                xe_temp = xe_def;
                xm_temp = xm_def;
                te_temp = te_def;
                ee_temp = ee_def;

                qe_def = qe_def - valor;

                qe_p_def = (qe_def * Math.PI / 180);
                le_def = (2 * r * qe_p_def);
                ye_def = (((qe_p_def / 3) - (Math.Pow(qe_p_def, 3) / 42) + (Math.Pow(qe_p_def, 5) / 1320) - (Math.Pow(qe_p_def, 7) / 75600) + (Math.Pow(qe_p_def, 9) / 6894720)) * le_def);
                a_def = (Math.Pow(le_def * r, 0.5));
                ir_def = (ye_def - r * (1 - Math.Cos(qe_p_def)));
                xe_def = ((1 - Math.Pow(qe_p_def, 2) / 10 + Math.Pow(qe_p_def, 4) / 216 - Math.Pow(qe_p_def, 6) / 9360 + Math.Pow(qe_p_def, 8) / 685440 - Math.Pow(qe_p_def, 10) / 76204800) * le_def);
                xm_def = (xe_def - r * Math.Sin(qe_p_def));
                te_def = (xm_def + (r + ir_def) * Math.Tan(qe_p_def / 2));
                ee_def = ((r + ir_def) / (Math.Cos(qe_p_def / 2)) - r);

                if (Math.Truncate((ir_def + r) * 100000000) == Math.Truncate(distancia * 100000000))
                {
                    ajustada = true;
                }
                else
                {
                    if (bajar)
                    {
                        if ((ir_def + r) < distancia)
                        {
                            qe_def = qe_temp;
                            qe_p_def = qe_p_temp;
                            le_def = le_temp;
                            ye_def = ye_temp;
                            a_def = a_temp;
                            ir_def = ir_temp;
                            xe_def = xe_temp;
                            xm_def = xm_temp;
                            te_def = te_temp;
                            ee_def = ee_temp;
                            valor /= 10;
                        }
                    }
                    if (subir)
                    {
                        if ((ir_def + r) > distancia)
                        {
                            qe_def = qe_temp;
                            qe_p_def = qe_p_temp;
                            le_def = le_temp;
                            ye_def = ye_temp;
                            a_def = a_temp;
                            ir_def = ir_temp;
                            xe_def = xe_temp;
                            xm_def = xm_temp;
                            te_def = te_temp;
                            ee_def = ee_temp;
                            valor /= 10;
                        }
                    }
                    if (qe_def - valor < 0)
                    {
                        valor /= 10;
                    }
                }

            }
            if (c1.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario)
            {
                az_tf = az_r + qe_def;
            }
            else
            {
                az_tf = az_r - qe_def;
            }
            if (az_tf < 0)
            {
                az_tf += 360;
            }
            if (az_tf > 360)
            {
                az_tf -= 360;
            }

            double xm0 = 0, xm1 = 0, ym0 = 0, ym1 = 0, xx = 0, yy = 0;

            if (c1.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario)
            {
                double teta = az_tf + 90;
                if (teta > 360)
                {
                    teta = teta - 360;
                }
                double seno, coseno;
                if (azts > azte)
                {
                    seno = Math.Sin(teta * Math.PI / 180 - qe_p_def);
                    xm1 = xcentro + (r + ir_def) * seno;

                    coseno = Math.Cos(teta * Math.PI / 180 - qe_p_def);
                    ym1 = ycentro + (r + ir_def) * coseno;
                    xm0 = xm1 - xm_def * Math.Sin((teta + 90 - qe_def) * Math.PI / 180);
                    ym0 = ym1 - xm_def * Math.Cos((teta + 90 - qe_def) * Math.PI / 180);
                }
                else
                {
                    seno = Math.Sin(teta * Math.PI / 180 - qe_p_def);
                    xm1 = xcentro + (r + ir_def) * seno;

                    coseno = Math.Cos(teta * Math.PI / 180 - qe_p_def);
                    ym1 = ycentro + (r + ir_def) * coseno;
                    xm0 = xm1 - xm_def * Math.Sin((teta + 90 - qe_def) * Math.PI / 180);//cambiado
                    ym0 = ym1 - xm_def * Math.Cos((teta + 90 - qe_def) * Math.PI / 180);//cambiado
                }



                xx = c1.xc + (r) * Math.Sin((teta) * Math.PI / 180);
                yy = c1.yc + (r) * Math.Cos((teta) * Math.PI / 180);
            }
            else
            {
                double teta = az_tf - 90;
                if (teta < 0)
                {
                    teta = teta + 360;
                }
                double seno = Math.Sin(teta * Math.PI / 180 + qe_p_def);
                xm1 = xcentro + (r + ir_def) * seno;

                double coseno = Math.Cos(teta * Math.PI / 180 + qe_p_def);
                ym1 = ycentro + (r + ir_def) * coseno;
                if (az_tf < 180)
                {
                    xm0 = xm1 - xm_def * Math.Sin((teta - 90 + qe_def) * Math.PI / 180);
                    ym0 = ym1 - xm_def * Math.Cos((teta - 90 + qe_def) * Math.PI / 180);
                }
                else
                {
                    xm0 = xm1 - xm_def * Math.Sin((teta - 90 + qe_def) * Math.PI / 180);
                    ym0 = ym1 - xm_def * Math.Cos((teta - 90 + qe_def) * Math.PI / 180);
                }

                xx = c1.xc + (r) * Math.Sin((az_tf - 90) * Math.PI / 180);
                yy = c1.yc + (r) * Math.Cos((az_tf - 90) * Math.PI / 180);
            }


            tadLayShare.puntos.Punto3d p3 = new tadLayShare.puntos.Punto3d(xm0, ym0, 0);
            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(xx, yy, 0);
            EjeDeTrazado.componentes.Clotoide Clo = null;

            if (contador == 10000)
            {

            }
            else
            {
                A = a_def;
                Clo = new EjeDeTrazado.componentes.Clotoide(p2, p3, r, 0, c1.direccion, 2, 2, false, EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.salida, az_r, false, 0, false, le_def, A);
            }


            return Clo;
        }
        private EjeDeTrazado.componentes.Clotoide Curva_Curva_M(Logica.Componente c1, Logica.Componente c2)
        {
            if (c1.radio > c2.radio)
            {
                //caso en el que una curva esta dentro de otra
                if ((c2.radio + Distancia(new Point2d(c1.xc, c1.yc), new Point2d(c2.xc, c2.yc))) > c1.radio)
                {
                    EjeDeTrazado.componentes.Clotoide C = null;
                    return C;
                }
            }
            else
            {
                //caso en el que una curva esta dentro de otra
                if ((c1.radio + Distancia(new Point2d(c1.xc, c1.yc), new Point2d(c2.xc, c2.yc))) > c2.radio)
                {
                    EjeDeTrazado.componentes.Clotoide C = null;
                    return C;
                }
            }
            Logica.Componente c3 = new Logica.Componente();
            bool cambio = false;
            if (c2.radio > c1.radio)
            {
                cambio = true;
                c3 = c1;
                c1 = c2;
                c2 = c3;

                if (c1.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    c1.direccion = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario;
                    c2.direccion = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    c1.direccion = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;
                    c2.direccion = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;
                }
            }
            double azt2 = c2.azte;
            double azt1 = 0;
            double azte = c1.azte;
            double azts = c1.azts;
            double azte2 = c2.azte;
            double azts2 = c2.azts;
            double r = c1.radio;
            double r2 = c2.radio;
            double xcentro = c1.xc;
            double ycentro = c1.yc;
            double xcentro2 = c2.xc;
            double ycentro2 = c2.yc;
            double az_r;

            double le1 = 0;
            double le2 = 0;

            double q1 = 0;
            double q2 = 0;

            double q1_p = 0;
            double q2_p = 0;

            double xe1 = 0;
            double xm1 = 0;
            double ye1 = 0;

            double xe2 = 0;
            double xm2 = 0;
            double ye2 = 0;
            double xm_1 = 0, ym_1 = 0, xm_0 = 0, ym_0 = 0;
            double d_az = 0;
            //d_az = 19.3587;
            double d_az_p = 0;
            double a1, a2;
            double dr1, dr1r1, dr2, dr2r2;
            dr1 = 0;
            dr1r1 = 0;
            dr2 = 0; dr2r2 = 0;
            a1 = 0;
            a2 = 0;
            double dis_c_clo, dis_c;
            bool salir = true;
            bool aumentar = true;
            double valor = 1;
            d_az += 0.1;
            d_az_p = d_az * Math.PI / 180;
            le1 = (d_az_p) / (r / (2 * r2 * r2) - 1 / (2 * r));
            le2 = r * le1 / r2;

            a1 = Math.Sqrt(r * le1);
            a2 = Math.Sqrt(r2 * le2);

            q1 = le1 / (2 * r);
            q2 = q1 + d_az_p;

            q1_p = q1 * 180 / Math.PI;
            q2_p = q2 * 180 / Math.PI;

            xe1 = (1 - Math.Pow(q1, 2) / 10 + Math.Pow(q1, 4) / 216 - Math.Pow(q1, 6) / 9360 + Math.Pow(q1, 8) / 685440
                - xe1_re(q1, 10) + xe1_re(q1, 12) - xe1_re(q1, 14) + xe1_re(q1, 16) - xe1_re(q1, 18) + xe1_re(q1, 20) - xe1_re(q1, 22)) * le1;
            xe2 = (1 - Math.Pow(q2, 2) / 10 + Math.Pow(q2, 4) / 216 - Math.Pow(q2, 6) / 9360 + Math.Pow(q2, 8) / 685440
                - xe1_re(q2, 10) + xe1_re(q2, 12) - xe1_re(q2, 14) + xe1_re(q2, 16) - xe1_re(q2, 18) + xe1_re(q2, 20) - xe1_re(q2, 22)) * le2;

            ye1 = ((q1 / 3) - (Math.Pow(q1, 3) / 42) + (Math.Pow(q1, 5) / 1320) - (Math.Pow(q1, 7) / 75600)
                + ye1_re(q1, 9) - ye1_re(q1, 11) + ye1_re(q1, 13) - ye1_re(q1, 15) + ye1_re(q1, 17) - ye1_re(q1, 19) + ye1_re(q1, 21) - ye1_re(q1, 23)) * le1;
            ye2 = ((q2 / 3) - (Math.Pow(q2, 3) / 42) + (Math.Pow(q2, 5) / 1320) - (Math.Pow(q2, 7) / 75600)
                + ye1_re(q2, 9) - ye1_re(q2, 11) + ye1_re(q2, 13) - ye1_re(q2, 15) + ye1_re(q2, 17) - ye1_re(q2, 19) + ye1_re(q2, 21) - ye1_re(q2, 23)) * le2;

            xm1 = xe1 - r * Math.Sin(q1);
            xm2 = xe2 - r2 * Math.Sin(q2);


            dr1 = ye1 - r * (1 - Math.Cos(q1));
            dr1r1 = r + dr1;

            dr2 = ye2 - r2 * (1 - Math.Cos(q2));
            dr2r2 = r2 + dr2;
            dis_c_clo = Distancia(xm1, dr1r1, xm2, dr2r2);
            dis_c = Distancia(c1.xc, c1.yc, c2.xc, c2.yc);
            if (Math.Truncate(dis_c_clo * 100000) == Math.Truncate(dis_c * 100000))
            {
                salir = false;
            }
            else
            {
                if (dis_c_clo > dis_c)
                {
                    aumentar = true;
                }
                if (dis_c > dis_c_clo)
                {
                    aumentar = false;
                }
            }
            while (salir)
            {
                if (d_az > 200 || valor < 0.00000001)
                {
                    break;
                }
                d_az += valor;
                d_az_p = d_az * Math.PI / 180;
                le1 = (d_az_p) / (r / (2 * r2 * r2) - 1 / (2 * r));
                le2 = r * le1 / r2;

                a1 = Math.Sqrt(r * le1);
                a2 = Math.Sqrt(r2 * le2);

                q1 = le1 / (2 * r);
                q2 = q1 + d_az_p;

                q1_p = q1 * 180 / Math.PI;
                q2_p = q2 * 180 / Math.PI;

                xe1 = (1 - Math.Pow(q1, 2) / 10 + Math.Pow(q1, 4) / 216 - Math.Pow(q1, 6) / 9360 + Math.Pow(q1, 8) / 685440
                    - xe1_re(q1, 10) + xe1_re(q1, 12) - xe1_re(q1, 14) + xe1_re(q1, 16) - xe1_re(q1, 18) + xe1_re(q1, 20) - xe1_re(q1, 22)) * le1;
                xe2 = (1 - Math.Pow(q2, 2) / 10 + Math.Pow(q2, 4) / 216 - Math.Pow(q2, 6) / 9360 + Math.Pow(q2, 8) / 685440
                    - xe1_re(q2, 10) + xe1_re(q2, 12) - xe1_re(q2, 14) + xe1_re(q2, 16) - xe1_re(q2, 18) + xe1_re(q2, 20) - xe1_re(q2, 22)) * le2;

                ye1 = ((q1 / 3) - (Math.Pow(q1, 3) / 42) + (Math.Pow(q1, 5) / 1320) - (Math.Pow(q1, 7) / 75600)
                    + ye1_re(q1, 9) - ye1_re(q1, 11) + ye1_re(q1, 13) - ye1_re(q1, 15) + ye1_re(q1, 17) - ye1_re(q1, 19) + ye1_re(q1, 21) - ye1_re(q1, 23)) * le1;
                ye2 = ((q2 / 3) - (Math.Pow(q2, 3) / 42) + (Math.Pow(q2, 5) / 1320) - (Math.Pow(q2, 7) / 75600)
                    + ye1_re(q2, 9) - ye1_re(q2, 11) + ye1_re(q2, 13) - ye1_re(q2, 15) + ye1_re(q2, 17) - ye1_re(q2, 19) + ye1_re(q2, 21) - ye1_re(q2, 23)) * le2;

                xm1 = xe1 - r * Math.Sin(q1);
                xm2 = xe2 - r2 * Math.Sin(q2);


                dr1 = ye1 - r * (1 - Math.Cos(q1));
                dr1r1 = r + dr1;

                dr2 = ye2 - r2 * (1 - Math.Cos(q2));
                dr2r2 = r2 + dr2;
                dis_c_clo = Distancia(xm1, dr1r1, xm2, dr2r2);
                dis_c = Distancia(c1.xc, c1.yc, c2.xc, c2.yc);
                if (Math.Truncate(dis_c_clo * 100000) == Math.Truncate(dis_c * 100000))
                {
                    salir = false;
                }
                else
                {
                    if (aumentar)
                    {
                        if (dis_c > dis_c_clo)
                        {
                            d_az -= valor;
                            valor /= 10;
                        }
                    }
                    else
                    {
                        if (dis_c_clo > dis_c)
                        {
                            d_az -= valor;
                            valor /= 10;
                        }
                    }
                }
            }

            double x = xcentro;
            double y = ycentro;
            if (c1.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario)
            {
                dr1r1 = dr1r1;
                dr2r2 = dr2r2;
            }
            else
            {
                dr1r1 = -dr1r1;
                dr2r2 = -dr2r2;
                ye2 = -ye2;
                ye1 = -ye1;
            }
            double x2 = (xm2 - xm1) + xcentro;
            double y2 = (dr2r2 - dr1r1) + ycentro;
            double az1 = Rellenar_centro(xcentro, ycentro, xcentro2, ycentro2, 2).Az;
            double az2 = Rellenar_centro(x, y, x2, y2, 2).Az;
            tadLayShare.puntos.Punto3d p3 = new tadLayShare.puntos.Punto3d(0, 0, 0);
            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(xe2, ye2, 0);
            //los 2 centros de las curvas reales
            tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(xcentro, ycentro, 0);
            tadLayShare.puntos.Punto3d p11 = new tadLayShare.puntos.Punto3d(xcentro2, ycentro2, 0);

            double giro = az1 - az2;

            p3 = new tadLayShare.puntos.Punto3d(-xm1 + xcentro, -dr1r1 + ycentro, 0);
            double az3 = Rellenar_centro(x, y, p3.coordenadaX, p3.coordenadaY, 2).Az;
            Point2d p = Girar_Punto(-xm1 + xcentro, -dr1r1 + ycentro, xcentro, ycentro, giro + az3);
            p3 = new tadLayShare.puntos.Punto3d(p.X, p.Y, 0);

            p2 = new tadLayShare.puntos.Punto3d(xe2 - xm1 + xcentro, ye2 - dr1r1 + ycentro, 0);
            double az32 = Rellenar_centro(x, y, p2.coordenadaX, p2.coordenadaY, 2).Az;
            Point2d pp = Girar_Punto(xe2 - xm1 + xcentro, ye2 - dr1r1 + ycentro, xcentro, ycentro, giro + az32);
            p2 = new tadLayShare.puntos.Punto3d(pp.X, pp.Y, 0);

            tadLayShare.puntos.Punto3d p5 = new tadLayShare.puntos.Punto3d(xe1 - xm1 + xcentro, ye1 - dr1r1 + ycentro, 0);
            double az33 = Rellenar_centro(x, y, p5.coordenadaX, p5.coordenadaY, 2).Az;
            Point2d ppp = Girar_Punto(xe1 - xm1 + xcentro, ye1 - dr1r1 + ycentro, xcentro, ycentro, giro + az33);
            p5 = new tadLayShare.puntos.Punto3d(ppp.X, ppp.Y, 0);

            EjeDeTrazado.componentes.Clotoide Clo = null;
            if (cambio)
            {
                if (c1.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    giro -= 180;
                    c1.direccion = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario;
                    c2.direccion = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    giro += 180;
                    c1.direccion = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;
                    c2.direccion = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;
                }
                // Clo = new EjeDeTrazado.componentes.Clotoide(p2, p3, r2, 0, c1.direccion, 2, 2, false, EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.salida, 90 + giro, false, 0, false, le2, a2);
                // Dibujar_Clotoide(Clo);
                if (d_az > 190 || valor < 0.00000001)
                {
                    Clo = null;
                }
                else
                {
                    Clo = new EjeDeTrazado.componentes.Clotoide(p2, p3, r2, 0, c1.direccion, 2, 2, false, EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.salida, 90 + giro, false, 0, false, 0, a2, le2 - le1);

                }
                //Dibujar_Clotoide(Clo, 0, le2 - le1);
                c3 = c1;
                c1 = c2;
                c2 = c3;

            }
            else
            {
                if (d_az > 190 || valor < 0.00000001)
                {
                    Clo = null;
                }
                else
                {
                    Clo = new EjeDeTrazado.componentes.Clotoide(p3, p2, r2, 0, c1.direccion, 2, 2, false, EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.entrada, 90 + giro, false, 0, false, le2, a2, le1);
                    //Dibujar_Clotoide(Clo,le1);
                }
            }


            return Clo;
        }


        public double Distancia(Point2d a, Point2d b)
        {
            double d;
            d = Math.Pow(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2), 0.5);
            return d;
        }
        private double Distancia(double a, double b, double c, double d)
        {
            return Math.Pow(Math.Pow(c - a, 2) + Math.Pow(d - b, 2), 0.5);
        }
        private double Distancia_P_R(double y, double x, double xc, double yc)
        {
            return Math.Abs(y * xc - yc + x) / Math.Sqrt(y * y + 1);
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
        private Point2d Girar_Punto(double x, double y, double x2, double y2, double giro)
        {
            Point2d pm = new Point2d(x2, y2);
            double seno;
            double coseno;
            double x_p_2, y_p_2, x_p_1, y_p_1;

            double dis = Distancia(new Point2d(x2, y2), new Point2d(x, y));

            seno = Math.Sin((giro) * Math.PI / 180);
            coseno = Math.Cos((giro) * Math.PI / 180);


            x_p_2 = pm.X + dis * seno;
            y_p_2 = pm.Y + dis * coseno;



            return new Point2d(x_p_2, y_p_2);
        }
        public void Rellenar_Curva(Logica.Componente c1)
        {

            c1.lista_puntos = Puntos_iguales_listas(c1.lista_puntos);
            double xc = ((c1.lista_puntos[1 + 1].p.Y + c1.lista_puntos[1].p.Y) * 0.5 + ((c1.lista_puntos[1 + 1].p.X - c1.lista_puntos[1].p.X) * (c1.lista_puntos[1 + 1].p.X + c1.lista_puntos[1].p.X) * 0.5 / (c1.lista_puntos[1 + 1].p.Y - c1.lista_puntos[1].p.Y)) - (c1.lista_puntos[1].p.Y + c1.lista_puntos[1 - 1].p.Y) * 0.5 - ((c1.lista_puntos[1].p.X - c1.lista_puntos[1 - 1].p.X) * (c1.lista_puntos[1].p.X + c1.lista_puntos[1 - 1].p.X) * 0.5 / (c1.lista_puntos[1].p.Y - c1.lista_puntos[1 - 1].p.Y))) / (((c1.lista_puntos[1 + 1].p.X - c1.lista_puntos[1].p.X) / (c1.lista_puntos[1 + 1].p.Y - c1.lista_puntos[1].p.Y)) - ((c1.lista_puntos[1].p.X - c1.lista_puntos[1 - 1].p.X) / (c1.lista_puntos[1].p.Y - c1.lista_puntos[1 - 1].p.Y)));
            double yc = (c1.lista_puntos[1].p.Y + c1.lista_puntos[1 - 1].p.Y) * 0.5 + (c1.lista_puntos[1].p.X - c1.lista_puntos[1 - 1].p.X) * 0.5 * (c1.lista_puntos[1].p.X + c1.lista_puntos[1 - 1].p.X) * Math.Pow(c1.lista_puntos[1].p.Y - c1.lista_puntos[1 - 1].p.Y, -1) - xc * (c1.lista_puntos[1].p.X - c1.lista_puntos[1 - 1].p.X) * Math.Pow(c1.lista_puntos[1].p.Y - c1.lista_puntos[1 - 1].p.Y, -1);
            double r = Get_R(c1);
            //Cálculo del azimut del radio a la entrada Azte
            Punto p_e = Rellenar_centro(c1.lista_puntos[0], xc, yc, 1);
            //Cálculo del azimut del radio a la salida Azts
            Punto p_s = Rellenar_centro(c1.lista_puntos[c1.lista_puntos.Count - 1], xc, yc, 1);
            if (p_e.Az > 0 && p_e.Az < 180)
            {
                if (p_e.Az - p_s.Az < 0 && Math.Abs(p_e.Az - p_s.Az) < 180)
                {
                    //horario
                    if (p_e.Az + 90 > 360)
                    {
                        c1.azte = p_e.Az + 90 - 360;
                    }
                    else
                    {
                        c1.azte = p_e.Az + 90;
                    }

                    if (p_s.Az + 90 > 360)
                    {
                        c1.azts = p_s.Az + 90 - 360;
                    }
                    else
                    {
                        c1.azts = p_s.Az + 90;
                    }
                }
                else
                {
                    //antihorario
                    if (p_e.Az - 90 < 0)
                    {
                        c1.azte = 360 + p_e.Az - 90;
                    }
                    else
                    {
                        c1.azte = p_e.Az - 90;
                    }

                    c1.azts = p_s.Az - 90;
                }
            }
            else
            {
                if (p_e.Az - p_s.Az > 0 && Math.Abs(p_e.Az - p_s.Az) < 180)
                {
                    //antihorario
                    if (p_e.Az - 90 < 0)
                    {
                        c1.azte = 360 + p_e.Az - 90;
                    }
                    else
                    {
                        c1.azte = p_e.Az - 90;
                    }

                    c1.azts = p_s.Az - 90;
                }
                else
                {
                    //horario
                    if (p_e.Az + 90 > 360)
                    {
                        c1.azte = p_e.Az + 90 - 360;
                    }
                    else
                    {
                        c1.azte = p_e.Az + 90;
                    }

                    if (p_s.Az + 90 > 360)
                    {
                        c1.azts = p_s.Az + 90 - 360;
                    }
                    else
                    {
                        c1.azts = p_s.Az + 90;
                    }
                }
            }
            if (c1.azts < 0)
            {
                c1.azts += 360;
            }
            if (c1.azte < 0)
            {
                c1.azte += 360;
            }
            if (c1.azts > 360)
            {
                c1.azts -= 360;
            }
            if (c1.azte > 360)
            {
                c1.azte -= 360;
            }
            c1.azcurva = -c1.azte + c1.azts;
            c1.azmax = c1.azcurva / 2;
            c1.xc = xc;
            c1.yc = yc;
            if (c1.direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                if (c1.azte < c1.azts)
                {
                    c1.dif_az = c1.azts - c1.azte;
                }
                else
                {
                    if (c1.azte < 0)
                    {
                        c1.dif_az = c1.azts - c1.azte;
                    }
                    else
                    {
                        c1.dif_az = c1.azts + (360 - c1.azte);
                    }

                }
            }
            else
            {
                if (c1.azte > c1.azts)
                {
                    c1.dif_az = c1.azte - c1.azts;
                }
                else
                {
                    if (c1.azts < 0)
                    {
                        c1.dif_az = c1.azte - c1.azts;
                    }
                    else
                    {
                        c1.dif_az = c1.azte + (360 - c1.azts);
                    }
                }
            }
            //azimut_e_s(c1);
        }
        public List<Punto> Puntos_iguales_listas(List<Punto> l)
        {
            for (int i = 0; i < l.Count - 1; i++)
            {
                if (Math.Truncate(l[i].p.X * 100000) == Math.Truncate(l[i + 1].p.X * 100000))
                {
                    l[i + 1].Cambiar_X_Y(l[i + 1].p.X + 0.0001, l[i + 1].p.Y);
                }
                if (Math.Truncate(l[i].p.Y * 100000) == Math.Truncate(l[i + 1].p.Y * 100000))
                {
                    l[i + 1].Cambiar_X_Y(l[i + 1].p.X, l[i + 1].p.Y + 0.0001);
                }
            }
            for (int i = 0; i < l.Count - 2; i++)
            {
                if ((Math.Truncate(l[i].p.X * 100000) + (Math.Truncate(l[i + 1].p.X * 100000))) == (Math.Truncate(l[i + 1].p.X * 100000) + (Math.Truncate(l[i + 2].p.X * 100000))))
                {
                    l[i + 1].Cambiar_X_Y(l[i + 1].p.X + 0.0001, l[i + 1].p.Y);
                }
                if ((Math.Truncate(l[i].p.Y * 100000) + (Math.Truncate(l[i + 1].p.Y * 100000))) == (Math.Truncate(l[i + 1].p.Y * 100000) + (Math.Truncate(l[i + 2].p.Y * 100000))))
                {
                    l[i + 1].Cambiar_X_Y(l[i + 1].p.X, l[i + 1].p.Y + 0.0001);
                }
            }
            return l;
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
        public void Crear_RECM(List<Logica.Componente> componentes, int i)//Crea una recta entre curvas de mismo sentido
        {

            double r1, r2;
            r1 = componentes[i].radio;
            r2 = componentes[i + 1].radio;
            double d1_2 = Distancia(new Point2d(componentes[i].xc, componentes[i].yc), new Point2d(componentes[i + 1].xc, componentes[i + 1].yc));

            double alfa = Math.Asin((componentes[i].radio - componentes[i + 1].radio) / (d1_2));

            Punto p = Rellenar_centro(componentes[i + 1].xc, componentes[i + 1].yc, componentes[i].xc, componentes[i].yc, 1);
            //corrección
            double az = 0, az2 = 0;
            az = p.Az - (90 - alfa * (180 / Math.PI));
            az2 = p.Az + (90 - alfa * (180 / Math.PI));
            double az_aux;
            if (az > 360)
            {
                az = az - 360;
            }
            else
            {
                if (az < 0)
                {
                    az = az + 360;
                }
            }
            if (az2 > 360)
            {
                az2 = az2 - 360;
            }
            else
            {
                if (az2 < 0)
                {
                    az2 = az2 + 360;
                }
            }
            double az_temp;
            double aze = componentes[i].azte;
            double azs = componentes[i].azts;
            double aze2 = componentes[i + 1].azte;
            double azs2 = componentes[i + 1].azts;
            bool ninguna_valida = true;
            double az1_temp = 0, az2_temp = 0;
            if (componentes[i].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                az_temp = az;
                aze -= 90;
                azs -= 90;
                aze2 -= 90;
                azs2 -= 90;
                if (aze < 0)
                {
                    aze += 360;
                }
                if (azs < 0)
                {
                    azs += 360;
                }
                if (aze2 < 0)
                {
                    aze2 += 360;
                }
                if (azs2 < 0)
                {
                    azs2 += 360;
                }
                if (aze > aze2)
                {
                    aze2 += 360;
                }
                if (aze > azs)
                {
                    azs += 360;
                }
                if (aze > azs2)
                {
                    azs2 += 360;
                }

                if (aze < az_temp && az_temp < azs2)
                {
                    if (((aze + aze2) / 2) > az)
                    {
                        az1_temp = ((aze + aze2) / 2) - az;
                    }
                    if (((aze + aze2) / 2) < az)
                    {
                        az1_temp = az - ((aze + aze2) / 2);
                    }
                    if (((aze + aze2) / 2) > az2)
                    {
                        az2_temp = ((aze + aze2) / 2) - az2;
                    }
                    if (((aze + aze2) / 2) < az2)
                    {
                        az2_temp = az2 - ((aze + aze2) / 2);
                    }
                    if (az1_temp > az2_temp)
                    {
                        //az = az2;
                    }
                }
                else
                {
                    if (aze < az2 && az2 < azs2)
                    {
                        //az = az2;
                    }
                    else
                    {
                        //ninguna_valida = false;
                    }
                }
            }
            else
            {
                az_temp = az;
                aze += 90;
                azs += 90;
                aze2 += 90;
                azs2 += 90;
                ///
                if (aze < 0)
                {
                    aze += 360;
                }
                if (azs < 0)
                {
                    azs += 360;
                }
                if (aze2 < 0)
                {
                    aze2 += 360;
                }
                if (azs2 < 0)
                {
                    azs2 += 360;
                }
                ////
                if (aze > 360)
                {
                    aze -= 360;
                }
                if (azs > 360)
                {
                    azs -= 360;
                }
                if (aze2 > 360)
                {
                    aze2 -= 360;
                }
                if (azs2 > 360)
                {
                    azs2 -= 360;
                }
                ////
                if (aze < aze2)
                {
                    aze += 360;
                }
                if (aze < azs)
                {
                    aze += 360;
                }
                if (aze < azs2)
                {
                    aze += 360;
                }
                if (aze > az_temp && az_temp > azs2)
                {
                    if (((aze + aze2) / 2) > az)
                    {
                        az1_temp = ((aze + aze2) / 2) - az;
                    }
                    if (((aze + aze2) / 2) < az)
                    {
                        az1_temp = az - ((aze + aze2) / 2);
                    }
                    if (((aze + aze2) / 2) > az2)
                    {
                        az2_temp = ((aze + aze2) / 2) - az2;
                    }
                    if (((aze + aze2) / 2) < az2)
                    {
                        az2_temp = az2 - ((aze + aze2) / 2);
                    }
                    if (az1_temp > az2_temp)
                    {
                        //az = az2;
                    }

                }
                else
                {
                    if (aze > az2 && az2 > azs2)
                    {
                        //az = az2;
                    }
                    else
                    {
                        //ninguna_valida = false;
                    }

                }
            }



            if (ninguna_valida)
            {
                double seno = Math.Sin(az * Math.PI / 180);
                double coseno = Math.Cos(az * Math.PI / 180);
                double xx = componentes[i].xc + (r1) * Math.Sin((az) * Math.PI / 180);
                double yy = componentes[i].yc + (r1) * Math.Cos((az) * Math.PI / 180);
                double xx2 = componentes[i + 1].xc + (r2) * Math.Sin((az) * Math.PI / 180);
                double yy2 = componentes[i + 1].yc + (r2) * Math.Cos((az) * Math.PI / 180);
                double x_med = (xx + xx2) / 2;
                double y_med = (yy + yy2) / 2;

                double d1 = Distancia(new Point2d(xx, yy), componentes[i].lista_puntos[0].p) + Distancia(new Point2d(xx, yy), componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1].p);
                double d2 = Distancia(new Point2d(xx2, yy2), componentes[i + 1].lista_puntos[0].p) + Distancia(new Point2d(xx2, yy2), componentes[i + 1].lista_puntos[componentes[i + 1].lista_puntos.Count - 1].p);

                double seno2 = Math.Sin(az2 * Math.PI / 180);
                double coseno2 = Math.Cos(az2 * Math.PI / 180);
                double xx_2 = componentes[i].xc + (r1) * Math.Sin((az2) * Math.PI / 180);
                double yy_2 = componentes[i].yc + (r1) * Math.Cos((az2) * Math.PI / 180);
                double xx2_2 = componentes[i + 1].xc + (r2) * Math.Sin((az2) * Math.PI / 180);
                double yy2_2 = componentes[i + 1].yc + (r2) * Math.Cos((az2) * Math.PI / 180);
                double x_med_2 = (xx_2 + xx2_2) / 2;
                double y_med_2 = (yy_2 + yy2_2) / 2;

                double d3 = Distancia(new Point2d(xx_2, yy_2), componentes[i].lista_puntos[0].p) + Distancia(new Point2d(xx_2, yy_2), componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1].p);
                double d4 = Distancia(new Point2d(xx2_2, yy2_2), componentes[i + 1].lista_puntos[0].p) + Distancia(new Point2d(xx2_2, yy2_2), componentes[i + 1].lista_puntos[componentes[i + 1].lista_puntos.Count - 1].p);

                aze = componentes[i].azte;
                azs = componentes[i].azts;
                aze2 = componentes[i + 1].azte;
                azs2 = componentes[i + 1].azts;
                double az_fin = p.Az - (90 - alfa * (180 / Math.PI));
                double az_fin2 = p.Az + (90 - alfa * (180 / Math.PI));

                if (componentes[i].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    aze -= 90;
                    azs -= 90;
                    aze2 -= 90;
                    azs2 -= 90;

                    if (aze > azs)
                    {
                        aze -= 360;
                    }
                    if (aze2 > azs2)
                    {
                        azs2 += 360;
                    }
                    if (az_fin2 > 360)
                    {
                        az_fin2 -= 360;
                    }
                    if (az_fin2 < 0)
                    {
                        az_fin2 += 360;
                    }
                    if (aze < az_fin && az_fin < azs2)
                    {
                        if (aze < az_fin2 && az_fin2 < azs2)
                        {
                            if ((d1 + d2) > (d3 + d4))
                            {
                                xx = xx_2;
                                yy = yy_2;
                                xx2 = xx2_2;
                                yy2 = yy2_2;
                            }
                        }
                    }
                    else
                    {
                        if (aze < az_fin2 && az_fin2 < azs2)
                        {
                            if ((d1 + d2) > (d3 + d4))
                            {
                                xx = xx_2;
                                yy = yy_2;
                                xx2 = xx2_2;
                                yy2 = yy2_2;
                            }
                        }
                        /* xx = xx_2;
                         yy = yy_2;
                         xx2 = xx2_2;
                         yy2 = yy2_2;*/
                    }
                }
                else
                {
                    aze += 90;
                    azs += 90;
                    aze2 += 90;
                    azs2 += 90;

                    if (aze < azs)
                    {
                        aze += 360;
                    }
                    if (aze2 < azs2)
                    {
                        azs2 -= 360;
                    }
                    if (aze > 360 && azs2 > 360)
                    {
                        aze -= 360;
                        azs2 -= 360;
                    }
                    if (az_fin2 > 360)
                    {
                        az_fin2 -= 360;
                    }
                    if (az_fin2 < 0)
                    {
                        az_fin2 += 360;
                    }
                    if (aze > az_fin && az_fin > azs2)
                    {
                        if (aze > az_fin2 && az_fin2 > azs2)
                        {
                            if ((d1 + d2) > (d3 + d4))
                            {
                                xx = xx_2;
                                yy = yy_2;
                                xx2 = xx2_2;
                                yy2 = yy2_2;
                            }
                        }
                    }
                    else
                    {
                        /*if (aze > az_fin2 && az_fin2 > azs2)
                        {
                            if ((d1 + d2) > (d3 + d4))
                            {
                                xx = xx_2;
                                yy = yy_2;
                                xx2 = xx2_2;
                                yy2 = yy2_2;
                            }
                        }*/
                        xx = xx_2;
                        yy = yy_2;
                        xx2 = xx2_2;
                        yy2 = yy2_2;
                    }
                }


                double distancia = 1;
                EntidadFlotante entidadFlotante = new EntidadFlotante();
                entidadFlotante.CargarDatos("RECTA", distancia, i + 1, 0);

                if (entidadFlotante.ShowDialog() == DialogResult.OK)  // Solo ShowDialog() para evitar doble ventana
                {
                    distancia = entidadFlotante.dato;
                }


                if (componentes[i].xc < xx)
                {
                    xx += distancia;
                    xx2 += distancia;
                }
                else
                {
                    xx -= distancia;
                    xx2 -= distancia;
                }
                if (componentes[i].yc < yy)
                {
                    yy += distancia;
                    yy2 += distancia;
                }
                else
                {
                    yy -= distancia;
                    yy2 -= distancia;
                }
                if (Distancia(componentes[i].lista_puntos[0].p, new Point2d(xx, yy)) > Distancia(componentes[i].lista_puntos[0].p, new Point2d(xx2, yy2)))
                {
                    componentes.Insert(i + 1, new Logica.Componente(new Punto(new Point2d(xx2, yy2)), 1));
                    componentes[i + 1].add(new Punto(new Point2d(xx, yy)));
                }
                else
                {
                    componentes.Insert(i + 1, new Logica.Componente(new Punto(new Point2d(xx, yy)), 1));
                    componentes[i + 1].add(new Punto(new Point2d(xx2, yy2)));
                }

                componentes[i + 1].creacion = 2;
                tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[0].p.X, componentes[i + 1].lista_puntos[0].p.Y, 0);
                tadLayShare.puntos.Punto3d p3 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[1].p.X, componentes[i + 1].lista_puntos[1].p.Y, 0);
                //Dibujar_r(p2, p3);

            }
            else
            {
                //               if ((componentes[i].radio - componentes[i+1].radio/ componentes[i].radio)<0.25)
                //               {
                List<Punto> lista = new List<Punto>();
                /*for (int r = componentes[i].ini; r <= componentes[i + 1].fin; r++)
                {
                    lista.Add(new Punto(polilinea[r].p));
                }*/
                for (int r = 0; r <= componentes[i].lista_puntos.Count - 1; r++)
                {
                    lista.Add(new Punto(componentes[i].lista_puntos[r].p));
                }
                for (int r = 0; r <= componentes[i + 1].lista_puntos.Count - 1; r++)
                {
                    lista.Add(new Punto(componentes[i + 1].lista_puntos[r].p));
                }
                Tuple<double, double, double> Curva = this.Clusterizacion(lista);
                Crear_Curva_2Puntos(componentes, lista[0], lista[lista.Count - 1], Curva.Item1, Curva.Item2, Curva.Item3, i);
                //Crear_Curva(componentes[i].ini, componentes[i + 1].fin, Curva.Item1, Curva.Item2, Curva.Item3, i);
                componentes[i].direccion = componentes[i + 1].direccion;
                componentes[i].Clusterizada = true;
                componentes.RemoveAt(i + 1);
                componentes.RemoveAt(i + 1);
                //Viabilidad_Clusterizacion(i);
                /*                }
                                else
                                {
                                    Eliminar_Curva(i);
                                }
                 */
                /*
                */
                if (i + 1 < componentes.Count)
                {
                    if (componentes[i + 1].creacion == 2)
                    {
                        componentes.RemoveAt(i + 1);
                    }
                }
                if (i - 1 > 0)
                {
                    if (componentes[i - 1].creacion == 2)
                    {
                        componentes.RemoveAt(i - 1);
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
        private void Crear_Curva_2Puntos(List<Logica.Componente> componentes, Punto min, Punto max, double xc, double yc, double r, int i)
        {
            Punto p1 = new Punto();
            p1 = Rellenar_centro(min, xc, yc, 1);//primer punto
            double az1 = p1.Az + 90;

            double xx1 = xc - (r) * Math.Sin((az1 + 90) * Math.PI / 180);
            double yy1 = yc - (r) * Math.Cos((az1 + 90) * Math.PI / 180);
            p1 = new Punto(new Point2d(xx1, yy1));

            Punto p3 = new Punto();
            p3 = Rellenar_centro(max, xc, yc, 1);//ultimo punto

            double az3 = p3.Az + 90;
            double xx3 = xc - (r) * Math.Sin((az3 + 90) * Math.PI / 180);
            double yy3 = yc - (r) * Math.Cos((az3 + 90) * Math.PI / 180);
            p3 = new Punto(new Point2d(xx3, yy3));

            Punto p2 = new Punto();
            double az2;
            if (az1 > 360)
            {
                az1 -= 360;
            }
            if (az1 < 0)
            {
                az1 += 360;
            }
            if (az3 > 360)
            {
                az3 -= 360;
            }
            if (az3 < 0)
            {
                az3 += 360;
            }

            if (componentes[i + 1].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                if (az1 < az3)
                {
                    az2 = (az3 + az1) / 2;
                }
                else
                {
                    if (r > 5000 && az1 < 300 && az3 > 45)
                    {
                        az2 = (az3 + az1) / 2;
                    }
                    else
                    {
                        az2 = az1 + (az3 + (360 - az1)) / 2;
                    }

                }
            }
            else
            {
                if (az1 > az3)
                {
                    az2 = (az3 + az1) / 2;
                }
                else
                {
                    if (r > 5000 && az3 < 300 && az1 > 45)
                    {
                        az2 = (az3 + az1) / 2;
                    }
                    else
                    {
                        az2 = az1 - ((360 - az3) + az1) / 2;
                    }

                }
            }
            double xx2 = xc - (r) * Math.Sin((az2 + 90) * Math.PI / 180);
            double yy2 = yc - (r) * Math.Cos((az2 + 90) * Math.PI / 180);
            p2 = new Punto(new Point2d(xx2, yy2));



            //                               Dibujar_c(Listas_curvas[t].Item2[cr[0]], Listas_curvas[t].Item3[cr[0]], Listas_curvas[t].Item4[cr[0]]);
            componentes.Insert(i, new Logica.Componente(p1, 2));
            componentes[i].add(p2);
            componentes[i].add(p3);
            componentes[i].radio = r;
            componentes[i].xc = xc;
            componentes[i].yc = yc;
            componentes[i].ini = 0;
            componentes[i].fin = 0;
            if (Dif_az(componentes, i + 1) > Dif_az(componentes, i + 2))
            {
                componentes[i].index = componentes[i + 2].index;
            }
            else
            {
                componentes[i].index = componentes[i + 1].index;
            }

            //Dibujar_entidad(i);
        }
        public double Dif_az(List<Logica.Componente> componentes, int i)
        {
            if (componentes[i].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                if (componentes[i].azte < componentes[i].azts)
                {
                    return componentes[i].azts - componentes[i].azte;
                }
                else
                {
                    return 360 - componentes[i].azte + componentes[i].azts;
                }
            }
            else
            {
                if (componentes[i].azte > componentes[i].azts)
                {
                    return componentes[i].azte - componentes[i].azts;
                }
                else
                {
                    return 360 - componentes[i].azts + componentes[i].azte;
                }
            }
        }
        public void Crear_RECT(List<Logica.Componente> componentes, int i)
        {
            if (componentes[i].direccion == componentes[i + 1].direccion)
            {
                Crear_RECM(componentes, i);
            }
            else
            {
                double primer_x, primer_y, segundo_x, segundo_y;
                Point2d puntoa1 = new Point2d(componentes[i].lista_puntos[0].p.X, componentes[i].lista_puntos[0].p.Y);
                Point2d puntoa2 = new Point2d(componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1].p.X, componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1].p.Y);

                Point2d puntob1 = new Point2d(componentes[i + 1].lista_puntos[0].p.X, componentes[i + 1].lista_puntos[0].p.Y);
                Point2d puntob2 = new Point2d(componentes[i + 1].lista_puntos[componentes[i + 1].lista_puntos.Count - 1].p.X, componentes[i + 1].lista_puntos[componentes[i + 1].lista_puntos.Count - 1].p.Y);
                if (componentes[i].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    primer_x = componentes[i].xc - (componentes[i].radio) * Math.Sin((componentes[i].azte + 90) * Math.PI / 180);
                    primer_y = componentes[i].yc - (componentes[i].radio) * Math.Cos((componentes[i].azte + 90) * Math.PI / 180);
                    segundo_x = componentes[i].xc - (componentes[i].radio) * Math.Sin((componentes[i].azts + 90) * Math.PI / 180);
                    segundo_y = componentes[i].yc - (componentes[i].radio) * Math.Cos((componentes[i].azts + 90) * Math.PI / 180);
                }
                else
                {
                    primer_x = componentes[i].xc + (componentes[i].radio) * Math.Sin((componentes[i].azte + 90) * Math.PI / 180);
                    primer_y = componentes[i].yc + (componentes[i].radio) * Math.Cos((componentes[i].azte + 90) * Math.PI / 180);
                    segundo_x = componentes[i].xc + (componentes[i].radio) * Math.Sin((componentes[i].azts + 90) * Math.PI / 180);
                    segundo_y = componentes[i].yc + (componentes[i].radio) * Math.Cos((componentes[i].azts + 90) * Math.PI / 180);
                }

                componentes[i].lista_puntos[0] = new Punto(new Point2d(primer_x, primer_y));
                componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1] = new Punto(new Point2d(segundo_x, segundo_y));
                if (componentes[i + 1].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    primer_x = componentes[i + 1].xc - (componentes[i + 1].radio) * Math.Sin((componentes[i + 1].azte + 90) * Math.PI / 180);
                    primer_y = componentes[i + 1].yc - (componentes[i + 1].radio) * Math.Cos((componentes[i + 1].azte + 90) * Math.PI / 180);
                    segundo_x = componentes[i + 1].xc - (componentes[i + 1].radio) * Math.Sin((componentes[i + 1].azts + 90) * Math.PI / 180);
                    segundo_y = componentes[i + 1].yc - (componentes[i + 1].radio) * Math.Cos((componentes[i + 1].azts + 90) * Math.PI / 180);
                }
                else
                {
                    primer_x = componentes[i + 1].xc + (componentes[i + 1].radio) * Math.Sin((componentes[i + 1].azte + 90) * Math.PI / 180);
                    primer_y = componentes[i + 1].yc + (componentes[i + 1].radio) * Math.Cos((componentes[i + 1].azte + 90) * Math.PI / 180);
                    segundo_x = componentes[i + 1].xc + (componentes[i + 1].radio) * Math.Sin((componentes[i + 1].azts + 90) * Math.PI / 180);
                    segundo_y = componentes[i + 1].yc + (componentes[i + 1].radio) * Math.Cos((componentes[i + 1].azts + 90) * Math.PI / 180);
                }

                componentes[i + 1].lista_puntos[0] = new Punto(new Point2d(primer_x, primer_y));
                componentes[i + 1].lista_puntos[componentes[i + 1].lista_puntos.Count - 1] = new Punto(new Point2d(segundo_x, segundo_y));
                //punto intermedio para recta para clotoide en s
                double d1_2 = Distancia(new Point2d(componentes[i].xc, componentes[i].yc), new Point2d(componentes[i + 1].xc, componentes[i + 1].yc));

                //vector unitario
                double v_x = (-componentes[i].xc + componentes[i + 1].xc) / d1_2;
                double v_y = (-componentes[i].yc + componentes[i + 1].yc) / d1_2;

                double d_p = d_p = d1_2 / (1 + (componentes[i + 1].radio / componentes[i].radio));


                double x_pi = componentes[i].xc + v_x * d_p;
                double y_pi = componentes[i].yc + v_y * d_p;

                Punto p = Rellenar_centro(componentes[i + 1].xc, componentes[i + 1].yc, componentes[i].xc, componentes[i].yc, 1);
                //corrección
                double az;
                if (componentes[i].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    az = p.Az + 90;
                }
                else
                {
                    az = p.Az - 90;
                }
                if (az > 360)
                {
                    az = az - 360;
                }
                else
                {
                    if (az < 0)
                    {
                        az = az + 360;
                    }
                }
                if (componentes[i].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    az = az - 0.1;
                }
                else
                {
                    az = az + 0.1;
                }

                double seno = Math.Sin(az * Math.PI / 180);
                double coseno = Math.Cos(az * Math.PI / 180);

                componentes[i].lista_puntos[0] = new Punto(puntoa1);
                componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1] = new Punto(puntoa2);

                componentes[i + 1].lista_puntos[0] = new Punto(puntob1);
                componentes[i + 1].lista_puntos[componentes[i + 1].lista_puntos.Count - 1] = new Punto(puntob2);

                componentes.Insert(i + 1, new Logica.Componente(new Punto(new Point2d(x_pi - 1 * seno, y_pi - 1 * coseno)), 1));
                componentes[i + 1].add(new Punto(new Point2d(x_pi + 1 * seno, y_pi + 1 * coseno)));
                componentes[i + 1].creacion = 1;

                Giro_Tangente(componentes, i + 1);

                tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[0].p.X, componentes[i + 1].lista_puntos[0].p.Y, 0);
                tadLayShare.puntos.Punto3d p3 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[1].p.X, componentes[i + 1].lista_puntos[1].p.Y, 0);
                CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
                //calculoPolilinea.Dibujar_r(p2, p3, "pruebaaa");
            }


        }
        private void Giro_Tangente(List<Logica.Componente> componentes, int i,double girocorto=1)
        {
            Rellenar_Recta(componentes[i]);
            int cont = 0;
            double giro = Girar_acercar(componentes[i].lista_puntos, componentes[i - 1], componentes[i].azr);
            if (giro > 0)
            {
                //while (giro > 0)
                //{
                Rellenar_Recta(componentes[i]);
                giro = Girar_acercar(componentes[i].lista_puntos, componentes[i - 1], componentes[i].azr)/girocorto;

                double[] l = new double[2];
                List<Punto> l_p2 = Girar_Recta_2_Buena(componentes[i].lista_puntos, giro, 0, componentes[i].azr);
                l = ajuste_recta(l_p2, 0).Item2;
                double r2 = Distancia_P_R(l[0], l[1], componentes[i - 1].xc, componentes[i - 1].yc);
                //r2 = Distancia(l_p2[0].p, new Point2d(componentes[i - 1].xc, componentes[i - 1].yc));

                if (r2 > componentes[i - 1].radio)
                {
                    Girar_Recta(componentes, i, giro, componentes[i].azr);
                }
                else
                {
                    //break;
                }
                //}
            }
            else
            {
                //while (giro < 0)
                //{

                Rellenar_Recta(componentes[i]);
                giro = Girar_acercar(componentes[i].lista_puntos, componentes[i - 1], componentes[i].azr) / girocorto;

                double[] l = new double[2];
                List<Punto> l_p2 = Girar_Recta_2_Buena(componentes[i].lista_puntos, giro, 0, componentes[i].azr);
                l = ajuste_recta(l_p2, 0).Item2;
                double r2 = Distancia_P_R(l[0], l[1], componentes[i - 1].xc, componentes[i - 1].yc);
                //r2 = Distancia(l_p2[0].p, new Point2d(componentes[i - 1].xc, componentes[i - 1].yc));

                if (r2 > componentes[i - 1].radio)
                {
                    Girar_Recta(componentes, i, giro, componentes[i].azr);

                }
                else
                {
                    //break;
                }

                //}
            }

        }
        private double Girar_acercar(List<Punto> l, Logica.Componente c)
        {
            double ec1, ec2;
            ec1 = ((l[0].p.Y - l[1].p.Y) / (l[0].p.X - l[1].p.X));
            ec2 = -l[0].p.X * ((l[0].p.Y - l[1].p.Y) / (l[0].p.X - l[1].p.X)) + l[0].p.Y;
            double result = Math.Pow(2 * ec1 * ec2 - 2 * c.xc - 2 * c.yc * ec1, 2) - 4 * (1 + Math.Pow(ec1, 2)) * (Math.Pow(c.xc, 2) + Math.Pow(ec2, 2) + Math.Pow(c.yc, 2) - 2 * c.yc * ec2 - Math.Pow(c.radio, 2));
            List<Punto> l_p = Girar_Recta_2(l, 0.1, 1);
            Tuple<List<Punto>, double[], int> l_t;
            l_t = ajuste_recta(l_p, 0);
            double result2 = Math.Pow(2 * l_t.Item2[0] * l_t.Item2[1] - 2 * c.xc - 2 * c.yc * l_t.Item2[0], 2) - 4 * (1 + Math.Pow(l_t.Item2[0], 2)) * (Math.Pow(c.xc, 2) + Math.Pow(l_t.Item2[1], 2) + Math.Pow(c.yc, 2) - 2 * c.yc * l_t.Item2[1] - Math.Pow(c.radio, 2));
            if (result < result2 && result < 0)
            {
                return 0.1;
            }
            else
            {
                return -0.1;
            }

        }
        public Tuple<List<Punto>, double[], int> ajuste_recta(List<Punto> listaa, int posicion)
        {
            List<Punto> ajustada = new List<Punto>();
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
                if (double.IsNaN(recta[0]) && double.IsNaN(recta[1]))
                {
                    recta[0] = listaa[0].p.X;
                    recta[1] = 0;
                    for (int i = 0; i < listaa.Count; i++)
                    {
                        ajustada.Add(new Punto(listaa[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < listaa.Count; i++)
                    {
                        ajustada.Add(new Punto(listaa[i]));
                        ajustada[i].p = new Point2d(ajustada[i].p.X, recta[0] * ajustada[i].p.X + recta[1]);

                    }
                }

            }
            else
            {
                ajustada = listaa;
                if (Math.Truncate(listaa[0].p.X * 100000) == Math.Truncate(listaa[1].p.X * 100000))
                {
                    recta[0] = listaa[0].p.X;
                    recta[1] = 0;
                }
                else
                {
                    recta[0] = (listaa[1].p.Y - listaa[0].p.Y) / (listaa[1].p.X - listaa[0].p.X);
                    recta[1] = listaa[0].p.Y - listaa[0].p.X * ((listaa[1].p.Y - listaa[0].p.Y) / (listaa[1].p.X - listaa[0].p.X));
                }

            }


            return Tuple.Create(ajustada, recta, posicion);
        }
        private List<Punto> Girar_Recta_2(List<Punto> l, double giro, int p)//el punto de giro es un extremo-->p==1 punto inicial//p==2 punto final
        {
            double xc_1 = l[0].p.X;
            double yc_1 = l[0].p.Y;
            double xc_2 = l[1].p.X;
            double yc_2 = l[1].p.Y;
            Point2d pm;
            double x_p_1 = xc_1;
            double y_p_1 = yc_1;
            double x_p_2 = xc_2;
            double y_p_2 = yc_2;
            pm = new Point2d((xc_1 + xc_2) / 2, (yc_1 + yc_2) / 2);
            if (p == 1)
            {
                pm = new Point2d(xc_1, yc_1);
                double eee = Math.Abs(xc_2 - pm.X) * Math.Cos(-giro * Math.PI / 180) - Math.Abs(yc_2 - pm.Y) * Math.Sin(-giro * Math.PI / 180);
                x_p_2 = pm.X - eee;
                double ttt = Math.Abs(xc_2 - pm.X) * Math.Sin(-giro * Math.PI / 180) + Math.Abs(yc_2 - pm.Y) * Math.Cos(-giro * Math.PI / 180);
                y_p_2 = pm.Y - ttt;
            }
            else if (p == 2)
            {
                pm = new Point2d(xc_2, yc_2);
                x_p_1 = pm.X - (pm.X - xc_1) * Math.Cos(-giro * Math.PI / 180) + (pm.Y - yc_1) * Math.Sin(-giro * Math.PI / 180);
                y_p_1 = pm.Y - (pm.X - xc_1) * Math.Sin(-giro * Math.PI / 180) - (pm.Y - yc_1) * Math.Cos(-giro * Math.PI / 180);
            }
            else
            {
                double eee = Math.Abs(xc_2 - pm.X) * Math.Cos(-giro * Math.PI / 180) - Math.Abs(yc_2 - pm.Y) * Math.Sin(-giro * Math.PI / 180);
                x_p_2 = pm.X - eee;
                double ttt = Math.Abs(xc_2 - pm.X) * Math.Sin(-giro * Math.PI / 180) + Math.Abs(yc_2 - pm.Y) * Math.Cos(-giro * Math.PI / 180);
                y_p_2 = pm.Y - ttt;
                x_p_1 = pm.X - (pm.X - xc_1) * Math.Cos(-giro * Math.PI / 180) + (pm.Y - yc_1) * Math.Sin(-giro * Math.PI / 180);
                y_p_1 = pm.Y - (pm.X - xc_1) * Math.Sin(-giro * Math.PI / 180) - (pm.Y - yc_1) * Math.Cos(-giro * Math.PI / 180);
            }

            tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(x_p_1, y_p_1, 0);
            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(x_p_2, y_p_2, 0);
            if (p == 3)
            {
                //Dibujar_r(p1,p2);
            }
            l = new List<Punto>();
            l.Add(new Punto(new Point2d(p1.coordenadaX, p1.coordenadaY)));
            l.Add(new Punto(new Point2d(p2.coordenadaX, p2.coordenadaY)));

            return l;
        }
        public void Rellenar_Recta(Logica.Componente c1)
        {
            Punto p_r = new Punto();
            tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(c1.lista_puntos[c1.lista_puntos.Count - 2].p.X, c1.lista_puntos[c1.lista_puntos.Count - 2].p.Y, 0);
            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(c1.lista_puntos[c1.lista_puntos.Count - 1].p.X, c1.lista_puntos[c1.lista_puntos.Count - 1].p.Y, 0);
            double xc = c1.lista_puntos[c1.lista_puntos.Count - 2].p.X;
            double yc = c1.lista_puntos[c1.lista_puntos.Count - 2].p.Y;
            p_r = Rellenar_centro(c1.lista_puntos[c1.lista_puntos.Count - 1], xc, yc, 1);
            c1.azr = p_r.Az;
        }
        public void Crear_RECD(List<Logica.Componente> componentes, int i)//Crea una recta entre curvas de distinto sentido
        {

            double primer_x, primer_y, segundo_x, segundo_y;
            if (componentes[i].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                primer_x = componentes[i].xc - (componentes[i].radio) * Math.Sin((componentes[i].azte + 90) * Math.PI / 180);
                primer_y = componentes[i].yc - (componentes[i].radio) * Math.Cos((componentes[i].azte + 90) * Math.PI / 180);
                segundo_x = componentes[i].xc - (componentes[i].radio) * Math.Sin((componentes[i].azts + 90) * Math.PI / 180);
                segundo_y = componentes[i].yc - (componentes[i].radio) * Math.Cos((componentes[i].azts + 90) * Math.PI / 180);
            }
            else
            {
                primer_x = componentes[i].xc + (componentes[i].radio) * Math.Sin((componentes[i].azte + 90) * Math.PI / 180);
                primer_y = componentes[i].yc + (componentes[i].radio) * Math.Cos((componentes[i].azte + 90) * Math.PI / 180);
                segundo_x = componentes[i].xc + (componentes[i].radio) * Math.Sin((componentes[i].azts + 90) * Math.PI / 180);
                segundo_y = componentes[i].yc + (componentes[i].radio) * Math.Cos((componentes[i].azts + 90) * Math.PI / 180);
            }

            componentes[i].lista_puntos[0] = new Punto(new Point2d(primer_x, primer_y));
            componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1] = new Punto(new Point2d(segundo_x, segundo_y));
            if (componentes[i + 1].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                primer_x = componentes[i + 1].xc - (componentes[i + 1].radio) * Math.Sin((componentes[i + 1].azte + 90) * Math.PI / 180);
                primer_y = componentes[i + 1].yc - (componentes[i + 1].radio) * Math.Cos((componentes[i + 1].azte + 90) * Math.PI / 180);
                segundo_x = componentes[i + 1].xc - (componentes[i + 1].radio) * Math.Sin((componentes[i + 1].azts + 90) * Math.PI / 180);
                segundo_y = componentes[i + 1].yc - (componentes[i + 1].radio) * Math.Cos((componentes[i + 1].azts + 90) * Math.PI / 180);
            }
            else
            {
                primer_x = componentes[i + 1].xc + (componentes[i + 1].radio) * Math.Sin((componentes[i + 1].azte + 90) * Math.PI / 180);
                primer_y = componentes[i + 1].yc + (componentes[i + 1].radio) * Math.Cos((componentes[i + 1].azte + 90) * Math.PI / 180);
                segundo_x = componentes[i + 1].xc + (componentes[i + 1].radio) * Math.Sin((componentes[i + 1].azts + 90) * Math.PI / 180);
                segundo_y = componentes[i + 1].yc + (componentes[i + 1].radio) * Math.Cos((componentes[i + 1].azts + 90) * Math.PI / 180);
            }

            componentes[i + 1].lista_puntos[0] = new Punto(new Point2d(primer_x, primer_y));
            componentes[i + 1].lista_puntos[componentes[i + 1].lista_puntos.Count - 1] = new Punto(new Point2d(segundo_x, segundo_y));
            //punto intermedio para recta para clotoide en s
            double d1_2 = Distancia(new Point2d(componentes[i].xc, componentes[i].yc), new Point2d(componentes[i + 1].xc, componentes[i + 1].yc));
            //vector unitario
            double v_x = (-componentes[i].xc + componentes[i + 1].xc) / d1_2;
            double v_y = (-componentes[i].yc + componentes[i + 1].yc) / d1_2;

            double d_p = componentes[i].radio + (d1_2 - componentes[i].radio - componentes[i + 1].radio) / 2;

            double x_pi = componentes[i].xc + v_x * d_p;
            double y_pi = componentes[i].yc + v_y * d_p;

            Punto p = Rellenar_centro(componentes[i + 1].xc, componentes[i + 1].yc, componentes[i].xc, componentes[i].yc, 1);
            //corrección
            double az;
            if (componentes[i].direccion == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                az = p.Az + 90;
            }
            else
            {
                az = p.Az - 90;
            }
            if (az > 360)
            {
                az = az - 360;
            }
            else
            {
                if (az < 0)
                {
                    az = az + 360;
                }
            }
            if (az > componentes[i].azts)
            {
                az = az - 0.1;
            }
            else
            {
                az = az + 0.1;
            }
            double seno = Math.Sin(az * Math.PI / 180);
            double coseno = Math.Cos(az * Math.PI / 180);
            /* double seno = Math.Sin(99 * Math.PI / 180);
             double coseno = Math.Cos(99 * Math.PI / 180);*/

            Logica.Componente c = new Logica.Componente(new Punto(new Point2d(x_pi - 1 * seno, y_pi - 1 * coseno)), 1);
            c.add(new Punto(new Point2d(x_pi + 1 * seno, y_pi + 1 * coseno)));
            c.creacion = 1;
            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            calculoPolilinea.Rellenar_Recta(c);
            componentes.Insert(i + 1, c);
            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[0].p.X, componentes[i + 1].lista_puntos[0].p.Y, 0);
            tadLayShare.puntos.Punto3d p3 = new tadLayShare.puntos.Punto3d(componentes[i + 1].lista_puntos[1].p.X, componentes[i + 1].lista_puntos[1].p.Y, 0);
            //calculoPolilinea.Dibujar_r(p2, p3, "pruebaa");
        }
        private double Girar_acercar(List<Punto> list, Logica.Componente c, double az)
        {
            double[] l = new double[2];
            l = ajuste_recta(list, 0).Item2;
            double r1 = Distancia_P_R(l[0], l[1], c.xc, c.yc);
            if (az == 0 || az == 180)
            {
                r1 = Math.Abs(c.xc - l[0]);
            }
            //double r1 = Distancia(list[0].p,new Point2d( c.xc, c.yc));

            List<Punto> l_p2 = Girar_Recta_2_Buena(list, 0.1, 0, az);
            l = ajuste_recta(l_p2, 0).Item2;
            double r2 = Distancia_P_R(l[0], l[1], c.xc, c.yc);
            //double r2 = Distancia(l_p2[0].p, new Point2d(c.xc, c.yc));


            if (r1 > r2)
            {
                return 0.01;
            }
            else
            {
                return -0.01;
            }

        }
        private double Girar_acercar_ini_fin(List<Punto> list, Logica.Componente c, double az, int p)
        {
            double[] l = new double[2];
            l = ajuste_recta(list, 0).Item2;
            double r1 = Distancia_P_R(l[0], l[1], c.xc, c.yc);
            if (az == 0 || az == 180)
            {
                r1 = Math.Abs(c.xc - l[0]);
            }
            //double r1 = Distancia(list[0].p,new Point2d( c.xc, c.yc));

            List<Punto> l_p2 = Girar_Recta_2_Buena(list, 0.1, p, az);
            l = ajuste_recta(l_p2, 0).Item2;
            double r2 = Distancia_P_R(l[0], l[1], c.xc, c.yc);
            //double r2 = Distancia(l_p2[0].p, new Point2d(c.xc, c.yc));


            if (r1 > r2)
            {
                return 0.1;
            }
            else
            {
                return -0.1;
            }

        }
        private double Girar_acercar_C_C(List<Punto> list, Logica.Componente c, double az)
        {
            double[] l = new double[2];
            l = ajuste_recta(list, 0).Item2;
            //double r1 = Distancia_P_R(l[0], l[1], c.xc, c.yc);
            double r1 = Distancia(list[0].p, new Point2d(c.xc, c.yc));

            List<Punto> l_p2 = Girar_Recta_2_Buena(list, 0.1, 0, az);
            l = ajuste_recta(l_p2, 0).Item2;
            //double r2 = Distancia_P_R(l[0], l[1], c.xc, c.yc);
            double r2 = Distancia(l_p2[0].p, new Point2d(c.xc, c.yc));


            if (r1 > r2)
            {
                if (r2 <= c.radio)
                {
                    return 0.001;
                }
                return 0.1;
            }
            else
            {
                if ((r1 - 0.1) <= c.radio)
                {
                    return -0.001;
                }
                return -0.1;
            }

        }
        private List<Punto> Girar_Recta_2_Buena(List<Punto> l, double giro, int p, double az)//el punto de giro es un extremo-->p==1 punto inicial//p==2 punto final
        {
            double xc_1 = l[0].p.X;
            double yc_1 = l[0].p.Y;
            double xc_2 = l[1].p.X;
            double yc_2 = l[1].p.Y;

            double x_p_1 = xc_1;
            double y_p_1 = yc_1;
            double x_p_2 = xc_2;
            double y_p_2 = yc_2;
            Point2d pm = new Point2d((xc_1 + xc_2) / 2, (yc_1 + yc_2) / 2);
            if (p == 1)
            {
                pm = new Point2d(xc_1, yc_1);
            }
            else if (p == 2)
            {
                pm = new Point2d(xc_2, yc_2);
            }
            double seno;
            double coseno;

            double dis = Distancia(new Point2d(xc_1, yc_1), new Point2d(xc_2, yc_2)) / 2;

            seno = Math.Sin((az + giro) * Math.PI / 180);
            coseno = Math.Cos((az + giro) * Math.PI / 180);

            x_p_2 = pm.X + dis * seno;
            y_p_2 = pm.Y + dis * coseno;

            x_p_1 = pm.X - dis * seno;
            y_p_1 = pm.Y - dis * coseno;


            tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(x_p_1, y_p_1, 0);
            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(x_p_2, y_p_2, 0);
            if (p == 3)
            {
                //Dibujar_r(p1, p2);
            }
            l = new List<Punto>();
            l.Add(new Punto(new Point2d(p1.coordenadaX, p1.coordenadaY)));
            l.Add(new Punto(new Point2d(p2.coordenadaX, p2.coordenadaY)));

            return l;
        }
        private void Girar_Recta(List<Logica.Componente> componentes, int i, double giro, double az)
        {
            double xc_1 = 0;
            double yc_1 = 0;
            double xc_2 = 0;
            double yc_2 = 0;
            if (i == 0)
            {
                xc_1 = componentes[i].lista_puntos[0].p.X;
                yc_1 = componentes[i].lista_puntos[0].p.Y;
                xc_2 = componentes[i].lista_puntos[1].p.X;
                yc_2 = componentes[i].lista_puntos[1].p.Y;
            }
            else
            {
                xc_1 = componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 2].p.X;
                yc_1 = componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 2].p.Y;
                xc_2 = componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1].p.X;
                yc_2 = componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1].p.Y;
            }

            Point2d pm = new Point2d((xc_1 + xc_2) / 2, (yc_1 + yc_2) / 2);
            if (i == 0)
            {
                pm = new Point2d(xc_1, yc_1);
            }
            else if (i == componentes.Count - 1)
            {
                pm = new Point2d(xc_2, yc_2);
            }
            double seno;
            double coseno;
            double x_p_2, y_p_2, x_p_1, y_p_1;

            double dis = Distancia(new Point2d(xc_1, yc_1), new Point2d(xc_2, yc_2)) / 2;

            seno = Math.Sin((az + giro) * Math.PI / 180);
            coseno = Math.Cos((az + giro) * Math.PI / 180);
            if (i == 0 || i == componentes.Count - 1)
            {
                dis = dis * 2;
            }

            x_p_2 = pm.X + dis * seno;
            y_p_2 = pm.Y + dis * coseno;

            x_p_1 = pm.X - dis * seno;
            y_p_1 = pm.Y - dis * coseno;
            if (i == 0)
            {
                x_p_1 = pm.X;
                y_p_1 = pm.Y;
            }
            else if (i == componentes.Count - 1)
            {
                x_p_2 = pm.X;
                y_p_2 = pm.Y;
            }

            /*x_p_2 = pm.X + Math.Abs(xc_2 - pm.X) * Math.Cos(-giro * Math.PI / 180) - Math.Abs(yc_2 - pm.Y) * Math.Sin(-giro * Math.PI / 180);
            y_p_2 = pm.Y + Math.Abs(xc_2 - pm.X) * Math.Sin(-giro * Math.PI / 180) + Math.Abs(yc_2 - pm.Y) * Math.Cos(-giro * Math.PI / 180);

            x_p_1 = pm.X - (pm.X - xc_1) * Math.Cos(-giro * Math.PI / 180) + (pm.Y - yc_1) * Math.Sin(-giro * Math.PI / 180);
            y_p_1 = pm.Y - (pm.X - xc_1) * Math.Sin(-giro * Math.PI / 180) - (pm.Y - yc_1) * Math.Cos(-giro * Math.PI / 180);*/
            tadLayShare.puntos.Punto3d p1 = new tadLayShare.puntos.Punto3d(x_p_1, y_p_1, 0);
            tadLayShare.puntos.Punto3d p2 = new tadLayShare.puntos.Punto3d(x_p_2, y_p_2, 0);

            //Dibujar_r(p1, p2);
            if (i == 0)
            {
                componentes[i].lista_puntos[1].Cambiar_X_Y(p2.coordenadaX, p2.coordenadaY);
                componentes[i].lista_puntos[0].Cambiar_X_Y(p1.coordenadaX, p1.coordenadaY);
                if (componentes[i].lista_puntos.Count > 2)
                {
                    componentes[i].lista_puntos.RemoveAt(2);
                }
            }
            else
            {
                componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 1].Cambiar_X_Y(p2.coordenadaX, p2.coordenadaY);
                componentes[i].lista_puntos[componentes[i].lista_puntos.Count - 2].Cambiar_X_Y(p1.coordenadaX, p1.coordenadaY);
                if (i == componentes.Count - 1)
                {
                    if (componentes[i].lista_puntos.Count > 2)
                    {
                        componentes[i].lista_puntos.RemoveAt(0);
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            id_componente = 0;
            Componentes = new List<Logica.Componente>();
            ComponentesTabla = new List<ComponenteTabla>();
            System.Data.DataTable dt = dsApp1.Tables["Componentes"];
            dt.Clear();
            dt.Columns["id"].AutoIncrementSeed = 0; // Reinicia desde 0
            dt.Columns["id"].AutoIncrementStep = 1; // Paso normal (de 1 en 1)
        }

        static EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva SentidoArco(double xc, double yc, double x1, double y1, double x2, double y2)
        {
            // Calcular el producto cruzado
            double cross = (x1 - xc) * (y2 - yc) - (y1 - yc) * (x2 - xc);

            if (cross > 0)
                return EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario;
            else if (cross < 0)
                return EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;
            else
                return EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.noValorado;
        }


        private EjeDeTrazado.componentes.Curva Curva_No_Paso(Logica.Componente c1, double radio, Logica.Componente c3, int var, double A)
        {
            //ec de la primera recta
            double a_x0 = c1.lista_puntos[0].p.X;
            double a_y0 = c1.lista_puntos[0].p.Y;
            double b_x1 = c1.lista_puntos[1].p.X;
            double b_y1 = c1.lista_puntos[1].p.Y;

            double a_1 = (a_y0 - b_y1) / (a_x0 - b_x1);
            double b_1 = -b_x1 * (a_y0 - b_y1) / (a_x0 - b_x1) + b_y1;

            double c_x0 = c3.lista_puntos[0].p.X;
            double c_y0 = c3.lista_puntos[0].p.Y;
            double d_x1 = c3.lista_puntos[1].p.X;
            double d_y1 = c3.lista_puntos[1].p.Y;

            double c_1 = (c_y0 - d_y1) / (c_x0 - d_x1);
            double d_1 = -d_x1 * ((c_y0 - d_y1) / (c_x0 - d_x1)) + d_y1;

            double i_x = (d_1 - b_1) / (a_1 - c_1);
            double i_y = a_1 * ((d_1 - b_1) / (a_1 - c_1)) + b_1;
            tadLayShare.puntos.Punto3d p_vertice = new tadLayShare.puntos.Punto3d(i_x, i_y, 0);
            tadLayShare.puntos.Punto3d[] puntosSing = addCurvaNoPaso(radio, c1.azr, c3.azr, p_vertice, getSentidoCurva(c1.azr, c3.azr), false, var, A);
            return new EjeDeTrazado.componentes.Curva(puntosSing[1], puntosSing[2], puntosSing[4], radio, 0, 0, getSentidoCurva(c1.azr, c3.azr));
            /*if ((c1.azr<90 && c3.azr<90))
            {
                return new Curva(puntosSing[1], puntosSing[2], puntosSing[4], radio, 0, 0, getSentidoCurva(c1.azr, c3.azr));
            }
            else
            {
                return new Curva(puntosSing[2], puntosSing[1], puntosSing[4], radio, 0, 0, getSentidoCurva(c1.azr, c3.azr));
            }*/

        }
        public tadLayShare.puntos.Punto3d[] addCurvaNoPaso(double iRc, double iAzimut1, double iAzimut2, tadLayShare.puntos.Punto3d iVertice,
            EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva sentG, bool iReducido, int var, double A)
        {

            double miA, miLe, miLmin;
            double miDelta = getDelta(iAzimut1, iAzimut2);
            double miDeltaRad = miDelta * Math.PI / 180;
            double miRc = iRc;

            //miA = Math.Pow((12 * Math.Pow(miRc, 3)), 0.25);
            miA = A;
            miLe = Math.Pow(miA, 2) / miRc;

            if (miRc < 2)
            {
                if (miRc < 0.1)
                {
                    var /= 100;
                }

                miLe = 0.5;
            }


            double miQe = miLe / (2 * miRc);
            double miYe = ((miQe / 3) - (Math.Pow(miQe, 3) / 42) + (Math.Pow(miQe, 5) / 1320) - (Math.Pow(miQe, 7) / 75600)) * miLe;
            double miDR = miYe - miRc * (1 - Math.Cos(miQe));
            double miXe = (1 - Math.Pow(miQe, 2) / 10 + Math.Pow(miQe, 4) / 216 - Math.Pow(miQe, 6) / 9360 + Math.Pow(miQe, 8) / 685440) * miLe;
            double miXM = miXe - miRc * Math.Sin(miQe);

            double miPhi = 180 - miDelta;
            double miTe = miXM + (miRc + miDR) * Math.Tan(miDelta * Math.PI / 180 / 2);
            double miEe = ((miRc + miDR) / Math.Abs(Math.Cos(miDelta / 2 * Math.PI / 180))) - iRc;
            double miPcx, miPcy, miPx1x, miPx1y, miPx2x, miPx2y;
            double miPcl1x, miPcl1y, miPc1x, miPc1y, miPc2x, miPc2y, miPcl2x, miPcl2y;

            double az1_m_90 = iAzimut1 - 90;
            double az1_M_90 = iAzimut1 + 90;
            double az1_m_180 = iAzimut1 - 180;
            double az1_M_180 = iAzimut1 + 180;

            double az2_m_90 = iAzimut2 - 90;
            double az2_M_90 = iAzimut2 + 90;
            double az2_m_180 = iAzimut2 - 180;
            double az2_M_180 = iAzimut2 + 180;

            if (az1_m_90 < 0)
            {
                az1_m_90 += 360;
            }
            if (az1_M_90 > 360)
            {
                az1_M_90 -= 360;
            }
            if (az1_m_180 < 0)
            {
                az1_m_180 += 360;
            }
            if (az1_M_180 > 360)
            {
                az1_M_180 -= 360;
            }

            if (az2_m_90 < 0)
            {
                az2_m_90 += 360;
            }
            if (az2_M_90 > 360)
            {
                az2_M_90 -= 360;
            }
            if (az2_m_180 < 0)
            {
                az2_m_180 += 360;
            }
            if (az2_M_180 > 360)
            {
                az2_M_180 -= 360;
            }

            if (sentG == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                miPcx = iVertice.coordenadaX + (miEe / var + iRc) * Math.Sin((iAzimut2 + miPhi / 2) * Math.PI / 180);
                miPcy = iVertice.coordenadaY + (miEe / var + iRc) * Math.Cos((iAzimut2 + miPhi / 2) * Math.PI / 180);
                miPx1x = miPcx + (miRc + miDR) * Math.Sin((iAzimut1 - 90) * Math.PI / 180);
                miPx1y = miPcy + (miRc + miDR) * Math.Cos((iAzimut1 - 90) * Math.PI / 180);
                miPx2x = miPcx + (miRc + miDR) * Math.Sin((iAzimut2 - 90) * Math.PI / 180);
                miPx2y = miPcy + (miRc + miDR) * Math.Cos((iAzimut2 - 90) * Math.PI / 180);

                miPcl1x = miPx1x + miXM * Math.Sin((iAzimut1 - 180) * Math.PI / 180);
                miPcl1y = miPx1y + miXM * Math.Cos((iAzimut1 - 180) * Math.PI / 180);

                miPc1x = miPcx + miRc * Math.Sin((iAzimut1 - 90 + miQe * 180 / Math.PI) * Math.PI / 180);
                miPc1y = miPcy + miRc * Math.Cos((iAzimut1 - 90 + miQe * 180 / Math.PI) * Math.PI / 180);

                miPc2x = miPcx + miRc * Math.Sin((iAzimut2 - 90 - miQe * 180 / Math.PI) * Math.PI / 180);
                miPc2y = miPcy + miRc * Math.Cos((iAzimut2 - 90 - miQe * 180 / Math.PI) * Math.PI / 180);

                miPcl2x = miPx2x + miXM * Math.Sin(iAzimut2 * Math.PI / 180);
                miPcl2y = miPx2y + miXM * Math.Cos(iAzimut2 * Math.PI / 180);

            }
            else
            {
                miPcx = iVertice.coordenadaX + (miEe / var + iRc) * Math.Sin((iAzimut2 - miPhi / 2) * Math.PI / 180);
                miPcy = iVertice.coordenadaY + (miEe / var + iRc) * Math.Cos((iAzimut2 - miPhi / 2) * Math.PI / 180);
                miPx1x = miPcx + (miRc + miDR) * Math.Sin((iAzimut1 + 90) * Math.PI / 180);
                miPx1y = miPcy + (miRc + miDR) * Math.Cos((iAzimut1 + 90) * Math.PI / 180);
                miPx2x = miPcx + (miRc + miDR) * Math.Sin((iAzimut2 + 90) * Math.PI / 180);
                miPx2y = miPcy + (miRc + miDR) * Math.Cos((iAzimut2 + 90) * Math.PI / 180);


                miPcl1x = miPx1x + miXM * Math.Sin((iAzimut1 - 180) * Math.PI / 180);
                miPcl1y = miPx1y + miXM * Math.Cos((iAzimut1 - 180) * Math.PI / 180);

                miPc1x = miPcx + miRc * Math.Sin((iAzimut1 + 90 - miQe * 180 / Math.PI) * Math.PI / 180);
                miPc1y = miPcy + miRc * Math.Cos((iAzimut1 + 90 - miQe * 180 / Math.PI) * Math.PI / 180);

                miPc2x = miPcx + miRc * Math.Sin((iAzimut2 + 90 + miQe * 180 / Math.PI) * Math.PI / 180);
                miPc2y = miPcy + miRc * Math.Cos((iAzimut2 + 90 + miQe * 180 / Math.PI) * Math.PI / 180);

                miPcl2x = miPx2x + miXM * Math.Sin(iAzimut2 * Math.PI / 180);
                miPcl2y = miPx2y + miXM * Math.Cos(iAzimut2 * Math.PI / 180);
            }

            tadLayShare.puntos.Punto3d miPuntoC = new tadLayShare.puntos.Punto3d(miPcx, miPcy, 0);
            tadLayShare.puntos.Punto3d miPunto1 = new tadLayShare.puntos.Punto3d(miPcl1x, miPcl1y, 0);
            tadLayShare.puntos.Punto3d miPunto2 = new tadLayShare.puntos.Punto3d(miPc1x, miPc1y, 0);
            tadLayShare.puntos.Punto3d miPunto3 = new tadLayShare.puntos.Punto3d(miPc2x, miPc2y, 0);
            tadLayShare.puntos.Punto3d miPunto4 = new tadLayShare.puntos.Punto3d(miPcl2x, miPcl2y, 0);

            tadLayShare.puntos.Punto3d[] puntosSing = new tadLayShare.puntos.Punto3d[5];
            puntosSing[0] = miPunto1;
            puntosSing[1] = miPunto2;
            puntosSing[2] = miPunto3;
            puntosSing[3] = miPunto4;
            puntosSing[4] = miPuntoC;

          

            return puntosSing;
        }
        public static double getDelta(double iAzimut1, double iAzimut2)
        {
            double miDelta;
            if (Math.Abs(iAzimut2 - iAzimut1) > 180)
            {
                miDelta = 360 - Math.Abs(iAzimut2 - iAzimut1);
            }
            else
            {
                miDelta = Math.Abs(iAzimut2 - iAzimut1);
            }
            return miDelta;
        }
        public EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva getSentidoCurva(double iAzSegAnt, double iAz)
        {
            EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva miSent;
            if ((iAzSegAnt >= 0) && (iAzSegAnt <= 180))
            {
                if (((iAzSegAnt - iAz) < 0) && (Math.Abs(iAzSegAnt - iAz) < 180))
                {
                    miSent = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;
                }
                else
                {
                    miSent = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario;
                }
            }
            else
            {
                if (((iAzSegAnt - iAz) > 0) && (Math.Abs(iAzSegAnt - iAz) < 180))
                {
                    miSent = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario;
                }
                else
                {
                    miSent = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;
                }
            }


            return miSent;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Desea hacer con el componente?\nSí = Actualizar\nNo = Eliminar\nCancelar = No hacer nada ",
                                      "Acción requerida",
                                      MessageBoxButtons.YesNoCancel,
                                      MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                ActualizarComponente(e.RowIndex);

            }
            else if (result == DialogResult.No)
            {
                EliminarComponente(e.RowIndex);
            }
            else
            {
                // Cancelado
                MessageBox.Show("Operación cancelada.");
            }

        }
        private void ActualizarComponente(int rowIndex)
        {
            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            System.Data.DataTable dt = dsApp1.Tables["Componentes"];
            foreach (DataRow fila in dt.Rows)
            {
                int idActual = Convert.ToInt32(fila["id_componente"]);
                if (idActual == rowIndex)
                {
                    EditarParametros form = new EditarParametros();
                    form.CargarDatos(Componentes[idActual]);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        Componentes[idActual] = form.c;
                        if (Componentes[idActual].Tipo == 1)
                        {
                            calculoPolilinea.Rellenar_Recta(Componentes[idActual]);
                            Modificar_fila_componentes(idActual, "recta", "", Componentes[idActual].lista_puntos[0].p.ToString(),
                                Componentes[idActual].lista_puntos[Componentes[idActual].lista_puntos.Count() - 1].p.ToString(),
                0, Componentes[idActual].azte, Componentes[idActual].azts, "", 0, 0, Componentes[idActual].azr, 0, 0, "");

                        }
                        else if (Componentes[idActual].Tipo == 2)
                        {
                            calculoPolilinea.Rellenar_Curva(Componentes[idActual]);
                            Modificar_fila_componentes(idActual, "curva", new Point3d(Componentes[idActual].xc, Componentes[idActual].yc, 0).ToString(),
                                   new Point3d(Componentes[idActual].lista_puntos[0].p.X, Componentes[idActual].lista_puntos[0].p.Y, 0).ToString(),
                                   new Point3d(Componentes[idActual].lista_puntos[Componentes[idActual].lista_puntos.Count() - 1].p.X,
                                   Componentes[idActual].lista_puntos[Componentes[idActual].lista_puntos.Count() - 1].p.Y, 0).ToString(),
                              Componentes[idActual].radio, Componentes[idActual].azte, Componentes[idActual].azts, "", 0, 0, Componentes[idActual].azr, 0, 0, "");
                        }
                    }
                }
            }
        }
        private void EliminarComponente(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < Componentes.Count)
            {
                // Eliminar de la lista Componentes
                Componentes.RemoveAt(rowIndex);

                // Eliminar del DataTable (asegúrate que el orden coincide con rowIndex)
                dsApp1.Tables["Componentes"].Rows.RemoveAt(rowIndex);

                // Eliminar del DataGridView si es necesario (aunque se sincroniza con el DataTable normalmente)
                //dataGridView1.Rows.RemoveAt(rowIndex);
                int cont = 0;
                foreach (DataRow fila in dsApp1.Tables["Componentes"].Rows)
                {
                    fila["id_componente"] = cont;
                    cont++;
                }

                MessageBox.Show("Componente eliminado correctamente.");
            }
            else
            {
                MessageBox.Show("Índice no válido para eliminar.");
            }
        }
    }
}
