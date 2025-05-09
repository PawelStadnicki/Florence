namespace Florence

open Florence
open Microsoft.DotNet.Interactive.Commands
open Microsoft.DotNet.Interactive
open System.Linq
open Microsoft.DotNet.Interactive.FSharp
open Microsoft.DotNet.Interactive.Events

module Fsharp =

    let requestStringValue (name: string) =
        //TODO: test and enwrap with Option expressions from FsToolkit
        task {
            try
                let! requestedValue = 
                    RequestValue(name, "text/plain", "fsharp")
                    |> Kernel.Root.SendAsync
                    
                if requestedValue.Events.ToList() |> Seq.exists (fun t -> t.Command.GetType().Name = "ValueProduced") then
                    let event = 
                        requestedValue.Events.First(fun x -> x.GetType() = typeof<ValueProduced>) 
                    match event with
                    | null -> return None
                    | _evt ->
                        let valueProduced = event :?> ValueProduced
                        return Some valueProduced.FormattedValue.Value
                else 
                    return None
            with
            | _ex -> return None
        }
    let runOpt (code: string, name: string) =
         //TODO: test and enwrap with Option expressions from FsToolkit
        task {
            let kernel = new FSharpKernel()

            let! _runCode = kernel.SubmitCodeAsync(code)
            let! requestValue = 
                RequestValue(name, "text/plain", "fsharp")
                |> kernel.SendAsync

            let events = requestValue.Events.ToList().Select( fun x -> x.GetType().Name)

            if events |> Seq.contains "CommandFailed" then return None
            else if events |> Seq.contains "ValueProduced" then
                let event =  requestValue.Events.First(fun x -> x.GetType() = typeof<ValueProduced>)  
                let valueProduced = event :?> ValueProduced

                return Some valueProduced.FormattedValue.Value 
            else return None
        }

    let literal (name: string) = 

        let code x = SubmitCode(x, "fsharp")

        let value =
            task {    
                return! requestStringValue name
            } 
            |> Async.AwaitTask 
            |> Async.RunSynchronously
            |>> _.trippleQuote()

        $"""[<Literal>]{System.Environment.NewLine}let {name} = {value}"""
        |> code
        |> Kernel.Current.DeferCommand

    let jsonProvider (name: string) (value: string)= 
        task {            
            $"""let {name} = JsonProvider<{value.trippleQuote()}>.GetSample()"""
            |> Lang.fsharpDefer  
        } 
        |> Async.AwaitTask 
        |> Async.StartImmediate

    let addVariable (name: string) (value: string)= 
        task {            
            return $"""let {name} = {value.trippleQuote()} """
            |> Lang.fsharpDefer
        } 