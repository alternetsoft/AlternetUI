namespace Alternet.UI
{
    /// <summary>
    /// Specifies constants indicating which elements of the Help file to display.
    /// </summary>
    public enum HelpNavigator
    {
        /// <summary>
        /// The Help file opens to a specified topic, if the topic exists.
        /// </summary>
        Topic = -2147483647,

        /// <summary>
        /// The Help file opens to the table of contents.
        /// </summary>
        TableOfContents,

        /// <summary>
        /// The Help file opens to the index.
        /// </summary>
        Index,

        /// <summary>
        /// The Help file opens to the search page.
        /// </summary>
        Find,

        /// <summary>
        /// The Help file opens to the index entry for the first letter of a specified topic.
        /// </summary>
        AssociateIndex,

        /// <summary>
        /// The Help file opens to the topic with the specified index entry, if one exists; otherwise,
        /// the index entry closest to the specified keyword is displayed.
        /// </summary>
        KeywordIndex,

        /// <summary>
        /// The Help file opens to a topic indicated by a numeric topic identifier.
        /// </summary>
        TopicId,
    }
}
