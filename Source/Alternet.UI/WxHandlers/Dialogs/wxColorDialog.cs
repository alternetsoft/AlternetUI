using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class ColorDialog : Alternet.UI.IColorDialogHandler
    {
        public bool ShowHelp { get; set; }

        public static void DefaultShowAsync(
            Alternet.UI.Window? owner,
            Action<bool>? onClose,
            Func<Alternet.UI.Window?, Alternet.UI.ModalResult> showModal)
        {
            var result = showModal(owner);
            var resultAsBool = result == Alternet.UI.ModalResult.Accepted;
            onClose?.Invoke(resultAsBool);
        }

        public Alternet.UI.ModalResult ShowModal(Alternet.UI.Window? owner)
        {
            return ShowModal(GetNativeWindow(owner));
        }

        public void ShowAsync(Alternet.UI.Window? owner, Action<bool>? onClose)
        {
            DefaultShowAsync(owner, onClose, ShowModal);
        }
    }
}
