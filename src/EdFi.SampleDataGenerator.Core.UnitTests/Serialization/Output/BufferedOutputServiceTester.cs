using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output
{
    [TestFixture]
    public class BufferedOutputServiceTester
    {
        private class TestBufferedOutputService : BufferedOutputService<string, string>
        {
            public int WriteToOutputFileCallCount { get; private set; } = 0;
            public int FlushCountCallCount { get; private set; } = 0;

            public List<string> TestBuffer => OutputBuffer; 

            public override void FlushOutput()
            {
                ++FlushCountCallCount;
                base.FlushOutput();
            }

            protected override void WriteOutputToFile()
            {
                ++WriteToOutputFileCallCount;
            }
        }

        [Test]
        public void ShouldFlushOnConfiguration()
        {
            var sut = new TestBufferedOutputService();
            sut.Configure("test");
            sut.WriteToOutput("test");

            sut.FlushCountCallCount.ShouldBe(1);
        }

        [Test]
        public void ShouldAddItemToBuffer()
        {
            var sut = new TestBufferedOutputService();
            sut.Configure("test");
            sut.WriteToOutput("test");

            sut.TestBuffer.Count.ShouldBe(1);
            sut.TestBuffer[0].ShouldBe("test");
        }

        [Test]
        public void ShouldWriteToOutputFileOnFlush()
        {
            var sut = new TestBufferedOutputService();
            sut.Configure("test");
            sut.WriteToOutput("test");

            sut.FlushOutput();

            sut.FlushCountCallCount.ShouldBe(2);
            sut.WriteToOutputFileCallCount.ShouldBe(1);
        }

        [Test]
        public void ShouldClearBufferOnFlush()
        {
            var sut = new TestBufferedOutputService();
            sut.Configure("test");
            sut.WriteToOutput("test");

            sut.FlushOutput();

            sut.TestBuffer.Count.ShouldBe(0);
        }
    }
}
