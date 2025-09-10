using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.IO;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class ImageConverter : IConverter
    {
        public async Task<ConversionResult> ConvertAsync(FileJob job, IProgress<int> progress)
        {
            try
            {
                using var image = await Image.LoadAsync(job.SourcePath);

                string destPath = Path.ChangeExtension(job.SourcePath, ".jpg");
                await image.SaveAsJpegAsync(destPath, new JpegEncoder());

                progress.Report(100);
                return new ConversionResult { Success = true, Message = "Image converted" };
            }
            catch (Exception ex)
            {
                return new ConversionResult { Success = false, Message = ex.Message };
            }
        }
    }
}
