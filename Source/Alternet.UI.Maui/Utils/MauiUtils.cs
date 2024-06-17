using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Alternet.UI
{
    public static class MauiUtils
    {
        public static void AddAllViewsToParent(Layout parent)
        {
            var types = AssemblyUtils.GetTypeDescendants(typeof(View));

            foreach(var type in types)
            {
                if (type == typeof(WebView) || type == typeof(VerticalStackLayout) || type == typeof(GroupableItemsView))
                    continue;

                View? instance;

                try
                {
                    if (!AssemblyUtils.HasConstructorNoParams(type))
                        continue;
                    instance = (View?)Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    instance = null;
                }

                if (instance is null)
                    continue;

                try
                {
                    parent.Children.Add(instance);
                    var nativeType = instance.Handler?.PlatformView?.GetType().FullName;
                    LogUtils.LogToFile($"{instance.GetType().FullName} uses {nativeType}");
                    parent.Children.Remove(instance);
                }
                catch (Exception e)
                {
                }
            }
        }

        public static void EnumViewsToLog(Layout parent)
        {
            foreach (var item in parent.Children)
            {
                var nativeType = item.Handler?.PlatformView?.GetType().Name;

                LogUtils.LogToFile($"{item.GetType().Name} uses {nativeType}");
            }
        }
    }
}
