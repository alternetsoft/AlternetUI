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
            var fieldName = "xml9FBD47B0178D4085BBE279FC109E6465";

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

                var resName = document.ResourceName;

                /* w.WriteLine($"Alternet.UI.UixmlLoader.LoadExisting(\"{resName}\", this);");*/

                w.WriteLine($"Alternet.UI.UixmlLoader.LoadExistingFromString({fieldName}, this, \"{resName}\");");

                w.WriteLine();
                w.WriteLine("AssignNamedElementsToFields();");
                w.WriteLine();

                foreach (var namedObject in namedObjects)
                    w.WriteLine($"{namedObject.Name} ??= ({namedObject.TypeFullName})FindElement(\"{namedObject.Name}\");");

                w.WriteLine();
                foreach (var eventBinding in document.EventBindings)
                    WriteEventBinding(w, eventBinding, task, taskItem);
            }

            w.WriteLine();

            var s = document.XmlContentAsString;
            var literal = CreateStringLiteral(s, useRaw: true);

            w.WriteLine($"private static readonly string {fieldName} = {literal};");
        }

        /// <summary>
        /// Create a verbatim string literal (@"...") safe for multiline text.
        /// </summary>
        private static string CreateVerbatimLiteral(string s)
        {
            // Replace " with "" for verbatim string and prefix with @"
            // Verbatim strings support multiline content.
            string doubledQuotes = s.Replace("\"", "\"\"");
            return $"@\"{doubledQuotes}\"";
        }

        /// <summary>
        /// Create a C# 11+ raw string literal. It chooses the number of double-quotes
        /// needed so the literal is safe even if the content contains sequences of quotes.
        /// Produces syntax like: """...""" or """"..."""" etc.
        /// </summary>
        private static string CreateRawStringLiteral(string s)
        {
            // Find the longest run of consecutive double quotes in the content
            int maxRun = 0;
            int currentRun = 0;
            foreach (char c in s)
            {
                if (c == '\"')
                {
                    currentRun++;
                    if (currentRun > maxRun) maxRun = currentRun;
                }
                else
                {
                    currentRun = 0;
                }
            }

            // Need at least one more quote than the longest run inside content
            int quoteCount = Math.Max(3, maxRun + 1);

            // Opening/closing quote sequence
            string quotes = new string('\"', quoteCount);

            // For raw string literals, if the content starts or ends with a newline,
            // the indentation rules may matter; here we emit it simply with newlines preserved.
            return $"{quotes}\n{s}\n{quotes}";
        }

        /// <summary>
        /// Choose literal form: if useRaw is true, returns a C#11 raw literal; otherwise a verbatim literal.
        /// </summary>
        private static string CreateStringLiteral(string s, bool useRaw = false)
        {
            if (useRaw)
                return CreateRawStringLiteral(s);
            else
                return CreateVerbatimLiteral(s);
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
