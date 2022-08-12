using LarcanumCds.Server.Configuration;
using LarcanumCds.Server.Model;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LarcanumCds.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ContentController
{
	private readonly ILogger<ContentController> _logger;
	private readonly SourceSettings _sourceSettings;
	private readonly IDeserializer _yamlDeserializer;

	public ContentController(ILogger<ContentController> logger, IOptions<SourceSettings> sourceSettings)
	{
		_logger = logger;
		_sourceSettings = sourceSettings.Value;
		_yamlDeserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
	}

	[HttpGet("{*slug}")]
	public async Task<IActionResult> Data(string slug)
	{
		var contentFilePath = Path.Combine(_sourceSettings.ContentPath, "data", slug, $"{Path.GetFileName(slug)}.yaml");

		_logger.LogInformation($"Resolving content request to {contentFilePath}");

		try
		{
			var dataFile = new DataFile(_yamlDeserializer, contentFilePath);
			await ProcessData(dataFile);
			return new JsonResult(dataFile.Data);
		}
		catch (FileNotFoundException)
		{
			return new NotFoundResult();
		}
	}

	private async Task ProcessData(DataFile dataFile)
	{
		if (dataFile.Data.ContainsKey("Type"))
		{
			var blueprintFilePath =
				Path.Combine(_sourceSettings.ContentPath, "blueprints", $"{dataFile.Data["Type"]}.yaml");
			var blueprintFile = new FileInfo(blueprintFilePath);

			if (blueprintFile.Exists)
			{
				using var blueprintReader = blueprintFile.OpenText();
				var blueprint = _yamlDeserializer.Deserialize<Blueprint>(blueprintReader);

				foreach (var prop in blueprint.Properties)
				{
					if (prop.Type == "blob")
					{
						var blobFilePath = Path.Combine(dataFile.File.DirectoryName!, (string)dataFile.Data[prop.Name]);
						var fileContent = await File.ReadAllTextAsync(blobFilePath);

						if (Path.GetExtension(blobFilePath) == ".md")
						{
							var pipeline = new MarkdownPipelineBuilder().UseDiagrams().Build();
							fileContent = Markdown.ToHtml(fileContent, pipeline);
						}

						dataFile.Data[prop.Name] = fileContent;
					}
					else if (prop.Type == "markdown")
					{
						var pipeline = new MarkdownPipelineBuilder().Build();
						dataFile.Data[prop.Name] = Markdown.ToHtml((string)dataFile.Data[prop.Name], pipeline);
					}
					else if (prop.Type == "inline")
					{
						dataFile.Data[prop.Name] = await Task.WhenAll(dataFile.Derive(prop.Name).Select(
							async childData =>
							{
								await ProcessData(childData);
								return childData.Data;
							}));
					}
				}
			}
		}
	}
}
