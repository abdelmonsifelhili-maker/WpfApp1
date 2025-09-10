using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class PDFConverter : IConverter
    {
        public async Task<ConversionResult> ConvertAsync(FileJob job, IProgress<int> progress)
        {
            try
            {
                string text = await File.ReadAllTextAsync(job.SourcePath);
                string destPath = Path.ChangeExtension(job.SourcePath, ".pdf");

                using var doc = new PdfDocument();
                var page = doc.AddPage();
                using var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Verdana", 12);
                gfx.DrawString(text, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height));

                doc.Save(destPath);

                progress.Report(100);
                return new ConversionResult { Success = true, Message = "PDF created" };
            }
            catch (Exception ex)
            {
                return new ConversionResult { Success = false, Message = ex.Message };
            }
        }
    }
}
