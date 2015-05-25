using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.CommandLineParser.Exceptions;
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

        private static bool IsSwitch(ref string arg)
        {
            if (!IsSwitch(arg))
            {
                return false;
            }
            arg = arg.Substring(_switch.Length).ToLower();
            return true;
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
                _result = new Lazy<T>(FitArguments);
                // Find (writeable) properties with attribute
                _options = GetOptionInformation().ToArray();
                // Store all option names
                _optionNames = _options.Select(opt => opt.Name);
                // Check for ambiguity and promote partial names
                _args = args.Select(PromotePartialNames);
            }

            public static IEnumerable<OptionInfo> GetOptionInformation()
            {
                return typeof (T).GetProperties().Where(p => p.CanWrite).Select(OptionInfo.Create).Where(oi => oi != null);
            }

            private string PromotePartialNames(string name)
            {
                // Not an option
                if (!IsSwitch(ref name))
                {
                    return name;
                }
                // Full match
                if (_optionNames.Contains(name))
                {
                    return MakeSwitch(name);
                }
                // Partial match
                var possible = _optionNames.Where(nm => nm.StartsWith(name)).ToArray();
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
                var leftOver = _options.Aggregate(_args, (current, opt) => opt.FulFill(current));
                var results = new T();
                _options.ForEach(option => option.Populate(results));
                return results;
            }

            private static string MakeSwitch(string arg)
            {
                return IsSwitch(arg) ? arg : string.Format("{0}{1}", _switch, arg);
            }
        }
    }
}
