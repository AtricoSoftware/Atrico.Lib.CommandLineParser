using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.CommandLineParser.Tests
{
    [TestFixture]
    public class TestOptionOptionalWithDefaultBoolean : CommandLineParserTestFixture<TestOptionOptionalBoolean.Options>
    {
        public class Options
        {
            [Option(DefaultValue = true)]
            public bool Boolean { get; private set; }
        }

        public TestOptionOptionalWithDefaultBoolean()
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

        [Test]
        public void TestUsageSummary()
        {
            // Act
            var usage = Parser.GetUsage<Options>(Parser.UsageDetails.Summary).ToArray();

            // Assert
            foreach (var line in usage)
            {
                Debug.WriteLine(line);
            }
            Assert.That(Value.Of(usage).Count().Is().EqualTo(1), "Number of summary lines");
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("{0} [-Boolean]", ExeName)), "Summary");
        }

        [Test]
        public void TestUsageParameterDetails()
        {
            // Act
            var usage = Parser.GetUsage<Options>(Parser.UsageDetails.ParameterDetails).ToArray();

            // Assert
            foreach (var line in usage)
            {
                Debug.WriteLine(line);
            }
            Assert.That(Value.Of(usage).Count().Is().EqualTo(1), "Number of detail lines");
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("Boolean: (default = True)")), "Detail");
        }
    }
}
