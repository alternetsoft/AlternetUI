using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DragAndDropSample
{
    public partial class DragAndDropWindow : Window
    {
        private static readonly string[] SupportedFormats =
            { DataFormats.Text, DataFormats.Files, DataFormats.Bitmap };

        private readonly Bitmap testBitmap;

        public DragAndDropWindow()
        {
            Icon = App.DefaultIcon;

            InitializeComponent();

            var sizePixels = PixelFromDip(new SizeD(64, 64));
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

            if ((Keyboard.Modifiers & Alternet.UI.ModifierKeys.Alt) != 0
                && allowedEffects.Contains(DragDropEffects.Link))
                return DragDropEffects.Link;

            if ((Keyboard.Modifiers & Alternet.UI.ModifierKeys.Control) != 0
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
            App.Log($"DragDrop: {e.MouseClientLocation}, {e.Effect}");
            App.Log($"Dropped Data: {GetStringFromDropResultObject(e.Data)}");
        }

        private void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = GetDropEffect(e.Effect);
            App.LogReplace($"DragOver: {e.MouseClientLocation}, {e.Effect}", "DragOver");
        }

        private void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = GetDropEffect(e.Effect);
            App.Log($"DragEnter: {e.MouseClientLocation}, {e.Effect}");
        }

        private void DropTarget_DragLeave(object sender, EventArgs e)
        {
            App.Log("DragLeave");
        }

        private void PasteButton_Click(object sender, System.EventArgs e)
        {
            var value = Clipboard.GetDataObject();
            if (IsDataObjectSupported(value))
                App.Log($"Pasted Data: {GetStringFromDropResultObject((object?)value)}");
            else
                App.Log("Clipboard doesn't contain data in a supported format.");
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
                    (Assembly.GetEntryAssembly() ?? throw new Exception()).Location,
                    typeof(App).Assembly.Location);

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

        private void DragSource_MouseDown(object sender, Alternet.UI.MouseEventArgs e)
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
                App.LogReplace($"{prefix}: {result}", prefix);
                isDragging = false;
            }
        }

        private void DragSource_MouseUp(object sender, Alternet.UI.MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}