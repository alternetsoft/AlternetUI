using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list control item with notification capabilities.
    /// </summary>
    public partial class ListControlItemWithNotify : ListControlItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItemWithNotify"/> class
        /// with the default value for the <see cref="Text"/> property.
        /// </summary>
        /// <param name="text">The initial value of the <see cref="Text"/> property.</param>
        public ListControlItemWithNotify(string text)
            : base(text)
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItemWithNotify"/> class
        /// with the default values for the <see cref="Text"/> and <see cref="Value"/> properties.
        /// </summary>
        /// <param name="text">The default value of the <see cref="Text"/> property.</param>
        /// <param name="value">User data.</param>
        public ListControlItemWithNotify(string text, object? value)
            : base(text, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItemWithNotify"/> class.
        /// </summary>
        public ListControlItemWithNotify()
        {
        }

        /// <inheritdoc/>
        public override int? ImageIndex
        {
            get => base.ImageIndex;

            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.ImageIndex == value)
                        return;
                    base.ImageIndex = value;
                    RaisePropertyChanged(nameof(ImageIndex));
                }
            }
        }

        /// <inheritdoc/>
        public override string Text
        {
            get => base.Text;

            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.Text == value)
                        return;
                    base.Text = value;
                    RaisePropertyChanged(nameof(Text));
                }
            }
        }

        /// <inheritdoc/>
        public override FontStyle? FontStyle
        {
            get => base.FontStyle;
            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.FontStyle == value)
                        return;
                    base.FontStyle = value;
                    RaisePropertyChanged(nameof(FontStyle));
                }
            }
        }

        /// <inheritdoc/>
        public override CheckState CheckState
        {
            get => base.CheckState;
            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.CheckState == value)
                        return;
                    base.CheckState = value;
                    RaisePropertyChanged(nameof(CheckState));
                }
            }
        }

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;

            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.BackgroundColor == value)
                        return;
                    base.BackgroundColor = value;
                    RaisePropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        /// <inheritdoc/>
        public override Color? ForegroundColor
        {
            get => base.ForegroundColor;

            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.ForegroundColor == value)
                        return;
                    base.ForegroundColor = value;
                    RaisePropertyChanged(nameof(ForegroundColor));
                }
            }
        }

        /// <inheritdoc/>
        public override object? Value
        {
            get => base.Value;

            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.Value == value)
                        return;
                    base.Value = value;
                    RaisePropertyChanged(nameof(Value));
                }
            }
        }

        /// <inheritdoc/>
        public override bool IsVisible
        {
            get => base.IsVisible;

            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.IsVisible == value)
                        return;

                    base.IsVisible = value;
                    RaisePropertyChanged(nameof(IsVisible));
                }
            }
        }

        /// <inheritdoc/>
        public override string? DisplayText
        {
            get => base.DisplayText;

            set
            {
                SmartInvoke(Internal);

                void Internal()
                {
                    if (base.DisplayText == value)
                        return;
                    base.DisplayText = value;
                    RaisePropertyChanged(nameof(DisplayText));
                }
            }
        }
    }
}
