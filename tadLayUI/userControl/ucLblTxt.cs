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
    using System.Media;

    using System.ComponentModel;
    
    public partial class ucLblTxt : UserControl , IisValido
    {
        public ucLblTxt()
        {
            InitializeComponent();
        }


        [Category("_CADnex_02")]
        public int ancho
        {
            get { return txt.Width; }
            set { txt.Width = value; }
        }

        [Category("_CADnex_02")]
        public Point location
        {
            get { return txt.Location; }
            set { txt.Location = value; }
        }

        [Category("_CADnex_02")]
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

        public txtNex textbox
        {
            get { return this.txt; }
        }



        [Category("_CADnex_01")]
        public virtual bool isObligatorio
        {
            get { return txt.isObligatorio; }
            set { txt.isObligatorio = value; }
        }


        [Category("_CADnex_01")]
        [DefaultValue(false)]
        public virtual bool isTexto
        {
            get { return txt.isTexto; }
            set { txt.isTexto = value; }
        }

        [Category("_CADnex_02")]
        public string uitxt
        {
            get
            {
                return txt.Text;
            }
            set
            {
                txt.Text = value;
            }
        }








        [Category("_CADnex_Txt")]
        [DefaultValue(false)]
        public virtual bool isMultilinea
        {
            get { return txt.Multiline; }
            set { txt.Multiline = value; }
        }

        [Category("_CADnex_Txt")]
        [DefaultValue(false)]
        public virtual int lonMax
        {
           get { return txt.MaxLength; }       
           set { txt.MaxLength = value; }
        }


        [Category("_CADnex_Num")]
        public virtual bool isNegativo
        {
            get { return txt.isNegativo; }
            set { txt.isNegativo = value; }
        }
        [Category("_CADnex_Num")]
        public virtual bool isEntero
        {
            get { return txt.isEntero; }
            set { txt.isEntero = value; }
        }
        [Category("_CADnex_Num")]
        public virtual bool isSimboloDecimalPunto
        {
            get { return txt.isSimboloDecimalPunto; }
            set { txt.isSimboloDecimalPunto = value; }
        }

        [Category("_CADnex_Num")]
        [DefaultValue(-1)]
        public virtual double valorMinimo
        {
            get { return txt.valorMin; }
            set { txt.valorMin = value; }
        }
        [Category("_CADnex_Num")]
        [DefaultValue(-1)]
        public virtual double valorMaximo
        {
            get { return txt.valorMax; }
            set { txt.valorMax = value; }
        }



        public double valorDouble
        {
            get
            {
                return txt.valorDouble;
            } 
        }


        public double? valorDoubleNull
        {
            get
            {
                return txt.valorDoubleNull;
            }

            set
            {

                


            }
        }


        public int valorInt
        {
            get
            {
                return txt.valorInt;
            }
        }


        public bool isEmptyOrNull ()
        {
            return string.IsNullOrEmpty(this.textbox.Text);
        }
             


        #region "Interface"
        public bool isValido()
        {
            return textbox.isValido();
        }
        #endregion


    }


    public class txtNex : TextBox
    {

        //Propiedades
        public bool isObligatorio { get; set; }
        public bool isTexto { get; set; }
        public bool isNegativo { get; set; }
        public bool isEntero { get; set; }
        public bool isSimboloDecimalPunto { get; set; }
       


        [DefaultValue (-1)]
        public double valorMin { get; set; }
        [DefaultValue(-1)]
        public double valorMax { get; set; }

        private ErrorProvider error = new ErrorProvider();


        //Caracteres Admitidos
        char[] digitos = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ',', '-', '\b' };

        /// <summary>
        /// Constructor
        /// </summary>
        public txtNex()
        {      
            this.CausesValidation = true;
            this.Validating += new CancelEventHandler(oTxtNum_Validating);
            this.ReadOnlyChanged += new EventHandler(txtNum_ReadOnlyChanged);
            this.EnabledChanged += new EventHandler(txtNum_EnabledChanged);
        }






 

        #region "Metodos Publicos"
        public bool isValido()
        {
            oTxtNum_Validating(this, new CancelEventArgs());

            if (error.GetError(this) == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void addError(string iMensaje)
        {
            error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
            error.SetError(this, iMensaje);  
        }
        #endregion


     

        void txtNum_EnabledChanged(object sender, EventArgs e)
        {

            if (this.Enabled)
            {
                this.BackColor = Color.White;
                this.ForeColor = Color.Black;
            }
            else
            {
                error.Clear();
                this.BackColor = Color.WhiteSmoke;
                this.ForeColor = Color.Black;
            }

        }
        void txtNum_ReadOnlyChanged(object sender, EventArgs e)
        {

            if (this.ReadOnly)
            {
                this.Enabled = false;
                this.BackColor = Color.WhiteSmoke;
                this.ForeColor = Color.Black;
            }
            else
            {

                this.Enabled = true;
                this.BackColor = Color.White;
                this.ForeColor = Color.Black;
            }
        }
        void oTxtNum_Validating(object sender, CancelEventArgs e)
        {

            if (isObligatorio)
            {
                //Valido Campo Obligatorio
                if (this.Text == string.Empty | this.Text.Trim().Length == 0)
                {
                    error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
                    error.SetError(this, strError.eValorObligatorio);                      
                    return;
                }


                //ES TEXTO
                if (isTexto)
                {
                    if (this.Text.Length > this.MaxLength)
                    {
                        error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
                        error.SetError(this, strError.eValorStrMaxOutRango);
                        return;
                    }


                }

               //ES NUMERO
                else
                {

                    if (valorMin != -1)
                    {

                        if (valorDouble < valorMin)
                        {
                            error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
                            error.SetError(this, string.Format(strError.eValorMinOutRango, valorMin));
                            return;
                        }
                    }


                    if (valorMax != -1)
                    {
                        if (valorDouble > valorMax)
                        {
                            error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
                            error.SetError(this, string.Format(strError.eValorMaxOutRango, valorMax));
                            return;
                        }
                    }


                }





            }





            

        }


        


         public virtual double? valorDoubleNull
        {
            get
            {

                if (string.IsNullOrEmpty(this.Text))
                {
                    return null;
                }
                
                
                double myValor;

                string mySimboloDecimalCliente = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

                //CASO 1
                if (isSimboloDecimalPunto && mySimboloDecimalCliente == ".")
                {

                    if (double.TryParse(this.Text, out myValor))
                    {
                        return myValor;
                    }
                    else
                    {
                        throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");
                    }
                }

                //CASO 2
                if (isSimboloDecimalPunto && mySimboloDecimalCliente == ",")
                {
                    string myStr = this.Text.Replace(".", ",");

                    if (double.TryParse(myStr, out myValor))
                    {
                        return myValor;
                    }
                    else
                    {
                        throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");
                    }
                }

                //CASO 3, SimboloTextBox = ',' SimboloDecimalSistema = ','
                if (!isSimboloDecimalPunto && mySimboloDecimalCliente == ",")
                {

                    if (double.TryParse(this.Text, out myValor))
                    {
                        return myValor;
                    }
                    else
                    {
                        throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");
                    }
                }

                //CASO 4 SimboloTextBox = ',' SimboloDecimalSistema = '.'
                if (!isSimboloDecimalPunto && mySimboloDecimalCliente == ".")
                {
                    string myStr = this.Text.Replace(",", ".");

                    if (double.TryParse(myStr, out myValor))
                    {
                        return myValor;
                    }
                    else
                    {
                        throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");
                    }
                }



                throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");

            }

            set
            {

                    string miTexto = Convert.ToString(value);


                    if (isSimboloDecimalPunto)
                    {
                        miTexto = miTexto.Replace(",", ".");
                    }
                    else
                    {
                        miTexto = miTexto.Replace(".", ",");
                    }
              
                    
                    base.Text = miTexto;

            }
        }



        /// <summary>
        /// Convertir Txt a Double
        /// </summary>
        public virtual double valorDouble
        {
            get
            {

                if (string.IsNullOrEmpty(this.Text))
                {
                    return 0;
                }
                
                
                double myValor;

                string mySimboloDecimalCliente = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

                //CASO 1
                if (isSimboloDecimalPunto && mySimboloDecimalCliente == ".")
                {

                    if (double.TryParse(this.Text, out myValor))
                    {
                        return myValor;
                    }
                    else
                    {
                        throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");
                    }
                }

                //CASO 2
                if (isSimboloDecimalPunto && mySimboloDecimalCliente == ",")
                {
                    string myStr = this.Text.Replace(".", ",");

                    if (double.TryParse(myStr, out myValor))
                    {
                        return myValor;
                    }
                    else
                    {
                        throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");
                    }
                }

                //CASO 3, SimboloTextBox = ',' SimboloDecimalSistema = ','
                if (!isSimboloDecimalPunto && mySimboloDecimalCliente == ",")
                {

                    if (double.TryParse(this.Text, out myValor))
                    {
                        return myValor;
                    }
                    else
                    {
                        throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");
                    }
                }

                //CASO 4 SimboloTextBox = ',' SimboloDecimalSistema = '.'
                if (!isSimboloDecimalPunto && mySimboloDecimalCliente == ".")
                {
                    string myStr = this.Text.Replace(",", ".");

                    if (double.TryParse(myStr, out myValor))
                    {
                        return myValor;
                    }
                    else
                    {
                        throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");
                    }
                }



                throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Double");

            }

            set
            {

                    string miTexto = Convert.ToString(value);


                    if (isSimboloDecimalPunto)
                    {
                        miTexto = miTexto.Replace(",", ".");
                    }
                    else
                    {
                        miTexto = miTexto.Replace(".", ",");
                    }
              
                    

                    base.Text = miTexto;

  
      
            }
        }




 

        /// <summary>
        /// Convertir Txt a Integer
        /// </summary>
        public virtual int valorInt
        {

            get
            {
                int myValor;

                if (int.TryParse(this.Text, out myValor))
                {
                    return myValor;
                }
                else
                {
                    throw new Exception("Error al Convertir la Cadena : " + this.Text + " a Integer");
                }
            }

            set
            {

                string miTexto = Convert.ToString(value);

                base.Text = miTexto;


            }


        }



        // Filtro los Caracteres Correctos
        protected virtual bool CaracterCorrecto(char c)
        {

          

                if (!isNegativo)
                {

                    if (c == '-')
                    {
                        return false;
                    }

                }


                if (isEntero)
                {
                    if (c == '.' || c == ',')
                    {
                        return false;
                    }
                }


                if (Array.IndexOf(digitos, c) == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            
        

        }



        protected override void OnKeyPress(KeyPressEventArgs e)
       {

           if (error.GetError(this) != string.Empty)
           {
               error.Clear();
           }
            

            if (!isTexto)
            {
                if (!isEntero)
                {
                    //Verificamos el SimboloDecimal
                    if ((isSimboloDecimalPunto) & (e.KeyChar == ','))
                    {
                        e.KeyChar = '.';
                    }

                    //Verificamos el SimboloDecimal
                    if ((!isSimboloDecimalPunto) & (e.KeyChar == '.'))
                    {
                        e.KeyChar = ',';
                    }
                }



                // Comprobar si aceptamos el carácter pulsado,
                // si el carácter pulsado no está en el array, no aceptarlo
                if (CaracterCorrecto(e.KeyChar))
                {
                    base.OnKeyPress(e);
                }
                else
                {
                    e.Handled = true;
                    SystemSounds.Beep.Play();
                }

            }

            else
            {

                if (this.Text.Length >= MaxLength)
                {

                    if (error.GetError(this) == "")
                    {
                        SystemSounds.Beep.Play();
                        error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
                        error.SetError(this, string.Format(strError.eValorStrMaxOutRango, MaxLength));
                    }

                }
                else
                {
                    error.SetError(this, null);
                  
                }
            
            
            
            
            }
        }


        #region"EVENTOS"
        protected override void OnEnter(EventArgs e)
        {
            error.Clear();
            base.OnEnter(e);
            base.BackColor = Color.LightGreen;
        }
        protected override void OnLeave(EventArgs e)
        {

            base.OnLeave(e);

            if (this.Text.Trim().Length == 0)
            {
                error.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
                error.SetError(this, strError.eValorObligatorio);
                this.CausesValidation = false;
            }
            else
            {
                this.CausesValidation = true;
            
            }


            if (this.Enabled)
            {
                base.BackColor = Color.White;
            }
            else
            {
                base.BackColor = Color.Empty;
            }
        }
        #endregion


        public override string Text
        {
            get
            {

                if (isSimboloDecimalPunto)
                {                   
                    return base.Text.Replace(",", ".");
                }
                else
                {
                    return base.Text.Replace(".", ",");
                }
            }
            set
            {

                if (isTexto)
                {
                    error.Clear();
                    
                    base.Text = value;
                }
                else
                {

                    // aceptar sólo dígitos
                    if (value != null)
                    {


                        string miId = value;

                        string s = "";
                        foreach (char c in value)
                        {
                            // si es un carácter correcto,
                            // lo agregamos a la nueva cadena
                            if (CaracterCorrecto(c))

                                s += c;
                        }

                        string mik = s;

                        if (isSimboloDecimalPunto)
                        {
                            base.Text = s.Replace(",", ".");
                        }
                        else
                        {

                            base.Text = s.Replace(".", ",");
                        }

                    }

                }


            }
        }
    
    }
}
