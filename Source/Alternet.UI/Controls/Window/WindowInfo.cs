using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WindowInfo
    {
        public bool MinimizeEnabled = true;
        
        public bool MaximizeEnabled = true;
        
        public bool ShowInTaskbar = true;
        
        public WindowState State = WindowState.Normal;
        
        public bool CloseEnabled = true;
        
        public bool HasSystemMenu = true;
        
        public bool HasBorder = true;
        
        public bool AlwaysOnTop = false;
        
        public bool IsToolWindow = false;
        
        public bool IsPopupWindow = false;
        
        public bool Resizable = true;
        
        public bool HasTitleBar = true;

        public WindowStartLocation StartLocation = WindowStartLocation.Default;
    }
}
