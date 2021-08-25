namespace OpenAL

open System
open System.Reflection
open System.Runtime.InteropServices

module Entry =
    let loadOpenAL () =
        let assembly = Assembly.GetAssembly(typeof<OpenAL.Buffer>)
        let importResolver (libraryName: string) _ _ =
            match libraryName with
            | "OpenAL" when RuntimeInformation.IsOSPlatform(OSPlatform.Windows) -> NativeLibrary.Load("OpenAL32.dll")
            | "OpenAL" when RuntimeInformation.IsOSPlatform(OSPlatform.Linux) -> NativeLibrary.Load("libopenal.so")
            | _ -> IntPtr.Zero
        NativeLibrary.SetDllImportResolver(assembly, DllImportResolver importResolver)
