using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a menu.
    /// </summary>
    public class MenuItem : Menu, ICommandSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class.
        /// </summary>
        public MenuItem() :
            this("")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with the specified text for the menu item.
        /// </summary>
        public MenuItem(string text) :
            this(text, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with the specified text for the menu item.
        /// </summary>
        public MenuItem(string text, KeyGesture shortcut) :
            this(text, null, shortcut)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with the specified text and action for the menu item.
        /// </summary>
        public MenuItem(string text, EventHandler onClick) :
            this(text, onClick, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with a specified properties for the menu item.
        /// </summary>
        public MenuItem(string text, EventHandler? onClick, KeyGesture? shortcut)
        {
            Text = text;
            Shortcut = shortcut;
            if (onClick != null)
                Click += onClick;
        }

        /// <inheritdoc />
        public override void RaiseClick(EventArgs e)
        {
            base.RaiseClick(e);
            CommandHelpers.ExecuteCommandSource(this);
        }

        string text = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating the caption of the menu item.
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

        KeyGesture? shortcut;

        /// <summary>
        /// Gets or sets a value indicating the shortcut key associated with the menu item.
        /// </summary>
        public KeyGesture? Shortcut
        {
            get
            {
                CheckDisposed();
                return shortcut;
            }

            set
            {
                CheckDisposed();

                if (shortcut == value)
                    return;

                shortcut = value;
                ShortcutChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Shortcut"/> property changes.
        /// </summary>
        public event EventHandler? ShortcutChanged;

        MenuItemRole? role;

        /// <summary>
        /// Gets or sets this menu item's role, which is used to automate macOS-specific standard items layout.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Menu items roles provide a mechanism for automatically adjusting certain menu items to macOS conventions.
        /// For example, "About", "Exit", and "Preferences" items should be placed in the application menu on macOS, and have a standard shortcuts.
        /// For more information, see <see cref="MenuItemRoles"/> class members.
        /// </para>
        /// <para>
        /// Setting <see cref="Role"/> to <see langword="null"/>
        /// has the same effect as the <see cref="MenuItemRoles.Auto"/> value. The <see langword="null"/> is the default value of this property.
        /// </para>
        /// </remarks>
        public MenuItemRole? Role
        {
            get
            {
                CheckDisposed();
                return role;
            }

            set
            {
                CheckDisposed();

                if (role == value)
                    return;

                role = value;
                RoleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Role"/> property changes.
        /// </summary>
        public event EventHandler? RoleChanged;

        bool @checked;

        /// <summary>
        /// Gets or sets a value indicating whether a check mark appears next to the text of the menu item.
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
            DependencyProperty.Register("Command", typeof(ICommand), typeof(MenuItem), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           MenuItem b = (MenuItem)d;
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
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(MenuItem), new PropertyMetadata(null));

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
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(MenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Occurs when the <see cref="Checked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;
    }
}