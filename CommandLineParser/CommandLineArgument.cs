// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.CommandLineParser
{
    /// <summary>
    /// Represents a single command-line argument.
    /// </summary>
    public class CommandLineArgument
    {
        /// <summary>
        /// Returns true if this argument is a flag argument. Flag arguments are
        /// arguments that were preceeded with a '/' or '-' on the command line.
        /// </summary>
        public bool IsFlag { get; set; }

        /// <summary>
        /// The text value for this argument.
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// Returns the extended argument, if any. Extended arguments are in the
        /// form Argument:ExtendedArgument or -Argument:ExtendedArgument. Will be
        /// null if the argument has no extended argument.
        /// </summary>
        public string ExtendedArgument { get; set; }
    }
}
