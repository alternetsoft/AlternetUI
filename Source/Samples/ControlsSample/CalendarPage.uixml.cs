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

            checkboxPanel.AddCheckBox("Show Holidays")
                .BindBoolProp(calendar,nameof(Calendar.ShowHolidays));
            checkboxPanel.AddCheckBox("No Month Change")
                .BindBoolProp(calendar, nameof(Calendar.NoMonthChange));
            var useGenericCheckBox = checkboxPanel.AddCheckBox("Use Generic")
                .BindBoolProp(calendar, nameof(Calendar.UseGeneric));
            
            var sequentalMonthSelectCheckBox = checkboxPanel.AddCheckBox("Generic: Sequental Month Select")
                .BindBoolProp(calendar, nameof(Calendar.SequentalMonthSelect));
            sequentalMonthSelectCheckBox.Enabled = useGenericCheckBox.IsChecked;
            
            var showSurroundWeeksCheckBox = checkboxPanel.AddCheckBox("Generic: Show Surround Weeks")
                .BindBoolProp(calendar, nameof(Calendar.ShowSurroundWeeks));
            showSurroundWeeksCheckBox.Enabled = useGenericCheckBox.IsChecked;
            
            checkboxPanel.AddCheckBox("Native: Week Numbers")
                .BindBoolProp(calendar, nameof(Calendar.ShowWeekNumbers));
            checkboxPanel.ChildrenSet.Margin(5);

            var buttonPanel = optionsPanel.AddVerticalStackPanel();
            var setDayColorsButton = buttonPanel.AddButton("Set days (5, 7) style", SetDayColors);
            setDayColorsButton.Enabled = useGenericCheckBox.IsChecked;

            buttonPanel.AddButton("Mark days (2, 3)", SelectDay);

            buttonPanel.ChildrenSet.Margin(5);

            useGenericCheckBox.BindBoolProp(setDayColorsButton, nameof(Button.Enabled));
            useGenericCheckBox.BindBoolProp(sequentalMonthSelectCheckBox, nameof(Button.Enabled));
            useGenericCheckBox.BindBoolProp(showSurroundWeeksCheckBox, nameof(Button.Enabled));

            calendar.Parent = mainPanel;
            calendar.PerformLayout();

            void SelectDay()
            {
                calendar.Mark(2);
                calendar.Mark(3);
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