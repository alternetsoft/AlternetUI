using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;

using System.Runtime.InteropServices;

namespace Alternet.UI.Integration.VisualStudio
{
    [Guid("D1A2B3C4-D5E6-47F8-ABCD-1234567890AB")]
    public class MySidebarToolWindow : ToolWindowPane
    {
        public MySidebarToolWindow() : base(null)
        {
            this.Caption = "My Sidebar Panel";
            this.Content = new MySidebarControl();
        }
    }
}
