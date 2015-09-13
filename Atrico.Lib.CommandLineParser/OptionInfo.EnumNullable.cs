using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        private class OptionInfoEnumNullable : OptionInfoParameterisedSwitch
        {
            public OptionInfoEnumNullable(OptionDetails details, Type type)
                : base(details, type)
            {
            }

            protected override bool GetOptionValue(Queue<string> args, out object value)
            {
                if (!args.Any())
                {
                    value = null;
                    return false;
                }
                var valueStr = args.Dequeue();
                try
                {
                    value = Enum.Parse(Type, valueStr, true);
                    return true;
                }
                catch (Exception ex)
                {
                    // Try for minimum match
                    var matches = Enum.GetNames(Type).Where(name => name.StartsWith(valueStr, StringComparison.OrdinalIgnoreCase)).ToArray();
                    if (matches.Length != 1) throw new ParameterWrongTypeException(Name, Type, valueStr, ex);
                    value = Enum.Parse(Type, matches[0], true);
                    return true;
                }
            }

            protected override IEnumerable<string> CalculateWarnings()
            {
                var warnings = new List<string>(base.CalculateWarnings());
                // Nullable with default
                if (HasDefaultValue) warnings.Add(string.Format("Property is nullable but has default value: {0} ({1})", Name, DefaultValue));
                // Mandatory Nullable
                if (Required) warnings.Add(string.Format("Property is nullable but mandatory: {0}", Name));
                return warnings;
            }

            protected override string UsageType
            {
                get { return string.Format("{0}?", Type.Name); }
            }
        }
    }
}