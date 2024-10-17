using System;

namespace DebugRemoteScript.Wpf
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var remote = RemoteAPI.InitializeAPI(args);
            var text = remote.GetEditorText();
            var str = string.Empty;
            foreach (var c in text)
            {
                str = c + str;
            }
            remote.ShowMessage(str);
        }
    }
}