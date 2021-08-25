namespace OpenAL

open System

[<Sealed>]
type Context =
    interface IDisposable
    member MakeCurrent : unit -> unit

[<Sealed>]
type Device =
    interface IDisposable
    static member TryOpenDefault : unit -> Device option
    member TryCreateContext : unit -> Context option
