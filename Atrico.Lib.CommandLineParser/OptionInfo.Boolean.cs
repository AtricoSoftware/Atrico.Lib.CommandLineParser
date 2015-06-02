using System.Collections.Generic;
using Atrico.Lib.CommandLineParser.Exceptions.Options;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        private class OptionInfoBoolean : OptionInfoSwitch
        {
            public OptionInfoBoolean(OptionDetails details)
                : base(details)
            {
                // Position not valid
                if (Position != -1) throw new PositionOnBooleanException(Property, Position);
            }

            protected override bool GetOptionValue(Queue<string> args, out object value)
            {
                // True by existence
                value = true;
                return true;
            }

            protected override IEnumerable<string> CalculateWarnings()
            {
                var warnings = new List<string>(base.CalculateWarnings());
                // Boolean with default
                if (HasDefaultValue) warnings.Add(string.Format("Property is boolean but has default value: {0} ({1})", Name, DefaultValue));
                return warnings;
            }

             protected override string UsageType
            {
                get { return ""; }
            }
        }
    }
}