using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IControlStateObjectChanged
    {
        void DisabledChanged(object? sender);

        void NormalChanged(object? sender);

        void FocusedChanged(object? sender);

        void HoveredChanged(object? sender);

        void PressedChanged(object? sender);
    }
}
