using System;
using System.Collections.Generic;
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
    public class TestValidateOptions : CommandLineParserTestFixture
    {
        public static class Options
        {
            public class Empty
            {
            }

            public class Valid
            {
                [Option]
                private bool Boolean { get; set; }
            }
        }

        [Test]
        public void TestValid()
        {
            Implementation<Options.Valid>(true, false);
        }

        [Test]
        public void TestEmpty()
        {
            Implementation<Options.Empty>(true, false);
        }

        public void Implementation<T>(bool isValid, bool hasWarnings) where T : class, new()
        {
            // Arrange
            IEnumerable<string> warnings = new[] {"Should be replaced"};

            // Act
            var ex = Catch.Exception(() => warnings = Parser.Validate<T>());

            // Assert 
            if (isValid) Assert.That(Value.Of(ex).Is().Null(), "No Error");
            else
            {
                Assert.That(Value.Of(ex).Is().TypeOf(typeof (InvalidOptionsPropertyException)), "Error");
                Debug.WriteLine(string.Format("ERROR: {0}", ex.Message));
            }
            if (!hasWarnings) Assert.That(Value.Of(warnings).Is().EqualTo(new String[] {}), "No Warnings");
            else
            {
                Assert.That(Value.Of(warnings).Count().Is().GreaterThan(0), "Warnings");
                foreach (var warning in warnings) Debug.WriteLine(string.Format("WARNING: {0}", warning));
            }
        }
    }
}