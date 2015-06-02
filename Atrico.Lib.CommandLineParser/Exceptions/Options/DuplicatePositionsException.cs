using System;
using System.Reflection;

namespace Atrico.Lib.CommandLineParser.Exceptions.Options
{
    /// <summary>
    ///     Property has duplicate position
    /// </summary>
    public class DuplicatePositionsException : InvalidOptionsPropertyException
    {
        public DuplicatePositionsException(PropertyInfo property1, PropertyInfo property2, int position, Exception inner = null)
            : base(property2, string.Format("Property position is duplicate: {0} ({1})", property1.Name, position), inner)
        {
        }
    }
}