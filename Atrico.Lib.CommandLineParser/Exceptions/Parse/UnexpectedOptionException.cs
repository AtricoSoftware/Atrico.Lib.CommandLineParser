namespace Atrico.Lib.CommandLineParser.Exceptions.Parse
{
    /// <summary>
    ///     An Option was not expected
    /// </summary>
    public class UnexpectedOptionException : ParseException
    {
        public UnexpectedOptionException(string option)
            : base(string.Format("An unexpected option was specified: {0}", option))
        {
        }
    }
}