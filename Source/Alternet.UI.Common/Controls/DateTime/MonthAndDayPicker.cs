using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a month and day.
    /// </summary>
    public partial class MonthAndDayPicker : TransparentPanel
    {
        /// <summary>
        /// The default margin between the label and the picker control.
        /// </summary>
        public static float DefaultLabelAndPickerMargin = 5;

        private readonly MonthPicker monthPicker = new();
        private readonly XIntPicker dayPicker = new();
        private readonly Label label = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthAndDayPicker"/> class.
        /// </summary>
        public MonthAndDayPicker()
        {
            Layout = LayoutStyle.Horizontal;

            label.Visible = false;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.MarginRight = DefaultLabelAndPickerMargin;
            label.Parent = this;

            monthPicker.Parent = this;

            dayPicker.Minimum = 1;
            dayPicker.Maximum = 31;
            dayPicker.Parent = this;
        }

        /// <summary>
        /// Gets the <see cref="MonthPicker"/> control used for selecting the month.
        /// </summary>
        public MonthPicker MonthPicker => monthPicker;

        /// <summary>
        /// Gets the <see cref="XIntPicker"/> control used for selecting the day.
        /// </summary>
        public XIntPicker DayPicker => dayPicker;

        /// <summary>
        /// Gets the <see cref="Label"/> control used for displaying a label for the picker.
        /// </summary>
        public Label Label => label;
    }
}
