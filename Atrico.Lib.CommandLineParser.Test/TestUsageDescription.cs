using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public class TestUsageDescription : CommandLineParserTestFixture
    {
        private const string _description = "Brief help text for parameter";

        public class OptionsNoDefault
        {
            [Option(Description = _description)]
            public bool Boolean { get; private set; }
        }

        public class OptionsWithDefault
        {
            [Option(Description = _description, DefaultValue = true)]
            public bool Boolean { get; private set; }
        }

        [Test]
        public void TestNoDefault()
        {
            // Act
            var usage = Parser.GetUsage<OptionsNoDefault>(Parser.UsageDetails.ParameterDetails).ToArray();

            // Assert
            foreach (var line in usage) Debug.WriteLine(line);
            Assert.That(Value.Of(usage).Count().Is().EqualTo(1), "Number of lines");
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("Boolean: {0}", _description, "Description")));
        }

        [Test]
        public void TestWithDefault()
        {
            // Act
            var usage = Parser.GetUsage<OptionsWithDefault>(Parser.UsageDetails.ParameterDetails).ToArray();

            // Assert
            foreach (var line in usage) Debug.WriteLine(line);
            Assert.That(Value.Of(usage).Count().Is().EqualTo(1), "Number of lines");
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("Boolean: {0} (default = True)", _description, "Description and default")));
        }
    }
}