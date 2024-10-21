using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace ApiDoc
{
    public class MainWindowSimple : Window
    {
        public static bool AddBuildNumber = false;

        private readonly ApiDocSamplesPage samplesPage = new();

        public MainWindowSimple()
        {
            Title = $"Alternet.UI.Documentation Samples";

            if (AddBuildNumber)
            {
                Title = $"{Title} {SystemSettings.Handler.GetUIVersion()}";
            }

            Size = (600, 600);
            StartLocation = WindowStartLocation.CenterScreen;

            samplesPage.Parent = this;
        }
    }
}
