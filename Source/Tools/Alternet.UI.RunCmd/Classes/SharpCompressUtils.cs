using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpCompress;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace Alternet.UI
{
    public static class SharpCompressUtils
    {
        static int reportedUnzipProgress = -1;

        public static void ZipFile(
            string pathToFile,
            string pathToArch,
            CompressionType compressionType = CompressionType.Deflate,
            string? otherExtensions = null)
        {
            List<string> files = new();
            files.Add(pathToFile);

            if(otherExtensions is not null)
            {
                var extList = otherExtensions.Split(',');

                foreach(var ext in extList)
                {
                    var extension = "." + ext.Trim().Trim('.');
                    var fileWithOtherExt = Path.ChangeExtension(pathToFile, extension);
                    if (!File.Exists(fileWithOtherExt))
                        continue;
                    files.Add(fileWithOtherExt);
                }
            }

            using var archive = ZipArchive.Create();
            
            foreach (var file in files)
            {
                var fileInArchive = file;
                fileInArchive = Path.GetFileName(fileInArchive);
                archive.AddEntry(fileInArchive, file);
            }

            archive.SaveTo(pathToArch, compressionType);
        }

        public static void ZipFolderWithRootSubFolder(
            string pathToFolder,
            string pathToArch,
            string? rootSubFolder = null,
            CompressionType compressionType = CompressionType.Deflate)
        {
            if(rootSubFolder is null)
            {
                ZipFolder(pathToFolder, pathToArch, compressionType);
                return;
            }

            pathToFolder = PathUtils.AddDirectorySeparatorChar(Path.GetFullPath(pathToFolder));

            var files = Directory.EnumerateFiles(
                pathToFolder,
                "*",
                SearchOption.AllDirectories);

            using var archive = ZipArchive.Create();

            foreach(var file in files)
            {
                var fileInArchive = file;

                if(fileInArchive.StartsWith(pathToFolder))
                {
                    fileInArchive = fileInArchive.Remove(0, pathToFolder.Length);
                }
                else
                {
                    fileInArchive = Path.GetFileName(fileInArchive);
                }

                fileInArchive = Path.Combine(rootSubFolder, fileInArchive);

                archive.AddEntry(fileInArchive, file);
            }

            archive.SaveTo(pathToArch, compressionType);
        }

        public static void ZipFolder(
            string pathToFolder,
            string pathToArch,
            CompressionType compressionType = CompressionType.Deflate)
        {
            using var archive = ZipArchive.Create();
            archive.AddAllFromDirectory(pathToFolder);
            archive.SaveTo(pathToArch, compressionType);
        }

        public static void Unzip(
            string filePath,
            string extractToFolder,
            Action<string>? writeLine = null,
            Action<string>? writeProgress = null)
        {
            reportedUnzipProgress = -1;

            WriteLine($"Unzip from: {filePath}");
            WriteLine($"Unzip to: {extractToFolder}");
            WriteLine(string.Empty);

            if (!Directory.Exists(extractToFolder))
                Directory.CreateDirectory(extractToFolder);

            void WriteLine(string? s = default)
            {
                s ??= string.Empty;

                if (writeLine is not null)
                    writeLine(s);
                else
                    Console.WriteLine(s);
            }

            void WriteProgress(string s)
            {
                if (writeProgress is not null)
                    writeProgress(s);
                else
                    Console.Write($"\r{s}");
            }

            static float GetProgressPercentage(float total, float current)
            {
                if (current >= total)
                    return 100;
                var result = (current * 100f) / total;
                return result;
            }

            try
            {
                WriteLine("Unzip started.");
                WriteLine();

                var opt = new ExtractionOptions()
                {
                    ExtractFullPath = true,
                    Overwrite = true
                };

                if (SevenZipArchive.IsSevenZipFile(filePath))
                    Extract7z();
                else
                    ExtractOther();

                void Extract7z()
                {
                    using Stream stream = File.OpenRead(filePath);
                    using var archive = ArchiveFactory.Open(stream);

                    var total = archive.Entries.Count();
                    var current = 0;

                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    Fn();

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;

                    WriteIntProgress(100);

                    // Format and display the TimeSpan value.
                    string elapsedTime = string.Format(
                        "{0:00}:{1:00}:{2:00}",
                        ts.Hours,
                        ts.Minutes,
                        ts.Seconds);
                    WriteLine("Unzip Time: " + elapsedTime);

                    void WriteIntProgress(int p)
                    {
                        WriteProgress(string.Format("Extracting {0}%    ", p.ToString()));
                    }

                    void Fn()
                    {
                        var reader = archive.ExtractAllEntries();
                        while (reader.MoveToNextEntry())
                        {
                            var entry = reader.Entry;

                            if (!entry.IsDirectory)
                            {
                                var p = (int)GetProgressPercentage(total, current);

                                if(p != reportedUnzipProgress)
                                {
                                    WriteIntProgress(p);
                                    reportedUnzipProgress = p;
                                }

                                if (entry.Size == 0)
                                {
                                    WriteEntryToDirectory(
                                        entry,
                                        extractToFolder,
                                        opt,
                                        WriteEmptyFile);

                                    void WriteEmptyFile(string path, ExtractionOptions? opt)
                                    {
                                        var stream = File.Create(path);
                                        stream.Flush();
                                        stream.Dispose();
                                    }
                                }
                                else
                                {
                                    reader.WriteEntryToDirectory(extractToFolder, opt);
                                }
                            }

                            current++;
                        }
                    }
                }

                void ExtractOther()
                {
                    using Stream stream = File.OpenRead(filePath);
                    using var reader = ReaderFactory.Open(stream);

                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            WriteProgress($"Extracted: {reader.Entry.Key}");
                            reader.WriteEntryToDirectory(extractToFolder, opt);
                        }
                    }
                }

                WriteLine();
                WriteLine("Unzip finished.");
            }
            catch (Exception e)
            {
                LogUtils.LogExceptionToConsole(e);
                LogUtils.LogException(e);
                throw;
            }
        }

        /// <summary>
        /// Extract to specific directory, retaining filename
        /// </summary>
        private static void WriteEntryToDirectory(
            IEntry entry,
            string destinationDirectory,
            ExtractionOptions? options,
            Action<string, ExtractionOptions?> write
        )
        {
            string destinationFileName;
            var fullDestinationDirectoryPath = Path.GetFullPath(destinationDirectory);

            //check for trailing slash.
            if (
                fullDestinationDirectoryPath[fullDestinationDirectoryPath.Length - 1]
                != Path.DirectorySeparatorChar
            )
            {
                fullDestinationDirectoryPath += Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(fullDestinationDirectoryPath))
            {
                throw new ExtractionException(
                    $"Directory does not exist to extract to: {fullDestinationDirectoryPath}"
                );
            }

            options ??= new ExtractionOptions() { Overwrite = true };

            var file = Path.GetFileName(entry.Key.NotNull("Entry Key is null")).NotNull("File is null");
            if (options.ExtractFullPath)
            {
                var folder = Path.GetDirectoryName(entry.Key.NotNull("Entry Key is null"))
                    .NotNull("Directory is null");
                var destdir = Path.GetFullPath(Path.Combine(fullDestinationDirectoryPath, folder));

                if (!Directory.Exists(destdir))
                {
                    if (!destdir.StartsWith(fullDestinationDirectoryPath, StringComparison.Ordinal))
                    {
                        throw new ExtractionException(
                            "Entry is trying to create a directory outside of the destination directory."
                        );
                    }

                    Directory.CreateDirectory(destdir);
                }
                destinationFileName = Path.Combine(destdir, file);
            }
            else
            {
                destinationFileName = Path.Combine(fullDestinationDirectoryPath, file);
            }

            if (!entry.IsDirectory)
            {
                destinationFileName = Path.GetFullPath(destinationFileName);

                if (
                    !destinationFileName.StartsWith(
                        fullDestinationDirectoryPath,
                        StringComparison.Ordinal
                    )
                )
                {
                    throw new ExtractionException(
                        "Entry is trying to write a file outside of the destination directory."
                    );
                }
                write(destinationFileName, options);
            }
            else if (options.ExtractFullPath && !Directory.Exists(destinationFileName))
            {
                Directory.CreateDirectory(destinationFileName);
            }
        }
    }
}
