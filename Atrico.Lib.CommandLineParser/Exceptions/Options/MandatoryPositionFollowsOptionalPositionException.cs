using System;
using System.Reflection;

namespace Atrico.Lib.CommandLineParser.Exceptions.Options
{
    /// <summary>
    ///     Mandatory property with positon after an optional property
    /// </summary>
    public class MandatoryPositionFollowsOptionalPositionException : InvalidOptionsPropertyException
    {
        public MandatoryPositionFollowsOptionalPositionException(PropertyInfo property, Exception inner = null)
            : base(property, string.Format("Property is mandatory but has positon after another optional property: {0}", property.Name), inner)
        {
        }
    }
}