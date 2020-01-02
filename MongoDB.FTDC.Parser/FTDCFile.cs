using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.IO;

namespace MongoDB.FTDC.Parser
{
    using Metric = System.Int64;

    public class FTDCFile
    {
        public List<FTDCChunk> Chunks { get; set; }
        public List<FTDCMeta> Meta { get; set; }

        public void Open(String path)
        {
            if (!File.Exists(path))
            {
                throw new FTDCException($"Cannot load ${path}");
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

    public class FTDCChunk
    {
        // mutex

        public Metric TMin { get; set; }

        public Metric TMax { get; set; }

        public byte[] CompressedData { get; set; }

        public BsonDocument ReferenceDocument { get; set; }
        public int KeyCount { get; set; }
        public int DeltaCount { get; set; }
        public byte[] Deltas { get; set; }

        public List<string> Keys { get; set; }
        public Metric[][] Metrics { get; set; }
    }

    public class FTDCMeta
    {
        public BsonDocument Document { get; set; }
    }
}