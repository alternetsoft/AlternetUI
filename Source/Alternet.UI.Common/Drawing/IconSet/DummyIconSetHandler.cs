using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class DummyIconSetHandler : DummyImageContainer, IIconSetHandler
    {
        public static IIconSetHandler Default = new DummyIconSetHandler();
    }
}
