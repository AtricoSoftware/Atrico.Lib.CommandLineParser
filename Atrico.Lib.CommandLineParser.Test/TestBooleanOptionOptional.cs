using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public class TestBooleanOptionOptional : TestFixtureBase
    {
        private class Options
        {
            [Option]
            public bool Boolean { get; set; }
        }

        [Test]
        public void TestPresent()
        {
            // Arrange
            var args = Helpers.CreateArgs("-boolean");

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Boolean).Is().True(), "Switch is true");
        }

        [Test]
        public void TestMissing()
        {
            // Arrange
            var args = Helpers.CreateArgs("");

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Boolean).Is().False(), "Switch is false");
        }
    }
}