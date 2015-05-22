using System;

namespace Atrico.Lib.CommandLineParser.Attributes
{
    /// <summary>
    ///     Attribute to mark a property as an option
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
        /// <summary>
        ///     Determines if option is mandatory
        /// </summary>
        /// <value>
        ///     <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }
    }
}