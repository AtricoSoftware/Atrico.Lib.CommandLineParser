using System.Collections.Generic;
using Atrico.Lib.Common.Collections;

namespace Atrico.Lib.CommandLineParser.Exceptions.Parse
{
    /// <summary>
    ///     An Option was ambiguous
    ///     It did not exactly name an expected option and it was a partial match for multiple options
    /// </summary>
    public class AmbiguousOptionException : ParseException
    {
        public AmbiguousOptionException(string option, IEnumerable<string> possibilites)
            : base(string.Format("An option was ambiguous: {0} {1}", option, possibilites.ToCollectionString("(", ")")))
        {
        }
    }
}
