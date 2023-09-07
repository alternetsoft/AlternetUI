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
    /// <see cref="Toolbar"/> as it is deprecated and has limited functionality.
    /// </remarks>
    public class ToolbarItem : NonVisualControl, ICommandSource
    {
        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the
        /// <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(ToolbarItem),
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
                typeof(ToolbarItem),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the
        /// <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register(
                "CommandTarget",
                typeof(IInputElement),
                typeof(ToolbarItem),
                new PropertyMetadata(null));

        private string text = string.Empty;
        private ImageSet? image = null;
        private ImageSet? disabledImage = null;
        private bool @checked;
        private bool preCommandEnabledValue = true;

        /// <summary>
        /// Initializes a new instance of the <see cref='ToolbarItem'/> class.
        /// </summary>
        public ToolbarItem()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='ToolbarItem'/> class
        /// with the specified text for the toolbar item.
        /// </summary>
        public ToolbarItem(string text)
            : this(text, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='ToolbarItem'/> class
        /// with a specified properties for the toolbar item.
        /// </summary>
        public ToolbarItem(string text, EventHandler? onClick)
        {
            Text = text;
            if (onClick != null)
                Click += onClick;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Occurs when the <see cref="Text"/> property changes.
        /// </summary>
        public event EventHandler? TextChanged;

        /// <summary>
        /// Occurs when the <see cref="Checked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Gets a <see cref='ToolbarItemHandler'/> associated with this class.
        /// </summary>
        public new ToolbarItemHandler Handler
        {
            get
            {
                CheckDisposed();
                return (ToolbarItemHandler)base.Handler;
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.ToolbarItem;

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
        public string Text
        {
            get
            {
                CheckDisposed();
                return text;
            }

            set
            {
                CheckDisposed();

                if (value == text)
                    return;

                text = value;
                TextChanged?.Invoke(this, EventArgs.Empty);
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
                CheckDisposed();
                return @checked;
            }

            set
            {
                CheckDisposed();

                if (@checked == value)
                    return;

                @checked = value;
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
        public IInputElement? CommandTarget
        {
            get { return (IInputElement?)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        /// <inheritdoc/>
        public ICommand? Command
        {
            get { return (ICommand?)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
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

        /// <inheritdoc />
        public override void RaiseClick(EventArgs e)
        {
            base.RaiseClick(e);
            CommandHelpers.ExecuteCommandSource(this);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
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
           ToolbarItem b = (ToolbarItem)d;
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