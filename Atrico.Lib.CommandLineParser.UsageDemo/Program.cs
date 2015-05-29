using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atrico.Lib.CommandLineParser.Attributes;

namespace Atrico.Lib.CommandLineParser.UsageDemo
{
    class Options
    {
        [Option(Required = true)]
        bool BoolMand { get; set; }
        [Option]
        bool BoolOpt { get; set; }
        [Option(Required = true)]
        int IntMand { get; set; }
        [Option(DefaultValue = 1.23f)]
        float FloatOpt { get; set; }
        [Option(Required = true)]
        byte? NullByteMand { get; set; }
        [Option]
        char? NullCharOpt { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var line in Parser.GetUsage<Options>())
            {
                Console.WriteLine(line);
            }
        }
    }
}
