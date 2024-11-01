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
        private int counter = 0;

        void InitTestsListView()
        {
            AddControlAction<ListView>("Clear Columns", (listView) => listView.Columns.Clear());
            AddControlAction<ListView>("Add items with svg images", TestListViewAddSvgImages);
            AddControlAction<ListView>("Clear", (listView)=>listView.Clear());
            AddControlAction<ListView>("Remove All", (listView) => listView.RemoveAll());
            AddControlAction<ListView>(
                "View = Details",
                (listView) => listView.View = ListViewView.Details);
            AddControlAction<ListView>(
                "View = SmallIcon",
                (listView) => listView.View = ListViewView.SmallIcon);
            AddControlAction<ListView>(
                "View = List",
                (listView) => listView.View = ListViewView.List);
            AddControlAction<ListView>(
                "Add Item",
                (listView) => listView.Items.Add(new($"Item {counter++}", 1)));
            AddControlAction<ListView>(
                "Add Column",
                (listView) => listView.Columns.Add(new($"Column {counter++}")));
        }

        void TestListViewAddSvgImages(ListView control)
        {
            var imgError = KnownColorSvgImages.ImgError;
            var imgInfo = KnownColorSvgImages.ImgInformation;
            var imgWarning = KnownColorSvgImages.ImgWarning;

            var size = ImageList.GetSuggestedSize(control.ScaleFactor);
            ImageList images = new();
            images.ImageSize = size;

            images.AddSvg(imgError);
            images.AddSvg(imgInfo);
            images.AddSvg(imgWarning);

            control.Clear();

            control.View = ListViewView.Details;
            control.SmallImageList = images;

            ListViewColumn column1 = new("Name");
            column1.WidthMode = ListViewColumnWidthMode.AutoSize;

            ListViewColumn column2 = new("Description");
            column1.WidthMode = ListViewColumnWidthMode.AutoSizeHeader;

            control.Columns.Add(column1);
            control.Columns.Add(column2);

            control.DoInsideUpdate(() =>
            {

                ListViewItem item1 = new("Error image", 0);
                ListViewItem item2 = new("Information image", 1);
                ListViewItem item3 = new("Warning image", 2);
                control.Items.Add(item1);
                control.Items.Add(item2);
                control.Items.Add(item3);
            });

            column1.RaiseChanged();
            column2.RaiseChanged();
        }
    }
}