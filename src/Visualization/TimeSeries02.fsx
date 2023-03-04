#r "nuget: XPlot.Plotly"

open System
open XPlot.Plotly

let x =
    [
        DateTime(2013, 10, 4);
        DateTime(2013, 11, 5);
        DateTime(2013, 12, 6)
    ]

let layout = Layout(title = "Time Series Plot with datetime Objects")

let chart1 =
    Scatter(
        x = x,
        y = [1; 3; 6]
    )
    |> Chart.Plot
    |> Chart.WithLayout layout
    |> Chart.WithWidth 700
    |> Chart.WithHeight 500

chart1
|> Chart.Show