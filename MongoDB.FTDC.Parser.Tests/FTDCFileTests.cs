using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace MongoDB.FTDC.Parser.Tests
{
    [TestClass]
    public class FTDCFileTests
    {
        [TestMethod]
        public void FileOpenShouldFailWithInvalidPath()
        {
            var ftdc = new FTDCFile();

            Should.Throw<FTDCException>(() => ftdc.Open("invalid path xxx"));
        }

        [TestMethod]
        public void FileOpenShouldLoadAValidPath()
        {
            var ftdc = new FTDCFile();
            ftdc.Open(@"diagnostic.data\metrics.2020-01-02T11-02-43Z-00000");
            ftdc.Contents.Count.ShouldNotBe(0);
        }

        [TestMethod]
        public void ParsedFTDCFileShouldDecodeData()
        {
            var ftdc = new FTDCFile();
            ftdc.Open(@"diagnostic.data\metrics.2020-01-02T11-02-43Z-00000");
            ftdc.Contents.Count.ShouldNotBe(0);

            Should.NotThrow(() => ftdc.Contents[1].DecompressData());
        }
    }
}