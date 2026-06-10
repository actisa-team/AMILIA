using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Drawing;


    //TADIL
    using tadLayLogica;
    using tadLayLan;
    using userControl;





    public static class oValidar
    {

        /// <summary>
        /// Valido todos los Controles que tienen el Interfave IisValido
        /// Solo valido los Grupos que estan ENABLE !!!!!!!
        /// </summary>
        public static bool isValidoGrupoByFrm(ContainerControl iForm)
        { 
        
            //Obtengo todos los controles
            List<Control> myLstControl = GetAllControls(iForm.Controls);

            List<bool> miValidos = new List<bool>();

            //Me quedo con los controles dentro

            foreach (Control item in myLstControl)
            {

                if (item is GroupBox && item.Enabled)
                {  
                  miValidos.Add(isValidoGrupo(item as GroupBox));            
                }


            }


            if ((from p in miValidos where p == false select p).Count() > 0)
            {
           
                return false;
            }
            else
            {
                return true;

            }
        
        
        
        
        }



        /// <summary>
        /// Validos los controles que tienen el Interface IisValido dentro de un Grupo
        /// </summary>
        public static bool isValidoGrupo(GroupBox iGr)
        {

            List<bool> miValidos = new List<bool>();

            foreach (Control item in iGr.Controls)
            {        
                if ( item.Enabled && item is IisValido)
                {
                    IisValido miControl = (IisValido)item;
                    miValidos.Add(miControl.isValido());
                }
            }


            if ((from p in miValidos where p == false select p).Count() > 0)
            {            
                return false;
            }
            else
            {
               return true;            
            }
         
        }


        //Get Control
        private static List<Control> GetAllControls(IList ctrls)
        {
            List<Control> RetCtrls = new List<Control>();
            foreach (Control ctl in ctrls)
            {
                RetCtrls.Add(ctl);
                List<Control> SubCtrls = GetAllControls(ctl.Controls);
                RetCtrls.AddRange(SubCtrls);
            }
            return RetCtrls;
        }


    }

}
