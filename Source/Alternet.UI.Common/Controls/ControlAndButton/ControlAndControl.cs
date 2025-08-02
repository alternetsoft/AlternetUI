using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Parent class for <see cref="ControlAndButton"/> and
    /// <see cref="ControlAndLabel{TControl,TLabel}"/> controls.
    /// </summary>
    [ControlCategory("Editors")]
    public abstract partial class ControlAndControl : HiddenBorder
    {
        private ErrorPictureBox? errorPicture;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndControl"/> class.
        /// </summary>
        public ControlAndControl()
        {
        }

        /// <summary>
        /// Gets attached <see cref="PictureBox"/> control which
        /// displays validation error information.
        /// </summary>
        [Browsable(false)]
        public virtual ErrorPictureBox ErrorPicture
        {
            get
            {
                if (errorPicture is null)
                {
                    errorPicture = new();
                    errorPicture.Visible = false;
                    errorPicture.Dock = DockStyle.RightAutoSize;
                    UpdateErrorPictureLayout();
                    errorPicture.Parent = this;
                }

                return errorPicture;
            }
        }

        /// <summary>
        /// Gets or sets visibility of the <see cref="ErrorPicture"/> control which
        /// displays validation error information.
        /// </summary>
        /// <remarks>
        /// There is also <see cref="ErrorPictureImageVisible"/> property which specifies
        /// whether to show image on the <see cref="ErrorPicture"/>.
        /// </remarks>
        public virtual bool ErrorPictureVisible
        {
            get => ErrorPicture.Visible;
            set => ErrorPicture.Visible = value;
        }

        /// <summary>
        /// Gets or sets tooltip of the <see cref="ErrorPicture"/> control which
        /// displays validation error information.
        /// </summary>
        public virtual string? ErrorPictureToolTip
        {
            get
            {
                return errorPicture is null ? null : ErrorPicture.ToolTip;
            }

            set
            {
                if (ErrorPictureToolTip == value)
                    return;
                ErrorPicture.ToolTip = value;
            }
        }

        /// <summary>
        /// Gets or sets visibility of the image on the <see cref="ErrorPicture"/> control which
        /// displays validation error information.
        /// </summary>
        /// <remarks>
        /// There is also <see cref="ErrorPictureVisible"/> property which specifies
        /// whether to show <see cref="ErrorPicture"/> control itself.
        /// </remarks>
        public virtual bool ErrorPictureImageVisible
        {
            get
            {
                return (errorPicture is not null) && ErrorPicture.ImageVisible;
            }

            set
            {
                if (errorPicture is null && !value)
                    return;
                ErrorPicture.ImageVisible = value;
            }
        }

        /// <summary>
        /// Gets whether <see cref="ErrorPicture"/> is already created.
        /// </summary>
        protected bool IsErrorPictureCreated => errorPicture is not null;

        /// <summary>
        /// Called when image layout of the error picture need to be updated.
        /// </summary>
        protected virtual void UpdateErrorPictureLayout()
        {
        }
    }
}
