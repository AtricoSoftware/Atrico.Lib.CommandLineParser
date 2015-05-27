using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.CommandLineParser
{
    /// <summary>
    ///     Command line parser entry point
    /// </summary>
    public static partial class Parser
    {
        /// <summary>
        ///     Validates the options class
        ///     Returns warnings (which will not stop the parser)
        ///     Throws exception if anything will stop the parser
        /// </summary>
        /// <typeparam name="T">Type of options</typeparam>
        /// <returns>List of warnings (if any)</returns>
        /// <exception cref="Atrico.Lib.CommandLineParser.Exceptions.InvalidOptionsPropertyException">
        ///     Thrown for first property
        ///     with an error
        /// </exception>
        public static IEnumerable<string> Validate<T>() where T : class, new()
        {
            var warnings = new List<string>();
            var options = GetOptionInformation<T>().ToArray();

            return warnings;
        }
    }
}