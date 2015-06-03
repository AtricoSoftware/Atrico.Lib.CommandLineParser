using System;
using Atrico.Lib.CommandLineParser.Attributes;

namespace Atrico.Lib.CommandLineParser.UsageDemo
{
    internal class Options
    {
        [Option(Required = true, Description = "Mandatory boolean")]
        private bool BoolMand { get; set; }

        [Option(Description = "Optional boolean")]
        private bool BoolOpt { get; set; }

        [Option(Position = 0, Required = true, Description = "Mandatory integer")]
        private int IntMand { get; set; }

        [Option(Position = 1, Required = true, Description = "Mandatory byte?")]
        private byte? NullByteMand { get; set; }

        [Option(Position = 2, DefaultValue = 1.23f, Description = "Optional float")]
        private float FloatOpt { get; set; }

        [Option(Position = 3, Description = "Optional char?")]
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