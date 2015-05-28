using System.Reflection;

namespace Atrico.Lib.CommandLineParser.Exceptions.Options
{
    /// <summary>
    ///     Property is non-nullable but marked as optional with no default value
    /// </summary>
    public class OptionalNonNullableException : InvalidOptionsPropertyException
    {
        public OptionalNonNullableException(PropertyInfo property)
            : base(property, string.Format("Property is non-nullable but marked as optional with no default value: {0}", property.Name))
        {
        }
    }
}