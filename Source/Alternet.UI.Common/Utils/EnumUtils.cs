using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to enums.
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Converts <see cref="ModalResult"/> to <see cref="DialogResult"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DialogResult Convert(ModalResult value)
        {
            switch (value)
            {
                case ModalResult.None:
                default:
                    return DialogResult.None;
                case ModalResult.Canceled:
                    return DialogResult.Cancel;
                case ModalResult.Accepted:
                    return DialogResult.OK;
            }
        }

        /// <summary>
        /// Gets maximum value of the enum.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        public static T GetMaxValue<T>()
            where T : struct, Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Last();
        }

        /// <summary>
        /// Gets maximum value of the enum as integer.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        public static int GetMaxValueAsInt<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<int>().Last();
        }
    }
}
