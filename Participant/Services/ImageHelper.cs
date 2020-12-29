using ImageMagick;
using Microsoft.AspNetCore.Http;
using Presentation.Participant.Interfaces;
using System;
using System.IO;

namespace Presentation.Participant.Services
{
    public class ImageHelper : IImageHelper
    {
        public Stream ResizeImage(IFormFile file, int width, int height, bool keepAspectRatio = true)
        {
            try
            {
                var input = file.OpenReadStream();

                var original = new MagickImage(input);

                if (keepAspectRatio)
                {
                    if (original.Width > original.Height) height = original.Height * width / original.Width;
                    else width = original.Width * height / original.Height;
                }

                original.Resize(width, height);

                var stream = new MemoryStream();
                original.Write(stream);

                stream.Position = 0;

                var optimizer = new ImageOptimizer();
                optimizer.LosslessCompress(stream);

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
