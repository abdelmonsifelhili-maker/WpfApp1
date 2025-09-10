namespace WpfApp1.Models
{
    public class ConversionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        // أضف هذه الخاصية علشان تنحل المشكلة
        public FileJob? Job { get; set; }

    }
}
