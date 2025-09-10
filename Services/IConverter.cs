using System;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public interface IConverter
    {
        Task<ConversionResult> ConvertAsync(FileJob job, IProgress<int> progress);
    }

    public class ConversionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }
}
