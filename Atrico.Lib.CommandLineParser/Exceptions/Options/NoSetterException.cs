using System.Reflection;

namespace Atrico.Lib.CommandLineParser.Exceptions.Options
{
    /// <summary>
    ///     A property has no setter
    /// </summary>
    public class NoSetterException : InvalidOptionsPropertyException
    {
        public NoSetterException(PropertyInfo property)
            : base(property, string.Format("Property has no setter: {0}", property.Name))
        {
        }
    }
}