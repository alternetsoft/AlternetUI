using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Validates text controls, providing a variety of filtering behaviours.
    /// </summary>
    public interface IValueValidatorText : IValueValidator
    {
        /// <summary>
        /// Gets or sets the exclude char list (the list of invalid characters).
        /// </summary>
        string CharExcludes { get; set; }

        /// <summary>
        /// Gets or sets style of the validator.
        /// </summary>
        ValueValidatorTextStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the include char list (the list of additional valid characters).
        /// </summary>
        string CharIncludes { get; set; }

        /// <summary>
        /// Adds chars to the list of included characters.
        /// </summary>
        /// <param name="chars">Characters to include.</param>
        void AddCharIncludes(string chars);

        /// <summary>
        /// Adds include to the list of included values.
        /// </summary>
        /// <param name="include">Value to include.</param>
        void AddInclude(string include);

        /// <summary>
        /// Adds exclude to the list of excluded values.
        /// </summary>
        /// <param name="exclude">Value to exclude.</param>
        void AddExclude(string exclude);

        /// <summary>
        /// Adds chars to the list of excluded (invalid) characters.
        /// </summary>
        /// <param name="chars">Characters to exclude.</param>
        void AddCharExcludes(string chars);

        /// <summary>
        /// Clears the list of excluded values.
        /// </summary>
        void ClearExcludes();

        /// <summary>
        /// Clears the list of included values.
        /// </summary>
        void ClearIncludes();
    }
}