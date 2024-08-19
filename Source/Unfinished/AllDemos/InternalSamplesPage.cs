using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using System.IO;
using System.Diagnostics;

namespace AllDemos
{
    public partial class InternalSamplesPage : CustomInternalSamplesPage
    {
        protected override void AddDefaultItems()
        {
            Add("Property Grid", () => new PropertyGridSample.MainWindow());
        }
    }
}