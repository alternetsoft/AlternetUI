namespace Alternet.UI
{
    /// <summary>
    /// Implements an <see cref="IControlHandlerFactory"/> for the Generic visual theme.
    /// </summary>
    internal class GenericControlHandlerFactory : IControlHandlerFactory
    {
        /// <inheritdoc/>
        public ControlHandler CreateButtonHandler(Control control) =>
            new GenericButtonHandler();

        /// <inheritdoc/>
        public ControlHandler CreateStackPanelHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateStackPanelHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateBorderHandler(Control control) =>
            new Border.BorderHandler();

        /// <inheritdoc/>
        public ControlHandler CreatePictureBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreatePictureBoxHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateLabelHandler(Control control) =>
            new GenericLabelHandler();

        /// <inheritdoc/>
        public ControlHandler CreateTextBoxHandler(Control control) =>
            new GenericTextBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateCheckBoxHandler(Control control) =>
            new GenericCheckBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateRadioButtonHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateRadioButtonHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateAuiToolbarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateAuiToolbarHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateAuiNotebookHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateAuiNotebookHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateTabControlHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateTabControlHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateGroupBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateGroupBoxHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreatePropertyGridHandler(Control control) =>
            StockControlHandlerFactories.Native.CreatePropertyGridHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateProgressBarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateProgressBarHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateSliderHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateSliderHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateNumericUpDownHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateNumericUpDownHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateCheckListBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateCheckListBoxHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateListBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateListBoxHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateComboBoxHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateComboBoxHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateListViewHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateListViewHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateTreeViewHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateTreeViewHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateMainMenuHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateMainMenuHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateMenuItemHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateMenuItemHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateContextMenuHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateContextMenuHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateToolbarHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateToolbarHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateToolbarItemHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateToolbarItemHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateColorPickerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateColorPickerHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateDateTimePickerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateDateTimePickerHandler(control);

        /// <inheritdoc/>
        public ControlHandler CreateScrollViewerHandler(Control control) =>
            StockControlHandlerFactories.Native.CreateScrollViewerHandler(control);
    }
}