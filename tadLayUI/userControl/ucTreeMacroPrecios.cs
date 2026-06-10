using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdb;

namespace tadLayUI.userControl
{
    using System.Windows.Forms;
    

    using engNet.eventos;
    using tadLayLan;
    
    public class ucTreeMacroPrecios : ucTreePrecios
    {


      

       public ucTreeMacroPrecios()
         :base()
       { 
          
       }



       public override void populate()
       {

               tree.Nodes.Clear();

               mLstItem = new List<TreeViewItem>();


               //Creo el Nodo Padre
               TreeViewItem miRoot = new TreeViewItem() { ParentID = "padre", ID = "root", descripcion = strFrmMacroPrecios.uiAdminMacroPrecios };

               mLstItem.Add(miRoot);


               //Creo los Item Grupos
               TreeViewItem miFolderCarretera = null;

                
                //Nivel Inferior
                TreeViewItem miFolder = null;

                //GRUPO CARRETERA
                 miFolderCarretera = new TreeViewItem("padre", "ROAD" ,2,strFrmSecciones.uiCARRETERAS);
                 mLstItem.Add(miFolderCarretera);

                //CALZADA UNICA
                miFolder = new TreeViewItem("ROAD", "CALUNI" ,1,strFrmSecciones.uiCalzadaUnica);
                mLstItem.Add(miFolder);
                miFolder = new TreeViewItem("CALUNI", "UNIGEN" ,1,strFrmSecciones.uiUNIGEN);
                mLstItem.Add(miFolder);

                //CALZADA DOBLE
                miFolder = new TreeViewItem("ROAD", "CALDOB" ,2,strFrmSecciones.uiCalzadaDoble);
                mLstItem.Add(miFolder);
                miFolder = new TreeViewItem("CALDOB", "DOBAUT" ,1,strFrmSecciones.uiDOBAUT); //AUTOVIA
                mLstItem.Add(miFolder);
                miFolder = new TreeViewItem("CALDOB", "DOBSIN" ,3,strFrmSecciones.uiDOBSIN); //SIN MEDIANA
                mLstItem.Add(miFolder);

                //miFolder = new TreeViewItem("CALDOB", "DOBURB" ,2,strFrmSecciones.uiDOBURB); //DOBLE URBANA
                //mLstItem.Add(miFolder);
   
               mLstItem.Sort(delegate(TreeViewItem item1, TreeViewItem item2)
               {
                   return item1.orden.CompareTo(item2.orden);
               });



           PopulateTreeView("padre", null);

           tree.ExpandAll();

       }




      
       protected override void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
       {

           //Nodos LEAF
           if (e.Node.Name != "root" && e.Node.Nodes.Count == 0)
           {
               TreeViewItem miItem = (TreeViewItem)(from p in mLstItem where p.ID == e.Node.Name select p).First();

               evThrowCode(miItem.ParentID, miItem.ID);
           }
           //Nodos Padres
           else
           {
               evThrowCode(string.Empty, string.Empty);
           }

           evThrowPath(e.Node.FullPath);

       }


    }
}
