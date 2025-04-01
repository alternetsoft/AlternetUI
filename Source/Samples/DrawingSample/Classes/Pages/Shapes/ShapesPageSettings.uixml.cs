using System;
using Alternet.UI;

namespace DrawingSample
{
    partial class ShapesPageSettings : Panel
    {
        private ShapesPage? page;

        public ShapesPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(ShapesPage page)
        {
            brushColorCombo.Value = page.BrushColor;
            penColorCombo.Value = page.PenColor;
            this.page = page;
        }

        private void BrushColorCombo_Changed(object? sender, EventArgs e)
        {
            if(page is not null && brushColorCombo.Value is not null)
                page.BrushColor = brushColorCombo.Value;
        }

        private void PenColorCombo_Changed(object? sender, EventArgs e)
        {
            if (page is not null && penColorCombo.Value is not null)
                page.BrushColor = penColorCombo.Value;
        }
    }
}