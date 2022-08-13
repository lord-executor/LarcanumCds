using LarcanumCds.Server.Configuration;
using Microsoft.Extensions.Options;

namespace LarcanumCds.Server;

public class SpaRoutingMiddleware
{
	private readonly RequestDelegate _next;

	public SpaRoutingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext httpContext, IOptions<SpaRoutingSettings> settings, IWebHostEnvironment env)
	{
		if (settings.Value.IsRequestMatching(httpContext))
		{
			var appIndex = settings.Value.GetAppFile(env);
			if (appIndex.Exists)
			{
				await httpContext.Response.SendFileAsync(appIndex);
				return;
			}
		}

		await _next(httpContext);
	}
}
