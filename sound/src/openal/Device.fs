namespace OpenAL

#nowarn "9"

open System
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Logger

module private NativeContext =
    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alcDestroyContext(IntPtr _context)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alcMakeContextCurrent(IntPtr _context)

[<Sealed>]
type Context(contextPtr: IntPtr) =
    member _.MakeCurrent() =
        log.D "make context current"
        NativeContext.alcMakeContextCurrent contextPtr

    interface IDisposable with
        member _.Dispose() =
            log.T "destroy context"
            NativeContext.alcDestroyContext contextPtr

module private NativeDevice =
    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern IntPtr alcOpenDevice(IntPtr _deviceName)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alcCloseDevice(IntPtr _devicePtr)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern IntPtr alcCreateContext(IntPtr _devicePtr, int* _attrList)

[<Sealed>]
type Device private (devicePtr: IntPtr) =
    static member TryOpenDefault() =
        let ptr = NativeDevice.alcOpenDevice IntPtr.Zero

        if (ptr <> IntPtr.Zero) then
            Some(new Device(ptr))
        else
            None

    member _.TryCreateContext() =
        let ptr =
            NativeDevice.alcCreateContext (devicePtr, NativePtr.ofNativeInt<int> IntPtr.Zero)

        if (ptr <> IntPtr.Zero) then
            Some(new Context(ptr))
        else
            None

    interface IDisposable with
        member _.Dispose() =
            log.T "close device"
            NativeDevice.alcCloseDevice devicePtr
