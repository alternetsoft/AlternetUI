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
        private protected void SetVisibleValue(bool value) => visible = value;

        private void CreateAndAttachHandler()
        {
            if (GetRequiredHandlerType() == HandlerType.Native)
                handler = CreateHandler();
            else
                handler = new GenericControlHandler();

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
            Handler.Control_Children_ItemInserted(item);
        }

        private void Children_ItemRemoved(object? sender, int index, Control item)
        {
            item.SetParentInternal(null);
            Handler.Control_Children_ItemRemoved(item);
        }

        private void ResetColor(bool isBackground, ResetColorType method = ResetColorType.Auto)
        {
            if (NativeControl is null)
                return;

            void SetColor(Color? color)
            {
                if (isBackground)
                {
                    backgroundColor = color;
                    if (color is null)
                        NativeControl.ResetBackgroundColor();
                    else
                        NativeControl.BackgroundColor = color;
                }
                else
                {
                    foregroundColor = color;
                    if (color is null)
                        NativeControl.ResetForegroundColor();
                    else
                        NativeControl.ForegroundColor = color;
                }
            }

            if (method == ResetColorType.Auto)
                SetColor(null);
            else
            {
                var colors = ControlUtils.GetResetColors(method, this);
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
            handler?.Control_EnabledChanged();
            Parent?.OnChildPropertyChanged(this, nameof(Enabled));
        }
    }
}
