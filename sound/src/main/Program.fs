module Program

open FSharpPlus

[<EntryPoint>]
let main _ =
    OpenAL.Entry.loadOpenAL ()

    [ PlayPulseWave.playPulseWaveAsync
      RenderSineWave.renderSineWave |> Async.map Ok ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore

    0
