using System.IO;
using System;
using System.Collections.Generic;

namespace Alternet.UI.PublicSourceGenerator.Utils
{
    sealed class DirectoryCopier
    {
        void _CopyFoldersDeep(string currentPath, string rootPathCurrent, string rootPathNew)
        {
            // recursivly copies from one root dir to another dir all data.

            // Walk Dir
            DirectoryInfo di = new DirectoryInfo(currentPath);

            //if(!MakeFlatCopy)
            //{
            //    // create Dir
            //    string dirPathNew = currentPath.Replace(rootPathCurrent, rootPathNew);
            //    Directory.CreateDirectory(dirPathNew);
            //}

            // copy files from old Dir to new Dir.
            foreach (FileInfo fi in di.GetFiles())
            {
                if (FileFilter != null)
                {
                    if (!FileFilter(fi))
                        continue;
                }

                string filePath = fi.FullName;
                string filePathNew;

                if (MakeFlatCopy)
                {
                    filePathNew = Path.Combine(rootPathNew, fi.Name);
                }
                else
                {
                    filePathNew = filePath.Replace(rootPathCurrent, rootPathNew);
                }

                if (PostProcessOutputFilePathFunc != null)
                    filePathNew = PostProcessOutputFilePathFunc(filePathNew);

                if (!MakeFlatCopy)
                {
                    var dirPathNew = Path.GetDirectoryName(filePathNew);
                    if (dirPathNew == null)
                        throw new InvalidOperationException();
                    if (!Directory.Exists(dirPathNew))
                        Directory.CreateDirectory(dirPathNew);
                }

                File.Copy(filePath, filePathNew, true);
            }

            // process deeper
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                if (DirectoryFilter != null)
                {
                    if (!DirectoryFilter(d))
                        continue;
                }

                if (_ExcludedDirectories.Contains(_NormalizeDirectoryName(d.FullName)))
                    continue;

                _CopyFoldersDeep(d.FullName, rootPathCurrent, rootPathNew);
            }
        }

        public Func<string, string>? PostProcessOutputFilePathFunc { get; set; }

        public bool MakeFlatCopy { get; set; }

        public string? SourceDirectoryPath
        {
            get;
            set;
        }

        public string? TargetDirectoryPath
        {
            get;
            set;
        }

        public void Run()
        {
            if (SourceDirectoryPath == null || TargetDirectoryPath == null)
                throw new InvalidOperationException();

            SourceDirectoryPath = Path.GetFullPath(SourceDirectoryPath);
            TargetDirectoryPath = Path.GetFullPath(TargetDirectoryPath);

            if (!Directory.Exists(SourceDirectoryPath))
                throw new Exception(string.Format("Directory '{0}' does not exist.", SourceDirectoryPath));
            _CopyFoldersDeep(SourceDirectoryPath, SourceDirectoryPath, TargetDirectoryPath);
        }

        public static void Copy(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            var copier = new DirectoryCopier()
            {
                SourceDirectoryPath = sourceDirectoryPath,
                TargetDirectoryPath = destinationDirectoryPath
            };
            copier.Run();
        }

        public Predicate<DirectoryInfo>? DirectoryFilter
        {
            get;
            set;
        }

        public Predicate<FileInfo>? FileFilter
        {
            get;
            set;
        }

        static string _NormalizeDirectoryName(string path)
        {
            return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        HashSet<string> _ExcludedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public void ExcludeDirectory(string path)
        {
            _ExcludedDirectories.Add(_NormalizeDirectoryName(path));
        }

        public void ResetExcludedDirectories()
        {
            _ExcludedDirectories.Clear();
        }
    }
}
