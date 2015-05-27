using System;
using System.Collections.Generic;
using Atrico.Lib.CommandLineParser.Exceptions;

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
        ///     Gets the usage information for this type
        /// </summary>
        /// <typeparam name="T">Type of options class</typeparam>
        /// <returns>Usage info as multiple lines of text</returns>
        public static IEnumerable<string> GetUsage<T>() where T : class, new()
        {
            // TODO
            return new String[] {};
        }

        public class ValidationResult
        {
            public IEnumerable<string> Errors { get; private set; }
            public IEnumerable<string> Warnings { get; private set; }

            public ValidationResult(IEnumerable<string> errors, IEnumerable<string> warnings)
            {
                Errors = errors;
                Warnings = warnings;
            }
        }

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
            return new String[] {};
        }
    }
}