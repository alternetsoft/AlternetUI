namespace Alternet.UI
{
    /// <summary>
    /// Implements an <see cref="IControlHandlerFactory"/> for the Native visual theme.
    /// </summary>
    public class NativeControlHandlerFactory : IControlHandlerFactory
    {
        /// <inheritdoc/>
        public ControlHandler CreateButtonHandler(Control control) =>
            new NativeButtonHandler();

        /// <inheritdoc/>
        public ControlHandler CreateStackPanelHandler(Control control) =>
            new StackPanelHandler();

        /// <inheritdoc/>
        public ControlHandler CreateBorderHandler(Control control) =>
            new GenericBorderHandler();

        /// <inheritdoc/>
        public ControlHandler CreatePictureBoxHandler(Control control) =>
            new PictureBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateLabelHandler(Control control) =>
            new NativeLabelHandler();

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
            new NativeTabControlHandler();

        /// <inheritdoc/>
        public ControlHandler CreateGroupBoxHandler(Control control) =>
            new NativeGroupBoxHandler();

        /// <inheritdoc/>
        public ControlHandler CreateProgressBarHandler(Control control) =>
            new NativeProgressBarHandler();

        /// <inheritdoc/>
        public ControlHandler CreateSliderHandler(Control control) =>
            new NativeSliderHandler();

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
            new NativeMainMenuHandler();

        /// <inheritdoc/>
        public ControlHandler CreateMenuItemHandler(Control control) =>
            new NativeMenuItemHandler();

        /// <inheritdoc/>
        public ControlHandler CreateContextMenuHandler(Control control) =>
            new NativeContextMenuHandler();

        /// <inheritdoc/>
        public ControlHandler CreateToolbarHandler(Control control) =>
            new NativeToolbarHandler();

        /// <inheritdoc/>
        public ControlHandler CreateToolbarItemHandler(Control control) =>
            new NativeToolbarItemHandler();

        /// <inheritdoc/>
        public ControlHandler CreateStatusBarHandler(Control control) =>
            new NativeStatusBarHandler();

        /// <inheritdoc/>
        public ControlHandler CreateStatusBarPanelHandler(Control control) =>
            new NativeStatusBarPanelHandler();

        /// <inheritdoc/>
        public ControlHandler CreateColorPickerHandler(Control control) =>
            new NativeColorPickerHandler();

        /// <inheritdoc/>
        public ControlHandler CreateDateTimePickerHandler(Control control) =>
            new NativeDateTimePickerHandler();

        /// <inheritdoc/>
        public ControlHandler CreateScrollViewerHandler(Control control) =>
            new NativeScrollViewerHandler();
    }
}