﻿using System.CodeDom.Compiler;
using System.IO;

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
                    w.WriteLine("private bool contentLoaded;");
                    w.WriteLine();
                    w.WriteLine("[System.Diagnostics.DebuggerNonUserCodeAttribute()]");
                    w.WriteLine("public void InitializeComponent()");
                    using (new BlockIndent(w))
                    {
                        w.WriteLine("if (contentLoaded)");
                        using (new LineIndent(w))
                            w.WriteLine("return;");
                        w.WriteLine("contentLoaded = true;");

                        w.WriteLine($"var uixmlStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(\"{document.ResourceName}\");");
                        w.WriteLine("if (uixmlStream == null)");
                        using (new LineIndent(w))
                            w.WriteLine("throw new InvalidOperationException();");
                        w.WriteLine("new Alternet.UI.XamlLoader().LoadExisting(uixmlStream, this);");
                    }
                }
            }

            return codeWriter.ToString();
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
