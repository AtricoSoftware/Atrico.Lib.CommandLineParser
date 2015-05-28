using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public class TestInvalidCommandLine : CommandLineParserTestFixture<TestInvalidCommandLine.Options>
    {
        public class Options
        {
            [Option]
            public bool Ab { get; private set; }
        }

        [Test]
        public void TestUnexpectedOption()
        {
            // Arrange
            var args = CreateArgs("-ac");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (UnexpectedOptionException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }
    }
}