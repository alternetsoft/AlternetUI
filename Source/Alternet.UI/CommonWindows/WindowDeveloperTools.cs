using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Developer tools window with additional debug related features.
    /// </summary>
    internal class WindowDeveloperTools : Window
    {
        private readonly PanelDeveloperTools panel = new()
        {
            SuggestedSize = new(800, 600),
        };

        public WindowDeveloperTools()
            : base()
        {
            StartLocation = WindowStartLocation.CenterScreen;
            Title = "Developer Tools";
            panel.Parent = this;
            panel.Update();
            SetSizeToContent();
        }
    }
}
