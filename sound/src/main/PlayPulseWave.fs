module PlayPulseWave

open FSharpPlus
open FsToolkit.ErrorHandling.AsyncResultCE
open OpenAL

let playPulseWaveAsync =
    asyncResult {
        use! device =
            Device.TryOpenDefault()
            |> Option.toResultWith "Failed to open default device"
            |> async.Return

        use! context =
            device.TryCreateContext()
            |> Option.toResultWith "Failed to create context"
            |> async.Return

        context.MakeCurrent()

        let data =
            [| 0xFFuy
               0x00uy
               0x00uy
               0x00uy
               0x00uy
               0x00uy
               0x00uy
               0x00uy |]

        use buffer = new Buffer(data)
        use source = new Source(buffer)
        source.EnableLooping()
        source.SetGain(0.1f)

        source.Play()

        do! Async.Sleep 2000 |> Async.map Ok

        source.Stop()
    }
