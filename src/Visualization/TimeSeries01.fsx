#r "nuget: Plotly.NET"
#r "nuget: Plotly.NET.LayoutObjects"
#r "nuget: FSharp.Data"


open Plotly.NET
open Plotly.NET.LayoutObjects
open FSharp.Data

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


[ getChart "AAPL"
  getChart "SBUX"
  getChart "IBM"
  getChart "MSFT" ]
|> Chart.combine
|> Chart.withXAxis (LinearAxis.init (DTick = "M1", TickFormat = "%b\n%Y", TickLabelMode=StyleParam.TickLabelMode.Period))
|> Chart.withTitle("custom tick labels with TickLabelMode='Period'")
|> Chart.show