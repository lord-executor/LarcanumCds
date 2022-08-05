namespace LarcanumCds.Server.Model;

public class Blueprint
{
    public string Key { get; set; } = String.Empty;

    // ReSharper disable once CollectionNeverUpdated.Global
    public IList<Property> Properties { get; set; } = new List<Property>();
}
