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
        private protected override bool GetIsEnabled() => Enabled;

        private protected void SetVisibleValue(bool value) => visible = value;

        /// <summary>
        /// Callback for changes to the Enabled property
        /// </summary>
        private static void OnEnabledPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Control control = (Control)d;
            control?.OnEnabledPropertyChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        private void CreateAndAttachHandler()
        {
            handler = CreateHandler();
            handler?.Attach(this);
            OnHandlerAttached(EventArgs.Empty);
        }

        private void Children_ItemInserted(object? sender, int index, Control item)
        {
            item.SetParentInternal(this);
        }

        private void Children_ItemRemoved(object? sender, int index, Control item)
        {
            item.SetParentInternal(null);
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
                        NativeControl.BackgroundColor = color.Value;
                }
                else
                {
                    foregroundColor = color;
                    if (color is null)
                        NativeControl.ResetForegroundColor();
                    else
                        NativeControl.ForegroundColor = color.Value;
                }
            }

            if (method == ResetColorType.Auto)
                SetColor(null);
            else
            {
                var colors = FontAndColor.GetResetColors(method, this);
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
        }

#pragma warning disable
        private void OnEnabledPropertyChanged(bool oldValue, bool newValue)
#pragma warning restore
        {
            RaiseEnabledChanged(EventArgs.Empty);
        }
    }
}
