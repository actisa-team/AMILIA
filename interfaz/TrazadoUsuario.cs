using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Logica;
using Logica.Componentes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaterialSkin.Controls;

namespace interfaz
{
    public partial class TrazadoUsuario : MaterialForm
    {
        public Point2d punto_ini;
        public Point2d punto_fin;
        public double radio;
        private List<ObjectId> entityIds = new List<ObjectId>();

        private List<Logica.Componente> Componentes = new List<Logica.Componente>();
        private int id_componente = 0;
        private int indiceEditado = -1;

        // Estructura para guardar ejes en memoria
        public class EjeEnMemoria
        {
            public string Nombre { get; set; }
            public List<Logica.Componente> Componentes { get; set; }
            public List<EjeDeTrazado.componentes.Componente> MComponentes { get; set; }
            public List<Autodesk.AutoCAD.Geometry.Point2d> Trazado { get; set; }
            public List<Autodesk.AutoCAD.Geometry.Point3d> Lista_original { get; set; }
        }
        public static List<EjeEnMemoria> EjesGuardados = new List<EjeEnMemoria>();

        // Último VistaComponentes usado en el cálculo, para acceder a sus resultados
        private VistaComponentes _lastVistaComponentes = null;


        public TrazadoUsuario()
        {
            InitializeComponent();
            b_radio.TextChanged += B_radio_TextChanged;
            IniciarComponentes();
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            ActualizarComboEjes();
        }

        private void IniciarComponentes()
        {
            dataGridView1.DataSource = dsApp1.Tables["Componentes"];

            // Ocultar columnas innecesarias para el usuario
            if (dataGridView1.Columns.Contains("id")) dataGridView1.Columns["id"].Visible = false;
            if (dataGridView1.Columns.Contains("centro")) dataGridView1.Columns["centro"].Visible = false;
            if (dataGridView1.Columns.Contains("az_inicial")) dataGridView1.Columns["az_inicial"].Visible = false;
            if (dataGridView1.Columns.Contains("az_final")) dataGridView1.Columns["az_final"].Visible = false;
            if (dataGridView1.Columns.Contains("sentido")) dataGridView1.Columns["sentido"].Visible = false;
            if (dataGridView1.Columns.Contains("pk_inicial")) dataGridView1.Columns["pk_inicial"].Visible = false;
            if (dataGridView1.Columns.Contains("pk_final")) dataGridView1.Columns["pk_final"].Visible = false;
            if (dataGridView1.Columns.Contains("a")) dataGridView1.Columns["a"].Visible = false;
            if (dataGridView1.Columns.Contains("le")) dataGridView1.Columns["le"].Visible = false;
            if (dataGridView1.Columns.Contains("tipo_clotoide")) dataGridView1.Columns["tipo_clotoide"].Visible = false;
            if (dataGridView1.Columns.Contains("tipo_clotoide")) dataGridView1.Columns["id_componente"].Visible = false;

            // Ajustar anchos automáticos opcionalmente
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Asignar iconos a los botones
            if (button2 != null) button2.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("delete");
            if (B_insertar != null) B_insertar.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("insert");
            if (AniadirEntidad != null) AniadirEntidad.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("add");
            if (b_cargarejes != null) b_cargarejes.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("cargar");
            if (b_borrareje != null) b_borrareje.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("delete");
            if (b_ejenuevo != null) b_ejenuevo.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("add");
            if (b_guardareje != null) b_guardareje.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("save");
            if (b_guardarejes != null) b_guardarejes.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("descargar");
            if (B_calcular != null) B_calcular.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("calular");
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < Componentes.Count)
            {
                indiceEditado = e.RowIndex;
                Logica.Componente c = Componentes[indiceEditado];

                // Cargar el EntityId del componente para que DibujarEntidad borre la entidad anterior
                if (!string.IsNullOrEmpty(c.EntityId))
                {
                    try
                    {
                        Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                        Database db = doc.Database;
                        Handle handle = new Handle(long.Parse(c.EntityId));
                        if (db.TryGetObjectId(handle, out ObjectId objId))
                        {
                            entityIds.Add(objId);
                        }
                    }
                    catch
                    {
                        entityIds.Clear();
                    }
                }
                else
                {
                    entityIds.Clear();
                }

                // Cargar datos en variables y controles
                if (c.Tipo == 1) // Recta
                {
                    punto_ini = new Point2d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y);
                    punto_fin = new Point2d(c.lista_puntos[c.lista_puntos.Count - 1].p.X, c.lista_puntos[c.lista_puntos.Count - 1].p.Y);
                    radio = 0;
                    b_radio.Text = "";
                }
                else if (c.Tipo == 2) // Curva
                {
                    punto_ini = new Point2d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y);
                    punto_fin = new Point2d(c.lista_puntos[c.lista_puntos.Count - 1].p.X, c.lista_puntos[c.lista_puntos.Count - 1].p.Y);
                    radio = c.radio;

                    // Ajustar signo del radio para UI si es necesario (según dirección centro)
                    // Calculamos si el centro está a la izquierda o derecha para poner signo negativo
                    // Pero Componente guarda radio positivo.
                    // Podemos verificar la posición del centro almacenado vs calculado.
                    // Por simplicidad, cargamos el radio absoluto. El usuario puede cambiar signo.
                    // O mejor: intentamos deducir el signo.

                    Vector2d v = punto_fin - punto_ini;
                    double dist = punto_ini.GetDistanceTo(punto_fin);
                    double h = Math.Sqrt(radio * radio - (dist / 2) * (dist / 2));
                    Vector2d normal = new Vector2d(-v.Y, v.X).GetNormal();
                    Point2d mid = punto_ini + v / 2.0;

                    // Centro hipotético positivo
                    Point2d cPos = mid + normal * h;

                    // Si el centro real (c.xc, c.yc) dista menos de cPos, es positivo. Si no, negativo.
                    if (new Point2d(c.xc, c.yc).GetDistanceTo(cPos) < 1.0) // margen error
                        b_radio.Texts = radio.ToString();
                    else
                        b_radio.Texts = (-radio).ToString();
                }

                P_ini_x.Texts = punto_ini.X.ToString();
                P_ini_y.Texts = punto_ini.Y.ToString();
                P_fin_x.Texts = punto_fin.X.ToString();
                P_fin_y.Texts = punto_fin.Y.ToString();

                AniadirEntidad.Text = "Guardar Cambios";
                DibujarEntidad("Entidad_Trazado_Usuario", 6, LineWeight.LineWeight030);
            }
        }

        private void B_radio_TextChanged(object sender, EventArgs e)
        {
            DibujarEntidad();
        }

        private void AniadirEntidad_Click(object sender, EventArgs e)
        {
            // Validar
            if (punto_ini == null || punto_fin == null) return;
            if (punto_ini.IsEqualTo(new Point2d(0, 0)) || punto_fin.IsEqualTo(new Point2d(0, 0))) return;

            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();

            // Si estamos editando, usamos el objeto existente o reemplazamos en índice
            Logica.Componente c;
            if (indiceEditado != -1)
            {
                c = Componentes[indiceEditado];
                // Limpiamos sus datos anteriores relevantes
            }
            else
            {
                c = new Logica.Componente();
            }

            double valRadio = 0;
            bool tieneRadio = double.TryParse(b_radio.Texts, out valRadio) && valRadio != 0;

            if (!tieneRadio)
            {
                // LINEA
                c.lista_puntos = new List<Punto>();
                Punto p1 = new Punto(new Point2d(punto_ini.X, punto_ini.Y));
                Punto p2 = new Punto(new Point2d(punto_fin.X, punto_fin.Y));
                c.lista_puntos.Add(p1);
                c.lista_puntos.Add(p2);
                calculoPolilinea.Rellenar_Recta(c);
                c.Tipo = 1;

                if (indiceEditado == -1)
                {
                    // Guardar el entityId en el componente (el último creado en DibujarEntidad)
                    if (entityIds.Count > 0)
                        c.EntityId = entityIds[entityIds.Count - 1].Handle.Value.ToString();

                    Componentes.Add(c);
                    Add_fila_componentes("recta", "", c.lista_puntos[0].p.ToString(), c.lista_puntos[c.lista_puntos.Count() - 1].p.ToString(),
                                    0, c.azte, c.azts, "", 0, 0, c.azr, 0, 0, "", id_componente);
                    id_componente++;

                    // Resetear entityIds
                    entityIds.Clear();
                }
                else
                {
                    // Modificar existente
                    // Guardar el nuevo entityId
                    if (entityIds.Count > 0)
                        c.EntityId = entityIds[entityIds.Count - 1].Handle.Value.ToString();

                    // Obtenemos id_componente de la tabla usando el indice
                    DataRow row = dsApp1.Tables["Componentes"].Rows[indiceEditado];
                    int idEnTabla = Convert.ToInt32(row["id_componente"]);

                    Modificar_fila_componentes(idEnTabla, "recta", "", c.lista_puntos[0].p.ToString(), c.lista_puntos[c.lista_puntos.Count() - 1].p.ToString(),
                                    0, c.azte, c.azts, "", 0, 0, c.azr, 0, 0, "");

                    // Resetear modo edicion
                    indiceEditado = -1;
                    AniadirEntidad.Text = "Añadir Entidad";
                    entityIds.Clear(); // Evitar que LimpiarDatos borre la entidad guardada
                    button1_Click(null, null); // Limpiar form
                }
            }
            else
            {
                // CURVA (Arco)
                double dist = punto_ini.GetDistanceTo(punto_fin);
                double absRadio = Math.Abs(valRadio);

                if (dist <= 2 * absRadio)
                {
                    // Calcular centro y puntos
                    int sign = Math.Sign(valRadio);
                    Vector2d v = punto_fin - punto_ini;
                    Point2d mid = punto_ini + v / 2.0;
                    double h = Math.Sqrt(absRadio * absRadio - (dist / 2) * (dist / 2));
                    Vector2d normal = new Vector2d(-v.Y, v.X).GetNormal();
                    Point2d center2d = mid + normal * h * sign;

                    c.radio = absRadio;
                    c.xc = center2d.X;
                    c.yc = center2d.Y;

                    // Determinar el sentido real (horario/antihorario) mediante el producto cruzado
                    // entre el vector (ini - centro) y (fin - centro)
                    double crossProduct = (punto_ini.X - c.xc) * (punto_fin.Y - c.yc) - (punto_ini.Y - c.yc) * (punto_fin.X - c.xc);
                    c.direccion = crossProduct > 0
                        ? EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario
                        : EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario;

                    c.lista_puntos = new List<Punto>();

                    Punto p1 = new Punto(new Point2d(punto_ini.X, punto_ini.Y));
                    Punto p3 = new Punto(new Point2d(punto_fin.X, punto_fin.Y));

                    Punto p2 = calculoPolilinea.Crear_Curva_2Puntos_Modificada(p1, p3, c.xc, c.yc, c.radio, c);

                    c.lista_puntos.Add(p1);
                    c.lista_puntos.Add(p2);
                    c.lista_puntos.Add(p3);
                    c.Tipo = 2;

                    calculoPolilinea.Rellenar_Curva(c);

                    if (indiceEditado == -1)
                    {
                        // Guardar el entityId en el componente
                        if (entityIds.Count > 0)
                            c.EntityId = entityIds[entityIds.Count - 1].Handle.Value.ToString();

                        Componentes.Add(c);
                        Add_fila_componentes("curva", new Point3d(c.xc, c.yc, 0).ToString(),
                                       new Point3d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y, 0).ToString(),
                                       new Point3d(c.lista_puntos[c.lista_puntos.Count() - 1].p.X, c.lista_puntos[c.lista_puntos.Count() - 1].p.Y, 0).ToString(),
                                  c.radio, c.azte, c.azts, c.direccion.ToString(), 0, 0, c.azr, 0, 0, "", id_componente);
                        id_componente++;

                        // Resetear entityIds
                        entityIds.Clear();
                    }
                    else
                    {
                        // Guardar el nuevo entityId
                        if (entityIds.Count > 0)
                            c.EntityId = entityIds[entityIds.Count - 1].Handle.Value.ToString();

                        DataRow row = dsApp1.Tables["Componentes"].Rows[indiceEditado];
                        int idEnTabla = Convert.ToInt32(row["id_componente"]);

                        Modificar_fila_componentes(idEnTabla, "curva", new Point3d(c.xc, c.yc, 0).ToString(),
                                        new Point3d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y, 0).ToString(),
                                        new Point3d(c.lista_puntos[c.lista_puntos.Count() - 1].p.X, c.lista_puntos[c.lista_puntos.Count() - 1].p.Y, 0).ToString(),
                                   c.radio, c.azte, c.azts, c.direccion.ToString(), 0, 0, c.azr, 0, 0, "");

                        indiceEditado = -1;
                        AniadirEntidad.Text = "Añadir Entidad";
                        entityIds.Clear(); // Evitar que LimpiarDatos borre la entidad guardada
                        button1_Click(null, null); // Limpiar
                    }
                }
                else
                {
                    MessageBox.Show("Radio insuficiente para la distancia entre puntos.");
                }
            }
            LimpiarDatos();
        }

        private void Modificar_fila_componentes(int id_componente, string tipo, string centro, string punto_inicial, string punto_final, double radio,
            double az_inicial, double az_final, string sentido, double pk_inicial, double pk_final, double azimut, double a,
            double le, string tipo_componente)
        {
            System.Data.DataTable dt = dsApp1.Tables["Componentes"];
            DataRow[] filas = dt.Select("id_componente = " + id_componente);

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

        private void Seleccionar_Primer_Punto(object sender, EventArgs e)
        {
            this.Hide();
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            PromptPointResult pPr = ed.GetPoint("\nSeleccione el primer punto: ");
            if (pPr.Status == PromptStatus.OK)
            {
                punto_ini = new Point2d(pPr.Value.X, pPr.Value.Y);
                P_ini_x.Texts = punto_ini.X.ToString();
                P_ini_y.Texts = punto_ini.Y.ToString();
                DibujarEntidad();
            }
            this.Show();
        }

        private void Seleccionar_Segundo_Punto(object sender, EventArgs e)
        {
            this.Hide();
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            PromptPointResult pPr = ed.GetPoint("\nSeleccione el segundo punto: ");
            if (pPr.Status == PromptStatus.OK)
            {
                punto_fin = new Point2d(pPr.Value.X, pPr.Value.Y);
                P_fin_x.Texts = punto_fin.X.ToString();
                P_fin_y.Texts = punto_fin.Y.ToString();
                DibujarEntidad();
            }
            this.Show();
        }
        private void LimpiarDatos()
        {
            // Borrar entidades de AutoCAD si existen
            if (entityIds.Count > 0)
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                using (doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        foreach (ObjectId id in entityIds)
                        {
                            if (id != ObjectId.Null && !id.IsErased)
                            {
                                try
                                {
                                    Entity ent = (Entity)tr.GetObject(id, OpenMode.ForWrite);
                                    ent.Erase();
                                }
                                catch { }
                            }
                        }
                        tr.Commit();
                    }
                }
            }

            // Resetear modo edición
            indiceEditado = -1;
            AniadirEntidad.Text = "Añadir Entidad";

            // Limpiar la lista de entidades
            entityIds.Clear();

            // Resetear variables (invalidar puntos)
            punto_ini = new Point2d(0, 0);
            punto_fin = new Point2d(0, 0);
            radio = 0;

            // Limpiar controles
            P_ini_x.Texts = "";
            P_ini_y.Texts = "";
            P_fin_x.Texts = "";
            P_fin_y.Texts = "";
            b_radio.Texts = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            LimpiarDatos();
        }

        private void DibujarEntidad(string capa = "0", int colorIndex = 7, LineWeight grosor = LineWeight.ByLineWeightDefault)
        {
            // Validar que tengamos al menos el punto inicial
            if (punto_ini == null || punto_ini.IsEqualTo(new Point2d(0, 0))) return;

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (doc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                    // Borrar entidades anteriores si existen
                    if (entityIds.Count > 0)
                    {
                        foreach (ObjectId id in entityIds)
                        {
                            if (id != ObjectId.Null && !id.IsErased)
                            {
                                try
                                {
                                    Entity ent = (Entity)tr.GetObject(id, OpenMode.ForWrite);
                                    ent.Erase();
                                }
                                catch { }
                            }
                        }
                        entityIds.Clear();
                    }

                    // Crear la capa si no existe
                    if (!string.IsNullOrEmpty(capa))
                    {
                        LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                        if (!lt.Has(capa))
                        {
                            lt.UpgradeOpen();
                            LayerTableRecord ltr = new LayerTableRecord();
                            ltr.Name = capa;
                            lt.Add(ltr);
                            tr.AddNewlyCreatedDBObject(ltr, true);
                        }
                    }

                    List<Entity> nuevasEntidades = new List<Entity>();

                    // Si solo tenemos el primer punto, dibujamos un marcador (circulo amarillo + cruz)
                    if (punto_fin == null || punto_fin.IsEqualTo(new Point2d(0, 0)))
                    {
                        Point3d centro = new Point3d(punto_ini.X, punto_ini.Y, 0);

                        // Circulo
                        Circle circulo = new Circle();
                        circulo.Center = centro;
                        circulo.Radius = 5.0;
                        circulo.ColorIndex = 2; // Amarillo
                        nuevasEntidades.Add(circulo);

                        // Cruz (Lineas)
                        double size = 7.0;
                        Line l1 = new Line(new Point3d(centro.X - size, centro.Y, 0), new Point3d(centro.X + size, centro.Y, 0));
                        Line l2 = new Line(new Point3d(centro.X, centro.Y - size, 0), new Point3d(centro.X, centro.Y + size, 0));
                        l1.ColorIndex = 2;
                        l2.ColorIndex = 2;
                        nuevasEntidades.Add(l1);
                        nuevasEntidades.Add(l2);
                    }
                    else
                    {
                        // Dibujar Linea o Arco
                        double valRadio = 0;
                        bool tieneRadio = double.TryParse(b_radio.Texts, out valRadio) && valRadio != 0;

                        Point3d p1 = new Point3d(punto_ini.X, punto_ini.Y, 0);
                        Point3d p2 = new Point3d(punto_fin.X, punto_fin.Y, 0);

                        if (!tieneRadio)
                        {
                            // Dibujar Linea
                            nuevasEntidades.Add(new Line(p1, p2));
                        }
                        else
                        {
                            // Dibujar Arco
                            Entity entPreview = null;
                            // Calculo basico de arco dados 2 puntos y radio (Solucion "Menor")
                            try
                            {
                                double dist = punto_ini.GetDistanceTo(punto_fin);
                                double absRadio = Math.Abs(valRadio);

                                if (dist <= 2 * absRadio)
                                {
                                    // Calcular centro
                                    int sign = Math.Sign(valRadio);
                                    Vector2d v = punto_fin - punto_ini;
                                    Point2d mid = punto_ini + v / 2.0;
                                    double h = Math.Sqrt(absRadio * absRadio - (dist / 2) * (dist / 2));
                                    Vector2d normal = new Vector2d(-v.Y, v.X).GetNormal();
                                    Point2d center2d = mid + normal * h * sign;

                                    // Angulos
                                    Vector2d vCentP1 = punto_ini - center2d;
                                    Vector2d vCentP2 = punto_fin - center2d;
                                    double ang1 = vCentP1.Angle;
                                    double ang2 = vCentP2.Angle;

                                    // Asegurar arco menor... (simplificado: Arc siempre dibuja CCW)
                                    // Si valRadio es positivo, queremos un arco "a la izquierda" (o derecha, segun convenio).
                                    // Simplemente construiremos el arco. Si el arco resultante es el "mayor" (> 180), invertimos.
                                    // OJO: La direccion del centro ya define la curvatura. Solo necesitamos asegurar que se dibuje el segmento corto.

                                    bool clockwise = false; // AutoCAD Arcs draw CCW normally.

                                    // Para simplificar visualizacion rapida:
                                    entPreview = new Arc(new Point3d(center2d.X, center2d.Y, 0), absRadio, ang1, ang2);

                                    // Verificar si es el arco corto
                                    double diff = ang2 - ang1;
                                    while (diff < 0) diff += 2 * Math.PI;
                                    if (diff > Math.PI)
                                    {
                                        // Es el arco largo. Intercambiamos angulos para pintar el corto.
                                        entPreview = new Arc(new Point3d(center2d.X, center2d.Y, 0), absRadio, ang2, ang1);
                                    }
                                }
                                else
                                {
                                    // Radio insuficiente -> Linea
                                    entPreview = new Line(p1, p2);
                                }
                            }
                            catch
                            {
                                entPreview = new Line(p1, p2); // Fallback
                            }

                            if (entPreview != null) nuevasEntidades.Add(entPreview);
                        }
                    }

                    foreach (Entity ent in nuevasEntidades)
                    {
                        if (!string.IsNullOrEmpty(capa))
                        {
                            ent.Layer = capa;
                        }
                        if (punto_fin != null && !punto_fin.IsEqualTo(new Point2d(0, 0)))
                        {
                            ent.ColorIndex = colorIndex;
                            ent.LineWeight = grosor;
                        }
                        ObjectId id = btr.AppendEntity(ent);
                        tr.AddNewlyCreatedDBObject(ent, true);
                        entityIds.Add(id);
                    }

                    tr.Commit();
                }
            }
            ed.UpdateScreen();
        }
        private void B_insertar_Click(object sender, EventArgs e)
        {
            // Validar que tengamos puntos
            if (punto_ini == null || punto_fin == null)
            {
                MessageBox.Show("Debe definir los puntos inicial y final.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (punto_ini.IsEqualTo(new Point2d(0, 0)) || punto_fin.IsEqualTo(new Point2d(0, 0)))
            {
                MessageBox.Show("Los puntos no pueden ser (0,0).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que la posición de inserción sea válida (usando la fila seleccionada en el grid)
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar una fila en la tabla como punto de inserción.",
                    "Punto de inserción requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int posicionInsercion = dataGridView1.CurrentRow.Index;

            // Si el grid está vacío o algo falla, aseguramos que sea una posición válida
            if (posicionInsercion < 0) posicionInsercion = 0;
            if (posicionInsercion > Componentes.Count) posicionInsercion = Componentes.Count;


            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            Logica.Componente c = new Logica.Componente();

            double valRadio = 0;
            bool tieneRadio = double.TryParse(b_radio.Texts, out valRadio) && valRadio != 0;

            if (!tieneRadio)
            {
                // LINEA
                c.lista_puntos = new List<Punto>();
                Punto p1 = new Punto(new Point2d(punto_ini.X, punto_ini.Y));
                Punto p2 = new Punto(new Point2d(punto_fin.X, punto_fin.Y));
                c.lista_puntos.Add(p1);
                c.lista_puntos.Add(p2);
                calculoPolilinea.Rellenar_Recta(c);
                c.Tipo = 1;

                // Guardar el entityId en el componente
                if (entityIds.Count > 0)
                    c.EntityId = entityIds[entityIds.Count - 1].Handle.Value.ToString();

                // Insertar en la posición especificada
                Componentes.Insert(posicionInsercion, c);

                Add_fila_componentes("recta", "", c.lista_puntos[0].p.ToString(),
                    c.lista_puntos[c.lista_puntos.Count() - 1].p.ToString(),
                    0, c.azte, c.azts, "", 0, 0, c.azr, 0, 0, "", posicionInsercion, true);

                // Resetear entityIds
                entityIds.Clear();
            }
            else
            {
                // CURVA (Arco)
                double dist = punto_ini.GetDistanceTo(punto_fin);
                double absRadio = Math.Abs(valRadio);

                if (dist <= 2 * absRadio)
                {
                    // Calcular centro y puntos
                    int sign = Math.Sign(valRadio);
                    Vector2d v = punto_fin - punto_ini;
                    Point2d mid = punto_ini + v / 2.0;
                    double h = Math.Sqrt(absRadio * absRadio - (dist / 2) * (dist / 2));
                    Vector2d normal = new Vector2d(-v.Y, v.X).GetNormal();
                    Point2d center2d = mid + normal * h * sign;

                    c.radio = absRadio;
                    c.xc = center2d.X;
                    c.yc = center2d.Y;
                    c.lista_puntos = new List<Punto>();

                    Punto p1 = new Punto(new Point2d(punto_ini.X, punto_ini.Y));
                    Punto p3 = new Punto(new Point2d(punto_fin.X, punto_fin.Y));

                    Punto p2 = calculoPolilinea.Crear_Curva_2Puntos_Modificada(p1, p3, c.xc, c.yc, c.radio, c);

                    c.lista_puntos.Add(p1);
                    c.lista_puntos.Add(p2);
                    c.lista_puntos.Add(p3);
                    c.Tipo = 2;

                    calculoPolilinea.Rellenar_Curva(c);

                    // Guardar el entityId en el componente
                    if (entityIds.Count > 0)
                        c.EntityId = entityIds[entityIds.Count - 1].Handle.Value.ToString();

                    // Insertar en la posición especificada
                    Componentes.Insert(posicionInsercion, c);

                    Add_fila_componentes("curva", new Point3d(c.xc, c.yc, 0).ToString(),
                        new Point3d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y, 0).ToString(),
                        new Point3d(c.lista_puntos[c.lista_puntos.Count() - 1].p.X,
                            c.lista_puntos[c.lista_puntos.Count() - 1].p.Y, 0).ToString(),
                        c.radio, c.azte, c.azts, "", 0, 0, c.azr, 0, 0, "", posicionInsercion, true);

                    // Resetear entityIds
                    entityIds.Clear();
                }
                else
                {
                    MessageBox.Show("Radio insuficiente para la distancia entre puntos.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show($"Entidad insertada en la posición {posicionInsercion}.",
                "Inserción exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Limpiar el formulario
            button1_Click(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Validar que haya componentes para borrar
            if (Componentes.Count == 0)
            {
                MessageBox.Show("No hay entidades para borrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el índice a borrar de la selección del grid
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar una fila en la tabla para borrar.",
                    "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int indiceBorrar = dataGridView1.CurrentRow.Index + 1; // 1-based para mantener compatibilidad con el resto del método


            // Confirmar antes de borrar
            DialogResult resultado = MessageBox.Show(
                $"¿Está seguro de que desea borrar la entidad en la posición {indiceBorrar}?",
                "Confirmar borrado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // Borrar la entidad de AutoCAD si existe
                Logica.Componente componenteBorrar = Componentes[indiceBorrar - 1];
                if (!string.IsNullOrEmpty(componenteBorrar.EntityId))
                {
                    try
                    {
                        Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                        Database db = doc.Database;

                        using (doc.LockDocument())
                        {
                            using (Transaction tr = db.TransactionManager.StartTransaction())
                            {
                                long handleVal = long.Parse(componenteBorrar.EntityId);
                                Handle handle = new Handle(handleVal);

                                if (db.TryGetObjectId(handle, out ObjectId objId))
                                {
                                    if (!objId.IsNull && !objId.IsErased)
                                    {
                                        Entity ent = (Entity)tr.GetObject(objId, OpenMode.ForWrite);
                                        ent.Erase();
                                    }
                                }

                                tr.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"No se pudo borrar la entidad de AutoCAD: {ex.Message}",
                            "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Borrar de la lista de componentes
                Componentes.RemoveAt(indiceBorrar - 1);

                // Borrar del DataGrid (tabla)
                System.Data.DataTable dt = dsApp1.Tables["Componentes"];

                // Buscar la fila con el id_componente correspondiente al índice
                DataRow[] filasABorrar = dt.Select($"id_componente = {indiceBorrar - 1}");

                if (filasABorrar.Length > 0)
                {
                    dt.Rows.Remove(filasABorrar[0]);
                }

                // Actualizar los id_componente de las filas posteriores (decrementar en 1)
                foreach (DataRow fila in dt.Rows)
                {
                    int idActual = Convert.ToInt32(fila["id_componente"]);
                    if (idActual > indiceBorrar - 1)
                    {
                        fila["id_componente"] = idActual - 1;
                    }
                }

                MessageBox.Show($"Entidad en la posición {indiceBorrar} borrada exitosamente.",
                    "Borrado exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Si estábamos editando esta entidad, resetear
                if (indiceEditado == indiceBorrar - 1)
                {
                    button1_Click(null, null);
                }
                // Si estábamos editando una entidad posterior, actualizar el índice
                else if (indiceEditado > indiceBorrar - 1)
                {
                    indiceEditado--;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al borrar la entidad: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void B_calcular_Click(object sender, EventArgs e)
        {
            // Validar que el nombre del eje no esté vacío
            if (string.IsNullOrWhiteSpace(B_nombre.Text))
            {
                MessageBox.Show("Debe especificar un nombre para el eje antes de calcular.\n" +
                               "Ingrese el nombre en el campo correspondiente.",
                    "Nombre de eje requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                B_nombre.Focus();
                return;
            }

            // Validar que haya al menos 1 componentes
            if (Componentes.Count < 1)
            {
                MessageBox.Show("Debe tener al menos 1 entidades para calcular el trazado.\n" +
                               "Agregue más líneas o arcos antes de calcular.",
                    "Componentes insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string nombreEje = B_nombre.Text.Trim();

                // Borrar el trazado anterior si existe
                BorrarTrazadoAnterior(nombreEje);

                // Crear la vista de componentes para usar su método Crear_Trazado
                VistaComponentes vistaComponentes = new VistaComponentes();

                // Parámetros para el cálculo
                double gran_r = 1000.0; // Radio grande para verificaciones (puede ser configurable en el futuro)

                // Ejecutar el cálculo del trazado con clotoides
                MessageBox.Show($"Calculando trazado '{nombreEje}' con {Componentes.Count} componentes...\n",
                    "Calculando", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Componentes = vistaComponentes.Crear_Trazado(gran_r, Componentes, nombreEje);
                _lastVistaComponentes = vistaComponentes;
                ActualizarDataGridDesdeComponentes(Componentes);

                // Guardar automáticamente en memoria con la estructura completa
                GuardarEjeActualSilencioso(nombreEje);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular el trazado:\n\n{ex.Message}\n\n" +
                               $"Detalles técnicos:\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BorrarTrazadoAnterior(string nombreEje)
        {
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;

                using (doc.LockDocument())
                {
                    Editor ed = doc.Editor;
                    // Crear un filtro para seleccionar todas las entidades en capas que empiecen por "nombreEje-"
                    // Esto incluye -Trazado, -Rotulacion-*, -Linea_Rotulacion_*, etc.
                    TypedValue[] filter = new TypedValue[] {
                        new TypedValue((int)DxfCode.LayerName, nombreEje + "-*")
                    };
                    SelectionFilter sf = new SelectionFilter(filter);
                    PromptSelectionResult psr = ed.SelectAll(sf);

                    if (psr.Status == PromptStatus.OK)
                    {
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            foreach (ObjectId objId in psr.Value.GetObjectIds())
                            {
                                if (!objId.IsErased)
                                {
                                    Entity ent = (Entity)tr.GetObject(objId, OpenMode.ForWrite);
                                    ent.Erase();
                                }
                            }
                            tr.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Si falla el borrado, solo mostrar advertencia pero continuar
                MessageBox.Show($"Advertencia: No se pudo borrar el trazado anterior.\n{ex.Message}",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void b_ejenuevo_Click(object sender, EventArgs e)
        {
            // Preguntar si desea guardar el eje actual
            DialogResult result = MessageBox.Show(
                "¿Desea guardar el eje actual en memoria antes de crear uno nuevo?",
                "Nuevo Eje",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            if (result == DialogResult.Yes)
            {
                if (!GuardarEjeActual()) return; // Si falla el guardado (por ejemplo, cancela sobrescritura), no seguimos
            }

            // Reiniciar todos los datos para un nuevo trazado
            Componentes.Clear();
            dsApp1.Tables["Componentes"].Clear();
            B_nombre.Clear();
            id_componente = 0;
            indiceEditado = -1;
            AniadirEntidad.Text = "Añadir Entidad";

            // Limpiar puntos y entidades temporales de AutoCAD
            LimpiarDatos();

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Eje guardado y formulario reiniciado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Formulario reiniciado sin guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>Guarda el eje en memoria sin preguntar nombre (lo usa B_calcular tras calcular).</summary>
        private void GuardarEjeActualSilencioso(string nombreEje)
        {
            var mcomp = _lastVistaComponentes?.LastMComponentes;
            var trazado = _lastVistaComponentes?.LastTrazado;

            var existente = EjesGuardados.FirstOrDefault(x => x.Nombre.Equals(nombreEje, StringComparison.OrdinalIgnoreCase));
            if (existente != null)
            {
                existente.Componentes = new List<Logica.Componente>(Componentes);
                existente.MComponentes = mcomp;
                existente.Trazado = trazado;
            }
            else
            {
                EjesGuardados.Add(new EjeEnMemoria
                {
                    Nombre = nombreEje,
                    Componentes = new List<Logica.Componente>(Componentes),
                    MComponentes = mcomp,
                    Trazado = trazado
                });
            }
            ActualizarComboEjes();
        }

        private bool GuardarEjeActual()
        {
            // Validar si hay un nombre
            string nombreEje = B_nombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombreEje))
            {
                MessageBox.Show("Debe especificar un nombre para el eje antes de guardar.",
                    "Nombre requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                B_nombre.Focus();
                return false;
            }

            var mcomp = _lastVistaComponentes?.LastMComponentes;
            var trazado = _lastVistaComponentes?.LastTrazado;

            // Comprobar si ya existe un eje con el mismo nombre
            var ejeExistente = EjesGuardados.FirstOrDefault(x => x.Nombre.Equals(nombreEje, StringComparison.OrdinalIgnoreCase));
            if (ejeExistente != null)
            {
                DialogResult overwriteResult = MessageBox.Show(
                    $"Ya existe un eje guardado con el nombre '{nombreEje}'. ¿Desea sobrescribirlo?",
                    "Confirmar sobrescritura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (overwriteResult == DialogResult.No)
                {
                    return false;
                }

                // Si dijo que sí, actualizamos todos los campos
                ejeExistente.Componentes = new List<Logica.Componente>(Componentes);
                ejeExistente.MComponentes = mcomp;
                ejeExistente.Trazado = trazado;
            }
            else
            {
                EjesGuardados.Add(new EjeEnMemoria
                {
                    Nombre = nombreEje,
                    Componentes = new List<Logica.Componente>(Componentes),
                    MComponentes = mcomp,
                    Trazado = trazado
                });
            }
            ActualizarComboEjes();
            return true;
        }

        private void ActualizarComboEjes()
        {
            // Desconectar el evento para evitar que Items.Add dispare SelectedIndexChanged
            comboBox1.SelectedIndexChanged -= ComboBox1_SelectedIndexChanged;
            comboBox1.Items.Clear();

            int maxWidth = comboBox1.Width;
            using (Graphics g = comboBox1.CreateGraphics())
            {
                foreach (var eje in EjesGuardados)
                {
                    comboBox1.Items.Add(eje.Nombre);
                    // Calcular el ancho del texto para ajustar el desplegable
                    int textWidth = (int)g.MeasureString(eje.Nombre, comboBox1.Font).Width;
                    if (textWidth > maxWidth)
                        maxWidth = textWidth;
                }
            }

            // Añadir margen para el scrollbar si es necesario
            if (comboBox1.Items.Count > comboBox1.MaxDropDownItems)
                maxWidth += SystemInformation.VerticalScrollBarWidth;

            comboBox1.DropDownWidth = maxWidth;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;

            string nombreSeleccionado = comboBox1.SelectedItem.ToString();
            var eje = EjesGuardados.FirstOrDefault(x => x.Nombre == nombreSeleccionado);

            if (eje != null)
            {
                // Crear nueva lista independiente para no afectar al eje guardado
                Componentes = new List<Logica.Componente>();
                dsApp1.Tables["Componentes"].Clear();
                LimpiarDatos();

                // Cargar seleccionado
                B_nombre.Text = eje.Nombre;
                foreach (var c in eje.Componentes)
                {
                    Componentes.Add(c);
                    int currentId = dsApp1.Tables["Componentes"].Rows.Count;

                    // Re-añadir al grid
                    if (c.Tipo == 1) // Recta
                    {
                        Add_fila_componentes("recta", "", c.lista_puntos[0].p.ToString(),
                            c.lista_puntos[c.lista_puntos.Count - 1].p.ToString(),
                            0, c.azte, c.azts, "", 0, 0, c.azr, 0, 0, "", currentId, false);
                    }
                    else // Curva
                    {
                        Add_fila_componentes("curva", new Point3d(c.xc, c.yc, 0).ToString(),
                            new Point3d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y, 0).ToString(),
                            new Point3d(c.lista_puntos[c.lista_puntos.Count - 1].p.X,
                                c.lista_puntos[c.lista_puntos.Count - 1].p.Y, 0).ToString(),
                            c.radio, c.azte, c.azts, "", 0, 0, c.azr, 0, 0, "", currentId, false);
                    }
                }

                // Actualizar contador de ID
                if (Componentes.Count > 0)
                {
                    id_componente = Componentes.Count;
                }
                else
                {
                    id_componente = 0;
                }
            }
        }

        private void b_guardareje_Click(object sender, EventArgs e)
        {
            if (GuardarEjeActual())
            {
                MessageBox.Show("Eje guardado correctamente en memoria.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void b_borrareje_Click(object sender, EventArgs e)
        {
            string nombreEje = B_nombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombreEje))
            {
                MessageBox.Show("Debe seleccionar un eje (o especificar su nombre) para borrarlo.",
                    "Nombre requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var eje = EjesGuardados.FirstOrDefault(x => x.Nombre.Equals(nombreEje, StringComparison.OrdinalIgnoreCase));
            if (eje == null)
            {
                MessageBox.Show($"No se encontró ningún eje con el nombre '{nombreEje}' en memoria.",
                    "No encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"¿Está seguro de que desea eliminar el eje '{nombreEje}' de la memoria?",
                "Confirmar borrado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                EjesGuardados.Remove(eje);
                ActualizarComboEjes();

                // Limpiar formulario ya que el eje ha sido borrado
                Componentes.Clear();
                dsApp1.Tables["Componentes"].Clear();
                B_nombre.Clear();
                id_componente = 0;
                indiceEditado = -1;
                LimpiarDatos();

                MessageBox.Show("Eje eliminado de la memoria.", "Borrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarDataGridDesdeComponentes(List<Logica.Componente> componentes)
        {
            // Limpiar el DataTable
            System.Data.DataTable dt = dsApp1.Tables["Componentes"];
            dt.Rows.Clear();

            // Recorrer todos los componentes y añadirlos con ID secuencial
            for (int i = 0; i < componentes.Count; i++)
            {
                var c = componentes[i];

                if (c.Tipo == 1) // Recta
                {
                    Add_fila_componentes(
                        "recta",
                        "",
                        c.lista_puntos[0].p.ToString(),
                        c.lista_puntos[c.lista_puntos.Count - 1].p.ToString(),
                        0,
                        c.azte,
                        c.azts,
                        "",
                        0,
                        0,
                        c.azr,
                        0,
                        0,
                        "",
                        i,
                        false // No insertar, solo añadir
                    );
                }
                else if (c.Tipo == 2) // Curva
                {
                    Add_fila_componentes(
                        "curva",
                        new Point3d(c.xc, c.yc, 0).ToString(),
                        new Point3d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y, 0).ToString(),
                        new Point3d(c.lista_puntos[c.lista_puntos.Count - 1].p.X, c.lista_puntos[c.lista_puntos.Count - 1].p.Y, 0).ToString(),
                        c.radio,
                        c.azte,
                        c.azts,
                        "",
                        0,
                        0,
                        c.azr,
                        0,
                        0,
                        "",
                        i,
                        false // No insertar, solo añadir
                    );
                }
            }

            // Actualizar id_componente para reflejar IDs correctos
            id_componente = componentes.Count;
        }

        // salida_longuitudinal eliminada — se usa principal.SalidaAmilia

        private void b_guardarejes_Click(object sender, EventArgs e)
        {
            if (EjesGuardados == null || EjesGuardados.Count == 0)
            {
                MessageBox.Show("No hay ejes en memoria para guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Archivos AMILIA (*.amilia)|*.amilia|Todos los archivos (*.*)|*.*";
            saveDialog.DefaultExt = "amilia";
            saveDialog.FileName = "EjesGuardados.amilia";
            saveDialog.Title = "Guardar Todos los Ejes";
            saveDialog.InitialDirectory = !string.IsNullOrEmpty(principal.ruta_principal) ? principal.ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var listaParaGuardar = new List<principal.SalidaAmilia>();

                    foreach (var ejeMemoria in EjesGuardados)
                    {
                        var salida = new principal.SalidaAmilia();
                        salida.Tipo = principal.TipoSalida.Longitudinal;
                        salida.Nombre = ejeMemoria.Nombre;
                        salida.Componentes = ejeMemoria.Componentes;
                        salida.MComponentes = ejeMemoria.MComponentes ?? new List<EjeDeTrazado.componentes.Componente>();
                        salida.Trazado = ejeMemoria.Trazado ?? new List<Autodesk.AutoCAD.Geometry.Point2d>();
                        salida.Lista_original = ejeMemoria.Lista_original ?? new List<Point3d>();
                        listaParaGuardar.Add(salida);
                    }

                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    settings.Converters.Add(new AutoCADPointConverter());

                    string json = JsonConvert.SerializeObject(listaParaGuardar, Formatting.Indented, settings);
                    File.WriteAllText(saveDialog.FileName, json);

                    MessageBox.Show($"Se han guardado correctamente en:\n{saveDialog.FileName}", "Guardado Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar los ejes:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private EjeDeTrazado.componentes.Componente ConvertToEjeComponente(Logica.Componente c)
        {
            if (c.lista_puntos == null || c.lista_puntos.Count == 0) return null;

            var pIni = c.lista_puntos[0].p;
            var pFin = c.lista_puntos[c.lista_puntos.Count - 1].p;

            tadLayShare.puntos.Punto3d puntoEntrada = new tadLayShare.puntos.Punto3d(pIni.X, pIni.Y, 0);
            tadLayShare.puntos.Punto3d puntoSalida = new tadLayShare.puntos.Punto3d(pFin.X, pFin.Y, 0);

            double pkIni = 0;

            if (c.Tipo == 1) // Recta
            {
                return new EjeDeTrazado.componentes.Linea(puntoEntrada, puntoSalida, pkIni, 0, c.azts);
            }
            else if (c.Tipo == 2) // Curva
            {
                tadLayShare.puntos.Punto3d centro = new tadLayShare.puntos.Punto3d(c.xc, c.yc, 0);
                return new EjeDeTrazado.componentes.Curva(puntoEntrada, puntoSalida, centro, c.radio, pkIni, 0, c.direccion);
            }
            return null;
        }

        private void b_cargarejes_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Archivos AMILIA (*.amilia)|*.amilia|Todos los archivos (*.*)|*.*";
            openDialog.Title = "Cargar Ejes";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    principal.ruta_principal = System.IO.Path.GetDirectoryName(openDialog.FileName);
                    string json = File.ReadAllText(openDialog.FileName);
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    settings.Converters.Add(new AutoCADPointConverter());

                    List<principal.SalidaAmilia> listaCargada = null;
                    try
                    {
                        listaCargada = JsonConvert.DeserializeObject<List<principal.SalidaAmilia>>(json, settings);
                    }
                    catch
                    {
                        MessageBox.Show("Error al deserializar el archivo. Formato incorrecto.");
                        return;
                    }

                    if (listaCargada != null && listaCargada.Count > 0)
                    {
                        // Filtrar solo entradas Longitudinal (los de Perfil van a TrazadoUsuarioPerfil)
                        var ejesLongitudinales = listaCargada
                            .Where(x => x.Tipo == principal.TipoSalida.Longitudinal)
                            .ToList();

                        if (ejesLongitudinales.Count == 0)
                        {
                            MessageBox.Show("El archivo no contiene ejes de trazado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        EjesGuardados.Clear();
                        foreach (var item in ejesLongitudinales)
                        {
                            var nuevoEje = new EjeEnMemoria();
                            nuevoEje.Nombre = item.Nombre;
                            nuevoEje.Componentes = item.Componentes ?? new List<Logica.Componente>();
                            nuevoEje.MComponentes = item.MComponentes ?? null;
                            nuevoEje.Trazado = item.Trazado ?? null;
                            nuevoEje.Lista_original = (item.Lista_original != null && item.Lista_original.Count > 0)
                                                        ? item.Lista_original
                                                        : null;

                            EjesGuardados.Add(nuevoEje);
                        }

                        ActualizarComboEjes();
                        if (EjesGuardados.Count > 0) CargarEjeEnDataGrid(EjesGuardados[0]);

                        MessageBox.Show($"Se cargaron {EjesGuardados.Count} ejes de trazado correctamente.", "Carga Exitosa");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar: {ex.Message}");
                }
            }
        }

        private void CargarEjeEnDataGrid(EjeEnMemoria eje)
        {
            if (eje == null || eje.Componentes == null) return;

            // Limpiar componentes actuales
            Componentes.Clear();
            System.Data.DataTable dt = dsApp1.Tables["Componentes"];
            dt.Clear();

            // Rellenar nombre
            B_nombre.Text = eje.Nombre;

            // Cargar copia de los componentes (para no compartir referencia con el eje guardado)
            Componentes = new List<Logica.Componente>(eje.Componentes);

            // Reconstruir DataGrid
            for (int i = 0; i < Componentes.Count; i++)
            {
                var c = Componentes[i];

                if (c.Tipo == 1) // Recta
                {
                    Add_fila_componentes(
                        "recta",
                        "",
                        c.lista_puntos[0].p.ToString(),
                        c.lista_puntos[c.lista_puntos.Count - 1].p.ToString(),
                        0,
                        c.azte,
                        c.azts,
                        "",
                        0,
                        0,
                        c.azr,
                        0,
                        0,
                        "",
                        i,
                        false
                    );
                }
                else if (c.Tipo == 2) // Curva
                {
                    Add_fila_componentes(
                        "curva",
                        new Point3d(c.xc, c.yc, 0).ToString(),
                        new Point3d(c.lista_puntos[0].p.X, c.lista_puntos[0].p.Y, 0).ToString(),
                        new Point3d(c.lista_puntos[c.lista_puntos.Count - 1].p.X,
                                    c.lista_puntos[c.lista_puntos.Count - 1].p.Y, 0).ToString(),
                        c.radio,
                        c.azte,
                        c.azts,
                        "",
                        0,
                        0,
                        c.azr,
                        0,
                        0,
                        "",
                        i,
                        false
                    );
                }
            }

            id_componente = Componentes.Count;
        }

        private void P_ini_x_TextChanged(object sender, EventArgs e)
        {

        }

        private void P_ini_x__TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }



    // DTOs para serialización segura

    // DTOs eliminados — se usa principal.SalidaAmilia y EjeDeTrazado.componentes
}
