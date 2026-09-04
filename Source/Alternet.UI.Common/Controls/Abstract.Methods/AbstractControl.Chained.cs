using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Sets the margin for the control, defining the space around it.
        /// This is the same as setting the <see cref="Margin"/> property but implemented as method.
        /// This method is useful for chaining calls when you want to set the margin
        /// and perform other operations on the control in a fluent manner.
        /// </summary>
        /// <param name="margin">The margin to apply, represented as a Thickness
        /// structure that specifies the left, top, right, and bottom margins.</param>
        /// <returns>Returns the current instance of the AbstractControl, allowing for method chaining.</returns>
        public AbstractControl SetMargin(Thickness margin)
        {
            Margin = margin;
            return this;
        }

        /// <summary>
        /// Sets new suggested size value using nullable width and height values.
        /// </summary>
        public virtual AbstractControl SetSuggestedSize(Coord? newSuggestedWidth, Coord? newSuggestedHeight)
        {
            var suggestedWidth = newSuggestedWidth is null
            ? Coord.NaN : newSuggestedWidth.Value;

            var suggestedHeight = newSuggestedHeight is null
            ? Coord.NaN : newSuggestedHeight.Value;

            SuggestedSize = (suggestedWidth, suggestedHeight);
            return this;
        }

        /// <summary>
        /// Sets the parent control of this control. This is the same as setting the <see cref="Parent"/>
        /// property but implemented as method.
        /// This method is useful for chaining calls when you want to set the parent control
        /// and perform other operations on the control in a fluent manner.
        /// </summary>
        /// <param name="value">The new parent control.</param>
        /// <returns>The current control instance.</returns>
        public AbstractControl SetParent(AbstractControl? value)
        {
            Parent = value;
            return this;
        }

        /// <summary>
        /// Sets the margin of the control using the specified values for each side.
        /// This is the same as setting the <see cref="Margin"/> property but implemented as method.
        /// This method is useful for chaining calls when you want to set the margin and perform
        /// other operations on the control in a fluent manner.
        /// </summary>
        /// <remarks>Use this method to customize the spacing around the control by specifying individual
        /// margin values for each side. This can affect the layout and positioning of the control within its parent
        /// container.</remarks>
        /// <param name="left">The margin value to apply to the left side of the control.</param>
        /// <param name="top">The margin value to apply to the top side of the control.</param>
        /// <param name="right">The margin value to apply to the right side of the control.</param>
        /// <param name="bottom">The margin value to apply to the bottom side of the control.</param>
        /// <returns>The current instance of the control with the updated margin applied.</returns>
        public AbstractControl SetMargin(Coord left, Coord top, Coord right, Coord bottom)
        {
            Margin = new Thickness(left, top, right, bottom);
            return this;
        }

        /// <summary>
        /// Sets left and right margin values of the <see cref="MinChildMargin"/> property for the control.
        /// </summary>
        /// <param name="left">The left margin to set for the control. If not specified, the default is 0.</param>
        /// <param name="right">The right margin to set for the control. If not specified, the default is 0.</param>
        public void SetMinChildMarginLeftRight(float left = 0, float right = 0)
        {
            MinChildMargin = MinChildMargin?.WithLeftRight(left, right);
        }

        /// <summary>
        /// Sets <see cref="Margin"/> property for the control.
        /// </summary>
        /// <param name="margin">The margin to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithMargin(Thickness margin)
        {
            Margin = margin;
            return this;
        }

        /// <summary>
        /// Sets <see cref="Margin"/> property for the control.
        /// </summary>
        /// <param name="bottom">The bottom margin to set for the control.</param>
        /// <param name="left">The left margin to set for the control.</param>
        /// <param name="top">The top margin to set for the control.</param>
        /// <param name="right">The right margin to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithMargin(float left, float top, float right, float bottom)
        {
            Margin = new Thickness(left, top, right, bottom);
            return this;
        }

        /// <summary>
        /// Sets <see cref="MarginLeft"/> property for the control.
        /// </summary>
        /// <param name="left">The left margin to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithMarginLeft(float left)
        {
            Margin = new Thickness(left, Margin.Top, Margin.Right, Margin.Bottom);
            return this;
        }

        /// <summary>
        /// Sets <see cref="MarginBottom"/> property for the control.
        /// </summary>
        /// <param name="bottom">The bottom margin to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithMarginBottom(float bottom)
        {
            Margin = new Thickness(Margin.Left, Margin.Top, Margin.Right, bottom);
            return this;
        }

        /// <summary>
        /// Sets <see cref="MarginRight"/> property for the control.
        /// </summary>
        /// <param name="right">The right margin to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithMarginRight(float right)
        {
            Margin = new Thickness(Margin.Left, Margin.Top, right, Margin.Bottom);
            return this;
        }

        /// <summary>
        /// Sets <see cref="MarginTop"/> property for the control.
        /// </summary>
        /// <param name="top">The top margin to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithMarginTop(float top)
        {
            Margin = new Thickness(Margin.Left, top, Margin.Right, Margin.Bottom);
            return this;
        }

        /// <summary>
        /// Sets <see cref="Padding"/> property for the control.
        /// </summary>
        /// <param name="padding">The padding to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithPadding(Thickness padding)
        {
            Padding = padding;
            return this;
        }

        /// <summary>
        /// Sets <see cref="HorizontalAlignment"/> property for the control.
        /// </summary>
        /// <param name="horizontalAlignment">The horizontal alignment to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithAlignment(HorizontalAlignment horizontalAlignment)
        {
            HorizontalAlignment = horizontalAlignment;
            return this;
        }

        /// <summary>
        /// Sets <see cref="VerticalAlignment"/> property for the control.
        /// </summary>
        /// <param name="verticalAlignment">The vertical alignment to set for the control.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithAlignment(VerticalAlignment verticalAlignment)
        {
            VerticalAlignment = verticalAlignment;
            return this;
        }

        /// <summary>
        /// Sets <see cref="Font"/> property for the control. Additionally, sets <see cref="ParentFont"/> to false.
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public AbstractControl WithFont(Font? font = null)
        {
            ParentFont = false;
            Font = font ?? Control.DefaultFont;
            return this;
        }

        /// <summary>
        /// Sets <see cref="Parent"/> property for the control.
        /// </summary>
        /// <param name="parent">The parent control to set.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public AbstractControl WithParent(AbstractControl? parent)
        {
            Parent = parent;
            return this;
        }

        /// <summary>
        /// Adds the specified child controls to the container.
        /// </summary>
        /// <param name="children">The child controls to add.</param>
        /// <returns>The current instance of <see cref="AbstractControl"/>.</returns>
        public virtual AbstractControl WithChildren(params AbstractControl[] children)
        {
            DoInsideLayout(() =>
            {
                Children.AddRange(children);
            });

            return this;
        }
    }
}
