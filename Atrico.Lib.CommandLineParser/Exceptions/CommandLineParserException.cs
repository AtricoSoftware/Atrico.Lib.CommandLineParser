using System;

namespace Atrico.Lib.CommandLineParser.Exceptions
{
    /// <summary>
    ///     Base class for all CLP exceptions
    /// </summary>
    public abstract class CommandLineParserException : Exception
    {
        protected CommandLineParserException(string message, Exception inner = null)
            : base(message, inner)
        {
        }
    }
}