using System;

namespace Atrico.Lib.CommandLineParser.Exceptions.Parse
{
    /// <summary>
    ///     A Required Option parameter is the wrong type
    /// </summary>
    public class OptionParameterWrongTypeException : ParseException
    {
        public OptionParameterWrongTypeException(string option, Type parameterType, string actual, Exception inner = null)
            : base(string.Format("Option parameter was the wrong type: {0} requires parameter of type {1} ({2})", option, parameterType.Name, actual), inner)
        {
        }
    }
}