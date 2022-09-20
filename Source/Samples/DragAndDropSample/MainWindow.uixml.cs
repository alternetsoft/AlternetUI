using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;
using System.Text;

namespace DragAndDropSample
{
    public partial class MainWindow : Window
    {
        private const string CustomTextFormatName = "MyCustomTextFormat";

        private static readonly string[] SupportedFormats =
            new[] { DataFormats.Text, DataFormats.Files, DataFormats.Bitmap, CustomTextFormatName };

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

        private IDataObject GetDataObject()
        {
            var result = new DataObject();

            if (textFormatCheckBox!.IsChecked)
                result.SetData(DataFormats.Text, "Test data string.");

            if (customFormatCheckBox!.IsChecked)
            {
                result.SetData(CustomTextFormatName, "Custom data string.");
            }

            if (filesFormatCheckBox!.IsChecked)
                result.SetData(DataFormats.Files,
                    new string[]
                    {
#if WINDOWS
                        Path.Combine(Environment.SystemDirectory, "notepad.exe"),
                        Path.Combine(Environment.SystemDirectory, "mspaint.exe"),
#elif MACOS
                        "/System/Applications/TextEdit.app/Contents/Info.plist",
                        "/System/Applications/TextEdit.app/Contents/PkgInfo",
#endif
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
            if (value.GetDataPresent(CustomTextFormatName))
                result.AppendLine("Custom text: " + value.GetData(CustomTextFormatName));
            if (value.GetDataPresent(DataFormats.Files))
                result.AppendLine("Files: " + string.Join(";", (string[])value.GetData(DataFormats.Files)!));
            if (value.GetDataPresent(DataFormats.Bitmap))
            {
                var bitmap = (Image)value.GetData(DataFormats.Bitmap)!;
                result.AppendLine($"Bitmap: {bitmap.Size.Width}x{bitmap.Size.Height}");
            }

#if MACOS
            const string FinderNodeFormat = "com.apple.finder.node";
            if (value.GetDataPresent(FinderNodeFormat))
                result.AppendLine("FinderNode: " + ((byte[])value.GetData(FinderNodeFormat)).Length + " bytes");
#endif

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