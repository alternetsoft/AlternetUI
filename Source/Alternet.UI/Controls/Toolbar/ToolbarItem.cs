using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a toolbar.
    /// </summary>
    public class ToolbarItem : Control, ICommandSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='ToolbarItem'/> class.
        /// </summary>
        public ToolbarItem() :
            this("")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref='ToolbarItem'/> class with the specified text for the toolbar item.
        /// </summary>
        public ToolbarItem(string text) :
            this(text, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='ToolbarItem'/> class with a specified properties for the toolbar item.
        /// </summary>
        public ToolbarItem(string text, EventHandler? onClick)
        {
            Text = text;
            if (onClick != null)
                Click += onClick;
        }

        public Menu DropDownMenu { get; set; }

        /// <inheritdoc />
        public override void RaiseClick(EventArgs e)
        {
            base.RaiseClick(e);
            CommandHelpers.ExecuteCommandSource(this);
        }

        string text = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating the caption of the toolbar item.
        /// </summary>
        /// <remarks>
        /// Setting this property to "-" makes the item a separator.
        /// Use underscore (<c>_</c>) symbol before a character to make it an access key.
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
        /// Occurs when the <see cref="Text"/> property changes.
        /// </summary>
        public event EventHandler? TextChanged;

        bool @checked;

        /// <summary>
        /// Gets or sets a value indicating whether a check mark appears next to the text of the toolbar item.
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
        public ICommand? Command
        {
            get { return (ICommand?)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ToolbarItem), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        bool preCommandEnabledValue = true;

        private void UnhookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.RemoveHandler(command, OnCanExecuteChanged);
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

        /// <inheritdoc/>
        public object? CommandParameter
        {
            get { return (object?)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ToolbarItem), new PropertyMetadata(null));

        /// <inheritdoc/>
        public IInputElement? CommandTarget
        {
            get { return (IInputElement?)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ToolbarItem), new PropertyMetadata(null));

        /// <summary>
        /// Occurs when the <see cref="Checked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;
    }
}