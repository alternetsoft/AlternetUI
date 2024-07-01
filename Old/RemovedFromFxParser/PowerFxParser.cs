using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Common;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Lsp;

namespace Alternet.Syntax.Parsers.PowerFx
{
    public class PowerFxParser : SyntaxParser
    {
        /// <summary>
        /// Represents default set of flags determining syntax parsing and formatting behavior.
        /// </summary>
        public static SyntaxOptions DefaultFxSyntaxOptions
            = ParserConsts.DefaultLspSyntaxOptions | SyntaxOptions.IndentationBasedFolding;

        /// <summary>
        /// Represents a default collection of characters that initializes a smart
        /// formatting procedure when typing.
        /// </summary>
        public static string DefaultFxSmartFormatChars = "}";

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerFxParser"/> class with default settings.
        /// </summary>
        public PowerFxParser()
            : this(null)
        {
            Options = DefaultFxSyntaxOptions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerFxParser"/> class with specified container.
        /// </summary>
        /// <param name="container">Specifies <see cref="IContainer"/> that contains
        /// this new instance.</param>
        public PowerFxParser(System.ComponentModel.IContainer container)
            : base(container)
        {
        }

        /// <inheritdoc/>
        public override bool CaseSensitive => true;

        /// <summary>
        /// Gets or sets a boolean value which indicates is code style warnings are enabled.
        /// </summary>
        [DefaultValue(false)]
        public bool CodeStyleWarningsEnabled { get; set; }

        /// <summary>
        /// Resets <c>Options</c> to the default value.
        /// </summary>
        public override void ResetOptions()
        {
            Options = DefaultFxSyntaxOptions;
        }

        /// <summary>
        /// Indicates whether the <c>Options</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>Options</c> differs from its default value; otherwise false.</returns>
        public override bool ShouldSerializeOptions()
        {
            return Options != DefaultFxSyntaxOptions;
        }

        /// <summary>
        /// Resets the <c>SmartFormatChars</c> to the default value.
        /// </summary>
        public override void ResetSmartFormatChars()
        {
            SmartFormatChars = DefaultFxSmartFormatChars.ToCharArray();
        }

        /// <summary>
        /// Indicates whether the <c>SmartFormatChars</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>SmartFormatChars</c> differs from its
        /// default value; otherwise false.</returns>
        public override bool ShouldSerializeSmartFormatChars()
        {
            return new string(SmartFormatChars) != DefaultFxSmartFormatChars;
        }
    }
}
