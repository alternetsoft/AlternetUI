using System;
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
    public partial class ListBoxHeader : HiddenBorder, IListBoxHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxHeader"/> class.
        /// </summary>
        public ListBoxHeader()
        {
            TabStop = false;
            CanSelect = false;
            ParentBackColor = true;
            ParentForeColor = true;
            Layout = LayoutStyle.Dock;
            LayoutFlags = LayoutFlags.IterateBackward;
        }

        /// <summary>
        /// Occurs when a column is deleted from the header.
        /// </summary>
        public event EventHandler<ColumnEventArgs>? ColumnDeleted;

        /// <summary>
        /// Occurs when a column is inserted into the header.
        /// </summary>
        public event EventHandler<ColumnEventArgs>? ColumnInserted;

        /// <summary>
        /// Occurs when the width of a column changes.
        /// </summary>
        /// <remarks>Subscribe to this event to be notified when a column's width is modified, such as by
        /// user interaction or programmatic adjustment. The event provides information about the affected column
        /// through the <see cref="ColumnEventArgs"/> parameter.</remarks>
        public event EventHandler<ColumnEventArgs>? ColumnSizeChanged;

        /// <summary>
        /// Occurs when the visibility of a column changes.
        /// </summary>
        /// <remarks>Subscribe to this event to be notified when a column is shown or hidden. The event
        /// provides details about the affected column through the <see cref="ColumnEventArgs"/> parameter.</remarks>
        public event EventHandler<ColumnEventArgs>? ColumnVisibleChanged;

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

        /// <summary>
        /// Retrieves the splitter control attached to the specified column, if one exists.
        /// </summary>
        /// <param name="column">The column control for which to retrieve the attached splitter. Can be null.</param>
        /// <returns>The splitter control attached to the specified column, or null if no splitter is attached or if the column
        /// is null.</returns>
        public virtual AbstractControl? GetColumnSplitter(AbstractControl? column)
        {
            if (column == null)
                return null;

            var splitterId = column.CustomAttr.GetAttribute<ObjectUniqueId>("AttachedSplitter");

            var splitter = FindChild(splitterId);
            return splitter;
        }

        /// <inheritdoc/>
        public virtual bool DeleteColumn(ObjectUniqueId? columnId)
        {
            AbstractControl? column = FindChild(columnId);
            if (column == null)
                return false;

            var splitter = GetColumnSplitter(column);

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

            ColumnDeleted?.Invoke(this, new ColumnEventArgs(column));

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
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            if (HasBorder)
            {
                var width = NormalBorder.Width;
                return Internal() + width.Size;
            }
            else
            {
                return Internal();
            }

            SizeD Internal()
            {
                var children = ColumnControls.ToArray();
                var result = AbstractControl.GetPreferredSizeWhenStack(this, context, isVert: false, children, ignoreDocked: false);
                result.Width = context.AvailableSize.Width;
                return result;
            }
        }

        /// <summary>
        /// Gets the width of the splitter between columns.
        /// </summary>
        /// <returns>The width of the splitter between columns.</returns>
        public virtual Coord GetSplitterWidth()
        {
            return Splitter.DefaultWidth;
        }

        /// <summary>
        /// Sets the visibility of the specified column in the control.
        /// </summary>
        /// <param name="column">The column whose visibility is to be changed. Can be null.</param>
        /// <param name="visible">A value indicating whether the column should be visible.
        /// Set to <see langword="true"/> to make the column
        /// visible; otherwise, <see langword="false"/>.</param>
        /// <returns>true if the column's visibility was successfully set; otherwise, false.</returns>
        public virtual bool SetColumnVisible(ListControlColumn? column, bool visible)
        {
            if (column == null)
                return false;
            var headerColumn = column.HeaderColumn(this);

            if (headerColumn == null)
                return false;

            headerColumn.Visible = visible;
            column.IsVisible = visible;

            return true;
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

            label.SizeChanged += (s, e) =>
            {
                OnColumnSizeChanged(label);
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
                Width = GetSplitterWidth(),
            };

            label.CustomAttr.SetAttribute("AttachedSplitter", splitter.UniqueId);

            label.VisibleChanged += (s, e) =>
            {
                OnColumnVisibleChanged(label);
                splitter.Visible = label.Visible;
            };

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

            ColumnInserted?.Invoke(this, new ColumnEventArgs(label));

            return label;
        }

        /// <summary>
        /// Raises the event that notifies subscribers when the size of a column associated with the specified label
        /// control has changed.
        /// </summary>
        /// <remarks>Override this method in a derived class to provide custom handling when a column size
        /// changes. This method invokes the ColumnSizeChanged event with the provided label control.</remarks>
        /// <param name="label">The label control representing the column whose size has changed. Cannot be null.</param>
        protected virtual void OnColumnSizeChanged(AbstractControl label)
        {
            ColumnSizeChanged?.Invoke(this, new ColumnEventArgs(label));
        }

        /// <summary>
        /// Raises the event that notifies subscribers when the visibility of a column associated with the specified label
        /// control has changed.
        /// </summary>
        /// <param name="label"></param>
        protected virtual void OnColumnVisibleChanged(AbstractControl label)
        {
            ColumnVisibleChanged?.Invoke(this, new ColumnEventArgs(label));
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

        /// <summary>
        /// Provides data for events that are associated with a specific column.
        /// </summary>
        /// <remarks>Use this class as the event data when handling events that relate to a particular
        /// column. The associated column is accessible through the <see cref="Column"/> property.</remarks>
        public class ColumnEventArgs : BaseEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ColumnEventArgs"/> class.
            /// </summary>
            /// <param name="column">The <see cref="AbstractControl"/> representing the column.</param>
            public ColumnEventArgs(AbstractControl column)
            {
                Column = column;
            }

            /// <summary>
            /// Gets or sets the column associated with the event.
            /// </summary>
            public AbstractControl Column { get; set; }
        }
    }
}