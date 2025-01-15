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
        private KeyGesture? shortcut;
        private MenuItemRole? role;
        private bool isChecked;
        private Action? action;
        private CachedSvgImage<ImageSet> svgImage = new();
        private bool shortcutEnabled = true;
        private CommandSourceStruct commandSource;

        static MenuItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class.
        /// </summary>
        public MenuItem()
        {
            commandSource = new(this);
            commandSource.Changed = () =>
            {
                Enabled = commandSource.CanExecute;
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> and <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public MenuItem(string? text, KeyGesture shortcut)
            : this()
        {
            Text = text ?? string.Empty;
            this.shortcut = shortcut;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem" /> class that displays the
        /// specified text and image and that does the specified action when
        /// the <see cref="MenuItem" /> is clicked.</summary>
        /// <param name="text">The text to display on the menu item.</param>
        /// <param name="image">The <see cref="Image" /> to display on the control.</param>
        /// <param name="onClick">An event handler that raises the
        /// <see cref="AbstractControl.Click" />
        /// event when the control is clicked.</param>
        public MenuItem(string? text, Image image, EventHandler onClick)
            : this()
        {
            Text = text ?? string.Empty;
            Click += onClick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> for the menu item.
        /// </summary>
        public MenuItem(string? text)
            : this()
        {
            Text = text ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/>, <paramref name="onClick"/> and
        /// <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public MenuItem(string? text, EventHandler? onClick = null, KeyGesture? shortcut = null)
            : this()
        {
            Text = text ?? string.Empty;
            this.shortcut = shortcut;
            if (onClick != null)
                Click += onClick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> and <paramref name="onClick"/>
        /// for the menu item.
        /// </summary>
        public MenuItem(string? text, Action? onClick)
            : this()
        {
            Text = text ?? string.Empty;
            ClickAction = onClick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> and <paramref name="onClick"/>
        /// for the menu item.
        /// </summary>
        public MenuItem(string? text, Func<bool>? onClick)
            : this()
        {
            Text = text ?? string.Empty;
            if(onClick is not null)
                ClickAction = () => onClick();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/>, <paramref name="onClick"/> and
        /// <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public MenuItem(string? text, Action? onClick, KeyGesture? shortcut)
            : this()
        {
            Text = text ?? string.Empty;
            this.shortcut = shortcut;
            ClickAction = onClick;
        }

        /// <summary>
        /// Occurs when menu item is opened.
        /// </summary>
        public event EventHandler? Opened;

        /// <summary>
        /// Occurs when menu item is closed.
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Occurs when menu item is highlighted.
        /// </summary>
        public event EventHandler? Highlighted;

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

        /// <summary>
        /// Occurs when the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Occurs when the <see cref="DisabledImage"/> property changes.
        /// </summary>
        public event EventHandler? DisabledImageChanged;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.MenuItem;

        /// <summary>
        /// Gets or sets <see cref="ImageSet"/> associated with the menu item.
        /// </summary>
        public virtual ImageSet? Image
        {
            get
            {
                return svgImage.GetImage(VisualControlState.Normal);
            }

            set
            {
                if (Image == value)
                    return;
                svgImage.SetImage(VisualControlState.Normal, value);
                RaiseImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="SvgImage"/> associated with the menu item.
        /// </summary>
        public virtual SvgImage? SvgImage
        {
            get
            {
                return svgImage.SvgImage;
            }

            set
            {
                if (svgImage.SvgImage == value)
                    return;
                svgImage.SvgImage = value;
                RaiseImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets size of the svg image.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// Currently only rectangular svg images are supported.
        /// </remarks>
        [Browsable(false)]
        public virtual SizeI? SvgImageSize
        {
            get => svgImage.SvgSize;

            set
            {
                if (SvgImageSize == value)
                    return;
                svgImage.SvgSize = value;
                if(SvgImage is not null)
                {
                    RaiseImageChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets disabled image associated with the menu item.
        /// </summary>
        public virtual ImageSet? DisabledImage
        {
            get
            {
                return svgImage.GetImage(VisualControlState.Disabled);
            }

            set
            {
                if (DisabledImage == value)
                    return;
                svgImage.SetImage(VisualControlState.Disabled, value);
                RaiseDisabledImageChanged();
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
        public virtual Keys ShortcutKeys
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
        /// Gets or sets function which is called inside
        /// <see cref="ContextMenu.OnOpening(CancelEventArgs)"/> in order
        /// to update enabled state.
        /// </summary>
        [Browsable(false)]
        public virtual Func<bool>? EnabledFunc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which will be executed when
        /// this <see cref="MenuItem"/> is clicked by the user.
        /// </summary>
        [Browsable(false)]
        public virtual Action? ClickAction
        {
            get => action;
            set
            {
                action = value;
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
                return base.Text;
            }

            set
            {
                if (value == Text)
                    return;

                base.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="Shortcut"/> is enabled.
        /// </summary>
        public virtual bool IsShortcutEnabled
        {
            get
            {
                return shortcutEnabled;
            }

            set
            {
                if (shortcutEnabled == value)
                    return;
                shortcutEnabled = value;
                RaiseShortcutChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the shortcut key associated with
        /// the menu item.
        /// </summary>
        public virtual KeyGesture? Shortcut
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
                RaiseShortcutChanged();
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
        public virtual MenuItemRole? Role
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
        public virtual bool Checked
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
        public virtual ICommand? Command
        {
            get
            {
                return commandSource.Command;
            }

            set
            {
                commandSource.Command = value;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public virtual object? CommandTarget
        {
            get
            {
                return commandSource.CommandParameter;
            }

            set
            {
                commandSource.CommandParameter = value;
            }
        }

        /// <inheritdoc/>
        public virtual object? CommandParameter
        {
            get
            {
                return commandSource.CommandParameter;
            }

            set
            {
                commandSource.CommandParameter = value;
            }
        }

        /// <inheritdoc/>
        protected override bool IsDummy => true;

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

        /// <summary>
        /// Same as <see cref="ShortcutKeys"/> but implemented as method.
        /// </summary>
        /// <param name="keys"></param>
        public void SetShortcutKeys(Keys keys) => ShortcutKeys = keys;

        /// <summary>
        /// Raises <see cref="Opened"/> event.
        /// </summary>
        public void RaiseOpened()
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="Closed"/> event.
        /// </summary>
        public void RaiseClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="Highlighted"/> event.
        /// </summary>
        public void RaiseHighlighted() => Highlighted?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return base.ToString() ?? nameof(MenuItem);
            else
                return Text;
        }

        /// <summary>
        /// Raises <see cref="ShortcutChanged"/> event.
        /// </summary>
        public void RaiseShortcutChanged()
        {
            ShortcutChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="ImageChanged"/> event.
        /// </summary>
        public void RaiseImageChanged()
        {
            ImageChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="DisabledImageChanged"/> event.
        /// </summary>
        public void RaiseDisabledImageChanged()
        {
            DisabledImageChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets real menu image for the specified state constructed from the
        /// following properties: <see cref="Image"/>, <see cref="DisabledImage"/>,
        /// <see cref="SvgImage"/>, <see cref="SvgImageSize"/>.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag.</param>
        /// <param name="control">Control which scale factor is used.</param>
        /// <returns></returns>
        public virtual ImageSet? GetRealImage(
            VisualControlState state,
            bool? isDark = null,
            Control? control = null)
        {
            svgImage.UpdateImageSet(
                state,
                isDark ?? control?.IsDarkBackground ?? SystemSettings.IsUsingDarkBackground,
                control);

            var img = state == VisualControlState.Disabled ? DisabledImage : Image;
            return img;
        }

        /// <inheritdoc />
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            action?.Invoke();
            commandSource.Execute();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            commandSource.Changed = null;
            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return (IControlHandler)ControlFactory.Handler.CreateMenuItemHandler(this);
        }
    }
}