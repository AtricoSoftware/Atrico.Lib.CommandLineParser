using System;

namespace Atrico.Lib.CommandLineParser.Exceptions.Parse
{
    /// <summary>
    ///     A Required Option parameter was missing
    /// </summary>
    public class MissingOptionParameterException : ParseException
    {
        public MissingOptionParameterException(string option, Type parameterType)
            : base(string.Format("A Required option parameter was missing: {0} requires parameter of type {1}", option, parameterType.Name))
        {
        }
    }
}