using System.IO;
using System.Xml.Serialization;

namespace LANDXML
{
    public class Loader
    {
        public LandXML Load(string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LandXML));
            
            using (FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (NamespaceIgnorantXmlTextReader objXmlReader = new NamespaceIgnorantXmlTextReader(fileStream))
                {
                    return (LandXML)xmlSerializer.Deserialize(objXmlReader);
                }
            }
        }
    }
}
