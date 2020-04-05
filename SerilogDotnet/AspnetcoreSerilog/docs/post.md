---
title: Serilog Configuration in aspnet core
published: true
description: On the previous post we fully configure serilog, now we will add to our aspnet application
tags: aspnetcore, webapi, serilog, tutorial
series: dotnet serilog tutorial 
---

On the previous post we fully configurate a serilog on a console application, one of the advantages of defining configuration on a json file, is that we can migrate configuration only copiying the file.

#### Create the project 

```powershell
mkdir AspnetcoreSerilog
cd AspnetcoreSerilog
dotnet run webapi
```

Now, we copy serilog config from our [previous appsettings file](https://github.com/mandrewcito/devto/blob/master/SerilogDotnet/ConsoleDotnetcoreSerilog/appsettings.json) and add dependencies to our new csproj and restore:


```xml
  <PackageReference Include="Serilog.AspNetCore" Version="3.2.0" />
  <PackageReference Include="Serilog.Settings.Configuration" Version="3.1.0" />
  <PackageReference Include="Serilog.Enrichers.Thread" Version="3.1.0" />
  <PackageReference Include="Serilog.Sinks.File" Version="4.1.0" />
  <PackageReference Include="Serilog.Sinks.Console" Version="3.1.1" />
  <PackageReference Include="Serilog.Filters.Expressions" Version="2.1.0" />
  <PackageReference Include="Serilog.Sinks.RollingFile" Version="3.3.0" />
```

Modify your program.cs file, add to CreateHostbuilder the  function use serilog call.

```csharp
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            	.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
				    .ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
```

Now we continue host configuration modifications: Enable request logging modifying Startup.cs 

```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            /* ** file continues  ** */
        }
```

On the previous post i described the filter expressions functionality. This time we will use expressions to send log messages to differente files, in particular  depending on it http method. 

For doing this we need a few things. First i will destructure Http Request and then i will filter with a expression depending on the http method.

```json
{
    "Destructure": [
      {
        "Name": "ByTransformingWhere",
        "Args": {
          "predicate": "t => typeof(HttpRequest).Equals(t)",
          "transformedType": "HttpRequest",
          "transformation": "a => new { RawUrl = a.RawUrl, Method = a.Method }"
        }
      }
    ],
}
```

```json
{
           "Filter": [
              {
                "Name": "ByIncludingOnly",
                "Args": {
                  "expression": "Method = 'POST'"
                }
              }
            ]
}
```

Now we will see on console log all logs, and http post calls in his respective file.

My example includes GET and POST filter with two subloggers, you can check http file on my github repo see references below.


# References

[AspnetSerilog repository (this example)](https://github.com/mandrewcito/devto/tree/master/SerilogDotnet/AspnetcoreSerilog)
[desctructure] (https://github.com/serilog/serilog/wiki/Structured-Data)
[web classic enrich](https://github.com/serilog-web/classic)
[filter expressions](https://github.com/serilog/serilog-filters-expressions)
[destructure transforming](https://github.com/serilog/serilog-settings-configuration/issues/106)