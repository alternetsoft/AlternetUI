using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="VirtualListBox"/> descendant which allows to browse folder contents.
    /// </summary>
    public partial class FileListBox : VirtualTreeControl
    {
        /// <summary>
        /// Gets or sets global <see cref="FolderInfoItem"/> for the file or folder.
        /// <see cref="FolderInfoItem"/> allows to specify icon, custom title
        /// and some other information.
        /// </summary>
        public static readonly Dictionary<string, FolderInfoItem>? FolderInfo;

        /// <summary>
        /// Gets or sets list of special folder which are hidden.
        /// </summary>
        public static List<Environment.SpecialFolder>? HiddenSpecialFolders;

        /// <summary>
        /// Gets or sets list of visible special folders. If specified, only these
        /// folders will be visible on the root level.
        /// </summary>
        public static List<Environment.SpecialFolder>? VisibleSpecialFolders;

        /// <summary>
        /// Gets or sets list of additional special folders. If specified, these
        /// folders will be added to the list of root level folders.
        /// </summary>
        public static List<NewItemInfo>? AdditionalSpecialFolders;

        /// <summary>
        /// Gets or sets whether to add all drives (c:\, d:\, etc.) to
        /// the list of the special folders.
        /// </summary>
        public static bool AddDrivesToRootFolder = true;

        /// <summary>
        /// Gets or sets template for the drive item when it is added to the
        /// list of the special folders.
        /// Default is "{0}".
        /// </summary>
        public static string DriveItemTemplate = "{0}";

        /// <summary>
        /// Gets or sets a value indicating whether to use solid default images
        /// for folder items when <see cref="DefaultFolderImage"/> not specified.
        /// </summary>
        public static bool UseSolidFolderDefaultImages = true;

        /// <summary>
        /// Gets or sets a value indicating whether to use solid default images
        /// for file items when <see cref="DefaultFileImage"/> not specified.
        /// </summary>
        public static bool UseSolidFileDefaultImages = false;

        /// <summary>
        /// Gets or sets default svg image used for the "file" items.
        /// </summary>
        public static SvgImage? DefaultFileImage;

        /// <summary>
        /// Gets or sets default svg image used for the "folder" items.
        /// </summary>
        public static SvgImage? DefaultFolderImage;

        /// <summary>
        /// Gets or sets a function that provides a color override for file icons.
        /// This function returns a <see cref="LightDarkColor"/> object, which specifies
        /// the color to be used for file icons in light and dark themes.
        /// </summary>
        public static Func<LightDarkColor?>? FileImageColorOverride = () =>
        {
            return null;
        };

        /// <summary>
        /// Gets or sets a function that provides a color override for folder icons.
        /// This function returns a <see cref="LightDarkColor"/> object, which specifies
        /// the color to be used for file icons in light and dark themes.
        /// </summary>
        public static Func<LightDarkColor?>? FolderImageColorOverride = () =>
        {
            return LightDarkColors.Yellow;
        };

        private string? selectedFolder;
        private string searchPattern = "*";
        private int reloading;
        private bool sorted = true;

        static FileListBox()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public FileListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileListBox"/> class.
        /// </summary>
        public FileListBox()
        {
        }

        /// <summary>
        /// Occurs when root folder items are added to the control.
        /// This event is called when <see cref="SelectedFolder"/> is assigned
        /// with <c>null</c> value.
        /// </summary>
        public event EventHandler? AddRootFolder;

        /// <summary>
        /// Occurs when the <see cref="SelectedFolder"/> property value changes.
        /// This event is raised after the folder selection has been updated.
        /// </summary>
        public event EventHandler? SelectedFolderChanged;

        /// <summary>
        /// Gets or sets whether folder and file names are sorted.
        /// </summary>
        public virtual bool Sorted
        {
            get => sorted;
            set
            {
                if (sorted == value)
                    return;
                sorted = value;
                Reload();
            }
        }

        /// <summary>
        /// Gets or sets whether to add ".." item which allows to open parent folder.
        /// </summary>
        public virtual bool AddUpperFolderItem { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to add "/" item which allows to open root folder.
        /// </summary>
        public virtual bool AddRootFolderItem { get; set; } = true;

        /// <summary>
        /// Gets or sets whether double click on folder opens its content in the control.
        /// </summary>
        public virtual bool AllowGoToSubFolder { get; set; } = true;

        /// <summary>
        /// Gets or sets search pattern which allows to limit files shown in the control.
        /// An example: "*.uixml". Default value is "*" (all files).
        /// </summary>
        public virtual string SearchPattern
        {
            get
            {
                return searchPattern;
            }

            set
            {
                if (searchPattern == value)
                    return;
                searchPattern = value;
                Reload();
            }
        }

        /// <summary>
        /// Gets or sets folder which is currently opened in the control.
        /// </summary>
        public virtual string? SelectedFolder
        {
            get => selectedFolder;

            set
            {
                if (selectedFolder == value && value is not null)
                    return;
                App.LogIf($"FileListBox.SelectedFolder = {value}", false);
                var oldSelectedFolder = selectedFolder;
                selectedFolder = value;
                try
                {
                    Reload();
                }
                catch (Exception e)
                {
                    LogUtils.LogException(e);

                    try
                    {
                        selectedFolder = oldSelectedFolder;
                        Reload();
                    }
                    catch
                    {
                        selectedFolder = null;
                        Reload();
                    }
                }

                SelectedFolderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets whether selected item is file.
        /// </summary>
        [Browsable(false)]
        public virtual bool SelectedItemIsFile => ItemIsFile(SelectedItem);

        /// <summary>
        /// Gets path of the selected item. When this property is changed,
        /// selected item is updated to the item with the specified path.
        /// </summary>
        [Browsable(false)]
        public virtual string? SelectedItemPath
        {
            get => SelectedItem?.Path;

            set
            {
                if (SelectedItemPath == value)
                    return;
                if(value is null)
                {
                    SelectedItem = null;
                    return;
                }

                foreach(var item in VisibleItems)
                {
                    if (item is not FileListBoxItem item2)
                        continue;
                    if(item2.Path == value)
                    {
                        SelectedItem = item2;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets item which is currently selected.
        /// </summary>
        [Browsable(false)]
        public new FileListBoxItem? SelectedItem
        {
            get => (FileListBoxItem?)base.SelectedItem;
            set => base.SelectedItem = value;
        }

        /// <summary>
        /// Gets whether control is reloading and selection change events
        /// should be ignored.
        /// </summary>
        [Browsable(false)]
        public bool IsReloading { get => reloading > 0; }

        /// <summary>
        /// Gets whether item is folder.
        /// </summary>
        public virtual bool ItemIsFolder(FileListBoxItem? item)
        {
            var result = !string.IsNullOrEmpty(item?.Path);
            if (result)
                result = Directory.Exists(item!.Path);
            return result;
        }

        /// <summary>
        /// Gets whether item is file.
        /// </summary>
        public virtual bool ItemIsFile(FileListBoxItem? item)
        {
            var result = !string.IsNullOrEmpty(item?.Path);
            if (result)
                result = File.Exists(item!.Path);
            return result;
        }

        /// <summary>
        /// Reloads contents of the currently opened folder.
        /// </summary>
        public virtual void Reload()
        {
            SelectedItem = null;
            ListBox.EnsureVisible(0);

            try
            {
                reloading++;

                App.AddIdleTask(() =>
                {
                    if (IsDisposed)
                        return;
                    reloading--;
                    SelectedItem = null;
                    ListBox.EnsureVisible(0);
                });

                DoInsideUpdate(ReloadFn);
            }
            finally
            {
            }

            void ReloadFn()
            {
                this.Clear();

                if (selectedFolder is null)
                {
                    AddSpecialFolders();
                    return;
                }

                if (AddRootFolderItem)
                {
                    var rootFolderItem = new FileListBoxItem("/", null);
                    rootFolderItem.DoubleClickAction = NavigateToRootFolder;
                    rootFolderItem.SvgImage = GetFolderImage();
                    Add(rootFolderItem);
                }

                if (AddUpperFolderItem)
                {
                    var upperFolderItem = new FileListBoxItem("..", null);
                    upperFolderItem.DoubleClickAction = NavigateToParentFolder;
                    upperFolderItem.SvgImage = GetFolderImage();
                    Add(upperFolderItem);
                }

                var dirs = GetFileSystem().GetDirectories(selectedFolder);
                if(Sorted)
                    Array.Sort(dirs, PathUtils.CompareByFileName);

                foreach (var dir in dirs)
                {
                    AddFolder(null, dir);
                }

                var files = GetFileSystem().GetFiles(selectedFolder, searchPattern);
                if (Sorted)
                    Array.Sort(files, PathUtils.CompareByFileName);

                foreach (var file in files)
                {
                    AddFile(null, file);
                }
            }
        }

        /// <summary>
        /// Selects a folder by file name or folder path.
        /// </summary>
        /// <param name="fileNameOrFolder">The file name or folder path to select.</param>
        /// <returns>
        /// <c>true</c> if the folder or file exists and the folder was successfully selected;
        /// otherwise, <c>false</c>.
        /// </returns>
        public virtual bool SelectFolderByFileName(string? fileNameOrFolder)
        {
            if (fileNameOrFolder is null)
                return false;

            var isFile = GetFileSystem().FileExists(fileNameOrFolder);
            var isFolder = !isFile && GetFileSystem().DirectoryExists(fileNameOrFolder);

            if (isFile || isFolder)
            {
                var path = isFolder ? fileNameOrFolder : Path.GetDirectoryName(fileNameOrFolder);
                SelectedFolder = path;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the default SVG image used for file items.
        /// If <see cref="DefaultFileImage"/> is not set, a known default file icon is returned.
        /// </summary>
        /// <returns>The SVG image representing a file icon.</returns>
        public virtual SvgImage? GetFileImage()
        {
            var result = DefaultFileImage
                ?? (UseSolidFileDefaultImages
                ? KnownSvgImages.ImgIconFileSolid : KnownSvgImages.ImgIconFile);
            result.SetColorOverride(KnownSvgColor.Normal, FileImageColorOverride?.Invoke());
            return result;
        }

        /// <summary>
        /// Gets the default SVG image used for folder items.
        /// If <see cref="DefaultFolderImage"/> is not set, a known default folder icon is returned.
        /// </summary>
        /// <returns>The SVG image representing a folder icon.</returns>
        public virtual SvgImage? GetFolderImage()
        {
            var result = DefaultFolderImage
                ?? (UseSolidFolderDefaultImages
                ? KnownSvgImages.ImgIconFolderSolid : KnownSvgImages.ImgIconFolder);
            result.SetColorOverride(KnownSvgColor.Normal, FolderImageColorOverride?.Invoke());
            return result;
        }

        /// <summary>
        /// Adds file to the control.
        /// </summary>
        /// <param name="title">Optional title of the file.</param>
        /// <param name="path">Path to file.</param>
        /// <param name="image">File image. If not specified, default image will be used.</param>
        /// <returns></returns>
        public virtual bool AddFile(string? title, string path, SvgImage? image = null)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            image ??= GetFileImage();
            title ??= Path.GetFileName(path);
            var item = new FileListBoxItem(title, path);
            item.SvgImage = image;
            Add(item);
            return true;
        }

        /// <summary>
        /// Adds folder to the control.
        /// </summary>
        /// <param name="item">Folder information.</param>
        /// <returns></returns>
        public virtual bool AddFolder(NewItemInfo item)
        {
            return AddFolder(item.Title, item.Path, item.Image);
        }

        /// <summary>
        /// Adds file to the control.
        /// </summary>
        /// <param name="item">File information.</param>
        /// <returns></returns>
        public virtual bool AddFile(NewItemInfo item)
        {
            return AddFile(item.Title, item.Path, item.Image);
        }

        /// <summary>
        /// Adds folder to the control.
        /// </summary>
        /// <param name="title">Optional title of the folder.</param>
        /// <param name="path">Path to folder.</param>
        /// <param name="image">Folder image. If not specified, default image will be used.</param>
        /// <returns></returns>
        public virtual bool AddFolder(string? title, string path, SvgImage? image = null)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            image ??= GetFolderImage();
            title ??= Path.GetFileName(path);
            var item = new FileListBoxItem(title, path);
            item.DoubleClickAction = FolderAction;
            item.SvgImage = image;

            Add(item);

            return true;

            void FolderAction()
            {
                if (AllowGoToSubFolder)
                    SelectedFolder = item.Path;
            }
        }

        /// <summary>
        /// Adds special folder specified using <see cref="Environment.SpecialFolder"/>
        /// enumeration.
        /// </summary>
        /// <param name="folder">Special folder kind.</param>
        /// <param name="defaultTitle">Default folder title.</param>
        /// <returns></returns>
        public virtual bool AddSpecialFolder(
            Environment.SpecialFolder folder,
            string? defaultTitle = null)
        {
            try
            {
                if(HiddenSpecialFolders is not null)
                {
                    if (HiddenSpecialFolders.IndexOf(folder) >= 0)
                        return false;
                }

                var path = Environment.GetFolderPath(folder);

                string? title = defaultTitle;
                SvgImage? image = null;

                if(FolderInfo is not null)
                {
                    if (FolderInfo.TryGetValue(path, out var folderInfo))
                    {
                        title = folderInfo?.Title;
                        image = folderInfo?.Image;
                    }
                }

                title ??= folder.ToString();

                return AddFolder(title, path, image);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Adds all special folders to the control.
        /// <see cref="VisibleSpecialFolders"/> and <see cref="HiddenSpecialFolders"/>
        /// can be used to specify what folders are added.
        /// </summary>
        public virtual void AddSpecialFolders()
        {
            if(AddRootFolder is not null)
            {
                AddRootFolder(this, EventArgs.Empty);
                return;
            }

            if (VisibleSpecialFolders is not null)
            {
                foreach(var item in VisibleSpecialFolders)
                    AddSpecialFolder(item);
                return;
            }

            AddSpecialFolder(Environment.SpecialFolder.Desktop);
            AddSpecialFolder(Environment.SpecialFolder.MyComputer, "My Computer");
            AddSpecialFolder(Environment.SpecialFolder.MyDocuments, "Documents");
            AddSpecialFolder(Environment.SpecialFolder.MyMusic, "Music");
            AddSpecialFolder(Environment.SpecialFolder.MyPictures, "Pictures");
            AddSpecialFolder(Environment.SpecialFolder.MyVideos, "Videos");

            AddSpecialFolder(Environment.SpecialFolder.Favorites);
            AddSpecialFolder(Environment.SpecialFolder.Programs);
            AddSpecialFolder(Environment.SpecialFolder.Recent);

            if (AdditionalSpecialFolders is not null)
            {
                foreach (var item in AdditionalSpecialFolders)
                    AddFolder(item);
            }

            if (AddDrivesToRootFolder)
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                List<NewItemInfo> drivesList = new();

                foreach (DriveInfo d in allDrives)
                {
                    if (!Directory.Exists(d.Name))
                        continue;
                    var template = string.Format(DriveItemTemplate, d.Name);
                    drivesList.Add(new(d.Name, template));
                }

                drivesList.Sort(Comparison);

                int Comparison(NewItemInfo x, NewItemInfo y)
                {
                    return string.Compare(x.Title, y.Title, false);
                }

                foreach (var item in drivesList)
                {
                    AddFolder(item);
                }
            }
        }

        /// <summary>
        /// Navigates to the root folder.
        /// By default, this method sets the <see cref="SelectedFolder"/> property to <c>null</c>,
        /// effectively navigating to the root folder. This method is called when root folder item
        /// is double-clicked.
        /// </summary>
        protected virtual void NavigateToRootFolder()
        {
            SelectedFolder = null;
        }

        /// <summary>
        /// Navigates to the parent folder.
        /// If the parent folder is the same as the current folder, it navigates to the root folder.
        /// If an exception occurs, it logs the exception and resets the selected folder to null.
        /// This method is called when parent folder item is double-clicked.
        /// </summary>
        protected virtual void NavigateToParentFolder()
        {
            try
            {
                var upperFolder = Path.Combine(selectedFolder, "..");
                var fullFolder = Path.GetFullPath(upperFolder);
                if (SelectedFolder == fullFolder)
                    NavigateToRootFolder();
                else
                    SelectedFolder = fullFolder;
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                SelectedFolder = null;
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.KeyData == Keys.F5)
            {
                e.Handled = true;
                Reload();
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Impements item of the <see cref="FileListBox"/> control.
        /// </summary>
        public class FileListBoxItem : TreeControlItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FileListBoxItem"/> class.
            /// </summary>
            /// <param name="title">Title of the item.</param>
            /// <param name="path">Path to file or folder.</param>
            /// <param name="doubleClick">Double click action.</param>
            public FileListBoxItem(string? title, string? path, Action? doubleClick = null)
            {
                Text = title ?? System.IO.Path.GetFileName(path) ?? string.Empty;
                Path = path;
                DoubleClickAction = doubleClick;
            }

            /// <summary>
            /// Gets or sets path to the file.
            /// </summary>
            public string? Path { get; set; }

            /// <summary>
            /// Gets extension in the lower case and without "." character.
            /// </summary>
            [Browsable(false)]
            public string ExtensionLower => PathUtils.GetExtensionLower(Path);
        }

        /// <summary>
        /// Contains properties which allow to specify custom image and title
        /// for the file. Used in <see cref="FolderInfo"/> dictionary.
        /// </summary>
        public class FolderInfoItem
        {
            /// <summary>
            /// Gets or sets custom image.
            /// </summary>
            public SvgImage? Image;

            /// <summary>
            /// Gets or sets custom title.
            /// </summary>
            public string? Title;
        }

        /// <summary>
        /// Contains properties which allow to specify information useful when new item is added.
        /// </summary>
        public class NewItemInfo
        {
            /// <summary>
            /// Gets or sets custom title.
            /// </summary>
            public string? Title;

            /// <summary>
            /// Gets or sets path to the file or folder.
            /// </summary>
            public string Path;

            /// <summary>
            /// Gets or sets custom image.
            /// </summary>
            public SvgImage? Image;

            /// <summary>
            /// Initializes a new instance of the <see cref="NewItemInfo"/> class.
            /// </summary>
            /// <param name="path">Path to the file or folder.</param>
            /// <param name="title">Custom title.</param>
            /// <param name="image">Custom image.</param>
            public NewItemInfo(string path, string? title = null, SvgImage? image = null)
            {
                Path = path;
                Title = title;
                Image = image;
            }
        }
    }
}
