using System.Drawing;
using System.IO;

namespace Alternet.UI
{
    /// <summary>
    /// Provides static, predefined Clipboard and Drag and Drop format names.
    /// Use them to identify the format of data that you store in an <see cref="IDataObject"/>.
    /// </summary>
    public static class DataFormats
    {
        /// <summary>
        /// Specifies the text format. This static field is read-only.
        /// </summary>
        public static readonly string Text = "Text";

        /// <summary>
        /// Specifies the files format. This static field is read-only.
        /// </summary>
        public static readonly string Files = "Files";

        /// <summary>
        /// Specifies the bitmap format. This static field is read-only.
        /// </summary>
        public static readonly string Bitmap = "Bitmap";

        /// <summary>
        /// Specifies the standard Unicode text format.
        /// This <see langword="static" /> field is read-only.</summary>
        public static readonly string UnicodeText = "Text";

        /// <summary>
        /// Specifies the DIB data format.
        /// </summary>
        public static readonly string Dib = "DeviceIndependentBitmap";

        /// <summary>
        /// Specifies the Windows enhanced metafile format.
        /// </summary>
        public static readonly string EnhancedMetafile = "EnhancedMetafile";

        /// <summary>
        /// Specifies the Windows metafile picture data format.
        /// </summary>
        public static readonly string MetafilePicture = "MetaFilePict";

        /// <summary>
        /// Specifies the Windows symbolic link data format.
        /// </summary>
        public static readonly string SymbolicLink = "SymbolicLink";

        /// <summary>
        /// Specifies the Windows Data Interchange Format (DIF) data format.
        /// </summary>
        public static readonly string Dif = "DataInterchangeFormat";

        /// <summary>
        /// Specifies the Tagged Image File Format (TIFF) data format.
        /// </summary>
        public static readonly string Tiff = "TaggedImageFileFormat";

        /// <summary>
        /// Specifies the standard Windows OEM text data format.
        /// </summary>
        public static readonly string OemText = "OEMText";

        /// <summary>
        /// Specifies the Windows palette data format.
        /// </summary>
        public static readonly string Palette = "Palette";

        /// <summary>
        /// Specifies the Windows pen data format.
        /// </summary>
        public static readonly string PenData = "PenData";

        /// <summary>
        /// Specifies the Resource Interchange File Format (RIFF) audio data format.
        /// </summary>
        public static readonly string Riff = "RiffAudio";

        /// <summary>
        /// Specifies the wave audio data format.
        /// </summary>
        public static readonly string WaveAudio = "WaveAudio";

        /// <summary>
        /// Specifies the Windows file drop format.
        /// </summary>
        public static readonly string FileDrop = "FileDrop";

        /// <summary>
        /// Specifies the Windows locale (culture) data format.
        /// </summary>
        public static readonly string Locale = "Locale";

        /// <summary>
        /// Specifies the HTML data format.
        /// </summary>
        public static readonly string Html = "HTML Format";

        /// <summary>
        /// Specifies the Rich Text Format (RTF) data format.
        /// </summary>
        public static readonly string Rtf = "Rich Text Format";

        /// <summary>
        /// Specifies a comma-separated value (CSV) data format.
        /// </summary>
        public static readonly string CommaSeparatedValue = "CSV";

        /// <summary>
        /// Specifies the common language runtime (CLR) string class data format.
        /// </summary>
        public static readonly string StringFormat = typeof(string).FullName;

        /// <summary>
        /// Specifies a data format that encapsulates any type of serializable data objects.
        /// </summary>
        public static readonly string Serializable = "PersistentObject";

        /// <summary>
        /// Specifies the Extensible Application Markup Language (XAML) data format.
        /// </summary>
        public static readonly string Xaml = "Xaml";

        /// <summary>
        /// Specifies the Extensible Application Markup Language (XAML) package data format.
        /// </summary>
        public static readonly string XamlPackage = "XamlPackage";

        /// <summary>
        /// Gets string representation of the <see cref="TextDataFormat"/>.
        /// </summary>
        /// <param name="textDataformat">Data format.</param>
        /// <returns></returns>
        public static string ConvertToDataFormats(TextDataFormat textDataformat)
        {
            string result = UnicodeText;
            switch (textDataformat)
            {
                case TextDataFormat.Text:
                    result = Text;
                    break;
                /*case TextDataFormat.UnicodeText:
                    result = UnicodeText;
                    break;*/
                case TextDataFormat.Rtf:
                    result = Rtf;
                    break;
                case TextDataFormat.Html:
                    result = Html;
                    break;
                case TextDataFormat.CommaSeparatedValue:
                    result = CommaSeparatedValue;
                    break;
                case TextDataFormat.Xaml:
                    result = Xaml;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets whether specified data format is valid.
        /// </summary>
        /// <param name="textDataFormat">Data format.</param>
        /// <returns></returns>
        public static bool IsValidTextDataFormat(TextDataFormat textDataFormat)
        {
            if (textDataFormat == TextDataFormat.Text || textDataFormat == TextDataFormat.UnicodeText
                || textDataFormat == TextDataFormat.Rtf || textDataFormat == TextDataFormat.Html
                || textDataFormat == TextDataFormat.CommaSeparatedValue
                || textDataFormat == TextDataFormat.Xaml)
            {
                return true;
            }

            return false;
        }
    }
}