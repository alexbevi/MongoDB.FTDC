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
    }
}