using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a color.
    /// </summary>
    [ControlCategory("Dialogs")]
    public class ColorDialog : CommonDialog
    {
        /// <summary>
        /// Gets default <see cref="ColorDialog"/> instance.
        /// </summary>
        public static ColorDialog Default = defaultDialog ??= new ColorDialog();

        private static ColorDialog? defaultDialog;

        private readonly Native.ColorDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="ColorDialog"/>.
        /// </summary>
        public ColorDialog()
        {
            nativeDialog = new Native.ColorDialog();
        }

        /// <summary>
        /// Gets or sets the color selected by the color dialog window.
        /// </summary>
        public Color Color
        {
            get
            {
                CheckDisposed();
                return nativeDialog.Color;
            }

            set
            {
                CheckDisposed();
                nativeDialog.Color = value;
            }
        }

        /// <inheritdoc/>
        public override string? Title
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

        /// <inheritdoc/>
        public override ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();
            var nativeOwner = owner == null ?
                null : ((WindowHandler)owner.Handler).NativeControl;
            return (ModalResult)nativeDialog.ShowModal(nativeOwner);
        }
    }
}