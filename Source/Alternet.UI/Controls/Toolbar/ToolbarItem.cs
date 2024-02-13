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
    /// <see cref="ToolBar"/> as it is deprecated and has limited functionality.
    /// </remarks>
    [ControlCategory("Hidden")]
    public partial class ToolBarItem : NonVisualControl, ICommandSource
    {
        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the
        /// <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(ToolBarItem),
                new FrameworkPropertyMetadata(
                    null, new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the
        /// <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(ToolBarItem),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the
        /// <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register(
                "CommandTarget",
                typeof(object),
                typeof(ToolBarItem),
                new PropertyMetadata(null));

        private string text = string.Empty;
        private ImageSet? image = null;
        private ImageSet? disabledImage = null;
        private bool isChecked;
        private bool preCommandEnabledValue = true;

        /// <summary>
        /// Initializes a new instance of the <see cref='ToolBarItem'/> class.
        /// </summary>
        public ToolBarItem()
            : this(string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='ToolBarItem'/> class
        /// with the specified text for the toolbar item.
        /// </summary>
        public ToolBarItem(string text)
            : this(text, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='ToolBarItem'/> class
        /// with a specified properties for the toolbar item.
        /// </summary>
        public ToolBarItem(string text, EventHandler? onClick)
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
        /// Gets or sets a value indicating the caption of the toolbar item.
        /// </summary>
        /// <remarks>
        /// Setting this property to "-" makes the item a separator.
        /// Use underscore (<c>_</c>) symbol before a character to make it
        /// an access key.
        /// </remarks>
        public override string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (value == text)
                    return;

                text = value;
                OnTextChanged(EventArgs.Empty);
            }
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
            get { return (object?)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <inheritdoc/>
        public object CommandTarget
        {
            get { return GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        /// <inheritdoc/>
        public ICommand? Command
        {
            get { return (ICommand?)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets a <see cref='ToolBarItemHandler'/> associated with this class.
        /// </summary>
        internal new ToolBarItemHandler Handler
        {
            get
            {
                return (ToolBarItemHandler)base.Handler;
            }
        }

        internal override bool IsDummy => true;

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
                return base.ToString() ?? nameof(ToolBarItem);
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
        internal override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateToolbarItemHandler(this);
        }

        /// <summary>
        /// Called when the value of the <see cref="Image"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnImageChanged(EventArgs e)
        {
        }

        private static void OnCommandChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
           ToolBarItem b = (ToolBarItem)d;
           b.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
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