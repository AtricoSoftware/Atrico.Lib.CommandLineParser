using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        private class OptionInfoNullable<T> : OptionInfoParameterisedSwitch where T : struct
        {
            public OptionInfoNullable(OptionDetails details)
                : base(details)
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
                    value = (T) Convert.ChangeType(valueStr, typeof (T));
                    return true;
                }
                catch (Exception ex)
                {
                    throw new OptionParameterWrongTypeException(Name, typeof (T), valueStr, ex);
                }
            }

            protected override object ChangeValueType(object obj)
            {
                return Convert.ChangeType(obj, typeof (T));
            }

            protected override IEnumerable<string> CalculateWarnings()
            {
                var warnings = new List<string>(base.CalculateWarnings());
                // Nullable with default
                if (HasDefaultValue) warnings.Add(string.Format("Property is nullable but has default value: {0} ({1})", Property.Name, DefaultValue));
                // Mandatory Nullable
                if (Required) warnings.Add(string.Format("Property is nullable but mandatory: {0}", Property.Name));
                return warnings;
            }

            protected override string UsageType
            {
                get { return string.Format("{0}?", typeof (T).Name); }
            }
        }
    }
}