using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Displays a list of enum members, from which the user can select a value.
    /// </summary>
    public class EnumPickerView : Picker
    {
        /// <summary>
        /// Defines bindable property for enum type.
        /// </summary>
        public static readonly BindableProperty EnumTypeProperty =
            BindableProperty.Create(
                nameof(EnumType),
                typeof(Type),
                typeof(EnumPickerView),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    EnumPickerView picker = (EnumPickerView)bindable;

                    if (oldValue != null)
                    {
                        picker.ItemsSource = null;
                    }

                    if (newValue != null)
                    {
                        if (!((Type)newValue).GetTypeInfo().IsEnum)
                        {
                            throw new ArgumentException(
                                "EnumPickerView: EnumType property must be enumeration type");
                        }

                        picker.ItemsSource = Enum.GetValues((Type)newValue);
                    }
                });

        /// <summary>
        /// Gets or sets type of the enum which will be picked.
        /// </summary>
        public Type EnumType
        {
            get => (Type)GetValue(EnumTypeProperty);

            set => SetValue(EnumTypeProperty, value);
        }
    }
}
