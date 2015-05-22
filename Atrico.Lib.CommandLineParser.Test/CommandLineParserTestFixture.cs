using System;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.CommandLineParser.Test
{
    [TestFixture]
    public abstract class CommandLineParserTestFixture : TestFixtureBase
    {
        protected static string[] CreateArgs(string commandLine)
        {
            return commandLine.Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}