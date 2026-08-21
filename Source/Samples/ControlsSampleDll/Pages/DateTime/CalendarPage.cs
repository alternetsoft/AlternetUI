using System;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    internal partial class CalendarPage : Panel
    {
        private readonly Calendar calendar = new();
        private readonly TabControl tabControl = new();

        static CalendarPage()
        {
        }

        public CalendarPage()
        {
            Layout = LayoutStyle.Horizontal;
            calendar.Margin = 5;
            calendar.Alignment = (HorizontalAlignment.Left, VerticalAlignment.Top);
            calendar.Parent = this;
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
                showHolidaysCheckBox.BindBoolProp(calendar, nameof(Calendar.ShowHolidays));

                var noMonthChangeCheckBox = new XCheckBox(GenericStrings.NoMonthChange);
                noMonthChangeCheckBox.Parent = checkboxPanel;
                noMonthChangeCheckBox.BindBoolProp(calendar, nameof(Calendar.NoMonthChange));

                var useGenericCheckBox = new XCheckBox(GenericStrings.UseGeneric);
                useGenericCheckBox.Parent = checkboxPanel;
                useGenericCheckBox.BindBoolProp(calendar, nameof(Calendar.UseGeneric));

                var sequentialMonthSelectCheckBox = new XCheckBox(GenericStrings.SequentalMonthSelect);
                sequentialMonthSelectCheckBox.Parent = checkboxPanel;
                sequentialMonthSelectCheckBox.BindBoolProp(calendar, nameof(Calendar.SequentialMonthSelect));
                sequentialMonthSelectCheckBox.Enabled = useGenericCheckBox.IsChecked;

                var showSurroundWeeksCheckBox = new XCheckBox(GenericStrings.ShowSurroundWeeks);
                showSurroundWeeksCheckBox.Parent = checkboxPanel;
                showSurroundWeeksCheckBox.BindBoolProp(calendar, nameof(Calendar.ShowSurroundWeeks));
                showSurroundWeeksCheckBox.Enabled = useGenericCheckBox.IsChecked;

                var weekNumbersCheckBox = new XCheckBox(GenericStrings.WeekNumbers);
                weekNumbersCheckBox.Parent = checkboxPanel;
                weekNumbersCheckBox.BindBoolProp(calendar, nameof(Calendar.ShowWeekNumbers));
                checkboxPanel.ChildrenSet.Margin(3);

                // Buttons panel

                var buttonPanel = new VerticalStackPanel();
                buttonPanel.Title = GenericStrings.Mark;
                buttonPanel.Margin = 5;
                tabControl.Add(buttonPanel);

                var setDayColorsButton = new XButton($"{GenericStrings.DaysStyle} (5, 7)", SetDayColors);
                setDayColorsButton.Enabled = useGenericCheckBox.IsChecked;
                setDayColorsButton.Margin = 5;
                buttonPanel.Children.Add(setDayColorsButton);

                var markDaysButton = new XButton($"{GenericStrings.MarkDays} (2, 3)", MarkDays);
                var selectTodayButton = new XButton(GenericStrings.Today, calendar.SelectToday);
                var clearMarksButton = new XButton("Clear marks", ()=>calendar.MarkAll(false));
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
                    "First Day of Week",
                    () => calendar.FirstDayOfWeek ?? DateUtils.SystemFirstDayOfWeek,
                    (value) => calendar.FirstDayOfWeek = value,
                    itemTitles: [ "Sunday", "Monday" ],
                    itemValues: [ DayOfWeek.Sunday, DayOfWeek.Monday ]);

                // Other initializations

                useGenericCheckBox.BindBoolProp(setDayColorsButton, nameof(XButton.Enabled));
                useGenericCheckBox.BindBoolProp(sequentialMonthSelectCheckBox, nameof(XButton.Enabled));
                useGenericCheckBox.BindBoolProp(showSurroundWeeksCheckBox, nameof(XButton.Enabled));

                useGenericCheckBox.CheckedChanged += Generic_CheckedChanged;

                void Generic_CheckedChanged(object? sender, EventArgs e)
                {
                    showHolidaysCheckBox.IsChecked = calendar.ShowHolidays;
                    noMonthChangeCheckBox.IsChecked = calendar.NoMonthChange;
                    sequentialMonthSelectCheckBox.IsChecked = calendar.SequentialMonthSelect;
                    showSurroundWeeksCheckBox.IsChecked = calendar.ShowSurroundWeeks;
                    weekNumbersCheckBox.IsChecked = calendar.ShowWeekNumbers;

                    if (calendar.UseGeneric)
                        calendar.BackgroundColor = SystemColors.Window;
                }
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
            var s = calendar.Value?.ToString("yyyy-MM-dd");
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
        }

        private void Calendar_SelectionChanged(object? sender, EventArgs e)
        {
            var s = calendar.Value?.ToString("yyyy-MM-dd");
            LogEvent($"SelectionChanged {s}");
        }

        private void LogEvent(string evName)
        {
            App.Log($"Calendar: {evName}");
        }
    }
}