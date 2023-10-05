using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains layout related static methods.
    /// </summary>
    public static class LayoutFactory
    {
        /// <summary>
        /// Initializes <see cref="Grid"/> rows and columns for the specified controls.
        /// </summary>
        /// <param name="grid">Grid instance.</param>
        /// <param name="controls">Grid controls.</param>
        /// <remarks>
        /// Dimensions for all rows and columns are set to <see cref="GridLength.Auto"/>.
        /// <see cref="Grid.SetRowColumn"/> is called for the each control with row and column
        /// indexes equal to position in the <paramref name="controls"/> array.
        /// </remarks>
        public static void SetupGrid(Grid grid, Control[,] controls)
        {
            var rowCount = controls.GetLength(0);
            var colCount = controls.GetLength(1);

            grid.DoInsideLayout(() =>
            {
                for (int i = 0; i < rowCount; i++)
                    grid.AddAutoRow();
                for (int i = 0; i < colCount; i++)
                    grid.AddAutoColumn();

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        var control = controls[i, j];
                        control.Parent ??= grid;
                        Grid.SetRowColumn(control, i, j);
                    }
                }
            });
        }

        /// <summary>
        /// Increases height of all <see cref="TextBox"/> controls in the specified
        /// container to height of the <see cref="ComboBox"/> control, if it
        /// is present in the container.
        /// </summary>
        /// <remarks>
        /// Used in Linux where height of the <see cref="ComboBox"/>
        /// is bigger than height of the <see cref="TextBox"/>.
        /// </remarks>
        /// <remarks>
        /// All <see cref="TextBox"/> of the child controls are also affected
        /// as this procedure works recursively.
        /// </remarks>
        /// <param name="container">Specifies container control in which
        /// operation is performed</param>
        public static void AdjustTextBoxesHeight(Control container)
        {
            if (container == null || !AllPlatformDefaults.PlatformCurrent.AdjustTextBoxesHeight)
                return;

            Control? comboBox = null;
            Control? textBox = null;

            FindTextEditors(container);
            if (comboBox == null)
                return;
            AdjustTextBoxesHeightInternal(container, comboBox, textBox);

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

        /// <summary>
        /// Sets background of the control's parents to Red, Green, Blue and
        /// Yellow colors.
        /// </summary>
        /// <param name="control">Specifies the control which parent's background
        /// is changed</param>
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

            control = SetParentBackground(control, Brushes.Red);
            control = SetParentBackground(control, Brushes.Green);
            control = SetParentBackground(control, Brushes.Blue);
            SetParentBackground(control, Brushes.Yellow);
        }

        internal static void AdjustTextBoxesHeightInternal(
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
                    control.SuggestedHeight = maxHeight;

                /*control.SetBounds(0, 0, 0, maxHeight, BoundsSpecified.Height);*/

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
