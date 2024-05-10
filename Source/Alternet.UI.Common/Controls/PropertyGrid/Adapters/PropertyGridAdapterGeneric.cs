using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base helper class for complex <see cref="PropertyGrid"/> items.
    /// </summary>
    public abstract class PropertyGridAdapterGeneric
    {
        private object? instance;
        private object? simpleValue;
        private PropertyInfo? propInfo;
        private string? propName;

        /// <summary>
        /// Gets or sets object instance of the property value container.
        /// </summary>
        /// <remarks>
        /// If it is null, internal value is used.
        /// </remarks>
        public object? Instance
        {
            get
            {
                return instance;
            }

            set
            {
                if (instance == value)
                    return;
                instance = value;
                OnValueSourceChanged();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="PropertyInfo"/> of the property where value is contained.
        /// </summary>
        /// <remarks>
        /// If <see cref="PropInfo"/> and <see cref="PropName"/> are empty, an internal value is used.
        /// </remarks>
        public PropertyInfo? PropInfo
        {
            get
            {
                return propInfo;
            }

            set
            {
                if (propInfo == value)
                    return;
                propInfo = value;
                OnValueSourceChanged();
            }
        }

        /// <summary>
        /// Gets or sets name of the property where value is contained.
        /// </summary>
        /// <remarks>
        /// If <see cref="PropInfo"/> and <see cref="PropName"/> are empty, an internal value is used.
        /// </remarks>
        public string? PropName
        {
            get
            {
                if(propInfo == null)
                    return propName;
                return propInfo.Name;
            }

            set
            {
                if (propName == value)
                    return;
                propName = value;
                OnValueSourceChanged();
            }
        }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <remarks>
        /// If <see cref="Instance"/> and <see cref="PropInfo"/> (or <see cref="PropName"/>)
        /// are not empty value is get from the instance property; otherwise an internal
        /// value is used.
        /// </remarks>
        public object? Value
        {
            get
            {
                if(instance == null || propInfo == null)
                    return simpleValue;
                object? propValue = propInfo.GetValue(instance, null);
                return propValue;
            }

            set
            {
                if (Value == value)
                    return;
                if (instance == null || propInfo == null)
                    simpleValue = value;
                else
                    propInfo.SetValue(instance, value);
            }
        }

        /// <summary>
        /// Creates properties to show in <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="propGrid">Use this <see cref="PropertyGrid"/> instance
        /// to create properties.</param>
        /// <returns>List of created properties.</returns>
        public abstract IEnumerable<IPropertyGridItem> CreateProps(IPropertyGrid propGrid);

        /// <inheritdoc/>
        public override string? ToString()
        {
            return Value?.ToString();
        }

        internal virtual void OnPropertyChanged(object? sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Updates instance property value.
        /// </summary>
        protected abstract void Save();

        /// <summary>
        /// Loads instance property value.
        /// </summary>
        protected abstract void Load();

        private void OnValueSourceChanged()
        {
            void Fn()
            {
                simpleValue = null;
                if (instance == null || propInfo != null)
                    return;
                if (propName == null)
                    return;
                var type = instance.GetType();
                propInfo = type.GetProperty(propName);
            }

            Fn();
            Load();
        }
    }
}