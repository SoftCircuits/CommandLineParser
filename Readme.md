# CommandLineParser

[![NuGet version (SoftCircuits.CommandLineParser)](https://img.shields.io/nuget/v/SoftCircuits.CommandLineParser.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.CommandLineParser/)

```
Install-Package SoftCircuits.CommandLineParser
```

# Introduction

CommandLineParser is a simple and lightweight class library that makes it easy to parse a Windows application's command line.

The code distinguishes between arguments and flag arguments (also called flags or switches). The difference between the two is that a flag argument is immediately preceded by a hypen (-) or forward slash (/). Flag argument are normally used to enable or disable an application setting.

The code also supports extended arguments. Extended arguments appear after an argument or flag argument and use a colon (:) as a delimiter. Consider the following argument:

`-log:off`

If extended arguments are enabled, the argument is "log", it's a flag argument because it was preceded with a hyphen, and "off" is the extended argument. If extended arguments are not enabled, the argument would be "log:off". Extended arguments are enabled via an argument passed to the `CommandLine` constructor.

Any argument, flag argument, or extended argument can be enclosed in single or double quotes, providing support for arguments that contain whitespace or other special characters. (In the case of flag arguments, the hyphen or forward slash must not be enclosed in quotes.) And no space is required between flag arguments.

# Usage

The `CommandLine` constructor accepts an optional Boolean value that specifies if extended arguments are supported. Call the `Parse()` method to parse a command line.

```cs
// Construct a CommandLine instance and enable extended arguments
CommandLine commandLine = new CommandLine(true);

commandLine.Parse();

foreach (CommandLineArgument argument in commandLine.Arguments)
{
    // Process command line arguments
    if (argument.IsFlag)
    {
        // Process flag argument
    }
    else
    {
        // Process regular argument
    }
}
```

In the example above, `Parse()` parses `Environment.CommandLine` and automatically discards the application name. `Parse()` is overloaded with a version that accepts a command-line argument. When using the second version, the application name is not discarded. So take care when calling the second version that your command line does not include the application name unless you want that to be returned as one of the arguments.

`CommandLineParser` provides additional methods for examing the arguments that were parsed. You can use the `HasArgument()` method to determine if a particular argument has been specified. Similiarly, use the `HasFlagArgument()` method to determine if a particular flag argument has been specified. If you need to inspect the arguments, you can instead use the `GetArgument()` and `GetFlagArgument()` methods.

You might also find it useful to process regular arguments and flag arguments separately. Use the `GetArguments()` method to retrieve all of the regular arguments, and the `GetFlagArguments()` method to retrieve all of the flag arguments.

# Examples

Example: `filename.ext`

| Argument | Extended Argument | Is Flag 
|---|---|---|
| filename.ext | *NULL* | False |

Example: `infile.ext outfile.ext`

| Argument | Extended Argument | Is Flag 
|---|---|---|
| infile.ext | *NULL* | False |
| outfile.ext | *NULL* | False |

Example: `"Long File Name.ext"`

| Argument | Extended Argument | Is Flag 
|---|---|---|
| Long File Name.ext | *NULL* | False |

Example: `filename.ext -a /b -c`

| Argument | Extended Argument | Is Flag 
|---|---|---|
| filename.ext | *NULL* | False |
| a | *NULL* | True |
| b | *NULL* | True |
| c | *NULL* | True |

Example: `filename.ext /mode:read-option:off` (supportExtendedArguments == False)

| Argument | Extended Argument | Is Flag 
|---|---|---|
| filename.ext | *NULL* | False |
| mode:read | *NULL* | True |
| option:off | *NULL* | True |

Example: `filename.ext /mode:read-option:off` (supportExtendedArguments == True)

| Argument | Extended Argument | Is Flag 
|---|---|---|
| filename.ext | *NULL* | False |
| mode | read | True |
| option | off | True |

Example: `/a infile.ext -b/c outfile.ext /d-e/f /g -h:yes` (supportExtendedArguments == False)

| Argument | Extended Argument | Is Flag 
|---|---|---|
| a | *NULL* | True |
| infile.ext | *NULL* | False |
| b | *NULL* | True |
| c | *NULL* | True |
| outfile.ext | *NULL* | False |
| d | *NULL* | True |
| e | *NULL* | True |
| f | *NULL* | True |
| g | *NULL* | True |
| h:yes | *NULL* | True |

Example: `/a infile.ext -b/c outfile.ext /d-e/f /g -h:yes` (supportExtendedArguments == True)

| Argument | Extended Argument | Is Flag 
|---|---|---|
| a | *NULL* | True |
| infile.ext | *NULL* | False |
| b | *NULL* | True |
| c | *NULL* | True |
| outfile.ext | *NULL* | False |
| d | *NULL* | True |
| e | *NULL* | True |
| f | *NULL* | True |
| g | *NULL* | True |
| h | yes | True |

In the example above, you would likely use `commandLine.GetArguments()` to return only the non-flag arguments in the order they were specified on the command line.
