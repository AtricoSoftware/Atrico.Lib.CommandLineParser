namespace Atrico.Lib.CommandLineParser.Exceptions
{
    /// <summary>
    ///     An Option was not expected
    /// </summary>
    public class UnexpectedOptionException : CommandLineParserException
    {
        public UnexpectedOptionException(string option)
            : base(string.Format("An unexpected option was specified: {0}", option))
        {
        }
    }
}