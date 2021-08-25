module RenderSineWave

open System
open System.IO
open MathExt
open Pcm
open Units

/// 周波数 `f0` Hz のサイン波の、時間 `n` における変位を求める
let sineWave (f0: float<Hz>) (n: int) : int16 =
    /// 振幅
    let a = 30000.0
    /// 標本化周波数
    let fs = 44100.0<Hz>

    a * sin (2.0<rad> * Math.PI * f0 * double n / fs)
    |> int16

let renderSineWave =
    let samplesPerSec = 44100<Hz>
    let numSamples = samplesPerSec * 2<s>

    let initializeRawData (n: int) =
        let f0 =
            if n > 44100 then
                587.33<Hz>
            else
                523.25<Hz>

        sineWave f0 n

    let rawData = Array.init numSamples initializeRawData

    let pcm =
        Pcm(samplesPerSec = samplesPerSec, numSamples = numSamples, rawData = rawData)

    async {
        use fileStream =
            new FileStream("out.wav", FileMode.Create)

        use binaryWriter = new BinaryWriter(fileStream)
        PcmWriter.writePcm binaryWriter pcm
    }
