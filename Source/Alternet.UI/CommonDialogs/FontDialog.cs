using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a font.
    /// </summary>
    public sealed class FontDialog : CommonDialog
    {
        private Native.FontDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="FontDialog"/>.
        /// </summary>
        public FontDialog()
        {
            nativeDialog = new Native.FontDialog();
        }

        /// <summary>
        /// Gets or sets the font selected by the font dialog window.
        /// </summary>
        public Font Font
        {
            get
            {
                CheckDisposed();
                return new(nativeDialog.Font);
            }

            set
            {
                CheckDisposed();
                nativeDialog.Font = value.NativeFont;
            }
        }

        private protected override string? TitleCore
        {
            get
            {
                CheckDisposed();
                return nativeDialog.Title;
            }

            set
            {
                CheckDisposed();
                nativeDialog.Title = value;
            }
        }

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            CheckDisposed();
            var nativeOwner = owner == null ?
                null : ((NativeWindowHandler)owner.Handler).NativeControl;
            return (ModalResult)nativeDialog.ShowModal(nativeOwner);
        }
    }
}