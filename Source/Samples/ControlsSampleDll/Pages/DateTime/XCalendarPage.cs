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
        private readonly ScrollableRepeatPatternPicker patternPickerContainer;
        private readonly RepeatPatternPicker patternPicker;
        private bool highlightDates;

        static XCalendarPage()
        {
        }

        public XCalendarPage()
        {
            patternPickerContainer = new ScrollableRepeatPatternPicker();
            patternPicker = patternPickerContainer.ScrolledControl;
            patternPicker.Value.Kind = ScheduleRepeatPattern.Daily;

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

            calendar.PageChanged += (s, e) =>
            {
                UpdateHighlightedDates();
                App.Log("Page changed");
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

                // Options Panel

                var panel = new ScrollablePanelSettings();
                panel.Margin = 5;
                panel.Title = GenericStrings.Options;

                var p = panel.ScrolledControl;

                p.Add<BoldLabel>("Options");

                p.AddInput("Show Holidays", calendar, nameof(calendar.ShowHolidays));
                p.AddInput("Show Header Border", calendar, nameof(calendar.ShowHeaderBorder));
                p.AddInput("Show Month DropDown", calendar, nameof(calendar.ShowMonthDropDown));
                p.AddInput("Show Year DropDown", calendar, nameof(calendar.ShowYearDropDown));
                p.AddInput("No Month Change", calendar, nameof(calendar.NoMonthChange));
                p.AddInput("No Year Change", calendar, nameof(calendar.NoYearChange));
                p.AddInput("Show Surround Weeks", calendar, nameof(calendar.ShowSurroundWeeks));
                p.AddInput("Enabled", calendar, nameof(calendar.Enabled));
                p.AddInput("Is Bold", calendar, nameof(calendar.IsBold));

                var valueItem = p.AddInput("Value", calendar, nameof(calendar.Value));

                valueItem.WithEditor<DatePicker>(c =>
                {
                    calendar.ValueChanged += (s, e) =>
                    {
                        c.AsDateOnly = calendar.Value;
                    };
                });

                var dayNamesKindItem = p.AddInput("Day Names Kind:", calendar, nameof(calendar.DayNamesKind));
                dayNamesKindItem.WithEditor<EnumPickerAndButton>(c =>
                {
                });

                var firstDayOfWeekItem = p.AddInput("First Day Of Week:", calendar, nameof(calendar.FirstDayOfWeek));

                // Range Settings

                p.AddHorizontalLine();
                p.Add<BoldLabel>("Range Settings");

                var minDateItem = p.AddInput("MinDate:", calendar, nameof(calendar.MinDate));
                var maxDateItem = p.AddInput("MaxDate:", calendar, nameof(calendar.MaxDate));
                var useMinDateItem = p.AddInput("Use MinDate", calendar, nameof(calendar.UseMinDate));
                var useMaxDateItem = p.AddInput("Use MaxDate", calendar, nameof(calendar.UseMaxDate));

                void RangeAnyDate()
                {
                    useMinDateItem.Value = false;
                    useMaxDateItem.Value = false;
                }

                void RangeTomorrow()
                {
                    RangeAnyDate();
                    maxDateItem.Value = DateTime.Today.AddDays(1).ToDateOnly();
                    useMaxDateItem.Value = true;
                }

                void RangeYesterday()
                {
                    RangeAnyDate();
                    minDateItem.Value = DateTime.Today.AddDays(-1).ToDateOnly();
                    useMinDateItem.Value = true;
                }

                void RangeYesterdayTomorrow()
                {
                    RangeAnyDate();
                    maxDateItem.Value = DateTime.Today.AddDays(1).ToDateOnly();
                    minDateItem.Value = DateTime.Today.AddDays(-1).ToDateOnly();
                    useMinDateItem.Value = true;
                    useMaxDateItem.Value = true;
                }

                // Actions

                p.AddHorizontalLine();
                p.Add<BoldLabel>("Actions");

                p.AddButton("Today", calendar.SelectToday);
                p.AddButton("Clear Day Attributes", () => calendar.ClearAttrAll());
                p.AddButton("Reset Day Attributes", () => calendar.ResetAttrAll());
                p.AddButton($"{GenericStrings.Allow} {GenericStrings.AnyDate}", RangeAnyDate);
                p.AddButton($"{GenericStrings.Allow} <= {GenericStrings.Tomorrow}", RangeTomorrow);
                p.AddButton($"{GenericStrings.Allow} >= {GenericStrings.Yesterday}", RangeYesterday);
                p.AddButton($"{GenericStrings.Allow} {GenericStrings.Yesterday}..{GenericStrings.Tomorrow}", RangeYesterdayTomorrow);

                tabControl.Add(panel);

                // Repeat Pattern Panel

                var patternSettings = new PanelSettings();
                patternSettings.SetMinChildMarginLeftRight();
                patternSettings.AddInput("Highlight dates", this, nameof(XCalendarPage.HighlightDates));

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
            }

            calendar.QueryHoliday += (s, e) =>
            {
                if (DateUtils.USAHolidayHelper.IsKnownHoliday(e.Date))
                {
                    e.IsHoliday = true;
                }
            };

            calendar.QueryDayAttributes += (s, e) =>
            {
                if (e.Cell.Date.Day == 5)
                {
                    e.DateAttr = calendar.DateAttributes.Blue;
                }
            };

            UpdateHighlightedDates();
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
            calendar.ResetAttrAll(invalidate: false);

            if (HighlightDates)
            {
                calendar.MarkWithRule(patternPicker.Value, calendar.DateAttributes.Green, invalidate: false);
            }
            else
            {
            }

            calendar.Invalidate();
        }
    }
}