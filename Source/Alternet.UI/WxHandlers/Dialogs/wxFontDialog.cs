using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class FontDialog : Alternet.UI.IFontDialogHandler
    {
        private Alternet.Drawing.FontInfo fontInfo = Alternet.UI.Control.DefaultFont;

        FontDialogRestrictSelection Alternet.UI.IFontDialogHandler.RestrictSelection
        {
            get
            {
                return (FontDialogRestrictSelection)Enum.ToObject(
                    typeof(FontDialogRestrictSelection),
                    RestrictSelection);
            }

            set
            {
                if (!App.IsWindowsOS)
                    return;
                RestrictSelection = (int)value;
            }
        }

        public Alternet.Drawing.FontInfo FontInfo
        {
            get => fontInfo;
            set => fontInfo = value;
        }

        public Alternet.UI.ModalResult ShowModal(Alternet.UI.Window? owner)
        {
            CheckDisposed();
            var nativeOwner = owner == null ?
                null : ((WindowHandler)owner.Handler).NativeControl;

            var fontName = fontInfo.Name;
            var style = fontInfo.Style;
            var genericFamily = fontInfo.FontFamily.GenericFamily
                ?? Alternet.Drawing.GenericFontFamily.None;

            SetInitialFont(
                genericFamily,
                fontName,
                fontInfo.SizeInPoints,
                style);

            var result = ShowModal(nativeOwner);

            fontInfo.Style = ResultFontStyle;
            fontInfo.SizeInPoints = ResultFontSizeInPoints;
            fontInfo.Name = ResultFontName;

            return result;
        }
    }
}
