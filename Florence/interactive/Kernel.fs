namespace Florence.Interactive

open System.Threading.Tasks
open Microsoft.DotNet.Interactive.Commands
open Microsoft.DotNet.Interactive.Directives
open Microsoft.DotNet.Interactive.Formatting
open Microsoft.DotNet.Interactive

open Microsoft.DotNet.Interactive.FSharp    
open Microsoft.AspNetCore.Html
open Microsoft.DotNet.Interactive.Events
open System.Linq
open Florence

type MyCommand() =
    inherit KernelDirectiveCommand()

module Utils = 
    let formatHtml (value:string) =
        FormattedValue(
            HtmlFormatter.MimeType,
            HtmlString(value).ToDisplayString(HtmlFormatter.MimeType))
        
type FeltParams(context, id) =
    member val Context: KernelInvocationContext = context
    member val Id: string = id

type FlorenceExtension() =
    let mk = """
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/daisyui@5.0.0-beta.7/daisyui.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@geoapify/geocoder-autocomplete@2.1.0/styles/minimal.min.css" />
    <link rel="stylesheet" href="https://cdn.tailwindcss.com" />
    <div class="card card-compact w-96 bg-base-100 shadow-xl">
             <div class="card-body  items-center text-center">
    <h2 class="card-title">Florence</h2>
    
    <p>DSL for evaluating places</p>
    <img src="https://wrometrcloud.blob.core.windows.net/florence/kafel.png" style="width:100px"/>
    <div class="card-actions justify-end">
      <button class="btn btn-primary">github</button>
    </div>
  </div></div>"""
            
            
    let formatHtml (value: HtmlString) =
        FormattedValue(
            HtmlFormatter.MimeType,
            value.ToDisplayString(HtmlFormatter.MimeType))
        
    let handler (_command: MyCommand)  (context: KernelInvocationContext): Task =
        task {
            if context <> null then
                context.DisplayAs("""
    <div id="sutil-app"></div>
      <script type="module">
    import { render, renderCommand } from 'https://wrometrcloud.blob.core.windows.net/florence/bundle.js';
    render();
  </script>
    ""","text/html") |> ignore
            return Task.CompletedTask
        }
    let entry (kernel: Kernel) =
        let cmd = KernelActionDirective("#!entry")
        cmd.Parameters.Add(KernelDirectiveParameter("--id", "city of ..."))
        kernel.AddDirective<MyCommand>(cmd, handler)

    interface IKernelExtension with 

        member this.OnLoadAsync(kernel: Kernel): Task =
            Formatter.Register<Geojson>((fun (Geojson s) -> $"""
    <div id="entry"></div>
    <script type="module">
    import {{ render, renderCommand }} from 'https://wrometrcloud.blob.core.windows.net/florence/bundle.js';
    console.log(`{s}`)
    renderCommand("entry", `{s}`);
  </script>"""), "text/html")
            entry(kernel)
            let ck = kernel :?> CompositeKernel
            if not (kernel.Name.Contains "Florence") then 
                ck.Add(new FlorenceKernel("Florence"))

            task {

                let! _ = Lang.fsharp Dependencies.packages
                let! _ = Lang.fsharp Dependencies.``open``
                let! _sample1 = IO.content Store.cento_luoghi (nameof(Store.cento_luoghi))
                let! _sample2 = IO.content Store.tram_fermate (nameof(Store.tram_fermate))
                let! _sample3 = IO.content Store.sez_censimento2011 (nameof(Store.sez_censimento2011))
                let! _sample4 = IO.content Store.famousPeople (nameof(Store.famousPeople))
                let! _sample5 = Lang.fsharp $"""type FamousPeopleDistances = DistanceProvider.GeojsonContent<{Store.famousPeople.trippleQuote()}>"""
                
                let! _markup = 
                    mk
                    |> HtmlString
                    |> formatHtml
                    |> DisplayValue
                    |> Kernel.Root.SendAsync 

                return ()
            }
