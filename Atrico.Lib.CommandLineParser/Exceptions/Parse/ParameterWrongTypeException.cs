using System;

namespace Atrico.Lib.CommandLineParser.Exceptions.Parse
{
    /// <summary>
    ///     A Required parameter is the wrong type
    /// </summary>
    public class ParameterWrongTypeException : ParseException
    {
        public ParameterWrongTypeException(string option, Type parameterType, string actual, Exception inner = null)
            : base(string.Format("Parameter was the wrong type: {0} requires parameter of type {1} ({2})", option, parameterType.Name, actual), inner)
        {
        }
    }
}