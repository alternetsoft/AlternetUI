using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void SetDefaultOwnerDrawItems(
            VirtualListBox control,
            bool addLong = true)
        {
            NotNullCollection<ListControlItem> items = new();

            AddDefaultOwnerDrawItems(
                control,
                (item) =>
                {
                    items.Add(item);
                },
                addLong);

            control.SetItemsFast(items, VirtualListBox.SetItemsKind.ChangeField);
        }

        public static void AddDefaultOwnerDrawItems(
            Control control,
            Action<TreeViewItem> addAction,
            bool addLong = true)
        {
            string ResPrefix2 = $"{UrlResPrefix}ToolBarPng.Large.";
            string CalendarUrl = $"{ResPrefix2}Calendar32.png";
            string PencilUrl = $"{ResPrefix2}Pencil32.png";
            string PhotoUrl = $"{ResPrefix2}Photo32.png";

            var svgImageSize = 24; /* image sizes are always in pixels */

            TreeViewItem item = new();
            item.DisplayText = "This is display text";
            item.Text = "This is some text";
            item.Image = Image.FromUrlCached(PhotoUrl);
            addAction(item);

            item = new();
            item.Text = "Bold item (right, vert center)";
            item.Alignment = (HorizontalAlignment.Right, VerticalAlignment.Center);
            item.FontStyle = FontStyle.Bold;
            item.MinHeight = control.PixelToDip(svgImageSize) * 3;
            item.SvgImage = KnownSvgImages.ImgBold;
            item.SvgImageSize = svgImageSize;
            addAction(item);

            item = new();
            item.Alignment = HVAlignment.Center;
            item.Image = Image.FromUrlCached(CalendarUrl);
            item.CheckState = CheckState.Indeterminate;
            item.DisabledImage = item.Image?.ToGrayScale();
            item.ForegroundColor = Color.White;
            item.BackgroundColor = Color.ForestGreen;
            item.Text = "Green <b>item</b> at center";
            item.LabelFlags = DrawLabelFlags.TextHasBold;
            addAction(item);

            item = new();
            item.Text = "H = 60 (bottom, center)";
            item.CheckBoxVisible = false;
            item.MinHeight = 60;
            item.Alignment = (HorizontalAlignment.Center, VerticalAlignment.Bottom);
            item.Image = Image.FromUrlCached(PencilUrl);
            item.DisabledImage = item.Image?.ToGrayScale();
            item.ForegroundColor = Color.Indigo;
            item.BackgroundColor = Color.LightSkyBlue;
            addAction(item);

            item = new();
            item.FontStyle = FontStyle.Underline;
            item.CheckState = CheckState.Checked;
            item.Text = "Underlined item";
            addAction(item);

            item = new();
            item.Font = Control.DefaultFont.Scaled(1.5f);
            item.Text = "Custom Font";
            addAction(item);

            item = new();
            item.Text = "Custom border";
            item.Alignment = HVAlignment.Center;
            item.CheckBoxVisible = false;
            item.Border = new();
            item.Border.Color = LightDarkColors.Red;
            item.Border.UniformCornerRadius = 25;
            item.Border.UniformRadiusIsPercent = true;
            addAction(item);

            if (addLong)
            {
                item = new();
                item.Text = LoremIpsumSmall;
                addAction(item);
            }

            addAction(new TreeViewSeparatorItem());

            for (int i = 0; i < 150; i++)
            {
                TreeViewItem newItem = new($"Item {i}");

                if (i == 128)
                    newItem.DisplayText = newItem.Text + ": dd";

                addAction(newItem);
            }
        }

        public static void AddManyItems(VirtualListBox listBox)
        {
            listBox.DoInsideUpdate(() =>
            {
                for(int i = 0; i < 5000; i++)
                    listBox.Items.Add(new($"Item #{LogUtils.GenNewId()}"));

                App.Log("Added 5000 items");
            });

            listBox.SelectLastItemAndScroll();
        }

        public static void InitVListBox(object control)
        {
            if (control is not VirtualListBox listBox)
                return;

            SetDefaultOwnerDrawItems(listBox);

            listBox.HorizontalScrollbar = true;
            listBox.Count = 200;
            listBox.CustomItemText += ListBox_CustomItemText;

            static void ListBox_CustomItemText(object? sender, GetItemTextEventArgs e)
            {
                if(string.IsNullOrEmpty(e.Result))
                {
                    e.Result = "Virtual item " + e.ItemIndex.ToString();
                    e.Handled = true;
                }
            }
        }

        public static void InitListBox(object control)
        {
            if (control is not ListBox listBox)
                return;
            listBox.SuggestedSize = defaultListSize;

            foreach (var item in GetTenItems())
                listBox.Add(item);
        }

        public static void InitStdListBox(object control)
        {
            if (control is not StdListBox listBox)
                return;
            listBox.SuggestedSize = defaultListSize;
            listBox.Items.AddRange(GetTenItems());
        }

        public static void InitCheckListBox(object control)
        {
            if (control is not StdCheckListBox listBox)
                return;
            listBox.SuggestedSize = defaultListHeight;
            listBox.Items.AddRange(GetTenItems());
        }

        public static void InitStdComboBox(object control)
        {
            if (control is not StdComboBox comboBox)
                return;
            var items = GetTenItems();
            comboBox.AddRange(items);
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.IsEditable = false;
            comboBox.SuggestedWidth = 200;
        }

        public static void InitComboBox(object control)
        {
            if (control is not ComboBox comboBox)
                return;
            comboBox.Items.AddRange(GetTenItems());
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.SuggestedWidth = 200;
        }
    }
}