using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Alternet.UI.Build.Tasks
{
    public static class CSharpUIXmlCodeGenerator
    {
        public static string Generate(
            UIXmlDocument document,
            GenerateUIXmlCodeTask task,
            ITaskItem taskItem)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            WriteHeader(w);

            w.WriteLine(@"
using System;
");
            w.WriteLine($"namespace {document.ClassNamespaceName}");

            using (new BlockIndent(w))
            {
                w.WriteLine($"partial class {document.ClassName} : {document.BaseClassFullName}");
                using (new BlockIndent(w))
                {
                    var namedObjects = document.NamedObjects;

                    WriteNamedObjectsFields(w, namedObjects);
                    WriteInitializeComponent(document, w, namedObjects, task, taskItem);
                    WriteUIXmlPreviewerConstructor(document, w);
                }
            }

            return codeWriter.ToString();
        }

        private static void WriteNamedObjectsFields(
            IndentedTextWriter w,
            IReadOnlyList<UIXmlDocument.NamedObject> namedObjects)
        {
            foreach (var namedObject in namedObjects)
                w.WriteLine($"private {namedObject.TypeFullName} {namedObject.Name};");
        }

        public static void WriteLineIndented(this IndentedTextWriter w, string s)
        {
            using (new LineIndent(w))
                w.WriteLine(s);
        }

        private static void WriteInitializeComponent(
            UIXmlDocument document,
            IndentedTextWriter w,
            IReadOnlyList<UIXmlDocument.NamedObject> namedObjects,
            GenerateUIXmlCodeTask task,
            ITaskItem taskItem)
        {
            w.WriteLine();
            w.WriteLine("private bool contentLoaded;");
            w.WriteLine();
            w.WriteLine("public void InitializeComponent()");
            using (new BlockIndent(w))
            {
                w.WriteLine("if (Alternet.UI.UixmlLoader.DisableComponentInitialization)");
                    w.WriteLineIndented("return;");
                w.WriteLine("if (contentLoaded)");
                    w.WriteLineIndented("return;");
                w.WriteLine("contentLoaded = true;");
                w.WriteLine($"Alternet.UI.UixmlLoader.LoadExisting(\"{document.ResourceName}\", this);");

                w.WriteLine();
                foreach (var namedObject in namedObjects)
                    w.WriteLine($"{namedObject.Name} = ({namedObject.TypeFullName})FindElement(\"{namedObject.Name}\");");

                w.WriteLine();
                foreach (var eventBinding in document.EventBindings)
                    WriteEventBinding(w, eventBinding, task, taskItem);
            }
        }

        private static void WriteUIXmlPreviewerConstructor(UIXmlDocument document, IndentedTextWriter w)
        {
            const string UIXmlPreviewerConstructorMarkerTypeName = "UIXmlPreviewerConstructorMarkerType";

            w.WriteLine();
            w.WriteLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
            w.WriteLine("class " + UIXmlPreviewerConstructorMarkerTypeName + " {}");
            w.WriteLine();
            w.WriteLine($"{document.ClassName}(UIXmlPreviewerConstructorMarkerType _)");
            using (new BlockIndent(w))
            {
            }
        }

        private static void WriteEventBinding(
            IndentedTextWriter w,
            UIXmlDocument.EventBinding eventBinding,
            GenerateUIXmlCodeTask task,
            ITaskItem taskItem)
        {
            if(eventBinding is UIXmlDocument.IndexedObjectEventBinding bind
                /*&& bind.Accessors.Count > 1*/)
            {
                var s = $"Bad binding '{bind.HandlerName}' to '{bind.EventName}' event for the element with an empty Name property.";
                task.LogError(taskItem, s);
                return;
            }

            switch (eventBinding)
            {
                case UIXmlDocument.NamedObjectEventBinding x:
                    w.WriteLine($"{x.ObjectName}.{x.EventName} += {x.HandlerName};");
                    break;
                case UIXmlDocument.IndexedObjectEventBinding x:
                    w.WriteLine(
                        $"{GetIndexedObjectRetrievalExpression(x.ObjectTypeFullName, x.Accessors)}.{x.EventName} += {x.HandlerName};");
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static string GetIndexedObjectRetrievalExpression(
            string objectTypeFullName,
            IReadOnlyList<UIXmlDocument.AccessorInfo> accessors)
        {
            var result = new StringBuilder();
            result.Append($"(({objectTypeFullName})(");
            
            if (accessors.Count == 0)
                throw new InvalidOperationException();

            for (int i = 0; i < accessors.Count; i++)
            {
                var accessor = accessors[i];
                switch (accessor)
                {
                    case UIXmlDocument.IndexAccessorInfo indexAccessor:
                        if (i > 0 && indexAccessor.CollectionName != "")
                            result.Append(".");
                        result.Append($"{indexAccessor.CollectionName}[{indexAccessor.Index}]");
                        break;
                    case UIXmlDocument.MemberAccessorInfo memberAccessor:
                        if (i > 0)
                            result.Append(".");
                        result.Append(memberAccessor.Name);
                        break;
                    default:
                        throw new Exception();
                }
            }

            result.Append("))");
            return result.ToString();
        }

        private static void WriteHeader(IndentedTextWriter w)
        {
            w.WriteLine("//------------------------------------------------------------------------------");
            w.WriteLine("// <auto-generated>");
            w.WriteLine("//     This code was generated by a tool.");
            w.WriteLine("//     Changes to this file may cause incorrect behavior and will be lost if");
            w.WriteLine("//     the code is regenerated.");
            w.WriteLine("// </auto-generated>");
            w.WriteLine("//------------------------------------------------------------------------------");
            w.WriteLine();
        }
    }
}
