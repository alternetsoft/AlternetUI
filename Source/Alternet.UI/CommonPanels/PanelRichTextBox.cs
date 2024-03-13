using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [ControlCategory("Panels")]
    public partial class PanelRichTextBox : PanelAuiManager
    {
        private RichTextBox? textBox;
        private int buttonIdNew;
        private int buttonIdOpen;
        private int buttonIdSave;
        private int buttonIdUndo;
        private int buttonIdRedo;
        private int buttonIdBold;
        private int buttonIdItalic;
        private int buttonIdUnderline;

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
        [Browsable(false)]
        public int ButtonIdNew => buttonIdNew;

        /// <summary>
        /// Gets id of the 'Open' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdOpen => buttonIdOpen;

        /// <summary>
        /// Gets id of the 'Save' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdSave => buttonIdSave;

        /// <summary>
        /// Gets id of the 'Undo' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdUndo => buttonIdUndo;

        /// <summary>
        /// Gets id of the 'Redo' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdRedo => buttonIdRedo;

        /// <summary>
        /// Gets id of the 'Bold' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdBold => buttonIdBold;

        /// <summary>
        /// Gets id of the 'Italic' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdItalic => buttonIdItalic;

        /// <summary>
        /// Gets id of the 'Underline' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdUnderline => buttonIdUnderline;

        /// <summary>
        /// Gets <see cref="RichTextBox"/> control used in this panel.
        /// </summary>
        [Browsable(false)]
        public RichTextBox TextBox
        {
            get
            {
                textBox ??= new RichTextBox();
                return textBox;
            }
        }

        /// <inheritdoc/>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null || StateFlags.HasFlag(ControlFlags.ParentAssigned))
                return;

            Manager.AddPane(TextBox, CenterPane);
            Manager.AddPane(Toolbar, ToolbarPane);

            Manager.Update();
        }

        /// <summary>
        /// Creates toolbar items.
        /// </summary>
        protected virtual void CreateToolbarItems()
        {
            var toolbar = Toolbar;
            var imageSize = GetBaseToolSvgSize();
            toolbar.ToolBitmapSizeInPixels = imageSize;

            buttonIdNew = toolbar.AddToolButton(
                CommonStrings.Default.ButtonNew,
                KnownSvgImages.ImgFileNew);

            buttonIdOpen = toolbar.AddToolButton(
                CommonStrings.Default.ButtonOpen,
                KnownSvgImages.ImgFileOpen);

            buttonIdSave = toolbar.AddToolButton(
                CommonStrings.Default.ButtonSave,
                KnownSvgImages.ImgFileSave);

            buttonIdUndo = toolbar.AddToolButton(
                CommonStrings.Default.ButtonUndo,
                KnownSvgImages.ImgUndo);

            buttonIdRedo = toolbar.AddToolButton(
                CommonStrings.Default.ButtonRedo,
                KnownSvgImages.ImgRedo);

            buttonIdBold = toolbar.AddToolButton(
                CommonStrings.Default.ButtonBold,
                KnownSvgImages.ImgBold);

            buttonIdItalic = toolbar.AddToolButton(
                CommonStrings.Default.ButtonItalic,
                KnownSvgImages.ImgItalic);

            buttonIdUnderline = toolbar.AddToolButton(
                CommonStrings.Default.ButtonUnderline,
                KnownSvgImages.ImgUnderline);

            toolbar.AddToolOnClick(buttonIdNew, FileNew_Click);
            toolbar.AddToolOnClick(buttonIdOpen, FileOpen_Click);
            toolbar.AddToolOnClick(buttonIdSave, FileSave_Click);
            toolbar.AddToolOnClick(buttonIdUndo, Undo_Click);
            toolbar.AddToolOnClick(buttonIdRedo, Redo_Click);
            toolbar.AddToolOnClick(buttonIdBold, Bold_Click);
            toolbar.AddToolOnClick(buttonIdItalic, Italic_Click);
            toolbar.AddToolOnClick(buttonIdUnderline, Underline_Click);

            /* toolbar.AddLabel("Work in progress..."); */

            toolbar.Realize();
        }

        private void Undo_Click(object? sender, EventArgs e)
        {
            TextBox.Undo();
        }

        private void Redo_Click(object? sender, EventArgs e)
        {
            TextBox.Redo();
        }

        private void Bold_Click(object? sender, EventArgs e)
        {
            TextBox.SelectionToggleBold();
        }

        private void Italic_Click(object? sender, EventArgs e)
        {
            TextBox.SelectionToggleItalic();
        }

        private void Underline_Click(object? sender, EventArgs e)
        {
            TextBox.SelectionToggleUnderlined();
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

        private void Initialize()
        {
            DefaultRightPaneBestSize = new(150, 200);
            DefaultRightPaneMinSize = new(150, 200);
            RightNotebookDefaultCreateStyle = AuiNotebookCreateStyle.Top;
            TextBox.HasBorder = false;
            DefaultToolbarStyle &=
                ~(AuiToolbarCreateStyle.Text | AuiToolbarCreateStyle.HorzLayout);
            Toolbar.Required();
            CreateToolbarItems();
        }
    }
}
