namespace Alternet.UI
{
    /// <summary>
    /// Implements an <see cref="IControlHandlerFactory"/> for the Native visual theme.
    /// </summary>
    internal class NativeControlHandlerFactory : IControlHandlerFactory
    {
        /// <inheritdoc/>
        public WxControlHandler CreateAuiToolbarHandler(Control control) =>
            new NativeAuiToolbarHandler();

        /// <inheritdoc/>
        public WxControlHandler CreatePropertyGridHandler(Control control) =>
            new PropertyGridHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateAuiNotebookHandler(Control control) =>
            new NativeAuiNotebookHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateButtonHandler(Control control) =>
            new NativeButtonHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateTextBoxHandler(Control control) =>
            new NativeTextBoxHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateCheckBoxHandler(Control control) =>
            new NativeCheckBoxHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateRadioButtonHandler(Control control) =>
            new NativeRadioButtonHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateTabControlHandler(Control control) =>
            new NativeTabControlHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateGroupBoxHandler(Control control) =>
            new GroupBoxHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateProgressBarHandler(Control control) =>
            new ProgressBarHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateSliderHandler(Control control) =>
            new SliderHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateNumericUpDownHandler(Control control) =>
            new NativeNumericUpDownHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateCheckListBoxHandler(Control control) =>
            new NativeCheckListBoxHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateListBoxHandler(Control control) =>
            new NativeListBoxHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateComboBoxHandler(Control control) =>
            new NativeComboBoxHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateListViewHandler(Control control) =>
            new NativeListViewHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateTreeViewHandler(Control control) =>
            new NativeTreeViewHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateMainMenuHandler(Control control) =>
            new MainMenuHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateMenuItemHandler(Control control) =>
            new MenuItemHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateContextMenuHandler(Control control) =>
            new ContextMenuHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateToolbarHandler(Control control) =>
            new ToolBarHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateToolbarItemHandler(Control control) =>
            new ToolBarItemHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateColorPickerHandler(Control control) =>
            new NativeColorPickerHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateDateTimePickerHandler(Control control) =>
            new NativeDateTimePickerHandler();

        /// <inheritdoc/>
        public WxControlHandler CreateScrollViewerHandler(Control control) =>
            new ScrollViewer.NativeScrollViewerHandler();
    }
}