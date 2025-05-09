namespace Polyglot

open App
open Fable.Core.JsInterop
open Fable.Core

module Languages =
    let [<Global>] webview : obj = jsNative

    [<Emit("typeof(kernel) !== 'undefined' && typeof(kernel.root) !== 'undefined'")>]
    let _webviewExists: bool = jsNative

    [<Emit("typeof(window.updateProgress) !== 'undefined'")>]
    let _updateProgressExists: bool = jsNative

    type Dispatcher() =
        member this.isInteractive() = _webviewExists

    let (|WebPage|PolyglotNotebooks|) (gate:Dispatcher) = 
        if gate.isInteractive() then PolyglotNotebooks
        else WebPage
        
    let js code =

        {|
            commandType = "SubmitCode"
            command =
                {|
                    code = code
                    targetKernelName = "javascript"
                |}
        |} 
    let fs code =

        {|
            commandType = "SubmitCode"
            command =
                {|
                    code = code
                    targetKernelName = "fsharp"
                |}
        |}

    let sendFs code= 
        promise {
            let! _ = webview?compositeKernel?send(fs code) |> Thenable.toPromise 
            return ()
        } 
        |> Async.AwaitPromise

    let sendJs code= 
        promise {
            let! _ = webview?compositeKernel?send(js code) |> Thenable.toPromise 
            ()
        } 
        |> Async.AwaitPromise