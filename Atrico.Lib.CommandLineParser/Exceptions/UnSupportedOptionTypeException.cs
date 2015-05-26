using System;

namespace Atrico.Lib.CommandLineParser.Exceptions
{
    /// <summary>
    ///     An Option had an unsupported type
    /// </summary>
    public class UnSupportedOptionTypeException : CommandLineParserException
    {
        public UnSupportedOptionTypeException(string option, Type type)
            : base(string.Format("An option was specified with an unsupported type: {0} ({1})", option, type.Name))
        {
        }
    }
}
