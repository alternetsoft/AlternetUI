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
    /// Preview control which can preview "uixml" forms using external preview server.
    /// </summary>
    public class PreviewForm : Control, IFilePreview
    {
        private readonly PictureBox pictureBox = new();

        private string? fileName;
        private Alternet.UI.Integration.PreviewerProcess previewProcess;

        static PreviewForm()
        {
            Alternet.UI.Integration.Log.Write = (s) =>
            {
                App.Log(s);
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewForm"/> class.
        /// </summary>
        public PreviewForm()
        {
            pictureBox.Parent = this;

            previewProcess = new Alternet.UI.Integration.PreviewerProcess();
            previewProcess.ErrorChanged += ErrorChanged;
            previewProcess.PreviewDataReceived += PreviewDataReceived;
            previewProcess.ProcessExited += ProcessExited;
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
            return new PreviewForm();
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
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            Reset();
            previewProcess.ErrorChanged -= ErrorChanged;
            previewProcess.PreviewDataReceived -= PreviewDataReceived;
            previewProcess.ProcessExited -= ProcessExited;
            previewProcess.Dispose();
            previewProcess = null!;

            /*
                await Process.StartAsync(assemblyPath, executablePath, hostAppPath);
                await Process.UpdateXamlAsync(await ReadAllTextAsync(_xamlPath), GetOwnerWindowLocation());

            */
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

        private void ErrorChanged(object? sender, EventArgs e)
        {
            Invoke(Fn);

            void Fn()
            {
                if (previewProcess.PreviewData == null || previewProcess.Error != null)
                {
                    App.LogError("Error while generating preview");
                }
                else
                if (previewProcess.Error == null)
                {
                    UpdateFormPreview();
                }
            }
        }

        private void PreviewDataReceived(object? sender, EventArgs e)
        {
            Invoke(Fn);

            void Fn()
            {
                if (previewProcess.PreviewData != null && previewProcess.Error == null)
                {
                    UpdateFormPreview();
                }
            }
        }

        private void ProcessExited(object? sender, EventArgs e)
        {
        }

        private void UpdateFormPreview()
        {
            try
            {
                var imageFileName = previewProcess.PreviewData.ImageFileName;
                if (File.Exists(imageFileName))
                {
                    var image = Image.FromUrl(imageFileName);
                    pictureBox.Image = image;
                }
                else
                {
                    pictureBox.Image = null;
                }
            }
            catch (Exception e)
            {
                pictureBox.Image = null;
                App.LogError(e);
            }
        }
    }
}
