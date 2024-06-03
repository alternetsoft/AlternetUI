using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class DummyImageListHandler : DummyImageContainer, IImageListHandler
    {
        public static IImageListHandler Default = new DummyImageListHandler();

        public SizeI Size { get; set; }
    }
}
