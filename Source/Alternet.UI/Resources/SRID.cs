#nullable disable
using System.Resources;

namespace Alternet.UI
{
    internal static partial class SRID
    {
        private static global::System.Resources.ResourceManager s_resourceManager;
        internal static global::System.Resources.ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new global::System.Resources.ResourceManager("Alternet.UI.Resources.Strings", typeof(SR).Assembly));
        internal static global::System.Globalization.CultureInfo Culture { get; set; }
#if !NET20
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        internal static string GetResourceString(string resourceKey, string defaultValue = null) => ResourceManager.GetString(resourceKey, Culture);
        /// <summary>en</summary>
        internal const string @WPF_UILanguage = "WPF_UILanguage";
        /// <summary>Cannot modify this property on the Empty Rect.</summary>
        internal const string @Rect_CannotModifyEmptyRect = "Rect_CannotModifyEmptyRect";
        /// <summary>Cannot call this method on the Empty Rect.</summary>
        internal const string @Rect_CannotCallMethod = "Rect_CannotCallMethod";
        /// <summary>Width and Height must be non-negative.</summary>
        internal const string @Size_WidthAndHeightCannotBeNegative = "Size_WidthAndHeightCannotBeNegative";
        /// <summary>Width must be non-negative.</summary>
        internal const string @Size_WidthCannotBeNegative = "Size_WidthCannotBeNegative";
        /// <summary>Height must be non-negative.</summary>
        internal const string @Size_HeightCannotBeNegative = "Size_HeightCannotBeNegative";
        /// <summary>Cannot modify this property on the Empty Size.</summary>
        internal const string @Size_CannotModifyEmptySize = "Size_CannotModifyEmptySize";
        /// <summary>Transform is not invertible.</summary>
        internal const string @Transform_NotInvertible = "Transform_NotInvertible";
        /// <summary>Expected object of type '{0}'.</summary>
        internal const string @General_Expected_Type = "General_Expected_Type";
        /// <summary>Value cannot be null. Object reference: '{0}'.</summary>
        internal const string @ReferenceIsNull = "ReferenceIsNull";
        /// <summary>The parameter value must be between '{0}' and '{1}'.</summary>
        internal const string @ParameterMustBeBetween = "ParameterMustBeBetween";
        /// <summary>Handler has not been registered with this event.</summary>
        internal const string @Freezable_UnregisteredHandler = "Freezable_UnregisteredHandler";
        /// <summary>Cannot use a DependencyObject that belongs to a different thread than its parent Freezable.</summary>
        internal const string @Freezable_AttemptToUseInnerValueWithDifferentThread = "Freezable_AttemptToUseInnerValueWithDifferentThread";
        /// <summary>This Freezable cannot be frozen.</summary>
        internal const string @Freezable_CantFreeze = "Freezable_CantFreeze";
        /// <summary>The provided DependencyObject is not a context for this Freezable.</summary>
        internal const string @Freezable_NotAContext = "Freezable_NotAContext";
        /// <summary>Cannot promote from '{0}' to '{1}' because the target map is too small.</summary>
        internal const string @FrugalList_TargetMapCannotHoldAllData = "FrugalList_TargetMapCannotHoldAllData";
        /// <summary>Cannot promote from Array.</summary>
        internal const string @FrugalList_CannotPromoteBeyondArray = "FrugalList_CannotPromoteBeyondArray";
        /// <summary>Cannot promote from '{0}' to '{1}' because the target map is too small.</summary>
        internal const string @FrugalMap_TargetMapCannotHoldAllData = "FrugalMap_TargetMapCannotHoldAllData";
        /// <summary>Cannot promote from Hashtable.</summary>
        internal const string @FrugalMap_CannotPromoteBeyondHashtable = "FrugalMap_CannotPromoteBeyondHashtable";
        /// <summary>Unrecognized Key '{0}'.</summary>
        internal const string @Unsupported_Key = "Unsupported_Key";
        /// <summary>Specified priority is not valid.</summary>
        internal const string @InvalidPriority = "InvalidPriority";
        /// <summary>The minimum priority must be less than or equal to the maximum priority.</summary>
        internal const string @InvalidPriorityRangeOrder = "InvalidPriorityRangeOrder";
        /// <summary>Cannot perform requested operation because the Dispatcher shut down.</summary>
        internal const string @DispatcherHasShutdown = "DispatcherHasShutdown";
        /// <summary>A thread cannot wait on operations already running on the same thread.</summary>
        internal const string @ThreadMayNotWaitOnOperationsAlreadyExecutingOnTheSameThread = "ThreadMayNotWaitOnOperationsAlreadyExecutingOnTheSameThread";
        /// <summary>The calling thread cannot access this object because a different thread owns it.</summary>
        internal const string @VerifyAccess = "VerifyAccess";
        /// <summary>Objects must be created by the same thread.</summary>
        internal const string @MismatchedDispatchers = "MismatchedDispatchers";
        /// <summary>Dispatcher processing has been suspended, but messages are still being processed.</summary>
        internal const string @DispatcherProcessingDisabledButStillPumping = "DispatcherProcessingDisabledButStillPumping";
        /// <summary>Cannot perform this operation while dispatcher processing is suspended.</summary>
        internal const string @DispatcherProcessingDisabled = "DispatcherProcessingDisabled";
        /// <summary>The DispatcherPriorityAwaiter was not configured with a valid Dispatcher.  The only supported usage is from Dispatcher.Yield.</summary>
        internal const string @DispatcherPriorityAwaiterInvalid = "DispatcherPriorityAwaiterInvalid";
        /// <summary>The thread calling Dispatcher.Yield does not have a current Dispatcher.</summary>
        internal const string @DispatcherYieldNoAvailableDispatcher = "DispatcherYieldNoAvailableDispatcher";
        /// <summary>The Dispatcher is unable to request processing.  This is often because the application has starved the Dispatcher's message pump.</summary>
        internal const string @DispatcherRequestProcessingFailed = "DispatcherRequestProcessingFailed";
        /// <summary>Exception Filter Code is not built and installed properly.</summary>
        internal const string @ExceptionFilterCodeNotPresent = "ExceptionFilterCodeNotPresent";
        /// <summary>Unrecognized ModifierKeys '{0}'.</summary>
        internal const string @Unsupported_Modifier = "Unsupported_Modifier";
        /// <summary>TimeSpan period must be greater than or equal to zero.</summary>
        internal const string @TimeSpanPeriodOutOfRange_TooSmall = "TimeSpanPeriodOutOfRange_TooSmall";
        /// <summary>TimeSpan period must be less than or equal to Int32.MaxValue.</summary>
        internal const string @TimeSpanPeriodOutOfRange_TooLarge = "TimeSpanPeriodOutOfRange_TooLarge";
        /// <summary>Cannot clear properties on object '{0}' because it is in a read-only state.</summary>
        internal const string @ClearOnReadOnlyObjectNotAllowed = "ClearOnReadOnlyObjectNotAllowed";
        /// <summary>Cannot automatically generate a valid default value for property '{0}'. Specify a default value explicitly when owner type '{1}' is registering this DependencyProperty.</summary>
        internal const string @DefaultValueAutoAssignFailed = "DefaultValueAutoAssignFailed";
        /// <summary>An Expression object is not a valid default value for a DependencyProperty.</summary>
        internal const string @DefaultValueMayNotBeExpression = "DefaultValueMayNotBeExpression";
        /// <summary>Default value cannot be 'Unset'.</summary>
        internal const string @DefaultValueMayNotBeUnset = "DefaultValueMayNotBeUnset";
        /// <summary>Default value for the '{0}' property cannot be bound to a specific thread.</summary>
        internal const string @DefaultValueMustBeFreeThreaded = "DefaultValueMustBeFreeThreaded";
        /// <summary>Default value type does not match type of property '{0}'.</summary>
        internal const string @DefaultValuePropertyTypeMismatch = "DefaultValuePropertyTypeMismatch";
        /// <summary>Default value for '{0}' property is not valid because ValidateValueCallback failed.</summary>
        internal const string @DefaultValueInvalid = "DefaultValueInvalid";
        /// <summary>'{0}' type does not have a matching DependencyObjectType.</summary>
        internal const string @DTypeNotSupportForSystemType = "DTypeNotSupportForSystemType";
        /// <summary>'{0}' is not a valid value for property '{1}'.</summary>
        internal const string @InvalidPropertyValue = "InvalidPropertyValue";
        /// <summary>Local value enumeration position is out of range.</summary>
        internal const string @LocalValueEnumerationOutOfBounds = "LocalValueEnumerationOutOfBounds";
        /// <summary>Local value enumeration position is before the start, need to call MoveNext first.</summary>
        internal const string @LocalValueEnumerationReset = "LocalValueEnumerationReset";
        /// <summary>Current local value enumeration is outdated because one or more local values have been set since its creation.</summary>
        internal const string @LocalValueEnumerationInvalidated = "LocalValueEnumerationInvalidated";
        /// <summary>Default value factory user must override PropertyMetadata.CreateDefaultValue.</summary>
        internal const string @MissingCreateDefaultValue = "MissingCreateDefaultValue";
        /// <summary>Metadata override and base metadata must be of the same type or derived type.</summary>
        internal const string @OverridingMetadataDoesNotMatchBaseMetadataType = "OverridingMetadataDoesNotMatchBaseMetadataType";
        /// <summary>'{0}' property was already registered by '{1}'.</summary>
        internal const string @PropertyAlreadyRegistered = "PropertyAlreadyRegistered";
        /// <summary>This method overrides metadata only on read-only properties. This property is not read-only.</summary>
        internal const string @PropertyNotReadOnly = "PropertyNotReadOnly";
        /// <summary>'{0}' property was registered as read-only and cannot be modified without an authorization key.</summary>
        internal const string @ReadOnlyChangeNotAllowed = "ReadOnlyChangeNotAllowed";
        /// <summary>Property key is not authorized to modify property '{0}'.</summary>
        internal const string @ReadOnlyKeyNotAuthorized = "ReadOnlyKeyNotAuthorized";
        /// <summary>'{0}' property was registered as read-only and its metadata cannot be overridden without an authorization key.</summary>
        internal const string @ReadOnlyOverrideNotAllowed = "ReadOnlyOverrideNotAllowed";
        /// <summary>Property key is not authorized to override metadata of property '{0}'.</summary>
        internal const string @ReadOnlyOverrideKeyNotAuthorized = "ReadOnlyOverrideKeyNotAuthorized";
        /// <summary>'{0}' is registered as read-only, so its value cannot be coerced by using the DesignerCoerceValueCallback.</summary>
        internal const string @ReadOnlyDesignerCoersionNotAllowed = "ReadOnlyDesignerCoersionNotAllowed";
        /// <summary>Cannot set a property on object '{0}' because it is in a read-only state.</summary>
        internal const string @SetOnReadOnlyObjectNotAllowed = "SetOnReadOnlyObjectNotAllowed";
        /// <summary>Shareable Expression cannot use ChangeSources method.</summary>
        internal const string @ShareableExpressionsCannotChangeSources = "ShareableExpressionsCannotChangeSources";
        /// <summary>Cannot set Expression. It is marked as 'NonShareable' and has already been used.</summary>
        internal const string @SharingNonSharableExpression = "SharingNonSharableExpression";
        /// <summary>ShouldSerializeProperty and ResetProperty methods must be public ('{0}').</summary>
        internal const string @SpecialMethodMustBePublic = "SpecialMethodMustBePublic";
        /// <summary>Must create DependencySource on same Thread as the DependencyObject.</summary>
        internal const string @SourcesMustBeInSameThread = "SourcesMustBeInSameThread";
        /// <summary>Expression is not in use on DependencyObject. Cannot change DependencySource array.</summary>
        internal const string @SourceChangeExpressionMismatch = "SourceChangeExpressionMismatch";
        /// <summary>DependencyProperty limit has been exceeded upon registration of '{0}'. Dependency properties are normally static class members registered with static field initializers or static constructors. In this case, there may be dependency properties accidentally g ...</summary>
        internal const string @TooManyDependencyProperties = "TooManyDependencyProperties";
        /// <summary>Metadata is already associated with a type and property. A new one must be created.</summary>
        internal const string @TypeMetadataAlreadyInUse = "TypeMetadataAlreadyInUse";
        /// <summary>PropertyMetadata is already registered for type '{0}'.</summary>
        internal const string @TypeMetadataAlreadyRegistered = "TypeMetadataAlreadyRegistered";
        /// <summary>'{0}' type must derive from DependencyObject.</summary>
        internal const string @TypeMustBeDependencyObjectDerived = "TypeMustBeDependencyObjectDerived";
        /// <summary>Unrecognized Expression 'Mode' value.</summary>
        internal const string @UnknownExpressionMode = "UnknownExpressionMode";
        /// <summary>Buffer is too small to accommodate the specified parameters.</summary>
        internal const string @BufferTooSmall = "BufferTooSmall";
        /// <summary>Buffer offset cannot be negative.</summary>
        internal const string @BufferOffsetNegative = "BufferOffsetNegative";
        /// <summary>CompoundFile path must be non-empty.</summary>
        internal const string @CompoundFilePathNullEmpty = "CompoundFilePathNullEmpty";
        /// <summary>Cannot create new package on a read-only stream.</summary>
        internal const string @CanNotCreateContainerOnReadOnlyStream = "CanNotCreateContainerOnReadOnlyStream";
        /// <summary>Cannot create a read-only stream.</summary>
        internal const string @CanNotCreateAsReadOnly = "CanNotCreateAsReadOnly";
        /// <summary>Cannot create a stream in a read-only package.</summary>
        internal const string @CanNotCreateInReadOnly = "CanNotCreateInReadOnly";
        /// <summary>Cannot create StorageRoot on a nonreadable stream.</summary>
        internal const string @CanNotCreateStorageRootOnNonReadableStream = "CanNotCreateStorageRootOnNonReadableStream";
        /// <summary>Cannot delete element.</summary>
        internal const string @CanNotDelete = "CanNotDelete";
        /// <summary>Cannot delete element because access is denied.</summary>
        internal const string @CanNotDeleteAccessDenied = "CanNotDeleteAccessDenied";
        /// <summary>Cannot create data storage because access is denied.</summary>
        internal const string @CanNotCreateAccessDenied = "CanNotCreateAccessDenied";
        /// <summary>Cannot delete read-only packages.</summary>
        internal const string @CanNotDeleteInReadOnly = "CanNotDeleteInReadOnly";
        /// <summary>Cannot delete because the storage is not empty. Try a recursive delete with Delete(true).</summary>
        internal const string @CanNotDeleteNonEmptyStorage = "CanNotDeleteNonEmptyStorage";
        /// <summary>Cannot delete the root StorageInfo.</summary>
        internal const string @CanNotDeleteRoot = "CanNotDeleteRoot";
        /// <summary>Cannot perform this function on a storage that does not exist.</summary>
        internal const string @CanNotOnNonExistStorage = "CanNotOnNonExistStorage";
        /// <summary>Cannot open data storage.</summary>
        internal const string @CanNotOpenStorage = "CanNotOpenStorage";
        /// <summary>Cannot find specified package file.</summary>
        internal const string @ContainerNotFound = "ContainerNotFound";
        /// <summary>Cannot open specified package file.</summary>
        internal const string @ContainerCanNotOpen = "ContainerCanNotOpen";
        /// <summary>Create mode parameter must be either FileMode.Create or FileMode.Open.</summary>
        internal const string @CreateModeMustBeCreateOrOpen = "CreateModeMustBeCreateOrOpen";
        /// <summary>Compound File API failure.</summary>
        internal const string @CFAPIFailure = "CFAPIFailure";
        /// <summary>The given data space label name is already in use.</summary>
        internal const string @DataSpaceLabelInUse = "DataSpaceLabelInUse";
        /// <summary>Empty string is not a valid data space label.</summary>
        internal const string @DataSpaceLabelInvalidEmpty = "DataSpaceLabelInvalidEmpty";
        /// <summary>Specified data space label has not been defined.</summary>
        internal const string @DataSpaceLabelUndefined = "DataSpaceLabelUndefined";
        /// <summary>DataSpaceManager object was disposed.</summary>
        internal const string @DataSpaceManagerDisposed = "DataSpaceManagerDisposed";
        /// <summary>DataSpace map entry is not valid.</summary>
        internal const string @DataSpaceMapEntryInvalid = "DataSpaceMapEntryInvalid";
        /// <summary>FileAccess value is not valid.</summary>
        internal const string @FileAccessInvalid = "FileAccessInvalid";
        /// <summary>
        /// RoutedEvent/EventPrivateKey limit exceeded. Routed events or EventPrivateKey for CLR events are typically static class members registered with field initializers or static constructors. In this case, routed events or EventPrivateKeys might be getting initialized in instance constructors, causing the limit to be exceeded.
        /// </summary>
        internal const string @TooManyRoutedEvents = "TooManyRoutedEvents";
        /// <summary>File already exists.</summary>
        internal const string @FileAlreadyExists = "FileAlreadyExists";
        /// <summary>FileMode value is not supported.</summary>
        internal const string @FileModeUnsupported = "FileModeUnsupported";
        /// <summary>FileMode value is not valid.</summary>
        internal const string @FileModeInvalid = "FileModeInvalid";
        /// <summary>FileShare value is not supported.</summary>
        internal const string @FileShareUnsupported = "FileShareUnsupported";
        /// <summary>FileShare value is not valid.</summary>
        internal const string @FileShareInvalid = "FileShareInvalid";
        /// <summary>Streams for exposure as ILockBytes must be seekable.</summary>
        internal const string @ILockBytesStreamMustSeek = "ILockBytesStreamMustSeek";
        /// <summary>'{1}' is not a valid value for '{0}'.</summary>
        internal const string @InvalidArgumentValue = "InvalidArgumentValue";
        /// <summary>Cannot locate information for stream that should exist. This is an internally inconsistent condition.</summary>
        internal const string @InvalidCondition01 = "InvalidCondition01";
        /// <summary>String format is not valid.</summary>
        internal const string @InvalidStringFormat = "InvalidStringFormat";
        /// <summary>Internal table type value is not valid. This is an internally inconsistent condition.</summary>
        internal const string @InvalidTableType = "InvalidTableType";
        /// <summary>MoveTo Destination storage does not exist.</summary>
        internal const string @MoveToDestNotExist = "MoveToDestNotExist";
        /// <summary>IStorage/IStream::MoveTo not supported.</summary>
        internal const string @MoveToNYI = "MoveToNYI";
        /// <summary>'{0}' name is already in use.</summary>
        internal const string @NameAlreadyInUse = "NameAlreadyInUse";
        /// <summary>'{0}' cannot contain the path delimiter: '{1}'.</summary>
        internal const string @NameCanNotHaveDelimiter = "NameCanNotHaveDelimiter";
        /// <summary>Failed call to '{0}'.</summary>
        internal const string @NamedAPIFailure = "NamedAPIFailure";
        /// <summary>Name table data is corrupt in data storage.</summary>
        internal const string @NameTableCorruptStg = "NameTableCorruptStg";
        /// <summary>Name table data is corrupt in memory.</summary>
        internal const string @NameTableCorruptMem = "NameTableCorruptMem";
        /// <summary>Name table cannot be read by this version of the program.</summary>
        internal const string @NameTableVersionMismatchRead = "NameTableVersionMismatchRead";
        /// <summary>Name table cannot be updated by this version of the program.</summary>
        internal const string @NameTableVersionMismatchWrite = "NameTableVersionMismatchWrite";
        /// <summary>This feature is not supported.</summary>
        internal const string @NYIDefault = "NYIDefault";
        /// <summary>Path string cannot include an empty element.</summary>
        internal const string @PathHasEmptyElement = "PathHasEmptyElement";
        /// <summary>Count of bytes to read cannot be negative.</summary>
        internal const string @ReadCountNegative = "ReadCountNegative";
        /// <summary>Cannot seek to given position.</summary>
        internal const string @SeekFailed = "SeekFailed";
        /// <summary>Cannot set seek pointer to a negative position.</summary>
        internal const string @SeekNegative = "SeekNegative";
        /// <summary>SeekOrigin value is not valid.</summary>
        internal const string @SeekOriginInvalid = "SeekOriginInvalid";
        /// <summary>This combination of flags is not supported.</summary>
        internal const string @StorageFlagsUnsupported = "StorageFlagsUnsupported";
        /// <summary>Storage already exists.</summary>
        internal const string @StorageAlreadyExist = "StorageAlreadyExist";
        /// <summary>Stream already exists.</summary>
        internal const string @StreamAlreadyExist = "StreamAlreadyExist";
        /// <summary>StorageInfo object was disposed.</summary>
        internal const string @StorageInfoDisposed = "StorageInfoDisposed";
        /// <summary>Storage does not exist.</summary>
        internal const string @StorageNotExist = "StorageNotExist";
        /// <summary>StorageRoot object was disposed.</summary>
        internal const string @StorageRootDisposed = "StorageRootDisposed";
        /// <summary>StreamInfo object was disposed.</summary>
        internal const string @StreamInfoDisposed = "StreamInfoDisposed";
        /// <summary>Stream length cannot be negative.</summary>
        internal const string @StreamLengthNegative = "StreamLengthNegative";
        /// <summary>Cannot perform this function on a stream that does not exist.</summary>
        internal const string @StreamNotExist = "StreamNotExist";
        /// <summary>Stream name cannot be '{0}'.</summary>
        internal const string @StreamNameNotValid = "StreamNameNotValid";
        /// <summary>Stream time stamp not implemented in OLE32 implementation of Compound Files.</summary>
        internal const string @StreamTimeStampNotImplemented = "StreamTimeStampNotImplemented";
        /// <summary>'{0}' cannot start with the reserved character range 0x01-0x1F.</summary>
        internal const string @StringCanNotBeReservedName = "StringCanNotBeReservedName";
        /// <summary>Requested time stamp is not available.</summary>
        internal const string @TimeStampNotAvailable = "TimeStampNotAvailable";
        /// <summary>Transform label name is already in use.</summary>
        internal const string @TransformLabelInUse = "TransformLabelInUse";
        /// <summary>Data space transform stack includes undefined transform labels.</summary>
        internal const string @TransformLabelUndefined = "TransformLabelUndefined";
        /// <summary>Transform object type is required to have a constructor which takes a TransformEnvironment object.</summary>
        internal const string @TransformObjectConstructorParam = "TransformObjectConstructorParam";
        /// <summary>Transform object type is required to implement IDataTransform interface.</summary>
        internal const string @TransformObjectImplementIDataTransform = "TransformObjectImplementIDataTransform";
        /// <summary>Stream transformation failed due to uninitialized data transform objects.</summary>
        internal const string @TransformObjectInitFailed = "TransformObjectInitFailed";
        /// <summary>Transform identifier type is not supported.</summary>
        internal const string @TransformTypeUnsupported = "TransformTypeUnsupported";
        /// <summary>Transform stack must have at least one transform.</summary>
        internal const string @TransformStackValid = "TransformStackValid";
        /// <summary>Cannot create package on stream.</summary>
        internal const string @UnableToCreateOnStream = "UnableToCreateOnStream";
        /// <summary>Cannot create data storage.</summary>
        internal const string @UnableToCreateStorage = "UnableToCreateStorage";
        /// <summary>Cannot create data stream.</summary>
        internal const string @UnableToCreateStream = "UnableToCreateStream";
        /// <summary>Cannot open data stream.</summary>
        internal const string @UnableToOpenStream = "UnableToOpenStream";
        /// <summary>Encountered unsupported type of storage element when building storage enumerator.</summary>
        internal const string @UnsupportedTypeEncounteredWhenBuildingStgEnum = "UnsupportedTypeEncounteredWhenBuildingStgEnum";
        /// <summary>Cannot write all data as specified.</summary>
        internal const string @WriteFailure = "WriteFailure";
        /// <summary>Write-only mode is not supported.</summary>
        internal const string @WriteOnlyUnsupported = "WriteOnlyUnsupported";
        /// <summary>Cannot write a negative number of bytes.</summary>
        internal const string @WriteSizeNegative = "WriteSizeNegative";
        /// <summary>Object metadata stream in the package is corrupt and the content is not valid.</summary>
        internal const string @CFM_CorruptMetadataStream = "CFM_CorruptMetadataStream";
        /// <summary>Object metadata stream in the package is corrupt and the root tag is not valid.</summary>
        internal const string @CFM_CorruptMetadataStream_Root = "CFM_CorruptMetadataStream_Root";
        /// <summary>Object metadata stream in the package is corrupt with duplicated key tags.</summary>
        internal const string @CFM_CorruptMetadataStream_DuplicateKey = "CFM_CorruptMetadataStream_DuplicateKey";
        /// <summary>Object used as metadata key must be an instance of the CompoundFileMetadataKey class.</summary>
        internal const string @CFM_ObjectMustBeCompoundFileMetadataKey = "CFM_ObjectMustBeCompoundFileMetadataKey";
        /// <summary>Cannot perform this operation when the package is in read-only mode.</summary>
        internal const string @CFM_ReadOnlyContainer = "CFM_ReadOnlyContainer";
        /// <summary>Failed to read a stream type table - the data appears to be a different format.</summary>
        internal const string @CFM_TypeTableFormat = "CFM_TypeTableFormat";
        /// <summary>Unicode character is not valid.</summary>
        internal const string @CFM_UnicodeCharInvalid = "CFM_UnicodeCharInvalid";
        /// <summary>Only strings can be used as value.</summary>
        internal const string @CFM_ValueMustBeString = "CFM_ValueMustBeString";
        /// <summary>XML character is not valid.</summary>
        internal const string @CFM_XMLCharInvalid = "CFM_XMLCharInvalid";
        /// <summary>Cannot compare different types.</summary>
        internal const string @CanNotCompareDiffTypes = "CanNotCompareDiffTypes";
        /// <summary>CompoundFileReference: Corrupted CompoundFileReference.</summary>
        internal const string @CFRCorrupt = "CFRCorrupt";
        /// <summary>CompoundFileReference: Corrupted CompoundFileReference - multiple stream components found.</summary>
        internal const string @CFRCorruptMultiStream = "CFRCorruptMultiStream";
        /// <summary>CompoundFileReference: Corrupted CompoundFileReference - storage component cannot follow stream component.</summary>
        internal const string @CFRCorruptStgFollowStm = "CFRCorruptStgFollowStm";
        /// <summary>Cannot have leading path delimiter.</summary>
        internal const string @DelimiterLeading = "DelimiterLeading";
        /// <summary>Cannot have trailing path delimiter.</summary>
        internal const string @DelimiterTrailing = "DelimiterTrailing";
        /// <summary>Offset must be greater than or equal to zero.</summary>
        internal const string @OffsetNegative = "OffsetNegative";
        /// <summary>Unrecognized reference component type.</summary>
        internal const string @UnknownReferenceComponentType = "UnknownReferenceComponentType";
        /// <summary>Cannot serialize unknown CompoundFileReference subclass.</summary>
        internal const string @UnknownReferenceSerialize = "UnknownReferenceSerialize";
        /// <summary>CompoundFileReference: malformed path encountered.</summary>
        internal const string @MalformedCompoundFilePath = "MalformedCompoundFilePath";
        /// <summary>Stream length cannot be negative.</summary>
        internal const string @CannotMakeStreamLengthNegative = "CannotMakeStreamLengthNegative";
        /// <summary>Stream operation failed because stream is corrupted.</summary>
        internal const string @CorruptStream = "CorruptStream";
        /// <summary>Stream does not support Length property.</summary>
        internal const string @LengthNotSupported = "LengthNotSupported";
        /// <summary>Buffer too small to hold results of Read.</summary>
        internal const string @ReadBufferTooSmall = "ReadBufferTooSmall";
        /// <summary>Stream does not support reading.</summary>
        internal const string @ReadNotSupported = "ReadNotSupported";
        /// <summary>Stream does not support Seek.</summary>
        internal const string @SeekNotSupported = "SeekNotSupported";
        /// <summary>Stream does not support SetLength.</summary>
        internal const string @SetLengthNotSupported = "SetLengthNotSupported";
        /// <summary>Stream does not support setting the Position property.</summary>
        internal const string @SetPositionNotSupported = "SetPositionNotSupported";
        /// <summary>Negative stream position not supported.</summary>
        internal const string @StreamPositionNegative = "StreamPositionNegative";
        /// <summary>Cannot change Transform parameters after the transform is initialized.</summary>
        internal const string @TransformParametersFixed = "TransformParametersFixed";
        /// <summary>Buffer of bytes to be written is too small.</summary>
        internal const string @WriteBufferTooSmall = "WriteBufferTooSmall";
        /// <summary>Count of bytes to write cannot be negative.</summary>
        internal const string @WriteCountNegative = "WriteCountNegative";
        /// <summary>Stream does not support writing.</summary>
        internal const string @WriteNotSupported = "WriteNotSupported";
        /// <summary>Compression requires ZLib library version {0}.</summary>
        internal const string @ZLibVersionError = "ZLibVersionError";
        /// <summary>Expected a VersionPair object.</summary>
        internal const string @ExpectedVersionPairObject = "ExpectedVersionPairObject";
        /// <summary>Major and minor version number components cannot be negative.</summary>
        internal const string @VersionNumberComponentNegative = "VersionNumberComponentNegative";
        /// <summary>Feature ID string cannot have zero length.</summary>
        internal const string @ZeroLengthFeatureID = "ZeroLengthFeatureID";
        /// <summary>Cannot find version stream.</summary>
        internal const string @VersionStreamMissing = "VersionStreamMissing";
        /// <summary>Cannot update version because of a version field size mismatch.</summary>
        internal const string @VersionUpdateFailure = "VersionUpdateFailure";
        /// <summary>Cannot remove signature from read-only file.</summary>
        internal const string @CannotRemoveSignatureFromReadOnlyFile = "CannotRemoveSignatureFromReadOnlyFile";
        /// <summary>Cannot change the RoutedEvent property while the RoutedEvent is being routed.</summary>
        internal const string @RoutedEventCannotChangeWhileRouting = "RoutedEventCannotChangeWhileRouting";
        /// <summary>Cannot sign read-only file.</summary>
        internal const string @CannotSignReadOnlyFile = "CannotSignReadOnlyFile";
        /// <summary>Cannot locate the selected digital certificate.</summary>
        internal const string @DigSigCannotLocateCertificate = "DigSigCannotLocateCertificate";
        /// <summary>Certificate error. Multiple certificates found with the same thumbprint.</summary>
        internal const string @DigSigDuplicateCertificate = "DigSigDuplicateCertificate";
        /// <summary>Digital Signature</summary>
        internal const string @CertSelectionDialogTitle = "CertSelectionDialogTitle";
        /// <summary>Select a certificate</summary>
        internal const string @CertSelectionDialogMessage = "CertSelectionDialogMessage";
        /// <summary>Duplicates not allowed - signature part already exists.</summary>
        internal const string @DuplicateSignature = "DuplicateSignature";
        /// <summary>Error parsing XML Signature.</summary>
        internal const string @XmlSignatureParseError = "XmlSignatureParseError";
        /// <summary>Required attribute '{0}' not found.</summary>
        internal const string @RequiredXmlAttributeMissing = "RequiredXmlAttributeMissing";
        /// <summary>Unexpected tag '{0}'.</summary>
        internal const string @UnexpectedXmlTag = "UnexpectedXmlTag";
        /// <summary>Required tag '{0}' not found.</summary>
        internal const string @RequiredTagNotFound = "RequiredTagNotFound";
        /// <summary>Required Package-specific Object tag is missing.</summary>
        internal const string @PackageSignatureObjectTagRequired = "PackageSignatureObjectTagRequired";
        /// <summary>Required Package-specific Reference tag is missing.</summary>
        internal const string @PackageSignatureReferenceTagRequired = "PackageSignatureReferenceTagRequired";
        /// <summary>Expected exactly one Package-specific Reference tag.</summary>
        internal const string @MoreThanOnePackageSpecificReference = "MoreThanOnePackageSpecificReference";
        /// <summary>Uri attribute in Reference tag must refer using fragment identifiers.</summary>
        internal const string @InvalidUriAttribute = "InvalidUriAttribute";
        /// <summary>Cannot countersign an unsigned package.</summary>
        internal const string @NoCounterSignUnsignedContainer = "NoCounterSignUnsignedContainer";
        /// <summary>Time format string is not valid.</summary>
        internal const string @BadSignatureTimeFormatString = "BadSignatureTimeFormatString";
        /// <summary>Signature structures are corrupted in this package.</summary>
        internal const string @PackageSignatureCorruption = "PackageSignatureCorruption";
        /// <summary>Unsupported hash algorithm specified.</summary>
        internal const string @UnsupportedHashAlgorithm = "UnsupportedHashAlgorithm";
        /// <summary>Relationship transform must be followed by an XML canonicalization transform.</summary>
        internal const string @RelationshipTransformNotFollowedByCanonicalizationTransform = "RelationshipTransformNotFollowedByCanonicalizationTransform";
        /// <summary>There must be at most one relationship transform specified for a given relationship part.</summary>
        internal const string @MultipleRelationshipTransformsFound = "MultipleRelationshipTransformsFound";
        /// <summary>Unsupported transform algorithm specified.</summary>
        internal const string @UnsupportedTransformAlgorithm = "UnsupportedTransformAlgorithm";
        /// <summary>Unsupported canonicalization method specified.</summary>
        internal const string @UnsupportedCanonicalizationMethod = "UnsupportedCanonicalizationMethod";
        /// <summary>Reusable hash algorithm must be specified.</summary>
        internal const string @HashAlgorithmMustBeReusable = "HashAlgorithmMustBeReusable";
        /// <summary>Malformed Part URI in Reference tag.</summary>
        internal const string @PartReferenceUriMalformed = "PartReferenceUriMalformed";
        /// <summary>Relationship was found to the signature origin but the part is missing. Package signature structures are corrupted.</summary>
        internal const string @SignatureOriginNotFound = "SignatureOriginNotFound";
        /// <summary>Multiple signature origin relationships found.</summary>
        internal const string @MultipleSignatureOrigins = "MultipleSignatureOrigins";
        /// <summary>Must specify an item to sign.</summary>
        internal const string @NothingToSign = "NothingToSign";
        /// <summary>Signature Identifier cannot be empty.</summary>
        internal const string @EmptySignatureId = "EmptySignatureId";
        /// <summary>Signature was deleted.</summary>
        internal const string @SignatureDeleted = "SignatureDeleted";
        /// <summary>Specified object ID conflicts with predefined Package Object ID.</summary>
        internal const string @SignaturePackageObjectTagMustBeUnique = "SignaturePackageObjectTagMustBeUnique";
        /// <summary>Specified reference object conflicts with predefined Package specific reference.</summary>
        internal const string @PackageSpecificReferenceTagMustBeUnique = "PackageSpecificReferenceTagMustBeUnique";
        /// <summary>Object identifiers must be unique within the same signature.</summary>
        internal const string @SignatureObjectIdMustBeUnique = "SignatureObjectIdMustBeUnique";
        /// <summary>Can only countersign parts with Digital Signature ContentType.</summary>
        internal const string @CanOnlyCounterSignSignatureParts = "CanOnlyCounterSignSignatureParts";
        /// <summary>Certificate part is not of the correct type.</summary>
        internal const string @CertificatePartContentTypeMismatch = "CertificatePartContentTypeMismatch";
        /// <summary>Signing certificate must be of type DSA or RSA.</summary>
        internal const string @CertificateKeyTypeNotSupported = "CertificateKeyTypeNotSupported";
        /// <summary>Specified part to sign does not exist.</summary>
        internal const string @PartToSignMissing = "PartToSignMissing";
        /// <summary>Duplicate object ID found. IDs must be unique within the signature XML.</summary>
        internal const string @DuplicateObjectId = "DuplicateObjectId";
        /// <summary>Caller-supplied parameter to callback function is not of expected type.</summary>
        internal const string @CallbackParameterInvalid = "CallbackParameterInvalid";
        /// <summary>Cannot change publish license after the rights management transform settings are fixed.</summary>
        internal const string @CannotChangePublishLicense = "CannotChangePublishLicense";
        /// <summary>Cannot change CryptoProvider after the rights management transform settings are fixed.</summary>
        internal const string @CannotChangeCryptoProvider = "CannotChangeCryptoProvider";
        /// <summary>Length prefix specifies {0} characters, which exceeds the maximum of {1} characters.</summary>
        internal const string @ExcessiveLengthPrefix = "ExcessiveLengthPrefix";
        /// <summary>OLE property ID {0} cannot be read (error {1}).</summary>
        internal const string @GetOlePropertyFailed = "GetOlePropertyFailed";
        /// <summary>Authentication type string (the part before the colon) is not valid in user ID '{0}'.</summary>
        internal const string @InvalidAuthenticationTypeString = "InvalidAuthenticationTypeString";
        /// <summary>'{0}' document property type is not valid.</summary>
        internal const string @InvalidDocumentPropertyType = "InvalidDocumentPropertyType";
        /// <summary>'{0}' document property variant type is not valid.</summary>
        internal const string @InvalidDocumentPropertyVariantType = "InvalidDocumentPropertyVariantType";
        /// <summary>User ID in use license stream is not of the form "authenticationType:userName".</summary>
        internal const string @InvalidTypePrefixedUserName = "InvalidTypePrefixedUserName";
        /// <summary>Feature name in the transform's primary stream is '{0}', but expected '{1}'.</summary>
        internal const string @InvalidTransformFeatureName = "InvalidTransformFeatureName";
        /// <summary>Document does not contain a package.</summary>
        internal const string @PackageNotFound = "PackageNotFound";
        /// <summary>File does not contain a stream to hold the publish license.</summary>
        internal const string @NoPublishLicenseStream = "NoPublishLicenseStream";
        /// <summary>File does not contain a storage to hold use licenses.</summary>
        internal const string @NoUseLicenseStorage = "NoUseLicenseStorage";
        /// <summary>File contains data in format version {0}, but the software can only read that data in format version {1} or lower.</summary>
        internal const string @ReaderVersionError = "ReaderVersionError";
        /// <summary>Document's publish license stream is corrupted.</summary>
        internal const string @PublishLicenseStreamCorrupt = "PublishLicenseStreamCorrupt";
        /// <summary>Document does not contain a publish license.</summary>
        internal const string @PublishLicenseNotFound = "PublishLicenseNotFound";
        /// <summary>Document does not contain any rights management-protected streams.</summary>
        internal const string @RightsManagementEncryptionTransformNotFound = "RightsManagementEncryptionTransformNotFound";
        /// <summary>Document contains multiple Rights Management Encryption Transforms.</summary>
        internal const string @MultipleRightsManagementEncryptionTransformFound = "MultipleRightsManagementEncryptionTransformFound";
        /// <summary>The stream on which the encrypted package is created must have read/write access.</summary>
        internal const string @StreamNeedsReadWriteAccess = "StreamNeedsReadWriteAccess";
        /// <summary>Cannot perform stream operation because CryptoProvider is not set to allow decryption.</summary>
        internal const string @CryptoProviderCanNotDecrypt = "CryptoProviderCanNotDecrypt";
        /// <summary>Only cryptographic providers based on a block cipher are supported.</summary>
        internal const string @CryptoProviderCanNotMergeBlocks = "CryptoProviderCanNotMergeBlocks";
        /// <summary>EncryptedPackageEnvelope object was disposed.</summary>
        internal const string @EncryptedPackageEnvelopeDisposed = "EncryptedPackageEnvelopeDisposed";
        /// <summary>CryptoProvider object was disposed.</summary>
        internal const string @CryptoProviderDisposed = "CryptoProviderDisposed";
        /// <summary>File contains data in format version {0}, but the software can only update that data in format version {1} or lower.</summary>
        internal const string @UpdaterVersionError = "UpdaterVersionError";
        /// <summary>The dictionary is read-only.</summary>
        internal const string @DictionaryIsReadOnly = "DictionaryIsReadOnly";
        /// <summary>The CryptoProvider cannot encrypt or decrypt.</summary>
        internal const string @CryptoProviderIsNotReady = "CryptoProviderIsNotReady";
        /// <summary>One of the document's use licenses is corrupted.</summary>
        internal const string @UseLicenseStreamCorrupt = "UseLicenseStreamCorrupt";
        /// <summary>Encrypted data stream is corrupted.</summary>
        internal const string @EncryptedDataStreamCorrupt = "EncryptedDataStreamCorrupt";
        /// <summary>Unrecognized document property: FMTID = '{0}', property ID = '{1}'.</summary>
        internal const string @UnknownDocumentProperty = "UnknownDocumentProperty";
        /// <summary>'{0}' document property in property set '{1}' is of incorrect variant type '{2}'. Expected type '{3}'.</summary>
        internal const string @WrongDocumentPropertyVariantType = "WrongDocumentPropertyVariantType";
        /// <summary>User is not activated.</summary>
        internal const string @UserIsNotActivated = "UserIsNotActivated";
        /// <summary>User does not have a client licensor certificate.</summary>
        internal const string @UserHasNoClientLicensorCert = "UserHasNoClientLicensorCert";
        /// <summary>Encryption right is not granted.</summary>
        internal const string @EncryptionRightIsNotGranted = "EncryptionRightIsNotGranted";
        /// <summary>Decryption right is not granted.</summary>
        internal const string @DecryptionRightIsNotGranted = "DecryptionRightIsNotGranted";
        /// <summary>CryptoProvider does not have privileges required for decryption of the PublishLicense.</summary>
        internal const string @NoPrivilegesForPublishLicenseDecryption = "NoPrivilegesForPublishLicenseDecryption";
        /// <summary>Signed Publish License is not valid.</summary>
        internal const string @InvalidPublishLicense = "InvalidPublishLicense";
        /// <summary>Variable-length header in publish license stream is {0} bytes, which exceeds the maximum length of {1} bytes.</summary>
        internal const string @PublishLicenseStreamHeaderTooLong = "PublishLicenseStreamHeaderTooLong";
        /// <summary>User must be either Windows or Passport authenticated. Other authentication types are not allowed in this context.</summary>
        internal const string @OnlyPassportOrWindowsAuthenticatedUsersAreAllowed = "OnlyPassportOrWindowsAuthenticatedUsersAreAllowed";
        /// <summary>Rights management operation failed.</summary>
        internal const string @RmExceptionGenericMessage = "RmExceptionGenericMessage";
        /// <summary>License is not valid.</summary>
        internal const string @RmExceptionInvalidLicense = "RmExceptionInvalidLicense";
        /// <summary>Information not found.</summary>
        internal const string @RmExceptionInfoNotInLicense = "RmExceptionInfoNotInLicense";
        /// <summary>License signature is not valid.</summary>
        internal const string @RmExceptionInvalidLicenseSignature = "RmExceptionInvalidLicenseSignature";
        /// <summary>Encryption not permitted.</summary>
        internal const string @RmExceptionEncryptionNotPermitted = "RmExceptionEncryptionNotPermitted";
        /// <summary>Right not granted.</summary>
        internal const string @RmExceptionRightNotGranted = "RmExceptionRightNotGranted";
        /// <summary>Version is not valid.</summary>
        internal const string @RmExceptionInvalidVersion = "RmExceptionInvalidVersion";
        /// <summary>Encoding type is not valid.</summary>
        internal const string @RmExceptionInvalidEncodingType = "RmExceptionInvalidEncodingType";
        /// <summary>Numerical value is not valid.</summary>
        internal const string @RmExceptionInvalidNumericalValue = "RmExceptionInvalidNumericalValue";
        /// <summary>Algorithm type is not valid.</summary>
        internal const string @RmExceptionInvalidAlgorithmType = "RmExceptionInvalidAlgorithmType";
        /// <summary>Environment not loaded.</summary>
        internal const string @RmExceptionEnvironmentNotLoaded = "RmExceptionEnvironmentNotLoaded";
        /// <summary>Cannot load environment.</summary>
        internal const string @RmExceptionEnvironmentCannotLoad = "RmExceptionEnvironmentCannotLoad";
        /// <summary>Cannot load more than one environment.</summary>
        internal const string @RmExceptionTooManyLoadedEnvironments = "RmExceptionTooManyLoadedEnvironments";
        /// <summary>Incompatible objects.</summary>
        internal const string @RmExceptionIncompatibleObjects = "RmExceptionIncompatibleObjects";
        /// <summary>Library fail.</summary>
        internal const string @RmExceptionLibraryFail = "RmExceptionLibraryFail";
        /// <summary>Enabling principal failure.</summary>
        internal const string @RmExceptionEnablingPrincipalFailure = "RmExceptionEnablingPrincipalFailure";
        /// <summary>Information not found.</summary>
        internal const string @RmExceptionInfoNotPresent = "RmExceptionInfoNotPresent";
        /// <summary>Get information query is not valid.</summary>
        internal const string @RmExceptionBadGetInfoQuery = "RmExceptionBadGetInfoQuery";
        /// <summary>Key type not supported.</summary>
        internal const string @RmExceptionKeyTypeUnsupported = "RmExceptionKeyTypeUnsupported";
        /// <summary>Crypto operation not supported.</summary>
        internal const string @RmExceptionCryptoOperationUnsupported = "RmExceptionCryptoOperationUnsupported";
        /// <summary>Clock rollback detected.</summary>
        internal const string @RmExceptionClockRollbackDetected = "RmExceptionClockRollbackDetected";
        /// <summary>Query reports no results.</summary>
        internal const string @RmExceptionQueryReportsNoResults = "RmExceptionQueryReportsNoResults";
        /// <summary>Unexpected exception.</summary>
        internal const string @RmExceptionUnexpectedException = "RmExceptionUnexpectedException";
        /// <summary>Binding validity time violated.</summary>
        internal const string @RmExceptionBindValidityTimeViolated = "RmExceptionBindValidityTimeViolated";
        /// <summary>Broken certificate chain.</summary>
        internal const string @RmExceptionBrokenCertChain = "RmExceptionBrokenCertChain";
        /// <summary>Binding policy violation.</summary>
        internal const string @RmExceptionBindPolicyViolation = "RmExceptionBindPolicyViolation";
        /// <summary>Manifest policy violation.</summary>
        internal const string @RmExceptionManifestPolicyViolation = "RmExceptionManifestPolicyViolation";
        /// <summary>License has been revoked.</summary>
        internal const string @RmExceptionBindRevokedLicense = "RmExceptionBindRevokedLicense";
        /// <summary>Issuer has been revoked.</summary>
        internal const string @RmExceptionBindRevokedIssuer = "RmExceptionBindRevokedIssuer";
        /// <summary>Principal has been revoked.</summary>
        internal const string @RmExceptionBindRevokedPrincipal = "RmExceptionBindRevokedPrincipal";
        /// <summary>Resource has been revoked.</summary>
        internal const string @RmExceptionBindRevokedResource = "RmExceptionBindRevokedResource";
        /// <summary>Module has been revoked.</summary>
        internal const string @RmExceptionBindRevokedModule = "RmExceptionBindRevokedModule";
        /// <summary>Binding content not in the End Use License.</summary>
        internal const string @RmExceptionBindContentNotInEndUseLicense = "RmExceptionBindContentNotInEndUseLicense";
        /// <summary>Binding access principal is not enabling.</summary>
        internal const string @RmExceptionBindAccessPrincipalNotEnabling = "RmExceptionBindAccessPrincipalNotEnabling";
        /// <summary>Binding access unsatisfied.</summary>
        internal const string @RmExceptionBindAccessUnsatisfied = "RmExceptionBindAccessUnsatisfied";
        /// <summary>Principal provided for binding is missing.</summary>
        internal const string @RmExceptionBindIndicatedPrincipalMissing = "RmExceptionBindIndicatedPrincipalMissing";
        /// <summary>Machine is not found in group identity certificate.</summary>
        internal const string @RmExceptionBindMachineNotFoundInGroupIdentity = "RmExceptionBindMachineNotFoundInGroupIdentity";
        /// <summary>Unsupported library plug-in.</summary>
        internal const string @RmExceptionLibraryUnsupportedPlugIn = "RmExceptionLibraryUnsupportedPlugIn";
        /// <summary>Binding revocation list is stale.</summary>
        internal const string @RmExceptionBindRevocationListStale = "RmExceptionBindRevocationListStale";
        /// <summary>Binding missing application revocation list.</summary>
        internal const string @RmExceptionBindNoApplicableRevocationList = "RmExceptionBindNoApplicableRevocationList";
        /// <summary>Handle is not valid.</summary>
        internal const string @RmExceptionInvalidHandle = "RmExceptionInvalidHandle";
        /// <summary>Binding time interval is violated.</summary>
        internal const string @RmExceptionBindIntervalTimeViolated = "RmExceptionBindIntervalTimeViolated";
        /// <summary>Binding cannot find a satisfied rights group.</summary>
        internal const string @RmExceptionBindNoSatisfiedRightsGroup = "RmExceptionBindNoSatisfiedRightsGroup";
        /// <summary>Cannot find content specified for binding.</summary>
        internal const string @RmExceptionBindSpecifiedWorkMissing = "RmExceptionBindSpecifiedWorkMissing";
        /// <summary>No more data.</summary>
        internal const string @RmExceptionNoMoreData = "RmExceptionNoMoreData";
        /// <summary>License acquisition failed.</summary>
        internal const string @RmExceptionLicenseAcquisitionFailed = "RmExceptionLicenseAcquisitionFailed";
        /// <summary>ID mismatch.</summary>
        internal const string @RmExceptionIdMismatch = "RmExceptionIdMismatch";
        /// <summary>Cannot have more than one certificate.</summary>
        internal const string @RmExceptionTooManyCertificates = "RmExceptionTooManyCertificates";
        /// <summary>Distribution Point URL was not set.</summary>
        internal const string @RmExceptionNoDistributionPointUrlFound = "RmExceptionNoDistributionPointUrlFound";
        /// <summary>Rights management server transaction already in progress.</summary>
        internal const string @RmExceptionAlreadyInProgress = "RmExceptionAlreadyInProgress";
        /// <summary>Group identity not set.</summary>
        internal const string @RmExceptionGroupIdentityNotSet = "RmExceptionGroupIdentityNotSet";
        /// <summary>Record not found.</summary>
        internal const string @RmExceptionRecordNotFound = "RmExceptionRecordNotFound";
        /// <summary>Connection failed.</summary>
        internal const string @RmExceptionNoConnect = "RmExceptionNoConnect";
        /// <summary>License not found.</summary>
        internal const string @RmExceptionNoLicense = "RmExceptionNoLicense";
        /// <summary>Machine must be activated.</summary>
        internal const string @RmExceptionNeedsMachineActivation = "RmExceptionNeedsMachineActivation";
        /// <summary>User identity must be activated.</summary>
        internal const string @RmExceptionNeedsGroupIdentityActivation = "RmExceptionNeedsGroupIdentityActivation";
        /// <summary>Activation failed.</summary>
        internal const string @RmExceptionActivationFailed = "RmExceptionActivationFailed";
        /// <summary>Command interrupted.</summary>
        internal const string @RmExceptionAborted = "RmExceptionAborted";
        /// <summary>Transaction quota exceeded.</summary>
        internal const string @RmExceptionOutOfQuota = "RmExceptionOutOfQuota";
        /// <summary>Authentication failed.</summary>
        internal const string @RmExceptionAuthenticationFailed = "RmExceptionAuthenticationFailed";
        /// <summary>Server side error.</summary>
        internal const string @RmExceptionServerError = "RmExceptionServerError";
        /// <summary>Installation failed.</summary>
        internal const string @RmExceptionInstallationFailed = "RmExceptionInstallationFailed";
        /// <summary>Hardware ID corrupted.</summary>
        internal const string @RmExceptionHidCorrupted = "RmExceptionHidCorrupted";
        /// <summary>Server response is not valid.</summary>
        internal const string @RmExceptionInvalidServerResponse = "RmExceptionInvalidServerResponse";
        /// <summary>Service not found.</summary>
        internal const string @RmExceptionServiceNotFound = "RmExceptionServiceNotFound";
        /// <summary>Use default.</summary>
        internal const string @RmExceptionUseDefault = "RmExceptionUseDefault";
        /// <summary>Server not found.</summary>
        internal const string @RmExceptionServerNotFound = "RmExceptionServerNotFound";
        /// <summary>E-mail address is not valid.</summary>
        internal const string @RmExceptionInvalidEmail = "RmExceptionInvalidEmail";
        /// <summary>License validity time violation.</summary>
        internal const string @RmExceptionValidityTimeViolation = "RmExceptionValidityTimeViolation";
        /// <summary>Outdated module.</summary>
        internal const string @RmExceptionOutdatedModule = "RmExceptionOutdatedModule";
        /// <summary>Service moved.</summary>
        internal const string @RmExceptionServiceMoved = "RmExceptionServiceMoved";
        /// <summary>Service gone.</summary>
        internal const string @RmExceptionServiceGone = "RmExceptionServiceGone";
        /// <summary>Ad entry not found.</summary>
        internal const string @RmExceptionAdEntryNotFound = "RmExceptionAdEntryNotFound";
        /// <summary>Not a certificate chain.</summary>
        internal const string @RmExceptionNotAChain = "RmExceptionNotAChain";
        /// <summary>Rights management server denied request.</summary>
        internal const string @RmExceptionRequestDenied = "RmExceptionRequestDenied";
        /// <summary>Not set.</summary>
        internal const string @RmExceptionNotSet = "RmExceptionNotSet";
        /// <summary>Metadata not set.</summary>
        internal const string @RmExceptionMetadataNotSet = "RmExceptionMetadataNotSet";
        /// <summary>Revocation information not set.</summary>
        internal const string @RmExceptionRevocationInfoNotSet = "RmExceptionRevocationInfoNotSet";
        /// <summary>Time information is not valid.</summary>
        internal const string @RmExceptionInvalidTimeInfo = "RmExceptionInvalidTimeInfo";
        /// <summary>Right not set.</summary>
        internal const string @RmExceptionRightNotSet = "RmExceptionRightNotSet";
        /// <summary>License binding to Windows Identity failed (NTLM bind failure).</summary>
        internal const string @RmExceptionLicenseBindingToWindowsIdentityFailed = "RmExceptionLicenseBindingToWindowsIdentityFailed";
        /// <summary>Issuance license template is not valid because of incorrectly formatted string.</summary>
        internal const string @RmExceptionInvalidIssuanceLicenseTemplate = "RmExceptionInvalidIssuanceLicenseTemplate";
        /// <summary>Key size length is not valid.</summary>
        internal const string @RmExceptionInvalidKeyLength = "RmExceptionInvalidKeyLength";
        /// <summary>Expired official Publish License template.</summary>
        internal const string @RmExceptionExpiredOfficialIssuanceLicenseTemplate = "RmExceptionExpiredOfficialIssuanceLicenseTemplate";
        /// <summary>Client Licensor Certificate is not valid.</summary>
        internal const string @RmExceptionInvalidClientLicensorCertificate = "RmExceptionInvalidClientLicensorCertificate";
        /// <summary>Hardware ID is not valid.</summary>
        internal const string @RmExceptionHidInvalid = "RmExceptionHidInvalid";
        /// <summary>E-mail not verified.</summary>
        internal const string @RmExceptionEmailNotVerified = "RmExceptionEmailNotVerified";
        /// <summary>Debugger detected.</summary>
        internal const string @RmExceptionDebuggerDetected = "RmExceptionDebuggerDetected";
        /// <summary>Lockbox type is not valid.</summary>
        internal const string @RmExceptionInvalidLockboxType = "RmExceptionInvalidLockboxType";
        /// <summary>Lockbox path is not valid.</summary>
        internal const string @RmExceptionInvalidLockboxPath = "RmExceptionInvalidLockboxPath";
        /// <summary>Registry path is not valid.</summary>
        internal const string @RmExceptionInvalidRegistryPath = "RmExceptionInvalidRegistryPath";
        /// <summary>No AES Crypto provider found.</summary>
        internal const string @RmExceptionNoAesCryptoProvider = "RmExceptionNoAesCryptoProvider";
        /// <summary>Global option is already set.</summary>
        internal const string @RmExceptionGlobalOptionAlreadySet = "RmExceptionGlobalOptionAlreadySet";
        /// <summary>Owner's license not found.</summary>
        internal const string @RmExceptionOwnerLicenseNotFound = "RmExceptionOwnerLicenseNotFound";
        /// <summary>Archive file cannot be size 0.</summary>
        internal const string @ZipZeroSizeFileIsNotValidArchive = "ZipZeroSizeFileIsNotValidArchive";
        /// <summary>Cannot perform a write operation in read-only mode.</summary>
        internal const string @CanNotWriteInReadOnlyMode = "CanNotWriteInReadOnlyMode";
        /// <summary>Cannot perform a read operation in write-only mode.</summary>
        internal const string @CanNotReadInWriteOnlyMode = "CanNotReadInWriteOnlyMode";
        /// <summary>Cannot perform a read/write operation in write-only or read-only modes.</summary>
        internal const string @CanNotReadWriteInReadOnlyWriteOnlyMode = "CanNotReadWriteInReadOnlyWriteOnlyMode";
        /// <summary>Cannot create file because the specified file name is already in use.</summary>
        internal const string @AttemptedToCreateDuplicateFileName = "AttemptedToCreateDuplicateFileName";
        /// <summary>Cannot find specified file.</summary>
        internal const string @FileDoesNotExists = "FileDoesNotExists";
        /// <summary>Truncate and Append FileModes are not supported.</summary>
        internal const string @TruncateAppendModesNotSupported = "TruncateAppendModesNotSupported";
        /// <summary>Only FileShare.Read and FileShare.None are supported.</summary>
        internal const string @OnlyFileShareReadAndFileShareNoneSupported = "OnlyFileShareReadAndFileShareNoneSupported";
        /// <summary>Cannot read data from stream that does not support reading.</summary>
        internal const string @CanNotReadDataFromStreamWhichDoesNotSupportReading = "CanNotReadDataFromStreamWhichDoesNotSupportReading";
        /// <summary>Cannot write data to stream that does not support writing.</summary>
        internal const string @CanNotWriteDataToStreamWhichDoesNotSupportWriting = "CanNotWriteDataToStreamWhichDoesNotSupportWriting";
        /// <summary>Cannot operate on stream that does not support seeking.</summary>
        internal const string @CanNotOperateOnStreamWhichDoesNotSupportSeeking = "CanNotOperateOnStreamWhichDoesNotSupportSeeking";
        /// <summary>Cannot get stream with FileMode.Create, FileMode.CreateNew, FileMode.Truncate, FileMode.Append when access is FileAccess.Read.</summary>
        internal const string @UnsupportedCombinationOfModeAccessShareStreaming = "UnsupportedCombinationOfModeAccessShareStreaming";
        /// <summary>File contains corrupted data.</summary>
        internal const string @CorruptedData = "CorruptedData";
        /// <summary>Multidisk ZIP format is not supported.</summary>
        internal const string @NotSupportedMultiDisk = "NotSupportedMultiDisk";
        /// <summary>ZIP archive was closed and disposed.</summary>
        internal const string @ZipArchiveDisposed = "ZipArchiveDisposed";
        /// <summary>ZIP file was closed, disposed, or deleted.</summary>
        internal const string @ZipFileItemDisposed = "ZipFileItemDisposed";
        /// <summary>ZIP archive contains unsupported data structures.</summary>
        internal const string @NotSupportedVersionNeededToExtract = "NotSupportedVersionNeededToExtract";
        /// <summary>ZIP archive contains data structures too large to fit in memory.</summary>
        internal const string @Zip64StructuresTooLarge = "Zip64StructuresTooLarge";
        /// <summary>ZIP archive contains unsupported encrypted data.</summary>
        internal const string @ZipNotSupportedEncryptedArchive = "ZipNotSupportedEncryptedArchive";
        /// <summary>ZIP archive contains unsupported signature data.</summary>
        internal const string @ZipNotSupportedSignedArchive = "ZipNotSupportedSignedArchive";
        /// <summary>ZIP archive contains data compressed using an unsupported algorithm.</summary>
        internal const string @ZipNotSupportedCompressionMethod = "ZipNotSupportedCompressionMethod";
        /// <summary>Compressed part has inconsistent data length.</summary>
        internal const string @CompressLengthMismatch = "CompressLengthMismatch";
        /// <summary>CreateNew is not a valid FileMode for a nonempty stream.</summary>
        internal const string @CreateNewOnNonEmptyStream = "CreateNewOnNonEmptyStream";
        /// <summary>Specified part does not exist in the package.</summary>
        internal const string @PartDoesNotExist = "PartDoesNotExist";
        /// <summary>Cannot add part for the specified URI because it is already in the package.</summary>
        internal const string @PartAlreadyExists = "PartAlreadyExists";
        /// <summary>Cannot add part to the package. Part names cannot be derived from another part name by appending segments to it.</summary>
        internal const string @PartNamePrefixExists = "PartNamePrefixExists";
        /// <summary>Cannot open package because FileMode or FileAccess value is not valid for the stream.</summary>
        internal const string @IncompatibleModeOrAccess = "IncompatibleModeOrAccess";
        /// <summary>Cannot be an absolute URI.</summary>
        internal const string @URIShouldNotBeAbsolute = "URIShouldNotBeAbsolute";
        /// <summary>Must have absolute URI.</summary>
        internal const string @UriShouldBeAbsolute = "UriShouldBeAbsolute";
        /// <summary>FileMode/FileAccess for Part.GetStream is not compatible with FileMode/FileAccess used to open the Package.</summary>
        internal const string @ContainerAndPartModeIncompatible = "ContainerAndPartModeIncompatible";
        /// <summary>Cannot get stream with FileMode.Create, FileMode.CreateNew, FileMode.Truncate, FileMode.Append when access is FileAccess.Read.</summary>
        internal const string @UnsupportedCombinationOfModeAccess = "UnsupportedCombinationOfModeAccess";
        /// <summary>Returned stream for the part is null.</summary>
        internal const string @NullStreamReturned = "NullStreamReturned";
        /// <summary>Package object was closed and disposed, so cannot carry out operations on this object or any stream opened on a part of this package.</summary>
        internal const string @ObjectDisposed = "ObjectDisposed";
        /// <summary>Cannot write to read-only stream.</summary>
        internal const string @ReadOnlyStream = "ReadOnlyStream";
        /// <summary>Cannot read from write-only stream.</summary>
        internal const string @WriteOnlyStream = "WriteOnlyStream";
        /// <summary>Cannot access part because parent package was closed.</summary>
        internal const string @ParentContainerClosed = "ParentContainerClosed";
        /// <summary>Part was deleted.</summary>
        internal const string @PackagePartDeleted = "PackagePartDeleted";
        /// <summary>PackageRelationship cannot target another PackageRelationship.</summary>
        internal const string @RelationshipToRelationshipIllegal = "RelationshipToRelationshipIllegal";
        /// <summary>PackageRelationship parts cannot have relationships to other parts.</summary>
        internal const string @RelationshipPartsCannotHaveRelationships = "RelationshipPartsCannotHaveRelationships";
        /// <summary>Incorrect content type for PackageRelationship part.</summary>
        internal const string @RelationshipPartIncorrectContentType = "RelationshipPartIncorrectContentType";
        /// <summary>PackageRelationship with specified ID does not exist at the Package level.</summary>
        internal const string @PackageRelationshipDoesNotExist = "PackageRelationshipDoesNotExist";
        /// <summary>PackageRelationship with specified ID does not exist for the source part.</summary>
        internal const string @PackagePartRelationshipDoesNotExist = "PackagePartRelationshipDoesNotExist";
        /// <summary>PackageRelationship target must be relative URI if TargetMode is Internal.</summary>
        internal const string @RelationshipTargetMustBeRelative = "RelationshipTargetMustBeRelative";
        /// <summary>Relationship tag requires attribute '{0}'.</summary>
        internal const string @RequiredRelationshipAttributeMissing = "RequiredRelationshipAttributeMissing";
        /// <summary>Relationship tag contains incorrect attribute.</summary>
        internal const string @RelationshipTagDoesntMatchSchema = "RelationshipTagDoesntMatchSchema";
        /// <summary>Relationships tag has extra attributes.</summary>
        internal const string @RelationshipsTagHasExtraAttributes = "RelationshipsTagHasExtraAttributes";
        /// <summary>Unrecognized tag found in Relationships XML.</summary>
        internal const string @UnknownTagEncountered = "UnknownTagEncountered";
        /// <summary>Relationships tag expected at root level.</summary>
        internal const string @ExpectedRelationshipsElementTag = "ExpectedRelationshipsElementTag";
        /// <summary>Relationships XML elements cannot specify attribute '{0}'.</summary>
        internal const string @InvalidXmlBaseAttributePresent = "InvalidXmlBaseAttributePresent";
        /// <summary>'{0}' ID conflicts with the ID of an existing relationship for the specified source.</summary>
        internal const string @NotAUniqueRelationshipId = "NotAUniqueRelationshipId";
        /// <summary>'{0}' ID is not a valid XSD ID.</summary>
        internal const string @NotAValidXmlIdString = "NotAValidXmlIdString";
        /// <summary>'{0}' attribute value is not valid.</summary>
        internal const string @InvalidValueForTheAttribute = "InvalidValueForTheAttribute";
        /// <summary>Relationship Type cannot contain only spaces or be empty.</summary>
        internal const string @InvalidRelationshipType = "InvalidRelationshipType";
        /// <summary>Part URI must start with a forward slash.</summary>
        internal const string @PartUriShouldStartWithForwardSlash = "PartUriShouldStartWithForwardSlash";
        /// <summary>Part URI cannot end with a forward slash.</summary>
        internal const string @PartUriShouldNotEndWithForwardSlash = "PartUriShouldNotEndWithForwardSlash";
        /// <summary>URI must contain pack:// scheme.</summary>
        internal const string @UriShouldBePackScheme = "UriShouldBePackScheme";
        /// <summary>Part URI is empty.</summary>
        internal const string @PartUriIsEmpty = "PartUriIsEmpty";
        /// <summary>Part URI is not valid per rules defined in the Open Packaging Conventions specification.</summary>
        internal const string @InvalidPartUri = "InvalidPartUri";
        /// <summary>PackageRelationship part URI is not expected.</summary>
        internal const string @RelationshipPartUriNotExpected = "RelationshipPartUriNotExpected";
        /// <summary>PackageRelationship part URI is expected.</summary>
        internal const string @RelationshipPartUriExpected = "RelationshipPartUriExpected";
        /// <summary>PackageRelationship part URI syntax is not valid.</summary>
        internal const string @NotAValidRelationshipPartUri = "NotAValidRelationshipPartUri";
        /// <summary>The 'fragment' parameter must start with a number sign.</summary>
        internal const string @FragmentMustStartWithHash = "FragmentMustStartWithHash";
        /// <summary>Part URI cannot contain a Fragment component.</summary>
        internal const string @PartUriCannotHaveAFragment = "PartUriCannotHaveAFragment";
        /// <summary>Part URI cannot start with two forward slashes.</summary>
        internal const string @PartUriShouldNotStartWithTwoForwardSlashes = "PartUriShouldNotStartWithTwoForwardSlashes";
        /// <summary>Package URI obtained from the pack URI cannot contain a Fragment.</summary>
        internal const string @InnerPackageUriHasFragment = "InnerPackageUriHasFragment";
        /// <summary>Cannot access Stream object because it was closed or disposed.</summary>
        internal const string @StreamObjectDisposed = "StreamObjectDisposed";
        /// <summary>GetContentTypeCore method cannot return null for the content type stream.</summary>
        internal const string @NullContentTypeProvided = "NullContentTypeProvided";
        /// <summary>PackagePart subclass must implement GetContentTypeCore method if passing a null value for the content type when PackagePart object is constructed.</summary>
        internal const string @GetContentTypeCoreNotImplemented = "GetContentTypeCoreNotImplemented";
        /// <summary>'{0}' tag requires attribute '{1}'.</summary>
        internal const string @RequiredAttributeMissing = "RequiredAttributeMissing";
        /// <summary>'{0}' tag requires a nonempty '{1}' attribute.</summary>
        internal const string @RequiredAttributeEmpty = "RequiredAttributeEmpty";
        /// <summary>Types tag has attributes not valid per the schema.</summary>
        internal const string @TypesTagHasExtraAttributes = "TypesTagHasExtraAttributes";
        /// <summary>Required Types tag not found.</summary>
        internal const string @TypesElementExpected = "TypesElementExpected";
        /// <summary>Content Types XML does not match schema.</summary>
        internal const string @TypesXmlDoesNotMatchSchema = "TypesXmlDoesNotMatchSchema";
        /// <summary>Default tag is not valid per the schema. Verify that attributes are correct.</summary>
        internal const string @DefaultTagDoesNotMatchSchema = "DefaultTagDoesNotMatchSchema";
        /// <summary>Override tag is not valid per the schema. Verify that attributes are correct.</summary>
        internal const string @OverrideTagDoesNotMatchSchema = "OverrideTagDoesNotMatchSchema";
        /// <summary>'{0}' element must be empty.</summary>
        internal const string @ElementIsNotEmptyElement = "ElementIsNotEmptyElement";
        /// <summary>Format error in package.</summary>
        internal const string @BadPackageFormat = "BadPackageFormat";
        /// <summary>Streaming mode is supported only for creating packages.</summary>
        internal const string @StreamingModeNotSupportedForConsumption = "StreamingModeNotSupportedForConsumption";
        /// <summary>Must have write-only access to produce a package in streaming mode.</summary>
        internal const string @StreamingPackageProductionImpliesWriteOnlyAccess = "StreamingPackageProductionImpliesWriteOnlyAccess";
        /// <summary>Cannot have concurrent write accesses on package being produced in streaming mode.</summary>
        internal const string @StreamingPackageProductionRequiresSingleWriter = "StreamingPackageProductionRequiresSingleWriter";
        /// <summary>'{0}' method can only be called on a package opened in streaming mode.</summary>
        internal const string @MethodAvailableOnlyInStreamingCreation = "MethodAvailableOnlyInStreamingCreation";
        /// <summary>Package.{0} is not supported in streaming production.</summary>
        internal const string @OperationIsNotSupportedInStreamingProduction = "OperationIsNotSupportedInStreamingProduction";
        /// <summary>Only write operations are supported in streaming production.</summary>
        internal const string @OnlyWriteOperationsAreSupportedInStreamingCreation = "OnlyWriteOperationsAreSupportedInStreamingCreation";
        /// <summary>Write-once semantics in streaming production precludes the use of '{0}'.</summary>
        internal const string @OperationViolatesWriteOnceSemantics = "OperationViolatesWriteOnceSemantics";
        /// <summary>Streaming consumption of packages not supported.</summary>
        internal const string @OnlyStreamingProductionIsSupported = "OnlyStreamingProductionIsSupported";
        /// <summary>Read or write operation references location outside the bounds of the buffer provided.</summary>
        internal const string @IOBufferOverflow = "IOBufferOverflow";
        /// <summary>Cannot change content of a read-only stream.</summary>
        internal const string @StreamDoesNotSupportWrite = "StreamDoesNotSupportWrite";
        /// <summary>Package has more than one Core Properties relationship.</summary>
        internal const string @MoreThanOneMetadataRelationships = "MoreThanOneMetadataRelationships";
        /// <summary>TargetMode for a Core Properties relationship must be 'Internal'.</summary>
        internal const string @NoExternalTargetForMetadataRelationship = "NoExternalTargetForMetadataRelationship";
        /// <summary>Unrecognized root element in Core Properties part.</summary>
        internal const string @CorePropertiesElementExpected = "CorePropertiesElementExpected";
        /// <summary>Core Properties part: core property elements can contain only text data.</summary>
        internal const string @NoStructuredContentInsideProperties = "NoStructuredContentInsideProperties";
        /// <summary>Unrecognized namespace in Core Properties part.</summary>
        internal const string @UnknownNamespaceInCorePropertiesPart = "UnknownNamespaceInCorePropertiesPart";
        /// <summary>'{0}' property name is not valid in Core Properties part.</summary>
        internal const string @InvalidPropertyNameInCorePropertiesPart = "InvalidPropertyNameInCorePropertiesPart";
        /// <summary>Core Properties part: A property start-tag was expected.</summary>
        internal const string @PropertyStartTagExpected = "PropertyStartTagExpected";
        /// <summary>Core Properties part: Text data of XSD type 'DateTime' was expected.</summary>
        internal const string @XsdDateTimeExpected = "XsdDateTimeExpected";
        /// <summary>The target of the Core Properties relationship does not reference an existing part.</summary>
        internal const string @DanglingMetadataRelationship = "DanglingMetadataRelationship";
        /// <summary>The Core Properties relationship references a part that has an incorrect content type.</summary>
        internal const string @WrongContentTypeForPropertyPart = "WrongContentTypeForPropertyPart";
        /// <summary>Unexpected number of attributes is found on '{0}'.</summary>
        internal const string @PropertyWrongNumbOfAttribsDefinedOn = "PropertyWrongNumbOfAttribsDefinedOn";
        /// <summary>Unknown xsi:type for DateTime on '{0}'.</summary>
        internal const string @UnknownDCDateTimeXsiType = "UnknownDCDateTimeXsiType";
        /// <summary>More than one '{0}' property found.</summary>
        internal const string @DuplicateCorePropertyName = "DuplicateCorePropertyName";
        /// <summary>PackageProperties object was disposed.</summary>
        internal const string @StorageBasedPackagePropertiesDiposed = "StorageBasedPackagePropertiesDiposed";
        /// <summary>Encoding format is not supported. Only UTF-8 and UTF-16 are supported.</summary>
        internal const string @EncodingNotSupported = "EncodingNotSupported";
        /// <summary>Duplicate pieces found in the package.</summary>
        internal const string @DuplicatePiecesFound = "DuplicatePiecesFound";
        /// <summary>Cannot find piece with the specified piece number.</summary>
        internal const string @PieceDoesNotExist = "PieceDoesNotExist";
        /// <summary>This serviceType is already registered to another service.</summary>
        internal const string @ServiceTypeAlreadyAdded = "ServiceTypeAlreadyAdded";
        /// <summary>'{0}' type name does not have the expected format 'className, assembly'.</summary>
        internal const string @QualifiedNameHasWrongFormat = "QualifiedNameHasWrongFormat";
        /// <summary>Too many attributes are specified for '{0}'.</summary>
        internal const string @ParserAttributeArgsHigh = "ParserAttributeArgsHigh";
        /// <summary>'{0}' requires more attributes.</summary>
        internal const string @ParserAttributeArgsLow = "ParserAttributeArgsLow";
        /// <summary>Cannot load assembly '{0}' because a different version of that same assembly is loaded '{1}'.</summary>
        internal const string @ParserAssemblyLoadVersionMismatch = "ParserAssemblyLoadVersionMismatch";
        /// <summary>(null)</summary>
        internal const string @ToStringNull = "ToStringNull";
        /// <summary>'{0}' ValueSerializer cannot convert '{1}' to '{2}'.</summary>
        internal const string @ConvertToException = "ConvertToException";
        /// <summary>'{0}' ValueSerializer cannot convert from '{1}'.</summary>
        internal const string @ConvertFromException = "ConvertFromException";
        /// <summary>SortDescription must have a nonempty property name.</summary>
        internal const string @SortDescriptionPropertyNameCannotBeEmpty = "SortDescriptionPropertyNameCannotBeEmpty";
        /// <summary>Cannot modify a '{0}' after it is sealed.</summary>
        internal const string @CannotChangeAfterSealed = "CannotChangeAfterSealed";
        /// <summary>Cannot group by property '{0}' because it cannot be found on type '{1}'.</summary>
        internal const string @BadPropertyForGroup = "BadPropertyForGroup";
        /// <summary>The CollectionView that originates this CurrentChanging event is in a state that does not allow the event to be canceled. Check CurrentChangingEventArgs.IsCancelable before assigning to this CurrentChangingEventArgs.Cancel property.</summary>
        internal const string @CurrentChangingCannotBeCanceled = "CurrentChangingCannotBeCanceled";
        /// <summary>Collection is read-only.</summary>
        internal const string @NotSupported_ReadOnlyCollection = "NotSupported_ReadOnlyCollection";
        /// <summary>Only single dimensional arrays are supported for the requested action.</summary>
        internal const string @Arg_RankMultiDimNotSupported = "Arg_RankMultiDimNotSupported";
        /// <summary>The lower bound of target array must be zero.</summary>
        internal const string @Arg_NonZeroLowerBound = "Arg_NonZeroLowerBound";
        /// <summary>Non-negative number required.</summary>
        internal const string @ArgumentOutOfRange_NeedNonNegNum = "ArgumentOutOfRange_NeedNonNegNum";
        /// <summary>Destination array is not long enough to copy all the items in the collection. Check array index and length.</summary>
        internal const string @Arg_ArrayPlusOffTooSmall = "Arg_ArrayPlusOffTooSmall";
        /// <summary>Target array type is not compatible with the type of items in the collection.</summary>
        internal const string @Argument_InvalidArrayType = "Argument_InvalidArrayType";
        /// <summary>'{0}' index is beyond maximum '{1}'.</summary>
        internal const string @ReachOutOfRange = "ReachOutOfRange";
        /// <summary>Permission state is not valid.</summary>
        internal const string @InvalidPermissionState = "InvalidPermissionState";
        /// <summary>Target is not a WebBrowserPermission.</summary>
        internal const string @TargetNotWebBrowserPermissionLevel = "TargetNotWebBrowserPermissionLevel";
        /// <summary>Target is not a MediaPermission.</summary>
        internal const string @TargetNotMediaPermissionLevel = "TargetNotMediaPermissionLevel";
        /// <summary>'{0}' attribute is not valid XML.</summary>
        internal const string @BadXml = "BadXml";
        /// <summary>Permission level is not valid.</summary>
        internal const string @InvalidPermissionLevel = "InvalidPermissionLevel";
        /// <summary>Choice is valid only in AlternateContent.</summary>
        internal const string @XCRChoiceOnlyInAC = "XCRChoiceOnlyInAC";
        /// <summary>Choice cannot follow a Fallback.</summary>
        internal const string @XCRChoiceAfterFallback = "XCRChoiceAfterFallback";
        /// <summary>Choice must contain Requires attribute.</summary>
        internal const string @XCRRequiresAttribNotFound = "XCRRequiresAttribNotFound";
        /// <summary>Requires attribute must contain a valid namespace prefix.</summary>
        internal const string @XCRInvalidRequiresAttribute = "XCRInvalidRequiresAttribute";
        /// <summary>Fallback is valid only in AlternateContent.</summary>
        internal const string @XCRFallbackOnlyInAC = "XCRFallbackOnlyInAC";
        /// <summary>AlternateContent must contain one or more Choice elements.</summary>
        internal const string @XCRChoiceNotFound = "XCRChoiceNotFound";
        /// <summary>AlternateContent must contain only one Fallback element.</summary>
        internal const string @XCRMultipleFallbackFound = "XCRMultipleFallbackFound";
        /// <summary>'{0}' attribute is not valid for '{1}' element.</summary>
        internal const string @XCRInvalidAttribInElement = "XCRInvalidAttribInElement";
        /// <summary>Unrecognized Compatibility element '{0}'.</summary>
        internal const string @XCRUnknownCompatElement = "XCRUnknownCompatElement";
        /// <summary>'{0}' element is not a valid child of AlternateContent. Only Choice and Fallback elements are valid children of an AlternateContent element.</summary>
        internal const string @XCRInvalidACChild = "XCRInvalidACChild";
        /// <summary>'{0}' format is not valid.</summary>
        internal const string @XCRInvalidFormat = "XCRInvalidFormat";
        /// <summary>'{0}' prefix is not defined.</summary>
        internal const string @XCRUndefinedPrefix = "XCRUndefinedPrefix";
        /// <summary>Unrecognized compatibility attribute '{0}'.</summary>
        internal const string @XCRUnknownCompatAttrib = "XCRUnknownCompatAttrib";
        /// <summary>'{0}' namespace cannot process content; it must be declared Ignorable first.</summary>
        internal const string @XCRNSProcessContentNotIgnorable = "XCRNSProcessContentNotIgnorable";
        /// <summary>Duplicate ProcessContent declaration for element '{1}' in namespace '{0}'.</summary>
        internal const string @XCRDuplicateProcessContent = "XCRDuplicateProcessContent";
        /// <summary>Cannot have both a specific and a wildcard ProcessContent declaration for namespace '{0}'.</summary>
        internal const string @XCRInvalidProcessContent = "XCRInvalidProcessContent";
        /// <summary>Duplicate wildcard ProcessContent declaration for namespace '{0}'.</summary>
        internal const string @XCRDuplicateWildcardProcessContent = "XCRDuplicateWildcardProcessContent";
        /// <summary>MustUnderstand condition failed on namespace '{0}'</summary>
        internal const string @XCRMustUnderstandFailed = "XCRMustUnderstandFailed";
        /// <summary>'{0}' namespace cannot preserve items; it must be declared Ignorable first.</summary>
        internal const string @XCRNSPreserveNotIgnorable = "XCRNSPreserveNotIgnorable";
        /// <summary>Duplicate Preserve declaration for element {1} in namespace '{0}'.</summary>
        internal const string @XCRDuplicatePreserve = "XCRDuplicatePreserve";
        /// <summary>Cannot have both a specific and a wildcard Preserve declaration for namespace '{0}'.</summary>
        internal const string @XCRInvalidPreserve = "XCRInvalidPreserve";
        /// <summary>Duplicate wildcard Preserve declaration for namespace '{0}'.</summary>
        internal const string @XCRDuplicateWildcardPreserve = "XCRDuplicateWildcardPreserve";
        /// <summary>'{0}' attribute value is not a valid XML name.</summary>
        internal const string @XCRInvalidXMLName = "XCRInvalidXMLName";
        /// <summary>There is a cycle of XML compatibility definitions, such that namespace '{0}' overrides itself. This could be due to inconsistent XmlnsCompatibilityAttributes in different assemblies. Please change the definitions to eliminate this cycle.</summary>
        internal const string @XCRCompatCycle = "XCRCompatCycle";
        /// <summary>'{1}' event not found on type '{0}'.</summary>
        internal const string @EventNotFound = "EventNotFound";
        /// <summary>Listener did not handle requested event.</summary>
        internal const string @ListenerDidNotHandleEvent = "ListenerDidNotHandleEvent";
        /// <summary>Listener of type '{0}' registered with event manager of type '{1}', but then did not handle the event. The listener is coded incorrectly.</summary>
        internal const string @ListenerDidNotHandleEventDetail = "ListenerDidNotHandleEventDetail";
        /// <summary>WeakEventManager supports only delegates with one target.</summary>
        internal const string @NoMulticastHandlers = "NoMulticastHandlers";
        /// <summary>Unrecoverable system error.</summary>
        internal const string @InvariantFailure = "InvariantFailure";
        /// <summary>ContentType string cannot have leading/trailing Linear White Spaces [LWS - RFC 2616].</summary>
        internal const string @ContentTypeCannotHaveLeadingTrailingLWS = "ContentTypeCannotHaveLeadingTrailingLWS";
        /// <summary>ContentType string is not valid. Expected format is type/subtype.</summary>
        internal const string @InvalidTypeSubType = "InvalidTypeSubType";
        /// <summary>';' must be followed by parameter=value pair.</summary>
        internal const string @ExpectingParameterValuePairs = "ExpectingParameterValuePairs";
        /// <summary>Parameter and value pair is not valid. Expected form is parameter=value.</summary>
        internal const string @InvalidParameterValuePair = "InvalidParameterValuePair";
        /// <summary>A token is not valid. Refer to RFC 2616 for correct grammar of content types.</summary>
        internal const string @InvalidToken = "InvalidToken";
        /// <summary>Parameter value must be a valid token or a quoted string as per RFC 2616.</summary>
        internal const string @InvalidParameterValue = "InvalidParameterValue";
        /// <summary>A Linear White Space character is not valid.</summary>
        internal const string @InvalidLinearWhiteSpaceCharacter = "InvalidLinearWhiteSpaceCharacter";
        /// <summary>Semicolon separator is required between two valid parameter=value pairs.</summary>
        internal const string @ExpectingSemicolon = "ExpectingSemicolon";
        /// <summary>HwndSubclass.Attach has already been called;  it cannot be called again.</summary>
        internal const string @HwndSubclassMultipleAttach = "HwndSubclassMultipleAttach";
        /// <summary>Cannot locate resource '{0}'.</summary>
        internal const string @UnableToLocateResource = "UnableToLocateResource";
        /// <summary>Please wait while the application opens</summary>
        internal const string @SplashScreenIsLoading = "SplashScreenIsLoading";
        /// <summary>Name cannot be an empty string.</summary>
        internal const string @NameScopeNameNotEmptyString = "NameScopeNameNotEmptyString";
        /// <summary>'{0}' Name is not found.</summary>
        internal const string @NameScopeNameNotFound = "NameScopeNameNotFound";
        /// <summary>Cannot register duplicate Name '{0}' in this scope.</summary>
        internal const string @NameScopeDuplicateNamesNotAllowed = "NameScopeDuplicateNamesNotAllowed";
        /// <summary>No NameScope found to {1} the Name '{0}'.</summary>
        internal const string @NameScopeNotFound = "NameScopeNotFound";
        /// <summary>'{0}' name is not valid for identifier.</summary>
        internal const string @NameScopeInvalidIdentifierName = "NameScopeInvalidIdentifierName";
        /// <summary>No dependency property {0} on {1}.</summary>
        internal const string @NoDependencyProperty = "NoDependencyProperty";
        /// <summary>Must set ArrayType before calling ProvideValue on ArrayExtension.</summary>
        internal const string @MarkupExtensionArrayType = "MarkupExtensionArrayType";
        /// <summary>Items in the array must be type '{0}'. One or more items cannot be cast to this type.</summary>
        internal const string @MarkupExtensionArrayBadType = "MarkupExtensionArrayBadType";
        /// <summary>Markup extension '{0}' requires '{1}' be implemented in the IServiceProvider for ProvideValue.</summary>
        internal const string @MarkupExtensionNoContext = "MarkupExtensionNoContext";
        /// <summary>'{0}' StaticExtension value cannot be resolved to an enumeration, static field, or static property.</summary>
        internal const string @MarkupExtensionBadStatic = "MarkupExtensionBadStatic";
        /// <summary>StaticExtension must have Member property set before ProvideValue can be called.</summary>
        internal const string @MarkupExtensionStaticMember = "MarkupExtensionStaticMember";
        /// <summary>TypeExtension must have TypeName property set before ProvideValue can be called.</summary>
        internal const string @MarkupExtensionTypeName = "MarkupExtensionTypeName";
        /// <summary>'{0}' string is not valid for type.</summary>
        internal const string @MarkupExtensionTypeNameBad = "MarkupExtensionTypeNameBad";
        /// <summary>'{0}' must be of type '{1}'.</summary>
        internal const string @MustBeOfType = "MustBeOfType";
        /// <summary>This operation requires the thread's apartment state to be '{0}'.</summary>
        internal const string @Verify_ApartmentState = "Verify_ApartmentState";
        /// <summary>The argument can neither be null nor empty.</summary>
        internal const string @Verify_NeitherNullNorEmpty = "Verify_NeitherNullNorEmpty";
        /// <summary>The argument can not be equal to '{0}'.</summary>
        internal const string @Verify_AreNotEqual = "Verify_AreNotEqual";
        /// <summary>No file exists at '{0}'.</summary>
        internal const string @Verify_FileExists = "Verify_FileExists";
        /// <summary>Event argument is invalid.</summary>
        internal const string @InvalidEvent = "InvalidEvent";
        /// <summary>The property '{0}' cannot be changed. The '{1}' class has been sealed.</summary>
        internal const string @CompatibilityPreferencesSealed = "CompatibilityPreferencesSealed";
        /// <summary>Desktop applications are required to opt in to all earlier accessibility improvements to get the later improvements. To do this, ensure that if the AppContext switch 'Switch.UseLegacyAccessibilityFeatures.N' is set to 'false', then 'Switch.UseLegacyAccessi ...</summary>
        internal const string @CombinationOfAccessibilitySwitchesNotSupported = "CombinationOfAccessibilitySwitchesNotSupported";
        /// <summary>Desktop applications setting AppContext switch '{0}' to false are required to opt in to all earlier accessibility improvements. To do this, ensure that the AppContext switch '{1}' is set to 'false', then 'Switch.UseLegacyAccessibilityFeatures' and all 'Swi ...</summary>
        internal const string @AccessibilitySwitchDependencyNotSatisfied = "AccessibilitySwitchDependencyNotSatisfied";
        /// <summary>Extra data encountered at position {0} while parsing '{1}'.</summary>
        internal const string @TokenizerHelperExtraDataEncountered = "TokenizerHelperExtraDataEncountered";
        /// <summary>Premature string termination encountered while parsing '{0}'.</summary>
        internal const string @TokenizerHelperPrematureStringTermination = "TokenizerHelperPrematureStringTermination";
        /// <summary>Missing end quote encountered while parsing '{0}'.</summary>
        internal const string @TokenizerHelperMissingEndQuote = "TokenizerHelperMissingEndQuote";
        /// <summary>Empty token encountered at position {0} while parsing '{1}'.</summary>
        internal const string @TokenizerHelperEmptyToken = "TokenizerHelperEmptyToken";
        /// <summary>No current object to return.</summary>
        internal const string @Enumerator_VerifyContext = "Enumerator_VerifyContext";
        /// <summary>PermissionState value '{0}' is not valid for this Permission.</summary>
        internal const string @InvalidPermissionStateValue = "InvalidPermissionStateValue";
        /// <summary>Permission type is not valid. Expected '{0}'.</summary>
        internal const string @InvalidPermissionType = "InvalidPermissionType";
        /// <summary>Parameter cannot be a zero-length string.</summary>
        internal const string @StringEmpty = "StringEmpty";
        /// <summary>Parameter must be greater than or equal to zero.</summary>
        internal const string @ParameterCannotBeNegative = "ParameterCannotBeNegative";
        /// <summary>Specified value of type '{0}' must have IsFrozen set to false to modify.</summary>
        internal const string @Freezable_CantBeFrozen = "Freezable_CantBeFrozen";
        /// <summary>Cannot change property metadata after it has been associated with a property.</summary>
        internal const string @TypeMetadataCannotChangeAfterUse = "TypeMetadataCannotChangeAfterUse";
        /// <summary>'{0}' enumeration value is not valid.</summary>
        internal const string @Enum_Invalid = "Enum_Invalid";
        /// <summary>Cannot convert string value '{0}' to type '{1}'.</summary>
        internal const string @CannotConvertStringToType = "CannotConvertStringToType";
        /// <summary>Cannot modify a read-only container.</summary>
        internal const string @CannotModifyReadOnlyContainer = "CannotModifyReadOnlyContainer";
        /// <summary>Cannot get part or part information from a write-only container.</summary>
        internal const string @CannotRetrievePartsOfWriteOnlyContainer = "CannotRetrievePartsOfWriteOnlyContainer";
        /// <summary>'{0}' file does not conform to the expected file format specification.</summary>
        internal const string @FileFormatExceptionWithFileName = "FileFormatExceptionWithFileName";
        /// <summary>Input file or data stream does not conform to the expected file format specification.</summary>
        internal const string @FileFormatException = "FileFormatException";
        /// <summary>{0} is an invalid handle.</summary>
        internal const string @Cryptography_InvalidHandle = "Cryptography_InvalidHandle";
        /// <summary>DLL Name: {0} DLL Location: {1}</summary>
        internal const string @WpfDllConsistencyErrorData = "WpfDllConsistencyErrorData";
        /// <summary>Failed Alternet UI DLL consistency checks. Expected location: {0}.</summary>
        internal const string @WpfDllConsistencyErrorHeader = "WpfDllConsistencyErrorHeader";
        /// <summary>Every RoutedEventArgs must have a non-null RoutedEvent associated with it.</summary>
        internal const string @RoutedEventArgsMustHaveRoutedEvent = "RoutedEventArgsMustHaveRoutedEvent";
        /// <summary>A '{0}' cannot be set on the '{1}' property of type '{2}'. A '{0}' can only be set on a DependencyProperty of a DependencyObject.</summary>
        internal const string @MarkupExtensionDynamicOrBindingOnClrProp = "MarkupExtensionDynamicOrBindingOnClrProp";
        /// <summary>A '{0}' cannot be used within a '{1}' collection. A '{0}' can only be set on a DependencyProperty of a DependencyObject.</summary>
        internal const string @MarkupExtensionDynamicOrBindingInCollection = "MarkupExtensionDynamicOrBindingInCollection";
        /// <summary>Binding cannot be changed after it has been used.</summary>
        internal const string @ChangeSealedBinding = "ChangeSealedBinding";
        /// <summary>Validation rule '{0}' received unexpected value '{1}'.  (This could be caused by assigning the wrong ValidationStep to the rule.)</summary>
        internal const string @ValidationRule_UnexpectedValue = "ValidationRule_UnexpectedValue";
        /// <summary>Syntax error in Binding.Path '{0}' ... '{1}'.</summary>
        internal const string @PathSyntax = "PathSyntax";
        /// <summary>Unmatched parenthesis '{0}'.</summary>
        internal const string @UnmatchedParen = "UnmatchedParen";
        /// <summary>Unmatched bracket '{0}'.</summary>
        internal const string @UnmatchedBracket = "UnmatchedBracket";
        /// <summary>URI must be absolute. Relative URIs are not supported.</summary>
        internal const string @UriMustBeAbsolute = "UriMustBeAbsolute";
        /// <summary>This factory supports only URIs with the '{0}' scheme.</summary>
        internal const string @UriSchemeMismatch = "UriSchemeMismatch";
        /// <summary>The package URI is not allowed in the package store.</summary>
        internal const string @NotAllowedPackageUri = "NotAllowedPackageUri";
        /// <summary>A package with the same URI is already in the package store.</summary>
        internal const string @PackageAlreadyExists = "PackageAlreadyExists";
        /// <summary>Current CachePolicy is CacheOnly but the requested resource does not exist in the cache.</summary>
        internal const string @ResourceNotFoundUnderCacheOnlyPolicy = "ResourceNotFoundUnderCacheOnlyPolicy";
        /// <summary>Cache policy is not valid.</summary>
        internal const string @PackWebRequestCachePolicyIllegal = "PackWebRequestCachePolicyIllegal";
        /// <summary>Cannot have empty name of a temporary file.</summary>
        internal const string @InvalidTempFileName = "InvalidTempFileName";
        /// <summary>The operation is not allowed after the first request is made.</summary>
        internal const string @RequestAlreadyStarted = "RequestAlreadyStarted";
        /// <summary>Cannot access a disposed HTTP byte range downloader.</summary>
        internal const string @ByteRangeDownloaderDisposed = "ByteRangeDownloaderDisposed";
        /// <summary>HTTP byte range downloader can support only HTTP or HTTPS schemes.</summary>
        internal const string @InvalidScheme = "InvalidScheme";
        /// <summary>The event handle is not usable.</summary>
        internal const string @InvalidEventHandle = "InvalidEventHandle";
        /// <summary>Byte range request failed.</summary>
        internal const string @ByteRangeDownloaderErroredOut = "ByteRangeDownloaderErroredOut";
        /// <summary>Byte ranges are not valid in '{0}'.</summary>
        internal const string @InvalidByteRanges = "InvalidByteRanges";
        /// <summary>Server does not support byte range request.</summary>
        internal const string @ByteRangeRequestIsNotSupported = "ByteRangeRequestIsNotSupported";
        /// <summary>Requested PackagePart not found in target resource.</summary>
        internal const string @WebResponsePartNotFound = "WebResponsePartNotFound";
        /// <summary>Error processing WebResponse.</summary>
        internal const string @WebResponseFailure = "WebResponseFailure";
        /// <summary>WebRequest timed out. Response did not arrive before the specified Timeout period elapsed.</summary>
        internal const string @WebRequestTimeout = "WebRequestTimeout";
        /// <summary>Cannot resolve current inner request URI schema. Bypass cache only for resolvable schema types such as http, ftp, or file.</summary>
        internal const string @SchemaInvalidForTransport = "SchemaInvalidForTransport";
        /// <summary>Cannot convert type '{0}' to '{1}'.</summary>
        internal const string @CannotConvertType = "CannotConvertType";
        /// <summary>Parameter is unexpected type '{0}'. Expected type is '{1}'.</summary>
        internal const string @UnexpectedParameterType = "UnexpectedParameterType";
        /// <summary>Property path is not valid. '{0}' does not have a public property named '{1}'.</summary>
        internal const string @PropertyPathNoProperty = "PropertyPathNoProperty";
        /// <summary>Cannot use indexed Value on PropertyDescriptor.</summary>
        internal const string @IndexedPropDescNotImplemented = "IndexedPropDescNotImplemented";
        /// <summary>Text formatting engine encountered a non-CLS exception.</summary>
        internal const string @NonCLSException = "NonCLSException";
        /// <summary>A TwoWay or OneWayToSource binding cannot work on the read-only property '{1}' of type '{0}'.</summary>
        internal const string @CannotWriteToReadOnly = "CannotWriteToReadOnly";
        /// <summary>Mode must be specified for RelativeSource.</summary>
        internal const string @RelativeSourceNeedsMode = "RelativeSourceNeedsMode";
        /// <summary>AncestorType must be specified for RelativeSource in FindAncestor mode.</summary>
        internal const string @RelativeSourceNeedsAncestorType = "RelativeSourceNeedsAncestorType";
        /// <summary>RelativeSource.Mode is immutable after initialization; instead of changing the Mode on this instance, create a new RelativeSource or use a different static instance.</summary>
        internal const string @RelativeSourceModeIsImmutable = "RelativeSourceModeIsImmutable";
        /// <summary>RelativeSource is not in FindAncestor mode.</summary>
        internal const string @RelativeSourceNotInFindAncestorMode = "RelativeSourceNotInFindAncestorMode";
        /// <summary>AncestorLevel cannot be set to less than 1.</summary>
        internal const string @RelativeSourceInvalidAncestorLevel = "RelativeSourceInvalidAncestorLevel";
        /// <summary>Invalid value for RelativeSourceMode enum.</summary>
        internal const string @RelativeSourceModeInvalid = "RelativeSourceModeInvalid";
        /// <summary>Syntax error in PropertyPath '{0}'.</summary>
        internal const string @PropertyPathSyntaxError = "PropertyPathSyntaxError";
        /// <summary>Cannot set BaseUri on this IUriContext implementation.</summary>
        internal const string @ParserProvideValueCantSetUri = "ParserProvideValueCantSetUri";
        /// <summary>Object '{0}' cannot be used as an accessor parameter for a PropertyPath. An accessor parameter must be DependencyProperty, PropertyInfo, or PropertyDescriptor.</summary>
        internal const string @PropertyPathInvalidAccessor = "PropertyPathInvalidAccessor";
        /// <summary>Index {0} is out of range of the PathParameters list, which has length {1}.</summary>
        internal const string @PathParametersIndexOutOfRange = "PathParametersIndexOutOfRange";
        /// <summary>Property path is not valid. Cannot resolve type name '{0}'.</summary>
        internal const string @PropertyPathNoOwnerType = "PropertyPathNoOwnerType";
        /// <summary>PathParameters list contains null at index {0}.</summary>
        internal const string @PathParameterIsNull = "PathParameterIsNull";
        /// <summary>Path indexer parameter has value that cannot be resolved to specified type: '{0}'</summary>
        internal const string @PropertyPathIndexWrongType = "PropertyPathIndexWrongType";
        /// <summary>Failed to compare two elements in the array.</summary>
        internal const string @InvalidOperation_IComparerFailed = "InvalidOperation_IComparerFailed";
        /// <summary>Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.</summary>
        internal const string @Argument_InvalidOffLen = "Argument_InvalidOffLen";
        /// <summary>Synchronization callback for '{0}' collection is no longer available.\n This could happen if the callback is an anonymous method.</summary>
        internal const string @CollectionView_MissingSynchronizationCallback = "CollectionView_MissingSynchronizationCallback";
        /// <summary>Number of elements in source Enumerable is greater than available space from index to the end of destination array.</summary>
        internal const string @CopyToNotEnoughSpace = "CopyToNotEnoughSpace";
        /// <summary>Collection was modified; enumeration operation may not execute.</summary>
        internal const string @EnumeratorVersionChanged = "EnumeratorVersionChanged";
        /// <summary>'{0}' type must derive from FrameworkElement or FrameworkContentElement.</summary>
        internal const string @MustBeFrameworkDerived = "MustBeFrameworkDerived";
        /// <summary>RoutedEvent Name '{0}' for OwnerType '{1}' already used.</summary>
        internal const string @DuplicateEventName = "DuplicateEventName";
        /// <summary>Class handlers can be registered only for UIElement or ContentElement and their subtypes.</summary>
        internal const string @ClassTypeIllegal = "ClassTypeIllegal";
        /// <summary>Handler type is mismatched.</summary>
        internal const string @HandlerTypeIllegal = "HandlerTypeIllegal";
        /// <summary>FrameworkPropertyMetadata.DefaultUpdateSourceTrigger cannot be set to UpdateSourceTrigger.Default; this would create a circular definition.</summary>
        internal const string @NoDefaultUpdateSourceTrigger = "NoDefaultUpdateSourceTrigger";
        /// <summary>The binding group has no binding that uses item '{0}' and property '{1}'.</summary>
        internal const string @BindingGroup_NoEntry = "BindingGroup_NoEntry";
        /// <summary>The value for item '{0}' and property '{1}' is not available because a previous validation rule deemed the value invalid, or because the value could not be computed (e.g., conversion failure).</summary>
        internal const string @BindingGroup_ValueUnavailable = "BindingGroup_ValueUnavailable";
        /// <summary>Underlying list of this CollectionView does not support filtering.</summary>
        internal const string @BindingListCannotCustomFilter = "BindingListCannotCustomFilter";
        /// <summary>'{0}' is not allowed during an AddNew or EditItem transaction.</summary>
        internal const string @MemberNotAllowedDuringAddOrEdit = "MemberNotAllowedDuringAddOrEdit";
        /// <summary>'{0}' is not allowed during a transaction begun by '{1}'.</summary>
        internal const string @MemberNotAllowedDuringTransaction = "MemberNotAllowedDuringTransaction";
        /// <summary>'{0}' is not allowed for this view.</summary>
        internal const string @MemberNotAllowedForView = "MemberNotAllowedForView";
        /// <summary>Removing the NewItem placeholder is not allowed.</summary>
        internal const string @RemovingPlaceholder = "RemovingPlaceholder";
        /// <summary>Editing the NewItem placeholder is not allowed.</summary>
        internal const string @CannotEditPlaceholder = "CannotEditPlaceholder";
        /// <summary>CancelEdit is not supported for the current edit item.</summary>
        internal const string @CancelEditNotSupported = "CancelEditNotSupported";
        /// <summary>Cannot set '{0}' property when '{1}' property is false.</summary>
        internal const string @CannotChangeLiveShaping = "CannotChangeLiveShaping";
        /// <summary>Unexpected collection change action '{0}'.</summary>
        internal const string @UnexpectedCollectionChangeAction = "UnexpectedCollectionChangeAction";
        /// <summary>IBindingList '{0}' has unexpected length after a '{1}' event.\nThis can happen if the IBindingList has been changed without raising a corresponding ListChanged event.</summary>
        internal const string @InconsistentBindingList = "InconsistentBindingList";
        /// <summary>Cannot find type information on collection; property names to SortBy cannot be resolved.</summary>
        internal const string @CannotDetermineSortByPropertiesForCollection = "CannotDetermineSortByPropertiesForCollection";
        /// <summary>'{0}' type does not have property named '{1}', so cannot sort data collection.</summary>
        internal const string @PropertyToSortByNotFoundOnType = "PropertyToSortByNotFoundOnType";
        /// <summary>Range actions are not supported.</summary>
        internal const string @RangeActionsNotSupported = "RangeActionsNotSupported";
        /// <summary>Cannot Move items to an unknown position (-1).</summary>
        internal const string @CannotMoveToUnknownPosition = "CannotMoveToUnknownPosition";
        /// <summary>IBindingList can sort by only one property.</summary>
        internal const string @BindingListCanOnlySortByOneProperty = "BindingListCanOnlySortByOneProperty";
        /// <summary>AccessCollection for '{0}' collection cannot be called after shutdown.</summary>
        internal const string @AccessCollectionAfterShutDown = "AccessCollectionAfterShutDown";
        /// <summary>If SortDescriptions is overridden in derived classes, then must also override '{0}'.</summary>
        internal const string @ImplementOtherMembersWithSort = "ImplementOtherMembersWithSort";
        /// <summary>This type of CollectionView does not support changes to its SourceCollection from a thread different from the Dispatcher thread.</summary>
        internal const string @MultiThreadedCollectionChangeNotSupported = "MultiThreadedCollectionChangeNotSupported";
        /// <summary>Cannot change or check the contents or Current position of CollectionView while Refresh is being deferred.</summary>
        internal const string @NoCheckOrChangeWhenDeferred = "NoCheckOrChangeWhenDeferred";
        /// <summary>Collection Remove event must specify item position.</summary>
        internal const string @RemovedItemNotFound = "RemovedItemNotFound";
        /// <summary>CollectionViewType property can only be set during initialization.</summary>
        internal const string @CollectionViewTypeIsInitOnly = "CollectionViewTypeIsInitOnly";
        /// <summary>'{0}' view does not support sorting.</summary>
        internal const string @CannotSortView = "CannotSortView";
        /// <summary>'{0}' view does not support filtering.</summary>
        internal const string @CannotFilterView = "CannotFilterView";
        /// <summary>'{0}' view does not support grouping.</summary>
        internal const string @CannotGroupView = "CannotGroupView";
        /// <summary>removeIndex is less than zero or greater than or equal to Count.</summary>
        internal const string @ItemCollectionRemoveArgumentOutOfRange = "ItemCollectionRemoveArgumentOutOfRange";
        /// <summary>CompositeCollectionView only supports NotifyCollectionChangeAction.Reset when the collection is empty or is being cleared.</summary>
        internal const string @CompositeCollectionResetOnlyOnClear = "CompositeCollectionResetOnlyOnClear";
        /// <summary>Enumeration has not started. Call MoveNext.</summary>
        internal const string @EnumeratorNotStarted = "EnumeratorNotStarted";
        /// <summary>Enumeration already finished.</summary>
        internal const string @EnumeratorReachedEnd = "EnumeratorReachedEnd";
        /// <summary>CompositeCollection can accept only CollectionContainers it does not already have.</summary>
        internal const string @CollectionContainerMustBeUniqueForComposite = "CollectionContainerMustBeUniqueForComposite";
        /// <summary>'{0}' index in collection change event is not valid for collection of size '{1}'.</summary>
        internal const string @CollectionChangeIndexOutOfRange = "CollectionChangeIndexOutOfRange";
        /// <summary>Added item does not appear at given index '{0}'.</summary>
        internal const string @AddedItemNotAtIndex = "AddedItemNotAtIndex";
        /// <summary>A collection Add event refers to item that does not belong to collection.</summary>
        internal const string @AddedItemNotInCollection = "AddedItemNotInCollection";
        /// <summary>BindingCollection does not support items of type {0}. Only Binding is allowed.</summary>
        internal const string @BindingCollectionContainsNonBinding = "BindingCollectionContainsNonBinding";
        /// <summary>'{0}' child does not have type '{1}' : '{2}'.</summary>
        internal const string @ChildHasWrongType = "ChildHasWrongType";
        /// <summary>Text content is not allowed on this element. Cannot add the text '{0}'.</summary>
        internal const string @NonWhiteSpaceInAddText = "NonWhiteSpaceInAddText";
        /// <summary>Cannot set MultiBinding because MultiValueConverter must be specified.</summary>
        internal const string @MultiBindingHasNoConverter = "MultiBindingHasNoConverter";
        /// <summary>Cannot set UpdateSourceTrigger on inner Binding of MultiBinding. Only the default Immediate UpdateSourceTrigger is valid.</summary>
        internal const string @NoUpdateSourceTriggerForInnerBindingOfMultiBinding = "NoUpdateSourceTriggerForInnerBindingOfMultiBinding";
        /// <summary>The binding expression already belongs to a BindingGroup;  it cannot be added to a different BindingGroup.</summary>
        internal const string @BindingGroup_CannotChangeGroups = "BindingGroup_CannotChangeGroups";
        /// <summary>Internal error: internal Alternet UI code tried to reactivate a BindingExpression that was already marked as detached.</summary>
        internal const string @BindingExpressionStatusChanged = "BindingExpressionStatusChanged";
        /// <summary>Binding.{0} cannot be set while using Binding.{1}.</summary>
        internal const string @BindingConflict = "BindingConflict";
        /// <summary>Cannot get response for web request to '{0}'.</summary>
        internal const string @GetResponseFailed = "GetResponseFailed";
        /// <summary>Cannot create web request for specified Pack URI.</summary>
        internal const string @WebRequestCreationFailed = "WebRequestCreationFailed";
        /// <summary>Cannot perform this operation when binding is detached.</summary>
        internal const string @BindingExpressionIsDetached = "BindingExpressionIsDetached";
        /// <summary>'{1}' property of argument '{0}' must not be null.</summary>
        internal const string @ArgumentPropertyMustNotBeNull = "ArgumentPropertyMustNotBeNull";
        /// <summary>'{0}' property cannot be data-bound.</summary>
        internal const string @PropertyNotBindable = "PropertyNotBindable";
        /// <summary>Two-way binding requires Path or XPath.</summary>
        internal const string @TwoWayBindingNeedsPath = "TwoWayBindingNeedsPath";
        /// <summary>Cannot find converter.</summary>
        internal const string @MissingValueConverter = "MissingValueConverter";
        /// <summary>Unrecognized ValidationStep '{0}' obtained from '{1}'.</summary>
        internal const string @ValidationRule_UnknownStep = "ValidationRule_UnknownStep";
        /// <summary>Value '{0}' could not be converted.</summary>
        internal const string @Validation_ConversionFailed = "Validation_ConversionFailed";
        /// <summary>XmlDataNamespaceMappingCollection child does not have type XmlNamespaceMapping '{0}'.</summary>
        internal const string @RequiresXmlNamespaceMapping = "RequiresXmlNamespaceMapping";
        /// <summary>XmlDataNamespaceMappingCollection cannot use XmlNamespaceMapping that has null URI.</summary>
        internal const string @RequiresXmlNamespaceMappingUri = "RequiresXmlNamespaceMappingUri";
        /// <summary>The number of elements in this collection is greater than the available space from '{0}' to the end of destination '{1}'.</summary>
        internal const string @Collection_CopyTo_NumberOfElementsExceedsArrayLength = "Collection_CopyTo_NumberOfElementsExceedsArrayLength";
        /// <summary>ConstructorParameters cannot be changed because ObjectDataProvider is using user-assigned ObjectInstance.</summary>
        internal const string @ObjectDataProviderParameterCollectionIsNotInUse = "ObjectDataProviderParameterCollectionIsNotInUse";
        /// <summary>ObjectDataProvider can only be assigned an ObjectType or an ObjectInstance, not both.</summary>
        internal const string @ObjectDataProviderCanHaveOnlyOneSource = "ObjectDataProviderCanHaveOnlyOneSource";
        /// <summary>ObjectDataProvider needs either an ObjectType or ObjectInstance.</summary>
        internal const string @ObjectDataProviderHasNoSource = "ObjectDataProviderHasNoSource";
        /// <summary>Unknown exception while creating type '{0}' for ObjectDataProvider.</summary>
        internal const string @ObjectDataProviderNonCLSException = "ObjectDataProviderNonCLSException";
        /// <summary>Unknown exception while invoking method '{0}' on type '{1}' for ObjectDataProvider.</summary>
        internal const string @ObjectDataProviderNonCLSExceptionInvoke = "ObjectDataProviderNonCLSExceptionInvoke";
        /// <summary>The required pattern for URI containing ";component" is "AssemblyName;Vxxxx;PublicKey;component", where Vxxxx is the assembly version and PublicKey is the 16-character string representing the assembly public key token. Vxxxx and PublicKey are optional.</summary>
        internal const string @WrongFirstSegment = "WrongFirstSegment";
        /// <summary>Cannot navigate to application resource '{0}' by using a WebBrowser control. For URI navigation, the resource must be at the application's site of origin. Use the pack://siteoforigin:,,,/ prefix to avoid hard-coding the URI.</summary>
        internal const string @CannotNavigateToApplicationResourcesInWebBrowser = "CannotNavigateToApplicationResourcesInWebBrowser";
        /// <summary>The '{0}' property of the '{1}' type can be set only during initialization.</summary>
        internal const string @PropertyIsInitializeOnly = "PropertyIsInitializeOnly";
        /// <summary>The '{0}' property of the '{1}' type cannot be changed after it has been set.</summary>
        internal const string @PropertyIsImmutable = "PropertyIsImmutable";
        /// <summary>The '{0}' property of the '{1}' type must be set during initialization.</summary>
        internal const string @PropertyMustHaveValue = "PropertyMustHaveValue";
        /// <summary>'{0}' type is not a CollectionView type.</summary>
        internal const string @CollectionView_WrongType = "CollectionView_WrongType";
        /// <summary>'{0}' does not have a constructor that accepts collection type '{1}'.</summary>
        internal const string @CollectionView_ViewTypeInsufficient = "CollectionView_ViewTypeInsufficient";
        /// <summary>Cannot get CollectionView of type '{0}' for CollectionViewSource that already uses type '{1}'.</summary>
        internal const string @CollectionView_NameTypeDuplicity = "CollectionView_NameTypeDuplicity";
        /// <summary>Must set Source in RoutedEventArgs before building event route or invoking handlers.</summary>
        internal const string @SourceNotSet = "SourceNotSet";
        /// <summary>RoutedEvent in RoutedEventArgs and EventRoute are mismatched.</summary>
        internal const string @Mismatched_RoutedEvent = "Mismatched_RoutedEvent";
        /// <summary>Potential cycle in tree found while building the event route.</summary>
        internal const string @TreeLoop = "TreeLoop";
        /// <summary>InheritanceBehavior must be set when the instance is not yet connected to a tree. Set InheritanceBehavior when the object is constructed.</summary>
        internal const string @Illegal_InheritanceBehaviorSettor = "Illegal_InheritanceBehaviorSettor";
        /// <summary>Logical tree depth exceeded while traversing the tree. This could indicate a cycle in the tree.</summary>
        internal const string @LogicalTreeLoop = "LogicalTreeLoop";
        /// <summary>'{0}' is not a valid type for IInputElement. UIElement or ContentElement expected.</summary>
        internal const string @Invalid_IInputElement = "Invalid_IInputElement";
        /// <summary>Only PreProcessInput and PostProcessInput events can access InputManager staging area.</summary>
        internal const string @NotAllowedToAccessStagingArea = "NotAllowedToAccessStagingArea";
        /// <summary>Result text cannot be null.</summary>
        internal const string @TextComposition_NullResultText = "TextComposition_NullResultText";
        /// <summary>'{0}' does not have a valid InputManager.</summary>
        internal const string @TextCompositionManager_NoInputManager = "TextCompositionManager_NoInputManager";
        /// <summary>'{0}' has already started.</summary>
        internal const string @TextCompositionManager_TextCompositionHasStarted = "TextCompositionManager_TextCompositionHasStarted";
        /// <summary>'{0}' has not yet started.</summary>
        internal const string @TextCompositionManager_TextCompositionNotStarted = "TextCompositionManager_TextCompositionNotStarted";
        /// <summary>'{0}' has already finished.</summary>
        internal const string @TextCompositionManager_TextCompositionHasDone = "TextCompositionManager_TextCompositionHasDone";
        /// <summary>The calling thread must be STA, because many UI components require this.</summary>
        internal const string @RequiresSTA = "RequiresSTA";

    }
}
