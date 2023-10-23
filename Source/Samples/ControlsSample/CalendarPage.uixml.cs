using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class CalendarPage : Control
    {
        private readonly Calendar calendar = new();
        private IPageSite? site;

        public CalendarPage()
        {
            InitializeComponent();

            calendar.SelectionChanged += Calendar_SelectionChanged;
            calendar.PageChanged += Calendar_PageChanged;
            calendar.WeekNumberClick += Calendar_WeekNumberClick;
            calendar.DayHeaderClick += Calendar_DayHeaderClick;
            calendar.DayDoubleClick += Calendar_DayDoubleClick;
            calendar.Margin = 5;

            var optionsPanel = mainPanel.AddHorizontalStackPanel();
            var checkboxPanel = optionsPanel.AddVerticalStackPanel();

            checkboxPanel.AddCheckBox("Show Holidays")
                .BindBoolProp(calendar,nameof(Calendar.ShowHolidays));
            checkboxPanel.AddCheckBox("No Month Change")
                .BindBoolProp(calendar, nameof(Calendar.NoMonthChange));
            checkboxPanel.AddCheckBox("Use Generic")
                .BindBoolProp(calendar, nameof(Calendar.UseGeneric));
            checkboxPanel.AddCheckBox("Generic: Sequental Month Select")
                .BindBoolProp(calendar, nameof(Calendar.SequentalMonthSelect));
            checkboxPanel.AddCheckBox("Generic: Show Surround Weeks")
                .BindBoolProp(calendar, nameof(Calendar.ShowSurroundWeeks));
            checkboxPanel.AddCheckBox("Native: Week Numbers")
                .BindBoolProp(calendar, nameof(Calendar.ShowWeekNumbers));
            checkboxPanel.ChildrenSet.Margin(5);

            calendar.Parent = mainPanel;
            calendar.PerformLayout();
        }

        private void Calendar_DayDoubleClick(object? sender, EventArgs e)
        {
            LogEvent("DayDoubleClick");
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
            LogEvent("SelectionChanged");
        }

        private void LogEvent(string evName)
        {
            site?.LogEvent($"Calendar event: {evName}");
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }
    }
}