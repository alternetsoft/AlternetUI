using System.IO;

namespace Alternet.UI
{
    public class XamlLoader
    {
        public object Load(Stream xamlStream)
        {
            using (var reader = new StreamReader(xamlStream))
            {
                var compiler = new XamlCompiler();
                var compilation = compiler.Compile(reader.ReadToEnd());
                return compilation.create(null);
            }
        }

        public void LoadExisting(Stream xamlStream, object existingObject)
        {
            using (var reader = new StreamReader(xamlStream))
            {
                var compiler = new XamlCompiler();
                var compilation = compiler.Compile(reader.ReadToEnd());
                compilation.populate(null, existingObject);
            }
        }
    }
}