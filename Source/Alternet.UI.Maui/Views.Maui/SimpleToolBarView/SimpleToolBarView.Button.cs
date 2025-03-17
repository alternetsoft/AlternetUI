using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.Maui
{
    public partial class SimpleToolBarView
    {
        /// <summary>
        /// Represents a label in the toolbar.
        /// </summary>
        internal partial class ToolBarLabel : ToolBarButton
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarLabel"/> class.
            /// </summary>
            public ToolBarLabel(SimpleToolBarView toolBar)
                : base(toolBar)
            {
            }

            /// <inheritdoc/>
            public override bool HasBorder
            {
                get
                {
                    return false;
                }

                set
                {
                }
            }
        }

        /// <summary>
        /// Represents a button in the toolbar.
        /// </summary>
        internal partial class ToolBarButton
            : Button, Alternet.UI.IRaiseSystemColorsChanged, IToolBarItem
        {
            private readonly SimpleToolBarView toolBar;

            private bool isSticky;
            private bool hasBorder = true;
            private Drawing.SvgImage? svgImage;
            private Alternet.UI.IBaseObjectWithAttr? attributesProvider;
            private StickyButtonStyle stickyStyle = StickyButtonStyle.Border;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarButton"/> class.
            /// </summary>
            public ToolBarButton(SimpleToolBarView toolBar)
            {
                this.toolBar = toolBar;
            }

            /// <summary>
            /// Occurs when the sticky state of the button changes.
            /// </summary>
            public event EventHandler? StickyChanged;

            public virtual Action? ClickedAction { get; set; }

            /// <summary>
            /// Gets or sets the style of the sticky button.
            /// </summary>
            public virtual StickyButtonStyle StickyStyle
            {
                get
                {
                    return stickyStyle;
                }

                set
                {
                    if (stickyStyle == value)
                        return;
                    stickyStyle = value;
                }
            }

            /// <summary>
            /// Gets or sets the SVG image associated with the toolbar button.
            /// </summary>
            public virtual Drawing.SvgImage? SvgImage
            {
                get
                {
                    return svgImage;
                }

                set
                {
                    if (svgImage == value)
                        return;
                    svgImage = value;
                    UpdateImage();
                }
            }

            /// <inheritdoc/>
            public virtual bool HasBorder
            {
                get
                {
                    return hasBorder;
                }

                set
                {
                    if (hasBorder == value)
                        return;
                    hasBorder = value;
                    UpdateVisualStates(true);
                }
            }

            /// <inheritdoc/>
            public virtual Alternet.UI.IBaseObjectWithAttr AttributesProvider
            {
                get => attributesProvider ??= new Alternet.UI.BaseObjectWithAttr();

                set
                {
                    attributesProvider = value;
                }
            }

            /// <inheritdoc/>
            public virtual bool IsSticky
            {
                get
                {
                    return isSticky;
                }

                set
                {
                    if (isSticky == value)
                        return;
                    isSticky = value;

                    if (value && !AllowMultipleSticky)
                    {
                        foreach (var sibling in ToolBar.Buttons)
                        {
                            if (sibling is not IToolBarItem buttonSibling)
                                continue;
                            if (buttonSibling.AttributesProvider.UniqueId == AttributesProvider.UniqueId)
                                buttonSibling.IsSticky = true;
                            else
                                buttonSibling.IsSticky = false;
                        }
                    }

                    UpdateVisualStates(true);

                    StickyChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Gets a value indicating whether the toolbar (in which button is inserted)
            /// allows multiple sticky buttons.
            /// </summary>
            [Browsable(false)]
            internal bool AllowMultipleSticky => ToolBar.AllowMultipleSticky;

            /// <summary>
            /// Gets the parent toolbar view.
            /// </summary>
            [Browsable(false)]
            internal SimpleToolBarView ToolBar
            {
                get
                {
                    return toolBar;
                }
            }

            /// <inheritdoc/>
            public virtual void RaiseSystemColorsChanged()
            {
                UpdateVisualStates(true);
            }

            public virtual void RaiseClickedAction()
            {
                ClickedAction?.Invoke();
            }

            /// <inheritdoc/>
            public virtual void UpdateVisualStates(bool setNormalState)
            {
                bool hasStickyBorder = IsSticky && HasBorder && StickyStyle == StickyButtonStyle.Border;

                var visualStateGroup = VisualStateUtils.CreateCommonStatesGroup();

                var normalState = VisualStateUtils.CreateNormalState();
                if (hasStickyBorder)
                {
                    ButtonPressedState.InitState(this, normalState);
                }
                else
                {
                    ButtonNormalState.InitState(this, normalState);
                }

                visualStateGroup.States.Add(normalState);

                var pointerOverState = VisualStateUtils.CreatePointerOverState();
                if (HasBorder)
                {
                    ButtonHotState.InitState(this, pointerOverState);
                }
                else
                {
                    ButtonNormalState.InitState(this, pointerOverState);
                }

                visualStateGroup.States.Add(pointerOverState);

                var pressedState = VisualStateUtils.CreatePressedState();
                if (HasBorder)
                    ButtonPressedState.InitState(this, pressedState);
                else
                    ButtonNormalState.InitState(this, pressedState);
                visualStateGroup.States.Add(pressedState);

                var disabledState = VisualStateUtils.CreateDisabledState();
                if (hasStickyBorder)
                {
                    ButtonStickyDisabledState.InitState(this, disabledState);
                }
                else
                {
                    ButtonDisabledState.InitState(this, disabledState);
                }

                visualStateGroup.States.Add(disabledState);

                var vsGroups = VisualStateManager.GetVisualStateGroups(this);
                vsGroups.Clear();
                vsGroups.Add(visualStateGroup);

                if (setNormalState)
                    VisualStateManager.GoToState(this, VisualStateUtils.NameNormal);
            }

            internal void UpdateImage()
            {
                Alternet.UI.MauiUtils.SetButtonImage(
                    this,
                    svgImage,
                    GetDefaultImageSize(),
                    !IsEnabled);
            }

            /// <inheritdoc/>
            protected override void OnPropertyChanged(string propertyName)
            {
                base.OnPropertyChanged(propertyName);

                if (svgImage is not null)
                {
                    if (propertyName == IsEnabledProperty.PropertyName)
                    {
                        UpdateImage();
                    }
                }
            }
        }

        internal partial class ToolbarButtonContainer
            : VerticalStackLayout, Alternet.UI.IRaiseSystemColorsChanged, IToolBarItem
        {
            private readonly ToolBarButton button;
            private readonly BoxView underline = new();

            public ToolbarButtonContainer(ToolBarButton button)
            {
                this.button = button;

                Padding = 5;

                underline.IsVisible = false;
                underline.Margin = DefaultStickyUnderlineMargin;
                underline.Color = Colors.Transparent;
                underline.HeightRequest = DefaultStickyUnderlineSize.Height;

                Children.Add(button);
                Children.Add(underline);
            }

            public event EventHandler? Clicked
            {
                add => button.Clicked += value;
                remove => button.Clicked -= value;
            }

            public Action? ClickedAction
            {
                get => button.ClickedAction;
                set => button.ClickedAction = value;
            }

            public virtual UI.IBaseObjectWithAttr AttributesProvider
            {
                get => button.AttributesProvider;
                set => button.AttributesProvider = value;
            }

            public virtual StickyButtonStyle StickyStyle
            {
                get => button.StickyStyle;

                set
                {
                    if (StickyStyle == value)
                        return;
                    button.StickyStyle = value;
                    UpdateVisualStates(false);
                }
            }

            public virtual bool IsSticky
            {
                get => button.IsSticky;

                set
                {
                    button.IsSticky = value;
                    UpdateVisualStates(false);
                }
            }

            public virtual bool HasBorder
            {
                get => button.HasBorder;
                set => button.HasBorder = value;
            }

            public virtual string Text
            {
                get => button.Text;
                set => button.Text = value;
            }

            public virtual void RaiseSystemColorsChanged()
            {
                UpdateVisualStates(false);
            }

            public virtual void UpdateVisualStates(bool setNormalState)
            {
                Color color;

                if (IsDark)
                    color = DefaultStickyUnderlineColorDark;
                else
                    color = DefaultStickyUnderlineColorLight;

                underline.Color = IsSticky ? color : Colors.Transparent;
                underline.IsVisible = StickyStyle == StickyButtonStyle.UnderlineFull ||
                    StickyStyle == StickyButtonStyle.UnderlinePartial;
            }
        }
    }
}
