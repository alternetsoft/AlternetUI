using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using XamlX.Ast;
using XamlX.Emit;
using XamlX.IL;
using XamlX.Parsers;
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Alternet.UI
{
    internal class XamlCompiler
    {
        private readonly IXamlTypeSystem _typeSystem;
        public TransformerConfiguration Configuration { get; }

        private XamlCompiler(IXamlTypeSystem typeSystem)
        {
            _typeSystem = typeSystem;
            Configuration = new TransformerConfiguration(typeSystem,
                typeSystem.FindAssembly("Alternet.UI"),
                new XamlLanguageTypeMappings(typeSystem)
                {
                    XmlnsAttributes =
                    {
                        typeSystem.GetType("Alternet.UI.XmlnsDefinitionAttribute"),

                    },
                    ContentAttributes =
                    {
                        typeSystem.GetType("Alternet.UI.ContentAttribute")
                    },
                    UsableDuringInitializationAttributes =
                    {
                        typeSystem.GetType("Alternet.UI.UsableDuringInitializationAttribute")
                    },
                    DeferredContentPropertyAttributes =
                    {
                        typeSystem.GetType("Alternet.UI.DeferredContentAttribute")
                    },
                    RootObjectProvider = typeSystem.GetType("Alternet.UI.ITestRootObjectProvider"),
                    UriContextProvider = typeSystem.GetType("Alternet.UI.ITestUriContext"),
                    ProvideValueTarget = typeSystem.GetType("Alternet.UI.ITestProvideValueTarget"),
                    ParentStackProvider = typeSystem.GetType("XamlX.Runtime.IXamlParentStackProviderV1"),
                    XmlNamespaceInfoProvider = typeSystem.GetType("XamlX.Runtime.IXamlXmlNamespaceInfoProviderV1")
                }
            );
        }

        protected object CompileAndRun(string xaml, IServiceProvider? prov = null) => Compile(xaml).create(prov);

        protected object CompileAndPopulate(string xaml, IServiceProvider? prov = null, object? instance = null)
            => Compile(xaml).create(prov);
        XamlDocument Compile(IXamlTypeBuilder<IXamlILEmitter> builder, IXamlType context, string xaml)
        {
            var parsed = XDocumentXamlParser.Parse(xaml);
            var compiler = new XamlILCompiler(
                Configuration,
                new XamlLanguageEmitMappings<IXamlILEmitter, XamlILNodeEmitResult>(),
                true)
            {
                EnableIlVerification = true
            };
            compiler.Transform(parsed);
            compiler.Compile(parsed, builder, context, "Populate", "Build",
                "XamlNamespaceInfo",
                "http://example.com/", null);
            return parsed;
        }
        //static object s_asmLock = new object();

        public XamlCompiler() : this(new SreTypeSystem())
        {

        }

        public (Func<IServiceProvider?, object> create, Action<IServiceProvider?, object> populate, Assembly assembly) Compile(string xaml, string? targetDllFileName = null)
        {
#if !NETCOREAPP && !NETSTANDARD
            var da = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString("N")),
                AssemblyBuilderAccess.RunAndSave,
                Directory.GetCurrentDirectory());
#else
            var da = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.Run);
#endif

            var dm = da.DefineDynamicModule(targetDllFileName == null ? "testasm.dll" : Path.GetFileName(targetDllFileName));
            var t = dm.DefineType(Guid.NewGuid().ToString("N"), TypeAttributes.Public);
            var ct = dm.DefineType(t.Name + "Context");
            var ctb = ((SreTypeSystem)_typeSystem).CreateTypeBuilder(ct);
            var contextTypeDef =
                XamlILContextDefinition.GenerateContextClass(
                    ctb,
                    _typeSystem,
                    Configuration.TypeMappings,
                    new XamlLanguageEmitMappings<IXamlILEmitter, XamlILNodeEmitResult>());


            var parserTypeBuilder = ((SreTypeSystem)_typeSystem).CreateTypeBuilder(t);

            var parsed = Compile(parserTypeBuilder, contextTypeDef, xaml);

            var created = t.CreateTypeInfo();
            if (created == null)
                throw new Exception();

            dm.CreateGlobalFunctions();

            return GetCallbacks(created, da);
        }

        (Func<IServiceProvider?, object> create, Action<IServiceProvider?, object> populate, Assembly assembly) GetCallbacks(Type created, Assembly assembly)
        {
            var isp = Expression.Parameter(typeof(IServiceProvider));
            var createCb = Expression.Lambda<Func<IServiceProvider?, object>>(
                Expression.Convert(Expression.Call(
                    created.GetMethod("Build"), isp), typeof(object)), isp).Compile();

            var epar = Expression.Parameter(typeof(object));
            var populate = created.GetMethod("Populate");
            if (populate == null)
                throw new InvalidOperationException();

            isp = Expression.Parameter(typeof(IServiceProvider));
            var populateCb = Expression.Lambda<Action<IServiceProvider?, object>>(
                Expression.Call(populate, isp, Expression.Convert(epar, populate.GetParameters()[1].ParameterType)),
                isp, epar).Compile();

            return (createCb, populateCb, assembly);
        }
    }
}