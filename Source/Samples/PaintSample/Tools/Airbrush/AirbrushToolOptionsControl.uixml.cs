using Alternet.UI;
using System;

namespace PaintSample
{
    internal partial class AirbrushToolOptionsControl : Control
    {
        private AirbrushTool? tool;

        public AirbrushToolOptionsControl()
        {
            InitializeComponent();
        }

        public AirbrushTool? Tool
        {
            get => tool;
            set
            {
                if (tool == value)
                    return;

                tool = value;

                if (tool != null)
                {
                    sizeNumericUpDown.Value = (decimal)tool.Size;
                    flowNumericUpDown.Value = (decimal)tool.Flow;
                }
            }
        }

        private void SizeNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            if (tool != null)
                tool.Size = (double)sizeNumericUpDown.Value;
        }
        
        private void FlowNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            if (tool != null)
                tool.Flow = (double)flowNumericUpDown.Value;
        }
    }
}