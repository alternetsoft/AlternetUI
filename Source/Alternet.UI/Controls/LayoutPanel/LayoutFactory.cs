using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public static class LayoutFactory
    {
        internal static IArrangedElementLite FromControl(Control control)
        {
            return new ArrangedElementControl(control);
        }

        internal static IArrangedElement CreateContainer(IArrangedElementLite? container = null)
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
    }
}
