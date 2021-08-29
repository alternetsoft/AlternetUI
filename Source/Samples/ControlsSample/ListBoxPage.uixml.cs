using Alternet.UI;
using System;

namespace ControlsSample
{
    partial class ListBoxPage : Control
    {
        private readonly IPageSite site;

        public ListBoxPage(IPageSite site)
        {
            InitializeComponent();

            listBox.SelectionChanged += ListBox_SelectionChanged;

            listBox.Items.Add("One");
            listBox.Items.Add("Two");
            listBox.Items.Add("Three");

            addItemButton.Click += AddItemButton_Click;
            removeItemButton.Click += RemoveItemButton_Click;
            addManyItemsButton.Click += AddManyItemsButton_Click;

            allowMultipleSelectionCheckBox.CheckedChanged += AllowMultipleSelectionCheckBox_CheckedChanged;
            this.site = site;
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            int start = listBox.Items.Count + 1;

            listBox.BeginUpdate();
            try
            {
                for (int i = start; i < start + 5000; i++)
                    listBox.Items.Add("Item " + i);
            }
            finally
            {
                listBox.EndUpdate();
            }
        }

        private void ListBox_SelectionChanged(object? sender, EventArgs e)
        {
            string selectedIndicesString = listBox.SelectedIndices.Count > 100 ? "too many indices to display" : string.Join(",", listBox.SelectedIndices);
            site.LogEvent($"ListBox: SelectionChanged. SelectedIndices: ({selectedIndicesString})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            listBox.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? ListBoxSelectionMode.Multiple : ListBoxSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            if (listBox.Items.Count > 0)
                listBox.Items.RemoveAt(listBox.Items.Count - 1);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            listBox.Items.Add("Item " + (listBox.Items.Count + 1));
        }
    }
}