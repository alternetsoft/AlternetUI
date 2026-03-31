using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic check box control. This control is implemented inside the library and can be used in the
    /// same way as a regular native <see cref="CheckBox"/> control. <see cref="StdCheckBox"/> is used when you need
    /// to have the same code for all platforms. <see cref="StdCheckBox"/> provides many additional features,
    /// which are not available in the native <see cref="CheckBox"/> control.
    /// </summary>
    /// <remarks>
    /// Use a <see cref="StdCheckBox"/> to give the user an option, such as true/false or yes/no.
    /// The <see cref="StdCheckBox"/> control can display an image or text or both.
    /// </remarks>
    [ControlCategory("Common")]
    public partial class StdCheckBox : GenericItemControl
    {
        /// <summary>
        /// Gets or sets a default value of the check box margin. This margin is used when check box mark is painted.
        /// </summary>
        public static Thickness DefaultCheckBoxMargin = (3, 0, 0, 0);

        /// <summary>
        /// Gets or sets the default foreground color used when a control is hovered, supporting both light and dark
        /// themes.
        /// </summary>
        /// <remarks>Assign this field to customize the appearance of controls when hovered. If not set,
        /// the control may use a framework or theme-defined default color.</remarks>
        public static LightDarkColor? DefaultHoveredForeColor;

        /// <summary>
        /// Gets or sets the default color used for a hovered check box in both light and dark themes.
        /// </summary>
        public static LightDarkColor? DefaultHoveredCheckBoxColor = DefaultColors.AccentColor.LighterPair();

        /// <summary>
        /// Gets or sets a value indicating whether the hovered color should be used for check boxes
        /// when the control is in the hovered state. If set to true, the check box will be drawn using <see cref="DefaultHoveredCheckBoxColor"/>
        /// </summary>
        public static bool UseHoveredCheckBoxColor = true;

        private bool useHoveredForeColor = false;
        private LightDarkColor? hoveredForeColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="StdCheckBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public StdCheckBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdCheckBox"/> class with the specified text.
        /// </summary>
        /// <param name="text"></param>
        public StdCheckBox(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdCheckBox"/> class.
        /// </summary>
        public StdCheckBox()
        {
            ItemDefaults.CheckBoxVisible = true;
            WantTab = true;
            IsGraphicControl = false;
            Item.CheckBoxMargin = DefaultCheckBoxMargin;
            ConfigureAccentFocusedBorder();
            Item.BeforeDrawCheckBox = OnBeforeDrawCheckBox;
        }

        /// <summary>
        /// Gets or sets the foreground color to use when the control is in the hovered state.
        /// <see cref="UseHoveredForeColor"/> must be set to true for this property to take effect.
        /// </summary>
        /// <remarks>If this property is null, the control uses <see cref="DefaultHoveredForeColor"/> when hovered.
        /// This property allows customization of the text or foreground appearance specifically for the hovered visual
        /// state.</remarks>
        public virtual LightDarkColor? HoveredForeColor
        {
            get => hoveredForeColor;
            set
            {
                if(hoveredForeColor == value)
                    return;
                hoveredForeColor = value;
                if (UseHoveredForeColor && VisualState == VisualControlState.Hovered)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control uses a different foreground color when hovered.
        /// <see cref="HoveredForeColor"/> and <see cref="DefaultHoveredForeColor"/> are used only if this property
        /// is set to true. Setting this property to false will make the control use the same foreground color in all states, including hovered."/>
        /// </summary>
        public virtual bool UseHoveredForeColor
        {
            get => useHoveredForeColor;
            set
            {
                if(useHoveredForeColor == value) return;
                useHoveredForeColor = value;
                if (UseHoveredForeColor && VisualState == VisualControlState.Hovered)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to align check box on the right side of the text.
        /// </summary>
        [DefaultValue(false)]
        public virtual bool AlignRight
        {
            get
            {
                return Item.IsCheckRightAligned;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (Item.IsCheckRightAligned == value)
                    return;
                Item.IsCheckRightAligned = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.CheckBox;

        /// <summary>
        /// Configures the control to use accent color for focused border. This method is called in the constructor, so the
        /// control will use accent color for focused border by default.
        /// </summary>
        public virtual void ConfigureAccentFocusedBorder()
        {
            HasBorder = true;

            Borders ??= new();
            Borders.Normal = BorderSettings.TransparentBorder.Clone();

            Borders.SetAllExceptNormal(
                (state) => Borders.Normal?.Clone(),
                VisualControlStates.Hovered | VisualControlStates.Pressed | VisualControlStates.Disabled);

            Borders.Focused = BorderSettings.AccentBorder.Clone();
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
        }

        /// <summary>
        /// Provides an opportunity to perform custom actions before a check box is drawn.
        /// </summary>
        /// <remarks>Override this method to customize behavior or appearance before the check box is
        /// rendered. This method is called during the drawing process for each item's check box.</remarks>
        /// <param name="item">The list control item for which the check box is about to be drawn.</param>
        /// <param name="info">Information about the check box associated with the specified item.</param>
        protected virtual void OnBeforeDrawCheckBox(ListControlItem item, ListControlItem.ItemCheckBoxInfo info)
        {
            var state = VisualState;

            if (state == VisualControlState.Hovered && UseHoveredCheckBoxColor)
            {
                var color = DefaultHoveredCheckBoxColor ?? LightDarkColors.Red;

                info.Color = color.LightOrDark(IsDarkBackground);
            }
            else
            {
                info.Color = null;
            }
        }

        /// <inheritdoc/>
        protected override void ConfigureItemDrawable(ListItemDrawable itemDrawable, PaintEventArgs e)
        {
            base.ConfigureItemDrawable(itemDrawable, e);

            if (itemDrawable.VisualState == VisualControlState.Hovered && UseHoveredForeColor)
            {
                var color = HoveredForeColor ?? DefaultHoveredForeColor ?? DefaultColors.AccentColor;
                Item.ForegroundColor = color.LightOrDark(IsDarkBackground);
            }
            else
                Item.ForegroundColor = null;
        }
    }
}