using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ.Vertice
{

    using tadLayShare.puntos;
    
    public class oVerNodo : oVer
    {

        private bool mIsOpen ; 
        private long mHandle  ;
        
        
        public oVerNodo(long iHandle, oP3d iPto, bool iIsOpen)
              :base(0,iPto)
        
        {
            mHandle = iHandle;
            mIsOpen = iIsOpen;
        
        }

        public long handle
        {
            get
            {
                return mHandle;
            }
        }
        public bool isOpen
        {
            get
            {
                return mIsOpen;
            }

            set
            {
                mIsOpen = value;
            }
        
        
        }



    }
}
