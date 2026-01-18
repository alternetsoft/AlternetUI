using System;
using System.Collections.Generic;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Extensions;

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
    /// method of the <see cref="AbstractControl.Children"/> property.
    /// </remarks>
    /// <remarks>
    /// <see cref="GroupBox"/> looks different on the different operating systems. We suggest
    /// to use <see cref="TabControl"/> with single page instead of labeled <see cref="GroupBox"/>
    /// and <see cref="Border"/> instead of unlabeled <see cref="GroupBox"/>. In this case
    /// you application will have the same look on the different operating systems.
    /// </remarks>
    [DefaultEvent("Enter")]
    [DefaultProperty("Text")]
    [ControlCategory("Containers")]
    public partial class GroupBox : Control
    {
        /// <summary>
        /// Gets or sets a default value of the <see cref="AbstractControl.ParentBackColor"/> property.
        /// </summary>
        public static bool? DefaultParentBackColor = true;

        /// <summary>
        /// Gets or sets a default value of the <see cref="AbstractControl.ParentForeColor"/> property.
        /// </summary>
        public static bool? DefaultParentForeColor = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public GroupBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupBox"/> class.
        /// </summary>
        public GroupBox()
        {
            if (DefaultParentBackColor is not null)
                ParentBackColor = DefaultParentBackColor.Value;
            if (DefaultParentForeColor is not null)
                ParentForeColor = DefaultParentForeColor.Value;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.GroupBox;

        /// <inheritdoc/>
        public override object? TitleAsObject
        {
            get => base.TitleAsObject;

            set
            {
                base.TitleAsObject = value;
                Text = value.SafeToString();
            }
        }

        /// <summary>
        /// Gets or sets title. Same as <see cref="AbstractControl.Title"/> property.
        /// </summary>
        [Browsable(false)]
        public new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            // Ensure the group box label is included in the size.
            var nativeControlSize = GetBestSizeWithPadding(context);
            var calculatedSize = base.GetPreferredSize(context);

            return new SizeD(
                Math.Max(nativeControlSize.Width, calculatedSize.Width),
                Math.Max(nativeControlSize.Height, calculatedSize.Height));
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateGroupBoxHandler(this);
        }
    }
}