namespace OpenAL

#nowarn "9"

open System
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Logger

module private NativeSource =
    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alGenSources(int _n, uint* _sources)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alDeleteSources(int _n, uint* _sources)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alSourcei(uint _sourceId, int _param, int _value)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alSourcef(uint _sourceId, int _param, float32 _value)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alSourcePlay(uint _sourceId)

    [<DllImport("OpenAL", CallingConvention = CallingConvention.Cdecl)>]
    extern void alSourceStop(uint _sourceId)

type private ALConst =
    | Buffer = 0x1009
    | Gain = 0x100A
    | Looping = 0x1007
    | True = 1

[<Sealed>]
type Source(buffer: OpenAL.Buffer) =
    let idNativeInt = Marshal.AllocCoTaskMem 4
    let idNativePtr = NativePtr.ofNativeInt<uint> idNativeInt
    do NativeSource.alGenSources (1, idNativePtr)
    do NativeSource.alSourcei (NativePtr.get idNativePtr 0, int ALConst.Buffer, int <| NativePtr.get<uint> buffer.Id 0)

    member _.EnableLooping() =
        NativeSource.alSourcei (NativePtr.get idNativePtr 0, int ALConst.Looping, int ALConst.True)

    member _.SetGain(gain: float32) =
        NativeSource.alSourcef (NativePtr.get idNativePtr 0, int ALConst.Gain, gain)

    member _.Play() =
        NativeSource.alSourcePlay (NativePtr.get idNativePtr 0)

    member _.Stop() =
        NativeSource.alSourceStop (NativePtr.get idNativePtr 0)

    interface IDisposable with
        member _.Dispose() =
            log.T "delete source"
            NativeSource.alSourcei (NativePtr.get idNativePtr 0, int ALConst.Buffer, 0)
            NativeSource.alDeleteSources (1, idNativePtr)
            Marshal.FreeCoTaskMem idNativeInt
