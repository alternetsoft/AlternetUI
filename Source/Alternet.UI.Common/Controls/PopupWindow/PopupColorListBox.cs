using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="ColorListBox"/> control.
    /// </summary>
    public class PopupColorListBox : PopupListBox<ColorListBox>
    {
        /// <summary>
        /// Gets popup result as <see cref="Color"/>.
        /// </summary>
        public Color? ResultValue
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
