using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines item of the <see cref="PanelSettings"/>.
    /// </summary>
    public class PanelSettingsItem : BaseControlItem
    {
        private PanelSettings? owner;
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
        [Browsable(false)]
        public virtual CustomEventArgs? CreateArg { get; internal set; }

        /// <summary>
        /// Gets or sets action which is invoked when value is changed.
        /// </summary>
        [Browsable(false)]
        public virtual PanelSettings.ItemActionDelegate? ValueChangedAction
        {
            get => valueChangedAction;
            set => valueChangedAction = value;
        }

        /// <summary>
        /// Gets the editor control associated with the item.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? Editor => owner?.GetItemControlEditor(this);

        /// <summary>
        /// Gets the editor container control associated with the item.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? EditorContainer => owner?.GetItemControl(this);

        /// <summary>
        /// Gets the editor label control associated with the item.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? EditorLabel => owner?.GetItemControlLabel(this);

        /// <summary>
        /// Gets owner of the item.
        /// </summary>
        [Browsable(false)]
        public PanelSettings? Owner
        {
            get => owner;
            internal set
            {
                owner = value;
            }
        }

        /// <summary>
        /// Gets a pick list attached to the item.
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
        public virtual IValueSource<object> ValueSource
        {
            get => valueSource ??= new ValueContainer<object>();

            internal set => valueSource = value;
        }

        /// <summary>
        /// Gets or sets delegate which is used to create control for the item.
        /// Default is null, which means default control creation logic is used.
        /// When set, this delegate is used to create control for the item instead of default control creation logic.
        /// </summary>
        public virtual PanelSettings.ItemToControlDelegate? ItemToControl { get; set; }

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

        /// <summary>
        /// Executes the specified action on the editor control if it is of the specified type.
        /// </summary>
        /// <typeparam name="TControl">The type of the editor control.</typeparam>
        /// <param name="action">The action to be executed on the editor control.</param>
        public virtual void WithEditor<TControl>(Action<TControl>? action = null)
        {
            if (Editor is TControl control)
            {
                action?.Invoke(control);
            }
        }

        /// <summary>
        /// Sets click action for the editor control associated with the item.
        /// </summary>
        /// <param name="clickAction">The action to be invoked when the editor is clicked.</param>
        public virtual void SetEditorClick(Action? clickAction)
        {
            if (Editor is null)
                return;
            Editor.Click -= OnEditorClick;
            
            if (clickAction is not null)
                Editor.Click += OnEditorClick;
            
            void OnEditorClick(object? sender, EventArgs e)
            {
                clickAction?.Invoke();
            }
        }
    }
}
