using System;
using System.Collections.Generic;
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
        private IPropertyGridNewItemParams? constructed;

        public PropertyGridNewItemParams(IPropertyGridPropInfoRegistry? owner, PropertyInfo? propInfo)
        {
            this.propInfo = propInfo;
            this.owner = owner;
        }

        public event EventHandler? ButtonClick;

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

            private bool loadedLabel;
            private bool loadedIsNullable;
            private bool loadedEditKindColor;
            private bool loadedEditKindString;
            private bool loadedHasEllipsis;
            private bool loadedTextReadOnly;
            private bool loadedOnlyTextReadOnly;

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