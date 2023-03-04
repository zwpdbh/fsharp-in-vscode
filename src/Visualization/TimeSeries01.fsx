#r "nuget: Plotly.NET"
#r "nuget: FSharp.Data"

open Plotly.NET
open Plotly.NET.LayoutObjects
open FSharp.Data
open System


let data =
    CsvFile.Load("https://raw.githubusercontent.com/plotly/datasets/master/stockdata.csv")

let getChart (stock: string) =
    let y =
        data.Rows
        |> Seq.map (fun row -> row.GetColumn(stock))
        |> Seq.take 250

    let x =
        data.Rows
        |> Seq.map (fun row -> row.GetColumn("Date"))
        |> Seq.take 250

    Chart.Line(x = x, y = y, Name = stock)

let demo01 () = 
    [ 
        getChart "AAPL"
        getChart "SBUX"
        getChart "IBM"
        getChart "MSFT" ]
    |> Chart.combine
    |> Chart.withXAxis (LinearAxis.init (DTick = "M1", TickFormat = "%b\n%Y", TickLabelMode=StyleParam.TickLabelMode.Period))
    |> Chart.withTitle("custom tick labels with TickLabelMode='Period'")
    |> Chart.show

let demo02 () = 
    let ts = [0. .. 0.1 .. 10.]
    let ys = ts |> Seq.map (Math.Sin)
    Chart.Scatter(ts, ys, mode=StyleParam.Mode.Markers)
    |> Chart.show


// run it from command-line with 
// dotnet fsi MyFirstScript.fsx "zhaowei"
match fsi.CommandLineArgs with
    | [| scriptName; word |] ->
        printfn "running script: %s" scriptName
        toPigLatin word |> printfn "%A"
    | _ ->
        printfn "USAGE: [word]"    
demo02()