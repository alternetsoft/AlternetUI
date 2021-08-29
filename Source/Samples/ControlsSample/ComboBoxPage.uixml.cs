using Alternet.UI;
using System;

namespace ControlsSample
{
    partial class ComboBoxPage : Control
    {
        private readonly IPageSite site;

        public ComboBoxPage(IPageSite site)
        {
            InitializeComponent();

            this.site = site;

            comboBox.SelectedItemChanged += ComboBox_SelectionChanged;
            comboBox.TextChanged += ComboBox_TextChanged;

            comboBox.Items.Add("One");
            comboBox.Items.Add("Two");
            comboBox.Items.Add("Three");
            comboBox.SelectedIndex = 1;

            addItemButton.Click += AddItemButton_Click;
            removeItemButton.Click += RemoveItemButton_Click;
            addManyItemsButton.Click += AddManyItemsButton_Click;
            setTextToEmptyStringButton.Click += SetTextToEmptyStringButton_Click;
            setSelectedIndexTo2Button.Click += SetSelectedIndexTo2_Click;
            setSelectedItemToNullButton.Click += SetSelectedItemToNullButton_Click;

            allowTextEditingCheckBox.CheckedChanged += AllowTextEditingCheckBox_CheckedChanged;
        }

        private void SetSelectedItemToNullButton_Click(object? sender, EventArgs e)
        {
            comboBox.SelectedItem = null;
        }

        private void SetSelectedIndexTo2_Click(object? sender, EventArgs e)
        {
            comboBox.SelectedIndex = 2;
        }

        private void SetTextToEmptyStringButton_Click(object? sender, EventArgs e)
        {
            comboBox.Text = "";
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            int start = comboBox.Items.Count + 1;
            comboBox.BeginUpdate();
            try
            {
                for (int i = start; i < start + 5000; i++)
                    comboBox.Items.Add("Item " + i);
            }
            finally
            {
                comboBox.EndUpdate();
            }
        }

        private void ComboBox_TextChanged(object? sender, EventArgs e)
        {
            var text = comboBox.Text == "" ? "\"\"" : comboBox.Text;
            site.LogEvent($"ComboBox: TextChanged. Text: {text}");
        }

        private void ComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            site.LogEvent($"ComboBox: SelectionChanged. SelectedIndex: {(comboBox.SelectedIndex == null ? "<null>" : comboBox.SelectedIndex.ToString() )}");
        }

        private void AllowTextEditingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            comboBox.IsEditable = allowTextEditingCheckBox.IsChecked;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            if (comboBox.Items.Count > 0)
                comboBox.Items.RemoveAt(comboBox.Items.Count - 1);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            comboBox.Items.Add("Item " + (comboBox.Items.Count + 1));
        }
    }
}