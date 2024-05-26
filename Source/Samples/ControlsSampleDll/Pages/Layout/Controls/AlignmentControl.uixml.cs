using System;
using System.Linq;
using Alternet.UI;

namespace LayoutSample
{
    public partial class AlignmentControl : Control
    {
        public AlignmentControl()
        {
            InitializeComponent();

            verticalAlignmentComboBox.Items.AddRange(Enum.GetValues(typeof(VerticalAlignment)).Cast<object>());
            horizontalAlignmentComboBox.Items.AddRange(Enum.GetValues(typeof(HorizontalAlignment)).Cast<object>());
        }

        Control? control;

        public Control? Control
        {
            get
            {
                return control;
            }

            set
            {
                if (control == value)
                    return;

                control = value;

                if (control == null)
                    return;

                horizontalAlignmentComboBox.SelectedItem = control.HorizontalAlignment;
                verticalAlignmentComboBox.SelectedItem = control.VerticalAlignment;
            }
        }

        private void HorizontalAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (control == null)
                return;

            control.HorizontalAlignment = (HorizontalAlignment)horizontalAlignmentComboBox.SelectedItem!;
        }

        private void VerticalAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (control == null)
                return;

            control.VerticalAlignment = (VerticalAlignment)verticalAlignmentComboBox.SelectedItem!;
        }
    }
}