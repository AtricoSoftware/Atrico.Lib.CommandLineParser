using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public class TestMinimumUniqueName : CommandLineParserTestFixture<TestMinimumUniqueName.Options>
    {
        public class Options
        {
            [Option]
            public bool Ab { get; private set; }

            [Option]
            public bool Abcd { get; private set; }

            [Option]
            public bool Abdc { get; private set; }
        }

        [Test]
        public void TestAmbiguous()
        {
            // Arrange
            var args = CreateArgs("-a");

            // Act
            var ex = Catch.Exception(() => Parser.Parse<Options>(args));

            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (AmbiguousOptionException)), "Exception thrown");
            Debug.WriteLine(ex.Message);
        }

        [Test]
        public void TestExactButSubset()
        {
            // Arrange
            var args = CreateArgs("-ab");

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Ab).Is().True(), "ab is true");
            Assert.That(Value.Of(options.Abcd).Is().False(), "abcd is false");
            Assert.That(Value.Of(options.Abdc).Is().False(), "abdc is false");
        }

        [Test]
        public void TestMinimumUnique()
        {
            // Arrange
            var args = CreateArgs("-abc");

            // Act
            var options = Parser.Parse<Options>(args);

            // Assert
            Assert.That(Value.Of(options).Is().Not().Null(), "Result is not null");
            Assert.That(Value.Of(options.Ab).Is().False(), "ab is false");
            Assert.That(Value.Of(options.Abcd).Is().True(), "abcd is true");
            Assert.That(Value.Of(options.Abdc).Is().False(), "abdc is false");
        }
    }
}