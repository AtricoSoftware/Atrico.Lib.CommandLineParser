using System.Collections;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public class TestBooleanSwitch : CommandLineParserTestFixture
    {
        private class Options
        {
            [Option]
            public bool Boolean { get; set; }
        }

        [Test]
        public void TestOptionalMissing()
        {
            // Arrange
            var args = CreateArgs("");

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result not null");
            Assert.That(Value.Of(options.Boolean).Is().False(), "Switch is false");
        }

        [Test]
        public void TestOptionalPresent()
        {
            // Arrange
            var args = CreateArgs("-boolean");

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result not null");
            Assert.That(Value.Of(options.Boolean).Is().True(), "Switch is true");
        }
    }
}