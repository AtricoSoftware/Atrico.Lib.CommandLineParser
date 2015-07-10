﻿using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public class TestPositionalOptionalWithDefaultString : CommandLineParserTestFixture<TestOptionOptionalString.Options>
    {
        public class Options
        {
            [Option(Position = 0, DefaultValue = "default")]
            public string Text { get; private set; }
        }

        [Test]
        public void TestPresent()
        {
            const string theText = "Some Text";
            // Arrange
            var args = CreateArgs(string.Format("'{0}'", theText));

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Text).Is().EqualTo(theText), "Value is correct");
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
            Assert.That(Value.Of(options.Text).Is().EqualTo("default"), "Value is default");
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
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("{0} [[-Text] <{1}>]", ExeName, typeof (string).Name)), "Summary");
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
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("Text: (default = default)")), "Detail");
        }
    }
}