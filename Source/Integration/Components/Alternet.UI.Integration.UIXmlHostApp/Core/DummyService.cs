using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    internal class DummyService
    {
        private readonly Action<IDictionary<string, object>> onUixmlUpdateSuccess;
        private readonly Action<IDictionary<string, object>> onUixmlUpdateFailure;
        private readonly Action onTick;
        private readonly string screenshotsDirectory;

        public DummyService(
            Action<IDictionary<string, object>> onUixmlUpdateSuccess,
            Action<IDictionary<string, object>> onUixmlUpdateFailure,
            Action onTick,
            string screenshotsDirectory)
        {
            this.onUixmlUpdateSuccess = onUixmlUpdateSuccess;
            this.onUixmlUpdateFailure = onUixmlUpdateFailure;
            this.onTick = onTick;
            this.screenshotsDirectory = screenshotsDirectory;
        }

        public void Run()
        {
        }

        public void ProcessUixmlUpdate(IDictionary<string, object> parameters)
        {
        }
    }
}
