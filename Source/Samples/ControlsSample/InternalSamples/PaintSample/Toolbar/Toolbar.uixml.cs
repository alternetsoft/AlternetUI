using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    public partial class Toolbar : Control
    {
        private readonly List<SpeedButton> toolButtons = new();
        private Tools? tools;

        public Toolbar()
        {
            InitializeComponent();

            CommandButtons = new Collection<SpeedButton>();
            CommandButtons.ItemInserted += CommandButtons_ItemInserted;
            CommandButtons.ItemRemoved += CommandButtons_ItemRemoved;
        }

        private void CommandButtons_ItemRemoved(object? sender, int index, SpeedButton item)
        {
            commandButtonsContainer.Children.Remove(item);
        }

        private void CommandButtons_ItemInserted(object? sender, int index, SpeedButton item)
        {
            item.Margin = new Thickness(0, 0, 5, 0);
            item.HorizontalAlignment = HorizontalAlignment.Center;
            commandButtonsContainer.Children.Add(item);
        }

        public void SetTools(Tools tools)
        {
            if (this.tools != null)
                throw new InvalidOperationException();

            this.tools = tools;

            foreach (var tool in tools.AllTools)
            {
                var button = new SpeedButton()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 5, 0),
                    ToolTip = tool.Name,
                };
                button.Image = MainWindow.LoadToolImage(tool);
                button.ImageStretch = true;
                button.Padding = 5;
                button.CenterHorz = true;
                button.CenterVert = true;
                button.SuggestedSize = 32;
                button.FlagsAndAttributes.SetAttribute("Tool", tool);

                button.Click += Button_ToggledChanged;

                toolButtons.Add(button);
                toolButtonsContainer.Children.Add(button);
            }

            UpdateCurrentTool();
            tools.CurrentToolChanged += Tools_CurrentToolChanged;
        }

        public Collection<SpeedButton> CommandButtons { get; }

        private void Tools_CurrentToolChanged(object? sender, EventArgs e)
        {
            UpdateCurrentTool();
        }

        private void UpdateCurrentTool()
        {
            if (tools == null)
                throw new InvalidOperationException();

            skipToggledEvent = true;

            foreach (var button in toolButtons.Where(x => x.Sticky))
                button.Sticky = false;

            toolButtons.First(
                x => x.FlagsAndAttributes.GetAttribute("Tool") == tools.CurrentTool).Sticky = true;
            
            skipToggledEvent = false;

            optionsPlaceholder.Children.Clear();

            var optionsControl = tools.CurrentTool?.OptionsControl;

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

            tools.CurrentTool = (sender as SpeedButton)?.FlagsAndAttributes.GetAttribute("Tool")
                as Tool;
        }
    }
}