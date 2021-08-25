module Pcm

open Units

/// 16bit PCM の内部表現
[<Struct>]
type Pcm16bit = Pcm of samplesPerSec: int<Hz> * numSamples: int * rawData: int16 []
