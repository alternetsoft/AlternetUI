using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class ListBoxHeader : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxHeader"/> class.
        /// </summary>
        public ListBoxHeader()
        {
            Layout = LayoutStyle.Dock;
            LayoutFlags = LayoutFlags.IterateBackward;
        }

        /// <summary>
        /// Gets or sets the background color of the column splitters.
        /// </summary>
        public virtual LightDarkColor? SplitterBackColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the column splitters.
        /// </summary>
        public virtual LightDarkColor? SplitterForeColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the splitter foreground is visible.
        /// </summary>
        public virtual bool SplitterForegroundVisible { get; set; } = true;

        /// <summary>
        /// Resolves the splitter colors based on the current settings.
        /// </summary>
        /// <param name="backColor">The background color for splitters.</param>
        /// <param name="foreColor">The foreground color for splitters.</param>
        public virtual void ResolveSplitterColors(out Color? backColor, out Color? foreColor)
        {
            backColor = SplitterBackColor ?? RealBackgroundColor;

            if(SplitterForegroundVisible)
                foreColor = SplitterForeColor ?? DefaultColors.BorderColor;
            else
                foreColor = null;
        }

        /// <summary>
        /// Deletes the specified column and its associated splitter from the control.
        /// </summary>
        /// <remarks>This method attempts to find and remove the column identified by
        /// <paramref
        /// name="columnId"/>. If the column is found, it also disposes of the associated
        /// splitter, if any, before
        /// disposing of the column itself.</remarks>
        /// <param name="columnId">The unique identifier of the column to be deleted.</param>
        /// <returns><see langword="true"/> if the column and its associated splitter
        /// were successfully deleted; otherwise, <see langword="false"/> 
        /// if the column was not found.</returns>
        public virtual bool DeleteColumn(ObjectUniqueId columnId)
        {
            AbstractControl? column = FindChild(columnId);
            if (column == null)
                return false;

            var splitterId = column.CustomAttr.GetAttribute<ObjectUniqueId>("AttachedSplitter");
            
            var splitter = FindChild(splitterId);

            if(splitter is not null)
            {
                splitter.Parent = null;
                splitter.Dispose();
            }

            column.Parent = null;
            column.Dispose();
            return true;
        }

        /// <summary>
        /// Retrieves a <see cref="SpeedButton"/> associated with the specified column identifier.
        /// </summary>
        /// <remarks>This method searches for a child control matching the specified
        /// <paramref name="columnId"/> and returns it if it is
        /// of type <see cref="SpeedButton"/>. If no matching control is
        /// found, or if the control is not a <see cref="SpeedButton"/>,
        /// the method returns <see langword="null"/>.</remarks>
        /// <param name="columnId">The unique identifier of the column for which to retrieve
        /// the control.</param>
        /// <returns>A <see cref="SpeedButton"/> if the column is associated with
        /// a <see cref="SpeedButton"/>; otherwise,
        /// <see langword="null"/>.</returns>
        public virtual SpeedButton? GetColumnControl(ObjectUniqueId columnId)
        {
            AbstractControl? column = FindChild(columnId);
            if (column == null)
                return null;
            if (column is SpeedButton textButton)
                return textButton;
            return null;
        }

        public virtual ObjectUniqueId AddColumn(string title, Coord width)
        {
            SpeedButton label = new()
            {
                Text = title,
                ImageVisible = true,
                TextVisible = true,
                ImageToText = ImageToText.Horizontal,
                Width = width,
                Dock = DockStyle.Left,
            };
            
            label.SetContentHorizontalAlignment(HorizontalAlignment.Left);

            Splitter splitter = new()
            {
                Dock = DockStyle.Left,
                ParentBackColor = true,
                ParentForeColor = false,
                ForeColor = DefaultColors.BorderColor,
                ResolveSplitterColorsOverride = ResolveSplitterColors,
            };

            label.CustomAttr.SetAttribute("AttachedSplitter", splitter.UniqueId);

            label.Parent = this;
            splitter.Parent = this;

            return label.UniqueId;
        }
    }
}
