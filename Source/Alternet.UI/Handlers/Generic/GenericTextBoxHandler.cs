using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GenericTextBoxHandler : ControlHandler<TextBox>
    {
        private Border? border;

        private TextBox? editTextBox;

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.TextChanged += Control_TextChanged;

            border = new Border
            {
                Padding = new Thickness(5),
                Background = Brushes.White
            };

            VisualChildren.Add(border);

            editTextBox = new TextBox { EditControlOnly = true, Background = Brushes.White };
            border.Handler.VisualChildren.Add(editTextBox);

            editTextBox.Text = Control.Text;
            editTextBox.TextChanged += EditTextBox_TextChanged;

            UpdateVisual();
        }

        private void EditTextBox_TextChanged(object? sender, EventArgs? e)
        {
            if (editTextBox == null)
                throw new InvalidOperationException();

            Control.Text = editTextBox.Text;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (border == null)
                throw new InvalidOperationException();

            if (editTextBox == null)
                throw new InvalidOperationException();

            editTextBox.TextChanged -= EditTextBox_TextChanged;

            Control.Handler.VisualChildren.Remove(border);
            Control.TextChanged -= Control_TextChanged;
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();
            UpdateVisual();
        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            UpdateVisual();
        }

        protected override void OnMouseMove()
        {
            base.OnMouseMove();
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (border == null)
                throw new InvalidOperationException();

            var color = Color.FromArgb(unchecked((int)0xFF92A0B5));
            if (IsMouseOver)
            {
                color = Color.FromArgb(unchecked((int)0xFF5C7FB2));
            }

            border.BorderBrush = new SolidBrush(color);
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            if (editTextBox == null)
                throw new InvalidOperationException();

            editTextBox.Text = Control.Text;
        }
    }
}