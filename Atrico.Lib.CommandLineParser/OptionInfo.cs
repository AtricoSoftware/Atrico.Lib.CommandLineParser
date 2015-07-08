using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions.Options;
using Atrico.Lib.CommandLineParser.Exceptions.Parse;
using Atrico.Lib.Common.Reflection;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        internal class OptionDetails
        {
            private readonly OptionAttribute _attribute;

            public PropertyInfo Property { get; private set; }

            public bool Required
            {
                get { return _attribute.Required; }
            }

            public bool HasDefaultValue
            {
                get { return _attribute.HasDefaultValue; }
            }

            public object DefaultValue
            {
                get { return _attribute.DefaultValue; }
            }

            public string Description
            {
                get { return _attribute.Description; }
            }

            public int Position
            {
                get { return _attribute.Position; }
            }

            public OptionDetails(PropertyInfo property, OptionAttribute attribute)
            {
                _attribute = attribute;
                Property = property;
            }
        }

        [DebuggerDisplay("Option: {_name}: {_fulfilled}")]
        internal abstract class OptionInfo
        {
            protected Type Type { get; private set; }

            private delegate OptionInfo OptionCreator(OptionDetails details);

            public int Position { get; private set; }
            public bool Fulfilled { get; private set; }
            public readonly PropertyInfo Property;
            public readonly bool Required;

            protected readonly bool HasDefaultValue;
            protected readonly object DefaultValue;
            protected readonly string Description;
            private bool _hasValue;
            private object _value;
            private readonly Lazy<IEnumerable<string>> _warnings;

            public string Name
            {
                get { return Property.Name; }
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
                var creator = GetOptionCreator(property.PropertyType);
                if (creator == null) throw new UnSupportedTypeException(property);

                // Create option
                var details = new OptionDetails(property, attribute);
                return creator(details);
            }

            protected OptionInfo(OptionDetails details, Type type)
            {
                Type = type;
                Property = details.Property;
                Required = details.Required;
                HasDefaultValue = details.HasDefaultValue;
                Description = details.Description;
                Position = details.Position;
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
                if (Required && HasDefaultValue) warnings.Add(string.Format("Property is mandatory but has default value: {0} ({1})", Name, DefaultValue));
                return warnings;
            }

            public IEnumerable<string> FulFillSwitches(IEnumerable<string> argsIn)
            {
                if (Fulfilled) return argsIn;
                var argsArray = argsIn.ToArray();
                var argsOut = argsArray.TakeWhile(item => !string.Equals(Name, RemoveSwitch(item), StringComparison.OrdinalIgnoreCase));
                var argsRemaining = argsArray.SkipWhile(item => !string.Equals(Name, RemoveSwitch(item), StringComparison.OrdinalIgnoreCase)).ToArray();
                if (!argsRemaining.Any()) return argsOut;
                Fulfilled = true;
                // Remove matched option
                var argsQueue = new Queue<string>(argsRemaining.Skip(1));
                _hasValue = GetOptionValue(argsQueue, out _value);
                return argsOut.Concat(argsQueue);
            }

            public IEnumerable<string> FulFillPositional(IEnumerable<string> argsIn)
            {
                if (Fulfilled || Position == -1) return argsIn;
                // Remove matched option
                var argsQueue = new Queue<string>(argsIn);
                Fulfilled = _hasValue = GetOptionValue(argsQueue, out _value);
                return argsQueue;
            }

            protected abstract bool GetOptionValue(Queue<string> args, out object value);

            public void Populate(object options)
            {
                if (Required && !Fulfilled) throw new MissingOptionException(string.Format("{0}{1}", _switch, Name));

                if (Fulfilled && !_hasValue) throw new MissingParameterException(string.Format("{0}{1}", _switch, Name), Property.PropertyType);

                Property.SetValue(options, (!Fulfilled && HasDefaultValue) ? DefaultValue : _value);
            }

            public override string ToString()
            {
                return Summary;
            }

            public string Summary
            {
                get
                {
                    // Name
                    var summary = new StringBuilder(UsageName);
                    // Positional?
                    if (Position != -1)
                    {
                        summary.Insert(0, '[');
                        summary.Append(']');
                    }
                    // Parameter
                    if (!String.IsNullOrWhiteSpace(UsageType)) summary.AppendFormat(" <{0}>", UsageType);
                    // Optional?
                    if (!Required)
                    {
                        summary.Insert(0, '[');
                        summary.Append(']');
                    }
                    return summary.ToString();
                }
            }

            public string Details
            {
                get
                {
                    var detail = new StringBuilder();
                    // Descritpion
                    if (Description != null) detail.Append(Description);
                    // Default value
                    if (HasDefaultValue)
                    {
                        if (detail.Length > 0) detail.Append(' ');
                        detail.AppendFormat("(default = {0})", DefaultValue);
                    }
                    return detail.ToString();
                }
            }

            protected abstract string UsageName { get; }
            protected abstract string UsageType { get; }

            private static OptionCreator GetOptionCreator(Type propertyType)
            {
                // Boolean option
                if (propertyType == typeof (bool)) return d => new OptionInfoBoolean(d);
                // String
                if (propertyType == typeof (string)) return d => new OptionInfoString(d);
                // POD types
                if (propertyType.IsPrimitive) return d => new OptionInfoPod(d, propertyType);
                // Enum
                if (propertyType.IsEnum()) return d => new OptionInfoEnum(d, propertyType);
                // Nullable...
                Type underlyingType;
                if (propertyType.IsNullable(out underlyingType))
                {
                    // Nullable POD types
                    if (underlyingType.IsPrimitive) return d => new OptionInfoPodNullable(d, underlyingType);
                    // Nullable Enum
                    if (underlyingType.IsEnum()) return d => new OptionInfoEnumNullable(d, underlyingType);
                }
                // Unsupported type
                return null;
            }
        }

        internal abstract class OptionInfoSwitch : OptionInfo
        {
            protected OptionInfoSwitch(OptionDetails details, Type type)
                : base(details, type)
            {
            }

            protected override string UsageName
            {
                get { return MakeSwitch(Name); }
            }
        }

        internal abstract class OptionInfoParameterisedSwitch : OptionInfoSwitch
        {
            protected OptionInfoParameterisedSwitch(OptionDetails details, Type type)
                : base(details, type)
            {
            }

            protected override string UsageType
            {
                get { return Property.PropertyType.Name; }
            }
        }
    }
}
