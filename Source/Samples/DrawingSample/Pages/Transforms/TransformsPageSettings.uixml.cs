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

        public void Initialize(TransformsPage page)
        {
            DataContext = page;
        }
    }
}