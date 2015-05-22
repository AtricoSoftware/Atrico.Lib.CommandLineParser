# Atrico.Lib.CommandLineParser
Command line parser project
*Currently still in development*

## Why?
Having looked around, nothing I can find fits my requirements exactly.  An old library I wrote years ago did exactly what I wanted so this is a refactored version using attributes to specify options

## Features

## Syntax

#### Options class
```c#
private class Options
{
    [Option]
    public bool Optional { get; set; }
    [Option, Required = true]
    public bool Mandatory { get; set; }
}
```
## Roadmap

* Implement types other than boolean
* Get Usage information
* Auto add help
* Multiple commands (like git commit, git push, etc)
