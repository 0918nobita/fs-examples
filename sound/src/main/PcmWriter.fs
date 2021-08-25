module PcmWriter

open System
open System.IO
open System.Text
open Units
open MathExt
open Pcm

type BinaryWriter with
    member inline this.WriteAsciiBytes(asciiStr: string) =
        this.Write(Encoding.ASCII.GetBytes asciiStr)

let inline writePcm (writer: BinaryWriter) (Pcm (samplesPerSec, numSamples, rawData)) =
    let channel = 1s // モノラル
    let rawDataSize = numSamples * 2 * int channel
    let riffChunkSize = 36 + rawDataSize
    let bitsPerSample = 16s<bit>
    /// 1 秒間の音データを記録するのに必要なデータ量
    let blockSize: int16<ubyte> = bitToByte (channel * bitsPerSample)

    let bytesPerSec: int<ubyte / s> =
        (int blockSize * 1<ubyte>) * samplesPerSec

    writer.WriteAsciiBytes "RIFF"
    writer.Write riffChunkSize
    writer.WriteAsciiBytes "WAVE"
    writer.WriteAsciiBytes "fmt "
    writer.Write 16 // fmt chunk size
    writer.Write 1s // wave format type
    writer.Write channel
    writer.Write(int samplesPerSec)
    writer.Write(int bytesPerSec)
    writer.Write(int16 blockSize)
    writer.Write(int16 bitsPerSample)
    writer.WriteAsciiBytes "data"
    writer.Write rawDataSize

    rawData
    |> Array.iter (BitConverter.GetBytes >> writer.Write)
