namespace Atrico.Lib.CommandLineParser.Exceptions.Parse
{
    /// <summary>
    ///     A Required  Option was missing
    /// </summary>
    public class MissingOptionException : ParseException
    {
        public MissingOptionException(string option)
            : base(string.Format("A Required option was missing: {0}", option))
        {
        }
    }
}