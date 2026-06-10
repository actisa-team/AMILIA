using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;
using engCadNet;
using System.IO;
using Newtonsoft.Json;
using MaterialSkin.Controls;

namespace interfaz
{
    public partial class TrazadoUsuarioPerfil : MaterialForm
    {
        // Listas para almacenar los objetos lógicos
        private List<Logica.Pendiente> listaRectas = new List<Logica.Pendiente>();
        private List<Logica.Parabola> listaParabolas = new List<Logica.Parabola>();
        // Lista para curvas circulares
        private List<EjeDeTrazado.componentes.Curva> listaCurvas = new List<EjeDeTrazado.componentes.Curva>();

        // UI Components


        // Lista para visualización en el DataGridView

        // Lista para visualización en el DataGridView
        private List<ComponentePerfil> listaComponentesPerfil = new List<ComponentePerfil>();
        private BindingSource bindingSource = new BindingSource();

        // Clase contenedora para el DataGridView
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
            public double? Pendiente { get; set; } 

            [Newtonsoft.Json.JsonProperty("radio", Order = 6, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public double? Radio { get; set; }

            [Newtonsoft.Json.JsonIgnore]
            public object ObjetoOriginal { get; set; }
            
            [Newtonsoft.Json.JsonIgnore]
            public ObjectId EntityId { get; set; }
        }

        // Referencia a CalculoPolilineaPerfil para obtener datos de inserción
        private Logica.CalculoPolilineaPerfil calculoPolilineaPerfil;
        private ObjectId _idUltimaPolilinea = ObjectId.Null;

        // IDs para vistas previas en tiempo real
        private List<ObjectId> _idPreview1 = new List<ObjectId>();
        private List<ObjectId> _idPreview2 = new List<ObjectId>();

        // Almacenar el índice previo del tipo de acuerdo para revertir cambios si el usuario cancela
        private int _previousAgreementTypeIndex = 0;
        
        // Estructura para almacenar perfiles en memoria
        public class PerfilEnMemoria
        {
            public string Nombre { get; set; }
            public List<ComponentePerfil> Componentes { get; set; }
        }

        // salida_perfil eliminada — se usa principal.SalidaAmilia con Tipo = principal.TipoSalida.Perfil

        // Lista de perfiles guardados en memoria
        private List<PerfilEnMemoria> PerfilesGuardados = new List<PerfilEnMemoria>();

        // Ruta del archivo .amilia asociado (compartida con principal)
        private string _rutaArchivoAmilia = null;

        // Variables para modo edición
        private bool _editMode = false;
        private int _editIndex = -1;

        public TrazadoUsuarioPerfil(Logica.CalculoPolilineaPerfil calculo, string rutaArchivoAmilia = null)
        {
            InitializeComponent();
            
            // Inicializar ComboBox Tipo Acuerdo (c_t_acuerdos ya existe en Designer)
            this.c_t_acuerdos.Items.Clear();
            this.c_t_acuerdos.Items.AddRange(new object[] { "Parabolico", "Circular" });
            this.c_t_acuerdos.SelectedIndex = 0; // Default Parabolico
            this.c_t_acuerdos.SelectedIndexChanged += new System.EventHandler(this.c_t_acuerdos_SelectedIndexChanged);

            this.calculoPolilineaPerfil = calculo;
            ConfigurarDataGridView();
            ConfigurarEstiloBotones();
            
            if (this.calculoPolilineaPerfil != null && 
                this.calculoPolilineaPerfil.Polilinea != null && 
                this.calculoPolilineaPerfil.Polilinea.Count > 0)
            {
                var pStart = this.calculoPolilineaPerfil.Polilinea[0];
                P_ini_x.Texts = pStart.p.X.ToString();
                P_ini_y.Texts = pStart.p.Y.ToString();

                
                // Fix for manual edit: Declare variables correctly
                double n_escala = (double)this.calculoPolilineaPerfil.Escala;
                ObjectId newId = DibujarMarcaPunto(new Point2d(pStart.p.X, pStart.p.Y), n_escala);
                
                // Assign to _idPreview1 as this is likely the first slope initialization
                if (newId != ObjectId.Null) _idPreview1.Add(newId);
            }

            
            // Suscribir eventos para vista previa en tiempo real
            P_ini_x.TextChanged += Coordinate_TextChanged;
            P_ini_y.TextChanged += Coordinate_TextChanged;
            P_fin_x.TextChanged += Coordinate_TextChanged;
            P_fin_y.TextChanged += Coordinate_TextChanged;

            P_ini_x_2.TextChanged += Coordinate_TextChanged;
            P_ini_y_2.TextChanged += Coordinate_TextChanged;
            P_fin_x_2.TextChanged += Coordinate_TextChanged;
            P_fin_y_2.TextChanged += Coordinate_TextChanged;

            // Wiring up persistence events
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            // Guardar ruta del archivo para usarla al guardar
            _rutaArchivoAmilia = rutaArchivoAmilia;

            // Auto-cargar perfiles desde el archivo .amilia si se proporcionó
            if (!string.IsNullOrEmpty(rutaArchivoAmilia) && File.Exists(rutaArchivoAmilia))
            {
                CargarPerfilesDesdeArchivo(rutaArchivoAmilia);
            }

            // Disparar vista previa inicial si ya hay datos
            if (!string.IsNullOrEmpty(P_ini_x.Text) && !string.IsNullOrEmpty(P_fin_x.Text))
            {
                ActualizarVistaPrevia(1);
            }
        }
        
        public TrazadoUsuarioPerfil()
        {
            InitializeComponent();
            ConfigurarDataGridView();
            ConfigurarEstiloBotones();
        }

        private void ConfigurarEstiloBotones()
        {
            BotonPersonalizado[] botones = { b_borrarentidad, B_insertar, AniadirEntidad, b_cargarejes, b_guardarejes, b_borrareje, b_guardareje, b_ejenuevo, B_calcular };

            if (b_borrarentidad != null) b_borrarentidad.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("delete");
            if (B_insertar != null) B_insertar.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("insert");
            if (AniadirEntidad != null) AniadirEntidad.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("add");
            if (b_cargarejes != null) b_cargarejes.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("cargar");
            if (b_borrareje != null) b_borrareje.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("delete");
            if (b_ejenuevo != null) b_ejenuevo.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("add");
            if (b_guardareje != null) b_guardareje.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("save");
            if (b_guardarejes != null) b_guardarejes.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("descargar");
            foreach (var btn in botones)
            {
                if (btn != null)
                {
                    btn.Cursor = Cursors.Hand;
                    btn.AutoSize = false; 
                    btn.BorderRadius = 8;
                    btn.GrosorBorde = 1;
                    btn.ColorBorde = System.Drawing.Color.LightGray;
                    btn.Sombrear = true;
                    btn.SombraDesplazamiento = 2;
                }
            }

            // Estilos para botón secundario general (botones grises)
            var botonesSecundarios = new BotonPersonalizado[] { b_borrarentidad, b_borrareje, B_insertar, AniadirEntidad, b_ejenuevo, b_cargarejes, b_guardareje, b_guardarejes };
            foreach(var btn in botonesSecundarios)
            {
                if (btn != null)
                {
                    btn.UsaGradiente = true;
                    btn.GradienteColorInicio = System.Drawing.Color.White;
                    btn.GradienteColorFin = System.Drawing.Color.FromArgb(235, 235, 235);
                    btn.GradienteHoverInicio = System.Drawing.Color.FromArgb(245, 245, 245);
                    btn.GradienteHoverFin = System.Drawing.Color.FromArgb(220, 220, 220);
                    btn.ForeColor = System.Drawing.Color.Black;
                }
            }

            // Estilos específicos para Calcular
            if (B_calcular != null)
            {
                B_calcular.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("calular");
                B_calcular.UsaGradiente = true;
                B_calcular.GradienteColorInicio = System.Drawing.Color.FromArgb(95, 125, 175);
                B_calcular.GradienteColorFin = System.Drawing.Color.FromArgb(65, 95, 145);
                B_calcular.GradienteHoverInicio = System.Drawing.Color.FromArgb(105, 135, 185);
                B_calcular.GradienteHoverFin = System.Drawing.Color.FromArgb(75, 105, 155);
                B_calcular.ColorBorde = System.Drawing.Color.FromArgb(50, 80, 130);
                B_calcular.ForeColor = System.Drawing.Color.White;
            }
        }


        private void ConfigurarDataGridView()
        {
            bindingSource.DataSource = listaComponentesPerfil;
            dataGridView1.DataSource = bindingSource;

            if (dataGridView1.Columns["ObjetoOriginal"] != null)
            {
                dataGridView1.Columns["ObjetoOriginal"].Visible = false;
            }

            if (dataGridView1.Columns["EntityId"] != null)
            {
                dataGridView1.Columns["EntityId"].Visible = false;
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // Suscribir evento para ocultar valores 0
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            
            // Suscribir evento para doble click (editar)
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Ocultar ceros en las columnas numéricas (como Kv y Pendiente si es 0)
            if (e.Value != null)
            {
                if (e.Value is double d && d == 0)
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                }
            }
        }
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= listaComponentesPerfil.Count) return;
            CargarDatosEdicion(e.RowIndex);
        }

        private void CargarDatosEdicion(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= listaComponentesPerfil.Count) return;

            // Borrar vistas previas anteriores si las hubiera
            LimpiarVistaPrevia(_idPreview1);
            LimpiarVistaPrevia(_idPreview2);

            ComponentePerfil comp = listaComponentesPerfil[rowIndex];
            _editIndex = rowIndex;
            CambiarModoEdicion(true);

            // Limpiar campos primero
            LimpiarCampos();

            if (comp.Tipo == "RECTA" && comp.ObjetoOriginal is Logica.Pendiente p)
            {
                // Caso 1: Seleccionada una RECTA
                // Cargar esta recta como Pendiente 1
                if (p.Puntos != null && p.Puntos.Count >= 2)
                {
                    P_ini_x.Texts = p.Puntos.First().X.ToString("F3");
                    P_ini_y.Texts = p.Puntos.First().Y.ToString("F3");
                    P_fin_x.Texts = p.Puntos.Last().X.ToString("F3");
                    P_fin_y.Texts = p.Puntos.Last().Y.ToString("F3");
                }

                // Intentar cargar la siguiente recta como Pendiente 2
                // Buscar si hay una parábola en medio
                // Lógica: Recta Seleccionada -> (Posible Parábola) -> Siguiente Recta
                
                // Buscar el siguiente componente en la lista visual
                if (_editIndex + 1 < listaComponentesPerfil.Count)
                {
                    ComponentePerfil nextComp = listaComponentesPerfil[_editIndex + 1];
                    
                    if (nextComp.Tipo == "PARABOLA" && nextComp.ObjetoOriginal is Logica.Parabola parabola)
                    {
                        // Hay una parábola, cargar su Kv
                        t_kv.Text = nextComp.Kv.GetValueOrDefault().ToString("F3");

                        // Y cargar la siguiente recta (si existe) como Pendiente 2
                        if (_editIndex + 2 < listaComponentesPerfil.Count)
                        {
                            ComponentePerfil nextSlopeComp = listaComponentesPerfil[_editIndex + 2];
                            if (nextSlopeComp.Tipo == "RECTA" && nextSlopeComp.ObjetoOriginal is Logica.Pendiente p2)
                            {
                                if (p2.Puntos != null && p2.Puntos.Count >= 2)
                                {
                                    P_ini_x_2.Text = p2.Puntos.First().X.ToString("F3");
                                    P_ini_y_2.Text = p2.Puntos.First().Y.ToString("F3");
                                    P_fin_x_2.Text = p2.Puntos.Last().X.ToString("F3");
                                    P_fin_y_2.Text = p2.Puntos.Last().Y.ToString("F3");
                                }
                            }
                        }
                    }
                    else if (nextComp.Tipo == "RECTA" && nextComp.ObjetoOriginal is Logica.Pendiente p2)
                    {
                        // No hay parábola, es directamente otra recta (caso raro si hay encadenamiento, pero posible)
                        // Ojo: Si no hay parábola, t_kv debería estar vacío o 0
                        t_kv.Text = "0";
                        
                        if (p2.Puntos != null && p2.Puntos.Count >= 2)
                        {
                            P_ini_x_2.Text = p2.Puntos.First().X.ToString("F3");
                            P_ini_y_2.Text = p2.Puntos.First().Y.ToString("F3");
                            P_fin_x_2.Text = p2.Puntos.Last().X.ToString("F3");
                            P_fin_y_2.Text = p2.Puntos.Last().Y.ToString("F3");
                        }
                    }
                }
            }
            else if (comp.Tipo == "PARABOLA" && comp.ObjetoOriginal is Logica.Parabola parabola)
            {
                // Caso 2: Seleccionada una PARABOLA
                // Cargar Kv
                t_kv.Text = comp.Kv.GetValueOrDefault().ToString("F3");

                // Cargar Recta Anterior como Pendiente 1
                if (_editIndex - 1 >= 0)
                {
                    ComponentePerfil prevComp = listaComponentesPerfil[_editIndex - 1];
                    if (prevComp.Tipo == "RECTA" && prevComp.ObjetoOriginal is Logica.Pendiente p1)
                    {
                        if (p1.Puntos != null && p1.Puntos.Count >= 2)
                        {
                            P_ini_x.Texts = p1.Puntos.First().X.ToString("F3");
                            P_ini_y.Texts = p1.Puntos.First().Y.ToString("F3");
                            P_fin_x.Texts = p1.Puntos.Last().X.ToString("F3");
                            P_fin_y.Texts = p1.Puntos.Last().Y.ToString("F3");
                        }
                    }
                    // Ajustamos el índice de edición para apuntar a la PRIMERA recta del conjunto que estamos editando
                    // Esto es importante para saber desde dónde reconstruir
                     _editIndex = _editIndex - 1;
                }

                // Cargar Recta Siguiente como Pendiente 2
                if (_editIndex + 2 < listaComponentesPerfil.Count) // +2 porque hemos bajado el índice en 1
                {
                    ComponentePerfil nextComp = listaComponentesPerfil[_editIndex + 2];
                    if (nextComp.Tipo == "RECTA" && nextComp.ObjetoOriginal is Logica.Pendiente p2)
                    {
                        if (p2.Puntos != null && p2.Puntos.Count >= 2)
                        {
                            P_ini_x_2.Text = p2.Puntos.First().X.ToString("F3");
                            P_ini_y_2.Text = p2.Puntos.First().Y.ToString("F3");
                            P_fin_x_2.Text = p2.Puntos.Last().X.ToString("F3");
                            P_fin_y_2.Text = p2.Puntos.Last().Y.ToString("F3");
                        }
                    }
                }
            }
            
            // Forzar actualización de vista previa
            ActualizarVistaPrevia(1);
            ActualizarVistaPrevia(2);
        }

        private void CambiarModoEdicion(bool activar)
        {
            _editMode = activar;
            if (activar)
            {
                AniadirEntidad.Text = "Guardar Cambios";
                // Opcional: Cambiar color del botón para indicar modo edición
                AniadirEntidad.BackColor = System.Drawing.Color.Orange;
            }
            else
            {
                AniadirEntidad.Text = "Añadir Entidad";
                AniadirEntidad.BackColor = SystemColors.Control;
                _editIndex = -1;
                LimpiarCampos();

                // Borrar las vistas previas de la capa especial al salir de edición
                LimpiarVistaPrevia(_idPreview1);
                LimpiarVistaPrevia(_idPreview2);
                
                // Restaurar punto inicial para siguiente inserción
                if (listaComponentesPerfil.Count > 0)
                {
                    var lastComp = listaComponentesPerfil.Last();
                    if (lastComp.Tipo == "RECTA" && lastComp.ObjetoOriginal is Logica.Pendiente p)
                    {
                        if (p.Puntos != null && p.Puntos.Count > 0)
                        {
                            P_ini_x.Texts = p.Puntos.First().X.ToString("F3");
                            P_ini_y.Texts = p.Puntos.First().Y.ToString("F3");
                            P_fin_x.Texts = p.Puntos.Last().X.ToString("F3");
                            P_fin_y.Texts = p.Puntos.Last().Y.ToString("F3");
                        }
                    }
                    else if (lastComp.Tipo == "PARABOLA" && lastComp.ObjetoOriginal is Logica.Parabola par)
                    {
                        // Si lo último es parábola (raro), ponemos sus puntos de entrada/salida
                        P_ini_x.Texts = par.puntoEntrada.X.ToString("F3");
                        P_ini_y.Texts = par.puntoEntrada.Y.ToString("F3");
                        P_fin_x.Texts = par.puntoSalida.X.ToString("F3");
                        P_fin_y.Texts = par.puntoSalida.Y.ToString("F3");
                    }
                }
            }
        }

        private void LimpiarCampos()
        {
            P_ini_x.Texts = ""; P_ini_y.Texts = "";
            P_fin_x.Texts = ""; P_fin_y.Texts = "";
            P_ini_x_2.Texts = ""; P_ini_y_2.Texts = "";
            P_fin_x_2.Texts = ""; P_fin_y_2.Texts = "";
            t_kv.Clear();
        }
        private void AgregarComponenteALista(object componente)
        {
            ComponentePerfil cp = new ComponentePerfil();

            if (componente is Logica.Pendiente p)
            {
                cp.Tipo = "RECTA";
                cp.ObjetoOriginal = p;

                if (p.Puntos != null && p.Puntos.Count > 0)
                {
                    cp.PuntoInicial = $"({p.Puntos.First().X:F3}, {p.Puntos.First().Y:F3})";
                    cp.PuntoFinal = $"({p.Puntos.Last().X:F3}, {p.Puntos.Last().Y:F3})";

                    // Calcular pendiente si hay al menos 2 puntos
                    if (p.Puntos.Count >= 2)
                    {
                        double dx = p.Puntos.Last().X - p.Puntos.First().X;
                        double dy = p.Puntos.Last().Y - p.Puntos.First().Y;
                        if (dx != 0)
                            cp.Pendiente = (dy / dx) * 100;
                    }
                }

                listaRectas.Add(p);
            }
            else if (componente is Logica.Parabola par)
            {
                cp.Tipo = "PARABOLA";
                cp.ObjetoOriginal = par;

                // Asumiendo que Parabola tiene alguna forma de obtener sus puntos o Kv
                // Por ahora usamos valores por defecto o calculados si existen propiedades
                // cp.Kv = par.CalcularKv(...); // Dependerá de la implementación de Parabola

                listaParabolas.Add(par);
            }

            listaComponentesPerfil.Add(cp);
            bindingSource.ResetBindings(false);
        }

        private void AniadirEntidadNueva()
        {
            try
            {
                // 1. Obtener datos de entrada
                // Pendiente 1
                if (!double.TryParse(P_ini_x.Text, out double x1)) return;
                if (!double.TryParse(P_ini_y.Text, out double y1)) return;
                if (!double.TryParse(P_fin_x.Text, out double x2)) return;
                if (!double.TryParse(P_fin_y.Text, out double y2)) return;

                // Pendiente 2
                if (!double.TryParse(P_ini_x_2.Text, out double x3)) return;
                if (!double.TryParse(P_ini_y_2.Text, out double y3)) return;
                if (!double.TryParse(P_fin_x_2.Text, out double x4)) return;
                if (!double.TryParse(P_fin_y_2.Text, out double y4)) return;

                // Parámetro Kv
                double kv = 0;
                bool tieneKv = double.TryParse(t_kv.Text, out kv) && kv != 0;

                // Escala vertical
                double n_escala = calculoPolilineaPerfil != null ? (double)calculoPolilineaPerfil.Escala : 1.0;

                // Validar
                // Validar (Solo para la primera entidad)
                /* 
                if (listaRectas.Count == 0 && x1 != 0)
                {
                    MessageBox.Show("El primer punto (P_ini_x) debe ser 0 para el inicio del trazado.");
                    return;
                }
                */

                // 2. Calcular pendientes (en tanto por uno)
                double m1 = (y2 - y1) / (x2 - x1);
                double m2 = (y4 - y3) / (x4 - x3);

                if (Math.Abs(m1 - m2) < 1e-6)
                {
                    MessageBox.Show("Las pendientes son paralelas o idénticas. No se puede generar una curva.");
                    return;
                }

                // 3. Calcular Punto de Intersección Vertical (PIV)
                // y - y1 = m1 * (x - x1)  =>  y = m1*x + (y1 - m1*x1)
                // y - y3 = m2 * (x - x3)  =>  y = m2*x + (y3 - m2*x3)
                // m1*x + b1 = m2*x + b2  =>  x * (m1 - m2) = b2 - b1  =>  x = (b2 - b1) / (m1 - m2)

                double b1 = y1 - m1 * x1;
                double b2 = y3 - m2 * x3;

                double x_piv = (b2 - b1) / (m1 - m2);
                double y_piv = m1 * x_piv + b1;

                // 4. Calcular geometría de la curva si hay Kv
                if (tieneKv)
                {
                    if (c_t_acuerdos.SelectedIndex == 1 && kv > 0) // CIRCULAR
                    {
                         // --- LOGICA CIRCULAR ---
                         // --- LOGICA CIRCULAR ---
                         double x_ptv, y_ptv;

                         if (listaRectas.Count == 0)
                         {
                             // --- CASO INICIAL: Recta 1 - Circular - Recta 2 ---
                             double radio = kv;
                             
                             // Intersection point (PI) is (x_piv, y_piv)
                             
                             // Calculate angles using Atan
                             double ang1 = Math.Atan(m1);
                             double ang2 = Math.Atan(m2);
                             
                             // Delta Angle (Deflection)
                             double delta = Math.Abs(ang2 - ang1);
                             
                             // Tangent Length (T) = R * tan(Delta/2)
                             double T = radio * Math.Tan(delta / 2.0);
                             
                             // Calculate PCV and PTV coordinates
                             // Vector Unitario Tangente 1 (hacia PI): (1, m1) normalizado
                             double mod1 = Math.Sqrt(1 + m1 * m1);
                             double ux1 = 1 / mod1;
                             double uy1 = m1 / mod1;
                             
                             // PCV = PI - T * u1
                             double x_pcv = x_piv - T * ux1;
                             double y_pcv = y_piv - T * uy1;
                             
                             // Vector Unitario Tangente 2 (desde PI): (1, m2) normalizado
                             double mod2 = Math.Sqrt(1 + m2 * m2);
                             double ux2 = 1 / mod2;
                             double uy2 = m2 / mod2;
                             
                             // PTV = PI + T * u2
                             x_ptv = x_piv + T * ux2;
                             y_ptv = y_piv + T * uy2;
                             
                             // VALIDATION
                             if (x_pcv < x1 || x_ptv > x4)
                             {
                                 MessageBox.Show($"El Radio ({radio}) genera una curva que excede los límites.\n" +
                                                 $"Tangente: {T:F3} m\n" +
                                                 $"Inicio (PCV): {x_pcv:F3}\n" +
                                                 $"Fin (PTV): {x_ptv:F3}\n" +
                                                 "Reduzca el Radio.");
                                 return;
                             }
    
                             // --- CREATE ENTITIES ---
                             
                             // 1. RECTA 1
                             Logica.Pendiente p1_obj = new Logica.Pendiente();
                             p1_obj.Puntos = new List<Autodesk.AutoCAD.Geometry.Point2d>
                             {
                                 new Autodesk.AutoCAD.Geometry.Point2d(x1, y1),
                                 new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv)
                             };
                             listaRectas.Add(p1_obj);
                             
                             ComponentePerfil cpRecta1 = new ComponentePerfil
                             {
                                 Tipo = "RECTA",
                                 PuntoInicial = $"({x1:F3}, {y1:F3})",
                                 PuntoFinal = $"({x_pcv:F3}, {y_pcv:F3})",
                                 Kv = 0,
                                 Pendiente = m1 * 100, 
                                 ObjetoOriginal = p1_obj
                             };
                             
                             // Promocionar la vista previa si existe
                             LimpiarVistaPrevia(_idPreview1);
                             cpRecta1.EntityId = ObjectId.Null;
                             
                             if (cpRecta1.EntityId == ObjectId.Null || cpRecta1.EntityId.IsErased)
                             {
                                 cpRecta1.EntityId = DibujarLineaEnAutoCAD(
                                     new Point2d(x1, y1), 
                                     new Point2d(x_pcv, y_pcv), 
                                     n_escala);
                             }
                             else
                             {
                                 CambiarColorEntidad(cpRecta1.EntityId, 7);
                                 ActualizarEntidadAutoCAD(cpRecta1, new Point2d(x1, y1), new Point2d(x_pcv, y_pcv));
                             }
                             
                             listaComponentesPerfil.Add(cpRecta1);
                             
                             // 2. CURVA CIRCULAR
                             double cx, cy;
                             EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva sentido;
                             
                             if (m2 > m1) // Sag / CCW
                             {
                                 sentido = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario; // Left
                                 cx = x_pcv + radio * (-uy1);
                                 cy = y_pcv + radio * (ux1);
                             }
                             else // Crest / CW
                             {
                                 sentido = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario; // Right
                                 cx = x_pcv + radio * (uy1);
                                 cy = y_pcv + radio * (-ux1);
                             }
    
                             EjeDeTrazado.componentes.Curva curvaCirc = new EjeDeTrazado.componentes.Curva(
                                 new tadLayShare.puntos.Punto3d(x_pcv, y_pcv, 0),
                                 new tadLayShare.puntos.Punto3d(x_ptv, y_ptv, 0),
                                 new tadLayShare.puntos.Punto3d(cx, cy, 0),
                                 radio,
                                 x_pcv, 
                                 0, 
                                 sentido
                             );
                             listaCurvas.Add(curvaCirc);
                             
                             ComponentePerfil cpCircular = new ComponentePerfil
                             {
                                 Tipo = "CIRCULAR",
                                 PuntoInicial = $"({x_pcv:F3}, {y_pcv:F3})",
                                 PuntoFinal = $"({x_ptv:F3}, {y_ptv:F3})",
                                 Kv = radio, 
                                 ObjetoOriginal = curvaCirc
                             };
                             // Draw Circular
                             double ang_start = new Vector2d(x_pcv - cx, y_pcv - cy).Angle;
                             double ang_end = new Vector2d(x_ptv - cx, y_ptv - cy).Angle;
                             
                             if (sentido == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                             {
                                 double tmp = ang_start;
                                 ang_start = ang_end;
                                 ang_end = tmp;
                             }
    
                             cpCircular.EntityId = DibujarArcoEnAutoCAD(new Point2d(cx, cy), radio, ang_start, ang_end, sentido, n_escala);
                             listaComponentesPerfil.Add(cpCircular);
                         }
                         else
                         {
                             // --- CASO ENCADENADO: Modificar Recta Anterior - Circular - Recta Nueva ---
                             double radio = kv;
                             
                             // 1. Obtener Recta Anterior
                             Logica.Pendiente lastSlope = listaRectas.Last();
                             Point2d p_prev_start = lastSlope.Puntos[0];
                             Point2d p_prev_end = lastSlope.Puntos[1];
                             
                             double m_prev = (p_prev_end.Y - p_prev_start.Y) / (p_prev_end.X - p_prev_start.X);
                             double b_prev = p_prev_start.Y - m_prev * p_prev_start.X;
                             
                             // 2. Pendiente Siguiente (definida por usuario)
                             double m_next = (y4 - y3) / (x4 - x3);
                             double b_next = y3 - m_next * x3;
                             
                             // 3. Intersección (PI)
                             double x_pi = (b_next - b_prev) / (m_prev - m_next);
                             double y_pi = m_prev * x_pi + b_prev;
                             
                             // 4. Geometría Circular
                             double ang1 = Math.Atan(m_prev);
                             double ang2 = Math.Atan(m_next);
                             
                             // Delta Angle
                             double delta = Math.Abs(ang2 - ang1);
                             double T = radio * Math.Tan(delta / 2.0);
                             
                             // PCV (sobre recta anterior)
                             double mod1 = Math.Sqrt(1 + m_prev * m_prev);
                             double ux1 = 1 / mod1;
                             double uy1 = m_prev / mod1;
                                                          
                             double x_pcv = x_pi - T * ux1;
                             double y_pcv = y_pi - T * uy1;
                             
                             // PTV (sobre nueva recta)
                             double mod2 = Math.Sqrt(1 + m_next * m_next);
                             double ux2 = 1 / mod2;
                             double uy2 = m_next / mod2;
                             
                             x_ptv = x_pi + T * ux2;
                             y_ptv = y_pi + T * uy2;
                             
                             // 5. VALIDACIÓN
                             if (x_pcv < p_prev_start.X)
                             {
                                 MessageBox.Show($"El Radio ({radio}) es muy grande y la curva se solapa con la anterior.\n" +
                                                 $"Inicio (PCV): {x_pcv:F3} < Inicio Recta Anterior: {p_prev_start.X:F3}\n" +
                                                 "Reduzca el Radio.");
                                 return;
                             }
                             if (x_ptv > x4)
                             {
                                  MessageBox.Show($"El Radio ({radio}) excede el punto final.\n" +
                                                 $"Fin (PTV): {x_ptv:F3} > Fin Recta: {x4:F3}\n" +
                                                 "Reduzca el Radio.");
                                 return;
                             }
                             
                             // 6. ACTUALIZAR RECTA ANTERIOR
                             lastSlope.Puntos[1] = new Point2d(x_pcv, y_pcv);
                             
                             ComponentePerfil cpLastSlope = listaComponentesPerfil.Last(comp => comp.ObjetoOriginal == lastSlope);
                             cpLastSlope.PuntoFinal = $"({x_pcv:F3}, {y_pcv:F3})";
                             
                             // Redibujar si es necesario
                             if (cpLastSlope.EntityId != ObjectId.Null && !cpLastSlope.EntityId.IsErased)
                             {
                                 try
                                 {
                                     using (DocumentLock docLock = oCadManager.thisEditor.Document.LockDocument())
                                     {
                                         using (Transaction tr = oCadManager.StartTransaction())
                                         {
                                             Entity ent = tr.GetObject(cpLastSlope.EntityId, OpenMode.ForWrite) as Entity;
                                             if (ent != null) ent.Erase();
                                             tr.Commit();
                                         }
                                     }
                                 }
                                 catch { /* Ignorar errores de borrado */ }
                             }
                             cpLastSlope.EntityId = DibujarLineaEnAutoCAD(lastSlope.Puntos[0], lastSlope.Puntos[1], n_escala);
                             
                             // 7. AÑADIR CURVA CIRCULAR
                             double cx, cy;
                             EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva sentido;
                             
                             if (m_next > m_prev) // Sag / CCW
                             {
                                 sentido = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario; 
                                 cx = x_pcv + radio * (-uy1);
                                 cy = y_pcv + radio * (ux1);
                             }
                             else // Crest / CW
                             {
                                 sentido = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario; 
                                 cx = x_pcv + radio * (uy1);
                                 cy = y_pcv + radio * (-ux1);
                             }
                             
                             EjeDeTrazado.componentes.Curva curvaCirc = new EjeDeTrazado.componentes.Curva(
                                 new tadLayShare.puntos.Punto3d(x_pcv, y_pcv, 0),
                                 new tadLayShare.puntos.Punto3d(x_ptv, y_ptv, 0),
                                 new tadLayShare.puntos.Punto3d(cx, cy, 0),
                                 radio,
                                 x_pcv, 
                                 0, 
                                 sentido
                             );
                             listaCurvas.Add(curvaCirc);
                             
                             ComponentePerfil cpCircular = new ComponentePerfil
                             {
                                 Tipo = "CIRCULAR",
                                 PuntoInicial = $"({x_pcv:F3}, {y_pcv:F3})",
                                 PuntoFinal = $"({x_ptv:F3}, {y_ptv:F3})",
                                 Kv = radio, 
                                 ObjetoOriginal = curvaCirc
                             };
                             
                             double ang_start = new Vector2d(x_pcv - cx, y_pcv - cy).Angle;
                             double ang_end = new Vector2d(x_ptv - cx, y_ptv - cy).Angle;
                             
                             if (sentido == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                             {
                                 double tmp = ang_start;
                                 ang_start = ang_end;
                                 ang_end = tmp;
                             }
    
                             cpCircular.EntityId = DibujarArcoEnAutoCAD(new Point2d(cx, cy), radio, ang_start, ang_end, sentido, n_escala);
                             listaComponentesPerfil.Add(cpCircular);
                         }

                         // 3. RECTA 2
                         Logica.Pendiente p2_obj = new Logica.Pendiente();
                         p2_obj.Puntos = new List<Autodesk.AutoCAD.Geometry.Point2d>
                         {
                             new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv),
                             new Autodesk.AutoCAD.Geometry.Point2d(x4, y4)
                         };
                         listaRectas.Add(p2_obj);
                         
                         ComponentePerfil cpRecta2 = new ComponentePerfil
                         {
                             Tipo = "RECTA",
                             PuntoInicial = $"({x_ptv:F3}, {y_ptv:F3})",
                             PuntoFinal = $"({x4:F4}, {y4:F4})",
                             Kv = 0,
                             Pendiente = m2 * 100,
                             ObjetoOriginal = p2_obj
                         };
                         cpRecta2.EntityId = DibujarLineaEnAutoCAD(
                                new Point2d(x_ptv, y_ptv), 
                                new Point2d(x4, y4), 
                                n_escala);
                         listaComponentesPerfil.Add(cpRecta2);                         
                         // Update UI for chaining (Duplicate logic from Parabolic block)
                         P_ini_x.Texts = x_ptv.ToString("F3");
                         P_ini_y.Texts = y_ptv.ToString("F3");

                         P_fin_x.Texts = x4.ToString("F3");
                         P_fin_y.Texts = y4.ToString("F3");

                         P_ini_x_2.Texts = "";
                         P_ini_y_2.Texts = "";
                         P_fin_x_2.Texts = "";
                         P_fin_y_2.Texts = "";
                         t_kv.Clear();

                         bindingSource.ResetBindings(false);
                    }
                    else if (kv > 0)
                {
                    double x_ptv = 0;
                    double y_ptv = 0;

                    // STATE CHECK: Are we adding the first set or chaining?
                    if (listaRectas.Count == 0)
                    {
                        // --- INITIAL CASE: Slope 1 - Parabola 1 - Slope 2 ---

                        // Calcule slopes (m = dy/dx)
                        m1 = (y2 - y1) / (x2 - x1);
                        m2 = (y4 - y3) / (x4 - x3);

                        // Intersection point (PI) of the two tangents
                        b1 = y1 - m1 * x1;
                        b2 = y3 - m2 * x3;
                        
                        x_piv = (b2 - b1) / (m1 - m2);
                        y_piv = m1 * x_piv + b1;

                        // Use outer variables: m1, m2, b1, b2, x_piv (as x_pi), y_piv (as y_pi)
                        double x_pi = x_piv; 
                        // y_pi is y_piv

                        // Calculate Curve Length L
                        double theta = Math.Abs(m2 - m1);
                        double L = kv * theta;
                        double T = L / 2.0;

                        // PCV (Principle of Curve Vertical) and PTV (Point of Tangent Vertical)
                        double x_pcv = x_pi - T;
                        x_ptv = x_pi + T;

                        // VALIDATION: Check if curve fits within the segments
                        if (x_pcv < x1 || x_ptv > x4)
                        {
                            MessageBox.Show($"El valor de Kv ({kv}) genera una curva que excede los límites localmente.\n" +
                                            $"Longitud Curva: {L:F3} m\n" +
                                            $"Inicio Curva (PCV): {x_pcv:F3} (Límite: {x1})\n" +
                                            $"Fin Curva (PTV): {x_ptv:F3} (Límite: {x4})\n" +
                                            "Por favor, reduzca el Kv.");
                            return;
                        }

                        // Calculate Y coordinates on the tangents
                        double y_pcv = m1 * x_pcv + b1; // PCV is on tangent 1
                        y_ptv = m2 * x_ptv + b2; // PTV is on tangent 2

                        // Parabola Coefficients
                        double a = (m2 - m1) / (2 * L);
                        double b = m1 - 2 * a * x_pcv;
                        double c = y_pcv - a * Math.Pow(x_pcv, 2) - b * x_pcv;

                        // --- CREATE ENTITIES ---

                        // 1. RECTA 1 (Pendiente Entrada) -> From P1 to PCV
                        Logica.Pendiente p1_obj = new Logica.Pendiente();
                        p1_obj.Puntos = new List<Autodesk.AutoCAD.Geometry.Point2d>
                        {
                            new Autodesk.AutoCAD.Geometry.Point2d(x1, y1),
                            new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv)
                        };
                        listaRectas.Add(p1_obj);
                        
                        ComponentePerfil cpRecta1 = new ComponentePerfil
                        {
                            Tipo = "RECTA",
                            PuntoInicial = $"({x1:F3}, {y1:F3})",
                            PuntoFinal = $"({x_pcv:F3}, {y_pcv:F3})",
                            Kv = 0,
                            Pendiente = m1 * 100, // Percentage
                            ObjetoOriginal = p1_obj
                        };
                        
                        // Promocionar la vista previa a entidad permanente
                        LimpiarVistaPrevia(_idPreview1);
                        cpRecta1.EntityId = ObjectId.Null; // Ya no es una vista previa
                        
                        // Si por alguna razón no había vista previa, dibujarla ahora
                        if (cpRecta1.EntityId == ObjectId.Null || cpRecta1.EntityId.IsErased)
                        {
                            cpRecta1.EntityId = DibujarLineaEnAutoCAD(
                                new Point2d(x1, y1), 
                                new Point2d(x_pcv, y_pcv), 
                                n_escala);
                        }
                        else
                        {
                            // Cambiar color de amarillo (preview) a rojo (final)
                            CambiarColorEntidad(cpRecta1.EntityId, 1);
                            ActualizarEntidadAutoCAD(cpRecta1, new Point2d(x1, y1), new Point2d(x_pcv, y_pcv));
                        }
                        
                        listaComponentesPerfil.Add(cpRecta1);

                        // 2. PARABOLA -> From PCV to PTV
                        List<double> coeffs = new List<double> { a, b, c };
                        
                        Logica.PuntoPerfil pp_start = new Logica.PuntoPerfil();
                        pp_start.p = new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv);
                        pp_start.pendiente = m1; 

                        Logica.PuntoPerfil pp_end = new Logica.PuntoPerfil();
                        pp_end.p = new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv);
                        pp_end.pendiente = m2; 

                        List<Logica.PuntoPerfil> poly_perfil = new List<Logica.PuntoPerfil> { pp_start, pp_end };

                        Logica.Parabola parabola = new Logica.Parabola(coeffs, poly_perfil);
                        parabola.puntoEntrada = new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv);
                        parabola.puntoSalida = new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv);
                        
                        listaParabolas.Add(parabola);

                        ComponentePerfil cpParabola = new ComponentePerfil
                        {
                            Tipo = "PARABOLA",
                            PuntoInicial = $"({x_pcv:F3}, {y_pcv:F3})",
                            PuntoFinal = $"({x_ptv:F3}, {y_ptv:F3})",
                            Kv = kv,
                            ObjetoOriginal = parabola
                        };
                        listaComponentesPerfil.Add(cpParabola);

                        // 3. RECTA 2 (Pendiente Salida) -> From PTV to P4
                        Logica.Pendiente p2_obj = new Logica.Pendiente();
                        p2_obj.Puntos = new List<Autodesk.AutoCAD.Geometry.Point2d>
                        {
                            new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv),
                            new Autodesk.AutoCAD.Geometry.Point2d(x4, y4)
                        };
                        listaRectas.Add(p2_obj);

                        ComponentePerfil cpRecta2 = new ComponentePerfil
                        {
                            Tipo = "RECTA",
                            PuntoInicial = $"({x_ptv:F3}, {y_ptv:F3})",
                            PuntoFinal = $"({x4:F4}, {y4:F4})",
                            Kv = 0,
                            Pendiente = m2 * 100, // Percentage
                            ObjetoOriginal = p2_obj
                        };
                        
                        // Promocionar la vista previa a entidad permanente
                        LimpiarVistaPrevia(_idPreview2);
                        cpRecta2.EntityId = ObjectId.Null;
                        
                        if (cpRecta2.EntityId == ObjectId.Null || cpRecta2.EntityId.IsErased)
                        {
                            cpRecta2.EntityId = DibujarLineaEnAutoCAD(
                                new Point2d(x_ptv, y_ptv), 
                                new Point2d(x4, y4), 
                                n_escala);
                        }
                        else
                        {
                            CambiarColorEntidad(cpRecta2.EntityId, 1);
                            ActualizarEntidadAutoCAD(cpRecta2, new Point2d(x_ptv, y_ptv), new Point2d(x4, y4));
                        }
                        
                        listaComponentesPerfil.Add(cpRecta2);
                    }
                    else
                    {
                        // --- SUBSEQUENT CASE: Add Parabola + New Slope ---
                        
                        // 1. Identify Previous Slope (Last added slope)
                        Logica.Pendiente lastSlope = listaRectas.Last();
                        // Get its geometric data. Note: The last slope's current end point is likely the user's "P4" from previous step,
                        // or a PTV if it was a middle piece.
                        // Actually, the last entity in the list is the "exit slope" of the previous operation.
                        // We need its start point (which is PTV of previous curve) and its defined logic slope.
                        
                        // Let's recalculate the slope m_prev from its points.
                        Autodesk.AutoCAD.Geometry.Point2d p_prev_start = lastSlope.Puntos[0];
                        Autodesk.AutoCAD.Geometry.Point2d p_prev_end = lastSlope.Puntos[1];
                        
                        double m_prev = (p_prev_end.Y - p_prev_start.Y) / (p_prev_end.X - p_prev_start.X);
                        double b_prev = p_prev_start.Y - m_prev * p_prev_start.X;

                        // 2. Identify New Slope (User input P3-P4)
                        // Note: P3 should match p_prev_end technically, but we use the new line defined by user P3-P4
                        // or we enforce continuity. User requirement: "la siguiente pendiente debe ser el punto inicial de la ultima pendiente ya añadida y el ultimo punto de la pendiente añadida"
                        // Wait, "punto inicial de la ultima pendiente ya añadida" -> PTV of previous curve?
                        // "y el ultimo punto de la pendiente añadida" -> the end of the slope we just added.
                        // Actually, users usually click P3, P4 to define the DIRECTION of the next tangent.
                        // The *actual* geometric line will start from the previous PTV (or modified point).
                        
                        // Let's use user input P3-P4 just to calculate the slope m_next.
                        // Or does the user click the absolute points?
                        // "la siguiente pendiente debe ser el punto inicial de(sde) la ultima pendiente... y el ultimo..."
                        // Maybe user means: Construct the new tangent starting from the end of the previous one?
                        // Let's assume user inputs define the *geometry* of the new segment (P3-P4).
                        double m_next = (y4 - y3) / (x4 - x3);
                        double b_next = y3 - m_next * x3;

                        // 3. Calculate Intersection (PI) between Previous Slope and New Slope
                        double x_pi = (b_next - b_prev) / (m_prev - m_next);
                        double y_pi = m_prev * x_pi + b_prev;

                        // 4. Clacule Curve
                        double theta = Math.Abs(m_next - m_prev);
                        double L = kv * theta;
                        double T = L / 2.0;

                        double x_pcv = x_pi - T;
                        x_ptv = x_pi + T;

                        // 5. VALIDATION
                        // Limit 1: x_pcv must be > Previous Slope's Start (p_prev_start.X)
                        // This ensures we don't eat into the previous curve.
                        if (x_pcv < p_prev_start.X)
                        {
                             MessageBox.Show($"El Kv ({kv}) es muy grande y la curva se solapa con la anterior.\n" +
                                            $"Inicio Curva (PCV): {x_pcv:F3} < Inicio Recta Anterior: {p_prev_start.X:F3}\n" +
                                            "Reduzca el Kv.");
                            return;
                        }

                        // Limit 2: x_ptv must be < New Slope's End (x4)
                        if (x_ptv > x4)
                        {
                             MessageBox.Show($"El Kv ({kv}) es muy grande y la curva excede el punto final.\n" +
                                            $"Fin Curva (PTV): {x_ptv:F3} > Fin Recta: {x4:F3}\n" +
                                            "Reduzca el Kv.");
                            return;
                        }

                        // 6. UPDATE PREVIOUS SLOPE
                        // The previous slope (which was just a line to infinity/end) now ends at PCV.
                        lastSlope.Puntos[1] = new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, m_prev * x_pcv + b_prev);
                        // Update visual component
                        ComponentePerfil cpLastSlope = listaComponentesPerfil.Last(comp => comp.ObjetoOriginal == lastSlope);
                        cpLastSlope.PuntoFinal = $"({x_pcv:F3}, {lastSlope.Puntos[1].Y:F3})";


                        // 7. ADD NEW PARABOLA (PCV -> PTV)
                        double y_pcv = m_prev * x_pcv + b_prev;
                        y_ptv = m_next * x_ptv + b_next;

                        double a = (m_next - m_prev) / (2 * L);
                        double b = m_prev - 2 * a * x_pcv;
                        double c = y_pcv - a * Math.Pow(x_pcv, 2) - b * x_pcv;

                        List<double> coeffs = new List<double> { a, b, c };
                        List<Logica.PuntoPerfil> poly_perfil = new List<Logica.PuntoPerfil> 
                        { 
                            new Logica.PuntoPerfil { p = new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv), pendiente = m_prev },
                            new Logica.PuntoPerfil { p = new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv), pendiente = m_next }
                        };

                        Logica.Parabola parabola = new Logica.Parabola(coeffs, poly_perfil);
                        parabola.puntoEntrada = new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv);
                        parabola.puntoSalida = new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv);
                        listaParabolas.Add(parabola);

                        listaComponentesPerfil.Add(new ComponentePerfil
                        {
                            Tipo = "PARABOLA",
                            PuntoInicial = $"({x_pcv:F3}, {y_pcv:F3})",
                            PuntoFinal = $"({x_ptv:F3}, {y_ptv:F3})",
                            Kv = kv,
                            ObjetoOriginal = parabola
                        });

                        // 8. ADD NEW SLOPE (PTV -> P4)
                        Logica.Pendiente pNew_obj = new Logica.Pendiente();
                        pNew_obj.Puntos = new List<Autodesk.AutoCAD.Geometry.Point2d>
                        {
                            new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv),
                            new Autodesk.AutoCAD.Geometry.Point2d(x4, y4)
                        };
                        listaRectas.Add(pNew_obj);

                        ComponentePerfil cpNewSlope = new ComponentePerfil
                        {
                            Tipo = "RECTA",
                            PuntoInicial = $"({x_ptv:F3}, {y_ptv:F3})",
                            PuntoFinal = $"({x4:F4}, {y4:F4})",
                            Kv = 0,
                            Pendiente = m_next * 100,
                            ObjetoOriginal = pNew_obj
                        };
                        
                        // Promocionar la vista previa a entidad permanente (en la segunda pendiente del encadenamiento)
                        LimpiarVistaPrevia(_idPreview2);
                        cpNewSlope.EntityId = ObjectId.Null;

                        if (cpNewSlope.EntityId == ObjectId.Null || cpNewSlope.EntityId.IsErased)
                        {
                            cpNewSlope.EntityId = DibujarLineaEnAutoCAD(
                                new Point2d(x_ptv, y_ptv), 
                                new Point2d(x4, y4), 
                                n_escala);
                        }
                        else
                        {
                            CambiarColorEntidad(cpNewSlope.EntityId, 1);
                        }
                        
                        listaComponentesPerfil.Add(cpNewSlope);

                    }

                    // Update UI for chaining: Set next Start Point to the last calculated PTV
                    // Previous Exit Slope START = PTV
                    // Previous Exit Slope END = P4 (x4, y4)
                    
                    P_ini_x.Texts = x_ptv.ToString("F3");
                    P_ini_y.Texts = y_ptv.ToString("F3");

                    P_fin_x.Texts = x4.ToString("F3");
                    P_fin_y.Texts = y4.ToString("F3");

                    // Clear other fields to force user input for next segment
                    P_ini_x_2.Texts = "";
                    P_ini_y_2.Texts = "";
                    P_fin_x_2.Texts = "";
                    P_fin_y_2.Texts = "";
                    t_kv.Clear();

                    bindingSource.ResetBindings(false);
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al añadir entidad: {ex.Message}");
            }
        }

        // Estructura auxiliar para manejar datos de rectas (incluso si no existen visualmente)
        private struct RectaInfo
        {
            public double m; // Pendiente
            public double b; // Ordenada en el origen (y = mx + b)
            public double x_start_original; // Inicio teórico/original
            public double x_end_original;   // Fin teórico/original
            public bool IsValid;
        }

        private RectaInfo ExtrapolarRecta(ComponentePerfil comp)
        {
            if (comp.Tipo == "RECTA" && comp.ObjetoOriginal is Logica.Pendiente p)
            {
                if (p.Puntos != null && p.Puntos.Count >= 2)
                {
                    Point2d p1 = p.Puntos.First();
                    Point2d p2 = p.Puntos.Last();
                    
                    double m = (p2.Y - p1.Y) / (p2.X - p1.X);
                    double b = p1.Y - m * p1.X;
                    
                    return new RectaInfo { m = m, b = b, x_start_original = p1.X, x_end_original = p2.X, IsValid = true };
                }
            }
            return new RectaInfo { IsValid = false };
        }

        private Point2d? CalcularInterseccion(RectaInfo r1, RectaInfo r2)
        {
            // r1: y = m1*x + b1
            // r2: y = m2*x + b2
            // m1*x + b1 = m2*x + b2  =>  x*(m1-m2) = b2-b1
            
            if (Math.Abs(r1.m - r2.m) < 1e-6) return null; // Paralelas
            
            double x = (r2.b - r1.b) / (r1.m - r2.m);
            double y = r1.m * x + r1.b;
            
            return new Point2d(x, y);
        }

        private void B_insertar_Click(object sender, EventArgs e)
        {
            // PROCESO DE INSERCIÓN SEGURA DE PENDIENTE
            // 1. Validar selección
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una RECTA existente donde o después de la cual desea insertar la nueva pendiente.");
                return;
            }

            int indexSeleccionado = dataGridView1.CurrentRow.Index;
            ComponentePerfil compSeleccionado = listaComponentesPerfil[indexSeleccionado];

            // Solo permitimos insertar si seleccionamos una RECTA.
            // Si es Parábola, se asume que se quiere insertar ENTRE las dos rectas de esa parábola.
            // Para simplificar, forzamos seleccionar una Recta.
            if (compSeleccionado.Tipo != "RECTA")
            {
                MessageBox.Show("Por favor, seleccione una RECTA para tomar como referencia de inserción.");
                return;
            }

            // 2. Leer datos de la NUEVA pendiente del formulario
            // Usamos P_ini_x_2 ... P_fin_y_2 como datos de la "Nueva Pendiente a Insertar"?
            // O usamos los inputs genericos?
            // Asumamos que el usuario ha rellenado P_ini_x, P_ini_y, P_fin_x, P_fin_y con la geometria DESEADA de la nueva recta.
            // IMPORTANTE: La "posición" de esta recta (X start, X end) determinará DÓNDE cae.
            
            double nx1, ny1, nx2, ny2;
            if (!double.TryParse(P_ini_x.Text, out nx1) || !double.TryParse(P_ini_y.Text, out ny1) ||
                !double.TryParse(P_fin_x.Text, out nx2) || !double.TryParse(P_fin_y.Text, out ny2))
            {
                 MessageBox.Show("Defina la geometría de la nueva pendiente en los campos de Pendiente 1 (P_ini/fin).");
                 return;
            }
            
            // Info de la Nueva Recta
            double m_new = (ny2 - ny1) / (nx2 - nx1);
            double b_new = ny1 - m_new * nx1;
            RectaInfo rNew = new RectaInfo { m = m_new, b = b_new, x_start_original = nx1, x_end_original = nx2, IsValid = true };


            // 3. Identificar Recta Anterior y Recta Posterior
            // MODIFICACION: Insertar ANTES de la seleccionada.
            // La "Recta Seleccionada" será la POSTERIOR (idxNext).
            // Buscar la ANTERIOR (idxPrev).
            
            int idxNext = indexSeleccionado;
            int idxPrev = -1;

            // Buscar ANTERIOR RECTA
            for (int i = idxNext - 1; i >= 0; i--)
            {
                if (listaComponentesPerfil[i].Tipo == "RECTA")
                {
                    idxPrev = i;
                    break;
                }
            }

            if (idxPrev == -1)
            {
                // Permitir insertar al principio
                //MessageBox.Show("No se encontró una recta anterior. Para insertar al principio, use otra función o asegúrese de tener una recta previa.");
                //return;
            }

            ComponentePerfil cpPrev = null;
            if (idxPrev != -1) cpPrev = listaComponentesPerfil[idxPrev];
            
            ComponentePerfil cpNext = listaComponentesPerfil[idxNext];

            // 4. Extrapolar rectas existentes
            RectaInfo rPrev = new RectaInfo();
            if (idxPrev != -1) rPrev = ExtrapolarRecta(cpPrev);
            
            RectaInfo rNext = ExtrapolarRecta(cpNext);

            if ((idxPrev != -1 && !rPrev.IsValid) || !rNext.IsValid)
            {
                 MessageBox.Show("Error al analizar geometría de las rectas existentes.");
                 return;
            }

            // 5. Calcular NUEVOS PIVs (Intersecciones)
            // PIV 1: Prev + New
            Point2d? piv1 = null;
            if (idxPrev != -1) piv1 = CalcularInterseccion(rPrev, rNew);
            
            // PIV 2: New + Next
            Point2d? piv2 = CalcularInterseccion(rNew, rNext);

            if ((idxPrev != -1 && piv1 == null) || piv2 == null)
            {
                MessageBox.Show("La nueva pendiente es paralela a alguna de las adyacentes. No se puede insertar.");
                return;
            }

            // Validar orden lógico: PIV1.X debe ser < PIV2.X
            if (idxPrev != -1 && piv1.Value.X >= piv2.Value.X)
            {
                MessageBox.Show($"Geometría inválida: El cruce con la anterior (X={piv1.Value.X:F3}) es posterior al cruce con la siguiente (X={piv2.Value.X:F3}).\nLa pendiente nueva cruza en el sentido incorrecto.");
                return;
            }

            // 6. Calcular Curvas
            // Solicitar Kvs al usuario mediante diálogo
            double kv1 = 0;
            double kv2 = 0;
            
            double existingKv = 0;
            // Verificar si hay una parábola ya existente entre prev y next (que vamos a romper)
            // Si idxPrev != -1, la parábola estaría en idxPrev + 1 (si idxNext == idxPrev + 2)
            if (idxPrev != -1 && idxNext == idxPrev + 2)
            {
               existingKv = listaComponentesPerfil[idxPrev + 1].Kv.GetValueOrDefault();
            }

            using (var dlg = new KvPromptDialog(idxPrev != -1, existingKv))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return; // Cancelado por el usuario
                }
                kv1 = dlg.Kv1;
                kv2 = dlg.Kv2;
            }

            // Curva 1 (Izquierda): Entre rPrev y rNew
            double L1 = 0, T1 = 0, x_pcv1 = 0, x_ptv1 = 0;
            
            if (idxPrev != -1)
            {
                double theta1 = Math.Abs(rNew.m - rPrev.m);
                L1 = kv1 * theta1;
                T1 = L1 / 2.0;
                
                x_pcv1 = piv1.Value.X - T1;
                x_ptv1 = piv1.Value.X + T1;
            }
            else
            {
                // Si es la primera, el PTV1 virtual es el inicio del usuario (P_ini_x)
                // OJO: rNew.x_start_original es el inicio geométrico definido por el usuario (P_ini_x).
                x_ptv1 = rNew.x_start_original;
            }
            
            // Curva 2 (Derecha): Entre rNew y rNext
            double theta2 = Math.Abs(rNext.m - rNew.m);
            double L2 = kv2 * theta2;
            double T2 = L2 / 2.0;

            double x_pcv2 = piv2.Value.X - T2;
            double x_ptv2 = piv2.Value.X + T2;

            // 7. VALIDACIÓN DE SOLAPES (CRÍTICO)
            
            // A. Solape Curva 1 con inicio de recta anterior
            if (idxPrev != -1 && x_pcv1 < rPrev.x_start_original)
            {
                MessageBox.Show($"Conflicto: La curva izquierda (Kv={kv1}) invade la recta anterior.\nPCV1 ({x_pcv1:F3}) < Inicio Recta ({rPrev.x_start_original:F3}).\nReduzca Kv o cambie la pendiente.");
                return;
            }

            // B. Solape entre Curva 1 y Curva 2 (¿Cabe la recta nueva?)
            // OJO: Si es la primera pendiente, x_ptv1 es el inicio del usuario.
            if (x_ptv1 > x_pcv2)
            {
                 string msg = (idxPrev != -1) 
                    ? $"Conflicto: Las curvas nuevas se solapan entre sí.\nFin Curva 1 ({x_ptv1:F3}) > Inicio Curva 2 ({x_pcv2:F3}).\nNo hay espacio para la recta intermedia."
                    : $"Conflicto: La curva derecha (Kv={kv2}) invade el punto inicial definido ({x_ptv1:F3}).\nPCV2 ({x_pcv2:F3}) < Inicio ({x_ptv1:F3}).";
                 
                 MessageBox.Show(msg);
                 return;
            }

            // C. Solape Curva 2 con fin de recta posterior
            // rNext.x_end_original es el fin "duro" (ej. final del eje o PCV de curva siguiente-siguiente)
            if (x_ptv2 > rNext.x_end_original)
            {
                 MessageBox.Show($"Conflicto: La curva derecha (Kv={kv2}) invade la recta posterior.\nFin PTV2 ({x_ptv2:F3}) > Fin Recta ({rNext.x_end_original:F3}).");
                 return;
            }

            // 8. SI TODO OK -> APLICAR CAMBIOS
            
            // Estrategia: "Borrar" lo que hay en medio (Parabola vieja si existe) e insertar la secuencia nueva.
            
            // Verificar si habia una parabola ANTES de Next
            // Si insertamos ANTES de Next, hay que ver si Next tenia una Parabola ANTERIOR.
            // idxPrev debería estar justo antes de Next (si había parábola, habría que borrarla).
            
            // Pero nuestra búsqueda de idxPrev salta parábolas.
            // Si idxPrev != -1, entre Prev y Next podría haber una parábola.
            // idxPrev es el indice de la recta anterior.
            // Si hay una parábola, estará en idxPrev + 1.
            // Y Next estará en idxPrev + 2.
            // Así que si idxNext == idxPrev + 2, borramos la parábola.
            
            if (idxPrev != -1 && idxNext == idxPrev + 2)
            {
               BorrarComponente(idxPrev + 1); // Borramos la parabola antigua
               // Al borrar, idxNext disminuye en 1. Ahora idxNext es idxPrev + 1.
               idxNext = idxPrev + 1;
            }
            // Si insertamos al principio (idxPrev == -1), no debería haber parábola antes de Next (pq es el primero),
            // SALVO que Next ya no sea el primero real (hubiera una parábola huérfana al principio? No debería).
            
            // Ahora tenemos [Prev] [Next] contiguos (o [Next] al principio).
            
            // A. MODIFICAR PREV (Hasta PCV1)
            if (idxPrev != -1)
            {
                Logica.Pendiente objPrev = cpPrev.ObjetoOriginal as Logica.Pendiente;
                objPrev.Puntos[1] = new Point2d(x_pcv1, rPrev.m * x_pcv1 + rPrev.b);
                cpPrev.PuntoFinal = $"({x_pcv1:F3}, {objPrev.Puntos[1].Y:F3})";
                ActualizarEntidadAutoCAD(cpPrev, objPrev.Puntos[0], objPrev.Puntos[1]);
            }
            
            // B. INSERTAR PARABOLA 1 (PCV1 -> PTV1)
            if (idxPrev != -1 && L1 > 0)
            {
                CrearEInsertarParabola(idxPrev + 1, rPrev.m, rNew.m, L1, x_pcv1, rPrev.m * x_pcv1 + rPrev.b, x_ptv1, rNew.m * x_ptv1 + rNew.b, kv1);
                idxNext++; // Desplazamos indice de Next
            }

            // C. INSERTAR NUEVA RECTA (PTV1 -> PCV2)
            Logica.Pendiente objNew = new Logica.Pendiente();
            double y_start_new = rNew.m * x_ptv1 + rNew.b;
            double y_end_new = rNew.m * x_pcv2 + rNew.b;
            
            objNew.Puntos = new List<Point2d> { new Point2d(x_ptv1, y_start_new), new Point2d(x_pcv2, y_end_new) };
            
            ComponentePerfil cpNew = new ComponentePerfil
            {
                Tipo = "RECTA",
                PuntoInicial = $"({x_ptv1:F3}, {y_start_new:F3})",
                PuntoFinal = $"({x_pcv2:F3}, {y_end_new:F3})",
                Pendiente = m_new * 100,
                ObjetoOriginal = objNew
            };
            
            // Dibujar
            double n_escala = calculoPolilineaPerfil != null ? (double)calculoPolilineaPerfil.Escala : 1.0;
            cpNew.EntityId = DibujarLineaEnAutoCAD(objNew.Puntos[0], objNew.Puntos[1], n_escala);
            
            // Insertar en listas
            // Si idxPrev != -1, insertamos después de Prev + posible parabola
            // Si idxPrev == -1, insertamos al PRINCIPIO (índice 0 o después de posible parábola inicial que no debería haber).
            // Pero si idxPrev == -1, idxNext es actualmente 0 (o 0 tras borrar parábola).
            // Entonces insertamos en 0.
            
            int insertIndex = (idxPrev != -1) ? (idxNext) : 0; 
            // idxNext ya se desplazó si hubo parábola 1.
            // Si hubo parábola 1, idxNext subió 1. insertIndex sería prev+2 == next.
            // Asi que insertamos en insertIndex (que es idxNext actual).
            
            // Ojo: listaRectas
            if (idxPrev != -1)
            {
                 Logica.Pendiente objPrev = cpPrev.ObjetoOriginal as Logica.Pendiente;
                 int idxObjPrev = listaRectas.IndexOf(objPrev);
                 listaRectas.Insert(idxObjPrev + 1, objNew);
            }
            else
            {
                listaRectas.Insert(0, objNew);
            }
            
            listaComponentesPerfil.Insert(insertIndex, cpNew); 
            
            idxNext++; // Desplazamos
            
            // D. INSERTAR PARABOLA 2 (PCV2 -> PTV2)
            if (L2 > 0)
            {
                // Indice donde va: Despues de la nueva recta
                int idxPara2 = insertIndex + 1;
                CrearEInsertarParabola(idxPara2, rNew.m, rNext.m, L2, x_pcv2, rNew.m * x_pcv2 + rNew.b, x_ptv2, rNext.m * x_ptv2 + rNext.b, kv2);
                idxNext++;
            }
            
            // E. MODIFICAR NEXT (Desde PTV2)
            // cpNext sigue apuntando al objeto correcto
            Logica.Pendiente objNext = cpNext.ObjetoOriginal as Logica.Pendiente;
            double y_ptv2 = rNext.m * x_ptv2 + rNext.b;
            objNext.Puntos[0] = new Point2d(x_ptv2, y_ptv2);
            cpNext.PuntoInicial = $"({x_ptv2:F3}, {y_ptv2:F3})";
            ActualizarEntidadAutoCAD(cpNext, objNext.Puntos[0], objNext.Puntos[1]);

            // IMPORTANTE: Reordenar las listas lógicas por coordenada X para asegurar que la rotulación
            // y el cálculo subsecuente procesen los elementos en el orden geométrico correcto.
            // Especialmente listaParabolas, ya que las nuevas se añadieron al final.
            listaRectas.Sort((p1, p2) => 
            {
                if (p1.Puntos == null || p1.Puntos.Count == 0) return 0;
                if (p2.Puntos == null || p2.Puntos.Count == 0) return 0;
                return p1.Puntos[0].X.CompareTo(p2.Puntos[0].X);
            });

            listaParabolas.Sort((p1, p2) => p1.puntoEntrada.X.CompareTo(p2.puntoEntrada.X));

            bindingSource.ResetBindings(false);
            MessageBox.Show("Pendiente insertada correctamente.");

            // Actualizar UI con datos de la última pendiente (para seguir añadiendo si se desea)
            // Buscar la última Recta en la lista
            if (listaComponentesPerfil.Count > 0)
            {
                ComponentePerfil lastComp = listaComponentesPerfil.Last();
                // Si lo último es una parábola (raro, pero posible si no hay recta final), buscamos la anterior
                if (lastComp.Tipo != "RECTA" && listaComponentesPerfil.Count > 1)
                {
                    lastComp = listaComponentesPerfil[listaComponentesPerfil.Count - 2];
                }

                if (lastComp.Tipo == "RECTA" && lastComp.ObjetoOriginal is Logica.Pendiente pLast)
                {
                    if (pLast.Puntos != null && pLast.Puntos.Count >= 2)
                    {
                        // En AniadirEntidadNueva, se prepara para el SIGUIENTE segmento
                        // Poniendo en P_ini el P_fin del último.
                        // El usuario dice: "que ponga en la primera pendiente los datos de la ultima pendiente"
                        // Datos = Start -> End
                        // PERO AniadirEntidadNueva pone:
                        // P_ini = x_ptv (Start of last segment)
                        // P_fin = x4 (End of last segment)
                        // Así que rellenamos con la geometría de la última recta.
                        
                        P_ini_x.Texts = pLast.Puntos.First().X.ToString("F3");
                        P_ini_y.Texts = pLast.Puntos.First().Y.ToString("F3");
                        P_fin_x.Texts = pLast.Puntos.Last().X.ToString("F3");
                        P_fin_y.Texts = pLast.Puntos.Last().Y.ToString("F3");
                    }
                }
            }

            // Limpiar resto de campos
            P_ini_x_2.Texts = "";
            P_ini_y_2.Texts = "";
            P_fin_x_2.Texts = "";
            P_fin_y_2.Texts = "";
            t_kv.Clear();
        }

        private void CrearEInsertarParabola(int insertIndex, double m1, double m2, double L, double x_pcv, double y_pcv, double x_ptv, double y_ptv, double kv)
        {
             double a = (m2 - m1) / (2 * L);
             double b = m1 - 2 * a * x_pcv;
             double c = y_pcv - a * Math.Pow(x_pcv, 2) - b * x_pcv;
             
             List<double> coeffs = new List<double> { a, b, c };
             List<Logica.PuntoPerfil> poly = new List<Logica.PuntoPerfil>
             {
                 new Logica.PuntoPerfil { p = new Point2d(x_pcv, y_pcv), pendiente = m1 },
                 new Logica.PuntoPerfil { p = new Point2d(x_ptv, y_ptv), pendiente = m2 }
             };
             
             Logica.Parabola para = new Logica.Parabola(coeffs, poly);
             para.puntoEntrada = new Point2d(x_pcv, y_pcv);
             para.puntoSalida = new Point2d(x_ptv, y_ptv);
             para.max_min = kv;
             
             listaParabolas.Add(para); // Añadir al final de la lista logica (orden no critico aqui si no se usa por indice)
             
             ComponentePerfil cpPara = new ComponentePerfil
             {
                 Tipo = "PARABOLA",
                 PuntoInicial = $"({x_pcv:F3}, {y_pcv:F3})",
                 PuntoFinal = $"({x_ptv:F3}, {y_ptv:F3})",
                 Kv = kv,
                 ObjetoOriginal = para
             };
             
             listaComponentesPerfil.Insert(insertIndex, cpPara);
        }

        private void b_borrarentidad_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            
            int rowIndex = dataGridView1.CurrentRow.Index;
            if (rowIndex < 0 || rowIndex >= listaComponentesPerfil.Count) return;

            ComponentePerfil comp = listaComponentesPerfil[rowIndex];

            if (comp.Tipo == "PARABOLA")
            {
                // Si es parábola, no se borra directamente porque depende de las pendientes
                // Lo que hacemos es ponerla en modo edición para que el usuario cambie el Kv o las pendientes
                MessageBox.Show("Las parábolas no se pueden eliminar de forma aislada ya que dependen de las pendientes adyacentes.\n\n" +
                                "Se activará el modo edición para que pueda modificar el Kv o las pendientes que la definen.\n" +
                                "Si desea eliminarla, borre una de las rectas adyacentes.", 
                                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                CargarDatosEdicion(rowIndex);
            }
            else if (comp.Tipo == "RECTA")
            {
                // Si es recta, comprobamos si tiene parábolas asociadas (anterior o posterior)
                // y avisamos al usuario que se borrarán también.
                
                List<int> indicesABorrar = new List<int>();
                indicesABorrar.Add(rowIndex);

                bool tieneParabolaAnterior = false;
                bool tieneParabolaPosterior = false;

                // Chequear anterior
                if (rowIndex - 1 >= 0)
                {
                    if (listaComponentesPerfil[rowIndex - 1].Tipo == "PARABOLA")
                    {
                        indicesABorrar.Add(rowIndex - 1);
                        tieneParabolaAnterior = true;
                    }
                }

                // Chequear posterior
                if (rowIndex + 1 < listaComponentesPerfil.Count)
                {
                    if (listaComponentesPerfil[rowIndex + 1].Tipo == "PARABOLA")
                    {
                        indicesABorrar.Add(rowIndex + 1);
                        tieneParabolaPosterior = true;
                    }
                }

                string mensaje = "Está a punto de borrar una RECTA.";
                if (tieneParabolaAnterior || tieneParabolaPosterior)
                {
                    mensaje += "\n\nAVISO: Esta recta tiene curvas verticales asociadas que también serán eliminadas para mantener la coherencia.";
                }
                mensaje += "\n\n¿Desea continuar?";

                if (MessageBox.Show(mensaje, "Confirmar borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    // Borrar en orden descendente para no afectar los índices de los que quedan por borrar
                    indicesABorrar.Sort((a, b) => b.CompareTo(a)); // Orden descendente

                    foreach (int idx in indicesABorrar)
                    {
                        BorrarComponente(idx);
                    }

                    CambiarModoEdicion(false); // Resetear modo edición tras borrar
                }
            }
        }

        private void AniadirEntidad_Click(object sender, EventArgs e)
        {
            if (_editMode)
            {
                GuardarCambios();
            }
            else
            {
                AniadirEntidadNueva();
            }
        }

        private void GuardarCambios()
        {
            // 1. Obtener valores de los TextBox
            double x1, y1, x2, y2, x3, y3, x4, y4, kv;
            
            if (!double.TryParse(P_ini_x.Text, out x1) || !double.TryParse(P_ini_y.Text, out y1) ||
                !double.TryParse(P_fin_x.Text, out x2) || !double.TryParse(P_fin_y.Text, out y2))
            {
                MessageBox.Show("Coordenadas de la primera pendiente inválidas.");
                return;
            }

            if (!double.TryParse(P_ini_x_2.Text, out x3) || !double.TryParse(P_ini_y_2.Text, out y3) ||
                !double.TryParse(P_fin_x_2.Text, out x4) || !double.TryParse(P_fin_y_2.Text, out y4))
            {
                 MessageBox.Show("Coordenadas de la segunda pendiente inválidas.");
                 return;
            }

            if (!double.TryParse(t_kv.Text, out kv)) kv = 0;
            bool tieneKv = kv != 0;

            // 2. Identificar componentes a actualizar
            if (_editIndex < 0 || _editIndex >= listaComponentesPerfil.Count) return;

            double m1 = (y2 - y1) / (x2 - x1);
            double m2 = (y4 - y3) / (x4 - x3);
            
            double b1 = y1 - m1 * x1;
            double b2 = y3 - m2 * x3;
            
            double x_piv = (b2 - b1) / (m1 - m2);
            double y_piv = m1 * x_piv + b1;

            if (tieneKv && kv > 0)
            {
                // --- CASO CON PARABOLA ---
                double theta = Math.Abs(m2 - m1);
                double L = kv * theta;
                double T = L / 2.0;

                double x_pcv = x_piv - T;
                double x_ptv = x_piv + T;
                
                if (x_pcv < x1) { MessageBox.Show("Kv demasiado grande: PCV antes de inicio."); return; }
                if (x_ptv > x4) { MessageBox.Show("Kv demasiado grande: PTV después de final."); return; }

                double y_pcv = m1 * x_pcv + b1;
                double y_ptv = m2 * x_ptv + b2;

                double a = (m2 - m1) / (2 * L);
                double b_param = m1 - 2 * a * x_pcv;
                double c_param = y_pcv - a * Math.Pow(x_pcv, 2) - b_param * x_pcv;

                int idxRecta1 = _editIndex;
                int idxParabola = -1;
                int idxRecta2 = -1;

                ComponentePerfil cpRecta1 = listaComponentesPerfil[idxRecta1];
                Logica.Pendiente p1_obj = cpRecta1.ObjetoOriginal as Logica.Pendiente;
                
                p1_obj.Puntos[0] = new Autodesk.AutoCAD.Geometry.Point2d(x1, y1);
                p1_obj.Puntos[1] = new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv);
                
                cpRecta1.PuntoInicial = $"({x1:F3}, {y1:F3})";
                cpRecta1.PuntoFinal = $"({x_pcv:F3}, {y_pcv:F3})";
                cpRecta1.Pendiente = m1 * 100;

                ActualizarEntidadAutoCAD(cpRecta1, new Point2d(x1, y1), new Point2d(x_pcv, y_pcv));

                ComponentePerfil cpParabola = null;
                bool existeParabola = false;
                
                if (idxRecta1 + 1 < listaComponentesPerfil.Count && listaComponentesPerfil[idxRecta1 + 1].Tipo == "PARABOLA")
                {
                    idxParabola = idxRecta1 + 1;
                    cpParabola = listaComponentesPerfil[idxParabola];
                    existeParabola = true;
                    idxRecta2 = idxRecta1 + 2; 
                }
                else
                {
                    idxRecta2 = idxRecta1 + 1;
                }

                if (existeParabola)
                {
                    Logica.Parabola parab_obj = cpParabola.ObjetoOriginal as Logica.Parabola;
                                        
                    List<double> coeffs = new List<double> { a, b_param, c_param };
                    List<Logica.PuntoPerfil> poly_perfil = new List<Logica.PuntoPerfil> 
                    { 
                        new Logica.PuntoPerfil { p = new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv), pendiente = m1 },
                        new Logica.PuntoPerfil { p = new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv), pendiente = m2 }
                    };
                    
                    Logica.Parabola nuevaParabola = new Logica.Parabola(coeffs, poly_perfil);
                    nuevaParabola.puntoEntrada = new Autodesk.AutoCAD.Geometry.Point2d(x_pcv, y_pcv);
                    nuevaParabola.puntoSalida = new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv);
                    
                    int indexInListParabolas = listaParabolas.IndexOf(parab_obj);
                    if (indexInListParabolas != -1) listaParabolas[indexInListParabolas] = nuevaParabola;
                    
                    cpParabola.ObjetoOriginal = nuevaParabola;
                    cpParabola.PuntoInicial = $"({x_pcv:F3}, {y_pcv:F3})";
                    cpParabola.PuntoFinal = $"({x_ptv:F3}, {y_ptv:F3})";
                    cpParabola.Kv = kv;
                }

                if (idxRecta2 < listaComponentesPerfil.Count)
                {
                    ComponentePerfil cpRecta2 = listaComponentesPerfil[idxRecta2];
                    Logica.Pendiente p2_obj = cpRecta2.ObjetoOriginal as Logica.Pendiente;

                    p2_obj.Puntos[0] = new Autodesk.AutoCAD.Geometry.Point2d(x_ptv, y_ptv);
                    p2_obj.Puntos[1] = new Autodesk.AutoCAD.Geometry.Point2d(x4, y4);

                    cpRecta2.PuntoInicial = $"({x_ptv:F3}, {y_ptv:F3})";
                    cpRecta2.PuntoFinal = $"({x4:F4}, {y4:F4})";
                    cpRecta2.Pendiente = m2 * 100;

                     ActualizarEntidadAutoCAD(cpRecta2, new Point2d(x_ptv, y_ptv), new Point2d(x4, y4));
                }
            }
            else
            {
                int idxRecta1 = _editIndex;
                int idxRecta2 = -1;
                
                if (idxRecta1 + 1 < listaComponentesPerfil.Count && listaComponentesPerfil[idxRecta1 + 1].Tipo == "PARABOLA")
                {
                    BorrarComponente(idxRecta1 + 1); 
                    idxRecta2 = idxRecta1 + 1;
                }
                else
                {
                    idxRecta2 = idxRecta1 + 1;
                }

                ComponentePerfil cpRecta1 = listaComponentesPerfil[idxRecta1];
                Logica.Pendiente p1_obj = cpRecta1.ObjetoOriginal as Logica.Pendiente;
                
                p1_obj.Puntos[0] = new Autodesk.AutoCAD.Geometry.Point2d(x1, y1);
                p1_obj.Puntos[1] = new Autodesk.AutoCAD.Geometry.Point2d(x_piv, y_piv);
                
                cpRecta1.PuntoInicial = $"({x1:F3}, {y1:F3})";
                cpRecta1.PuntoFinal = $"({x_piv:F3}, {y_piv:F3})";
                cpRecta1.Pendiente = m1 * 100;
                
                ActualizarEntidadAutoCAD(cpRecta1, new Point2d(x1, y1), new Point2d(x_piv, y_piv));

                if (idxRecta2 < listaComponentesPerfil.Count)
                {
                    ComponentePerfil cpRecta2 = listaComponentesPerfil[idxRecta2];
                    Logica.Pendiente p2_obj = cpRecta2.ObjetoOriginal as Logica.Pendiente;
                    
                    p2_obj.Puntos[0] = new Autodesk.AutoCAD.Geometry.Point2d(x_piv, y_piv);
                    p2_obj.Puntos[1] = new Autodesk.AutoCAD.Geometry.Point2d(x4, y4);
                    
                    cpRecta2.PuntoInicial = $"({x_piv:F3}, {y_piv:F3})";
                    cpRecta2.PuntoFinal = $"({x4:F4}, {y4:F4})";
                    cpRecta2.Pendiente = m2 * 100;
                    
                    ActualizarEntidadAutoCAD(cpRecta2, new Point2d(x_piv, y_piv), new Point2d(x4, y4));
                }
            }

            PropagarCambiosContinuidad(_editIndex + (tieneKv ? 2 : 1), new Point2d(x4, y4));

            bindingSource.ResetBindings(false);
            CambiarModoEdicion(false); 
            
             LimpiarVistaPrevia(_idPreview1);
             LimpiarVistaPrevia(_idPreview2);
        }

        private void c_t_acuerdos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if we need to reset
            if (listaComponentesPerfil.Count > 0)
            {
                // Trigger New Profile logic
                bool result = IniciarNuevoPerfil();
                if (!result)
                {
                    // User cancelled, revert selection
                    // Avoid triggering this event again if possible, or handle re-entry
                    this.c_t_acuerdos.SelectedIndexChanged -= this.c_t_acuerdos_SelectedIndexChanged;
                    this.c_t_acuerdos.SelectedIndex = _previousAgreementTypeIndex;
                    this.c_t_acuerdos.SelectedIndexChanged += this.c_t_acuerdos_SelectedIndexChanged;
                    return;
                }
            }

            if (c_t_acuerdos.SelectedIndex == 0) // Parabolico
            {
                 l_kv.Text = "Kv:";
            }
            else // Circular
            {
                 l_kv.Text = "Radio:";
            }

            // Update previous index
            _previousAgreementTypeIndex = c_t_acuerdos.SelectedIndex;
        }

        private void PropagarCambiosContinuidad(int startIndex, Point2d newStartPoint)
        {
            if (startIndex + 1 < listaComponentesPerfil.Count)
            {
                ComponentePerfil next = listaComponentesPerfil[startIndex + 1];
                if (next.Tipo == "RECTA" && next.ObjetoOriginal is Logica.Pendiente p)
                {
                    p.Puntos[0] = newStartPoint; 
                    
                    double dx = p.Puntos[1].X - p.Puntos[0].X;
                    double dy = p.Puntos[1].Y - p.Puntos[0].Y;
                    double m = dy / dx;
                    
                    next.PuntoInicial = $"({p.Puntos[0].X:F3}, {p.Puntos[0].Y:F3})";
                    next.Pendiente = m * 100;
                    
                    ActualizarEntidadAutoCAD(next, p.Puntos[0], p.Puntos[1]);
                }
            }
        }

        private void ActualizarEntidadAutoCAD(ComponentePerfil comp, Point2d p1, Point2d p2)
        {
            if (comp.EntityId != ObjectId.Null && !comp.EntityId.IsErased)
            {
                try
                {
                    Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    using (doc.LockDocument())
                    {
                        using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
                        {
                            Entity ent = tr.GetObject(comp.EntityId, OpenMode.ForWrite) as Entity;
                            if (ent is Line line)
                            {
                                double n_escala = calculoPolilineaPerfil != null ? (double)calculoPolilineaPerfil.Escala : 1.0;
                                double x_ins = calculoPolilineaPerfil != null ? calculoPolilineaPerfil.x_ins : 0;
                                double y_ins = calculoPolilineaPerfil != null ? calculoPolilineaPerfil.y_ins : 0;

                                line.StartPoint = new Point3d(p1.X + x_ins, p1.Y * n_escala + y_ins, 0);
                                line.EndPoint = new Point3d(p2.X + x_ins, p2.Y * n_escala + y_ins, 0);
                            }
                            tr.Commit();
                        }
                    }
                }
                catch { }
            }
        }

//        private void b_guardareje_Click(object sender, EventArgs e)
//        {
//
//        }

//        private void b_ejenuevo_Click(object sender, EventArgs e)
//        {
//
//        }

//        private void b_borrareje_Click(object sender, EventArgs e)
//        {
//
//        }

//        private void b_guardarejes_Click(object sender, EventArgs e)
//        {
//
//        }

//        private void b_cargarejes_Click(object sender, EventArgs e)
//        {
//
//        }

        private void P_ini_x_DoubleClick(object sender, EventArgs e)
        {
            SeleccionarPuntoEnAutoCAD(P_ini_x, P_ini_y);
        }

        private void P_fin_x_DoubleClick(object sender, EventArgs e)
        {
            SeleccionarPuntoEnAutoCAD(P_fin_x, P_fin_y);
        }

        private void textBox5_DoubleClick(object sender, EventArgs e)
        {
            SeleccionarPuntoEnAutoCAD(P_ini_x_2, P_ini_y_2);
        }

        private void textBox7_DoubleClick(object sender, EventArgs e)
        {
            SeleccionarPuntoEnAutoCAD(P_fin_x_2, P_fin_y_2);
        }

        private void SeleccionarPuntoEnAutoCAD(TextBoxPersonalizado txtX, TextBoxPersonalizado txtY)
        {
            // 1. Check for Active Document
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            if (acDoc == null) return;

            // Hide the window to allow selection
            this.Hide();

            try
            {
                // 2. Prompt user for point
                PromptPointOptions pPtOpts = new PromptPointOptions("");
                pPtOpts.Message = "\nSeleccione un punto del perfil: ";
                PromptPointResult pPtRes = acDoc.Editor.GetPoint(pPtOpts);
                
                if (pPtRes.Status == PromptStatus.OK)
                {
                    Point3d pCad = pPtRes.Value;

                    // 3. Get Scale and Offsets
                    double x_ins = 0;
                    double y_ins = 0;
                    double n_escala = 1.0;

                    if (calculoPolilineaPerfil != null)
                    {
                        x_ins = calculoPolilineaPerfil.x_ins;
                        y_ins = calculoPolilineaPerfil.y_ins;
                        n_escala = (double)calculoPolilineaPerfil.Escala;
                    }
                    
                    // Avoid division by zero
                    if (Math.Abs(n_escala) < 1e-6) n_escala = 1.0;

                    // 4. Apply Inverse Transformation
                    // Drawing Logic: Y_cad = (Y_real * Scale) + Y_ins
                    // Inverse: Y_real = (Y_cad - Y_ins) / Scale
                    
                    // Drawing Logic: X_cad = X_real + X_ins
                    // Inverse: X_real = X_cad - X_ins
                    
                    double x_real = pCad.X - x_ins;
                    double y_real = (pCad.Y - y_ins) / n_escala;

                    // 5. Update TextBoxes
                    // Using F3 format as seen elsewhere in the file
                    txtX.Texts = x_real.ToString("F3");
                    txtY.Texts = y_real.ToString("F3");
                }
            }
            finally
            {
                // Show the window again
                this.Show();
            }
        }



        private void t_kv_TextChanged(object sender, EventArgs e)
        {

        }

        private void P_fin_x_TextChanged(object sender, EventArgs e)
        {

        }

        private void B_calcular_Click(object sender, EventArgs e)
        {
            // Validar que se haya ingresado un nombre
            if (string.IsNullOrWhiteSpace(B_nombre.Text))
            {
                MessageBox.Show("Debe indicar un nombre para el cálculo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {
                // 1. Obtener la escala vertical
                double n_escala = 1.0;
                if (calculoPolilineaPerfil != null)
                {
                    n_escala = (double)calculoPolilineaPerfil.Escala;
                }

                // 2. Obtener el documento activo de AutoCAD
                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                if (acDoc == null)
                {
                    MessageBox.Show("No hay ningún documento de AutoCAD activo.");
                    return;
                }

                Database acCurDb = acDoc.Database;

                // Limpiar listas de entidades para el cálculo actual
                this.listaRectas.Clear();
                this.listaParabolas.Clear();
                this.listaCurvas.Clear();

                // Prepare for Rotulacion
                Logica.CalculoPolilineaPerfil cpRotulo = new Logica.CalculoPolilineaPerfil();
                if (calculoPolilineaPerfil != null)
                {
                    cpRotulo.x_ins = calculoPolilineaPerfil.x_ins;
                    cpRotulo.y_ins = calculoPolilineaPerfil.y_ins;
                    cpRotulo.Escala = (int)calculoPolilineaPerfil.Escala;
                }
                cpRotulo.Polilinea_Perfil = new List<Logica.PuntoPerfil>();
                cpRotulo.documento = B_nombre.Text;

                // 3. Bloquear el documento y comenzar transacción
                using (DocumentLock acLck = acDoc.LockDocument())
                {
                    using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                    {
                        // --- CLEANUP LOGIC START ---
                        
                        // 1. Delete previous Polyline
                        if (_idUltimaPolilinea.IsValid && !_idUltimaPolilinea.IsErased)
                        {
                            try
                            {
                                DBObject obj = acTrans.GetObject(_idUltimaPolilinea, OpenMode.ForWrite);
                                obj.Erase();
                            }
                            catch { /* Ignore if already deleted or invalid */ }
                        }

                        // 2. Delete previous Rotulacion entities
                        BorrarTrazadoAnterior(cpRotulo.documento);
                        // --- CLEANUP LOGIC END ---

                        // 4. Abrir la tabla de bloques (Model Space)
                        BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // 5. Crear la Polilinea
                        Polyline acPoly = new Polyline();
                        acPoly.SetDatabaseDefaults();
                        
                        // Create a layer if needed or use current
                        // engCadNet.oLayer.addLayer("Perfil-Trazado", 2, false);
                        // acPoly.Layer = "Perfil-Trazado";

                        int index = 0;
                        
                        // Clear the rotulacion points list to avoid accumulation from previous runs
                        if (cpRotulo.Polilinea_Perfil != null)
                        {
                            cpRotulo.Polilinea_Perfil.Clear();
                        }
                        else
                        {
                            cpRotulo.Polilinea_Perfil = new List<Logica.PuntoPerfil>();
                        }
                        var punto_ultimo = new Logica.PuntoPerfil();
                        Logica.Pendiente lastRecta = null;
                        
                        // 6. Recorrer los componentes
                        foreach (var componente in listaComponentesPerfil)
                        {
                            if (componente.Tipo == "RECTA")
                            {
                                if (componente.ObjetoOriginal is Logica.Pendiente linea)
                                {
                                    if (linea.Puntos != null && linea.Puntos.Count >= 2)
                                    {
                                        // Start Point
                                        Point2d p1 = linea.Puntos.First();
                                        // End Point
                                        Point2d p2 = linea.Puntos.Last();
                                        
                                        // System.Windows.Forms.MessageBox.Show($"DEBUG Trazado: Pendiente Points: P1=({p1.X}, {p1.Y}), P2=({p2.X}, {p2.Y})");

                                        double x1_final = p1.X;
                                        double y1_final = p1.Y * n_escala;
                                        double x2_final = p2.X;
                                        double y2_final = p2.Y * n_escala;

                                        // Apply offset if calculoPolilineaPerfil is available
                                        if (calculoPolilineaPerfil != null)
                                        {
                                            // El usuario quiere:
                                             // 1. El primer punto sea siempre el punto inicial del eje cargado (x_inicial) + desplazamiento (x_ins)
                                             //    OJO: p1.X es relativo al inicio del trazado nuevo (generalmente empieza en 0).
                                             //    Si el perfil nuevo empieza en 0, entonces:
                                             //    X_dibujo = p1.X + calculoPolilineaPerfil.x_inicial + calculoPolilineaPerfil.x_ins ?
                                             //    Revisando la petición: "el primer punto se siempre el punto inicial del eje cargado ... y la z inicial sea la y del punto inicial teniendo en cuenta el desplazamiento"
                                             
                                             // El "Punto Inicial del Eje Cargado" suele ser (0, Z_ini) en coordenadas locales del perfil.
                                             // Pero en AutoCAD está en (x_ins, y_ins).
                                             // Si el usuario "insertó" el eje, x_ins e y_ins son las coordenadas de inserción.
                                             
                                             // Asumimos que p1.X es la distancia desde el inicio del nuevo perfil.
                                             // Entonces:
                                             
                                             double base_x = calculoPolilineaPerfil.x_ins;
                                             double base_y = calculoPolilineaPerfil.y_ins; 
                                    
                                             // Modificacion: "tener en cuenta el desplazamiento que ha tenido el usuario al insertar el eje".
                                             // Si el eje original empieza en X=0, Y=Z_start_terrain?
                                             // En CalculoPolilineaPerfil.Dibujar_Trazado, se usa: pk + x_ins, y * escala + y_ins
                                             
                                             // Vamos a replicar esa lógica.
                                             x1_final = p1.X + base_x;
                                             y1_final = (p1.Y * n_escala) + base_y;
                                    
                                             x2_final = p2.X + base_x;
                                             y2_final = (p2.Y * n_escala) + base_y;
                                         }
                                    
                                         // Si es el primer punto de toda la polilínea, añadirlo
                                         if (index == 0)
                                         {
                                            acPoly.AddVertexAt(index, new Point2d(x1_final, y1_final), 0, 0, 0);
                                            index++;
                                            
                                            // Add to Rotulacion
                                            Logica.PuntoPerfil ppStart = new Logica.PuntoPerfil();
                                            // p1 is local, but maybe Rotular expects local relative to start?
                                            // If we use p1.X directly, that's what the user sees in the text box.
                                            ppStart.p = new Point2d(p1.X, p1.Y);
                                            cpRotulo.Polilinea_Perfil.Add(ppStart);
                                        }

                                        // Añadir punto final
                                        acPoly.AddVertexAt(index, new Point2d(x2_final, y2_final), 0, 0, 0);
                                        index++;
                                        
                                         // Add to Rotulacion
                                        Logica.PuntoPerfil ppEnd = new Logica.PuntoPerfil();
                                        ppEnd.p = new Point2d(p2.X, p2.Y);
                                        cpRotulo.Polilinea_Perfil.Add(ppEnd);
                                        punto_ultimo = ppEnd;
                                        lastRecta = linea;
                                        this.listaRectas.Add(linea);
                                    }
                                }
                            }
                            else if (componente.Tipo == "PARABOLA")
                            {
                                if (componente.ObjetoOriginal is Logica.Parabola parabola)
                                {
                                    this.listaParabolas.Add(parabola);
                                    if (parabola.polilinea_perfil == null) parabola.polilinea_perfil = new List<Logica.PuntoPerfil>();
                                    parabola.polilinea_perfil.Clear();
                                    // Comprobar si faltan los parámetros (caso de ser la primera/última componente o carga JSON)
                                    if (parabola.parabola == null || parabola.parabola.Count < 3)
                                    {
                                        Logica.Pendiente rectaRef = lastRecta;
                                        bool tangenciaAlFinal = false;
                                        if (rectaRef == null)
                                        {
                                            var nextR = listaComponentesPerfil.FirstOrDefault(c => c.Tipo == "RECTA" && c.ObjetoOriginal is Logica.Pendiente);
                                            if (nextR != null) { rectaRef = nextR.ObjetoOriginal as Logica.Pendiente; tangenciaAlFinal = true; }
                                        }

                                        if (rectaRef != null && rectaRef.Puntos.Count >= 2)
                                        {
                                            Point2d pt1 = rectaRef.Puntos[0];
                                            Point2d pt2 = rectaRef.Puntos.Last();
                                            double mRef = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                                            double dX = parabola.puntoSalida.X - parabola.puntoEntrada.X;

                                            if (Math.Abs(dX) > 0.001)
                                            {
                                                double a, b, c;
                                                if (!tangenciaAlFinal)
                                                {
                                                    // Tangencia normal (al inicio)
                                                    a = (parabola.puntoSalida.Y - parabola.puntoEntrada.Y - mRef * dX) / (dX * dX);
                                                    b = mRef - 2 * a * parabola.puntoEntrada.X;
                                                    c = parabola.puntoEntrada.Y - a * Math.Pow(parabola.puntoEntrada.X, 2) - b * parabola.puntoEntrada.X;
                                                }
                                                else
                                                {
                                                    // Tangencia al final (porque usamos la recta siguiente)
                                                    a = (parabola.puntoEntrada.Y - parabola.puntoSalida.Y + mRef * dX) / (dX * dX);
                                                    b = mRef - 2 * a * parabola.puntoSalida.X;
                                                    c = parabola.puntoSalida.Y - a * Math.Pow(parabola.puntoSalida.X, 2) - b * parabola.puntoSalida.X;
                                                }
                                                parabola.parabola = new List<double> { a, b, c };
                                            }
                                        }
                                    }

                                    // Dibujar Parábola
                                    if (parabola.parabola != null && parabola.parabola.Count >= 3)
                                    {
                                        double a = parabola.parabola[0];
                                        double b = parabola.parabola[1];
                                        double c = parabola.parabola[2];
                                        double x_start = parabola.puntoEntrada.X;
                                        double x_end = parabola.puntoSalida.X;
                                        double base_x = calculoPolilineaPerfil != null ? calculoPolilineaPerfil.x_ins : 0;
                                        double base_y = calculoPolilineaPerfil != null ? calculoPolilineaPerfil.y_ins : 0;

                                        double step = 1.0;
                                        double currentX = x_start;
                                        
                                        // Siempre añadir el punto inicial a la propia lista de la parábola
                                        double y_start = a * x_start * x_start + b * x_start + c;
                                        Logica.PuntoPerfil ppS = new Logica.PuntoPerfil { p = new Point2d(x_start, y_start) };
                                        parabola.polilinea_perfil.Add(ppS);

                                        if (index == 0)
                                        {
                                            acPoly.AddVertexAt(index, new Point2d(x_start + base_x, (y_start * n_escala) + base_y), 0, 0, 0);
                                            index++;
                                            cpRotulo.Polilinea_Perfil.Add(ppS);
                                        }
                                        currentX += step;

                                        while (currentX < x_end)
                                        {
                                            double y = a * currentX * currentX + b * currentX + c;
                                            acPoly.AddVertexAt(index, new Point2d(currentX + base_x, (y * n_escala) + base_y), 0, 0, 0);
                                            index++;
                                            Logica.PuntoPerfil ppM = new Logica.PuntoPerfil { p = new Point2d(currentX, y) };
                                            cpRotulo.Polilinea_Perfil.Add(ppM);
                                            parabola.polilinea_perfil.Add(ppM);
                                            currentX += step;
                                        }

                                        double y_e = a * x_end * x_end + b * x_end + c;
                                        acPoly.AddVertexAt(index, new Point2d(x_end + base_x, (y_e * n_escala) + base_y), 0, 0, 0);
                                        index++;
                                        Logica.PuntoPerfil ppE = new Logica.PuntoPerfil { p = new Point2d(x_end, y_e) };
                                        cpRotulo.Polilinea_Perfil.Add(ppE);
                                        parabola.polilinea_perfil.Add(ppE);
                                    }
                                }
                            }
                            else if (componente.Tipo == "CIRCULAR")
                            {
                                if (componente.ObjetoOriginal is EjeDeTrazado.componentes.Curva curva)
                                {
                                    this.listaCurvas.Add(curva);
                                     if (curva.getCentroCurva.coordenadaX == 0 && curva.getCentroCurva.coordenadaY == 0)
                                     {
                                          Logica.Pendiente rectaRef = lastRecta;
                                          bool tangenciaAlFinal = false;
                                          if (rectaRef == null)
                                          {
                                              var nextR = listaComponentesPerfil.FirstOrDefault(c => c.Tipo == "RECTA" && c.ObjetoOriginal is Logica.Pendiente);
                                              if (nextR != null) { rectaRef = nextR.ObjetoOriginal as Logica.Pendiente; tangenciaAlFinal = true; }
                                          }

                                          if (rectaRef != null && rectaRef.Puntos.Count >= 2)
                                          {
                                              Point2d pt1 = rectaRef.Puntos[0];
                                              Point2d pt2 = rectaRef.Puntos.Last();
                                              double mRef = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                                              
                                              double x_pcv = curva.getPuntoEntrada.coordenadaX;
                                              double y_pcv = curva.getPuntoEntrada.coordenadaY;
                                              double x_ptv = curva.getPuntoSalida.coordenadaX;
                                              double y_ptv = curva.getPuntoSalida.coordenadaY;
                                              double radio = curva.getRadio;
                                              
                                              double mod = Math.Sqrt(1 + mRef * mRef);
                                              double ux = 1 / mod; double uy = mRef / mod;
                                              
                                              // Punto de tangencia
                                              double xt = tangenciaAlFinal ? x_ptv : x_pcv;
                                              double yt = tangenciaAlFinal ? y_ptv : y_pcv;

                                              double cx1 = xt - radio * uy; double cy1 = yt + radio * ux;
                                              double cx2 = xt + radio * uy; double cy2 = yt - radio * ux;
                                              
                                              // El otro punto para validar el radio
                                              double xo = tangenciaAlFinal ? x_pcv : x_ptv;
                                              double yo = tangenciaAlFinal ? y_pcv : y_ptv;

                                              double d1 = Math.Sqrt(Math.Pow(xo - cx1, 2) + Math.Pow(yo - cy1, 2));
                                              double d2 = Math.Sqrt(Math.Pow(xo - cx2, 2) + Math.Pow(yo - cy2, 2));
                                              
                                              double cx_c, cy_c;
                                              EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva sentido;
                                              if (Math.Abs(d1 - radio) < Math.Abs(d2 - radio)) { cx_c = cx1; cy_c = cy1; sentido = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario; }
                                              else { cx_c = cx2; cy_c = cy2; sentido = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario; }
                                              
                                              curva = new EjeDeTrazado.componentes.Curva(new tadLayShare.puntos.Punto3d(x_pcv, y_pcv, 0), new tadLayShare.puntos.Punto3d(x_ptv, y_ptv, 0), new tadLayShare.puntos.Punto3d(cx_c, cy_c, 0), radio, x_pcv, 0, sentido);
                                              componente.ObjetoOriginal = curva;
                                          }
                                     }

                                     double cx = curva.getCentroCurva.coordenadaX;
                                     double cy = curva.getCentroCurva.coordenadaY;
                                     double r = curva.getRadio;
                                     double sX = curva.getPuntoEntrada.coordenadaX;
                                     double sY = curva.getPuntoEntrada.coordenadaY;
                                     double eX = curva.getPuntoSalida.coordenadaX;
                                     double eY = curva.getPuntoSalida.coordenadaY;
                                     
                                     double aS = Math.Atan2(sY - cy, sX - cx);
                                     double aE = Math.Atan2(eY - cy, eX - cx);
                                     bool ccw = (curva.getSentido() == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario);
                                     if (ccw) { if (aE < aS) aE += 2 * Math.PI; }
                                     else { if (aE > aS) aE -= 2 * Math.PI; }
                                     
                                     double len = Math.Abs(aE - aS) * r;
                                     int stps = (int)(len / 1.0); if (stps < 2) stps = 2;
                                     double aStep = (aE - aS) / stps;
                                     double bX = calculoPolilineaPerfil != null ? calculoPolilineaPerfil.x_ins : 0;
                                     double bY = calculoPolilineaPerfil != null ? calculoPolilineaPerfil.y_ins : 0;
                                     
                                     for (int i = 0; i <= stps; i++)
                                     {
                                         double th = aS + i * aStep;
                                         double px = cx + r * Math.Cos(th);
                                         double py = cy + r * Math.Sin(th);
                                         if (index == 0 && i == 0)
                                         {
                                             acPoly.AddVertexAt(index, new Point2d(px + bX, (py * n_escala) + bY), 0, 0, 0);
                                             index++;
                                             cpRotulo.Polilinea_Perfil.Add(new Logica.PuntoPerfil { p = new Point2d(px, py) });
                                         }
                                         else if (index > 0 && i > 0) 
                                         {
                                             acPoly.AddVertexAt(index, new Point2d(px + bX, (py * n_escala) + bY), 0, 0, 0);
                                             index++;
                                             cpRotulo.Polilinea_Perfil.Add(new Logica.PuntoPerfil { p = new Point2d(px, py) });
                                         }
                                     }
                                }
                            }
                        }

                        if (acPoly.NumberOfVertices > 0)
                        {
                            acBlkTblRec.AppendEntity(acPoly);
                            acTrans.AddNewlyCreatedDBObject(acPoly, true);
                            _idUltimaPolilinea = acPoly.ObjectId; // Store for next cleanup
                            
                            // Zoom extents to see the result (Optional)
                            // acDoc.Editor.Command("._ZOOM", "_E");
                        }

                        acTrans.Commit();
                    }
                    
                    // Rotulacion Execution
                    try 
                    {
                        cpRotulo.Lista_Rectas = this.listaRectas;
                        cpRotulo.Lista_Parabolas = this.listaParabolas;
                        cpRotulo.Lista_Curvas = this.listaCurvas;
                        
                        if (calculoPolilineaPerfil != null)
                        {
                            double pk_inicio_ref = (this.listaRectas.Count > 0 && this.listaRectas[0].Puntos.Count > 0) ? this.listaRectas[0].Puntos[0].X : 0;
                            
                            // Use the passed text height
                            if (cpRotulo.Lista_Curvas != null && cpRotulo.Lista_Curvas.Count > 0)
                            {
                                cpRotulo.Rotular_Circular(calculoPolilineaPerfil.AlturaTexto, 1, pk_inicio_ref);
                            }
                            else
                            {
                                cpRotulo.Rotular(calculoPolilineaPerfil.AlturaTexto, 1, pk_inicio_ref);
                            }
                        }
                        else
                        {
                             // Fallback default
                             cpRotulo.Rotular(2.5, 1);
                        }
                    }
                    catch(Exception exRot)
                    {
                         MessageBox.Show("Error al rotular: " + exRot.Message);
                    }
                }
                
                acDoc.Editor.WriteMessage("\nPerfil dibujado correctamente.");
                
                // Exportar LandXML e Informe (Métodos Locales)
                if (calculoPolilineaPerfil != null)
                {
                    // No hace falta actualizar calculoPolilineaPerfil porque usaremos nuestros métodos locales
                    ExportarLandXML_Local();
                    GenerarInforme();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dibujar en AutoCAD: " + ex.Message);
            }
        }

        private void CambiarColorEntidad(ObjectId id, short colorIndex)
        {
            if (id == ObjectId.Null || id.IsErased) return;
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                using (doc.LockDocument())
                {
                    using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
                    {
                        Entity ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                        {
                            ent.ColorIndex = colorIndex;
                        }
                        tr.Commit();
                    }
                }
            }
            catch { }
        }

        private void Coordinate_TextChanged(object sender, EventArgs e)
        {
            // Determinar qué pendiente actualizar basándose en el control que lanzó el evento
            Control ctrl = sender as Control;
            if (ctrl == null) return;

            if (ctrl.Name.Contains("_2"))
            {
                ActualizarVistaPrevia(2);
            }
            else
            {
                ActualizarVistaPrevia(1);
            }
        }

        private void LimpiarVistaPrevia(List<ObjectId> previews)
        {
            if (previews == null || previews.Count == 0) return;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())
            {
                using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
                {
                    foreach (var id in previews)
                    {
                        if (id != ObjectId.Null && !id.IsErased)
                        {
                            try { tr.GetObject(id, OpenMode.ForWrite).Erase(); } catch { }
                        }
                    }
                    tr.Commit();
                }
            }
            previews.Clear();
        }

        private void ActualizarVistaPrevia(int pendiente)
        {
            try
            {
                double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                bool p1Valid = false;
                bool p2Valid = false;
                
                List<ObjectId> currentPreviewIds = null;
                
                if (pendiente == 1)
                {
                    p1Valid = double.TryParse(P_ini_x.Text, out x1) && double.TryParse(P_ini_y.Text, out y1);
                    p2Valid = double.TryParse(P_fin_x.Text, out x2) && double.TryParse(P_fin_y.Text, out y2);
                    currentPreviewIds = _idPreview1;
                }
                else
                {
                    p1Valid = double.TryParse(P_ini_x_2.Text, out x1) && double.TryParse(P_ini_y_2.Text, out y1);
                    p2Valid = double.TryParse(P_fin_x_2.Text, out x2) && double.TryParse(P_fin_y_2.Text, out y2);
                    currentPreviewIds = _idPreview2;
                }

                // Borrar la vista previa anterior si existe
                if (currentPreviewIds != null && currentPreviewIds.Count > 0)
                {
                    Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    using (doc.LockDocument())
                    {
                        using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
                        {
                            try
                            {
                                foreach(var id in currentPreviewIds)
                                {
                                    if (id != ObjectId.Null && !id.IsErased)
                                    {
                                        Entity ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                                        if (ent != null) ent.Erase();
                                    }
                                }
                                tr.Commit();
                            }
                            catch { }
                        }
                    }
                    currentPreviewIds.Clear();
                }

                double n_escala = calculoPolilineaPerfil != null ? (double)calculoPolilineaPerfil.Escala : 1.0;
                List<ObjectId> newIds = new List<ObjectId>();

                if (p1Valid && p2Valid)
                {
                    // Ambos puntos: dibujar solo la línea (sin marcas)
                    ObjectId lineId;
                    if (_editMode)
                    {
                        lineId = DibujarLineaEnAutoCAD(new Point2d(x1, y1), new Point2d(x2, y2), n_escala, "Entidad_Trazado_Usuario_Perfil", 6, LineWeight.LineWeight030);
                    }
                    else
                    {
                        lineId = DibujarLineaEnAutoCAD(new Point2d(x1, y1), new Point2d(x2, y2), n_escala);
                    }
                    if (lineId != ObjectId.Null) newIds.Add(lineId);
                }
                else if (p1Valid)
                {
                    // Solo punto inicial -> Dibujar Marca (Cruz con Círculo)
                    ObjectId m = DibujarMarcaPunto(new Point2d(x1, y1), n_escala);
                    if (m != ObjectId.Null) newIds.Add(m);
                }
                // Si solo hay punto final, no se dibuja nada

                if (pendiente == 1) _idPreview1.AddRange(newIds);
                else _idPreview2.AddRange(newIds);
            }
            catch { }
        }

        private void BorrarComponente(int indice)
        {
            if (indice < 0 || indice >= listaComponentesPerfil.Count)
            {
                MessageBox.Show("Índice inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ComponentePerfil componente = listaComponentesPerfil[indice];

            // Borrar la entidad de AutoCAD si existe
            if (componente.EntityId != ObjectId.Null && !componente.EntityId.IsErased)
            {
                try
                {
                    Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    Database acCurDb = acDoc.Database;

                    using (DocumentLock acLck = acDoc.LockDocument())
                    {
                        using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                        {
                            DBObject obj = acTrans.GetObject(componente.EntityId, OpenMode.ForWrite);
                            obj.Erase();
                            acTrans.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al borrar entidad de AutoCAD: {ex.Message}", 
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Borrar de las listas
            if (componente.ObjetoOriginal is Logica.Pendiente)
            {
                listaRectas.Remove(componente.ObjetoOriginal as Logica.Pendiente);
            }
            else if (componente.ObjetoOriginal is Logica.Parabola)
            {
                listaParabolas.Remove(componente.ObjetoOriginal as Logica.Parabola);
            }

            listaComponentesPerfil.RemoveAt(indice);
            bindingSource.ResetBindings(false);
        }

        private ObjectId DibujarLineaEnAutoCAD(Point2d p1, Point2d p2, double escalaVertical, string layerName = null, int colorIndex = 7, LineWeight lineWeight = LineWeight.ByLineWeightDefault)
        {
            try
            {
                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database acCurDb = acDoc.Database;

                using (DocumentLock acLck = acDoc.LockDocument())
                {
                    using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                    {
                        BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // Aplicar escala vertical y offsets si están disponibles
                        double x1 = p1.X;
                        double y1 = p1.Y * escalaVertical;
                        double x2 = p2.X;
                        double y2 = p2.Y * escalaVertical;

                        if (calculoPolilineaPerfil != null)
                        {
                            x1 += calculoPolilineaPerfil.x_ins;
                            y1 += calculoPolilineaPerfil.y_ins;
                            x2 += calculoPolilineaPerfil.x_ins;
                            y2 += calculoPolilineaPerfil.y_ins;
                        }

                        Line acLine = new Line(new Point3d(x1, y1, 0), new Point3d(x2, y2, 0));
                        acLine.ColorIndex = colorIndex;
                        acLine.LineWeight = lineWeight;
                        
                        if (!string.IsNullOrEmpty(layerName))
                        {
                            LayerTable lt = (LayerTable)acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead);
                            if (!lt.Has(layerName))
                            {
                                lt.UpgradeOpen();
                                LayerTableRecord ltr = new LayerTableRecord();
                                ltr.Name = layerName;
                                lt.Add(ltr);
                                acTrans.AddNewlyCreatedDBObject(ltr, true);
                            }
                            acLine.Layer = layerName;
                        }

                        ObjectId lineId = acBlkTblRec.AppendEntity(acLine);
                        acTrans.AddNewlyCreatedDBObject(acLine, true);
                        acTrans.Commit();

                        return lineId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al dibujar línea: {ex.Message}");
                return ObjectId.Null;
            }
        }

        

        private ObjectId DibujarMarcaPunto(Point2d p, double scale)
        {
            try
            {
                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database acCurDb = acDoc.Database;

                using (DocumentLock acLck = acDoc.LockDocument())
                {
                    using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                    {
                        BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite) as BlockTable;
                        BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // Create an anonymous block for the marker
                        BlockTableRecord blkRec = new BlockTableRecord();
                        blkRec.Name = "*U";
                        ObjectId blkId = acBlkTbl.Add(blkRec);
                        acTrans.AddNewlyCreatedDBObject(blkRec, true);

                        // Draw Circle
                        double radius = 2.0 * scale;
                        Circle circle = new Circle(new Point3d(0, 0, 0), Vector3d.ZAxis, radius);
                        circle.ColorIndex = 1; // Red
                        blkRec.AppendEntity(circle);
                        acTrans.AddNewlyCreatedDBObject(circle, true);

                        // Draw Cross (X)
                        double crossSize = radius * 0.7; // Fit inside
                        Line l1 = new Line(new Point3d(-crossSize, -crossSize, 0), new Point3d(crossSize, crossSize, 0));
                        l1.ColorIndex = 1;
                        blkRec.AppendEntity(l1);
                        acTrans.AddNewlyCreatedDBObject(l1, true);

                        Line l2 = new Line(new Point3d(-crossSize, crossSize, 0), new Point3d(crossSize, -crossSize, 0));
                        l2.ColorIndex = 1;
                        blkRec.AppendEntity(l2);
                        acTrans.AddNewlyCreatedDBObject(l2, true);

                        // Insert the block at point p
                        double x = p.X;
                        double y = p.Y * scale; // Apply vertical scale? Usually markers are invariant or follow data?
                        // If it's a "point in the graph", y should be scaled. 
                        // But the shape itself (circle) should probably be uniform? 
                        // The user said "Preview". 
                        // TrazadoUsuario draws components. 
                        // Let's assume uniform scaling for the MARKER SHAPE, but position is transformed.
                        
                        if (calculoPolilineaPerfil != null)
                        {
                            x += calculoPolilineaPerfil.x_ins;
                            y += calculoPolilineaPerfil.y_ins;
                            // scale is passed as n_escala which is vertical scale.
                            // If we want the MARKER to look round, we shouldn't scale Y differently, 
                            // but the POSITION Y is definitely scaled.
                        }

                        BlockReference blkRef = new BlockReference(new Point3d(x, y, 0), blkId);
                        blkRef.ColorIndex = 1;
                        // acBlkTblRec.AppendEntity(blkRef); // Insert into ModelSpace
                        // Wait, we need to append to ModelSpace, not the anonymous block definition.
                        ObjectId refId = acBlkTblRec.AppendEntity(blkRef);
                        acTrans.AddNewlyCreatedDBObject(blkRef, true);

                        acTrans.Commit();
                        return refId;
                    }
                }
            }
            catch (Exception ex)
            {
                 // MessageBox.Show($"Error drawing marker: {ex.Message}");
                 return ObjectId.Null;
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
                    // Esto incluye -Rotulacion-*, -Linea_Rotulacion_*, etc.
                    // Tambien añadimos las capas especificas "Guitarra*", "Linea_Rotulacion*", "Linea_Cota*"
                    TypedValue[] filter = new TypedValue[] {
                        new TypedValue((int)DxfCode.Operator, "<OR"),
                        new TypedValue((int)DxfCode.LayerName, nombreEje + "-*"),
                        new TypedValue((int)DxfCode.LayerName, "Guitarra*"),
                        new TypedValue((int)DxfCode.LayerName, "Linea_Rotulacion*"),
                        new TypedValue((int)DxfCode.LayerName, "Linea_Cota*"),
                        new TypedValue((int)DxfCode.Operator, "OR>")
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

        // ---------------------------------------------------------
        // GESTIÓN DE PERFILES (MEMORIA Y ARCHIVOS)
        // ---------------------------------------------------------

        private void b_ejenuevo_Click(object sender, EventArgs e)
        {
            IniciarNuevoPerfil();
        }

        private bool IniciarNuevoPerfil()
        {
            // Preguntar si desea guardar el perfil actual
            DialogResult result = MessageBox.Show(
                "¿Desea guardar el perfil actual en memoria antes de crear uno nuevo?",
                "Nuevo Perfil",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return false;

            if (result == DialogResult.Yes)
            {
                if (!GuardarPerfilActual()) return false; 
            }

            // Reiniciar todos los datos para un nuevo perfil
            listaRectas.Clear();
            listaParabolas.Clear();
            listaComponentesPerfil.Clear();
            bindingSource.ResetBindings(false);
            CambiarModoEdicion(false);
            
            // Limpiar TextBoxes
            B_nombre.Texts = "";
            
            // Limpiar datos auxiliares de trazado
            _idUltimaPolilinea = ObjectId.Null;
            
            // Resetear visualización
            LimpiarVistaPrevia(_idPreview1);
            LimpiarVistaPrevia(_idPreview2);
            _editIndex = -1;

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Perfil guardado y formulario reiniciado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Formulario reiniciado sin guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }

        private bool GuardarPerfilActual()
        {
            // Validar nombre
            string nombrePerfil = "";
            Control[] ctrls = Controls.Find("B_nombre", true);
            if (ctrls.Length > 0) nombrePerfil = ctrls[0].Text.Trim();
            
            if (string.IsNullOrWhiteSpace(nombrePerfil))
            {
                MessageBox.Show("Debe especificar un nombre para el perfil antes de guardar.", 
                    "Nombre requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (ctrls.Length > 0) ctrls[0].Focus();
                return false;
            }

            // Comprobar existencia
            var perfilExistente = PerfilesGuardados.FirstOrDefault(x => x.Nombre.Equals(nombrePerfil, StringComparison.OrdinalIgnoreCase));
            if (perfilExistente != null)
            {
                DialogResult overwriteResult = MessageBox.Show(
                    $"Ya existe un perfil guardado con el nombre '{nombrePerfil}'. ¿Desea sobrescribirlo?",
                    "Confirmar sobrescritura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (overwriteResult == DialogResult.No) return false;

                // Actualizar
                perfilExistente.Componentes = new List<ComponentePerfil>(listaComponentesPerfil);
            }
            else
            {
                // Nuevo
                PerfilEnMemoria nuevo = new PerfilEnMemoria
                {
                    Nombre = nombrePerfil,
                    Componentes = new List<ComponentePerfil>(listaComponentesPerfil)
                };
                PerfilesGuardados.Add(nuevo);
            }
            ActualizarComboPerfiles();
            return true;
        }

        private void ActualizarComboPerfiles()
        {
            // Desconectar evento para evitar que Items.Add dispare SelectedIndexChanged
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox1.Items.Clear();

            int maxWidth = comboBox1.Width;
            using (Graphics g = comboBox1.CreateGraphics())
            {
                foreach (var p in PerfilesGuardados)
                {
                    comboBox1.Items.Add(p.Nombre);
                    // Calcular el ancho del texto para ajustar el desplegable
                    int textWidth = (int)g.MeasureString(p.Nombre, comboBox1.Font).Width;
                    if (textWidth > maxWidth)
                        maxWidth = textWidth;
                }
            }

            // Añadir margen para el scrollbar si es necesario
            if (comboBox1.Items.Count > comboBox1.MaxDropDownItems)
                maxWidth += SystemInformation.VerticalScrollBarWidth;

            comboBox1.DropDownWidth = maxWidth;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void b_guardareje_Click(object sender, EventArgs e)
        {
            if (GuardarPerfilActual())
            {
                MessageBox.Show("Perfil guardado correctamente en memoria.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void b_borrareje_Click(object sender, EventArgs e)
        {
            string nombrePerfil = "";
            Control[] ctrls = Controls.Find("B_nombre", true);
            if (ctrls.Length > 0) nombrePerfil = ctrls[0].Text.Trim();

            if (string.IsNullOrWhiteSpace(nombrePerfil))
            {
                MessageBox.Show("Debe seleccionar un perfil (o especificar su nombre) para borrarlo.", 
                    "Nombre requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var perfil = PerfilesGuardados.FirstOrDefault(x => x.Nombre.Equals(nombrePerfil, StringComparison.OrdinalIgnoreCase));
            if (perfil == null)
            {
                MessageBox.Show($"No se encontró ningún perfil con el nombre '{nombrePerfil}' en memoria.", 
                    "No encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"¿Está seguro de eliminar el perfil '{nombrePerfil}'?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PerfilesGuardados.Remove(perfil);
                ActualizarComboPerfiles();
                
                // Limpiar UI
                listaComponentesPerfil.Clear();
                listaRectas.Clear();
                listaParabolas.Clear();
                bindingSource.ResetBindings(false);
                if (ctrls.Length > 0) ctrls[0].Text = "";
                
                MessageBox.Show("Perfil eliminado de la memoria.", "Borrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb == null || cb.SelectedIndex == -1) return;

            string nombreSeleccionado = cb.SelectedItem.ToString();
            var perfil = PerfilesGuardados.FirstOrDefault(x => x.Nombre == nombreSeleccionado);

            if (perfil != null)
            {
                CargarPerfilEnDataGrid(perfil);
            }
        }

        private void CargarPerfilEnDataGrid(PerfilEnMemoria perfil)
        {
            // Limpiar
            listaRectas.Clear();
            listaParabolas.Clear();
            listaComponentesPerfil.Clear();
           
            // Actualizar nombre en UI
            Control[] ctrls = Controls.Find("B_nombre", true);
            if (ctrls.Length > 0) ctrls[0].Text = perfil.Nombre;

            // Cargar
            // NOTA: ComponentePerfil contiene ObjetoOriginal. Necesitamos restaurar listaRectas y listaParabolas desde ahí.
            // Si ObjetoOriginal se pierde en la UI (no debería si es in-memory), estamos bien.
            // Regex para extraer coordenadas de la forma "(X, Y)" o "X, Y"
            System.Text.RegularExpressions.Regex regexPunto = new System.Text.RegularExpressions.Regex(@"[-\d\.]+");

            Func<string, Point2d?> parsearPunto = (str) =>
            {
                if (string.IsNullOrWhiteSpace(str)) return null;
                var matches = regexPunto.Matches(str);
                if (matches.Count >= 2 && double.TryParse(matches[0].Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double x) && 
                    double.TryParse(matches[1].Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double y))
                {
                    return new Point2d(x, y);
                }
                return null;
            };

            foreach (var comp in perfil.Componentes)
            {
                listaComponentesPerfil.Add(comp);
                
                var ptIni = parsearPunto(comp.PuntoInicial);
                var ptFin = parsearPunto(comp.PuntoFinal);
                
                if (comp.Tipo == "RECTA")
                {
                    Logica.Pendiente pNueva = new Logica.Pendiente();
                    if (ptIni.HasValue && ptFin.HasValue)
                    {
                        pNueva.Puntos = new List<Point2d> { ptIni.Value, ptFin.Value };
                    }
                    comp.ObjetoOriginal = pNueva;
                    listaRectas.Add(pNueva);
                }
                else if (comp.Tipo == "PARABOLA")
                {
                    Logica.Parabola parNueva = new Logica.Parabola(new List<double>(), new List<Logica.PuntoPerfil>());
                    if (ptIni.HasValue) parNueva.puntoEntrada = ptIni.Value;
                    if (ptFin.HasValue) parNueva.puntoSalida = ptFin.Value;
                    comp.ObjetoOriginal = parNueva;
                    listaParabolas.Add(parNueva);
                }
                else if (comp.Tipo == "CIRCULAR")
                {
                    // Se rellenará la geometría matemáticamente en la segunda pasada
                    EjeDeTrazado.componentes.Curva curvaNueva = new EjeDeTrazado.componentes.Curva(
                        new tadLayShare.puntos.Punto3d(ptIni.HasValue ? ptIni.Value.X : 0, ptIni.HasValue ? ptIni.Value.Y : 0, 0),
                        new tadLayShare.puntos.Punto3d(ptFin.HasValue ? ptFin.Value.X : 0, ptFin.HasValue ? ptFin.Value.Y : 0, 0),
                        new tadLayShare.puntos.Punto3d(0, 0, 0), // Temp center
                        comp.Radio.GetValueOrDefault(),
                        ptIni.HasValue ? ptIni.Value.X : 0,
                        0,
                        EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario // Temp direction
                    );
                    comp.ObjetoOriginal = curvaNueva;
                    listaCurvas.Add(curvaNueva);
                }
            }

            // Forzar la continuidad geométrica y recalcular parámetros de la parábola
            // El punto inicial de una parábola debe ser el punto final de la recta anterior,
            // y el punto final de la parábola debe ser el punto inicial de la recta siguiente.
            for (int i = 0; i < listaComponentesPerfil.Count; i++)
            {
                var compActual = listaComponentesPerfil[i];
                if (compActual.Tipo == "PARABOLA" && compActual.ObjetoOriginal is Logica.Parabola parabolaActual)
                {
                    Logica.Pendiente rectaAnterior = null;
                    Logica.Pendiente rectaSiguiente = null;

                    // Punto inicial de la parábola = punto final de la recta anterior
                    if (i > 0)
                    {
                        var compAnterior = listaComponentesPerfil[i - 1];
                        if (compAnterior.Tipo == "RECTA" && compAnterior.ObjetoOriginal is Logica.Pendiente ra)
                        {
                            rectaAnterior = ra;
                            if (rectaAnterior.Puntos != null && rectaAnterior.Puntos.Count > 0)
                            {
                                var pt = rectaAnterior.Puntos.Last();
                                parabolaActual.puntoEntrada = pt;
                                compActual.PuntoInicial = $"{pt.X:F3}, {pt.Y:F3}";
                            }
                        }
                    }

                    // Punto final de la parábola = punto inicial de la recta siguiente
                    if (i + 1 < listaComponentesPerfil.Count)
                    {
                        var compSiguiente = listaComponentesPerfil[i + 1];
                        if (compSiguiente.Tipo == "RECTA" && compSiguiente.ObjetoOriginal is Logica.Pendiente rs)
                        {
                            rectaSiguiente = rs;
                            if (rectaSiguiente.Puntos != null && rectaSiguiente.Puntos.Count > 0)
                            {
                                var pt = rectaSiguiente.Puntos.First();
                                parabolaActual.puntoSalida = pt;
                                compActual.PuntoFinal = $"{pt.X:F3}, {pt.Y:F3}";
                            }
                        }
                    }

                    // RECALCULAR LOS PARÁMETROS DE LA PARÁBOLA a, b, c y su polilínea
                    if (rectaAnterior != null && rectaSiguiente != null && rectaAnterior.Puntos.Count >= 2 && rectaSiguiente.Puntos.Count >= 2)
                    {
                        double xEntrada = parabolaActual.puntoEntrada.X;
                        double yEntrada = parabolaActual.puntoEntrada.Y;

                        // Cálculo de las pendientes de las rectas adyacentes
                        var pR1A = rectaAnterior.Puntos[0];
                        var pR1B = rectaAnterior.Puntos.Last();
                        double p1 = (pR1B.Y - pR1A.Y) / (pR1B.X - pR1A.X);

                        var pR2A = rectaSiguiente.Puntos[0];
                        var pR2B = rectaSiguiente.Puntos.Last();
                        double p2 = (pR2B.Y - pR2A.Y) / (pR2B.X - pR2A.X);

                        // Si tenemos Kv, se define a = 1 / (2 * Kv) o -1 / (2 * Kv) dependiendo del acuerdo (cóncavo o convexo)
                        // Para saber el signo, p2 - p1 debe tener el mismo signo que a. (a = (p2-p1) / (2*L))
                        // Otra forma: El vértice o comportamiento. a = (p2 - p1) / (2 * (xSalida - xEntrada))
                        double kv = compActual.Kv.GetValueOrDefault();
                        if (kv == 0) kv = 1000; // Evitar división por cero si viniera mal

                        double a = 1.0 / (2.0 * kv);
                        if (p2 < p1)
                        {
                            a = -a;
                        }

                        // b = p1 - 2*a*xEntrada
                        double b = p1 - 2 * a * xEntrada;

                        // c = yEntrada - a*xEntrada^2 - b*xEntrada
                        double c = yEntrada - a * Math.Pow(xEntrada, 2) - b * xEntrada;

                        // Guardar los 3 parámetros en el objeto
                        if (parabolaActual.parabola == null) parabolaActual.parabola = new List<double>();
                        parabolaActual.parabola.Clear();
                        parabolaActual.parabola.Add(a);
                        parabolaActual.parabola.Add(b);
                        parabolaActual.parabola.Add(c);

                        // Vaciar polilinea y meter el punto inicial para no afectar a futuros guardados
                        if (parabolaActual.polilinea_perfil == null) parabolaActual.polilinea_perfil = new List<Logica.PuntoPerfil>();
                        parabolaActual.polilinea_perfil.Clear();
                        
                        Logica.PuntoPerfil ppIni = new Logica.PuntoPerfil();
                        ppIni.p = new Autodesk.AutoCAD.Geometry.Point2d(parabolaActual.puntoEntrada.X, parabolaActual.puntoEntrada.Y);
                        parabolaActual.polilinea_perfil.Add(ppIni);
                        
                        Logica.PuntoPerfil ppFin = new Logica.PuntoPerfil();
                        ppFin.p = new Autodesk.AutoCAD.Geometry.Point2d(parabolaActual.puntoSalida.X, parabolaActual.puntoSalida.Y);
                        parabolaActual.polilinea_perfil.Add(ppFin);

                    }
                }
                else if (compActual.Tipo == "CIRCULAR" && compActual.ObjetoOriginal is EjeDeTrazado.componentes.Curva curvaActual)
                {
                    Logica.Pendiente rectaAnterior = null;
                    Logica.Pendiente rectaSiguiente = null;

                    if (i > 0)
                    {
                        var compAnterior = listaComponentesPerfil[i - 1];
                        if (compAnterior.Tipo == "RECTA" && compAnterior.ObjetoOriginal is Logica.Pendiente ra)
                             rectaAnterior = ra;
                    }

                    if (i + 1 < listaComponentesPerfil.Count)
                    {
                        var compSiguiente = listaComponentesPerfil[i + 1];
                        if (compSiguiente.Tipo == "RECTA" && compSiguiente.ObjetoOriginal is Logica.Pendiente rs)
                            rectaSiguiente = rs;
                    }

                    if (rectaAnterior != null && rectaSiguiente != null && rectaAnterior.Puntos.Count >= 2 && rectaSiguiente.Puntos.Count >= 2)
                    {
                        var pR1A = rectaAnterior.Puntos[0];
                        var pR1B = rectaAnterior.Puntos.Last();
                        double m1 = (pR1B.Y - pR1A.Y) / (pR1B.X - pR1A.X);

                        var pR2A = rectaSiguiente.Puntos[0];
                        var pR2B = rectaSiguiente.Puntos.Last();
                        double m2 = (pR2B.Y - pR2A.Y) / (pR2B.X - pR2A.X);

                        double x_pcv = curvaActual.getPuntoEntrada.coordenadaX;
                        double y_pcv = curvaActual.getPuntoEntrada.coordenadaY;
                        double radio = curvaActual.getRadio;

                        double cx, cy;
                        EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva sentido;

                        // Unit normal vector calculations
                        double mod1 = Math.Sqrt(1 + m1 * m1);
                        double ux1 = 1 / mod1;
                        double uy1 = m1 / mod1;

                        if (m2 > m1) // Sag / CCW
                        {
                            sentido = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario; 
                            cx = x_pcv + radio * (-uy1);
                            cy = y_pcv + radio * (ux1);
                        }
                        else // Crest / CW
                        {
                            sentido = EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario; 
                            cx = x_pcv + radio * (uy1);
                            cy = y_pcv + radio * (-ux1);
                        }

                        // Instanciar nueva curva con centro y sentido correcto
                        EjeDeTrazado.componentes.Curva curvaCorregida = new EjeDeTrazado.componentes.Curva(
                            new tadLayShare.puntos.Punto3d(x_pcv, y_pcv, 0),
                            new tadLayShare.puntos.Punto3d(curvaActual.getPuntoSalida.coordenadaX, curvaActual.getPuntoSalida.coordenadaY, 0),
                            new tadLayShare.puntos.Punto3d(cx, cy, 0), // Centro calculado
                            radio,
                            x_pcv, // pkInicial (simplificación)
                            0, // peralte
                            sentido // Sentido calculado
                        );
                        
                        // Sustituir referencias
                        compActual.ObjetoOriginal = curvaCorregida;
                        
                        int idxCurva = listaCurvas.IndexOf(curvaActual);
                        if (idxCurva >= 0)
                            listaCurvas[idxCurva] = curvaCorregida;
                    }
                }
            }

            bindingSource.ResetBindings(false);
        }

        /// <summary>Carga automáticamente los perfiles contenidos en un archivo .amilia.</summary>
        private void CargarPerfilesDesdeArchivo(string ruta)
        {
            try
            {
                string json = File.ReadAllText(ruta);
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                try { settings.Converters.Add(new AutoCADPointConverter()); } catch { }

                var listaCargada = JsonConvert.DeserializeObject<List<principal.SalidaAmilia>>(json, settings);
                if (listaCargada == null) return;

                var perfilesCargados = listaCargada.Where(x => x.Tipo == principal.TipoSalida.Perfil).ToList();
                if (perfilesCargados.Count == 0) return;

                PerfilesGuardados.Clear();
                foreach (var item in perfilesCargados)
                {
                    PerfilesGuardados.Add(new PerfilEnMemoria
                    {
                        Nombre = item.Nombre,
                        Componentes = item.ComponentesPerfil?.Select(c => FromSalidaComponentePerfil(c)).ToList()
                                      ?? new List<ComponentePerfil>()
                    });
                }
                ActualizarComboPerfiles();
                if (PerfilesGuardados.Count > 0) CargarPerfilEnDataGrid(PerfilesGuardados[0]);
            }
            catch { /* Si falla la carga silenciosamente, el usuario puede cargar manualmente */ }
        }

        private void b_guardarejes_Click(object sender, EventArgs e)
        {
            if (PerfilesGuardados.Count == 0)
            {
                MessageBox.Show("No hay perfiles en memoria para guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si hay un archivo .amilia ya cargado, preguntar si sobreescribir o guardar como nuevo
            string rutaDestino = null;
            bool usarSaveDialog = true;

            if (!string.IsNullOrEmpty(_rutaArchivoAmilia) && File.Exists(_rutaArchivoAmilia))
            {
                DialogResult result = MessageBox.Show(
                    $"Ya existe un archivo asociado:\n{_rutaArchivoAmilia}\n\n¿Desea sobreescribirlo?\n(Seleccione 'No' para elegir una nueva ubicación)",
                    "Confirmar Guardado",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Cancel) return;
                
                if (result == DialogResult.Yes)
                {
                    rutaDestino = _rutaArchivoAmilia;
                    usarSaveDialog = false;
                }
            }

            if (usarSaveDialog)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Archivos AMILIA (*.amilia)|*.amilia|Todos los archivos (*.*)|*.*";
                sfd.DefaultExt = "amilia";
                sfd.FileName = string.IsNullOrEmpty(_rutaArchivoAmilia) ? "PerfilesGuardados.amilia" : Path.GetFileName(_rutaArchivoAmilia);
                sfd.InitialDirectory = !string.IsNullOrEmpty(principal.ruta_principal) ? principal.ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                if (sfd.ShowDialog() != DialogResult.OK) return;
                rutaDestino = sfd.FileName;
            }

            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                };
                try { settings.Converters.Add(new AutoCADPointConverter()); } catch { }

                // Leer archivo existente para conservar entradas Longitudinal
                List<principal.SalidaAmilia> listaSalida = new List<principal.SalidaAmilia>();
                if (File.Exists(rutaDestino))
                {
                    try
                    {
                        string jsonExistente = File.ReadAllText(rutaDestino);
                        var existente = JsonConvert.DeserializeObject<List<principal.SalidaAmilia>>(jsonExistente, settings);
                        if (existente != null)
                            listaSalida.AddRange(existente.Where(x => x.Tipo == principal.TipoSalida.Longitudinal));
                    }
                    catch { }
                }

                // Añadir/actualizar entradas de Perfil
                foreach (var p in PerfilesGuardados)
                {
                    listaSalida.RemoveAll(x => x.Tipo == principal.TipoSalida.Perfil && x.Nombre == p.Nombre);
                    listaSalida.Add(new principal.SalidaAmilia
                    {
                        Tipo = principal.TipoSalida.Perfil,
                        Nombre = p.Nombre,
                        ComponentesPerfil = p.Componentes?.Select(c => ToSalidaComponentePerfil(c)).ToList()
                    });
                }

                string json = JsonConvert.SerializeObject(listaSalida, settings);
                File.WriteAllText(rutaDestino, json);
                _rutaArchivoAmilia = rutaDestino;

                MessageBox.Show($"Guardado correctamente en:\n{rutaDestino}", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error");
            }
        }

        private void b_cargarejes_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos AMILIA (*.amilia)|*.amilia|Todos los archivos (*.*)|*.*";
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    principal.ruta_principal = System.IO.Path.GetDirectoryName(ofd.FileName);
                    string json = File.ReadAllText(ofd.FileName);
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                         TypeNameHandling = TypeNameHandling.Auto
                    };
                    try { settings.Converters.Add(new AutoCADPointConverter()); } catch { }

                    var listaCargada = JsonConvert.DeserializeObject<List<principal.SalidaAmilia>>(json, settings);
                    
                    if (listaCargada != null)
                    {
                        // Filtrar solo entradas de Perfil
                        var perfilesCargados = listaCargada
                            .Where(x => x.Tipo == principal.TipoSalida.Perfil)
                            .ToList();

                        if (perfilesCargados.Count == 0)
                        {
                            MessageBox.Show("El archivo no contiene ninguna entrada de tipo Perfil.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        PerfilesGuardados.Clear();
                        foreach (var item in perfilesCargados)
                        {
                            PerfilesGuardados.Add(new PerfilEnMemoria 
                            { 
                                Nombre = item.Nombre, 
                                Componentes = item.ComponentesPerfil?.Select(c => FromSalidaComponentePerfil(c)).ToList() 
                                              ?? new List<ComponentePerfil>() 
                            });
                        }
                        ActualizarComboPerfiles();
                        if (PerfilesGuardados.Count > 0) CargarPerfilEnDataGrid(PerfilesGuardados[0]);
                        MessageBox.Show($"Se cargaron {PerfilesGuardados.Count} perfiles.", "Éxito");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar: {ex.Message}", "Error");
                }
            }
        }

        // Clase interna para solicitar Kvs en tiempo de ejecución
        private class KvPromptDialog : Form
        {
            public double Kv1 { get; private set; }
            public double Kv2 { get; private set; }

            private TextBox txtKv1;
            private TextBox txtKv2;
            private Button btnOk;
            private Button btnCancel;

            public KvPromptDialog(bool showKv1 = true, double defaultKv = 0)
            {
                this.Text = "Definir Curvas Verticales";
                this.Size = new System.Drawing.Size(300, showKv1 ? 200 : 150);
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                string defKvStr = defaultKv > 0 ? defaultKv.ToString() : "0";

                int topOffset = showKv1 ? 0 : -60;

                Label lblKv1 = new Label() { Text = "Kv Entrada (Previo-Nuevo):", Left = 20, Top = 20, Width = 240, Visible = showKv1 };
                txtKv1 = new TextBox() { Left = 20, Top = 45, Width = 240, Text = defKvStr, Visible = showKv1 };

                Label lblKv2 = new Label() { Text = "Kv Salida (Nuevo-Siguiente):", Left = 20, Top = 80 + topOffset, Width = 240 };
                txtKv2 = new TextBox() { Left = 20, Top = 105 + topOffset, Width = 240, Text = defKvStr };

                btnOk = new Button() { Text = "Aceptar", Left = 50, Top = 140 + topOffset, DialogResult = DialogResult.OK };
                btnCancel = new Button() { Text = "Cancelar", Left = 150, Top = 140 + topOffset, DialogResult = DialogResult.Cancel };

                if (showKv1)
                {
                    this.Controls.Add(lblKv1);
                    this.Controls.Add(txtKv1);
                }
                this.Controls.Add(lblKv2);
                this.Controls.Add(txtKv2);
                this.Controls.Add(btnOk);
                this.Controls.Add(btnCancel);

                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                base.OnFormClosing(e);

                if (this.DialogResult == DialogResult.OK)
                {
                    double val1 = 0, val2;
                    if (txtKv1 != null && txtKv1.Visible)
                    {
                        if (!double.TryParse(txtKv1.Text, out val1))
                        {
                            MessageBox.Show("Valor de Kv Entrada inválido.");
                            e.Cancel = true;
                            return;
                        }
                    }
                    if (!double.TryParse(txtKv2.Text, out val2))
                    {
                        MessageBox.Show("Valor de Kv Salida inválido.");
                        e.Cancel = true;
                        return;
                    }

                    Kv1 = val1;
                    Kv2 = val2;
                }
            }
        }
        private void ExportarLandXML_Local()
        {
            DialogResult resultinforme = MessageBox.Show("¿Desea guardar el trazado en un archivo LandXml?", "Guardar LandXml", MessageBoxButtons.YesNo);
            if (resultinforme == DialogResult.Yes)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                    sfd.FilterIndex = 2;
                    sfd.RestoreDirectory = true;
                    sfd.DefaultExt = "xml";
                    sfd.FileName = "Eje_Principal.xml";
                    sfd.InitialDirectory = !string.IsNullOrEmpty(principal.ruta_principal) ? principal.ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string ruta = sfd.FileName;

                        // Intentar obtener los MComponentes del eje longitudinal desde el archivo .amilia
                        List<EjeDeTrazado.componentes.Componente> trazadoEje = new List<EjeDeTrazado.componentes.Componente>();
                        if (!string.IsNullOrEmpty(_rutaArchivoAmilia) && File.Exists(_rutaArchivoAmilia))
                        {
                            try
                            {
                                string jsonAmilia = File.ReadAllText(_rutaArchivoAmilia);
                                JsonSerializerSettings settingsAmilia = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                                try { settingsAmilia.Converters.Add(new AutoCADPointConverter()); } catch { }

                                var listaAmilia = JsonConvert.DeserializeObject<List<principal.SalidaAmilia>>(jsonAmilia, settingsAmilia);
                                if (listaAmilia != null)
                                {
                                    // Filtrar entradas longitudinales que tengan MComponentes
                                    var ejesLong = listaAmilia
                                        .Where(x => x.Tipo == principal.TipoSalida.Longitudinal && x.MComponentes != null && x.MComponentes.Count > 0)
                                        .ToList();

                                    if (ejesLong.Count == 1)
                                    {
                                        trazadoEje = ejesLong[0].MComponentes;
                                    }
                                    else if (ejesLong.Count > 1)
                                    {
                                        // Varios ejes: mostrar diálogo de selección
                                        Form selForm = new Form();
                                        selForm.Text = "Seleccionar eje longitudinal";
                                        selForm.Width = 360; selForm.Height = 220;
                                        selForm.StartPosition = FormStartPosition.CenterParent;
                                        selForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                                        selForm.MinimizeBox = false; selForm.MaximizeBox = false;

                                        Label lbl = new Label { Text = "El archivo contiene varios ejes longitudinales.\nSelecciona el que quieres exportar:", Left = 10, Top = 10, Width = 330, Height = 36 };
                                        ListBox lb = new ListBox { Left = 10, Top = 52, Width = 330, Height = 100, SelectionMode = System.Windows.Forms.SelectionMode.One };
                                        foreach (var e in ejesLong)
                                            lb.Items.Add(e.Nombre ?? "(sin nombre)");
                                        lb.SelectedIndex = 0;
                                        Button btnOk     = new Button { Text = "Aceptar",   DialogResult = DialogResult.OK,     Left = 175, Top = 162, Width = 80 };
                                        Button btnCancel = new Button { Text = "Cancelar",  DialogResult = DialogResult.Cancel, Left = 265, Top = 162, Width = 75 };
                                        selForm.Controls.AddRange(new Control[] { lbl, lb, btnOk, btnCancel });
                                        selForm.AcceptButton = btnOk;
                                        selForm.CancelButton = btnCancel;

                                        if (selForm.ShowDialog() == DialogResult.OK && lb.SelectedIndex >= 0)
                                            trazadoEje = ejesLong[lb.SelectedIndex].MComponentes;
                                    }
                                }
                            }
                            catch { /* Si falla la lectura del .amilia, se exporta con trazado vacío */ }
                        }

                        // Asegurar que las parábolas tengan su Kv original para LandXML
                        if (listaComponentesPerfil != null)
                        {
                            foreach (var comp in listaComponentesPerfil)
                            {
                                if (comp.Tipo == "PARABOLA" && comp.ObjetoOriginal is Logica.Parabola p)
                                    p.max_min = comp.Kv.GetValueOrDefault();
                            }
                        }

                        ExportadorLandXml.Exportar(ruta, "Eje_Principal", trazadoEje, listaParabolas, null, listaRectas, "MiProyecto", true);

                    }
                }
            }
        }
        private void GenerarInforme()
        {
            DialogResult resultinforme = MessageBox.Show("¿Desea crear el archivo de informe del trazado?", "Informe del trazado", MessageBoxButtons.YesNo);
            if (resultinforme == DialogResult.Yes)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.DefaultExt = "csv";
                saveFileDialog1.FileName = "Informe_Perfil.csv";
                saveFileDialog1.InitialDirectory = !string.IsNullOrEmpty(principal.ruta_principal) ? principal.ruta_principal : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter output = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8))
                        {
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

                            if (listaComponentesPerfil != null && listaComponentesPerfil.Count > 0)
                            {
                                foreach (var comp in listaComponentesPerfil)
                                {
                                    if (comp.Tipo == "RECTA")
                                    {
                                        double startX = 0, startY = 0, endX = 0, endY = 0;
                                        double slope = 0;

                                        if (comp.ObjetoOriginal is Logica.Pendiente p && p.Puntos != null && p.Puntos.Count > 0)
                                        {
                                            startX = p.Puntos.First().X;
                                            startY = p.Puntos.First().Y;
                                            endX = p.Puntos.Last().X;
                                            endY = p.Puntos.Last().Y;
                                        }
                                        
                                        if (Math.Abs(endX - startX) > 0.0001)
                                            slope = (endY - startY) / (endX - startX);

                                        output.Write(";");
                                        output.Write(";");
                                        output.Write(componentes.ToString());
                                        output.Write(";");
                                        output.Write("Pendiente");
                                        output.Write(";");
                                        output.Write(slope.ToString());
                                        output.Write(";");
                                        output.Write(";");
                                        output.Write(startX.ToString());
                                        output.Write(";");
                                        output.Write(endX.ToString());
                                        output.Write(";");
                                        output.Write(startY.ToString());
                                        output.Write(";");
                                        output.Write(endY.ToString());
                                        output.Write(";");
                                        output.WriteLine();
                                        componentes++;
                                    }
                                    else if (comp.Tipo == "PARABOLA" || comp.Tipo == "CIRCULAR")
                                    {
                                        double kv = 0;
                                        double startX = 0, startY = 0, endX = 0, endY = 0;
                                        
                                        if (comp.Tipo == "PARABOLA" && comp.ObjetoOriginal is Logica.Parabola par)
                                        {
                                            if (par.puntoEntrada != null) { startX = par.puntoEntrada.X; startY = par.puntoEntrada.Y; }
                                            if (par.puntoSalida != null)  { endX = par.puntoSalida.X; endY = par.puntoSalida.Y; }
                                            
                                            // Kv calculation: 1/(2a)
                                            if (par.parabola != null && par.parabola.Count >= 3)
                                            {
                                                 double a = par.parabola[0];
                                                 if (Math.Abs(a) > 1e-9) kv = 1.0 / (2.0 * a);
                                            }
                                        }
                                        else if (comp.Tipo == "CIRCULAR" && comp.ObjetoOriginal is EjeDeTrazado.componentes.Curva curv)
                                        {
                                            kv = curv.get_Radio(); 
                                            if (curv.getPuntoEntrada != null) 
                                            { 
                                                startX = curv.getPuntoEntrada.coordenadaX; 
                                                startY = curv.getPuntoEntrada.coordenadaY; 
                                            }
                                            if (curv.getPuntoSalida != null)
                                            {
                                                endX = curv.getPuntoSalida.coordenadaX;
                                                endY = curv.getPuntoSalida.coordenadaY;
                                            }
                                        }
                                        else
                                        {
                                            kv = comp.Kv.GetValueOrDefault();
                                        }

                                        output.Write(";");
                                        output.Write(";");
                                        output.Write(componentes.ToString());
                                        output.Write(";");
                                        output.Write("Acuerdo");
                                        output.Write(";");
                                        output.Write(";"); 
                                        output.Write(kv.ToString());
                                        output.Write(";");
                                        output.Write(startX.ToString());
                                        output.Write(";");
                                        output.Write(endX.ToString());
                                        output.Write(";");
                                        output.Write(startY.ToString());
                                        output.Write(";");
                                        output.Write(endY.ToString());
                                        output.Write(";");
                                        output.WriteLine();
                                        componentes++;
                                    }
                                }
                            }
                        }
                        MessageBox.Show("Informe generado correctamente.", "Éxito");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al generar informe: {ex.Message}", "Error");
                    }
                }
            }
        }

        private ObjectId DibujarArcoEnAutoCAD(Point2d center, double radius, double startAngle, double endAngle, EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva sentido, double escalaVertical)
        {
            ObjectId id = ObjectId.Null;
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;

                using (DocumentLock docLock = doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                        Polyline poly = new Polyline();
                        poly.SetDatabaseDefaults();
                        poly.ColorIndex = 7; // White

                        // Determine steps for discretization
                        // Ensure angles are properly handled for the direction
                        double totalAngle = endAngle - startAngle;

                        if (sentido == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Antihorario) // CCW
                        {
                            while (totalAngle < 0) totalAngle += 2 * Math.PI;
                        }
                        else // CW
                        {
                            while (totalAngle > 0) totalAngle -= 2 * Math.PI;
                        }

                        // Calculate number of steps based on arc length or fixed number
                        // Arc Length = R * theta
                        double len = Math.Abs(radius * totalAngle);
                        int steps = (int)Math.Max(10, len); // min 10 segments or 1 segment per unit length

                        double stepAngle = totalAngle / steps;

                        double x_ins = 0;
                        double y_ins = 0;
                        if (calculoPolilineaPerfil != null)
                        {
                            x_ins = calculoPolilineaPerfil.x_ins;
                            y_ins = calculoPolilineaPerfil.y_ins;
                        }

                        for (int i = 0; i <= steps; i++)
                        {
                            double theta = startAngle + i * stepAngle;
                            double x = center.X + radius * Math.Cos(theta);
                            double y = center.Y + radius * Math.Sin(theta);

                            // Apply Scale and Offset
                            double drawX = x + x_ins;
                            double drawY = y * escalaVertical + y_ins;

                            poly.AddVertexAt(i, new Point2d(drawX, drawY), 0, 0, 0);
                        }

                        id = btr.AppendEntity(poly);
                        tr.AddNewlyCreatedDBObject(poly, true);
                        tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error dibujando arco: " + ex.Message);
            }
            return id;
        }

        private void P_ini_x_2_TextChanged(object sender, EventArgs e)
        {

        }

        // ---------- Helpers de conversión entre ComponentePerfil local y principal.ComponentePerfil ----------

        private static principal.ComponentePerfil ToSalidaComponentePerfil(ComponentePerfil c)
        {
            return new principal.ComponentePerfil
            {
                Tipo          = c.Tipo,
                PuntoInicial  = c.PuntoInicial,
                PuntoFinal    = c.PuntoFinal,
                Kv            = c.Kv,
                Pendiente     = c.Pendiente,
                Radio         = c.Radio,
                ObjetoOriginal = c.ObjetoOriginal,
                EntityId      = c.EntityId
            };
        }

        private static ComponentePerfil FromSalidaComponentePerfil(principal.ComponentePerfil c)
        {
            return new ComponentePerfil
            {
                Tipo          = c.Tipo,
                PuntoInicial  = c.PuntoInicial,
                PuntoFinal    = c.PuntoFinal,
                Kv            = c.Kv,
                Pendiente     = c.Pendiente,
                Radio         = c.Radio,
                ObjetoOriginal = c.ObjetoOriginal,
                EntityId      = c.EntityId
            };
        }
    }
}










