using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class ConverterManager
    {
        public List<FileJob> JobList { get; set; } = new();

        // حدث عام لتحديث Progress (يمكن ربطه بالـ UI)
        public event Action<FileJob, int>? ProgressChanged;

        // تحويل ملف واحد
        public async Task<ConversionResult> StartConversion(FileJob job)
        {
            IConverter? converter = job.FileType switch
            {
                FileType.Text => new TextConverter(),
                FileType.Image => new ImageConverter(),
                FileType.Audio => new AudioConverter(),
                FileType.PDF => new PDFConverter(),
                FileType.Video => new VideoConverter(),
                _ => null
            };

            if (converter == null)
                return new ConversionResult
                {
                    
                    Success = false,
                    Message = "نوع الملف غير مدعوم"
                };

            var progress = new Progress<int>(value => ProgressChanged?.Invoke(job, value));
            return await converter.ConvertAsync(job, progress);
        }

        // تحويل كل الملفات بالتوازي
        public async Task<List<ConversionResult>> StartAllConversionsParallel()
        {
            var tasks = JobList.Select(job => StartConversion(job)).ToList();
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }
    }
}
