using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle an event
    /// with <see cref="ValidationEventArgs"/> event data.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An object that contains an event data.</param>
    public delegate void ValidationEventHandler(object? sender, ValidationEventArgs e);

    /// <summary>
    /// Provides data for the events that have an url argument.
    /// </summary>
    public class ValidationEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationEventArgs"/> class.
        /// </summary>
        /// <param name="text"><see cref="string"/> value for the validation.</param>
        public ValidationEventArgs(string? text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets or sets whether <see cref="Text"/> is valid.
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// Gets <see cref="string"/> value for validation.
        /// </summary>
        public string? Text { get; }

        /// <summary>
        /// Gets whether <see cref="Text"/> is null or empty.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrEmpty => string.IsNullOrEmpty(Text);

        /// <summary>
        /// Gets whether <see cref="Text"/> is null or white space.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(Text);
    }
}