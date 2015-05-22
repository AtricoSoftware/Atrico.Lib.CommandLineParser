namespace Atrico.Lib.CommandLineParser.Exceptions
{
    /// <summary>
    ///     A Required  Option was missing
    /// </summary>
    public class MissingOptionException : CommandLineParserException
    {
        public MissingOptionException(string option)
            : base(string.Format("A Required option was missing: {0}", option))
        {
        }
    }
}