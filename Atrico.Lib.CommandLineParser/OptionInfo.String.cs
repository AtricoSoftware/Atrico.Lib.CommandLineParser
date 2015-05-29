using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        private class OptionInfoString : OptionInfoParameterisedSwitch
        {
            public OptionInfoString(OptionDetails details)
                : base(details)
            {
            }

            protected override bool GetOptionValue(Queue<string> args, out object value)
            {
                var hasArgs = args.Any();
                value = hasArgs ? args.Dequeue() : null;
                return hasArgs;
            }
        }
    }
}