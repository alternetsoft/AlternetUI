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
    /// Popup window with <see cref="Calendar"/> control.
    /// </summary>
    public class PopupCalendar : PopupWindow<Calendar>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCalendar"/> class.
        /// </summary>
        public PopupCalendar()
        {
            HideOnClick = false;
            HideOnDoubleClick = true;
            MainControl.RecreateWindow();
            MainControl.PerformLayout();
            MainControl.BackgroundColor = SystemColors.Window;
        }

        /// <inheritdoc/>
        protected override Calendar CreateMainControl()
        {
            return new Calendar()
            {
                HasBorder = false,
            };
        }

        /// <inheritdoc/>
        protected override bool HideOnClickPoint(PointD point)
        {
            var result = MainControl.HitTest(point) == Calendar.HitTestResult.Day;
            return result;
        }
    }
}
