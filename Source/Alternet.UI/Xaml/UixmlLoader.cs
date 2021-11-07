using System.IO;
using System.Reflection;

namespace Alternet.UI
{
    /// <summary>
    /// Creates an object graph from a source UIXML.
    /// </summary>
    public class UixmlLoader
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

        static int performanceLogNumber = 1;
        static System.TimeSpan totalCompilationTime = new System.TimeSpan();
        static System.TimeSpan totalInitializationTime = new System.TimeSpan();

        bool logPerformance = false;

        /// <summary>
        /// Populates an existing root object with the object property values created from a source XAML.
        /// </summary>
        public void LoadExisting(Stream xamlStream, object existingObject)
        {
            const string LogPath = @"c:\temp\UI\xaml-perf.log";
            System.Diagnostics.Stopwatch? performanceStopwatch = null;
            if (logPerformance)
            {
                if (performanceLogNumber == 1)
                    File.AppendAllText(LogPath, System.Environment.CommandLine + "---------------\n");
                performanceStopwatch = new System.Diagnostics.Stopwatch();
                performanceStopwatch.Start();
            }

            using (var reader = new StreamReader(xamlStream))
            {
                var compiler = new XamlCompiler();
                var compilation = compiler.Compile(reader.ReadToEnd(), null);
                
                if (performanceStopwatch != null)
                {
                    performanceStopwatch.Stop();
                    totalCompilationTime += performanceStopwatch.Elapsed;
                    File.AppendAllText(LogPath, $"XAML Load Compile #{performanceLogNumber}: {performanceStopwatch.Elapsed}, Total: {totalCompilationTime.TotalMilliseconds}\n");
                    performanceStopwatch.Restart();
                }

                compilation.populate(null, existingObject);
            }

            if (performanceStopwatch != null)
            {
                performanceStopwatch.Stop();
                totalInitializationTime += performanceStopwatch.Elapsed;
                File.AppendAllText(LogPath, $"XAML Load Init #{performanceLogNumber++}: {performanceStopwatch.Elapsed}, Total Init: {totalInitializationTime.TotalMilliseconds}\n");
            }
        }

        ///// <summary>
        ///// Compiles xamlStream to initialization assembly.
        ///// </summary>
        //public Assembly Compile(Stream xamlStream, string targetDllFileName)
        //{
        //    using (var reader = new StreamReader(xamlStream))
        //    {
        //        var compiler = new XamlCompiler();
        //        return compiler.Compile(reader.ReadToEnd(), targetDllFileName).assembly;
        //    }
        //}
    }
}