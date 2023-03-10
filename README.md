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
    <Compile Include="TimeSeries.fsx" />  (DO NOT DO THIS!)
    <Compile Include="Program.fs" />
  </ItemGroup>
Otherwise, it is not aware by the F# project.
Notice: DO NOT include ".fsx" file, otherwise you may not be able to run program as "dotnet run".

2. Add references like:
#r "nuget: Plotly.NET"
#r "nuget: Plotly.NET.LayoutObjects"
#r "nuget: FSharp.Data"


open Plotly.NET
open Plotly.NET.LayoutObjects
open FSharp.Data

3. Run script file

Run fsx script from command-line from project folder:
```
dotnet fsi TimeSeries01.fsx "plot" "demo02" 
dotnet fsi TimeSeries01.fsx "plot" "demo01" 
dotnet fsi TimeSeries01.fsx "pigLatin" "zwpdbh"
```

Or from root of solution
```
dotnet fsi ./src/Visualization/TimeSeries01.fsx "plot" "demo01" 
dotnet fsi ./src/Visualization/TimeSeries01.fsx "pigLatin" "zwpdbh" 
```

## Use Paket's generated script to load dependency packages
The above `#r "nuget: Plotly.NET"` has downside that it will not utilize the paket's depdnencies directly.

To solve this problem and avoid install nuget packages repeatedly. We just need to do the following changes:
1. Add `generate_load_scripts: true` at the top of file "paket.dependencies".
2. Run `paket restore`
3. Change the dependences references from 
```
#r "nuget: Plotly.NET"
#r "nuget: Plotly.NET.LayoutObjects"
#r "nuget: FSharp.Data"
```
to 
```
#load @"../../.paket/load/net6.0/Plotly.NET.fsx"
#load @"../../.paket/load/net6.0/FSharp.Data.fsx"
```

This feature comes from: [paket generate-load-scripts](https://fsprojects.github.io/Paket/paket-generate-load-scripts.html).

## How to reference library built from other project
1. Suppose we have another library project "Tools" located as src/Tools.
2. Built that project to generate the `Tools.dll` file.
3. Use it in our script (suppose our project is "Viaualization" located as "src/Visualization")
```
#r "../Tools/bin/Debug/net6.0/Tools.dll"
open Tools.PigLatin

toPigLatin "zwpdbh" |> printfn "%A"
```


# Jupyter Notebook for F# in Visual Studio Code
## Prepare vscode extensions
1. Install "Jupyter" extension.
2. Install "Polyglot Notebooks" extension

## Install the .NET Interactive globally
To let code block in Jupyter notebook has F# option, we need to run:
```
dotnet tool install -g Microsoft.dotnet-interactive
dotnet tool update -g Microsoft.dotnet-interactive
```

## Register F# kernel for Jupyter
The most easy way to do this is through "Anaconda".
1. Install Anaconda
2. Open the Anaconda Prompt (Windows) or a terminal (macOS, Linux) run the following commands:
```
# Ensure that Jupyter and .NET are installed and present on the path:
jupyter kernelspec list
dotnet --version

# Install the .NET kernel
dotnet interactive jupyter install

# Verify the installation by running the following again in the Anaconda Prompt.
> jupyter kernelspec list
  .net-csharp        ~\jupyter\kernels\.net-csharp
  .net-fsharp        ~\jupyter\kernels\.net-fsharp
  .net-powershell    ~\jupyter\kernels\.net-powershell
  python3            ~\jupyter\kernels\python3
```

## Prepare a FSharp project to use FSharp Script 
1. via vscode F# project -> console application -> src/Notebook -> Notebook
2. add file TravelMap.ipynb 
3. add file paket.references to reference dependencies
4. In ipynb file use code block with option F#. 


# Troubleshooting
Warning: Occationally vscode could not highlight the code or red line code with modules not found error. Usually, it could be resolved by reload vscode.
Even sometimes with red line errors in vscode, we could still be able to run the script from command-line. 
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
“fsi is not defined problem” during running program with "dotnet run".
Solution: DO NOT include ".fsx" into project as compile component. 
Afte I comment out them from project build, I could dotnet run project meanwhile I could run .fsx script file seperately.
```
    <!-- <Compile Include="TimeSeries01.fsx" />
    <Compile Include="TimeSeries02.fsx" /> -->
    <Compile Include="Program.fs" />
```