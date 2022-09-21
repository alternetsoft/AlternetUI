using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DragAndDropSample
{
    public partial class MainWindow : Window
    {
        private static readonly string[] SupportedFormats =
            new[] { DataFormats.Text, DataFormats.Files, DataFormats.Bitmap };

        private Image testBitmap;

        public MainWindow()
        {
            InitializeComponent();

            testBitmap = new Image(new Size(64, 64));
        }

        private void CopyButton_Click(object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(GetDataObject());
        }

        private void DropTarget_DragDrop(object sender, DragEventArgs e)
        {
            LogEvent("DragDrop");
        }

        private void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            LogEvent("DragOver");
        }

        private void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            LogEvent("DragEnter");
        }

        private void DropTarget_DragLeave(object sender, EventArgs e)
        {
            LogEvent("DragLeave");
        }

        private int lastEventNumber = 1;

        void LogEvent(string message)
        {
            eventsListBox.Items.Add($"{lastEventNumber++}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }

        private void DragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
        }

        private IDataObject GetDataObject()
        {
            var result = new DataObject();

            if (textFormatCheckBox!.IsChecked)
                result.SetData(DataFormats.Text, "Test data string.");

            if (filesFormatCheckBox!.IsChecked)
                result.SetData(DataFormats.Files,
                    new string[]
                    {
                        (Assembly.GetEntryAssembly() ?? throw new Exception()).Location,
                        typeof(Application).Assembly.Location
                    });

            if (bitmapFormatCheckBox!.IsChecked)
                result.SetData(DataFormats.Bitmap, testBitmap);

            return result;
        }

        private string GetDataObjectString(IDataObject value)
        {
            var result = new StringBuilder();
            if (value.GetDataPresent(DataFormats.Text))
                result.AppendLine("Text: " + value.GetData(DataFormats.Text));
            if (value.GetDataPresent(DataFormats.Files))
                result.AppendLine("Files: " + string.Join("\n", (string[])value.GetData(DataFormats.Files)!));
            if (value.GetDataPresent(DataFormats.Bitmap))
            {
                var bitmap = (Image)value.GetData(DataFormats.Bitmap)!;
                result.AppendLine($"Bitmap: {bitmap.Size.Width}x{bitmap.Size.Height}");
            }

            result.AppendLine().AppendLine("All formats: " + string.Join(";", value.GetFormats()));

            return result.ToString();
        }

        private string GetDragDropDataString(object value) => value switch
        {
            string x => x,
            IDataObject x => GetDataObjectString(x),
            _ => throw new Exception()
        };

        private bool IsDataObjectSupported(object value)
        {
            return value switch
            {
                string _ => true,
                IDataObject d => SupportedFormats.Any(f => d.GetDataPresent(f)),
                _ => false
            };
        }

        private void PasteButton_Click(object sender, System.EventArgs e)
        {
            var value = Clipboard.GetDataObject();
            if (IsDataObjectSupported(value))
                MessageBox.Show(this, GetDragDropDataString(value), "Pasted Data");
            else
                MessageBox.Show(this, "Clipboard doesn't contain data in a supported format.");
        }
    }
}