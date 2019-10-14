using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.ClassT
{

    using System.ComponentModel;
    
    /// <summary>
    /// Clase 2 Parametros
    /// </summary>
    /// <typeparam name="T">VAL</typeparam>
    /// <typeparam name="J">DES</typeparam>
    public class oValDesT<T,J>
    {

     private T mT;
     private J mJ;


     public oValDesT()
     {

     }

     /// <summary>
     /// Constructor
     /// </summary>
     /// <param name="iT">Valor</param>
     /// <param name="iJ">Descripcion</param>
     public oValDesT (T iT, J iJ)     
     { 
            mT = iT;
            mJ = iJ;
     }

     [Description("valor")]
     public T val
     {
         get { return mT; }

         set { mT = value; }
     }

     [Description("descripcion")]
     public J des
     {
         get { return mJ; }

         set { mJ = value; }
     }

       public override string ToString()
        {
            return "valor: " + val.ToString()  + " - " + "des:" + des.ToString();
        }


    }


    public class oValDesFilT<T, J, K> : oValDesT<T, J>
    {

        private K mK ;

        public oValDesFilT() { }

        public  oValDesFilT (T iT, J iJ, K iK)     
        :base(iT,iJ)
        
        {
            mK = iK;
        }

        [Description("filtro")]
        public K fil
        {
            get { return mK; }

            set { mK = value; }
        }




    
    }

   
}
