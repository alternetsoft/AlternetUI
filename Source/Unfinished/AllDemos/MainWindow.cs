using System;
using System.ComponentModel;
using System.IO;

using Alternet.Drawing;
using Alternet.UI;

namespace AllDemos
{
    public partial class MainWindow : CustomDemoWindow
    {
        protected override void AddPages()
        {
            AddPage("Welcome", CreateWelcomePage);
            AddPage("Samples", CreateOtherPage);
        }

        Control CreateOtherPage()
        {
            NameValue<Func<Control>>[] pages =
            {
                new("Internal", CreateInternalSamplesPage),
                new("External", CreateAllSamplesPage),
            };

            return CreateCustomPage(pages);
        }

        Control CreateAllSamplesPage() => new ControlsSample.AllSamplesPage();
        Control CreateInternalSamplesPage() => new InternalSamplesPage();
        Control CreateWelcomePage() => new WelcomePage();
    }
}