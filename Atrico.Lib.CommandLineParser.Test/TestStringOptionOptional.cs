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
    [TestFixture]
    public class TestStringOptionOptional : TestFixtureBase
    {
        private class Options
        {
            [Option]
            public string Text { get; set; }
        }

        [Test]
        public void TestPresent()
        {
            const string theText = "Some Text";
            // Arrange
            var args = Helpers.CreateArgs(string.Format("-text '{0}'", theText));

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
            var args = Helpers.CreateArgs("");

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Text).Is().Null(), "Value is null");
        }

        [Test]
        public void TestMissingParameter()
        {
            // Arrange
            var args = Helpers.CreateArgs("-text");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof(MissingOptionParameterException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }
    }
}
