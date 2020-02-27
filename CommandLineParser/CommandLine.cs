// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.Parsing.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftCircuits.CommandLineParser
{
    /// <summary>
    /// Parses a given command-line into any number of arguments.
    /// </summary>
    public class CommandLine
    {
        private static readonly char[] FlagChars = { '-', '/' };
        private static readonly char[] QuoteChars = { '"', '\'' };
        private static readonly char ExtendedArgumentDelimiter = ':';

        private readonly bool SupportExtendedArguments;

        /// <summary>
        /// List of parsed command-line arguments.
        /// </summary>
        public List<CommandLineArgument> Arguments { get; private set; }

        /// <summary>
        /// Constructs an instance of the <see cref="CommandLine" /> class.
        /// </summary>
        /// <param name="supportExtendedArguments">If true, extended arguments are supported in the
        /// form <c>filename:extArg</c> or <c>/f:extArg</c>.</param>
        public CommandLine(bool supportExtendedArguments = false)
        {
            Arguments = new List<CommandLineArgument>();
            SupportExtendedArguments = supportExtendedArguments;
        }

        /// <summary>
        /// Parses the command line arguments in <see cref="Environment.CommandLine"></see> and
        /// populates <see cref="Arguments"></see> with the results. This method automatically
        /// discards the application name from the command line.
        /// </summary>
        public void Parse()
        {
            // Create parsing helper
            ParsingHelper parser = new ParsingHelper(Environment.CommandLine);
            // Discard application name
            ParseArgument(parser, false);
            // Call main parser
            ParseInternal(parser);
        }

        /// <summary>
        /// Parses the given command line and populates <see cref="Arguments"></see> with the
        /// results. It is assumed that <paramref name="commandLine"/> does not include the
        /// application name. If you want to parse <see cref="Environment.CommandLine"></see>,
        /// you should use <see cref="Parse"></see> instead as it will automatically discard
        /// the application name.
        /// </summary>
        /// <param name="commandLine">Command line to parse.</param>
        public void Parse(string commandLine)
        {
            // Create parsing helper
            ParsingHelper parser = new ParsingHelper(commandLine);
            // Call main parser
            ParseInternal(parser);
        }

        /// <summary>
        /// Main command-line parsing code.
        /// </summary>
        /// <param name="parser">Parsing helper object.</param>
        private void ParseInternal(ParsingHelper parser)
        {
            // Clear any existing parsed arguments
            Arguments.Clear();

            // Skip whitespace
            parser.SkipWhiteSpace();
            while (!parser.EndOfText)
            {
                // Create new argument
                CommandLineArgument argument = new CommandLineArgument();

                // Determine if this is a flag
                argument.IsFlag = FlagChars.Contains(parser.Peek());
                if (argument.IsFlag)
                    parser++;

                // Parse argument
                argument.Argument = ParseArgument(parser, SupportExtendedArguments);

                // Parse extended argument
                if (parser.Peek() == ExtendedArgumentDelimiter)
                {
                    parser++;
                    argument.ExtendedArgument = ParseArgument(parser, false);
                }

                // Add to list of arguments if not empty
                if (!string.IsNullOrWhiteSpace(argument.Argument))
                    Arguments.Add(argument);

                // Skip whitespace
                parser.SkipWhiteSpace();
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
        private string ParseArgument(ParsingHelper parser, bool testForExtendedArgument)
        {
            if (QuoteChars.Contains(parser.Peek()))
                return parser.ParseQuotedText();
            // Parse unquoted argument
            return parser.ParseWhile(c => !char.IsWhiteSpace(c) &&
                !FlagChars.Contains(c) &&
                (!testForExtendedArgument || parser.Peek() != ExtendedArgumentDelimiter));
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
        /// found. Non-flag arguments are not included in the search.
        /// </summary>
        /// <param name="flag">The flag argument to search for.</param>
        /// <param name="ignoreCase">Set to true if case does not matter.</param>
        /// <returns>The first matching flag argument or null if the flag argument was not found.</returns>
        public CommandLineArgument GetFlagArgument(string flag, bool ignoreCase = false)
        {
            return Arguments.FirstOrDefault(a => a.IsFlag && string.Compare(a.Argument, flag, ignoreCase) == 0);
        }

        /// <summary>
        /// Returns all non-flag arguments (arguments not preceded with <c>/</c> or <c>-</c>)
        /// in the order they were specified on the command line.
        /// </summary>
        /// <returns>The collection of non-flag arguments.</returns>
        public IEnumerable<CommandLineArgument> GetArguments() => Arguments.Where(a => a.IsFlag == false);

        /// <summary>
        /// Returns all flag arguments (arguments preceded with <c>/</c> or <c>-</c>) in the
        /// order they were specified on the command line.
        /// </summary>
        /// <returns>The collection of flag arguments.</returns>
        public IEnumerable<CommandLineArgument> GetFlagArguments() => Arguments.Where(a => a.IsFlag == true);
    }
}
