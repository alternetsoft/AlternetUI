﻿using System;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ComboBoxPage : Control, IComboBoxItemPainter
    {
        private readonly bool ignoreEvents = false;
        private const bool supressUpDown = false;
        private int newItemIndex = 0;

        public ComboBoxPage()
        {
            ignoreEvents = true;
            InitializeComponent();

            comboBox.Items.Add("One");
            comboBox.Items.Add("Two");
            comboBox.Items.Add("Three");
            comboBox.SelectedIndex = 1;
            ignoreEvents = false;

            addItemButton.KeyDown += AddItemButton_KeyDown;
            KeyDown += ComboBoxPage_KeyDown;
        }

        private void ComboBoxPage_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void AddItemButton_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Up || e.Key == Key.Down) && supressUpDown)
                e.Handled = true;
        }

        private void Editor_Click(object? sender, System.EventArgs e)
        {
            DialogFactory.EditItemsWithListEditor(comboBox);
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
            comboBox.BeginUpdate();
            try
            {
                for (int i = 0; i < 500; i++)
                    comboBox.Items.Add("Item " + GenItemIndex());
            }
            finally
            {
                comboBox.EndUpdate();
            }
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
            comboBox.HasBorder = !comboBox.HasBorder;
        }

        private void ComboBox_TextChanged(object? sender, EventArgs e)
        {
            if (ignoreEvents)
                return;
            
            var text = comboBox.Text == string.Empty ? "\"\"" : comboBox.Text;
            var prefix = "ComboBox: TextChanged. Text:";
            Application.LogReplace($"{prefix} {text}", prefix);
        }

        private void ComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (ignoreEvents)
                return;
            var s = (comboBox.SelectedIndex == null ? "<null>" : comboBox.SelectedIndex.ToString());
            var prefix = "ComboBox: SelectedItemChanged.SelectedIndex:";
            Application.LogReplace($"{prefix} {s}", prefix);
        }

        private void OwnerDrawCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (ownerDrawCheckBox.IsChecked)
            {
                comboBox.ItemPainter = this;
                comboBox.OwnerDrawItem = true;
            }
            else
            {
                comboBox.ItemPainter = null;
                comboBox.OwnerDrawItem = false;
            }
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

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            comboBox.Items.Add("Item " + GenItemIndex());
        }

        private bool CheckComboBoxIsEditable()
        {
            bool isEditable = comboBox.IsEditable;

            if (!isEditable)
                Application.Log("Cannot perform this operation on a non-editable ComboBox.");

            return isEditable;
        }

        private void SelectTextRangeButton_Click(object? sender, System.EventArgs e)
        {
            if (!CheckComboBoxIsEditable())
                return;

            comboBox.SelectTextRange(2, 3);
        }

        private void GetTextSelectionButton_Click(object? sender, System.EventArgs e)
        {
            if (!CheckComboBoxIsEditable())
                return;

            var start = comboBox.TextSelectionStart;
            var length = comboBox.TextSelectionLength;
            var selectedText = comboBox.Text.Substring(start, length);
            var message = $"[{start}..{start + length}], selected text: '{selectedText}'";
            Application.Log("ComboBox Text Selection: " + message);
        }

        private void SetItem_Click(object? sender, System.EventArgs e)
        {
            comboBox.Items[2] = "hello";
        }

        private void SetTextToAbcButton_Click(object? sender, System.EventArgs e)
        {
            if (!CheckComboBoxIsEditable())
                return;
            comboBox.Text = "abc";
        }

        private void SetTextToOneButton_Click(object? sender, System.EventArgs e)
        {
            comboBox.Text = "One";
        }

        void IComboBoxItemPainter.Paint(ComboBox sender, ComboBoxItemPaintEventArgs e)
        {
            e.DefaultPaint();
            if(e.IsPaintingControl)
                e.Graphics.FillRectangle(Color.Red, (e.ClipRectangle.Location, (5, 5)));
            else
            {
                var point = e.ClipRectangle.TopRight;
                point.Offset(-5, 0);

                if (e.IsSelected)
                    e.Graphics.FillRectangle(Color.Yellow, (point, (10, e.ClipRectangle.Height)));
                else
                    e.Graphics.FillRectangle(Color.Green, (point, (10, e.ClipRectangle.Height)));
            }
        }

        double IComboBoxItemPainter.GetHeight(ComboBox sender, int index, double defaultHeight)
        {
            return -1;
        }

        double IComboBoxItemPainter.GetWidth(ComboBox sender, int index, double defaultWidth)
        {
            return -1;
        }
    }
}