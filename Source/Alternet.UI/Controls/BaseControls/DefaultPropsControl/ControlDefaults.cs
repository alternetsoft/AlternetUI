using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains default property values for the control.
    /// </summary>
    public class ControlDefaults
    {
        private readonly object?[] props =
            new object[(int)ControlDefaultsId.MaxValue + 1];

        static ControlDefaults()
        {
        }

        /// <summary>
        /// Occurs each time when new control is created.
        /// </summary>
        /// <remarks>
        /// In the <see cref="InitDefaults"/> event handler you can implement custom control
        /// initialization. Use <see cref="Control.GetDefaults"/> to get
        /// defaults for the specific control on the current platform.
        /// </remarks>
        public event EventHandler? InitDefaults;

        /// <summary>
        /// Gets or sets minimal margin value.
        /// </summary>
        public Thickness? MinMargin
        {
            get => (Thickness?)GetProp(ControlDefaultsId.MinMargin);
            set => SetProp(ControlDefaultsId.MinMargin, value);
        }

        /// <summary>
        /// Gets or sets minimal padding value.
        /// </summary>
        public Thickness? MinPadding
        {
            get => (Thickness?)GetProp(ControlDefaultsId.MinPadding);
            set => SetProp(ControlDefaultsId.MinPadding, value);
        }

        /// <summary>
        /// Gets or sets whether control has border when color scheme is white.
        /// </summary>
        public bool? HasBorderOnWhite
        {
            get => (bool?)GetProp(ControlDefaultsId.HasBorderOnWhite);
            set => SetProp(ControlDefaultsId.HasBorderOnWhite, value);
        }

        /// <summary>
        /// Gets or sets whether control has border when color scheme is black.
        /// </summary>
        public bool? HasBorderOnBlack
        {
            get => (bool?)GetProp(ControlDefaultsId.HasBorderOnBlack);
            set => SetProp(ControlDefaultsId.HasBorderOnBlack, value);
        }

        /// <summary>
        /// Gets default property value.
        /// </summary>
        /// <param name="prop">Property identifier.</param>
        /// <returns></returns>
        public object? GetProp(ControlDefaultsId prop)
        {
            return props[(int)prop];
        }

        /// <summary>
        /// Sets default property value.
        /// </summary>
        /// <param name="prop">Property identifier.</param>
        /// <param name="value">New property value.</param>
        public void SetProp(ControlDefaultsId prop, object? value)
        {
            props[(int)prop] = value;
        }

        /// <summary>
        /// Raises <see cref="InitDefaults"/> event.
        /// </summary>
        /// <param name="control"></param>
        public void RaiseInitDefaults(Control control) => InitDefaults?.Invoke(control, EventArgs.Empty);
    }
}