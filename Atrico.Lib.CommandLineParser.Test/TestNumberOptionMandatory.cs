using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    public class TestNumberOptionMandatory<T> : TestPODTypes<T> where T : struct
    {
        private readonly T _value;

        private class Options
        {
            [Option(Required = true)]
            public T Number { get; set; }
        }

        public TestNumberOptionMandatory(T value)
        {
            _value = value;
        }

        [Test]
        public void TestPresent()
        {
            // Arrange
            var args = Helpers.CreateArgs(string.Format("-number '{0}'", _value));

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Number).Is().EqualTo(_value), "Value is correct");
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