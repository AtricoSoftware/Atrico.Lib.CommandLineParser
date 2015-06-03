using System;

namespace Atrico.Lib.CommandLineParser.Exceptions.Parse
{
    /// <summary>
    ///     A Required parameter was missing
    /// </summary>
    public class MissingParameterException : ParseException
    {
        public MissingParameterException(string option, Type parameterType)
            : base(string.Format("A Required parameter was missing: {0} requires parameter of type {1}", option, parameterType.Name))
        {
        }
    }
}