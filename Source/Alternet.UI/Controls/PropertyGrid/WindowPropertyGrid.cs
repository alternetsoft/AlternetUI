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
    public partial class WindowPropertyGrid : Window
    {
        private static WindowPropertyGrid? defaultWindow;

        private readonly PropertyGrid propGrid = new()
        {
            HasBorder = false,
        };

        private readonly VerticalStackPanel panel = new()
        {
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
            Size = (500, 800);
            StartLocation = WindowStartLocation.ScreenTopRight;
            panel.Parent = this;
            propGrid.VerticalAlignment = VerticalAlignment.Fill;
            propGrid.Parent = panel;
            propGrid.SuggestedInitDefaults();
            propGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAllAfterSetValue;
            Disposed += WindowPropertyGrid_Disposed;
        }

        /// <summary>
        /// Gets default <see cref="WindowPropertyGrid"/>.
        /// </summary>
        public static new WindowPropertyGrid? Default
        {
            get
            {
                return defaultWindow;
            }
        }

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
            defaultWindow.Unbind();

            defaultWindow.Title = title ?? CommonStrings.Default.WindowTitleProperties;
            defaultWindow.PropGrid.SetProps(instance, true);

            if(instance is IDisposableObject disposable)
            {
                disposable.Disposed += defaultWindow.Disposable_Disposed;
            }

            defaultWindow.ShowAndFocus();
            return defaultWindow;
        }

        private object? GetPropGridItemInstance()
        {
            var first = PropGrid.Items.FirstOrDefault();
            var result = first?.Instance;
            return result;
        }

        private void Disposable_Disposed(object? sender, EventArgs e)
        {
            Unbind();
        }

        private void Unbind()
        {
            var instance = GetPropGridItemInstance();
            if (instance is IDisposableObject disposable)
            {
                disposable.Disposed -= Disposable_Disposed;
            }

            PropGrid.Clear();
        }

        private void WindowPropertyGrid_Disposed(object? sender, EventArgs e)
        {
            Unbind();
            if (defaultWindow == this)
                defaultWindow = null;
        }
    }
}
