using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.eventos
{

    /// <summary>
    /// CLASE PARa MANEJAR LOS EVENTOS
    /// EJEMPLO
    /// -Crear Evento
    /// public event EventHandler<oEventArgs<bool>> evHideFrm;
    /// -Lanzar el Evento
    /// this.evHideFrm(this, new oEventArgs<bool>(true));
    /// --COMPROBAR QUE TENEMOS SUBCRIPTORES
    /// if (evHideForm !=null)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class oEventArgs<T> : EventArgs
    {

        private T m_value;
        
        public oEventArgs(T value)
        {
            m_value = value;
        }



        public T Value
        {
            get { return m_value; }
        }
    }


    public class oEventArgs<T,J> : oEventArgs<T>
    {

      
        private J m_Jvalue;

        public oEventArgs (T iT, J iJ)
            :base(iT)
        
        {
           
            m_Jvalue = iJ;
        }


        public J Value2
        {
            get { return m_Jvalue; }
        }

        public override string ToString()
        {
            return this.Value.ToString() + " - " + this.Value2.ToString();
        }
    }
}
