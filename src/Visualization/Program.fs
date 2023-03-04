namespace Visualization

module MyTest = 
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


module Main = 
// Run the program within project folder
// dotnet run "pigLatin" "zwpdbh"
// dotnet run "plot" "demo01"
    [<EntryPoint>]
    let main args =
        args |> printfn "%A"
        match args with
        | [| option; arg |] ->
            match option, arg with 
            | "plot", "demo01" -> 
                printfn "running plot for demo01"
                MyTest.demo01()
            | "plot", "demo02" -> 
                printfn "running plot for demo02"
                MyTest.demo02()
            | "pigLatin", word -> 
                printfn "running module from tools"
                Tools.PigLatin.toPigLatin word |> printfn "%A"
            | _, _ -> 
                printfn "other options are not supported"       
        | _ ->
            printfn "USAGE: [word]"    

        0