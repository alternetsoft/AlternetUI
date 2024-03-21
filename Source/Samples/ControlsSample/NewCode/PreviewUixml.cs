using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PreviewUixml : Control
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

        public PreviewUixml()
        {
            control.Parent = this;
        }

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                e.Handled = true;
                Reload();
            }

            base.OnKeyDown(e);
        }

        public void Reload()
        {
            Application.DoInsideBusyCursor(ReloadInternal);
        }

        public void DisposeChildren(Control control)
        {
            var children = control.Children.Clone();
            foreach (var child in children)
            {
                child.Parent = null;
                child.Dispose();
            }
        }

        public void Reset()
        {
            DisposeChildren(control);
        }

        protected override void DisposeResources()
        {
            base.DisposeResources();
            Reset();
        }

        private void ReloadInternal()
        {
            Reset();

            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return;

            try
            {
                using var stream = File.Open(fileName, FileMode.Open);

                if(previewWindow is null)
                {
                    previewWindow = new();
                    previewWindow.Disposed += PreviewWindow_Disposed;
                }

                DisposeChildren(previewWindow);

                var saved = UixmlLoader.ShowExceptionDialog;

                try
                {
                    UixmlLoader.ShowExceptionDialog = false;
                    UixmlLoader.LoadExistingEx(
                        stream,
                        previewWindow,
                        false,
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

        private void PreviewWindow_Disposed(object sender, EventArgs e)
        {
            Application.LogIf("PreviewWindow.Disposed", false);
        }
    }
}
