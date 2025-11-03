using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides static methods related to dynamic assembly generation.
    /// </summary>
    public static class CodeGeneratorUtils
    {
        private static BaseDictionary<EventInfo, Delegate>? eventDelegates;
        private static ModuleBuilder? moduleBuilder;

        /// <summary>
        /// Gets path to folder with System.dll.
        /// Returned value is similar to this:
        /// 'C:\Program Files\dotnet\shared\Microsoft.NETCore.App\9.0.1'.
        /// </summary>
        /// <returns></returns>
        public static string? GetSystemDllPath()
        {
            var assembly = typeof(object).Assembly;
            var location = assembly.Location;
            var asmPath = Path.GetDirectoryName(location);
            return asmPath;
        }

        /// <summary>
        /// Gets dotnet path based on the <see cref="GetSystemDllPath"/> result.
        /// </summary>
        /// <returns></returns>
        public static string? GetDotNetPathFromSystemDllPath()
        {
            var systemDllPath = GetSystemDllPath();
            if (systemDllPath is null)
                return null;
            var combined = Path.Combine(systemDllPath, "..", "..", "..");
            var result = Path.GetFullPath(combined);
            return result;
        }

        /// <summary>
        /// Gets <see cref="ModuleBuilder"/> for dynamic assembly generation.
        /// </summary>
        public static ModuleBuilder GetModuleBuilder()
        {
            if (moduleBuilder is not null)
                return moduleBuilder;

            var uniqueString = StringUtils.GetUniqueString();
            var assemblyTypeName = "DynamicTypes" + uniqueString;

            // Use Reflection.Emit to create a dynamic assembly that
            // will be run but not saved. An assembly must have at
            // least one module, which in this case contains a single
            // type. The only purpose of this type is to contain the
            // event handler method. (You can use also dynamic methods,
            // which are simpler because there is no need to create an
            // assembly, module, or type.)
            AssemblyName aName = new()
            {
                Name = assemblyTypeName,
            };
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            moduleBuilder = ab.DefineDynamicModule(aName.Name);
            return moduleBuilder;
        }

        /// <summary>
        /// Gets event logging delegate.
        /// </summary>
        /// <param name="eInfo">Event information.</param>
        public static Delegate? GetEventLogDelegate(EventInfo eInfo)
        {
            if (eInfo is null)
                return null;
            var eventId = LogUtils.GetEventKey(eInfo.DeclaringType, eInfo);
            if (eventId is null)
                return null;
            if (!LogUtils.IsEventLogged(eventId))
                return null;

            eventDelegates ??= new();
            if (eventDelegates.TryGetValue(eInfo, out var result))
                return result;

            // In order to create a method to handle the Elapsed event,
            // it is necessary to know the signature of the delegate
            // used to raise the event. Reflection.Emit can then be
            // used to construct a dynamic class with a static method
            // that has the correct signature.

            // Get the event handler type of the Elapsed event. This is
            // a delegate type, so it has an Invoke method that has
            // the same signature as the delegate. The following code
            // creates an array of Type objects that represent the
            // parameter types of the Invoke method.
            Type? handlerType = eInfo?.EventHandlerType;
            MethodInfo? invokeMethod = handlerType?.GetMethod("Invoke");
            ParameterInfo[]? paramInfoArray = invokeMethod?.GetParameters();
            Type[] paramTypes = new Type[paramInfoArray?.Length ?? 0];
            for (int i = 0; i < paramInfoArray?.Length; i++)
            {
                paramTypes[i] = paramInfoArray[i].ParameterType;
            }

            var mb = GetModuleBuilder();
            var uniqueString = StringUtils.GetUniqueString();

            var handlerTypeName = "Handler" + uniqueString;
            TypeBuilder tb = mb.DefineType(handlerTypeName, TypeAttributes.Class | TypeAttributes.Public);

            var dynamicHandlerName = "DynamicHandler" + uniqueString;

            // Create the method that will handle the event. The name
            // is not important. The method is static, because there is
            // no reason to create an instance of the dynamic type.
            //
            // The parameter types and return type of the method are
            // the same as those of the delegate's Invoke method,
            // captured earlier.
            MethodBuilder handler = tb.DefineMethod(
                dynamicHandlerName,
                MethodAttributes.Public | MethodAttributes.Static,
                invokeMethod?.ReturnType,
                paramTypes);

            // Generate code to handle the event. In this case, the
            // handler simply prints a text string to the console.
            ILGenerator il = handler.GetILGenerator();
            EmitEventLog(
                il,
                eventId,
                $"Event {eInfo?.DeclaringType?.Name}.{eInfo?.Name} is raised.");
            il.Emit(OpCodes.Ret);

            // CreateType must be called before the Handler type can
            // be used. In order to create the delegate that will
            // handle the event, a MethodInfo from the finished type
            // is required.
            Type? finished = tb.CreateTypeInfo();
            MethodInfo? eventHandler = finished?.GetMethod(dynamicHandlerName);

            // Use the MethodInfo to create a delegate of the correct
            // type, and call the AddEventHandler method to hook up
            // the event.
            if (handlerType is not null && eventHandler is not null)
            {
                Delegate d = Delegate.CreateDelegate(handlerType, eventHandler);
                eventDelegates.Add(eInfo!, d);
                return d;
            }

            return null;
        }

        /// <summary>
        /// Emits method call with <paramref name="value"/> parameter.
        /// </summary>
        /// <param name="il">Code generator.</param>
        /// <param name="method">Method information.</param>
        /// <param name="value">Text string.</param>
        public static void EmitStringMethod(ILGenerator il, MethodInfo method, string value)
        {
            il.Emit(OpCodes.Ldstr, value);
            il.Emit(OpCodes.Call, method);
        }

        /// <summary>
        /// Emits <see cref="App.Log"/> with <paramref name="value"/> parameter.
        /// </summary>
        /// <param name="il">Code generator.</param>
        /// <param name="value">Text string.</param>
        public static void EmitApplicationLog(ILGenerator il, string value)
        {
            Type[] s_parameterTypes = new Type[] { typeof(string) };
            MethodInfo mi = typeof(CodeGeneratorUtils).GetMethod("Log", s_parameterTypes)!;
            EmitStringMethod(il, mi, value);
        }

        /// <summary>
        /// Logs string.
        /// </summary>
        /// <param name="text">Text string.</param>
        public static void Log(string text)
        {
            App.Log(text);
        }

        /// <summary>
        /// Logs event if it is allowed to log it. Used internally by the event logger.
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="text">Text string.</param>
        public static void LogEvent(string eventId, string text)
        {
            if(LogUtils.IsEventLogged(eventId))
                App.Log(text);
        }

        internal static void EmitEventLog(ILGenerator il, string eventId, string value)
        {
            Type[] s_parameterTypes = new Type[] { typeof(string), typeof(string) };
            MethodInfo mi = typeof(CodeGeneratorUtils).GetMethod("LogEvent", s_parameterTypes)!;
            il.Emit(OpCodes.Ldstr, eventId);
            il.Emit(OpCodes.Ldstr, value);
            il.Emit(OpCodes.Call, mi);
        }

        internal static void EmitWriteLine(ILGenerator il, string value)
        {
            const string ConsoleTypeFullName = "System.Console, System.Console";
            Type[] s_parameterTypes = new Type[] { typeof(string) };

            // Emits the IL to call Console.WriteLine with a string.
            il.Emit(OpCodes.Ldstr, value);
            Type consoleType = Type.GetType(ConsoleTypeFullName, throwOnError: true)!;
            MethodInfo mi = consoleType.GetMethod("WriteLine", s_parameterTypes)!;
            il.Emit(OpCodes.Call, mi);
        }
    }
}
