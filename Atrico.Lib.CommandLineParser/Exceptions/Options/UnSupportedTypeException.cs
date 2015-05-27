using System.Reflection;

namespace Atrico.Lib.CommandLineParser.Exceptions.Options
{
    /// <summary>
    ///     A property is an unsupported type
    /// </summary>
    public class UnSupportedTypeException : InvalidOptionsPropertyException
    {
        public UnSupportedTypeException(PropertyInfo property)
            : base(property, string.Format("Type not supported: {0}", property.PropertyType.Name))
        {
        }
    }
}