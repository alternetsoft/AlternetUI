using System;
using System.Linq;

using Alternet.UI;
using Alternet.Drawing;
using System.Collections.Generic;
using Alternet.UI.Extensions;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    public partial class CalendarPage : Panel
    {
        private readonly ICalendarDateAttr blueColor;
        private readonly ICalendarDateAttr greenColor;
        private readonly ScrollableRepeatPatternPicker patternPickerContainer;
        private readonly RepeatPatternPicker patternPicker;

#pragma warning disable
        private readonly Calendar calendar = new();
#pragma warning restore
        private readonly TabControl tabControl = new();
        private bool highlightDates;

        static CalendarPage()
        {
        }

        public CalendarPage()
        {
            patternPickerContainer = new ScrollableRepeatPatternPicker();
            patternPicker = patternPickerContainer.ScrolledControl;
            patternPicker.Value.Kind = ScheduleRepeatPattern.Daily;

            blueColor = calendar.CreateDateAttr();
            blueColor.TextColor = ThemedColors.Blue.GetColor(this);

            greenColor = calendar.CreateDateAttr();
            greenColor.TextColor = ThemedColors.Green.GetColor(this);

            Layout = LayoutStyle.Horizontal;
            calendar.Margin = 5;
            calendar.Alignment = (HorizontalAlignment.Left, VerticalAlignment.Top);
            calendar.Parent = this;
            tabControl.HorizontalAlignment = HorizontalAlignment.Fill;
            tabControl.Margin = 5;
            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Width;
            tabControl.Parent = this;

            DoInsideLayout(Fn);

            void Fn()
            {

                // CheckBoxes panel

                var checkboxPanel = new VerticalStackPanel();
                checkboxPanel.Margin = 5;
                checkboxPanel.Title = GenericStrings.Options;
                tabControl.Add(checkboxPanel);

                var showHolidaysCheckBox = new XCheckBox(GenericStrings.ShowHolidays);
                showHolidaysCheckBox.Parent = checkboxPanel;
                showHolidaysCheckBox.BindBoolProp(calendar, nameof(XCalendar.ShowHolidays));

                var noMonthChangeCheckBox = new XCheckBox(GenericStrings.NoMonthChange);
                noMonthChangeCheckBox.Parent = checkboxPanel;
                noMonthChangeCheckBox.BindBoolProp(calendar, nameof(XCalendar.NoMonthChange));

                var sequentialMonthSelectCheckBox = new XCheckBox(GenericStrings.SequentalMonthSelect);
                sequentialMonthSelectCheckBox.Visible = false;
                sequentialMonthSelectCheckBox.Parent = checkboxPanel;
                sequentialMonthSelectCheckBox.BindBoolProp(calendar, "SequentialMonthSelect");
                sequentialMonthSelectCheckBox.Enabled = true;

                var showSurroundWeeksCheckBox = new XCheckBox(GenericStrings.ShowSurroundWeeks);
                showSurroundWeeksCheckBox.Parent = checkboxPanel;
                showSurroundWeeksCheckBox.BindBoolProp(calendar, nameof(XCalendar.ShowSurroundWeeks));
                showSurroundWeeksCheckBox.Enabled = true;

                var weekNumbersCheckBox = new XCheckBox(GenericStrings.WeekNumbers);
                weekNumbersCheckBox.Parent = checkboxPanel;
                weekNumbersCheckBox.BindBoolProp(calendar, "ShowWeekNumbers");
                checkboxPanel.ChildrenSet.Margin(3);

                // Buttons panel

                var buttonPanel = new VerticalStackPanel();
                buttonPanel.Title = GenericStrings.Mark;
                buttonPanel.Margin = 5;
                tabControl.Add(buttonPanel);

                var setDayColorsButton = new XButton($"{GenericStrings.DaysStyle} (5, 7)", SetDayColors);
                setDayColorsButton.Enabled = true;
                setDayColorsButton.Margin = 5;
                buttonPanel.Children.Add(setDayColorsButton);

                var markDaysButton = new XButton($"{GenericStrings.MarkDays} (2, 3)", MarkDays);
                var selectTodayButton = new XButton(GenericStrings.Today, calendar.SelectToday);
                var clearMarksButton = new XButton("Clear marks", () => calendar.MarkAll(false));
                var clearStylesButton = new XButton("Clear styles", calendar.ResetAttrAll);

                new ControlSet(
                    markDaysButton,
                    selectTodayButton,
                    clearMarksButton,
                    clearStylesButton).Margin(5).Parent(buttonPanel);

                // Allow date range panel

                var rangePanel = new VerticalStackPanel();
                rangePanel.Margin = 5;
                rangePanel.Title = "Range";
                tabControl.Add(rangePanel);

                var rangeAnyDateButton = new XButton(
                    $"{GenericStrings.Allow} {GenericStrings.AnyDate}",
                    RangeAnyDate_Click);
                var rangeTomorrowButton = new XButton(
                    $"{GenericStrings.Allow} <= {GenericStrings.Tomorrow}",
                    RangeTomorrow_Click);
                var rangeYesterdayButton = new XButton(
                    $"{GenericStrings.Allow} >= {GenericStrings.Yesterday}",
                    RangeYesterday_Click);
                var rangeYesterdayTomorrowButton = new XButton(
                    $"{GenericStrings.Allow} {GenericStrings.Yesterday}..{GenericStrings.Tomorrow}",
                    RangeYesterdayTomorrow_Click);

                new ControlSet(
                    rangeAnyDateButton,
                    rangeTomorrowButton,
                    rangeYesterdayButton,
                    rangeYesterdayTomorrowButton).Margin(5).Parent(rangePanel);

                // First day panel

                var panelSettings = new PanelSettings();
                panelSettings.Margin = 5;
                panelSettings.Title = "Other";
                tabControl.Add(panelSettings);

                panelSettings.AddRadioButtons<DayOfWeek>(
                    "First Day of Week:",
                    () => calendar.FirstDayOfWeek ?? DateUtils.SystemFirstDayOfWeek,
                    (value) => calendar.FirstDayOfWeek = value,
                    itemTitles: ["Sunday", "Monday"],
                    itemValues: [DayOfWeek.Sunday, DayOfWeek.Monday]);

                panelSettings.AddHorizontalLine();

                panelSettings.AddFlagCheckBoxes<FontStyle>(
                            "Font Styles:",
                            () => calendar.RealFont.Style,
                            (value) => calendar.Font = calendar.RealFont.WithStyle(value),
                            itemTitles: ["Bold", "Italic", "Underline"],
                            itemValues: [FontStyle.Bold, FontStyle.Italic, FontStyle.Underline]);

                panelSettings.AddInput("Selected date:", calendar, "AsDateOnly");

                // Repeat Pattern Panel

                var patternSettings = new PanelSettings();
                patternSettings.SetMinChildMarginLeftRight();
                patternSettings.AddInput("Highlight dates", this, nameof(CalendarPage.HighlightDates));

                patternPicker.Children.Prepend(patternSettings);

                patternPicker.Value.StartDate = DateUtils.GetFirstDateOfMonth(DateTime.Today.ToDateOnly());
                patternPicker.Value.EndDate = DateUtils.GetLastDateOfMonth(DateTime.Today.ToDateOnly());

                patternPickerContainer.Margin = 5;
                patternPickerContainer.Title = "Highlight";
                tabControl.Add(patternPickerContainer);

                patternPickerContainer.ValueChanged += (s, e) =>
                {
                    App.Log($"RepeatPatternPicker: ValueChanged");
                    UpdateHighlightedDates();
                };

                // Other initializations

                setDayColorsButton.Enabled = true;
                sequentialMonthSelectCheckBox.Enabled = true;
                showSurroundWeeksCheckBox.Enabled = true;

                showHolidaysCheckBox.IsChecked = calendar.ShowHolidays;
                noMonthChangeCheckBox.IsChecked = calendar.NoMonthChange;
                sequentialMonthSelectCheckBox.IsChecked = calendar.SequentialMonthSelect;
                showSurroundWeeksCheckBox.IsChecked = calendar.ShowSurroundWeeks;
                weekNumbersCheckBox.IsChecked = calendar.ShowWeekNumbers;

                if (calendar.UseGeneric)
                    calendar.BackgroundColor = ThemedColors.Window;
            }

            calendar.SelectionChanged += Calendar_SelectionChanged;
            calendar.PageChanged += Calendar_PageChanged;
            calendar.WeekNumberClick += Calendar_WeekNumberClick;
            calendar.DayHeaderClick += Calendar_DayHeaderClick;
            calendar.DayDoubleClick += Calendar_DayDoubleClick;

            void MarkDays()
            {
                calendar.Mark(2);
                calendar.Mark(3);
                calendar.Refresh();
            }

            void SetDayColors()
            {
                var dateAttr = calendar.CreateDateAttr();

                dateAttr.Border = CalendarDateBorder.Round;
                dateAttr.BorderColor = Color.Red;
                dateAttr.BackgroundColor = Color.LightSkyBlue;
                dateAttr.TextColor = Color.Navy;

                calendar.SetAttr(5, dateAttr);
                calendar.SetAttr(7, dateAttr);
                calendar.Refresh();
            }
        }

        public bool HighlightDates
        {
            get => highlightDates;
            set
            {
                if (highlightDates == value)
                    return;
                highlightDates = value;
                UpdateHighlightedDates();
            }
        }

        private void UpdateHighlightedDates()
        {
            calendar.ResetAttrAll();

            if (HighlightDates)
            {
                calendar.MarkWithRule(patternPicker.Value, greenColor);
            }
            else
            {
                calendar.MarkWeekendsAsHolidays();
            }
        }

        private void RangeAnyDate_Click()
        {
            calendar.UseMinMaxDate = false;
        }

        private void RangeTomorrow_Click()
        {
            calendar.UseMinMaxDate = false;
            calendar.MaxDate = DateTime.Today.AddDays(1);
            calendar.UseMaxDate = true;
        }

        private void RangeYesterday_Click()
        {
            calendar.UseMinMaxDate = false;
            calendar.MinDate = DateTime.Today.AddDays(-1);
            calendar.UseMinDate = true;
        }

        private void RangeYesterdayTomorrow_Click()
        {
            calendar.UseMinMaxDate = false;
            calendar.MaxDate = DateTime.Today.AddDays(1);
            calendar.MinDate = DateTime.Today.AddDays(-1);
            calendar.UseMinMaxDate = true;
        }

        private void Calendar_DayDoubleClick(object? sender, EventArgs e)
        {
            var s = calendar.Value.ToString("yyyy-MM-dd");
            LogEvent($"DayDoubleClick {s}");
        }

        private void Calendar_DayHeaderClick(object? sender, EventArgs e)
        {
            LogEvent("DayHeaderClick");
        }

        private void Calendar_WeekNumberClick(object? sender, EventArgs e)
        {
            LogEvent("WeekNumberClick");
        }

        private void Calendar_PageChanged(object? sender, EventArgs e)
        {
            LogEvent("PageChanged");
            UpdateHighlightedDates();
        }

        private void Calendar_SelectionChanged(object? sender, EventArgs e)
        {
            var s = calendar.Value.ToString("yyyy-MM-dd");
            LogEvent($"SelectionChanged {s}");
        }

        private void LogEvent(string evName)
        {
            App.Log($"Calendar: {evName}");
        }
    }
}