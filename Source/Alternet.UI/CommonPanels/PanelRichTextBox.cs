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
        /// Occurs when 'New' button is clicked on the toolbar.
        /// </summary>
        public event EventHandler? FileNewClick;

        /// <summary>
        /// Occurs when 'Open' button is clicked on the toolbar.
        /// </summary>
        public event EventHandler? FileOpenClick;

        /// <summary>
        /// Occurs when 'Save' button is clicked on the toolbar.
        /// </summary>
        public event EventHandler? FileSaveClick;

        /// <summary>
        /// Gets id of the 'New' toolbar item.
        /// </summary>
        public int ButtonIdNew => buttonIdNew;

        /// <summary>
        /// Gets id of the 'Open' toolbar item.
        /// </summary>
        public int ButtonIdOpen => buttonIdOpen;

        /// <summary>
        /// Gets id of the 'Save' toolbar item.
        /// </summary>
        public int ButtonIdSave => buttonIdSave;

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

            toolbar.AddToolOnClick(buttonIdNew, FileNew_Click);
            toolbar.AddToolOnClick(buttonIdOpen, FileOpen_Click);
            toolbar.AddToolOnClick(buttonIdSave, FileSave_Click);

            toolbar.AddLabel("Work in progress...");

            toolbar.Realize();
        }

        private void FileNew_Click(object? sender, EventArgs e)
        {
            FileNewClick?.Invoke(this, EventArgs.Empty);
        }

        private void FileOpen_Click(object? sender, EventArgs e)
        {
            FileOpenClick?.Invoke(this, EventArgs.Empty);
        }

        private void FileSave_Click(object? sender, EventArgs e)
        {
            FileSaveClick?.Invoke(this, EventArgs.Empty);
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
