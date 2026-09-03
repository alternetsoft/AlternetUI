using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Label"/> and <see cref="MainControl"/> properties.
    /// </summary>
    public interface IControlAndLabel
    {
        /// <summary>
        /// Gets label control.
        /// </summary>
        AbstractControl Label { get; }

        /// <summary>
        /// Gets main control.
        /// </summary>
        AbstractControl MainControl { get; }

        /// <summary>
        /// Executes action with label control.
        /// </summary>
        /// <typeparam name="TControl">Type of the label control.</typeparam>
        /// <param name="action">Action to execute with the label control.</param>
        void WithLabel<TControl>(Action<TControl>? action = null)
        {
            if (Label is TControl control)
            {
                action?.Invoke(control);
            }
        }

        /// <summary>
        /// Executes action with main control.
        /// </summary>
        /// <typeparam name="TControl">Type of the main control.</typeparam>
        /// <param name="action">Action to execute with the main control.</param>
        void WithMainControl<TControl>(Action<TControl>? action = null)
        {
            if (MainControl is TControl control)
            {
                action?.Invoke(control);
            }
        }
    }
}
