using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="Window"/> with <see cref="PropertyGrid"/> control inside.
    /// </summary>
    public class WindowPropertyGrid : Window
    {
        private static WindowPropertyGrid? defaultWindow;

        private readonly PropertyGrid propGrid = new()
        {
            HasBorder = false,
        };

        private readonly VerticalStackPanel panel = new()
        {
            AllowStretch = true,
        };

        static WindowPropertyGrid()
        {
            PropertyGrid.RegisterCollectionEditors();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowPropertyGrid"/> class.
        /// </summary>
        public WindowPropertyGrid()
        {
            Size = (800, 600);
            panel.Parent = this;
            propGrid.Parent = panel;
            propGrid.SuggestedInitDefaults();
            propGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAllAfterSetValue;
            Disposed += WindowPropertyGrid_Disposed;
        }

        /// <summary>
        /// Gets default <see cref="WindowPropertyGrid"/>.
        /// </summary>
        public static WindowPropertyGrid? Default => defaultWindow;

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> control.
        /// </summary>
        public PropertyGrid PropGrid => propGrid;

        /// <summary>
        /// Shows default <see cref="WindowPropertyGrid"/>.
        /// </summary>
        /// <param name="title">Title of the window.</param>
        /// <param name="instance">Object to show in property grid.</param>
        /// <param name="sort">Whether to sort properties.</param>
        /// <returns></returns>
        public static WindowPropertyGrid ShowDefault(
            string? title,
            object? instance,
            bool sort = false)
        {
            defaultWindow ??= new();
            defaultWindow.Title = title ?? CommonStrings.Default.WindowTitleProperties;
            defaultWindow.PropGrid.SetProps(instance, true);
            defaultWindow.Show();
            return defaultWindow;
        }

        private void WindowPropertyGrid_Disposed(object sender, EventArgs e)
        {
            if (defaultWindow == this)
                defaultWindow = null;
        }
    }
}
