using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public abstract class CommandLineParserTestFixture : TestFixtureBase
    {
        protected static string[] CreateArgs(string commandLine)
        {
            return new string[] {};
        }
    }

    [TestFixture]
    public class TestBooleanSwitch : CommandLineParserTestFixture
    {
        private class Options
        {
            [Option]
            public bool Boolean { get; set; }
        }

        [Test]
        public void TestMissingOptional()
        {
            // Arrange
            var args = CreateArgs("");
            var parser = new Parser();

            // Act
            var options = parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result not null");
            Assert.That(Value.Of(options.Boolean).Is().False(), "Switch is false");
        }
    }
}