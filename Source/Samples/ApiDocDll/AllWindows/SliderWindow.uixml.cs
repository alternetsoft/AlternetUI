#pragma warning disable
using Alternet.UI;
using System;

namespace ApiDoc
{
    [ControlCategory("Internal")]
    public partial class SliderWindow : Window
    {
        public SliderWindow()
        {
            InitializeComponent();
        }

        public void SliderExample1()
        {
            #region SliderCSharpCreation
            var Slider = new Alternet.UI.StdSlider()
            {
                Minimum = 50,
                Maximum = 200,
                Value = 125,
                TickFrequency = 10,                
                Margin = new Thickness(0, 0, 0, 5),
            };
            #endregion
        }

        #region SliderEventHandler
        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            Title = slider.Value.ToString();
        }
        #endregion    
    }
}