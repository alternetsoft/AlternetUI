namespace Alternet.UI
{
    /// <summary>
    /// Contains parameters for the Help file opening.
    /// </summary>
    public class HelpInfo
    {
        private readonly string? helpFilePath;
        private readonly string? keyword;
        private readonly HelpNavigator? navigator;
        private readonly object? param;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpInfo"/> class.
        /// </summary>
        /// <param name="helpFilePath">The path and name of the Help file to display when the user clicks
        /// the Help button.</param>
        public HelpInfo(string helpFilePath)
        {
            this.helpFilePath = helpFilePath;
            navigator = HelpNavigator.TableOfContents;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpInfo"/> class.
        /// </summary>
        /// <param name="helpFilePath">The path and name of the Help file to display when the user clicks
        /// the Help button.</param>
        /// <param name="keyword">The Help keyword to display when the user clicks the Help button.</param>
        public HelpInfo(string helpFilePath, string keyword)
        {
            this.helpFilePath = helpFilePath;
            this.keyword = keyword;
            navigator = HelpNavigator.TableOfContents;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpInfo"/> class.
        /// </summary>
        /// <param name="helpFilePath">The path and name of the Help file to display when the user clicks
        /// the Help button.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator" /> values.</param>
        public HelpInfo(string helpFilePath, HelpNavigator navigator)
        {
            this.helpFilePath = helpFilePath;
            this.navigator = navigator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpInfo"/> class.
        /// </summary>
        /// <param name="helpFilePath">The path and name of the Help file to display when the user clicks
        /// the Help button.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator" /> values.</param>
        /// <param name="param">The numeric ID of the Help topic to display when the user clicks the Help button.</param>
        public HelpInfo(string helpFilePath, HelpNavigator navigator, object param)
        {
            this.helpFilePath = helpFilePath;
            this.navigator = navigator;
            this.param = param;
        }

        /// <summary>
        /// Gets or sets the path and name of the help file to display when the user clicks
        /// the Help button.
        /// </summary>
        public string? HelpFilePath => helpFilePath;

        /// Gets or sets the Help keyword to display when the user clicks the Help button.
        public string? Keyword => keyword;

        /// Gets or sets One of the <see cref="HelpNavigator" /> values.
        public HelpNavigator? Navigator => navigator;

        /// Gets or sets the numeric ID of the Help topic to display when the user clicks the Help button.
        public object? Param => param;

        /// <summary>
        /// Gets string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{HelpFilePath={helpFilePath}, keyword ={keyword}, navigator={navigator}}}";
        }
    }
}
