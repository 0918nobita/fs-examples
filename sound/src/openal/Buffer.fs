namespace OpenAL

#nowarn "9" // IntPtr から nativeptr<'T> に変換している部分で警告しない
#nowarn "51" // mutable な let 束縛からポインタ nativeptr<'T> を取得している部分で警告しない

open System
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Logger

module private NativeBuffer =
    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alGenBuffers(int _n, uint* _buffers)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alDeleteBuffers(int _n, uint* _buffers)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alBufferData(uint _buffer, int _format, nativeint _data, int _size, int _freq)

[<Sealed>]
type Buffer(initialData: byte array) =
    let idNativeInt = Marshal.AllocCoTaskMem 4

    let idNativePtr = NativePtr.ofNativeInt<uint> idNativeInt

    do NativeBuffer.alGenBuffers (1, idNativePtr)

    let dataNativeInt = Marshal.AllocCoTaskMem 8

    let dataSize = Array.length initialData
    do Marshal.Copy(initialData, 0, dataNativeInt, dataSize)

    do
        NativeBuffer.alBufferData (
            NativePtr.get idNativePtr 0,
            0x1100 (* AL_FORMAT_MONO8 *) ,
            dataNativeInt,
            dataSize,
            dataSize * 440
        )

    member internal _.Id = idNativePtr

    interface IDisposable with
        member _.Dispose() =
            log.T "delete buffer"
            NativeBuffer.alDeleteBuffers (1, idNativePtr)
            Marshal.FreeCoTaskMem idNativeInt
            Marshal.FreeCoTaskMem dataNativeInt
