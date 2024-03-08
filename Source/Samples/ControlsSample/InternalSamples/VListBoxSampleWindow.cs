using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class VListBoxSampleWindow : Window
    {
        private readonly VListBox listBox = new();

        public VListBoxSampleWindow()
        {
            Layout = LayoutStyle.Horizontal;

            for(int i = 0; i < 150; i++)
            {
                listBox.Add($"Item {i}");
            }

            listBox.Count = 5000;
            listBox.CustomItemText += ListBox_CustomItemText;

            listBox.Parent = this;
        }

        private void ListBox_CustomItemText(object? sender, GetItemTextEventArgs e)
        {
            if(e.ItemIndex >= listBox.Count)
            {
                e.Result = "Custom item " + e.ItemIndex.ToString();
                e.Handled = true;
            }
        }
    }
}
