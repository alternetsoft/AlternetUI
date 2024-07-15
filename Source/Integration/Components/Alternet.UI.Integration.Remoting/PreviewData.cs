using System;
using System.Drawing;

namespace Alternet.UI.Integration
{
    public class PreviewData
    {
        public PreviewData(string imageFileName)
        {
            ImageFileName = imageFileName;
        }

        public string ImageFileName { get; }
    }
}
