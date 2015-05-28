using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public class TestBooleanOptionOptionalWithDefault : CommandLineParserTestFixture<TestBooleanOptionOptional.Options>
    {
        public class Options
        {
            [Option(DefaultValue = true)]
            public bool Boolean { get; private set; }
        }

        public TestBooleanOptionOptionalWithDefault()
            : base(true)
        {
            
        }
        [Test]
        public void TestPresent()
        {
            // Arrange
            var args = CreateArgs("-boolean");

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
            var args = CreateArgs("");

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Boolean).Is().True(), "Switch is true");
        }
    }
}