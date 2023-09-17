#nullable disable


namespace Alternet.UI
{
    /// <summary>
    ///     XamlSerializer is used to persist an 
    ///     object instance to xaml markup.
    /// </summary>
    internal class XamlSerializer
    {
        #region Construction

        /// <summary>
        ///     Constructor for XamlSerializer
        /// </summary>
        /// <remarks>
        ///     This constructor will be used under 
        ///     the following three scenarios
        ///     1. Convert .. XamlToBaml
        ///     2. Convert .. XamlToObject
        ///     3. Convert .. BamlToObject
        /// </remarks>
        public XamlSerializer()
        {
        }

        #endregion Construction

        #region OtherConversions

        #endregion OtherConversions
        #region Data
        internal const string DefNamespacePrefix = "x"; // Used to emit Definitions namespace prefix
        internal const string DefNamespace = "http://schemas.microsoft.com/winfx/2006/xaml"; // Used to emit Definitions namespace
        internal const string ArrayTag = "Array"; // Used to emit the x:Array tag
        internal const string ArrayTagTypeAttribute = "Type"; // Used to emit the x:Type attribute for Array
        #endregion Data

    }
}