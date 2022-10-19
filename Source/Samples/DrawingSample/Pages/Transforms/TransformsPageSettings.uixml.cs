using Alternet.UI;
using DrawingSample.RandomArt;
using System;

namespace DrawingSample
{
    partial class TransformsPageSettings : Control
    {
        public TransformsPageSettings()
        {
            InitializeComponent();
        }

        TransformsPage page;

        public void Initialize(TransformsPage page)
        {
            this.page = page;
            DataContext = page;
        }
    }
}