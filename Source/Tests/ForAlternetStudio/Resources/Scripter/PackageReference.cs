using System.Windows.Forms;
using Newtonsoft.Json;

public class ScriptClass
{
    public static void Main()
    {
        string data = @"{
              a: 1,
              name: ""Bill Smith"",
              isTall: true
            }";
        dynamic obj = JsonConvert.DeserializeObject(data);
        if (obj != null)
        {
            MessageBox.Show(string.Format("Object name: {0}", obj.name));
        }
    }
}