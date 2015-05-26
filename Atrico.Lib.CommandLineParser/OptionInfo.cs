using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        internal class OptionInfoPod<T> : OptionInfo where T : struct
        {
            public OptionInfoPod(PropertyInfo property, OptionAttribute attribute)
                : base(property, attribute)
            {
            }

            protected override object GetOptionValue(Queue<string> args)
            {
                if (!args.Any()) return null;
                var valueStr = args.Dequeue();
                int value;
                if (!int.TryParse(valueStr, out value)) throw new OptionParameterWrongTypeException(Name, typeof(int), valueStr);
                return value;
            }
        }

        internal class OptionInfoString : OptionInfo
        {
            public OptionInfoString(PropertyInfo property, OptionAttribute attribute)
                : base(property, attribute)
            {
            }

            protected override object GetOptionValue(Queue<string> args)
            {
                return !args.Any() ? null : args.Dequeue();
            }
        }

        internal class OptionInfoBoolean : OptionInfo
        {
            public OptionInfoBoolean(PropertyInfo property, OptionAttribute attribute)
                : base(property, attribute)
            {
            }

            protected override object GetOptionValue(Queue<string> args)
            {
                // True by existence
                return true;
            }
        }

        [DebuggerDisplay("Option: {_name}: {_fulfilled}")]
        internal abstract class OptionInfo
        {
            private readonly PropertyInfo _property;
            private readonly OptionAttribute _attribute;
            private readonly string _name;
            private bool _fulfilled;
            private object _value;

            public string Name
            {
                get { return _name; }
            }

            public static OptionInfo Create(PropertyInfo property)
            {
                var attribute = property.GetCustomAttribute<OptionAttribute>();
                if (attribute == null) return null;
                // Boolean option
                if (property.PropertyType == typeof (bool)) return new OptionInfoBoolean(property, attribute);
                // String
                if (property.PropertyType == typeof (string)) return new OptionInfoString(property, attribute);
                // Int
                if (property.PropertyType == typeof (int)) return new OptionInfoPod<int>(property, attribute);
                // Unsupported
                throw new UnSupportedOptionTypeException(MakeSwitch(property.Name), property.PropertyType);
            }

            protected OptionInfo(PropertyInfo property, OptionAttribute attribute)
            {
                _property = property;
                _attribute = attribute;
                _name = property.Name.ToLower();
            }

            public IEnumerable<string> FulFill(IEnumerable<string> argsIn)
            {
                if (_fulfilled) return argsIn;
                var argsArray = argsIn.ToArray();
                var argsOut = argsArray.TakeWhile(item => !_name.Equals(RemoveSwitch(item)));
                var argsRemaining = argsArray.SkipWhile(item => !_name.Equals(RemoveSwitch(item))).ToArray();
                if (!argsRemaining.Any()) return argsOut;
                _fulfilled = true;
                // Remove matched option
                var argsQueue = new Queue<string>(argsRemaining.Skip(1));
                _value = GetOptionValue(argsQueue);
                return argsOut.Concat(argsQueue);
            }

            protected abstract object GetOptionValue(Queue<string> args);

            public void Populate(object options)
            {
                if (_attribute.Required && !_fulfilled)
                {
                    throw new MissingOptionException(string.Format("{0}{1}", _switch, _property.Name));
                }

                if (_fulfilled && ReferenceEquals(_value, null))
                {
                    throw new MissingOptionParameterException(string.Format("{0}{1}", _switch, _property.Name), _property.PropertyType);
                }
                _property.SetValue(options, _value);
            }
        };
    }
}