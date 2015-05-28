using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Atrico.Lib.CommandLineParser
{
    /// <summary>
    ///     Command line parser entry point
    /// </summary>
    public static partial class Parser
    {
        private static readonly string _exeName;

        static Parser()
        {
            var assem = Assembly.GetEntryAssembly();
            _exeName = assem != null ? assem.GetName().Name : "UNKNOWN";

        }

        /// <summary>
        ///     Gets the usage information for this type
        /// </summary>
        /// <typeparam name="T">Type of options class</typeparam>
        /// <returns>Usage info as multiple lines of text</returns>
        public static IEnumerable<string> GetUsage<T>() where T : class, new()
        {
            var line = new StringBuilder(_exeName);
            foreach (var option in GetOptionInformation<T>())
            {
                line.AppendFormat(" {0}", option);
            }
            return new[] {line.ToString()};
        }

    }
}