using Alternet.UI;
using System;

namespace PaintSample
{
    partial class AirbrushToolOptionsControl : Panel
    {
        public AirbrushToolOptionsControl()
        {
            InitializeComponent();
        }

        public AirbrushTool? Tool
        {
            get => (AirbrushTool?)DataContext;

            set
            {
                sizeNumericUpDown.ValueChanged -= SizeNumericUpDown_ValueChanged;
                flowNumericUpDown.ValueChanged -= FlowNumericUpDown_ValueChanged;
                DataContext = value;
                if(Tool is not null)
                {
                    sizeNumericUpDown.Value = (int)Tool.Size;
                    flowNumericUpDown.Value = (int)Tool.Flow;
                    sizeNumericUpDown.ValueChanged += SizeNumericUpDown_ValueChanged;
                    flowNumericUpDown.ValueChanged += FlowNumericUpDown_ValueChanged;
                }
            }
        }

        private void FlowNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            if(Tool is not null)
                Tool.Flow = flowNumericUpDown.Value;
        }

        private void SizeNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            if (Tool is not null)
                Tool.Size = sizeNumericUpDown.Value;
        }
    }
}