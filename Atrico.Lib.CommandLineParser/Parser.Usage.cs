using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Atrico.Lib.Common;

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
            ParameterDetails = 0x03,

            Full = Summary | ParameterDetails
        }

        /// <summary>
        ///     Gets the usage information for this type
        /// </summary>
        /// <typeparam name="T">Type of options class</typeparam>
        /// <returns>Usage info as multiple lines of text</returns>
        public static IEnumerable<string> GetUsage<T>(UsageDetails details = UsageDetails.Full) where T : class, new()
        {
            var lines = new List<string>();
            var parameterDetails = new List<string>();
            // AppInfo
            if (details.HasFlag(UsageDetails.AppInfo))
            {
                // Assembly name & version
                lines.Add(string.Format("{0} {1}", RunContextInfo.EntryAssemblyName, RunContextInfo.EntryAssemblyVersion));
                // Copyright
                lines.Add(RunContextInfo.EntryAssemblyCopyright);
            }
            // Summary
            if (details.HasFlag(UsageDetails.Summary))
            {
                if (lines.Any()) lines.Add(string.Empty);
                var line = new StringBuilder(Path.GetFileNameWithoutExtension(RunContextInfo.EntryAssemblyPath));
                foreach (var option in GetOptionInformation<T>())
                {
                    line.AppendFormat(" {0}", option);
                    // TODO - Update parameter details
                }
                lines.Add(line.ToString());
            }
            // Parameter Details
            if (details.HasFlag(UsageDetails.ParameterDetails) && parameterDetails.Any())
            {
                if (lines.Any()) lines.Add(string.Empty);
                lines.AddRange(parameterDetails);
            }
            return lines;
        }
    }
}