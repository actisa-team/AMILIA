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
    using engNet.eventos;
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLan;
    using tadLayLan.Tdb;
    
    
    public partial class ucTreeGis : UserControl
    {

        public event EventHandler<oEventArgs<string, string>> evNodeCurrent;
        private  List<TreeViewItem> mLstItem;
        
        
        public ucTreeGis()
        {
            InitializeComponent();
        }


        public TreeView tree
        {

            get
            {
                return this.treeGis;
            
            }
        
        }



        public void populate ()
        {

           
            
            TreeViewItem miRoot = null;
            TreeViewItem miFolder = null;
            mLstItem = new List<TreeViewItem>();

     
           
            //CREO EL PADRE
             miRoot = new TreeViewItem("padre", "root", "root", strFrmGisGeneral.uiAdminGis, 0);
             mLstItem.Add(miRoot);
   
            //RELLENO LAS CLASIFICACIONES (GEOTECNICA ; AMBIENTAL ; CLIMATICA ; SOCIOECONOMICAS ; PATRIMONIALES)
            foreach (dsBd.tbZgGrupoRow item in oSingletonDsBd.getInstance.dataset.tbZgGrupo.Rows)
            {
                miFolder = new TreeViewItem("padre",item.id,item.id,strFrmGisGeneral.ResourceManager.GetString("ui"+ item.code),item.orden);    
                mLstItem.Add(miFolder);
            }


            //RELLENO LOS GRUPOS 
            foreach (dsBd.tbZgFichaRow item in oSingletonDsBd.getInstance.dataset.tbZgFicha.Rows)
            {
                miFolder = new TreeViewItem(item.idzggrupo, item.id, item.code, strFrmGisGeneral.ResourceManager.GetString("ui" + item.code), item.orden);
                mLstItem.Add(miFolder); 
            }


            // Añado la Valoracion de los Procedimientos de Cimentacion en Estructuras y Muros
            miFolder = new TreeViewItem("e1", "VALCIM", "VALCIM", strFrmGisGeneral.uiVALCIM, 1);
            mLstItem.Add(miFolder);

            // Añado la Valoracion de los Procedimientos de TUNELES
            miFolder = new TreeViewItem("e2", "VALTUN", "VALTUN", strFrmGisGeneral.uiVALTUN, 1);
            mLstItem.Add(miFolder);


            //Añado los Formularios de Valoraciones Excavabilidad-Talud ; Movimiento Tierras
            miFolder = new TreeViewItem("e3", "VAEXTA", "VAEXTA", strFrmGisGeneral.uiVAEXTA, 1);   
            mLstItem.Add(miFolder);


            //Ordeno el Arbol
            mLstItem.Sort(delegate(TreeViewItem item1, TreeViewItem item2)
            {
                return item1.orden.CompareTo(item2.orden);
            });


           //Relleno el Arbol
            PopulateTreeView("padre", null);

            treeGis.ExpandAll();

   
        }




        private void PopulateTreeView(string parentId, TreeNode parentNode)
        {
            var filteredItems = mLstItem.Where(item => item.ParentID == parentId);


            TreeNode childNode;


            foreach (var i in filteredItems.ToList())
            {
                if (parentNode == null)
                {
                    childNode = treeGis.Nodes.Add(i.ID.ToString(), i.descripcion);
                }
                else
                {
                    childNode = parentNode.Nodes.Add(i.ID.ToString(), i.descripcion);
                }

                PopulateTreeView(i.ID, childNode);
            }
        }


        #region "Eventos"

        /// <summary>
        /// Lanzo el Evento con los Datos del ID y CODE para el Traductor
        /// </summary>
        private void treeGis_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
      
            if (e.Node.Level == 1 || e.Node.Level==2)
            {

                TreeViewItem miItem = (TreeViewItem)(from p in mLstItem where p.ID == e.Node.Name select p).First();

                if (evNodeCurrent != null)
                {
                    evNodeCurrent(this, new oEventArgs<string, string>(miItem.ID, miItem.code));
                }
   
            }
            else
            {
                
                if (evNodeCurrent != null)
                {
                    evNodeCurrent(this, new oEventArgs<string, string>("null", "null"));
                }
                
            }
            
        }

        #endregion

    }
}
