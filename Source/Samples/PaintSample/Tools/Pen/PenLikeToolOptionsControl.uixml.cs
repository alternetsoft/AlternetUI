using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;

namespace PaintSample
{
    partial class PenLikeToolOptionsControl : Control
    {
        public PenLikeToolOptionsControl()
        {
            InitializeComponent();
        }

        private PenLikeTool? tool;

        public PenLikeTool? Tool
        {
            get => tool;
            set
            {
                if (tool == value)
                    return;

                tool = value;

                if (tool != null)
                    thicknessNumericUpDown.Value = (decimal)tool.Thickness;
            }
        }

        void ThicknessNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            if (tool != null)
                tool.Thickness = (double)thicknessNumericUpDown.Value;
        }
    }
}