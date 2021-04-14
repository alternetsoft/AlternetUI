using System;
using System.Drawing;

namespace Alternet.UI
{
    public class Border : Control
    {
        private Color borderColor = Color.Gray;

        public Border()
        {
        }

        public event EventHandler? BorderColorChanged;

        public Color BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value)
                    return;

                borderColor = value;
                OnBorderColorChanged();
            }
        }

        private void OnBorderColorChanged()
        {
            BorderColorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}