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
    }
}
