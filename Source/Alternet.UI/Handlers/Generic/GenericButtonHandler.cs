using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GenericButtonHandler : ControlHandler<Button>
    {
        private Border? border;

        private Label? label;

        private bool isPressed;

        private bool IsPressed
        {
            get => isPressed;
            set
            {
                if (isPressed == value)
                    return;

                isPressed = value;
                UpdateVisual();
            }
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            return base.GetPreferredSize(availableSize);
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.TextChanged += Control_TextChanged;

            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.MouseClick += NativeControl_MouseClick;

            border = new Border
            {
                Padding = new Thickness(5)
            };

            Control.Handler.VisualChildren.Add(border);

            label = new Label();
            border.Handler.VisualChildren.Add(label);

            UpdateText();
            UpdateVisual();
        }

        private void NativeControl_MouseClick(object? sender, EventArgs? e)
        {
            if (IsMouseOver)
                Control.RaiseClick(EventArgs.Empty);
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.MouseClick -= NativeControl_MouseClick;

            if (border == null)
                throw new InvalidOperationException();

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

        protected override void OnMouseLeftButtonDown()
        {
            base.OnMouseLeftButtonDown();
            CaptureMouse();
            IsPressed = true;
        }

        protected override void OnMouseLeftButtonUp()
        {
            ReleaseMouseCapture();
            base.OnMouseLeftButtonUp();
            IsPressed = false;
        }

        private void UpdateVisual()
        {
            if (border == null)
                throw new InvalidOperationException();

            var borderColor = Color.FromArgb(unchecked((int)0xFF92A0B5));
            var backgroundColor = Color.FromArgb(unchecked((int)0xFFB5C7E2));
            if (IsMouseOver)
            {
                borderColor = Color.FromArgb(unchecked((int)0xFF5C7FB2));
                backgroundColor = Color.FromArgb(unchecked((int)0xFF80AFF2));
                if (IsPressed)
                    borderColor = Color.FromArgb(unchecked((int)0xFF729DD8));
            }

            border.BorderBrush = new SolidBrush(borderColor);
            border.Background = new SolidBrush(backgroundColor);
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