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
        private static SpeedButton? defaults;
        private Action? clickAction;
        private bool sticky;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButton"/> class.
        /// </summary>
        public SpeedButton()
        {
            AcceptsFocusAll = false;
            ImageStretch = false;
            Borders ??= new();

            if(defaults is null || defaults.Borders is null)
            {
                var border = BorderSettings.Default.Clone();
                border.UniformRadiusIsPercent = true;
                border.UniformCornerRadius = 25;
                Borders.SetObject(border, GenericControlState.Hovered);
                Borders.SetObject(border, GenericControlState.Pressed);
            }
            else
            {
                Borders.Assign(defaults.Borders);
                Backgrounds = defaults.Backgrounds;
            }
        }

        /// <summary>
        /// Gets or sets default settings for the <see cref="SpeedButton"/>.
        /// </summary>
        /// <remarks>
        /// Create instance of the <see cref="SpeedButton"/> and assign to this property.
        /// You can specify border and background settings and all new <see cref="SpeedButton"/>
        /// controls will inherit them.
        /// </remarks>
        public static SpeedButton? Defaults
        {
            get
            {
                return defaults;
            }

            set
            {
                defaults = value;
            }
        }

        /// <summary>
        /// Gets or sets whether control is sticky.
        /// </summary>
        /// <remarks>
        /// When this property is true, control painted as pressed if it is not disabled.
        /// </remarks>
        public bool Sticky
        {
            get
            {
                return sticky;
            }

            set
            {
                if (sticky == value)
                    return;
                sticky = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override GenericControlState CurrentState
        {
            get
            {
                var result = base.CurrentState;
                if (sticky)
                {
                    if (result == GenericControlState.Normal || result == GenericControlState.Focused)
                        result = GenericControlState.Pressed;
                }

                return result;
            }
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

        /// <summary>
        /// Gets or sets <see cref="ContextMenu"/> which is shown when control is clicked.
        /// </summary>
        public ContextMenu? DropDownMenu { get; set; }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.Handled)
                return;
            RaiseClick(EventArgs.Empty);
            if (DropDownMenu is null)
                return;
            e.Handled = true;
            PointD pt = (0, Bounds.Height);
            this.ShowPopupMenu(DropDownMenu, pt.X, pt.Y);
            Invalidate();
        }

        private void OnClickAction(object? sender, EventArgs? e)
        {
            clickAction?.Invoke();
        }
    }
}
