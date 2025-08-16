namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known routing strategies.
    /// </summary>
    public enum RoutingStrategy
    {
        /// <summary>
        /// Route the event starting at the root of the visual tree and ending with the source.
        /// </summary>
        Tunnel,

        /// <summary>
        /// Route the event starting at the source and ending with the root of the visual tree.
        /// </summary>
        Bubble,

        /// <summary>
        /// Raise the event at the source only.
        /// </summary>
        Direct,
    }
}