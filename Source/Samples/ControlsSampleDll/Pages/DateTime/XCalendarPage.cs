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
        private readonly TabControl tabControl = new();
        private readonly Splitter splitter = new();

        static XCalendarPage()
        {
        }

        public XCalendarPage()
        {
            Padding = 5;

            splitter.Dock = DockStyle.Right;

            Layout = LayoutStyle.Dock;
            calendar.Dock = DockStyle.Fill;

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

            calendar.HasBorder = true;

            calendar.HorizontalAlignment = HorizontalAlignment.Center;
            calendar.VerticalAlignment = VerticalAlignment.Center;

            calendar.MinDate = DateTime.Now.AddDays(-10).ToDateOnly();
            calendar.UseMinDate = false;

            calendar.MaxDate = DateTime.Now.AddDays(60).ToDateOnly();
            calendar.UseMaxDate = false;

            DoInsideLayout(Fn);

            void Fn()
            {
                calendar.Parent = this;
                splitter.Parent = this;
                tabControl.Parent = this;

                calendar.SetScrollBarVisible(isVert: false, isVisible: true);

                var panel = new ScrollablePanelSettings();
                panel.Margin = 5;
                panel.Title = GenericStrings.Options;

                var p = panel.ScrolledControl;

                p.Add<BoldLabel>("Options");
                
                p.AddInput("Show Month DropDown", calendar, nameof(calendar.ShowMonthDropDown));
                p.AddInput("Show Year DropDown", calendar, nameof(calendar.ShowYearDropDown));

                var dayNamesKindItem = p.AddInput("Day Names Kind:", calendar, nameof(calendar.DayNamesKind));
                dayNamesKindItem.WithEditor<EnumPickerAndButton>(c =>
                {
                });

                var firstDayOfWeekItem = p.AddInput("First Day Of Week:", calendar, nameof(calendar.FirstDayOfWeek));

                p.AddHorizontalLine();
                p.Add<BoldLabel>("Range Settings");

                p.AddInput("MinDate:", calendar, nameof(calendar.MinDate));
                p.AddInput("MaxDate:", calendar, nameof(calendar.MaxDate));
                p.AddInput("Use MinDate", calendar, nameof(calendar.UseMinDate));
                p.AddInput("Use MaxDate", calendar, nameof(calendar.UseMaxDate));

                tabControl.Add(panel);
            }
        }

        private void LogEvent(string evName)
        {
            App.Log($"Calendar: {evName}");
        }
    }
}