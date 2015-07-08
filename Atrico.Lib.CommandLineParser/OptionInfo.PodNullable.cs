using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        private class OptionInfoPodNullable : OptionInfoParameterisedSwitch
        {
            public OptionInfoPodNullable(OptionDetails details, Type type)
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
                    value = Convert.ChangeType(valueStr, Type);
                    return true;
                }
                catch (Exception ex)
                {
                    throw new ParameterWrongTypeException(Name, Type, valueStr, ex);
                }
            }

            protected override object ChangeValueType(object obj)
            {
                return Convert.ChangeType(obj, Type);
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