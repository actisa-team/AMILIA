using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.informes
{

    using engNet.CustomAtributos;

    using System.IO;
    using System.Text;
    using System.Reflection;
    using System.ComponentModel;
    using tadLayLan;
    
    
   public static class oExcelInforme
    {


        public static  void WriteCsv<H,I,F>(IEnumerable<H> iDataHeader, 
                                           IEnumerable<I>  iDataItems,
                                           IEnumerable<F>  iDataFooter,
                                           string iPath, 
                                           string iFileNameSinExtension)     
        {

            string miFile = iPath + @"\" + iFileNameSinExtension + ".csv";


            WriteCsv<H, I, F>(iDataHeader, iDataItems, iDataFooter, miFile);
  
        }


        public static void WriteCsv<H, I, F>(IEnumerable<H> iDataHeader,
                                             IEnumerable<I> iDataItems,
                                             IEnumerable<F> iDataFooter,
                                             string iFileConExtension)            
        {

            string miFile = iFileConExtension;

            System.IO.TextWriter output = new StreamWriter(miFile, false, Encoding.BigEndianUnicode);

            //CABEZERA
            PropertyDescriptorCollection propsHeader = TypeDescriptor.GetProperties(typeof(H));

            //LISTADO
            PropertyDescriptorCollection propsItems = PropertyDescriptorCollectionHelper.Current.GetPropertyCollection(typeof(I));

            //RESUMEN
            PropertyDescriptorCollection propsFooter = TypeDescriptor.GetProperties(typeof(F));


            string miVal;


            foreach (H item in iDataHeader)
            {
                foreach (PropertyDescriptor prop in propsHeader)
                {
                    if (prop.IsBrowsable)
                    {
                        if (prop.GetValue(item) == null)
                        {
                            miVal = string.Empty;
                        }
                        else
                        {
                            miVal = prop.GetValue(item).ToString();
                        }

                        output.Write(miVal.Replace('.',','));
                        output.Write(";");
                    }
                }

                output.WriteLine();
            }


            output.WriteLine();
            output.WriteLine();
            output.WriteLine();

            //////LISTADO/////////////////////
            //--PROPIEDADES--
            foreach (PropertyDescriptor prop in propsItems)
            {

                if (prop.IsBrowsable)
                {
                    output.Write(prop.DisplayName.Replace('.',',')); // header
                    output.Write(";");
                }

            }

            output.WriteLine();


            //--ITEMS
            foreach (I item in iDataItems)
            {
                foreach (PropertyDescriptor prop in propsItems)
                {
                    if (prop.IsBrowsable)
                    {

                        if (prop.GetValue(item) == null)
                        {
                            miVal = string.Empty;
                        }
                        else
                        {
                            miVal = prop.GetValue(item).ToString();
                        }

                        output.Write(miVal.Replace('.', ','));
                        output.Write(";");
                    }
                }

                output.WriteLine();
            }



            //----------------------------------------------
            //----FOOTER------------------------------------
            //----------------------------------------------
            output.WriteLine();
            output.WriteLine();
            output.WriteLine();

            foreach (F item in iDataFooter)
            {
                foreach (PropertyDescriptor prop in propsFooter)
                {
                    if (prop.IsBrowsable)
                    {
                        if (prop.GetValue(item) == null)
                        {
                            miVal = string.Empty;
                        }
                        else
                        {
                            miVal = prop.GetValue(item).ToString();
                        }


                        output.Write(miVal.Replace('.', ','));
                        output.Write(";");
                    }
                }

                output.WriteLine();
            }

            output.WriteLine();

            output.Flush();
            output.Close();




            oTadil.data.UserInfo.showInfo(strGeneralUser.uiFicheroExito);
        }



    }
}
