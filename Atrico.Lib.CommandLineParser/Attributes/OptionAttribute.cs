using System;

namespace Atrico.Lib.CommandLineParser.Attributes
{
    /// <summary>
    ///     Attribute to mark a property as an option
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
        private object _defaultValue;

        /// <summary>
        ///     Determines if option is mandatory
        /// </summary>
        /// <value>
        ///     <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }

        /// <summary>
        /// Default value for optional option
        /// </summary>
        /// <value>
        /// The default value to use if option is not specified
        /// </value>
        public object DefaultValue
        {
            get { return _defaultValue; }
            set
            {
                _defaultValue = value;
                HasDefaultValue = true;
            }
        }

        internal bool HasDefaultValue { get; private set; }
    }
}
