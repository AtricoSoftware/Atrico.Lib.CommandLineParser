using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions.Options;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        internal class OptionDetails
        {
            public PropertyInfo Property { get; private set; }
            public bool Required { get; private set; }
            public bool HasDefaultValue { get; private set; }
            public object DefaultValue { get; private set; }

            public OptionDetails(PropertyInfo property, OptionAttribute attribute)
            {
                Property = property;
                Required = attribute.Required;
                if (attribute.HasDefaultValue)
                {
                    DefaultValue = attribute.DefaultValue;
                    HasDefaultValue = true;
                }
            }
        }

 
        [DebuggerDisplay("Option: {_name}: {_fulfilled}")]
        internal abstract class OptionInfo
        {
            private delegate OptionInfo OptionCreator(OptionDetails details);

            private static readonly IDictionary<Type, OptionCreator> _supportedTypes = new Dictionary<Type, OptionCreator>
            {
                // Boolean option
                {typeof (bool), d => new OptionInfoBoolean(d)},
                // String
                {typeof (string), d => new OptionInfoString(d)},
                // POD types
                {typeof (char), d => new OptionInfoPod<char>(d)},
                {typeof (byte), d => new OptionInfoPod<byte>(d)},
                {typeof (sbyte), d => new OptionInfoPod<sbyte>(d)},
                {typeof (short), d => new OptionInfoPod<short>(d)},
                {typeof (ushort), d => new OptionInfoPod<ushort>(d)},
                {typeof (int), d => new OptionInfoPod<int>(d)},
                {typeof (uint), d => new OptionInfoPod<uint>(d)},
                {typeof (long), d => new OptionInfoPod<long>(d)},
                {typeof (ulong), d => new OptionInfoPod<ulong>(d)},
                {typeof (float), d => new OptionInfoPod<float>(d)},
                {typeof (double), d => new OptionInfoPod<double>(d)},
                // Nullable POD types
                {typeof (char?), d => new OptionInfoNullable<char>(d)},
                {typeof (byte?), d => new OptionInfoNullable<byte>(d)},
                {typeof (sbyte?), d => new OptionInfoNullable<sbyte>(d)},
                {typeof (short?), d => new OptionInfoNullable<short>(d)},
                {typeof (ushort?), d => new OptionInfoNullable<ushort>(d)},
                {typeof (int?), d => new OptionInfoNullable<int>(d)},
                {typeof (uint?), d => new OptionInfoNullable<uint>(d)},
                {typeof (long?), d => new OptionInfoNullable<long>(d)},
                {typeof (ulong?), d => new OptionInfoNullable<ulong>(d)},
                {typeof (float?), d => new OptionInfoNullable<float>(d)},
                {typeof (double?), d => new OptionInfoNullable<double>(d)},
            };

            protected readonly PropertyInfo Property;
            protected readonly bool Required;
            protected readonly bool HasDefaultValue;
            protected readonly object DefaultValue;
            private readonly string _name;
            private bool _fulfilled;
            private bool _hasValue;
            private object _value;
            private readonly Lazy<IEnumerable<string>> _warnings;

            public string Name
            {
                get { return _name; }
            }

            public IEnumerable<string> Warnings
            {
                get { return _warnings.Value; }
            }

            public static OptionInfo Create(PropertyInfo property)
            {
                var attribute = property.GetCustomAttribute<OptionAttribute>();
                if (attribute == null) return null;
                // Check for setter
                if (property.SetMethod == null) throw new NoSetterException(property);
                // Supported types
                OptionCreator creator;
                if (!_supportedTypes.TryGetValue(property.PropertyType, out creator)) throw new UnSupportedTypeException(property);

                // Create option
                var details = new OptionDetails(property, attribute);
                return creator(details);
            }

            protected OptionInfo(OptionDetails details)
            {
                Property = details.Property;
                Required = details.Required;
                _name = Property.Name.ToLower();
                HasDefaultValue = details.HasDefaultValue;
                _warnings = new Lazy<IEnumerable<string>>(CalculateWarnings);
                // Default value correct type?
                if (details.HasDefaultValue)
                {
                    try
                    {
                        DefaultValue = ChangeValueType(details.DefaultValue);
                    }
                    catch (Exception ex)
                    {
                        throw new DefaultValueWrongTypeException(Property, details.DefaultValue, ex);
                    }
                }
            }

            protected virtual object ChangeValueType(object obj)
            {
                return Convert.ChangeType(obj, Property.PropertyType);
            }

            protected virtual IEnumerable<string> CalculateWarnings()
            {
                var warnings = new List<string>();
                // Mandatory with default
                if (Required && HasDefaultValue) warnings.Add(string.Format("Property is mandatory but has default value: {0} ({1})", Property.Name, DefaultValue));
                return warnings;
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
                _hasValue = GetOptionValue(argsQueue, out _value);
                return argsOut.Concat(argsQueue);
            }

            protected abstract bool GetOptionValue(Queue<string> args, out object value);

            public void Populate(object options)
            {
                if (Required && !_fulfilled)
                {
                    throw new MissingOptionException(string.Format("{0}{1}", _switch, Property.Name));
                }

                if (_fulfilled && !_hasValue)
                {
                    throw new MissingOptionParameterException(string.Format("{0}{1}", _switch, Property.Name), Property.PropertyType);
                }

                Property.SetValue(options, (!_fulfilled && HasDefaultValue) ? DefaultValue : _value);
            }

            public override string ToString()
            {
                // Name
                var summary = new StringBuilder(UsageName);
                // Parameter
                if (!String.IsNullOrWhiteSpace(UsageType))
                {
                    summary.AppendFormat(" <{0}>", UsageType);
                }
                // Optional?
                if (!Required)
                {
                    summary.Insert(0, '[');
                    summary.Append(']');
                }
                return summary.ToString();
            }

            protected abstract string UsageName { get; }
            protected abstract string UsageType { get; }
        };

        internal abstract class OptionInfoSwitch : OptionInfo
        {
            protected OptionInfoSwitch(OptionDetails details)
                : base(details)
            {
            }

            protected override string UsageName
            {
                get { return MakeSwitch(Property.Name); }
            }
        }

        internal abstract class OptionInfoParameterisedSwitch : OptionInfoSwitch
        {

            protected OptionInfoParameterisedSwitch(OptionDetails details) : base(details)
            {
            }

            protected override string UsageType
            {
                get { return Property.PropertyType.Name; }
            }
        }
    }
}