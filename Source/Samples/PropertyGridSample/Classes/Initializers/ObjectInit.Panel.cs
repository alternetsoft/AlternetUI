using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void InitGenericTabControl(object control)
        {
            if (control is not TabControl tabControl)
                return;
            InitGenericTabControl(tabControl);
        }

        public static void InitGenericTabControl(TabControl tabControl, bool withButtons = true)
        {
            tabControl.SuggestedSize = (300, 300);

            var panel1 = Internal("Panel 1");

            tabControl.Add("Panel 1", panel1);
            tabControl.Add("Panel 2", () => { return Internal("Panel 2"); });
            tabControl.Add("Panel 3", () => { return Internal("Panel 3"); });
            tabControl.Add("Panel 4", () => { return Internal("Panel 4"); });

            tabControl.SetTabSvg(0, KnownSvgImages.ImgGear, null, LightDarkColors.Blue);
            tabControl.SetTabSvg(1, MessageBoxSvg.Error);
            tabControl.SetTabSvg(2, MessageBoxSvg.Information);

            AbstractControl Internal(string title)
            {
                if(withButtons)
                    return CreatePanelWithButtons(title);
                var result = new Panel();
                result.Title = title;
                return result;
            }
        }

        public static void InitPanel(object control)
        {
            if (control is not Panel panel)
                return;
            panel.HasBorder = true;
            panel.SuggestedSize = 150;
            panel.KeyPress += Panel_KeyPress;
            panel.Scroll += Panel_Scroll;

            static void Panel_KeyPress(object? sender, KeyPressEventArgs e)
            {
                var prefix = "Panel.KeyPress: ";
                var s = prefix + e.KeyChar;
                App.LogReplace(s, prefix);
            }
        }

        public static void InitPanelSettings(object control)
        {
            if (control is not PanelSettings panel)
                return;
            panel.HasBorder = false;
            panel.Dock = DockStyle.Fill;

            panel.AddInput("This is CheckBox:", samplePropContainer, nameof(SamplePropContainer.SampleBool));

            panel.AddInput("This is TextBox:", samplePropContainer, nameof(SamplePropContainer.SampleString));

            panel.AddHorizontalLine();

            panel.AddRadioButtons<DayOfWeek>(
                "First Day of Week:",
                () => samplePropContainer.FirstDayOfWeek ?? DateUtils.SystemFirstDayOfWeek,
                (value) => samplePropContainer.FirstDayOfWeek = value,
                itemTitles: ["Sunday", "Monday"],
                itemValues: [DayOfWeek.Sunday, DayOfWeek.Monday]);

            panel.AddHorizontalLine();

            panel.AddFlagCheckBoxes<FontStyle>(
                        "Font Styles:",
                        () => samplePropContainer.FontStyle,
                        (value) => samplePropContainer.FontStyle = value,
                        itemTitles: ["Bold", "Italic", "Underline"],
                        itemValues: [FontStyle.Bold, FontStyle.Italic, FontStyle.Underline]);

            panel.AddHorizontalLine();

            panel.AddInput("This is color:", samplePropContainer, nameof(SamplePropContainer.SampleColor));

            panel.AddInput("This is time picker:", samplePropContainer, nameof(SamplePropContainer.SampleTime));

            panel.AddHorizontalLine();

            panel.AddInput("This is Memo:", samplePropContainer, nameof(SamplePropContainer.SampleMemo), new("IsMultiline"));
        }

        private static void Panel_Scroll(object sender, ScrollEventArgs e)
        {
            var s = $"Panel.Scroll: {e.Type}";
            App.LogReplace($"{s}, {e.ScrollOrientation}, {e.NewValue}", s);
        }

        public class SamplePropContainer
        {
            public Color SampleColor { get; set; } = LightDarkColors.Red;

            public TimeOnly SampleTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

            public DateOnly SampleDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

            public DayOfWeek? FirstDayOfWeek { get; set; }

            public bool SampleBool { get; set; } = true;

            public string? SampleString { get; set; } = "Sample string";

            public string? SampleMemo { get; set; } = LoremIpsumSmall;

            public FontStyle FontStyle { get; set; } = FontStyle.Bold;
        }

        private static readonly SamplePropContainer samplePropContainer = new ();
    }
}
