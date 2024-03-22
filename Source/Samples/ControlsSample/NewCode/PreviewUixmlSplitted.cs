using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PreviewUixmlSplitted : PreviewFileSplitted
    {
        public static Func<IFilePreview>? CreateTextPreview;

        public PreviewUixmlSplitted()
            : base(new PreviewUixml(), DefaultCreateTextPreview())
        {

        }

        public static bool IsSupportedFile(string fileName)
        {
            return PreviewUixml.IsSupportedFile(fileName);
        }

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
