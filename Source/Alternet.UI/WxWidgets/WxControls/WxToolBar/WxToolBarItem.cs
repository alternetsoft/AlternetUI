using System;
using System.Collections.Generic;
using System.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a toolbar.
    /// </summary>
    /// <remarks>
    /// Please use <see cref="AuiManager"/> and <see cref="AuiToolbar"/> instead of
    /// <see cref="WxToolBar"/> as it is deprecated and has limited functionality.
    /// </remarks>
    [ControlCategory("Hidden")]
    internal partial class WxToolBarItem : NonVisualControl, ICommandSource
    {
        private object? commandTarget;
        private object? commandParameter;
        private ICommand? command;
        private ImageSet? image = null;
        private ImageSet? disabledImage = null;
        private bool isChecked;
        private bool preCommandEnabledValue = true;

        /// <summary>
        /// Initializes a new instance of the <see cref='WxToolBarItem'/> class.
        /// </summary>
        public WxToolBarItem()
            : this(string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='WxToolBarItem'/> class
        /// with the specified text for the toolbar item.
        /// </summary>
        public WxToolBarItem(string text)
            : this(text, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='WxToolBarItem'/> class
        /// with a specified properties for the toolbar item.
        /// </summary>
        public WxToolBarItem(string text, EventHandler? onClick)
        {
            this.Text = text;
            if (onClick != null)
                Click += onClick;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Occurs when the <see cref="Checked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ToolbarItem;

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements
        {
            get
            {
                if (DropDownMenu != null)
                    return new[] { DropDownMenu };
                return Array.Empty<FrameworkElement>();
            }
        }

        /// <summary>
        /// Gets or sets a menu used as this toolbar item drop-down.
        /// </summary>
        public Menu? DropDownMenu
        {
            get => Handler.DropDownMenu;
            set => Handler.DropDownMenu = value;
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar
        /// item is checkable.
        /// </summary>
        public bool IsCheckable
        {
            get => Handler.IsCheckable;
            set => Handler.IsCheckable = value;
        }

        /// <summary>
        /// Gets or sets the image for the toolbar item.
        /// </summary>
        /// <value>
        /// An <see cref="ImageSet"/> that represents the image for the toolbar item.
        /// </value>
        public ImageSet? Image
        {
            get => image;

            set
            {
                if (image == value)
                    return;

                image = value;
                OnImageChanged(EventArgs.Empty);
                ImageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the disable image for the toolbar item.
        /// </summary>
        /// <value>
        /// An <see cref="ImageSet"/> that represents the disabled image
        /// for the toolbar item.
        /// </value>
        public ImageSet? DisabledImage
        {
            get => disabledImage;

            set
            {
                if (disabledImage == value)
                    return;

                disabledImage = value;
                OnImageChanged(EventArgs.Empty);
                ImageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a check mark appears next to
        /// the text of the toolbar item.
        /// </summary>
        public bool Checked
        {
            get
            {
                return isChecked;
            }

            set
            {
                if (isChecked == value)
                    return;

                isChecked = value;
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public object? CommandParameter
        {
            get
            {
                return commandParameter;
            }

            set
            {
                if (commandParameter == value)
                    return;
                commandParameter = value;
            }
        }

        /// <inheritdoc/>
        public object? CommandTarget
        {
            get
            {
                return commandTarget;
            }

            set
            {
                if (commandTarget == value)
                    return;
                commandTarget = value;
            }
        }

        /// <inheritdoc/>
        public ICommand? Command
        {
            get
            {
                return command;
            }

            set
            {
                if (command == value)
                    return;
                var oldCommand = command;
                command = value;
                OnCommandChanged(oldCommand, value);
            }
        }

        /// <summary>
        /// Gets a <see cref='WxToolBarItemHandler'/> associated with this class.
        /// </summary>
        internal new WxToolBarItemHandler Handler
        {
            get
            {
                return (WxToolBarItemHandler)base.Handler;
            }
        }

        /// <inheritdoc />
        protected override bool IsDummy => true;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection
        {
            get
            {
                if (DropDownMenu != null)
                    yield return DropDownMenu;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return base.ToString() ?? nameof(WxToolBarItem);
            else
                return Text;
        }

        /// <inheritdoc />
        public override void RaiseClick(EventArgs e)
        {
            base.RaiseClick(e);
            CommandHelpers.ExecuteCommandSource(this);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return new WxToolBarItemHandler();
        }

        /// <summary>
        /// Called when the value of the <see cref="Image"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnImageChanged(EventArgs e)
        {
        }

        private void OnCommandChanged(ICommand? oldCommand, ICommand? newCommand)
        {
            if (oldCommand != null)
                UnhookCommand(oldCommand);

            if (newCommand != null)
                HookCommand(newCommand);

            if (oldCommand != null && newCommand != null)
            {
                Enabled = preCommandEnabledValue;
            }
            else
            {
                if (oldCommand == null && newCommand != null)
                    preCommandEnabledValue = Enabled;

                UpdateCanExecute();
            }
        }

        private void UnhookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.RemoveHandler(
                command,
                OnCanExecuteChanged);
            UpdateCanExecute();
        }

        private void HookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.AddHandler(command, OnCanExecuteChanged);
            UpdateCanExecute();
        }

        private void OnCanExecuteChanged(object? sender, EventArgs e)
        {
            UpdateCanExecute();
        }

        private void UpdateCanExecute()
        {
            if (Command != null)
                Enabled = CommandHelpers.CanExecuteCommandSource(this);
        }
    }
}