using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IDateTimePickerHandler : IControlHandler
    {
        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder { get; set; }

        DateTimePickerPopupKind PopupKind { get; set; }

        DateTimePickerKind Kind { get; set; }

        void SetRange(DateTime min, DateTime max, bool useMin, bool useMax);
    }
}
