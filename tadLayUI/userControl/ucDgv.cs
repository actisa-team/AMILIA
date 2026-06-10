using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.userControl
{
    using System.Drawing;
    using System.Windows.Forms;
    
    public class ucDgv :DataGridView
    {

        public ucDgv()
            :base ()
        {
            dgvSetUp();
        }


        
        protected virtual void dgvSetUp()
        {

            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            AlternatingRowsDefaultCellStyle.BackColor = Color.Snow;

            this.EnabledChanged += new EventHandler(ucDgv_EnabledChanged);
        }



        #region "Propiedades"
        #endregion




        #region "Metodos"

        public virtual void dgvSetUpUIDefault(bool iAutoGenerateColumns)
        {
            AutoGenerateColumns = iAutoGenerateColumns; ;
            AllowUserToResizeColumns = false;
            RowHeadersVisible = false;
            AllowUserToAddRows = false;
            MultiSelect = false;
            AllowUserToResizeRows = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ReadOnly = true;
        }


        /// <summary>
        /// ucDgvMatriz.dgvColumnsHide(new int[] { 0, 2 });
        /// </summary>
        public virtual void dgvColumnsHide(int[] iLstIndexColumnsBaseCero)
        {
            foreach (var item in iLstIndexColumnsBaseCero)
            {
                this.Columns[item].Visible = false;
            }
        }



   

        #endregion


        #region "Eventos"


        void ucDgv_EnabledChanged(object sender, EventArgs e)
        {

            if (Enabled == false)
            {
                DefaultCellStyle.SelectionBackColor = Color.Gray;
                DefaultCellStyle.SelectionForeColor = Color.Green;
            }
            else
            {
                DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight);
                DefaultCellStyle.SelectionForeColor = Color.White;

            }
        }


        #endregion












    }
}
