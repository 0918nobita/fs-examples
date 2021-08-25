module MathExt

open System
open Units

let inline sin (a: float<rad>) = Math.Sin(float a)

let inline bitToByte (bits: int16<bit>) : int16<ubyte> = bits / 8s<bit/ubyte>
