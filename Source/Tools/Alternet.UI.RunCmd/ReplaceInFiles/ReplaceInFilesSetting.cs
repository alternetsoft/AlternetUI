using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    public class ReplaceInFilesSetting
    {
        public string? Name { get; set; }

        public string? PathToFile { get; set; }

        public string? PathToResultFile { get; set; }

        public Collection<ReplaceInFileSetting> ReplaceItems { get; } = new();

        public void Prepare(ReplaceInFilesSettings globals, string pathToConfig)
        {
            var thisFileDirectory = Path.GetDirectoryName(pathToConfig);
            var thisFileDirectoryRootDrive = Path.GetPathRoot(pathToConfig);

            PathToFile = ReplaceParams(PathToFile);
            PathToResultFile = ReplaceParams(PathToResultFile);

            if (!string.IsNullOrEmpty(PathToFile))
                PathToFile = Path.GetFullPath(PathToFile);

            string? ReplaceParam(string? s, string prmName, string? prmValue)
            {
                if (s is null)
                    return null;

                if (!string.IsNullOrEmpty(prmValue))
                {
                    var result = s.Replace($"$({prmName})", prmValue);
                    return result;
                }

                return s;
            }

            string? ReplaceParams(string? s)
            {
                var result = ReplaceParam(s, "ThisFileDirectory", thisFileDirectory);
                result = ReplaceParam(result, "ThisFileDirectoryRootDrive", thisFileDirectoryRootDrive);
                return result;
            }

            if (string.IsNullOrWhiteSpace(PathToResultFile))
                PathToResultFile = PathToFile;
        }

        public bool Execute(ReplaceInFilesSettings globals)
        {
            return true;
        }
    }
}

