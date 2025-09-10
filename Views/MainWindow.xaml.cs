using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<FileJob> Jobs { get; set; } = new ObservableCollection<FileJob>();

        public MainWindow()
        {
            InitializeComponent();
            lvFiles.ItemsSource = Jobs;

            // ممكن تلغي السطر ده لو ما عندك Settings
            //string lastFolder = Properties.Settings.Default.LastFolder;
            //if (!string.IsNullOrEmpty(lastFolder))
            //{
            //    txtStatus.Text = $"Last folder: {lastFolder}";
            //}
        }

        // زر إضافة ملفات
        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "All Files|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                foreach (var file in dlg.FileNames)
                {
                    Jobs.Add(new FileJob
                    {
                        SourcePath = file,
                        DestinationPath = Path.GetDirectoryName(file) ?? string.Empty,
                        FileType = DetermineFileType(file)
                    });
                }
            }
        }

        // بدء التحويل
        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            foreach (var job in Jobs)
            {
                job.Status = "Converting...";
                var converter = GetConverter(job.FileType);
                if (converter != null)
                {
                    var progress = new Progress<int>(p => job.Progress = p);

                    // استخدم await عادي بدل Task.Run
                    var result = await converter.ConvertAsync(job, progress);

                    job.Status = result.Success ? "Completed" : result.Message;
                }
            }
        }


        // إلغاء التحويل لملف واحد
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is FileJob job)
            {
                job.CancellationTokenSource.Cancel();
                job.Status = "Cancelling...";
            }
        }

        // Drag & Drop
        private void lvFiles_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    Jobs.Add(new FileJob
                    {
                        SourcePath = file,
                        DestinationPath = Path.GetDirectoryName(file) ?? string.Empty,
                        FileType = DetermineFileType(file)
                    });
                }
            }
        }

        private void lvFiles_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        // تحديد نوع الملف
        private FileType DetermineFileType(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            return ext switch
            {
                ".txt" => FileType.Text,
                ".csv" => FileType.Text,
                ".png" => FileType.Image,
                ".jpg" => FileType.Image,
                ".jpeg" => FileType.Image,
                ".wav" => FileType.Audio,
                ".mp3" => FileType.Audio,
                ".pdf" => FileType.PDF,
                ".mp4" => FileType.Video,
                ".avi" => FileType.Video,
                _ => FileType.Text
            };
        }

        // الحصول على Converter المناسب
        private IConverter? GetConverter(FileType type)
        {
            return type switch
            {
                FileType.Text => new TextConverter(),
                FileType.Image => new ImageConverter(),
                FileType.Audio => new AudioConverter(),
                FileType.PDF => new PDFConverter(),
                FileType.Video => new VideoConverter(),
                _ => null
            };
        }

        // حفظ آخر مجلد عند إغلاق التطبيق (اختياري)
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // لو حابب تحفظ آخر مجلد
            //Properties.Settings.Default.LastFolder = Jobs.Count > 0
            //    ? Path.GetDirectoryName(Jobs[0].SourcePath)
            //    : string.Empty;
            //Properties.Settings.Default.Save();
        }
    }
}
