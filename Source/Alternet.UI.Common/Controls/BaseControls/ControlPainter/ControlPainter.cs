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
            get => handler ??= BaseApplication.Handler.CreateControlPainterHandler();
            set => handler = value;
        }

        internal static void LogPartSize(Control control)
        {
            BaseApplication.Log($"CheckMarkSize: {Handler.GetCheckMarkSize(control)}");
            BaseApplication.Log($"CheckBoxSize(0): {Handler.GetCheckBoxSize(control)}");
            BaseApplication.Log($"GetExpanderSize: {Handler.GetExpanderSize(control)}");
            BaseApplication.Log($"GetHeaderButtonHeight: {Handler.GetHeaderButtonHeight(control)}");
            BaseApplication.Log($"GetHeaderButtonMargin: {Handler.GetHeaderButtonMargin(control)}");
        }
    }
}
