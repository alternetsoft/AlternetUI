using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="Calendar"/> control.
    /// </summary>
    public class PopupCalendar : PopupWindow
    {
        /// <summary>
        /// Gets or sets <see cref="Calendar"/> control used in the popup window.
        /// </summary>
        [Browsable(false)]
        public new Calendar MainControl
        {
            get => (Calendar)base.MainControl;
            set => base.MainControl = value;
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl()
        {
            return new Calendar()
            {
                HasBorder = false,
            };
        }
    }
}
