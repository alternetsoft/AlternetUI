using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with date/time picker.
    /// </summary>
    public interface IDateTimePickerHandler
    {
        /// <inheritdoc cref="DateTimePicker.PopupKind"/>
        DateTimePickerPopupKind PopupKind { get; set; }

        /// <inheritdoc cref="DateTimePicker.Kind"/>
        DateTimePickerKind Kind { get; set; }

        /// <summary>
        /// Sets date range.
        /// </summary>
        /// <param name="min">Minimal date.</param>
        /// <param name="max">Maximal date.</param>
        /// <param name="useMin">Use minimal date.</param>
        /// <param name="useMax">use maximal date.</param>
        void SetRange(DateTime min, DateTime max, bool useMin, bool useMax);
    }
}
