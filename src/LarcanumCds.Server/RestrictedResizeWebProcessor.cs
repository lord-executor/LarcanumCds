using System.Globalization;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.Processors;

namespace LarcanumCds.Server;

public class RestrictedResizeWebProcessor : IImageWebProcessor
{
	private readonly IImageWebProcessor _resizeWebProcessor = new ResizeWebProcessor();

	public int MaxSize { get; set; } = 2048;

	public FormattedImage Process(FormattedImage image, ILogger logger, CommandCollection commands, CommandParser parser,
		CultureInfo culture)
	{
		// The command parser will reject negative numbers as it clamps values to ranges.
		int width = (int)parser.ParseValue<uint>(commands.GetValueOrDefault(ResizeWebProcessor.Width), culture);
		int height = (int)parser.ParseValue<uint>(commands.GetValueOrDefault(ResizeWebProcessor.Height), culture);

		if (width > MaxSize || height > MaxSize)
		{
			throw new ArgumentOutOfRangeException(nameof(commands), "Requested image size is too large");
		}

		return _resizeWebProcessor.Process(image, logger, commands, parser, culture);
	}

	public bool RequiresTrueColorPixelFormat(CommandCollection commands, CommandParser parser, CultureInfo culture)
	{
		return _resizeWebProcessor.RequiresTrueColorPixelFormat(commands, parser, culture);
	}

	public IEnumerable<string> Commands => _resizeWebProcessor.Commands;
}
