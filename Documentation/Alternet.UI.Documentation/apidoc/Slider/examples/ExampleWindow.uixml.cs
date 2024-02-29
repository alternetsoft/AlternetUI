using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.Slider
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SliderExample1()
        {
            #region SliderCSharpCreation
            var Slider = new Alternet.UI.Slider()
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