#pragma warning disable CA1822 // Mark members as static
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for all application strings.
    /// </summary>
    public class LocalizationManager
    {
        /// <summary>
        /// Current localizations for all application strings.
        /// </summary>
        public static LocalizationManager Default { get; set; } = new();

        /// <summary>
        /// Gets or sets localizations for property categories.
        /// </summary>
        public PropCategoryStrings PropCategories
        {
            get => PropCategoryStrings.Default;
            set => PropCategoryStrings.Default = value;
        }

        /// <summary>
        /// Gets or sets localizations for control categories.
        /// </summary>
        public ControlCategoryStrings ControlCategories
        {
            get => ControlCategoryStrings.Default;
            set => ControlCategoryStrings.Default = value;
        }

        /// <summary>
        /// Gets or sets localizations for common strings.
        /// </summary>
        public CommonStrings CommonStrings
        {
            get
            {
                return CommonStrings.Default;
            }

            set
            {
                CommonStrings.Default = value;
            }
        }

        /// <summary>
        /// Gets or sets localizations for system color names.
        /// </summary>
        public KnownColorStrings KnownColors
        {
            get
            {
                return KnownColorStrings.Default;
            }

            set
            {
                KnownColorStrings.Default = value;
            }
        }

        /// <summary>
        /// Gets or sets localizations for property names.
        /// </summary>
        public PropNameStrings PropNames
        {
            get
            {
                return PropNameStrings.Default;
            }

            set
            {
                PropNameStrings.Default = value;
            }
        }

        /// <summary>
        /// Gets or sets localizations for error messages.
        /// </summary>
        public ErrorMessages ErrorMessages
        {
            get
            {
                return ErrorMessages.Default;
            }

            set
            {
                ErrorMessages.Default = value;
            }
        }

        /// <summary>
        /// Returns <see cref="IPropertyGridChoices"/> for the given enumeration type.
        /// </summary>
        /// <typeparam name="T">Type of the enumeration.</typeparam>
        /// <remarks>
        /// You can use <see cref="IPropertyGridChoices.SetLabelForValue{T}(T, string)"/>
        /// to set localized label for the enumeration member.
        /// </remarks>
        public static IPropertyGridChoices GetEnumChoices<T>()
            where T : Enum
        {
            return PropertyGrid.CreateChoicesOnce(typeof(T));
        }

        /// <summary>
        /// Sets localized label for the property.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="propName">Property name.</param>
        /// <param name="label">New custom label of the property.</param>
        /// <returns><c>true</c> if operation successfull, <c>false</c> otherwise.</returns>
        public static bool SetPropertyLabel<T>(string propName, string label)
            where T : class
        {
            return PropertyGrid.SetCustomLabel<T>(propName, label);
        }
    }
}
