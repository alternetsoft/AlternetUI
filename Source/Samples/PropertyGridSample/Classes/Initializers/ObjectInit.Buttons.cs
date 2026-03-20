using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void LogClick(object? sender, EventArgs e)
        {
            var s = $"{sender?.GetType()} Click";
            App.Log(s);
        }

        public static void InitButton(object control)
        {
            if (control is not Button button)
                return;
            button.Text = "Butt&on";
            button.Click += LogClick;
            button.StateImages = GetButtonImages(button);
            button.SetImageMargins(5);
            button.SuggestedHeight = 100;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.MouseWheel += Button_MouseWheel;
        }

        public static void InitStdButton(object control)
        {
            if (control is not StdButton button)
                return;
            button.Text = "StdButton";
            button.Click += LogClick;
            button.StateImages = GetButtonImages(button);
            button.SetImageMargins(5);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.MouseWheel += Button_MouseWheel;

            var logMouseOver = false;
            var logVisualState = false;

            if (logMouseOver)
            {
                button.IsMouseOverChanged += (s, e) =>
                {
                    App.Log($"StdButton.IsMouseOver changed to [{button.IsMouseOver}]");
                };
            }

            if (logVisualState)
            {
                button.VisualStateChanged += (s, e) =>
                {
                    App.Log($"StdButton.VisualState changed to [{button.VisualState}]");
                };
            }
        }

        private static void Button_MouseWheel(object sender, MouseEventArgs e)
        {
            App.Log($"Button MouseWheel: {e.Delta}, {e.Timestamp}");
        }

        public static void InitSpeedTextButton(object control)
        {
            if (control is not SpeedTextButton button)
                return;
            button.Text = "Sample Text";
            button.UseTheme = SpeedButton.KnownTheme.StaticBorder;
            button.Click += LogClick;
        }

        public static void InitSpeedColorButton(object control)
        {
            if (control is not SpeedColorButton button)
                return;
            button.Value = Color.Red;
            button.UseTheme = SpeedButton.KnownTheme.StaticBorder;
            button.Click += LogClick;
        }

        public static void InitSpeedButton(object control)
        {
            if (control is not SpeedButton button)
                return;
            button.Text = "speedButton";
            button.TextVisible = true;
            button.Click += LogClick;

            button.ContextMenuStrip.Add("Menu Item 1", () => App.Log("Menu Item 1 Click"));
            button.ContextMenuStrip.Add("Menu Item 2", () => App.Log("Menu Item 2 Click"));

            button.LoadSvg(KnownSvgUrls.UrlImageOk, 32);

            button.StickyChanged += (s, e) =>
            {
                var stickyValue = button.Sticky;
                App.LogReplace(
                    $"SpeedButton.Sticky changed to [{stickyValue}]",
                    $"SpeedButton.Sticky");
            };
        }
    }
}