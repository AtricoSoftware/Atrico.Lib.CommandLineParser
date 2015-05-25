using System;

namespace Atrico.Lib.CommandLineParser.Exceptions
{
    /// <summary>
    ///     Base class for all CLP exceptions
    /// </summary>
    public class CommandLineParserException : Exception
    {
        protected CommandLineParserException(string message)
            : base(message)
        {
        }
    }
}
