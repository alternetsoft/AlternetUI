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

            translationXSlider.Value = page.TranslationX;
            translationXSlider.ValueChanged += (s, e) =>
            {
                page.TranslationX = translationXSlider.Value;
            };

            translationYSlider.Value = page.TranslationY;
            translationYSlider.ValueChanged += (s, e) =>
            {
                page.TranslationY = translationYSlider.Value;
            };

            scaleFactorXSlider.Value = page.ScaleFactorX;
            scaleFactorXSlider.ValueChanged += (s, e) =>
            {
                page.ScaleFactorX = scaleFactorXSlider.Value;
            };

            scaleFactorYSlider.Value = page.ScaleFactorY;
            scaleFactorYSlider.ValueChanged += (s, e) =>
            {
                page.ScaleFactorY = scaleFactorYSlider.Value;
            };

            rotationSlider.Value = page.Rotation;
            rotationSlider.ValueChanged += (s, e) =>
            {
                page.Rotation = rotationSlider.Value;
            };

            GetChildrenRecursive().Action<Slider>((c) => c.ClearTicks());
        }
    }
}