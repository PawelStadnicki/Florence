namespace Florence

  module Lang =

    open Microsoft.DotNet.Interactive.Commands
    open Microsoft.DotNet.Interactive

    let fsharp code = 
        Kernel.Root.SendAsync(SubmitCode(code ,"fsharp"))
        
    let custom kernel code = 
        Kernel.Root.SendAsync(SubmitCode(code ,kernel)) 
    let fsharpDefer code = 
        SubmitCode(code, "fsharp")
        |> Kernel.Current.DeferCommand

    let csharp code = 
        Kernel.Root.SendAsync(SubmitCode(code ,"csharp"))

    let python code =
        Kernel.Root.SendAsync(SubmitCode(code ,"pythonkernel"))

    let pythonDefer code =
        SubmitCode(code ,"pythonkernel")|> Kernel.Current.DeferCommand
