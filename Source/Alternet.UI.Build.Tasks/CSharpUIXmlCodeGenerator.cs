using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alternet.UI.Build.Tasks
{
    static class CSharpUIXmlCodeGenerator
    {
        public static string Generate(UIXmlDocument document)
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
                    WriteInitializeComponent(document, w, namedObjects);
                    WriteUIXmlPreviewerConstructor(document, w);
                }
            }

            return codeWriter.ToString();
        }

        private static void WriteNamedObjectsFields(IndentedTextWriter w, IReadOnlyList<UIXmlDocument.NamedObject> namedObjects)
        {
            foreach (var namedObject in namedObjects)
                w.WriteLine($"private {namedObject.TypeFullName} {namedObject.Name};");
        }

        private static void WriteInitializeComponent(UIXmlDocument document, IndentedTextWriter w, IReadOnlyList<UIXmlDocument.NamedObject> namedObjects)
        {
            w.WriteLine();
            w.WriteLine("private bool contentLoaded;");
            w.WriteLine();
            w.WriteLine("[System.Diagnostics.DebuggerNonUserCodeAttribute()]");
            w.WriteLine("public void InitializeComponent()");
            using (new BlockIndent(w))
            {
                w.WriteLine("if (Alternet.UI.UixmlLoader.DisableComponentInitialization)");
                using (new LineIndent(w))
                    w.WriteLine("return;");
                w.WriteLine("if (contentLoaded)");
                using (new LineIndent(w))
                    w.WriteLine("return;");
                w.WriteLine("contentLoaded = true;");

                w.WriteLine($"var uixmlStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(\"{document.ResourceName}\");");
                w.WriteLine("if (uixmlStream == null)");
                using (new LineIndent(w))
                    w.WriteLine("throw new InvalidOperationException();");
                w.WriteLine("new Alternet.UI.UixmlLoader().LoadExisting(uixmlStream, this);");

                w.WriteLine();
                foreach (var namedObject in namedObjects)
                    w.WriteLine($"{namedObject.Name} = ({namedObject.TypeFullName})FindElement(\"{namedObject.Name}\");");

                w.WriteLine();
                foreach (var eventBinding in document.EventBindings)
                    WriteEventBinding(w, eventBinding);
            }
        }

        private static void WriteUIXmlPreviewerConstructor(UIXmlDocument document, IndentedTextWriter w)
        {
            const string UIXmlPreviewerConstructorMarkerTypeName = "UIXmlPreviewerConstructorMarkerType";

            w.WriteLine();
            w.WriteLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
            w.WriteLine("class " + UIXmlPreviewerConstructorMarkerTypeName + " {}");
            w.WriteLine();
            w.WriteLine("[System.Diagnostics.DebuggerNonUserCodeAttribute()]");
            w.WriteLine($"{document.ClassName}(UIXmlPreviewerConstructorMarkerType _)");
            using (new BlockIndent(w))
            {
            }
        }

        private static void WriteEventBinding(IndentedTextWriter w, UIXmlDocument.EventBinding eventBinding)
        {
            switch (eventBinding)
            {
                case UIXmlDocument.NamedObjectEventBinding x:
                    w.WriteLine($"{x.ObjectName}.{x.EventName} += {x.HandlerName};");
                    break;
                case UIXmlDocument.IndexedObjectEventBinding x:
                    w.WriteLine($"{GetIndexedObjectRetreivalExpression(x.ObjectTypeFullName, x.Accessors)}.{x.EventName} += {x.HandlerName};");
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static string GetIndexedObjectRetreivalExpression(string objectTypeFullName, IReadOnlyList<UIXmlDocument.AccessorInfo> accessors)
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
