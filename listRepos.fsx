#r "nuget: dotenv.net, 3.1.0"
#r "nuget: FSharp.Data, 4.2.2"

open FSharp.Data

type UserRepositories = JsonProvider<"""[{ "full_name": "0918nobita/0918nobita" }]""">

let request (ghToken: string) (perPage: int) (page: int) : UserRepositories.Root array =
    printfn "Requesting..."

    Http.RequestString(
        "http://api.github.com/users/0918nobita/repos",
        [ ("per_page", string perPage)
          ("page", string page) ],
        [ ("Accept", "application/vnd.github.v3+json")
          ("User-Agent", "request")
          ("Authorization", $"token %s{ghToken}") ]
    )
    |> UserRepositories.Parse

let () =
    let ghToken =
        dotenv.net.DotEnv.Read()
        |> Seq.map (|KeyValue|)
        |> Map.ofSeq
        |> Map.tryFind "GH_TOKEN"
        |> Option.defaultWith (fun () -> failwith "GH_TOKEN wasn't set. Please set one in .env file.")

    let perPage = 100

    let repos =
        seq {
            let mutable page = 1
            let mutable continueLooping = true

            while continueLooping do
                let nextRepos = request ghToken perPage page
                yield! nextRepos
                page <- page + 1

                if Array.length nextRepos < perPage then
                    continueLooping <- false
        }
        |> Seq.toList

    repos
    |> List.map (fun repo -> repo.FullName)
    |> List.iter (printfn "%s")

    printfn "Total: %i" <| List.length repos
