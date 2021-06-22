﻿using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

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

            var tc = new TabControl();
            Children.Add(tc);

            var progressBarPage = new TabPage { Title = "Progress Bar" };
            tc.Pages.Add(progressBarPage);
            InitProgressBarPage(progressBarPage);

            var sliderPage = new TabPage { Title = "Slider" };
            tc.Pages.Add(sliderPage);
            InitSliderPage(sliderPage);

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

            panel.Children.Add(groupBox1);

            page.Children.Add(panel);
        }

        private static void InitSliderPage(TabPage page)
        {
            var panel = new StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };

            var groupBox1 = new GroupBox { Title = "Horizontal Sliders" };
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);

            panel2.Children.Add(new TextBlock() { Text = "0 [0..10]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new Slider() { Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new TextBlock() { Text = "5 [0..20]", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new Slider() { Value = 5, Maximum = 20, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new TextBlock() { Text = "125 [50..200]", Margin = new Thickness(0, 10, 0, 5) });
            panel2.Children.Add(new Slider() { Minimum = 50, Maximum = 200, Value = 125, Margin = new Thickness(0, 0, 0, 5) });

            panel2.Children.Add(new TextBlock() { Text = "Custom height, 50 [0..100]", Margin = new Thickness(0, 10, 0, 5) });
            panel2.Children.Add(new Slider() { Value = 50, Maximum = 100, Margin = new Thickness(0, 0, 0, 5), Height = 25 });

            panel.Children.Add(groupBox1);

            page.Children.Add(panel);
        }

        private void Option1RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            // MessageBox.Show(option1RadioButton.IsChecked.ToString(), "Option 1");
        }
    }
}