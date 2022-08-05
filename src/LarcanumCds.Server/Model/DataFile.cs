using YamlDotNet.Serialization;

namespace LarcanumCds.Server.Model;

public class DataFile
{
    public FileInfo File { get; }
    public IDictionary<string, object> Data { get; }

    public DataFile(IDeserializer deserializer, string dataFilePath)
    {
        File = new FileInfo(dataFilePath);
        if (!File.Exists)
        {
            throw new FileNotFoundException("Unable to locate data file", dataFilePath);
        }

        using var textReader = File.OpenText();
        Data = deserializer.Deserialize<IDictionary<string, object>>(textReader)!;
    }

    private DataFile(DataFile parent, object data)
    {
        if (data is not IDictionary<object, object> objects)
        {
            throw new ArgumentException("Data has to be a dictionary type", nameof(data));
        }

        File = parent.File;
        Data = objects.ToDictionary(kvp => kvp.Key.ToString()!, kvp => kvp.Value);
    }

    public IEnumerable<DataFile> Derive(string propName)
    {
        if (!Data.ContainsKey(propName))
        {
            return Enumerable.Empty<DataFile>();
        }

        return (Data[propName] as IEnumerable<object> ?? throw new InvalidOperationException()).Select(item => new DataFile(this, item));
    }
}
