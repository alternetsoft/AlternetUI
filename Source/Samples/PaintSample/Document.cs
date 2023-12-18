using Alternet.Drawing;
using System;
using System.IO;
using Alternet.UI;

namespace PaintSample
{
    public class Document : IDisposable
    {
        private readonly Control control;
        private bool isDisposed;

        private Image? bitmap;

        private Action<DrawingContext>? previewAction;

        public Document(Control control)
        {
            this.control = control;
            Bitmap = CreateBitmap(control);
            Dirty = false;
        }

        public Document(Control control, string fileName)
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


            var extensions = Image.GetExtensionsForSave();

            if(saveAllFormats)
                foreach (var ext in extensions)
                {
                    var newName = Path.ChangeExtension(fileName, ext);
                    if(!Bitmap.Save(newName))
                        Application.Log($"Error saving: {newName}");
                }
            else
            {
                if (!Bitmap.Save(fileName))
                    Application.Log($"Error saving: {fileName}");
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
                bitmap?.Dispose();
                bitmap = value;
                OnChanged();
            }
        }

        void OnChanged()
        {
            Dirty = true;
            RaiseChanged();
        }

        public Action<DrawingContext>? PreviewAction
        {
            get => previewAction;
            set
            {
                previewAction = value;
                OnChanged();
            }
        }

        public bool Dirty { get; private set; }

        public void Modify(Action<DrawingContext> action)
        {
            using (var dc = DrawingContext.FromImage(Bitmap))
                action(dc);

            using (var dc = DrawingContext.FromImage(Bitmap)) { }

            OnChanged();
        }

        public void UpdatePreview()
        {
            OnChanged();
        }

        public void Paint(Control control, DrawingContext drawingContext)
        {
            drawingContext.FillRectangle(Brushes.White, Bitmap.BoundsDip(control));
            drawingContext.DrawImage(Bitmap, PointD.Empty);
            previewAction?.Invoke(drawingContext);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Bitmap.Dispose();
                    Bitmap = null!;
                }

                isDisposed = true;
            }
        }

        private Bitmap CreateBitmap(Control control)
        {
            var pixelSize = control.PixelFromDip(new SizeD(600, 600));
            var bitmap = new Bitmap(pixelSize, control);
            using var dc = DrawingContext.FromImage(bitmap);
            dc.FillRectangle(new SolidBrush(BackgroundColor), bitmap.BoundsDip(control)); 
            return bitmap;
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