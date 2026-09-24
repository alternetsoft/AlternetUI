using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a calendar item that can be displayed in a calendar or scheduler control.
    /// </summary>
    public partial class CalendarItem : ImmutableObject
    {
        private string? title;
        private string? description;
        private object? location;
        private CalendarItemCategory? category;
        private CalendarItemStatus? status;
        private object? userData;
        private string? userName;
        private bool visible = true;
        private int imageIndex = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItem"/> class.
        /// </summary>
        public CalendarItem()
        {
        }

        /// <summary>
        /// Gets or sets the title of the calendar item.
        /// </summary>
        public virtual string? Title
        {
            get => title;
            set
            {
                this.SetProperty(ref title, value, nameof(Title));
            }
        }

        /// <summary>
        /// Gets or sets the description of the calendar item.
        /// </summary>
        public virtual string? Description
        {
            get => description;
            set
            {
                this.SetProperty(ref description, value, nameof(Description));
            }
        }

        /// <summary>
        /// Gets or sets the location property of the calendar item.
        /// </summary>
        public virtual object? Location
        {
            get => location;
            set
            {
                this.SetProperty(ref location, value, nameof(Location));
            }
        }

        /// <summary>
        /// Gets or sets the category property of the calendar item.
        /// </summary>
        public virtual CalendarItemCategory? Category
        {
            get => category;
            set
            {
                this.SetProperty(ref category, value, nameof(Category));
            }
        }

        /// <summary>
        /// Gets or sets the status property of the calendar item.
        /// </summary>
        public virtual CalendarItemStatus? Status
        {
            get => status;
            set
            {
                this.SetProperty(ref status, value, nameof(Status));
            }
        }

        /// <summary>
        /// Gets or sets the user data of the calendar item.
        /// </summary>
        public virtual object? UserData
        {
            get => userData;
            set
            {
                this.SetProperty(ref userData, value, nameof(UserData));
            }
        }

        /// <summary>
        /// Gets or sets the user name of the calendar item.
        /// </summary>
        public virtual string? UserName
        {
            get => userName;
            set
            {
                this.SetProperty(ref userName, value, nameof(UserName));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the calendar item is visible.
        /// </summary>
        public virtual bool Visible
        {
            get => visible;
            set
            {
                this.SetProperty(ref visible, value, nameof(Visible));
            }
        }

        /// <summary>
        /// Gets or sets the image index of the calendar item.
        /// </summary>
        public virtual int ImageIndex
        {
            get => imageIndex;
            set
            {
                this.SetProperty(ref imageIndex, value, nameof(ImageIndex));
            }
        }
    }
}
