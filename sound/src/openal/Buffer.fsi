namespace OpenAL

open System

[<Sealed>]
type Buffer =
    interface IDisposable
    new : byte array -> Buffer
    member internal Id : nativeptr<uint>
