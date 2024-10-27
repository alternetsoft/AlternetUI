using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace ControlsSample
{
    internal class VComboBoxWindow : Window
    {
        private readonly ComboBox comboBox = new()
        {
        };

        private readonly VirtualListBox listBox = new()
        {
        };

        private readonly VirtualListBox popupListBox = new()
        {
            HasBorder = false,
        };

        public VComboBoxWindow()
        {
            Size = (800, 600);
            Title = "ComboBox with VirtualListBox in the popup";
            StartLocation = WindowStartLocation.CenterScreen;
            Layout = LayoutStyle.Vertical;
            MinChildMargin = 10;

            PropertyGridSample.ObjectInit.AddDefaultOwnerDrawItems(
                this,
                (item) =>
                {
                    popupListBox.Add(item);
                    listBox.Add(item);
                },
                true);

            comboBox.PopupControl = popupListBox;
            comboBox.Parent = this;

            listBox.Parent = this;
            listBox.VerticalAlignment = VerticalAlignment.Fill;

            comboBox.TextChanged += ComboBox_TextChanged;

            ContextMenuStrip = new ContextMenuStrip();

            ContextMenuStrip.Add("Toggle HasBorder of the popup", () =>
            {
                popupListBox.HasBorder = !popupListBox.HasBorder;
            });
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
        }
    }
}         
