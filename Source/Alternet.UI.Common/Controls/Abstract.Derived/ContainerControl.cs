﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for all container controls.
    /// </summary>
    public partial class ContainerControl : HiddenBorder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ContainerControl(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerControl"/> class.
        /// </summary>
        public ContainerControl()
        {
            TabStop = false;
            CanSelect = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <summary>
        /// Creates new <see cref="TabControl"/> and adds it
        /// to the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual TabControl AddTabControl()
        {
            var result = new TabControl
            {
                Parent = this,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="VerticalStackPanel"/> and adds
        /// it to the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual VerticalStackPanel AddVerticalStackPanel()
        {
            var result = new VerticalStackPanel
            {
                Parent = this,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="StackPanel"/> and adds it to
        /// the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual StackPanel AddStackPanel(bool isVertical = true)
        {
            StackPanelOrientation orientation;

            if (isVertical)
                orientation = StackPanelOrientation.Vertical;
            else
                orientation = StackPanelOrientation.Horizontal;

            var result = new StackPanel
            {
                Parent = this,
                Orientation = orientation,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="Button"/> and adds it to
        /// the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual Button AddButton(string text, Action? action = null)
        {
            var result = new Button(text)
            {
                Parent = this,
            };

            if (action is not null)
                result.Click += Result_Click;

            return result;

            void Result_Click(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Creates new <see cref="CheckBox"/> and adds it
        /// to the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual CheckBox AddCheckBox(string text, Action? action = null)
        {
            var result = new CheckBox(text)
            {
                Parent = this,
            };

            if (action is not null)
                result.CheckedChanged += Result_CheckedChanged;

            return result;

            void Result_CheckedChanged(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Adds multiple buttons.
        /// </summary>
        /// <param name="buttons">Array of title and action.</param>
        /// <returns><see cref="ControlSet"/> with list of created buttons.</returns>
        public virtual ControlSet AddButtons(params (string, Action?)[] buttons)
        {
            List<AbstractControl> result = new();

            this.DoInsideLayout(() =>
            {
                foreach (var item in buttons)
                {
                    var button = AddButton(item.Item1, item.Item2);
                    result.Add(button);
                }
            });

            return new(result);
        }

        /// <summary>
        /// Creates new <see cref="HorizontalStackPanel"/> and adds
        /// it to the <see cref="AbstractControl.Children"/>.
        /// </summary>
        [Browsable(false)]
        public virtual HorizontalStackPanel AddHorizontalStackPanel()
        {
            var result = new HorizontalStackPanel
            {
                Parent = this,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> and adds it
        /// to the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual GroupBox AddGroupBox(string? title = default)
        {
            var result = new GroupBox
            {
                Parent = this,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> with vertical layout and adds
        /// it to the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual GroupBox AddVerticalGroupBox(string? title = default)
        {
            var result = new GroupBox
            {
                Layout = LayoutStyle.Vertical,
                Parent = this,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> with horizontal layout and adds
        /// it to the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual GroupBox AddHorizontalGroupBox(string? title = default)
        {
            var result = new GroupBox
            {
                Layout = LayoutStyle.Horizontal,
                Parent = this,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Adds multiple labels.
        /// </summary>
        /// <param name="text">Array of label text.</param>
        /// <returns><see cref="ControlSet"/> with list of created labels.</returns>
        public virtual ControlSet AddLabels(params string[] text)
        {
            List<AbstractControl> result = new();

            this.DoInsideLayout(() =>
            {
                foreach (var item in text)
                {
                    var label = AddLabel(item);
                    result.Add(label);
                }
            });

            return new(result);
        }

        /// <summary>
        /// Creates new <see cref="Label"/> and adds it
        /// to the <see cref="AbstractControl.Children"/>.
        /// </summary>
        public virtual Label AddLabel(string text)
        {
            var result = new Label(text)
            {
                Parent = this,
                VerticalAlignment = VerticalAlignment.Center,
            };

            return result;
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreatePanelHandler(this);
        }
    }
}
