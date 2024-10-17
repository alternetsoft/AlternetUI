using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
    /// <summary>
    /// This is just a test class.
    /// </summary>
    public class MyClass
    {
        /// <summary>
        /// This is just a test property
        /// </summary>
        public static string MyProperty { get; set; } = string.Empty;

        /// <summary>
        /// Gets the current time
        /// </summary>
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// This is just a test method.
        /// </summary>
        /// <param name="firstParam">This is a first integer parameter.</param>
        /// <param name="secondParam">This is a second integer parameter.</param>
        /// <returns>The sum of two parameters.</returns>
        public int FirstMethod(int firstParam, int secondParam)
        {
            return firstParam + secondParam;
        }

        /// <summary>
        /// This is a second method.
        /// </summary>
        /// <param name="booleanParam">Just a boolean value.</param>
        public void SecondMethod(bool booleanParam)
        {
            
        }

    }
}
