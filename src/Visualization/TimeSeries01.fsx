#r "nuget: Plotly.NET"
#r "nuget: FSharp.Data"
#r "../Tools/bin/Debug/net6.0/Tools.dll"

open Plotly.NET
open Plotly.NET.LayoutObjects
open FSharp.Data
open System

// Need to add "Tools" as project reference, and the Tools.dll is built out
open Tools.PigLatin

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


// Run fsx script from command-line from project folder:
// dotnet fsi TimeSeries01.fsx "plot" "demo02" 
// dotnet fsi TimeSeries01.fsx "plot" "demo01" 
// dotnet fsi TimeSeries01.fsx "pigLatin" "zwpdbh"
// Or from root of solution
// dotnet fsi ./src/Visualization/TimeSeries01.fsx "pigLatin" "zwpdbh" 
match fsi.CommandLineArgs with
    | [| _scriptName; option; arg |] ->
        match option, arg with 
        | "plot", "demo01" -> 
            printfn "running plot for demo01"
            demo01()
        | "plot", "demo02" -> 
            printfn "running plot for demo02"
            demo02()
        | "pigLatin", word -> 
            printfn "running module from tools"
            toPigLatin word |> printfn "%A"
        | _, _ -> 
            printfn "other options are not supported"       
    | _ ->
        printfn "USAGE: [word]"    