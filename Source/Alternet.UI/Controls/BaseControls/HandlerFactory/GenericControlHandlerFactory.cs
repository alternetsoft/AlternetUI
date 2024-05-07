namespace Alternet.UI
{
    /// <summary>
    /// Implements an <see cref="IControlHandlerFactory"/> for the Generic visual theme.
    /// </summary>
    internal class GenericControlHandlerFactory : IControlHandlerFactory
    {
        /// <inheritdoc/>
        public WxControlHandler CreateButtonHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateButtonHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateTextBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateTextBoxHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateCheckBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateCheckBoxHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateRadioButtonHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateRadioButtonHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateAuiToolbarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateAuiToolbarHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateAuiNotebookHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateAuiNotebookHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateTabControlHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateTabControlHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateGroupBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateGroupBoxHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreatePropertyGridHandler(Control control) =>
            StockControlHandlerFactories.Native.CreatePropertyGridHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateProgressBarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateProgressBarHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateSliderHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateSliderHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateNumericUpDownHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateNumericUpDownHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateCheckListBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateCheckListBoxHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateListBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateListBoxHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateComboBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateComboBoxHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateListViewHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateListViewHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateTreeViewHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateTreeViewHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateMainMenuHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateMainMenuHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateMenuItemHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateMenuItemHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateContextMenuHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateContextMenuHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateToolbarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateToolbarHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateToolbarItemHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateToolbarItemHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateColorPickerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateColorPickerHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateDateTimePickerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateDateTimePickerHandler(control);

        /// <inheritdoc/>
        public WxControlHandler CreateScrollViewerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateScrollViewerHandler(control);
    }
}