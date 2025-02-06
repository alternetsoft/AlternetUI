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

        public static string? ReplaceParam(
            string? s,
            string prmName,
            string? prmValue,
            string paramPrefixChars = "$")
        {
            if (s is null)
                return null;

            if (!string.IsNullOrEmpty(prmValue))
            {
                var result = s.Replace($"{paramPrefixChars}({prmName})", prmValue);
                result = result.Replace($"{paramPrefixChars}[{prmName}]", prmValue);
                return result;
            }

            return s;
        }

        public void Prepare(ReplaceInFilesSettings globals, string pathToConfig)
        {
            var thisFileDirectory = Path.GetDirectoryName(pathToConfig);
            var thisFileDirectoryRootDrive = Path.GetPathRoot(pathToConfig);

            PathToFile = ReplaceParams(PathToFile);
            PathToResultFile = ReplaceParams(PathToResultFile);

            if (!string.IsNullOrEmpty(PathToFile))
                PathToFile = Path.GetFullPath(PathToFile);

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
            if (PathToFile is null || PathToResultFile is null)
                return false;

            var data = FileUtils.StringFromFile(PathToFile);
            var tempFileName = StringUtils.GetUniqueString();
            var resultFolder = Path.GetDirectoryName(PathToResultFile) ?? string.Empty;
            var tempFilePath = Path.Combine(Path.GetFullPath(resultFolder), tempFileName);
            
            foreach(var replaceItem in ReplaceItems)
            {
                if (replaceItem.ReplaceFrom is null)
                    continue;

                data = data.Replace(replaceItem.ReplaceFrom, replaceItem.ReplaceTo);
            }

            File.Delete(tempFilePath);
            FileUtils.StringToFile(tempFilePath, data);

            var oldResult = PathToResultFile + ".old";
            File.Delete(oldResult);
            if(File.Exists(PathToResultFile))
            {
                File.Move(PathToResultFile, oldResult);
            }
            File.Move(tempFilePath, PathToResultFile);

            return true;
        }
    }
}

