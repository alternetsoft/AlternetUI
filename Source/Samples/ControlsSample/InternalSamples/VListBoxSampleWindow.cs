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
            InitVListBox(listBox);
            listBox.Parent = this;
        }

        public static void InitVListBox(VListBox listBox)
        {
            for (int i = 0; i < 150; i++)
            {
                listBox.Add($"Item {i}");
            }

            listBox.Count = 5000;
            listBox.CustomItemText += ListBox_CustomItemText;
        }

        private static void ListBox_CustomItemText(object? sender, GetItemTextEventArgs e)
        {
            if (sender is not VListBox listBox)
                return;
            if(e.ItemIndex >= listBox.Count)
            {
                e.Result = "Custom item " + e.ItemIndex.ToString();
                e.Handled = true;
            }
        }
    }
}
