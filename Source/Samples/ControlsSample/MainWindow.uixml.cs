using Alternet.UI;
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
            pc.Pages.Add(new PageContainer.Page("Progress Bar", new ProgressBarPage(this)));
            pc.Pages.Add(new PageContainer.Page("Slider", new SliderPage(this)));
            pc.Pages.Add(new PageContainer.Page("Numeric Input", new NumericInputPage(this)));

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