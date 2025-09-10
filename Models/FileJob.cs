using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace WpfApp1.Models
{
    public class FileJob : INotifyPropertyChanged
    {
        private string _status = "Pending";
        private int _progress;

        public string SourcePath { get; set; } = string.Empty;          // الملف الأصلي
        public string DestinationPath { get; set; } = string.Empty;     // مكان الحفظ
        public FileType FileType { get; set; }                          // نوع الملف

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public int Progress
        {
            get => _progress;
            set { _progress = value; OnPropertyChanged(); }
        }

        // لإلغاء العمليات الطويلة (مثلاً تحويل الفيديو أو الصوت)
        public CancellationTokenSource CancellationTokenSource { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // أنواع الملفات المدعومة
    public enum FileType
    {
        Text,
        Image,
        Audio,
        PDF,
        Video
    }
}
