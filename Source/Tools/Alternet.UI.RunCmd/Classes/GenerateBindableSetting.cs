﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class GenerateBindableSetting
    {
        public string? RootFolder { get; set; }

        public string? PathToDll { get; set; }

        public string? TypeName { get; set; }

        public string? PathToResult { get; set; }

        public string? ResultTypeName { get; set; }

        public string? SubPropertyName { get; set; }

        public bool? Ignore { get; set; }

        public static PathAssemblyResolver CreateAssemblyResolver(string pathToDll)
        {
            var folder = Path.GetDirectoryName(pathToDll);
            string[] runtimeAssemblies = Directory.GetFiles(folder!, "*.dll");
            var paths = new List<string>(runtimeAssemblies);
            
            paths.Add(typeof(object).Assembly.Location);

            var runTimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");

            paths.AddRange(runTimeAssemblies);

            var resolver = new PathAssemblyResolver(paths);
            return resolver;
        }

        public bool Execute()
        {
            if (PathToDll is null || TypeName is null || PathToResult is null || ResultTypeName is null)
                return false;

            var indent = "    ";

            var resolver = CreateAssemblyResolver(PathToDll);

            using var mlc = new MetadataLoadContext(resolver);

            Assembly assembly = mlc.LoadFromAssemblyPath(PathToDll);
            AssemblyName name = assembly.GetName();

            var type = assembly.GetType(TypeName);

            if(type is null)
            {
                Console.WriteLine($"Type is not found: {TypeName}");
                return false;
            }

            var props = type.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            Console.WriteLine();
            Console.WriteLine("Properties:");
            Console.WriteLine();

            List<string> generatedFile = new();

            var lastPointPos = ResultTypeName.LastIndexOf('.');

            if (lastPointPos <= 0)
                return false;

            var resultNamespace = ResultTypeName.Substring(0, lastPointPos);
            var resultTypeNameOnly = ResultTypeName.Substring(lastPointPos + 1);

            generatedFile.Add($"namespace {resultNamespace}");
            generatedFile.Add("{");

            generatedFile.Add($"{indent}public partial class {resultTypeNameOnly}");
            generatedFile.Add($"{indent}{{");

            foreach (var prop in props)
            {
                var canWrite = prop.CanWrite ? $"set => {SubPropertyName}.{prop.Name} = value; " : string.Empty;
                var canRead = prop.CanRead ? $"get => {SubPropertyName}.{prop.Name}; " : string.Empty;
                var propType = prop.PropertyType.FullName;

                var realType = AssemblyUtils.GetRealType(prop.PropertyType);
                var typeCode = Type.GetTypeCode(realType);

                if (typeCode == TypeCode.Object || !realType.IsValueType)
                    continue;

                var generatedDecl = $"{indent}{indent}public {propType} {prop.Name} {{ {canRead}{canWrite}}}";

                Console.WriteLine(generatedDecl);
                generatedFile.Add(generatedDecl);
                generatedFile.Add(string.Empty);
            }

            generatedFile.Add($"{indent}}}");
            generatedFile.Add("}");

            File.Delete(PathToResult);

            var s = StringUtils.ToString(
                generatedFile,
                string.Empty,
                string.Empty,
                Environment.NewLine);

            using var stream = File.Create(PathToResult);

            StreamUtils.StringToStream(stream, s);

            stream.Flush();
            stream.Close();

            return true;
        }

        public void Prepare()
        {
            if(!string.IsNullOrEmpty(RootFolder))
                RootFolder = Path.GetFullPath(RootFolder);
            
            PathToDll = ReplaceParams(PathToDll);
            PathToResult = ReplaceParams(PathToResult);

            string? ReplaceParams(string? s)
            {
                if (s is null)
                    return null;
                if (!string.IsNullOrEmpty(RootFolder))
                {
                    var result = s.Replace("$(RootFolder)", RootFolder);
                    return result;
                }

                return s;
            }
        }
    }
}
