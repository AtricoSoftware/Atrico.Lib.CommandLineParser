using System.Reflection;

namespace Atrico.Lib.CommandLineParser.Exceptions
{
    /// <summary>
    ///     Option property was invalid
    /// </summary>
    public abstract class InvalidOptionsPropertyException : CommandLineParserException
    {
        protected InvalidOptionsPropertyException(PropertyInfo property, string message)
            : base(string.Format("Options property {0}.{1} has errors: {2}", property.DeclaringType.Name, property.Name, message))
        {
        }
    }
}