(*
open Fake.Core
open Fake.DotNet

let shellExec (cmd: string) (args: string) =
    Shell.Exec(cmd, args) |> ignore

Target.create
    "LintCpp"
    (fun _ ->
        [ async { shellExec "cpplint" "--recursive ./sound" }
          async { shellExec "clang-format" "--dry-run --Werror ./sound/src/cpp/mysound.cc" } ]
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore)

let fsharplint (project: string) =
    DotNet.exec id "fsharplint" $"lint {project}" |> ignore

let fantomasCheck (files: string) =
    DotNet.exec id "fantomas" $"--check {files}" |> ignore

Target.create
    "LintFs"
    (fun _ ->
        [ async { fsharplint "sound/src/main/main.fsproj" }
          async { fsharplint "sound/src/openal/openal.fsproj" }
          async { fantomasCheck "build.fsx lint.fsx sound/src/main sound/src/openal" } ]
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore)

Target.create "Lint" ignore

open Fake.Core.TargetOperators

"LintCpp" ==> "Lint"
"LintFs" ==> "Lint"

Target.runOrDefault "Lint"
*)
