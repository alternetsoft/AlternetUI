﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    public partial class LogUtils
    {
        /// <summary>
        /// Gets title and action from the test method declaration.
        /// </summary>
        /// <param name="member">Test method info.</param>
        /// <returns></returns>
        public static (string Title, Action Action)? ActionAndTitleFromTestMethod(MemberInfo member)
        {
            if (member.MemberType != MemberTypes.Method)
                return null;
            var method = (MethodInfo)member;
            if (!method.IsStatic)
                return null;
            if (method.ContainsGenericParameters)
                return null;
            var prm = method.GetParameters();
            if (prm.Length > 0)
                return null;
            if (!member.Name.StartsWith("Test"))
                return null;

            var fullName = $"{member.DeclaringType.FullName}.{member.Name}";

            var desc = AssemblyUtils.GetDescription(member);

            if (desc is not null)
                fullName = desc;

            void Fn()
            {
                method.Invoke(null, null);
            }

            (string Title, Action Action) result = new(fullName, Fn);
            return result;
        }

        private class TestActionsWindow : Window
        {
            private readonly ActionsListBox listBox;
            private readonly VirtualListBox.AddRangeController<MemberInfo> controller;

            public TestActionsWindow()
            {
                Title = "Test Actions";
                StartLocation = WindowStartLocation.ScreenTopRight;

                MinimumSize = (400, 500);
                HasTitleBar = true;
                CloseEnabled = true;

                listBox = new ActionsListBox();
                listBox.Parent = this;

                SetLocationOnDisplay(HorizontalAlignment.Left, VerticalAlignment.Fill);

                LogUtils.EnumLogActions(Fn);

                void Fn(string title, Action action)
                {
                    if (title.StartsWith("Test "))
                        listBox.AddBusyAction(title, action);
                }

                ListControlItem? ConvertItem(MemberInfo member)
                {
                    var item = ActionAndTitleFromTestMethod(member);
                    if (item is null)
                        return null;
                    var result = new ListControlItem(item.Value.Title);
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
}
