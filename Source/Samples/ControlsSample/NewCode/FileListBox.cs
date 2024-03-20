using System;
using System.Collections.Generic;
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

        static FileListBox()
        {

        }

        public FileListBox()
        {
        }

        public bool AddFolder(string title, string path, SvgImage? image = null)
        {
            if (string.IsNullOrEmpty(path))
                    return false;
            image ??= DefaultFolderImage;
            var item = new FileListBoxItem(title, path, FolderAction);
            item.SvgImage = image;

            Add(item);

            return true;

            void FolderAction()
            {

            }
        }

        public void AddGoToParentFolderItem(string thisFolder)
        {

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

        public class FileListBoxItem : ListControlItem
        {
            public string? Path;

            public FileListBoxItem(string title, string path, Action? doubleClick)
            {
                Text = title;
                Path = path;
            }
        }

        public class FolderInfoItem
        {
            public SvgImage? Image;
            public string? Title;
        }
    }
}
