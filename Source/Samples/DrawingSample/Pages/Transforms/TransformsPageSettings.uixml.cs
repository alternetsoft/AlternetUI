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
            translationXSlider.BindValue(nameof(TransformsPage.TranslationX));
            translationYSlider.BindValue(nameof(TransformsPage.TranslationY));
            scaleFactorXSlider.BindValue(nameof(TransformsPage.ScaleFactorX));
            scaleFactorYSlider.BindValue(nameof(TransformsPage.ScaleFactorY));
            rotationSlider.BindValue(nameof(TransformsPage.Rotation));
            GetChildrenRecursive().Action<Slider>((c) => c.ClearTicks());
        }

        public void Initialize(TransformsPage page)
        {
            DataContext = page;
        }
    }
}