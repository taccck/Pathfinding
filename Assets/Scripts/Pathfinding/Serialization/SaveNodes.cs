using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public static class SaveNodes
{
    private static readonly string Path = Application.persistentDataPath + "\\nodes.txt";
    private static readonly XmlSerializer Ser = new XmlSerializer(typeof(NodeData));

    public static void Save(NodeData nodeData)
    {
        if (!File.Exists(Path))
        {
            FileStream fs = File.Create(Path);
            fs.Close();
        }

        TextWriter writer = new StreamWriter(Path);
        Ser.Serialize(writer, nodeData);
        writer.Close();
    }

    public static NodeData Load()
    {
        FileStream fs = new FileStream(Path, FileMode.Open);
        return (NodeData) Ser.Deserialize(fs);
    }

    public static bool FileExists => File.Exists(Path);
}