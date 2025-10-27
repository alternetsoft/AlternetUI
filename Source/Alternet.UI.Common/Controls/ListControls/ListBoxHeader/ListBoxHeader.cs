﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a header for a list box, providing functionality to manage
    /// columns and their visual appearance.
    /// </summary>
    /// <remarks>The <see cref="ListBoxHeader"/> class allows for the addition, deletion,
    /// and management of columns within a list box header.
    /// It provides properties to customize the appearance of column splitters and
    /// methods to manipulate columns.</remarks>
    public partial class ListBoxHeader : Panel, IListBoxHeader
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
        /// Gets the number of columns represented by the child elements
        /// that are of type <see cref="SpeedButton"/>.
        /// </summary>
        public virtual int ColumnCount
        {
            get
            {
                int count = 0;
                foreach (var child in Children)
                {
                    if (IsColumnControl(child))
                        count++;
                }

                return count;
            }
        }

        /// <summary>
        /// Gets the index of the last child element that is a column control.
        /// </summary>
        public virtual int? LastColumnControlIndex
        {
            get
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    if (IsColumnControl(Children[i]))
                        return i;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the last column control in the collection.
        /// </summary>
        /// <remarks>This property iterates through the child elements in reverse order to locate the last
        /// column control. If no column control is found, the property returns <see langword="null"/>.</remarks>
        public virtual SpeedButton? LastColumn
        {
            get
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    if (IsColumnControl(Children[i]))
                        return Children[i] as SpeedButton;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the first child control in the collection that is identified as a column control.
        /// </summary>
        public virtual SpeedButton? FirstColumn
        {
            get
            {
                foreach (var child in Children)
                {
                    if (IsColumnControl(child))
                        return child as SpeedButton;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets an enumerable collection of <see cref="SpeedButton"/> controls
        /// that are identified as column controls.
        /// </summary>
        /// <remarks>This property iterates over the child elements and returns
        /// those that are recognized
        /// as column controls. The determination of whether a child is a column control
        /// is made by the <c>IsColumnControl</c> method.</remarks>
        public virtual IEnumerable<SpeedButton> ColumnControls
        {
            get
            {
                foreach (var child in Children)
                {
                    if (IsColumnControl(child))
                        yield return (SpeedButton)child;
                }
            }
        }

        /// <summary>
        /// Retrieves the <see cref="SpeedButton"/> control at the specified index
        /// within the column controls.
        /// </summary>
        /// <param name="index">The zero-based index of the column control to retrieve.</param>
        /// <returns>The <see cref="SpeedButton"/> at the specified index,
        /// or <see langword="null"/> if the index is out of range.</returns>
        /// <remarks>
        /// This method uses <see cref="ColumnControls"/> in order to get the column control.
        /// </remarks>
        public virtual SpeedButton? GetColumnControlAt(int index)
        {
            var columnControls = ColumnControls.ToArray();

            if (index < 0 || index >= columnControls.Length)
                return null;
            return columnControls[index];
        }

        /// <summary>
        /// Resolves the splitter colors based on the current settings.
        /// </summary>
        /// <param name="backColor">The background color for splitters.</param>
        /// <param name="foreColor">The foreground color for splitters.</param>
        public virtual void ResolveSplitterColors(out Color? backColor, out Color? foreColor)
        {
            backColor = SplitterBackColor ?? RealBackgroundColor;

            if (SplitterForegroundVisible)
                foreColor = SplitterForeColor ?? DefaultColors.BorderColor;
            else
                foreColor = null;
        }

        /// <inheritdoc/>
        public virtual void DeleteColumns()
        {
            DoInsideLayout(() =>
            {
                foreach (var column in ColumnControls.Reverse().ToArray())
                {
                    var id = column.UniqueId;
                    DeleteColumn(id);
                }
            });
        }

        /// <inheritdoc/>
        public virtual bool DeleteColumn(ObjectUniqueId? columnId)
        {
            AbstractControl? column = FindChild(columnId);
            if (column == null)
                return false;

            var splitterId = column.CustomAttr.GetAttribute<ObjectUniqueId>("AttachedSplitter");

            var splitter = FindChild(splitterId);

            DoInsideLayout(() =>
            {
                if (splitter is not null)
                {
                    splitter.Parent = null;
                    splitter.Dispose();
                }

                column.Parent = null;
                column.Dispose();
            });

            return true;
        }

        /// <inheritdoc/>
        public virtual SpeedButton? GetColumnControl(ObjectUniqueId? columnId)
        {
            AbstractControl? column = FindChild(columnId);
            if (column == null)
                return null;
            if (column is SpeedButton textButton)
                return textButton;
            return null;
        }

        /// <inheritdoc/>
        public virtual bool SetColumnTitle(ObjectUniqueId columnId, string? title)
        {
            var column = GetColumnControl(columnId);
            if (column == null)
                return false;
            column.Text = title ?? string.Empty;
            return true;
        }

        /// <inheritdoc/>
        public virtual bool SetColumnWidth(ObjectUniqueId columnId, Coord? width)
        {
            var column = GetColumnControl(columnId);
            if (column == null)
                return false;

            if (width is null)
            {
                var preferredSize = column.GetPreferredSize();
                column.Width = preferredSize.Width;
            }
            else
            {
                column.Width = width.Value;
            }

            return true;
        }

        /// <inheritdoc/>
        public virtual ObjectUniqueId AddColumn(
            string? title,
            Coord? width = null,
            Action? onClick = null)
        {
            return InsertColumn(int.MaxValue, title, width, onClick);
        }

        /// <inheritdoc/>
        public virtual SpeedButton AddColumnCore(
            string? title,
            Coord? width = null,
            Action? onClick = null)
        {
            return InsertColumnCore(int.MaxValue, title, width, onClick);
        }

        /// <inheritdoc/>
        public virtual ObjectUniqueId InsertColumn(
            int index,
            string? title,
            Coord? width = null,
            Action? onClick = null)
        {
            return InsertColumnCore(index, title, width, onClick).UniqueId;
        }

        /// <inheritdoc/>
        public virtual SpeedButton InsertColumnCore(
            int index,
            string? title,
            Coord? width = null,
            Action? onClick = null)
        {
            SpeedButton label = new()
            {
                Text = title ?? string.Empty,
                ImageVisible = true,
                TextVisible = true,
                ImageToText = ImageToText.Horizontal,
                Dock = DockStyle.Left,
                ClickAction = onClick,
            };

            if (width is null)
            {
                var preferredSize = label.GetPreferredSize();
                label.Width = preferredSize.Width;
            }
            else
            {
                label.Width = width.Value;
            }

            SetIsColumnControl(label, true);

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

            DoInsideLayout(() =>
            {
                if (index < 0 || index >= ColumnCount)
                {
                    label.Parent = this;
                    splitter.Parent = this;
                }
                else
                {
                    var columnControl = GetColumnControlAt(index);

                    if (columnControl == null)
                    {
                        label.Parent = this;
                        splitter.Parent = this;
                    }
                    else
                    {
                        var indexInChildren = Children.IndexOf(columnControl);
                        Children.Insert(indexInChildren, label);
                        Children.Insert(indexInChildren + 1, splitter);
                    }
                }
            });

            return label;
        }

        /// <summary>
        /// Determines whether the specified control is a column control.
        /// </summary>
        /// <param name="control">The control to evaluate. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the control is a <see cref="SpeedButton"/>
        /// and has the "IsColumn" attribute set to
        /// <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
        protected virtual bool IsColumnControl(AbstractControl? control)
        {
            return control is SpeedButton && control.CustomAttr.GetAttribute<bool>("IsColumn");
        }

        /// <summary>
        /// Sets a custom attribute on the specified control to indicate whether it is a column control.
        /// </summary>
        /// <param name="control">The <see cref="SpeedButton"/> control
        /// on which to set the attribute.</param>
        /// <param name="isColumn">A <see langword="bool"/> value indicating
        /// whether the control is a column control. <see langword="true"/> if
        /// the control is a column; otherwise, <see langword="false"/>.</param>
        protected virtual void SetIsColumnControl(SpeedButton control, bool isColumn)
        {
            control.CustomAttr.SetAttribute("IsColumn", isColumn);
        }
    }
}