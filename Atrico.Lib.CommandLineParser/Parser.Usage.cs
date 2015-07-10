using Atrico.Lib.Common;
using Atrico.Lib.Common.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Atrico.Lib.CommandLineParser
{
    /// <summary>
    ///     Command line parser entry point
    /// </summary>
    public static partial class Parser
    {
        // Internal to allow injection for testing
        internal static IRunContextInfo RunContextInfo { get; set; }

        static Parser()
        {
            RunContextInfo = new RunContextInfo();
        }

        [Flags]
        public enum UsageDetails
        {
            AppInfo = 0x01,
            Summary = 0x02,
            ParameterDetails = 0x04,

            Full = AppInfo | Summary | ParameterDetails
        }

        /// <summary>
        ///     Gets the usage information for this type
        /// </summary>
        /// <typeparam name="T">Type of options class</typeparam>
        /// <returns>Usage info as multiple lines of text</returns>
        public static IEnumerable<string> GetUsage<T>(UsageDetails details = UsageDetails.Full) where T : class, new()
        {
            var options = GetOptionInformation<T>().ToArray();
            var lines = new List<string>();
            // AppInfo
            if (details.HasFlag(UsageDetails.AppInfo))
            {
                // Assembly name & version
                lines.Add(string.Format("{0} {1}", RunContextInfo.EntryAssemblyName, RunContextInfo.EntryAssemblyVersion));
                // Copyright
                lines.Add(RunContextInfo.EntryAssemblyCopyright);
            }
            // Summary
            if (details.HasFlag(UsageDetails.Summary) && options.Any())
            {
                if (lines.Any()) lines.Add(string.Empty);
                var line = new StringBuilder(Path.GetFileNameWithoutExtension(RunContextInfo.EntryAssemblyPath));
                foreach (var option in options)
                {
                    line.AppendFormat(" {0}", option);
                }
                lines.Add(line.ToString());
            }
            // Parameter Details
            if (details.HasFlag(UsageDetails.ParameterDetails))
            {
                var paramDetails = options.Select(opt => Tuple.Create(opt.Name, opt.Details)).Where(tup => !string.IsNullOrWhiteSpace(tup.Item2)).ToArray();
                var table = new Table();
                foreach (var detail in paramDetails)
                {
                    table.AppendRow(string.Format("{0}:", detail.Item1), detail.Item2);
                }
                if (lines.Any() && table.Rows > 0) lines.Add(string.Empty);
                lines.AddRange(table.ToMultilineString());
            }
            return lines;
        }
    }
}