using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Preview control which can preview "uixml" forms.
    /// There is also <see cref="PreviewUixmlSplitted"/> class which allows to preview
    /// uixml together with its source code.
    /// </summary>
    public class PreviewUixml : Control, IFilePreview
    {
        private static HiddenWindow? previewWindow;

        private readonly Border control = new()
        {
            MinimumSize = (400, 400),
            BackgroundColor = SystemColors.ButtonFace,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private string? fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewUixml"/> class.
        /// </summary>
        public PreviewUixml()
        {
            control.Parent = this;
        }

        /// <summary>
        /// <inheritdoc cref="IFilePreview.FileName"/>
        /// </summary>
        public string? FileName
        {
            get => fileName;

            set
            {
                if (fileName == value)
                    return;
                fileName = value;
                Reload();
            }
        }

        Control IFilePreview.Control { get => this; }

        /// <summary>
        /// Gets whether specified file is supported in this preview control.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
        /// <returns></returns>
        public static bool IsSupportedFile(string fileName)
        {
            var ext = PathUtils.GetExtensionLower(fileName);

            return ext == "uixml";
        }

        /// <summary>
        /// Creates this preview control.
        /// </summary>
        /// <returns></returns>
        public static IFilePreview CreatePreviewControl()
        {
            return new PreviewUixml();
        }

        /// <summary>
        /// Reloads previewed file.
        /// </summary>
        public virtual void Reload()
        {
            App.DoInsideBusyCursor(ReloadInternal);
        }

        /// <summary>
        /// Resets this object unloading the file which is currently previewed.
        /// </summary>
        public virtual void Reset()
        {
            if (previewWindow is null)
                return;

            var children = control.Children.Clone();
            foreach (var child in children)
                child.Parent = previewWindow;

            previewWindow.Close();
            previewWindow = null;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            Reset();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                e.Handled = true;
                Reload();
            }

            base.OnKeyDown(e);
        }

        private void ReloadInternal()
        {
            Reset();

            if (string.IsNullOrEmpty(fileName) || !GetFileSystem().FileExists(fileName))
                return;

            try
            {
                using var stream = GetFileSystem().OpenRead(fileName!);

                previewWindow = new();
                previewWindow.Disposed += PreviewWindow_Disposed;

                var saved = UixmlLoader.ShowExceptionDialog;

                try
                {
                    UixmlLoader.ShowExceptionDialog = false;
                    UixmlLoader.LoadExistingEx(
                        stream,
                        previewWindow,
                        0,
                        fileName);
                }
                finally
                {
                    UixmlLoader.ShowExceptionDialog = saved;
                    previewWindow.Visible = false;
                }

                control.Visible = false;
                control.MinimumSize = ClientSize;

                control.DoInsideLayout(() =>
                {
                    var children = previewWindow.Children.Clone();
                    foreach (var child in children)
                        child.Parent = control;
                });

                control.Visible = true;

                Refresh();
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
            }
        }

        private void PreviewWindow_Disposed(object? sender, EventArgs e)
        {
            App.LogIf("PreviewWindow.Disposed", false);
        }
    }
}
