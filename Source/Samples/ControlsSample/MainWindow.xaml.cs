using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace ControlsSample
{
    internal class MainWindow : Window
    {
        private RadioButton option1RadioButton;

        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ControlsSample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            Width = 600;
            Height = 600;

#if NETCOREAPP
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                Width = 750;
#endif

            var tc = new TabControl();
            Children.Add(tc);

            var progressBarPage = new TabPage { Title = "Progress Bar" };
            tc.Pages.Add(progressBarPage);
            InitProgressBarPage(progressBarPage);

            var sliderPage = new TabPage { Title = "Slider" };
            tc.Pages.Add(sliderPage);
            InitSliderPage(sliderPage);

            var numericInputPage = new TabPage { Title = "Numeric Input" };
            tc.Pages.Add(numericInputPage);
            InitNumericInputPage(numericInputPage);

            var radioButtonsPage = new TabPage { Title = "Radio Buttons" };
            tc.Pages.Add(radioButtonsPage);
            InitRadioButtonsPage(radioButtonsPage);

            var checkBoxesPage = new TabPage { Title = "Check Boxes" };
            tc.Pages.Add(checkBoxesPage);
            InitCheckBoxesPage(checkBoxesPage);

            var textBoxesPage = new TabPage { Title = "Text Boxes" };
            tc.Pages.Add(textBoxesPage);
            InitTextBoxesPage(textBoxesPage);

            // option1RadioButton = (RadioButton)FindControl("option1RadioButton");

            // option1RadioButton.CheckedChanged += Option1RadioButton_CheckedChanged;
        }

        private static void InitProgressBarPage(TabPage page)
        {
            var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };

            var groupBox1 = new GroupBox { Title = "Regular Progress Bars" };
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);

            panel2.Children.Add(new TextBlock() { Text = "0 [0..100]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Value = 0, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new TextBlock() { Text = "50 [0..100]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Value = 50, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new TextBlock() { Text = "100 [0..100]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Value = 100, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new TextBlock() { Text = "125 [50..200]", Margin = new Thickness(0, 10, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Minimum = 50, Maximum = 200, Value = 125, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new TextBlock() { Text = "Custom height, 50 [0..100]", Margin = new Thickness(0, 10, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Value = 50, Margin = new Thickness(0, 0, 0, 5), Height = 25 });

            var increaseAllButton = new Button() { Text = "Increase All", Margin = new Thickness(0, 10, 0, 5) };
            panel2.Children.Add(increaseAllButton);
            increaseAllButton.Click += (o, e) =>
            {
                foreach (var progressBar in panel2.Children.OfType<ProgressBar>())
                {
                    if (progressBar.Value < progressBar.Maximum)
                        progressBar.Value++;
                }
            };

            panel.Children.Add(groupBox1);

            page.Children.Add(panel);
        }

        private static void InitSliderPage(TabPage page)
        {
            var enableMessageHandlingCheckBox = new CheckBox { Text = "Enable value change event handling", IsChecked = true };
            Slider AddMessageBoxHandler(Slider slider)
            {
                slider.ValueChanged += (o, e) =>
                {
                    if (!enableMessageHandlingCheckBox.IsChecked)
                        return;

                    MessageBox.Show("New value is: " + ((Slider)o!).Value, "Slider Value Changed");
                };
                return slider;
            }

            var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };

            var groupBox1 = new GroupBox { Title = "Horizontal Sliders" };
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);

            panel2.Children.Add(new TextBlock() { Text = "0 [0..10]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new Slider() { Margin = new Thickness(0, 0, 0, 5) }));

            panel2.Children.Add(new TextBlock() { Text = "5 [0..20]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new Slider() { Value = 5, Maximum = 20, Margin = new Thickness(0, 0, 0, 5) }));

            panel2.Children.Add(new TextBlock() { Text = "125 [50..200]", Margin = new Thickness(0, 10, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new Slider() { Minimum = 50, Maximum = 200, Value = 125, Margin = new Thickness(0, 0, 0, 5) }));

            var increaseAllButton = new Button() { Text = "Increase All", Margin = new Thickness(0, 10, 0, 5) };
            panel2.Children.Add(increaseAllButton);
            increaseAllButton.Click += (o, e) =>
            {
                foreach (var slider in panel2.Children.OfType<Slider>())
                {
                    if (slider.Value < slider.Maximum)
                        slider.Value++;
                }
            };

            panel2.Children.Add(enableMessageHandlingCheckBox);

            panel.Children.Add(groupBox1);

            var groupBox2 = new GroupBox { Title = "Linked Progress Bar", Margin = new Thickness(0, 10, 0, 0) };
            var panel3 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox2.Children.Add(panel3);

            var progressBarControlSlider = new Slider() { Margin = new Thickness(0, 0, 0, 5) };
            var progressBar = new ProgressBar() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) };

            progressBarControlSlider.ValueChanged += (o, e) => progressBar.Value = progressBarControlSlider.Value;
            progressBarControlSlider.Value = 1;

            panel3.Children.Add(progressBarControlSlider);
            panel3.Children.Add(progressBar);

            panel.Children.Add(groupBox2);

            page.Children.Add(panel);
        }

        private static void InitNumericInputPage(TabPage page)
        {
            var enableMessageHandlingCheckBox = new CheckBox { Text = "Enable value change event handling", IsChecked = true };
            NumericUpDown AddMessageBoxHandler(NumericUpDown control)
            {
                control.ValueChanged += (o, e) =>
                {
                    if (!enableMessageHandlingCheckBox.IsChecked)
                        return;

                    MessageBox.Show("New value is: " + ((NumericUpDown)o!).Value, "NumericUpDown Value Changed");
                };
                return control;
            }

            var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };

            var groupBox1 = new GroupBox { Title = "Integer Numeric Input" };
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);

            panel2.Children.Add(new TextBlock() { Text = "0 [0..10]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new NumericUpDown() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) }));

            panel2.Children.Add(new TextBlock() { Text = "5 [-20..20]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new NumericUpDown() { Value = 5, Minimum = -20, Maximum = 20, Margin = new Thickness(0, 0, 0, 5) }));

            panel2.Children.Add(new TextBlock() { Text = "125 [50..200]", Margin = new Thickness(0, 10, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new NumericUpDown() { Minimum = 50, Maximum = 200, Value = 125, Margin = new Thickness(0, 0, 0, 5) }));

            var increaseAllButton = new Button() { Text = "Increase All", Margin = new Thickness(0, 10, 0, 5) };
            panel2.Children.Add(increaseAllButton);
            increaseAllButton.Click += (o, e) =>
            {
                foreach (var control in panel2.Children.OfType<NumericUpDown>())
                {
                    if (control.Value < control.Maximum)
                        control.Value++;
                }
            };

            panel2.Children.Add(enableMessageHandlingCheckBox);

            panel.Children.Add(groupBox1);

            var groupBox2 = new GroupBox { Title = "Linked Progress Bar", Margin = new Thickness(0, 10, 0, 0) };
            var panel3 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox2.Children.Add(panel3);

            var progressBarControlNumericUpDown = new NumericUpDown() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) };
            var progressBar = new ProgressBar() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) };

            progressBarControlNumericUpDown.ValueChanged += (o, e) => progressBar.Value = (int)progressBarControlNumericUpDown.Value;
            progressBarControlNumericUpDown.Value = 1;

            panel3.Children.Add(progressBarControlNumericUpDown);
            panel3.Children.Add(progressBar);

            panel.Children.Add(groupBox2);

            page.Children.Add(panel);
        }

        private static void InitTextBoxesPage(TabPage page)
        {
            var enableMessageHandlingCheckBox = new CheckBox { Text = "Enable text change event handling", IsChecked = true };
            TextBox AddMessageBoxHandler(TextBox control)
            {
                control.TextChanged += (o, e) =>
                {
                    if (!enableMessageHandlingCheckBox.IsChecked)
                        return;

                    MessageBox.Show("New value is: " + ((TextBox)o!).Text, "TextBox Text Changed");
                };
                return control;
            }

            var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };

            var groupBox1 = new GroupBox { Title = "Text Input" };
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);

            panel2.Children.Add(AddMessageBoxHandler(new TextBox() { Text = "Text 1.1", Margin = new Thickness(0, 0, 0, 5) }));
            panel2.Children.Add(AddMessageBoxHandler(new TextBox() { Text = "Text 1.2", Margin = new Thickness(0, 0, 0, 5) }));
            panel2.Children.Add(AddMessageBoxHandler(new TextBox() { Text = "Text 1.3", Margin = new Thickness(0, 0, 0, 5) }));

            var addLetterAButton = new Button() { Text = "Add Letter \"A\"", Margin = new Thickness(0, 10, 0, 5) };
            panel2.Children.Add(addLetterAButton);
            addLetterAButton.Click += (o, e) =>
            {
                foreach (var control in panel2.Children.OfType<TextBox>())
                    control.Text += "A";
            };

            panel2.Children.Add(enableMessageHandlingCheckBox);

            panel.Children.Add(groupBox1);

            page.Children.Add(panel);
        }

        private static void InitRadioButtonsPage(TabPage page)
        {
            var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };

            var groupBox1 = new GroupBox { Title = "Radio Group 1" };
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);

            panel2.Children.Add(new RadioButton() { Text = "Option 1.1", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new RadioButton() { Text = "Option 1.2", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new RadioButton() { Text = "Option 1.3", Margin = new Thickness(0, 0, 0, 5) });

            panel.Children.Add(groupBox1);

            var groupBox2 = new GroupBox { Title = "Radio Group 2", Margin = new Thickness(0, 10, 0, 0) };
            var panel3 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox2.Children.Add(panel3);

            panel3.Children.Add(new RadioButton() { Text = "Option 2.1", Margin = new Thickness(0, 0, 0, 5) });
            panel3.Children.Add(new RadioButton() { Text = "Option 2.2", Margin = new Thickness(0, 0, 0, 5) });
            panel3.Children.Add(new RadioButton() { Text = "Option 2.3", Margin = new Thickness(0, 0, 0, 5) });

            panel.Children.Add(groupBox2);

            page.Children.Add(panel);
        }

        private static void InitCheckBoxesPage(TabPage page)
        {
            var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };

            var groupBox1 = new GroupBox { Title = "Option Group 1" };
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);

            panel2.Children.Add(new CheckBox() { Text = "Option 1.1", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new CheckBox() { Text = "Option 1.2", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new CheckBox() { Text = "Option 1.3", Margin = new Thickness(0, 0, 0, 5) });

            panel.Children.Add(groupBox1);

            var groupBox2 = new GroupBox { Title = "Option Group 2", Margin = new Thickness(0, 10, 0, 0) };
            var panel3 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox2.Children.Add(panel3);

            panel3.Children.Add(new CheckBox() { Text = "Option 2.1", Margin = new Thickness(0, 0, 0, 5) });
            panel3.Children.Add(new CheckBox() { Text = "Option 2.2", Margin = new Thickness(0, 0, 0, 5) });
            panel3.Children.Add(new CheckBox() { Text = "Option 2.3", Margin = new Thickness(0, 0, 0, 5) });

            panel.Children.Add(groupBox2);

            page.Children.Add(panel);
        }

        private void Option1RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            // MessageBox.Show(option1RadioButton.IsChecked.ToString(), "Option 1");
        }
    }
}