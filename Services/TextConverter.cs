using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class TextConverter : IConverter
    {
        public async Task<ConversionResult> ConvertAsync(FileJob job, IProgress<int> progress)
        {
            try
            {
                // تأكد إن الملف موجود
                if (!File.Exists(job.SourcePath))
                {
                    return new ConversionResult
                    {
                        Success = false,
                        Message = "الملف غير موجود"
                    };
                }

                // تحديد مجلد الإخراج
                string outputDir = Path.Combine(job.DestinationPath, "Converted");
                Directory.CreateDirectory(outputDir);

                // اسم الملف الجديد
                string outputFile = Path.Combine(outputDir,
                    Path.GetFileNameWithoutExtension(job.SourcePath) + "_converted.txt");

                // قراءة الملف
                string text = await File.ReadAllTextAsync(job.SourcePath);

                // تحويل النص (مثلاً إلى UPPERCASE)
                string converted = text.ToUpperInvariant();

                // محاكاة التقدم Progress
                for (int i = 0; i <= 100; i += 20)
                {
                    await Task.Delay(100, job.CancellationTokenSource.Token);
                    progress.Report(i);
                }

                // كتابة الملف الجديد
                await File.WriteAllTextAsync(outputFile, converted, Encoding.UTF8);

                return new ConversionResult
                {
                    Success = true,
                    Message = $"تم التحويل: {outputFile}"
                };
            }
            catch (OperationCanceledException)
            {
                return new ConversionResult
                {
                    Success = false,
                    Message = "تم إلغاء العملية"
                };
            }
            catch (Exception ex)
            {
                return new ConversionResult
                {
                    Success = false,
                    Message = $"خطأ: {ex.Message}"
                };
            }
        }
    }
}
