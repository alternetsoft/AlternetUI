using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Menu : IContextMenuHandler
    {
        void IContextMenuHandler.Show(AbstractControl control, Drawing.PointD? position, Action? onClose)
        {
        }
    }
}
