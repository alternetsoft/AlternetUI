using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a panel which contains <see cref="PropertyGrid"/> and other controls.
    /// </summary>
    public partial class SplittedPropertyGridPanel : SplittedControlsPanel
    {
        private PropertyGrid? propertyGrid;

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> which can be used to show properties.
        /// </summary>
        [Browsable(false)]
        public virtual PropertyGrid PropGrid
        {
            get
            {
                if (propertyGrid == null)
                {
                    propertyGrid = new()
                    {
                        HasBorder = false,
                        VerticalAlignment = UI.VerticalAlignment.Fill,
                        Visible = false,
                    };

                    RightPanel.Add(
                        CommonStrings.Default.WindowTitleProperties,
                        propertyGrid);
                    RightPanel.SelectFirstTab();
                }

                return propertyGrid;
            }
        }
    }
}
