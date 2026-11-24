using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    internal class TestActionsWindow : Window
    {
        private readonly VirtualListBox listBox;
        private readonly VirtualListBox.RangeAdditionController<MemberInfo> controller;

        public TestActionsWindow()
        {
            Title = "Test Actions";
            StartLocation = WindowStartLocation.ScreenTopRight;

            MinimumSize = (400, 500);
            HasTitleBar = true;
            CloseEnabled = true;

            listBox = new VirtualListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.Parent = this;

            SetLocationOnDisplay(HorizontalAlignment.Left, VerticalAlignment.Fill);

            LogUtilsActions.EnumLogActions(Fn);

            void Fn(string title, Action action)
            {
                if (title.StartsWith("Test "))
                    ActionsListBox.AddBusyAction(DoAddAction, title, action);

                ListControlItem DoAddAction(string title, Action action)
                {
                    ListControlItem item = new(title)
                    {
                        DoubleClickAction = action,
                    };

                    listBox.Add(item);
                    return item;
                }
            }

            ListControlItem? ConvertItem(MemberInfo member)
            {
                var item = LogUtils.ActionAndTitleFromTestMethod(member);
                if (item is null)
                    return null;
                var result = new TreeViewItem(item.Value.Title);
                result.DoubleClickAction = item.Value.Action;
                return result;
            }

            controller = new(
                listBox,
                () => AssemblyUtils.GetAllPublicMembers("Test", KnownAssemblies.AllLoadedAlternet),
                ConvertItem,
                () =>
                {
                    return !IsDisposed;
                });
            controller.Start();
        }
    }
}
