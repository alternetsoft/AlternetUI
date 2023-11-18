using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsTest
{
    internal class PanelLinkLabelsPage : Control
    {
        private readonly PanelLinkLabels linkLabels = new();

        public PanelLinkLabelsPage()
        {
            EmptyWindow window = new();

            linkLabels.Parent = window;

            for (int i = 0; i < 50; i++)
            {
                linkLabels.Add($"Action {i}", () =>
                {
                    Application.Log($"Action {i} executed.");
                });
            }

            window.Show();
        }
    }
}
