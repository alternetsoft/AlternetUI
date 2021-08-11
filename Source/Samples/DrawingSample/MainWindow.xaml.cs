using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Image = Alternet.UI.Image;

namespace DrawingSample
{
    internal class MainWindow : Window
    {
        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("DrawingSample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            canvasControl = (CanvasControl)FindControl("canvasControl");
            
            FindControl("clearButton").Click += ClearButton_Click;

            InitializeCheckBoxes((StackPanel)FindControl("checkBoxesPanel"));
        }

        private void InitializeCheckBoxes(StackPanel panel)
        {
            foreach (var layer in Enum.GetValues(typeof(CanvasControl.Layer)).Cast<CanvasControl.Layer>())
            {
                var checkBox = new CheckBox { Text = layer.ToString(), Tag = layer, Margin = new Thickness(0, 0, 0, 7) };
                checkBox.CheckedChanged += LayerCheckBox_CheckedChanged;
                panel.Children.Add(checkBox);
            }
        }

        private void LayerCheckBox_CheckedChanged(object? sender, EventArgs e)
        {

        }

        CanvasControl canvasControl;

        private void ClearButton_Click(object? sender, EventArgs e)
        {
        }
    }
}