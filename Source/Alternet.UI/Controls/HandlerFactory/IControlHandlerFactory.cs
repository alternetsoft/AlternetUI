namespace Alternet.UI
{
    /// <summary>
    /// Represents an interface that can be implemented by classes providing
    /// creating <see cref="BaseControlHandler"/> instances for specified controls.
    /// </summary>
    internal interface IControlHandlerFactory
    {
        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="Button"/> control.
        /// </summary>
        BaseControlHandler CreateButtonHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="TextBox"/> control.
        /// </summary>
        BaseControlHandler CreateTextBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="CheckBox"/> control.
        /// </summary>
        BaseControlHandler CreateCheckBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="RadioButton"/> control.
        /// </summary>
        BaseControlHandler CreateRadioButtonHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="NativeTabControl"/> control.
        /// </summary>
        BaseControlHandler CreateTabControlHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="GroupBox"/>control.
        /// </summary>
        BaseControlHandler CreateGroupBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ProgressBar"/> control.
        /// </summary>
        BaseControlHandler CreateProgressBarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="Slider"/> control.
        /// </summary>
        BaseControlHandler CreateSliderHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="NumericUpDown"/> control.
        /// </summary>
        BaseControlHandler CreateNumericUpDownHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="CheckListBox"/> control.
        /// </summary>
        BaseControlHandler CreateCheckListBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ListBox"/> control.
        /// </summary>
        BaseControlHandler CreateListBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ComboBox"/> control.
        /// </summary>
        BaseControlHandler CreateComboBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ListView"/> control.
        /// </summary>
        BaseControlHandler CreateListViewHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="TreeView"/> control.
        /// </summary>
        BaseControlHandler CreateTreeViewHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="MainMenu"/> control.
        /// </summary>
        BaseControlHandler CreateMainMenuHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="MenuItem"/> control.
        /// </summary>
        BaseControlHandler CreateMenuItemHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ContextMenu"/> control.
        /// </summary>
        BaseControlHandler CreateContextMenuHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ToolBar"/> control.
        /// </summary>
        BaseControlHandler CreateToolbarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="AuiToolbar"/> control.
        /// </summary>
        BaseControlHandler CreateAuiToolbarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="PropertyGrid"/> control.
        /// </summary>
        BaseControlHandler CreatePropertyGridHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="AuiNotebook"/> control.
        /// </summary>
        BaseControlHandler CreateAuiNotebookHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ToolBarItem"/> control.
        /// </summary>
        BaseControlHandler CreateToolbarItemHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ColorPicker"/> control.
        /// </summary>
        BaseControlHandler CreateColorPickerHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="DateTimePicker"/> control.
        /// </summary>
        BaseControlHandler CreateDateTimePickerHandler(Control control);

        /// <summary>
        /// Creates a <see cref="BaseControlHandler"/> for
        /// <see cref="ScrollViewer"/> control.
        /// </summary>
        BaseControlHandler CreateScrollViewerHandler(Control control);
    }
}