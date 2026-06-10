using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.userControl
{
    using tadLayLan;
    using tadLayLogica.datos.proyecto;
    using tadLayLan.Tdi;


    public partial class ucDgvSolucionesValorar : UserControl
    {

        private BindingSource mBindMaster;
        private bool mAllowMultipleSeleccion;

        #region "Constructores"
        public ucDgvSolucionesValorar()
        {
            InitializeComponent();
        }
        #endregion


        [Category("_CADnex_Txt")]
        [DefaultValue(false)]
        public virtual bool allowMultipleSeleccion
        {
            get { return mAllowMultipleSeleccion; }
            set { mAllowMultipleSeleccion = value; }
        }



        #region "Propiedades"

        public Guid? solucion
        {
            get
            {
                if (ucDgvSoluciones.SelectedRows.Count == 1)
                {
                    return (Guid)ucDgvSoluciones.CurrentRow.Cells["Id"].Value;
                }
                else
                {
                    return null;

                }
                                      
            }

        }


        public string solucionNombre 
        {
             get
            {

                if (ucDgvSoluciones.SelectedRows.Count == 1)
                {
                    return (string) ucDgvSoluciones.CurrentRow.Cells["nombre"].Value;
                }
                else
                {
                    return string.Empty;

                }


            }

         }

        public List<Guid> lstSolucionesValorar
        {
            get
            {
                List<Guid> miLst = new List<Guid>();

                foreach (DataGridViewRow item in ucDgvSoluciones.Rows)
                {
                    if (item.Cells[1].Value != null && item.Cells["idSelect"].Value.Equals(true))
                    {
                        miLst.Add((Guid)item.Cells["id"].Value);
                    }              
                }

                return miLst;        
            }
        }
        #endregion

        #region "Metodos"

        public void populate()
        {

            var results = from table1 in oSingletonDsApp.getInstance.dataset.tbSolucion
                          where ! table1.IsisCompleteObraLinealNull() && table1.isCompleteObraLineal==true
                          join table2 in oSingletonDsApp.getInstance.dataset.tbSolucionRoad.AsEnumerable() on (Guid)table1["id"] equals (Guid)table2["id"]
                          select new
                          {
                              id = (Guid)table1["id"],
                              nombre = (string)table1["nombre"],
                              isCompleteObraLineal = (bool)table1["isCompleteObraLineal"],
                              grupoCarretera = (string)table2["grupoCarretera"],
                              velocidadProyecto = (double)table2["velocidadProyecto"],
                              radioProyecto = (double)table2["radioProyecto"]
                           
                          };


            mBindMaster = new BindingSource();
            mBindMaster.DataSource = results;


            string miQuery = "isCompleteObraLineal = {0}";
            mBindMaster.Filter = string.Format(miQuery, Convert.ToString(true));

            ucDgvSoluciones.DataSource = mBindMaster;

        }
        public void clearSelection ()
        {
            ucDgvSoluciones.ClearSelection();
        }

        #endregion


        #region "Eventos"
        private void ucDgvSolucionesValorar_Load(object sender, EventArgs e)
        {


            ucDgvSoluciones.dgvSetUpUIDefault(false);

            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn miIdSelect = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miGrupo = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miVelocidad = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miRadio = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miLonKm = new DataGridViewTextBoxColumn();

            miId.Name = "id";
            miId.HeaderText = "id";
            miId.DataPropertyName = "id";

            miIdSelect.Name = "idSelect";
            miIdSelect.HeaderText = strFrmSolucion.uiSeleccion;
            miIdSelect.Width = 50;

            miNombre.Name = "nombre";
            miNombre.HeaderText = strFrmSolucion.uiNombre;
            miNombre.DataPropertyName = "nombre";
            miNombre.MinimumWidth = 200;

            miGrupo.HeaderText = strFrmRoadVpRadio.uiGrupoCarreteras;
            miGrupo.DataPropertyName = "grupoCarretera";

            miVelocidad.HeaderText = strFrmRoadVpRadio.uiVp;
            miVelocidad.DataPropertyName = "velocidadProyecto";

            miRadio.HeaderText = strFrmRoadVpRadio.uiRp;
            miRadio.DataPropertyName = "radioProyecto";

            ucDgvSoluciones.Columns.Add(miId);
            ucDgvSoluciones.Columns.Add(miIdSelect);
            ucDgvSoluciones.Columns.Add(miNombre);
            ucDgvSoluciones.Columns.Add(miGrupo);
            ucDgvSoluciones.Columns.Add(miVelocidad);
            ucDgvSoluciones.Columns.Add(miRadio);

            ucDgvSoluciones.dgvColumnsHide(new int[] { 0 });

        }
        private void ucDgvSoluciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && ucDgvSoluciones.CurrentRow != null)
            {

                if (mAllowMultipleSeleccion)
                {
                    if (ucDgvSoluciones.CurrentRow.Cells["idSelect"].Value == null)
                    {
                        ucDgvSoluciones.CurrentRow.Cells["idSelect"].Value = true;
                    }
                    else
                    {
                        ucDgvSoluciones.CurrentRow.Cells["idSelect"].Value = !(bool)ucDgvSoluciones.CurrentRow.Cells[1].Value;
                    }
                }
                else
                {
                    int miIndex = ucDgvSoluciones.CurrentRow.Index;

                    foreach (DataGridViewRow item in ucDgvSoluciones.Rows)
                    {
                        if (item.Index == miIndex)
                        {
                            item.Cells["idSelect"].Value = true;
                        }
                        else
                        {
                            item.Cells["idSelect"].Value = false;
                        }                             
                    }

                }
            }
        }
        #endregion





 








    }
}
