using System;
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
        public static readonly ControlSet Empty = new(Array.Empty<Control>());

        private readonly IReadOnlyList<Control> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public ControlSet(params Control[] controls)
        {
            items = controls;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public ControlSet(IReadOnlyList<Control> controls)
        {
            items = controls;
        }

        /// <summary>
        /// Gets all controls which will be affected by the group operations.
        /// </summary>
        public IReadOnlyList<Control> Items => items;

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
        /// </summary>
        /// <remarks>
        /// Useful for <see cref="ControlAndLabel"/> and other controls that have inner label.
        /// </remarks>
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
        /// Useful for <see cref="ControlAndLabel"/> and other controls that have inner controls.
        /// This function works with <see cref="ControlAndLabel.MainControl"/>.
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
        public Control this[int index] => items[index];

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public static ControlSet New(params Control[] controls) => new(controls);

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public static ControlSet New(IReadOnlyList<Control> controls) => new(controls);

        /// <summary>
        /// Creates two dimensional array 'Control[,]' from the specified columns with controls.
        /// </summary>
        /// <param name="columns">Columns with the controls</param>
        public static Control[,] GridFromColumns(params ControlSet[] columns)
        {
            var colCount = columns.Length;
            var rowCount = columns[0].Items.Count;

            var result = new Control[rowCount, colCount];

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
        /// Sets <see cref="Control.IsBold"/> for all the controls in the set.
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
        /// Calls <see cref="Control.SuspendLayout()"/> for all parents of the controls in the set.
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
        /// Calls <see cref="Control.SuspendLayout()"/> for all parents of the controls in the set.
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
        /// Sets <see cref="Control.Visible"/> property for all the controls in the set.
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
        /// Sets <see cref="Control.Enabled"/> property for the specified control in the set.
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
        /// Sets <see cref="Control.Enabled"/> property for all the controls in the set.
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
            where T : Control
        {
            if (typeof(T) == typeof(Control))
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
        /// Sets <see cref="Control.Margin"/> property for all the controls in the set.
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
        /// Sets <see cref="Control.Padding"/> property for all the controls in the set.
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
        /// Sets <see cref="Control.Size"/> property for all the controls in the set.
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
        /// Sets <see cref="Control.MinimumSize"/> property for all the controls in the set.
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
        /// Sets <see cref="Control.MinWidth"/> property for all the controls in the set.
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
        /// Sets <see cref="Control.MinHeight"/> property for all the controls in the set.
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
        /// Sets <see cref="Control.Margin"/> property for all the controls in the set.
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
        /// Sets <see cref="ControlAndLabel.LabelSuggestedWidth"/> property for all
        /// the controls in the set.
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
        /// Sets <see cref="ControlAndLabel.InnerSuggestedWidth"/> property for all
        /// the controls in the set.
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
        /// Sets <see cref="Control.Parent"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">Parent control.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        public virtual ControlSet Parent(Control? value)
        {
            foreach (var item in items)
                item.Parent = value;
            return this;
        }
    }
}