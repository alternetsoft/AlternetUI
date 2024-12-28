using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="BaseObject"/> with <see cref="UniqueId"/>
    /// and id related features.
    /// </summary>
    public partial class BaseObjectWithId : BaseObject, IBaseObjectWithId
    {
        private readonly object locker = new();

        private ObjectUniqueId? uniqueId;

        /// <summary>
        /// Gets unique id of this object.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId UniqueId
        {
            get
            {
                if(uniqueId is null)
                {
                    lock (locker)
                    {
                        uniqueId = new();
                    }
                }

                return uniqueId.Value;
            }
        }
    }
}
