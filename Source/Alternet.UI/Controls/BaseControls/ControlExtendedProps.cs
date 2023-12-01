namespace Alternet.UI
{
    internal class ControlExtendedProps
    {
        public int GridColumnSpan { get; set; } = 1;

        public int GridRowSpan { get; set; } = 1;

        internal double DistanceRight { get; set; }

        internal double DistanceBottom { get; set; }

        internal AutoSizeMode AutoSizeMode { get; set; }

        internal AnchorStyles Anchor { get; set; }

        internal DockStyle Dock { get; set; }

        internal bool AutoSize { get; set; }
    }
}