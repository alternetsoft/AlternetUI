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

        public Alternet.Drawing.Color Color
        {
            get
            {
                var state = GetColorState();
                if (state == 0)
                    return Alternet.Drawing.Color.Empty;
                var r = GetColorR();
                var g = GetColorG();
                var b = GetColorB();
                var a = GetColorA();
                return new Alternet.Drawing.Color(a, r, g, b);
            }

            set
            {
                SetColor(value);
            }
        }

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
