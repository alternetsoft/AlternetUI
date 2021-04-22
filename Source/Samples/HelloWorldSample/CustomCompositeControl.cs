using Alternet.UI;
using System;
using System.Drawing;

namespace HelloWorldSample
{
    internal class CustomCompositeControl : Control
    {
        protected override ControlHandler CreateHandler() => new CustomHandler();

        private string text = "";

        public string Text
        {
            get => text;
            set
            {
                text = value;
                Parent?.PerformLayout();
                Update();
            }
        }

        private class CustomHandler : ControlHandler<CustomCompositeControl>
        {
            private Border? border;
            private TextBlock[]? lines;

            protected override void OnAttach()
            {
                base.OnAttach();

                border = new Border
                {
                    BackgroundColor = Color.FromArgb(unchecked((int)0xFFACAAFF)),
                    BorderColor = Color.FromArgb(unchecked((int)0xFF4A47FF))
                };

                Control.VisualChildren.Add(border);

                var panel = new StackPanel
                {
                    Orientation = StackPanelOrientation.Vertical,
                    Margin = new Thickness(5)
                };
                border.VisualChildren.Add(panel);

                panel.VisualChildren.Add(new TextBlock { Text = "Composite Control" });
                
                lines = new TextBlock[3];
                for (int i = 0; i < lines.Length; i++)
                    panel.VisualChildren.Add(lines[i] = new TextBlock());
                
                UpdateVisual();
            }

            void UpdateVisual()
            {
                for (int i = 0; i < lines!.Length; i++)
                    lines[i].Text = $"{Control.Text} {i}";
            }

            protected override void OnDetach()
            {
                base.OnDetach();

                if (border == null)
                    throw new InvalidOperationException();

                Control.VisualChildren.Remove(border);
                border.Dispose();
            }
        }
    }
}