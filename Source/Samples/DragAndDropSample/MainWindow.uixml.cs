using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
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

        private DragDropEffects GetDropEffect(DragDropEffects defaultEffect)
        {
            var allowedEffects = GetAllowedEffects();
            if (allowedEffects.Count == 0)
                return DragDropEffects.None;

            if (allowedEffects.Count == 1)
                return allowedEffects.Single();

            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0 && allowedEffects.Contains(DragDropEffects.Link))
                return DragDropEffects.Link;

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0 && allowedEffects.Contains(DragDropEffects.Copy))
                return DragDropEffects.Copy;

            if (allowedEffects.Contains(defaultEffect))
                return defaultEffect;

            return allowedEffects.First();
        }

        private IReadOnlyList<DragDropEffects> GetAllowedEffects()
        {
            var output = new List<DragDropEffects>();
            if (moveDropEffectCheckBox.IsChecked)
                output.Add(DragDropEffects.Move);
            if (copyDropEffectCheckBox.IsChecked)
                output.Add(DragDropEffects.Copy);
            if (linkDropEffectCheckBox.IsChecked)
                output.Add(DragDropEffects.Link);

            return output;
        }

        private DragDropEffects GetAllowedEffectsFlags() => GetAllowedEffects().Aggregate((a, b) => a | b);

        private void DropTarget_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = GetDropEffect(e.Effect);
            LogEvent($"DragDrop: {e.MouseClientLocation}, {e.Effect}");
            MessageBox.Show(this, GetStringFromDropResultObject(e.Data), "Dropped Data");
        }

        private void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = GetDropEffect(e.Effect);
            LogEvent($"DragOver: {e.MouseClientLocation}, {e.Effect}");
        }

        private void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = GetDropEffect(e.Effect);
            LogEvent($"DragEnter: {e.MouseClientLocation}, {e.Effect}");
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

        private string GetStringFromDropResultObject(object? value) => value switch
        {
            string x => x,
            IDataObject x => GetDataObjectString(x),
            _ => throw new Exception()
        };

        private bool IsDataObjectSupported(object? value)
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
                MessageBox.Show(this, GetStringFromDropResultObject((object?)value), "Pasted Data");
            else
                MessageBox.Show(this, "Clipboard doesn't contain data in a supported format.");
        }

        bool isDragging = false;

        private void DragSource_MouseDown(object sender, Alternet.UI.MouseButtonEventArgs e)
        {
            isDragging = true;
        }

        private void DragSource_MouseMove(object sender, Alternet.UI.MouseEventArgs e)
        {
            if (isDragging)
            {
                //dragStopwatch.Restart();
                var result = DoDragDrop(GetDataObject(), GetAllowedEffectsFlags());
                MessageBox.Show(this, result.ToString(), "DoDragDrop Result");
                isDragging = false;
            }
        }

        private void DragSource_MouseUp(object sender, Alternet.UI.MouseButtonEventArgs e)
        {
            isDragging = false;
        }
    }
}