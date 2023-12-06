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
            [DataFormats.Text, DataFormats.Files, DataFormats.Bitmap];

        private readonly Bitmap testBitmap;

        public MainWindow()
        {
            Icon = new("embres:DragAndDropSample.Sample.ico");

            InitializeComponent();

            var sizePixels = PixelFromDip(new Size(64, 64));
            testBitmap = new Bitmap(sizePixels);

            SetSizeToContent();

            eventsListBox.BindApplicationLog();
            eventsListBox.ContextMenu.Required();
        }

        private DragDropEffects GetDropEffect(DragDropEffects defaultEffect)
        {
            var allowedEffects = GetAllowedEffects();
            if (allowedEffects.Count == 0)
                return DragDropEffects.None;

            if (allowedEffects.Count == 1)
                return allowedEffects.Single();

            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0
                && allowedEffects.Contains(DragDropEffects.Link))
                return DragDropEffects.Link;

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0
                && allowedEffects.Contains(DragDropEffects.Copy))
                return DragDropEffects.Copy;

            if (allowedEffects.Contains(defaultEffect))
                return defaultEffect;

            return allowedEffects[0];
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

        private DragDropEffects GetAllowedEffectsFlags() =>
            GetAllowedEffects().Aggregate((a, b) => a | b);

        private void DropTarget_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = GetDropEffect(e.Effect);
            LogEvent($"DragDrop: {e.MouseClientLocation}, {e.Effect}");
            LogEvent($"Dropped Data: {GetStringFromDropResultObject(e.Data)}");
        }

        private void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = GetDropEffect(e.Effect);
            LogSmart($"DragOver: {e.MouseClientLocation}, {e.Effect}", "DragOver");
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

        void LogEvent(string message)
        {
            eventsListBox.Items.Add($"{message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }

        void LogSmart(string message, string prefix)
        {
            var s = eventsListBox.LastItem?.ToString();
            var b = s?.StartsWith(prefix) ?? false;

            if (b)
            {
                eventsListBox.LastItem = message;
            }
            else
                LogEvent(message);
        }

        private void PasteButton_Click(object sender, System.EventArgs e)
        {
            var value = Clipboard.GetDataObject();
            if (IsDataObjectSupported(value))
                LogEvent($"Pasted Data: {GetStringFromDropResultObject((object?)value)}");
            else
                LogEvent("Clipboard doesn't contain data in a supported format.");
        }

        private void CopyButton_Click(object sender, System.EventArgs e)
        {
            var dataObject = GetDataObject();
            Clipboard.SetDataObject(dataObject);
        }

        private IDataObject GetDataObject()
        {
            var result = new DataObject();

            if (textFormatCheckBox!.IsChecked)
                result.SetText("Test data string.");

            if (filesFormatCheckBox!.IsChecked)
                result.SetFiles(
                    [
                        (Assembly.GetEntryAssembly() ?? throw new Exception()).Location,
                        typeof(Application).Assembly.Location
                    ]);

            if (bitmapFormatCheckBox!.IsChecked)
                result.SetBitmap(testBitmap);

            return result;
        }

        private string GetStringFromDropResultObject(object? value) => value switch
        {
            string x => x,
            IDataObject x => DataObject.ToDebugString(x),
            _ => throw new Exception(),
        };

        private bool IsDataObjectSupported(object? value)
        {
            return value switch
            {
                string _ => true,
                IDataObject d => SupportedFormats.Any(f => d.GetDataPresent(f)),
                _ => false,
            };
        }

        bool isDragging = false;

        private void DragSource_MouseDown(object sender, Alternet.UI.MouseButtonEventArgs e)
        {
            isDragging = true;
        }

        private void DragSource_MouseMove(object sender, Alternet.UI.MouseEventArgs e)
        {
            /*LogSmart($"MouseMove: {LogUtils.GenNewId()}", "MouseMove");*/

            if (isDragging)
            {
                var result = DoDragDrop(GetDataObject(), GetAllowedEffectsFlags());
                var prefix = "DoDragDrop Result";
                LogSmart($"{prefix}: {result}", prefix);
                isDragging = false;
            }
        }

        private void DragSource_MouseUp(object sender, Alternet.UI.MouseButtonEventArgs e)
        {
            isDragging = false;
        }
    }
}