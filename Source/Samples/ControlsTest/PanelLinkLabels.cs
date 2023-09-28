using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public class PanelLinkLabels : Control
    {
        public static readonly Thickness DefaultPadding = new(5);
        public static readonly Thickness DefaultItemMargin = new(5);
        public static readonly double DefaultSpacerSize = 15;

        private readonly ScrollViewer scrollViewer = new();

        private readonly StackPanel panel = new()
        {
            Orientation = StackPanelOrientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        public PanelLinkLabels()
        {
            scrollViewer.Parent = this;
            panel.Parent = scrollViewer;
            panel.Padding = DefaultPadding;
            BackgroundColor = Color.White;
            scrollViewer.BackgroundColor = Color.White;
            panel.BackgroundColor = Color.White;
        }

        public LinkLabel Add(string text, Action? action)
        {
            var label = CreateLabel(text);

            void LinkClicked(object? sender, CancelEventArgs e)
            {
                e.Cancel = true;
                action();
            }

            if(action is not null)
                label.LinkClicked += LinkClicked;
            return label;
        }

        public LinkLabel Add(string text, string url)
        {
            var label = CreateLabel(text, url);
            return label;
        }

        public Control AddSpacer()
        {
            Control separator = new()
            {
                SuggestedHeight = DefaultSpacerSize,
                Parent = panel,
            };

            return separator;
        }

        private LinkLabel CreateLabel(string text, string? url = default)
        {
            url ??= "https://www.alternet-ui.com/";
            LinkLabel label = new()
            {
                Margin = DefaultItemMargin,
                Text = text,
                Parent = panel,
            };

            if (url is not null)
                label.Url = url;
            return label;
        }
    }
}
