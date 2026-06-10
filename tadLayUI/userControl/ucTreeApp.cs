using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.userControl
{
    using System.Windows.Forms;

    using tadLayLogica;
    using engNet.eventos;
    using tadLayLan;
    using tadLayShare;
    using tadLayLan.Tdi;
    using System.Drawing;
    
    public class ucTreeApp : ucTreePrecios
    {



       public ucTreeApp()
         :base()
       { 
          
       }



       public void populateByEstudio(eEstudioTipo iEstudioTipo)
       {



           tree.Nodes.Clear();

           mLstItem = new List<TreeViewItem>();
           TreeViewItem miLeaf;

           //Creo el Nodo Padre
           TreeViewItem miRoot = new TreeViewItem() { ParentID = "padre", ID = "root", descripcion = strFrmApp.uiDatos };

           mLstItem.Add(miRoot);

           //Creo los Item Grupos
           TreeViewItem miFolderConfig = new TreeViewItem("padre", "CONCON", 1, strFrmApp.uiCONCON);
           mLstItem.Add(miFolderConfig);

           //GRUPO CONFIGURACION RUTAS
           miLeaf = new TreeViewItem("CONCON", "CONFIL", 1, strFrmApp.uiCONFIL);

           //HIJOS
           mLstItem.Add(miLeaf);

           //GRUPO DATOS PROYECTO
           TreeViewItem miFolderDatos = new TreeViewItem("padre", "DATDAT", 2, strFrmApp.uiDATDAT);
           mLstItem.Add(miFolderDatos);

           //HIJOS
           miLeaf = new TreeViewItem("DATDAT", "DATPRO", 1, strFrmApp.uiDATPRO);
           mLstItem.Add(miLeaf);
           miLeaf = new TreeViewItem("DATDAT", "DATTER", 2, strFrmApp.uiDATTER);
           mLstItem.Add(miLeaf);
           miLeaf = new TreeViewItem("DATDAT", "PTOORI", 3, strFrmApp.uiPTOORI);
           mLstItem.Add(miLeaf);
           miLeaf = new TreeViewItem("DATDAT", "PTODES", 4, strFrmApp.uiPTODES);
           mLstItem.Add(miLeaf);
           //miLeaf = new TreeViewItem("DATDAT", "ESTVIS", 5, strFrmApp.uiESTVIS);
           //mLstItem.Add(miLeaf);


           if (iEstudioTipo == eEstudioTipo.ESTPRE)
           {
               TreeViewItem miFolderEstudioPrevio = new TreeViewItem("padre", "ESTPRE", 3, strFrmApp.uiESTPRE);
               mLstItem.Add(miFolderEstudioPrevio);

               miLeaf = new TreeViewItem("ESTPRE", "EJEVIS", 1, strFrmApp.uiEJEVIS);
               mLstItem.Add(miLeaf);

               miLeaf = new TreeViewItem("ESTPRE", "EJEBAS", 2, strFrmApp.uiEJEBAS);
               mLstItem.Add(miLeaf);

               miLeaf = new TreeViewItem("ESTPRE", "SOLEDI", 3, strFrmApp.uiSOLEDI);
               mLstItem.Add(miLeaf);

           }
           else if (iEstudioTipo == eEstudioTipo.ESTINF)
           {
               TreeViewItem miFolderEstudioInformativo = new TreeViewItem("padre", "ESTINF", 3, strFrmApp.uiESTINF);
               mLstItem.Add(miFolderEstudioInformativo);



               //HIJOS  ESTUDIO INFORMATIVO

               TreeViewItem miFolderSeccionesZonasGenerales = new TreeViewItem("ESTINF", "INFDAT", 1, strFrmApp.uiINFDAT);
               mLstItem.Add(miFolderSeccionesZonasGenerales);

               miLeaf = new TreeViewItem("ESTINF", "EJEVIS", 2, strFrmApp.uiEJEVIS);
               mLstItem.Add(miLeaf);

               miLeaf = new TreeViewItem("ESTINF", "EJEBAS", 3, strFrmApp.uiEJEBAS);
               mLstItem.Add(miLeaf);

               miLeaf = new TreeViewItem("ESTINF", "SOLEDI", 4, strFrmApp.uiSOLEDI);
               mLstItem.Add(miLeaf);

               TreeViewItem miFolderPresupuestos = new TreeViewItem("ESTINF", "PREPRE", 5, strFrmApp.uiPREPRE);
               mLstItem.Add(miFolderPresupuestos);

               TreeViewItem miFolderRentabilidad = new TreeViewItem("ESTINF", "RENREN", 6, strFrmApp.uiRENREN);
               mLstItem.Add(miFolderRentabilidad);

               TreeViewItem miFolderValoracion = new TreeViewItem("ESTINF", "VALVAL", 7, strFrmApp.uiVALVAL);
               mLstItem.Add(miFolderValoracion);

               TreeViewItem miFolderInformes = new TreeViewItem("ESTINF", "INFINF", 8, strFrmApp.uiINFINF);
               mLstItem.Add(miFolderInformes);



               //HIJOS RENTABILIDAD
               miLeaf = new TreeViewItem("RENREN", "INDATE", 1, strFrmRentabilidad.uiINDATE);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("RENREN", "INVTIP", 2, strFrmRentabilidad.uiINVTIP);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("RENREN", "DATTRA", 3, strFrmRentabilidad.uiDATTRA);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("RENREN", "COSACC", 4, strFrmRentabilidad.uiCOSACC);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("RENREN", "COTIFU", 5, strFrmRentabilidad.uiCOTIFU);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("RENREN", "GACORE", 6, strFrmRentabilidad.uiGACORE);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("RENREN", "VEHCON", 7, strFrmRentabilidad.uiVEHCON);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("RENREN", "VEHMAN", 8, strFrmRentabilidad.uiVEHMAN);
               mLstItem.Add(miLeaf);

               //HIJOS VALORACION
               miLeaf = new TreeViewItem("VALVAL", "VALTRA", 1, strFrmValoracion.uiVALTRA);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("VALVAL", "VALGEO", 2, strFrmValoracion.uiVALGEO);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("VALVAL", "VAESTU", 3, strFrmValoracion.uiVAESTU);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("VALVAL", "VALMED", 4, strFrmValoracion.uiVALMED);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("VALVAL", "VALCLI", 5, strFrmValoracion.uiVALCLI);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("VALVAL", "VALSOC", 6, strFrmValoracion.uiVALSOC);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("VALVAL", "VALPAT", 7, strFrmValoracion.uiVALPAT);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("VALVAL", "VALECO", 8, strFrmValoracion.uiVALECO);
               mLstItem.Add(miLeaf);
               miLeaf = new TreeViewItem("VALVAL", "VALMAT", 9, strFrmValoracion.uiVALMAT);
               mLstItem.Add(miLeaf);

               //HIJOS PRESUPUESTO
               miLeaf = new TreeViewItem("PREPRE", "SETPRE", 1, strFrmPresupuesto.uiSETPRE);
               mLstItem.Add(miLeaf);

               //HIJOS GESTOR INFORMES
               miLeaf = new TreeViewItem("INFINF", "INFSOL", 1, strFrmInformes.uiINFSOL);
               mLstItem.Add(miLeaf);

           }
           else
           {
               throw new oExEnumNotImplemented(iEstudioTipo.ToString());
           }

           mLstItem.Sort(delegate(TreeViewItem item1, TreeViewItem item2)
           {
               return item1.orden.CompareTo(item2.orden);

           });



           PopulateTreeView("padre", null);
           tree.ExpandAll();

       }

       public void cambiaColorTreeNodeAt(string iID)
       {
           foreach (TreeNode i in tree.Nodes)
           {
               if (i.Name == "ESTINF")
               {
                   foreach (TreeNode miHijo in i.Nodes)
                   {
                       if (miHijo.Name == "RENREN")
                       {
                           foreach (TreeNode miHijo2 in miHijo.Nodes)
                           {
                               if (miHijo2.Name.Equals(iID))
                               {
                                   miHijo2.ForeColor = Color.Black;
                               }
                           }
                       }

                       if (miHijo.Name == "VALVAL")
                       {
                           foreach (TreeNode miHijo2 in miHijo.Nodes)
                           {
                               if (miHijo2.Name.Equals(iID))
                               {
                                   miHijo2.ForeColor = Color.Black;
                               }
                           }
                       }
                       if (miHijo.Name == "INFINF")
                       {
                           foreach (TreeNode miHijo2 in miHijo.Nodes)
                           {
                               if (miHijo2.Name.Equals(iID))
                               {
                                   miHijo2.ForeColor = Color.Black;
                               }
                           }
                       }
                   }
               }
           }

           //PopulateTreeView("padre", null);
           tree.ExpandAll();
       }

       

       protected override void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
       {

           if (e.Node.Name != "root" && e.Node.Nodes.Count == 0)
           {
               if (e.Node.ForeColor == Color.Gray)
               {
                   oTadil.data.UserInfo.showInfo(strGeneralUser.uiCompletarLasPestAnteriores);
               }
               else
               {
                   TreeViewItem miItem = (TreeViewItem)(from p in mLstItem where p.ID == e.Node.Name select p).First();

                   evThrowCode(miItem.ParentID, miItem.ID);
               }
           }
           else
           {

               evThrowCode(string.Empty, string.Empty);
           }

           evThrowPath(e.Node.FullPath);

       }


    }
}
