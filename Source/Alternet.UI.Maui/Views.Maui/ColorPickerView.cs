using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Displays a list of colors, from which the user can select a color.
    /// </summary>
    public partial class ColorPickerView : BasePickerView
    {
        private readonly ObservableCollection<ColorPickerItem> colors = new();

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
                var newIndex = GetColorIndex(value);
                SelectedIndex = newIndex;
            }
        }

        /// <summary>
        /// Adds the specified color to the list if it is not already present.
        /// </summary>
        /// <param name="color">The color to add.</param>
        /// <returns>The index of the color in the list.</returns>
        public virtual int AddIfMissing(Color? color)
        {
            if (color is null)
            {
                return -1;
            }

            var index = GetColorIndex(color);
            if (index < 0)
                index = AddColor(color);

            return index;
        }

        /// <summary>
        /// Ensures the specified color is added to the list and selects it.
        /// </summary>
        /// <param name="color">The color to add and select.</param>
        public virtual void EnsureAddedAndSelect(Color? color)
        {
            var index = AddIfMissing(color);
            if (index >= 0)
                SelectedIndex = index;
        }

        /// <summary>
        /// Gets the index of the specified color in the list of colors.
        /// </summary>
        /// <param name="color">The color to find the index of.</param>
        /// <returns>The index of the specified color, or -1 if the color is not found.</returns>
        public virtual int GetColorIndex(Color? color)
        {
            if (color is null)
                return -1;

            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i].Color.AsStruct == color.AsStruct)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Clears list of the colors.
        /// </summary>
        public virtual void ClearColors()
        {
            colors.Clear();
        }

        /// <summary>
        /// Adds color to the list of colors.
        /// </summary>
        /// <param name="color">Color to add.</param>
        public virtual int AddColor(Color color)
        {
            ColorPickerItem controlItem = new(color);
            colors.Add(controlItem);
            return colors.Count - 1;
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
                return Color.ToDisplayString();
            }
        }
    }
}