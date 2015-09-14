using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.CommandLineParser.Tests
{
    [TestFixture]
    public class TestPositionalMandatoryEnum : CommandLineParserTestFixture<TestPositionalMandatoryEnum.Options>
    {
        public enum OptionEnum
        {
            Zero,
            One,
            Two,
            Three
        }

        public class Options
        {
            [Option(Position = 0, Required = true)]
            public OptionEnum Opt { get; private set; }
        }

        [Test]
        public void TestPresent()
        {
            const OptionEnum value = OptionEnum.One;
            // Arrange
            var args = CreateArgs("{0}", value);

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Opt).Is().EqualTo(value), "Value is correct");
        }

        [Test]
        public void TestMissing()
        {
            // Arrange
            var args = CreateArgs("");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof(MissingOptionException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }

        [Test]
        public void TestParameterWrongType()
        {
            // Arrange
            var args = CreateArgs("text");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof(ParameterWrongTypeException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }

        // No wrong type for string

        [Test]
        public void TestUsageSummary()
        {
            // Act
            var usage = Parser.GetUsage<Options>(Parser.UsageDetails.Summary).ToArray();

            // Assert
            foreach (var line in usage) Debug.WriteLine(line);
            Assert.That(Value.Of(usage).Count().Is().EqualTo(1), "Number of summary lines");
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("{0} [-Opt] <{1}>", ExeName, typeof(OptionEnum).Name)), "Summary");
        }
    }
}
