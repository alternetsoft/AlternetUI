using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    internal partial class Toolbar : Control
    {
        private List<ToolButton> toolButtons = new List<ToolButton>();

        private Tools? tools;

        public Toolbar()
        {
            InitializeComponent();

            CommandButtons = new Collection<CommandButton>();
            CommandButtons.ItemInserted += CommandButtons_ItemInserted;
            CommandButtons.ItemRemoved += CommandButtons_ItemRemoved;
        }

        private void CommandButtons_ItemRemoved(object? sender, CollectionChangeEventArgs<CommandButton> e)
        {
            commandButtonsContainer.Children.Remove(e.Item);
        }

        private void CommandButtons_ItemInserted(object? sender, CollectionChangeEventArgs<CommandButton> e)
        {
            e.Item.Margin = new Thickness(0, 0, 5, 0);
            e.Item.HorizontalAlignment = HorizontalAlignment.Center;
            commandButtonsContainer.Children.Add(e.Item);
        }

        public void SetTools(Tools tools)
        {
            if (this.tools != null)
                throw new InvalidOperationException();

            this.tools = tools;

            foreach (var tool in tools.AllTools)
            {
                var button = new ToolButton(tool)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 5, 0),
                    ToolTip = tool.Name
                };

                button.ToggledChanged += Button_ToggledChanged;

                toolButtons.Add(button);
                toolButtonsContainer.Children.Add(button);
            }

            UpdateCurrentTool();
            tools.CurrentToolChanged += Tools_CurrentToolChanged;
        }

        public Collection<CommandButton> CommandButtons { get; }

        private void Tools_CurrentToolChanged(object? sender, EventArgs e)
        {
            UpdateCurrentTool();
        }

        private void UpdateCurrentTool()
        {
            if (tools == null)
                throw new InvalidOperationException();

            skipToggledEvent = true;

            foreach (var button in toolButtons.Where(x => x.IsToggled))
                button.IsToggled = false;

            toolButtons.First(x => x.Tool == tools.CurrentTool).IsToggled = true;
            
            skipToggledEvent = false;

            optionsPlaceholder.Children.Clear();

            var optionsControl = tools.CurrentTool.OptionsControl;

            if (optionsControl != null)
                optionsPlaceholder.Children.Add(optionsControl);
        }

        bool skipToggledEvent;

        private void Button_ToggledChanged(object? sender, EventArgs e)
        {
            if (skipToggledEvent)
                return;

            if (tools == null)
                throw new InvalidOperationException();

            tools.CurrentTool = ((ToolButton)sender!).Tool;
        }
    }
}