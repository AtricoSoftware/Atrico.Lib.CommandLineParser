using System;

namespace Atrico.Lib.CommandLineParser.Exceptions
{
    /// <summary>
    ///     A Required  Option parameter was missing
    /// </summary>
    public class MissingOptionParameterException : CommandLineParserException
    {
        public MissingOptionParameterException(string option, Type parameterType)
            : base(string.Format("A Required option parameter was missing: {0} requires parameter of type {1}", option, parameterType))
        {
        }
    }

    /// <summary>
    ///     A Required  Option parameter was missing
    /// </summary>
    public class MissingOptionParameterException<T> : MissingOptionParameterException
    {
        public MissingOptionParameterException(string option)
            : base(option, typeof (T))
        {
        }
    }
}
