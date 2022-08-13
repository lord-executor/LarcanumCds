using Microsoft.Extensions.FileProviders;

namespace LarcanumCds.Server.Configuration;

public class SpaRoutingSettings
{
	public const string SectionKey = "SpaRouting";

	public string Prefix { get; set; } = String.Empty;

	public string AppFile { get; set; } = String.Empty;

	public IFileInfo GetAppFile(IWebHostEnvironment env)
	{
		return env.WebRootFileProvider.GetFileInfo(Path.Combine(Prefix.TrimStart('/'), AppFile));
	}

	public bool IsRequestMatching(HttpContext httpContext)
	{
		return Prefix != String.Empty && httpContext.Request.Method == "GET" &&
		       httpContext.Request.Path.StartsWithSegments(new PathString(Prefix));
	}
}
