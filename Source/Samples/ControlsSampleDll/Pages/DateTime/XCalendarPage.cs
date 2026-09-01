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

        static XCalendarPage()
        {
        }

        public XCalendarPage()
        {
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

                // Options panel

                var checkboxPanel = new VerticalStackPanel();
                checkboxPanel.Margin = 5;
                checkboxPanel.Title = GenericStrings.Options;
                tabControl.Add(checkboxPanel);
            }
        }

        private void LogEvent(string evName)
        {
            App.Log($"Calendar: {evName}");
        }
    }
}