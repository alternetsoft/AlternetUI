using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a repeat pattern for an event or task.
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class RepeatPatternPicker : HiddenBorder
    {
        /// <summary>
        /// Gets or sets the default minimum margin between child controls
        /// in the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public static Thickness DefaultMinChildMargin = 5;

        /// <summary>
        /// Gets or sets a value indicating whether the drop-down image for
        /// the combo controls is shown by default.
        /// </summary>
        public static bool DefaultShowDropDownImage = false;

        /// <summary>
        /// Gets or sets the default time unit text format used for displaying durations
        /// in the duration label of the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public static DateUtils.TimePeriodUnitTextFormat DefaultTimeUnitTextFormat
            = DateUtils.TimePeriodUnitTextFormat.Short;

        /// <summary>
        /// Gets or sets the default date format used for displaying dates
        /// in the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public static string DefaultDateFormat = "D";

        /// <summary>
        /// Gets the default padding for the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public static readonly Thickness DefaultPadding = 5;

        private readonly LinkLabel editExcludedDatesLinkLabel = new(Localized.CalendarItemLabelEditExcludedDates);
        private readonly GenericControlAndImage<DatePicker> startPicker = new();
        private readonly RoundBorderWithTitle tabControlPanel = new(CommonStrings.Default.SeriesPattern);      
        private readonly GenericRoundBorderWithTitle rangePanel = new(CommonStrings.Default.SeriesRange);
        private readonly TransparentPanel endDatePanel = new ();
        private readonly GenericControlAndLabel<DatePicker, Label> untilDatePicker = new();
        private readonly XRadioButtonAndSuffix endsNeverRadioButton;
        private readonly XRadioButtonAndSuffix untilRadioButton;
        private readonly XRadioButtonAndSuffix endsAfterOccurrenceRadioButton;
        private readonly XIntPickerWithLabels occurrencePicker = new();

        private readonly TabControl tabControl = new();
        private readonly DailyPatternPicker dailyPicker;
        private readonly WeeklyPatternPicker weeklyPicker;
        private readonly MonthlyPatternPicker monthlyPicker;
        private readonly YearlyPatternPicker yearlyPicker;
        private readonly HiddenBorder nonePicker = new();
        private readonly RepeatPatternRule data;

        private IFormatProvider? formatProvider;
        private int valueToControlsSuspendCounter;
        private bool valuesTransparent = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPatternPicker"/> class.
        /// </summary>
        public RepeatPatternPicker()
        {
            Layout = LayoutStyle.Vertical;
            Padding = DefaultPadding;
            MinChildMargin = DefaultMinChildMargin;

            data = CreateRule();

            // Start on date

            startPicker.Parent = rangePanel;

            // End on date

            var endDateControlsPanel = new TransparentPanel();
            endDateControlsPanel.Layout = LayoutStyle.Vertical;

            untilDatePicker.Label.Text = Localized.UntilDatePrefix;
            untilDatePicker.Label.InputTransparent = true;
            untilDatePicker.MainControl.ImageVisible = DefaultShowDropDownImage;
            
            untilRadioButton = new(untilDatePicker);
            untilRadioButton.Parent = endDateControlsPanel;
            untilDatePicker.Click += (s, e) => untilRadioButton.IsChecked = true;

            // Ends after occurrence

            occurrencePicker.PrefixText = Localized.After;

            endsAfterOccurrenceRadioButton = new(occurrencePicker);
            endsAfterOccurrenceRadioButton.MarginTop = DefaultMinChildMargin.Top;
            endsAfterOccurrenceRadioButton.Parent = endDateControlsPanel;
            occurrencePicker.Click += (s, e) => endsAfterOccurrenceRadioButton.IsChecked = true;

            // Ends never

            endsNeverRadioButton = new();
            endsNeverRadioButton.MarginTop = DefaultMinChildMargin.Top;
            endsNeverRadioButton.SuffixControl.Text = CommonStrings.Default.Never;
            endsNeverRadioButton.Parent = endDateControlsPanel;

            // End panels

            endDatePanel.SetLayout(LayoutStyle.Horizontal);

            endDateControlsPanel.SetParent(endDatePanel);
            endDatePanel.Parent = rangePanel;

            // Other

            XRadioButton[] radioButtons = {
                untilRadioButton.MainControl,
                endsAfterOccurrenceRadioButton.MainControl,
                endsNeverRadioButton.MainControl };

            untilRadioButton.MainControl.RadioSiblings = radioButtons;
            endsAfterOccurrenceRadioButton.MainControl.RadioSiblings = radioButtons;
            endsNeverRadioButton.MainControl.RadioSiblings = radioButtons;

            dailyPicker = CreateDailyPatternPicker();
            weeklyPicker = CreateWeeklyPatternPicker();
            monthlyPicker = CreateMonthlyPatternPicker();
            yearlyPicker = CreateYearlyPatternPicker();

            tabControl.ActiveTabHasBorder = true;
            tabControl.TabHasBorder = true;
            tabControl.HasInteriorBorder = false;
            tabControl.ActiveTabTheme = SpeedButton.KnownTheme.StaticBorder;
            tabControl.TabTheme = SpeedButton.KnownTheme.Default;
            tabControl.ContentVisible = false;

            tabControl.Add(Localized.ScheduleRepeatPatternNone, nonePicker);
            tabControl.Add(Localized.ScheduleRepeatPatternDaily, dailyPicker);
            tabControl.Add(Localized.ScheduleRepeatPatternWeekly, weeklyPicker);
            tabControl.Add(Localized.ScheduleRepeatPatternMonthly, monthlyPicker);
            tabControl.Add(Localized.ScheduleRepeatPatternYearly, yearlyPicker);

            HasBorder = false;

            editExcludedDatesLinkLabel.Parent = rangePanel;
            editExcludedDatesLinkLabel.LinkClicked += OnEditExcludedDatesLinkLabelClick;

            tabControl.Parent = tabControlPanel;

            tabControlPanel.Parent = this;
            rangePanel.Parent = this;

            tabControl.SelectedIndexChanged += OnTabControlSelectedIndexChanged;

            DateFormat = DefaultDateFormat;

            endsNeverRadioButton.IsEnabled = false;
            endsAfterOccurrenceRadioButton.IsEnabled = false;

            ValueToControls();

            data.PropertyChanged += OnValuePropertyChanged;
            startPicker.MainControl.ValueChanged += OnStartPickerChanged;
            untilDatePicker.MainControl.ValueChanged += OnEndDatePickerChanged;
            occurrencePicker.ValueChanged += OnOccurrencePickerValueChanged;
            endsNeverRadioButton.CheckedChanged += OnEndsNeverRadioChanged;
            untilRadioButton.CheckedChanged += OnEndsOnRadioButton;
            endsAfterOccurrenceRadioButton.CheckedChanged += OnEndsAfterOccurrenceRadioChanged;
        }

        /// <summary>
        /// Wraps the specified event handler with logic to suspend and resume property change notifications
        /// </summary>
        /// <param name="handler">The event handler to wrap.</param>
        /// <returns>A new event handler that suspends and resumes property change notifications.</returns>
        public EventHandler WithValueToControlsSuspended(EventHandler handler)
        {
            return (s, e) =>
            {
                valueToControlsSuspendCounter++;
                try
                {
                    handler(s, e);
                }
                finally
                {
                    valueToControlsSuspendCounter--;
                }
            };
        }

        /// <summary>
        /// Occurs when the selected repeat pattern changes.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets the panel which contains the <see cref="TabControl"/> used for selecting the repeat pattern.
        /// </summary>
        [Browsable(false)]
        public RoundBorderWithTitle TabControlPanel => tabControlPanel;

        /// <summary>
        /// Gets or sets a value indicating whether the collapse buttons
        /// are displayed in the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public virtual bool ShowCollapseButtons
        {
            get
            {
                return tabControlPanel.HeaderToolBar.CollapseButton.Visible;
            }

            set
            {
                tabControlPanel.HeaderToolBar.CollapseButton.Visible = value;
                rangePanel.HeaderToolBar.CollapseButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets the inner <see cref="TabControl"/> used for selecting the repeat pattern.
        /// Its pages contain controls for selecting specific repeat pattern rules.
        /// </summary>
        [Browsable(false)]
        public TabControl InnerTabControl => tabControl;

        /// <summary>
        /// Gets the start date panel which contains controls for specifying
        /// the start date and time of the repeat pattern.
        /// </summary>
        [Browsable(false)]
        public TransparentPanel RangePanel => rangePanel;

        /// <summary>
        /// Gets the end date panel which contains controls for specifying the end date of the repeat pattern.
        /// </summary>
        [Browsable(false)]
        public TransparentPanel EndDatePanel => endDatePanel;

        /// <summary>
        /// Gets or sets a value indicating whether the values of the inner controls are transparent.
        /// </summary>
        public virtual bool ValuesTransparent
        {
            get => valuesTransparent;
            set
            {
                if (valuesTransparent == value)
                    return;
                valuesTransparent = value;
                startPicker.MainControl.TextTransparent = value;
                untilDatePicker.MainControl.TextTransparent = value;
                occurrencePicker.IntPicker.TextPicker.TextTransparent = value;
                dailyPicker.ValuesTransparent = value;
                weeklyPicker.ValuesTransparent = value;
                monthlyPicker.ValuesTransparent = value;
                yearlyPicker.ValuesTransparent = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the borders of inner panels are shown
        /// in the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public virtual bool ShowPanelBorders
        {
            get
            {
                return rangePanel.HasBorder;
            }

            set
            {
                rangePanel.HasBorder = value;
                tabControlPanel.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets the format provider used for culture-specific formatting of date and time values.
        /// </summary>
        public virtual IFormatProvider? FormatProvider
        {
            get => formatProvider;
            set
            {
                if (value == formatProvider) return;

                formatProvider = value;
                startPicker.MainControl.FormatProvider = value;
                untilDatePicker.MainControl.FormatProvider = value;
                dailyPicker.FormatProvider = value;
                weeklyPicker.FormatProvider = value;
                monthlyPicker.FormatProvider = value;
                yearlyPicker.FormatProvider = value;
            }
        }

        /// <summary>
        /// Gets the start date picker control used for selecting the start date and time of the repeat pattern.
        /// </summary>
        [Browsable(false)]
        public GenericControlAndImage<DatePicker> StartPicker => startPicker;

        /// <summary>
        /// Gets the <see cref="DailyPatternPicker"/> control for selecting a daily repeat pattern.
        /// </summary>
        [Browsable(false)]
        public DailyPatternPicker DailyPicker => dailyPicker;

        /// <summary>
        /// Gets the <see cref="WeeklyPatternPicker"/> control for selecting a weekly repeat pattern.
        /// </summary>
        [Browsable(false)]
        public WeeklyPatternPicker WeeklyPicker => weeklyPicker;

        /// <summary>
        /// Gets the <see cref="MonthlyPatternPicker"/> control for selecting a monthly repeat pattern.
        /// </summary>
        [Browsable(false)]
        public MonthlyPatternPicker MonthlyPicker => monthlyPicker;

        /// <summary>
        /// Gets the <see cref="YearlyPatternPicker"/> control for selecting a yearly repeat pattern.
        /// </summary>
        [Browsable(false)]
        public YearlyPatternPicker YearlyPicker => yearlyPicker;

        /// <summary>
        /// Gets the control for selecting "no repeat" pattern.
        /// </summary>
        [Browsable(false)]
        public HiddenBorder NonePicker => nonePicker;

        /// <summary>
        /// Provides access to the <see cref="RepeatPatternRule"/> representing the selected repeat
        /// pattern and its associated rules. Setter of this property doesn't change the instance of the value,
        /// but rather assigns the properties of the provided
        /// <see cref="RepeatPatternRule"/> instance to the current instance.
        /// </summary>
        [Browsable(false)]
        public virtual RepeatPatternRule Value
        {
            get
            {
                return data;
            }

            set
            {
                data.Assign(value);
            }
        }

        /// <summary>
        /// Gets or sets the date format used for displaying dates in the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public virtual string? DateFormat
        {
            get => startPicker.MainControl.Format;
            set
            {
                startPicker.MainControl.Format = value;
                untilDatePicker.MainControl.Format = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected repeat pattern.
        /// </summary>
        public virtual ScheduleRepeatPattern SelectedPattern
        {
            get
            {
                return data.Kind;
            }

            set
            {
                data.Kind = value;
            }
        }

        /// <summary>
        /// Shows the editor for excluded dates in the repeat pattern.
        /// This method opens a collection editor window allowing users to add or remove excluded dates
        /// from the repeat pattern.
        /// </summary>
        public virtual void ShowExcludedDatesEditor()
        {
            IListEditSource source = new DatesListEditSource(
                () => Value.ExcludedDates,
                items =>
                {
                    Value.ExcludedDates = items;
                });

            WindowCollectionEditor.ShowCollectionEditor<WindowDateCollectionEditor>(source, OnCreate);

            void OnCreate(WindowDateCollectionEditor e)
            {
                e.Title = CommonStrings.Default.WindowTitleExcludedDatesEditor;
            }
        }

        /// <summary>
        /// Called to update the control values based on the current
        /// state of the <see cref="RepeatPatternRule"/> instance.
        /// </summary>
        public virtual void ValueToControls()
        {
            if (valueToControlsSuspendCounter > 0)
                return;

            try
            {
                valueToControlsSuspendCounter++;

                var tabIndex = (int)SelectedPattern;
                tabControl.SelectedIndex = tabIndex;

                tabControl.SelectedIndex = (int)SelectedPattern;
                startPicker.MainControl.AsDateOnly = data.ValidFromDate;
                untilDatePicker.MainControl.AsDateOnly = data.ValidToDate;

                untilRadioButton.IsChecked = data.EndCondition == RepeatPatternEndCondition.OnDate;
                endsAfterOccurrenceRadioButton.IsChecked
                    = data.EndCondition == RepeatPatternEndCondition.AfterOccurrence;
                endsNeverRadioButton.IsChecked = data.EndCondition == RepeatPatternEndCondition.Never;

                occurrencePicker.Value = data.OccurrenceCount;
                UpdateOccurrenceText();

                dailyPicker.ValueToControls();
                weeklyPicker.ValueToControls();
                monthlyPicker.ValueToControls();
                yearlyPicker.ValueToControls();
            }
            finally
            {
                valueToControlsSuspendCounter--;
            }
        }

        /// <summary>
        /// Called when the selected index of the inner tab control changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnTabControlSelectedIndexChanged(object? sender, EventArgs e)
        {
            var index = tabControl.SelectedIndex;

            var newPattern = ScheduleRepeatPattern.None;

            if (index >= 0 && index <= (int)ScheduleRepeatPattern.Yearly)
                newPattern = (ScheduleRepeatPattern)index;

            tabControl.ContentVisible = newPattern != ScheduleRepeatPattern.None;

            SelectedPattern = newPattern;

            untilRadioButton.IsChecked = true;

            endsNeverRadioButton.SetEnabled(index > 0);
            endsAfterOccurrenceRadioButton.SetEnabled(index > 0);
        }

        /// <summary>
        /// Updates the suffix text of the occurrence picker based on the current occurrence count.
        /// </summary>
        protected virtual void UpdateOccurrenceText()
        {
            occurrencePicker.SuffixText = data.OccurrenceCount == 1
                ? CommonStrings.Default.Occurrence : CommonStrings.Default.Occurrences;
        }

        /// <summary>
        /// Called when a property of the repeat pattern rule changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnValuePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValueToControls();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the "Edit Excluded Dates" link label is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnEditExcludedDatesLinkLabelClick(object? sender, CancelEventArgs e)
        {
            ShowExcludedDatesEditor();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RepeatPatternRule"/> class.
        /// </summary>
        /// <returns> A new instance of the <see cref="RepeatPatternRule"/> class. </returns>
        protected virtual RepeatPatternRule CreateRule() => new();

        /// <summary>
        /// Creates a new instance of the <see cref="DailyPatternPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="DailyPatternPicker"/> control. </returns>
        protected virtual DailyPatternPicker CreateDailyPatternPicker() => new(data.DailyRule);

        /// <summary>
        /// Creates a new instance of the <see cref="WeeklyPatternPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="WeeklyPatternPicker"/> control. </returns>
        protected virtual WeeklyPatternPicker CreateWeeklyPatternPicker() => new(data.WeeklyRule);

        /// <summary>
        /// Creates a new instance of the <see cref="MonthlyPatternPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="MonthlyPatternPicker"/> control. </returns>
        protected virtual MonthlyPatternPicker CreateMonthlyPatternPicker() => new(data.MonthlyRule);

        /// <summary>
        /// Creates a new instance of the <see cref="YearlyPatternPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="YearlyPatternPicker"/> control. </returns>
        protected virtual YearlyPatternPicker CreateYearlyPatternPicker() => new(data.YearlyRule);

        /// <summary>
        /// Handles the ValueChanged event of the <see cref="StartPicker"/> control.
        /// </summary>
        protected virtual void OnStartPickerChanged(object? s, EventArgs e)
        {
            data.DoInsideSuspendedPropertyChanged(() =>
            {
                data.ValidFromDate = startPicker.MainControl.AsDateOnlyOrToday;
            });
        }

        /// <summary>
        /// Handles the ValueChanged event of the end date picker control.
        /// </summary>
        /// <param name="s"> The source of the event. </param>
        /// <param name="e"> The <see cref="EventArgs"/> instance containing the event data. </param>
        protected virtual void OnEndDatePickerChanged(object? s, EventArgs e)
        {
            data.ValidToDate = untilDatePicker.MainControl.AsDateOnlyOrToday;
        }

        /// <summary>
        /// Handles the ValueChanged event of the occurrence picker controlt
        /// </summary>
        /// <param name="s"> The source of the event. </param>
        /// <param name="e"> The <see cref="EventArgs"/> instance containing the event data. </param>
        protected virtual void OnOccurrencePickerValueChanged(object? s, EventArgs e)
        {
            data.OccurrenceCount = occurrencePicker.Value;
            UpdateOccurrenceText();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the "Ends Never" radio button control.
        /// </summary>
        /// <param name="s"> The source of the event. </param>
        /// <param name="e"> The <see cref="EventArgs"/> instance containing the event data. </param>
        protected virtual void OnEndsNeverRadioChanged(object? s, EventArgs e)
        {
            if (endsNeverRadioButton.IsChecked)
            {
                data.EndCondition = RepeatPatternEndCondition.Never;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the "Ends On" radio button control.
        /// </summary>
        /// <param name="s"> The source of the event. </param>
        /// <param name="e"> The <see cref="EventArgs"/> instance containing the event data. </param>
        protected virtual void OnEndsOnRadioButton(object? s, EventArgs e)
        {
            if (untilRadioButton.IsChecked)
            {
                data.EndCondition = RepeatPatternEndCondition.OnDate;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the "Ends After Occurrence" radio button control.
        /// </summary>
        /// <param name="s"> The source of the event. </param>
        /// <param name="e"> The <see cref="EventArgs"/> instance containing the event data. </param>
        protected virtual void OnEndsAfterOccurrenceRadioChanged(object? s, EventArgs e)
        {
            if (endsAfterOccurrenceRadioButton.IsChecked)
            {
                data.EndCondition = RepeatPatternEndCondition.AfterOccurrence;
            }
        }
    }

    /// <summary>
    /// Represents a scrollable version of the <see cref="RepeatPatternPicker"/> control.
    /// </summary>
    public class ScrollableRepeatPatternPicker : ScrollViewer<RepeatPatternPicker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollableRepeatPatternPicker"/> class.
        /// </summary>
        public ScrollableRepeatPatternPicker()
        {
            ScrolledControl.HasBorder = false;
        }

        /// <summary>
        /// Occurs when the selected repeat pattern changes in the inner <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public event EventHandler? ValueChanged
        {
            add => ScrolledControl.ValueChanged += value;
            remove => ScrolledControl.ValueChanged -= value;
        }

        /// <summary>
        /// Gets the <see cref="RepeatPatternRule"/> instance representing
        /// the selected repeat pattern and its associated rules.
        /// </summary>
        public RepeatPatternRule Value
        {
            get
            {
                return ScrolledControl.Value;
            }

            set
            {
                ScrolledControl.Value = value;
            }
        }
    }
}
