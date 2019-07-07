# CommandLineParser

The `CommandLine` class is a lightweight class library that makes it easy to parse a desktop application's command-line into any number of arguments.

The code distinguishes between arguments and flag arguments (or flags). The difference between the two is that a flag argument is an argument immediately preceded by a hypen (-) or forward slash (/). A flag argument is sometimes called a switch and is normally used to enable or disable an application setting.

The code also supports extended arguments. Extended arguments appear after an argument or flag argument and use a colon (:) as a delimiter. Consider the following argument:

`-log:off`

If extended arguments are enabled, the argument is "log". It's a flag argument because it was preceded with a hyphen. And "off" is the extended argument. If extended arguments are not enabled, the argument would be "log:off". Extended arguments are enabled from the arguments passed to the `CommandLine` constructor.

Any argument, flag or extended argument can be enclosed in single or double quotes, providing support for arguments that contain whitespace or other special characters.

**IMPORTANT:** Note that the first argument is always ignored. This is because `Environment.CommandLine` always includes the application name (with or without a full path) as the first item on the command line. If you obtain the command line from somewhere else, you'll need to ensure it follows this same standard, or else add a placeholder application name at the start of the command line.

# Usage

The `CommandLine` constructor accepts a command line and an option Boolean value that indicates if extended arguments are supported.

```cs
// Construct a CommandLine instance and enable extended arguments
CommandLine commandLine = new CommandLine(Environment.CommandLine, true);

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

Once a `CommandLine` instance has been created, you can use it to process additional command lines by using the `ParseCommandLine()` method.

Additional methods are provided to simply processing a command line. You can use the `HasArgument()` method to determine if a particular argument has been specified. Similiarly, use the `HasFlagArgument()` method to determine if a particular flag argument has been specified. If you need to further inspect the argument, you can instead use the `GetArgument()` and `GetFlagArgument()` methods.

You might also find it useful to process regular arguments and flag arguments separately. Use the `GetArguments()` and `GetFlagArguments()` methods to retrieve




# Examples

Example: `ExeName filename.ext`

| Argument | Extended Argument | Is Flag 
|---|---|---|
| filename.ext | *NULL* | False |

Example: `ExeName infile.ext outfile.ext`

| Argument | Extended Argument | Is Flag 
|---|---|---|
| infile.ext | *NULL* | False |
| outfile.ext | *NULL* | False |

Example: `ExeName "Long File Name.ext"`

| Argument | Extended Argument | Is Flag 
|---|---|---|
| Long File Name.ext | *NULL* | False |

Example: `ExeName filename.ext -a /b -c`

| Argument | Extended Argument | Is Flag 
|---|---|---|
| filename.ext | *NULL* | False |
| a | *NULL* | True |
| b | *NULL* | True |
| c | *NULL* | True |

Example: `ExeName filename.ext /mode:read-option:off` (supportExtendedArguments == False)

| Argument | Extended Argument | Is Flag 
|---|---|---|
| filename.ext | *NULL* | False |
| mode:read | *NULL* | True |
| option:off | *NULL* | True |

Example: `ExeName filename.ext /mode:read-option:off` (supportExtendedArguments == True)

| Argument | Extended Argument | Is Flag 
|---|---|---|
| filename.ext | *NULL* | False |
| mode | read | True |
| option | off | True |

Example: `ExeName /a infile.ext -b/c outfile.ext /d-e/f /g -h:yes` (supportExtendedArguments == False)

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

Example: `ExeName /a infile.ext -b/c outfile.ext /d-e/f /g -h:yes` (supportExtendedArguments == True)

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
