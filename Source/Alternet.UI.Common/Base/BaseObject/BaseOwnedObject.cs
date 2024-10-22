﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class with properties and methods common to all child objects.
    /// </summary>
    public class BaseOwnedObject<T> : BaseObjectWithId
    {
        private readonly T owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseOwnedObject{T}"/> class.
        /// </summary>
        public BaseOwnedObject(T owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Gets owner.
        /// </summary>
        [Browsable(false)]
        public T Owner => owner;
    }
}
