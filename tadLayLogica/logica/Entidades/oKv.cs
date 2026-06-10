using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare;

namespace tadLayLogica
{
   
    
      
    public class oKv :IDisposable
    {

        private double? mKvConvexo = null;
        private double? mKvConcavo = null;

 
        public oKv(double iKvConvexo, double iKvConcavo)
        {
            mKvConvexo = iKvConvexo;
            mKvConcavo = iKvConcavo;           
        }


        public double kvConvexo
        {
            get
            {
                if (mKvConvexo.HasValue)
                {
                    return mKvConvexo.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Convexo");
                }

            }
            set
            {
                mKvConvexo = value;
            }
        }
        public double KvConcavo
        {
            get
            {
                if (mKvConcavo.HasValue)
                {
                    return mKvConcavo.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Concavo");

                }

            }
            set
            {
                mKvConcavo = value;
            }
        }


        public void Dispose()
        {
            mKvConvexo = null;
            mKvConcavo = null;
        }
    }
}
