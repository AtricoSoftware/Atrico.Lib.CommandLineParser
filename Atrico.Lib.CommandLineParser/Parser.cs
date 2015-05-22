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
    }

    public static partial class Parser
    {
        /// <summary>
        ///     Command line parser
        /// </summary>
        private class ParserT<T> where T : class, new()
        {
            private const string _switch = "-";

            private readonly Lazy<T> _result;
            private readonly IDictionary<string, PropertyInfo> _switches = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
            private readonly string[] _args;

            public T Result { get { return _result.Value; } }

            public ParserT(IEnumerable<string> args)
            {
                _args = args.ToArray();
                _result = new Lazy<T>(FitArguments);
                _switches = AnalyseSwitches();
            }

            private static IDictionary<string, PropertyInfo> AnalyseSwitches()
            {
                var switches = new Dictionary<string, PropertyInfo>();
                // Find properties with attribute
                foreach (var property in typeof (T).GetProperties().Where(p => p.CanWrite))
                {
                    var attribute = property.GetCustomAttributes(typeof (OptionAttribute), false).FirstOrDefault();
                    if (attribute != null) switches.Add(property.Name.ToLower(), property);
                }
                return switches;
            }

            private T FitArguments()
            {
                var options = new T();
                for (var i = 0; i < _args.Length; ++i)
                {
                    var sw = IsSwitch(_args[i]);
                    PropertyInfo prop;
                    if (sw != null && _switches.TryGetValue(sw, out prop))
                    {
                        prop.SetValue(options, true);
                    }
                }
                return options;
            }

            private static string IsSwitch(string arg)
            {
                return !arg.StartsWith(_switch) ? null : arg.Substring(_switch.Length).ToLower();
            }
        }
    }
}