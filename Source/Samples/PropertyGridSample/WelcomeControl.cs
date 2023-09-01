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
        private const string descText = "Specialized grid for editing properties.";
        private readonly Label desc = new()
        {
            Text = descText,
        };

        public WelcomeControl()
        {
            VerticalStackPanel stackPanel = new()
            {
                Padding = new(10),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Children.Add(stackPanel);

            Label header = new()
            {
                Text = "PropertyGrid",
                Font = new Font(Font.Default.Name, Font.Default.SizeInPoints * 2, FontStyle.Bold)
            };
            stackPanel.Children.Add(header);

            stackPanel.Children.Add(desc);

        }

    }
}
