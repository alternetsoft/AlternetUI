using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the type of file name picker dialog to display.
    /// </summary>
    public enum FileNamePickerKind
    {
        /// <summary>
        /// The dialog is not displayed.
        /// </summary>
        None,

        /// <summary>
        /// The dialog is used to select a file to open.
        /// </summary>
        OpenFile,

        /// <summary>
        /// The dialog is used to select a file to save.
        /// </summary>
        SaveFile,

        /// <summary>
        /// The dialog is used to select a directory.
        /// </summary>
        SelectDirectory,
    }

    /// <summary>
    /// Represents a control that allows the user to select a file name.
    /// </summary>
    public partial class FileNamePicker : TextPickerAndButton
    {
        /// <summary>
        /// Gets or sets the default svg image which can be used for indicating an action that opens a dialog window.
        /// </summary>
        public static SvgImage? DefaultButtonImage;

        private readonly OpenFileDialog openFileDialog = new DummyOpenFileDialog();
        private readonly SaveFileDialog saveFileDialog = new DummySaveFileDialog();        
        private readonly SelectDirectoryDialog selectDirectoryDialog = new DummySelectDirectoryDialog();

        private FileNamePickerKind kind = FileNamePickerKind.OpenFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileNamePicker"/> class.
        /// </summary>
        public FileNamePicker()
        {
            BtnComboBoxSvg = DefaultButtonImage ?? KnownSvgImages.ImgIconFolder;
            InnerOuterBorder = InnerOuterSelector.Outer;
            RoundCorners();
            HasBorder = true;
            IgnoreTransparency = true;
        }

        /// <summary>
        /// Gets or sets the properties of the <see cref="OpenFileDialog"/>
        /// used by the <see cref="FileNamePicker"/> control.
        /// </summary>
        public virtual OpenFileDialog OpenFileProps
        {
            get
            {
                return openFileDialog;
            }

            set
            {
                openFileDialog.Assign(value);
            }
        }

        /// <summary>
        /// Gets or sets the properties of the <see cref="SaveFileDialog"/>
        /// used by the <see cref="FileNamePicker"/> control.
        /// </summary>
        public virtual SaveFileDialog SaveFileProps
        {
            get
            {
                return saveFileDialog;
            }

            set
            {
                saveFileDialog.Assign(value);
            }
        }

        /// <summary>
        /// Gets or sets the type of file name picker dialog to display.
        /// </summary>
        public virtual FileNamePickerKind Kind
        {
            get => kind;
            set
            {
                if (kind == value)
                    return;
                kind = value;
            }
        }

        /// <inheritdoc/>
        public override void OnButtonClick(ControlAndButtonClickEventArgs e)
        {
            base.OnButtonClick(e);
            if (e.Handled)
                return;
            if (e.ButtonId != IdButtonCombo)
                return;
            e.Handled = true;

            switch (kind)
            {
                case FileNamePickerKind.OpenFile:
                    var openDlg = OpenFileDialog.Default;
                    openDlg.Assign(openFileDialog);
                    openDlg.ShowAsync(ParentWindow, (result) =>
                    {
                        openFileDialog.Assign(openDlg);
                        if (result && openDlg.FileName is not null)
                            Text = openDlg.FileName;
                    });

                    break;
                case FileNamePickerKind.SaveFile:
                    var saveDlg = SaveFileDialog.Default;
                    saveDlg.Assign(saveFileDialog);
                    saveDlg.ShowAsync(ParentWindow, (result) =>
                    {
                        saveFileDialog.Assign(saveDlg);
                        if (result && saveDlg.FileName is not null)
                            Text = saveDlg.FileName;
                    });

                    break;
                case FileNamePickerKind.SelectDirectory:
                    var selectDirDlg = SelectDirectoryDialog.Default;
                    selectDirDlg.Assign(selectDirectoryDialog);
                    selectDirDlg.ShowAsync(ParentWindow, (result) =>
                    {
                        selectDirectoryDialog.Assign(selectDirDlg);
                        if (result && selectDirDlg.DirectoryName is not null)
                            Text = selectDirDlg.DirectoryName;
                    });

                    break;
            }
        }
    }
}
