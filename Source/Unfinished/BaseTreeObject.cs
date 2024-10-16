using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class BaseTreeObject : ImmutableObject
    {
        private BaseTreeObject? parent;
        private BaseTreeObject? firstChild;
        private BaseTreeObject? lastChild;
        private BaseTreeObject? previousSibling;
        private BaseTreeObject? nextSibling;

        public BaseTreeObject()
        {
        }

        public BaseTreeObject? Parent
        {
            get
            {
                return parent;
            }
        }

        public BaseTreeObject? FirstChild
        {
            get
            {
                return firstChild;
            }
        }

        public BaseTreeObject? LastChild
        {
            get
            {
                return lastChild;
            }
        }

        private BaseTreeObject? PreviousSibling
        {

        }

        private BaseTreeObject? NextSibling
        {

        }
    }
}
