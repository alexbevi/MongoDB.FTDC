using CommandLine;
using System;
using Serilog;
using MongoDB.FTDC.Parser;

namespace MongoDB.FTDC
{
    internal class Program
    {
        public class Options
        {
            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
        }

        private static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                if (o.Verbose)
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();
                    Log.Debug($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                    Log.Information("Quick Start Example! App is in Verbose mode!");
                }
                else
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console()
                        .CreateLogger();
                    Log.Information("Quick Start Example!");
                }

                var ftdc = new FTDCFile();
                ftdc.Open(@"C:\Users\Administrator\source\repos\MongoDB.FTDC\MongoDB.FTDC.Parser.Tests\diagnostic.data\metrics.2020-01-02T11-02-43Z-00000");
            });
        }
    }
}