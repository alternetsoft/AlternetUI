using Alternet.Drawing;
using System;
using System.IO;
using Alternet.UI;
using Alternet.UI.Extensions;

namespace PaintSample
{
    public class PaintSampleDocument : DisposableObject
    {
        private readonly Control control;

        private Image? bitmap;

        private Action<Graphics>? previewAction;

        public PaintSampleDocument(Control control)
        {
            this.control = control;
            Bitmap = CreateBitmap(control);
            Dirty = false;
        }

        public PaintSampleDocument(Control control, string fileName)
        {
            this.control = control;
            Bitmap = LoadBitmap(fileName);
            FileName = fileName;
            Dirty = false;
        }

        public Control Control => control;

        public void Save(string fileName)
        {
            var saveAllFormats = false;


            var extensions = Image.ExtensionsForSave;

            if(saveAllFormats)
                foreach (var ext in extensions)
                {
                    var newName = Path.ChangeExtension(fileName, ext);
                    if(!Bitmap.Save(newName))
                        App.Log($"Error saving: {newName}");
                }
            else
            {
                if (!Bitmap.Save(fileName))
                    App.Log($"Error saving: {fileName}");
            }
            Dirty = false;
            FileName = fileName;
            RaiseChanged();
        }

        public string? FileName { get; set; }

        public Color BackgroundColor => Color.White;

        public event EventHandler? Changed;

        public Image Bitmap
        {
            get => bitmap ?? throw new Exception();
            set
            {
                if (bitmap == value)
                    return;
                SafeDispose(ref bitmap);
                bitmap = value;
                OnChanged();
            }
        }

        void OnChanged()
        {
            Dirty = true;
            RaiseChanged();
        }

        public Action<Graphics>? PreviewAction
        {
            get => previewAction;
            set
            {
                previewAction = value;
                OnChanged();
            }
        }

        public bool Dirty { get; private set; }

        public void Modify(Action<Graphics> action)
        {
            using (var dc = Graphics.FromImage(Bitmap))
                action(dc);

            using (var dc = Graphics.FromImage(Bitmap)) { }

            OnChanged();
        }

        public void UpdatePreview()
        {
            OnChanged();
        }

        public void Paint(Control control, Graphics drawingContext)
        {
            drawingContext.FillRectangle(Brushes.White, Bitmap.BoundsDip(control));
            drawingContext.DrawImage(Bitmap, PointD.Empty);
            previewAction?.Invoke(drawingContext);
        }

        protected override void DisposeManaged()
        {
            SafeDispose(ref bitmap);
        }

        private Image CreateBitmap(Control control)
        {
            var pixelSize = control.PixelFromDip(new SizeD(600, 600));
            return Image.Create(pixelSize.Width, pixelSize.Height, BackgroundColor);
        }

        private Bitmap LoadBitmap(string fileName)
        {
            var image = new GenericImage(fileName);
            image.ResizeNoScale((600, 600), (0, 0), Color.White);
            return (Bitmap)image;
        }

        private void RaiseChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}