using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Atrico.Lib.CommandLineParser.Attributes;

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
            var parser = new ParserT<T>(args);
            return parser.Result;
        }

        /// <summary>
        ///     Parses the command line with the options specified
        /// </summary>
        /// <typeparam name="T">Type of options class</typeparam>
        /// <returns>Populated options or null if error</returns>
        public static IEnumerable<string> GetUsage<T>() where T : class, new()
        {
            var parser = new ParserT<T>(args);
            return parser.Result;
        }
    }
}