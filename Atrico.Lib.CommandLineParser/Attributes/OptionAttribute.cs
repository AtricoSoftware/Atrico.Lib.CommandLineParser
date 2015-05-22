using System;

namespace Atrico.Lib.CommandLineParser.Attributes
{
    /// <summary>
    ///     Attribute to mark a property as an option
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
    }
}