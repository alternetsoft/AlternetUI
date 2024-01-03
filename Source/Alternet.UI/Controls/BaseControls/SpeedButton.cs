using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements speed button control.
    /// </summary>
    public class SpeedButton : PictureBox
    {
        private Action? clickAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButton"/> class.
        /// </summary>
        public SpeedButton()
        {
            AcceptsFocusAll = false;
            ImageStretch = false;
            Borders ??= new();

            var border = BorderSettings.Default.Clone();
            border.UniformCornerRadius = 50;
            border.UniformRadiusIsPercent = true;
            Borders.SetObject(border, GenericControlState.Hovered);
            Borders.SetObject(border, GenericControlState.Pressed);
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which will be executed when
        /// this control is clicked by the user.
        /// </summary>
        [Browsable(false)]
        public Action? ClickAction
        {
            get => clickAction;
            set
            {
                if (clickAction != null)
                    Click -= OnClickAction;
                clickAction = value;
                if (clickAction != null)
                    Click += OnClickAction;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            RaiseClick(EventArgs.Empty);
        }

        private void OnClickAction(object? sender, EventArgs? e)
        {
            clickAction?.Invoke();
        }
    }
}
