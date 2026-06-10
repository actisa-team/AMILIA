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
    
    public partial class ucWinColor : UserControl, IisValido
    {



     
        public ucWinColor()
        {
            InitializeComponent();
        }


        [Category("_CADnex_01")]
        [DefaultValue(true)]
        public virtual bool isObligatorio
        {
            get { return panColor.isObligatorio; }
            set { panColor.isObligatorio = value; }
        }

        [Category("_CADnex")]
        public panelNex panel
        {
            get { return this.panColor; }
        }

        [Category("_CADnex")]
        public string uiLbl
        {
            get
            {
                return lbl.Text;
            }
            set
            {
                lbl.Text = value;
            }
        }


        [Category("_CADnex")]
        public int ancho
        {
            get { return panColor.Width; }
            set { panColor.Width = value; }
        }

        [Category("_CADnex")]
        public Point location
        {
            get { return panColor.Location; }
            set { panColor.Location = value; }
        }

        private void ucWinColor_Load(object sender, EventArgs e)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes[typeof(DefaultValueAttribute)];
                if (attr != null)
                {
                    prop.SetValue(this, attr.Value);
                }
            }
        }


        public bool isValido()
        {
            return this.panColor.isValido();
        }
    }


    public class panelNex : Panel, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public bool isObligatorio { get; set; }

        private int mColorInt;

        private ErrorProvider error = new ErrorProvider();

        public panelNex()
        {
            this.CausesValidation = true;
            this.Validating += new CancelEventHandler(panelNex_Validating);
            this.MouseDoubleClick += new MouseEventHandler(panelNex_MouseDoubleClick);
            this.BackColor = Color.Transparent;
        }

  
         public bool isValido()
        {
            panelNex_Validating(this, new CancelEventArgs());

            if (error.GetError(this) == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       

        private void panelNex_Validating(object sender, CancelEventArgs e)
        {
            if (isObligatorio)
            {
                if (this.BackColor == Color.Transparent)
                {
                    error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
                    error.SetError(this, strError.eValorObligatorio);
                    return;
                }
            }
        }

        //protected override void OnEnter(EventArgs e)
        //{
        //    error.Clear();
        //    base.OnEnter(e);
        //}


        //protected override void OnLeave(EventArgs e)
        //{
        //    base.OnLeave(e);

        //    if (this.BackColor == Color.Transparent)
        //    {
        //        error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
        //        error.SetError(this, strError.eValorObligatorio);
        //        this.CausesValidation = false;
        //    }
        //    else
        //    {
        //        this.CausesValidation = true;
        //    }

        //}


        void panelNex_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ColorDialog miColor = new ColorDialog();

            if (miColor.ShowDialog() == DialogResult.OK)
            {
                colorInt = miColor.Color.ToArgb();

                error.Clear();
            }

        }


        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                this.BackColor = Color.FromArgb(colorInt);
                
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        
        public int colorInt
        {
            get
            {
                return mColorInt;
            
            }

            set
            {                   
                if (value != this.mColorInt)
                {
                    mColorInt = value;          
                    NotifyPropertyChanged("ColorInt");       
                }
            }

        }





       
    }


}
