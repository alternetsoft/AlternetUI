﻿using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;
using System.Linq;

namespace ControlsSample
{
    partial class MainWindow : Window, IPageSite
    {
        ListBox eventsListBox;

        public MainWindow()
        {
            InitializeComponent();

            eventsListBox = new ListBox();

            Width = 800;
            Height = 600;

            var pc = new PageContainer();
            var rootPanel = new Grid();
            rootPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            rootPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            Children.Add(rootPanel);

            rootPanel.Children.Add(pc);
            Grid.SetRow(pc, 0);

            pc.Pages.Add(new PageContainer.Page("Tree View", new TreeViewPage(this)));
            pc.Pages.Add(new PageContainer.Page("Grid", new GridPage(this)));
            pc.Pages.Add(new PageContainer.Page("List View", new ListViewPage(this)));
            pc.Pages.Add(new PageContainer.Page("List Box", new ListBoxPage(this)));
            pc.Pages.Add(new PageContainer.Page("Combo Box", new ComboBoxPage(this)));

            var progressBarPage = new Control();
            InitProgressBarPage(progressBarPage);
            pc.Pages.Add(new PageContainer.Page("Progress Bar", progressBarPage));

            var sliderPage = new Control();
            InitSliderPage(sliderPage);
            pc.Pages.Add(new PageContainer.Page("Slider", sliderPage));

            var numericInputPage = new Control();
            InitNumericInputPage(numericInputPage);
            pc.Pages.Add(new PageContainer.Page("Numeric Input", numericInputPage));

            var radioButtonsPage = new Control();
            InitRadioButtonsPage(radioButtonsPage);
            pc.Pages.Add(new PageContainer.Page("Radio Buttons", radioButtonsPage));

            var checkBoxesPage = new Control();
            InitCheckBoxesPage(checkBoxesPage);
            pc.Pages.Add(new PageContainer.Page("Check Boxes", checkBoxesPage));

            var textBoxesPage = new Control();
            InitTextBoxesPage(textBoxesPage);
            pc.Pages.Add(new PageContainer.Page("Text Boxes", textBoxesPage));

            eventsListBox.Height = 100;
            rootPanel.Children.Add(eventsListBox);
            Grid.SetRow(eventsListBox, 1);

            pc.SelectedIndex = 0;
        }

        private static void InitProgressBarPage(Control page)
        {
            var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };

            var groupBox1 = new GroupBox { Title = "Regular Progress Bars" };
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);

            panel2.Children.Add(new Label() { Text = "0 [0..100]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Value = 0, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new Label() { Text = "50 [0..100]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Value = 50, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new Label() { Text = "100 [0..100]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Value = 100, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new Label() { Text = "125 [50..200]", Margin = new Thickness(0, 10, 0, 5) });
            panel2.Children.Add(new ProgressBar() { Minimum = 50, Maximum = 200, Value = 125, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new Label() { Text = "Custom height, 50 [0..100]", Margin = new Thickness(0, 10, 0, 5) });
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

        private static void InitSliderPage(Control page)
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

            panel2.Children.Add(new Label() { Text = "0 [0..10]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new Slider() { Margin = new Thickness(0, 0, 0, 5) }));

            panel2.Children.Add(new Label() { Text = "5 [0..20]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new Slider() { Value = 5, Maximum = 20, Margin = new Thickness(0, 0, 0, 5) }));

            panel2.Children.Add(new Label() { Text = "125 [50..200]", Margin = new Thickness(0, 10, 0, 5) });
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

        private static void InitNumericInputPage(Control page)
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

            panel2.Children.Add(new Label() { Text = "0 [0..10]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new NumericUpDown() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) }));

            panel2.Children.Add(new Label() { Text = "5 [-20..20]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(AddMessageBoxHandler(new NumericUpDown() { Value = 5, Minimum = -20, Maximum = 20, Margin = new Thickness(0, 0, 0, 5) }));

            panel2.Children.Add(new Label() { Text = "125 [50..200]", Margin = new Thickness(0, 10, 0, 5) });
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

        private static void InitTextBoxesPage(Control page)
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

        private static void InitRadioButtonsPage(Control page)
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

        private static void InitCheckBoxesPage(Control page)
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

        int lastEventNumber = 1;

        void IPageSite.LogEvent(string message)
        {
            eventsListBox.Items.Add($"{lastEventNumber++}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }
    }
}