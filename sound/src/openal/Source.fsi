namespace OpenAL

open System

[<Sealed>]
type Source =
    interface IDisposable
    new : OpenAL.Buffer -> Source
    member EnableLooping : unit -> unit
    member SetGain : float32 -> unit
    member Play : unit -> unit
    member Stop : unit -> unit
