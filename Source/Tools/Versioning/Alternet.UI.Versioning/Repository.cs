namespace Alternet.UI.Versioning
{
    public sealed class Repository
    {
        public Repository(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }
    }
}