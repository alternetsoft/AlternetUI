using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using Alternet.Maui.Extensions;

namespace Alternet.Maui
{
    public partial class SimpleToolBarView
    {
        /// <summary>
        /// Represents a separator in the toolbar.
        /// </summary>
        internal partial class ToolBarSeparator
            : BoxView, Alternet.UI.IRaiseSystemColorsChanged, IToolBarItem
        {
            private readonly SimpleToolBarView toolbar;

            private Alternet.UI.IBaseObjectWithAttr? attributesProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarSeparator"/> class.
            /// </summary>
            public ToolBarSeparator(SimpleToolBarView toolbar)
            {
                this.toolbar = toolbar;
                WidthRequest = DefaultSeparatorWidth;
                Margin = DefaultSeparatorMargin;
                BackgroundColor = GetLineColor();
            }

            /// <inheritdoc/>
            public event EventHandler? Clicked;

            public Action? ClickedAction { get; set; }

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
            public StickyButtonStyle StickyStyle
            {
                get => StickyButtonStyle.Border;

                set
                {
                }
            }

            /// <inheritdoc/>
            public bool IsSticky
            {
                get => false;

                set
                {
                }
            }

            /// <inheritdoc/>
            public bool HasBorder
            {
                get => true;

                set
                {
                }
            }

            /// <inheritdoc/>
            public string Text
            {
                get => string.Empty;
                set
                {
                }
            }

            public ToolBarButtonContainer? Container => Parent as ToolBarButtonContainer;

            public View? Button => this;

            public View? ButtonContainer => Container;

            public Drawing.SvgImage? SvgImage { get; set; }

            public FontAttributes FontAttributes { get; set; }

            public string FontFamily
            {
                get => Alternet.UI.AbstractControl.DefaultFont.Name;

                set
                {
                }
            }

            public double FontSize { get; set; }

            /// <inheritdoc/>
            public bool IsBoldWhenSticky { get; set; }

            /// <summary>
            /// Gets the parent toolbar view.
            /// </summary>
            [Browsable(false)]
            internal SimpleToolBarView ToolBar => toolbar;

            /// <inheritdoc/>
            public virtual void RaiseSystemColorsChanged()
            {
                UpdateVisualStates(false);
            }

            /// <summary>
            /// Gets color of the separator line.
            /// </summary>
            /// <returns></returns>
            public virtual Color GetLineColor()
            {
                Color color;

                if (ToolBar is null)
                {
                    if (IsDark)
                        color = Alternet.Drawing.DefaultColors.MauiToolBar.SeparatorColorDark.ToMaui();
                    else
                        color = Alternet.Drawing.DefaultColors.MauiToolBar.SeparatorColorLight.ToMaui();
                }
                else
                    color = ToolBar.GetSeparatorColor();

                return color;
            }

            /// <inheritdoc/>
            public virtual void UpdateVisualStates(bool setNormalState)
            {
                BackgroundColor = GetLineColor();
            }

            internal void RaiseClicked()
            {
                Clicked?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
