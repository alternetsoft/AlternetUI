using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains useful template controls declarations.
    /// </summary>
    public static class TemplateControls
    {
        /// <summary>
        /// Sample template control with text which has a middle part with bold font.
        /// </summary>
        public class RichToolTipTemplate : TemplateControl
        {
            private readonly VerticalStackPanel labels = new();

            private readonly GenericLabel titleLabel = new();
            private readonly GenericLabel messageLabel = new();
            private readonly PictureBox pictureBox = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="RichToolTipTemplate"/> class.
            /// </summary>
            public RichToolTipTemplate()
            {
                DoInsideLayout(() =>
                {
                    Layout = LayoutStyle.Horizontal;

                    VerticalAlignment = VerticalAlignment.Top;
                    HorizontalAlignment = HorizontalAlignment.Left;

                    pictureBox.VerticalAlignment = VerticalAlignment.Top;
                    pictureBox.Margin = RichToolTip.DefaultImageMargin;
                    pictureBox.Parent = this;

                    labels.HorizontalAlignment = HorizontalAlignment.Fill;
                    labels.VerticalAlignment = VerticalAlignment.Fill;
                    labels.Parent = this;

                    titleLabel.WordWrap = true;
                    titleLabel.Margin = RichToolTip.DefaultTitleMargin;
                    titleLabel.Parent = labels;
                    titleLabel.MaxTextWidth = 500;

                    messageLabel.Margin = RichToolTip.DefaultMessageMargin;
                    messageLabel.WordWrap = true;
                    messageLabel.Parent = labels;
                    messageLabel.MaxTextWidth = 500;

                    SetChildrenUseParentBackColor(true, true);
                    SetChildrenUseParentForeColor(true, true);
                    SetChildrenUseParentFont(true, true);
                });
            }

            /// <summary>
            /// Gets control which contains title.
            /// </summary>
            public GenericLabel TitleLabel => titleLabel;

            /// <summary>
            /// Gets control which contains message.
            /// </summary>
            public GenericLabel MessageLabel => messageLabel;

            /// <summary>
            /// Gets control which contains image.
            /// </summary>
            public PictureBox PictureBox => pictureBox;
        }

        /// <summary>
        /// Sample template control with text which has a middle part with bold font.
        /// </summary>
        public class BoldText<TLabel> : TemplateControl
            where TLabel : AbstractControl, new()
        {
            private readonly TLabel prefixLabel = new();
            private readonly TLabel suffixLabel = new();

            private readonly TLabel boldLabel = new()
            {
                IsBold = true,
            };

            /// <summary>
            /// Initializes a new instance of the <see cref="BoldText{T}"/> class.
            /// </summary>
            /// <param name="prefix">First part of the text.</param>
            /// <param name="boldText">Middle part of the text with bold attribute.</param>
            /// <param name="suffix">Last part of the text.</param>
            /// <param name="hasBorder">Whether to draw default border around the text.</param>
            public BoldText(string prefix, string boldText, string suffix, bool hasBorder)
            {
                Layout = LayoutStyle.Horizontal;
                HasBorder = hasBorder;

                prefixLabel.Text = prefix;
                boldLabel.Text = boldText;
                suffixLabel.Text = suffix;

                DoInsideLayout(() =>
                {
                    prefixLabel.Parent = this;
                    boldLabel.Parent = this;
                    suffixLabel.Parent = this;

                    SetChildrenUseParentBackColor(true, true);
                    SetChildrenUseParentForeColor(true, true);
                    SetChildrenUseParentFont(true, true);
                });
            }

            /// <summary>
            /// Gets control which contains first part of the text.
            /// </summary>
            public AbstractControl PrefixLabel => prefixLabel;

            /// <summary>
            /// Gets control which contains last part of the text.
            /// </summary>
            public AbstractControl SuffixLabel => suffixLabel;

            /// <summary>
            /// Gets control which contains middle part of the text.
            /// </summary>
            public AbstractControl BoldLabel => boldLabel;
        }
    }
}
