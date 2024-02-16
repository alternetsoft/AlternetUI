namespace Alternet.UI
{
    /// <summary>
    /// Implements an <see cref="IControlHandlerFactory"/> for the Native visual theme.
    /// </summary>
    internal class NativeControlHandlerFactory : IControlHandlerFactory
    {
        /// <inheritdoc/>
        public ControlHandler CreateAuiToolbarHandler(Control control) =>
            new NativeAuiToolbarHandler();

        /// <inheritdoc/>
        public ControlHandler CreatePropertyGridHandler(Control control) =>
            new PropertyGridHandler();

        /// <inheritdoc/>
        public ControlHandler CreateAuiNotebookHandler(Control control) =>
            new NativeAuiNotebookHandler();

        /// <inheritdoc/>
        public ControlHandler CreateButtonHandler(Control control) =>
            new NativeButtonHandler();

        /// <inheritdoc/>
        public ControlHandler CreateTextBoxHandler(Control control) =>
            new NativeTextBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateCheckBoxHandler(Control control) =>
            new NativeCheckBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateRadioButtonHandler(Control control) =>
            new NativeRadioButtonHandler();

        /// <inheritdoc/>
        public ControlHandler CreateTabControlHandler(Control control) =>
            new TabControlHandler();

        /// <inheritdoc/>
        public ControlHandler CreateGroupBoxHandler(Control control) =>
            new GroupBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateProgressBarHandler(Control control) =>
            new ProgressBarHandler();

        /// <inheritdoc/>
        public ControlHandler CreateSliderHandler(Control control) =>
            new SliderHandler();

        /// <inheritdoc/>
        public ControlHandler CreateNumericUpDownHandler(Control control) =>
            new NativeNumericUpDownHandler();

        /// <inheritdoc/>
        public ControlHandler CreateCheckListBoxHandler(Control control) =>
            new NativeCheckListBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateListBoxHandler(Control control) =>
            new NativeListBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateComboBoxHandler(Control control) =>
            new NativeComboBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateListViewHandler(Control control) =>
            new NativeListViewHandler();

        /// <inheritdoc/>
        public ControlHandler CreateTreeViewHandler(Control control) =>
            new NativeTreeViewHandler();

        /// <inheritdoc/>
        public ControlHandler CreateMainMenuHandler(Control control) =>
            new MainMenuHandler();

        /// <inheritdoc/>
        public ControlHandler CreateMenuItemHandler(Control control) =>
            new MenuItemHandler();

        /// <inheritdoc/>
        public ControlHandler CreateContextMenuHandler(Control control) =>
            new ContextMenuHandler();

        /// <inheritdoc/>
        public ControlHandler CreateToolbarHandler(Control control) =>
            new ToolBarHandler();

        /// <inheritdoc/>
        public ControlHandler CreateToolbarItemHandler(Control control) =>
            new ToolBarItemHandler();

        /// <inheritdoc/>
        public ControlHandler CreateColorPickerHandler(Control control) =>
            new NativeColorPickerHandler();

        /// <inheritdoc/>
        public ControlHandler CreateDateTimePickerHandler(Control control) =>
            new NativeDateTimePickerHandler();

        /// <inheritdoc/>
        public ControlHandler CreateScrollViewerHandler(Control control) =>
            new ScrollViewer.NativeScrollViewerHandler();
    }
}