using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for dialog windows.
    /// </summary>
    internal class UIDialogWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIDialogWindow"/> class.
        /// </summary>
        public UIDialogWindow()
        {
            this.SuspendLayout();
            this.BeginIgnoreRecreate();
            try
            {
                ShowInTaskbar = false;
                MinimizeEnabled = false;
                MaximizeEnabled = false;
                StartLocation = WindowStartLocation.CenterScreen;
            }
            finally
            {
                this.EndIgnoreRecreate();
            }

            /* Size = new (600, 400);*/

            MainPanel.Children.Add(DataPanel);
            MainPanel.Children.Add(ButtonPanel);

            MainPanel.BackgroundColor = Color.White;
            DataPanel.BackgroundColor = Color.Yellow;
            ButtonPanel.BackgroundColor = Color.Red;

            Children.Add(MainPanel);
            this.ResumeLayout();
        }

        public Panel DataPanel { get; } = new()
        {
            Size = new(400, 300),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        public VerticalStackPanel MainPanel { get; } = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        /// <summary>
        /// Gets panel with Ok, Cancel and Apply buttons.
        /// </summary>
        public PanelOkCancelButtons ButtonPanel { get; } = new()
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
        };
    }
}
