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
            public bool Verbose { get; set; } = true;

            [Option("timespan", Required = false, HelpText = "Show Timespan the parsed FTDC covers")]
            public bool Timespan { get; set; } = true;

            [Option('f', "file", Required = false, HelpText = "FTDC file to parse")]
            public string Filename { get; set; } = @"C:\Users\Administrator\source\repos\MongoDB.FTDC\MongoDB.FTDC.Parser.Tests\diagnostic.data\metrics.2020-01-02T11-02-43Z-00000";
        }

        private static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                Log.Logger = o.Verbose
                    ? new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger()
                    : new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().CreateLogger();

                Log.Information("MongoDB FTDC Parser");

                var ftdc = new FTDCFile(o.Filename);
                Log.Debug($"FTDC Samples: {ftdc.Contents.Count}");

                if (o.Timespan)
                {
                    Log.Information($"Metrics Begin: {ftdc.MetricsStart}");
                    Log.Information($"Metrics End:   {ftdc.MetricsEnd}");
                }
            });
        }
    }
}