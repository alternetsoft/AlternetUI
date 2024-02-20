namespace Alternet.UI
{
    /// <summary>
    /// Represents an interface that can be implemented by classes providing
    /// creating <see cref="ControlHandler"/> instances for specified controls.
    /// </summary>
    internal interface IControlHandlerFactory
    {
        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="Button"/> control.
        /// </summary>
        ControlHandler CreateButtonHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="TextBox"/> control.
        /// </summary>
        ControlHandler CreateTextBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="CheckBox"/> control.
        /// </summary>
        ControlHandler CreateCheckBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="RadioButton"/> control.
        /// </summary>
        ControlHandler CreateRadioButtonHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="NativeTabControl"/> control.
        /// </summary>
        ControlHandler CreateTabControlHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="GroupBox"/>control.
        /// </summary>
        ControlHandler CreateGroupBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ProgressBar"/> control.
        /// </summary>
        ControlHandler CreateProgressBarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="Slider"/> control.
        /// </summary>
        ControlHandler CreateSliderHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="NumericUpDown"/> control.
        /// </summary>
        ControlHandler CreateNumericUpDownHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="CheckListBox"/> control.
        /// </summary>
        ControlHandler CreateCheckListBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ListBox"/> control.
        /// </summary>
        ControlHandler CreateListBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ComboBox"/> control.
        /// </summary>
        ControlHandler CreateComboBoxHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ListView"/> control.
        /// </summary>
        ControlHandler CreateListViewHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="TreeView"/> control.
        /// </summary>
        ControlHandler CreateTreeViewHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="MainMenu"/> control.
        /// </summary>
        ControlHandler CreateMainMenuHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="MenuItem"/> control.
        /// </summary>
        ControlHandler CreateMenuItemHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ContextMenu"/> control.
        /// </summary>
        ControlHandler CreateContextMenuHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ToolBar"/> control.
        /// </summary>
        ControlHandler CreateToolbarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="AuiToolbar"/> control.
        /// </summary>
        ControlHandler CreateAuiToolbarHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="PropertyGrid"/> control.
        /// </summary>
        ControlHandler CreatePropertyGridHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="AuiNotebook"/> control.
        /// </summary>
        ControlHandler CreateAuiNotebookHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ToolBarItem"/> control.
        /// </summary>
        ControlHandler CreateToolbarItemHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ColorPicker"/> control.
        /// </summary>
        ControlHandler CreateColorPickerHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="DateTimePicker"/> control.
        /// </summary>
        ControlHandler CreateDateTimePickerHandler(Control control);

        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for
        /// <see cref="ScrollViewer"/> control.
        /// </summary>
        ControlHandler CreateScrollViewerHandler(Control control);
    }
}