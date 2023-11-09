using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with <see cref="RichTextBox"/> and <see cref="AuiToolbar"/> with
    /// text edit buttons.
    /// </summary>
    public class PanelRichTextBox : PanelAuiManager
    {
        private RichTextBox? textBox;
        private int buttonIdNew;
        private int buttonIdOpen;
        private int buttonIdSave;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelRichTextBox"/> class.
        /// </summary>
        public PanelRichTextBox()
        {
            Initialize();
        }

        /// <summary>
        /// Gets <see cref="RichTextBox"/> control used in this panel.
        /// </summary>
        public RichTextBox TextBox
        {
            get
            {
                textBox ??= new RichTextBox();
                return textBox;
            }
        }

        /// <summary>
        /// Creates toolbar items.
        /// </summary>
        protected virtual void CreateToolbarItems()
        {
            var toolbar = Toolbar;
            var imageSize = GetToolBitmapSize();

            var images = KnownSvgImages.GetForSize(imageSize);

            buttonIdNew = toolbar.AddTool(
                CommonStrings.Default.ButtonNew,
                images.ImageFileNew,
                CommonStrings.Default.ButtonNew);

            buttonIdOpen = toolbar.AddTool(
                CommonStrings.Default.ButtonOpen,
                images.ImageFileOpen,
                CommonStrings.Default.ButtonOpen);

            buttonIdSave = toolbar.AddTool(
                CommonStrings.Default.ButtonSave,
                images.ImageFileSave,
                CommonStrings.Default.ButtonSave);

            toolbar.AddLabel("Work in progress...");

            toolbar.Realize();
        }

        /// <inheritdoc/>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null || Flags.HasFlag(ControlFlags.ParentAssigned))
                return;

            Manager.AddPane(TextBox, CenterPane);
            Manager.AddPane(Toolbar, ToolbarPane);

            Manager.Update();
        }

        private void Initialize()
        {
            DefaultRightPaneBestSize = new(150, 200);
            DefaultRightPaneMinSize = new(150, 200);
            TextBox.HasBorder = false;
            DefaultToolbarStyle &=
                ~(AuiToolbarCreateStyle.Text | AuiToolbarCreateStyle.HorzLayout);
            Toolbar.Required();
            CreateToolbarItems();
        }
    }
}
