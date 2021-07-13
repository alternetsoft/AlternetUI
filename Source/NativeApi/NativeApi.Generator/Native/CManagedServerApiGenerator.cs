using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ApiGenerator.Api;
using Namotion.Reflection;

namespace ApiGenerator.Native
{
    internal static class CManagedServerApiGenerator
    {
        public static string Generate(ApiType apiType)
        {
            var type = apiType.Type;
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CTypes();

            w.WriteLine("#include \"../ApiUtils.h\"");

            w.WriteLine();
            w.WriteLine("using namespace Alternet::UI;");
            w.WriteLine();

            WriteTrampolineLocator(w, types, ManagedServerMemberProvider.GetTrampolineMembers(apiType));

            return GetFinalCode(codeWriter.ToString(), types);
        }

        static string GetFinalCode(string code, Types types)
        {
            var sb = new StringBuilder();
            sb.AppendLine(GeneratorUtils.HeaderText);
            sb.AppendLine();
            sb.AppendLine($"#pragma once");
            sb.AppendLine();
            sb.AppendLine(types.GetIncludes());

            sb.Append(code);

            return sb.ToString();
        }

        private static void WriteTrampolineLocator(IndentedTextWriter w, Types types, MemberInfo[] members)
        {
            var declaringTypeName = types.GetTypeName(members[0].DeclaringType!.ToContextualType(), TypeUsage.Static);

            w.WriteLine($"{CApiGenerator.ExportMacro} void {declaringTypeName}_SetTrampolineLocatorCallback({declaringTypeName}::TTrampolineLocatorCallback callback)");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine($"{declaringTypeName}::SetTrampolineLocatorCallback(callback);");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }
    }
}