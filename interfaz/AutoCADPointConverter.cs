using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Autodesk.AutoCAD.Geometry;

namespace interfaz
{
    public class AutoCADPointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Point2d) || objectType == typeof(Point3d);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            // System.Windows.Forms.MessageBox.Show($"Reading JSON: {jo.ToString()}"); // DEBUG
            if (objectType == typeof(Point2d))
            {
                double x = 0, y = 0;
                if (jo["X"] != null) x = (double)jo["X"];
                else if (jo["x"] != null) x = (double)jo["x"];
                
                if (jo["Y"] != null) y = (double)jo["Y"];
                else if (jo["y"] != null) y = (double)jo["y"];

                return new Point2d(x, y);
            }
            else if (objectType == typeof(Point3d))
            {
                double x = 0, y = 0, z = 0;
                if (jo["X"] != null) x = (double)jo["X"];
                else if (jo["x"] != null) x = (double)jo["x"];
                
                if (jo["Y"] != null) y = (double)jo["Y"];
                else if (jo["y"] != null) y = (double)jo["y"];
                
                if (jo["Z"] != null) z = (double)jo["Z"];
                else if (jo["z"] != null) z = (double)jo["z"];

                return new Point3d(x, y, z);
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jo = new JObject();
            if (value is Point2d p2)
            {
                jo.Add("X", p2.X);
                jo.Add("Y", p2.Y);
            }
            else if (value is Point3d p3)
            {
                jo.Add("X", p3.X);
                jo.Add("Y", p3.Y);
                jo.Add("Z", p3.Z);
            }
            jo.WriteTo(writer);
        }
    }
}
