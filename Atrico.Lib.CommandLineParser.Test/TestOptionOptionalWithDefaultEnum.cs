﻿using System;
using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.CommandLineParser.Test
{
    public class TestOptionOptionalWithDefaultEnum : CommandLineParserTestFixture<TestOptionOptionalWithDefaultEnum.Options>
    {
        public enum OptionEnum
        {
            Zero, One, Two, Three
        }
        public class Options
        {
            [Option(DefaultValue = OptionEnum.Two)]
            public OptionEnum Opt { get; private set; }
        }

        [Test]
        public void TestPresent()
        {
            const OptionEnum value = OptionEnum.One;
            // Arrange
            var args = CreateArgs("-opt {0}", value);

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
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Opt).Is().EqualTo(OptionEnum.Two), "Value is correct");
        }

        [Test]
        public void TestMissingParameter()
        {
            // Arrange
            var args = CreateArgs("-opt");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof(MissingParameterException)), "Exception thrown");
            Debug.WriteLine(ex.Message);

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (MissingParameterException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }

        [Test]
        public void TestParameterWrongType()
        {
            // Arrange
            var args = CreateArgs("-opt text");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (ParameterWrongTypeException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
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
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("{0} [-Opt <{1}>]", ExeName, typeof(OptionEnum).Name)), "Summary");
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
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("Opt: (default = {0})", "Two")), "Detail");
        }
    }
}
