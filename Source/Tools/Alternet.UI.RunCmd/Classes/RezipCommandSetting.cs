using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public class RezipCommandSetting
    {
        public bool? Ignore { get; set; }

        public string? RootFolder { get; set; }

        public string? PathToZip { get; set; }

        public string? PathToResult { get; set; }

        public string? FileMasks { get; set; }

        public string? CommandToRun { get; set; }

        public string? CommandArgs { get; set; }

        public bool? RecreateZip { get; set; }

        public bool Execute(RezipCommandSettings globals)
        {
            return true;
        }

        public void Prepare()
        {
            if (!string.IsNullOrEmpty(RootFolder))
                RootFolder = Path.GetFullPath(RootFolder);

            PathToZip = ReplaceParams(PathToZip);
            PathToResult = ReplaceParams(PathToResult);
            CommandToRun = ReplaceParams(CommandToRun);
            CommandArgs = ReplaceParams(CommandArgs);

            string? ReplaceParams(string? s)
            {
                if (s is null)
                    return null;
                if (!string.IsNullOrEmpty(RootFolder))
                {
                    var result = s.Replace("$(RootFolder)", RootFolder);
                    return result;
                }

                return s;
            }
        }
    }
}
