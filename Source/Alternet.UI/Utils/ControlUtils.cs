using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods related to the control drawing.
    /// </summary>
    public static class ControlUtils
    {
        /// <summary>
        /// Sets <see cref="ComboBox.IsEditable"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value"><c>true</c> enables editing of the text;
        /// <c>false</c> disables it.</param>
        /// <param name="controlSet">Set of controls.</param>
        public static ControlSet IsEditable(this ControlSet controlSet, bool value)
        {
            foreach (var item in controlSet.Items)
            {
                if (item is ComboBox comboBox)
                    comboBox.IsEditable = value;
            }

            return controlSet;
        }

        /// <summary>
        /// Creates new <see cref="Button"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static Button AddButton(this Control control, string text, Action? action = null)
        {
            var result = new Button(text)
            {
                Parent = control,
            };

            if (action is not null)
                result.Click += Result_Click;

            return result;

            void Result_Click(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Adds multiple buttons.
        /// </summary>
        /// <param name="buttons">Array of title and action.</param>
        /// <returns><see cref="ControlSet"/> with list of created buttons.</returns>
        /// <param name="control">Parent control.</param>
        public static ControlSet AddButtons(this Control control, params (string, Action?)[] buttons)
        {
            List<Control> result = new();

            control.DoInsideLayout(() =>
            {
                foreach (var item in buttons)
                {
                    var button = AddButton(control, item.Item1, item.Item2);
                    result.Add(button);
                }
            });

            return new(result);
        }

        /// <summary>
        /// Adds multiple labels.
        /// </summary>
        /// <param name="control">Parent control.</param>
        /// <param name="text">Array of label text.</param>
        /// <returns><see cref="ControlSet"/> with list of created labels.</returns>
        public static ControlSet AddLabels(this Control control, params string[] text)
        {
            List<Control> result = new();

            control.DoInsideLayout(() =>
            {
                foreach (var item in text)
                {
                    var label = AddLabel(control, item);
                    result.Add(label);
                }
            });

            return new(result);
        }

        /// <summary>
        /// Creates new <see cref="CheckBox"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static CheckBox AddCheckBox(this Control control, string text, Action? action = null)
        {
            var result = new CheckBox(text)
            {
                Parent = control,
            };

            if (action is not null)
                result.CheckedChanged += Result_CheckedChanged;

            return result;

            void Result_CheckedChanged(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Creates new <see cref="Label"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static Label AddLabel(this Control control, string text)
        {
            var result = new Label(text)
            {
                Parent = control,
            };

            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static GroupBox AddGroupBox(this Control control, string? title = default)
        {
            var result = new GroupBox
            {
                Parent = control,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> with vertical layout and adds
        /// it to the <see cref="Control.Children"/>.
        /// </summary>
        public static GroupBox AddVerticalGroupBox(this Control control, string? title = default)
        {
            var result = new GroupBox
            {
                Layout = LayoutStyle.Vertical,
                Parent = control,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> with horizontal layout and adds
        /// it to the <see cref="Control.Children"/>.
        /// </summary>
        public static GroupBox AddHorizontalGroupBox(this Control control, string? title = default)
        {
            var result = new GroupBox
            {
                Layout = LayoutStyle.Horizontal,
                Parent = control,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }
    }
}
