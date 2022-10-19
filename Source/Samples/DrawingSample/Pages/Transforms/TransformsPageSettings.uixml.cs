using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class TransformsPageSettings : Control
    {
        public TransformsPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(TransformsPage page)
        {
            DataContext = page;
        }
    }
}