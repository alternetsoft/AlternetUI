using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Controls.LayoutPanel
{
    internal static class LayoutFactory
    {
        public static IArrangedElementLite FromControl(Control control)
        {
            return new ArrangedElementControl(control);
        }

        public static IArrangedElement CreateContainer(IArrangedElementLite? container = null)
        {
            IArrangedElement result = new ArrangedElement(null, container);
            return result;
        }

        public static IArrangedElement AddChild(
            IArrangedElement container,
            IArrangedElementLite? control = null)
        {
            IArrangedElement result = new ArrangedElement(container, control);
            result.Controls.Add(result);
            return result;
        }

        public static IArrangedElement CreateLayoutLeftFill(
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
