using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the create property
    /// event in <see cref="PropertyGrid"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="CreatePropertyEventArgs" /> object that
    /// contains the event data.</param>
    public delegate void CreatePropertyEventHandler(object? sender, CreatePropertyEventArgs e);

    /// <summary>
    /// Provides data for the create property event in <see cref="PropertyGrid"/>.
    /// </summary>
    public class CreatePropertyEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePropertyEventArgs"/> class.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="propName">Property name in <see cref="PropertyGrid"/>.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="propInfo">Property information.</param>
        /// <remarks>
        /// In order to override default property item creation mechanism, you need
        /// to create <see cref="IPropertyGridItem"/> instance and assign it to
        /// <see cref="PropertyItem"/> property. Also you need to set
        /// <see cref="HandledEventArgs.Handled"/> to <c>true</c>. Also you can leave
        /// default value (null) in the <see cref="PropertyItem"/>. In this case
        /// property will not be added to <see cref="PropertyGrid"/>.
        /// </remarks>
        public CreatePropertyEventArgs(
            string label,
            string propName,
            object instance,
            PropertyInfo propInfo)
        {
            Label = label;
            PropName = propName;
            Instance = instance;
            PropInfo = propInfo;
        }

        /// <summary>
        /// Property label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Property name in <see cref="PropertyGrid"/>.
        /// </summary>
        public string PropName { get; set; }

        /// <summary>
        /// Object instance which contains the property.
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Property information.
        /// </summary>
        public PropertyInfo PropInfo { get; set; }

        /// <summary>
        /// Result property item.
        /// </summary>
        public IPropertyGridItem? PropertyItem { get; set; }
    }
}