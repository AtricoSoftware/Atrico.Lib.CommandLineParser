using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions;
using Atrico.Lib.CommandLineParser.Exceptions.Options;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;

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
                if (!args.Any())
                {
                    return null;
                }
                var valueStr = args.Dequeue();
                try
                {
                    return (T) Convert.ChangeType(valueStr, typeof (T));
                }
                catch (Exception ex)
                {
                    throw new OptionParameterWrongTypeException(Name, typeof (T), valueStr, ex);
                }
            }
        }

        internal class OptionInfoNullable<T> : OptionInfo where T : struct
        {
            public OptionInfoNullable(PropertyInfo property, OptionAttribute attribute)
                : base(property, attribute)
            {
            }

            protected override object GetOptionValue(Queue<string> args)
            {
                if (!args.Any())
                {
                    return null;
                }
                var valueStr = args.Dequeue();
                try
                {
                    return (T) Convert.ChangeType(valueStr, typeof (T));
                }
                catch (Exception ex)
                {
                    throw new OptionParameterWrongTypeException(Name, typeof (T), valueStr, ex);
                }
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
            private delegate OptionInfo OptionCreator(PropertyInfo property, OptionAttribute attribute);

            private static readonly IDictionary<Type, OptionCreator> _supportedTypes = new Dictionary<Type, OptionCreator>
            {
                // Boolean option
                {typeof (bool), (p, a) => new OptionInfoBoolean(p, a)},
                // String
                {typeof (string), (p, a) => new OptionInfoString(p, a)},
                // POD types
                {typeof (char), (p, a) => new OptionInfoPod<char>(p, a)},
                {typeof (byte), (p, a) => new OptionInfoPod<byte>(p, a)},
                {typeof (sbyte), (p, a) => new OptionInfoPod<sbyte>(p, a)},
                {typeof (short), (p, a) => new OptionInfoPod<short>(p, a)},
                {typeof (ushort), (p, a) => new OptionInfoPod<ushort>(p, a)},
                {typeof (int), (p, a) => new OptionInfoPod<int>(p, a)},
                {typeof (uint), (p, a) => new OptionInfoPod<uint>(p, a)},
                {typeof (long), (p, a) => new OptionInfoPod<long>(p, a)},
                {typeof (ulong), (p, a) => new OptionInfoPod<ulong>(p, a)},
                {typeof (float), (p, a) => new OptionInfoPod<float>(p, a)},
                {typeof (double), (p, a) => new OptionInfoPod<double>(p, a)},
                // Nullable POD types
                {typeof (char?), (p, a) => new OptionInfoNullable<char>(p, a)},
                {typeof (byte?), (p, a) => new OptionInfoNullable<byte>(p, a)},
                {typeof (sbyte?), (p, a) => new OptionInfoNullable<sbyte>(p, a)},
                {typeof (short?), (p, a) => new OptionInfoNullable<short>(p, a)},
                {typeof (ushort?), (p, a) => new OptionInfoNullable<ushort>(p, a)},
                {typeof (int?), (p, a) => new OptionInfoNullable<int>(p, a)},
                {typeof (uint?), (p, a) => new OptionInfoNullable<uint>(p, a)},
                {typeof (long?), (p, a) => new OptionInfoNullable<long>(p, a)},
                {typeof (ulong?), (p, a) => new OptionInfoNullable<ulong>(p, a)},
                {typeof (float?), (p, a) => new OptionInfoNullable<float>(p, a)},
                {typeof (double?), (p, a) => new OptionInfoNullable<double>(p, a)},
            };

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
                if (attribute == null)
                {
                    return null;
                }
                OptionCreator creator;
                // Check for setter
                if (property.SetMethod == null) throw new NoSetterException(property);
                // Supported types
                if (_supportedTypes.TryGetValue(property.PropertyType, out creator))
                {
                    return creator(property, attribute);
                }
                // Unsupported
                throw new UnSupportedTypeException(property);
            }

            protected OptionInfo(PropertyInfo property, OptionAttribute attribute)
            {
                _property = property;
                _attribute = attribute;
                _name = property.Name.ToLower();
            }

            public IEnumerable<string> FulFill(IEnumerable<string> argsIn)
            {
                if (_fulfilled)
                {
                    return argsIn;
                }
                var argsArray = argsIn.ToArray();
                var argsOut = argsArray.TakeWhile(item => !_name.Equals(RemoveSwitch(item)));
                var argsRemaining = argsArray.SkipWhile(item => !_name.Equals(RemoveSwitch(item))).ToArray();
                if (!argsRemaining.Any())
                {
                    return argsOut;
                }
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
