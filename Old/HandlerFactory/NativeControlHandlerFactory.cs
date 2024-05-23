namespace Alternet.UI
{
    /// <summary>
    /// Implements an <see cref="IControlHandlerFactory"/> for the Native visual theme.
    /// </summary>
    internal class NativeControlHandlerFactory : IControlHandlerFactory
    {
        /// <inheritdoc/>
        public BaseControlHandler CreateAuiToolbarHandler(Control control) =>
            new NativeAuiToolbarHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreatePropertyGridHandler(Control control) =>
            new PropertyGridHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateAuiNotebookHandler(Control control) =>
            new NativeAuiNotebookHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateButtonHandler(Control control) =>
            new ButtonHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateTextBoxHandler(Control control) =>
            new TextBoxHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateCheckBoxHandler(Control control) =>
            new CheckBoxHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateRadioButtonHandler(Control control) =>
            new RadioButtonHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateTabControlHandler(Control control) =>
            new NativeTabControlHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateGroupBoxHandler(Control control) =>
            new GroupBoxHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateProgressBarHandler(Control control) =>
            new ProgressBarHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateSliderHandler(Control control) =>
            new SliderHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateNumericUpDownHandler(Control control) =>
            new NumericUpDownHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateCheckListBoxHandler(Control control) =>
            new CheckListBoxHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateListBoxHandler(Control control) =>
            new NativeListBoxHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateComboBoxHandler(Control control) =>
            new ComboBoxHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateListViewHandler(Control control) =>
            new NativeListViewHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateTreeViewHandler(Control control) =>
            new NativeTreeViewHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateMainMenuHandler(Control control) =>
            new MainMenuHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateMenuItemHandler(Control control) =>
            new MenuItemHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateContextMenuHandler(Control control) =>
            new ContextMenuHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateToolbarHandler(Control control) =>
            new ToolBarHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateToolbarItemHandler(Control control) =>
            new ToolBarItemHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateColorPickerHandler(Control control) =>
            new ColorPickerHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateDateTimePickerHandler(Control control) =>
            new DateTimePickerHandler();

        /// <inheritdoc/>
        public BaseControlHandler CreateScrollViewerHandler(Control control) =>
            new ScrollViewer.NativeScrollViewerHandler();
    }
}