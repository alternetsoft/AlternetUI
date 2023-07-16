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
        /// Increases height of all <see cref="TextBox"/> controls in the specified
        /// container to height of the <see cref="ComboBox"/> control, if it
        /// is present in the container.
        /// </summary>
        /// <remarks>
        /// Used in Linux where height of the <see cref="ComboBox"/>
        /// control is bigger than <see cref="TextBox"/> control.
        /// </remarks>
        /// <remarks>
        /// All <see cref="TextBox"/> of the child controls are also affected
        /// as this procedure works recursively.
        /// </remarks>
        /// <param name="container">Specifies container control in which
        /// operation is performed</param>
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
            LayoutPanel.SetDock(leftControl,DockStyle.Left);
            LayoutPanel.SetDock(fillControl,DockStyle.Fill);
            DefaultLayout.Instance.Layout(container);
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

            control = SetParentBackground(control, new SolidBrush(Color.Red));
            control = SetParentBackground(control, new SolidBrush(Color.Green));
            control = SetParentBackground(control, new SolidBrush(Color.Blue));
            SetParentBackground(control, new SolidBrush(Color.Yellow));
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

        /*public static void AddToolbarSizer(Control control, Toolbar toolbar)
        {
            int wxLEFT = 0x0010;
            int wxRIGHT = 0x0020;
            int wxUP = 0x0040;
            int wxDOWN = 0x0080;

            int wxALL = (wxUP | wxDOWN | wxRIGHT | wxLEFT);

            int wxSTRETCH_NOT = 0x0000;
            int wxSHRINK = 0x1000;
            int wxGROW = 0x2000;
            int wxEXPAND = wxGROW;
            int wxSHAPED = 0x4000;
            int wxTILE = 0xc000;

            int wxHORIZONTAL = 0x0004;
            int wxVERTICAL = 0x0008;
            int wxBOTH = wxVERTICAL | wxHORIZONTAL;


            var sizer = new Native.BoxSizer();

            var nativeControl = control.Handler.NativeControl;
            var nativeSizer = sizer.Handle;
            var nativeToolbar = toolbar.Handler.NativeControl.WxWidget;

            nativeControl?.SetSizer(nativeSizer, true);
            sizer.AddWindow(nativeToolbar, 1, wxALL | wxGROW, 0, IntPtr.Zero);
        }*/
    }
}
