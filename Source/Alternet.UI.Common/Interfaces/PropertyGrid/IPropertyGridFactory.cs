using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to <see cref="IPropertyGrid"/>.
    /// </summary>
    /// <remarks>There is only one <see cref="IPropertyGridFactory"/> instance
    /// in the application.</remarks>
    public interface IPropertyGridFactory
    {
        /// <summary>
        /// Gets or sets default <see cref="PropertyGridCreateStyle"/> used when creating new <see cref="IPropertyGrid"/> instance.
        /// </summary>
        PropertyGridCreateStyle DefaultCreateStyle { get; set; }

        /// <inheritdoc cref="PropertyGridUtils.SetCustomLabel"/>
        bool SetCustomLabel<T>(string propName, string label)
            where T : class;

        /// <inheritdoc cref="PropertyGridUtils.GetNewItemParams(Type, PropertyInfo)"/>
        IPropertyGridNewItemParams GetNewItemParams(Type type, PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGridUtils.GetPropRegistry"/>
        IPropertyGridPropInfoRegistry GetPropRegistry(Type type, PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGridUtils.GetCustomLabel"/>
        string? GetCustomLabel<T>(string propName)
            where T : class;

        /// <inheritdoc cref="PropertyGridUtils.RegisterPropCreateFunc"/>
        void RegisterPropCreateFunc(Type type, PropertyGridItemCreate func);

        /// <inheritdoc cref="PropertyGridUtils.CreateNewItemParams(PropertyInfo)"/>
        IPropertyGridNewItemParams CreateNewItemParams(PropertyInfo? propInfo = null);

        /// <inheritdoc cref="PropertyGridUtils.GetTypeRegistry"/>
        IPropertyGridTypeRegistry GetTypeRegistry(Type type);

        /// <inheritdoc cref="PropertyGridUtils.CreateChoices()"/>
        IPropertyGridChoices CreateChoices();

        /// <summary>
        /// Creates instance of <see cref="IPropertyGrid"/>.
        /// </summary>
        /// <returns>Instance of <see cref="IPropertyGrid"/>.</returns>
        IPropertyGrid CreatePropertyGrid();

        /// <inheritdoc cref="PropertyGridUtils.CreateChoicesOnce"/>
        IPropertyGridChoices CreateChoicesOnce(Type enumType);

        /// <inheritdoc cref="PropertyGridUtils.CreateChoices(Type)"/>
        IPropertyGridChoices CreateChoices(Type enumType);

        /// <inheritdoc cref="PropertyGridUtils.GetChoices"/>
        IPropertyGridChoices GetChoices<T>()
            where T : Enum;
    }
}