using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInit
    {
        public static void InitVListBox(object control)
        {
            if (control is not VListBox listBox)
                return;

            for (int i = 0; i < 150; i++)
            {
                listBox.Add($"Item {i}");
            }

            listBox.Count = 5000;
            listBox.CustomItemText += ListBox_CustomItemText;

            static void ListBox_CustomItemText(object? sender, GetItemTextEventArgs e)
            {
                if (sender is not VListBox listBox)
                    return;
                if (e.ItemIndex >= listBox.Items.Count)
                {
                    e.Result = "Custom item " + e.ItemIndex.ToString();
                    e.Handled = true;
                }
            }
        }

        public static void InitListBox(object control)
        {
            if (control is not ListBox listBox)
                return;
            listBox.SuggestedSize = defaultListSize;
            listBox.Items.AddRange(GetTenItems());
        }

        public static void InitCheckListBox(object control)
        {
            if (control is not CheckListBox listBox)
                return;
            listBox.SuggestedSize = defaultListHeight;
            listBox.Items.AddRange(GetTenItems());
        }

        public static void InitComboBox(object control)
        {
            if (control is not ComboBox comboBox)
                return;
            comboBox.Items.AddRange(GetTenItems());
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.SuggestedWidth = 200;
        }

        public static void InitColorComboBox(object control)
        {
            if (control is not ColorComboBox comboBox)
                return;
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.SuggestedWidth = 200;
            comboBox.Value = Color.Red;
        }
    }
}