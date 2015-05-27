using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions.Options;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    public class TestValidateOptions : CommandLineParserTestFixture
    {
        public static class Options
        {
            #region Valid

            public class Empty
            {
            }

            public class Valid
            {
                [Option]
                public bool Property { get; private set; }
            }

            #endregion

            #region Invalid

            public class UnsupportedType
            {
                public class NotSupported
                {
                }

                [Option]
                public NotSupported Property { get; set; }
            }

            public class NoSetter
            {
                [Option]
                public bool Property
                {
                    get { return false; }
                }
            }

            public class NoDefaultForOptionalNonNullable
            {
                [Option]
                public int Property { get; private set; }
            }

            #endregion

            #region Warnings
 
            #endregion
        }

        #region Valid

        [Test]
        public void TestValid()
        {
            Implementation<Options.Valid>();
        }

        [Test]
        public void TestEmpty()
        {
            Implementation<Options.Empty>();
        }

        #endregion

        #region Invalid

        [Test]
        public void TestUnsupportedType()
        {
            Implementation<Options.UnsupportedType, UnSupportedTypeException>();
        }

        [Test]
        public void TestNoSetter()
        {
            Implementation<Options.NoSetter, NoSetterException>();
        }

        [Test]
        public void TestNoDefaultForOptionalNonNullable()
        {
            Implementation<Options.NoDefaultForOptionalNonNullable, Exception>();
        }

        #endregion

        #region Warnings

        #endregion

        private static void Implementation<TOpt, TEx>() where TOpt : class, new()
        {
            // Arrange
            IEnumerable<string> warnings = new[] {"Should be replaced"};

            // Act
            var ex = Catch.Exception(() => warnings = Parser.Validate<TOpt>());
            var exParser = Catch.Exception(() => Parser.Parse<TOpt>(new string[] {}));
            // Assert
            Assert.That(Value.Of(ex).Is().TypeOf(typeof (TEx)), "Error");
            Debug.WriteLine(string.Format("ERROR: {0}", ex.Message));
            Assert.That(Value.Of(exParser).Is().TypeOf(typeof (TEx)), "Error same from parser itself");
        }

        private static void Implementation<TOpt>(bool hasWarnings = false) where TOpt : class, new()
        {
            // Arrange
            IEnumerable<string> warnings = new[] {"Should be replaced"};

            // Act
            var ex = Catch.Exception(() => warnings = Parser.Validate<TOpt>());
            var exParser = Catch.Exception(() => Parser.Parse<TOpt>(new string[] {}));

            // Assert
            Assert.That(Value.Of(ex).Is().Null(), "No Error");
            if (hasWarnings)
            {
                Assert.That(Value.Of(warnings).Count().Is().GreaterThan(0), "Warnings");
                foreach (var warning in warnings) Debug.WriteLine(string.Format("WARNING: {0}", warning));
            }
            else
            {
                Assert.That(Value.Of(warnings).Is().EqualTo(new String[] {}), "No Warnings");
            }
        }
    }
}