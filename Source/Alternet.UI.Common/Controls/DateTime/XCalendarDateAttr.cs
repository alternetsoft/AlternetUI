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
        private BorderSettings? border;

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

        /// <summary>
        /// Gets or sets the border color assigned for the calendar date.
        /// </summary>
        public virtual Color? BorderColor
        {
            get
            {
                return Border?.Color;
            }

            set
            {
                if (value is null)
                {
                    Border = null;
                    return;
                }

                if (Border == null)
                {
                    Border = new BorderSettings(value);
                }
                else
                {
                    Border.Color = value;
                }
            }
        }

        /// <inheritdoc/>
        public virtual BorderSettings? Border
        {
            get => border;
            set
            {
                SetProperty(ref border, value);
            }
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

        /// <inheritdoc/>
        public bool HasBorderColor => BorderColor != null;
    }
}