using Alternet.UI;
using System;
using Alternet.Drawing;

namespace HelloWorldSample
{
    public class CustomCompositeControl : Control
    {
        private string text = "";

        private Border? border;

        private Label[]? lines;

        private Brush brush = Brushes.LightGreen;

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

        public Brush Brush
        {
            get => brush;

            set
            {
                brush = value;
                UpdateVisual();
            }
        }

        protected override void OnHandlerAttached(EventArgs e)
        {
            base.OnHandlerAttached(e);

            border = new Border
            {
                Background = new SolidBrush(Color.FromArgb(unchecked((int)0xFF333333))),
                BorderBrush = new SolidBrush(Color.FromArgb(unchecked((int)0xFF4A47FF)))
            };
            Handler.VisualChildren.Add(border);

            var panel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Margin = new Thickness(10)
            };
            border.Handler.VisualChildren.Add(panel);

            panel.Handler.VisualChildren.Add(new Label { Text = "Composite Control", Foreground = Brushes.White });

            lines = new Label[10];
            for (int i = 0; i < lines.Length; i++)
                panel.Handler.VisualChildren.Add(lines[i] = new Label());

            UpdateVisual();
        }

        protected override void OnHandlerDetaching(EventArgs e)
        {
            if (border == null)
                throw new InvalidOperationException();

            Handler.VisualChildren.Remove(border);
            border.Dispose();

            base.OnHandlerDetaching(e);
        }

        private void UpdateVisual()
        {
            var color = ((SolidBrush)brush).Color;
            var cl = new Skybrud.Colors.RgbColor(color.R, color.G, color.B);
            float darken = 50;
            for (int i = 0; i < lines!.Length; i++)
            {
                var textBlock = lines[i];
                textBlock.Text = $"{Text} {i}";
                
                var c = cl.Darken(darken).ToRgb();
                textBlock.Foreground = new SolidBrush(Color.FromArgb(c.R, c.G, c.B));
                darken -= 5f;
            }
        }
    }
}