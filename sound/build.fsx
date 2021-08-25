(*
open Fake.Core
open Fake.DotNet
open Fake.IO

let shellExec (cmd: string) (args: string) =
    let exitCode = Shell.Exec(cmd, args)
    assert (exitCode = 0)

// ----- Build -----

Target.create "BuildCpp" (fun _ -> shellExec "ninja" "")

Target.create
    "BuildFsDebug"
    (fun _ ->
        DotNet.build
            (fun options ->
                { options with
                      Configuration = DotNet.BuildConfiguration.Debug })
            "src/main")

Target.create
    "BuildFsRelease"
    (fun _ ->
        DotNet.build
            (fun options ->
                { options with
                      Configuration = DotNet.BuildConfiguration.Release })
            "src/main")

// ----- Clean -----

Target.create "CleanCpp" (fun _ -> Shell.cleanDir "build")

Target.create "CleanFsDebug" (fun _ -> DotNet.exec id "clean" "-c Debug" |> ignore)

Target.create "CleanFsRelease" (fun _ -> DotNet.exec id "clean" "-c Release" |> ignore)

Target.create "Clean" ignore

open Fake.Core.TargetOperators

"CleanCpp" ==> "BuildCpp"

"CleanFsDebug" ==> "BuildFsDebug"
"CleanFsRelease" ==> "BuildFsRelease"

"BuildCpp" ==> "BuildFsDebug"
"BuildCpp" ==> "BuildFsRelease"

"CleanCpp" ==> "Clean"
"CleanFsDebug" ==> "Clean"
"CleanFsRelease" ==> "Clean"

Target.runOrDefault "BuildFsDebug"
*)
