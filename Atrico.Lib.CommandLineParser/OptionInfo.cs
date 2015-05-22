using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Atrico.Lib.CommandLineParser.Attributes;
using Atrico.Lib.CommandLineParser.Exceptions;

namespace Atrico.Lib.CommandLineParser
{
    public static partial class Parser
    {
        public interface IOptionInfo
        {
            
        }

        /// <summary>
        ///     Command line parser for specific type
        /// </summary>
        private partial class ParserT<T> where T : class, new()
        {
            [DebuggerDisplay("Option: {_name}: {_fulfilled}")]
            private class OptionInfo : IOptionInfo
            {
                private readonly PropertyInfo _property;
                private readonly OptionAttribute _attribute;
                private readonly string _name;
                private bool _fulfilled;

                public string Name
                {
                    get { return _name; }
                }

                public static OptionInfo Create(PropertyInfo property)
                {
                    var attribute = property.GetCustomAttribute<OptionAttribute>();
                    return attribute == null ? null : new OptionInfo(property, attribute);
                }

                private OptionInfo(PropertyInfo property, OptionAttribute attribute)
                {
                    _property = property;
                    _attribute = attribute;
                    _name = property.Name.ToLower();
                }

                public IEnumerable<string> FulFill(IEnumerable<string> argsIn)
                {
                    var args = new Queue<string>(argsIn);
                    var argsOut = new Queue<string>();
                    while (args.Count > 0)
                    {
                        var current = args.Dequeue();
                        var name = current;
                        if (!_fulfilled && IsSwitch(ref name) && _name.Equals(name))
                        {
                            _fulfilled = true;
                            continue;
                        }
                        argsOut.Enqueue(current);
                    }
                    return argsOut;
                }

                public void Populate(T options)
                {
                    if (_attribute.Required && !_fulfilled) throw new MissingOptionException(string.Format("{0}{1}", _switch, _property.Name));
                    _property.SetValue(options, _fulfilled);
                }
            };
        }
    }
}