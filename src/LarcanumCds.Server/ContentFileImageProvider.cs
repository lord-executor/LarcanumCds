using LarcanumCds.Server.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Internal;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Providers;

namespace LarcanumCds.Server;

public class ContentFileImageProvider : FileProviderImageProvider
{
    public ContentFileImageProvider(IWebHostEnvironment environment, IOptions<SourceSettings> sourceSettings, FormatUtilities formatUtilities)
        : base(GetProvider(environment, sourceSettings.Value), ProcessingBehavior.CommandOnly, formatUtilities)
    {
    }

    private static IFileProvider GetProvider(IWebHostEnvironment environment, SourceSettings sourceSettings)
    {
        return new VirtualFileProvider(sourceSettings.ResolveDataPath(environment), sourceSettings.ImageProcessorPrefix);
    }

    private class VirtualFileProvider : IFileProvider
    {
        private readonly string _physicalRootPath;
        private readonly string _virtualPathPrefix;

        public VirtualFileProvider(string physicalRootPath, string virtualPathPrefix)
        {
            _physicalRootPath = physicalRootPath;
            _virtualPathPrefix = virtualPathPrefix;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var fileInfo = GetFileInfo(subpath);

            if (!fileInfo.Exists || !fileInfo.IsDirectory)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            return new PhysicalDirectoryContents(fileInfo.PhysicalPath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath) || !subpath.StartsWith(_virtualPathPrefix))
            {
                return new NotFoundFileInfo(subpath);
            }

            subpath = subpath.Substring(_virtualPathPrefix.Length);

            var fullPath = Path.GetFullPath(Path.Combine(_physicalRootPath, subpath));

            if (!fullPath.StartsWith(_physicalRootPath))
            {
                throw new InvalidOperationException(
                    $"Path {subpath} is navigating outside of the physical root path of this provider");
            }

            var fileInfo = new FileInfo(fullPath);
            return new PhysicalFileInfo(fileInfo);
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }
}
