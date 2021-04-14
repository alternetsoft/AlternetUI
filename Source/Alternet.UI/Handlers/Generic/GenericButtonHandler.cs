using System;
using System.Drawing;

namespace Alternet.UI
{
    internal class GenericButtonHandler : GenericControlHandler<Button>
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
            return new SizeF(50, 30);
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.TextChanged += Control_TextChanged;

            border = new Border
            {
                Padding = new Thickness(5)
            };

            Control.VisualChildren.Add(border);

            textBlock = new TextBlock();
            border.VisualChildren.Add(textBlock);

            UpdateText();
        }

        protected override void OnDetach()
        {
            base.OnDetach();

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
            Control.InvokeClick(EventArgs.Empty);
        }

        private void UpdateVisual()
        {
            if (border == null)
                throw new InvalidOperationException();

            var color = Color.Gray;
            if (IsMouseOver)
            {
                color = Color.LightBlue;
                if (IsPressed)
                    color = Color.Blue;
            }

            border.BorderColor = color;
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