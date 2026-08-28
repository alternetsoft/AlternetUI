using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic transparent panel with inner main and suffix controls.
    /// Suffix control is placed on the right side of the main control.
    /// </summary>
    /// <typeparam name="TControl">The type of the main control.</typeparam>
    /// <typeparam name="TSuffix">The type of the suffix control.</typeparam>
    public partial class ControlAndSuffix<TControl, TSuffix> : TransparentPanel
        where TControl : GenericControl, new()
        where TSuffix : GenericControl, new()
    {
        private readonly TControl control;
        private readonly TSuffix suffix;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndSuffix{TControl, TSuffix}"/> class
        /// with the specified main and suffix controls.
        /// </summary>
        /// <param name="control">The main control.</param>
        /// <param name="suffix">The suffix control.</param>
        public ControlAndSuffix(TControl? control, TSuffix? suffix)
        {
            Layout = LayoutStyle.Horizontal;
            control ??= CreateMainControl();
            suffix ??= CreateSuffixControl();
            this.control = control;
            this.suffix = suffix;

            control.VerticalAlignment = VerticalAlignment.Center;
            suffix.VerticalAlignment = VerticalAlignment.Center;

            control.Parent = this;
            suffix.Parent = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndSuffix{TControl, TSuffix}"/> class.
        /// </summary>
        public ControlAndSuffix()
            : this(null, null)
        {
        }

        /// <summary>
        /// Gets the main inner control of the <see cref="ControlAndSuffix{TControl, TSuffix}"/> instance.
        /// </summary>
        public TControl MainControl => control;

        /// <summary>
        /// Gets the suffix control of the <see cref="ControlAndSuffix{TControl, TSuffix}"/> instance.
        /// </summary>
        public TSuffix SuffixControl => suffix;

        /// <summary>
        /// Creates the main control of the <see cref="ControlAndSuffix{TControl, TSuffix}"/> instance.
        /// </summary>
        /// <returns>The created main control.</returns>
        protected virtual TControl CreateMainControl() => new ();

        /// <summary>
        /// Creates the suffix control of the <see cref="ControlAndSuffix{TControl, TSuffix}"/> instance.
        /// </summary>
        /// <returns>The created suffix control.</returns>
        protected virtual TSuffix CreateSuffixControl() => new ();
    }
}
