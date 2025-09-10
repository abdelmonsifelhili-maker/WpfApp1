using NAudio.Wave;
using System;
using System.IO;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class AudioConverter : IConverter
    {
        public async Task<ConversionResult> ConvertAsync(FileJob job, IProgress<int> progress)
        {
            try
            {
                string destPath = Path.ChangeExtension(job.SourcePath, ".wav");

                await Task.Run(() =>
                {
                    using var reader = new AudioFileReader(job.SourcePath);
                    WaveFileWriter.CreateWaveFile16(destPath, reader);
                });

                progress.Report(100);
                return new ConversionResult { Success = true, Message = "Audio converted" };
            }
            catch (Exception ex)
            {
                return new ConversionResult { Success = false, Message = ex.Message };
            }
        }
    }
}
