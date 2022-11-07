using CommandLine;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SitemapSplitter
{
    internal static class Splitter
    {
        public static void SplitSitemaps(Options options)
        {
            var configuration = ConfigLoader.Load(options.ConfigFilePath);

            var outputDocuments = new Dictionary<ConfigurationPart, List<urlsetUrl>>();
            using var sourceSitemapFile = File.OpenRead(options.SourceSiteMapPath);
            var sourceSitemap = (urlset)(new XmlSerializer(typeof(urlset)).Deserialize(sourceSitemapFile) ?? throw new Exception());

            Console.WriteLine($"Loaded {Path.GetFileName(options.SourceSiteMapPath)} which contains {sourceSitemap.url.Length} URLs.");

            var regexes = new Dictionary<ConfigurationPart, Regex>();

            Regex GetLocationRegex(ConfigurationPart part)
            {
                if (!regexes!.TryGetValue(part, out var r))
                {
                    r = new Regex(part.LocationRegex);
                    regexes.Add(part, r);
                }

                return r;
            }

            List<urlsetUrl> GetOutputUrlList(ConfigurationPart part)
            {
                if (!outputDocuments!.TryGetValue(part, out var r))
                {
                    r = new List<urlsetUrl>();
                    outputDocuments.Add(part, r);
                }

                return r;
            }

            string GetOutputFilePath(ConfigurationPart part)
            {
                if (!Directory.Exists(options.OutputDirectoryPath))
                    Directory.CreateDirectory(options.OutputDirectoryPath);

                return Path.Combine(
                    options.OutputDirectoryPath,
                    Path.GetFileNameWithoutExtension(configuration.Output.FileName) +
                    part.OutputFileSuffix +
                    Path.GetExtension(configuration.Output.FileName));
            }

            foreach (var url in sourceSitemap.url)
            {
                bool found = false;
                foreach (var part in configuration.Parts)
                {
                    var regex = GetLocationRegex(part);
                    if (regex.IsMatch(url.loc))
                    {
                        found = true;
                        GetOutputUrlList(part).Add(url);
                        break;
                    }
                }

                if (!found)
                    throw new Exception("No part LocationRegex rules matches the location: " + url.loc);
            }

            foreach (var part in configuration.Parts)
            {
                var document = new urlset();
                document.url = GetOutputUrlList(part).ToArray();
                string outputFilePath = GetOutputFilePath(part);
                Console.WriteLine($"Writing {Path.GetFileName(outputFilePath)} with {document.url.Length} URLs.");
                using var outputFile = File.Create(outputFilePath);
                var streamWriter = new XmlTextWriter(outputFile, Encoding.UTF8);
                streamWriter.Formatting = Formatting.Indented;
                new XmlSerializer(typeof(urlset)).Serialize(streamWriter, document);
            }
        }

        [Verb("split", HelpText = "Split sitemaps.")]
        public class Options
        {
            public Options(string sourceSiteMapPath, string outputDirectoryPath, string configFilePath)
            {
                SourceSiteMapPath = sourceSiteMapPath;
                OutputDirectoryPath = outputDirectoryPath;
                ConfigFilePath = configFilePath;
            }

            [Option("SourceSiteMapPath", Required = true)]
            public string SourceSiteMapPath { get; }

            [Option("OutputDirectoryPath", Required = true)]
            public string OutputDirectoryPath { get; }

            [Option("ConfigFilePath", Required = true)]
            public string ConfigFilePath { get; }
        }
    }
}