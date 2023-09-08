using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for dialog windows.
    /// </summary>
    public class UIDialogWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIDialogWindow"/> class.
        /// </summary>
        public UIDialogWindow()
        {
            ShowInTaskbar = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            StartLocation = WindowStartLocation.CenterScreen;
            Size = new (600, 400);

            ButtonPanel.Children.Add(OkButton);
            ButtonPanel.Children.Add(CancelButton);
            ButtonPanel.Children.Add(ApplyButton);

            LayoutPanel.SetDock(ButtonPanel, DockStyle.Bottom);
            LayoutPanel.SetDock(DataPanel, DockStyle.Fill);
            Children.Add(MainPanel);
            MainPanel.Children.Add(DataPanel);
            MainPanel.Children.Add(ButtonPanel);
        }

        /// <summary>
        /// Gets or sets default button margin for the Ok, Cancel and Apply buttons.
        /// </summary>
        public static Thickness DefaultButtonMargin { get; set; } = new(5);

        public Panel DataPanel { get; } = new()
        {
        };

        public LayoutPanel MainPanel { get; } = new()
        {
        };

        /// <summary>
        /// Gets Ok button.
        /// </summary>
        public Button OkButton { get; } = new()
        {
            Text = CommonStrings.Default.ButtonOk,
            Margin = DefaultButtonMargin,
            IsDefault = true,
        };

        /// <summary>
        /// Gets Cancel button.
        /// </summary>
        public Button CancelButton { get; } = new()
        {
            Text = CommonStrings.Default.ButtonCancel,
            Margin = DefaultButtonMargin,
            IsCancel = true,
        };

        /// <summary>
        /// Gets Apply button.
        /// </summary>
        public Button ApplyButton { get; } = new()
        {
            Margin = DefaultButtonMargin,
            Text = CommonStrings.Default.ButtonApply,
        };

        /// <summary>
        /// Gets panel with Ok, Cancel and Apply buttons.
        /// </summary>
        public HorizontalStackPanel ButtonPanel { get; } = new()
        {
        };
    }
}
