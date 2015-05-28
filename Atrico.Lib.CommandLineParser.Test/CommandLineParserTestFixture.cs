using System;
using System.Collections.Generic;
using System.Text;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    public abstract class CommandLineParserTestFixture : TestFixtureBase
    {
        private enum State
        {
            OutsideToken,
            InsideToken,
            InsideString
        }

        public static string[] CreateArgs(string commandLine, params object[] args)
        {
            var tokens = new List<string>();
            var currentToken = new StringBuilder();
            var state = State.OutsideToken;
            foreach (var ch in string.Format(commandLine, args))
            {
                switch (state)
                {
                    case State.OutsideToken:
                    {
                        if (ch == '\'')
                        {
                            state = State.InsideString;
                        }
                        else if (Char.IsWhiteSpace(ch))
                        {
                            // Ignore!
                        }
                        else
                        {
                            currentToken.Clear();
                            currentToken.Append(ch);
                            state = State.InsideToken;
                        }
                        break;
                    }
                    case State.InsideToken:
                    {
                        if (ch == '\'')
                        {
                            throw new Exception("token cannot contain ' except as first/last character");
                        }
                        if (Char.IsWhiteSpace(ch))
                        {
                            tokens.Add(currentToken.ToString());
                            currentToken.Clear();
                            state = State.OutsideToken;
                        }
                        else
                        {
                            currentToken.Append(ch);
                        }
                    }
                        break;
                    case State.InsideString:
                    {
                        if (ch == '\'')
                        {
                            tokens.Add(currentToken.ToString());
                            currentToken.Clear();
                            state = State.OutsideToken;
                        }
                        else
                        {
                            currentToken.Append(ch);
                        }
                    }
                        break;
                }
            }
            if (currentToken.Length > 0)
            {
                tokens.Add(currentToken.ToString());
            }

            return tokens.ToArray();
        }
    }

    public abstract class CommandLineParserTestFixture<T> : CommandLineParserTestFixture where T : class, new()
    {
        private readonly bool _allowWarnings;

        protected CommandLineParserTestFixture(bool allowWarnings = false)
        {
            _allowWarnings = allowWarnings;
        }

        [Test]
        public void ValidateOptions()
        {
            // Arrange
            IEnumerable<string> warnings = new[] {"Should be replaced"};

            // Act
            var ex = Catch.Exception(() => warnings = Parser.Validate<T>());

            // Assert 
            Assert.That(Value.Of(ex).Is().Null(), "No Errors");
            if (!_allowWarnings) Assert.That(Value.Of(warnings).Is().EqualTo(new String[] {}), "No Warnings");
        }
    }
}