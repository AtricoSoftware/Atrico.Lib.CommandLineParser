﻿using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture(typeof (char?))]
    [TestFixture(typeof (byte?))]
    [TestFixture(typeof (sbyte?))]
    [TestFixture(typeof (short?))]
    [TestFixture(typeof (ushort?))]
    [TestFixture(typeof (int?))]
    [TestFixture(typeof (uint?))]
    [TestFixture(typeof (long?))]
    [TestFixture(typeof (ulong?))]
    [TestFixture(typeof (float?))]
    [TestFixture(typeof (double?))]
    [TestFixture(typeof (string))]
    public class TestObjectOptionMandatory<T> : TestFixtureBase
    {
        private class Options
        {
            [Option(Required = true)]
            public T Number { get; set; }
        }

        [Test]
        public void TestPresent()
        {
            var value = RandomValues.Value<T>();
            // Arrange
            var args = Helpers.CreateArgs("-number {0}", value);

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Number).Is().EqualTo(value), "Value is correct");
        }

        [Test]
        public void TestMissing()
        {
            // Arrange
            var args = Helpers.CreateArgs("");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (MissingOptionException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }

        [Test]
        public void TestMissingParameter()
        {
            // Arrange
            var args = Helpers.CreateArgs("-number");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (MissingOptionParameterException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }

        [Test]
        public void TestParameterWrongType()
        {
            // No wrong type for a string!
            if (typeof (T) == typeof (string))
            {
                return;
            }
            // Arrange
            var args = Helpers.CreateArgs("-number text");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (OptionParameterWrongTypeException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }
    }
}
