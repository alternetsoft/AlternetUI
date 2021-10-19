using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Draws a border, background, or both around another control.
    /// </summary>
    public class Border : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border()
        {
            BorderBrush = Brushes.Gray;
            Padding = new Thickness(2);
        }
    }
}