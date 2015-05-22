using System;
using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions;
using Atrico.Lib.Testing;
using NUnit.Framework;
using Assert = Atrico.Lib.Assertions.Assert;

namespace Atrico.Lib.CommandLineParser.Test
{
    [Testing.NUnitAttributes.TestFixture]
    public class TestMandatoryBooleanSwitch : CommandLineParserTestFixture
    {
        private class Options
        {
            [Option(Required=true)]
            public bool Boolean { get; set; }
        }

        [Testing.NUnitAttributes.Test]
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

        [Testing.NUnitAttributes.Test]
        public void TestMissing()
        {
            // Arrange
            var args = CreateArgs("");

            // Act
            var ex = Catch.Exception(()=>Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof(MissingOptionException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }
    }
}