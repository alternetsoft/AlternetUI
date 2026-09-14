using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace PropertyGridSample
{
    internal class ShowDialogButton : XButton
    {
        public ShowDialogButton()
        {
            Text = "Show Dialog";
            Click += ShowDialogButton_Click;
        }

        private void ShowDialogButton_Click(object? sender, EventArgs e)
        {
            Dialog?.ShowAsync(ParentWindow, (result) =>
            {
                ComponentDesigner.SafeDefault.RaisePropertyChanged(Dialog, null);

                if (result)
                {
                    App.Log($"Dialog result: OK");

                    if (Dialog is OpenFileDialog fileOpenDialog)
                    {
                        if (fileOpenDialog.FileName is null)
                            return;

                        App.Log($"Selected file: {fileOpenDialog.FileName}");

                        var ext = Path.GetExtension(fileOpenDialog.FileName);

                        if (ext == ".ttf")
                        {
                            var family = FontFamily.FromFile(fileOpenDialog.FileName);
                            var font = new Font(family, 12);
                            font.Log();
                        }
                    }
                }
                else
                {
                    App.Log($"Dialog result: Cancel");
                }
            });
        }

        public CommonDialog? Dialog { get; set; }
    }
}
