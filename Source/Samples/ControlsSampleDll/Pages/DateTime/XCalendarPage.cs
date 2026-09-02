using System;
using System.Linq;

using Alternet.UI;
using Alternet.Drawing;
using System.Collections.Generic;
using Alternet.UI.Extensions;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    public partial class XCalendarPage : Panel
    {
        private readonly XCalendar calendar = new();
        private readonly Panel calendarPanel = new();
        private readonly TabControl tabControl = new();
        private readonly Splitter splitter = new();

        static XCalendarPage()
        {
        }

        public XCalendarPage()
        {
            Padding = 5;

            calendarPanel.HasBorder = true;

            splitter.Dock = DockStyle.Right;

            Layout = LayoutStyle.Dock;
            calendarPanel.Dock = DockStyle.Fill;

            tabControl.Dock = DockStyle.Right;
            tabControl.Width = 400;

            calendar.DayClick += (s, e) =>
            {
                App.Log("Day clicked: " + e.Cell.Text);
            };

            calendar.HeaderClick += (s, e) =>
            {
                App.Log("Header clicked: " + e.Cell.Text);
            };

            calendar.ValueChanged += (s, e) =>
            {
                App.Log("Value changed: " + calendar.Value);
            };

            calendar.HeaderYearClick += (s, e) =>
            {
                App.Log("Year clicked");
            };

            calendar.HeaderMonthClick += (s, e) =>
            {
                App.Log("Month clicked");
            };

            calendarPanel.HasBorder = true;

            calendar.HorizontalAlignment = HorizontalAlignment.Center;
            calendar.VerticalAlignment = VerticalAlignment.Center;
            calendar.Parent = calendarPanel;

            calendar.MinDate = DateTime.Now.AddDays(-10).ToDateOnly();
            calendar.UseMinDate = false;

            calendar.MaxDate = DateTime.Now.AddDays(60).ToDateOnly();
            calendar.UseMaxDate = false;

            DoInsideLayout(Fn);

            void Fn()
            {
                calendarPanel.Parent = this;
                splitter.Parent = this;
                tabControl.Parent = this;

                var settingsPanel = new PanelSettings();
                settingsPanel.Margin = 5;
                settingsPanel.Title = GenericStrings.Options;

                settingsPanel.AddInput("MinDate", calendar, nameof(calendar.MinDate));
                settingsPanel.AddInput("MaxDate", calendar, nameof(calendar.MaxDate));
                settingsPanel.AddInput("Use MinDate", calendar, nameof(calendar.UseMinDate));
                settingsPanel.AddInput("Use MaxDate", calendar, nameof(calendar.UseMaxDate));

                tabControl.Add(settingsPanel);
            }
        }

        private void LogEvent(string evName)
        {
            App.Log($"Calendar: {evName}");
        }
    }
}