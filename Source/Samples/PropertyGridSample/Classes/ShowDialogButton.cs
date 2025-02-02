using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal class ShowDialogButton : Button
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
            });
        }

        public CommonDialog? Dialog { get; set; }
    }
}
