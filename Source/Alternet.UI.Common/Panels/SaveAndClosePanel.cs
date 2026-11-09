using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a panel with 'Save and Close' and 'Close' buttons.
    /// </summary>
    public partial class SaveAndClosePanel : HiddenGenericBorder
    {
        private readonly XButton btSaveAndClose = new();
        private readonly XButton btClose = new();
        private readonly Spacer spacer = new(10);

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAndClosePanel"/> class.
        /// </summary>
        public SaveAndClosePanel()
        {
            btSaveAndClose.HorizontalAlignment = HorizontalAlignment.Center;
            btSaveAndClose.Text = CommonStrings.Default.ButtonSaveAndClose;
            btSaveAndClose.SvgImage = KnownSvgImages.ImgFileSave;
            btSaveAndClose.Enabled = false;

            spacer.HorizontalAlignment = HorizontalAlignment.Center;

            btClose.HorizontalAlignment = HorizontalAlignment.Center;
            btClose.Text = CommonStrings.Default.ButtonClose;
            btClose.SvgImage = KnownSvgImages.ImgClose;

            MinChildMargin = (0, 10, 0, 10);
            
            Layout = LayoutStyle.Horizontal;

            Controls.Add(btSaveAndClose);
            Controls.Add(spacer);
            Controls.Add(btClose);
        }

        /// <summary>
        /// Gets the <see cref="Spacer"/> control used to separate the buttons.
        /// </summary>
        public Spacer Spacer => spacer;

        /// <summary>
        /// Gets the 'Close' button.
        /// </summary>
        public XButton CloseButton => btClose;

        /// <summary>
        /// Gets the 'Save and Close' button.
        /// </summary>
        public XButton SaveAndCloseButton => btSaveAndClose;

        /// <inheritdoc/>
        protected override void OnAfterParentSizeChanged(object? sender, HandledEventArgs e)
        {
            base.OnAfterParentSizeChanged(sender, e);

            var ignorePaint = btClose.Right > Parent!.Width - 10;

            DoInsideLayout(() =>
            {
                btClose.IgnorePaint = ignorePaint;
                btClose.Enabled = !ignorePaint;
                btClose.IgnoreLayout = ignorePaint;
                spacer.IgnoreLayout = ignorePaint;
            });
        }
    }
}
