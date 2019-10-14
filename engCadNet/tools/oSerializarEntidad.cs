using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace engCadNet
{
    public class oSerializarEntidad
    {
        private const int MAX_CHUNK_LENGTH = 127;

        public static byte[][] ChunkStream(MemoryStream stream)
        {
            // determine the number of chunks needed to hold stream 
            int chunkCount = (int)Math.Ceiling((decimal)stream.Length /
            (decimal)MAX_CHUNK_LENGTH);
            byte[][] result = new byte[chunkCount][];
            // start read at beginning of stream 
            stream.Position = 0;
            int chunkIndex = 0;
            int numBytesToRead = (int)stream.Length;
            int numBytesRead = 0;
            // read through the stream, assigning each chunk of bytes into the 
            // next sequential byte array 
            while (numBytesToRead > 0)
            {
                int chunk = MAX_CHUNK_LENGTH;
                if (chunk > numBytesToRead)
                {
                    chunk = numBytesToRead;
                }
                byte[] buffer = new byte[chunk];
                int n = stream.Read(buffer, 0, chunk);
                if (n != 0) // make sure we haven't passed the end of the stream 
                {
                    result[chunkIndex] = buffer;
                    chunkIndex += 1;
                }
                else
                {
                    break; // we've reached the end 
                }
                numBytesToRead -= n;
                numBytesRead += n;
            }
            return result;
        }



        public static ObjectId StoreObjectInExtensionDictionary(string xRecordKey, DBObject dbObj, Transaction trans, MemoryStream stream, object objectTypeFull) // meter el stream directamente
        {
            ObjectId result = ObjectId.Null;

                byte[][] objectChunks = ChunkStream(stream);
                if (objectChunks.Length > 0)
                {
                    List<TypedValue> typedValues = new List<TypedValue>();
                    // identify the type name in a readable section 
                    typedValues.Add(new TypedValue((int)DxfCode.Text, objectTypeFull));
                    // add sequence of binary chunks to the result buffer 
                    for (int i = 0; i < objectChunks.Length; i++)
                    {
                        typedValues.Add(new TypedValue((int)(DxfCode.BinaryChunk),
                       objectChunks[i]));
                    }
                    ResultBuffer data = new ResultBuffer(typedValues.ToArray());
                    result = SaveXRecord(dbObj, data, xRecordKey, trans);
                }
                stream.Close();
            return result;
        }

        private static ObjectId SaveXRecord(DBObject o, ResultBuffer buffer, string key, Transaction trans) //DBObject o--- objeto CAD sobre el que se va a guardar la informacion
        {
            if (o.ExtensionDictionary == ObjectId.Null)
            {
                o.CreateExtensionDictionary();
            }
            using (DBDictionary dict = trans.GetObject(o.ExtensionDictionary,
            OpenMode.ForWrite, false) as DBDictionary)
            {
                // check to see if dictionary contains XRecord 
                // if so, update the data - important for Undo Operations 
                if (dict.Contains(key))
                {
                    Xrecord xRecord = (Xrecord)trans.GetObject(dict.GetAt(key), OpenMode.ForWrite);
                    xRecord.Data = buffer;
                    return xRecord.ObjectId;
                }
                else
                {
                    Xrecord xRecord = new Xrecord();
                    xRecord.Data = buffer;
                    dict.SetAt(key, xRecord);
                    trans.AddNewlyCreatedDBObject(xRecord, true);
                    return xRecord.ObjectId;
                }
            }
        }
    }
}
