using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class SelectDirectoryDialog : Alternet.UI.ISelectDirectoryDialogHandler
    {
        public bool ShowHelp { get; set; }

        public Alternet.UI.ModalResult ShowModal(Alternet.UI.Window? owner)
        {
            var nativeOwner = owner == null ?
                null : ((WindowHandler)owner.Handler).NativeControl;
            return ShowModal(nativeOwner);
        }
    }
}