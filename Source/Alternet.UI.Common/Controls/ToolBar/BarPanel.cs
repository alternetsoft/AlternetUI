using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a panel in the status bar or toolbar.
    /// </summary>
    public partial class BarPanel : FrameworkElement
    {
        private readonly BaseConcurrentStack<string> textStack = new();
        private PanelData data = new();

        /// <summary>
        /// Initializes a new instance of the <see cref='BarPanel'/> class.
        /// </summary>
        public BarPanel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='BarPanel'/> class with the specified kind.
        /// </summary>
        /// <param name="kind">The kind of the bar panel.</param>
        public BarPanel(BarPanelKind kind)
            : this()
        {
            this.data.Kind = kind;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='BarPanel'/> class with the
        /// specified text for the bar panel.
        /// </summary>
        /// <param name="text">The text for the bar panel.</param>
        public BarPanel(string text)
            : this()
        {
            this.data.Text = text;
        }

        /// <summary>
        /// Occurs when the <see cref="Text"/> property changes.
        /// </summary>
        public event EventHandler? TextChanged;

        /// <summary>
        /// Occurs when the <see cref="Width"/> property changes.
        /// </summary>
        public event EventHandler? WidthChanged;

        /// <summary>
        /// Occurs when the <see cref="MinWidth"/> property changes.
        /// </summary>
        public event EventHandler? MinWidthChanged;

        /// <summary>
        /// Occurs when the <see cref="HasBorder"/> property changes.
        /// </summary>
        public event EventHandler? HasBorderChanged;

        /// <summary>
        /// Occurs when control which is used to display panel content is created.
        /// </summary>
        public event EventHandler? ControlCreated;

        /// <summary>
        /// Occurs when control which is used to display panel content is clicked.
        /// </summary>
        public event EventHandler? ControlClicked;

        /// <summary>
        /// Occurs when control which is used to display panel content is updated.
        /// </summary>
        public event EventHandler? ControlUpdated;

        /// <summary>
        /// Occurs when the <see cref="HasBorder"/> property changes.
        /// </summary>
        [Obsolete("Use the HasBorderChanged event instead.")]
        public event EventHandler? StyleChanged;

        /// <summary>
        /// Gets or sets style of the status bar panel.
        /// </summary>
        [Obsolete("Use the HasBorder property instead.")]
        [Browsable(false)]
        public virtual StatusBarPanelStyle Style
        {
            get
            {
                return HasBorder ? StatusBarPanelStyle.Normal : StatusBarPanelStyle.Flat;
            }

            set
            {
                if (Style == value)
                    return;

                if (value == StatusBarPanelStyle.Normal)
                    HasBorder = true;
                else
                    HasBorder = false;

                StyleChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged(nameof(Style));
            }
        }

        /// <summary>
        /// Gets the control in which the panel is displayed.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? Control
        {
            get
            {
                return GetControl();
            }
        }

        /// <summary>
        /// Gets the index of the panel in <see cref="ToolBar.Panels"/> of the toolbar or status bar.
        /// If the panel is not added to any toolbar or status bar, returns null.
        /// </summary>
        public int? Index
        {
            get
            {
                if (Bar is null)
                    return null;

                var result = Bar.Panels.IndexOf(this);
                return result >= 0 ? result : null;
            }
        }

        /// <summary>
        /// Gets or sets the custom control displayed in the panel.
        /// This property is used when <see cref="BarPanelKind"/> is set to <see cref="BarPanelKind.CustomControl"/>.
        /// This property can be changed only when the panel is not attached to a bar.
        /// </summary>
        [Browsable(false)]
        public virtual AbstractControl? CustomControl
        {
            get
            {
                return data.CustomControl;
            }

            set
            {
                if (data.CustomControl == value)
                    return;

                var oldControl = data.CustomControl;
                data.CustomControl = value;

                ChangePanelControl(oldControl, value);

                RaisePropertyChanged(nameof(CustomControl));
            }
        }

        /// <summary>
        /// Gets or sets the SVG image displayed in the panel.
        /// </summary>
        public virtual SvgImage? SvgImage
        {
            get
            {
                return data.SvgImage;
            }

            set
            {
                if (data.SvgImage != value)
                {
                    data.SvgImage = value;
                    RaisePropertyChanged(nameof(SvgImage));
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the svg image.
        /// </summary>
        public virtual Color? SvgColor
        {
            get
            {
                return data.SvgColor;
            }

            set
            {
                if (data.SvgColor != value)
                {
                    data.SvgColor = value;
                    RaisePropertyChanged(nameof(SvgColor));
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the SVG image.
        /// </summary>
        public virtual int? SvgSize
        {
            get
            {
                return data.SvgSize;
            }

            set
            {
                if (data.SvgSize != value)
                {
                    data.SvgSize = value;
                    RaisePropertyChanged(nameof(SvgSize));
                }
            }
        }

        /// <summary>
        /// Gets or sets the image displayed in the panel. This property is applicable when the panel's <see cref="Kind"/>
        /// is set to <see cref="BarPanelKind.PictureBox"/> or <see cref="BarPanelKind.SpeedButton"/>.
        /// </summary>
        public virtual ImageSet? ImageSet
        {
            get => data.ImageSet;

            set
            {
                if (data.ImageSet == value)
                    return;
                data.ImageSet = value;
                RaisePropertyChanged(nameof(ImageSet));
            }
        }

        /// <summary>
        /// Gets or sets the image displayed in the panel when it is disabled. This property is applicable
        /// when the panel's <see cref="Kind"/> is set to <see cref="BarPanelKind.PictureBox"/>
        /// or <see cref="BarPanelKind.SpeedButton"/>.
        /// </summary>
        public virtual ImageSet? DisabledImageSet
        {
            get => data.DisabledImageSet;

            set
            {
                if (data.DisabledImageSet == value)
                    return;
                data.DisabledImageSet = value;
                RaisePropertyChanged(nameof(DisabledImageSet));
            }
        }

        /// <summary>
        /// Gets or sets the image displayed in the panel. This property is applicable when the panel's <see cref="Kind"/>
        /// is set to <see cref="BarPanelKind.PictureBox"/> or <see cref="BarPanelKind.SpeedButton"/>.
        /// </summary>
        public virtual Image? Image
        {
            get => data.Image;

            set
            {
                if (data.Image == value)
                    return;
                data.Image = value;
                RaisePropertyChanged(nameof(Image));
            }
        }

        /// <summary>
        /// Gets or sets the image displayed in the panel when it is disabled. This property is applicable
        /// when the panel's <see cref="Kind"/> is set to <see cref="BarPanelKind.PictureBox"/>
        /// or <see cref="BarPanelKind.SpeedButton"/>.
        /// </summary>
        public virtual Image? DisabledImage
        {
            get => data.DisabledImage;

            set
            {
                if (data.DisabledImage == value)
                    return;
                data.DisabledImage = value;
                RaisePropertyChanged(nameof(DisabledImage));
            }
        }

        /// <summary>
        /// Gets or sets the tool tip text for the panel.
        /// </summary>
        public virtual object? ToolTip
        {
            get => data.ToolTip;
            set
            {
                if (data.ToolTip == value)
                    return;
                data.ToolTip = value;
                RaisePropertyChanged(nameof(ToolTip));
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the panel inside the bar.
        /// </summary>
        public virtual HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return data.HorizontalAlignment;
            }

            set
            {
                if (data.HorizontalAlignment != value)
                {
                    data.HorizontalAlignment = value;
                    RaisePropertyChanged(nameof(HorizontalAlignment));
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the panel is visible.
        /// </summary>
        public virtual bool IsVisible
        {
            get
            {
                return data.IsVisible;
            }

            set
            {
                if (data.IsVisible != value)
                {
                    data.IsVisible = value;
                    RaisePropertyChanged(nameof(IsVisible));
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="PictureBox"/> control in which the panel is displayed
        /// in case the panel has <see cref="BarPanelKind.PictureBox"/> kind.
        /// </summary>
        [Browsable(false)]
        public PictureBox? AsPictureBox => Control as PictureBox;

        /// <summary>
        /// Gets the <see cref="SpeedButton"/> control in which the panel is displayed
        /// in case the panel has <see cref="BarPanelKind.SpeedButton"/> kind.
        /// </summary>
        [Browsable(false)]
        public SpeedButton? AsSpeedButton => Control as SpeedButton;

        /// <summary>
        /// Gets the <see cref="StdProgressBar"/> control in which the panel is displayed
        /// in case the panel has <see cref="BarPanelKind.ProgressBar"/> kind.
        /// </summary>
        [Browsable(false)]
        public StdProgressBar? AsProgressBar => Control as StdProgressBar;

        /// <summary>
        /// Gets the <see cref="Label"/> control in which the panel is displayed
        /// in case the panel has <see cref="BarPanelKind.Text"/> kind.
        /// </summary>
        [Browsable(false)]
        public Label? AsLabel => Control as Label;

        /// <summary>
        /// Gets or sets a value indicating the text displayed in the panel.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return data.Text;
            }

            set
            {
                if (value == data.Text)
                    return;

                data.Text = value;

                TextChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// Gets or sets width of the bar panel. By default, the width is determined by the content of the panel.
        /// When this property is changed, it's value is assigned to the <see cref="AbstractControl.SuggestedWidth"/>
        /// property of the control in which the panel is displayed. If you want to set the width of the panel
        /// to be determined by the content, set this property to <see cref="float.NaN"/>.
        /// </summary>
        public virtual float Width
        {
            get
            {
                return data.Width;
            }

            set
            {
                if (data.Width == value)
                    return;
                data.Width = value;

                WidthChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged(nameof(Width));
            }
        }

        /// <summary>
        /// Gets or sets the minimal width of the bar panel.
        /// </summary>
        public virtual float MinWidth
        {
            get
            {
                return data.MinWidth;
            }

            set
            {
                if (data.MinWidth == value)
                    return;
                data.MinWidth = value;

                MinWidthChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged(nameof(MinWidth));
            }
        }

        /// <summary>
        /// Gets or sets the maximal width of the bar panel.
        /// </summary>
        public virtual float? MaxWidth
        {
            get
            {
                return data.MaxWidth;
            }

            set
            {
                if (data.MaxWidth == value)
                    return;
                data.MaxWidth = value;

                RaisePropertyChanged(nameof(MaxWidth));
            }
        }

        /// <summary>
        /// Gets or sets the kind of the bar panel.
        /// If panel is already added to the bar, changing this property is not allowed.
        /// </summary>
        public virtual BarPanelKind Kind
        {
            get
            {
                return data.Kind;
            }

            set
            {
                if (data.Kind == value)
                    return;
                data.Kind = value;

                RaisePropertyChanged(nameof(Kind));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the panel automatically fills the available space on the bar as the bar is resized.
        /// </summary>
        /// <returns>
        /// true if the panel automatically fills the available space on the bar
        /// as the bar is resized; otherwise, false. The default is false.
        /// </returns>
        [DefaultValue(false)]
        public bool Spring
        {
            get
            {
                return HorizontalAlignment == HorizontalAlignment.Fill;
            }
            set
            {
                HorizontalAlignment = value ? HorizontalAlignment.Fill : HorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Pushes the current <see cref="Text"/> value onto the stack and sets a new text.
        /// </summary>
        /// <param name="text">The new text to set.</param>
        public virtual void PushText(string text)
        {
            textStack.Push(this.data.Text);
            Text = text;
        }

        /// <summary>
        /// Restores the previous text value that was saved by the last call to <see cref="PushText"/>.
        /// </summary>
        public virtual void PopText()
        {
            if (textStack.TryPop(out var value))
                Text = value;
        }


        /// <summary>
        /// Gets or sets whether the bar panel has a border. Default is false.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                return data.HasBorder;
            }

            set
            {
                if (data.HasBorder == value)
                    return;
                data.HasBorder = value;

                HasBorderChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged(nameof(HasBorder));
            }
        }

        /// <summary>
        /// Gets the toolbar or status bar that contains this panel.
        /// </summary>
        [Browsable(false)]
        public ToolBar? Bar
        {
            get
            {
                return LogicalParent as ToolBar;
            }
        }

        /// <summary>
        /// Gets control used to display the panel.
        /// </summary>
        /// <returns>The <see cref="AbstractControl"/> used to display the panel, or <c>null</c> if not found.</returns>
        public AbstractControl? GetControl()
        {
            if (Bar is null || !Bar.HasChildren)
                return null;

            foreach (var child in Bar.Children)
            {
                var panelId = child.CustomAttr.GetAttribute<ObjectUniqueId>(Bar.BarPanelIdPropName);
                if (panelId == UniqueId)
                    return child;
            }

            return null;
        }

        /// <summary>
        /// Gets the rectangle that represents the bounds of this panel within the bar.
        /// </summary>
        /// <returns></returns>
        public virtual RectD GetRect()
        {
            return Control?.Bounds ?? RectD.Empty;
        }

        /// <summary>
        /// Assigns properties from another <see cref="BarPanel"/>.
        /// </summary>
        /// <param name="item">Source of the properties to assign.</param>
        public virtual void Assign(BarPanel item)
        {
            data = item.data;
            Tag = item.Tag;
            AutomationId = item.AutomationId;
            Name = item.Name;
            RaisePropertyChanged();
        }

        /// <summary>
        /// Creates copy of this <see cref="BarPanel"/>.
        /// </summary>
        public virtual BarPanel Clone()
        {
            var result = new BarPanel();
            result.Assign(this);
            return result;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            switch (Kind)
            {
                case BarPanelKind.Text:
                case BarPanelKind.SpeedButton:
                case BarPanelKind.TextButton:
                    return $"[{Kind}] {Text}".TrimEnd();
                default:
                case BarPanelKind.Separator:
                case BarPanelKind.PictureBox:
                case BarPanelKind.ProgressBar:
                case BarPanelKind.Spacer:
                case BarPanelKind.CustomControl:
                    return $"[{Kind}]";
            }
        }

        /// <summary>
        /// Raises the <see cref="ControlCreated"/> event.
        /// </summary>
        public void RaiseControlCreated()
        {
            ControlCreated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ControlUpdated"/> event.
        /// </summary>
        public void RaiseControlUpdated()
        {
            ControlUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ControlClicked"/> event.
        /// </summary>
        public void RaiseControlClicked()
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Replaces the control associated with this panel with a new control.
        /// </summary>
        /// <param name="oldControl">The control to be replaced.</param>
        /// <param name="newControl">The new control to replace the old one.</param>
        protected virtual void ChangePanelControl(AbstractControl? oldControl, AbstractControl? newControl)
        {
            if (Bar is not null)
            {
                if (oldControl is not null)
                {
                    var oldIndex = oldControl.IndexInParent;
                    oldControl.Parent = null;
                    if (newControl is not null)
                    {
                        if (oldIndex is not null)
                            Bar.Children.Insert(oldIndex.Value, newControl);
                        else
                            Bar.Children.Add(newControl);
                    }
                }
                else
                {
                    if (newControl is not null)
                    {
                        var insertIndex = Bar.GetPanelControlInsertIndex(this);
                        Bar.Children.Insert(insertIndex, newControl);
                    }
                }
            }
        }

        private struct PanelData
        {
            public string Text = string.Empty;
            public float Width = float.NaN;
            public bool HasBorder;
            public float MinWidth;
            public float? MaxWidth;
            public BarPanelKind Kind = BarPanelKind.Text;
            public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Left;
            public bool IsVisible = true;
            public ImageSet? ImageSet;
            public ImageSet? DisabledImageSet;
            public object? ToolTip;
            public SvgImage? SvgImage;
            public Color? SvgColor;
            public int? SvgSize;
            public Image? Image;
            public Image? DisabledImage;
            public AbstractControl? CustomControl;

            public PanelData()
            {
            }
        }
    }
}
