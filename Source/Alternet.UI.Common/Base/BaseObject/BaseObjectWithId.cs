using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="BaseObject"/> with <see cref="UniqueId"/>
    /// and id related features.
    /// </summary>
    public partial class BaseObjectWithId : BaseObject, IBaseObjectWithId
    {
        private readonly object locker = new();

        private ObjectUniqueId uniqueId = ObjectUniqueId.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObjectWithId"/> class.
        /// </summary>
        public BaseObjectWithId()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObjectWithId"/> class with the specified unique id.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for the object. If you pass <see cref="ObjectUniqueId.Empty"/>,
        /// the ID will be generated on first access.</param>
        public BaseObjectWithId(ObjectUniqueId uniqueId)
        {
            this.uniqueId = uniqueId;
        }

        /// <summary>
        /// Gets a value indicating whether the unique identifier has been initialized.
        /// Checking <see cref="IsUniqueIdInitialized"/> does not generate an identifier.
        /// </summary>
        [Browsable(false)]
        public bool IsUniqueIdInitialized
        {
            get
            {
                lock (locker)
                {
                    return uniqueId.State != ObjectUniqueId.StateKind.None;
                }
            }
        }

        /// <summary>
        /// Gets the object's identifier. If no identifier was supplied at construction,
        /// one is generated on first access. Checking <see cref="IsUniqueIdInitialized"/>
        /// does not generate an identifier.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId UniqueId
        {
            get
            {
                lock (locker)
                {
                    if (uniqueId.State == ObjectUniqueId.StateKind.None)
                    {
                        uniqueId = new(UseGuidForUniqueId);
                    }

                    return uniqueId;
                }
            }
        }

        /// <summary>
        /// Gets whether a GUID should be used when generating the unique identifier.
        /// This property is evaluated once, when the identifier is first generated,
        /// while the object's identifier lock is held.
        /// Override it to customize the generation strategy.
        /// </summary>
        protected virtual bool UseGuidForUniqueId => false;
    }
}
