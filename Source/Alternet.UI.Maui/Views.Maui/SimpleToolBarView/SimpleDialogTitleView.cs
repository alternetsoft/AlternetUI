using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a simple dialog title view with a title label and a close button.
    /// </summary>
    public partial class SimpleDialogTitleView : SimpleToolBarView
    {
        /// <summary>
        /// Gets or sets the default margin for the title. Default is 0.
        /// </summary>
        public static Thickness DefaultTitleMargin = Thickness.Zero;

        private readonly IToolBarItem titleLabel;
        private readonly View? owner;

        private IToolBarItem? closeButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDialogTitleView"/> class.
        /// </summary>
        public SimpleDialogTitleView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDialogTitleView"/> class
        /// with an optional owner.
        /// </summary>
        /// <param name="owner">The owner view of the dialog title view.</param>
        public SimpleDialogTitleView(View? owner)
        {
            this.owner = owner;
            IsBottomBorderVisible = true;
            titleLabel = AddLabel(string.Empty);
            AddExpandingSpace();
            AddCloseButton();
            Margin = DefaultTitleMargin;
        }

        /// <summary>
        /// Occurs when the close button is clicked.
        /// </summary>
        public event EventHandler? CloseClicked;

        /// <summary>
        /// Gets the parent view that owns this view.
        /// </summary>
        public virtual View? Owner => owner;

        /// <summary>
        /// Gets the toolbar item that represents the "Close" button.
        /// </summary>
        public IToolBarItem? CloseButton => closeButton;

        /// <summary>
        /// Gets or sets a value indicating whether the close button is visible.
        /// </summary>
        public virtual bool HasCloseButton
        {
            get
            {
                return closeButton?.IsVisible ?? false;
            }

            set
            {
                if (closeButton is null)
                    AddCloseButton();
                if(closeButton is not null)
                    closeButton.IsVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the title of the dialog.
        /// </summary>
        public virtual string Title
        {
            get
            {
                return titleLabel.Text;
            }

            set
            {
                if (Title == value)
                    return;
                titleLabel.IsVisible = !string.IsNullOrEmpty(value);
                titleLabel.Text = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Adds a close button to the dialog title.
        /// </summary>
        /// <remarks>The close button displays a "Close" icon.
        /// When clicked, it triggers the <see cref="CloseClicked"/> event,
        /// passing the owner of the button or the
        /// current instance as the sender.</remarks>
        public virtual void AddCloseButton()
        {
            closeButton = AddButton(
                null,
                Alternet.UI.Localization.CommonStrings.Default.ButtonClose,
                Alternet.UI.KnownSvgImages.ImgCancel,
                () =>
                {
                    CloseClicked?.Invoke(Owner ?? this, EventArgs.Empty);
                });
        }
    }
}
