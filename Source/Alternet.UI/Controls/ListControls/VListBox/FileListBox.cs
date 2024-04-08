using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="VListBox"/> descendant which allows to browse folder contents.
    /// </summary>
    public class FileListBox : VListBox
    {
        /// <summary>
        /// Gets or sets list of special folder which are hidden.
        /// </summary>
        public static readonly List<Environment.SpecialFolder>? HiddenSpecialFolders;

        /// <summary>
        /// Gets or sets list of visible special folders. If specified, only these
        /// folders will be visible on the root level.
        /// </summary>
        public static readonly List<Environment.SpecialFolder>? VisibleSpecialFolders;

        /// <summary>
        /// Gets or sets global <see cref="FolderInfoItem"/> for the file or folder.
        /// <see cref="FolderInfoItem"/> allows to specify icon, custom title
        /// and some other information.
        /// </summary>
        public static readonly Dictionary<string, FolderInfoItem>? FolderInfo;

        /// <summary>
        /// Gets or sets default svg image used for the "file" items.
        /// </summary>
        public static SvgImage? DefaultFileImage = new MonoSvgImage(KnownSvgUrls.UrlIconFile);

        /// <summary>
        /// Gets or sets default svg image used for the "folder" items.
        /// </summary>
        public static SvgImage? DefaultFolderImage = new MonoSvgImage(KnownSvgUrls.UrlIconFolder);

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
        /// Gets or sets whether foder and file names are sorted.
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
        /// Gets or sets whether double click on folder opens it's content in the control.
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
                if (selectedFolder == value)
                    return;
                Application.LogIf($"FileListBox.SelectedFolder = {value}", false);
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

                foreach(var item in Items)
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
        /// should be ingored.
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
            EnsureVisible(0);

            try
            {
                reloading++;

                Application.AddIdleTask(() =>
                {
                    if (IsDisposed)
                        return;
                    reloading--;
                    SelectedItem = null;
                    EnsureVisible(0);
                });

                DoInsideUpdate(ReloadFn);
            }
            finally
            {
            }

            void ReloadFn()
            {
                RemoveAll();

                if (selectedFolder is null)
                {
                    AddSpecialFolders();
                    return;
                }

                if (AddRootFolderItem)
                {
                    var rootFolderItem = new FileListBoxItem("/", null);
                    rootFolderItem.DoubleClickAction = RootFolderAction;
                    rootFolderItem.SvgImage = DefaultFolderImage;
                    Add(rootFolderItem);
                }

                if (AddUpperFolderItem)
                {
                    var upperFolderItem = new FileListBoxItem("..", null);
                    upperFolderItem.DoubleClickAction = UpperFolderAction;
                    upperFolderItem.SvgImage = DefaultFolderImage;
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

            void RootFolderAction()
            {
                SelectedFolder = null;
            }

            void UpperFolderAction()
            {
                try
                {
                    var upperFolder = Path.Combine(selectedFolder, "..");
                    var fullFolder = Path.GetFullPath(upperFolder);
                    SelectedFolder = fullFolder;
                }
                catch (Exception e)
                {
                    LogUtils.LogException(e);
                    SelectedFolder = null;
                }
            }
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
            image ??= DefaultFileImage;
            title ??= Path.GetFileName(path);
            var item = new FileListBoxItem(title, path);
            item.SvgImage = image;
            Add(item);
            return true;
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
            image ??= DefaultFolderImage;
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
        /// <returns></returns>
        public virtual bool AddSpecialFolder(Environment.SpecialFolder folder)
        {
            try
            {
                if(HiddenSpecialFolders is not null)
                {
                    if (HiddenSpecialFolders.IndexOf(folder) >= 0)
                        return false;
                }

                var path = Environment.GetFolderPath(folder);

                string? title = null;
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
            }

            AddSpecialFolder(Environment.SpecialFolder.Desktop);
            AddSpecialFolder(Environment.SpecialFolder.MyComputer);
            AddSpecialFolder(Environment.SpecialFolder.MyDocuments);
            AddSpecialFolder(Environment.SpecialFolder.MyMusic);
            AddSpecialFolder(Environment.SpecialFolder.MyPictures);
            AddSpecialFolder(Environment.SpecialFolder.MyVideos);

            AddSpecialFolder(Environment.SpecialFolder.ApplicationData);
            AddSpecialFolder(Environment.SpecialFolder.CommonApplicationData);
            AddSpecialFolder(Environment.SpecialFolder.LocalApplicationData);
            AddSpecialFolder(Environment.SpecialFolder.Cookies);
            AddSpecialFolder(Environment.SpecialFolder.Favorites);
            AddSpecialFolder(Environment.SpecialFolder.History);
            AddSpecialFolder(Environment.SpecialFolder.InternetCache);
            AddSpecialFolder(Environment.SpecialFolder.Programs);
            AddSpecialFolder(Environment.SpecialFolder.Recent);
            AddSpecialFolder(Environment.SpecialFolder.SendTo);
            AddSpecialFolder(Environment.SpecialFolder.StartMenu);
            AddSpecialFolder(Environment.SpecialFolder.Startup);
            AddSpecialFolder(Environment.SpecialFolder.System);
            AddSpecialFolder(Environment.SpecialFolder.Templates);
            AddSpecialFolder(Environment.SpecialFolder.DesktopDirectory);
            AddSpecialFolder(Environment.SpecialFolder.Personal);
            AddSpecialFolder(Environment.SpecialFolder.ProgramFiles);
            AddSpecialFolder(Environment.SpecialFolder.CommonProgramFiles);
            AddSpecialFolder(Environment.SpecialFolder.AdminTools);
            AddSpecialFolder(Environment.SpecialFolder.CDBurning);
            AddSpecialFolder(Environment.SpecialFolder.CommonAdminTools);
            AddSpecialFolder(Environment.SpecialFolder.CommonDocuments);
            AddSpecialFolder(Environment.SpecialFolder.CommonMusic);
            AddSpecialFolder(Environment.SpecialFolder.CommonOemLinks);
            AddSpecialFolder(Environment.SpecialFolder.CommonPictures);
            AddSpecialFolder(Environment.SpecialFolder.CommonStartMenu);
            AddSpecialFolder(Environment.SpecialFolder.CommonPrograms);
            AddSpecialFolder(Environment.SpecialFolder.CommonStartup);
            AddSpecialFolder(Environment.SpecialFolder.CommonDesktopDirectory);
            AddSpecialFolder(Environment.SpecialFolder.CommonTemplates);
            AddSpecialFolder(Environment.SpecialFolder.CommonVideos);
            AddSpecialFolder(Environment.SpecialFolder.Fonts);
            AddSpecialFolder(Environment.SpecialFolder.NetworkShortcuts);
            AddSpecialFolder(Environment.SpecialFolder.PrinterShortcuts);
            AddSpecialFolder(Environment.SpecialFolder.UserProfile);
            AddSpecialFolder(Environment.SpecialFolder.Resources);
            AddSpecialFolder(Environment.SpecialFolder.LocalizedResources);

            if (Application.IsWindowsOS)
            {
                AddSpecialFolder(Environment.SpecialFolder.CommonProgramFilesX86);
                AddSpecialFolder(Environment.SpecialFolder.ProgramFilesX86);
                AddSpecialFolder(Environment.SpecialFolder.SystemX86);
                AddSpecialFolder(Environment.SpecialFolder.Windows);
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
        public class FileListBoxItem : ListControlItem
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
            /// Gets or sets custom image for the file.
            /// </summary>
            public SvgImage? Image;

            /// <summary>
            /// Gets or sets custom title.
            /// </summary>
            public string? Title;
        }
    }
}
