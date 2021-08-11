using Alternet.UI;
using System.Drawing;

namespace DrawingSample
{
    public sealed class TextLinesLayer : Layer
    {
        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nSuspendisse tincidunt orci vitae arcu congue commodo.\nProin fermentum rhoncus dictum.\nPhasellus mattis nibh eu euismod placerat.\nNam luctus euismod ex, vel aliquam libero auctor placerat.\nDonec et dignissim justo, eget dignissim massa.\nDonec dapibus, erat ut feugiat sollicitudin, est elit malesuada massa, ac convallis mi turpis auctor elit.\nIn semper ullamcorper purus et ultrices.\nQuisque mollis mauris sit amet enim sollicitudin vulputate.\nDonec eu tortor tincidunt ligula tempor semper ut sit amet velit.\nIn mollis libero dolor, ut ultrices lectus elementum at.\nProin eget nisi in arcu semper bibendum vel efficitur mi.";

        private Alternet.UI.Font font = new Alternet.UI.Font("Times New Roman", 15);

        public override string Name => "Text Lines";

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            dc.DrawText(LoremIpsum, new PointF(20, 20), font, Color.DarkViolet);
        }
    }
}