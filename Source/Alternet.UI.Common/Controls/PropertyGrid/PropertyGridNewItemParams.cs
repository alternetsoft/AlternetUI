﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridNewItemParams : IPropertyGridNewItemParams
    {
        public static readonly IPropertyGridNewItemParams Default =
            PropertyGrid.CreateNewItemParams(null!);

        private readonly IPropertyGridPropInfoRegistry? owner;
        private PropertyInfo? propInfo;
        private IPropertyGridChoices? choices;
        private IPropertyGridNewItemParams? constructed;

        public PropertyGridNewItemParams(IPropertyGridPropInfoRegistry? owner, PropertyInfo? propInfo)
        {
            this.propInfo = propInfo;
            this.owner = owner;
        }

        public event EventHandler? ButtonClick;

        public NumberStyles? NumberStyles { get; set; }

        public IFormatProvider? FormatProvider { get; set; }

        public IObjectToString? Converter { get; set; }

        public CultureInfo? Culture { get; set; }

        public ITypeDescriptorContext? Context { get; set; }

        public bool? UseInvariantCulture { get; set; }

        public string? DefaultFormat { get; set; }

        public IPropertyGridChoices? Choices
        {
            get
            {
                return choices;
            }

            set
            {
                choices = value;
            }
        }

        public IPropertyGridNewItemParams Constructed
        {
            get
            {
                if (constructed != null)
                    return constructed;
                constructed = new ConstructedParams(this);
                return constructed;
            }
        }

        public IPropertyGridPropInfoRegistry? Owner => owner;

        public PropertyInfo? PropInfo { get => propInfo; set => propInfo = value; }

        public string? Label { get; set; }

        public bool? EnumIsFlags
        {
            get;
            set;
        }

        public bool? IsNullable { get; set; }

        public PropertyGridEditKindColor? EditKindColor { get; set; }

        public PropertyGridEditKindString? EditKindString { get; set; }

        public bool? HasEllipsis { get; set; }

        public bool? TextReadOnly { get; set; }

        public bool? OnlyTextReadOnly { get; set; }

        public void RaiseButtonClick(IPropertyGridItem item)
        {
            ButtonClick?.Invoke(item, EventArgs.Empty);
        }

        internal class ConstructedParams : IPropertyGridNewItemParams
        {
            private readonly IPropertyGridNewItemParams owner;

            private string? constructedLabel;
            private bool? constructedIsNullable;
            private PropertyGridEditKindColor? constructedEditKindColor;
            private PropertyGridEditKindString? constructedEditKindString;
            private bool? constructedHasEllipsis;
            private bool? constructedTextReadOnly;
            private bool? constructedOnlyTextReadOnly;
            private NumberStyles? constructedNumberStyles;
            private IFormatProvider? constructedFormatProvider;
            private IObjectToString? constructedConverter;
            private string? constructedDefaultFormat;
            private CultureInfo? constructedCulture;
            private ITypeDescriptorContext? constructedContext;
            private bool? constructedUseInvariantCulture;

            private bool loadedContext;
            private bool loadedCulture;
            private bool loadedDefaultFormat;
            private bool loadedFormatProvider;
            private bool loadedConverter;
            private bool loadedNumberStyles;
            private bool loadedLabel;
            private bool loadedIsNullable;
            private bool loadedEditKindColor;
            private bool loadedEditKindString;
            private bool loadedHasEllipsis;
            private bool loadedTextReadOnly;
            private bool loadedOnlyTextReadOnly;
            private bool loadedUseInvariantCulture;

            public ConstructedParams(IPropertyGridNewItemParams owner)
            {
                this.owner = owner;
            }

            public event EventHandler? ButtonClick
            {
                add
                {
                    owner.ButtonClick += value;
                }

                remove
                {
                    owner.ButtonClick -= value;
                }
            }

            public IPropertyGridNewItemParams Constructed => this;

            public IPropertyGridPropInfoRegistry? Owner => owner.Owner;

            public IPropertyGridTypeRegistry? OwnerTypeRegistry => owner.Owner?.Owner;

            public Type? OwnerInstanceType => OwnerTypeRegistry?.InstanceType;

            public PropertyInfo? PropInfo
            {
                get => owner.PropInfo;
                set { }
            }

            public bool? EnumIsFlags
            {
                get
                {
                    return owner.EnumIsFlags;
                }

                set
                {
                }
            }

            public IPropertyGridChoices? Choices
            {
                get
                {
                    return owner.Choices;
                }

                set
                {
                }
            }

            public string? Label
            {
                get
                {
                    var result = GetConstructedValue<string?>(
                        ref loadedLabel,
                        ref constructedLabel,
                        (r) => r?.NewItemParams?.Label);
                    return result;
                }

                set
                {
                }
            }

            public NumberStyles? NumberStyles
            {
                get
                {
                    var result = GetConstructedValue<NumberStyles?>(
                        ref loadedNumberStyles,
                        ref constructedNumberStyles,
                        (r) => r?.NewItemParams?.NumberStyles);
                    return result;
                }

                set
                {
                }
            }

            public CultureInfo? Culture
            {
                get
                {
                    var result = GetConstructedValue<CultureInfo?>(
                        ref loadedCulture,
                        ref constructedCulture,
                        (r) => r?.NewItemParams?.Culture);
                    return result;
                }

                set
                {
                }
            }

            public ITypeDescriptorContext? Context
            {
                get
                {
                    var result = GetConstructedValue<ITypeDescriptorContext?>(
                        ref loadedContext,
                        ref constructedContext,
                        (r) => r?.NewItemParams?.Context);
                    return result;
                }

                set
                {
                }
            }

            public bool? UseInvariantCulture
            {
                get
                {
                    var result = GetConstructedValue<bool?>(
                        ref loadedUseInvariantCulture,
                        ref constructedUseInvariantCulture,
                        (r) => r?.NewItemParams?.UseInvariantCulture);
                    return result;
                }

                set
                {
                }
            }

            public IFormatProvider? FormatProvider
            {
                get
                {
                    var result = GetConstructedValue<IFormatProvider?>(
                        ref loadedFormatProvider,
                        ref constructedFormatProvider,
                        (r) => r?.NewItemParams?.FormatProvider);
                    return result;
                }

                set
                {
                }
            }

            public IObjectToString? Converter
            {
                get
                {
                    var result = GetConstructedValue<IObjectToString?>(
                        ref loadedConverter,
                        ref constructedConverter,
                        (r) => r?.NewItemParams?.Converter);
                    return result;
                }

                set
                {
                }
            }

            public string? DefaultFormat
            {
                get
                {
                    var result = GetConstructedValue<string?>(
                        ref loadedDefaultFormat,
                        ref constructedDefaultFormat,
                        (r) => r?.NewItemParams?.DefaultFormat);
                    return result;
                }

                set
                {
                }
            }

            public bool? IsNullable
            {
                get
                {
                    var result = GetConstructedValue<bool?>(
                        ref loadedIsNullable,
                        ref constructedIsNullable,
                        (r) => r?.NewItemParams?.IsNullable);
                    return result;
                }

                set
                {
                }
            }

            public PropertyGridEditKindColor? EditKindColor
            {
                get
                {
                    var result = GetConstructedValue<PropertyGridEditKindColor?>(
                        ref loadedEditKindColor,
                        ref constructedEditKindColor,
                        (r) => r?.NewItemParams?.EditKindColor);
                    return result;
                }

                set
                {
                }
            }

            public PropertyGridEditKindString? EditKindString
            {
                get
                {
                    var result = GetConstructedValue<PropertyGridEditKindString?>(
                        ref loadedEditKindString,
                        ref constructedEditKindString,
                        (r) => r?.NewItemParams?.EditKindString);
                    return result;
                }

                set
                {
                }
            }

            public bool? HasEllipsis
            {
                get
                {
                    var result = GetConstructedValue<bool?>(
                        ref loadedHasEllipsis,
                        ref constructedHasEllipsis,
                        (r) => r?.NewItemParams?.HasEllipsis);
                    return result;
                }

                set
                {
                }
            }

            public bool? TextReadOnly
            {
                get
                {
                    var result = GetConstructedValue<bool?>(
                        ref loadedTextReadOnly,
                        ref constructedTextReadOnly,
                        (r) => r?.NewItemParams?.TextReadOnly);
                    return result;
                }

                set
                {
                }
            }

            public bool? OnlyTextReadOnly
            {
                get
                {
                    var result = GetConstructedValue<bool?>(
                        ref loadedOnlyTextReadOnly,
                        ref constructedOnlyTextReadOnly,
                        (r) => r?.NewItemParams?.OnlyTextReadOnly);
                    return result;
                }

                set
                {
                }
            }

            public T GetValidValue<T>(Func<IPropertyGridPropInfoRegistry?, T> func)
            {
                bool ValidatorFunc(IPropertyGridPropInfoRegistry registry)
                {
                    return registry.HasNewItemParams && func(registry) is not null;
                }

                var pr = PropertyGrid.GetValidBasePropRegistry(
                            OwnerInstanceType,
                            PropInfo,
                            ValidatorFunc);
                return func(pr);
            }

            public T GetConstructedValue<T>(
                ref bool loaded,
                ref T loadedValue,
                Func<IPropertyGridPropInfoRegistry?, T> func)
            {
                var ownerValue = func(Owner);
                if (ownerValue is not null)
                    return ownerValue;
                if (loaded)
                    return loadedValue;
                loaded = true;
                loadedValue = GetValidValue<T>(func);
                return loadedValue;
            }

            public void RaiseButtonClick(IPropertyGridItem item)
            {
                owner.RaiseButtonClick(item);
            }
        }
    }
}