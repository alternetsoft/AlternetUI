namespace Alternet.UI
{
    /// <summary>
    /// Represents an item (also known as a node) of a <see cref="TreeView"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Items"/> collection holds all the child <see cref="TreeViewItem"/> objects assigned to the current <see cref="TreeViewItem"/>.
    /// You can add or remove a <see cref="TreeViewItem"/>; when you do this, all child tree items are added or removed.
    /// Each <see cref="TreeViewItem"/> can contain a collection of other <see cref="TreeViewItem"/> objects.
    /// </para>
    /// <para>
    /// The <see cref="TreeViewItem"/> text label is set by setting the <see cref="Text"/> property explicitly.
    /// The alternative is to create the tree item using one of the <see cref="TreeViewItem"/> constructors
    /// that has a string parameter that represents the <see cref="Text"/> property.
    /// The label is displayed next to the <see cref="TreeViewItem"/> image, if one is displayed.
    /// </para>
    /// <para>
    /// To display images next to the tree items, assign an <see cref="ImageList"/> to the <see cref="TreeView.ImageList"/> property
    /// of the parent <see cref="TreeView"/> control and assign an <see cref="Image"/> by referencing its index value in
    /// the <see cref="TreeView.ImageList"/> property. To do that, set the <see cref="ImageIndex"/> property to the index value
    /// of the <see cref="Image"/> you want to display in the <see cref="TreeViewItem"/>.
    /// </para>
    /// <para>
    /// <see cref="TreeView"/> items can be expanded to display the next level of child items.
    /// The user can expand the <see cref="TreeViewItem"/> by clicking the expand button, if one is displayed
    /// next to the <see cref="TreeViewItem"/>, or you can expand the <see cref="TreeViewItem"/> by calling the <see cref="TreeViewItem.Expand"/> method.
    /// To expand all the child item levels in the <see cref="Items"/> collection, call the <see cref="TreeViewItem.ExpandAll"/> method.
    /// You can collapse the child <see cref="TreeViewItem"/> level by calling the <see cref="TreeViewItem.Collapse"/> method,
    /// or the user can press the expand button, if one is displayed next to the <see cref="TreeViewItem"/>.
    /// You can also call the <see cref="TreeViewItem.Toggle"/> method to alternate between the expanded and collapsed states.
    /// </para>
    /// </remarks>
    public class TreeViewItem
    {
        private string text = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class with default values.
        /// </summary>
        public TreeViewItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class with the specified item text.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        public TreeViewItem(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class with the specified item text and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">
        /// The zero-based index of the image within the <see cref="TreeView.ImageList"/>
        /// associated with the <see cref="TreeView"/> that contains the item.
        /// </param>
        public TreeViewItem(string text, int imageIndex)
        {
            Text = text;
            ImageIndex = imageIndex;
        }

        /// <summary>
        /// Gets or sets an object that contains data to associate with the item.
        /// </summary>
        /// <value>An object that contains information that is associated with the item.</value>
        /// <remarks>
        /// The <see cref="Tag"/> property can be used to store any object that you want to associate with an item.
        /// Although you can store any item, the <see cref="Tag"/> property is typically used to store string information
        /// about the item, such as a unique identifier or the index position of the item's data in a database.
        /// </remarks>
        public object? Tag { get; set; }

        /// <summary>
        /// Gets or sets the text of the item.
        /// </summary>
        /// <value>The text to display for the item.</value>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text == value)
                    return;

                text = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the image that is displayed for the item.
        /// </summary>
        /// <value>The zero-based index of the image in the <see cref="TreeView.ImageList"/> that is displayed for the item. The default is <c>null</c>.</value>
        /// <remarks>
        /// The effect of setting this property depends on the value of the <see cref="TreeView.ImageList"/> property.
        /// You can specify which images from the list are displayed for items by default by setting the <see cref="TreeView.ImageIndex"/> property.
        /// Individual <see cref="TreeViewItem"/> objects can specify which image is displayed by setting the <see cref="ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override the settings in the corresponding <see cref="TreeView"/> properties.
        /// </remarks>
        public int? ImageIndex { get; set; }
    }
}