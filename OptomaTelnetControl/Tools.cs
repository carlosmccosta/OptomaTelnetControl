using System.IO;
using System.Xml.Serialization;

namespace OptomaTelnetControl
{
    public class Tools
    {
        public static void SaveToXml<T>(string filename, T obj, XmlAttributeOverrides xmlAttributeOverrides = null)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                XmlSerializer serializer;
                if (xmlAttributeOverrides == null)
                    serializer = new XmlSerializer(typeof(T));
                else
                    serializer = new XmlSerializer(typeof(T), xmlAttributeOverrides);

                serializer.Serialize(fs, obj);
            }
        }


        public static T LoadFromXml<T>(string filename, XmlAttributeOverrides xmlAttributeOverrides = null)
        {
            XmlSerializer serializer;
            if (xmlAttributeOverrides == null)
                serializer = new XmlSerializer(typeof(T));
            else
                serializer = new XmlSerializer(typeof(T), xmlAttributeOverrides);

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return (T)serializer.Deserialize(fs);
            }
        }
    }
}
