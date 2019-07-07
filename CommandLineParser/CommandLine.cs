// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System.Collections.Generic;
using System.Linq;

namespace SoftCircuits.CommandLineParser
{
    /// <summary>
    /// Parses a given command-line into any number of arguments.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Parses a command line into a collection of <see cref="CommandLineArgument" />
    /// objects and provides support for querying that collection.
    /// </para>
    /// <para>
    /// The code distinguishes between arguments and flag arguments (or flags). An
    /// argument is any token that appears by itself on the command line. For example,
    /// this could be the name of a file. A flag is an argument immediately preceded
    /// by a hypen (-) or forward slash (/). A flag is sometimes called a switch and
    /// is normally used to enable or disable an application setting.
    /// </para>
    /// <para>
    /// The code also supports extended arguments. Extended arguments appear after
    /// an argument or flag argument and use a colon (:) as a delimiter. Consider the
    /// following argument:
    /// </para>
    /// <para>
    /// -log:off
    /// </para>
    /// <para>
    /// The argument is &quot;log&quot;. It's a flag argument because it was preceded
    /// with a hyphen. And &quot;off&quot; is the extended argument.
    /// </para>
    /// <para>
    /// Any argument or extended argument can be enclosed in single or double quotes,
    /// providing support for arguments that contain whitespace or other special
    /// characters.
    /// </para>
    /// </remarks>
    public class CommandLine
    {
        private static readonly HashSet<char> FlagChars = new HashSet<char> { '-', '/' };
        private static readonly HashSet<char> QuoteChars = new HashSet<char> { '"', '\'' };
        private static readonly char ExtendedArgumentDelimiter = ':';

        private bool SupportExtendedArguments;

        /// <summary>
        /// List of parsed command-line arguments.
        /// </summary>
        public List<CommandLineArgument> Arguments { get; private set; }

        /// <summary>
        /// Constructs an instance of the <see cref="CommandLine" /> class.
        /// </summary>
        /// <param name="commandLine">The command line to parse.</param>
        /// <param name="supportExtendedArguments">If true, extended arguments are supported in the
        /// form /f:filename or filename:mode.</param>
        public CommandLine(string commandLine, bool supportExtendedArguments = false)
        {
            SupportExtendedArguments = supportExtendedArguments;
            ParseCommandLine(commandLine);
        }

        /// <summary>
        /// Parses a new command line. Populates the Arguments property with the results.
        /// </summary>
        /// <param name="commandLine">Command line to parse.</param>
        /// <param name="supportExtendedArguments">If true, extended arguments are supported in the
        /// form /f:filename or filename:mode.</param>
        public void ParseCommandLine(string commandLine)
        {
            bool parsedApp = false;

            // Parse all arguments on command line
            Arguments = new List<CommandLineArgument>();
            ParsingHelper parser = new ParsingHelper(commandLine);
            // Skip whitespace
            parser.SkipWhitespace();
            while (!parser.EndOfText)
            {
                // Create new argument
                CommandLineArgument argument = new CommandLineArgument();
                // Determine if this is a flag
                argument.IsFlag = FlagChars.Contains(parser.Peek());
                if (argument.IsFlag)
                    parser.MoveAhead();
                // Parse argument
                argument.Argument = ParseArgument(parser, SupportExtendedArguments);
                // Parse extended argument
                if (parser.Peek() == ExtendedArgumentDelimiter)
                {
                    parser.MoveAhead();
                    argument.ExtendedArgument = ParseArgument(parser, false);
                }
                // Add to list of arguments if not empty
                if (!string.IsNullOrWhiteSpace(argument.Argument))
                {
                    if (parsedApp == true)
                        Arguments.Add(argument);
                    else
                        parsedApp = true;
                }
                // Skip whitespace
                parser.SkipWhitespace();
            }
        }

        /// <summary>
        /// Parses a single argument.
        /// </summary>
        /// <param name="parser">An instance of the <see cref="ParsingHelper"/>
        /// class.</param>
        /// <param name="testForExtendedArgument">If true, a colon (:) stops argument parsing
        /// (The colon is considered to be the argument/extended argument delimiter). If false,
        /// the colon is considered a valid argument character.</param>
        /// <returns>The parsed argument.</returns>
        /// <seealso cref="ParsingHelper"/>
        private string ParseArgument(ParsingHelper parser, bool testForExtendedArgument)
        {
            if (QuoteChars.Contains(parser.Peek()))
                return parser.ParseQuotedText();
            // Parse unquoted argument
            int start = parser.Index;
            while (!parser.EndOfText &&
                !char.IsWhiteSpace(parser.Peek()) &&
                !FlagChars.Contains(parser.Peek()) &&
                (!testForExtendedArgument || parser.Peek() != ExtendedArgumentDelimiter))
                parser.MoveAhead();
            return parser.Extract(start, parser.Index);
        }

        /// <summary>
        /// Returns true if an argument with the specified value has been specified.
        /// Flag arguments are not included in the search.
        /// </summary>
        /// <param name="argument">The argument value to search for.</param>
        /// <param name="ignoreCase">Set to true if case does not matter.</param>
        /// <returns>Returns true if the arguments include the specified
        /// argument.</returns>
        public bool HasArgument(string argument, bool ignoreCase = false)
        {
            return Arguments.Any(a => !a.IsFlag && string.Compare(a.Argument, argument, ignoreCase) == 0);
        }

        /// <summary>
        /// Returns true if a flag with the specified value has been specified.
        /// Arguments that are not flags are not included in the search.
        /// </summary>
        /// <param name="flag">The flag argument value to search for.</param>
        /// <param name="ignoreCase">Set to true if case does not matter.</param>
        /// <returns>Returns true if the arguments include the specified
        /// flag argument.</returns>
        public bool HasFlagArgument(string flag, bool ignoreCase = false)
        {
            return Arguments.Any(a => a.IsFlag && string.Compare(a.Argument, flag, ignoreCase) == 0);
        }

        /// <summary>
        /// Returns the first argument with the specified value or <c>null</c> if the argument was not found.
        /// Flag arguments are not included in the search.
        /// </summary>
        /// <param name="argument">The argument value to search for.</param>
        /// <param name="ignoreCase">Set to true if case does not matter.</param>
        /// <returns>The first matching argument or null if the argument was not found.</returns>
        public CommandLineArgument GetArgument(string argument, bool ignoreCase = false)
        {
            return Arguments.FirstOrDefault(a => !a.IsFlag && string.Compare(a.Argument, argument, ignoreCase) == 0);
        }

        /// <summary>
        /// Returns the first flag argument with the specified value or <c>null</c> if the flag argument was not
        /// found. Arguments that are not flags are not included in the search.
        /// </summary>
        /// <param name="flag">The flag argument to search for.</param>
        /// <param name="ignoreCase">Set to true if case does not matter.</param>
        /// <returns>The first matching flag argument or null if the flag argument was not found.</returns>
        public CommandLineArgument GetFlagArgument(string flag, bool ignoreCase = false)
        {
            return Arguments.FirstOrDefault(a => a.IsFlag && string.Compare(a.Argument, flag, ignoreCase) == 0);
        }

        /// <summary>
        /// Returns all non-flag arguments (arguments not preceded with <c>/</c> or <c>-</c>
        /// in the order they were specified on the command line.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CommandLineArgument> GetArguments() => Arguments.Where(a => a.IsFlag == false);

        /// <summary>
        /// Returns all flag arguments (arguments preceded with <c>/</c> or <c>-</c> in the
        /// order they were specified on the command line.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CommandLineArgument> GetFlagArguments() => Arguments.Where(a => a.IsFlag == true);
    }
}
