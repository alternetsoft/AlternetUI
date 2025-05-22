namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the alignment of text within a layout container.
    /// </summary>
    public enum StringAlignment
    {
        /// <summary>
        /// Specifies the text be aligned near the starting edge. In a left-to-right layout,
        /// the near position is left. In a right-to-left layout, the near position is right.
        /// In vertical layouts, the near position is top.
        /// </summary>
        Near = 0,

        /// <summary>
        /// Centers text within the available space.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Specifies that text is aligned near the ending edge (far from the origin position of the
        /// layout rectangle). In a left-to-right layout, the far position is right.
        /// In a right-to-left layout, the far position is left.
        /// In vertical layouts, the near position is bottom.
        /// </summary>
        Far = 2,
    }
}