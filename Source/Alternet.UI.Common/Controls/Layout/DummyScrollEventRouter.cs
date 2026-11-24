using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a no-op implementation of the <see cref="IScrollEventRouter"/> interface.
    /// </summary>
    /// <remarks>This class is intended as a placeholder or default implementation
    /// of <see cref="IScrollEventRouter"/>.
    /// It performs no actual scrolling actions and returns default values for all
    /// operations. Use the <see cref="Default"/> property to access a shared instance
    /// of this class.</remarks>
    public class DummyScrollEventRouter : IScrollEventRouter
    {
        private static DummyScrollEventRouter? defaultInstance;

        /// <summary>
        /// Gets the default instance of the <see cref="DummyScrollEventRouter"/> class.
        /// </summary>
        public static IScrollEventRouter Default => defaultInstance ??= new DummyScrollEventRouter();

        /// <inheritdoc/>
        public virtual void CalcScrollBarInfo(
            out ScrollBarInfo horzScrollbar,
            out ScrollBarInfo vertScrollbar)
        {
            horzScrollbar = ScrollBarInfo.Default;
            vertScrollbar = ScrollBarInfo.Default;
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollCharLeft()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollCharRight()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollPageLeft()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollPageRight()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollPageUp()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollPageDown()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollLineUp()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollLineDown()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToHorzPos(int charIndex)
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToVertPos(int lineIndex)
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToFirstChar()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToFirstLine()
        {
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToLastLine()
        {
        }
    }
}
