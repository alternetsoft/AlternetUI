namespace Alternet.UI
{
    /// <summary>
    /// Represents an interface that can be implemented by classes providing
    /// creating <see cref="WxControlHandler"/> instances for specified controls.
    /// </summary>
    internal interface IControlHandlerFactory
    {
        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="Button"/> control.
        /// </summary>
        WxControlHandler CreateButtonHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="TextBox"/> control.
        /// </summary>
        WxControlHandler CreateTextBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="CheckBox"/> control.
        /// </summary>
        WxControlHandler CreateCheckBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="RadioButton"/> control.
        /// </summary>
        WxControlHandler CreateRadioButtonHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="NativeTabControl"/> control.
        /// </summary>
        WxControlHandler CreateTabControlHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="GroupBox"/>control.
        /// </summary>
        WxControlHandler CreateGroupBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ProgressBar"/> control.
        /// </summary>
        WxControlHandler CreateProgressBarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="Slider"/> control.
        /// </summary>
        WxControlHandler CreateSliderHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="NumericUpDown"/> control.
        /// </summary>
        WxControlHandler CreateNumericUpDownHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="CheckListBox"/> control.
        /// </summary>
        WxControlHandler CreateCheckListBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ListBox"/> control.
        /// </summary>
        WxControlHandler CreateListBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ComboBox"/> control.
        /// </summary>
        WxControlHandler CreateComboBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ListView"/> control.
        /// </summary>
        WxControlHandler CreateListViewHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="TreeView"/> control.
        /// </summary>
        WxControlHandler CreateTreeViewHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="MainMenu"/> control.
        /// </summary>
        WxControlHandler CreateMainMenuHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="MenuItem"/> control.
        /// </summary>
        WxControlHandler CreateMenuItemHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ContextMenu"/> control.
        /// </summary>
        WxControlHandler CreateContextMenuHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ToolBar"/> control.
        /// </summary>
        WxControlHandler CreateToolbarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="AuiToolbar"/> control.
        /// </summary>
        WxControlHandler CreateAuiToolbarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="PropertyGrid"/> control.
        /// </summary>
        WxControlHandler CreatePropertyGridHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="AuiNotebook"/> control.
        /// </summary>
        WxControlHandler CreateAuiNotebookHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ToolBarItem"/> control.
        /// </summary>
        WxControlHandler CreateToolbarItemHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ColorPicker"/> control.
        /// </summary>
        WxControlHandler CreateColorPickerHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="DateTimePicker"/> control.
        /// </summary>
        WxControlHandler CreateDateTimePickerHandler(Control control);

        /// <summary>
        /// Creates a <see cref="WxControlHandler"/> for
        /// <see cref="ScrollViewer"/> control.
        /// </summary>
        WxControlHandler CreateScrollViewerHandler(Control control);
    }
}