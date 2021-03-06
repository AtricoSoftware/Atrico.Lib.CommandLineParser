﻿using System.Diagnostics;
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
    [TestFixture(typeof (char))]
    [TestFixture(typeof (byte))]
    [TestFixture(typeof (sbyte))]
    [TestFixture(typeof (short))]
    [TestFixture(typeof (ushort))]
    [TestFixture(typeof (int))]
    [TestFixture(typeof (uint))]
    [TestFixture(typeof (long))]
    [TestFixture(typeof (ulong))]
    [TestFixture(typeof (float))]
    [TestFixture(typeof (double))]
    public class TestOptionOptionalPodNullable<T> : CommandLineParserTestFixture<TestOptionOptionalPodNullable<T>.Options> where T : struct
    {
        public class Options
        {
            [Option]
            public T? Nullable { get; private set; }
        }

        [Test]
        public void TestPresent()
        {
            RandomValues.DefaultCharsToInclude = RandomValueGenerator.CharsToInclude.AlphaNumeric;
            var value = RandomValues.Value<T>();
            // Arrange
            var args = CreateArgs("-nullable {0}", value);

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Nullable).Is().EqualTo(value), "Value is correct");
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
            Assert.That(Value.Of(options.Nullable.HasValue).Is().False(), "Value is null");
        }

        [Test]
        public void TestMissingParameter()
        {
            // Arrange
            var args = CreateArgs("-nullable");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (MissingParameterException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }

        [Test]
        public void TestParameterWrongType()
        {
            // Arrange
            var args = CreateArgs("-nullable text");

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
            Assert.That(Value.Of(usage[0]).Is().EqualTo(string.Format("{0} [-Nullable <{1}?>]", ExeName, typeof (T).Name)), "Summary");
        }
    }
}
