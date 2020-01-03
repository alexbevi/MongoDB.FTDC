using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.FTDC.Parser
{
    using CompressionLevel = ZLibNet.CompressionLevel;
    using CompressionMode = ZLibNet.CompressionMode;
    using Metric = System.Int64;

    public class Chunk
    {
        public Metric TMin { get; internal set; }
        public Metric TMax { get; internal set; }

        public BsonDocument ReferenceDoc { get; internal set; }
        public int NumKeys { get; internal set; }
        public int NumDeltas { get; internal set; }

        public byte[] Deltas { get; internal set; }
    }

    public class FTDCFileContents
    {
        public DateTime _id { get; set; }
        public int type { get; set; }
        public BsonDocument doc { get; set; }
        public BsonBinaryData data { get; set; }

        public BsonDocument DecompressedData { get; internal set; }

        public void DecompressData()
        {
            byte[] raw = data.AsByteArray;
            byte[] compressed = new byte[raw.Length - 4];
            Array.Copy(raw, 4, compressed, 0, compressed.Length);

            using var inputstr = new MemoryStream(compressed);
            using (var stream = new ZLibNet.ZLibStream(inputstr, CompressionMode.Decompress))
            {
                const int size = 256;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    DecompressedData = BsonSerializer.Deserialize<BsonDocument>(memory.ToArray());
                }
            }
        }
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