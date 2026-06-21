using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class ListControlItem
    {
        /// <summary>
        /// Represents parameters of the measure item operation.
        /// </summary>
        public struct MeasureItemSizeParams
        {
            /// <summary>
            /// Gets or sets a format provider used to format item's text.
            /// </summary>
            public IFormatProvider? FormatProvider;

            /// <summary>
            /// Gets or sets a value indicating whether the column width should be used instead of item width.
            /// </summary>
            public bool UseColumnWidth = true;

            /// <summary>
            /// Gets or sets a value indicating whether the cell sizes are requested in the result.
            /// </summary>
            public bool RequestCellSize = false;

            /// <summary>
            /// Creates a new instance of the <see cref="MeasureItemSizeParams"/> structure.
            /// </summary>
            public MeasureItemSizeParams()
            {
            }
        }

        /// <summary>
        /// Represents result of the measure item operation.
        /// </summary>
        public struct MeasureItemSizeResult
        {
            private List<MeasureItemSizeResult>? cells;

            /// <summary>
            /// Gets individual cell sizes for the item.
            /// </summary>
            public List<MeasureItemSizeResult> Cells
            {
                get => cells ??= new List<MeasureItemSizeResult>();
                set => cells = value;
            }

            /// <summary>
            /// Gets or sets the item for which the size is measured.
            /// </summary>
            public ListControlItem? Item { get; set; }

            /// <summary>
            /// Gets or sets the index of the item for which the size is measured.
            /// </summary>
            public int? ItemIndex { get; set; }

            /// <summary>
            /// Gets or sets the size of the item.
            /// </summary>
            public SizeD Size { get; set; }

            /// <summary>
            /// Gets or sets the width of the item.
            /// </summary>
            public float Width
            {
                readonly get => Size.Width;

                set => Size = new SizeD(value, Size.Height);
            }

            /// <summary>
            /// Gets or sets the height of the item.
            /// </summary>
            public float Height
            {
                readonly get => Size.Height;
                set => Size = new SizeD(Size.Width, value);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MeasureItemSizeResult"/> struct
            /// with the specified size.
            /// </summary>
            /// <param name="size">The size of the item.</param>
            public MeasureItemSizeResult(SizeD size)
            {
                Size = size;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MeasureItemSizeResult"/> struct.
            /// </summary>
            public MeasureItemSizeResult()
            {
            }

            /// <summary>
            /// Operator to implicitly convert the <see cref="MeasureItemSizeResult"/> to a <see cref="SizeD"/>.
            /// </summary>
            /// <param name="result">The <see cref="MeasureItemSizeResult"/> instance to convert.</param>
            public static implicit operator SizeD(MeasureItemSizeResult result) => result.Size;
        }

        /// <summary>
        /// Represents data related to a container for a <see cref="ListControlItem"/>.
        /// </summary>
        public struct ContainerRelatedData
        {
            /// <summary>
            /// Gets or sets a value indicating whether the item is selected.
            /// </summary>
            public bool IsSelected;

            private FlagsAndAttributesStruct attr = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="ContainerRelatedData"/> struct.
            /// </summary>
            public ContainerRelatedData()
            {
            }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.Tag"/>
            /// </summary>
            [Browsable(false)]
            public object? Tag { get; set; }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.IntFlags"/>
            /// </summary>
            [Browsable(false)]
            public ICustomIntFlags IntFlags
            {
                get
                {
                    return IntFlagsAndAttributes;
                }
            }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.IntFlagsAndAttributes"/>
            /// </summary>
            [Browsable(false)]
            public IIntFlagsAndAttributes IntFlagsAndAttributes
            {
                get
                {
                    return attr.IntFlagsAndAttributes;
                }
            }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.FlagsAndAttributes"/>
            /// </summary>
            [Browsable(false)]
            public IFlagsAndAttributes FlagsAndAttributes
            {
                get
                {
                    return attr.FlagsAndAttributes;
                }
            }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.CustomFlags"/>
            /// </summary>
            [Browsable(false)]
            public ICustomFlags<string> CustomFlags => FlagsAndAttributes.Flags;

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.CustomAttr"/>
            /// </summary>
            [Browsable(false)]
            public ICustomAttributes<string, object> CustomAttr => FlagsAndAttributes.Attr;

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.IntAttr"/>
            /// </summary>
            [Browsable(false)]
            public ICustomAttributes<int, object> IntAttr
            {
                get
                {
                    return IntFlagsAndAttributes.Attr;
                }
            }
        }

        /// <summary>
        /// Contains information about the checkbox associated with a <see cref="ListControlItem"/>.
        /// </summary>
        public class ItemCheckBoxInfo : BaseObject
        {
            /// <summary>
            /// Gets or sets the rectangle that defines the bounds of the item.
            /// </summary>
            public RectD Bounds;

            /// <summary>
            /// Gets or sets the visual state of the checkbox.
            /// </summary>
            public VisualControlState PartState;

            /// <summary>
            /// Gets or sets the size of the checkbox.
            /// </summary>
            public SizeD CheckSize;

            /// <summary>
            /// Represents the dimensions of the check image as a <see cref="SizeD"/> structure.
            /// </summary>
            public SizeD CheckImageSize;

            /// <summary>
            /// Gets or sets the rectangle that defines the position and size of the checkbox.
            /// </summary>
            public RectD CheckRect;

            /// <summary>
            /// Gets or sets the rectangle that defines the position and size of the text.
            /// </summary>
            public RectD TextRect;

            /// <summary>
            /// Gets or sets the state of the checkbox.
            /// </summary>
            public CheckState CheckState;

            /// <summary>
            /// Gets or sets the SVG image used when the checkbox is unchecked.
            /// </summary>
            public SvgImage? ImageUnchecked;

            /// <summary>
            /// Gets or sets the SVG image used when the checkbox is checked.
            /// </summary>
            public SvgImage? ImageChecked;

            /// <summary>
            /// Gets or sets the SVG image used when the checkbox is in an indeterminate state.
            /// </summary>
            public SvgImage? ImageIndeterminate;

            /// <summary>
            /// Gets or sets the color of the SVG image.
            /// </summary>
            public Color? SvgImageColor;

            /// <summary>
            /// Gets or sets the color used to draw the checkbox. If not set, the checkbox may use
            /// a default color defined by the container or theme.
            /// This property is not used when checkbox images are provided, as the images will be
            /// drawn in their original colors.
            /// However, if the checkbox is drawn using default rendering (without custom images),
            /// this color will be applied to the checkbox elements.
            /// </summary>
            public Color? Color;

            /// <summary>
            /// Gets or sets the visual state of the SVG image.
            /// </summary>
            public VisualControlState SvgState { get; set; } = VisualControlState.Normal;

            /// <summary>
            /// Gets or sets a value indicating whether the checkbox is visible.
            /// </summary>
            public bool IsCheckBoxVisible;

            /// <summary>
            /// Gets or sets whether to draw the checkbox as a radio button.
            /// </summary>
            public bool IsRadioButton;

            /// <summary>
            /// Gets or sets a value indicating whether to keep text padding
            /// even when the checkbox is not visible.
            /// </summary>
            public bool KeepTextPaddingWithoutCheckBox;
        }
    }
}
