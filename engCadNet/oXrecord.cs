using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;
    using System.IO;
    using tadLayLan;



    public class oXrecord
    {

        private static int mProgreso = 0;

        /// <summary>
        /// Obtener Xrecord Objeto, Dando la Key del Diccionario
        /// </summary>
        public static Xrecord getXrecord(ObjectId iObjectId, string iKey)
        {
            try
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {
                    Xrecord miXRecord = new Xrecord();

                    Entity miEntidad = (Entity)tr.GetObject(iObjectId, OpenMode.ForRead, false);

                    if (miEntidad.ExtensionDictionary.IsNull)
                    {
                        throw new Exception(string.Format(strError.eDiccionarioNoContieneClave, iKey));
                    }

                    DBDictionary miDbDictionary = (DBDictionary)tr.GetObject(miEntidad.ExtensionDictionary, OpenMode.ForRead, false);

                    if (miDbDictionary.Contains(iKey))
                    {
                        miXRecord = (Xrecord)tr.GetObject(miDbDictionary.GetAt(iKey), OpenMode.ForRead, false);

                        return miXRecord;
                    }
                    else
                    {
                        throw new Exception(string.Format(strError.eDiccionarioNoContieneClave, iKey));
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(strError.eDiccionarioNoContieneClave, iKey));
            }
        }

        public static MemoryStream getStream(Xrecord xrec)
        {
            mProgreso = 0;
            MemoryStream stream = new MemoryStream();
            if (xrec != null)
            {
                int count = 0;
                using (ResultBuffer rb = xrec.Data)
                {
                    if (rb != null)
                    {
                        TypedValue[] tvs = rb.AsArray();
                        if (tvs != null)
                        {
                            TypedValue typeRecord = tvs[0];
                            if (typeRecord.TypeCode == (short)DxfCode.Text)
                            {
                                for (int i = 1; i < tvs.Length; i++)
                                {
                                    if (tvs[i].TypeCode == (short)DxfCode.BinaryChunk)
                                    {
                                        byte[] buffer = (byte[])tvs[i].Value;
                                        count = buffer.Length;
                                        stream.Write(buffer, 0, count);
                                    }
                                    mProgreso = i * 100 / tvs.Length;
                                }
                                // reset stream position 
                                stream.Position = 0;
                            }

                        }

                    }
                }
            }
            mProgreso = 100;
            return stream;
        }

        public static int Progreso
        {
            get
            {
                return mProgreso;
            }
        }

    }
}
