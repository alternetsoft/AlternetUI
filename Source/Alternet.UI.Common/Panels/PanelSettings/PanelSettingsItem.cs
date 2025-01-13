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
        private IValueSource<object>? valueSource;
        private PanelSettingsItemKind kind;
        private Type? valueType;
        private bool? isNullable;
        private bool isVisible = true;
        private bool isEnabled = true;
        private IEnumerable<object>? pickList;
        private PanelSettings.ItemActionDelegate? clickAction;
        private PanelSettings.ItemActionDelegate? valueChangedAction;

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
        /// Gets additional arguments used when item was created.
        /// </summary>
        public virtual CustomEventArgs? CreateArg { get; internal set; }

        /// <summary>
        /// Gets or sets action which is invoked when value is changed.
        /// </summary>
        public virtual PanelSettings.ItemActionDelegate? ValueChangedAction
        {
            get => valueChangedAction;
            set => valueChangedAction = value;
        }

        /// <summary>
        /// Gets a pick list for the case when item is
        /// <see cref="PanelSettingsItemKind.EditableSelector"/> or
        /// <see cref="PanelSettingsItemKind.Selector"/>.
        /// </summary>
        public virtual IEnumerable<object>? PickList
        {
            get => pickList;

            internal set
            {
                pickList = value;
            }
        }

        /// <summary>
        /// Gets or sets click action.
        /// </summary>
        public virtual PanelSettings.ItemActionDelegate? ClickAction
        {
            get => clickAction;
            set => clickAction = value;
        }

        /// <summary>
        /// Gets kind of the item.
        /// </summary>
        public virtual PanelSettingsItemKind Kind
        {
            get => kind;

            internal set
            {
                kind = value;
            }
        }

        /// <summary>
        /// Gets whether item is visible.
        /// </summary>
        public virtual bool IsVisible
        {
            get => isVisible;

            internal set
            {
                isVisible = value;
            }
        }

        /// <summary>
        /// Gets whether item is enabled.
        /// </summary>
        public virtual bool IsEnabled
        {
            get => isEnabled;

            internal set
            {
                isEnabled = value;
            }
        }

        /// <summary>
        /// Gets whether value is nullable.
        /// </summary>
        public virtual bool IsNullable
        {
            get
            {
                if (ValueType is null)
                    return false;
                return isNullable ??= AssemblyUtils.IsNullableType(ValueType);
            }
        }

        /// <summary>
        /// Gets label text which is shown next to the editor for the value.
        /// </summary>
        public virtual object? Label
        {
            get
            {
                return label;
            }

            internal set
            {
                label = value;
            }
        }

        /// <summary>
        /// Gets type of the value.
        /// </summary>
        public virtual Type? ValueType
        {
            get => valueType;

            internal set
            {
                valueType = value;
                isNullable = null;
            }
        }

        /// <summary>
        /// Gets value source.
        /// </summary>
        public virtual IValueSource<object> ValueSource
        {
            get => valueSource ??= new ValueContainer<object>();

            internal set => valueSource = value;
        }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public virtual object? Value
        {
            get => ValueSource.Value;

            set
            {
                if (ValueSource.Value == value)
                    return;
                ValueSource.Value = value;
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
    }
}
