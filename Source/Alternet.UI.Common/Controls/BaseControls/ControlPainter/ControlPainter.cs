using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base class for the control painters.
    /// </summary>
    public abstract class ControlPainter : DisposableObject
    {
        private static IControlPainterHandler? handler;

        /// <summary>
        /// Gets or sets current control painter handler.
        /// </summary>
        public static IControlPainterHandler Handler
        {
            get => handler ??= App.Handler.CreateControlPainterHandler();
            set => handler = value;
        }

        internal static void LogPartSize(Control control)
        {
            App.Log($"CheckMarkSize: {Handler.GetCheckMarkSize(control)}");
            App.Log($"CheckBoxSize(0): {Handler.GetCheckBoxSize(control)}");
            App.Log($"GetExpanderSize: {Handler.GetExpanderSize(control)}");
            App.Log($"GetHeaderButtonHeight: {Handler.GetHeaderButtonHeight(control)}");
            App.Log($"GetHeaderButtonMargin: {Handler.GetHeaderButtonMargin(control)}");
        }
    }
}
