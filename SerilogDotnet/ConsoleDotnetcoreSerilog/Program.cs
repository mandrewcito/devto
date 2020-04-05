using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace AspnetcoreSerilog
{
    class Program
    {
        static void Main(string[] args)
        {  
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
                
            Log.Information("Hello world!");
            Log.Warning("Oops!! ");
            Log.Error("Ouch!!!! ");
        }
    }
}
