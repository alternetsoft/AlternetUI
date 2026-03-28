using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a base class for controls that display an inner <see cref="PictureBox"/> control with an image,
    /// typically used to indicate validation errors or other status information. Error image and tooltip are assigned
    /// by default, but can be customized by derived classes or via <see cref="InnerPicture"/> property.
    /// The inner picture is optional and can be shown or hidden as needed.
    /// By default, the inner picture is hidden
    /// and can be shown by setting the <see cref="InnerPictureVisible"/> property to true.
    /// When the inner picture is shown, it is docked to the right of the control and can display
    /// an image and tooltip to provide additional information to the user. Initalization of the inner picture
    /// is deferred until it is accessed for the first time, allowing for efficient resource usage when the picture is not needed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class manages an inner picture control that can be shown or hidden to indicate
    /// validation errors or other status information.
    /// Derived classes can customize the appearance and behavior of the inner picture by overriding
    /// related properties and methods.
    /// </para>
    /// <para>
    /// This is the parent class for <see cref="ControlAndButton"/> and
    /// <see cref="ControlAndLabel{TControl,TLabel}"/> controls. 
    /// </para>
    /// </remarks>
    [ControlCategory("Hidden")]
    public abstract partial class ControlAndPicture : HiddenBorder
    {
        private PictureBox? pictureBox;
        private int? innerPictureSvgSize;
        private SvgImage? innerPictureSvg;
        private MessageBoxIcon? innerPictureToolTipMessageIcon = MessageBoxIcon.Error;
        private bool isToolTipShownOnInnerPictureClick = true;
        private Color? innerPictureSvgColor;
        private KnownSvgColor innerPictureKnownSvgColor = KnownSvgColor.Normal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndPicture"/> class.
        /// </summary>
        public ControlAndPicture()
        {
        }

        /// <summary>
        /// Gets or sets the icon to display in the tooltip message for the inner picture, if any.
        /// </summary>
        [Browsable(false)]
        public virtual MessageBoxIcon? InnerPictureToolTipMessageIcon
        {
            get
            {
                return innerPictureToolTipMessageIcon;
            }

            set
            {
                innerPictureToolTipMessageIcon = value;
            }
        }

        /// <summary>
        /// Gets or sets the known SVG color used for the inner picture when svg image is used.
        /// This property is used to determine the color scheme for the inner picture based on predefined color categories.
        /// In order to set custom color for the inner picture, use <see cref="InnerPictureSvgColor"/> property.
        /// Setting this property will cause the inner picture to be reinitialized and its layout to be updated to reflect the new color scheme.
        /// </summary>
        [Browsable(false)]
        public virtual KnownSvgColor InnerPictureKnownSvgColor
        {
            get => innerPictureKnownSvgColor;
            set
            {
                if (innerPictureKnownSvgColor == value)
                    return;
                innerPictureKnownSvgColor = value;
                OnInnerPictureSvgChanged();
            }
        }

        /// <summary>
        /// Gets or sets the color of the inner picture SVG.
        /// </summary>
        [Browsable(false)]
        public virtual Color? InnerPictureSvgColor
        {
            get => innerPictureSvgColor;
            set
            {
                if (innerPictureSvgColor == value)
                    return;
                innerPictureSvgColor = value;
                OnInnerPictureSvgChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a tooltip is displayed when the inner picture is clicked.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsToolTipShownOnInnerPictureClick
        {
            get => isToolTipShownOnInnerPictureClick;
            set => isToolTipShownOnInnerPictureClick = value;
        }

        /// <summary>
        /// Gets attached <see cref="PictureBox"/> control which
        /// displays validation error information.
        /// </summary>
        [Browsable(false)]
        public virtual PictureBox InnerPicture
        {
            get
            {
                if (pictureBox is null)
                {
                    pictureBox = CreateInnerPicture();
                    pictureBox.VerticalAlignment = UI.VerticalAlignment.Center;
                    pictureBox.ImageVisible = false;
                    pictureBox.ImageStretch = false;
                    pictureBox.TabStop = false;
                    pictureBox.Margin = TextBoxUtils.GetInnerTextBoxPictureMargin(isRight: true);
                    pictureBox.ParentBackColor = true;

                    pictureBox.MouseLeftButtonUp += OnInnerPictureMouseLeftButtonUp;
                    pictureBox.Visible = false;
                    pictureBox.Dock = DockStyle.RightAutoSize;
                    InitInnerPicture();
                    UpdateInnerPictureLayout();
                    pictureBox.Parent = this;
                }

                return pictureBox;
            }
        }

        /// <summary>
        /// Gets or sets visibility of the <see cref="InnerPicture"/> control.
        /// </summary>
        /// <remarks>
        /// There is also <see cref="InnerPictureImageVisible"/> property which specifies
        /// whether to show image on the <see cref="InnerPicture"/>.
        /// </remarks>
        public virtual bool InnerPictureVisible
        {
            get => InnerPicture.Visible;
            set => InnerPicture.Visible = value;
        }

        /// <summary>
        /// Gets or sets tooltip of the <see cref="InnerPicture"/> control.
        /// </summary>
        public virtual string? InnerPictureToolTip
        {
            get
            {
                return pictureBox is null ? null : InnerPicture.ToolTip;
            }

            set
            {
                if (InnerPictureToolTip == value)
                    return;
                InnerPicture.ToolTip = value;
            }
        }

        /// <summary>
        /// Gets or sets visibility of the image on the inner picture control.
        /// </summary>
        /// <remarks>
        /// There is also <see cref="InnerPictureVisible"/> property which specifies
        /// whether to show <see cref="InnerPicture"/> control itself.
        /// </remarks>
        public virtual bool InnerPictureImageVisible
        {
            get
            {
                return (pictureBox is not null) && InnerPicture.ImageVisible;
            }

            set
            {
                if (pictureBox is null && !value)
                    return;
                InnerPicture.ImageVisible = value;
            }
        }

        /// <summary>
        /// Gets whether inner picture is already created.
        /// </summary>
        [Browsable(false)]
        public bool IsInnerPictureCreated => pictureBox is not null;

        /// <summary>
        /// Gets or sets the size, in pixels, of the inner SVG picture.
        /// If not specified, the default size of the SVG image for the control will be used.
        /// Setting this property will cause the inner picture to be reinitialized and its layout to be updated to reflect the new size.
        /// </summary>
        public virtual int? InnerPictureSvgSize
        {
            get => innerPictureSvgSize;
            set
            {
                if (innerPictureSvgSize == value)
                    return;
                innerPictureSvgSize = value;
                OnInnerPictureSvgChanged();
            }
        }

        /// <summary>
        /// Gets or sets the SVG image used as the inner picture.
        /// </summary>
        /// <remarks>If not explicitly set, a default SVG image is provided. Setting this property updates
        /// the displayed inner picture if a picture box is available.</remarks>
        public virtual SvgImage? InnerPictureSvg
        {
            get
            {
                return innerPictureSvg ??= GetDefaultInnerPictureSvg();
            }

            set
            {
                if (innerPictureSvg == value)
                    return;
                innerPictureSvg = value;
                OnInnerPictureSvgChanged();
            }
        }

        /// <summary>
        /// Resets the inner picture to its initial state.
        /// </summary>
        /// <remarks>Call this method to reinitialize the inner picture, discarding any modifications or
        /// changes made since initialization. This method is typically used to restore the default appearance or state
        /// of the inner picture.</remarks>
        public virtual void ResetInnerPicture()
        {
            InitInnerPicture();
            UpdateInnerPictureLayout();
        }

        /// <summary>
        /// Initializes the inner picture for the current instance. Intended to be overridden in derived classes to
        /// provide custom initialization logic.
        /// </summary>
        /// <remarks>The default implementation initializes the inner picture as an error state. Override
        /// this method in a subclass to customize how the inner picture is set up.</remarks>
        public virtual void InitInnerPicture()
        {
            InnerPicture.Image = GetDefaultInnerPictureImage();
        }

        /// <summary>
        /// Called when image layout of the inner picture need to be updated.
        /// </summary>
        protected virtual void UpdateInnerPictureLayout()
        {
        }

        /// <summary>
        /// Creates a new instance of the inner picture box used for displaying images or error states.
        /// The default implementation initializes the picture box with an error image and tooltip.
        /// </summary>
        /// <remarks>Override this method to provide a custom picture box implementation for specialized
        /// rendering or behavior.</remarks>
        /// <returns>A <see cref="PictureBox"/> instance that represents the inner picture box.
        /// </returns>
        protected virtual PictureBox CreateInnerPicture()
        {
            var result = new PictureBox();
            return result;
        }

        /// <summary>
        /// Handles changes to the inner SVG image and updates the display accordingly.
        /// </summary>
        /// <remarks>Call this method when the underlying SVG content changes to ensure the visual
        /// representation remains in sync. This method reinitializes and relayouts the inner picture as
        /// needed.</remarks>
        protected virtual void OnInnerPictureSvgChanged()
        {
            if (pictureBox is null)
                return;
            InitInnerPicture();
            UpdateInnerPictureLayout();
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event for an inner picture control, displaying a tooltip for the associated
        /// PictureBox.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes to customize tooltip
        /// behavior when the left mouse button is released over a PictureBox. If the sender is not a PictureBox, the
        /// method performs no action.</remarks>
        /// <param name="sender">The source of the event, expected to be a PictureBox control.</param>
        /// <param name="e">The MouseEventArgs instance containing the event data.</param>
        protected virtual void OnInnerPictureMouseLeftButtonUp(object? sender, MouseEventArgs e)
        {
            if (!IsToolTipShownOnInnerPictureClick)
                return;

            if (sender is not PictureBox pictureBox)
                return;

            pictureBox.HideToolTip();

            ToolTipFactory.ShowToolTip(
                pictureBox,
                title: null,
                message: pictureBox.ToolTip,
                icon: InnerPictureToolTipMessageIcon);
        }

        /// <summary>
        /// Retrieves the default image which is used when the inner picture is constructed.
        /// </summary>
        /// <remarks>Override this method to provide a custom default image for inner pictures in derived
        /// classes.</remarks>
        /// <returns>An <see cref="Image"/> instance representing the default image, or <see langword="null"/> if default image
        /// is not needed.</returns>
        protected virtual Image? GetDefaultInnerPictureImage()
        {
            var svg = InnerPictureSvg ?? GetDefaultInnerPictureSvg();

            if (InnerPictureSvgColor is null)
            {
                return svg?.ToImageWithDefaultSize(InnerPictureKnownSvgColor, IsDarkBackground, InnerPictureSvgSize, this);
            }

            return svg?.ToImageWithDefaultSize(InnerPictureSvgSize, this, InnerPictureSvgColor);
        }

        /// <summary>
        /// Retrieves the default SVG image to use when an inner picture is constructed.
        /// </summary>
        /// <remarks>Override this method to provide a custom default SVG image for inner pictures in
        /// derived classes.</remarks>
        /// <returns>An <see cref="SvgImage"/> representing the default svg image, or <see langword="null"/> if no default is
        /// set.</returns>
        protected virtual SvgImage? GetDefaultInnerPictureSvg()
        {
            return KnownColorSvgImages.ImgError;
        }
    }
}
