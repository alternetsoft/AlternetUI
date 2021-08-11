using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    internal class MainWindow : Window
    {
        private CanvasControl canvasControl;

        private StackPanel checkBoxesPanel;

        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("DrawingSample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            canvasControl = (CanvasControl)FindControl("canvasControl");
            checkBoxesPanel = (StackPanel)FindControl("checkBoxesPanel");

            FindControl("clearButton").Click += ClearButton_Click;
            InitializeCheckBoxes();
        }

        private void InitializeCheckBoxes()
        {
            foreach (var layer in CanvasControl.Layers)
            {
                var checkBox = new CheckBox
                {
                    Text = layer.Name,
                    Tag = layer,
                    Margin = new Thickness(0, 0, 0, 7)
                };

                checkBox.CheckedChanged += LayerCheckBox_CheckedChanged;
                checkBoxesPanel.Children.Add(checkBox);
            }
        }

        private void LayerCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            var checkBox = sender as CheckBox ?? throw new Exception();
            var layer = (Layer)(checkBox.Tag ?? throw new Exception());

            if (checkBox.IsChecked)
                canvasControl.ShowLayer(layer);
            else
                canvasControl.HideLayer(layer);
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            foreach (CheckBox checkBox in checkBoxesPanel.Children)
                checkBox.IsChecked = false;
        }
    }
}