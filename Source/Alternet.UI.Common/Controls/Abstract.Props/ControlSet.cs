﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to perform group operations on the controls.
    /// </summary>
    public partial class ControlSet
    {
        /// <summary>
        /// Gets <see cref="ControlSet"/> without items.
        /// </summary>
        public static readonly ControlSet Empty = new(Array.Empty<AbstractControl>());

        private readonly IReadOnlyList<AbstractControl> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public ControlSet(params AbstractControl[] controls)
        {
            items = controls;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public ControlSet(IReadOnlyList<AbstractControl> controls)
        {
            items = controls;
        }

        /// <summary>
        /// Gets whether <see cref="Items"/> collection is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return items.Count == 0;
            }
        }

        /// <summary>
        /// Gets first item in the <see cref="Items"/> collection or Null.
        /// </summary>
        public AbstractControl? First
        {
            get
            {
                if (items.Count == 0)
                    return null;
                return items[0];
            }
        }

        /// <summary>
        /// Gets all controls which will be affected by the group operations.
        /// </summary>
        public IReadOnlyList<AbstractControl> Items => items;

        /// <summary>
        /// Gets the maximum width among all controls in the set.
        /// </summary>
        public virtual Coord MaxWidth
        {
            get
            {
                Coord result = 0;
                foreach (var item in items)
                    result = Math.Max(result, item.Width);
                return result;
            }
        }

        /// <summary>
        /// Gets the maximum inner label width among all controls in the set.
        /// Control must implement <see cref="IControlAndLabel"/> interface in order
        /// to allow this method to work.
        /// </summary>
        public virtual Coord LabelMaxWidth
        {
            get
            {
                Coord result = 0;
                foreach (var item in items)
                {
                    if (item is IControlAndLabel control)
                        result = Math.Max(result, control.Label.Width);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the maximum inner control width among all controls in the set.
        /// </summary>
        /// <remarks>
        /// Control must implement <see cref="IControlAndLabel"/> interface in order
        /// to allow this method to work.
        /// This function works with <see cref="IControlAndLabel.MainControl"/>.
        /// </remarks>
        public virtual Coord InnerMaxWidth
        {
            get
            {
                Coord result = 0;
                foreach (var item in items)
                {
                    if (item is IControlAndLabel control)
                        result = Math.Max(result, control.MainControl.Width);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the maximum height among all controls in the set.
        /// </summary>
        public virtual Coord MaxHeight
        {
            get
            {
                Coord result = 0;
                foreach (var item in items)
                    result = Math.Max(result, item.Height);
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public AbstractControl this[int index] => items[index];

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public static ControlSet New(params AbstractControl[] controls) => new(controls);

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public static ControlSet New(IReadOnlyList<AbstractControl> controls) => new(controls);

        /// <summary>
        /// Creates two dimensional array 'Control[,]' from the specified columns with controls.
        /// </summary>
        /// <param name="columns">Columns with the controls</param>
        public static AbstractControl[,] GridFromColumns(params ControlSet[] columns)
        {
            var colCount = columns.Length;
            var rowCount = columns[0].Items.Count;

            var result = new AbstractControl[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    var column = columns[j];
                    var item = column[i];
                    result[i, j] = item;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets vertical alignmnent for all the controls in the set.
        /// </summary>
        /// <param name="value">A vertical alignment setting.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet VerticalAlignment(VerticalAlignment value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.VerticalAlignment = value;
            });
        }

        /// <summary>
        /// Sets font for all the controls in the set.
        /// </summary>
        /// <param name="value">New font.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Font(Font? value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.Font = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.IsBold"/> for all the controls in the set.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet IsBold(bool value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.IsBold = value;
            });
        }

        /// <summary>
        /// Sets background color for all the controls in the set.
        /// </summary>
        /// <param name="value">New font.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet BackgroundColor(Color? value)
        {
            foreach (var item in items)
                item.BackgroundColor = value;
            return this;
        }

        /// <summary>
        /// Sets background color for all the controls in the set.
        /// </summary>
        /// <param name="value">New font.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet ForegroundColor(Color? value)
        {
            foreach (var item in items)
                item.ForegroundColor = value;
            return this;
        }

        /// <summary>
        /// Calls <see cref="AbstractControl.SuspendLayout()"/> for all parents of the controls in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet SuspendLayout()
        {
            foreach (var item in items)
            {
                item.Parent?.SuspendLayout();
            }

            return this;
        }

        /// <summary>
        /// Calls <see cref="AbstractControl.SuspendLayout()"/> for all parents of the controls in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet ResumeLayout()
        {
            foreach (var item in items)
            {
                item.Parent?.ResumeLayout();
            }

            return this;
        }

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="SuspendLayout"/>
        /// and <see cref="ResumeLayout"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet DoInsideLayout(Action action)
        {
            SuspendLayout();
            try
            {
                action();
                return this;
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.Visible"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">New property value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Visible(bool value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.Visible = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.Enabled"/> property for the specified control in the set.
        /// </summary>
        /// <param name="value">New 'Enabled' property value.</param>
        /// <param name="index">Index of the control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Enabled(int index, bool value)
        {
            items[index].Enabled = value;
            return this;
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.Enabled"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">New 'Enabled' property value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Enabled(bool value)
        {
            foreach (var item in items)
                item.Enabled = value;
            return this;
        }

        /// <summary>
        /// Executes specified action for all the controls in the set.
        /// </summary>
        /// <typeparam name="T">Type of the action parameter.</typeparam>
        /// <param name="action">Action to execute.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Action<T>(Action<T> action)
            where T : AbstractControl
        {
            if (typeof(T) == typeof(AbstractControl))
            {
                foreach (var item in items)
                    action((T)item);
                return this;
            }

            foreach (var item in items)
            {
                if(item is T t)
                    action(t);
            }

            return this;
        }

        /// <summary>
        /// Sets suggested width for all the controls to the max value in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet SuggestedWidthToMax()
        {
            var v = MaxWidth;
            return SuggestedWidth(v);
        }

        /// <summary>
        /// Sets suggested width for all the controls to the max value in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet WidthToMax()
        {
            var v = MaxWidth;
            return Width(v);
        }

        /// <summary>
        /// Sets suggested height for all the controls to the max value in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet SuggestedHeightToMax()
        {
            return SuggestedHeight(MaxHeight);
        }

        /// <summary>
        /// Sets the suggested width for all the controls in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet SuggestedWidth(Coord width)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                {
                    item.SuggestedWidth = width;
                }
            });
        }

        /// <summary>
        /// Sets the width for all the controls in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Width(Coord width)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                {
                    item.Width = width;
                }
            });
        }

        /// <summary>
        /// Sets the suggested height for all the controls in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet SuggestedHeight(Coord height)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.SuggestedHeight = height;
            });
        }

        /// <summary>
        /// Sets horizontal alignmnent for all the controls in the set.
        /// </summary>
        /// <param name="value">A horizontal alignment setting.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet HorizontalAlignment(HorizontalAlignment value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.HorizontalAlignment = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.Margin"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">An oouter margin of a control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Margin(Thickness value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.Margin = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.Padding"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">A padding of a control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Padding(Thickness value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.Padding = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.Size"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">size of a control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Size(SizeD value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.Size = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.MinimumSize"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">Minimum size of a control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet MinSize(SizeD value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.MinimumSize = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.MinWidth"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">Minimum width of a control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet MinWidth(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.MinWidth = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.MinHeight"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">Minimum height of a control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet MinHeight(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.MinHeight = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.Margin"/> property for all the controls in the set.
        /// </summary>
        /// <param name="left">The margin for the left side.</param>
        /// <param name="top">The margin for the top side.</param>
        /// <param name="right">The margin for the right side.</param>
        /// <param name="bottom">The margin for the bottom side.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Margin(Coord left, Coord top, Coord right, Coord bottom)
        {
            return DoInsideLayout(() =>
            {
                Thickness value = new(left, top, right, bottom);
                foreach (var item in items)
                    item.Margin = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.MarginTop"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">The margin value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet MarginTop(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.MarginTop = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.MarginBottom"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">The margin value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet MarginBottom(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.MarginBottom = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.MarginLeft"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">The margin value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet MarginLeft(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.MarginLeft = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.MarginRight"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">The margin value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet MarginRight(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.MarginRight = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.SuggestedWidth"/> property for
        /// the inner labels of all controls in the set.
        /// Control must implement <see cref="IControlAndLabel"/> interface in order
        /// to allow this method to work.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet LabelSuggestedWidth(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in Items)
                {
                    if (item is IControlAndLabel control)
                        control.Label.SuggestedWidth = value;
                }
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.PaddingTop"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">The margin value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet PaddingTop(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.PaddingTop = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.PaddingBottom"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">The margin value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet PaddingBottom(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.PaddingBottom = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.PaddingLeft"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">The margin value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet PaddingLeft(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.PaddingLeft = value;
            });
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.PaddingRight"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">The margin value.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet PaddingRight(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in items)
                    item.PaddingRight = value;
            });
        }

        /// <summary>
        /// Sets suggested width for inner childs of the controls in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet InnerSuggestedWidthToMax()
        {
            var v = InnerMaxWidth;
            return InnerSuggestedWidth(v);
        }

        /// <summary>
        /// Sets suggested width for all the controls in the set.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet LabelSuggestedWidthToMax()
        {
            var v = LabelMaxWidth;
            return LabelSuggestedWidth(v);
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.SuggestedWidth"/> property for the
        /// main child of all the controls in the set.
        /// Control must implement <see cref="IControlAndLabel"/> interface in order
        /// to allow this method to work.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet InnerSuggestedWidth(Coord value)
        {
            return DoInsideLayout(() =>
            {
                foreach (var item in Items)
                {
                    if (item is IControlAndLabel control)
                        control.MainControl.SuggestedWidth = value;
                }
            });
        }

        /// <summary>
        /// Sets <see cref="ComboBox.IsEditable"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value"><c>true</c> enables editing of the text;
        /// <c>false</c> disables it.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet IsEditable(bool value)
        {
            foreach (var item in Items)
            {
                if (item is ComboBox comboBox)
                    comboBox.IsEditable = value;
            }

            return this;
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.ParentForeColor"/> property for all the controls in the set.
        /// </summary>
        public virtual ControlSet ParentForeColor(bool value)
        {
            foreach (var item in Items)
            {
                item.ParentForeColor = value;
            }

            return this;
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.ParentBackColor"/> property for all the controls in the set.
        /// </summary>
        public virtual ControlSet ParentBackColor(bool value)
        {
            foreach (var item in Items)
            {
                item.ParentBackColor = value;
            }

            return this;
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.Parent"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">Parent control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Parent(AbstractControl? value)
        {
            foreach (var item in items)
                item.Parent = value;
            return this;
        }

        /// <summary>
        /// Adds event handler to 'CheckedChanged' event.
        /// </summary>
        /// <param name="evt">Event Handler.</param>
        /// <returns></returns>
        public virtual ControlSet WhenCheckedChanged(EventHandler evt)
        {
            foreach (var item in items)
            {
                if (item is CheckBox checkBox)
                    checkBox.CheckedChanged += evt;
                else
                if (item is RadioButton radioButton)
                    radioButton.CheckedChanged += evt;
            }

            return this;
        }
    }
}