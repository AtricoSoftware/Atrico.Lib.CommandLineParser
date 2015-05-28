using System;
using System.Collections.Generic;

namespace Atrico.Lib.CommandLineParser
{
    /// <summary>
    ///     Command line parser entry point
    /// </summary>
    public static partial class Parser
    {
        /// <summary>
        ///     Parses the command line with the options specified
        /// </summary>
        /// <typeparam name="T">Type of options class</typeparam>
        /// <returns>Populated options or null if error</returns>
        public static T Parse<T>(string[] args) where T : class, new()
        {
            return new ParserT<T>(args).Result;
        }
    }
}