namespace LarcanumCds.Server.Configuration;

public class SourceSettings
{
	public const string SectionKey = "Source";

	private const string DataDirName = "data";

	public string ContentPath { get; set; } = String.Empty;

	public string ImageProcessorPrefix { get; set; } = String.Empty;

	public string ResolveDataPath(IWebHostEnvironment env)
	{
		return Path.GetFullPath(Path.IsPathFullyQualified(ContentPath)
			? Path.Combine(ContentPath, DataDirName)
			: Path.Combine(env.ContentRootPath, ContentPath, DataDirName));
	}
}
