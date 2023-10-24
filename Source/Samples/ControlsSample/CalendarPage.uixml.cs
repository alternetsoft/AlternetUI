using System;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;

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
            calendar.HorizontalAlignment = HorizontalAlignment.Left;
            calendar.VerticalAlignment = VerticalAlignment.Top;

            var optionsPanel = mainPanel.AddHorizontalStackPanel();
            var checkboxPanel = optionsPanel.AddVerticalStackPanel();

            var showHolidaysCheckBox = checkboxPanel.AddCheckBox("Show Holidays")
                .BindBoolProp(calendar,nameof(Calendar.ShowHolidays));
            
            var noMonthChangeCheckBox = checkboxPanel.AddCheckBox("No Month Change")
                .BindBoolProp(calendar, nameof(Calendar.NoMonthChange));
            
            var useGenericCheckBox = checkboxPanel.AddCheckBox("Use Generic")
                .BindBoolProp(calendar, nameof(Calendar.UseGeneric));

            var sequentalMonthSelectCheckBox = checkboxPanel.AddCheckBox("Sequental Month Select")
                .BindBoolProp(calendar, nameof(Calendar.SequentalMonthSelect));
            sequentalMonthSelectCheckBox.Enabled = useGenericCheckBox.IsChecked;
            
            var showSurroundWeeksCheckBox = checkboxPanel.AddCheckBox("Show Surround Weeks")
                .BindBoolProp(calendar, nameof(Calendar.ShowSurroundWeeks));
            showSurroundWeeksCheckBox.Enabled = useGenericCheckBox.IsChecked;
            
            var weekNumbersCheckBox = checkboxPanel.AddCheckBox("Week Numbers")
                .BindBoolProp(calendar, nameof(Calendar.ShowWeekNumbers));
            checkboxPanel.ChildrenSet.Margin(5);

            var buttonPanel = optionsPanel.AddVerticalStackPanel();
            var setDayColorsButton = buttonPanel.AddButton("Set days (5, 7) style", SetDayColors);
            setDayColorsButton.Enabled = useGenericCheckBox.IsChecked;

            buttonPanel.AddButton("Mark days (2, 3)", MarkDays);

            buttonPanel.ChildrenSet.Margin(5);

            useGenericCheckBox.BindBoolProp(setDayColorsButton, nameof(Button.Enabled));
            useGenericCheckBox.BindBoolProp(sequentalMonthSelectCheckBox, nameof(Button.Enabled));
            useGenericCheckBox.BindBoolProp(showSurroundWeeksCheckBox, nameof(Button.Enabled));

            calendar.Parent = mainPanel;
            calendar.PerformLayout();

            useGenericCheckBox.CheckedChanged += Generic_CheckedChanged;

            void MarkDays()
            {
                calendar.Mark(2);
                calendar.Mark(3);
                calendar.Refresh();
            }

            void SetDayColors()
            {
                var dateAttr = Calendar.CreateDateAttr();

                dateAttr.Border = CalendarDateBorder.Round;
                dateAttr.BorderColor = Color.Red;
                dateAttr.BackgroundColor = Color.LightSkyBlue;
                dateAttr.TextColor = Color.Navy;

                calendar.SetAttr(5, dateAttr);
                calendar.SetAttr(7, dateAttr);
                calendar.Refresh();
            }

            void Generic_CheckedChanged(object? sender, EventArgs e)
            {
                showHolidaysCheckBox.IsChecked = calendar.ShowHolidays;
                noMonthChangeCheckBox.IsChecked = calendar.NoMonthChange;
                sequentalMonthSelectCheckBox.IsChecked = calendar.SequentalMonthSelect;
                showSurroundWeeksCheckBox.IsChecked = calendar.ShowSurroundWeeks;
                weekNumbersCheckBox.IsChecked = calendar.ShowWeekNumbers;

                if (calendar.UseGeneric)
                    calendar.BackgroundColor = SystemColors.Window;
            }
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
        }

        private void Calendar_SelectionChanged(object? sender, EventArgs e)
        {
            var s = calendar.Value.ToString("yyyy-MM-dd");
            LogEvent($"SelectionChanged {s}");
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