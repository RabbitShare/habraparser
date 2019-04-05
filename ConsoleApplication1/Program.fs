open FSharp.Data
open System.IO
open FSharp.Collections.ParallelSeq

let getTitels (doc: HtmlDocument) =
    doc.Descendants ["a"]
    |> PSeq.filter (fun a -> a.HasClass "post__title_link")
    |> PSeq.choose (fun x -> 
        x.TryGetAttribute("href")
        |> Option.map (fun a -> x.InnerText(), a.Value()))

let appendLines file strs =
    File.AppendAllLines(file, strs)

let pages url i =
    [ 1 .. i ]
    |> Seq.iter (fun x -> 
        url + string x 
        |> HtmlDocument.Load
        |> getTitels
        |> PSeq.map(fun x -> match x with (a,b) -> sprintf "%s\n   %s" a b )
        |> appendLines "file.txt")

[<EntryPoint>]
let main argv =
    pages "https://habr.com/all/page" 100
    0
