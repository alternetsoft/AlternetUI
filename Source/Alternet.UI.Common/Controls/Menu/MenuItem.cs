using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Localization;

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

        private ShortcutInfo? shortcutInfo;
        private MenuItemRole? role;
        private bool isChecked;
        private Action? action;
        private CachedSvgImage<ImageSet> svgImage = new();
        private bool shortcutEnabled = true;
        private CommandSourceStruct commandSource;
        private Func<bool>? enabledFunc;
        private string text = string.Empty;
        private bool visible = true;
        private bool enabled = true;
        private ContextMenu? itemsMenu;

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
            this.shortcutInfo = new ShortcutInfo(shortcut);
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
            this.shortcutInfo = new ShortcutInfo(shortcut);
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
            this.shortcutInfo = new ShortcutInfo(shortcut);
            ClickAction = onClick;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Enabled"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? EnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? VisibleChanged;

        /// <summary>
        /// Occurs when the menu item is clicked.
        /// </summary>
        [Category("Action")]
        public virtual event EventHandler? Click;

        /// <summary>
        /// Occurs when the <see cref="Text" /> property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? TextChanged;

        /// <summary>
        /// Occurs when a change is detected, providing details about the type of change.
        /// </summary>
        /// <remarks>This event is triggered whenever a change occurs, and
        /// it provides information about
        /// the change through the <see cref="BaseEventArgs{T}"/> parameter.
        /// The <see cref="MenuChangeKind"/> value
        /// indicates the specific type of change that occurred.</remarks>
        [Category("Property Changed")]
        public event EventHandler<BaseEventArgs<MenuChangeKind>>? Changed;

        /// <summary>
        /// Occurs when menu item is opened.
        /// </summary>
        [Category("Action")]
        public event EventHandler? Opened;

        /// <summary>
        /// Occurs when menu item is closed.
        /// </summary>
        [Category("Action")]
        public event EventHandler? Closed;

        /// <summary>
        /// Occurs when menu item is highlighted.
        /// </summary>
        [Category("Action")]
        public event EventHandler? Highlighted;

        /// <summary>
        /// Occurs when the <see cref="Shortcut"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? ShortcutChanged;

        /// <summary>
        /// Occurs when the <see cref="Checked"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Occurs when the <see cref="Role"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? RoleChanged;

        /// <summary>
        /// Occurs when the <see cref="Image"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Occurs when the <see cref="DisabledImage"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? DisabledImageChanged;

        /// <summary>
        /// Occurs when the associated click action has changed.
        /// </summary>
        /// <remarks>This event is triggered whenever the click action is updated,
        /// allowing subscribers to respond to the change.</remarks>
        [Category("Property Changed")]
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

        /// <summary>
        /// Gets or sets the associated shortcut keys.
        /// </summary>
        /// <returns>
        /// One of the <see cref="Keys" /> values.
        /// The default is <see cref="Keys.None" />.</returns>
        [Localizable(true)]
        [DefaultValue(Keys.None)]
        [Browsable(false)]
        public virtual Keys ShortcutKeys
        {
            get
            {
                return shortcutInfo;
            }

            set
            {
                Invoke(() =>
                {
                    if (ShortcutKeys == value)
                        return;
                    shortcutInfo = value;
                    RaiseShortcutChanged();
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the associated shortcut key.
        /// </summary>
        [Browsable(false)]
        public virtual KeyGesture? Shortcut
        {
            get
            {
                return shortcutInfo;
            }

            set
            {
                Invoke(() =>
                {
                    if (Shortcut == value)
                        return;
                    shortcutInfo = value;
                    RaiseShortcutChanged();
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the associated shortcut key.
        /// </summary>
        [Browsable(false)]
        public virtual ShortcutInfo? ShortcutInfo
        {
            get
            {
                return shortcutInfo;
            }

            set
            {
                Invoke(() =>
                {
                    if (shortcutInfo == value)
                        return;
                    shortcutInfo = value;
                    RaiseShortcutChanged();
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the associated shortcut key.
        /// </summary>
        [Browsable(false)]
        public virtual KeyInfo[]? ShortcutKeyInfo
        {
            get
            {
                return shortcutInfo;
            }

            set
            {
                Invoke(() =>
                {
                    shortcutInfo = value;
                    RaiseShortcutChanged();
                });
            }
        }

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
                Invoke(() =>
                {
                    if (Image == value)
                        return;
                    svgImage.SetImage(VisualControlState.Normal, value);
                    RaiseImageChanged();
                });
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
                Invoke(() =>
                {
                    if (svgImage.SvgImage == value)
                        return;
                    svgImage.SvgImage = value;
                    RaiseImageChanged();
                });
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
                Invoke(() =>
                {
                    if (SvgImageSize == value)
                        return;
                    svgImage.SvgSize = value;
                    if (SvgImage is not null)
                    {
                        RaiseImageChanged();
                    }
                });
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
                Invoke(() =>
                {
                    if (DisabledImage == value)
                        return;
                    svgImage.SetImage(VisualControlState.Disabled, value);
                    RaiseDisabledImageChanged();
                });
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
                Invoke(() =>
                {
                    if (enabledFunc == value)
                        return;
                    enabledFunc = value;
                    RaiseEnabledChanged(EventArgs.Empty);
                });
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
                Invoke(() =>
                {
                    if (action == value)
                        return;
                    action = value;
                    RaiseClickActionChanged();
                });
            }
        }

        /// <summary>
        /// Gets a value indicating whether the text represents a separator.
        /// </summary>
        [Browsable(false)]
        public bool IsSeparator
        {
            get
            {
                return Text == "-";
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
        public virtual string Text
        {
            get
            {
                return text;
            }

            set
            {
                Invoke(() =>
                {
                    if (value == Text)
                        return;

                    text = value;

                    RaiseTextChanged(EventArgs.Empty);
                });
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="Text"/> property should be localizable.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsTextLocalized
        {
            get
            {
                return IntFlags[LocalizationManager.ShouldLocalizeTextIdentifier];
            }

            set
            {
                IntFlags[LocalizationManager.ShouldLocalizeTextIdentifier] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the menu item is visible.
        /// </summary>
        /// <remarks>Changing this property raises a change notification event to indicate that the
        /// visibility state has been updated.</remarks>
        [Browsable(false)]
        public virtual bool Visible
        {
            get => visible;
            set
            {
                Invoke(() =>
                {
                    if (visible == value)
                        return;
                    visible = value;
                    RaiseVisibleChanged(EventArgs.Empty);
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the menu item is enabled.
        /// </summary>
        /// <remarks>Changing this property raises the <c>EnabledChanged</c> event and notifies listeners
        /// of the change.</remarks>
        public bool IsEnabled
        {
            get => Enabled;
            set => Enabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the menu item is enabled.
        /// </summary>
        /// <remarks>Changing this property raises the <c>EnabledChanged</c> event and notifies listeners
        /// of the change.</remarks>
        [Browsable(false)]
        public virtual bool Enabled
        {
            get => enabled;
            set
            {
                Invoke(() =>
                {
                    if (enabled == value)
                        return;
                    enabled = value;
                    RaiseChanged(MenuChangeKind.Enabled);
                });
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
                Invoke(() =>
                {
                    if (shortcutEnabled == value)
                        return;
                    shortcutEnabled = value;
                    RaiseShortcutChanged();
                });
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
                Invoke(() =>
                {
                    if (role == value)
                        return;

                    role = value;
                    RaiseRoleChanged();
                });
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
                Invoke(() =>
                {
                    if (isChecked == value)
                        return;

                    isChecked = value;
                    RaiseCheckedChanged();
                });
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
                Invoke(() =>
                {
                    commandSource.Command = value;
                });
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
                Invoke(() =>
                {
                    commandSource.CommandParameter = value;
                });
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
                Invoke(() =>
                {
                    commandSource.CommandParameter = value;
                });
            }
        }

        ICommandSource IMenuItemProperties.CommandSource => this;

        /// <summary>
        /// Gets the menu with the child menu-items.
        /// </summary>
        [Browsable(false)]
        public virtual ContextMenu ItemsMenu
        {
            get
            {
                if (itemsMenu is null)
                {
                    itemsMenu = new ContextMenu();
                    itemsMenu.LogicalParent = this;
                    itemsMenu.Items = Items;
                }

                return itemsMenu;
            }
        }

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
        /// Raises the <see cref="EnabledChanged"/> event and calls
        /// <see cref="OnEnabledChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public virtual void RaiseEnabledChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnEnabledChanged(e);
            EnabledChanged?.Invoke(this, e);
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
            if (DisposingOrDisposed)
                return;
            Enabled = commandSource.CanExecute;
            RaiseChanged(MenuChangeKind.CommandSource);
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
            if (DisposingOrDisposed)
                return;
            ClickActionChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuChangeKind.ClickAction);
        }

        /// <summary>
        /// Raises <see cref="Opened"/> event.
        /// </summary>
        public virtual void RaiseOpened()
        {
            if (DisposingOrDisposed)
                return;
            Opened?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="Closed"/> event.
        /// </summary>
        public virtual void RaiseClosed()
        {
            if (DisposingOrDisposed)
                return;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="Highlighted"/> event.
        /// </summary>
        public virtual void RaiseHighlighted()
        {
            if (DisposingOrDisposed)
                return;
            Highlighted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Same as <see cref="Enabled"/> but implemented as method.
        /// </summary>
        /// <param name="value">New <see cref="Enabled"/> property value.</param>
        public void SetEnabled(bool value) => Enabled = value;

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
        /// Calls <see cref="RaiseClick(EventArgs)"/> with an empty arguments.
        /// </summary>
        public void RaiseClick() => RaiseClick(EventArgs.Empty);

        /// <summary>
        /// Raises the <see cref="Click"/> event and calls
        /// <see cref="OnClick(EventArgs)"/>.
        /// See <see cref="Click"/> event description for more details.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            OnClick(e);
            action?.Invoke();
            commandSource.Execute();
            Click?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ShortcutChanged"/> event.
        /// </summary>
        public virtual void RaiseShortcutChanged()
        {
            if (DisposingOrDisposed)
                return;
            ShortcutChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuChangeKind.Shortcut);
        }

        /// <summary>
        /// Raises <see cref="ImageChanged"/> event.
        /// </summary>
        public virtual void RaiseImageChanged()
        {
            if (DisposingOrDisposed)
                return;
            ImageChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuChangeKind.Image);
        }

        /// <summary>
        /// Raises <see cref="DisabledImageChanged"/> event.
        /// </summary>
        public virtual void RaiseDisabledImageChanged()
        {
            if (DisposingOrDisposed)
                return;
            DisabledImageChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuChangeKind.DisabledImage);
        }

        /// <summary>
        /// Raises <see cref="VisibleChanged"/> event and <see cref="OnVisibleChanged"/>
        /// method.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseVisibleChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            OnVisibleChanged(e);
            VisibleChanged?.Invoke(this, e);
            RaiseChanged(MenuChangeKind.Visible);
        }

        /// <summary>
        /// Raises the <see cref="RoleChanged"/> event to notify subscribers of a role change.
        /// </summary>
        public virtual void RaiseRoleChanged()
        {
            if (DisposingOrDisposed)
                return;
            RoleChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuChangeKind.Role);
        }

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event to notify subscribers
        /// of a checked state change.
        /// </summary>
        public virtual void RaiseCheckedChanged()
        {
            if (DisposingOrDisposed)
                return;
            CheckedChanged?.Invoke(this, EventArgs.Empty);
            RaiseChanged(MenuChangeKind.Checked);
        }

        /// <summary>
        /// Raises an event or performs an action to indicate that a change
        /// of the specified kind has occurred.
        /// </summary>
        /// <remarks>Derived classes can override this method to provide custom
        /// behavior when a change is raised.</remarks>
        /// <param name="kind">The type of change that occurred.
        /// This value determines the nature of the change being reported.</param>
        public virtual void RaiseChanged(MenuChangeKind kind)
        {
            if(DisposingOrDisposed)
                return;
            Changed?.Invoke(this, new (kind));
            if(LogicalParent is Menu parentMenu)
                parentMenu.RaiseItemChanged(this, kind);
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
            ClickAction = source.RaiseClick;
            Command = source.CommandSource.Command;
            CommandParameter = source.CommandSource.CommandParameter;
            Enabled = source.Enabled;
            Visible = source.Visible;
        }

        /// <summary>
        /// Raises the <see cref="TextChanged" /> event.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseTextChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            TextChanged?.Invoke(this, e);
            OnTextChanged(e);
            RaiseChanged(MenuChangeKind.Text);
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

        /// <summary>
        /// Called when the enabled of the <see cref="Enabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when menu item is clicked.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="TextChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnVisibleChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            Shortcut = null;
            commandSource.Changed = null;
            base.DisposeManaged();
        }
    }
}