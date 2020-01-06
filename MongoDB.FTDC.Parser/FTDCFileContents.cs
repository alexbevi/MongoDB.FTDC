using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MongoDB.FTDC.Parser
{
    using CompressionMode = ZLibNet.CompressionMode;

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
            using var stream = new ZLibNet.ZLibStream(inputstr, CompressionMode.Decompress);
            const int size = 256;
            byte[] buffer = new byte[size];
            using MemoryStream memory = new MemoryStream();
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