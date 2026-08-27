using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle creation of the property.
    /// </summary>
    /// <param name="sender">Instance of the property grid.</param>
    /// <param name="label">Property label.</param>
    /// <param name="name">Property name.</param>
    /// <param name="instance">Object instance which contains the property.</param>
    /// <param name="propInfo">Property information.</param>
    /// <returns>Property declaration for use with property add methods.</returns>
    /// <remarks>
    /// If <paramref name="label"/> or <paramref name="name"/> is null,
    /// <paramref name="propInfo"/> is used to get them.
    /// </remarks>
    public delegate IPropertyGridItem PropertyGridItemCreate(
            IPropertyGrid sender,
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

    /// <summary>
    /// Contains methods and properties which allow to work with property grid.
    /// </summary>
    public static class PropertyGridUtils
    {
        /// <summary>
        /// Defines default style for the newly created property grid controls.
        /// </summary>
        public static PropertyGridCreateStyle DefaultCreateStyle { get; set; }
            = PropertyGridCreateStyle.DefaultStyle;

        /// <summary>
        /// Dictionary used to get type related information.
        /// </summary>
        public static readonly BaseDictionaryCached<Type, IPropertyGridTypeRegistry>
            TypeRegistry = new();

        private static BaseDictionary<Type, IPropertyGridChoices>? choicesCache = null;
        private static StaticStateFlags staticStateFlags;
        private static ConcurrentStack<Action>? initializers;

        /// <summary>
        /// Occurs when collection editor is called in the property grid.
        /// </summary>
        public static event EventHandler? EditWithListEdit;

        /// <summary>
        /// Defines static states related to the property grid.
        /// </summary>
        [Flags]
        public enum StaticStateFlags
        {
            /// <summary>
            /// Collection editors were registered.
            /// </summary>
            CollectionEditorsRegistered = 1,

            /// <summary>
            /// Known colors were added.
            /// </summary>
            KnownColorsAdded = 2,
        }

        /// <summary>
        /// Gets or sets static states related to the property grid.
        /// </summary>
        public static StaticStateFlags StaticFlags
        {
            get => staticStateFlags;
            set => staticStateFlags = value;
        }

        /// <summary>
        /// Gets the initializers stack.
        /// </summary>
        public static ConcurrentStack<Action>? Initializers => initializers;

        /// <summary>
        /// Adds simple action for the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type for which action is registered.</typeparam>
        /// <param name="name">Action name.</param>
        /// <param name="action">Action.</param>
        /// <returns><see cref="IPropertyGridTypeRegistry"/> of the specified
        /// <typeparamref name="T"/> type so you can chain calls and perform other actions
        /// on it.</returns>
        public static IPropertyGridTypeRegistry AddSimpleAction<T>(string name, Action action)
        {
            var registry = PropertyGridUtils.GetTypeRegistry(typeof(T));
            registry.AddSimpleAction(name, action);
            return registry;
        }

        /// <summary>
        /// Gets list of simple actions or <c>null</c> if there are no actions.
        /// </summary>
        /// <param name="t">Type for which actions are requested.</param>
        /// <returns></returns>
        public static IEnumerable<(string Title, Action Action)>? GetSimpleActions(Type t)
        {
            var registry = PropertyGridUtils.GetTypeRegistryOrNull(t);
            return registry?.GetSimpleActions();
        }

        /// <summary>
        /// Shows or hides ellipsis button in the property editor.
        /// </summary>
        /// <param name="type">Type which contains the property.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="value"><c>true</c> to show ellipsis button, <c>false</c> to hide it.</param>
        /// <returns><see cref="IPropertyGridPropInfoRegistry"/> item for the property
        /// specified in <paramref name="propName"/>.</returns>
        public static IPropertyGridPropInfoRegistry? ShowEllipsisButton(
            Type type,
            string propName,
            bool value = true)
        {
            var typeRegistry = PropertyGridUtils.GetTypeRegistry(type);
            var propRegistry = typeRegistry.GetPropRegistry(propName);
            if(propRegistry is not null)
                propRegistry.NewItemParams.HasEllipsis = value;
            return propRegistry;
        }

        /// <summary>
        /// Registers <see cref="IPropertyGridItem"/> create function for specific <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="func">Create function.</param>
        public static void RegisterPropCreateFunc(Type type, PropertyGridItemCreate func)
        {
            var registry = GetTypeRegistry(type);
            registry.CreateFunc = func;
        }

        /// <summary>
        /// Sets custom label for the property.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="propName">Property name.</param>
        /// <param name="label">New custom label of the property.</param>
        /// <returns><c>true</c> if operation successful, <c>false</c> otherwise.</returns>
        public static bool SetCustomLabel<T>(string propName, string label)
            where T : class
        {
            var propInfo = AssemblyUtils.GetPropertySafe(typeof(T), propName);
            if (propInfo == null)
                return false;
            var propRegistry = GetPropRegistry(typeof(T), propInfo);
            propRegistry.NewItemParams.Label = label;
            return true;
        }

        /// <summary>
        /// Registers collection editor for the specified property of the class.
        /// </summary>
        /// <param name="type">Type which contains the property.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="editType">Editor type which implements
        /// <see cref="IListEditSource"/> interface.</param>
        /// <returns><see cref="IPropertyGridPropInfoRegistry"/> item for the property
        /// specified in <paramref name="propName"/>.</returns>
        public static IPropertyGridPropInfoRegistry? RegisterCollectionEditor(
            Type type,
            string propName,
            Type? editType)
        {
            var propRegistry = ShowEllipsisButton(type, propName);

            if(propRegistry is not null)
            {
                propRegistry.NewItemParams.OnlyTextReadOnly = true;
                propRegistry.ListEditSourceType = editType;
                propRegistry.NewItemParams.ButtonClick += (s, e) =>
                {
                    EditWithListEdit?.Invoke(s, e);
                };
            }

            return propRegistry;
        }

        /// <summary>
        /// Register collection editors for all controls.
        /// </summary>
        public static void RegisterCollectionEditors()
        {
            // List edit for ImageList.Images
            // List edit for ImageSet.Images
            // List edit for TabControl.Pages
            // List edit for Toolbar.Items
            // List edit for Menu.Items
            /* List edit for Window.InputBindings*/

            if (staticStateFlags.HasFlag(StaticStateFlags.CollectionEditorsRegistered))
                return;
            staticStateFlags |= StaticStateFlags.CollectionEditorsRegistered;

            /*RegisterCollectionEditor(
                typeof(ImageList),
                nameof(ImageList.Images),
                null);*/

            /*RegisterCollectionEditor(
                typeof(ImageSet),
                nameof(ImageSet.Images),
                null);*/

            /*
            RegisterCollectionEditor(
                typeof(ListView),
                nameof(ListView.Items),
                typeof(ListEditSourceForListViewItem));

            RegisterCollectionEditor(
                typeof(ListView),
                nameof(ListView.Columns),
                typeof(ListEditSourceForListViewColumn));

            RegisterCollectionEditor(
                typeof(ListViewItem),
                nameof(ListViewItem.Cells),
                typeof(ListEditSourceForListViewCell));
            */

            RegisterCollectionEditor(
                typeof(VirtualListBox),
                nameof(VirtualListBox.Items),
                typeof(ListEditSourceForListBox));

            RegisterCollectionEditor(
                typeof(ToolBar),
                nameof(ToolBar.Panels),
                typeof(ListEditSourceForToolBar));

            /*RegisterCollectionEditor(
                typeof(TabControl),
                nameof(TabControl.Pages),
                null);*/

            /*RegisterCollectionEditor(
                typeof(Menu),
                nameof(Menu.Items),
                null);*/

            /*RegisterCollectionEditor(
                typeof(Window),
                nameof(Window.InputBindings),
                null);*/

            RegisterCollectionEditor(
                typeof(PropertyGridAdapterBrush),
                nameof(PropertyGridAdapterBrush.GradientStops),
                typeof(ListEditSourceForGradientStops));
        }

        /// <summary>
        /// Gets type of the registered list editor source for the specified <paramref name="type"/>
        /// and <paramref name="propInfo"/>. This is used in list editor dialog.
        /// </summary>
        /// <param name="type">Type which contains the property.</param>
        /// <param name="propInfo">Property information.</param>
        public static Type? GetListEditSourceType(Type? type, PropertyInfo? propInfo)
        {
            static bool ValidatorFunc(IPropertyGridPropInfoRegistry registry)
            {
                var result = registry.ListEditSourceType != null;
                return result;
            }

            var registry = GetValidBasePropRegistry(type, propInfo, ValidatorFunc);
            var result = registry?.ListEditSourceType;
            return result;
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridTypeRegistry"/> for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type value.</param>
        public static IPropertyGridTypeRegistry GetTypeRegistry(Type type)
        {
            return TypeRegistry.GetOrCreateCached(type, () =>
            {
                return new PropertyGridTypeRegistry(type);
            });
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridTypeRegistry"/> for the given <see cref="Type"/>
        /// if its available, otherwise returns <c>null</c>.
        /// </summary>
        /// <param name="type">Type value.</param>
        public static IPropertyGridTypeRegistry? GetTypeRegistryOrNull(Type type)
        {
            return TypeRegistry.GetValueOrDefaultCached(type);
        }

        /// <summary>
        /// Gets "constructed" <see cref="IPropertyGridNewItemParams"/> for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propInfo">Property information.</param>
        /// <remarks>
        /// See <see cref="IPropertyGridNewItemParams.Constructed"/> for the details.
        /// </remarks>
        public static IPropertyGridNewItemParams ConstructNewItemParams(
            Type type,
            PropertyInfo propInfo)
        {
            var prm = GetNewItemParams(type, propInfo);
            return prm.Constructed;
        }

        /// Gets "constructed" <see cref="IPropertyGridNewItemParams"/> for the given
        /// object instance and <see cref="PropertyInfo"/>.
        /// <param name="instance">Object instance.</param>
        /// <param name="propInfo">Property information.</param>
        /// <remarks>
        /// See <see cref="IPropertyGridNewItemParams.Constructed"/> for the details.
        /// </remarks>
        public static IPropertyGridNewItemParams ConstructNewItemParams(
            object instance,
            PropertyInfo propInfo)
        {
            if (instance == null)
                return PropertyGridNewItemParams.Default;
            var type = instance.GetType();
            return ConstructNewItemParams(type, propInfo);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridNewItemParams GetNewItemParams(
            Type type,
            PropertyInfo propInfo)
        {
            var registry = GetTypeRegistry(type);
            var propRegistry = registry.GetPropRegistry(propInfo);
            return propRegistry.NewItemParams;
        }

        /// <summary>
        /// Determines whether the specified property represents an enumeration,
        /// a flags enumeration, or neither.
        /// </summary>
        /// <remarks>This method evaluates the property's type to determine if it is
        /// an enumeration or a flags enumeration. If the property type is not an enumeration,
        /// <see cref="FlagsOrEnum.None"/> is returned.</remarks>
        /// <param name="instance">The object instance containing the property to evaluate.</param>
        /// <param name="propInfo">The metadata information for the property to evaluate.
        /// Cannot be null.</param>
        /// <returns>A <see cref="FlagsOrEnum"/> value indicating the type
        /// of the property: <see cref="FlagsOrEnum.Flags"/> if
        /// the property is a flags enumeration, <see cref="FlagsOrEnum.Enum"/>
        /// if the property is a standard
        /// enumeration, or <see cref="FlagsOrEnum.None"/> if the property is neither.</returns>
        public static FlagsOrEnum IsFlagsOrEnum(object instance, PropertyInfo propInfo)
        {
            var valueType = propInfo.PropertyType ?? typeof(object);

            var realType = AssemblyUtils.GetRealType(valueType);
            var isEnum = realType.IsEnum;

            if (isEnum)
            {
                var prm = PropertyGridUtils.ConstructNewItemParams(instance, propInfo);
                bool isFlags;
                if (prm.EnumIsFlags is null)
                    isFlags = AssemblyUtils.EnumIsFlags(realType);
                else
                    isFlags = prm.EnumIsFlags.Value;

                if (isFlags)
                    return FlagsOrEnum.Flags;
                return FlagsOrEnum.Enum;
            }
            else
            {
                return FlagsOrEnum.None;
            }
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// <see cref="Type"/> and property name.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propName">Property name.</param>
        public static IPropertyGridNewItemParams? GetNewItemParams(Type type, string propName)
        {
            var registry = GetTypeRegistry(type);
            var propRegistry = registry.GetPropRegistry(propName);
            return propRegistry?.NewItemParams;
        }

        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// object instance and <see cref="PropertyInfo"/>.
        /// <param name="instance">Object instance.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridNewItemParams GetNewItemParams(
            object instance,
            PropertyInfo propInfo)
        {
            if (instance == null)
                return PropertyGridNewItemParams.Default;
            var type = instance.GetType();
            return GetNewItemParams(type, propInfo);
        }

        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/> if its available,
        /// otherwise returns <c>null</c>.
        /// <param name="type">Object type.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridNewItemParams? GetNewItemParamsOrNull(
            Type type,
            PropertyInfo propInfo)
        {
            var registry = GetTypeRegistryOrNull(type);
            if (registry == null)
                return null;
            var propRegistry = registry.GetPropRegistryOrNull(propInfo);
            if (propRegistry == null)
                return null;
            if (propRegistry.HasNewItemParams)
                return propRegistry.NewItemParams;
            return null;
        }

        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// object instance and <see cref="PropertyInfo"/> if its available,
        /// otherwise returns <c>null</c>.
        /// <param name="instance">Object instance.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridNewItemParams? GetNewItemParamsOrNull(
            object instance,
            PropertyInfo propInfo)
        {
            if (instance == null)
                return null;
            var type = instance.GetType();
            return GetNewItemParamsOrNull(type, propInfo);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridPropInfoRegistry"/> for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridPropInfoRegistry GetPropRegistry(Type type, PropertyInfo propInfo)
        {
            var registry = GetTypeRegistry(type);
            var propRegistry = registry.GetPropRegistry(propInfo);
            return propRegistry;
        }

        /// <summary>
        /// Gets custom label for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/>.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="propName">Property name.</param>
        public static string? GetCustomLabel<T>(string propName)
            where T : class
        {
            var propInfo = AssemblyUtils.GetPropertySafe(typeof(T), propName);
            if (propInfo == null)
                return null;

            var propRegistry = GetPropRegistry(typeof(T), propInfo);
            return propRegistry.NewItemParams.Label;
        }

        /// <summary>
        /// Returns <see cref="IPropertyGridChoices"/> for the specified <paramref name="instance"/>
        /// and <paramref name="propName"/>.
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <param name="propName">Property name.</param>
        /// <returns></returns>
        public static IPropertyGridChoices? GetPropChoices(object instance, string propName)
        {
            var propInfo = AssemblyUtils.GetPropInfo(instance, propName);
            if (propInfo is null)
                return null;
            var propType = propInfo.PropertyType;
            var prm = ConstructNewItemParams(instance, propInfo);
            var choices = prm.Choices;
            var realType = AssemblyUtils.GetRealType(propType);
            choices ??= CreateChoicesOnce(realType);
            return choices;
        }

        /// <summary>
        /// Adds initialization action which is called before property grid
        /// is created for the first time.
        /// </summary>
        /// <param name="action"></param>
        public static void AddInitializer(Action action)
        {
            initializers ??= new();
            initializers.Push(action);
        }

        /// <summary>
        /// Creates property choices list.
        /// </summary>
        public static IPropertyGridChoices CreateChoices()
        {
            return ControlFactory.Handler.CreateChoices();
        }

        /// <summary>
        /// Returns <see cref="IPropertyGridChoices"/> for the given enumeration type.
        /// </summary>
        /// <typeparam name="T">Type of the enumeration.</typeparam>
        public static IPropertyGridChoices GetChoices<T>()
            where T : Enum
        {
            return CreateChoicesOnce(typeof(T));
        }

        /// <summary>
        /// Creates property choices list for the given enumeration type or returns it from
        /// the internal cache if it was previously created.
        /// </summary>
        public static IPropertyGridChoices CreateChoicesOnce(Type enumType)
        {
            choicesCache ??= new();
            if (choicesCache.TryGetValue(enumType, out IPropertyGridChoices? result))
                return result;
            result = CreateChoices(enumType);
            choicesCache.Add(enumType, result);
            return result;
        }

        /// <summary>
        /// Creates property choices list for the given enumeration type.
        /// </summary>
        public static IPropertyGridChoices CreateChoices(Type enumType)
        {
            var result = CreateChoices();

            if (!enumType.IsEnum)
                return result;

            var values = Enum.GetValues(enumType);
            var names = Enum.GetNames(enumType);

            bool isFlags = AssemblyUtils.EnumIsFlags(enumType);

            for (int i = 0; i < values.Length; i++)
            {
                var value = (int)values.GetValue(i)!;
                if (isFlags && value == 0)
                    continue;
                result.Add(names[i], value);
            }

            return result;
        }

        /// <summary>
        /// Creates default <see cref="IPropertyGridNewItemParams"/> provider.
        /// </summary>
        /// <param name="owner">Object owner.</param>
        /// <param name="propInfo">Property information.</param>
        /// <returns></returns>
        public static IPropertyGridNewItemParams CreateNewItemParams(
           IPropertyGridPropInfoRegistry? owner, PropertyInfo? propInfo = null)
        {
            return new PropertyGridNewItemParams(owner, propInfo);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridPropInfoRegistry"/> item for the specified
        /// <paramref name="type"/> and <paramref name="propInfo"/>. Uses validator
        /// functions to check whether results is ok.
        /// </summary>
        /// <param name="type">Type which contains the property.</param>
        /// <param name="propInfo">Property information.</param>
        /// <param name="validatorFunc">Validator function.</param>
        /// <remarks>
        /// This method also searches for the result in all base types of
        /// the <paramref name="type"/>.
        /// </remarks>
        public static IPropertyGridPropInfoRegistry? GetValidBasePropRegistry(
            Type? type,
            PropertyInfo? propInfo,
            Func<IPropertyGridPropInfoRegistry, bool> validatorFunc)
        {
            if (type == null || propInfo == null)
                return null;
            var registry = GetTypeRegistry(type);

            while (true)
            {
                if (registry == null)
                    return null;
                var propRegistry = registry.GetPropRegistryOrNull(propInfo.Name);
                if (propRegistry == null)
                {
                    registry = registry.BaseTypeRegistry;
                    continue;
                }

                var isOk = validatorFunc(propRegistry);
                if (!isOk)
                {
                    registry = registry.BaseTypeRegistry;
                    continue;
                }

                return propRegistry;
            }
        }

        /// <summary>
        /// Creates new <see cref="IPropertyGridNewItemParams"/> instance.
        /// </summary>
        public static IPropertyGridNewItemParams CreateNewItemParams(
            PropertyInfo? propInfo = null)
        {
            return new PropertyGridNewItemParams(null, propInfo);
        }
    }
}
