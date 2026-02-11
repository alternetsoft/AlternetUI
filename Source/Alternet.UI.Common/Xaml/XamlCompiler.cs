#pragma warning disable
using System;
using System.IO;
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
        internal static bool DefaultEnableIlVerification = false;

        private readonly IXamlTypeSystem _typeSystem;

        public TransformerConfiguration Configuration { get; }

        public XamlCompiler()
            : this(new SreTypeSystem())
        {
        }

        public (
                Func<IServiceProvider?, object> create,
                Action<IServiceProvider?, object> populate,
                Assembly assembly)
            Compile(string xaml, string? targetDllFileName = null)
        {
            var da = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.Run);

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

            var created = t.CreateTypeInfo() ?? throw new Exception();
            dm.CreateGlobalFunctions();

            return GetCallbacks(created, da);
        }

        private XamlCompiler(IXamlTypeSystem typeSystem)
        {
            _typeSystem = typeSystem;
            Configuration = new TransformerConfiguration(
                typeSystem,
                typeSystem.FindAssembly("Alternet.UI"),
                new XamlLanguageTypeMappings(typeSystem)
                {
                    XmlnsAttributes =
                    {
                        typeSystem.FindType(typeof(Alternet.UI.XmlnsDefinitionAttribute)),
                    },
                    ContentAttributes =
                    {
                        typeSystem.FindType(typeof(Alternet.UI.ContentAttribute)),
                    },
                    UsableDuringInitializationAttributes =
                    {
                        typeSystem.FindType(typeof(Alternet.UI.UsableDuringInitializationAttribute)),
                    },
                    DeferredContentPropertyAttributes =
                    {
                        typeSystem.FindType(typeof(Alternet.UI.DeferredContentAttribute)),
                    },
                    RootObjectProvider = typeSystem.FindType(typeof(Alternet.UI.ITestRootObjectProvider)),
                    UriContextProvider = typeSystem.FindType(typeof(Alternet.UI.ITestUriContext)),
                    ProvideValueTarget = typeSystem.FindType(typeof(Alternet.UI.ITestProvideValueTarget)),
                    ParentStackProvider = typeSystem.FindType(typeof(XamlX.Runtime.IXamlParentStackProviderV1)),
                    XmlNamespaceInfoProvider = typeSystem.FindType(typeof(XamlX.Runtime.IXamlXmlNamespaceInfoProviderV1)),
                });
        }

        protected object CompileAndRun(string xaml, IServiceProvider? prov = null)
            => Compile(xaml).create(prov);

#pragma warning disable
        protected object CompileAndPopulate(
            string xaml,
            IServiceProvider? prov = null,
            object? instance = null)
#pragma warning restore
            => Compile(xaml).create(prov);

        private XamlDocument Compile(
            IXamlTypeBuilder<IXamlILEmitter> builder,
            IXamlType context,
            string xaml)
        {
            var parsed = XDocumentXamlParser.Parse(xaml);
            var compiler = new XamlILCompiler(
                Configuration,
                new XamlLanguageEmitMappings<IXamlILEmitter, XamlILNodeEmitResult>(),
                true)
            {
                EnableIlVerification = DefaultEnableIlVerification,
            };
            compiler.Transform(parsed);
            compiler.Compile(
                parsed,
                builder,
                context,
                "Populate",
                "Build",
                "XamlNamespaceInfo",
                "http://example.com/",
                null);
            return parsed;
        }

        private (Func<IServiceProvider?, object> Create, Action<IServiceProvider?, object> Populate, Assembly Assembly)
            GetCallbacks(Type created, Assembly assembly)
        {
            var isp = System.Linq.Expressions.Expression.Parameter(typeof(IServiceProvider));
            var createCb = System.Linq.Expressions.Expression.Lambda<Func<IServiceProvider?, object>>(
                System.Linq.Expressions.Expression.Convert(
                    System.Linq.Expressions.Expression.Call(
                        created.GetMethod("Build")!,
                        isp),
                    typeof(object)),
                isp).Compile();

            var epar = System.Linq.Expressions.Expression.Parameter(typeof(object));
            var populate = created.GetMethod("Populate") ?? throw new InvalidOperationException();
            isp = System.Linq.Expressions.Expression.Parameter(typeof(IServiceProvider));
            var populateCb =
                System.Linq.Expressions.Expression.Lambda<Action<IServiceProvider?, object>>(
                    System.Linq.Expressions.Expression.Call(
                        populate,
                        isp,
                        System.Linq.Expressions.Expression.Convert(
                            epar,
                            populate.GetParameters()[1].ParameterType)),
                    isp,
                    epar).Compile();

            return (createCb, populateCb, assembly);
        }
    }
}