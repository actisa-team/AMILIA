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
    
    public class ucTreeSecciones : ucTreePrecios
    {


      

       public ucTreeSecciones()
         :base()
       { 
          
       }

       public override void populate()
       {

               tree.Nodes.Clear();

               mLstItem = new List<TreeViewItem>();


               //Creo el Nodo Padre
               TreeViewItem miRoot = new TreeViewItem() { ParentID = "padre", ID = "root", descripcion = strFrmSecciones.uiAdminSecciones };

               mLstItem.Add(miRoot);


               //Creo los Item Grupos
               TreeViewItem miFolderCuneta = null;
               TreeViewItem miFolderCarretera = null;

                
                //Nivel Inferior
                TreeViewItem miFolder = null;

                //GRUPO CUNETA
                miFolderCuneta = new TreeViewItem("padre", "CUNETA" ,1,strFrmSecciones.uiCUNETAS);
                mLstItem.Add(miFolderCuneta);

                miFolder = new TreeViewItem("CUNETA", "TRIANG" ,1,strFrmSecciones.uiTRIANG);
                mLstItem.Add(miFolder);
                miFolder = new TreeViewItem("CUNETA", "TRAPEZ" ,1,strFrmSecciones.uiTRAPEZ);
                mLstItem.Add(miFolder);

                //GRUPO CARRETERA
                 miFolderCarretera = new TreeViewItem("padre", "CARRETE" ,2,strFrmSecciones.uiCARRETERAS);
                 mLstItem.Add(miFolderCarretera);

                //CALZADA UNICA
                miFolder = new TreeViewItem("CARRETE", "CALUNI" ,1,strFrmSecciones.uiCalzadaUnica);
                mLstItem.Add(miFolder);
                miFolder = new TreeViewItem("CALUNI", "UNIGEN" ,1,strFrmSecciones.uiUNIGEN);
                mLstItem.Add(miFolder);

                //CALZADA DOBLE CON MEDIANA
                miFolder = new TreeViewItem("CARRETE", "CALDOB" ,2,strFrmSecciones.uiCalzadaDoble);
                mLstItem.Add(miFolder);
                miFolder = new TreeViewItem("CALDOB", "DOBAUT" ,1,strFrmSecciones.uiDOBAUT);
                mLstItem.Add(miFolder);

                //CALZADA DOBLE SIN MEDIANA
                miFolder = new TreeViewItem("CALDOB", "DOBSIN" ,3,strFrmSecciones.uiDOBSIN);
                mLstItem.Add(miFolder);
 
               mLstItem.Sort(delegate(TreeViewItem item1, TreeViewItem item2)
               {
                   return item1.orden.CompareTo(item2.orden);
               });



           PopulateTreeView("padre", null);

           tree.ExpandAll();

       }




      


       protected override void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
       {

           if (e.Node.Name != "root" &&   e.Node.Nodes.Count == 0)
           {
                   TreeViewItem miItem = (TreeViewItem)(from p in mLstItem where p.ID == e.Node.Name select p).First();

                   evThrowCode(miItem.ParentID, miItem.ID);            
           }

           evThrowPath(e.Node.FullPath);

       }


    }
}
