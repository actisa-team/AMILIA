using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Runtime;

    using engNet.Extension.String;
    using tadLayLan;
   

   public class oXdata 
    {
       public static List<string> getXData(Entity iEntidad, string iKey, string iStrSep)
       {

           Entity myEntidad = iEntidad;

           string myData = string.Empty;
           if (myEntidad != null)
           {
               using (Transaction tr = oCadManager.StartTransaction())
               {
                   DBObject obj = tr.GetObject(myEntidad.ObjectId, OpenMode.ForRead);


                   //comprobar si tiene Xdata

                   if (obj.XData == null)
                   {
                       return new List<string>();
                   }


                   TypedValue[] myValues = obj.XData.AsArray();

                   for (int i = 0; i < myValues.Count(); i++)
                   {
                       if (myValues[i].TypeCode == 1001 && myValues[i].Value.ToString() == iKey)
                       {
                           myData = myValues[i + 1].Value.ToString();
                       }

                   }


                   if (myData != string.Empty)
                   {
                       if (string.IsNullOrEmpty(iStrSep))
                       {
                           return new List<string>(new string[] { myData });

                       }
                       else
                       {
                           return oExtensionString.lineaSplit(myData, iStrSep);
                       }

                   }
                   else
                   {
                       throw new System.Exception(string.Format(strError.eNoSePuedeobtenerXdata, iKey));

                   }


               }
           }
           return new List<string>(new string[] { myData });
       }




       public static void setXdata(ObjectId iObjId, string iKey, string iData)
       {

               using (Transaction tr = oCadManager.StartTransaction())
               {
                   DBObject obj = tr.GetObject(iObjId, OpenMode.ForWrite);

                   AddRegAppTableRecord(iKey);


                   ResultBuffer rb =
                       new ResultBuffer(
                       new TypedValue(1001, iKey),
                       new TypedValue(1000, iData)
                     );

                   obj.XData = rb;
                   rb.Dispose();
                   tr.Commit();
               }
         
       }



       private static void AddRegAppTableRecord(string regAppName)
        {
             
            using (Transaction tr = oCadManager.StartTransaction())
            {
                RegAppTable rat = (RegAppTable)tr.GetObject(oCadManager.thisBase.RegAppTableId, OpenMode.ForRead, false);
                 
                if (!rat.Has(regAppName))
                {
                    rat.UpgradeOpen();
                    RegAppTableRecord ratr = new RegAppTableRecord();
                      
                    ratr.Name = regAppName;
                    rat.Add(ratr);
                    tr.AddNewlyCreatedDBObject(ratr, true);
                }
                tr.Commit();
            }
        }
    }
}
