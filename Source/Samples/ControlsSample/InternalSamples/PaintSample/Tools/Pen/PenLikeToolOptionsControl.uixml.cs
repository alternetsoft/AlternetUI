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

            thicknessNumericUpDown.BindValue("Thickness");
        }

        public PenLikeTool? Tool
        {
            get => (PenLikeTool?)DataContext;
            set => DataContext = value;
        }
    }
}