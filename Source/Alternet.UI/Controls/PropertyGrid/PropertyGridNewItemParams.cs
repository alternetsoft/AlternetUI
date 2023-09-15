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
        private bool isConstructed;
        private IPropertyGridNewItemParams? constructed;

        public PropertyGridNewItemParams(IPropertyGridNewItemParams prm)
        {
            propInfo = prm.PropInfo;
            Label = prm.Label;
            IsNullable = prm.IsNullable;
            EditKindColor = prm.EditKindColor;
            EditKindString = prm.EditKindString;
            HasEllipsis = prm.HasEllipsis;
            TextReadOnly = prm.TextReadOnly;
            OnlyTextReadOnly = prm.OnlyTextReadOnly;
        }

        public PropertyGridNewItemParams(IPropertyGridPropInfoRegistry? owner, PropertyInfo? propInfo)
        {
            this.propInfo = propInfo;
            this.owner = owner;
        }

        public IPropertyGridNewItemParams Constructed
        {
            get
            {
                if (constructed != null)
                    return constructed;
                if (isConstructed)
                    return this;
                PropertyGridNewItemParams created = new(this)
                {
                    isConstructed = true,
                };
                constructed = created;
                return created;
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
    }
}