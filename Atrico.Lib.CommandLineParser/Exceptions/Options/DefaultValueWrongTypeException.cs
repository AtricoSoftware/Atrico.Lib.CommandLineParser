using System;
using System.Reflection;

namespace Atrico.Lib.CommandLineParser.Exceptions.Options
{
    /// <summary>
    ///     Property has default value of wrong type
    /// </summary>
    public class DefaultValueWrongTypeException : InvalidOptionsPropertyException
    {
        public DefaultValueWrongTypeException(PropertyInfo property, object defaultValue, Exception inner = null)
            : base(property, string.Format("Property has default value of wrong type: {0} ({1})", property.Name, defaultValue), inner)
        {
        }
    }
}