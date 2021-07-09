using System.IO;

namespace Alternet.UI
{
    /// <summary>
    /// Creates an object graph from a source XAML.
    /// </summary>
    public class XamlLoader
    {
        /// <summary>
        /// Returns an object graph created from a source XAML.
        /// </summary>
        public object Load(Stream xamlStream)
        {
            using (var reader = new StreamReader(xamlStream))
            {
                var compiler = new XamlCompiler();
                var compilation = compiler.Compile(reader.ReadToEnd());
                return compilation.create(null);
            }
        }

        /// <summary>
        /// Populates an existing root object with the object property values created from a source XAML.
        /// </summary>
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