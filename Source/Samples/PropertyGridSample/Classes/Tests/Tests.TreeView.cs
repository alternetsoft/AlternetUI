using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        void InitTestsTreeView()
        {
            AddControlAction<TreeView>("Load png from resources", TestTreeViewLoadPngFromResource);
            
            AddControlAction<TreeView>(
                "Load all small *.png in folder...",
                TestTreeViewLoadAllPngInFolder);

            AddControlAction<TreeView>("Load all *.svg in folder...", TestTreeViewLoadAllSvgInFolder);
            AddControlAction<TreeView>("Load known svg", TestTreeViewLoadKnownSvg);

            AddControlAction<TreeView>("BackColor = Black", (c) =>
            {
                c.BackColor = Color.Black;
            });

            AddControlAction<TreeView>("Lighten images", (c) =>
            {
                var images = c.ImageList;
                var converted = images?.WithConvertedColors(ControlPaint.LightLight);
                c.ImageList = converted;
            });
        }

        void TestTreeViewLoadPngFromResource(TreeView control)
        {
            const string highDpiSuffix = "_HighDpi";

            string[] resNames =
            [
                "ClassAlpha",
                "ConstantAlpha",
                "DelegateAlpha",
                "EventAlpha",
                "FieldAlpha",
                "GenericParameterAlpha",
                "InterfaceAlpha",
                "KeywordAlpha",
                "LocalOrParameterAlpha",
                "MethodAlpha",
                "NamespaceAlpha",
                "PropertyAlpha",
                "StructAlpha",
            ];

            string pathPrefix = "Resources.CodeComletionSymbols.";

            int size = control.HasScaleFactor ? 32 : 16;

            ImageList imgList = new();
            imgList.ImageSize = size;

            control.RemoveAll();
            control.ImageList = imgList;
            int index = 0;

            foreach (var s in resNames)
            {
                var resNameHighDpi = $"{pathPrefix}{s}{highDpiSuffix}.png";
                var resName = $"{pathPrefix}{s}.png";
                var selectedName = HasScaleFactor ? resNameHighDpi : resName;

                if (!imgList.AddFromAssemblyUrl(typeof(ObjectInit).Assembly, selectedName))
                    continue;

                TreeViewItem item = new(Path.GetFileName(selectedName), index);
                control.Add(item);
                index++;
            }
        }

        void TestTreeViewLoadAllPngInFolder(TreeView control)
        {
            var dialog = SelectDirectoryDialog.Default;

            dialog.ShowAsync(() =>
            {
                int size = 16;
                if (control.HasScaleFactor)
                    size = 32;

                ImageList imgList = new();
                imgList.ImageSize = size;

                control.RemoveAll();
                control.ImageList = imgList;
                int index = 0;

                var folder = dialog.DirectoryName;
                var files = Directory.GetFiles(folder, "*.png");

                Array.Sort(files);

                foreach (var file in files)
                {
                    var image = new Bitmap(file);
                    if (image.Size != (size, size))
                        continue;
                    if (!imgList.Add(image))
                        continue;

                    TreeViewItem item = new(Path.GetFileName(file), index);
                    control.Add(item);
                    index++;
                }
            });
        }

        void TestTreeViewLoadKnownSvg(TreeView control)
        {
            int size = 32;
            ImageList imgList = new();
            imgList.ImageSize = size;

            control.RemoveAll();
            control.ImageList = imgList;
            int index = 0;

            control.DoInsideUpdate(() =>
            {
                AddImages(KnownSvgImages.GetAllImages());
                AddImages(KnownColorSvgImages.GetAllImages());
            });

            void AddImages(IEnumerable<SvgImage> images)
            {
                foreach (var svg in images)
                {
                    imgList.AddSvg(svg, control.IsDarkBackground);

                    TreeViewItem item = new(svg.Url ?? "<empty url>", index);
                    control.Add(item);
                    index++;
                }
            }
        }

        void TestTreeViewLoadAllSvgInFolder(TreeView control)
        {
            var dialog = SelectDirectoryDialog.Default;

            dialog.ShowAsync(() =>
            {
                int size = 32;
                ImageList imgList = new();
                imgList.ImageSize = size;

                control.RemoveAll();
                control.ImageList = imgList;
                int index = 0;

                var folder = dialog.DirectoryName;
                var files = Directory.GetFiles(folder, "*.svg");

                Array.Sort(files);

                foreach (var file in files)
                {
                    var svg = new MonoSvgImage(file);
                    imgList.AddSvg(svg, control.IsDarkBackground);

                    TreeViewItem item = new(Path.GetFileName(file), index);
                    control.Add(item);
                    index++;
                }
            });
        }
    }
}