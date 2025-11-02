using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Assembly"/>, <see cref="PropertyInfo"/> and <see cref="Type"/>
    /// related static methods.
    /// </summary>
    public static partial class AssemblyUtils
    {
        /// <summary>
        /// Gets default binding flags.
        /// </summary>
        public const BindingFlags DefaultBindingFlags
            = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

        private static BaseDictionary<string, Assembly>? resNameToAssembly;
        private static int resNameToAssemblySavedLength = 0;
        private static SortedList<string, EventInfo>? allControlEvents;
        private static SortedList<string, Type>? allControlDescendants;
        private static CodeDomProvider? codeDomProvider;

        static AssemblyUtils()
        {
        }

        /// <summary>
        /// Gets or sets list of all <see cref="AbstractControl"/> descendants.
        /// </summary>
        public static SortedList<string, Type> AllControlDescendants
        {
            get
            {
                allControlDescendants
                    ??= GetSortedTypeDescendantsWithFullNames(typeof(AbstractControl));
                return allControlDescendants;
            }

            set
            {
                allControlDescendants = value;
            }
        }

        /// <summary>
        /// Gets or sets list of events for all <see cref="AbstractControl"/>
        /// descendants.
        /// </summary>
        public static SortedList<string, EventInfo> AllControlEvents
        {
            get
            {
                allControlEvents ??= AssemblyUtils.GetAllDescendantsEvents(typeof(AbstractControl));
                return allControlEvents;
            }

            set
            {
                allControlEvents = value;
            }
        }

        /// <summary>
        /// Gets whether the specified type is nullable.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns></returns>
        public static bool IsNullableType(Type type)
        {
            return type!.IsGenericType && (type!.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Gets whether the specified type is enumeration.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns></returns>
        public static bool IsEnumType(Type type)
        {
            var realType = GetRealType(type);
            return realType.IsEnum;
        }

        /// <summary>
        /// Gets whether the specified type is internal.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns></returns>
        public static bool IsInternalType(Type? type)
        {
            Type? originalType = type;
            Debug.Assert(type != null, "Type passed to IsInternalType is null");

            // If this is an internal nested type or a parent nested public type,
            // walk up the declaring types.
            while (type!.IsNestedAssembly || type.IsNestedFamORAssem
                || (originalType != type && type.IsNestedPublic))
            {
                type = type.DeclaringType;
            }

            // If we're on a non-internal nested type, IsNotPublic & IsPublic will both return false.
            // If we were originally on a nested type and have currently reached a parent
            // top-level(non nested) type, then it must be top level internal or public type.
            return type.IsNotPublic || (originalType != type && type.IsPublic);
        }

        /// <summary>
        /// Helper for determine if the type is a public.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if type is public</returns>
        public static bool IsPublicType(Type? type)
        {
            if (type is null)
                return false;

            // If this is a nested internal type, walk up the declaring types.
            while (type.IsNestedPublic)
            {
                type = type.DeclaringType;
            }

            // If we're on a non-public nested type, IsPublic will return false.
            return type.IsPublic;
        }

        /// <summary>
        /// Creates the specified type via default parameterless constructor.
        /// </summary>
        /// <typeparam name="T">Type of the object to create.</typeparam>
        /// <returns></returns>
        /// <exception cref="MissingMemberException">Raised when type
        /// does not have a public, parameterless constructor.</exception>
        public static T CreateViaDefaultConstructor<T>()
        {
            try
            {
                return Activator.CreateInstance<T>();
            }
            catch (MissingMethodException)
            {
                throw new MissingMemberException(
                    $"Type '{typeof(T)}' does not have a public, parameterless constructor.");
            }
        }

        /// <summary>
        /// Gets whether specified string is name of the event declared in
        /// any descendant of the <see cref="AbstractControl"/>.
        /// </summary>
        /// <param name="name">Property or event name.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsControlDescendantEventName(string name)
        {
            return AllControlEvents.ContainsKey(name);
        }

        /// <summary>
        /// Gets type of the second parameter for the event. This is usually an
        /// <see cref="EventArgs"/> descendant.
        /// </summary>
        /// <param name="ev">Event information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type? GetEventArgsType(EventInfo? ev)
        {
            var eventArgsType
                = ev?.EventHandlerType.GetMethod("Invoke")?.GetParameters()[1]?.ParameterType;
            return eventArgsType;
        }

        /// <summary>
        /// Gets image url for loading image resource from the specified assembly using
        /// "embres" protocol.
        /// </summary>
        /// <param name="asm">Assembly to load image from.</param>
        /// <param name="name">Image name or relative path.
        /// Slash characters will be changed to '.'.
        /// Example: "ToolBarPng/Large\Calendar32.png" -> "ToolBarPng.Large.Calendar32.png".
        /// </param>
        /// <returns></returns>
        public static string GetImageUrlInAssembly(Assembly asm, string? name = null)
        {
            name ??= string.Empty;
            name = name.Replace('/', '.');
            name = name.Replace('\\', '.');
            var resName = AssemblyUtils.GetAssemblyResPrefix(asm) + name;
            var result = $"embres:{resName}";
            return result;
        }

        /// <summary>
        /// Gets prefix string for the embedded resource.
        /// </summary>
        /// <param name="asm">Assembly.</param>
        /// <returns></returns>
        public static string GetAssemblyResPrefix(Assembly asm)
        {
            var path = asm.Location;
            var name = Path.GetFileNameWithoutExtension(path);
            var result = name + ".";
            return result;
        }

        /// <summary>
        /// Loads a set of assemblies by their names and returns those that
        /// are successfully resolved.
        /// </summary>
        /// <param name="assemblies">
        /// A collection of assembly name strings to be loaded.
        /// These can be simple names (e.g., "System.Xml")
        /// or fully qualified assembly names.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{Assembly}"/> containing the assemblies that
        /// were successfully loaded.
        /// Assemblies that cannot be resolved or are null are excluded from the result.
        /// </returns>
        /// <remarks>
        /// Uses <c>AssemblyUtils.GetOrLoadAssemblyByName</c> to perform loading.
        /// If an assembly name does not
        /// correspond to a valid or accessible assembly, it is silently skipped.
        /// </remarks>
        public static IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> assemblies)
        {
            foreach (var asm in assemblies)
            {
                var loadedAssembly = AssemblyUtils.GetOrLoadAssemblyByName(asm);
                if (loadedAssembly != null)
                    yield return loadedAssembly;
            }
        }

        /// <summary>
        /// Gets <see cref="Assembly"/> with the specified name
        /// searching it through all assemblies of the current domain.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        /// <returns></returns>
        public static Assembly? GetAssemblyByName(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var asmName = assembly.GetName().Name;
                if (asmName == name)
                    return assembly;
            }

            return null;
        }

        /// <summary>
        /// Gets <see cref="Assembly"/> with the specified name
        /// searching it through all assemblies of the current domain.
        /// If assembly is not found, tries to load it.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        /// <returns></returns>
        public static Assembly? GetOrLoadAssemblyByName(string name)
        {
            try
            {
                var result = GetAssemblyByName(name);
#pragma warning disable
                if (result is null)
                    result = Assembly.Load(name);
#pragma warning restore
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Enumerates members of all types in the loaded assemblies.
        /// </summary>
        /// <param name="bindingAttr">The bindings constraint. Optional.
        /// Default is Null. If not specified, members are not filtered
        /// by the binding constraint.</param>
        /// <returns></returns>
        /// <param name="assemblies">Collection of the assemblies to enum their members. Optional.
        /// If Null, uses all loaded assemblies.</param>
        public static IEnumerable<MemberInfo[]> GetAllPublicMembers(
            IEnumerable<Assembly>? assemblies = null,
            BindingFlags? bindingAttr = null)
        {
            Func<Type, MemberInfo[]> getMembers;

            if (bindingAttr is null)
            {
                getMembers = (type) =>
                {
                    return type.GetMembers();
                };
            }
            else
            {
                getMembers = (type) =>
                {
                    return type.GetMembers(bindingAttr.Value);
                };
            }

            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = GetExportedTypesSafe(assembly);
                foreach (var type in types)
                {
                    var members = getMembers(type);
                    yield return members;
                }
            }
        }

        /// <summary>
        /// Gets the exported types from the specified assembly safely.
        /// </summary>
        /// <param name="asm">The assembly to retrieve exported types from.</param>
        /// <returns>
        /// An array of <see cref="Type"/> objects representing the exported types in the assembly.
        /// Returns an empty array if the assembly is null, dynamic, or has no exported types.
        /// </returns>
        public static Type[] GetExportedTypesSafe(Assembly? asm)
        {
            try
            {
                if (asm is null || asm.IsDynamic)
                    return [];
                var types = asm.GetExportedTypes();
                return types;
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Enumerates members of all types in the loaded assemblies filtered
        /// by name and binding constraint.
        /// </summary>
        /// <param name="bindingAttr">The bindings constraint. Optional.
        /// Default is Null. If not specified, members are not filtered
        /// by the binding constraint.</param>
        /// <returns></returns>
        /// <param name="nameContainsText">The string which specifies member name constraint.</param>
        /// <param name="assemblies">Collection of the assemblies to enum their members. Optional.
        /// If Null, uses all loaded assemblies.</param>
        public static IEnumerable<MemberInfo> GetAllPublicMembers(
            string nameContainsText,
            IEnumerable<Assembly>? assemblies = null,
            BindingFlags? bindingAttr = null)
        {
            SortedList<string, int> addedMethods = new();
            var allMembers = GetAllPublicMembers(assemblies, bindingAttr);

            foreach (var memberArray in allMembers)
            {
                addedMethods.Clear();

                foreach (var member in memberArray)
                {
                    var type = member.DeclaringType;
                    var name = member.Name;

                    if (name == ".ctor")
                        continue;

                    if (member is MethodInfo method)
                    {
                        if (method.IsSpecialName || method.IsConstructor || method.IsAbstract)
                            continue;
                        if (addedMethods.ContainsKey(name))
                            continue;
                        addedMethods.Add(name, 0);
                    }

                    var fullName = $"{type.Name}.{name}";

                    if (fullName.IndexOf(
                        nameContainsText,
                        StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        yield return member;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Type"/> object with the specified name
        /// searching it through all assemblies of the current domain.
        /// </summary>
        /// <param name="name">Name of the type in the same format as passed to
        /// <see cref="Assembly.GetType(string)"/>.</param>
        /// <param name="reverse">if <c>true</c> gives priority to most recently loaded types.
        /// Optional. Default is <c>false</c>.</param>
        /// <returns></returns>
        public static Type? GetTypeByName(string name, bool reverse = false)
        {
            IEnumerable<Assembly> assemblies;

            if (reverse)
                assemblies = AppDomain.CurrentDomain.GetAssemblies().Reverse();
            else
                assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var tt = assembly.GetType(name);
                if (tt != null)
                {
                    return tt;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets whether property item can be reset.
        /// </summary>
        /// <param name="instance">Object instance with the property.</param>
        /// <param name="propInfo">Property info.</param>
        /// <returns>True if property can be reset.</returns>
        /// <returns></returns>
        /// <remarks>
        /// Property can be reset if it is nullable, has <see cref="DefaultValueAttribute"/>
        /// specified or class has reset method which
        /// starts with 'Reset' and has property name at the end.
        /// </remarks>
        public static bool CanResetProp(object? instance, PropertyInfo? propInfo)
        {
            if (propInfo is null || instance is null)
                return false;
            var nullable = AssemblyUtils.GetNullable(propInfo);
            var value = propInfo.GetValue(instance);
            var resetMethod = AssemblyUtils.GetResetPropMethod(instance, propInfo.Name);
            var hasDefaultAttr = AssemblyUtils.GetDefaultValue(propInfo, out _);
            return hasDefaultAttr || resetMethod != null || (nullable && value is not null);
        }

        /// <summary>
        /// Resets property value using the specified instance and property info.
        /// Uses different methods to reset the property including: calling reset method,
        /// assigning default value from the attribute, assigning null value if property is nullable.
        /// </summary>
        /// <param name="instance">Object instance with the property.</param>
        /// <param name="propInfo">Property info.</param>
        /// <returns>True if property was reset.</returns>
        public static bool ResetProperty(object? instance, PropertyInfo? propInfo)
        {
            if (instance is null || propInfo is null)
                return false;

            var resetMethod = AssemblyUtils.GetResetPropMethod(instance, propInfo.Name);
            if (resetMethod is not null)
            {
                resetMethod.Invoke(instance, []);
                return true;
            }

            var hasDefaultAttr = AssemblyUtils.GetDefaultValue(propInfo, out var defValue);
            if (hasDefaultAttr)
            {
                propInfo.SetValue(instance, defValue);
                return true;
            }

            var nullable = AssemblyUtils.GetNullable(propInfo);
            var value = propInfo.GetValue(instance);
            if (nullable && value is not null)
            {
                propInfo.SetValue(instance, null);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates <see cref="Action"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="instance">Object which contains the method.</param>
        /// <param name="method">Method which needs to be converted to <see cref="Action"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action CreateAction(object instance, MethodInfo method)
        {
            Action action = (Action)Delegate.CreateDelegate(typeof(Action), instance, method);
            return action;
        }

        /// <summary>
        /// Gets reset method for the specified property.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static MethodInfo? GetResetPropMethod(object instance, string propName)
        {
            var methodName = "Reset" + propName;
            var type = instance.GetType();
            var result = type.GetMethod(methodName, Array.Empty<Type>());
            return result;
        }

        /// <summary>
        /// Gets whether specified type has declared events (events declared in the parent
        /// types are not counted).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns></returns>
        public static bool HasOwnEvents(Type? type)
        {
            if (type == null)
                return false;
            var events = AssemblyUtils.EnumEvents(type, true);

            foreach (var item in events)
            {
                if (item.DeclaringType != type)
                    continue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Enumerates event information for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type which events are enumerated.</param>
        /// <param name="sort">Defines whether to sort result events by name.</param>
        /// <param name="bindingFlags">Specifies flags that control the way in which
        /// the search for members is conducted.</param>
        /// <returns></returns>
        public static IEnumerable<EventInfo> EnumEvents(
            Type type,
            bool sort = false,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            List<EventInfo> result = new();

            var props = new List<EventInfo>(type.GetEvents(bindingFlags));

            SortedList<string, EventInfo> addedNames = new();

            foreach (var p in props)
            {
                var propName = p.Name;
                if (addedNames.ContainsKey(propName))
                    continue;
                result.Add(p);
                addedNames.Add(propName, p);
            }

            if (sort)
                return addedNames.Values;
            else
                return result;
        }

        /// <summary>
        /// Safely retrieves the location of the specified assembly.
        /// This method catches all exception raised by <see cref="Assembly.Location"/>.
        /// </summary>
        /// <param name="assembly">The assembly whose location is to be retrieved.</param>
        /// <returns>
        /// The location of the assembly as a string, or <c>null</c> if the location
        /// cannot be determined.
        /// </returns>
        public static string? GetLocationSafe(Assembly? assembly)
        {
            try
            {
                if (assembly is null || assembly.IsDynamic)
                    return null;
                return assembly.Location;
            }
            catch (Exception e)
            {
                BaseObject.Nop(e);
                return null;
            }
        }

        /// <summary>
        /// Compares two specified <see cref="EventInfo"/> objects by their names
        /// and returns an
        /// integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">First item to compare.</param>
        /// <param name="y">Second item to compare.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relationship between the two
        /// comparands. Result value less than 0 means that <paramref name="x"/> precedes
        /// <paramref name="y"/> in the sort order.
        /// Result value equal to 0 means <paramref name="x"/> occurs in the same
        /// position as <paramref name="y"/>
        /// in the sort order. Result value greater than 0 means that <paramref name="x"/> follows
        /// <paramref name="y"/> in the sort order.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareByName(EventInfo x, EventInfo y)
        {
            return string.Compare(x.Name, y.Name);
        }

        /// <summary>
        /// Compares two specified <see cref="PropertyInfo"/> objects by their names
        /// and returns an
        /// integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">First item to compare.</param>
        /// <param name="y">Second item to compare.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relationship between the two
        /// comparands. Result value less than 0 means that <paramref name="x"/> precedes
        /// <paramref name="y"/> in the sort order.
        /// Result value equal to 0 means <paramref name="x"/> occurs in the same
        /// position as <paramref name="y"/>
        /// in the sort order. Result value greater than 0 means that <paramref name="x"/> follows
        /// <paramref name="y"/> in the sort order.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareByName(PropertyInfo x, PropertyInfo y)
        {
            return string.Compare(x.Name, y.Name);
        }

        /// <summary>
        /// Enumerates method information for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type which methods are enumerated.</param>
        /// <param name="bindingFlags">Specifies flags that control the way in which
        /// the search for methods is conducted.</param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> EnumMethods(
            Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            var items = type.GetMethods(bindingFlags);

            SortedList<string, MethodInfo> addedNames = new();

            foreach (var p in items)
            {
                var itemName = p.Name;
                if (addedNames.ContainsKey(itemName))
                    continue;
                addedNames.Add(itemName, p);
            }

            return addedNames.Values;
        }

        /// <summary>
        /// Enumerates property information for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type which properties are enumerated.</param>
        /// <param name="sort">Defines whether to sort returned properties by name.</param>
        /// <param name="bindingFlags">Specifies flags that control the way in which
        /// the search for members is conducted.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> EnumProps(
            Type type,
            bool sort = false,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            List<PropertyInfo> result = new();

            var props = new List<PropertyInfo>(type.GetProperties(bindingFlags));

            SortedList<string, PropertyInfo> addedNames = new();

            foreach (var p in props)
            {
                var propName = p.Name;
                if (addedNames.ContainsKey(propName))
                    continue;
                result.Add(p);
                addedNames.Add(propName, p);
            }

            if (sort)
                return addedNames.Values;
            else
                return result;
        }

        /// <summary>
        /// Gets collection of the fields and properties marked with
        /// <see cref="AutoResetAttribute"/> attribute.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetAutoResetMembers(
            Type type,
            bool enumFields = true,
            bool enumProps = true,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public
            | BindingFlags.NonPublic)
        {
            var fields = AssemblyUtils.EnumFields(type, false, bindingFlags);
            var props = AssemblyUtils.EnumFields(type, false, bindingFlags);

            foreach (var item in fields)
            {
                if (AssemblyUtils.IsAutoReset(item))
                    yield return item;
            }

            foreach (var item in props)
            {
                if (AssemblyUtils.IsAutoReset(item))
                    yield return item;
            }
        }

        /// <summary>
        /// Enumerates field information for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type which fields are enumerated.</param>
        /// <param name="sort">Defines whether to sort returned fields by name.</param>
        /// <param name="bindingFlags">Specifies flags that control the way in which
        /// the search for members is conducted.</param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> EnumFields(
            Type type,
            bool sort = false,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            List<FieldInfo> result = new();

            var fields = new List<FieldInfo>(type.GetFields(bindingFlags));

            SortedList<string, FieldInfo> addedNames = new();

            foreach (var p in fields)
            {
                var name = p.Name;
                if (addedNames.ContainsKey(name))
                    continue;
                result.Add(p);
                addedNames.Add(name, p);
            }

            if (sort)
                return addedNames.Values;
            else
                return result;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> has a public instance constructor
        /// whose parameters match the types in the specified array.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="paramTypes">Array of constructor parameter types.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasConstructorWithParams(Type type, Type[] paramTypes)
        {
            var result = type.GetConstructor(paramTypes);
            return result != null;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> has a public instance constructor
        /// with single parameter which has the specified type.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="paramType">Constructor parameter type.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasConstructorWithParam(Type type, Type paramType)
        {
            var result = type.GetConstructor([paramType]);
            return result != null;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> has constructor without parameters.
        /// </summary>
        /// <param name="type">Object type.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasConstructorNoParams(Type type)
        {
            var result = type.GetConstructor(Type.EmptyTypes);
            return result != null;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> is struct.
        /// </summary>
        /// <param name="type">Object type.</param>
        public static bool IsStruct(Type? type)
        {
            if (type is null)
                return false;
            var realType = AssemblyUtils.GetRealType(type);
            TypeCode typeCode = Type.GetTypeCode(realType);
            var result = (typeCode == TypeCode.Object) && realType.IsValueType && !realType.IsEnum;
            return result;
        }

        /// <summary>
        /// Gets field value using the specified object instance and field info.
        /// </summary>
        /// <param name="instance">Instance which contains the field.
        /// Specify Null, for the static fields.</param>
        /// <param name="fieldInfo">Field info.</param>
        /// <param name="defValue">Default field value (used if field value is null).</param>
        /// <typeparam name="T">Type of result.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetFieldValue<T>(object? instance, FieldInfo fieldInfo, T defValue)
        {
            object? result = fieldInfo.GetValue(instance);
            if (result == null)
                return defValue;
            else
                return (T)result;
        }

        /// <summary>
        /// Gets property value using the specified object instance and property info.
        /// </summary>
        /// <param name="instance">Instance which contains the property.
        /// Specify Null, for the static properties.</param>
        /// <param name="propInfo">Property info.</param>
        /// <param name="defValue">Default property value (used if property value is null).</param>
        /// <typeparam name="T">Type of result.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetPropValue<T>(object? instance, PropertyInfo propInfo, T defValue)
        {
            object? result = propInfo.GetValue(instance, null);
            if (result == null)
                return defValue;
            else
                return (T)result;
        }

        /// <summary>
        /// Toggles boolean field or property specified by the type (or instance) and name.
        /// </summary>
        /// <param name="instance">Instance which contains the property.
        /// Specify Null, for the static properties.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="type">Type of the object with the property.</param>
        public static bool? ToggleBoolMember(Type? type, object? instance, string propName)
        {
            var success = TryGetMemberValue(
                type,
                instance,
                propName,
                out object? result);

            if (!success || result is not bool)
                return null;

            var value = !(bool)result;

            TrySetMemberValue(
                type,
                instance,
                propName,
                value);

            return value;
        }

        /// <summary>
        /// Gets field or property information using the specified object type
        /// (or instance) and property name.
        /// Type of the object or the object itself must be specified.
        /// </summary>
        /// <param name="propName">Property name.</param>
        /// <param name="bindingFlags">Specifies binding flags used when member is searched.
        /// Optional.</param>
        /// <param name="memberTypes">Specifies member types to search.
        /// Optional. By default searches for the fields and properties.</param>
        /// <param name="type">Type of the object with the property.</param>
        /// <returns></returns>
        public static MemberInfo? GetFirstMember(
            Type? type,
            string propName,
            MemberTypes memberTypes = MemberTypes.Field | MemberTypes.Property,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.Static)
        {
            var members = type?.GetMember(
                propName,
                memberTypes,
                bindingFlags);

            if (members is null || members.Length == 0)
                return null;

            var member = members[0];

            return member;
        }

        /// <summary>
        /// Sets property or field value using the specified object type
        /// (or instance) and property name.
        /// Type of the object or the object itself must be specified.
        /// </summary>
        /// <param name="instance">Instance which contains the property or field.
        /// Specify Null, for the static properties.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="value">New property value.</param>
        public static bool TrySetMemberValue(
            object? instance,
            string propName,
            object? value)
        {
            return TrySetMemberValue(null, instance, propName, value);
        }

        /// <summary>
        /// Sets property or field value using the specified object type
        /// (or instance) and property name.
        /// Type of the object or the object itself must be specified.
        /// </summary>
        /// <param name="instance">Instance which contains the property or field.
        /// Specify Null, for the static properties.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="type">Type of the object with the property.</param>
        /// <param name="value">New property value.</param>
        public static bool TrySetMemberValue(
            Type? type,
            object? instance,
            string propName,
            object? value)
        {
            var member = GetFirstMember(type ?? instance?.GetType(), propName);

            if (member is PropertyInfo propInfo)
            {
                propInfo.SetValue(instance, value);
                return true;
            }
            else
            if (member is FieldInfo fieldInfo)
            {
                fieldInfo.SetValue(instance, value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets property or field value using the specified object type
        /// (or instance) and property name.
        /// Type of the object or the object itself must be specified.
        /// </summary>
        /// <param name="instance">Instance which contains the property or field.
        /// Specify Null, for the static properties.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="type">Type of the object with the property.</param>
        /// <param name="result">Property value.</param>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="defValue">Default property value (used if member not found).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetMemberValue<T>(
            Type? type,
            object? instance,
            string propName,
            out T result,
            T defValue = default!)
        {
            var member = GetFirstMember(type ?? instance?.GetType(), propName);

            if (member is PropertyInfo propInfo)
            {
                result = GetPropValue(instance, propInfo, defValue);
                return true;
            }
            else
            if (member is FieldInfo fieldInfo)
            {
                result = GetFieldValue(instance, fieldInfo, defValue);
                return true;
            }

            result = defValue;
            return false;
        }

        /// <summary>
        /// Gets real type, using <see cref="Nullable.GetUnderlyingType"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns><see cref="Nullable.GetUnderlyingType"/> if its not null or
        /// <paramref name="type"/> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetRealType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            var realType = underlyingType ?? type;
            return realType;
        }

        /// <summary>
        /// Gets <see cref="TypeCode"/> of the real type using <see cref="GetRealType"/>
        /// and <see cref="Type.GetTypeCode"/>.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeCode GetRealTypeCode(Type type)
        {
            var realType = GetRealType(type);
            var typeCode = Type.GetTypeCode(realType);
            return typeCode;
        }

        /// <summary>
        /// Gets property info.
        /// </summary>
        /// <param name="instance">Object instance.</param>
        /// <param name="name">Property name.</param>
        public static PropertyInfo? GetPropInfo(object? instance, string? name)
        {
            if (instance == null || name is null || name.Length == 0)
                return null;
            var type = instance.GetType();
            var propInfo = GetPropertySafe(type, name);
            return propInfo;
        }

        /// <summary>
        /// Same as <see cref="Type.GetProperty(string)"/> but doesn't raise exceptions.
        /// </summary>
        /// <param name="type">Type where to search for the property.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="bindingFlags">Binding flags used when property
        /// is searched. Optional.</param>
        /// <returns></returns>
        public static PropertyInfo? GetPropertySafe(
            Type? type,
            string propName,
            BindingFlags bindingFlags = DefaultBindingFlags)
        {
            var members = type?.GetMember(
                propName,
                MemberTypes.Property,
                bindingFlags);

            if (members is null || members.Length == 0)
                return null;

            var member = members[0] as PropertyInfo;

            return member;
        }

        /// <summary>
        /// Gets minimal possible variable value for the given <see cref="TypeCode"/>.
        /// </summary>
        public static object? GetMinValue(TypeCode code)
        {
            return code switch
            {
                TypeCode.SByte => MinValueSByte,
                TypeCode.Byte => MinValueByte,
                TypeCode.Int16 => MinValueInt16,
                TypeCode.UInt16 => MinValueUInt16,
                TypeCode.Int32 => MinValueInt32,
                TypeCode.UInt32 => MinValueUInt32,
                TypeCode.Int64 => MinValueInt64,
                TypeCode.UInt64 => MinValueUInt64,
                TypeCode.Single => MinValueSingle,
                TypeCode.Double => MinValueDouble,
                TypeCode.Decimal => MinValueDecimal,
                TypeCode.DateTime => MinValueDateTime,
                _ => null,
            };
        }

        /// <summary>
        /// Returns minimal and maximal possible values for the given <see cref="TypeCode"/>
        /// as a range string using specified <paramref name="format"/>.
        /// </summary>
        /// <remarks>
        /// For example, if format is "{0}..{1}", result is "0..255".
        /// </remarks>
        public static string? GetMinMaxRangeStr(
            TypeCode code,
            string? format = null,
            object? minValue = null,
            object? maxValue = null,
            bool needUnsigned = false)
        {
            var minValueT = MathUtils.Max(GetMinValue(code), minValue);

            if (needUnsigned)
            {
                minValueT = MathUtils.Max(minValueT, GetDefaultValue(code));
            }

            var maxValueT = MathUtils.Min(GetMaxValue(code), maxValue);
            if (minValueT is null || maxValueT is null)
                return null;
            format ??= "{0}..{1}";
            return string.Format(format, minValueT, maxValueT);
        }

        /// <summary>
        /// Upgrades signed integers to <see cref="long"/>, unsigned integers to
        /// <see cref="ulong"/> and returns all other types as is.
        /// </summary>
        /// <param name="value">Value of any type.</param>
        public static object UpgradeNumberType(object value)
        {
            var typeCode = AssemblyUtils.GetRealTypeCode(value.GetType());
            return typeCode switch
            {
                TypeCode.SByte => LongOrULong((long)((sbyte)value)),
                TypeCode.Int16 => LongOrULong((long)((short)value)),
                TypeCode.Int32 => LongOrULong((long)((int)value)),
                TypeCode.Byte => (ulong)((byte)value),
                TypeCode.UInt16 => (ulong)((ushort)value),
                TypeCode.UInt32 => (ulong)((uint)value),
                _ => value,
            };

            static object LongOrULong(long longValue)
            {
                if (longValue < 0)
                    return longValue;
                return (ulong)longValue;
            }
        }

        /// <summary>
        /// Gets default <see cref="NumberStyles"/> for the specified <paramref name="typeCode"/>.
        /// </summary>
        public static NumberStyles GetDefaultNumberStyles(TypeCode typeCode)
        {
            const NumberStyles NumberStylesInt = NumberStyles.Integer;
            const NumberStyles NumberStylesFloat = NumberStyles.Float | NumberStyles.AllowThousands;

            return typeCode switch
            {
                TypeCode.SByte => NumberStylesInt,
                TypeCode.Byte => NumberStylesInt,
                TypeCode.Int16 => NumberStylesInt,
                TypeCode.UInt16 => NumberStylesInt,
                TypeCode.Int32 => NumberStylesInt,
                TypeCode.UInt32 => NumberStylesInt,
                TypeCode.Int64 => NumberStylesInt,
                TypeCode.UInt64 => NumberStylesInt,

                TypeCode.Single => NumberStylesFloat,
                TypeCode.Double => NumberStylesFloat,

                TypeCode.Decimal => NumberStyles.Number,

                _ => NumberStyles.None,
            };
        }

        /// <summary>
        /// Gets default value for the given <see cref="TypeCode"/>.
        /// </summary>
        public static object? GetDefaultValue(TypeCode code)
        {
            return code switch
            {
                TypeCode.SByte => DefaultSByte,
                TypeCode.Byte => DefaultByte,
                TypeCode.Int16 => DefaultInt16,
                TypeCode.UInt16 => DefaultUInt16,
                TypeCode.Int32 => DefaultInt32,
                TypeCode.UInt32 => DefaultUInt32,
                TypeCode.Int64 => DefaultInt64,
                TypeCode.UInt64 => DefaultUInt64,
                TypeCode.Single => DefaultSingle,
                TypeCode.Double => DefaultDouble,
                TypeCode.Decimal => DefaultDecimal,
                TypeCode.DateTime => DefaultDateTime,
                _ => null,
            };
        }

        /// <summary>
        /// Gets maximal possible variable value for the given <see cref="TypeCode"/>.
        /// </summary>
        public static Type? TypeFromTypeCode(TypeCode code)
        {
            return code switch
            {
                TypeCode.SByte => typeof(sbyte),
                TypeCode.Byte => typeof(byte),
                TypeCode.Int16 => typeof(short),
                TypeCode.UInt16 => typeof(ushort),
                TypeCode.Int32 => typeof(int),
                TypeCode.UInt32 => typeof(uint),
                TypeCode.Int64 => typeof(long),
                TypeCode.UInt64 => typeof(ulong),
                TypeCode.Single => typeof(float),
                TypeCode.Double => typeof(double),
                TypeCode.Decimal => typeof(decimal),
                TypeCode.DateTime => typeof(DateTime),
                _ => null,
            };
        }

        /// <summary>
        /// Gets maximal possible variable value for the given <see cref="TypeCode"/>.
        /// </summary>
        public static object? GetMaxValue(TypeCode code)
        {
            return code switch
            {
                TypeCode.SByte => MaxValueSByte,
                TypeCode.Byte => MaxValueByte,
                TypeCode.Int16 => MaxValueInt16,
                TypeCode.UInt16 => MaxValueUInt16,
                TypeCode.Int32 => MaxValueInt32,
                TypeCode.UInt32 => MaxValueUInt32,
                TypeCode.Int64 => MaxValueInt64,
                TypeCode.UInt64 => MaxValueUInt64,
                TypeCode.Single => MaxValueSingle,
                TypeCode.Double => MaxValueDouble,
                TypeCode.Decimal => MaxValueDecimal,
                TypeCode.DateTime => MaxValueDateTime,
                _ => null,
            };
        }

        /// <summary>
        /// Returns whether property is nullable (for example byte?).
        /// </summary>
        /// <param name="p">Property info.</param>
        /// <returns><c>true</c> if property is nullable, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetNullable(PropertyInfo p)
        {
            var propType = p.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propType);
            var isNullable = underlyingType != null;
            return isNullable;
        }

        /// <summary>
        /// Returns whether property, method or other member is browsable.
        /// </summary>
        /// <param name="p">Member info.</param>
        /// <returns><c>true</c> if property is browsable, <c>false</c> otherwise.</returns>
        public static bool GetBrowsable(MemberInfo p)
        {
            var browsable = p.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute;
            if (browsable is not null)
                return browsable.Browsable;
            return true;
        }

        /// <summary>
        /// Returns whether type is browsable.
        /// </summary>
        /// <param name="p">The type to check.</param>
        /// <returns><c>true</c> if type is browsable, <c>false</c> otherwise.</returns>
        public static bool TypeIsBrowsable(Type p)
        {
            var browsable = p.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute;
            if (browsable is not null)
                return browsable.Browsable;
            return true;
        }

        /// <summary>
        /// Returns whether property or field has <see cref="AutoResetAttribute"/> attribute
        /// and it's value property is True.
        /// </summary>
        /// <param name="p">Member info.</param>
        public static bool IsAutoReset(MemberInfo p)
        {
            var attribute = p.GetCustomAttribute<AutoResetAttribute>(true);
            if (attribute is not null)
                return attribute.Value;
            return false;
        }

        /// <summary>
        /// Gets value of thr <see cref="DescriptionAttribute.Description"/>
        /// for the specified member.
        /// </summary>
        /// <param name="p">Property or method info.</param>
        /// <returns></returns>
        public static string? GetDescription(MemberInfo p)
        {
            var attr = p.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attr?.Description;
        }

        /// <summary>
        /// Gets whether toolbox category of the control is 'Hidden'.
        /// </summary>
        /// <param name="type">Type of the control.</param>
        /// <returns></returns>
        public static bool IsControlCategoryHidden(Type type)
        {
            var category = AssemblyUtils.GetControlCategory(type);
            if (category is not null && category.IsHidden)
                return true;
            return false;
        }

        /// <summary>
        /// Gets whether toolbox category of the control is 'Internal'.
        /// </summary>
        /// <param name="type">Type of the control.</param>
        /// <returns></returns>
        public static bool IsControlCategoryInternal(Type type)
        {
            var category = AssemblyUtils.GetControlCategory(type);
            if (category is not null && category.IsInternal)
                return true;
            return false;
        }

        /// <summary>
        /// Returns toolbox category of the control.
        /// </summary>
        /// <param name="type">Type of the control.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ControlCategoryAttribute? GetControlCategory(Type type)
        {
            var attr = type.GetCustomAttribute(typeof(ControlCategoryAttribute))
                as ControlCategoryAttribute;
            return attr;
        }

        /// <summary>
        /// Gets default value of the property using <see cref="DefaultValueAttribute"/>.
        /// </summary>
        /// <param name="p">Property information.</param>
        /// <param name="defValue">Default value of the property.</param>
        /// <returns></returns>
        public static bool GetDefaultValue(PropertyInfo p, out object? defValue)
        {
            var attr = p.GetCustomAttribute(typeof(DefaultValueAttribute));
            if (attr is not DefaultValueAttribute defaultValueAttr)
            {
                defValue = null;
                return false;
            }

            defValue = defaultValueAttr.Value;
            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="typeCode"/> is number type.
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTypeCodeNumber(TypeCode typeCode)
        {
            return typeCode >= TypeCode.SByte && typeCode <= TypeCode.Decimal;
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="typeCode"/> is an unsigned integer number type
        /// (byte, ushort, uint, ulong).
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static bool IsTypeCodeUnsignedInt(TypeCode typeCode)
        {
            switch (typeCode)
            {
                default:
                    return false;
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="typeCode"/> is a signed integer number type
        /// (sbyte, short, int, long).
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static bool IsTypeCodeSignedInt(TypeCode typeCode)
        {
            switch (typeCode)
            {
                default:
                    return false;
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="typeCode"/> is a floating point number type
        /// (float, double or decimal).
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static bool IsTypeCodeFloat(TypeCode typeCode)
        {
            switch (typeCode)
            {
                default:
                    return false;
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
            }
        }

        /// <summary>
        /// Returns whether enumeration has <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <param name="type">Type of enumeration.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EnumIsFlags(Type type)
        {
            var flags = type.GetCustomAttribute(typeof(FlagsAttribute)) as FlagsAttribute;
            if (flags is not null)
                return true;
            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="type"/> is a descendant of any type
        /// in the array of types.
        /// </summary>
        /// <remarks>This method checks all base types recursively not only
        /// the first <see cref="Type.BaseType"/> value.</remarks>
        /// <param name="type">Type to check.</param>
        /// <param name="baseTypes">Base types array.</param>
        public static bool TypeIsDescendant(Type type, Type[] baseTypes)
        {
            foreach (var item in baseTypes)
            {
                if (TypeIsDescendant(type, item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if specified type equals any type from <paramref name="baseTypes"/>
        /// or is a descendant of that type.
        /// </summary>
        /// <remarks>This method checks all base types recursively not only
        /// the first <see cref="Type.BaseType"/> value.</remarks>
        /// <param name="type">Type to check.</param>
        /// <param name="baseTypes">Base types array.</param>
        public static bool TypeEqualsOrDescendant(Type type, Type[] baseTypes)
        {
            foreach (var item in baseTypes)
            {
                if (type == item)
                    return true;
            }

            foreach (var item in baseTypes)
            {
                if (TypeIsDescendant(type, item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if specified type equals <paramref name="baseType"/>
        /// or is a descendant of that type.
        /// </summary>
        /// <remarks>This method checks all base types recursively not only
        /// the first <see cref="Type.BaseType"/> value.</remarks>
        /// <param name="type">Type to check.</param>
        /// <param name="baseType">Base type.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TypeEqualsOrDescendant(Type type, Type baseType)
        {
            var result = type == baseType || TypeIsDescendant(type, baseType);
            return result;
        }

        /// <summary>
        /// Returns <c>true</c> if specified type is a descendant of another type.
        /// </summary>
        /// <remarks>This method checks all base types recursively not only
        /// the first <see cref="Type.BaseType"/> value.</remarks>
        /// <param name="type">Descendant type.</param>
        /// <param name="baseTypeToCheck">Base type.</param>
        /// <returns></returns>
        public static bool TypeIsDescendant(Type type, Type baseTypeToCheck)
        {
            while (true)
            {
                if (type == null)
                    return false;

                var baseType = type.BaseType;

                if (baseType == baseTypeToCheck)
                    return true;

                if (type.IsInterface)
                {
                    if (baseType == null)
                        return false;
                }

                if (type.IsClass)
                {
                    if (baseType == typeof(object))
                        return false;
                }

                type = baseType!;
            }
        }

        /// <summary>
        /// Gets whether property is readonly.
        /// </summary>
        /// <param name="prop">Property information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsReadOnly(PropertyInfo prop)
        {
            var result = prop.SetMethod == null || !prop.SetMethod.IsPublic;
            return result;
        }

        /// <summary>
        /// Runs the action for all descendants of the specified type.
        /// </summary>
        /// <param name="baseType">The base type which descendants are passed to the action.</param>
        /// <param name="action">The action to call for the each descendant
        /// of the specified type.</param>
        public static void RunActionForDerivedTypes(
            Type baseType,
            Action<Type> action)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var descendants = GetTypeDescendants(baseType, assemblies);
            foreach (var type in descendants)
            {
                action(type);
            }
        }

        /// <summary>
        /// Returns list of types which descend from the specified type.
        /// Search is performed in the collection of assemblies.
        /// </summary>
        /// <param name="type">Base type.</param>
        /// <param name="assemblies">The collection of assemblies where search is performed.</param>
        public static IEnumerable<Type> GetTypeDescendants(Type type, IEnumerable<Assembly> assemblies)
        {
            foreach (var asm in assemblies)
            {
                if (asm is null)
                    continue;
                var result = GetTypeDescendants(type, asm);
                foreach (var item in result)
                    yield return item;
            }
        }

        /// <summary>
        /// Returns list of types which descend from the specified type.
        /// </summary>
        /// <param name="type">The base type.</param>
        /// <param name="asm">The assembly in which search is performed. If it is null,
        /// search is done in the assembly where base type is declared</param>
        public static IEnumerable<Type> GetTypeDescendants(Type type, Assembly? asm = null)
        {
            List<Type> result = new();

            asm ??= type.Assembly;

            var definedTypes = asm.DefinedTypes;

            foreach (TypeInfo typeInfo in definedTypes)
            {
                var resultType = typeInfo.AsType();
                if (resultType.IsAbstract || !resultType.IsPublic)
                    continue;
                if (TypeIsDescendant(resultType, type))
                    result.Add(resultType);
            }

            return result;
        }

        /// <summary>
        /// Returns list of types which descend from specified type.
        /// Result is sorted by <see cref="Type.FullName"/> and uses it as keys.
        /// </summary>
        /// <param name="type">Base type.</param>
        /// <returns></returns>
        public static SortedList<string, Type> GetSortedTypeDescendantsWithFullNames(Type type)
        {
            SortedList<string, Type> result = new();
            var types = GetTypeDescendants(type);
            foreach (var item in types)
            {
                if(item.FullName is null)
                    continue;

                result.Add(item.FullName, item);
            }

            return result;
        }

        /// <summary>
        /// Finds assembly for the specified resource name.
        /// </summary>
        /// <param name="resName">Resource name.</param>
        /// <returns></returns>
        public static Assembly? FindAssemblyForResource(string resName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (resNameToAssembly is null || assemblies.Length != resNameToAssemblySavedLength)
            {
                resNameToAssembly ??= new();
                resNameToAssemblySavedLength = assemblies.Length;

                foreach (var assembly in assemblies)
                {
                    if (assembly.IsDynamic)
                        continue;

                    var resources = assembly.GetManifestResourceNames();

                    if (resources.Length == 0)
                        continue;

                    foreach (var resource in resources)
                    {
                        resNameToAssembly.TryAdd(resource, assembly);
                    }
                }
            }

            if (resNameToAssembly.TryGetValue(resName, out var result))
                return result;
            return null;
        }

        /// <summary>
        /// Enumerates events in all descendants of the specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="bindingFlags">Binding flags.</param>
        /// <returns></returns>
        public static SortedList<string, EventInfo> GetAllDescendantsEvents(
            Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            IEnumerable<Type> types = GetTypeDescendants(type);

            var result = new SortedList<string, EventInfo>();

            foreach (var item in types)
            {
                IEnumerable<EventInfo> events = EnumEvents(item, false, bindingFlags);
                foreach (var ev in events)
                {
                    var name = ev.Name;
                    if (result.ContainsKey(name))
                        continue;
                    result.Add(name, ev);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets device the app is running on, such as a desktop computer or a tablet.
        /// Uses invoke of 'MauiUtils.GetDeviceType'.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenericDeviceType InvokeMauiUtilsGetDeviceType()
        {
            var result = InvokeMethodWithResult(KnownTypes.MauiUtils.Value, "GetDeviceType");
            if (result is null)
            {
                if (App.IsLinuxOS || App.IsWindowsOS || App.IsMacOS)
                    return GenericDeviceType.Desktop;
                else
                    return GenericDeviceType.Unknown;
            }

            return (GenericDeviceType)result;
        }

        /// <summary>
        /// Gets device platform using invoke of 'MauiUtils.GetDevicePlatform'.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object? InvokeMauiUtilsGetDevicePlatform()
        {
            var result = InvokeMethodWithResult(KnownTypes.MauiUtils.Value, "GetDevicePlatform");
            return result;
        }

        /// <summary>
        /// Gets whether device platform is MacCatalyst using invoke
        /// of Maui platform code if it is present.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InvokeMauiUtilsIsMacCatalyst()
        {
            var asm = KnownAssemblies.LibraryMicrosoftMaui.Value;
            if (asm is null)
                return false;

            var essentials = KnownAssemblies.LibraryMicrosoftMauiEssentials.Value;
            if (essentials is null)
                return false;

            var devicePlatform = essentials.GetType("Microsoft.Maui.Devices.DevicePlatform");
            if (devicePlatform is null)
                return false;

            var deviceInfo = essentials.GetType("Microsoft.Maui.Devices.DeviceInfo");
            if (deviceInfo is null)
                return false;

            var devicePlatformMacCatalyst =
                devicePlatform.GetProperty("MacCatalyst")?.GetValue(null);
            if (devicePlatformMacCatalyst is null)
                return false;

            var deviceInfoCurrent = deviceInfo.GetProperty("Current")?.GetValue(null);

            var deviceInfoCurrentPlatform = deviceInfoCurrent?.GetType()
                .GetProperty("Platform")?.GetValue(deviceInfoCurrent);

            var result = deviceInfoCurrentPlatform?.Equals(devicePlatformMacCatalyst) ?? false;
            return result;
        }

        /// <summary>
        /// Invokes 'OperatingSystem.IsAndroid' through the reflection.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? InvokeIsAndroid()
        {
            var result = InvokeBoolMethod<OperatingSystem>("IsAndroid");
            return result;
        }

        /// <summary>
        /// Invokes 'OperatingSystem.IsAndroid' through the reflection.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? InvokeIsIOS()
        {
            var result = InvokeBoolMethod<OperatingSystem>("IsIOS");
            return result;
        }

        /// <summary>
        /// Invokes method with boolean result through the reflection.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="methodName">Method name.</param>
        /// <param name="obj">Object instance. Optional. Default is null.</param>
        /// <param name="param">Method parameters. Optional. Default is null.</param>
        /// <returns>
        /// <c>null</c> if method not found; otherwise result of the method invoke.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? InvokeBoolMethod<T>(
            string methodName,
            object? obj = null,
            object[]? param = null)
        {
            return InvokeBoolMethod(typeof(T), methodName, obj, param);
        }

        /// <summary>
        /// Invokes method with boolean result through the reflection.
        /// </summary>
        /// <param name="type">Type where to search the method.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="obj">Object instance. Optional. Default is null.</param>
        /// <param name="param">Method parameters. Optional. Default is null.</param>
        /// <returns>
        /// <c>null</c> if method not found; otherwise result of the method invoke.
        /// </returns>
        public static bool? InvokeBoolMethod(
            Type? type,
            string methodName,
            object? obj = null,
            object[]? param = null)
        {
            try
            {
                var method = type?.GetMethod(methodName);

                if (method is not null)
                {
                    var result = (bool?)method.Invoke(obj, param);
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Invokes method with object result through the reflection.
        /// </summary>
        /// <param name="type">Type where to search the method.</param>
        /// <param name="methodName">Method name</param>
        /// <param name="obj">Object instance. Optional. Default is null.</param>
        /// <param name="param">Method parameters. Optional. Default is null.</param>
        /// <returns>
        /// <c>null</c> if method not found; otherwise result of the method invoke.
        /// </returns>
        public static object? InvokeMethodWithResult(
            Type? type,
            string methodName,
            object? obj = null,
            object?[]? param = null)
        {
            try
            {
                var method = type?.GetMethod(methodName);

                if (method is not null)
                {
                    var result = method.Invoke(obj, param);
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Finds a non-generic method in the specified type by name and exact parameter types.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for the method.</param>
        /// <param name="methodName">The name of the method to find.</param>
        /// <param name="parameterTypes">An array of <see cref="Type"/> objects
        /// representing the method's parameters.</param>
        /// <returns>
        /// A <see cref="MethodInfo"/> representing the matched non-generic method,
        /// or <c>null</c> if no matching method is found.
        /// </returns>
        public static MethodInfo? FindNonGenericMethod(
            Type type,
            string methodName,
            Type[] parameterTypes)
        {
            return type.GetMethods(DefaultBindingFlags)
                .Where(m => m.Name == methodName && !m.IsGenericMethod)
                .FirstOrDefault(m =>
                {
                    var parameters = m.GetParameters();
                    if (parameters.Length != parameterTypes.Length)
                        return false;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i].ParameterType != parameterTypes[i])
                            return false;
                    }

                    return true;
                });
        }

        /// <summary>
        /// Invokes method with object result through the reflection.
        /// </summary>
        /// <param name="type">Type where to search the method.</param>
        /// <param name="methodName">Method name</param>
        /// <param name="obj">Object instance. Optional. Default is null.</param>
        /// <param name="param">Method parameters. Optional. Default is null.</param>
        /// <param name="paramTypes">Types of parameters. Optional. Default is null.</param>
        /// <returns>
        /// <c>null</c> if method not found; otherwise result of the method invoke.
        /// </returns>
        /// <param name="methodInfo">Reference to the variable which stores
        /// found method. Used in order to speed up the calls. Updated by this function
        /// if equals Null.</param>
        public static object? InvokeMethodWithResult(
            Type? type,
            string methodName,
            ref MethodInfo? methodInfo,
            object? obj = null,
            object?[]? param = null,
            Type[]? paramTypes = null)
        {
            try
            {
                if(methodInfo is null)
                {
                    if (paramTypes == null)
                    {
                        methodInfo = type?.GetMethods()
                            .Where(m => m.Name == methodName && !m.IsGenericMethod)
                            .FirstOrDefault();
                    }
                    else
                    {
                        if(type is not null)
                            methodInfo = FindNonGenericMethod(type, methodName, paramTypes);
                    }
                }

                if (methodInfo is not null)
                {
                    var result = methodInfo.Invoke(obj, param);
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all static fields of a specified type from a given container type.
        /// </summary>
        /// <remarks>This method searches for static fields in the specified
        /// type and filters them by the specified field type.
        /// Only fields that are assignable to <typeparamref name="TField"/> are included
        /// in the result.</remarks>
        /// <typeparam name="TField">The type of the static fields to retrieve.</typeparam>
        /// <param name="containerType">The type that contains the static fields to search.
        /// Must not be <see langword="null"/>.</param>
        /// <param name="includeNonPublic">The <see langword="bool"/> value indicating
        /// whether to include non-public fields.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing
        /// all static fields of type <typeparamref name="TField"/>  defined
        /// in the specified <paramref name="containerType"/>. Returns an
        /// empty collection if no matching fields are
        /// found.</returns>
        public static IEnumerable<TField> GetStaticFields<TField>(
            Type containerType,
            bool includeNonPublic = false)
        {
            List<TField> result = new();

            var fields = containerType.GetFields(
                BindingFlags.Static | BindingFlags.Public | (includeNonPublic ? BindingFlags.NonPublic : 0));

            foreach (var f in fields)
            {
                if (f.FieldType != typeof(TField))
                    continue;

                if (f.GetValue(null) is not TField value)
                    continue;
                result.Add(value);
            }

            return result;
        }

        /// <summary>
        /// Gets all static properties of the specified type in the specified container type.
        /// Returned only not null properties of the exact type <typeparamref name="TProperty"/>.
        /// </summary>
        /// <typeparam name="TProperty">Type of the properties.</typeparam>
        /// <returns></returns>
        public static IEnumerable<TProperty> GetStaticProperties<TProperty>(Type containerType)
        {
            List<TProperty> result = new();

            var props = containerType.GetProperties(
                BindingFlags.Static | BindingFlags.Public);

            foreach (var p in props)
            {
                if (p.PropertyType != typeof(TProperty))
                    continue;

                if (p.GetValue(null) is not TProperty value)
                    continue;
                result.Add(value);
            }

            return result;
        }

        /// <summary>
        /// Enumerates public objects declared in the specified namespace.
        /// </summary>
        /// <param name="asm">Assembly.</param>
        /// <param name="nameSpace">Namespace.</param>
        /// <returns></returns>
        public static IEnumerable<Type> EnumPublicObjectsForNamespace(Assembly asm, string nameSpace)
        {
            var types = asm.ExportedTypes;

            foreach (var t in types)
            {
                if (t.Namespace == nameSpace)
                    yield return t;
            }
        }

        /// <summary>
        /// Gets whether <paramref name="referenceContainer"/> assembly references
        /// <paramref name="possibleReference"/> assembly.
        /// </summary>
        /// <param name="referenceContainer">The Assembly which possibly
        /// contains the reference.</param>
        /// <param name="possibleReference">The assembly which is possibly referenced.</param>
        /// <returns></returns>
        public static bool IsAssemblyReferencedFrom(
            Assembly referenceContainer,
            Assembly possibleReference)
        {
            var names = referenceContainer.GetReferencedAssemblies();
            foreach (var name in names)
            {
                if (name.FullName == possibleReference.FullName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets whether <paramref name="referenceContainer"/> assembly references
        /// any assembly from the <paramref name="possibleReference"/> collection.
        /// </summary>
        /// <param name="referenceContainer">The Assembly which possibly
        /// contains the reference.</param>
        /// <param name="possibleReference">Collection of the assemblies
        /// which are possibly referenced.</param>
        /// <returns></returns>
        public static bool IsAssemblyReferencedFrom(
            Assembly referenceContainer,
            IEnumerable<Assembly> possibleReference)
        {
            var names = referenceContainer.GetReferencedAssemblies();

            foreach (var name in names)
            {
                foreach (var asm in possibleReference)
                {
                    if (name.FullName == asm.FullName)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets type name using <see cref="CodeDomProvider"/>.
        /// </summary>
        /// <param name="type">Type for which to get the name.</param>
        /// <returns></returns>
        /// <param name="providerName">Name of the provider. If not specified, c# is used.</param>
        /// <returns></returns>
        public static string GetTypeNameUsingCodeDom(Type type, string? providerName = null)
        {
            providerName ??= "C#";

            codeDomProvider ??= CodeDomProvider.CreateProvider(providerName);
            var typeReferenceExpression
                = new CodeTypeReferenceExpression(new CodeTypeReference(type));
            using var writer = new StringWriter();

            codeDomProvider.GenerateCodeFromExpression(
                typeReferenceExpression,
                writer,
                new CodeGeneratorOptions());
            return writer.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Gets user friendly type name.
        /// </summary>
        /// <param name="type">Type for which to get the user friendly name.</param>
        /// <returns></returns>
        public static string GetTypeDisplayName(Type type)
        {
            const string nullablePrefix = "System.Nullable<";

            var result = GetTypeNameUsingCodeDom(type);

            var indexOfNullable = result.IndexOf(nullablePrefix);

            if (indexOfNullable == 0)
            {
                result = result.Remove(0, nullablePrefix.Length).TrimEnd('>') + "?";
            }

            return result;
        }

        /// <summary>
        /// Invokes the specified method on the given instance and logs the result.
        /// </summary>
        /// <param name="instance">The object instance on which the method should be invoked.
        /// Can be null for static methods.</param>
        /// <param name="method">The method to invoke. If null or requires parameters,
        /// invocation is skipped.</param>
        /// <returns>
        /// <c>true</c> if the method was successfully invoked and logged; otherwise, <c>false</c>.
        /// </returns>
        public static bool InvokeMethodAndLogResult(object? instance, MethodInfo? method)
        {
            if (method is null)
                return false;

            var declaringType = method.DeclaringType;

            var methodName = $"{method.Name}()";
            var retParam = method.ReturnParameter;
            var resultIsVoid = retParam.ParameterType == typeof(void);

            var methodParameters = method.GetParameters();
            if (methodParameters.Length > 0)
            {
                App.LogWarning(
                    $"{declaringType?.Name}.{methodName} has parameters, so it is not called.");
                return false;
            }

            try
            {
                var result = method.Invoke(instance, null);

                TreeViewItem item = new();
                item.TextHasBold = true;

                var itemText = $"Called <b>{declaringType?.Name}.{methodName}</b>";

                var withChilds = false;

                if (!resultIsVoid)
                {
                    itemText += $" with result = <b>{result}</b>";

                    withChilds = LogUtils.LogAsTreeItemChilds(item, result);
                }

                item.Text = itemText;

                App.AddLogItem(item, LogItemKind.Information, true);

                return true;
            }
            catch (Exception e)
            {
                App.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of the given type using the default constructor.
        /// Type is specified using its name and assembly name where it is located.
        /// </summary>
        /// <typeparam name="T">The type to which created object is casted.</typeparam>
        /// <param name="asmName">The name of the assembly containing the type.</param>
        /// <param name="typeName">The name of the type to create.</param>
        /// <returns>An instance of the specified type, or null if the type cannot be found.</returns>
        public static T? ActivatorCreateInstance<T>(string asmName, string typeName)
        {
            var asm = AssemblyUtils.GetOrLoadAssemblyByName(asmName);
            var objType = asm?.GetType(typeName);
            if (objType is null)
                return default;
            var result = Activator.CreateInstance(objType);
            return (T?)result;
        }

        /*
        public static unsafe bool TryGetRawMetadataViaReflection(
            Assembly assembly,
            out byte* blob,
            out int length)
        {
            blob = default;
            length = 0;

            var tp = Type.GetType("System.Reflection.Metadata.AssemblyExtensions");

            if (tp is null)
                return false;

            // Get the method info
            var method =
                tp.GetMethod("TryGetRawMetadata", BindingFlags.Static | BindingFlags.Public);

            if (method is null)
                return false;

            object[] parameters = new object[] { assembly, blob, 0 };

            bool result = (bool)method.Invoke(null, parameters);

            blob = (byte*)(IntPtr)parameters[1];
            length = (int)parameters[2];

            return result;
        }
        */
    }
}