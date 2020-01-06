using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.IO;
using MongoDB.Bson.Serialization;
using Serilog;

namespace MongoDB.FTDC.Parser
{
    public class FTDCFile
    {
        public List<FTDCFileContents> Contents { get; internal set; } = new List<FTDCFileContents>();

        public void Open(String path)
        {
            if (!File.Exists(path))
            {
                throw new FTDCException($"Cannot load ${path}");
            }

            Log.Debug($"Loading ${path}");

            using var sourceStream = new FileStream(path, FileMode.Open);
            using var reader = new BsonBinaryReader(sourceStream);
            while (!reader.IsAtEndOfFile())
            {
                var result = BsonSerializer.Deserialize<FTDCFileContents>(reader);
                Contents.Add(result);
            }
        }
    }
}