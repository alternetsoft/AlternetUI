using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public class GenericToolBar : Control
    {
        private double defaultSize = 24;

        private readonly StackPanel panel = new()
        {
            Orientation = StackPanelOrientation.Horizontal,
        };

        public GenericToolBar()
        {
            Height = defaultSize;
            SuggestedHeight = defaultSize;
            panel.Parent = this;
            TabStop = false;
            AcceptsFocusAll = false;
        }

        public virtual ObjectUniqueId Add(
            string? text,
            ImageSet? imageSet,
            ImageSet? imageSetDisabled,
            string? toolTip,
            EventHandler? action = null)
        {
            SizeI imageSize = Toolbar.GetDefaultImageSize(this);

            var image = imageSet?.AsImage(imageSize);
            var imageDisabled = imageSetDisabled?.AsImage(imageSize);

            SpeedButton speedButton = new()
            {
                Image = image,
                ToolTip = toolTip,
                SuggestedSize = defaultSize,
            };

            if (imageDisabled is not null)
            {
                speedButton.DisabledImage = imageDisabled;
            }

            if (BackgroundColor is not null)
                speedButton.BackgroundColor = BackgroundColor;


            speedButton.Click += action;
            panel.Children.Add(speedButton);

            return speedButton.UniqueId;
        }

        public virtual void SetToolVisible(ObjectUniqueId id, bool visible)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.Visible = visible;
        }

        public virtual void SetToolEnabled(ObjectUniqueId id, bool enabled)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.Enabled = enabled;
        }

        public virtual bool GetToolEnabled(ObjectUniqueId id)
        {
            var item = FindTool(id);
            if (item is null)
                return false;
            return item.Enabled;
        }

        public virtual bool GetToolVisible(ObjectUniqueId id)
        {
            var item = FindTool(id);
            if (item is null)
                return false;
            return item.Visible;
        }

        public virtual void DeleteTool(ObjectUniqueId id)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.Parent = null;
            item.Dispose();
        }

        public virtual int GetToolCount()
        {
            return panel.Children.Count;
        }

        public virtual ObjectUniqueId GetToolId(int index)
        {
            return panel.Children[index].UniqueId;
        }

        private SpeedButton? FindTool(ObjectUniqueId id)
        {
            var result = panel.FindChild(id) as SpeedButton;
            return result;
        }
    }
}
