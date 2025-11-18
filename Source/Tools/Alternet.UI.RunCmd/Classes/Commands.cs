using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using SharpCompress.Common;

namespace Alternet.UI
{
    public static partial class Commands
    {
        private static readonly Dictionary<string, Action<CommandLineArgs>> commands = new();
        private static readonly List<string> originalCommandNames = new();

        static Commands()
        {
        }

        public static IEnumerable<string> CommandNames
        {
            get
            {
                return originalCommandNames;
            }
        }

        public static void RunApplication(string[] args, bool adv = false)
        {
            RegisterCommands(adv);

            Console.WriteLine();

            CommandLineArgs.Default.Parse(args);

            var commandNames = CommandLineArgs.Default.AsString("-r");

            if (CommandLineArgs.Default.HasArgument("-hr"))
            {
                Console.WriteLine("==================================================");
            }

            var appName = adv ? "Alternet.UI.RunCmdAdv" : "Alternet.UI.RunCmd";
            Console.WriteLine($"{appName} (c) 2023-{DateTime.Now.Year} AlterNET Software");

            Console.WriteLine();

            if (string.IsNullOrWhiteSpace(commandNames))
            {
                Console.WriteLine("No commands are specified.");
                Console.WriteLine();
                Console.WriteLine("Known commands:");
                Console.WriteLine();

                var commands = Commands.CommandNames;

                foreach (var command in commands)
                {
                    Console.WriteLine(command);
                }

                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("Alternet.UI.RunCmd.exe -r=<CommandName> [Parameters]");

                Console.WriteLine();
                Console.WriteLine("Usage example:");
                Console.WriteLine("Alternet.UI.RunCmd.exe -r=zipFolder Folder=\"d:\\testFolder\" Result=\"d:\\result.zip\"");

                return;
            }

            if (CommandLineArgs.Default.HasArgument("-details"))
            {
                Console.WriteLine($"Commands: {commandNames}");
                Console.WriteLine();
                Console.WriteLine("Arguments:");

                foreach (var a in args)
                {
                    Console.WriteLine($"[{a}]");
                }

                Console.WriteLine();
            }

            Commands.RunCommand(commandNames, CommandLineArgs.Default);
        }

        public static void CmdDownload(CommandLineArgs args)
        {
            string docUrl = args.AsString("Url");
            string filePath = args.AsString("Path");
            filePath = Path.GetFullPath(filePath);
            DownloadUtils.DownloadFileWithConsoleProgress(docUrl, filePath).Wait();
        }

        public static void CmdDownloadAndUnzip(CommandLineArgs args)
        {
            string docUrl = args.AsString("Url");
            string filePath = args.AsString("Path");
            string extractToPath = args.AsString("ExtractTo");
            filePath = Path.GetFullPath(filePath);
            DownloadUtils.DownloadFileWithConsoleProgress(docUrl, filePath).Wait();
            Console.WriteLine();
            SharpCompressUtils.Unzip(filePath, extractToPath);
        }

        public static void CmdWaitEnter(CommandLineArgs args)
        {
            Console.WriteLine("Press ENTER to close this window...");
            Console.ReadLine();
        }

        public static void CmdRunControlsSample(CommandLineArgs args)
        {
            string path = Path.Combine(
                CommonUtils.GetAppFolder(),
                "..", "..", "..", "..", "..", "Samples", "ControlsSample", "ControlsSample.csproj");
            path = Path.GetFullPath(path);
            string? pathFolder = Path.GetDirectoryName(path)?.TrimEnd('\\')?.TrimEnd('/');
            Console.WriteLine("Run ControlsSample: " + path);
            CommonProcs.ProcessStart("dotnet", $"run --framework net8.0", pathFolder);
        }

        public static void CmdWaitAnyKey(CommandLineArgs args)
        {
            Console.WriteLine("Press any key to close this window...");
            Console.ReadKey();
        }

        public static void CmdUnzip(CommandLineArgs args)
        {
            string filePath = args.AsString("Path");
            string extractToPath = args.AsString("ExtractTo");
            filePath = Path.GetFullPath(filePath);
            SharpCompressUtils.Unzip(filePath, extractToPath);
        }

        public static void CmdDeleteBinObjSubFolders(CommandLineArgs args)
        {
            string filePath = args.AsString("Path");
            CommonProcs.DeleteBinObjFiles(filePath);
        }

        public static void CmdRemoveFolder(CommandLineArgs args)
        {
            string path = args.AsString("Path");

            Console.WriteLine($"RemoveFolder: [{path}]");
            try
            {
                CommonProcs.DeleteFilesByMasks(path, "*", Console.WriteLine);
                if(Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error removing folder: [{path}]");
                LogUtils.LogExceptionToFile(e);
            }
        }

        public static void CmdGenerateBindable(CommandLineArgs args)
        {
            string pathToSettings = args.AsString("Settings");
            pathToSettings = Path.GetFullPath(pathToSettings);

            Console.WriteLine($"Generate Bindable Properties...");
            Console.WriteLine($"Settings: {pathToSettings}");

            DebugUtils.DebugCallIf(false, GenerateBindableSettings.TestSave);

            if(!File.Exists(pathToSettings))
            {
                Console.WriteLine($"Settings file not found");
                return;
            }

            try
            {
                var settings = XmlUtils.DeserializeFromFile<GenerateBindableSettings>(pathToSettings);

                if (settings is null)
                {
                    Console.WriteLine($"Settings file reading error");
                    return;
                }

                foreach (var item in settings.Items)
                {
                    if (item.Ignore ?? false)
                        continue;

                    item.Prepare();

                    Console.WriteLine($"======================");

                    Console.WriteLine($"RootFolder: {item.RootFolder}");
                    Console.WriteLine($"PathToDll: {item.PathToDll}");
                    Console.WriteLine($"TypeName: {item.TypeName}");
                    Console.WriteLine($"PathToResult: {item.PathToResult}");
                    Console.WriteLine($"ResultTypeName: {item.ResultTypeName}");

                    if (!File.Exists(item.PathToDll))
                    {
                        Console.WriteLine($"File not exists: {item.PathToDll}");
                        continue;
                    }

                    var destFolder = Path.GetDirectoryName(item.PathToResult);

                    if (!Path.Exists(destFolder))
                    {
                        Console.WriteLine($"Path not exists: {destFolder}");
                        continue;
                    }

                    item.Execute(settings);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error generating bindable properties");
                Console.WriteLine($"Error info logged to file");
                LogUtils.LogExceptionToFile(e);
            }
        }

        public static void CmdDeleteFilesByMasks(CommandLineArgs args)
        {
            string path = args.AsString("Path");
            string masks = args.AsString("Masks");
            CommonProcs.DeleteFilesByMasks(path, masks, Console.WriteLine);
        }

        public static void CmdDeleteBinFolders(CommandLineArgs args)
        {
            string path = Path.Combine(
                CommonUtils.GetAppFolder(), "..", "..", "..", "..", "..");
            CommonProcs.DeleteBinObjFiles(path);
        }

        public static string GetOSArchitectureAsString(bool x64)
        {
            if (App.IsWindowsOS)
            {
                if (App.IsArmOS)
                    return "win-arm64";
                else
                {
                    if (x64)
                        return "win-x64";
                    else
                        return "win-x86";
                }
            }

            if (App.IsMacOS)
            {
                if(App.IsArmOS)
                    return "maccatalyst-arm64";
                else
                    return "maccatalyst-x64";
            }

            if (App.IsLinuxOS)
            {
                if (App.IsArmOS)
                    return "linux-arm64";
                else
                {
                    return "linux-x64";
                }
            }

            return "other";
        }

        public static string? ReplaceParamsInArchivePath(string? pathToArch)
        {
            pathToArch = ReplaceInFilesSetting.ReplaceParam(
                pathToArch,
                "OSArchitecture",
                GetOSArchitectureAsString(true));
            pathToArch = ReplaceInFilesSetting.ReplaceParam(
                pathToArch,
                "OSArchitectureX86",
                GetOSArchitectureAsString(false));
            pathToArch = ReplaceInFilesSetting.ReplaceParam(
                pathToArch,
                "UIVersion",
                AppUtils.GetUIVersion());

            return pathToArch;
        }

        public static void CmdZipFile(CommandLineArgs args)
        {
            string pathToFile = args.AsString("File");
            var pathToArch = args.AsString("Result");
            var otherExtensions = args.AsString("Ext");

            pathToArch = ReplaceParamsInArchivePath(pathToArch);

            var compressionType = CompressionType.Deflate;

            Console.WriteLine($"Command: zipFile");
            Console.WriteLine($"File: {pathToFile}");
            Console.WriteLine($"Other extensions: {otherExtensions}");
            Console.WriteLine($"Result: {pathToArch}");
            Console.WriteLine($"CompressionType: {compressionType}");

            pathToFile = Path.GetFullPath(pathToFile);
            pathToArch = Path.GetFullPath(pathToArch ?? string.Empty);

            if (!File.Exists(pathToFile))
            {
                Console.WriteLine($"File doesn't exist: [{pathToFile}]");
                return;
            }

            FileUtils.CreateFilePath(pathToArch);
            FileUtils.DeleteIfExists(pathToArch);

            SharpCompressUtils.ZipFile(
                pathToFile,
                pathToArch,
                compressionType,
                otherExtensions);

            Console.WriteLine("Completed");
        }

        public static void CmdDarkImages(CommandLineArgs args)
        {
            string pathToFolder = args.AsString("Path");
            string pathToResult = args.AsString("Result");

            if (string.IsNullOrWhiteSpace(pathToResult))
                pathToResult = pathToFolder;

            Console.WriteLine($"Command: lightImages");
            Console.WriteLine($"Folder: {pathToFolder}");
            Console.WriteLine($"Result: {pathToResult}");

            pathToFolder = Path.GetFullPath(pathToFolder);
            pathToResult = Path.GetFullPath(pathToResult);

            if (!Directory.Exists(pathToFolder))
            {
                Console.WriteLine($"Folder doesn't exist: [{pathToFolder}]");
                return;
            }

            if (!Directory.Exists(pathToResult))
            {
                Console.WriteLine($"Output folder doesn't exist: [{pathToResult}]");
                return;
            }

            pathToFolder = PathUtils.AddDirectorySeparatorChar(Path.GetFullPath(pathToFolder));

            var files = Directory.EnumerateFiles(
                pathToFolder,
                "*.png",
                SearchOption.TopDirectoryOnly);

            FileUtils.CreateFilePath(pathToResult);

            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var ext = Path.GetExtension(file);
                name += "_Dark"+ext;                

                var resultPath = Path.Combine(pathToResult, name);

                FileUtils.DeleteIfExists(resultPath);

                var image = new Bitmap(file);
                var converted = image.WithLightLightColors();
                converted.Save(resultPath);
            }

            Console.WriteLine("Completed");
        }

        public static void ExtractFromImageList()
        {
            const string basePath = @"E:\DIMA\AlternetStudio\NewImages\CodeExplorerImages\RoslynImages\";

            ExtractFromImageList(Path.Combine(basePath, @"RoslynDark16.png"));
            ExtractFromImageList(Path.Combine(basePath, @"RoslynDark32.png"));
            ExtractFromImageList(Path.Combine(basePath, @"RoslynLight16.png"));
            ExtractFromImageList(Path.Combine(basePath, @"RoslynLight32.png"));
        }

        public static void ExtractFromImageList(string pathToImageList)
        {
            try
            {
                var dir = Path.GetDirectoryName(pathToImageList);
                var onlyName = Path.GetFileNameWithoutExtension(pathToImageList);
                var resultDir = Path.Combine(dir ?? string.Empty, onlyName);
                var namesFile = Path.Combine(dir ?? string.Empty, "Names.txt");

                if (!Directory.Exists(resultDir))
                {
                    Directory.CreateDirectory(resultDir);
                }

                if (!File.Exists(pathToImageList))
                {
                    Console.WriteLine($"File not found: {pathToImageList}");
                    return;
                }

                var imageList = new ImageList(pathToImageList);

                if (File.Exists(namesFile))
                {
                    var str = FileUtils.StringFromFile(namesFile);
                    var splitted = StringUtils.Split(str);
                    imageList.ExportImagesToFiles(resultDir, splitted);
                }
                else
                {
                    var generatedNames = new List<string>();
                    for (int i = 0; i < imageList.Images.Count; i++)
                    {
                        var name = $"onlyName-{i}";
                        generatedNames.Add(name);
                    }

                    imageList.ExportImagesToFiles(resultDir, generatedNames.ToArray());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error extracting from image list");
                Console.WriteLine($"Error info logged to file");
                LogUtils.LogExceptionToFile(e);
            }
        }

        public static void CombineCodeExplorerImages()
        {
            const string basePath = @"E:\DIMA\AlternetStudio\NewImages\CodeExplorerImages\RoslynImages\";
            const string resultPath = @"E:\DIMA\AlternetStudio\NewImages\CodeExplorerImages\";
            const string svgPath = resultPath;

            var roslynDark16 = Path.Combine(basePath, @"RoslynDark16");
            var roslynDark32 = Path.Combine(basePath, @"RoslynDark32");
            var roslynLight16 = Path.Combine(basePath, @"RoslynLight16");
            var roslynLight32 = Path.Combine(basePath, @"RoslynLight32");

            var resultDark16 = Path.Combine(resultPath, "CodeExplorerImages.16.Dark.png");
            var resultLight16 = Path.Combine(resultPath, "CodeExplorerImages.16.png");
            var resultDark32 = Path.Combine(resultPath, "CodeExplorerImages.32.Dark.png");
            var resultLight32 = Path.Combine(resultPath, "CodeExplorerImages.32.png");

            var imageListDark16 = new ImageList(16);
            var imageListLight16 = new ImageList(16);
            var imageListDark32 = new ImageList(32);
            var imageListLight32 = new ImageList(32);

            void AddToImageList(ImageList images, string imageFolder, string imageName)
            {
                var path = Path.Combine(imageFolder, imageName);
                var image = new Bitmap(path);
                images.Add(image);
            }

            void AddImage(int index, string imageName)
            {
                AddToImageList(imageListDark16, roslynDark16, imageName);
                AddToImageList(imageListLight16, roslynLight16, imageName);
                AddToImageList(imageListDark32, roslynDark32, imageName);
                AddToImageList(imageListLight32, roslynLight32, imageName);
            }

            void AddSvgImage(int index, string svgName, LightDarkColor? color)
            {
                var path = Path.Combine(svgPath, svgName);
                var svgImage = new MonoSvgImage(path);

                imageListDark16.AddSvg(svgImage, color?.Dark);
                imageListLight16.AddSvg(svgImage, color?.Light);
                imageListDark32.AddSvg(svgImage, color?.Dark);
                imageListLight32.AddSvg(svgImage, color?.Light);
            }

            void AddColorSvgImage(int index, string svgName)
            {
                var path = Path.Combine(svgPath, svgName);
                var svgImage = new ColorSvgImage(path);

                imageListDark16.AddSvg(svgImage);
                imageListLight16.AddSvg(svgImage);
                imageListDark32.AddSvg(svgImage);
                imageListLight32.AddSvg(svgImage);
            }

            AddImage(0, "0-None.png"); //DotImage.txt
            AddImage(1, "21-EnumItem.png"); //EnumFieldImage-Blue.txt
            AddImage(2, "47-Namespace.png"); //NameSpaceImage-Std.txt
            AddImage(3, "1-Class.png"); //ClassImage-Yellow.txt
            AddImage(4, "6-Constant.png"); //ConstImage-Std.txt
            AddImage(5, "11-Delegate.png"); //DelegateImage-Purple.txt
            AddImage(6, "16-Enum.png"); //EnumImage-Yellow.txt
            AddImage(7, "26-Event.png"); //EventImage-Yellow.txt
            AddImage(8, "31-Field.png"); //FieldImage-Blue.txt
            AddImage(9, "36-Interface.png"); //InterfaceImage-Blue.txt
            AddImage(10, "42-Method.png"); //MethodImage-Purple.txt
            AddImage(11, "52-Property.png"); //PropImage-Std.svg
            AddImage(12, "57-Struct.png"); //StructImage-Blue.txt
            
            AddImage(13, "3-ClassPrivate.png"); //ClassImage-Private.txt
            AddImage(14, "8-ConstantPrivate.png"); //ConstImage-Private.txt
            AddImage(15, "13-DelegatePrivate.png"); //DelegateImage-Private.txt
            AddImage(16, "18-EnumPrivate.png"); //EnumImage-Private.txt
            AddImage(17, "28-EventPrivate.png"); //EventImage-Private.txt
            AddImage(18, "33-FieldPrivate.png"); //FieldImage-Private.txt
            AddImage(19, "38-InterfacePrivate.png"); //InterfaceImage-Private.txt
            AddImage(20, "44-MethodPrivate.png"); //MethodImage-Private.txt
            AddImage(21, "54-PropertyPrivate.png"); //PropImage-Private.txt
            AddImage(22, "59-StructPrivate.png"); //StructImage-Private.txt
            
            AddImage(23, "4-ClassProtected.png"); //ClassImage-Protected-Internal.txt
            AddImage(24, "9-ConstantProtected.png"); //ConstImage-Protected-Internal.txt
            AddImage(25, "14-DelegateProtected.png"); //DelegateImage-Protected-Internal.txt
            AddImage(26, "19-EnumProtected.png"); //EnumImage-Protected-Internal.txt
            AddImage(27, "29-EventProtected.png"); //EventImage-Protected-Internal.txt
            AddImage(28, "34-FieldProtected.png"); //FieldImage-Protected-Internal.txt
            AddImage(29, "39-InterfaceProtected.png"); //InterfaceImage-Protected-Internal.txt
            AddImage(30, "45-MethodProtected.png"); //MethodImage-Protected-Internal.txt
            AddImage(31, "55-PropertyProtected.png"); //PropImage-Protected-Internal.txt
            AddImage(32, "60-StructProtected.png"); //StructImage-Protected-Internal.txt
            
            AddImage(33, "1-Class.png"); //ClassImage-Yellow.txt
            AddImage(34, "6-Constant.png"); //ConstImage-Std.txt
            AddImage(35, "11-Delegate.png"); //DelegateImage-Purple.txt
            AddImage(36, "16-Enum.png"); //EnumImage-Yellow.txt
            AddImage(37, "26-Event.png"); //EventImage-Yellow.txt
            AddImage(38, "31-Field.png"); //FieldImage-Blue.txt
            AddImage(39, "36-Interface.png"); //InterfaceImage-Blue.txt
            AddImage(40, "42-Method.png"); //MethodImage-Purple.txt
            AddImage(41, "52-Property.png"); //PropImage-Std.svg
            AddImage(42, "57-Struct.png"); //StructImage-Blue.txt

            AddColorSvgImage(43, "43-ErrorImage-Red.svg"); //ErrorImage-Red.txt
            AddSvgImage(44, "44-CSharpImage-Green.svg", LightDarkColors.Green); //CSharpImage-Green.svg
            AddSvgImage(45, "45-VisualBasicImage-Blue.svg", LightDarkColors.Blue); //VisualBasicImage-Blue.svg            

            var stripDark16 = imageListDark16.AsSkiaStrip();
            var stripLight16 = imageListLight16.AsSkiaStrip();
            var stripDark32 = imageListDark32.AsSkiaStrip();
            var stripLight32 = imageListLight32.AsSkiaStrip();

            SkiaUtils.SaveBitmapToPng(stripDark16!, resultDark16);
            SkiaUtils.SaveBitmapToPng(stripLight16!, resultLight16);
            SkiaUtils.SaveBitmapToPng(stripDark32!, resultDark32);
            SkiaUtils.SaveBitmapToPng(stripLight32!, resultLight32);
        }

        public static void CmdSomeAction(CommandLineArgs args)
        {
            CombineCodeExplorerImages();
        }

        public static void CmdSvgToPng(CommandLineArgs args)
        {
            string pathToConfig = args.AsString("Config");

            Console.WriteLine($"Command: svgToPng");
            Console.WriteLine($"Config: {pathToConfig}");

            var fullPath = Path.GetFullPath(pathToConfig);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"File doesn't exist: [{fullPath}]");
                return;
            }

            SvgToPngUtils.ConvertSvgToPng(fullPath);
        }

        public static void CmdZipFolder(CommandLineArgs args)
        {
            string pathToFolder = args.AsString("Folder");
            string pathToArch = args.AsString("Result");
            string rootSubFolder = args.AsString("RootSubFolder");

            var compressionType = CompressionType.Deflate;

            Console.WriteLine($"Command: zipFolder");
            Console.WriteLine($"Folder: {pathToFolder}");
            Console.WriteLine($"Result: {pathToArch}");
            Console.WriteLine($"RootSubFolder: {rootSubFolder}");
            Console.WriteLine($"CompressionType: {compressionType}");

            pathToFolder = Path.GetFullPath(pathToFolder);
            pathToArch = Path.GetFullPath(pathToArch);

            if (!Directory.Exists(pathToFolder))
            {
                Console.WriteLine($"Folder doesn't exist: [{pathToFolder}]");
                return;
            }

            FileUtils.CreateFilePath(pathToArch);
            FileUtils.DeleteIfExists(pathToArch);

            SharpCompressUtils.ZipFolderWithRootSubFolder(
                pathToFolder,
                pathToArch,
                rootSubFolder,
                compressionType);

            Console.WriteLine("Completed");
        }

        public static void CmdReplaceInFiles(CommandLineArgs args)
        {
            var pathToConfig = args.AsString("Settings");
            var pathToConfigFull = Path.GetFullPath(pathToConfig);
            var configNames = args.AsString("Names");

            Console.WriteLine($"Command: replaceInFiles");
            Console.WriteLine($"Path to config: {pathToConfigFull}");
            Console.WriteLine($"Config names: {configNames}");

            configNames = $";{configNames};";

            if (!File.Exists(pathToConfigFull))
            {
                Console.WriteLine($"Settings file doesn't exist: [{pathToConfigFull}]");
                return;
            }

            try
            {
                var settings = XmlUtils.DeserializeFromFile<ReplaceInFilesSettings>(pathToConfigFull);

                if (settings is null)
                {
                    Console.WriteLine($"Settings file reading error");
                    return;
                }

                foreach (var item in settings.Items)
                {
                    if (!configNames.Contains($";{item.Name};"))
                        continue;

                    item.Prepare(settings, pathToConfigFull);

                    Console.WriteLine($"======================");
                    Console.WriteLine($"Name: {item.Name}");
                    Console.WriteLine($"PathToFile: {item.PathToFile}");
                    Console.WriteLine($"PathToResultFile: {item.PathToResultFile}");

                    if (!File.Exists(item.PathToFile))
                    {
                        Console.WriteLine($"File not exists: {item.PathToFile}");
                        continue;
                    }

                    var destFolder = Path.GetDirectoryName(item.PathToResultFile);

                    if (!Path.Exists(destFolder))
                    {
                        Console.WriteLine($"Path not exists: {destFolder}");
                        continue;
                    }

                    item.Execute(settings);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error replace in files");
                Console.WriteLine($"Error info logged to file");
                LogUtils.LogExceptionToFile(e);
            }
        }

        public static void CmdFilterLog(CommandLineArgs args)
        {
            string logFilter = args.AsString("Filter").ToLower();
            string logPath = args.AsString("Log");
            string resultPath = args.AsString("Result");
            logPath = Path.GetFullPath(logPath);
            resultPath = Path.GetFullPath(resultPath);

            Console.WriteLine($"Command: filterLog");
            Console.WriteLine($"logPath: {logPath}");
            Console.WriteLine($"resultPath: {resultPath}");
            Console.WriteLine($"logFilter: {logFilter}");

            IEnumerable<string> lines = File.ReadLines(logPath);

            string contents = string.Empty;

            foreach (string s in lines)
            {
                if (s.Contains(logFilter, StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine(s);
                    contents += $"{s}{Environment.NewLine}";
                }
            }

            File.WriteAllText(resultPath, contents);
        }

        public static bool RunCommand(string? originalName, CommandLineArgs args)
        {
            if (originalName is null || string.IsNullOrWhiteSpace(originalName))
                return false;

            var name = originalName.ToLower().Trim().Trim('"','\'');

            if(commands.TryGetValue(name, out var action))
            {
                if (action is null)
                    return false;
                try
                {
                    action(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error running command: {originalName}");
                    LogUtils.LogException(e);
                    LogUtils.LogExceptionToConsole(e);
                    return false;                    
                }
                return true;
            }

            return false;
        }

        public static void RegisterCommand(
            string name,
            Action<CommandLineArgs> action,
            string? example = null)
        {
            originalCommandNames.Add(name);
            commands.Add(name.ToLower(), action);
        }

        public static void RegisterCommands(bool adv)
        {
            if (adv)
            {
                RegisterCommand(
                    "darkImages",
                    CmdDarkImages,
                    "-r=darkImages Path=\"d:\\Images\" Result=\"d:\\ImagesConverted\"");

                RegisterCommand(
                    "svgToPng",
                    CmdSvgToPng,
                    "-r=svgToPng Config=\"d:\\svg2png.xml\"");

                RegisterCommand(
                    "someAction",
                    CmdSomeAction,
                    "");
            }

            RegisterCommand(
                "replaceInFiles",
                CmdReplaceInFiles,
                "");

            RegisterCommand(
                "del",
                CmdDeleteFilesByMasks,
                "");

            RegisterCommand(
                "rmdir",
                CmdRemoveFolder,
                "");

            RegisterCommand(
                "zipFolder",
                CmdZipFolder,
                "-r=zipFolder Folder=\"d:\\testFolder\" Result=\"d:\\result.zip\"");

            RegisterCommand(
                "zipFile",
                CmdZipFile,
                "-r=zipFile File=\"d:\\testFile.txt\" Result=\"d:\\result.zip\"");

            RegisterCommand(
                "unzip",
                CmdUnzip,
                "-r=unzip Path=\"e:/file.zip\" ExtractTo=\"e:/ExtractedZip\"");

            RegisterCommand(
                "runControlsSample",
                CmdRunControlsSample,
                "");

            RegisterCommand(
                "waitAnyKey",
                CmdWaitAnyKey,
                "");

            RegisterCommand(
                "waitEnter",
                CmdWaitEnter,
                "");

            RegisterCommand(
                "deleteBinFolders",
                CmdDeleteBinFolders,
                "");

            RegisterCommand(
                "generateBindable",
                CmdGenerateBindable,
                "-r=generateBindable Settings=\"E:\\GenerateBindableSettings.xml\"");

            RegisterCommand(
                "deleteBinObjSubFolders",
                CmdDeleteBinObjSubFolders,
                "-r=deleteBinObjSubFolders Path=\"E:\\SomeFolder\"");

            RegisterCommand(
                "filterLog",
                CmdFilterLog,
                "");

            RegisterCommand(
                "download",
                CmdDownload,
                "-r=download Url=\"http://localhost/file.7z\" Path=\"e:/file.7z\"");

            RegisterCommand(
                "downloadAndUnzip",
                CmdDownloadAndUnzip,
                "-r=downloadAndUnzip Url=\"http://localhost/file.7z\" Path=\"e:/file.7z\" ExtractTo=\"e:/Extracted7z\"");

            originalCommandNames.Sort();
        }
    }
}