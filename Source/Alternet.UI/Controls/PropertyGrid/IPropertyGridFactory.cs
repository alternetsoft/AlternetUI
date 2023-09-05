using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <inheritdoc cref="PropertyGrid.DefaultCreateStyle"/>
        PropertyGridCreateStyle DefaultCreateStyle { get; set; }

        /// <inheritdoc cref="PropertyGrid.RegisterPropCreateFunc"/>
        void RegisterPropCreateFunc(Type type, PropertyGridItemCreate func);

        /// <inheritdoc cref="PropertyGrid.GetTypeRegistry"/>
        IPropertyGridTypeRegistry GetTypeRegistry(Type type);

        /// <inheritdoc cref="PropertyGrid.IsSmallScreen"/>
        bool IsSmallScreen();

        /// <inheritdoc cref="PropertyGrid.CreateChoices()"/>
        IPropertyGridChoices CreateChoices();

        /// <inheritdoc cref="PropertyGrid.CreatePropertyGrid"/>
        IPropertyGrid CreatePropertyGrid();

        /// <inheritdoc cref="PropertyGrid.CreateVariant"/>
        IPropertyGridVariant CreateVariant();

        /// <inheritdoc cref="PropertyGrid.CreateChoicesOnce"/>
        IPropertyGridChoices CreateChoicesOnce(Type enumType);

        /// <inheritdoc cref="PropertyGrid.CreateChoices(Type)"/>
        IPropertyGridChoices CreateChoices(Type enumType);

        /// <inheritdoc cref="PropertyGrid.AutoGetTranslation"/>
        void AutoGetTranslation(bool enable);

        /// <inheritdoc cref="PropertyGrid.InitAllTypeHandlers"/>
        void InitAllTypeHandlers();

        /// <inheritdoc cref="PropertyGrid.RegisterAdditionalEditors"/>
        void RegisterAdditionalEditors();

        /// <inheritdoc cref="PropertyGrid.SetBoolChoices"/>
        void SetBoolChoices(string trueChoice, string falseChoice);
    }
}
