using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Controls.LayoutPanel
{
    public static class LayoutFactory
    {
        public static IArrangedElement CreateContainerElement(Control container)
        {
            IArrangedElement result = new ArrangedElement(null, container);
            return result;
        }

        public static IArrangedElement AddChildElement(
            IArrangedElement container,
            Control control)
        {
            IArrangedElement result = new ArrangedElement(container, control);
            result.Controls.Add(result);
            return result;
        }

        public static IArrangedElement CreateLayoutLeftFill(
            Control leftControl,
            Control fillControl)
        {
            var container = CreateContainerElement(leftControl.Parent!);
            var leftControlElement = AddChildElement(container, leftControl);
            var fillControlElement = AddChildElement(container, fillControl);
            leftControlElement.Dock = DockStyle.Left;
            fillControlElement.Dock = DockStyle.Fill;
            return container;
        }
    }
}
