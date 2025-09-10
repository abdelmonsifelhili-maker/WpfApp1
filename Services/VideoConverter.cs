using System;
using System.Threading.Tasks;
using WpfApp1.Models;
using Xabe.FFmpeg;

namespace WpfApp1.Services
{
    public class VideoConverter : IConverter
    {
        public async Task<ConversionResult> ConvertAsync(FileJob job, IProgress<int> progress)
        {
            try
            {
                string destPath = System.IO.Path.ChangeExtension(job.SourcePath, ".mp4");

                var conversion = await FFmpeg.Conversions.FromSnippet.ToMp4(job.SourcePath, destPath);
                conversion.OnProgress += (s, e) =>
                {
                    progress.Report((int)e.Percent);
                };

                await conversion.Start();

                return new ConversionResult { Success = true, Message = "Video converted" };
            }
            catch (Exception ex)
            {
                return new ConversionResult { Success = false, Message = ex.Message };
            }
        }
    }
}
