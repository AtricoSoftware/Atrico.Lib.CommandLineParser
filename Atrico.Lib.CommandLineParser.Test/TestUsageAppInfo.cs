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
    public class TestUsageAppInfo : CommandLineParserTestFixture
    {
        public class Options
        {
            [Option]
            public bool Boolean { get; private set; }
        }

        [Test]
        public void TestAssemblyVersion()
        {
            // Act
            var usage = Parser.GetUsage<Options>(Parser.UsageDetails.AppInfo).ToArray();

            // Assert
            foreach (var line in usage)
            {
                Debug.WriteLine(line);
            }
            Assert.That(Value.Of(usage).Count().Is().GreaterThanOrEqualTo(1), "Number of lines");
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("{0} {1}", AssemblyName, AssemblyVersion)), "Assembly version");
        }

        [Test]
        public void TestCopyright()
        {
            // Act
            var usage = Parser.GetUsage<Options>(Parser.UsageDetails.AppInfo).ToArray();

            // Assert
            foreach (var line in usage)
            {
                Debug.WriteLine(line);
            }
            Assert.That(Value.Of(usage).Count().Is().GreaterThanOrEqualTo(2), "Number of lines");
            Assert.That(Value.Of(usage[1]).Is().EqualTo(AssemblyCopyright), "Assembly copyright");
        }

        [Test]
        public void TestNoMore()
        {
            // Act
            var usage = Parser.GetUsage<Options>(Parser.UsageDetails.AppInfo).ToArray();

            // Assert
            foreach (var line in usage)
            {
                Debug.WriteLine(line);
            }
            Assert.That(Value.Of(usage).Count().Is().LessThanOrEqualTo(2), "Number of lines");
        }

        [Test]
        public void TestEmptyLineIfFollowed()
        {
            // Act
            var usage = Parser.GetUsage<Options>(Parser.UsageDetails.Full).ToArray();

            // Assert
            foreach (var line in usage)
            {
                Debug.WriteLine(line);
            }
            Assert.That(Value.Of(usage).Count().Is().GreaterThanOrEqualTo(3), "Number of lines");
            Assert.That(Value.Of(usage[2]).Is().EqualTo(string.Empty), "Empty line");
        }
    }
}
