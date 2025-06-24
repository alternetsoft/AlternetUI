using System;
using System.Linq;
using Alternet.UI;

namespace LayoutSample
{
    public partial class AlignmentControl : Panel
    {
        public AlignmentControl()
        {
            InitializeComponent();

            verticalAlignmentComboBox.EnumType = typeof(VerticalAlignment);
            horizontalAlignmentComboBox.EnumType = typeof(HorizontalAlignment);
        }

        AbstractControl? control;

        public AbstractControl? Control
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

                horizontalAlignmentComboBox.Value = control.HorizontalAlignment;
                verticalAlignmentComboBox.Value = control.VerticalAlignment;
            }
        }

        private void HorizontalAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (control == null || horizontalAlignmentComboBox.Value is null)
                return;

            control.HorizontalAlignment = (HorizontalAlignment)horizontalAlignmentComboBox.Value;
        }

        private void VerticalAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (control == null || verticalAlignmentComboBox.Value is null)
                return;

            control.VerticalAlignment = (VerticalAlignment)verticalAlignmentComboBox.Value;
        }
    }
}