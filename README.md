# Atrico.Lib.CommandLineParser
Command line parser project<br/>
Currently still in development

## Why?
Having looked around, nothing I can find fits my requirements exactly.  An old library I wrote years ago did exactly what I wanted so this is a refactored version using attributes to specify options

## Features
Windows powershell like command line, Options can be given by position and/or name.  Options can be specified by the minimum unique name.

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
