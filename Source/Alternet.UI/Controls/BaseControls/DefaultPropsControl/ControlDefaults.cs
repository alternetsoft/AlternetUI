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
        /// Returns minimal margin value.
        /// </summary>
        public Thickness? MinMargin
        {
            get => (Thickness?)GetProp(ControlDefaultsId.MinMargin);
            set => SetProp(ControlDefaultsId.MinMargin, value);
        }

        /// <summary>
        /// Returns minimal padding value.
        /// </summary>
        public Thickness? MinPadding
        {
            get => (Thickness?)GetProp(ControlDefaultsId.MinPadding);
            set => SetProp(ControlDefaultsId.MinPadding, value);
        }

        /// <summary>
        /// Returns default property value.
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