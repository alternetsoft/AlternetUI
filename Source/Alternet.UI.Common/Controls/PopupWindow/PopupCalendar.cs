using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="Calendar"/> control.
    /// </summary>
    public partial class PopupCalendar : PopupWindow<Calendar>
    {
        private static PopupCalendar? defaultCalendar;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCalendar"/> class.
        /// </summary>
        public PopupCalendar()
        {
            Title = CommonStrings.Default.WindowTitleSelectDate;
            HideOnClick = false;
            HideOnDoubleClick = true;
            SetSizeToContent();
        }

        /// <summary>
        /// Gets or sets default instance of the <see cref="PopupCalendar"/>.
        /// </summary>
        public static new PopupCalendar Default
        {
            get
            {
                if (defaultCalendar == null)
                {
                    defaultCalendar = new PopupCalendar();
                }

                return defaultCalendar;
            }

            set
            {
                defaultCalendar = value;
            }
        }

        /// <inheritdoc/>
        protected override Calendar CreateMainControl()
        {
            var result = new Calendar()
            {
                HasBorder = false,
            };

            return result;
        }

        /// <inheritdoc/>
        protected override bool HideOnClickPoint(PointD point)
        {
            var result = MainControl.HitTest(point) == Calendar.HitTestResult.Day;
            return result;
        }
    }
}
