using System;

namespace Atrico.Lib.CommandLineParser.Exceptions
{
    /// <summary>
    ///     Error whilst parsing
    /// </summary>
    public abstract class ParseException : CommandLineParserException
    {
        protected ParseException(string message, Exception inner = null)
            : base(message, inner)
        {
        }
    }
}