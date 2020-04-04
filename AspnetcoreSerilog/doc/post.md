---
title: Playing with serilog and aspnetcore
published: false
description: Serilog configuration via appsettings.json in aspnetcore 3.1
tags: aspnetcore, serilog, expressions, appsettings
---

Logging it's a must on any application. Making a simmilitude with the [journalism](https://en.wikipedia.org/wiki/Five_Ws) when a event appears on our application,  we need to answer some questions before writing it  This is not strict at all but that i try to say is that an event alone means nothing, we need enrich it with some context. With that a log line will be more usefull:

* Who: Method that generates our event.
* What: Kind of event.
* Where: Context of event, class, request ...
* When: Timestamp of the event.
* Why: Inner exception, data asociated to the event ...

And one thing that i concider very usefull  wich is very personal, is that we need wrapping with quotes all parameter that we log, becouse there is a huge difference between this two log lines:

```
12-12-1988 Warning query paramters id = 12341
```

```
12-12-1988 Warning query paramters id = '12341    ' 
```

#### Setup application

I will start with something very simple. The simplest app is a console application, so we create a folder and open a terminal on the folder.

```powershell
dotnet new console
```

Add serilog to our csproj and restore packages 

```xml
<ItemGroup>
  <PackageReference Include="Serilog.AspNetCore" Version="3.2.0" />
</ItemGroup>
```

```powershell
dotnet restore
```

#### My first log

Using serilog is very simple, so next snippet is a fast configuration, you can fount it on the [serilog github doc](https://github.com/serilog/serilog-aspnetcore). Let's modify our program.cs

```csharp

namespace AspnetcoreSerilog
{
    class Program
    {
        static void Main(string[] args)
        {  
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Console()
                .CreateLogger();
            Log.Information("Hello world!");
        }
    }
}
```

Output of the previous program:

```powershell
PS D:\Workspace\dev.to\AspnetcoreSerilog> dotnet run
[23:21:53 INF] Hello world!
```

#### Configure via appsettings.json

Ok, but serilog's configuration will grow a lot. In a real world app we want a log with a lot of features, subloggers... and if we need changing a configuration section we will need recompile library, we dont want that. 

So lets introduce the package serilog-configuration. As a dependency, we will need adding some packages.Reading configuration from a json file can be easly done  with microsoft extensions [this post](https://pradeeploganathan.com/dotnet/configuration-in-a-net-core-console-application/) may be helpfull. I directly add new dependencies to our csproj:

```xml
  <ItemGroup>
    <PackageReference Include="Serilog.AspNetCore" Version="3.2.0" />
    <PackageReference Include="Serilog.Settings.Configuration" Version="3.1.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="3.1.3" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="3.1.3" />
    <PackageReference Include="Microsoft.Extensions.Configuration.CommandLine" Version="3.1.3" />
    <PackageReference Include="Microsoft.Extensions.Configuration.EnvironmentVariables " Version="3.1.3" />
  </ItemGroup>
  <ItemGroup>
      <None Update="appsettings.json" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
```

Our program.cs will load configuration and provide it to the logger.

```csharp
namespace AspnetcoreSerilog
{
    class Program
    {
        static void Main(string[] args)
        {  
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            Log.Information("Hello world!");
        }
    }
}
```


Now, we can configure serilog with via appsettings.json 


```json
{
    "Serilog": {
		"Using": [],
		"MinimumLevel": {
			"Default": "Debug",
			"Override": {
				"Microsoft": "Information",
				"System": "Warning"
			}
        },
        "WriteTo": [
            { "Name": "Console" }
        ]
    }
}
```

If we run again the program, output is the same:

```powershell
PS D:\Workspace\dev.to\AspnetcoreSerilog> dotnet run
[23:24:34 INF] Hello world!
```

#### I need  customizing output

Serilog output can be easily configured with output template parameter:

```json
{
 ... Take a look to previous json 
        "WriteTo": [
            {
                 "Name": "Console",
                 "Args": {
                    "outputTemplate": "{Timestamp:dd/MM/yy HH:mm:ss,fff} [{Level:u3}] {Message}{NewLine:1}"
                 }
            }
        ]
    }
}
```

Output will the following:

```powershell
PS D:\Workspace\dev.to\AspnetcoreSerilog> dotnet run
04/04/20 23:39:09,387 [INF] Hello world!
```

#### This information it´is not enough, i want more. Enrich helps you

There are a lot of enrichers, depending on your needs, you can add its packages to your csproj file. For my example i will use thread enricher:

Add your dependency on csproj and restore:

```xml
    <PackageReference Include="Serilog.Enrichers.Thread" Version="3.1.0" />
```

Modify serilog configuration file adding enricher's using and modifying output template to include it:

```json
{
    "Serilog": {
        "Using": [],
        "Enrich":["WithThreadId"],
        "MinimumLevel": {...},
        "WriteTo": [
            {
                 "Name": "Console",
                 "Args": {
                    "outputTemplate": "{Timestamp:dd/MM/yy HH:mm:ss,fff} [{Level:u3}] Thread:{ThreadId} {Message}{NewLine:1}"
                 }
            }
        ]
    }
}
```

Test output:

```powershell
PS D:\Workspace\dev.to\AspnetcoreSerilog> dotnet run
04/04/20 23:48:37,002 [INF] Thread:1 Hello world!
```

#### Logger it's cool but i need sending my output to various sources

With WriteTo(subloggers) you can send log to any resource, and there are a [lot of Sinks already implemented](https://github.com/serilog/serilog/wiki/Provided-Sinks). So the only decisiton you have to take is where you want to place log. For simplicity i will store my log messages in two files and console. So the only sink i need is file sink.

```xml
 <PackageReference Include="Serilog.Sinks.File" Version="4.1.0" />
```

Add this 2 nodes to WriteTo section:

```json
 {
                "Name":"File",
                "Args": {
                    "path": "./log1.log"
                }

            },
            {
                "Name":"File",
                "Args": {
                    "path": "./log2.log"
                }
            }
```

Output on powershell continue being the same but if we check content of the files will be the same too.

```powershell
PS D:\Workspace\dev.to\AspnetcoreSerilog> dotnet run
05/04/20 00:01:16,177 [INF] Thread:1 Hello world!
PS D:\Workspace\dev.to\AspnetcoreSerilog> type .\log1.log
2020-04-05 00:01:16.177 +02:00 [INF] Hello world!
PS D:\Workspace\dev.to\AspnetcoreSerilog> type .\log2.log
2020-04-05 00:01:16.177 +02:00 [INF] Hello world!
```

#### Information it's duplicate! expressions to the rescue 

On a real world app, we will probably need sending logs to diferent files or resources.On this example i will send error logs to one file and information to another. There are a lot of choices for log filtering you can find it [here](https://github.com/serilog/serilog-filters-expressions):

First, lets make some changes on our program.cs adding an error log.

```csharp
namespace AspnetcoreSerilog
{
    class Program
    {
        static void Main(string[] args)
        {  
            // Configuration here
            Log.Information("Hello world!");
            Log.Warning("Oops!! ");
            Log.Error("Ouch!!!! ");
        }
    }
}
```

Add package to our csproj:

```xml
    <PackageReference Include="Serilog.Filters.Expressions" Version="2.1.0" />
```

Now our json file will change significally, despite of having 2 file sinks we will have 2 subloggers filtered with file sinks. I will show appsettings.json file complete:

```json
{
    "Serilog": {
        "Using": [
        ],
        "Enrich": [
            "WithThreadId"
        ],
        "MinimumLevel": {
            "Default": "Debug"
        },
        "WriteTo": [
            {
                "Name": "Console",
                "Args": {
                    "outputTemplate": "{Timestamp:dd/MM/yy HH:mm:ss,fff} [{Level:u3}] Thread:{ThreadId} {Message}{NewLine:1}"
                }
            },
            {
                "Name": "Logger",
                "Args": {
                    "configureLogger": {
                        "Filter": [
                            {
                                "Name": "ByIncludingOnly",
                                "Args": {
                                    "expression": "@Level = 'Warning'"
                                }
                            }
                        ],
                        "WriteTo": [
                            {
                                "Name": "File",
                                "Args": {
                                    "path": "./warning.log"
                                }
                            }
                        ]
                    }
                }
            },
            {
                "Name": "Logger",
                "Args": {
                    "configureLogger": {
                        "Filter": [
                            {
                                "Name": "ByIncludingOnly",
                                "Args": {
                                    "expression": "@Level = 'Error'"
                                }
                            }
                        ],
                        "WriteTo": [
                            {
                                "Name": "File",
                                "Args": {
                                    "path": "./error.log"
                                }
                            }
                        ]
                    }
                }
            }
        ]
    }
}
```

Output must be the following: Log writes all messages, warning.log file only has warnings logs and error.log only has errors.

```powershell
PS D:\Workspace\dev.to\AspnetcoreSerilog> dotnet run
05/04/20 00:33:51,927 [INF] Thread:1 Hello world!
05/04/20 00:33:51,948 [WRN] Thread:1 Oops!! 
05/04/20 00:33:51,954 [ERR] Thread:1 Ouch!!!! 
PS D:\Workspace\dev.to\AspnetcoreSerilog> type .\error.log
2020-04-05 00:33:51.954 +02:00 [ERR] Ouch!!!!
PS D:\Workspace\dev.to\AspnetcoreSerilog> type .\warning.log
2020-04-05 00:33:51.948 +02:00 [WRN] Oops!!
```

That's all! fell free to leave a comment below!

#### Resouces 

Example repository: 

* [working example app](https://github.com/mandrewcito/dev.to/

Serilog´s documentation it is enough:

* [serilog - aspnetcore](https://github.com/serilog/serilog-aspnetcore)
* [serilog - appsettings](https://github.com/serilog/serilog-settings-configuration)
* [serilog - enrich example](https://github.com/serilog/serilog-aspnetcore#request-logging)
* [serilog - sink File](https://github.com/serilog/serilog-sinks-file)
* [serilog - sink RollingFile](https://github.com/serilog/serilog-sinks-rollingfile)
* [serilog - expressions](https://github.com/serilog/serilog-filters-expressions)
