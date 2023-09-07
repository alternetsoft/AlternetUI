using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    internal class ShowContextMenuButton : Button
    {
        public ShowContextMenuButton()
        {
            Text = "Show Context Menu";
            Click += Button_Click;
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            Menu!.Show(this);
        }

        public ContextMenu? Menu { get; set; }
    }
}
