# Prepare project with FAKE and Paket
1. create solution folder: cd into a empty folder(fsharp-in-vscode) as solution folder
2. create solution
```
dotnet new sln -n FSharpSolution (FSharpSolution is solution name)
```
3. Open vscode by: `code .`
4. Install FAKE
```
dotnet new -i "fake-template::*"  (just once)
dotnet new fake
```
5. Install Paket
```
dotnet tool install paket -g
```
6. Init paket
ctrl+shift+P (via vscode) “paket init”

7. via vscode: create console project 
project directory (relative to solution root): src/Visualization
project name: Visualization

8. via vscode: create library project
project ditory (relative to solution root): src/Tools
project name: Tools 

9. Add project to solution by 
Selecing the solution in F# view, right click > Add project 

# Use paket to manage dependencies:
1. Install plotly depdencies for examples from:https://plotly.com/fsharp/time-series/ 
Edit paket.dependencies file as:
```
source https://api.nuget.org/v3/index.json

storage: none
framework: net6.0, netstandard2.0, netstandard2.1

nuget Newtonsoft.Json >= 12.0.3
nuget Plotly.NET 2.0.0-preview.10
nuget Plotly.NET.Interactive 2.0.0-preview.10
nuget Deedle
nuget FSharp.Data
```

2. Reference them from particular project such as Visualization project.
Create paket.references and fill it with:
```
Newtonsoft.Json
Plotly.NET
Plotly.NET.Interactive
Deedle
FSharp.Data
```

3. In solution root, run 
```
dotnet paket install
```

4. check outdate and update
```
dotnet paket outdated
dotnet paket update
```

# Use .fsx script file with packages
1. Add file to F# project via vscode F# explorer:
Such that, the file is included in “Compile Include”
  <ItemGroup>
    <Compile Include="TimeSeries.fsx" />
    <Compile Include="Program.fs" />
  </ItemGroup>
Otherwise, it is not aware by the F# project.

2. Add references like:
#r "nuget: Plotly.NET"
#r "nuget: Plotly.NET.LayoutObjects"
#r "nuget: FSharp.Data"


open Plotly.NET
open Plotly.NET.LayoutObjects
open FSharp.Data

3. Run it as:
// run it from command-line with 
dotnet fsi TimeSeries.fsx 

# Troubleshooting
## Try to fix:
dotnet fsi TimeSeries.fsx 

/Users/zw/code/fsharp_programming/fsharp-in-vscode/src/Visualization/TimeSeries.fsx(1,1): error FS3217: /Users/zw/.packagemanagement/nuget/Projects/24850--96ad0771-b2b6-48c9-8cae-a8c6a2d155ea/Project.fsproj : error NU1101: Unable to find package Plotly.NET.LayoutObjects. No packages exist with this id in source(s): /usr/local/share/dotnet/library-packs, /usr/local/share/dotnet/sdk/6.0.401/FSharp/library-packs, nuget.org

Steps:
1. change paket.dependencies as:
nuget Plotly.NET >= 3.0.1
nuget Plotly.NET.Interactive >= 3.0.2

2. Update packages (run from solution root)
dotnet paket outdated
dotnet paket update

3. Remove
#r “nuget: Plotly.NET.LayoutObjects”
But still keep:  open Plotly.NET.LayoutObjects

(If after installed package, there red lines for not finding references, reload vscode could resolve this problem)


## Try to fix:
“fsi is not defined problem”