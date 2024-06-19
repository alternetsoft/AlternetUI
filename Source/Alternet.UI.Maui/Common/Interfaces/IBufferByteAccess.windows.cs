using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage.Streams;
using WinRT;
using WinRT.Interop;

namespace Alternet.UI
{
    [WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
    [Guid("905a0fef-bc53-11df-8c49-001e4fc686da")]
    internal interface IBufferByteAccess
    {
        IntPtr Buffer { get; }
    }
}

namespace ABI.Alternet.UI
{
    [DynamicInterfaceCastableImplementation]
    [Guid("905a0fef-bc53-11df-8c49-001e4fc686da")]
    internal interface IBufferByteAccess : global::Alternet.UI.IBufferByteAccess
    {
        public struct VftblPtr
        {
            public IntPtr Vftbl;
        }

        [Guid("905a0fef-bc53-11df-8c49-001e4fc686da")]
        public struct Vftbl
        {
            public delegate int GetBufferDelegate(IntPtr thisPtr, out IntPtr buffer);

            internal IUnknownVftbl IUnknownVftbl;
            public GetBufferDelegate getBuffer;

            static unsafe Vftbl()
            {
                AbiToProjectionVftable = new Vftbl
                {
                    IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
                    getBuffer = DoAbiGetBuffer,
                };
                var nativeVftbl =
                    (IntPtr*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IUnknownVftbl>() + (sizeof(IntPtr) * 12));
                Marshal.StructureToPtr(AbiToProjectionVftable.IUnknownVftbl, (IntPtr)nativeVftbl, false);
                nativeVftbl[3] = Marshal.GetFunctionPointerForDelegate(AbiToProjectionVftable.getBuffer);
                AbiToProjectionVftablePtr = (IntPtr)nativeVftbl;
            }

            public static readonly Vftbl AbiToProjectionVftable;
            public static readonly IntPtr AbiToProjectionVftablePtr;

            internal unsafe Vftbl(IntPtr ptr)
            {
                var vftblPtr = Marshal.PtrToStructure<VftblPtr>(ptr);
                var vftbl = (IntPtr*)vftblPtr.Vftbl;
                IUnknownVftbl = Marshal.PtrToStructure<IUnknownVftbl>(vftblPtr.Vftbl);
                getBuffer = Marshal.GetDelegateForFunctionPointer<GetBufferDelegate>(vftbl[3]);
            }

            private static int DoAbiGetBuffer(IntPtr thisPtr, out IntPtr buffer)
            {
                buffer = default;
                try
                {
                    buffer = ComWrappersSupport.FindObject<global::Alternet.UI.IBufferByteAccess>(thisPtr).Buffer;
                }
                catch (Exception ex)
                {
                    return Marshal.GetHRForException(ex);
                }

                return 0;
            }
        }

        IntPtr global::Alternet.UI.IBufferByteAccess.Buffer
        {
            get
            {
                var obj = (ObjectReference<Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(global::Alternet.UI.IBufferByteAccess).TypeHandle);
                var ThisPtr = obj.ThisPtr;
                Marshal.ThrowExceptionForHR(obj.Vftbl.getBuffer(ThisPtr, out IntPtr buffer));
                return buffer;
            }
        }

        internal static ObjectReference<Vftbl> FromAbi(IntPtr thisPtr) =>
            ObjectReference<Vftbl>.FromAbi(thisPtr);
    }
}