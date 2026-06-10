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
    using System.Resources;
    using System.Reflection;

    using System.ComponentModel;

    using tadLayLan;

    public partial class ucRmrTabla : UserControl
    {
        public ucRmrTabla()
        {
            InitializeComponent();

           
        }


        private void ucRmrTabla_Load(object sender, EventArgs e)
        {
            dgv.DataSource = tbRmr.getTbRmr();


            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AutoGenerateColumns = false;
            dgv.MultiSelect = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.ReadOnly = true;


            dgv.Columns[0].HeaderText = strRmr.uiRMR;
            dgv.Columns[1].HeaderText = strRmr.uiEXC;
            dgv.Columns[2].HeaderText = strRmr.uiBUL;
            dgv.Columns[3].HeaderText = strRmr.uiGUT;
            dgv.Columns[4].HeaderText = strRmr.uiCER;

            lblTabla.Text = strRmr.uiTbName;

        }
    }



    internal class tbRmr
    {

   
        public string rmr { get; set;}
        public string exc { get; set;}
        public string bul { get; set;}
        public string gut { get; set;}
        public string cer { get; set;}


        public tbRmr()
        { }



        public static List<tbRmr> getTbRmr()
        {

            List<tbRmr> miLst = new List<tbRmr>
            {          
                new tbRmr { rmr=">81",   exc= strRmr.ui81EXC, bul=strRmr.ui81BUL, gut= strRmr.ui81GUN, cer= strRmr.ui81CER},
                new tbRmr { rmr="61-80", exc= strRmr.ui80EXC, bul=strRmr.ui80BUL, gut= strRmr.ui80GUN, cer= strRmr.ui80CER},
                new tbRmr { rmr="41-60", exc= strRmr.ui60EXC, bul=strRmr.ui60BUL, gut= strRmr.ui60GUN, cer= strRmr.ui60CER},
                new tbRmr { rmr="21-40", exc= strRmr.ui40EXC, bul=strRmr.ui40BUL, gut= strRmr.ui40GUN, cer= strRmr.ui40CER},
                new tbRmr { rmr="<20",   exc= strRmr.ui20EXC, bul=strRmr.ui20BUL, gut= strRmr.ui20GUN, cer= strRmr.ui20CER}

            };

          
            return miLst;

       }
        
        
        
        
        }
    
    
    
    

}
