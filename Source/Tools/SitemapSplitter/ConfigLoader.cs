using System.Xml.Serialization;

static class ConfigLoader
{
    public static Configuration Load(string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open);
        return (Configuration)(new XmlSerializer(typeof(Configuration)).Deserialize(stream) ?? throw new Exception());
    }
}