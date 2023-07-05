using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    public static class LayoutFactory
    {
        public static void AdjustTextBoxesHeight(Control container)
        {
            if (container == null)
                return;

            Control? comboBox = null;
            Control? textBox = null;

            FindTextEditors(container);
            if (comboBox == null)
                return;
            AdjustTextBoxesHeight(container, comboBox, textBox);

            void FindTextEditors(Control container)
            {
                if (comboBox != null && textBox != null)
                    return;
                foreach (Control control in container.Children)
                {
                    if (control is TextBox box)
                        textBox = box;
                    else
                    if (control is ComboBox box1)
                        comboBox = box1;
                    else
                        FindTextEditors(control);
                    if (comboBox != null && textBox != null)
                        return;
                }
            }
        }

        public static void PerformLayoutLeftFill(
            Control container,
            Control leftControl,
            Control fillControl)
        {
            IArrangedElement result = CreateLayoutLeftFill(
                CreateContainer(FromControl(container)),
                FromControl(leftControl),
                FromControl(fillControl));
            result.PerformLayout();
        }

        public static void SetDebugBackgroundToParents(Control? control)
        {
            static Control? SetParentBackground(Control? control, Brush brush)
            {
                if (control == null)
                    return null;
                Control? parent = control?.Parent;
                if (parent != null)
                    parent.Background = brush;
                return parent;
            }

            control = SetParentBackground(control, new SolidBrush(Color.Green));
            control = SetParentBackground(control, new SolidBrush(Color.Yellow));
            control = SetParentBackground(control, new SolidBrush(Color.Blue));
            SetParentBackground(control, new SolidBrush(Color.Red));
        }

        internal static IArrangedElementLite FromControl(Control control)
        {
            return new ArrangedElementControl(control);
        }

        internal static IArrangedElement CreateContainer(
            IArrangedElementLite? container = null)
        {
            IArrangedElement result = new ArrangedElement(null, container);
            return result;
        }

        internal static IArrangedElement AddChild(
            IArrangedElement container,
            IArrangedElementLite? control = null)
        {
            IArrangedElement result = new ArrangedElement(container, control);
            result.Controls.Add(result);
            return result;
        }

        internal static IArrangedElement CreateLayoutLeftFill(
            IArrangedElement container,
            IArrangedElementLite leftControl,
            IArrangedElementLite fillControl)
        {
            var leftControlElement = AddChild(container, leftControl);
            var fillControlElement = AddChild(container, fillControl);
            leftControlElement.Dock = DockStyle.Left;
            fillControlElement.Dock = DockStyle.Fill;
            return container;
        }

        internal static void AdjustTextBoxesHeight(
            Control container,
            Control comboBox,
            Control? textBox)
        {
            var comboBoxHeight = comboBox.Bounds.Height;
            double textBoxHeight = 0;
            if (textBox != null)
            {
                textBoxHeight = textBox.Bounds.Height;
                if (comboBoxHeight == textBoxHeight)
                    return;
            }

            var maxHeight = Math.Max(comboBoxHeight, textBoxHeight);

            container.SuspendLayout();

            try
            {
                var editors = new Collection<Control>();
                AddTextEditors(container);

                foreach (Control control in editors)
                    control.Height = maxHeight;

                void AddTextEditors(Control container)
                {
                    foreach (Control control in container.Children)
                    {
                        if (control is TextBox || control is ComboBox)
                        {
                            if (control.Bounds.Height < maxHeight)
                                editors.Add(control);
                        }
                        else
                            AddTextEditors(control);
                    }
                }
            }
            finally
            {
                container.ResumeLayout(true);
            }
        }
    }
}
