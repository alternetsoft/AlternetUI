using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains default property values for the control.
    /// </summary>
    public class DefaultPropsControl
    {
        private readonly object?[] props = new object[(int)AllControlProps.MaxValue + 1];

        static DefaultPropsControl()
        {
        }

        public Thickness? MinMargin
        {
            get => (Thickness?)GetProp(AllControlProps.MinMargin);
            set => SetProp(AllControlProps.MinMargin, value);
        }

        public Thickness? MinPadding
        {
            get => (Thickness?)GetProp(AllControlProps.MinPadding);
            set => SetProp(AllControlProps.MinPadding, value);
        }

        public object? GetProp(AllControlProps prop)
        {
            return props[(int)prop];
        }

        public void SetProp(AllControlProps prop, object? value)
        {
            props[(int)prop] = value;
        }
    }
}