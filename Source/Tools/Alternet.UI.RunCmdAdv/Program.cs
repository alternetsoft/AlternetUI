using Alternet.UI;

using System.Diagnostics;
using System.IO;

var application = new Application();

Commands.RunApplication(args, adv: true);

application.Dispose();
