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
            var oldColor = Color;

            var result = ShowModal(GetNativeWindow(owner));
            
            if (App.IsMacOS && WxGlobalSettings.MacOs.ColorDialogAcceptIfChanged)
            {
                if (oldColor.AsStruct == Color.AsStruct)
                    result = ModalResult.Canceled;
                else
                    result = ModalResult.Accepted;
            }

            return result;
        }

        public void ShowAsync(Alternet.UI.Window? owner, Action<bool>? onClose)
        {
            DefaultShowAsync(owner, onClose, ShowModal);
        }
    }
}
