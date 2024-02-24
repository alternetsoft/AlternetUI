using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a frame around a group of controls
    /// with an optional title.
    /// </summary>
    /// <remarks>
    /// The <see cref="GroupBox"/> displays a frame around a group of controls
    /// with or without a title.
    /// Use a <see cref="GroupBox"/> to logically group a collection of controls
    /// in a window.
    /// The group box is a container control that can be used to define groups
    /// of controls.
    /// The typical use for a group box is to contain a logical group of
    /// <see cref="RadioButton"/> controls.
    /// If you have two group boxes, each of which contain several option buttons
    /// (also known as radio buttons),
    /// each group of buttons is mutually exclusive, setting one option value per group.
    /// You can add controls to the <see cref="GroupBox"/> by using the Add
    /// method of the <see cref="Control.Children"/> property.
    /// </remarks>
    [DefaultEvent("Enter")]
    [DefaultProperty("Text")]
    [ControlCategory("Containers")]
    public partial class GroupBox : Control
    {
        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.GroupBox;

        internal new Native.GroupBox NativeControl => (Native.GroupBox)base.NativeControl;

        /// <summary>
        /// Gets the top border ( it is the margin at the top where the title is).
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This is used to account for the
        /// need for extra space taken by the <see cref="GroupBox"/>.
        /// </remarks>
        public int GetTopBorderForSizer()
        {
            return NativeControl.GetTopBorderForSizer();
        }

        /// <summary>
        /// Gets the margin on all other sides except top side.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This is used to account for the
        /// need for extra space taken by the <see cref="GroupBox"/>.
        /// </remarks>
        public int GetOtherBorderForSizer()
        {
            return NativeControl.GetOtherBorderForSizer();
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            // Ensure the group box label is included in the size.
            var nativeControlSize = Handler.GetNativeControlSize(availableSize);
            var calculatedSize = base.GetPreferredSize(availableSize);

            return new SizeD(
                Math.Max(nativeControlSize.Width, calculatedSize.Width),
                Math.Max(nativeControlSize.Height, calculatedSize.Height));
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateGroupBoxHandler(this);
        }
    }
}