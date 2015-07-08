using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        private class OptionInfoEnum : OptionInfoParameterisedSwitch
        {
            public OptionInfoEnum(OptionDetails details, Type type)
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
                    throw new ParameterWrongTypeException(Name, Type, valueStr, ex);
                }
            }
        }
    }
}