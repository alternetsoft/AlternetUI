using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GenericCheckBoxHandler : ControlHandler<CheckBox>
    {
        private StackPanel? panel;

        private Label? label;
        private Border? outerBorder;
        private Border? innerBorder;

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            return base.GetPreferredSize(availableSize);
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.TextChanged += Control_TextChanged;
            Control.CheckedChanged += Control_CheckedChanged;

            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.MouseClick += NativeControl_MouseClick;

            panel = new StackPanel
            {
                Orientation = StackPanelOrientation.Horizontal
            };

            VisualChildren.Add(panel);

            outerBorder = new Border { Width = 13, Height = 10, Margin = new Thickness(0, 0, 5, 0) };
            panel.Handler.VisualChildren.Add(outerBorder);

            innerBorder = new Border { Width = 6, Height = 6, Margin = new Thickness(4, 4, 0, 0) };
            outerBorder.Handler.VisualChildren.Add(innerBorder);

            label = new Label();
            panel.Handler.VisualChildren.Add(label);

            UpdateText();
            UpdateVisual();
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.MouseClick -= NativeControl_MouseClick;
            Control.CheckedChanged -= Control_CheckedChanged;

            if (panel == null)
                throw new InvalidOperationException();

            VisualChildren.Remove(panel);
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

        private void Control_CheckedChanged(object? sender, EventArgs? e)
        {
            UpdateVisual();
        }

        private void NativeControl_MouseClick(object? sender, EventArgs? e)
        {
            if (IsMouseOver)
                Control.IsChecked = !Control.IsChecked;
        }

        private void UpdateVisual()
        {
            if (innerBorder == null || outerBorder == null)
                throw new InvalidOperationException();

            var borderColor = Color.FromArgb(unchecked((int)0xFF92A0B5));
            var backgroundColor = Color.FromArgb(unchecked((int)0xFFB5C7E2));
            if (IsMouseOver)
            {
                borderColor = Color.FromArgb(unchecked((int)0xFF5C7FB2));
                backgroundColor = Color.FromArgb(unchecked((int)0xFF80AFF2));
            }

            outerBorder.BorderBrush = new SolidBrush(borderColor);
            outerBorder.Background = new SolidBrush(backgroundColor);

            innerBorder.Background = new SolidBrush(Control.IsChecked ? Color.FromArgb(unchecked((int)0xFF4A5C77)) : Color.Transparent);
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            UpdateText();
        }

        private void UpdateText()
        {
            if (label == null)
                throw new InvalidOperationException();

            label.Text = Control.Text;
            Update();
        }
    }
}