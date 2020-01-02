using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.FTDC.Parser
{
    using Metric = System.Int64;

    public class FTDCFileContents
    {
        public DateTime _id { get; set; }
        public int type { get; set; }
        public BsonDocument doc { get; set; }
        public BsonBinaryData data { get; set; }
    }

    public class FTDCFile
    {
        public List<FTDCFileContents> Contents { get; internal set; } = new List<FTDCFileContents>();

        public void Open(String path)
        {
            if (!File.Exists(path))
            {
                throw new FTDCException($"Cannot load ${path}");
            }

            using (var sourceStream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BsonBinaryReader(sourceStream))
                {
                    while (!reader.IsAtEndOfFile())
                    {
                        var result = BsonSerializer.Deserialize<FTDCFileContents>(reader);
                        Contents.Add(result);
                    }
                }
            }
        }
    }

    public class FTDCException : Exception
    {
        public FTDCException(string message) : base(message)
        {
        }

        public FTDCException()
        {
        }

        public FTDCException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}