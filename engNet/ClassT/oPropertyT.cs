using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.ClassT
{

    using System.ComponentModel;

    public class oValT<T>:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        private T mT;

        public oValT()
        {
      
        }

        public oValT(T iT)
        {
            mT = iT;
        }


        public T val
        {
            get
            {       
                return mT; 
            }

            set
            { 
                mT = value;

                NotifyPropertyChanged("val");
            }
        }


        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        } 



       
    }
}
