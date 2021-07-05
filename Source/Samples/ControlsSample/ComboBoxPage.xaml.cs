using Alternet.UI;
using System;

namespace ControlsSample
{
    internal class ComboBoxPage : Control
    {
        private ComboBox comboBox;
        private CheckBox allowTextEditingCheckBox;
        private readonly IPageSite site;

        public ComboBoxPage(IPageSite site)
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ControlsSample.ComboBoxPage.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            comboBox = (ComboBox)FindControl("comboBox");
            comboBox.SelectedItemChanged += ComboBox_SelectionChanged;
            comboBox.TextChanged += ComboBox_TextChanged;

            comboBox.Items.Add("One");
            comboBox.Items.Add("Two");
            comboBox.Items.Add("Three");

            ((Button)FindControl("addItemButton")).Click += AddItemButton_Click;
            ((Button)FindControl("removeItemButton")).Click += RemoveItemButton_Click;
            
            allowTextEditingCheckBox = (CheckBox)FindControl("allowTextEditingCheckBox");
            allowTextEditingCheckBox.CheckedChanged += AllowTextEditingCheckBox_CheckedChanged;
            this.site = site;
        }

        private void ComboBox_TextChanged(object? sender, EventArgs e)
        {
            site.LogEvent($"ComboBox: TextChanged. Text: {comboBox.Text}");
        }

        private void ComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            site.LogEvent($"ComboBox: SelectionChanged. SelectedIndex: {(comboBox.SelectedIndex == null ? "<none>" : comboBox.SelectedIndex.ToString() )}");
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