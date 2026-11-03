using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows the user to select a calendar alarm.
    /// </summary>
    public partial class CalendarAlarmPicker : TransparentPanel
    {
        private readonly CalendarAlarmInfo data;
        private readonly FileNamePicker soundFilePicker = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarAlarmPicker"/> class
        /// with the specified <see cref="CalendarAlarmInfo"/> data.
        /// </summary>
        /// <param name="data"></param>
        public CalendarAlarmPicker(CalendarAlarmInfo data)
        {
            this.data = data;
            data.PropertyChanged += (s, e) => OnValueChanged(e);
            soundFilePicker.OpenFileProps.Filter = FileMaskUtils.FileDialogFilterSoundFiles;
            soundFilePicker.OpenFileProps.Title = CommonStrings.Default.WindowTitleSelectSoundFile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarAlarmPicker"/> class.
        /// </summary>
        public CalendarAlarmPicker()
            : this(new ())
        {
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property changes.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the <see cref="CalendarAlarmInfo"/> data of the <see cref="CalendarAlarmPicker"/> control.
        /// </summary>
        public virtual CalendarAlarmInfo Value
        {
            get => data;
            set
            {
                data.Assign(value);
            }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event when the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnValueChanged(PropertyChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
