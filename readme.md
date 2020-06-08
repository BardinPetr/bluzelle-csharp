<a href="https://bluzelle.com/"><img src='https://raw.githubusercontent.com/bluzelle/api/master/source/images/Bluzelle%20-%20Logo%20-%20Big%20-%20Colour.png' alt="Bluzelle" style="width: 100%"/></a>

# BluzelleC#
BluzelleC# is a native *.Net Core 3* library that can be used to access the Bluzelle database service.

* [API docs](https://bardinpetr.github.io/bluzelle-csharp/api/index.html)
* [Example application](https://github.com/BardinPetr/bluzelle-csharp/tree/master/ExampleApp)
* [Quickstart Video Demo](https://drive.google.com/file/d/1jwpO9U1uuKS51o8UX_ANZsEWOabnseOe/view)
* [Main repo](https://github.com/BardinPetr/bluzelle-csharp)


## Installation
### Preparation
* Install .Net Core 3.1 (https://dotnet.microsoft.com/download)

### Including into project
#### Using Nuget 
```
dotnet add package Bluzelle
or
nuget install Bluzelle
```
#### Building from source
```
git clone https://github.com/BardinPetr/bluzelle-csharp
dotnet build
```

## Quickstart

```c#
using BluzelleCSharp;
using BluzelleCSharp.Models;

var bz = new BluzelleApi(
                    "7f346254-2024-496f-bfa3-572a2e87ebd2",
                    "around buzz diagram captain obtain detail salon mango muffin brother morning jeans display attend knife carry green dwarf vendor hungry fan route pumpkin car",
                    "bluzelle1upsfjftremwgxz3gfy0wf3xgvwpymqx754ssu9");

// Create key "x" with value "data" and lease of 1 min
await bz.Create("x", "data",
    new LeaseInfo(0, 0, 1, 0),
    new GasInfo {GasPrice = 10});

// Read key "x" from DB
Console.WriteLine(await bz.Read("x"));
```
### Main Points About Library

* All methods in lib are async
* Library ensures FIFO order of transaction execution
* Some methods has duplicate with *Tx* prefix: it means that command can be executed as transaction *(with prefix)* and as query *(without prefix)* 
* Library can run under mono

## Tests
Tests can be run using external test suite via HTTP API server located in TestAPI project. 
To run server:
```
cd TestAPI
dotnet run
```
Then use http://localhost:5000/ as HTTP POST endpoint for queries in format:
```json
{
  "method": "$method name in Blzjs style$",
  "args": ["$method arguments in Blzjs style$", ...] 
}
```
All arguments are casted to string and then used in methods. For transaction calls gas configuration should be omitted, it is already set to *GasPrice=10*. Account data and namespace uuid should be placed in *TestAPI/appsettings.json*.

Example:
```json
{
  "method": "create",
  "args": ["key", "value", 1000] // or ["key", "value", "1000"]
}
```