using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the attributes of a calendar date.
    /// </summary>
    public partial class XCalendarDateAttr : ImmutableObject, IXCalendarDateAttr
    {
        private Color? textColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="XCalendarDateAttr"/> class.
        /// </summary>
        public XCalendarDateAttr()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XCalendarDateAttr"/> class with the specified text color.
        /// </summary>
        /// <param name="textColor">The text color for the calendar date.</param>
        public XCalendarDateAttr(Color textColor)
        {
            this.textColor = textColor;
        }

        /// <inheritdoc/>
        public virtual Color? TextColor
        {
            get => textColor;
            set
            {
                SetProperty(ref textColor, value);
            }
        }

        /// <inheritdoc/>
        public bool HasTextColor => TextColor != null;
    }
}