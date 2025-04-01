using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;

namespace PaintSample
{
    partial class PenLikeToolOptionsControl : Panel
    {
        public PenLikeToolOptionsControl()
        {
            InitializeComponent();
        }

        public PenLikeTool? Tool
        {
            get => (PenLikeTool?)DataContext;
            set
            {
                thicknessNumericUpDown.ValueChanged -= ThicknessNumericUpDown_ValueChanged;
                DataContext = value;
                if(value != null)
                {
                    thicknessNumericUpDown.Value = (int)value.Thickness;
                    thicknessNumericUpDown.ValueChanged += ThicknessNumericUpDown_ValueChanged;
                }
            }
        }

        private void ThicknessNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            if(Tool is not null)
                Tool.Thickness = thicknessNumericUpDown.Value;
        }
    }
}