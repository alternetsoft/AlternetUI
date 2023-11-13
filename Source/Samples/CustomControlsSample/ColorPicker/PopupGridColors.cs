using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace CustomControlsSample
{
    public class PopupGridColors : PopupWindow
    {
        public Color Value { get; set; }

        private readonly Color[] colors = new[]
        {
            Color.IndianRed,
            Color.LightSalmon,
            Color.Firebrick,
            Color.DarkRed,

            Color.ForestGreen,
            Color.YellowGreen,
            Color.PaleGreen,
            Color.Olive,

            Color.PowderBlue,
            Color.DodgerBlue,
            Color.DarkBlue,
            Color.SteelBlue,

            Color.Silver,
            Color.LightSlateGray,
            Color.DarkSlateGray,
            Color.Black
        };

        /// <inheritdoc/>
        protected override Control CreateMainControl()
        {
            int RowCount = 4;
            int ColumnCount = 4;

            var grid = new Grid();

            for (int y = 0; y < ColumnCount; y++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int x = 0; x < RowCount; x++)
                grid.RowDefinitions.Add(new RowDefinition());

            int i = 0;
            for (int x = 0; x < RowCount; x++)
            {
                for (int y = 0; y < ColumnCount; y++)
                {
                    var button = new ColorButton { Value = colors[i++] };
                    button.Click += ColorButton_Click;
                    grid.Children.Add(button);
                    Grid.SetRow(button, y);
                    Grid.SetColumn(button, x);
                }
            }

            return grid;
        }

        private void ColorButton_Click(object? sender, EventArgs e)
        {
            Value = ((ColorPicker)sender!).Value;
            HidePopup(ModalResult.Accepted);
        }
    }
}
