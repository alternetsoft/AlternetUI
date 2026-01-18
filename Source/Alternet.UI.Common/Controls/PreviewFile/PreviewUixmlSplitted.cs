using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements Uixml preview control which splits it's view into two docked panels.
    /// In the first panel Uixml is previewed as text and in the second as form.
    /// </summary>
    public partial class PreviewUixmlSplitted : PreviewFileSplitted
    {
        /// <summary>
        /// Gets or sets callback function which is fired when
        /// <see cref="PreviewUixmlSplitted"/> needs to create
        /// sub-control which is used to preview uixml as text.
        /// </summary>
        public static Func<IFilePreview>? CreateTextPreview;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewUixmlSplitted"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PreviewUixmlSplitted(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewUixmlSplitted"/> class.
        /// </summary>
        public PreviewUixmlSplitted()
            : base(new PreviewUixml(), DefaultCreateTextPreview(), false)
        {
        }

        /// <summary>
        /// Gets whether specified file is supported in this preview control.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
        /// <returns></returns>
        public static bool IsSupportedFile(string fileName)
        {
            return PreviewUixml.IsSupportedFile(fileName);
        }

        /// <summary>
        /// Creates this preview control.
        /// </summary>
        /// <returns></returns>
        public static IFilePreview CreatePreviewControl()
        {
            return new PreviewUixmlSplitted();
        }

        private static IFilePreview DefaultCreateTextPreview()
        {
            if(CreateTextPreview is null)
                return new PreviewTextFile();
            return CreateTextPreview();
        }
    }
}
