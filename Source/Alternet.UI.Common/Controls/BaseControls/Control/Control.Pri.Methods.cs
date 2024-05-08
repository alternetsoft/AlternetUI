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
        private void CreateAndAttachHandler()
        {
            if (GetRequiredHandlerType() == HandlerType.Native)
                handler = CreateHandler();
            else
                handler = GetNative().CreateControlHandler(this);

            handler?.Attach(this);
            OnHandlerAttached(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="TitleChanged"/> event and calls
        /// <see cref="OnTitleChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void RaiseTitleChanged(EventArgs e)
        {
            OnTitleChanged(e);
            TitleChanged?.Invoke(this, e);
            Parent?.OnChildPropertyChanged(this, nameof(Title));
        }

        private void Children_ItemInserted(object? sender, int index, Control item)
        {
            item.SetParentInternal(this);
            Handler.RaiseChildInserted(item);
            RaiseLayoutChanged();
            PerformLayout();
        }

        private void Children_ItemRemoved(object? sender, int index, Control item)
        {
            item.SetParentInternal(null);
            Handler.RaiseChildRemoved(item);
            RaiseLayoutChanged();
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
                        GetNative().ResetBackgroundColor(this);
                    else
                        GetNative().SetBackgroundColor(this, color);
                }
                else
                {
                    foregroundColor = color;
                    if (color is null)
                        GetNative().ResetForegroundColor(this);
                    else
                        GetNative().SetForegroundColor(this, color);
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
            OnEnabledChanged(e);
            EnabledChanged?.Invoke(this, e);
            GetNative().SetEnabled(this, Enabled);
            Parent?.OnChildPropertyChanged(this, nameof(Enabled));
        }
    }
}
