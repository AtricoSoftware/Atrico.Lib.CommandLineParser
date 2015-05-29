using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.CommandLineParser.Exceptions.Options;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        private class OptionInfoPod<T> : OptionInfoParameterisedSwitch where T : struct
        {
            public OptionInfoPod(OptionDetails details)
                : base(details)
            {
                // Check for optional/default value
                if (!details.Required && !details.HasDefaultValue) throw new OptionalNonNullableException(details.Property);
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
        }
    }
}