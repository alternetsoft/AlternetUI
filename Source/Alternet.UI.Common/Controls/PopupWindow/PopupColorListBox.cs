using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="ColorListBox"/> control.
    /// </summary>
    public partial class PopupColorListBox : PopupListBox<ColorListBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupColorListBox"/> class.
        /// </summary>
        public PopupColorListBox()
        {
            Title = CommonStrings.Default.WindowTitleSelectColor;
        }

        /// <summary>
        /// Gets popup result as <see cref="Color"/>.
        /// </summary>
        public virtual Color? ResultValue
        {
            get
            {
                if (ResultIndex is null)
                    return null;

                var color = ColorListBox.GetItemValueOrDefault(
                    MainControl,
                    ResultIndex.Value,
                    Color.Empty);

                return color;
            }
        }
    }
}
