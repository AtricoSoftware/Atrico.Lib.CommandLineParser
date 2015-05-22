namespace Atrico.Lib.CommandLineParser
{
    /// <summary>
    ///     Command line parser
    /// </summary>
    public class Parser
    {
        /// <summary>
        ///     Parses the command line with the options specified
        /// </summary>
        /// <typeparam name="T">Type of options class</typeparam>
        /// <returns>Populated options or null if error</returns>
        public T Parse<T>(string[] args) where T : class, new()
        {
            return new T();
        }
    }
}