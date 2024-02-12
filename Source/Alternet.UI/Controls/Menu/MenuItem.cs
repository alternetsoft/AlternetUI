using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a menu.
    /// </summary>
    [ControlCategory("Hidden")]
    public partial class MenuItem : Menu, ICommandSource
    {
        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the
        /// <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(MenuItem),
                new FrameworkPropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the
        /// <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(MenuItem),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> field for the
        /// <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register(
                "CommandTarget",
                typeof(object),
                typeof(MenuItem),
                new PropertyMetadata(null));

        private KeyGesture? shortcut;
        private string text = string.Empty;
        private MenuItemRole? role;
        private bool preCommandEnabledValue = true;
        private bool isChecked;
        private Action? action;
        private ImageSet? image;
        private ImageSet? disabledImage;

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class.
        /// </summary>
        public MenuItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> and <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public MenuItem(string text, KeyGesture shortcut)
        {
            this.text = text;
            this.shortcut = shortcut;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem" /> class that displays the
        /// specified text and image and that does the specified action when
        /// the <see cref="MenuItem" /> is clicked.</summary>
        /// <param name="text">The text to display on the menu item.</param>
        /// <param name="image">The <see cref="Image" /> to display on the control.</param>
        /// <param name="onClick">An event handler that raises the <see cref="Control.Click" />
        /// event when the control is clicked.</param>
        public MenuItem(string text, Image image, EventHandler onClick)
        {
            this.text = text;
            Click += onClick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> for the menu item.
        /// </summary>
        public MenuItem(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/>, <paramref name="onClick"/> and
        /// <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public MenuItem(string text, EventHandler? onClick = null, KeyGesture? shortcut = null)
        {
            this.text = text;
            this.shortcut = shortcut;
            if (onClick != null)
                Click += onClick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> and <paramref name="onClick"/>
        /// for the menu item.
        /// </summary>
        public MenuItem(string text, Action? onClick)
        {
            this.text = text;
            ClickAction = onClick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/>, <paramref name="onClick"/> and
        /// <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public MenuItem(string text, Action? onClick, KeyGesture? shortcut)
        {
            this.text = text;
            this.shortcut = shortcut;
            ClickAction = onClick;
        }

        /// <summary>
        /// Occurs when the <see cref="Shortcut"/> property changes.
        /// </summary>
        public event EventHandler? ShortcutChanged;

        /// <summary>
        /// Occurs when the <see cref="Checked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Occurs when the <see cref="Role"/> property changes.
        /// </summary>
        public event EventHandler? RoleChanged;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.MenuItem;

        /// <summary>
        /// Gets or sets image associated with the menu item.
        /// </summary>
        public ImageSet? Image
        {
            get
            {
                return image;
            }

            set
            {
                if (image == value)
                    return;
                image = value;
                NativeControl.NormalImage = image?.NativeImageSet;
            }
        }

        /// <summary>
        /// Gets or sets disabled image associated with the menu item.
        /// </summary>
        public ImageSet? DisabledImage
        {
            get
            {
                return disabledImage;
            }

            set
            {
                if (disabledImage == value)
                    return;
                disabledImage = value;
                NativeControl.DisabledImage = disabledImage?.NativeImageSet;
            }
        }

        /// <summary>
        /// Gets or sets the shortcut keys associated with the
        /// <see cref="MenuItem" />.</summary>
        /// <returns>
        /// One of the <see cref="Keys" /> values. The default is <see cref="Keys.None" />.</returns>
        [Localizable(true)]
        [DefaultValue(Keys.None)]
        [Browsable(false)]
        public Keys ShortcutKeys
        {
            get
            {
                if (Shortcut is null)
                    return Keys.None;
                var result = Shortcut.Key.ToKeys(Shortcut.Modifiers);
                return result;
            }

            set
            {
                var key = value.ToKey();
                var modifiers = value.ToModifiers();
                Shortcut = new(key, modifiers);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which will be executed when
        /// this <see cref="MenuItem"/> is clicked by the user.
        /// </summary>
        [Browsable(false)]
        public Action? ClickAction
        {
            get => action;
            set
            {
                if (action != null)
                    Click -= OnClickAction;
                action = value;
                if (action != null)
                    Click += OnClickAction;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the caption of the menu item.
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
        /// Gets or sets a value indicating the shortcut key associated with
        /// the menu item.
        /// </summary>
        public KeyGesture? Shortcut
        {
            get
            {
                return shortcut;
            }

            set
            {
                if (shortcut == value)
                    return;

                shortcut = value;
                ShortcutChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets this menu item's role, which is used to automate
        /// macOS-specific standard items layout.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Menu items roles provide a mechanism for automatically adjusting
        /// certain menu items to macOS conventions.
        /// For example, "About", "Exit", and "Preferences" items should be
        /// placed in the application menu on macOS, and have a standard shortcuts.
        /// For more information, see <see cref="MenuItemRoles"/> class members.
        /// </para>
        /// <para>
        /// Setting <see cref="Role"/> to <see langword="null"/>
        /// has the same effect as the <see cref="MenuItemRoles.Auto"/> value.
        /// The <see langword="null"/> is the default value of this property.
        /// </para>
        /// </remarks>
        public MenuItemRole? Role
        {
            get
            {
                return role;
            }

            set
            {
                if (role == value)
                    return;

                role = value;
                RoleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a check mark appears next
        /// to the text of the menu item.
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
        public ICommand? Command
        {
            get { return (ICommand?)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <inheritdoc/>
        public object? CommandParameter
        {
            get { return (object?)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <inheritdoc/>
        public object? CommandTarget
        {
            get { return GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        internal override bool IsDummy => true;

        internal new MenuItemHandler Handler => (MenuItemHandler)base.Handler;

        internal new Native.MenuItem NativeControl => (Native.MenuItem)base.NativeControl;

        /// <summary>
        /// Implicit conversion operator from <see cref="string"/> to
        /// <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="s">Value of the <see cref="MenuItem.Text"/> property.
        /// </param>
        public static implicit operator MenuItem(string s)
        {
            return new(s);
        }

        /// <inheritdoc cref="ListControlItem.ToString"/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return base.ToString() ?? nameof(MenuItem);
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
            return GetEffectiveControlHandlerHactory().CreateMenuItemHandler(this);
        }

        private static void OnCommandChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
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

        private void OnClickAction(object? sender, EventArgs? e)
        {
            action?.Invoke();
        }
    }
}