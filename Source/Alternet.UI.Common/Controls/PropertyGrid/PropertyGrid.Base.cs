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
    public partial class PropertyGrid : Control
    {
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
        /// Defines static states for <see cref="PropertyGrid"/> class.
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
        /// Gets or sets static states for <see cref="PropertyGrid"/> class.
        /// </summary>
        public static StaticStateFlags StaticFlags
        {
            get => staticStateFlags;
            set => staticStateFlags = value;
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
            var typeRegistry = PropertyGrid.GetTypeRegistry(type);
            var propRegistry = typeRegistry.GetPropRegistry(propName);
            if(propRegistry is not null)
                propRegistry.NewItemParams.HasEllipsis = value;
            return propRegistry;
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

            RegisterCollectionEditor(
                typeof(ListView),
                nameof(ListView.Items),
                typeof(ListEditSourceListViewItem));

            RegisterCollectionEditor(
                typeof(ListView),
                nameof(ListView.Columns),
                typeof(ListEditSourceListViewColumn));

            RegisterCollectionEditor(
                typeof(ListViewItem),
                nameof(ListViewItem.Cells),
                typeof(ListEditSourceListViewCell));

            RegisterCollectionEditor(
                typeof(VirtualListBox),
                nameof(VirtualListBox.Items),
                typeof(ListEditSourceListBox));

            RegisterCollectionEditor(
                typeof(StatusBar),
                nameof(StatusBar.Panels),
                typeof(ListEditSourceStatusBar));

            RegisterCollectionEditor(
                typeof(ComboBox),
                nameof(ComboBox.Items),
                typeof(ListEditSourceListBox));

            /*RegisterCollectionEditor(
                typeof(TabControl),
                nameof(TabControl.Pages),
                null);*/

            /*RegisterCollectionEditor(
                typeof(Toolbar),
                nameof(Toolbar.Items),
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
                typeof(ListEditSourceGradientStops));
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
        /// Adds initialization action which is called before <see cref="PropertyGrid"/>
        /// is created for the first time.
        /// </summary>
        /// <param name="action"></param>
        public static void AddInitializer(Action action)
        {
            initializers ??= new();
            initializers.Push(action);
        }

        /// <summary>
        /// Creates property choices list for use with <see cref="CreateFlagsItem"/> and
        /// <see cref="CreateChoicesItem"/>.
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
        /// <remarks>
        /// Result can be used in <see cref="CreateFlagsItem"/> and
        /// <see cref="CreateChoicesItem"/>.
        /// </remarks>
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
        /// <remarks>
        /// Result can be used in <see cref="CreateFlagsItem"/> and
        /// <see cref="CreateChoicesItem"/>.
        /// </remarks>
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
