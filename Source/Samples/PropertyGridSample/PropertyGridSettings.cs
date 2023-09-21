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
        private readonly PropertyGrid eventGrid;
        private readonly MainWindow mainWindow;
        private PropertyGridKnownColors colorScheme = PropertyGridKnownColors.White;

        public PropertyGridSettings(MainWindow window)
        {
            this.mainWindow = window;
            this.propertyGrid = window.PropGrid;
            this.eventGrid = window.panel.EventGrid;
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
                eventGrid.ApplyKnownColors(colorScheme);
            }
        }

        public bool LogPropertySelected { get; set; } = false;
        public bool LogPropertyChanged { get; set; } = true;
        public bool LogPropertyChanging { get; set; } = false;
        public bool LogPropertyHighlighted { get; set; } = false;
        public bool LogPropertyRightClick { get; set; } = false;
        public bool LogPropertyDoubleClick { get; set; } = false;
        public bool LogItemCollapsed { get; set; } = false;
        public bool LogItemExpanded { get; set; } = false;
        public bool LogLabelEditBegin { get; set; } = false;
        public bool LogLabelEditEnding { get; set; } = false;
        public bool LogColBeginDrag { get; set; } = false;
        public bool LogColDragging { get; set; } = false;
        public bool LogColEndDrag { get; set; } = false;
        public bool LogButtonClick { get; set; } = true;

        public PropertyGridCreateStyle CreateStyle
        {
            get
            {
                return propertyGrid.CreateStyle;
            }

            set
            {
                propertyGrid.CreateStyle = value;
                mainWindow.UpdatePropertyGrid();
            }
        }

        public PropertyGridCreateStyleEx CreateStyleEx
        {
            get
            {
                return propertyGrid.CreateStyleEx;
            }

            set
            {
                propertyGrid.CreateStyleEx = value;
                mainWindow.UpdatePropertyGrid();
            }
        }
    }
}