using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Alternet.UI
{
    public partial class UixmlLoader
    {
        /// <summary>
        /// Gets or sets xml string for an empty window.
        /// </summary>
        internal static string EmptyWindowXml =
            @"<Window xmlns = ""http://schemas.alternetsoft.com/ui/2021""" +
            @"        xmlns:x=""http://schemas.alternetsoft.com/ui/2021/uixml""" +
            @"        x:Class=""Window""" +
            @"        Title=""Window1"">" +
            @"</Window>";

        /// <summary>
        /// Prepares uixml stream for the preview. This method demoves event assignments from the uixml
        /// and makes other preview related changes.
        /// </summary>
        /// <param name="stream">Stream with uixml data.</param>
        /// <returns>Stream with changed uixml.</returns>
        public static Stream? PrepareUixmlStreamForPreview(Stream stream)
        {
            bool unsupportedFormat = false;

            var prm = new XmlUtils.XmlStreamConvertParams(ChildFn);
            prm.RootNodeAction = RootFn;

            var result = XmlUtils.ConvertXml(stream, prm);
            if (unsupportedFormat)
                return null;
            return result;

            bool RootFn(XmlNode node, object? prm)
            {
                if (node is not XmlElement element)
                    return false;

                var name = node.Name;

                if (name == "Control")
                    Set("Control");
                else
                if (name == "Window")
                    Set("Window");
                else
                {
                    var descendants = AssemblyUtils.AllControlDescendants;
                    var fullName = "Alternet.UI." + name;
                    if(descendants.ContainsKey(fullName))
                        Set(fullName);
                    else
                        unsupportedFormat = true;
                }

                void Set(string xClass)
                {
                    element.SetAttribute("Class", @"http://schemas.alternetsoft.com/ui/2021/uixml", xClass);
                }

                return RemoveEventNames(element);
            }

            bool RemoveEventNames(XmlElement element)
            {
                var result = false;
                var attrs = XmlUtils.GetAttributes(element);
                foreach (var attr in attrs)
                {
                    var name = attr.Name;
                    var isEvent = AssemblyUtils.IsControlDescendantEventName(name);
                    if (isEvent)
                    {
                        element.RemoveAttribute(name);
                        result = true;
                    }
                }

                return result;
            }

            bool ChildFn(XmlNode node, object? prm)
            {
                if (node is not XmlElement element)
                    return false;
                return RemoveEventNames(element);
            }
        }
    }
}
