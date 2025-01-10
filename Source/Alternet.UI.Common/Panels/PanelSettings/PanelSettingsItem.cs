using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines item of the <see cref="PanelSettings"/>.
    /// </summary>
    public class PanelSettingsItem : BaseControlItem
    {
        private object? label;
        private object? data;
        private PanelSettingsItemKind kind;
        private Type? valueType;
        private bool isNullable = false;
        private bool isVisible = true;
        private bool isEnabled = true;
        private IEnumerable? pickList;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelSettingsItem"/> class.
        /// </summary>
        public PanelSettingsItem()
        {
        }

        /// <summary>
        /// Occurs when value is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets action which is invoked when value is changed.
        /// </summary>
        public Action<PanelSettingsItem, EventArgs>? ValueChangedAction { get; set; }

        /// <summary>
        /// Gets or sets a pick list for the case when item is
        /// <see cref="PanelSettingsItemKind.EditableSelector"/> or
        /// <see cref="PanelSettingsItemKind.Selector"/>.
        /// </summary>
        public IEnumerable? PickList
        {
            get => pickList;

            set
            {
                pickList = value;
            }
        }

        /// <summary>
        /// Gets or sets kind of the item.
        /// </summary>
        public virtual PanelSettingsItemKind Kind
        {
            get => kind;

            set
            {
                kind = value;
            }
        }

        /// <summary>
        /// Gets or sets whether item is visible.
        /// </summary>
        public virtual bool IsVisible
        {
            get => isVisible;

            set
            {
                isVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether item is enabled.
        /// </summary>
        public virtual bool IsEnabled
        {
            get => isEnabled;

            set
            {
                isEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets whether value is nullable.
        /// </summary>
        public virtual bool IsNullable
        {
            get => isNullable;
            set => isNullable = value;
        }

        /// <summary>
        /// Gets or sets label text which is shown next to the editor for the value.
        /// </summary>
        public virtual object? Label
        {
            get
            {
                return label;
            }

            set
            {
                label = value;
            }
        }

        /// <summary>
        /// Gets or sets type of the value.
        /// </summary>
        public virtual Type? ValueType
        {
            get => valueType;

            set
            {
                valueType = value;
            }
        }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public virtual object? Value
        {
            get => data;

            set
            {
                if (data == value)
                    return;
                data = value;
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises <see cref="ValueChanged"/> event.
        /// </summary>
        public virtual PanelSettingsItem RaiseValueChanged(EventArgs e)
        {
            ValueChangedAction?.Invoke(this, e);
            ValueChanged?.Invoke(this, e);
            return this;
        }

        /// <summary>
        /// Sets <see cref="ValueChangedAction"/> property.
        /// </summary>
        /// <param name="actionToInvoke">New value of the
        /// <see cref="ValueChangedAction"/> property.</param>
        /// <returns>This object. Result can be used to implement chained calls.</returns>
        public PanelSettingsItem InvokeWhenChanged(
            Action<PanelSettingsItem, EventArgs>? actionToInvoke)
        {
            ValueChangedAction = actionToInvoke;
            return this;
        }

        /// <summary>
        /// Sets <see cref="Value"/> property.
        /// </summary>
        /// <param name="value">New value of the
        /// <see cref="Value"/> property.</param>
        /// <returns>This object. Result can be used to implement chained calls.</returns>
        public PanelSettingsItem SetValue(object? value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        /// Sets <see cref="ValueType"/> property.
        /// </summary>
        /// <param name="valueType">New value of the
        /// <see cref="ValueType"/> property.</param>
        /// <returns>This object. Result can be used to implement chained calls.</returns>
        public PanelSettingsItem SetValueType(Type? valueType)
        {
            ValueType = valueType;
            return this;
        }

        /// <summary>
        /// Sets value of the <see cref="IsNullable"/> property.
        /// </summary>
        /// <param name="value">New value for <see cref="IsNullable"/> property.</param>
        /// <returns></returns>
        public PanelSettingsItem SetIsNullable(bool value)
        {
            IsNullable = value;
            return this;
        }
    }
}
