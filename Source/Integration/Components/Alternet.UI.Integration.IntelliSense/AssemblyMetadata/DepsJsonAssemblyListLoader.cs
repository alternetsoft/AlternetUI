﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Alternet.UI.Integration.IntelliSense.AssemblyMetadata
{
    public static class DepsJsonAssemblyListLoader
    {
        class Library
        {
            public string PackageName { get; set; }
            public string LibraryPath { get; set; }
            public string DllName { get; set; }
        }

        static IEnumerable<Library> TransformDeps(JObject lstr)
        {
            foreach (var prop in lstr.Properties())
            {
                var package = prop.Name;
                var runtime = ((JObject) prop.Value["runtime"]);
                if(runtime == null)
                    continue;
                foreach (var dllprop in runtime.Properties())
                {
                    var libraryPath = dllprop.Name;
                    var dllName = libraryPath.Split('/').Last();
                    yield return new Library
                    {
                        PackageName = package,
                        DllName = dllName,
                        LibraryPath = libraryPath
                    };
                }

            }
        }

        public static IEnumerable<string> ParseFile(string path)
        {
            var dir = Path.GetDirectoryName(path);
            var nugetDirs = NuGetPackagesLocator.GetNugetPackagesDirs();
            var deps = JObject.Parse(File.ReadAllText(path));
            var target = deps["runtimeTarget"]["name"].ToString();
            foreach (var l in TransformDeps((JObject) deps["targets"][target]))
            {
                var localPath = Path.Combine(dir, l.DllName);
                if (File.Exists(localPath))
                {
                    yield return localPath;
                    continue;
                }
                foreach (var nugetPath in nugetDirs)
                {
                    foreach (var tolower in new[]{false, true})
                    {
                        var packagePath = Path.Combine(nugetPath,
                            tolower ? l.PackageName.ToLowerInvariant() : l.PackageName, l.LibraryPath);
                        if (File.Exists(packagePath))
                        {
                            yield return packagePath;
                            break;
                        }
                    }
                }

            }


        }
    }
}
