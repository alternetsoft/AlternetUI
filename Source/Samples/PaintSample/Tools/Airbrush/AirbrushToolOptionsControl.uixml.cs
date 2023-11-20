using Alternet.UI;
using System;

namespace PaintSample
{
    partial class AirbrushToolOptionsControl : Control
    {
        public AirbrushToolOptionsControl()
        {
            InitializeComponent();

            sizeNumericUpDown.BindValue("Size");
            flowNumericUpDown.BindValue("Flow");
        }

        public AirbrushTool? Tool
        {
            get => (AirbrushTool?)DataContext;
            set => DataContext = value;
        }
    }
}