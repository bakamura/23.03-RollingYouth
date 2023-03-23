using System.IO;
using System.Xml.Serialization;

public static class DataSerializer
{
    public static string Serialize<T>(T data)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, data);
        return writer.ToString();
    }

    public static T Deserialize<T>(this string data)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(data);
        return (T)xml.Deserialize(reader);
    }
}
