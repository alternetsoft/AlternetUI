namespace Alternet.UI
{
    /// <summary>
    /// Implements an <see cref="IControlHandlerFactory"/> for the Generic visual theme.
    /// </summary>
    internal class GenericControlHandlerFactory : IControlHandlerFactory
    {
        /// <inheritdoc/>
        public BaseControlHandler CreateButtonHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateButtonHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateTextBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateTextBoxHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateCheckBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateCheckBoxHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateRadioButtonHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateRadioButtonHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateAuiToolbarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateAuiToolbarHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateAuiNotebookHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateAuiNotebookHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateTabControlHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateTabControlHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateGroupBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateGroupBoxHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreatePropertyGridHandler(Control control) =>
            StockControlHandlerFactories.Native.CreatePropertyGridHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateProgressBarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateProgressBarHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateSliderHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateSliderHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateNumericUpDownHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateNumericUpDownHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateCheckListBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateCheckListBoxHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateListBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateListBoxHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateComboBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateComboBoxHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateListViewHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateListViewHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateTreeViewHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateTreeViewHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateMainMenuHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateMainMenuHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateMenuItemHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateMenuItemHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateContextMenuHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateContextMenuHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateToolbarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateToolbarHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateToolbarItemHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateToolbarItemHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateColorPickerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateColorPickerHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateDateTimePickerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateDateTimePickerHandler(control);

        /// <inheritdoc/>
        public BaseControlHandler CreateScrollViewerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateScrollViewerHandler(control);
    }
}