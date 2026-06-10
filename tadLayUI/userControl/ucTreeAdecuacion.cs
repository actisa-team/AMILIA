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
    
    public partial class ucTreeAdecuacion : ucTreePrecios
    {
        private TreeView treeAdec;
        private TreeView treeView1;

        public ucTreeAdecuacion()
         :base()
       { 
          
       }

       public override void populate()
       {
           tree.Nodes.Clear();
           mLstItem = new List<TreeViewItem>();

           // Un solo nivel plano (todos con ParentID = "padre")
           mLstItem.Add(new TreeViewItem("padre", "Adecuación", 1, "Adecuación y mejoras"));

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

        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.treeAdec = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.LineColor = System.Drawing.Color.Black;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(8, 8);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 97);
            this.treeView1.TabIndex = 1;
            // 
            // treeAdec
            // 
            this.treeAdec.Location = new System.Drawing.Point(0, 0);
            this.treeAdec.Name = "treeAdec";
            this.treeAdec.Size = new System.Drawing.Size(332, 526);
            this.treeAdec.TabIndex = 2;
            // 
            // ucTreeAdecuacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.treeAdec);
            this.Controls.Add(this.treeView1);
            this.Name = "ucTreeAdecuacion";
            this.Controls.SetChildIndex(this.treeView1, 0);
            this.Controls.SetChildIndex(this.tree, 0);
            this.Controls.SetChildIndex(this.treeAdec, 0);
            this.ResumeLayout(false);

        }
    }
}
