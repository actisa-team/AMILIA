using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminPresupuesto
{

    using System.IO;
    using tadLayData;
    using tadLayLogica;

    using tadLayLogica.datos.precios;
    using tadLayLogica.datos.Gis;
    using tadLayLan;
    
    public partial class frmPreciosFactory : Form
    {



        dsBd mDs;

        string mFile;
        
      
        
        public frmPreciosFactory(string iFileData)
        {
            InitializeComponent();

            if (File.Exists(iFileData))
            {
                mFile = iFileData;
            }
            else
            {
                throw new FileNotFoundException(iFileData);
            }

            mDs = new dsBd();

            mDs.ReadXml(mFile);

        }

        private void saveXml()
        {

            try
            {

                mDs.AcceptChanges();

                mDs.WriteXml(mFile);

                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
            }
            catch (Exception ex)
            {

                oTadil.data.UserInfo.showError(ex);
            }
            
        }


        private void button1_Click(object sender, EventArgs e)
        {


           
            dsBd mDsPre = new dsBd();

            //Creo las Unidades

            mDsPre.tbUds.AddtbUdsRow("ML");
            mDsPre.tbUds.AddtbUdsRow("M2");
            mDsPre.tbUds.AddtbUdsRow("M3");
            mDsPre.tbUds.AddtbUdsRow("KM");
            mDsPre.tbUds.AddtbUdsRow("%");


            //Creo los Grupos

           // dsGis.tbZgFichaRow myFicha;

            dsBd.tbGruposRow miGrupo = mDsPre.tbGrupos.NewtbGruposRow();

            miGrupo.idGrupo = "EXC";
            miGrupo.idUd = "M3";


            mDsPre.tbGrupos.AddtbGruposRow(miGrupo);



            mDsPre.WriteXml(@"d:\precios.xml");






        }


   

        private void btnLoadGrupo_Click(object sender, EventArgs e)
        {

            //Vinculo los DATAGRIDVIEW
            BindingSource myGrupo = new BindingSource();
            myGrupo.DataMember = mDs.tbGrupos.TableName;
            myGrupo.DataSource = mDs;

            dgvGrupos.DataSource = myGrupo;

            mDs.AcceptChanges();

        }

        private void btnGrupoSave_Click(object sender, EventArgs e)
        {

            try
            {
                saveXml();
            }
            catch (Exception ex )
            {

                oTadil.data.UserInfo.showError(ex);
            }
            

        }

        private void btnLoadGrupoClasificacion_Click(object sender, EventArgs e)
        {
            //Creo el DATASET
            mDs = new dsBd();

            //Cargo con datos el DataSet
            mDs.ReadXml(mFile);


      
            //Vinculo los DATAGRIDVIEW
            BindingSource myMaster = new BindingSource();
            myMaster.DataMember = mDs.tbGrupos.TableName;
            myMaster.DataSource = mDs;


            BindingSource myDetail = new BindingSource();
            myDetail.DataSource = myMaster;
            myDetail.DataMember = "FK_tbGrupo_tbClasificacion";

            dgvGrupoMaster.DataSource = myMaster;
            dgvClasificacion.DataSource = myDetail;



            ////Segundo Enlace
            //BindingSource myClasificacion = new BindingSource();
            //myClasificacion.DataSource = myDetail;
            //myClasificacion.DataMember = "FK_tbZgFicha_tbZgClasificaciones";

            //dgvGrupoMaster.DataSource = myClasificacion;


            ////Tercer Enlace
            //BindingSource myDetalle = new BindingSource();
            //myDetalle.DataSource = myClasificacion;
            //myDetalle.DataMember = "FK_tbZgClasificaciones_tbZgItems";

            //dgvItems.DataSource = myDetalle;


        }

        private void btnSubGrupo_Click(object sender, EventArgs e)
        {

            //Creo el DATASET
            mDs = new dsBd();

            //Cargo con datos el DataSet
            mDs.ReadXml(mFile);

            string miGrupo = "REL";
            string miClas = "PLA";

            string miQuery = "idGrupo  like  '{0}%' AND code like '{1}%'";



            //Vinculo los DATAGRIDVIEW
            BindingSource myMaster = new BindingSource();
            myMaster.DataMember = mDs.tbClasificaciones.TableName;
            myMaster.Filter = string.Format(miQuery, miGrupo, miClas);
            myMaster.DataSource = mDs;


            DataRowView miRow = (DataRowView)myMaster.Current;

            bool miHasPrecioSecundario = Convert.ToBoolean( miRow["hasPrecioSecundario"]);

           


            BindingSource myDetail = new BindingSource();
            myDetail.DataSource = myMaster;
            myDetail.DataMember = "FK_tbClasificaciones_tbItems";

            dgvItems.DataSource = myDetail;

            dgvItems.Columns[0].Visible = false;
            dgvItems.Columns[3].Visible = miHasPrecioSecundario;
            dgvItems.Columns[4].Visible = false;


        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            ////Creo el DATASET
            //mDs = new dsBd();

            ////Cargo con datos el DataSet
            //mDs.ReadXml(mFile);

            //mDs.tbMoneda.AddtbMonedaRow();

            //mDs.tbMoneda.AddtbMonedaRow("€", "Euro",0);

            //mDs.AcceptChanges();

            //mDs.WriteXml(@"d:\precios.xml");

        }

        private void button3_Click(object sender, EventArgs e)
        {

            //Creo el DATASET

            dsBd mDsPre = new dsBd();

        
            
            //Creo las Unidades

            mDsPre.tbUds.AddtbUdsRow("ML");
            mDsPre.tbUds.AddtbUdsRow("M2");
            mDsPre.tbUds.AddtbUdsRow("M3");
            mDsPre.tbUds.AddtbUdsRow("KM");
            mDsPre.tbUds.AddtbUdsRow("%");


            //Creo los Grupos

            // dsGis.tbZgFichaRow myFicha;

            dsBd.tbGruposRow miGrupo = mDsPre.tbGrupos.NewtbGruposRow();

            miGrupo.idGrupo = "EXC";
            miGrupo.idUd = "M3";


            mDsPre.tbGrupos.AddtbGruposRow(miGrupo);



            mDsPre.WriteXml(@"d:\precios.xml");


        }



        //CREAR ZONAS GEO
        private void button4_Click(object sender, EventArgs e)
        {

               
 

        }

        private void button2_Click(object sender, EventArgs e)
        {
          
 
 
        }


        private void btnGeoLoad_Click(object sender, EventArgs e)
        {



        }





 



      








   
    }
}
