using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace ApiDoc
{
    public partial class ApiDocSamplesPage : PanelFormSelector
    {
        public void AddSample(Type type)
        {
            var title = type.Name;

            var windowType = this.GetType().Assembly.GetType($"ApiDoc.{title}Window");

            if(windowType is not null)
            {
                if (AssemblyUtils.IsControlCategoryInternal(windowType))
                    return;

                Add(title, () =>
                {
                    var result = (Window?)Activator.CreateInstance(windowType);
                    result ??= new Window();
                    result.Title = $"{title} Sample";
                    return result;
                });
            }
        }

        protected override void AddDefaultItems()
        {
            AddGroup("Controls");

            var types = AssemblyUtils.AllControlDescendants.Values;

            foreach(var type in types)
            {
                if (AssemblyUtils.IsControlCategoryHidden(type))
                    continue;
                AddSample(type);
            }
        }
    }
}