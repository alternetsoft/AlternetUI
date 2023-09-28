using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    internal class WelcomeControl : Control
    {
        private readonly VerticalStackPanel stackPanel = new()
        {
            Padding = new(10),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        private const string descText = "Specialized grid for editing properties.";
        private readonly Label header = new()
        {
            Text = "PropertyGrid",
            Font = Font.Default.AsBold,
        };
        private readonly Label desc = new()
        {
            Text = descText,
            Margin = new(0,5,0,15),
        };

        public WelcomeControl()
        {
            DoInsideLayout(() =>
            {
                Children.Add(stackPanel);
                stackPanel.Children.Add(header);
                stackPanel.Children.Add(desc);
            });
        }
    }
}
