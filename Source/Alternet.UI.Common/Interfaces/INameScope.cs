namespace Alternet.UI
{
    /// <summary>
    /// Defines the basic name scoping interface for root classes.
    /// </summary>
    internal interface INameScope
    {
        /// <summary>
        /// Registers the name-element combination.
        /// </summary>
        /// <param name="name">Name of the element.</param>
        /// <param name="scopedElement">Element where name is defined.</param>
        void RegisterName(string name, object scopedElement);

        /// <summary>
        /// Unregisters the name-element combination.
        /// </summary>
        /// <param name="name">Name of the element.</param>
        void UnregisterName(string name);

        /// <summary>
        /// Finds the element by given name.
        /// </summary>
        object FindName(string name);
    }
}