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

            var asm = typeof(ApiDoc.MultilineTextBoxWindow).Assembly;

            try
            {
                var windowType = asm.GetTypes()
                              .FirstOrDefault(t =>
                              {
                                  var result = t.Name.EndsWith($"{title}Window");
                                  return result;
                              });
                if (windowType is not null)
                {
                    if (AssemblyUtils.IsControlCategoryInternal(windowType))
                        return;

                    Add(title, () =>
                    {
                        Window? result = null;

                        try
                        {
                            result = (Window?)Activator.CreateInstance(windowType);
                        }
                        catch
                        {
                        }

                        result ??= new Window();
                        result.Title = $"{title} Sample";
                        return result;
                    });
                }
                else
                {
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error creating sample for {title}: {e.Message}");
            }
        }

        protected override void AddDefaultItems()
        {
            AddGroup("Controls");

            var types = AssemblyUtils.AllControlDescendants.Values;

            foreach (var type in types)
            {
                if (AssemblyUtils.IsControlCategoryHidden(type))
                    continue;
                AddSample(type);
            }
        }
    }
}