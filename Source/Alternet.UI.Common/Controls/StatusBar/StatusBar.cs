using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a status bar control.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public partial class StatusBar : ToolBar
    {
        /// <summary>
        /// Represents the height, in dips, of the Visual Studio status bar.
        /// </summary>
        public const int VisualStudioStatusBarHeight = 28;
        private bool sizingGripVisible;
        private TextEllipsisType textEllipsis;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        public StatusBar()
        {
            Panels.ItemInserted += OnItemInserted;
            Panels.ItemRemoved += OnItemRemoved;
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
        /// Gets or sets a value indicating whether a sizing grip is displayed in the
        /// lower-right corner of the control.
        /// </summary>
        public virtual bool SizingGripVisible
        {
            get
            {
                return sizingGripVisible;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (SizingGripVisible == value)
                    return;
                sizingGripVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets status texts replacement method when the text widths exceed the
        /// container's widths.
        /// </summary>
        public virtual TextEllipsisType TextEllipsis
        {
            get
            {
                return textEllipsis;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (TextEllipsis == value)
                    return;
                textEllipsis = value;
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
        public virtual StatusBarPanel Add(string text)
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
        public virtual int GetFieldsCount()
        {
            return Panels.Count;
        }

        /// <summary>
        /// Returns panel by index.
        /// </summary>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns><see cref="StatusBarPanel"/> if found; <c>null</c> otherwise.</returns>
        public virtual StatusBarPanel? GetField(int index)
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
        public virtual bool SetStatusText(string? text = null, int index = 0)
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
        /// Gets the status text for the specified panel.
        /// </summary>
        /// <param name="index">Panel index, starting from zero.</param>
        /// <returns>
        /// <see cref="string"/> with the status text if success; <c>null</c> otherwise.
        /// </returns>
        public virtual string? GetStatusText(int index = 0)
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
        public virtual bool PushStatusText(string? text = null, int index = 0)
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
        public virtual bool PopStatusText(int index = 0)
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
        /// <param name="widths">Contains an array of width, each of which is either an
        /// absolute status panel width in dips if positive or indicates a variable width
        /// panel if negative. </param>
        /// <returns><c>true</c> if success; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This method doesn't affect <see cref="Panels"/>, it works with the native control.
        /// </remarks>
        /// <remarks>
        /// There are two types of panels: fixed widths and variable width panels. For the fixed
        /// width panels you should specify their(constant) width in dips. For the variable width
        /// panels, specify a negative number which indicates how the panels should expand:
        /// the space left for all variable width panels is divided between them according
        /// to the absolute value of this number. A variable width panels with width
        /// of (-2) gets twice as much of it as a panels with width (-1) and so on.
        /// </remarks>
        /// <remarks>
        /// For example, to create one fixed width panels of width 100 in the right part
        /// of the status bar and two more panels which get 66% and 33% of the
        /// remaining space correspondingly, you should use an array containing (-2), (-1) and 100.
        /// </remarks>
        /// <remarks>
        /// Size of the <paramref name="widths"/> array must be equal to the number passed to
        /// <see cref="SetFieldsCount"/> the last time it was called.
        /// </remarks>
        /// <remarks>
        /// The widths of the variable panels are calculated from the total width of all panels,
        /// minus the sum of widths of the non-variable panels, divided by the number of
        /// variable panels.
        /// </remarks>
        public virtual bool SetStatusWidths(float[] widths)
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
        public virtual bool SetFieldsCount(int count)
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
        public virtual float? GetStatusWidth(int index)
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
        public virtual StatusBarPanelStyle? GetStatusStyle(int index)
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
        public virtual bool SetStatusStyles(StatusBarPanelStyle[] styles)
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
        public virtual RectD? GetFieldRect(int index)
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
        public virtual bool SetMinHeight(float height)
        {
            this.MinHeight = height;
            return true;
        }

        /// <summary>
        /// Clears <see cref="Panels"/>.
        /// </summary>
        public virtual void Clear()
        {
            Panels.Clear();
        }

        /// <summary>
        /// Called when item was inserted in the <see cref="Panels"/>.
        /// </summary>
        protected virtual void OnItemInserted(object? sender, int index, StatusBarPanel item)
        {
            item.PropertyChanged += OnItemPropertyChanged;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
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
        protected virtual void OnItemPropertyChanged(object? sender, EventArgs e)
        {
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
            item.PropertyChanged -= OnItemPropertyChanged;
        }
    }
}