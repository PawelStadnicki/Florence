namespace Florence

open System.Net.Http
open System.Threading.Tasks

module IO =
    
    let httpClient = new HttpClient()

    let getUrlContent (url: string) : Task<string> =
        httpClient.GetStringAsync url
        
    let loadAsync (path: string) = 
        task {
            let! content = getUrlContent(path)
            let name = System.IO.Path.GetFileNameWithoutExtension path
            return Parser.get content name |> Lang.fsharpDefer
        }
            
    let local (path: string) = 
        task {
            let content = System.IO.File.ReadAllText(path)
            let name = System.IO.Path.GetFileNameWithoutExtension path
            do Parser.get content name |> Lang.fsharpDefer
        }
        |> Async.AwaitTask
        |> Async.RunSynchronously
    
    let content (content: string) (name: string) = 
        task {

            do Parser.get content name |> Lang.fsharpDefer
        }