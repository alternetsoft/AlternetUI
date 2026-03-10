using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a window that displays tooltips in the application, ensuring a single instance is used throughout.
    /// </summary>
    /// <remarks>Use this class to provide consistent tooltip display functionality across the application.
    /// The singleton instance is created on first access, which enables lazy initialization. Thread safety is not
    /// guaranteed unless the underlying assignment operation is thread-safe.</remarks>
    public partial class ToolTipWindow : Window, IToolTipProvider
    {
        private static ToolTipWindow? instance;

        private readonly RichToolTip toolTip = new();

        /// <summary>
        /// Initializes a new instance of the ToolTipWindow class.
        /// </summary>
        public ToolTipWindow()
        {
            toolTip.ToolTipVisibleChanged += (s, e) =>
            {
                if (toolTip.ToolTipVisible)
                {
                    ClientSize = toolTip.LayoutMaxSize ?? SizeD.Empty;

                    var location = toolTip.ToolTipLocation;

                    if (location is null || toolTip.ToolTipOwner is not AbstractControl locationOwner)
                    {
                        var mousePos = Mouse.GetPosition(this.ScaleFactor);

                        Location = mousePos + new PointD(ToolTipFactory.OverlayToolTipOffset);
                    }
                    else
                    {
                        Location = locationOwner.PointToScreen(location.Value);
                    }

                    Show();
                }
                else
                {
                    Hide();
                }
            };

            ShowInTaskbar = false;
            Resizable = false;
            HasTitleBar = false;
            MaximizeEnabled = false;
            MinimizeEnabled = false;
            HasSystemMenu = false;
            CloseEnabled = false;
            TopMost = true;
            StartLocation = WindowStartLocation.Manual;

            toolTip.Parent = this;

            this.Deactivated += (s, e) =>
            {
                Hide();
            };
        }

        /// <summary>
        /// Gets the singleton instance of the ToolTipWindow class.
        /// </summary>
        /// <remarks>This property ensures that only one instance of ToolTipWindow is created and reused
        /// throughout the application. The instance is initialized on first access, providing a lazy-loading mechanism.
        /// This property is thread-safe only if the underlying implementation of the null-coalescing assignment is
        /// thread-safe.</remarks>
        public static ToolTipWindow Instance
        {
            get
            {
                return instance ??= new ToolTipWindow();
            }

            set
            {
                instance = value;
            }
        }

        /// <inheritdoc/>
        IRichToolTip? IToolTipProvider.Get(object? sender)
        {
            toolTip.ToolTipOwner = sender;
            return toolTip;
        }
    }
}
