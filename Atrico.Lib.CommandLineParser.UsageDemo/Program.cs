using System;
using Atrico.Lib.CommandLineParser.Attributes;

namespace Atrico.Lib.CommandLineParser.UsageDemo
{
    internal class Options
    {
        [Option(Required = true, Description="Some help text")]
        private bool BoolMand { get; set; }

        [Option]
        private bool BoolOpt { get; set; }

        [Option(Required = true)]
        private int IntMand { get; set; }

        [Option(DefaultValue = 1.23f)]
        private float FloatOpt { get; set; }

        [Option(Required = true)]
        private byte? NullByteMand { get; set; }

        [Option]
        private char? NullCharOpt { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (var line in Parser.GetUsage<Options>())
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }
}