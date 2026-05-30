using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that is typically used to display information about the current
    /// state of the application, such as the status of a document, the position of the cursor, or other contextual information.
    /// It is derived from the <see cref="ToolBar"/> class and can contain not only simple text panels but also other types of controls,
    /// such as speed buttons, images, combo boxes, progress bars, and other interactive elements.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public partial class StatusBar : ToolBar
    {
        /// <summary>
        /// Represents the height, in dips, of the Visual Studio status bar.
        /// </summary>
        public const float VisualStudioStatusBarHeight = 28;

        /// <summary>
        /// Gets or sets the padding of the panel controls in case the panel style is set to <see cref="StatusBarPanelStyle.Normal"/>
        /// and panel has a border.
        /// </summary>
        public static Thickness DefaultNormalPanelPadding { get; set; } = new Thickness(4, 2);
        
        /// <summary>
        /// Gets or sets the padding of the panel controls in case the panel style is set to <see cref="StatusBarPanelStyle.Flat"/>
        /// and the panel has no border.
        /// </summary>
        public static Thickness DefaultFlatPanelPadding { get; set; } = new Thickness(2, 2);

        /// <summary>
        /// Gets or sets the default minimum height of the status bar.
        /// </summary>
        public static float DefaultMinHeight { get; set; } = VisualStudioStatusBarHeight;

        /// <summary>
        /// Gets or sets the default background color of the status bar.
        /// </summary>
        public static LightDarkColor DefaultBackgroundColor { get; set; }
            = new LightDarkColor(light: Color.FromArgb(218, 218, 218), dark:Color.FromArgb(28, 28, 28));

        /// <summary>
        /// Gets or sets the default foreground color of the status bar.
        /// </summary>
        public static LightDarkColor DefaultForegroundColor { get; set; }
            = new LightDarkColor(light: Color.Black, dark: Color.White);        

        private readonly string statusBarPanelIdPropName;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        public StatusBar()
        {
            Panels.ItemInserted += OnItemInserted;
            Panels.ItemRemoved += OnItemRemoved;
            MinHeight = DefaultMinHeight;
            statusBarPanelIdPropName =  AttributesFactory.GenUniqueAttributeName("StatusBarPanelId");

            ParentBackColor = false;
            ParentForeColor = false;
            BackgroundColor = DefaultBackgroundColor.Current;
            ForegroundColor = DefaultForegroundColor.Current;
            SizingGripVisible = true;
        }

        /// <summary>
        /// Gets or sets text of the first status bar panel.
        /// </summary>
        public new virtual string? Text
        {
            get
            {
                if (GetFieldsCount() < 0)
                    return string.Empty;
                return GetStatusText();
            }

            set
            {
                SetStatusText(value);
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="StatusBarPanel"/> objects associated with the control.
        /// </summary>
        [Content]
        public virtual BaseCollection<StatusBarPanel> Panels { get; } = new(CollectionSecurityFlags.NoNullOrReplace);

        /// <summary>
        /// Gets a sizing grip that allows the user to resize the parent form.
        /// </summary>
        [Browsable(false)]
        public GripControl? SizingGrip
        {
            get
            {
                var result = FirstChildOfType<GripControl>();
                return result;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a sizing grip is displayed in the
        /// lower-right corner of the control.
        /// </summary>
        public virtual bool SizingGripVisible
        {
            get
            {
                return SizingGrip?.Visible ?? false;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                var sizingGrip = SizingGrip;

                if (sizingGrip is null)
                {
                    if (value)
                    {
                        AddSizingGrip(() => Parent);
                    }
                }
                else
                {
                    sizingGrip.Visible = value;
                }
            }
        }

        /// <summary>
        /// Gets whether control is fully active and is attached to the window.
        /// </summary>
        public virtual bool IsOk
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return true;
            }
        }

        /// <summary>
        /// Gets or sets text of the first status bar panel. Same as <see cref="Text"/>
        /// property but implemented as method.
        /// </summary>
        public void SetText(string? value)
        {
            Text = value;
        }

        /// <summary>
        /// Adds new item to <see cref="Panels"/>.
        /// </summary>
        /// <param name="text">The text displayed in the status bar panel.</param>
        public StatusBarPanel Add(string text)
        {
            var result = new StatusBarPanel(text);
            Panels.Add(result);
            return result;
        }

        /// <summary>
        /// Returns number of the panels.
        /// </summary>
        /// <returns>
        /// number of the panels.
        /// </returns>
        public int GetFieldsCount()
        {
            return Panels.Count;
        }

        /// <summary>
        /// Returns panel by index.
        /// </summary>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns><see cref="StatusBarPanel"/> if found; <c>null</c> otherwise.</returns>
        public StatusBarPanel? GetField(int index)
        {
            if (index < 0 || index >= Panels.Count)
                return null;

            return Panels[index];
        }

        /// <summary>
        /// Sets the status text for the specified panel.
        /// </summary>
        /// <param name="text">The text to be set. Use an empty string or <c>null</c>
        /// to clear the panel.</param>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns><c>true</c> if success; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// The given text will replace the current text. The display of the status bar is
        /// updated immediately.
        /// </remarks>
        public bool SetStatusText(string? text = null, int index = 0)
        {
            if (!IsOk)
                return false;
            var field = GetField(index);
            if (field == null)
                return false;
            text ??= string.Empty;
            field.Text = text;
            return true;
        }

        /// <summary>
        /// Adds separator panel to the status bar.
        /// </summary>
        /// <returns>The newly created <see cref="StatusBarPanel"/> representing the separator.</returns>
        public new virtual StatusBarPanel AddSeparator()
        {
            var result = new StatusBarPanel()
            {
                Kind = StatusBarPanelKind.Separator,
            };

            Panels.Add(result);

            return result;
        }

        /// <summary>
        /// Gets the status text for the specified panel.
        /// </summary>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns>
        /// <see cref="string"/> with the status text if success; <c>null</c> otherwise.
        /// </returns>
        public string? GetStatusText(int index = 0)
        {
            if (!IsOk)
                return null;
            var field = GetField(index);
            if (field == null)
                return null;
            return field.Text;
        }

        /// <summary>
        /// Saves the current status text in a per-panel stack, and sets the
        /// status text to the string passed as argument.
        /// </summary>
        /// <param name="text">New panel status text.</param>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns><c>true</c> if success; <c>false</c> otherwise.</returns>
        /// <seealso cref="PopStatusText"/>
        public bool PushStatusText(string? text = null, int index = 0)
        {
            if (!IsOk)
                return false;
            var field = GetField(index);
            if (field == null)
                return false;
            text ??= string.Empty;
            field.PushText(text);
            return true;
        }

        /// <summary>
        /// Restores the text to the value it had before the last call
        /// to <see cref="PushStatusText"/>.
        /// </summary>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns><c>true</c> if success; <c>false</c> otherwise.</returns>
        /// <seealso cref="PushStatusText"/>
        public bool PopStatusText(int index = 0)
        {
            if (!IsOk)
                return false;
            var field = GetField(index);
            if (field == null)
                return false;
            field.PopText();
            return true;
        }

        /// <summary>
        /// Sets the widths of the panels.
        /// </summary>
        /// <param name="widths">Contains an array of width, each of which is an
        /// absolute status panel width in dips.</param>
        /// <returns><c>true</c> if success; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// Size of the <paramref name="widths"/> array must be equal to the number passed to
        /// <see cref="SetFieldsCount"/> the last time it was called.
        /// </remarks>
        public bool SetStatusWidths(float[] widths)
        {
            if (!IsOk || widths.Length == 0)
                return false;

            for (int i = 0; i < widths.Length; i++)
            {
                var field = GetField(i);
                if (field == null)
                    return false;
                field.Width = widths[i];
            }

            return true;
        }

        /// <summary>
        /// Sets the number of panels.
        /// </summary>
        /// <param name="count">New number of panels. Must be greater than zero.</param>
        /// <returns><c>true</c> if success; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// If <paramref name="count"/> is greater than the previous number of panels,
        /// then new panels with empty strings will be added to the status bar.
        /// </remarks>
        public bool SetFieldsCount(int count)
        {
            if (!IsOk || count < 1)
                return false;
            Panels.SetCount(count, () =>
            {
                return Add(string.Empty);
            });
            return true;
        }

        /// <summary>
        /// Gets the width of the specified panel.
        /// </summary>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns>
        /// <see cref="float"/> with the panel width if success; <c>null</c> otherwise.
        /// </returns>
        public float? GetStatusWidth(int index)
        {
            if (!IsOk)
                return null;
            var field = GetField(index);
            if (field == null)
                return null;
            return field.Width;
        }

        /// <summary>
        /// Gets the style of the specified panel.
        /// </summary>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method doesn't affect <see cref="Panels"/>, it works with the native control.
        /// </remarks>
        public StatusBarPanelStyle? GetStatusStyle(int index)
        {
            if (!IsOk)
                return null;
            var field = GetField(index);
            if (field == null)
                return null;
            return field.Style;
        }

        /// <summary>
        /// Sets the styles of the panels in the status bar.
        /// </summary>
        /// <param name="styles">Contains an array of <see cref="StatusBarPanelStyle"/> with
        /// the styles for each panel.</param>
        /// <returns><c>true</c> if success; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// Size of the <paramref name="styles"/> array must be equal to the number passed to
        /// <see cref="SetFieldsCount"/> the last time it was called.
        /// </remarks>
        public bool SetStatusStyles(StatusBarPanelStyle[] styles)
        {
            if (!IsOk || styles.Length == 0)
                return false;
            for(int i = 0; i < styles.Length; i++)
            {
                var field = GetField(i);
                if (field == null)
                    return false;
                field.Style = styles[i];
            }

            return true;
        }

        /// <summary>
        /// Gets the size and position of a panels's internal bounding rectangle.
        /// </summary>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns><see cref="RectD"/> with the size and position of a panels's
        /// internal bounding rectangle on success; <c>null</c> otherwise.</returns>
        public RectD? GetFieldRect(int index)
        {
            if (!IsOk)
                return null;
            var field = GetField(index);
            if(field == null)
                return null;
            return field.GetRect();
        }

        /// <summary>
        /// Sets the minimal possible height for the status bar.
        /// </summary>
        /// <param name="height">New height value.</param>
        /// <returns><c>true</c> if success; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// The real height may be bigger than the height specified here depending
        /// on the size of the font used by the status bar.
        /// </remarks>
        public bool SetMinHeight(float height)
        {
            this.MinHeight = height;
            return true;
        }

        /// <summary>
        /// Clears <see cref="Panels"/>.
        /// </summary>
        public void Clear()
        {
            Panels.Clear();
        }

        /// <summary>
        /// Sets colors used in the control to the dark theme.
        /// </summary>
        public virtual void SetColorThemeToDark()
        {
            DoInsideUpdate(() =>
            {
                BackgroundColor = DefaultBackgroundColor.Dark;
                ForegroundColor = DefaultForegroundColor.Dark;
            });
        }

        /// <summary>
        /// Sets colors used in the control to the light theme.
        /// </summary>
        public virtual void SetColorThemeToLight()
        {
            DoInsideUpdate(() =>
            {
                BackgroundColor = DefaultBackgroundColor.Light;
                ForegroundColor = DefaultForegroundColor.Light;
            });
        }

        /// <summary>
        /// Gets control used to display the status bar panel.
        /// </summary>
        /// <param name="panel">The <see cref="StatusBarPanel"/> for which to get the control.</param>
        /// <returns>The <see cref="AbstractControl"/> used to display the panel, or <c>null</c> if not found.</returns>
        public AbstractControl? GetFieldControl(StatusBarPanel? panel)
        {
            if (!IsOk || !HasChildren || panel is null)
                return null;

            foreach (var child in Children)
            {
                var panelId = child.CustomAttr.GetAttribute<ObjectUniqueId>(statusBarPanelIdPropName);
                if (panelId == panel.UniqueId)
                    return child;
            }

            return null;
        }

        /// <summary>
        /// Adds separator panel to the status bar.
        /// </summary>
        /// <returns>The newly created <see cref="StatusBarPanel"/> representing the separator.</returns>
        internal new StatusBarPanel AddSeparatorCore()
        {
            return AddSeparator();
        }

        /// <summary>
        /// Called when item was inserted in the <see cref="Panels"/>.
        /// </summary>
        /// <param name="sender">The source of the event, typically the collection that raised the event.
        /// Can be <see langword="null"/>.</param>
        /// <param name="index">The zero-based index at which the item was inserted.</param>
        /// <param name="item">The <see cref="StatusBarPanel"/> instance that was inserted.
        /// This parameter is never <see langword="null"/>.</param>
        protected virtual void OnItemInserted(object? sender, int index, StatusBarPanel item)
        {
            item.PropertyChanged += OnItemPropertyChanged;
            item.StatusBar = this;

            var insertAfterPanel = GetField(index - 1);
            var insertAfterPanelControl = GetFieldControl(insertAfterPanel);

            AbstractControl panelControl;

            if (index < 0)
            {
                index = 0;
            }
            else
            if (insertAfterPanelControl is null)
            {
                index = Panels.Count;
            }
            else
            {
                var panelControlIndex = Children.IndexOf(insertAfterPanelControl);

                if (panelControlIndex < 0)
                {
                    index = Panels.Count;
                }
                else
                {
                    index = panelControlIndex + 1;
                }
            }

            if (index > Panels.Count)
            {
                index = Panels.Count;
            }

            switch (item.Kind)
            {
                default:
                case StatusBarPanelKind.Text:
                    panelControl = InsertTextCore(index, item.Text);
                    break;
                case StatusBarPanelKind.Separator:
                    panelControl = InsertSeparatorCore(index);
                    break;
            }

            panelControl.CustomAttr.SetAttribute(statusBarPanelIdPropName, item.UniqueId);
            UpdatePanelControl(item, panelControl);
        }

        /// <summary>
        /// Called when property of the item in the <see cref="Panels"/> was changed.
        /// </summary>
        /// <param name="panel">The <see cref="StatusBarPanel"/> instance whose property changed.</param>
        /// <param name="control">The <see cref="AbstractControl"/> associated with the panel.</param>
        protected virtual bool UpdatePanelControl(StatusBarPanel? panel, AbstractControl? control = null)
        {
            if (panel is null)
                return false;

            control ??= GetFieldControl(panel);

            if (control == null)
                return false;

            switch (panel.Kind)
            {
                default:
                case StatusBarPanelKind.Text:
                    UpdateTextPanel();
                    break;
                case StatusBarPanelKind.Separator:
                    break;
            }

            void UpdateTextPanel()
            {
                control.Text = panel.Text;
                control.SuggestedWidth = panel.Width;
                control.MinWidth = panel.MinWidth;
                control.HasBorder = panel.Style == StatusBarPanelStyle.Normal;

                if (panel.Style == StatusBarPanelStyle.Normal)
                {
                    control.Padding = DefaultNormalPanelPadding;
                }
                else
                {
                    control.Padding = DefaultFlatPanelPadding;
                }
            }

            control.HorizontalAlignment = panel.HorizontalAlignment;

            return true;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            foreach (var panel in Panels)
            {
                OnItemRemoved(panel);
            }

            base.DisposeManaged();
        }

        /// <summary>
        /// Handles the "PropertyChanged" event for an item in the panel collection.
        /// </summary>
        /// <remarks>This method is called when a property of an item in the collection changes. Derived
        /// classes can override this method to provide custom handling for property changes.</remarks>
        /// <param name="sender">The source of the event, typically the item whose property changed.
        /// Can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the property change.</param>
        protected virtual void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdatePanelControl(sender as StatusBarPanel);
        }

        /// <summary>
        /// Invoked when an item is removed from the panels collection.
        /// </summary>
        /// <remarks>This method detaches event handlers from the removed item and updates the state of
        /// the status bar panels. Subclasses can override this method to provide additional behavior
        /// when an item is removed.</remarks>
        /// <param name="sender">The source of the event, typically the collection that raised the event.
        /// Can be <see langword="null"/>.</param>
        /// <param name="index">The zero-based index at which the item was removed.</param>
        /// <param name="item">The <see cref="StatusBarPanel"/> instance that was removed.
        /// This parameter is never <see langword="null"/>.</param>
        protected virtual void OnItemRemoved(object? sender, int index, StatusBarPanel item)
        {
            OnItemRemoved(item);
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            if (AutoUpdateColors)
            {
                if (SystemSettings.AppearanceIsDark)
                    SetColorThemeToDark();
                else
                    SetColorThemeToLight();
            }

            base.OnSystemColorsChanged(e);
        }

        private void OnItemRemoved(StatusBarPanel item)
        {
            var control = item.Control;

            item.PropertyChanged -= OnItemPropertyChanged;
            item.StatusBar = null;

            if (control != null)
            {
                control.Parent = null;
                control.Dispose();
            }
        }
    }
}