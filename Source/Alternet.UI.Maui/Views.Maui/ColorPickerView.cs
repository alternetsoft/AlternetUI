using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Controls;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a list of colors, from which the user can select a color.
    /// </summary>
    public partial class ColorPickerView : Picker
    {
        private readonly List<ColorPickerItem> colors = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerView"/> class.
        /// </summary>
        public ColorPickerView()
        {
            AddDefaultColors();
            ItemsSource = colors;
        }

        /// <summary>
        /// Gets or sets selected color.
        /// </summary>
        public virtual Color? SelectedColor
        {
            get
            {
                int selectedIndex = SelectedIndex;
                if (selectedIndex < 0)
                    return null;
                return colors[selectedIndex].Color;
            }

            set
            {
                if (value is null)
                {
                    SelectedIndex = -1;
                    return;
                }

                for (int i = 0; i < colors.Count; i++)
                {
                    if (colors[i].Color == value)
                    {
                        SelectedIndex = i;
                        return;
                    }
                }

                SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Clears list of the colors.
        /// </summary>
        public void ClearColors()
        {
            colors.Clear();
        }

        /// <summary>
        /// Adds color to the list of colors.
        /// </summary>
        /// <param name="color">Color to add.</param>
        public void AddColor(Color color)
        {
            ColorPickerItem controlItem = new(color);
            colors.Add(controlItem);
        }

        /// <summary>
        /// Adds default colors from the specified color categories.
        /// </summary>
        /// <param name="cats">Color categories.</param>
        public virtual void AddDefaultColors(KnownColorCategory[]? cats = null)
        {
            cats ??= [KnownColorCategory.Standard, KnownColorCategory.Web];

            var knownColors = Color.GetKnownColors(cats);

            foreach (var item in knownColors)
            {
                AddColor(item);
            }
        }

        private class ColorPickerItem
        {
            public Color Color;

            public ColorPickerItem(Color color)
            {
                Color = color;
            }

            public override string? ToString()
            {
                return Color.NameLocalized;
            }
        }
    }
}