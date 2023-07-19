using System;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ComboBoxPage : Control
    {
        private IPageSite? site;
        bool ignoreEvents = false;

        public ComboBoxPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;

                ignoreEvents = true;
                comboBox.Items.Add("One");
                comboBox.Items.Add("Two");
                comboBox.Items.Add("Three");
                comboBox.SelectedIndex = 1;
                ignoreEvents = false;
            }
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
            comboBox.Text = string.Empty;
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

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {

        }

        private void ComboBox_TextChanged(object? sender, EventArgs e)
        {
            if (ignoreEvents)
                return;
            
            var text = comboBox.Text == string.Empty ? "\"\"" : comboBox.Text;
            site?.LogEvent($"ComboBox: TextChanged. Text: {text}");
        }

        private void ComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (ignoreEvents)
                return;
            site?.LogEvent($"ComboBox: SelectedItemChanged. SelectedIndex: {(comboBox.SelectedIndex == null ? "<null>" : comboBox.SelectedIndex.ToString())}");
        }

        private void AllowTextEditingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            comboBox.IsEditable = allowTextEditingCheckBox.IsChecked;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            if (comboBox.Items.Count > 0)
                comboBox.Items.RemoveAt(comboBox.SelectedIndex ?? comboBox.Items.Count - 1);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            comboBox.Items.Add("Item " + (comboBox.Items.Count + 1));
        }

        private bool CheckComboBoxIsEditable()
        {
            bool isEditable = comboBox.IsEditable;

            if (!isEditable)
                site?.LogEvent("Cannot perform this operation on a non-editable ComboBox.");

            return isEditable;
        }

        private void SelectTextRangeButton_Click(object sender, System.EventArgs e)
        {
            if (!CheckComboBoxIsEditable())
                return;

            comboBox.SelectTextRange(2, 3);
        }

        private void GetTextSelectionButton_Click(object sender, System.EventArgs e)
        {
            if (!CheckComboBoxIsEditable())
                return;

            var start = comboBox.TextSelectionStart;
            var length = comboBox.TextSelectionLength;
            var selectedText = comboBox.Text.Substring(start, length);
            var message = $"ComboBox text selection is: [{start}..{start + length}], selected text: '{selectedText}'";
            site?.LogEvent("ComboBox Text Selection: " + message);
        }

        private void SetTextToAbcButton_Click(object sender, System.EventArgs e)
        {
            if (!CheckComboBoxIsEditable())
                return;
            comboBox.Text = "abc";
        }

        private void SetTextToOneButton_Click(object sender, System.EventArgs e)
        {
            comboBox.Text = "One";
        }
    }
}