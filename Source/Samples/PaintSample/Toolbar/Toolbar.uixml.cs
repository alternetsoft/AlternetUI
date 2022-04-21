using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    internal partial class Toolbar : Control
    {
        private List<ToolButton> toolButtons = new List<ToolButton>();

        private Tools? tools;

        public Toolbar()
        {
            InitializeComponent();

            UserPaint = true;
        }

        public void SetTools(Tools tools)
        {
            if (this.tools != null)
                throw new InvalidOperationException();

            this.tools = tools;

            foreach (var tool in tools.AllTools)
            {
                var button = new ToolButton(tool)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                button.ToggledChanged += Button_ToggledChanged;

                toolButtons.Add(button);
                container.Children.Add(button);
            }

            UpdateCurrentTool();
            tools.CurrentToolChanged += Tools_CurrentToolChanged;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;

            dc.DrawLine(Pens.Black, e.Bounds.TopRight + new Size(-1, 0), e.Bounds.BottomRight + new Size(-1, 0));
        }

        private void Tools_CurrentToolChanged(object? sender, EventArgs e)
        {
            UpdateCurrentTool();
        }

        private void UpdateCurrentTool()
        {
            if (tools == null)
                throw new InvalidOperationException();

            skipToggledEvent = true;

            foreach (var button in toolButtons.Where(x => x.IsToggled))
                button.IsToggled = false;

            toolButtons.First(x => x.Tool == tools.CurrentTool).IsToggled = true;
            
            skipToggledEvent = false;
        }

        bool skipToggledEvent;

        private void Button_ToggledChanged(object? sender, EventArgs e)
        {
            if (skipToggledEvent)
                return;

            if (tools == null)
                throw new InvalidOperationException();

            tools.CurrentTool = ((ToolButton)sender!).Tool;
        }
    }
}