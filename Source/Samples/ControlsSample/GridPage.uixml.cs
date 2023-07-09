using System;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class GridPage : Control
    {
        private IPageSite? site;

        public GridPage()
        {
            InitializeComponent();
        }

        private void BackgroundButton_Click(object sender, System.EventArgs e)
        {
            if (mainGrid.Background == null)
                mainGrid.Background = new SolidBrush(Color.Olive);
            else
                mainGrid.Background = null;
            mainGrid.Invalidate();
        }


        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }
    }
}