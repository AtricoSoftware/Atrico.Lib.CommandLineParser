using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions.Options;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.CommandLineParser.Tests
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

            public class OptionalNonNullableWithDefault
            {
                [Option(DefaultValue = 123)]
                public int Property { get; private set; }
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

            public class OptionalNonNullableNoDefault
            {
                [Option]
                public int Property { get; private set; }
            }

            public class DefaultValueWrongType
            {
                [Option(DefaultValue = "text")]
                public int Property { get; private set; }
            }

            public class DuplicatePositions
            {
                [Option(Position = 0, Required = true)]
                public int Property1 { get; private set; }

                [Option(Position = 0, Required = true)]
                public int Property2 { get; private set; }
            }

            public class PositionOnBoolean
            {
                [Option(Position = 0)]
                public bool Property { get; private set; }
            }

            public class MandatoryPositionFollowsOptionalPosition
            {
                [Option(Position = 0)]
                public string Property1 { get; private set; }

                [Option(Position = 1, Required = true)]
                public string Property2 { get; private set; }
            }

            #endregion

            #region Warnings

            public class MandatoryWithDefault
            {
                [Option(Required = true, DefaultValue = 123)]
                public int Property { get; private set; }
            }

            public class NullableWithDefault
            {
                [Option(DefaultValue = 123)]
                public int? Property { get; private set; }
            }

            public class BooleanWithDefault
            {
                [Option(DefaultValue = true)]
                public bool Property { get; private set; }
            }

            public class MandatoryNullable
            {
                [Option(Required = true)]
                public int? Property { get; private set; }
            }

            #endregion
        }

        #region Valid

        [Test]
        public void TestEmpty()
        {
            Implementation<Options.Empty>();
        }

        [Test]
        public void TestValid()
        {
            Implementation<Options.Valid>();
        }

        [Test]
        public void TestOptionalNonNullableWithDefault()
        {
            Implementation<Options.OptionalNonNullableWithDefault>();
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
        public void TestOptionalNonNullableNoDefault()
        {
            Implementation<Options.OptionalNonNullableNoDefault, OptionalNonNullableException>();
        }

        [Test]
        public void TestDefaultValueWrongType()
        {
            Implementation<Options.DefaultValueWrongType, DefaultValueWrongTypeException>();
        }

        [Test]
        public void TestDuplicatePositions()
        {
            Implementation<Options.DuplicatePositions, DuplicatePositionsException>();
        }

        [Test]
        public void TestPositionOnBoolean()
        {
            Implementation<Options.PositionOnBoolean, PositionOnBooleanException>();
        }

        [Test]
        public void TestMandatoryPositionFollowsOptionalPosition()
        {
            Implementation<Options.MandatoryPositionFollowsOptionalPosition, MandatoryPositionFollowsOptionalPositionException>();
        }

        #endregion

        #region Warnings

        [Test]
        public void TestMandatoryWithDefault()
        {
            Implementation<Options.MandatoryWithDefault>(true);
        }

        [Test]
        public void TestNullableWithDefault()
        {
            Implementation<Options.NullableWithDefault>(true);
        }

        [Test]
        public void TestBooleanWithDefault()
        {
            Implementation<Options.BooleanWithDefault>(true);
        }

        [Test]
        public void TestMandatoryNullable()
        {
            Implementation<Options.MandatoryNullable>(true);
        }

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
                foreach (var warning in warnings)
                {
                    Debug.WriteLine(string.Format("WARNING: {0}", warning));
                }
            }
            else
            {
                Assert.That(Value.Of(warnings).Is().EqualTo(new String[] {}), "No Warnings");
            }
        }
    }
}
