using Microsoft.AspNetCore.Http;
using System.IO;

namespace Presentation.Admin.Interfaces
{
    public interface IImageHelper
    {
        Stream ResizeImage(IFormFile file, int width, int height, bool keepAspectRatio = true);
    }
}
