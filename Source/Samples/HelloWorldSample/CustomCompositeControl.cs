using Alternet.UI;
using System;
using System.Drawing;

namespace HelloWorldSample
{
    internal class CustomCompositeControl : Control
    {
        protected override ControlHandler CreateHandler() => new CustomHandler();

        private class CustomHandler : ControlHandler<CustomCompositeControl>
        {
            private Border? border;

            protected override void OnAttach()
            {
                base.OnAttach();

                border = new Border
                {
                    BackgroundColor = Color.FromArgb(unchecked((int)0xFFACAAFF)),
                    BorderColor = Color.FromArgb(unchecked((int)0xFF4A47FF))
                };

                Control.VisualChildren.Add(border);

                var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical };
                border.VisualChildren.Add(panel);

                panel.VisualChildren.Add(new TextBlock { Text = "Composite Control" });
                panel.VisualChildren.Add(new TextBlock { Text = "Text 1" });
                panel.VisualChildren.Add(new TextBlock { Text = "Text 2" });
                panel.VisualChildren.Add(new TextBlock { Text = "Text 3" });
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