namespace Alternet.UI.Integration.IntelliSense
{
    /// <summary>
    /// Represents edit to be applied to textbuffer
    /// For simplicity sake two types of manipulations are offered only - Insertion and Deletion
    /// </summary>
    public class TextManipulation
    {
        public static TextManipulation Insert(int position, string text)
        {
            return new TextManipulation(position, position + text.Length, text, ManipulationType.Insert);
        }
        public static TextManipulation Delete(int position, int length)
        {
            return new TextManipulation(position, position + length, null, ManipulationType.Delete);
        }

        private TextManipulation(int start, int end, string text, ManipulationType type)
        {
            Start = start;
            End = end;
            Text = text;
            Type = type;
        }

        public int Start { get; }

        public int End { get; }

        public string Text { get; }

        public ManipulationType Type { get; }
    }
}
