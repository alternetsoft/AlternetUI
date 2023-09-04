using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal class PropertyGridSettings
    {
        private readonly PropertyGrid propertyGrid;
        private readonly MainWindow mainWindow;
        private PropertyGridKnownColors colorScheme = PropertyGridKnownColors.White;

        public PropertyGridSettings(MainWindow window)
        {
            this.mainWindow = window;
            this.propertyGrid = window.propertyGrid;
        }

        public static PropertyGridSettings? Default { get; set; }

        public bool BoolAsCheckBox
        {
            get
            {
                return propertyGrid.BoolAsCheckBox;
            }

            set
            {
                propertyGrid.BoolAsCheckBox = value;
                mainWindow.UpdatePropertyGrid();
            }
        }

        public bool ColorHasAlpha
        {
            get
            {
                return propertyGrid.ColorHasAlpha;
            }

            set
            {
                propertyGrid.ColorHasAlpha = value;
                mainWindow.UpdatePropertyGrid();
            }
        }

        public PropertyGridKnownColors ColorScheme
        {
            get
            {
                return colorScheme;
            }

            set
            {
                if (colorScheme == value)
                    return;
                colorScheme = value;
                propertyGrid.ApplyKnownColors(colorScheme);
            }
        }
    }
}
