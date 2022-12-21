using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class ScrollingPage : Control
    {
        private IPageSite? site;

        public ScrollingPage()
        {
            InitializeComponent();

            InitializeComboBoxes();

            InitializeStackPanel();
        }

        private void InitializeStackPanel()
        {
            stackPanel.BeginInit();
            for (int i = 0; i < 50; i++)
                AddControlToStackPanel();
            stackPanel.EndInit();
        }

        private void AddControlToStackPanel()
        {
            stackPanel.Children.Add(new Button("Button " + (stackPanel.Children.Count + 1)));
        }

        private void InitializeComboBoxes()
        {
            InitializeEnumComboBox(orientationComboBox, StackPanelOrientation.Vertical);
            InitializeEnumComboBox(stackPanelVerticalAlignmentComboBox, VerticalAlignment.Stretch);
            InitializeEnumComboBox(stackPanelHorizontalAlignmentComboBox, HorizontalAlignment.Stretch);
        }

        private void InitializeEnumComboBox<TEnum>(ComboBox comboBox, TEnum defaultValue) where TEnum : Enum
        {
            foreach (var item in Enum.GetValues(typeof(TEnum)))
                comboBox.Items.Add(item ?? throw new Exception());
            comboBox.SelectedItem = defaultValue;
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void OrientationComboBox_SelectedItemChanged(object? sender, System.EventArgs e)
        {
            stackPanel.Orientation = (StackPanelOrientation)orientationComboBox.SelectedItem!;
        }

        private void StackPanelVerticalAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            stackPanel.VerticalAlignment = (VerticalAlignment)stackPanelVerticalAlignmentComboBox.SelectedItem!;
        }

        private void StackPanelHorizontalAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            stackPanel.HorizontalAlignment = (HorizontalAlignment)stackPanelHorizontalAlignmentComboBox.SelectedItem!;
        }

        private void AddControlToStackPanelButton_Click(object? sender, EventArgs e)
        {
            AddControlToStackPanel();
        }

        private void RemoveControlFromStackPanelButton_Click(object? sender, EventArgs e)
        {
            int count = stackPanel.Children.Count;
            if (count > 0)
                stackPanel.Children.RemoveAt(count - 1);
        }
    }
}