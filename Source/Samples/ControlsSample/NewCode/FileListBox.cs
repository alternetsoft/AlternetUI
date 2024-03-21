using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class FileListBox : VListBox
    {
        public static SvgImage? DefaultFileImage = new MonoSvgImage(KnownSvgUrls.UrlIconFile);

        public static SvgImage? DefaultFolderImage = new MonoSvgImage(KnownSvgUrls.UrlIconFolder);

        public static readonly List<Environment.SpecialFolder>? HiddenSpecialFolders;

        public static readonly List<Environment.SpecialFolder>? VisibleSpecialFolders;

        public static readonly Dictionary<string, FolderInfoItem>? FolderInfo;

        private string? selectedFolder;
        private string searchPattern = "*";

        static FileListBox()
        {

        }

        public FileListBox()
        {
        }

        public bool AddUpperFolderItem { get; set; } = true;

        public bool AddRootFolderItem { get; set; } = true;

        public string SearchPattern
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

        public string? SelectedFolder
        {
            get => selectedFolder;

            set
            {
                if (selectedFolder == value)
                    return;
                Application.LogIf($"FileListBox.SelectedFolder = {value}", true);
                var oldSelectedFolder = selectedFolder;
                selectedFolder = value;
                try
                {
                    Reload(selectedFolder);
                }
                catch (Exception e)
                {
                    LogUtils.LogException(e);

                    try
                    {
                        SelectedFolder = oldSelectedFolder;
                    }
                    catch
                    {
                        SelectedFolder = null;
                    }
                }
            }
        }

        [Browsable(false)]
        public bool SelectedItemIsFile => SelectedItem?.IsFile ?? false;

        [Browsable(false)]
        public string? SelectedItemPath
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

        [Browsable(false)]
        public new FileListBoxItem? SelectedItem
        {
            get => (FileListBoxItem?)base.SelectedItem;
            set => base.SelectedItem = value;
        }

        public void Reload(string? selectPath = null)
        {
            DoInsideUpdate(() =>
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

                var dirs = Directory.GetDirectories(selectedFolder);

                foreach (var dir in dirs)
                {
                    AddFolder(null, dir);
                }

                var files = Directory.GetFiles(selectedFolder, searchPattern);

                foreach (var file in files)
                {
                    AddFile(null, file);
                }
            });

            Application.AddIdleTask(() =>
            {
                if (IsDisposed)
                    return;
                SelectedItem = null;
            });

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

        public bool AddFile(string? title, string path, SvgImage? image = null)
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

        public bool AddFolder(string? title, string path, SvgImage? image = null)
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
                SelectedFolder = item.Path;
            }
        }

        public bool AddSpecialFolder(Environment.SpecialFolder folder)
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

        public void AddSpecialFolders()
        {
            if(VisibleSpecialFolders is not null)
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.KeyData == Keys.F5)
            {
                e.Handled = true;
                Reload();
            }

            base.OnKeyDown(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            var item = SelectedItem as ListControlItem;
            var action = item?.DoubleClickAction;
            action?.Invoke();
        }

        public class FileListBoxItem : ListControlItem
        {
            public string? Path;

            public FileListBoxItem(string title, string? path, Action? doubleClick = null)
            {
                Text = title;
                Path = path;
                DoubleClickAction = doubleClick;
            }

            [Browsable(false)]
            public bool IsFolder
            {
                get
                {
                    var result = !string.IsNullOrEmpty(Path);
                    if (result)
                        result = Directory.Exists(Path);
                    return result;
                }
            }

            [Browsable(false)]
            public bool IsFile
            {
                get
                {
                    var result = !string.IsNullOrEmpty(Path);
                    if(result)
                        result = File.Exists(Path);
                    return result;
                }
            }

            [Browsable(false)]
            public string ExtensionLower => PathUtils.GetExtensionLower(Path);
        }

        public class FolderInfoItem
        {
            public SvgImage? Image;
            public string? Title;
        }
    }
}
