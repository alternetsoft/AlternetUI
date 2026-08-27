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
        public static void InitShapeControl(ShapeControl control)
        {
            control.SuggestedSize = new (400, 400);

            var innerShape1 = new ShapeControl
            {
                Stroke = new Pen(Color.Red, 2),
                Fill = new SolidBrush(Color.FromArgb(128, Color.Red)),
                IgnoreLayout = true,
                Location = new (50, 50),
                Size = new (300, 300),
            };

            var innerShape2 = new ShapeControl
            {
                Stroke = new Pen(Color.Green, 2),
                Fill = new SolidBrush(Color.FromArgb(128, Color.Green)),
                ShapeType = DrawingShapeType.Ellipse,
                IgnoreLayout = true,
                Location = new(100, 100),
                Size = new(300, 300),
            };

            innerShape1.Parent = control;
            innerShape2.Parent = control;
        }

        public static void InitGenericListItemControl(GenericItemControl control)
        {
            ListControlItem item = new();

            item.Alignment = HVAlignment.Center;
            item.Image = Image.FromUrlCached(DemoUtils.CalendarUrl);
            item.CheckState = CheckState.Checked;
            item.DisabledImage = item.Image?.ToGrayScale();
            item.ForegroundColor = Color.Black;
            item.BackgroundColor = Color.BlanchedAlmond;
            item.Text = "Sample <b>item</b> at center";
            item.LabelFlags = DrawLabelFlags.TextHasBold;

            control.ItemDefaults.CheckBoxVisible = true;
            control.Item = item;
        }

        public static void AddManyItems(VirtualListBox listBox)
        {
            listBox.DoInsideUpdate(() =>
            {
                for(int i = 0; i < 5000; i++)
                    listBox.Add(new($"Item #{LogUtils.GenNewId()}"));

                App.Log("Added 5000 items");
            });

            listBox.SelectLastItemAndScroll();
        }

        public static void InitFileListBox(object control)
        {
            if (control is not FileListBox listBox)
                return;
            listBox.SuggestedSize = DemoUtils.DefaultListSize;
            listBox.SelectInitialFolder();
        }

        public static void LogItems(string prefix, IReadOnlyList<object?> items)
        {
            if (items.Count > 100)
                App.LogReplace($"{prefix}: {items.Count} items", prefix);
            else
            {
                var st = items.Count == 0 ? "<none>" :
                string.Join(", ", items.Select(x => x?.ToString()));
                App.LogReplace($"{prefix}: {st}", prefix);
            }
        }

        public static void InitStdListBox(object control)
        {
            if (control is not XListBox listBox)
                return;
            listBox.SuggestedSize = DemoUtils.DefaultListSize;
            listBox.Items.AddRange(GetTenItems());
        }

        public static void InitCheckListBox(object control)
        {
            if (control is not XCheckListBox listBox)
                return;
            listBox.SuggestedSize = DemoUtils.DefaultListSize;
            listBox.Items.AddRange(GetTenItems());
        }

        public static void InitStdComboBox(object control)
        {
            if (control is not XComboBox comboBox)
                return;
            var items = GetTenItems();
            comboBox.AddRange(items);
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.IsEditable = false;
            comboBox.SuggestedWidth = 200;
        }
    }
}