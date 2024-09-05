using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class GenerateBindableSetting
    {
        public string? RootFolder { get; set; }

        public string? PathToDll { get; set; }

        public string? TypeName { get; set; }

        public string? PathToResult { get; set; }

        public string? ResultTypeName { get; set; }

        public void Prepare()
        {
            if(!string.IsNullOrEmpty(RootFolder))
                RootFolder = Path.GetFullPath(RootFolder);
            
            PathToDll = ReplaceParams(PathToDll);
            PathToResult = ReplaceParams(PathToResult);

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
