#nullable disable
#pragma warning disable IDE0079
#pragma warning disable SA1310 // Field names should not contain underscore

using System.Resources;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static properties which identify string resource names.
    /// </summary>
    public static partial class SRID
    {
        /// <summary>en</summary>
        public const string @WPF_UILanguage = "WPF_UILanguage";

        /// <summary>Cannot modify this property on the Empty Rect.</summary>
        public const string @Rect_CannotModifyEmptyRect = "Rect_CannotModifyEmptyRect";

        /// <summary>Cannot call this method on the Empty Rect.</summary>
        public const string @Rect_CannotCallMethod = "Rect_CannotCallMethod";

        /// <summary>Width and Height must be non-negative.</summary>
        public const string @Size_WidthAndHeightCannotBeNegative =
            "Size_WidthAndHeightCannotBeNegative";

        /// <summary>Width must be non-negative.</summary>
        public const string @Size_WidthCannotBeNegative = "Size_WidthCannotBeNegative";

        /// <summary>Height must be non-negative.</summary>
        public const string @Size_HeightCannotBeNegative = "Size_HeightCannotBeNegative";

        /// <summary>Cannot modify this property on the Empty Size.</summary>
        public const string @Size_CannotModifyEmptySize = "Size_CannotModifyEmptySize";

        /// <summary>Transform is not invertible.</summary>
        public const string @Transform_NotInvertible = "Transform_NotInvertible";

        /// <summary>Expected object of type '{0}'.</summary>
        public const string @General_Expected_Type = "General_Expected_Type";

        /// <summary>Value cannot be null. Object reference: '{0}'.</summary>
        public const string @ReferenceIsNull = "ReferenceIsNull";

        /// <summary>The parameter value must be between '{0}' and '{1}'.</summary>
        public const string @ParameterMustBeBetween = "ParameterMustBeBetween";

        /// <summary>Handler has not been registered with this event.</summary>
        public const string @Freezable_UnregisteredHandler = "Freezable_UnregisteredHandler";

        /// <summary>Cannot use a DependencyObject that belongs to a different thread than its
        /// parent Freezable.</summary>
        public const string @Freezable_AttemptToUseInnerValueWithDifferentThread =
            "Freezable_AttemptToUseInnerValueWithDifferentThread";

        /// <summary>This Freezable cannot be frozen.</summary>
        public const string @Freezable_CantFreeze = "Freezable_CantFreeze";

        /// <summary>The provided DependencyObject is not a context for this Freezable.</summary>
        public const string @Freezable_NotAContext = "Freezable_NotAContext";

        /// <summary>Cannot promote from '{0}' to '{1}' because the target map is too small.</summary>
        public const string @FrugalList_TargetMapCannotHoldAllData =
            "FrugalList_TargetMapCannotHoldAllData";

        /// <summary>Cannot promote from Array.</summary>
        public const string @FrugalList_CannotPromoteBeyondArray =
            "FrugalList_CannotPromoteBeyondArray";

        /// <summary>Cannot promote from '{0}' to '{1}' because the target map is too small.</summary>
        public const string @FrugalMap_TargetMapCannotHoldAllData =
            "FrugalMap_TargetMapCannotHoldAllData";

        /// <summary>Cannot promote from Hashtable.</summary>
        public const string @FrugalMap_CannotPromoteBeyondHashtable =
            "FrugalMap_CannotPromoteBeyondHashtable";

        /// <summary>Unrecognized Key '{0}'.</summary>
        public const string @Unsupported_Key = "Unsupported_Key";

        /// <summary>Specified priority is not valid.</summary>
        public const string @InvalidPriority = "InvalidPriority";

        /// <summary>The minimum priority must be less than or equal to the maximum
        /// priority.</summary>
        public const string @InvalidPriorityRangeOrder = "InvalidPriorityRangeOrder";

        /// <summary>Cannot perform requested operation because the Dispatcher shut down.</summary>
        public const string @DispatcherHasShutdown = "DispatcherHasShutdown";

        /// <summary>A thread cannot wait on operations already running on the same thread.</summary>
        public const string @ThreadMayNotWaitOnOperationsAlreadyExecutingOnTheSameThread = "ThreadMayNotWaitOnOperationsAlreadyExecutingOnTheSameThread";

        /// <summary>The calling thread cannot access this object because a different thread
        /// owns it.</summary>
        public const string @VerifyAccess = "VerifyAccess";

        /// <summary>Objects must be created by the same thread.</summary>
        public const string @MismatchedDispatchers = "MismatchedDispatchers";

        /// <summary>Dispatcher processing has been suspended, but messages are still
        /// being processed.</summary>
        public const string @DispatcherProcessingDisabledButStillPumping = "DispatcherProcessingDisabledButStillPumping";

        /// <summary>Cannot perform this operation while dispatcher processing is suspended.</summary>
        public const string @DispatcherProcessingDisabled = "DispatcherProcessingDisabled";

        /// <summary>The DispatcherPriorityAwaiter was not configured with a valid Dispatcher.
        /// The only supported usage is from Dispatcher.Yield.</summary>
        public const string @DispatcherPriorityAwaiterInvalid = "DispatcherPriorityAwaiterInvalid";

        /// <summary>The thread calling Dispatcher.Yield does not have a current Dispatcher.</summary>
        public const string @DispatcherYieldNoAvailableDispatcher = "DispatcherYieldNoAvailableDispatcher";

        /// <summary>The Dispatcher is unable to request processing.  This is often because
        /// the application has starved the Dispatcher's message pump.</summary>
        public const string @DispatcherRequestProcessingFailed = "DispatcherRequestProcessingFailed";

        /// <summary>Exception Filter Code is not built and installed properly.</summary>
        public const string @ExceptionFilterCodeNotPresent = "ExceptionFilterCodeNotPresent";

        /// <summary>Unrecognized ModifierKeys '{0}'.</summary>
        public const string @Unsupported_Modifier = "Unsupported_Modifier";

        /// <summary>TimeSpan period must be greater than or equal to zero.</summary>
        public const string @TimeSpanPeriodOutOfRange_TooSmall = "TimeSpanPeriodOutOfRange_TooSmall";

        /// <summary>TimeSpan period must be less than or equal to Int32.MaxValue.</summary>
        public const string @TimeSpanPeriodOutOfRange_TooLarge = "TimeSpanPeriodOutOfRange_TooLarge";

        /// <summary>Cannot clear properties on object '{0}' because it is in a read-only
        /// state.</summary>
        public const string @ClearOnReadOnlyObjectNotAllowed = "ClearOnReadOnlyObjectNotAllowed";

        /// <summary>Cannot automatically generate a valid default value for property '{0}'.
        /// Specify a default value explicitly when owner type '{1}' is registering this DependencyProperty.</summary>
        public const string @DefaultValueAutoAssignFailed = "DefaultValueAutoAssignFailed";

        /// <summary>An Expression object is not a valid default value for a
        /// DependencyProperty.</summary>
        public const string @DefaultValueMayNotBeExpression = "DefaultValueMayNotBeExpression";

        /// <summary>Default value cannot be 'Unset'.</summary>
        public const string @DefaultValueMayNotBeUnset = "DefaultValueMayNotBeUnset";

        /// <summary>Default value for the '{0}' property cannot be bound to a specific
        /// thread.</summary>
        public const string @DefaultValueMustBeFreeThreaded = "DefaultValueMustBeFreeThreaded";

        /// <summary>Default value type does not match type of property '{0}'.</summary>
        public const string @DefaultValuePropertyTypeMismatch = "DefaultValuePropertyTypeMismatch";

        /// <summary>Default value for '{0}' property is not valid because
        /// ValidateValueCallback failed.</summary>
        public const string @DefaultValueInvalid = "DefaultValueInvalid";

        /// <summary>'{0}' type does not have a matching DependencyObjectType.</summary>
        public const string @DTypeNotSupportForSystemType = "DTypeNotSupportForSystemType";

        /// <summary>'{0}' is not a valid value for property '{1}'.</summary>
        public const string @InvalidPropertyValue = "InvalidPropertyValue";

        /// <summary>Local value enumeration position is out of range.</summary>
        public const string @LocalValueEnumerationOutOfBounds = "LocalValueEnumerationOutOfBounds";

        /// <summary>Local value enumeration position is before the start, need to call
        /// MoveNext first.</summary>
        public const string @LocalValueEnumerationReset = "LocalValueEnumerationReset";

        /// <summary>Current local value enumeration is outdated because one or more local
        /// values have been set since its creation.</summary>
        public const string @LocalValueEnumerationInvalidated = "LocalValueEnumerationInvalidated";

        /// <summary>Default value factory user must override
        /// PropertyMetadata.CreateDefaultValue.</summary>
        public const string @MissingCreateDefaultValue = "MissingCreateDefaultValue";

        /// <summary>Metadata override and base metadata must be of the same type or derived
        /// type.</summary>
        public const string @OverridingMetadataDoesNotMatchBaseMetadataType = "OverridingMetadataDoesNotMatchBaseMetadataType";

        /// <summary>'{0}' property was already registered by '{1}'.</summary>
        public const string @PropertyAlreadyRegistered = "PropertyAlreadyRegistered";

        /// <summary>This method overrides metadata only on read-only properties. This property
        /// is not read-only.</summary>
        public const string @PropertyNotReadOnly = "PropertyNotReadOnly";

        /// <summary>'{0}' property was registered as read-only and cannot be modified without an authorization key.</summary>
        public const string @ReadOnlyChangeNotAllowed = "ReadOnlyChangeNotAllowed";

        /// <summary>Property key is not authorized to modify property '{0}'.</summary>
        public const string @ReadOnlyKeyNotAuthorized = "ReadOnlyKeyNotAuthorized";

        /// <summary>'{0}' property was registered as read-only and its metadata cannot be
        /// overridden without an authorization key.</summary>
        public const string @ReadOnlyOverrideNotAllowed = "ReadOnlyOverrideNotAllowed";

        /// <summary>Property key is not authorized to override metadata of property '{0}'.</summary>
        public const string @ReadOnlyOverrideKeyNotAuthorized = "ReadOnlyOverrideKeyNotAuthorized";

        /// <summary>'{0}' is registered as read-only, so its value cannot be coerced by using
        /// the DesignerCoerceValueCallback.</summary>
        public const string @ReadOnlyDesignerCoersionNotAllowed = "ReadOnlyDesignerCoersionNotAllowed";

        /// <summary>Cannot set a property on object '{0}' because it is in a read-only
        /// state.</summary>
        public const string @SetOnReadOnlyObjectNotAllowed = "SetOnReadOnlyObjectNotAllowed";

        /// <summary>Shareable Expression cannot use ChangeSources method.</summary>
        public const string @ShareableExpressionsCannotChangeSources = "ShareableExpressionsCannotChangeSources";

        /// <summary>Cannot set Expression. It is marked as 'NonShareable' and has already been used.</summary>
        public const string @SharingNonSharableExpression = "SharingNonSharableExpression";

        /// <summary>ShouldSerializeProperty and ResetProperty methods must be public ('{0}').</summary>
        public const string @SpecialMethodMustBePublic = "SpecialMethodMustBePublic";

        /// <summary>Must create DependencySource on same Thread as the DependencyObject.</summary>
        public const string @SourcesMustBeInSameThread = "SourcesMustBeInSameThread";

        /// <summary>Expression is not in use on DependencyObject. Cannot change DependencySource array.</summary>
        public const string @SourceChangeExpressionMismatch = "SourceChangeExpressionMismatch";

        /// <summary>DependencyProperty limit has been exceeded upon registration of '{0}'. Dependency properties are normally static class members registered with static field initializers or static constructors. In this case, there may be dependency properties accidentally g ...</summary>
        public const string @TooManyDependencyProperties = "TooManyDependencyProperties";

        /// <summary>Metadata is already associated with a type and property. A new one must be created.</summary>
        public const string @TypeMetadataAlreadyInUse = "TypeMetadataAlreadyInUse";

        /// <summary>PropertyMetadata is already registered for type '{0}'.</summary>
        public const string @TypeMetadataAlreadyRegistered = "TypeMetadataAlreadyRegistered";

        /// <summary>'{0}' type must derive from DependencyObject.</summary>
        public const string @TypeMustBeDependencyObjectDerived = "TypeMustBeDependencyObjectDerived";

        /// <summary>Unrecognized Expression 'Mode' value.</summary>
        public const string @UnknownExpressionMode = "UnknownExpressionMode";

        /// <summary>Buffer is too small to accommodate the specified parameters.</summary>
        public const string @BufferTooSmall = "BufferTooSmall";

        /// <summary>Buffer offset cannot be negative.</summary>
        public const string @BufferOffsetNegative = "BufferOffsetNegative";

        /// <summary>CompoundFile path must be non-empty.</summary>
        public const string @CompoundFilePathNullEmpty = "CompoundFilePathNullEmpty";

        /// <summary>Cannot create new package on a read-only stream.</summary>
        public const string @CanNotCreateContainerOnReadOnlyStream = "CanNotCreateContainerOnReadOnlyStream";

        /// <summary>Cannot create a read-only stream.</summary>
        public const string @CanNotCreateAsReadOnly = "CanNotCreateAsReadOnly";

        /// <summary>Cannot create a stream in a read-only package.</summary>
        public const string @CanNotCreateInReadOnly = "CanNotCreateInReadOnly";

        /// <summary>Cannot create StorageRoot on a nonreadable stream.</summary>
        public const string @CanNotCreateStorageRootOnNonReadableStream = "CanNotCreateStorageRootOnNonReadableStream";

        /// <summary>Cannot delete element.</summary>
        public const string @CanNotDelete = "CanNotDelete";

        /// <summary>Cannot delete element because access is denied.</summary>
        public const string @CanNotDeleteAccessDenied = "CanNotDeleteAccessDenied";

        /// <summary>Cannot create data storage because access is denied.</summary>
        public const string @CanNotCreateAccessDenied = "CanNotCreateAccessDenied";

        /// <summary>Cannot delete read-only packages.</summary>
        public const string @CanNotDeleteInReadOnly = "CanNotDeleteInReadOnly";

        /// <summary>Cannot delete because the storage is not empty. Try a recursive delete with Delete(true).</summary>
        public const string @CanNotDeleteNonEmptyStorage = "CanNotDeleteNonEmptyStorage";

        /// <summary>Cannot delete the root StorageInfo.</summary>
        public const string @CanNotDeleteRoot = "CanNotDeleteRoot";

        /// <summary>Cannot perform this function on a storage that does not exist.</summary>
        public const string @CanNotOnNonExistStorage = "CanNotOnNonExistStorage";

        /// <summary>Cannot open data storage.</summary>
        public const string @CanNotOpenStorage = "CanNotOpenStorage";

        /// <summary>Cannot find specified package file.</summary>
        public const string @ContainerNotFound = "ContainerNotFound";

        /// <summary>Cannot open specified package file.</summary>
        public const string @ContainerCanNotOpen = "ContainerCanNotOpen";

        /// <summary>Create mode parameter must be either FileMode.Create or FileMode.Open.</summary>
        public const string @CreateModeMustBeCreateOrOpen = "CreateModeMustBeCreateOrOpen";

        /// <summary>Compound File API failure.</summary>
        public const string @CFAPIFailure = "CFAPIFailure";

        /// <summary>The given data space label name is already in use.</summary>
        public const string @DataSpaceLabelInUse = "DataSpaceLabelInUse";

        /// <summary>Empty string is not a valid data space label.</summary>
        public const string @DataSpaceLabelInvalidEmpty = "DataSpaceLabelInvalidEmpty";

        /// <summary>Specified data space label has not been defined.</summary>
        public const string @DataSpaceLabelUndefined = "DataSpaceLabelUndefined";

        /// <summary>DataSpaceManager object was disposed.</summary>
        public const string @DataSpaceManagerDisposed = "DataSpaceManagerDisposed";

        /// <summary>DataSpace map entry is not valid.</summary>
        public const string @DataSpaceMapEntryInvalid = "DataSpaceMapEntryInvalid";

        /// <summary>FileAccess value is not valid.</summary>
        public const string @FileAccessInvalid = "FileAccessInvalid";

        /// <summary>
        /// RoutedEvent/EventPrivateKey limit exceeded. Routed events or EventPrivateKey
        /// for CLR events are typically static class members registered with
        /// field initializers or static constructors. In this case, routed events
        /// or EventPrivateKeys might be getting initialized in instance
        /// constructors, causing the limit to be exceeded.
        /// </summary>
        public const string @TooManyRoutedEvents = "TooManyRoutedEvents";

        /// <summary>File already exists.</summary>
        public const string @FileAlreadyExists = "FileAlreadyExists";

        /// <summary>FileMode value is not supported.</summary>
        public const string @FileModeUnsupported = "FileModeUnsupported";

        /// <summary>FileMode value is not valid.</summary>
        public const string @FileModeInvalid = "FileModeInvalid";

        /// <summary>FileShare value is not supported.</summary>
        public const string @FileShareUnsupported = "FileShareUnsupported";

        /// <summary>FileShare value is not valid.</summary>
        public const string @FileShareInvalid = "FileShareInvalid";

        /// <summary>Streams for exposure as ILockBytes must be seekable.</summary>
        public const string @ILockBytesStreamMustSeek = "ILockBytesStreamMustSeek";

        /// <summary>'{1}' is not a valid value for '{0}'.</summary>
        public const string @InvalidArgumentValue = "InvalidArgumentValue";

        /// <summary>Cannot locate information for stream that should exist. This is an internally inconsistent condition.</summary>
        public const string @InvalidCondition01 = "InvalidCondition01";

        /// <summary>String format is not valid.</summary>
        public const string @InvalidStringFormat = "InvalidStringFormat";

        /// <summary>Internal table type value is not valid. This is an internally inconsistent condition.</summary>
        public const string @InvalidTableType = "InvalidTableType";

        /// <summary>MoveTo Destination storage does not exist.</summary>
        public const string @MoveToDestNotExist = "MoveToDestNotExist";

        /// <summary>IStorage/IStream::MoveTo not supported.</summary>
        public const string @MoveToNYI = "MoveToNYI";

        /// <summary>'{0}' name is already in use.</summary>
        public const string @NameAlreadyInUse = "NameAlreadyInUse";

        /// <summary>'{0}' cannot contain the path delimiter: '{1}'.</summary>
        public const string @NameCanNotHaveDelimiter = "NameCanNotHaveDelimiter";

        /// <summary>Failed call to '{0}'.</summary>
        public const string @NamedAPIFailure = "NamedAPIFailure";

        /// <summary>Name table data is corrupt in data storage.</summary>
        public const string @NameTableCorruptStg = "NameTableCorruptStg";

        /// <summary>Name table data is corrupt in memory.</summary>
        public const string @NameTableCorruptMem = "NameTableCorruptMem";

        /// <summary>Name table cannot be read by this version of the program.</summary>
        public const string @NameTableVersionMismatchRead = "NameTableVersionMismatchRead";

        /// <summary>Name table cannot be updated by this version of the program.</summary>
        public const string @NameTableVersionMismatchWrite = "NameTableVersionMismatchWrite";

        /// <summary>This feature is not supported.</summary>
        public const string @NYIDefault = "NYIDefault";

        /// <summary>Path string cannot include an empty element.</summary>
        public const string @PathHasEmptyElement = "PathHasEmptyElement";

        /// <summary>Count of bytes to read cannot be negative.</summary>
        public const string @ReadCountNegative = "ReadCountNegative";

        /// <summary>Cannot seek to given position.</summary>
        public const string @SeekFailed = "SeekFailed";

        /// <summary>Cannot set seek pointer to a negative position.</summary>
        public const string @SeekNegative = "SeekNegative";

        /// <summary>SeekOrigin value is not valid.</summary>
        public const string @SeekOriginInvalid = "SeekOriginInvalid";

        /// <summary>This combination of flags is not supported.</summary>
        public const string @StorageFlagsUnsupported = "StorageFlagsUnsupported";

        /// <summary>Storage already exists.</summary>
        public const string @StorageAlreadyExist = "StorageAlreadyExist";

        /// <summary>Stream already exists.</summary>
        public const string @StreamAlreadyExist = "StreamAlreadyExist";

        /// <summary>StorageInfo object was disposed.</summary>
        public const string @StorageInfoDisposed = "StorageInfoDisposed";

        /// <summary>Storage does not exist.</summary>
        public const string @StorageNotExist = "StorageNotExist";

        /// <summary>StorageRoot object was disposed.</summary>
        public const string @StorageRootDisposed = "StorageRootDisposed";

        /// <summary>StreamInfo object was disposed.</summary>
        public const string @StreamInfoDisposed = "StreamInfoDisposed";

        /// <summary>Stream length cannot be negative.</summary>
        public const string @StreamLengthNegative = "StreamLengthNegative";

        /// <summary>Cannot perform this function on a stream that does not exist.</summary>
        public const string @StreamNotExist = "StreamNotExist";

        /// <summary>Stream name cannot be '{0}'.</summary>
        public const string @StreamNameNotValid = "StreamNameNotValid";

        /// <summary>Stream time stamp not implemented in OLE32 implementation of Compound Files.</summary>
        public const string @StreamTimeStampNotImplemented = "StreamTimeStampNotImplemented";

        /// <summary>'{0}' cannot start with the reserved character range 0x01-0x1F.</summary>
        public const string @StringCanNotBeReservedName = "StringCanNotBeReservedName";

        /// <summary>Requested time stamp is not available.</summary>
        public const string @TimeStampNotAvailable = "TimeStampNotAvailable";

        /// <summary>Transform label name is already in use.</summary>
        public const string @TransformLabelInUse = "TransformLabelInUse";

        /// <summary>Data space transform stack includes undefined transform labels.</summary>
        public const string @TransformLabelUndefined = "TransformLabelUndefined";

        /// <summary>Transform object type is required to have a constructor which takes a TransformEnvironment object.</summary>
        public const string @TransformObjectConstructorParam = "TransformObjectConstructorParam";

        /// <summary>Transform object type is required to implement IDataTransform interface.</summary>
        public const string @TransformObjectImplementIDataTransform = "TransformObjectImplementIDataTransform";

        /// <summary>Stream transformation failed due to uninitialized data transform objects.</summary>
        public const string @TransformObjectInitFailed = "TransformObjectInitFailed";

        /// <summary>Transform identifier type is not supported.</summary>
        public const string @TransformTypeUnsupported = "TransformTypeUnsupported";

        /// <summary>Transform stack must have at least one transform.</summary>
        public const string @TransformStackValid = "TransformStackValid";

        /// <summary>Cannot create package on stream.</summary>
        public const string @UnableToCreateOnStream = "UnableToCreateOnStream";

        /// <summary>Cannot create data storage.</summary>
        public const string @UnableToCreateStorage = "UnableToCreateStorage";

        /// <summary>Cannot create data stream.</summary>
        public const string @UnableToCreateStream = "UnableToCreateStream";

        /// <summary>Cannot open data stream.</summary>
        public const string @UnableToOpenStream = "UnableToOpenStream";

        /// <summary>Encountered unsupported type of storage element when building storage enumerator.</summary>
        public const string @UnsupportedTypeEncounteredWhenBuildingStgEnum = "UnsupportedTypeEncounteredWhenBuildingStgEnum";

        /// <summary>Cannot write all data as specified.</summary>
        public const string @WriteFailure = "WriteFailure";

        /// <summary>Write-only mode is not supported.</summary>
        public const string @WriteOnlyUnsupported = "WriteOnlyUnsupported";

        /// <summary>Cannot write a negative number of bytes.</summary>
        public const string @WriteSizeNegative = "WriteSizeNegative";

        /// <summary>Object metadata stream in the package is corrupt and the content is not valid.</summary>
        public const string @CFM_CorruptMetadataStream = "CFM_CorruptMetadataStream";

        /// <summary>Object metadata stream in the package is corrupt and the root tag is not valid.</summary>
        public const string @CFM_CorruptMetadataStream_Root = "CFM_CorruptMetadataStream_Root";

        /// <summary>Object metadata stream in the package is corrupt with duplicated key tags.</summary>
        public const string @CFM_CorruptMetadataStream_DuplicateKey = "CFM_CorruptMetadataStream_DuplicateKey";

        /// <summary>Object used as metadata key must be an instance of the CompoundFileMetadataKey class.</summary>
        public const string @CFM_ObjectMustBeCompoundFileMetadataKey = "CFM_ObjectMustBeCompoundFileMetadataKey";

        /// <summary>Cannot perform this operation when the package is in read-only mode.</summary>
        public const string @CFM_ReadOnlyContainer = "CFM_ReadOnlyContainer";

        /// <summary>Failed to read a stream type table - the data appears to be a different format.</summary>
        public const string @CFM_TypeTableFormat = "CFM_TypeTableFormat";

        /// <summary>Unicode character is not valid.</summary>
        public const string @CFM_UnicodeCharInvalid = "CFM_UnicodeCharInvalid";

        /// <summary>Only strings can be used as value.</summary>
        public const string @CFM_ValueMustBeString = "CFM_ValueMustBeString";

        /// <summary>XML character is not valid.</summary>
        public const string @CFM_XMLCharInvalid = "CFM_XMLCharInvalid";

        /// <summary>Cannot compare different types.</summary>
        public const string @CanNotCompareDiffTypes = "CanNotCompareDiffTypes";

        /// <summary>CompoundFileReference: Corrupted CompoundFileReference.</summary>
        public const string @CFRCorrupt = "CFRCorrupt";

        /// <summary>CompoundFileReference: Corrupted CompoundFileReference - multiple stream components found.</summary>
        public const string @CFRCorruptMultiStream = "CFRCorruptMultiStream";

        /// <summary>CompoundFileReference: Corrupted CompoundFileReference - storage component cannot follow stream component.</summary>
        public const string @CFRCorruptStgFollowStm = "CFRCorruptStgFollowStm";

        /// <summary>Cannot have leading path delimiter.</summary>
        public const string @DelimiterLeading = "DelimiterLeading";

        /// <summary>Cannot have trailing path delimiter.</summary>
        public const string @DelimiterTrailing = "DelimiterTrailing";

        /// <summary>Offset must be greater than or equal to zero.</summary>
        public const string @OffsetNegative = "OffsetNegative";

        /// <summary>Unrecognized reference component type.</summary>
        public const string @UnknownReferenceComponentType = "UnknownReferenceComponentType";

        /// <summary>Cannot serialize unknown CompoundFileReference subclass.</summary>
        public const string @UnknownReferenceSerialize = "UnknownReferenceSerialize";

        /// <summary>CompoundFileReference: malformed path encountered.</summary>
        public const string @MalformedCompoundFilePath = "MalformedCompoundFilePath";

        /// <summary>Stream length cannot be negative.</summary>
        public const string @CannotMakeStreamLengthNegative = "CannotMakeStreamLengthNegative";

        /// <summary>Stream operation failed because stream is corrupted.</summary>
        public const string @CorruptStream = "CorruptStream";

        /// <summary>Stream does not support Length property.</summary>
        public const string @LengthNotSupported = "LengthNotSupported";

        /// <summary>Buffer too small to hold results of Read.</summary>
        public const string @ReadBufferTooSmall = "ReadBufferTooSmall";

        /// <summary>Stream does not support reading.</summary>
        public const string @ReadNotSupported = "ReadNotSupported";

        /// <summary>Stream does not support Seek.</summary>
        public const string @SeekNotSupported = "SeekNotSupported";

        /// <summary>Stream does not support SetLength.</summary>
        public const string @SetLengthNotSupported = "SetLengthNotSupported";

        /// <summary>Stream does not support setting the Position property.</summary>
        public const string @SetPositionNotSupported = "SetPositionNotSupported";

        /// <summary>Negative stream position not supported.</summary>
        public const string @StreamPositionNegative = "StreamPositionNegative";

        /// <summary>Cannot change Transform parameters after the transform is initialized.</summary>
        public const string @TransformParametersFixed = "TransformParametersFixed";

        /// <summary>Buffer of bytes to be written is too small.</summary>
        public const string @WriteBufferTooSmall = "WriteBufferTooSmall";

        /// <summary>Count of bytes to write cannot be negative.</summary>
        public const string @WriteCountNegative = "WriteCountNegative";

        /// <summary>Stream does not support writing.</summary>
        public const string @WriteNotSupported = "WriteNotSupported";

        /// <summary>Compression requires ZLib library version {0}.</summary>
        public const string @ZLibVersionError = "ZLibVersionError";

        /// <summary>Expected a VersionPair object.</summary>
        public const string @ExpectedVersionPairObject = "ExpectedVersionPairObject";

        /// <summary>Major and minor version number components cannot be negative.</summary>
        public const string @VersionNumberComponentNegative = "VersionNumberComponentNegative";

        /// <summary>Feature ID string cannot have zero length.</summary>
        public const string @ZeroLengthFeatureID = "ZeroLengthFeatureID";

        /// <summary>Cannot find version stream.</summary>
        public const string @VersionStreamMissing = "VersionStreamMissing";

        /// <summary>Cannot update version because of a version field size mismatch.</summary>
        public const string @VersionUpdateFailure = "VersionUpdateFailure";

        /// <summary>Cannot remove signature from read-only file.</summary>
        public const string @CannotRemoveSignatureFromReadOnlyFile = "CannotRemoveSignatureFromReadOnlyFile";

        /// <summary>Cannot change the RoutedEvent property while the RoutedEvent is being routed.</summary>
        public const string @RoutedEventCannotChangeWhileRouting = "RoutedEventCannotChangeWhileRouting";

        /// <summary>Cannot sign read-only file.</summary>
        public const string @CannotSignReadOnlyFile = "CannotSignReadOnlyFile";

        /// <summary>Cannot locate the selected digital certificate.</summary>
        public const string @DigSigCannotLocateCertificate = "DigSigCannotLocateCertificate";

        /// <summary>Certificate error. Multiple certificates found with the same thumbprint.</summary>
        public const string @DigSigDuplicateCertificate = "DigSigDuplicateCertificate";

        /// <summary>Digital Signature</summary>
        public const string @CertSelectionDialogTitle = "CertSelectionDialogTitle";

        /// <summary>Select a certificate</summary>
        public const string @CertSelectionDialogMessage = "CertSelectionDialogMessage";

        /// <summary>Duplicates not allowed - signature part already exists.</summary>
        public const string @DuplicateSignature = "DuplicateSignature";

        /// <summary>Error parsing XML Signature.</summary>
        public const string @XmlSignatureParseError = "XmlSignatureParseError";

        /// <summary>Required attribute '{0}' not found.</summary>
        public const string @RequiredXmlAttributeMissing = "RequiredXmlAttributeMissing";

        /// <summary>Unexpected tag '{0}'.</summary>
        public const string @UnexpectedXmlTag = "UnexpectedXmlTag";

        /// <summary>Required tag '{0}' not found.</summary>
        public const string @RequiredTagNotFound = "RequiredTagNotFound";

        /// <summary>Required Package-specific Object tag is missing.</summary>
        public const string @PackageSignatureObjectTagRequired = "PackageSignatureObjectTagRequired";

        /// <summary>Required Package-specific Reference tag is missing.</summary>
        public const string @PackageSignatureReferenceTagRequired = "PackageSignatureReferenceTagRequired";

        /// <summary>Expected exactly one Package-specific Reference tag.</summary>
        public const string @MoreThanOnePackageSpecificReference = "MoreThanOnePackageSpecificReference";

        /// <summary>Uri attribute in Reference tag must refer using fragment identifiers.</summary>
        public const string @InvalidUriAttribute = "InvalidUriAttribute";

        /// <summary>Cannot countersign an unsigned package.</summary>
        public const string @NoCounterSignUnsignedContainer = "NoCounterSignUnsignedContainer";

        /// <summary>Time format string is not valid.</summary>
        public const string @BadSignatureTimeFormatString = "BadSignatureTimeFormatString";

        /// <summary>Signature structures are corrupted in this package.</summary>
        public const string @PackageSignatureCorruption = "PackageSignatureCorruption";

        /// <summary>Unsupported hash algorithm specified.</summary>
        public const string @UnsupportedHashAlgorithm = "UnsupportedHashAlgorithm";

        /// <summary>Relationship transform must be followed by an XML canonicalization transform.</summary>
        public const string @RelationshipTransformNotFollowedByCanonicalizationTransform = "RelationshipTransformNotFollowedByCanonicalizationTransform";

        /// <summary>There must be at most one relationship transform specified for a given relationship part.</summary>
        public const string @MultipleRelationshipTransformsFound = "MultipleRelationshipTransformsFound";

        /// <summary>Unsupported transform algorithm specified.</summary>
        public const string @UnsupportedTransformAlgorithm = "UnsupportedTransformAlgorithm";

        /// <summary>Unsupported canonicalization method specified.</summary>
        public const string @UnsupportedCanonicalizationMethod = "UnsupportedCanonicalizationMethod";

        /// <summary>Reusable hash algorithm must be specified.</summary>
        public const string @HashAlgorithmMustBeReusable = "HashAlgorithmMustBeReusable";

        /// <summary>Malformed Part URI in Reference tag.</summary>
        public const string @PartReferenceUriMalformed = "PartReferenceUriMalformed";

        /// <summary>Relationship was found to the signature origin but the part is missing. Package signature structures are corrupted.</summary>
        public const string @SignatureOriginNotFound = "SignatureOriginNotFound";

        /// <summary>Multiple signature origin relationships found.</summary>
        public const string @MultipleSignatureOrigins = "MultipleSignatureOrigins";

        /// <summary>Must specify an item to sign.</summary>
        public const string @NothingToSign = "NothingToSign";

        /// <summary>Signature Identifier cannot be empty.</summary>
        public const string @EmptySignatureId = "EmptySignatureId";

        /// <summary>Signature was deleted.</summary>
        public const string @SignatureDeleted = "SignatureDeleted";

        /// <summary>Specified object ID conflicts with predefined Package Object ID.</summary>
        public const string @SignaturePackageObjectTagMustBeUnique = "SignaturePackageObjectTagMustBeUnique";

        /// <summary>Specified reference object conflicts with predefined Package specific reference.</summary>
        public const string @PackageSpecificReferenceTagMustBeUnique = "PackageSpecificReferenceTagMustBeUnique";

        /// <summary>Object identifiers must be unique within the same signature.</summary>
        public const string @SignatureObjectIdMustBeUnique = "SignatureObjectIdMustBeUnique";

        /// <summary>Can only countersign parts with Digital Signature ContentType.</summary>
        public const string @CanOnlyCounterSignSignatureParts = "CanOnlyCounterSignSignatureParts";

        /// <summary>Certificate part is not of the correct type.</summary>
        public const string @CertificatePartContentTypeMismatch = "CertificatePartContentTypeMismatch";

        /// <summary>Signing certificate must be of type DSA or RSA.</summary>
        public const string @CertificateKeyTypeNotSupported = "CertificateKeyTypeNotSupported";

        /// <summary>Specified part to sign does not exist.</summary>
        public const string @PartToSignMissing = "PartToSignMissing";

        /// <summary>Duplicate object ID found. IDs must be unique within the signature XML.</summary>
        public const string @DuplicateObjectId = "DuplicateObjectId";

        /// <summary>Caller-supplied parameter to callback function is not of expected type.</summary>
        public const string @CallbackParameterInvalid = "CallbackParameterInvalid";

        /// <summary>Cannot change publish license after the rights management transform settings are fixed.</summary>
        public const string @CannotChangePublishLicense = "CannotChangePublishLicense";

        /// <summary>Cannot change CryptoProvider after the rights management transform settings are fixed.</summary>
        public const string @CannotChangeCryptoProvider = "CannotChangeCryptoProvider";

        /// <summary>Length prefix specifies {0} characters, which exceeds the maximum of {1} characters.</summary>
        public const string @ExcessiveLengthPrefix = "ExcessiveLengthPrefix";

        /// <summary>OLE property ID {0} cannot be read (error {1}).</summary>
        public const string @GetOlePropertyFailed = "GetOlePropertyFailed";

        /// <summary>Authentication type string (the part before the colon) is not valid in user ID '{0}'.</summary>
        public const string @InvalidAuthenticationTypeString = "InvalidAuthenticationTypeString";

        /// <summary>'{0}' document property type is not valid.</summary>
        public const string @InvalidDocumentPropertyType = "InvalidDocumentPropertyType";

        /// <summary>'{0}' document property variant type is not valid.</summary>
        public const string @InvalidDocumentPropertyVariantType = "InvalidDocumentPropertyVariantType";

        /// <summary>User ID in use license stream is not of the form "authenticationType:userName".</summary>
        public const string @InvalidTypePrefixedUserName = "InvalidTypePrefixedUserName";

        /// <summary>Feature name in the transform's primary stream is '{0}', but expected '{1}'.</summary>
        public const string @InvalidTransformFeatureName = "InvalidTransformFeatureName";

        /// <summary>Document does not contain a package.</summary>
        public const string @PackageNotFound = "PackageNotFound";

        /// <summary>File does not contain a stream to hold the publish license.</summary>
        public const string @NoPublishLicenseStream = "NoPublishLicenseStream";

        /// <summary>File does not contain a storage to hold use licenses.</summary>
        public const string @NoUseLicenseStorage = "NoUseLicenseStorage";

        /// <summary>File contains data in format version {0}, but the software can only read that data in format version {1} or lower.</summary>
        public const string @ReaderVersionError = "ReaderVersionError";

        /// <summary>Document's publish license stream is corrupted.</summary>
        public const string @PublishLicenseStreamCorrupt = "PublishLicenseStreamCorrupt";

        /// <summary>Document does not contain a publish license.</summary>
        public const string @PublishLicenseNotFound = "PublishLicenseNotFound";

        /// <summary>Document does not contain any rights management-protected streams.</summary>
        public const string @RightsManagementEncryptionTransformNotFound = "RightsManagementEncryptionTransformNotFound";

        /// <summary>Document contains multiple Rights Management Encryption Transforms.</summary>
        public const string @MultipleRightsManagementEncryptionTransformFound = "MultipleRightsManagementEncryptionTransformFound";

        /// <summary>The stream on which the encrypted package is created must have read/write access.</summary>
        public const string @StreamNeedsReadWriteAccess = "StreamNeedsReadWriteAccess";

        /// <summary>Cannot perform stream operation because CryptoProvider is not set to allow decryption.</summary>
        public const string @CryptoProviderCanNotDecrypt = "CryptoProviderCanNotDecrypt";

        /// <summary>Only cryptographic providers based on a block cipher are supported.</summary>
        public const string @CryptoProviderCanNotMergeBlocks = "CryptoProviderCanNotMergeBlocks";

        /// <summary>EncryptedPackageEnvelope object was disposed.</summary>
        public const string @EncryptedPackageEnvelopeDisposed = "EncryptedPackageEnvelopeDisposed";

        /// <summary>CryptoProvider object was disposed.</summary>
        public const string @CryptoProviderDisposed = "CryptoProviderDisposed";

        /// <summary>File contains data in format version {0}, but the software can only update that data in format version {1} or lower.</summary>
        public const string @UpdaterVersionError = "UpdaterVersionError";

        /// <summary>The dictionary is read-only.</summary>
        public const string @DictionaryIsReadOnly = "DictionaryIsReadOnly";

        /// <summary>The CryptoProvider cannot encrypt or decrypt.</summary>
        public const string @CryptoProviderIsNotReady = "CryptoProviderIsNotReady";

        /// <summary>One of the document's use licenses is corrupted.</summary>
        public const string @UseLicenseStreamCorrupt = "UseLicenseStreamCorrupt";

        /// <summary>Encrypted data stream is corrupted.</summary>
        public const string @EncryptedDataStreamCorrupt = "EncryptedDataStreamCorrupt";

        /// <summary>Unrecognized document property: FMTID = '{0}', property ID = '{1}'.</summary>
        public const string @UnknownDocumentProperty = "UnknownDocumentProperty";

        /// <summary>'{0}' document property in property set '{1}' is of incorrect variant type '{2}'. Expected type '{3}'.</summary>
        public const string @WrongDocumentPropertyVariantType = "WrongDocumentPropertyVariantType";

        /// <summary>User is not activated.</summary>
        public const string @UserIsNotActivated = "UserIsNotActivated";

        /// <summary>User does not have a client licensor certificate.</summary>
        public const string @UserHasNoClientLicensorCert = "UserHasNoClientLicensorCert";

        /// <summary>Encryption right is not granted.</summary>
        public const string @EncryptionRightIsNotGranted = "EncryptionRightIsNotGranted";

        /// <summary>Decryption right is not granted.</summary>
        public const string @DecryptionRightIsNotGranted = "DecryptionRightIsNotGranted";

        /// <summary>CryptoProvider does not have privileges required for decryption of the PublishLicense.</summary>
        public const string @NoPrivilegesForPublishLicenseDecryption = "NoPrivilegesForPublishLicenseDecryption";

        /// <summary>Signed Publish License is not valid.</summary>
        public const string @InvalidPublishLicense = "InvalidPublishLicense";

        /// <summary>Variable-length header in publish license stream is {0} bytes, which exceeds the maximum length of {1} bytes.</summary>
        public const string @PublishLicenseStreamHeaderTooLong = "PublishLicenseStreamHeaderTooLong";

        /// <summary>User must be either Windows or Passport authenticated. Other authentication types are not allowed in this context.</summary>
        public const string @OnlyPassportOrWindowsAuthenticatedUsersAreAllowed = "OnlyPassportOrWindowsAuthenticatedUsersAreAllowed";

        /// <summary>Rights management operation failed.</summary>
        public const string @RmExceptionGenericMessage = "RmExceptionGenericMessage";

        /// <summary>License is not valid.</summary>
        public const string @RmExceptionInvalidLicense = "RmExceptionInvalidLicense";

        /// <summary>Information not found.</summary>
        public const string @RmExceptionInfoNotInLicense = "RmExceptionInfoNotInLicense";

        /// <summary>License signature is not valid.</summary>
        public const string @RmExceptionInvalidLicenseSignature = "RmExceptionInvalidLicenseSignature";

        /// <summary>Encryption not permitted.</summary>
        public const string @RmExceptionEncryptionNotPermitted = "RmExceptionEncryptionNotPermitted";

        /// <summary>Right not granted.</summary>
        public const string @RmExceptionRightNotGranted = "RmExceptionRightNotGranted";

        /// <summary>Version is not valid.</summary>
        public const string @RmExceptionInvalidVersion = "RmExceptionInvalidVersion";

        /// <summary>Encoding type is not valid.</summary>
        public const string @RmExceptionInvalidEncodingType = "RmExceptionInvalidEncodingType";

        /// <summary>Numerical value is not valid.</summary>
        public const string @RmExceptionInvalidNumericalValue = "RmExceptionInvalidNumericalValue";

        /// <summary>Algorithm type is not valid.</summary>
        public const string @RmExceptionInvalidAlgorithmType = "RmExceptionInvalidAlgorithmType";

        /// <summary>Environment not loaded.</summary>
        public const string @RmExceptionEnvironmentNotLoaded = "RmExceptionEnvironmentNotLoaded";

        /// <summary>Cannot load environment.</summary>
        public const string @RmExceptionEnvironmentCannotLoad = "RmExceptionEnvironmentCannotLoad";

        /// <summary>Cannot load more than one environment.</summary>
        public const string @RmExceptionTooManyLoadedEnvironments = "RmExceptionTooManyLoadedEnvironments";

        /// <summary>Incompatible objects.</summary>
        public const string @RmExceptionIncompatibleObjects = "RmExceptionIncompatibleObjects";

        /// <summary>Library fail.</summary>
        public const string @RmExceptionLibraryFail = "RmExceptionLibraryFail";

        /// <summary>Enabling principal failure.</summary>
        public const string @RmExceptionEnablingPrincipalFailure = "RmExceptionEnablingPrincipalFailure";

        /// <summary>Information not found.</summary>
        public const string @RmExceptionInfoNotPresent = "RmExceptionInfoNotPresent";

        /// <summary>Get information query is not valid.</summary>
        public const string @RmExceptionBadGetInfoQuery = "RmExceptionBadGetInfoQuery";

        /// <summary>Key type not supported.</summary>
        public const string @RmExceptionKeyTypeUnsupported = "RmExceptionKeyTypeUnsupported";

        /// <summary>Crypto operation not supported.</summary>
        public const string @RmExceptionCryptoOperationUnsupported = "RmExceptionCryptoOperationUnsupported";

        /// <summary>Clock rollback detected.</summary>
        public const string @RmExceptionClockRollbackDetected = "RmExceptionClockRollbackDetected";

        /// <summary>Query reports no results.</summary>
        public const string @RmExceptionQueryReportsNoResults = "RmExceptionQueryReportsNoResults";

        /// <summary>Unexpected exception.</summary>
        public const string @RmExceptionUnexpectedException = "RmExceptionUnexpectedException";

        /// <summary>Binding validity time violated.</summary>
        public const string @RmExceptionBindValidityTimeViolated = "RmExceptionBindValidityTimeViolated";

        /// <summary>Broken certificate chain.</summary>
        public const string @RmExceptionBrokenCertChain = "RmExceptionBrokenCertChain";

        /// <summary>Binding policy violation.</summary>
        public const string @RmExceptionBindPolicyViolation = "RmExceptionBindPolicyViolation";

        /// <summary>Manifest policy violation.</summary>
        public const string @RmExceptionManifestPolicyViolation = "RmExceptionManifestPolicyViolation";

        /// <summary>License has been revoked.</summary>
        public const string @RmExceptionBindRevokedLicense = "RmExceptionBindRevokedLicense";

        /// <summary>Issuer has been revoked.</summary>
        public const string @RmExceptionBindRevokedIssuer = "RmExceptionBindRevokedIssuer";

        /// <summary>Principal has been revoked.</summary>
        public const string @RmExceptionBindRevokedPrincipal = "RmExceptionBindRevokedPrincipal";

        /// <summary>Resource has been revoked.</summary>
        public const string @RmExceptionBindRevokedResource = "RmExceptionBindRevokedResource";

        /// <summary>Module has been revoked.</summary>
        public const string @RmExceptionBindRevokedModule = "RmExceptionBindRevokedModule";

        /// <summary>Binding content not in the End Use License.</summary>
        public const string @RmExceptionBindContentNotInEndUseLicense = "RmExceptionBindContentNotInEndUseLicense";

        /// <summary>Binding access principal is not enabling.</summary>
        public const string @RmExceptionBindAccessPrincipalNotEnabling = "RmExceptionBindAccessPrincipalNotEnabling";

        /// <summary>Binding access unsatisfied.</summary>
        public const string @RmExceptionBindAccessUnsatisfied = "RmExceptionBindAccessUnsatisfied";

        /// <summary>Principal provided for binding is missing.</summary>
        public const string @RmExceptionBindIndicatedPrincipalMissing = "RmExceptionBindIndicatedPrincipalMissing";

        /// <summary>Machine is not found in group identity certificate.</summary>
        public const string @RmExceptionBindMachineNotFoundInGroupIdentity = "RmExceptionBindMachineNotFoundInGroupIdentity";

        /// <summary>Unsupported library plug-in.</summary>
        public const string @RmExceptionLibraryUnsupportedPlugIn = "RmExceptionLibraryUnsupportedPlugIn";

        /// <summary>Binding revocation list is stale.</summary>
        public const string @RmExceptionBindRevocationListStale = "RmExceptionBindRevocationListStale";

        /// <summary>Binding missing application revocation list.</summary>
        public const string @RmExceptionBindNoApplicableRevocationList = "RmExceptionBindNoApplicableRevocationList";

        /// <summary>Handle is not valid.</summary>
        public const string @RmExceptionInvalidHandle = "RmExceptionInvalidHandle";

        /// <summary>Binding time interval is violated.</summary>
        public const string @RmExceptionBindIntervalTimeViolated = "RmExceptionBindIntervalTimeViolated";

        /// <summary>Binding cannot find a satisfied rights group.</summary>
        public const string @RmExceptionBindNoSatisfiedRightsGroup = "RmExceptionBindNoSatisfiedRightsGroup";

        /// <summary>Cannot find content specified for binding.</summary>
        public const string @RmExceptionBindSpecifiedWorkMissing = "RmExceptionBindSpecifiedWorkMissing";

        /// <summary>No more data.</summary>
        public const string @RmExceptionNoMoreData = "RmExceptionNoMoreData";

        /// <summary>License acquisition failed.</summary>
        public const string @RmExceptionLicenseAcquisitionFailed = "RmExceptionLicenseAcquisitionFailed";

        /// <summary>ID mismatch.</summary>
        public const string @RmExceptionIdMismatch = "RmExceptionIdMismatch";

        /// <summary>Cannot have more than one certificate.</summary>
        public const string @RmExceptionTooManyCertificates = "RmExceptionTooManyCertificates";

        /// <summary>Distribution Point URL was not set.</summary>
        public const string @RmExceptionNoDistributionPointUrlFound = "RmExceptionNoDistributionPointUrlFound";

        /// <summary>Rights management server transaction already in progress.</summary>
        public const string @RmExceptionAlreadyInProgress = "RmExceptionAlreadyInProgress";

        /// <summary>Group identity not set.</summary>
        public const string @RmExceptionGroupIdentityNotSet = "RmExceptionGroupIdentityNotSet";

        /// <summary>Record not found.</summary>
        public const string @RmExceptionRecordNotFound = "RmExceptionRecordNotFound";

        /// <summary>Connection failed.</summary>
        public const string @RmExceptionNoConnect = "RmExceptionNoConnect";

        /// <summary>License not found.</summary>
        public const string @RmExceptionNoLicense = "RmExceptionNoLicense";

        /// <summary>Machine must be activated.</summary>
        public const string @RmExceptionNeedsMachineActivation = "RmExceptionNeedsMachineActivation";

        /// <summary>User identity must be activated.</summary>
        public const string @RmExceptionNeedsGroupIdentityActivation = "RmExceptionNeedsGroupIdentityActivation";

        /// <summary>Activation failed.</summary>
        public const string @RmExceptionActivationFailed = "RmExceptionActivationFailed";

        /// <summary>Command interrupted.</summary>
        public const string @RmExceptionAborted = "RmExceptionAborted";

        /// <summary>Transaction quota exceeded.</summary>
        public const string @RmExceptionOutOfQuota = "RmExceptionOutOfQuota";

        /// <summary>Authentication failed.</summary>
        public const string @RmExceptionAuthenticationFailed = "RmExceptionAuthenticationFailed";

        /// <summary>Server side error.</summary>
        public const string @RmExceptionServerError = "RmExceptionServerError";

        /// <summary>Installation failed.</summary>
        public const string @RmExceptionInstallationFailed = "RmExceptionInstallationFailed";

        /// <summary>Hardware ID corrupted.</summary>
        public const string @RmExceptionHidCorrupted = "RmExceptionHidCorrupted";

        /// <summary>Server response is not valid.</summary>
        public const string @RmExceptionInvalidServerResponse = "RmExceptionInvalidServerResponse";

        /// <summary>Service not found.</summary>
        public const string @RmExceptionServiceNotFound = "RmExceptionServiceNotFound";

        /// <summary>Use default.</summary>
        public const string @RmExceptionUseDefault = "RmExceptionUseDefault";

        /// <summary>Server not found.</summary>
        public const string @RmExceptionServerNotFound = "RmExceptionServerNotFound";

        /// <summary>E-mail address is not valid.</summary>
        public const string @RmExceptionInvalidEmail = "RmExceptionInvalidEmail";

        /// <summary>License validity time violation.</summary>
        public const string @RmExceptionValidityTimeViolation = "RmExceptionValidityTimeViolation";

        /// <summary>Outdated module.</summary>
        public const string @RmExceptionOutdatedModule = "RmExceptionOutdatedModule";

        /// <summary>Service moved.</summary>
        public const string @RmExceptionServiceMoved = "RmExceptionServiceMoved";

        /// <summary>Service gone.</summary>
        public const string @RmExceptionServiceGone = "RmExceptionServiceGone";

        /// <summary>Ad entry not found.</summary>
        public const string @RmExceptionAdEntryNotFound = "RmExceptionAdEntryNotFound";

        /// <summary>Not a certificate chain.</summary>
        public const string @RmExceptionNotAChain = "RmExceptionNotAChain";

        /// <summary>Rights management server denied request.</summary>
        public const string @RmExceptionRequestDenied = "RmExceptionRequestDenied";

        /// <summary>Not set.</summary>
        public const string @RmExceptionNotSet = "RmExceptionNotSet";

        /// <summary>Metadata not set.</summary>
        public const string @RmExceptionMetadataNotSet = "RmExceptionMetadataNotSet";

        /// <summary>Revocation information not set.</summary>
        public const string @RmExceptionRevocationInfoNotSet = "RmExceptionRevocationInfoNotSet";

        /// <summary>Time information is not valid.</summary>
        public const string @RmExceptionInvalidTimeInfo = "RmExceptionInvalidTimeInfo";

        /// <summary>Right not set.</summary>
        public const string @RmExceptionRightNotSet = "RmExceptionRightNotSet";

        /// <summary>License binding to Windows Identity failed (NTLM bind failure).</summary>
        public const string @RmExceptionLicenseBindingToWindowsIdentityFailed = "RmExceptionLicenseBindingToWindowsIdentityFailed";

        /// <summary>Issuance license template is not valid because of incorrectly formatted string.</summary>
        public const string @RmExceptionInvalidIssuanceLicenseTemplate = "RmExceptionInvalidIssuanceLicenseTemplate";

        /// <summary>Key size length is not valid.</summary>
        public const string @RmExceptionInvalidKeyLength = "RmExceptionInvalidKeyLength";

        /// <summary>Expired official Publish License template.</summary>
        public const string @RmExceptionExpiredOfficialIssuanceLicenseTemplate = "RmExceptionExpiredOfficialIssuanceLicenseTemplate";

        /// <summary>Client Licensor Certificate is not valid.</summary>
        public const string @RmExceptionInvalidClientLicensorCertificate = "RmExceptionInvalidClientLicensorCertificate";

        /// <summary>Hardware ID is not valid.</summary>
        public const string @RmExceptionHidInvalid = "RmExceptionHidInvalid";

        /// <summary>E-mail not verified.</summary>
        public const string @RmExceptionEmailNotVerified = "RmExceptionEmailNotVerified";

        /// <summary>Debugger detected.</summary>
        public const string @RmExceptionDebuggerDetected = "RmExceptionDebuggerDetected";

        /// <summary>Lockbox type is not valid.</summary>
        public const string @RmExceptionInvalidLockboxType = "RmExceptionInvalidLockboxType";

        /// <summary>Lockbox path is not valid.</summary>
        public const string @RmExceptionInvalidLockboxPath = "RmExceptionInvalidLockboxPath";

        /// <summary>Registry path is not valid.</summary>
        public const string @RmExceptionInvalidRegistryPath = "RmExceptionInvalidRegistryPath";

        /// <summary>No AES Crypto provider found.</summary>
        public const string @RmExceptionNoAesCryptoProvider = "RmExceptionNoAesCryptoProvider";

        /// <summary>Global option is already set.</summary>
        public const string @RmExceptionGlobalOptionAlreadySet = "RmExceptionGlobalOptionAlreadySet";

        /// <summary>Owner's license not found.</summary>
        public const string @RmExceptionOwnerLicenseNotFound = "RmExceptionOwnerLicenseNotFound";

        /// <summary>Archive file cannot be size 0.</summary>
        public const string @ZipZeroSizeFileIsNotValidArchive = "ZipZeroSizeFileIsNotValidArchive";

        /// <summary>Cannot perform a write operation in read-only mode.</summary>
        public const string @CanNotWriteInReadOnlyMode = "CanNotWriteInReadOnlyMode";

        /// <summary>Cannot perform a read operation in write-only mode.</summary>
        public const string @CanNotReadInWriteOnlyMode = "CanNotReadInWriteOnlyMode";

        /// <summary>Cannot perform a read/write operation in write-only or read-only modes.</summary>
        public const string @CanNotReadWriteInReadOnlyWriteOnlyMode = "CanNotReadWriteInReadOnlyWriteOnlyMode";

        /// <summary>Cannot create file because the specified file name is already in use.</summary>
        public const string @AttemptedToCreateDuplicateFileName = "AttemptedToCreateDuplicateFileName";

        /// <summary>Cannot find specified file.</summary>
        public const string @FileDoesNotExists = "FileDoesNotExists";

        /// <summary>Truncate and Append FileModes are not supported.</summary>
        public const string @TruncateAppendModesNotSupported = "TruncateAppendModesNotSupported";

        /// <summary>Only FileShare.Read and FileShare.None are supported.</summary>
        public const string @OnlyFileShareReadAndFileShareNoneSupported = "OnlyFileShareReadAndFileShareNoneSupported";

        /// <summary>Cannot read data from stream that does not support reading.</summary>
        public const string @CanNotReadDataFromStreamWhichDoesNotSupportReading = "CanNotReadDataFromStreamWhichDoesNotSupportReading";

        /// <summary>Cannot write data to stream that does not support writing.</summary>
        public const string @CanNotWriteDataToStreamWhichDoesNotSupportWriting = "CanNotWriteDataToStreamWhichDoesNotSupportWriting";

        /// <summary>Cannot operate on stream that does not support seeking.</summary>
        public const string @CanNotOperateOnStreamWhichDoesNotSupportSeeking = "CanNotOperateOnStreamWhichDoesNotSupportSeeking";

        /// <summary>Cannot get stream with FileMode.Create, FileMode.CreateNew, FileMode.Truncate, FileMode.Append when access is FileAccess.Read.</summary>
        public const string @UnsupportedCombinationOfModeAccessShareStreaming = "UnsupportedCombinationOfModeAccessShareStreaming";

        /// <summary>File contains corrupted data.</summary>
        public const string @CorruptedData = "CorruptedData";

        /// <summary>Multidisk ZIP format is not supported.</summary>
        public const string @NotSupportedMultiDisk = "NotSupportedMultiDisk";

        /// <summary>ZIP archive was closed and disposed.</summary>
        public const string @ZipArchiveDisposed = "ZipArchiveDisposed";

        /// <summary>ZIP file was closed, disposed, or deleted.</summary>
        public const string @ZipFileItemDisposed = "ZipFileItemDisposed";

        /// <summary>ZIP archive contains unsupported data structures.</summary>
        public const string @NotSupportedVersionNeededToExtract = "NotSupportedVersionNeededToExtract";

        /// <summary>ZIP archive contains data structures too large to fit in memory.</summary>
        public const string @Zip64StructuresTooLarge = "Zip64StructuresTooLarge";

        /// <summary>ZIP archive contains unsupported encrypted data.</summary>
        public const string @ZipNotSupportedEncryptedArchive = "ZipNotSupportedEncryptedArchive";

        /// <summary>ZIP archive contains unsupported signature data.</summary>
        public const string @ZipNotSupportedSignedArchive = "ZipNotSupportedSignedArchive";

        /// <summary>ZIP archive contains data compressed using an unsupported algorithm.</summary>
        public const string @ZipNotSupportedCompressionMethod = "ZipNotSupportedCompressionMethod";

        /// <summary>Compressed part has inconsistent data length.</summary>
        public const string @CompressLengthMismatch = "CompressLengthMismatch";

        /// <summary>CreateNew is not a valid FileMode for a nonempty stream.</summary>
        public const string @CreateNewOnNonEmptyStream = "CreateNewOnNonEmptyStream";

        /// <summary>Specified part does not exist in the package.</summary>
        public const string @PartDoesNotExist = "PartDoesNotExist";

        /// <summary>Cannot add part for the specified URI because it is already in the package.</summary>
        public const string @PartAlreadyExists = "PartAlreadyExists";

        /// <summary>Cannot add part to the package. Part names cannot be derived from another part name by appending segments to it.</summary>
        public const string @PartNamePrefixExists = "PartNamePrefixExists";

        /// <summary>Cannot open package because FileMode or FileAccess value is not valid for the stream.</summary>
        public const string @IncompatibleModeOrAccess = "IncompatibleModeOrAccess";

        /// <summary>Cannot be an absolute URI.</summary>
        public const string @URIShouldNotBeAbsolute = "URIShouldNotBeAbsolute";

        /// <summary>Must have absolute URI.</summary>
        public const string @UriShouldBeAbsolute = "UriShouldBeAbsolute";

        /// <summary>FileMode/FileAccess for Part.GetStream is not compatible with FileMode/FileAccess used to open the Package.</summary>
        public const string @ContainerAndPartModeIncompatible = "ContainerAndPartModeIncompatible";

        /// <summary>Cannot get stream with FileMode.Create, FileMode.CreateNew, FileMode.Truncate, FileMode.Append when access is FileAccess.Read.</summary>
        public const string @UnsupportedCombinationOfModeAccess = "UnsupportedCombinationOfModeAccess";

        /// <summary>Returned stream for the part is null.</summary>
        public const string @NullStreamReturned = "NullStreamReturned";

        /// <summary>Package object was closed and disposed, so cannot carry out operations on this object or any stream opened on a part of this package.</summary>
        public const string @ObjectDisposed = "ObjectDisposed";

        /// <summary>Cannot write to read-only stream.</summary>
        public const string @ReadOnlyStream = "ReadOnlyStream";

        /// <summary>Cannot read from write-only stream.</summary>
        public const string @WriteOnlyStream = "WriteOnlyStream";

        /// <summary>Cannot access part because parent package was closed.</summary>
        public const string @ParentContainerClosed = "ParentContainerClosed";

        /// <summary>Part was deleted.</summary>
        public const string @PackagePartDeleted = "PackagePartDeleted";

        /// <summary>PackageRelationship cannot target another PackageRelationship.</summary>
        public const string @RelationshipToRelationshipIllegal = "RelationshipToRelationshipIllegal";

        /// <summary>PackageRelationship parts cannot have relationships to other parts.</summary>
        public const string @RelationshipPartsCannotHaveRelationships = "RelationshipPartsCannotHaveRelationships";

        /// <summary>Incorrect content type for PackageRelationship part.</summary>
        public const string @RelationshipPartIncorrectContentType = "RelationshipPartIncorrectContentType";

        /// <summary>PackageRelationship with specified ID does not exist at the Package level.</summary>
        public const string @PackageRelationshipDoesNotExist = "PackageRelationshipDoesNotExist";

        /// <summary>PackageRelationship with specified ID does not exist for the source part.</summary>
        public const string @PackagePartRelationshipDoesNotExist = "PackagePartRelationshipDoesNotExist";

        /// <summary>PackageRelationship target must be relative URI if TargetMode is Internal.</summary>
        public const string @RelationshipTargetMustBeRelative = "RelationshipTargetMustBeRelative";

        /// <summary>Relationship tag requires attribute '{0}'.</summary>
        public const string @RequiredRelationshipAttributeMissing = "RequiredRelationshipAttributeMissing";

        /// <summary>Relationship tag contains incorrect attribute.</summary>
        public const string @RelationshipTagDoesntMatchSchema = "RelationshipTagDoesntMatchSchema";

        /// <summary>Relationships tag has extra attributes.</summary>
        public const string @RelationshipsTagHasExtraAttributes = "RelationshipsTagHasExtraAttributes";

        /// <summary>Unrecognized tag found in Relationships XML.</summary>
        public const string @UnknownTagEncountered = "UnknownTagEncountered";

        /// <summary>Relationships tag expected at root level.</summary>
        public const string @ExpectedRelationshipsElementTag = "ExpectedRelationshipsElementTag";

        /// <summary>Relationships XML elements cannot specify attribute '{0}'.</summary>
        public const string @InvalidXmlBaseAttributePresent = "InvalidXmlBaseAttributePresent";

        /// <summary>'{0}' ID conflicts with the ID of an existing relationship for the specified source.</summary>
        public const string @NotAUniqueRelationshipId = "NotAUniqueRelationshipId";

        /// <summary>'{0}' ID is not a valid XSD ID.</summary>
        public const string @NotAValidXmlIdString = "NotAValidXmlIdString";

        /// <summary>'{0}' attribute value is not valid.</summary>
        public const string @InvalidValueForTheAttribute = "InvalidValueForTheAttribute";

        /// <summary>Relationship Type cannot contain only spaces or be empty.</summary>
        public const string @InvalidRelationshipType = "InvalidRelationshipType";

        /// <summary>Part URI must start with a forward slash.</summary>
        public const string @PartUriShouldStartWithForwardSlash = "PartUriShouldStartWithForwardSlash";

        /// <summary>Part URI cannot end with a forward slash.</summary>
        public const string @PartUriShouldNotEndWithForwardSlash = "PartUriShouldNotEndWithForwardSlash";

        /// <summary>URI must contain pack:// scheme.</summary>
        public const string @UriShouldBePackScheme = "UriShouldBePackScheme";

        /// <summary>Part URI is empty.</summary>
        public const string @PartUriIsEmpty = "PartUriIsEmpty";

        /// <summary>Part URI is not valid per rules defined in the Open Packaging Conventions specification.</summary>
        public const string @InvalidPartUri = "InvalidPartUri";

        /// <summary>PackageRelationship part URI is not expected.</summary>
        public const string @RelationshipPartUriNotExpected = "RelationshipPartUriNotExpected";

        /// <summary>PackageRelationship part URI is expected.</summary>
        public const string @RelationshipPartUriExpected = "RelationshipPartUriExpected";

        /// <summary>PackageRelationship part URI syntax is not valid.</summary>
        public const string @NotAValidRelationshipPartUri = "NotAValidRelationshipPartUri";

        /// <summary>The 'fragment' parameter must start with a number sign.</summary>
        public const string @FragmentMustStartWithHash = "FragmentMustStartWithHash";

        /// <summary>Part URI cannot contain a Fragment component.</summary>
        public const string @PartUriCannotHaveAFragment = "PartUriCannotHaveAFragment";

        /// <summary>Part URI cannot start with two forward slashes.</summary>
        public const string @PartUriShouldNotStartWithTwoForwardSlashes = "PartUriShouldNotStartWithTwoForwardSlashes";

        /// <summary>Package URI obtained from the pack URI cannot contain a Fragment.</summary>
        public const string @InnerPackageUriHasFragment = "InnerPackageUriHasFragment";

        /// <summary>Cannot access Stream object because it was closed or disposed.</summary>
        public const string @StreamObjectDisposed = "StreamObjectDisposed";

        /// <summary>GetContentTypeCore method cannot return null for the content type stream.</summary>
        public const string @NullContentTypeProvided = "NullContentTypeProvided";

        /// <summary>PackagePart subclass must implement GetContentTypeCore method if passing a null value for the content type when PackagePart object is constructed.</summary>
        public const string @GetContentTypeCoreNotImplemented = "GetContentTypeCoreNotImplemented";

        /// <summary>'{0}' tag requires attribute '{1}'.</summary>
        public const string @RequiredAttributeMissing = "RequiredAttributeMissing";

        /// <summary>'{0}' tag requires a nonempty '{1}' attribute.</summary>
        public const string @RequiredAttributeEmpty = "RequiredAttributeEmpty";

        /// <summary>Types tag has attributes not valid per the schema.</summary>
        public const string @TypesTagHasExtraAttributes = "TypesTagHasExtraAttributes";

        /// <summary>Required Types tag not found.</summary>
        public const string @TypesElementExpected = "TypesElementExpected";

        /// <summary>Content Types XML does not match schema.</summary>
        public const string @TypesXmlDoesNotMatchSchema = "TypesXmlDoesNotMatchSchema";

        /// <summary>Default tag is not valid per the schema. Verify that attributes are correct.</summary>
        public const string @DefaultTagDoesNotMatchSchema = "DefaultTagDoesNotMatchSchema";

        /// <summary>Override tag is not valid per the schema. Verify that attributes are correct.</summary>
        public const string @OverrideTagDoesNotMatchSchema = "OverrideTagDoesNotMatchSchema";

        /// <summary>'{0}' element must be empty.</summary>
        public const string @ElementIsNotEmptyElement = "ElementIsNotEmptyElement";

        /// <summary>Format error in package.</summary>
        public const string @BadPackageFormat = "BadPackageFormat";

        /// <summary>Streaming mode is supported only for creating packages.</summary>
        public const string @StreamingModeNotSupportedForConsumption = "StreamingModeNotSupportedForConsumption";

        /// <summary>Must have write-only access to produce a package in streaming mode.</summary>
        public const string @StreamingPackageProductionImpliesWriteOnlyAccess = "StreamingPackageProductionImpliesWriteOnlyAccess";

        /// <summary>Cannot have concurrent write accesses on package being produced in streaming mode.</summary>
        public const string @StreamingPackageProductionRequiresSingleWriter = "StreamingPackageProductionRequiresSingleWriter";

        /// <summary>'{0}' method can only be called on a package opened in streaming mode.</summary>
        public const string @MethodAvailableOnlyInStreamingCreation = "MethodAvailableOnlyInStreamingCreation";

        /// <summary>Package.{0} is not supported in streaming production.</summary>
        public const string @OperationIsNotSupportedInStreamingProduction = "OperationIsNotSupportedInStreamingProduction";

        /// <summary>Only write operations are supported in streaming production.</summary>
        public const string @OnlyWriteOperationsAreSupportedInStreamingCreation = "OnlyWriteOperationsAreSupportedInStreamingCreation";

        /// <summary>Write-once semantics in streaming production precludes the use of '{0}'.</summary>
        public const string @OperationViolatesWriteOnceSemantics = "OperationViolatesWriteOnceSemantics";

        /// <summary>Streaming consumption of packages not supported.</summary>
        public const string @OnlyStreamingProductionIsSupported = "OnlyStreamingProductionIsSupported";

        /// <summary>Read or write operation references location outside the bounds of the buffer provided.</summary>
        public const string @IOBufferOverflow = "IOBufferOverflow";

        /// <summary>Cannot change content of a read-only stream.</summary>
        public const string @StreamDoesNotSupportWrite = "StreamDoesNotSupportWrite";

        /// <summary>Package has more than one Core Properties relationship.</summary>
        public const string @MoreThanOneMetadataRelationships = "MoreThanOneMetadataRelationships";

        /// <summary>TargetMode for a Core Properties relationship must be 'Internal'.</summary>
        public const string @NoExternalTargetForMetadataRelationship = "NoExternalTargetForMetadataRelationship";

        /// <summary>Unrecognized root element in Core Properties part.</summary>
        public const string @CorePropertiesElementExpected = "CorePropertiesElementExpected";

        /// <summary>Core Properties part: core property elements can contain only text data.</summary>
        public const string @NoStructuredContentInsideProperties = "NoStructuredContentInsideProperties";

        /// <summary>Unrecognized namespace in Core Properties part.</summary>
        public const string @UnknownNamespaceInCorePropertiesPart = "UnknownNamespaceInCorePropertiesPart";

        /// <summary>'{0}' property name is not valid in Core Properties part.</summary>
        public const string @InvalidPropertyNameInCorePropertiesPart = "InvalidPropertyNameInCorePropertiesPart";

        /// <summary>Core Properties part: A property start-tag was expected.</summary>
        public const string @PropertyStartTagExpected = "PropertyStartTagExpected";

        /// <summary>Core Properties part: Text data of XSD type 'DateTime' was expected.</summary>
        public const string @XsdDateTimeExpected = "XsdDateTimeExpected";

        /// <summary>The target of the Core Properties relationship does not reference an existing part.</summary>
        public const string @DanglingMetadataRelationship = "DanglingMetadataRelationship";

        /// <summary>The Core Properties relationship references a part that has an incorrect content type.</summary>
        public const string @WrongContentTypeForPropertyPart = "WrongContentTypeForPropertyPart";

        /// <summary>Unexpected number of attributes is found on '{0}'.</summary>
        public const string @PropertyWrongNumbOfAttribsDefinedOn = "PropertyWrongNumbOfAttribsDefinedOn";

        /// <summary>Unknown xsi:type for DateTime on '{0}'.</summary>
        public const string @UnknownDCDateTimeXsiType = "UnknownDCDateTimeXsiType";

        /// <summary>More than one '{0}' property found.</summary>
        public const string @DuplicateCorePropertyName = "DuplicateCorePropertyName";

        /// <summary>PackageProperties object was disposed.</summary>
        public const string @StorageBasedPackagePropertiesDiposed = "StorageBasedPackagePropertiesDiposed";

        /// <summary>Encoding format is not supported. Only UTF-8 and UTF-16 are supported.</summary>
        public const string @EncodingNotSupported = "EncodingNotSupported";

        /// <summary>Duplicate pieces found in the package.</summary>
        public const string @DuplicatePiecesFound = "DuplicatePiecesFound";

        /// <summary>Cannot find piece with the specified piece number.</summary>
        public const string @PieceDoesNotExist = "PieceDoesNotExist";

        /// <summary>This serviceType is already registered to another service.</summary>
        public const string @ServiceTypeAlreadyAdded = "ServiceTypeAlreadyAdded";

        /// <summary>'{0}' type name does not have the expected format 'className, assembly'.</summary>
        public const string @QualifiedNameHasWrongFormat = "QualifiedNameHasWrongFormat";

        /// <summary>Too many attributes are specified for '{0}'.</summary>
        public const string @ParserAttributeArgsHigh = "ParserAttributeArgsHigh";

        /// <summary>'{0}' requires more attributes.</summary>
        public const string @ParserAttributeArgsLow = "ParserAttributeArgsLow";

        /// <summary>Cannot load assembly '{0}' because a different version of that same assembly is loaded '{1}'.</summary>
        public const string @ParserAssemblyLoadVersionMismatch = "ParserAssemblyLoadVersionMismatch";

        /// <summary>(null)</summary>
        public const string @ToStringNull = "ToStringNull";

        /// <summary>'{0}' ValueSerializer cannot convert '{1}' to '{2}'.</summary>
        public const string @ConvertToException = "ConvertToException";

        /// <summary>'{0}' ValueSerializer cannot convert from '{1}'.</summary>
        public const string @ConvertFromException = "ConvertFromException";

        /// <summary>SortDescription must have a nonempty property name.</summary>
        public const string @SortDescriptionPropertyNameCannotBeEmpty = "SortDescriptionPropertyNameCannotBeEmpty";

        /// <summary>Cannot modify a '{0}' after it is sealed.</summary>
        public const string @CannotChangeAfterSealed = "CannotChangeAfterSealed";

        /// <summary>Cannot group by property '{0}' because it cannot be found on type '{1}'.</summary>
        public const string @BadPropertyForGroup = "BadPropertyForGroup";

        /// <summary>The CollectionView that originates this CurrentChanging event is in a state that does not allow the event to be canceled. Check CurrentChangingEventArgs.IsCancelable before assigning to this CurrentChangingEventArgs.Cancel property.</summary>
        public const string @CurrentChangingCannotBeCanceled = "CurrentChangingCannotBeCanceled";

        /// <summary>Collection is read-only.</summary>
        public const string @NotSupported_ReadOnlyCollection = "NotSupported_ReadOnlyCollection";

        /// <summary>Only single dimensional arrays are supported for the requested action.</summary>
        public const string @Arg_RankMultiDimNotSupported = "Arg_RankMultiDimNotSupported";

        /// <summary>The lower bound of target array must be zero.</summary>
        public const string @Arg_NonZeroLowerBound = "Arg_NonZeroLowerBound";

        /// <summary>Non-negative number required.</summary>
        public const string @ArgumentOutOfRange_NeedNonNegNum = "ArgumentOutOfRange_NeedNonNegNum";

        /// <summary>Destination array is not long enough to copy all the items in the collection. Check array index and length.</summary>
        public const string @Arg_ArrayPlusOffTooSmall = "Arg_ArrayPlusOffTooSmall";

        /// <summary>Target array type is not compatible with the type of items in the collection.</summary>
        public const string @Argument_InvalidArrayType = "Argument_InvalidArrayType";

        /// <summary>'{0}' index is beyond maximum '{1}'.</summary>
        public const string @ReachOutOfRange = "ReachOutOfRange";

        /// <summary>Permission state is not valid.</summary>
        public const string @InvalidPermissionState = "InvalidPermissionState";

        /// <summary>Target is not a WebBrowserPermission.</summary>
        public const string @TargetNotWebBrowserPermissionLevel = "TargetNotWebBrowserPermissionLevel";

        /// <summary>Target is not a MediaPermission.</summary>
        public const string @TargetNotMediaPermissionLevel = "TargetNotMediaPermissionLevel";

        /// <summary>'{0}' attribute is not valid XML.</summary>
        public const string @BadXml = "BadXml";

        /// <summary>Permission level is not valid.</summary>
        public const string @InvalidPermissionLevel = "InvalidPermissionLevel";

        /// <summary>Choice is valid only in AlternateContent.</summary>
        public const string @XCRChoiceOnlyInAC = "XCRChoiceOnlyInAC";

        /// <summary>Choice cannot follow a Fallback.</summary>
        public const string @XCRChoiceAfterFallback = "XCRChoiceAfterFallback";

        /// <summary>Choice must contain Requires attribute.</summary>
        public const string @XCRRequiresAttribNotFound = "XCRRequiresAttribNotFound";

        /// <summary>Requires attribute must contain a valid namespace prefix.</summary>
        public const string @XCRInvalidRequiresAttribute = "XCRInvalidRequiresAttribute";

        /// <summary>Fallback is valid only in AlternateContent.</summary>
        public const string @XCRFallbackOnlyInAC = "XCRFallbackOnlyInAC";

        /// <summary>AlternateContent must contain one or more Choice elements.</summary>
        public const string @XCRChoiceNotFound = "XCRChoiceNotFound";

        /// <summary>AlternateContent must contain only one Fallback element.</summary>
        public const string @XCRMultipleFallbackFound = "XCRMultipleFallbackFound";

        /// <summary>'{0}' attribute is not valid for '{1}' element.</summary>
        public const string @XCRInvalidAttribInElement = "XCRInvalidAttribInElement";

        /// <summary>Unrecognized Compatibility element '{0}'.</summary>
        public const string @XCRUnknownCompatElement = "XCRUnknownCompatElement";

        /// <summary>'{0}' element is not a valid child of AlternateContent. Only Choice and Fallback elements are valid children of an AlternateContent element.</summary>
        public const string @XCRInvalidACChild = "XCRInvalidACChild";

        /// <summary>'{0}' format is not valid.</summary>
        public const string @XCRInvalidFormat = "XCRInvalidFormat";

        /// <summary>'{0}' prefix is not defined.</summary>
        public const string @XCRUndefinedPrefix = "XCRUndefinedPrefix";

        /// <summary>Unrecognized compatibility attribute '{0}'.</summary>
        public const string @XCRUnknownCompatAttrib = "XCRUnknownCompatAttrib";

        /// <summary>'{0}' namespace cannot process content; it must be declared Ignorable first.</summary>
        public const string @XCRNSProcessContentNotIgnorable = "XCRNSProcessContentNotIgnorable";

        /// <summary>Duplicate ProcessContent declaration for element '{1}' in namespace '{0}'.</summary>
        public const string @XCRDuplicateProcessContent = "XCRDuplicateProcessContent";

        /// <summary>Cannot have both a specific and a wildcard ProcessContent declaration for namespace '{0}'.</summary>
        public const string @XCRInvalidProcessContent = "XCRInvalidProcessContent";

        /// <summary>Duplicate wildcard ProcessContent declaration for namespace '{0}'.</summary>
        public const string @XCRDuplicateWildcardProcessContent = "XCRDuplicateWildcardProcessContent";

        /// <summary>MustUnderstand condition failed on namespace '{0}'</summary>
        public const string @XCRMustUnderstandFailed = "XCRMustUnderstandFailed";

        /// <summary>'{0}' namespace cannot preserve items; it must be declared Ignorable first.</summary>
        public const string @XCRNSPreserveNotIgnorable = "XCRNSPreserveNotIgnorable";

        /// <summary>Duplicate Preserve declaration for element {1} in namespace '{0}'.</summary>
        public const string @XCRDuplicatePreserve = "XCRDuplicatePreserve";

        /// <summary>Cannot have both a specific and a wildcard Preserve declaration for namespace '{0}'.</summary>
        public const string @XCRInvalidPreserve = "XCRInvalidPreserve";

        /// <summary>Duplicate wildcard Preserve declaration for namespace '{0}'.</summary>
        public const string @XCRDuplicateWildcardPreserve = "XCRDuplicateWildcardPreserve";

        /// <summary>'{0}' attribute value is not a valid XML name.</summary>
        public const string @XCRInvalidXMLName = "XCRInvalidXMLName";

        /// <summary>There is a cycle of XML compatibility definitions, such that namespace '{0}' overrides itself. This could be due to inconsistent XmlnsCompatibilityAttributes in different assemblies. Please change the definitions to eliminate this cycle.</summary>
        public const string @XCRCompatCycle = "XCRCompatCycle";

        /// <summary>'{1}' event not found on type '{0}'.</summary>
        public const string @EventNotFound = "EventNotFound";

        /// <summary>Listener did not handle requested event.</summary>
        public const string @ListenerDidNotHandleEvent = "ListenerDidNotHandleEvent";

        /// <summary>Listener of type '{0}' registered with event manager of type '{1}', but then did not handle the event. The listener is coded incorrectly.</summary>
        public const string @ListenerDidNotHandleEventDetail = "ListenerDidNotHandleEventDetail";

        /// <summary>WeakEventManager supports only delegates with one target.</summary>
        public const string @NoMulticastHandlers = "NoMulticastHandlers";

        /// <summary>Unrecoverable system error.</summary>
        public const string @InvariantFailure = "InvariantFailure";

        /// <summary>ContentType string cannot have leading/trailing Linear White Spaces [LWS - RFC 2616].</summary>
        public const string @ContentTypeCannotHaveLeadingTrailingLWS = "ContentTypeCannotHaveLeadingTrailingLWS";

        /// <summary>ContentType string is not valid. Expected format is type/subtype.</summary>
        public const string @InvalidTypeSubType = "InvalidTypeSubType";

        /// <summary>';' must be followed by parameter=value pair.</summary>
        public const string @ExpectingParameterValuePairs = "ExpectingParameterValuePairs";

        /// <summary>Parameter and value pair is not valid. Expected form is parameter=value.</summary>
        public const string @InvalidParameterValuePair = "InvalidParameterValuePair";

        /// <summary>A token is not valid. Refer to RFC 2616 for correct grammar of content types.</summary>
        public const string @InvalidToken = "InvalidToken";

        /// <summary>Parameter value must be a valid token or a quoted string as per RFC 2616.</summary>
        public const string @InvalidParameterValue = "InvalidParameterValue";

        /// <summary>A Linear White Space character is not valid.</summary>
        public const string @InvalidLinearWhiteSpaceCharacter = "InvalidLinearWhiteSpaceCharacter";

        /// <summary>Semicolon separator is required between two valid parameter=value pairs.</summary>
        public const string @ExpectingSemicolon = "ExpectingSemicolon";

        /// <summary>HwndSubclass.Attach has already been called;  it cannot be called again.</summary>
        public const string @HwndSubclassMultipleAttach = "HwndSubclassMultipleAttach";

        /// <summary>Cannot locate resource '{0}'.</summary>
        public const string @UnableToLocateResource = "UnableToLocateResource";

        /// <summary>Please wait while the application opens</summary>
        public const string @SplashScreenIsLoading = "SplashScreenIsLoading";

        /// <summary>Name cannot be an empty string.</summary>
        public const string @NameScopeNameNotEmptyString = "NameScopeNameNotEmptyString";

        /// <summary>'{0}' Name is not found.</summary>
        public const string @NameScopeNameNotFound = "NameScopeNameNotFound";

        /// <summary>Cannot register duplicate Name '{0}' in this scope.</summary>
        public const string @NameScopeDuplicateNamesNotAllowed = "NameScopeDuplicateNamesNotAllowed";

        /// <summary>No NameScope found to {1} the Name '{0}'.</summary>
        public const string @NameScopeNotFound = "NameScopeNotFound";

        /// <summary>'{0}' name is not valid for identifier.</summary>
        public const string @NameScopeInvalidIdentifierName = "NameScopeInvalidIdentifierName";

        /// <summary>No dependency property {0} on {1}.</summary>
        public const string @NoDependencyProperty = "NoDependencyProperty";

        /// <summary>Must set ArrayType before calling ProvideValue on ArrayExtension.</summary>
        public const string @MarkupExtensionArrayType = "MarkupExtensionArrayType";

        /// <summary>Items in the array must be type '{0}'. One or more items cannot be cast to this type.</summary>
        public const string @MarkupExtensionArrayBadType = "MarkupExtensionArrayBadType";

        /// <summary>Markup extension '{0}' requires '{1}' be implemented in the IServiceProvider for ProvideValue.</summary>
        public const string @MarkupExtensionNoContext = "MarkupExtensionNoContext";

        /// <summary>'{0}' StaticExtension value cannot be resolved to an enumeration, static field, or static property.</summary>
        public const string @MarkupExtensionBadStatic = "MarkupExtensionBadStatic";

        /// <summary>StaticExtension must have Member property set before ProvideValue can be called.</summary>
        public const string @MarkupExtensionStaticMember = "MarkupExtensionStaticMember";

        /// <summary>TypeExtension must have TypeName property set before ProvideValue can be called.</summary>
        public const string @MarkupExtensionTypeName = "MarkupExtensionTypeName";

        /// <summary>'{0}' string is not valid for type.</summary>
        public const string @MarkupExtensionTypeNameBad = "MarkupExtensionTypeNameBad";

        /// <summary>'{0}' must be of type '{1}'.</summary>
        public const string @MustBeOfType = "MustBeOfType";

        /// <summary>This operation requires the thread's apartment state to be '{0}'.</summary>
        public const string @Verify_ApartmentState = "Verify_ApartmentState";

        /// <summary>The argument can neither be null nor empty.</summary>
        public const string @Verify_NeitherNullNorEmpty = "Verify_NeitherNullNorEmpty";

        /// <summary>The argument can not be equal to '{0}'.</summary>
        public const string @Verify_AreNotEqual = "Verify_AreNotEqual";

        /// <summary>No file exists at '{0}'.</summary>
        public const string @Verify_FileExists = "Verify_FileExists";

        /// <summary>Event argument is invalid.</summary>
        public const string @InvalidEvent = "InvalidEvent";

        /// <summary>The property '{0}' cannot be changed. The '{1}' class has been sealed.</summary>
        public const string @CompatibilityPreferencesSealed = "CompatibilityPreferencesSealed";

        /// <summary>Desktop applications are required to opt in to all earlier accessibility improvements to get the later improvements. To do this, ensure that if the AppContext switch 'Switch.UseLegacyAccessibilityFeatures.N' is set to 'false', then 'Switch.UseLegacyAccessi ...</summary>
        public const string @CombinationOfAccessibilitySwitchesNotSupported = "CombinationOfAccessibilitySwitchesNotSupported";

        /// <summary>Desktop applications setting AppContext switch '{0}' to false are required to opt in to all earlier accessibility improvements. To do this, ensure that the AppContext switch '{1}' is set to 'false', then 'Switch.UseLegacyAccessibilityFeatures' and all 'Swi ...</summary>
        public const string @AccessibilitySwitchDependencyNotSatisfied = "AccessibilitySwitchDependencyNotSatisfied";

        /// <summary>Extra data encountered at position {0} while parsing '{1}'.</summary>
        public const string @TokenizerHelperExtraDataEncountered = "TokenizerHelperExtraDataEncountered";

        /// <summary>Premature string termination encountered while parsing '{0}'.</summary>
        public const string @TokenizerHelperPrematureStringTermination = "TokenizerHelperPrematureStringTermination";

        /// <summary>Missing end quote encountered while parsing '{0}'.</summary>
        public const string @TokenizerHelperMissingEndQuote = "TokenizerHelperMissingEndQuote";

        /// <summary>Empty token encountered at position {0} while parsing '{1}'.</summary>
        public const string @TokenizerHelperEmptyToken = "TokenizerHelperEmptyToken";

        /// <summary>No current object to return.</summary>
        public const string @Enumerator_VerifyContext = "Enumerator_VerifyContext";

        /// <summary>PermissionState value '{0}' is not valid for this Permission.</summary>
        public const string @InvalidPermissionStateValue = "InvalidPermissionStateValue";

        /// <summary>Permission type is not valid. Expected '{0}'.</summary>
        public const string @InvalidPermissionType = "InvalidPermissionType";

        /// <summary>Parameter cannot be a zero-length string.</summary>
        public const string @StringEmpty = "StringEmpty";

        /// <summary>Parameter must be greater than or equal to zero.</summary>
        public const string @ParameterCannotBeNegative = "ParameterCannotBeNegative";

        /// <summary>Specified value of type '{0}' must have IsFrozen set to false to modify.</summary>
        public const string @Freezable_CantBeFrozen = "Freezable_CantBeFrozen";

        /// <summary>Cannot change property metadata after it has been associated with a property.</summary>
        public const string @TypeMetadataCannotChangeAfterUse = "TypeMetadataCannotChangeAfterUse";

        /// <summary>'{0}' enumeration value is not valid.</summary>
        public const string @Enum_Invalid = "Enum_Invalid";

        /// <summary>Cannot convert string value '{0}' to type '{1}'.</summary>
        public const string @CannotConvertStringToType = "CannotConvertStringToType";

        /// <summary>Cannot modify a read-only container.</summary>
        public const string @CannotModifyReadOnlyContainer = "CannotModifyReadOnlyContainer";

        /// <summary>Cannot get part or part information from a write-only container.</summary>
        public const string @CannotRetrievePartsOfWriteOnlyContainer = "CannotRetrievePartsOfWriteOnlyContainer";

        /// <summary>'{0}' file does not conform to the expected file format specification.</summary>
        public const string @FileFormatExceptionWithFileName = "FileFormatExceptionWithFileName";

        /// <summary>Input file or data stream does not conform to the expected file format specification.</summary>
        public const string @FileFormatException = "FileFormatException";

        /// <summary>{0} is an invalid handle.</summary>
        public const string @Cryptography_InvalidHandle = "Cryptography_InvalidHandle";

        /// <summary>DLL Name: {0} DLL Location: {1}</summary>
        public const string @WpfDllConsistencyErrorData = "WpfDllConsistencyErrorData";

        /// <summary>Failed Alternet UI DLL consistency checks. Expected location: {0}.</summary>
        public const string @WpfDllConsistencyErrorHeader = "WpfDllConsistencyErrorHeader";

        /// <summary>Every RoutedEventArgs must have a non-null RoutedEvent associated with it.</summary>
        public const string @RoutedEventArgsMustHaveRoutedEvent = "RoutedEventArgsMustHaveRoutedEvent";

        /// <summary>A '{0}' cannot be set on the '{1}' property of type '{2}'. A '{0}' can only be set on a DependencyProperty of a DependencyObject.</summary>
        public const string @MarkupExtensionDynamicOrBindingOnClrProp = "MarkupExtensionDynamicOrBindingOnClrProp";

        /// <summary>A '{0}' cannot be used within a '{1}' collection. A '{0}' can only be set on a DependencyProperty of a DependencyObject.</summary>
        public const string @MarkupExtensionDynamicOrBindingInCollection = "MarkupExtensionDynamicOrBindingInCollection";

        /// <summary>Binding cannot be changed after it has been used.</summary>
        public const string @ChangeSealedBinding = "ChangeSealedBinding";

        /// <summary>Validation rule '{0}' received unexpected value '{1}'.  (This could be caused by assigning the wrong ValidationStep to the rule.)</summary>
        public const string @ValidationRule_UnexpectedValue = "ValidationRule_UnexpectedValue";

        /// <summary>Syntax error in Binding.Path '{0}' ... '{1}'.</summary>
        public const string @PathSyntax = "PathSyntax";

        /// <summary>Unmatched parenthesis '{0}'.</summary>
        public const string @UnmatchedParen = "UnmatchedParen";

        /// <summary>Unmatched bracket '{0}'.</summary>
        public const string @UnmatchedBracket = "UnmatchedBracket";

        /// <summary>URI must be absolute. Relative URIs are not supported.</summary>
        public const string @UriMustBeAbsolute = "UriMustBeAbsolute";

        /// <summary>This factory supports only URIs with the '{0}' scheme.</summary>
        public const string @UriSchemeMismatch = "UriSchemeMismatch";

        /// <summary>The package URI is not allowed in the package store.</summary>
        public const string @NotAllowedPackageUri = "NotAllowedPackageUri";

        /// <summary>A package with the same URI is already in the package store.</summary>
        public const string @PackageAlreadyExists = "PackageAlreadyExists";

        /// <summary>Current CachePolicy is CacheOnly but the requested resource does not exist in the cache.</summary>
        public const string @ResourceNotFoundUnderCacheOnlyPolicy = "ResourceNotFoundUnderCacheOnlyPolicy";

        /// <summary>Cache policy is not valid.</summary>
        public const string @PackWebRequestCachePolicyIllegal = "PackWebRequestCachePolicyIllegal";

        /// <summary>Cannot have empty name of a temporary file.</summary>
        public const string @InvalidTempFileName = "InvalidTempFileName";

        /// <summary>The operation is not allowed after the first request is made.</summary>
        public const string @RequestAlreadyStarted = "RequestAlreadyStarted";

        /// <summary>Cannot access a disposed HTTP byte range downloader.</summary>
        public const string @ByteRangeDownloaderDisposed = "ByteRangeDownloaderDisposed";

        /// <summary>HTTP byte range downloader can support only HTTP or HTTPS schemes.</summary>
        public const string @InvalidScheme = "InvalidScheme";

        /// <summary>The event handle is not usable.</summary>
        public const string @InvalidEventHandle = "InvalidEventHandle";

        /// <summary>Byte range request failed.</summary>
        public const string @ByteRangeDownloaderErroredOut = "ByteRangeDownloaderErroredOut";

        /// <summary>Byte ranges are not valid in '{0}'.</summary>
        public const string @InvalidByteRanges = "InvalidByteRanges";

        /// <summary>Server does not support byte range request.</summary>
        public const string @ByteRangeRequestIsNotSupported = "ByteRangeRequestIsNotSupported";

        /// <summary>Requested PackagePart not found in target resource.</summary>
        public const string @WebResponsePartNotFound = "WebResponsePartNotFound";

        /// <summary>Error processing WebResponse.</summary>
        public const string @WebResponseFailure = "WebResponseFailure";

        /// <summary>WebRequest timed out. Response did not arrive before the specified Timeout period elapsed.</summary>
        public const string @WebRequestTimeout = "WebRequestTimeout";

        /// <summary>Cannot resolve current inner request URI schema. Bypass cache only for resolvable schema types such as http, ftp, or file.</summary>
        public const string @SchemaInvalidForTransport = "SchemaInvalidForTransport";

        /// <summary>Cannot convert type '{0}' to '{1}'.</summary>
        public const string @CannotConvertType = "CannotConvertType";

        /// <summary>Parameter is unexpected type '{0}'. Expected type is '{1}'.</summary>
        public const string @UnexpectedParameterType = "UnexpectedParameterType";

        /// <summary>Property path is not valid. '{0}' does not have a public property named '{1}'.</summary>
        public const string @PropertyPathNoProperty = "PropertyPathNoProperty";

        /// <summary>Cannot use indexed Value on PropertyDescriptor.</summary>
        public const string @IndexedPropDescNotImplemented = "IndexedPropDescNotImplemented";

        /// <summary>Text formatting engine encountered a non-CLS exception.</summary>
        public const string @NonCLSException = "NonCLSException";

        /// <summary>A TwoWay or OneWayToSource binding cannot work on the read-only property '{1}' of type '{0}'.</summary>
        public const string @CannotWriteToReadOnly = "CannotWriteToReadOnly";

        /// <summary>Mode must be specified for RelativeSource.</summary>
        public const string @RelativeSourceNeedsMode = "RelativeSourceNeedsMode";

        /// <summary>AncestorType must be specified for RelativeSource in FindAncestor mode.</summary>
        public const string @RelativeSourceNeedsAncestorType = "RelativeSourceNeedsAncestorType";

        /// <summary>RelativeSource.Mode is immutable after initialization; instead of changing the Mode on this instance, create a new RelativeSource or use a different static instance.</summary>
        public const string @RelativeSourceModeIsImmutable = "RelativeSourceModeIsImmutable";

        /// <summary>RelativeSource is not in FindAncestor mode.</summary>
        public const string @RelativeSourceNotInFindAncestorMode = "RelativeSourceNotInFindAncestorMode";

        /// <summary>AncestorLevel cannot be set to less than 1.</summary>
        public const string @RelativeSourceInvalidAncestorLevel = "RelativeSourceInvalidAncestorLevel";

        /// <summary>Invalid value for RelativeSourceMode enum.</summary>
        public const string @RelativeSourceModeInvalid = "RelativeSourceModeInvalid";

        /// <summary>Syntax error in PropertyPath '{0}'.</summary>
        public const string @PropertyPathSyntaxError = "PropertyPathSyntaxError";

        /// <summary>Cannot set BaseUri on this IUriContext implementation.</summary>
        public const string @ParserProvideValueCantSetUri = "ParserProvideValueCantSetUri";

        /// <summary>Object '{0}' cannot be used as an accessor parameter for a PropertyPath. An accessor parameter must be DependencyProperty, PropertyInfo, or PropertyDescriptor.</summary>
        public const string @PropertyPathInvalidAccessor = "PropertyPathInvalidAccessor";

        /// <summary>Index {0} is out of range of the PathParameters list, which has length {1}.</summary>
        public const string @PathParametersIndexOutOfRange = "PathParametersIndexOutOfRange";

        /// <summary>Property path is not valid. Cannot resolve type name '{0}'.</summary>
        public const string @PropertyPathNoOwnerType = "PropertyPathNoOwnerType";

        /// <summary>PathParameters list contains null at index {0}.</summary>
        public const string @PathParameterIsNull = "PathParameterIsNull";

        /// <summary>Path indexer parameter has value that cannot be resolved to specified type: '{0}'</summary>
        public const string @PropertyPathIndexWrongType = "PropertyPathIndexWrongType";

        /// <summary>Failed to compare two elements in the array.</summary>
        public const string @InvalidOperation_IComparerFailed = "InvalidOperation_IComparerFailed";

        /// <summary>Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.</summary>
        public const string @Argument_InvalidOffLen = "Argument_InvalidOffLen";

        /// <summary>Synchronization callback for '{0}' collection is no longer available.\n This could happen if the callback is an anonymous method.</summary>
        public const string @CollectionView_MissingSynchronizationCallback = "CollectionView_MissingSynchronizationCallback";

        /// <summary>Number of elements in source Enumerable is greater than available space from index to the end of destination array.</summary>
        public const string @CopyToNotEnoughSpace = "CopyToNotEnoughSpace";

        /// <summary>Collection was modified; enumeration operation may not execute.</summary>
        public const string @EnumeratorVersionChanged = "EnumeratorVersionChanged";

        /// <summary>'{0}' type must derive from FrameworkElement or FrameworkContentElement.</summary>
        public const string @MustBeFrameworkDerived = "MustBeFrameworkDerived";

        /// <summary>RoutedEvent Name '{0}' for OwnerType '{1}' already used.</summary>
        public const string @DuplicateEventName = "DuplicateEventName";

        /// <summary>Class handlers can be registered only for UIElement or ContentElement and their subtypes.</summary>
        public const string @ClassTypeIllegal = "ClassTypeIllegal";

        /// <summary>Handler type is mismatched.</summary>
        public const string @HandlerTypeIllegal = "HandlerTypeIllegal";

        /// <summary>FrameworkPropertyMetadata.DefaultUpdateSourceTrigger cannot be set to UpdateSourceTrigger.Default; this would create a circular definition.</summary>
        public const string @NoDefaultUpdateSourceTrigger = "NoDefaultUpdateSourceTrigger";

        /// <summary>The binding group has no binding that uses item '{0}' and property '{1}'.</summary>
        public const string @BindingGroup_NoEntry = "BindingGroup_NoEntry";

        /// <summary>The value for item '{0}' and property '{1}' is not available because a previous validation rule deemed the value invalid, or because the value could not be computed (e.g., conversion failure).</summary>
        public const string @BindingGroup_ValueUnavailable = "BindingGroup_ValueUnavailable";

        /// <summary>Underlying list of this CollectionView does not support filtering.</summary>
        public const string @BindingListCannotCustomFilter = "BindingListCannotCustomFilter";

        /// <summary>'{0}' is not allowed during an AddNew or EditItem transaction.</summary>
        public const string @MemberNotAllowedDuringAddOrEdit = "MemberNotAllowedDuringAddOrEdit";

        /// <summary>'{0}' is not allowed during a transaction begun by '{1}'.</summary>
        public const string @MemberNotAllowedDuringTransaction = "MemberNotAllowedDuringTransaction";

        /// <summary>'{0}' is not allowed for this view.</summary>
        public const string @MemberNotAllowedForView = "MemberNotAllowedForView";

        /// <summary>Removing the NewItem placeholder is not allowed.</summary>
        public const string @RemovingPlaceholder = "RemovingPlaceholder";

        /// <summary>Editing the NewItem placeholder is not allowed.</summary>
        public const string @CannotEditPlaceholder = "CannotEditPlaceholder";

        /// <summary>CancelEdit is not supported for the current edit item.</summary>
        public const string @CancelEditNotSupported = "CancelEditNotSupported";

        /// <summary>Cannot set '{0}' property when '{1}' property is false.</summary>
        public const string @CannotChangeLiveShaping = "CannotChangeLiveShaping";

        /// <summary>Unexpected collection change action '{0}'.</summary>
        public const string @UnexpectedCollectionChangeAction = "UnexpectedCollectionChangeAction";

        /// <summary>IBindingList '{0}' has unexpected length after a '{1}' event.\nThis can
        /// happen if the IBindingList has been changed without raising a corresponding ListChanged event.</summary>
        public const string @InconsistentBindingList = "InconsistentBindingList";

        /// <summary>Cannot find type information on collection; property names to SortBy
        /// cannot be resolved.</summary>
        public const string @CannotDetermineSortByPropertiesForCollection = "CannotDetermineSortByPropertiesForCollection";

        /// <summary>'{0}' type does not have property named '{1}', so cannot sort data
        /// collection.</summary>
        public const string @PropertyToSortByNotFoundOnType = "PropertyToSortByNotFoundOnType";

        /// <summary>Range actions are not supported.</summary>
        public const string @RangeActionsNotSupported = "RangeActionsNotSupported";

        /// <summary>Cannot Move items to an unknown position (-1).</summary>
        public const string @CannotMoveToUnknownPosition = "CannotMoveToUnknownPosition";

        /// <summary>IBindingList can sort by only one property.</summary>
        public const string @BindingListCanOnlySortByOneProperty =
            "BindingListCanOnlySortByOneProperty";

        /// <summary>AccessCollection for '{0}' collection cannot be called after shutdown.</summary>
        public const string @AccessCollectionAfterShutDown = "AccessCollectionAfterShutDown";

        /// <summary>If SortDescriptions is overridden in derived classes, then must also
        /// override '{0}'.</summary>
        public const string @ImplementOtherMembersWithSort = "ImplementOtherMembersWithSort";

        /// <summary>This type of CollectionView does not support changes to its SourceCollection
        /// from a thread different from the Dispatcher thread.</summary>
        public const string @MultiThreadedCollectionChangeNotSupported =
            "MultiThreadedCollectionChangeNotSupported";

        /// <summary>Cannot change or check the contents or Current position of CollectionView
        /// while Refresh is being deferred.</summary>
        public const string @NoCheckOrChangeWhenDeferred = "NoCheckOrChangeWhenDeferred";

        /// <summary>Collection Remove event must specify item position.</summary>
        public const string @RemovedItemNotFound = "RemovedItemNotFound";

        /// <summary>CollectionViewType property can only be set during initialization.</summary>
        public const string @CollectionViewTypeIsInitOnly = "CollectionViewTypeIsInitOnly";

        /// <summary>'{0}' view does not support sorting.</summary>
        public const string @CannotSortView = "CannotSortView";

        /// <summary>'{0}' view does not support filtering.</summary>
        public const string @CannotFilterView = "CannotFilterView";

        /// <summary>'{0}' view does not support grouping.</summary>
        public const string @CannotGroupView = "CannotGroupView";

        /// <summary>removeIndex is less than zero or greater than or equal to Count.</summary>
        public const string @ItemCollectionRemoveArgumentOutOfRange =
            "ItemCollectionRemoveArgumentOutOfRange";

        /// <summary>CompositeCollectionView only supports NotifyCollectionChangeAction.Reset
        /// when the collection is empty or is being cleared.</summary>
        public const string @CompositeCollectionResetOnlyOnClear =
            "CompositeCollectionResetOnlyOnClear";

        /// <summary>Enumeration has not started. Call MoveNext.</summary>
        public const string @EnumeratorNotStarted = "EnumeratorNotStarted";

        /// <summary>Enumeration already finished.</summary>
        public const string @EnumeratorReachedEnd = "EnumeratorReachedEnd";

        /// <summary>CompositeCollection can accept only CollectionContainers it does not
        /// already have.</summary>
        public const string @CollectionContainerMustBeUniqueForComposite =
            "CollectionContainerMustBeUniqueForComposite";

        /// <summary>'{0}' index in collection change event is not valid for collection of
        /// size '{1}'.</summary>
        public const string @CollectionChangeIndexOutOfRange = "CollectionChangeIndexOutOfRange";

        /// <summary>Added item does not appear at given index '{0}'.</summary>
        public const string @AddedItemNotAtIndex = "AddedItemNotAtIndex";

        /// <summary>A collection Add event refers to item that does not belong to
        /// collection.</summary>
        public const string @AddedItemNotInCollection = "AddedItemNotInCollection";

        /// <summary>BindingCollection does not support items of type {0}. Only Binding is
        /// allowed.</summary>
        public const string @BindingCollectionContainsNonBinding = "BindingCollectionContainsNonBinding";

        /// <summary>'{0}' child does not have type '{1}' : '{2}'.</summary>
        public const string @ChildHasWrongType = "ChildHasWrongType";

        /// <summary>Text content is not allowed on this element. Cannot add the text '{0}'.</summary>
        public const string @NonWhiteSpaceInAddText = "NonWhiteSpaceInAddText";

        /// <summary>Cannot set MultiBinding because MultiValueConverter must be specified.</summary>
        public const string @MultiBindingHasNoConverter = "MultiBindingHasNoConverter";

        /// <summary>Cannot set UpdateSourceTrigger on inner Binding of MultiBinding. Only
        /// the default Immediate UpdateSourceTrigger is valid.</summary>
        public const string @NoUpdateSourceTriggerForInnerBindingOfMultiBinding =
            "NoUpdateSourceTriggerForInnerBindingOfMultiBinding";

        /// <summary>The binding expression already belongs to a BindingGroup;  it cannot
        /// be added to a different BindingGroup.</summary>
        public const string @BindingGroup_CannotChangeGroups = "BindingGroup_CannotChangeGroups";

        /// <summary>Internal error: internal Alternet UI code tried to reactivate a
        /// BindingExpression that was already marked as detached.</summary>
        public const string @BindingExpressionStatusChanged = "BindingExpressionStatusChanged";

        /// <summary>Binding.{0} cannot be set while using Binding.{1}.</summary>
        public const string @BindingConflict = "BindingConflict";

        /// <summary>Cannot get response for web request to '{0}'.</summary>
        public const string @GetResponseFailed = "GetResponseFailed";

        /// <summary>Cannot create web request for specified Pack URI.</summary>
        public const string @WebRequestCreationFailed = "WebRequestCreationFailed";

        /// <summary>Cannot perform this operation when binding is detached.</summary>
        public const string @BindingExpressionIsDetached = "BindingExpressionIsDetached";

        /// <summary>'{1}' property of argument '{0}' must not be null.</summary>
        public const string @ArgumentPropertyMustNotBeNull = "ArgumentPropertyMustNotBeNull";

        /// <summary>'{0}' property cannot be data-bound.</summary>
        public const string @PropertyNotBindable = "PropertyNotBindable";

        /// <summary>Two-way binding requires Path or XPath.</summary>
        public const string @TwoWayBindingNeedsPath = "TwoWayBindingNeedsPath";

        /// <summary>Cannot find converter.</summary>
        public const string @MissingValueConverter = "MissingValueConverter";

        /// <summary>Unrecognized ValidationStep '{0}' obtained from '{1}'.</summary>
        public const string @ValidationRule_UnknownStep = "ValidationRule_UnknownStep";

        /// <summary>Value '{0}' could not be converted.</summary>
        public const string @Validation_ConversionFailed = "Validation_ConversionFailed";

        /// <summary>XmlDataNamespaceMappingCollection child does not have type
        /// XmlNamespaceMapping '{0}'.</summary>
        public const string @RequiresXmlNamespaceMapping = "RequiresXmlNamespaceMapping";

        /// <summary>XmlDataNamespaceMappingCollection cannot use XmlNamespaceMapping that has
        /// null URI.</summary>
        public const string @RequiresXmlNamespaceMappingUri = "RequiresXmlNamespaceMappingUri";

        /// <summary>The number of elements in this collection is greater than the available
        /// space from '{0}' to the end of destination '{1}'.</summary>
        public const string @Collection_CopyTo_NumberOfElementsExceedsArrayLength = "Collection_CopyTo_NumberOfElementsExceedsArrayLength";

        /// <summary>ConstructorParameters cannot be changed because ObjectDataProvider is using
        /// user-assigned ObjectInstance.</summary>
        public const string @ObjectDataProviderParameterCollectionIsNotInUse = "ObjectDataProviderParameterCollectionIsNotInUse";

        /// <summary>ObjectDataProvider can only be assigned an ObjectType or an ObjectInstance,
        /// not both.</summary>
        public const string @ObjectDataProviderCanHaveOnlyOneSource = "ObjectDataProviderCanHaveOnlyOneSource";

        /// <summary>ObjectDataProvider needs either an ObjectType or ObjectInstance.</summary>
        public const string @ObjectDataProviderHasNoSource = "ObjectDataProviderHasNoSource";

        /// <summary>Unknown exception while creating type '{0}' for ObjectDataProvider.</summary>
        public const string @ObjectDataProviderNonCLSException = "ObjectDataProviderNonCLSException";

        /// <summary>Unknown exception while invoking method '{0}' on type '{1}'
        /// for ObjectDataProvider.</summary>
        public const string @ObjectDataProviderNonCLSExceptionInvoke = "ObjectDataProviderNonCLSExceptionInvoke";

        /// <summary>The required pattern for URI containing ";component"
        /// is "AssemblyName;Vxxxx;PublicKey;component", where Vxxxx is the assembly version and
        /// PublicKey is the 16-character string representing the assembly public key token. Vxxxx and
        /// PublicKey are optional.</summary>
        public const string @WrongFirstSegment = "WrongFirstSegment";

        /// <summary>Cannot navigate to application resource '{0}' by using a WebBrowser control.
        /// For URI navigation, the resource must be at the application's site of origin.
        /// Use the pack://siteoforigin:,,,/ prefix to avoid hard-coding the URI.</summary>
        public const string @CannotNavigateToApplicationResourcesInWebBrowser = "CannotNavigateToApplicationResourcesInWebBrowser";

        /// <summary>The '{0}' property of the '{1}' type can be set only during
        /// initialization.</summary>
        public const string @PropertyIsInitializeOnly = "PropertyIsInitializeOnly";

        /// <summary>The '{0}' property of the '{1}' type cannot be changed after it has been
        /// set.</summary>
        public const string @PropertyIsImmutable = "PropertyIsImmutable";

        /// <summary>The '{0}' property of the '{1}' type must be set during initialization.</summary>
        public const string @PropertyMustHaveValue = "PropertyMustHaveValue";

        /// <summary>'{0}' type is not a CollectionView type.</summary>
        public const string @CollectionView_WrongType = "CollectionView_WrongType";

        /// <summary>'{0}' does not have a constructor that accepts collection type '{1}'.</summary>
        public const string @CollectionView_ViewTypeInsufficient = "CollectionView_ViewTypeInsufficient";

        /// <summary>Cannot get CollectionView of type '{0}' for CollectionViewSource that
        /// already uses type '{1}'.</summary>
        public const string @CollectionView_NameTypeDuplicity = "CollectionView_NameTypeDuplicity";

        /// <summary>Must set Source in RoutedEventArgs before building event route or
        /// invoking handlers.</summary>
        public const string @SourceNotSet = "SourceNotSet";

        /// <summary>RoutedEvent in RoutedEventArgs and EventRoute are mismatched.</summary>
        public const string @Mismatched_RoutedEvent = "Mismatched_RoutedEvent";

        /// <summary>Potential cycle in tree found while building the event route.</summary>
        public const string @TreeLoop = "TreeLoop";

        /// <summary>InheritanceBehavior must be set when the instance is not yet connected to a
        /// tree. Set InheritanceBehavior when the object is constructed.</summary>
        public const string @Illegal_InheritanceBehaviorSettor = "Illegal_InheritanceBehaviorSettor";

        /// <summary>Logical tree depth exceeded while traversing the tree. This could indicate
        /// a cycle in the tree.</summary>
        public const string @LogicalTreeLoop = "LogicalTreeLoop";

        /// <summary>'{0}' is not a valid type for IInputElement. UIElement or
        /// ContentElement expected.</summary>
        public const string @Invalid_IInputElement = "Invalid_IInputElement";

        /// <summary>Only PreProcessInput and PostProcessInput events can access InputManager
        /// staging area.</summary>
        public const string @NotAllowedToAccessStagingArea = "NotAllowedToAccessStagingArea";

        /// <summary>Result text cannot be null.</summary>
        public const string @TextComposition_NullResultText = "TextComposition_NullResultText";

        /// <summary>'{0}' does not have a valid InputManager.</summary>
        public const string @TextCompositionManager_NoInputManager = "TextCompositionManager_NoInputManager";

        /// <summary>'{0}' has already started.</summary>
        public const string @TextCompositionManager_TextCompositionHasStarted = "TextCompositionManager_TextCompositionHasStarted";

        /// <summary>'{0}' has not yet started.</summary>
        public const string @TextCompositionManager_TextCompositionNotStarted = "TextCompositionManager_TextCompositionNotStarted";

        /// <summary>'{0}' has already finished.</summary>
        public const string @TextCompositionManager_TextCompositionHasDone = "TextCompositionManager_TextCompositionHasDone";

        /// <summary>The calling thread must be STA, because many UI components require this.</summary>
        public const string @RequiresSTA = "RequiresSTA";

        /// <summary>'{0}+{1}' key and modifier combination is not supported for KeyGesture.</summary>
        public const string @KeyGesture_Invalid = "KeyGesture_Invalid";

        /// <summary>Operation not supported on a read-only InputGestureCollection.</summary>
        public const string @ReadOnlyInputGesturesCollection = "ReadOnlyInputGesturesCollection";

        /// <summary>Collection accepts only objects of type InputGesture.</summary>
        public const string @CollectionOnlyAcceptsInputGestures = "CollectionOnlyAcceptsInputGestures";

        /// <summary>Gesture accepts only objects of type '{0}'.</summary>
        public const string @InputBinding_ExpectedInputGesture = "InputBinding_ExpectedInputGesture";

        private static global::System.Resources.ResourceManager resourceManager;

        internal static global::System.Resources.ResourceManager ResourceManager =>
            resourceManager ??= new global::System.Resources.ResourceManager(
                "Alternet.UI.Resources.Strings",
                typeof(SR).Assembly);

        internal static global::System.Globalization.CultureInfo Culture { get; set; }

        [global::System.Runtime.CompilerServices.MethodImpl(
            global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#pragma warning disable
        internal static string GetResourceString(string resourceKey, string defaultValue = null) =>
            ResourceManager.GetString(resourceKey, Culture);
#pragma warning restore
    }
}
