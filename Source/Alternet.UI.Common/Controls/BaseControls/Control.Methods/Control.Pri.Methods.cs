using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        private void Children_ItemInserted(object? sender, int index, Control item)
        {
            item.SetParentInternal(this);
            RaiseChildInserted(index, item);
            PerformLayout();
        }

        private void Children_ItemRemoved(object? sender, int index, Control item)
        {
            item.SetParentInternal(null);
            RaiseChildRemoved(item);
            PerformLayout();
        }

        private void ResetColor(bool isBackground, ResetColorType method = ResetColorType.Auto)
        {
            void SetColor(Color? color)
            {
                if (isBackground)
                {
                    backgroundColor = color;
                    if (color is null)
                        Handler.ResetBackgroundColor();
                    else
                        Handler.BackgroundColor = color;
                }
                else
                {
                    foregroundColor = color;
                    if (color is null)
                        Handler.ResetForegroundColor();
                    else
                        Handler.ForegroundColor = color;
                }
            }

            if (method == ResetColorType.Auto)
                SetColor(null);
            else
            {
                var colors = ColorUtils.GetResetColors(method, this);
                var color = isBackground ? colors?.BackgroundColor : colors?.ForegroundColor;
                SetColor(color);
            }

            Refresh();
        }

        /// <summary>
        /// Raises the <see cref="EnabledChanged"/> event and calls
        /// <see cref="OnEnabledChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        private void RaiseEnabledChanged(EventArgs e)
        {
            RaiseVisualStateChanged();
            OnEnabledChanged(e);
            EnabledChanged?.Invoke(this, e);
            Handler.SetEnabled(Enabled);
            Parent?.OnChildPropertyChanged(this, nameof(Enabled));
        }
    }
}
