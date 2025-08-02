using Alternet.UI;
using DrawingSample.RandomArt;
using System;

namespace DrawingSample
{
    partial class TransformsPageSettings : Panel
    {
        public TransformsPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(TransformsPage page)
        {
            DataContext = page;

            if (App.IsMacOS)
            {
                translationXSlider.IsEnabled = false;
                translationYSlider.IsEnabled = false;
                scaleFactorXSlider.IsEnabled = false;
                scaleFactorYSlider.IsEnabled = false;
                rotationSlider.IsEnabled = false;
            }

            resetButton.Click += (s, e) =>
            {
                translationXSlider.Value = 0;
                translationYSlider.Value = 0;
                scaleFactorXSlider.Value = 0;
                scaleFactorYSlider.Value = 0;
                rotationSlider.Value = 0;
            };

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

            GetChildrenRecursive().Action<StdSlider>((c) => c.ClearTicks());
        }
    }
}