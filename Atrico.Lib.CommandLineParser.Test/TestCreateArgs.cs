using System.Collections.Generic;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public class TestCreateArgs : CommandLineParserTestFixture
    {
        private static void TestImplementation(string commandline, IEnumerable<string> expected)
        {
            // Act
            var args = CreateArgs(commandline);

            // Assert
            Assert.That(Value.Of(args).Is().EqualTo(expected), "Correct args");
        }

        [Test]
        public void TestEmpty()
        {
            TestImplementation("", new string[] {});
        }

        [Test]
        public void TestSimpleElements()
        {
            TestImplementation("-a -b cdef --g /h", new[] {"-a", "-b", "cdef", "--g", "/h"});
        }

        [Test]
        public void TestQuotedString()
        {
            TestImplementation("-a 'string'", new[] {"-a", "string"});
        }

        [Test]
        public void TestQuotedStringWithSpace()
        {
            TestImplementation("-a 'string with space'", new[] {"-a", "string with space"});
        }

        [Test]
        public void TestQuotedStringWithMultipleSpaces()
        {
            TestImplementation("-a 'string  with  spaces'", new[] {"-a", "string  with  spaces"});
        }
    }
}
