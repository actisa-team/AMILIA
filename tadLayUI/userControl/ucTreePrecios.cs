using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.userControl
{

    using engNet.eventos;

    using tadLayData;
    using tadLayLan;
    using tadLayLogica.datos.BaseDatos;
    
    

   
    
    public partial class ucTreePrecios : UserControl
    {


       public event EventHandler<oEventArgs<string, string>> evNodeCurrentCode;
       public event EventHandler<oEventArgs<string>> evNodeCurrentPath;
       

       protected  List<TreeViewItem> mLstItem;
        
        
        public ucTreePrecios()
        {
            InitializeComponent();

            tree.NodeMouseClick += new TreeNodeMouseClickEventHandler(tree_NodeMouseClick);
        }

        #region "Events"
        protected void evThrowPath(string iValue)
        {

            if (evNodeCurrentPath != null)
            {
                evNodeCurrentPath(this, new oEventArgs<string>(iValue));         
            }
                
        }
        protected void evThrowCode(string iValue1, string iValue2)
        {
            if (evNodeCurrentCode != null)
            {
                evNodeCurrentCode(this, new oEventArgs<string, string>(iValue1, iValue2));
            }
        
        }
        protected virtual void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            
            
            
            if (e.Node.Level == 1)
            {

                TreeViewItem miItem = (TreeViewItem)(from p in mLstItem where p.ID == e.Node.Name select p).First();

                evThrowCode(miItem.ParentID, miItem.code);
            }
            else
            {

                evThrowCode("null", "null");
            }


            evThrowPath(e.Node.FullPath);
        }
        #endregion


        public virtual void populate()
        {

                tree.Nodes.Clear();

                mLstItem = new List<TreeViewItem>();


                //Creo el Nodo Padre
                TreeViewItem miRoot = new TreeViewItem() { ParentID = "padre", ID = "root", code = "root", descripcion = strFrmPrecios.uiAdminPrecio, orden=-1 };

                mLstItem.Add(miRoot);


                //Creo los Item Grupos
                TreeViewItem miFolder = null;

                //CREO EL FORMULARIO  UNIDAD MONETARIA

                miFolder = new TreeViewItem("padre", "UDS",0, strFrmUnidadMonetaria.uiUNI);
                mLstItem.Add(miFolder);

                miFolder = new TreeViewItem("UDS","MON","MON", strFrmUnidadMonetaria.uiUNIMON, 0);

                mLstItem.Add(miFolder);
               
                //CREO LOS GRUPOS
                foreach (dsBd.tbGruposRow item in oSingletonDsBd.getInstance.dataset.tbGrupos.Rows)
                {

                    miFolder = new TreeViewItem("padre", item.idGrupo, item.idGrupo, strFrmPrecios.ResourceManager.GetString("ui" + item.idGrupo), item.orden);

                    mLstItem.Add(miFolder);        
                }

                //CREO LAS CLASIFICACIONES
                foreach (dsBd.tbClasificacionesRow item in oSingletonDsBd.getInstance.dataset.tbClasificaciones)
                {
                    miFolder = new TreeViewItem(item.idGrupo, item.idClasificacion.ToString(), item.code, strFrmPrecios.ResourceManager.GetString("ui" + item.code),item.orden);

                    mLstItem.Add(miFolder);
                }

                //ORDENO LA COLECCION
                mLstItem.Sort(delegate(TreeViewItem item1, TreeViewItem item2)
                {
                    return item1.orden.CompareTo(item2.orden);
                });


              
 
            PopulateTreeView("padre", null);

            tree.ExpandAll();
          
        }

        public virtual void clear ()
        {
            this.tree.SelectedNode = null;
        }

        protected void PopulateTreeView(string parentId, TreeNode parentNode)
        {
            
            var filteredItems = mLstItem.Where(item => item.ParentID == parentId);


            TreeNode childNode;


            foreach (var i in filteredItems.ToList())
            {
                if (parentNode == null)
                {
                    childNode = tree.Nodes.Add(i.ID.ToString(), i.descripcion);
                }
                else
                {
                    childNode = parentNode.Nodes.Add(i.ID.ToString(), i.descripcion);
                    if ((i.ID.ToString().Equals("INDATE")) || (i.ID.ToString().Equals("INVTIP")) || (i.ID.ToString().Equals("DATTRA")) || (i.ID.ToString().Equals("COSACC"))
                        || (i.ID.ToString().Equals("COTIFU")) || (i.ID.ToString().Equals("GACORE")) || (i.ID.ToString().Equals("VEHCON")) || (i.ID.ToString().Equals("VEHMAN")))
                    {
                        childNode.ForeColor = Color.Gray;
                    }


                    if ((i.ID.ToString().Equals("VALTRA")) || (i.ID.ToString().Equals("VALGEO")) || (i.ID.ToString().Equals("VAESTU"))
                        || (i.ID.ToString().Equals("VALMED")) || (i.ID.ToString().Equals("VALCLI")) || (i.ID.ToString().Equals("VALSOC"))
                        || (i.ID.ToString().Equals("VALPAT")) || (i.ID.ToString().Equals("VALECO")) || (i.ID.ToString().Equals("VALMAT")))
                    {
                        childNode.ForeColor = Color.Gray;
                    }


                    if ((i.ID.ToString().Equals("INFSOL")))
                    {
                        childNode.ForeColor = Color.Gray;
                    }
                }

                PopulateTreeView(i.ID, childNode);
            }
        }







    }


    public class TreeViewItem
    {
        public string ParentID { get; set; }
        public string ID { get; set; }
        public string code { get; set; }
        public string descripcion { get; set; }
        public int orden { get; set; }


        public TreeViewItem() { ;}

        public TreeViewItem(string iIdParent, string iID, string iCode, string iDes, int iOrden)
        {
            ParentID = iIdParent;
            ID = iID;
            code = iCode;
            descripcion = iDes;
            orden = iOrden;
        }

        public TreeViewItem(string iIdParent, string iID, int iOrden, string iDescripcion)
        {
            ParentID = iIdParent;
            ID = iID;
            orden = iOrden;
            descripcion = iDescripcion;
            code = string.Empty;
           
        }

    }
}
