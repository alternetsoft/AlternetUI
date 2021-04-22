using Alternet.UI;
using System;
using System.Drawing;

namespace HelloWorldSample
{
    internal class CustomCompositeControl : Control
    {
        private string text = "";

        private Border? border;

        private TextBlock[]? lines;

        private Color color = Color.LightGreen;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                UpdateVisual();
                Parent?.PerformLayout();
            }
        }

        public Color Color
        {
            get => color;

            set
            {
                color = value;
                UpdateVisual();
            }
        }

        protected override void OnAttachHandler()
        {
            base.OnAttachHandler();

            border = new Border
            {
                BackgroundColor = Color.FromArgb(unchecked((int)0xFF333333)),
                BorderColor = Color.FromArgb(unchecked((int)0xFF4A47FF))
            };

            VisualChildren.Add(border);

            var panel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Margin = new Thickness(10)
            };
            border.VisualChildren.Add(panel);

            panel.VisualChildren.Add(new TextBlock { Text = "Composite Control", ForegroundColor = Color.White });

            lines = new TextBlock[10];
            for (int i = 0; i < lines.Length; i++)
                panel.VisualChildren.Add(lines[i] = new TextBlock());

            UpdateVisual();
        }

        protected override void OnDetachHandler()
        {
            base.OnDetachHandler();

            if (border == null)
                throw new InvalidOperationException();

            VisualChildren.Remove(border);
            border.Dispose();
        }

        private void UpdateVisual()
        {
            var cl = new Skybrud.Colors.RgbColor(color.R, color.G, color.B);
            float darken = 50;
            for (int i = 0; i < lines!.Length; i++)
            {
                var textBlock = lines[i];
                textBlock.Text = $"{Text} {i}";
                
                var c = cl.Darken(darken).ToRgb();
                textBlock.ForegroundColor = Color.FromArgb(c.R, c.G, c.B);
                darken -= 5f;
            }
        }
    }
}