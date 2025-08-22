using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a menu.
    /// </summary>
    [ControlCategory("Hidden")]
    public partial class MenuItem : Menu, ICommandSource, IMenuItemProperties
    {
        /// <summary>
        /// Represents the default size of the menu arrow image.
        /// </summary>
        /// <remarks>The size is specified as a <see cref="CoordAndUnit"/> with a value
        /// of 50 and a unit of <see cref="CoordUnit.Percent"/> (default toolbar image size is
        /// used as a base value for the calculation).
        /// This value is used as the default when no specific size is provided for
        /// menu arrow images.</remarks>
        public static CoordAndUnit DefaultMenuArrowImageSize = new(75, CoordUnit.Percent);

        private static KnownButtonImage? defaultMenuArrowImage;

        private KeyGesture? shortcut;
        private MenuItemRole? role;
        private bool isChecked;
        private Action? action;
        private CachedSvgImage<ImageSet> svgImage = new();
        private bool shortcutEnabled = true;
        private CommandSourceStruct commandSource;
        private Func<bool>? enabledFunc;

        static MenuItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class.
        /// </summary>
        public MenuItem()
        {
            commandSource = new(this);
            commandSource.Changed = RaiseCommandSourceChanged;
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
        public MenuItem(string? text, Image? image, EventHandler onClick)
            : this()
        {
            Text = text ?? string.Empty;
            if (image is not null)
                Image = new ImageSet(image);
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
            if (onClick is not null)
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
        /// Occurs when a change is detected, providing details about the type of change.
        /// </summary>
        /// <remarks>This event is triggered whenever a change occurs, and
        /// it provides information about
        /// the change through the <see cref="BaseEventArgs{T}"/> parameter.
        /// The <see cref="MenuItemChangeKind"/> value
        /// indicates the specific type of change that occurred.</remarks>
        public event EventHandler<BaseEventArgs<MenuItemChangeKind>>? Changed;

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

        /// <summary>
        /// Occurs when the associated click action has changed.
        /// </summary>
        /// <remarks>This event is triggered whenever the click action is updated,
        /// allowing subscribers to respond to the change.</remarks>
        public event EventHandler? ClickActionChanged;

        /// <summary>
        /// Gets or sets the default arrow image which is shown when menu item
        /// has a submenu.
        /// </summary>
        public static KnownButtonImage DefaultMenuArrowImage
        {
            get
            {
                if(defaultMenuArrowImage is null)
                {
                    defaultMenuArrowImage = new(KnownSvgImages.ImgAngleRight, DefaultMenuArrowImageSize);
                }

                return defaultMenuArrowImage.Value;
            }

            set
            {
                defaultMenuArrowImage = value;
            }
        }

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
                if (SvgImage is not null)
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
            get => enabledFunc;

            set
            {
                if (enabledFunc == value)
                    return;
                enabledFunc = value;
                RaiseEnabledChanged(EventArgs.Empty);
            }
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
                if (action == value)
                    return;
                action = value;
                RaiseClickActionChanged();
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

                RaiseChanged(MenuItemChangeKind.Text);
            }
        }

        /// <inheritdoc/>
        public override bool Visible
        {
            get => base.Visible;
            set
            {
                if (Visible == value)
                    return;
                base.Visible = value;
                RaiseChanged(MenuItemChangeKind.Visible);
            }
        }

        /// <inheritdoc/>
        public override bool Enabled
        {
            get => base.Enabled;

            set
            {
                if (Enabled == value)
                    return;
                base.Enabled = value;
                RaiseChanged(MenuItemChangeKind.Enabled);
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
                RaiseRoleChanged();
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
                RaiseCheckedChanged();
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

        ICommandSource IMenuItemProperties.CommandSource => this;

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
        public void SetShortcutKeys(Keys keys)
        {
            ShortcutKeys = keys;
        }

        /// <summary>
        /// Notifies that the state of the command source has changed.
        /// </summary>
        /// <remarks>This method should be called when the conditions affecting
        /// the command source's
        /// ability to execute have changed. It updates the enabled state
        /// to reflect the current state of the command source.</remarks>
        public virtual void RaiseCommandSourceChanged()
        {
            Enabled = commandSource.CanExecute;
            RaiseChanged(MenuItemChangeKind.CommandSource);
        }

        /// <summary>
        /// Raises the <see cref="ClickActionChanged"/> event to notify
        /// subscribers of a change in the click action.
        /// </summary>
        /// <remarks>This method invokes the <see cref="ClickActionChanged"/> event with the current
        /// instance as the sender and an empty <see cref="EventArgs"/> object.
        /// Ensure that any subscribers to the event
        /// are properly registered to handle the notification.</remarks>
        public virtual void RaiseClickActionChanged()
        {
            ClickActionChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuItemChangeKind.ClickAction);
        }

        /// <summary>
        /// Raises <see cref="Opened"/> event.
        /// </summary>
        public virtual void RaiseOpened()
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="Closed"/> event.
        /// </summary>
        public virtual void RaiseClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="Highlighted"/> event.
        /// </summary>
        public virtual void RaiseHighlighted()
        {
            Highlighted?.Invoke(this, EventArgs.Empty);
        }

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
        public virtual void RaiseShortcutChanged()
        {
            ShortcutChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuItemChangeKind.Shortcut);
        }

        /// <summary>
        /// Raises <see cref="ImageChanged"/> event.
        /// </summary>
        public virtual void RaiseImageChanged()
        {
            ImageChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuItemChangeKind.Image);
        }

        /// <summary>
        /// Raises <see cref="DisabledImageChanged"/> event.
        /// </summary>
        public virtual void RaiseDisabledImageChanged()
        {
            DisabledImageChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuItemChangeKind.DisabledImage);
        }

        /// <summary>
        /// Raises the <see cref="RoleChanged"/> event to notify subscribers of a role change.
        /// </summary>
        public virtual void RaiseRoleChanged()
        {
            RoleChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuItemChangeKind.Role);
        }

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event to notify subscribers
        /// of a checked state change.
        /// </summary>
        public virtual void RaiseCheckedChanged()
        {
            CheckedChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuItemChangeKind.Checked);
        }

        /// <summary>
        /// Raises an event or performs an action to indicate that a change
        /// of the specified kind has occurred.
        /// </summary>
        /// <remarks>Derived classes can override this method to provide custom
        /// behavior when a change is raised.</remarks>
        /// <param name="kind">The type of change that occurred.
        /// This value determines the nature of the change being reported.</param>
        public virtual void RaiseChanged(MenuItemChangeKind kind)
        {
            Changed?.Invoke(this, new (kind));
        }

        /// <summary>
        /// Copies the properties from the specified source to the current instance.
        /// </summary>
        /// <param name="source">An object implementing <see cref="IMenuItemProperties"/>
        /// whose properties will be copied. Cannot be <see langword="null"/>.</param>
        public virtual void Assign(IMenuItemProperties source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            Text = source.Text;
            Image = source.Image;
            DisabledImage = source.DisabledImage;
            SvgImage = source.SvgImage;
            SvgImageSize = source.SvgImageSize;
            Shortcut = source.Shortcut;
            IsShortcutEnabled = source.IsShortcutEnabled;
            Role = source.Role;
            Checked = source.Checked;
            ClickAction = source.DoClick;
            Command = source.CommandSource.Command;
            CommandParameter = source.CommandSource.CommandParameter;
            Enabled = source.Enabled;
            Visible = source.Visible;
        }

        void IMenuItemProperties.DoClick()
        {
            RaiseClick();
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