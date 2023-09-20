using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the category in which the control will be displayed in a tool box.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ControlCategoryAttribute : Attribute
    {
        private readonly object locker = new();
        private readonly string categoryId;
        private bool localized;
        private string? categoryTitle;

        /// <summary>
        /// Initializes a new instance of the <see cref='ControlCategoryAttribute'/>
        /// class with the specified category name.
        /// </summary>
        public ControlCategoryAttribute(string categoryId)
        {
            this.categoryId = categoryId;
        }

        /// <summary>
        /// Gets the id of the category for the control that this attribute is
        /// bound to.
        /// </summary>
        public string CategoryId => categoryId;

        /// <summary>
        /// Gets the title of the category for the control that this attribute is
        /// bound to.
        /// </summary>
        public string CategoryTitle
        {
            get
            {
                if (!localized)
                {
                    lock (locker)
                    {
                        string? localizedValue = GetLocalizedTitle(categoryId);
                        if (localizedValue != null)
                            categoryTitle = localizedValue;

                        localized = true;
                    }
                }

                return categoryTitle ?? categoryId;
            }
        }

        /// <summary>
        /// Gets localized title for the specified category id.
        /// </summary>
        /// <param name="id">Category id.</param>
        public static string GetLocalizedTitle(string id)
        {
            return ControlCategoryStrings.Default.GetLocalizedTitle(id);
        }
    }
}