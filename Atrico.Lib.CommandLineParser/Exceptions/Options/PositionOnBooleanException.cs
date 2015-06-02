using System;
using System.Reflection;

namespace Atrico.Lib.CommandLineParser.Exceptions.Options
{
    /// <summary>
    ///     Boolean property has positon
    /// </summary>
    public class PositionOnBooleanException : InvalidOptionsPropertyException
    {
        public PositionOnBooleanException(PropertyInfo property, int positon, Exception inner = null)
            : base(property, string.Format("Property is boolean but has position: {0} ({1})", property.Name, positon), inner)
        {
        }
    }
}