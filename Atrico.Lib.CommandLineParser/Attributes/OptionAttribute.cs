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
        private int _position = -1;

        /// <summary>
        ///     Determines if option is mandatory
        /// </summary>
        /// <value>
        ///     <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }

        /// <summary>
        ///     Position to select parameter without switch
        /// </summary>
        /// <value>
        ///     -1 if not set
        /// </value>
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        ///     Default value for optional option
        /// </summary>
        /// <value>
        ///     The default value to use if option is not specified
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

        /// <summary>
        ///     Description for display in usage
        /// </summary>
        public string Description { get; set; }

        internal bool HasDefaultValue { get; private set; }
    }
}