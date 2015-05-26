using System;
using System.Collections.Generic;
using System.Text;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public abstract class CommandLineParserTestFixture : TestFixtureBase
    {
        private enum State
        {
            OutsideToken,
            InsideToken,
            InsideString
        }

        protected static string[] CreateArgs(string commandLine)
        {
            var tokens = new List<string>();
            var currentToken = new StringBuilder();
            var state = State.OutsideToken;
            foreach (var ch in commandLine)
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
            if (currentToken.Length > 0) tokens.Add(currentToken.ToString());

            return tokens.ToArray();
        }
    }
}