using Alternet.Drawing;
using System;
using System.IO;
using Alternet.UI;

namespace PaintSample
{
    public class Document : IDisposable
    {
        private bool isDisposed;

        private Bitmap? bitmap;

        private Action<DrawingContext>? previewAction;

        public Document()
        {
            Bitmap = CreateBitmap();
            Dirty = false;
        }

        public Document(string fileName)
        {
            Bitmap = LoadBitmap(fileName);
            FileName = fileName;
            Dirty = false;
        }

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

        public Bitmap Bitmap
        {
            get => bitmap ?? throw new Exception();
            set
            {
                if (bitmap == value)
                    return;

                if (bitmap != null)
                    bitmap.Dispose();

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

        public void Paint(DrawingContext drawingContext)
        {
            drawingContext.FillRectangle(Brushes.White, Bitmap.Bounds);
            drawingContext.DrawImage(Bitmap, Point.Empty);
            if (previewAction != null)
                previewAction(drawingContext);
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

        private Bitmap CreateBitmap()
        {
            var bitmap = new Bitmap((600, 600));
            using var dc = DrawingContext.FromImage(bitmap);
            dc.FillRectangle(new SolidBrush(BackgroundColor), bitmap.Bounds); 
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