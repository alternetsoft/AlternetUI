using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Descendant of the <see cref="PictureBox"/> which is initialized to show
    /// text validation or other errors.
    /// </summary>
    public class ErrorPictureBox : PictureBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorPictureBox"/> class.
        /// </summary>
        public ErrorPictureBox()
        {
            TextBox.InitErrorPicture(this);
        }
    }
}
