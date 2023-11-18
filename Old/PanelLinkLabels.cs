using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public class PanelLinkLabels : ScrollViewer
    {
        public static readonly Thickness DefaultPadding = new(5);
        public static readonly Thickness DefaultItemMargin = new(5);
        public static readonly double DefaultSpacerSize = 15;

        private readonly StackPanel panel = new()
        {
            Orientation = StackPanelOrientation.Vertical,
        };

        public PanelLinkLabels()
        {
            panel.Parent = this;
            panel.Padding = DefaultPadding;

           /* var color = SystemColors.Control;

            BackgroundColor = color;
            scrollViewer.BackgroundColor = color;
            panel.BackgroundColor = color;*/
        }

        public Control Add(string text, Action? action)
        {
            var label = CreateLabel(text);

            void LinkClicked(object? sender, CancelEventArgs e)
            {
                e.Cancel = true;
                action?.Invoke();
            }

            void LinkClicked2(object? sender, EventArgs e)
            {
                action?.Invoke();
            }

            if (action is not null)
            {
                if (label is LinkLabel linkLabel)
                    linkLabel.LinkClicked += LinkClicked;
                if (label is Button linkButton)
                    linkButton.Click += LinkClicked2;
            }

            return label;
        }

        public Control Add(string text, string url)
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

        private Control CreateLabel(string text, string? url = default)
        {
            Control label = new Button()
            {
                Margin = DefaultItemMargin,
                Text = text,
                Parent = panel,
            };

            if (url is not null)
            {
                if (label is LinkLabel linkLabel)
                    linkLabel.Url = url;
            }

            return label;
        }
    }
}
