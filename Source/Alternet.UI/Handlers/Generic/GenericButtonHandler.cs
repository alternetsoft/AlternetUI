using System;
using System.Drawing;

namespace Alternet.UI
{
    internal class GenericButtonHandler : ControlHandler<Button>
    {
        private Border? border;

        private TextBlock? textBlock;

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

            Control.VisualChildren.Add(border);

            textBlock = new TextBlock();
            border.VisualChildren.Add(textBlock);

            UpdateText();
            UpdateVisual();
        }

        private void NativeControl_MouseClick(object? sender, EventArgs? e)
        {
            if (IsMouseOver)
                Control.InvokeClick(EventArgs.Empty);
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.MouseClick -= NativeControl_MouseClick;

            if (border == null)
                throw new InvalidOperationException();

            Control.VisualChildren.Remove(border);
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

            var borderColor = Color.FromArgb(0x92A0B5);
            var backgroundColor = Color.FromArgb(0xB5C7E2);
            if (IsMouseOver)
            {
                borderColor = Color.FromArgb(0x5C7FB2);
                backgroundColor = Color.FromArgb(0x80AFF2);
                if (IsPressed)
                    borderColor = Color.FromArgb(0x729DD8);
            }

            border.BorderColor = borderColor;
            border.BackgroundColor = backgroundColor;
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            UpdateText();
        }

        private void UpdateText()
        {
            if (textBlock == null)
                throw new InvalidOperationException();

            textBlock.Text = Control.Text;
            Update();
        }
    }
}