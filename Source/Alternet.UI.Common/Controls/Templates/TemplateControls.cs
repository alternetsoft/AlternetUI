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
        public class RichToolTip<TLabel> : TemplateControl
            where TLabel : AbstractControl, new()
        {
            private readonly Border border = new()
            {
                Layout = LayoutStyle.Horizontal,
                HasBorder = false,
            };

            private readonly Grid grid = new()
            {
                RowColumnCount = (2, 2),
            };

            private readonly TLabel titleLabel = new();
            private readonly TLabel messageLabel = new();
            private readonly PictureBox pictureBox = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="RichToolTip{T}"/> class.
            /// </summary>
            public RichToolTip()
            {
                DoInsideLayout(() =>
                {
                    border.Parent = this;
                    grid.Parent = border;

                    pictureBox.Parent = grid;

                    titleLabel.RowColumn = (0, 1);
                    titleLabel.Parent = grid;

                    messageLabel.RowColumn = (1, 1);
                    messageLabel.Parent = grid;

                    SetChildrenUseParentBackColor(true, true);
                    SetChildrenUseParentForeColor(true, true);
                    SetChildrenUseParentFont(true, true);
                });
            }

            /// <summary>
            /// Gets control which contains title.
            /// </summary>
            public AbstractControl TitleLabel => titleLabel;

            /// <summary>
            /// Gets control which contains message.
            /// </summary>
            public AbstractControl MessageLabel => messageLabel;

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
            private readonly Border border = new()
            {
                Layout = LayoutStyle.Horizontal,
                HasBorder = false,
            };

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
                HasBorder = hasBorder;

                prefixLabel.Text = prefix;
                boldLabel.Text = boldText;
                suffixLabel.Text = suffix;

                DoInsideLayout(() =>
                {
                    border.Parent = this;
                    prefixLabel.Parent = border;
                    boldLabel.Parent = border;
                    suffixLabel.Parent = border;

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
