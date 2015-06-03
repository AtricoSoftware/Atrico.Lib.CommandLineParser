using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Atrico.Lib.CommandLineParser.Exceptions;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;
using Atrico.Lib.Common.Collections;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        private const string _switch = "-";

        private static bool IsSwitch(string arg)
        {
            return arg.StartsWith(_switch);
        }

        private static string RemoveSwitch(string arg)
        {
            return !IsSwitch(arg) ? null : arg.Substring(_switch.Length).ToLower();
        }

        internal static string MakeSwitch(string arg)
        {
            return IsSwitch(arg) ? arg : string.Format("{0}{1}", _switch, arg);
        }

        private static IEnumerable<OptionInfo> GetOptionInformation<T>() where T : class, new()
        {
            return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Select(OptionInfo.Create).OrderBy(oi=>oi.Position);
        }

        /// <summary>
        ///     Command line parser for specific type
        /// </summary>
        private class ParserT<T> where T : class, new()
        {
            private readonly OptionInfo[] _options;
            private readonly IEnumerable<string> _args;
            private readonly Lazy<T> _result;
            private readonly IEnumerable<string> _optionNames;

            public T Result
            {
                get { return _result.Value; }
            }

            public ParserT(IEnumerable<string> args)
            {
                Validate<T>();
                _result = new Lazy<T>(FitArguments);
                // Find (writeable) properties with attribute
                _options = GetOptionInformation<T>().ToArray();
                // Store all option names
                _optionNames = _options.Select(opt => opt.Name.ToLower());
                // Check for ambiguity and promote partial names
                _args = args.Select(PromotePartialNames);
            }

            private string PromotePartialNames(string arg)
            {
                // Not an option
                var name = RemoveSwitch(arg);
                if (name == null)
                {
                    return arg;
                }
                // Full match
                if (_optionNames.Contains(name.ToLower()))
                {
                    return arg;
                }
                // Partial match
                var possible = _optionNames.Where(nm => nm.StartsWith(name, StringComparison.OrdinalIgnoreCase)).ToArray();
                // One found
                if (possible.Length == 1)
                {
                    return MakeSwitch(possible.First());
                }
                // None/Multiple found
                throw possible.Any() ? new AmbiguousOptionException(MakeSwitch(name), possible.Select(MakeSwitch)) : new UnexpectedOptionException(MakeSwitch(name)) as CommandLineParserException;
            }

            private T FitArguments()
            {
                // Options
                var leftOverArgs = _options.Aggregate(_args, (current, opt) => opt.FulFillSwitches(current));
                // Positional
                leftOverArgs = _options.Aggregate(leftOverArgs, (current, opt) => opt.FulFillPositional(current));
                var results = new T();
                _options.ForEach(option => option.Populate(results));
                return results;
            }
        }
    }
}