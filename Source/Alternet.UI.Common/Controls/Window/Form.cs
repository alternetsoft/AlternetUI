namespace Alternet.UI
{
    /// <summary>
    /// Defined in order to make library more compatible with the legacy code.
    /// </summary>
    public partial class Form : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Form"/> class.
        /// </summary>
        public Form()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Form"/> class.
        /// </summary>
        /// <param name="windowKind">Window kind to use instead of default value.</param>
        /// <remarks>
        /// Fo example, this constructor allows to use window as control
        /// (specify <see cref="WindowKind.Control"/>) as a parameter.
        /// </remarks>
        public Form(WindowKind windowKind)
            : base(windowKind)
        {
        }
    }
}
